using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Token: 0x02000167 RID: 359
public class CameraController : MonoBehaviour
{
	// Token: 0x17000156 RID: 342
	// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0002D0A6 File Offset: 0x0002B2A6
	public global::CameraType CameraType
	{
		get
		{
			return this.cameraType;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x060008BA RID: 2234 RVA: 0x0002D0AE File Offset: 0x0002B2AE
	public CinemachineVirtualCamera VirtualCamera
	{
		get
		{
			return this.virtualCamera;
		}
	}

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x060008BB RID: 2235 RVA: 0x0002D0B6 File Offset: 0x0002B2B6
	public Transform Target
	{
		get
		{
			return this.actualTarget;
		}
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x0002D0BE File Offset: 0x0002B2BE
	private void Awake()
	{
		MainCamera.OnRenderModeChanged += this.HandleMainCameraRenderTypeChanged;
		this.ApplyFollowBodyModeIfReady();
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0002D0D8 File Offset: 0x0002B2D8
	private void Initialize()
	{
		this.dollyTarget = new GameObject().GetComponent<Transform>();
		this.dollyTarget.parent = CameraSystem.Instance.transform;
		this.dollyTarget.gameObject.SetActive(false);
		this.fakeFollowTarget = new GameObject().GetComponent<Transform>();
		this.fakeFollowTarget.parent = CameraSystem.Instance.transform;
		this.noise = this.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		this.initialized = true;
		this.ApplyFollowBodyModeIfReady();
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x0002D160 File Offset: 0x0002B360
	public void SetTarget(Transform targetTransform, float duration = 0f, Action completeCallback = null)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		if (duration.EqualsTo(0f, 1E-05f))
		{
			this.SetTargetInstant(targetTransform);
			if (completeCallback != null)
			{
				completeCallback();
				return;
			}
		}
		else
		{
			this.actualTarget = targetTransform;
			this.dollyTarget.position = base.transform.position;
			this.followStartPosition = this.dollyTarget.position;
			this.currentTime = 0f;
			this.followTargetTime = duration;
			this.virtualCamera.Follow = this.dollyTarget;
			this.currentState = CameraController.State.TrackingDolly;
			this.onTargetFollowCompleted = completeCallback;
		}
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x0002D1FD File Offset: 0x0002B3FD
	public void SetTargetInstant(Transform targetTransform)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.actualTarget = targetTransform;
		this.SetDefaultParams();
		this.UpdateTargetPosInstant();
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x0002D220 File Offset: 0x0002B420
	public void SetPosition(Vector3 pos, float duration = 0f, Action completeCallback = null)
	{
		if (!this.initialized)
		{
			this.Initialize();
		}
		this.fakeFollowTarget.position = pos;
		this.isFollowingFakeTransform = true;
		this.SetTarget(this.fakeFollowTarget, duration, completeCallback);
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x0002D251 File Offset: 0x0002B451
	public void UpdateTargetPosInstant()
	{
		this.virtualCamera.UpdateCameraState(Vector3.up, -1f);
		this.virtualCamera.CancelDamping(true);
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x0002D274 File Offset: 0x0002B474
	public void SetActive(bool isActive)
	{
		this.virtualCamera.Priority = (isActive ? 10 : 0);
	}

	// Token: 0x060008C3 RID: 2243 RVA: 0x0002D289 File Offset: 0x0002B489
	public void SetCameraSpaceOffset(Vector3 offset)
	{
		if (this.CameraSpaceOffset == null)
		{
			return;
		}
		this.CameraSpaceOffset.m_Offset = offset;
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0002D2A6 File Offset: 0x0002B4A6
	public void ResetCameraSpaceOffset()
	{
		this.SetCameraSpaceOffset(Vector3.zero);
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x0002D2B3 File Offset: 0x0002B4B3
	public void TrуReset()
	{
		if (this.currentState == CameraController.State.TrackingDolly)
		{
			this.onTargetFollowCompleted = null;
			this.SetDefaultParams();
		}
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x0002D2CC File Offset: 0x0002B4CC
	public static Vector3 GetRoundedCameraPosition(Vector3 position, int pixelSize = 2, bool writeOffsetToShader = false)
	{
		float num = 2f / (float)Mathf.Max(1, pixelSize);
		float num2 = 0.01f * num;
		float num3 = 0.01666667f * num;
		float num4 = 0.0125f * num;
		Vector3 vector = new Vector3(Mathf.Round(position.x / num2) * num2, Mathf.Round(position.y / num3) * num3, Mathf.Round(position.z / num4) * num4);
		Vector3 roundedPosXYZ = VisualConsts.GetRoundedPosXYZ(vector, 2, 1);
		Vector3 vector2 = Vector3.Scale(vector - roundedPosXYZ, VisualConsts.XYZ_STEP_INV) * ((float)ResolutionConfig.PixelSize / 2f);
		Shader.SetGlobalVector(CameraController.idCameraSubOffset, CameraController.subpixelPosition ? vector2 : Vector3.zero);
		if (writeOffsetToShader)
		{
			Shader.SetGlobalVector(CameraController.idCameraOffset, vector2);
		}
		if (!CameraController.canMoveBySubpixel)
		{
			return roundedPosXYZ;
		}
		return vector;
	}

	// Token: 0x060008C7 RID: 2247 RVA: 0x0002D3A4 File Offset: 0x0002B5A4
	public void ShakeCamera(float duration, float fadeDuration = 0f, float amplitude = 1f)
	{
		base.StopAllCoroutines();
		base.StartCoroutine(this.DoShakeCamera(duration, amplitude, fadeDuration));
	}

	// Token: 0x060008C8 RID: 2248 RVA: 0x0002D3BC File Offset: 0x0002B5BC
	public void ShakeCamera(CameraNoiseType type, float duration, float fadeDuration = 0f, float amplitude = 1f, Action callback = null)
	{
		if (this.noiseCoroutine != null)
		{
			base.StopCoroutine(this.noiseCoroutine);
			Action action = this.noiseCallback;
			if (action != null)
			{
				action();
			}
		}
		this.noiseCallback = callback;
		CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = this.noise;
		CameraController.NoiseProfile noiseProfile = this.noiseProfiles.Find((CameraController.NoiseProfile x) => x.type == type);
		cinemachineBasicMultiChannelPerlin.m_NoiseProfile = ((noiseProfile != null) ? noiseProfile.profile : null);
		if (this.noise.m_NoiseProfile == null)
		{
			Debug.LogError(string.Format("[{0}]: noise type [{1}] is not set up.", "CameraController", type));
			return;
		}
		this.noiseCoroutine = base.StartCoroutine(this.DoShakeCamera(duration, fadeDuration, amplitude));
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x0002D47C File Offset: 0x0002B67C
	public bool TrySet3DConfinerBounds(Collider collider)
	{
		CinemachineCustomConfiner cinemachineCustomConfiner;
		if (base.TryGetComponent<CinemachineCustomConfiner>(out cinemachineCustomConfiner))
		{
			cinemachineCustomConfiner.BoundingCollider = collider;
			return true;
		}
		return false;
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x0002D4A0 File Offset: 0x0002B6A0
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		if (this.currentState == CameraController.State.TrackingDolly)
		{
			float num = this.currentTime / this.followTargetTime;
			this.dollyTarget.transform.position = Vector3.Lerp(this.followStartPosition, this.isFollowingFakeTransform ? this.fakeFollowTarget.transform.position : this.actualTarget.transform.position, num);
			if (num >= 1f)
			{
				this.SetDefaultParams();
				Action action = this.onTargetFollowCompleted;
				if (action != null)
				{
					action();
				}
			}
			this.currentTime += Time.deltaTime;
		}
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x0002D546 File Offset: 0x0002B746
	private void SetDefaultParams()
	{
		this.virtualCamera.Follow = this.actualTarget;
		this.currentState = CameraController.State.Default;
		this.isFollowingFakeTransform = false;
	}

	// Token: 0x060008CC RID: 2252 RVA: 0x0002D567 File Offset: 0x0002B767
	private IEnumerator DoShakeCamera(float duration, float fadeDuration, float amplitude)
	{
		bool hasFade = fadeDuration == 0f;
		if (hasFade)
		{
			this.noise.m_AmplitudeGain = amplitude;
		}
		else
		{
			yield return this.DoAmplitudeEase(0f, amplitude, fadeDuration);
		}
		yield return new WaitForSeconds(duration);
		if (hasFade)
		{
			this.noise.m_AmplitudeGain = 0f;
		}
		else
		{
			yield return this.DoAmplitudeEase(amplitude, 0f, fadeDuration);
		}
		this.noise.m_NoiseProfile = null;
		this.noiseCoroutine = null;
		Action action = this.noiseCallback;
		if (action != null)
		{
			action();
		}
		yield break;
	}

	// Token: 0x060008CD RID: 2253 RVA: 0x0002D58B File Offset: 0x0002B78B
	private IEnumerator DoAmplitudeEase(float amplitudeValueFrom, float amplitudeValueTo, float easeDuration)
	{
		for (float t = 0f; t < easeDuration; t += Time.deltaTime)
		{
			this.noise.m_AmplitudeGain = Mathf.Lerp(amplitudeValueFrom, amplitudeValueTo, t / easeDuration);
			yield return null;
		}
		this.noise.m_AmplitudeGain = amplitudeValueTo;
		yield return null;
		yield break;
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x0002D5AF File Offset: 0x0002B7AF
	private void HandleMainCameraRenderTypeChanged(MainCamera.RenderMode renderType)
	{
		CameraController.canMoveBySubpixel = renderType == MainCamera.RenderMode.Native;
		this.ApplyFollowBodyMode(renderType);
	}

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x060008CF RID: 2255 RVA: 0x0002D5C1 File Offset: 0x0002B7C1
	private CinemachineCameraOffset CameraSpaceOffset
	{
		get
		{
			if (this.cameraSpaceOffset == null && this.virtualCamera != null)
			{
				this.cameraSpaceOffset = this.virtualCamera.GetComponent<CinemachineCameraOffset>();
			}
			return this.cameraSpaceOffset;
		}
	}

	// Token: 0x060008D0 RID: 2256 RVA: 0x0002D5F6 File Offset: 0x0002B7F6
	private void ApplyFollowBodyModeIfReady()
	{
		if (CameraSystem.Instance == null || CameraSystem.Instance.MainCamera == null)
		{
			return;
		}
		CameraController.subpixelPosition = false;
		this.HandleMainCameraRenderTypeChanged(CameraSystem.Instance.MainCamera.GetRenderType());
	}

	// Token: 0x060008D1 RID: 2257 RVA: 0x0002D634 File Offset: 0x0002B834
	private void ApplyFollowBodyMode(MainCamera.RenderMode renderType)
	{
		if (this.virtualCamera == null)
		{
			return;
		}
		Transform componentOwner = this.virtualCamera.GetComponentOwner();
		if (componentOwner == null)
		{
			return;
		}
		this.CacheFollowBodyComponents(componentOwner);
		if (renderType == MainCamera.RenderMode.Lightweight && this.cameraType != global::CameraType.BuildMode)
		{
			this.EnableHardLockBody(componentOwner);
		}
		else
		{
			this.EnableFramingTransposerBody(componentOwner, renderType == MainCamera.RenderMode.Lightweight);
		}
		this.virtualCamera.InvalidateComponentPipeline();
		this.virtualCamera.PreviousStateIsValid = false;
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0002D6AD File Offset: 0x0002B8AD
	private void CacheFollowBodyComponents(Transform pipelineOwner)
	{
		if (this.framingTransposer == null)
		{
			this.framingTransposer = pipelineOwner.GetComponent<CinemachineFramingTransposer>();
		}
		if (this.hardLockToTarget == null)
		{
			this.hardLockToTarget = pipelineOwner.GetComponent<CinemachineHardLockToTarget>();
		}
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x0002D6E4 File Offset: 0x0002B8E4
	private void EnableHardLockBody(Transform pipelineOwner)
	{
		if (this.hardLockToTarget == null)
		{
			this.hardLockToTarget = pipelineOwner.gameObject.AddComponent<CinemachineHardLockToTarget>();
		}
		this.hardLockToTarget.m_Damping = 0f;
		if (this.framingTransposer != null)
		{
			this.framingTransposer.enabled = false;
		}
		this.hardLockToTarget.enabled = true;
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x0002D748 File Offset: 0x0002B948
	private void EnableFramingTransposerBody(Transform pipelineOwner, bool zeroDamping)
	{
		if (this.hardLockToTarget != null)
		{
			this.hardLockToTarget.enabled = false;
		}
		if (this.framingTransposer == null)
		{
			this.framingTransposer = pipelineOwner.gameObject.AddComponent<CinemachineFramingTransposer>();
		}
		this.CacheDefaultFramingDamping();
		this.framingTransposer.m_XDamping = (zeroDamping ? 0f : this.defaultFramingDamping.x);
		this.framingTransposer.m_YDamping = (zeroDamping ? 0f : this.defaultFramingDamping.y);
		this.framingTransposer.m_ZDamping = (zeroDamping ? 0f : this.defaultFramingDamping.z);
		this.framingTransposer.enabled = true;
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x0002D800 File Offset: 0x0002BA00
	private void CacheDefaultFramingDamping()
	{
		if (this.defaultFramingDampingCached || this.framingTransposer == null)
		{
			return;
		}
		this.defaultFramingDamping = new Vector3(this.framingTransposer.m_XDamping, this.framingTransposer.m_YDamping, this.framingTransposer.m_ZDamping);
		this.defaultFramingDampingCached = true;
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0002D857 File Offset: 0x0002BA57
	private void OnDestroy()
	{
		MainCamera.OnRenderModeChanged -= this.HandleMainCameraRenderTypeChanged;
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0002D86A File Offset: 0x0002BA6A
	public static void SetFollowTarget(Transform targetTransform, float duration = 0f, Action callback = null)
	{
		CameraSystem.Instance.ActiveCameraController.SetTarget(targetTransform, duration, callback);
	}

	// Token: 0x060008D8 RID: 2264 RVA: 0x0002D87E File Offset: 0x0002BA7E
	public static void SetFollowTargetInstant(Transform targetTransform)
	{
		CameraSystem.Instance.ActiveCameraController.SetTargetInstant(targetTransform);
	}

	// Token: 0x04000A76 RID: 2678
	private const int INACTIVE_VIRTUAL_CAM_PRIORITY = 0;

	// Token: 0x04000A77 RID: 2679
	private const int ACTIVE_VIRTUAL_CAM_PRIORITY = 10;

	// Token: 0x04000A78 RID: 2680
	private static readonly int idCameraSubOffset = Shader.PropertyToID("_CameraSubOffset");

	// Token: 0x04000A79 RID: 2681
	private static readonly int idCameraOffset = Shader.PropertyToID("_CameraOffset");

	// Token: 0x04000A7A RID: 2682
	private Action onTargetFollowCompleted;

	// Token: 0x04000A7B RID: 2683
	[SerializeField]
	private global::CameraType cameraType;

	// Token: 0x04000A7C RID: 2684
	[SerializeField]
	private CinemachineVirtualCamera virtualCamera;

	// Token: 0x04000A7D RID: 2685
	[SerializeField]
	private List<CameraController.NoiseProfile> noiseProfiles;

	// Token: 0x04000A7E RID: 2686
	[SerializeField]
	private Transform dollyTarget;

	// Token: 0x04000A7F RID: 2687
	[SerializeField]
	private Transform fakeFollowTarget;

	// Token: 0x04000A80 RID: 2688
	private Action noiseCallback;

	// Token: 0x04000A81 RID: 2689
	private Coroutine noiseCoroutine;

	// Token: 0x04000A82 RID: 2690
	private CinemachineBasicMultiChannelPerlin noise;

	// Token: 0x04000A83 RID: 2691
	private CinemachineFramingTransposer framingTransposer;

	// Token: 0x04000A84 RID: 2692
	private CinemachineHardLockToTarget hardLockToTarget;

	// Token: 0x04000A85 RID: 2693
	private CinemachineCameraOffset cameraSpaceOffset;

	// Token: 0x04000A86 RID: 2694
	private Vector3 defaultFramingDamping;

	// Token: 0x04000A87 RID: 2695
	private bool defaultFramingDampingCached;

	// Token: 0x04000A88 RID: 2696
	private Transform actualTarget;

	// Token: 0x04000A89 RID: 2697
	private bool initialized;

	// Token: 0x04000A8A RID: 2698
	private float currentTime;

	// Token: 0x04000A8B RID: 2699
	private float followTargetTime;

	// Token: 0x04000A8C RID: 2700
	private Vector3 followStartPosition;

	// Token: 0x04000A8D RID: 2701
	private bool isFollowingFakeTransform;

	// Token: 0x04000A8E RID: 2702
	private CameraController.State currentState;

	// Token: 0x04000A8F RID: 2703
	private static bool subpixelPosition = true;

	// Token: 0x04000A90 RID: 2704
	private static bool canMoveBySubpixel = false;

	// Token: 0x02000168 RID: 360
	[Serializable]
	public class NoiseProfile
	{
		// Token: 0x04000A91 RID: 2705
		public CameraNoiseType type;

		// Token: 0x04000A92 RID: 2706
		public NoiseSettings profile;
	}

	// Token: 0x02000169 RID: 361
	private enum State
	{
		// Token: 0x04000A94 RID: 2708
		Default,
		// Token: 0x04000A95 RID: 2709
		TrackingDolly
	}
}
