using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B19 RID: 2841
public class LightRTManager : MonoBehaviour
{
	// Token: 0x17000B5C RID: 2908
	// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x00166171 File Offset: 0x00164371
	public static LightRTManager Instance
	{
		get
		{
			if (LightRTManager.instance == null)
			{
				LightRTManager.EnsureInstance();
			}
			return LightRTManager.instance;
		}
	}

	// Token: 0x06004BB8 RID: 19384 RVA: 0x0016618A File Offset: 0x0016438A
	public static void NotifyWorldRenderTargetChanged()
	{
		if (LightRTManager.instance == null)
		{
			return;
		}
		LightRTManager.instance.ReleaseRT();
	}

	// Token: 0x06004BB9 RID: 19385 RVA: 0x001661A4 File Offset: 0x001643A4
	public static void EnsureInstance()
	{
		if (LightRTManager.instance != null)
		{
			return;
		}
		GameObject gameObject = new GameObject("LightRTManager");
		global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		LightRTManager.instance = gameObject.AddComponent<LightRTManager>();
		LightRTManager.instance.Init();
	}

	// Token: 0x06004BBA RID: 19386 RVA: 0x001661D8 File Offset: 0x001643D8
	private void Awake()
	{
		if (LightRTManager.instance != null && LightRTManager.instance != this)
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		LightRTManager.instance = this;
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.Init();
	}

	// Token: 0x06004BBB RID: 19387 RVA: 0x00166217 File Offset: 0x00164417
	private void OnEnable()
	{
		this.HookPreRender();
	}

	// Token: 0x06004BBC RID: 19388 RVA: 0x0016621F File Offset: 0x0016441F
	private void OnDisable()
	{
		this.UnhookPreRender();
	}

	// Token: 0x06004BBD RID: 19389 RVA: 0x00166227 File Offset: 0x00164427
	private void OnDestroy()
	{
		this.UnhookPreRender();
		if (LightRTManager.instance == this)
		{
			LightRTManager.instance = null;
		}
		this.ReleaseRT();
		this.SetKeyword(false);
	}

	// Token: 0x06004BBE RID: 19390 RVA: 0x0016624F File Offset: 0x0016444F
	private void Init()
	{
		this.CreateLightCamera();
		this.EnsureRT(this.GetSourceCamera());
		if (this.lightCamera)
		{
			this.lightCamera.targetTexture = this.lightRT;
		}
		this.HookPreRender();
		this.UpdateKeyword();
	}

	// Token: 0x06004BBF RID: 19391 RVA: 0x0016628D File Offset: 0x0016448D
	private void HookPreRender()
	{
		if (this.preRenderHooked)
		{
			return;
		}
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Combine(Camera.onPreRender, new Camera.CameraCallback(this.OnCameraPreRender));
		this.preRenderHooked = true;
	}

	// Token: 0x06004BC0 RID: 19392 RVA: 0x001662BF File Offset: 0x001644BF
	private void UnhookPreRender()
	{
		if (!this.preRenderHooked)
		{
			return;
		}
		Camera.onPreRender = (Camera.CameraCallback)Delegate.Remove(Camera.onPreRender, new Camera.CameraCallback(this.OnCameraPreRender));
		this.preRenderHooked = false;
	}

	// Token: 0x06004BC1 RID: 19393 RVA: 0x001662F1 File Offset: 0x001644F1
	private Camera GetSourceCamera()
	{
		if (CameraSystem.Instance != null)
		{
			return CameraSystem.Instance.WorldCamera;
		}
		return Camera.main;
	}

	// Token: 0x06004BC2 RID: 19394 RVA: 0x00166310 File Offset: 0x00164510
	private void OnCameraPreRender(Camera cam)
	{
		if (!SwitchLightPolicy.UseLightRT)
		{
			this.ClearGlobals();
			return;
		}
		if (this.fakers.Count == 0)
		{
			return;
		}
		Camera sourceCamera = this.GetSourceCamera();
		if (sourceCamera == null || cam != sourceCamera)
		{
			return;
		}
		this.EnsureRT(sourceCamera);
		this.SyncLightCameraFromSource(sourceCamera);
		this.RenderLightRT();
		this.PublishGlobals();
	}

	// Token: 0x06004BC3 RID: 19395 RVA: 0x0016636D File Offset: 0x0016456D
	private void LateUpdate()
	{
		if (this.fakers.Count == 0)
		{
			this.ClearGlobals();
		}
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x00166384 File Offset: 0x00164584
	private void EnsureRT(Camera source)
	{
		int num;
		int num2;
		if (!LightRTManager.TryGetWorldRenderTargetSize(source, out num, out num2))
		{
			num2 = (SwitchLightPolicy.IsSwitchPlatform ? 256 : 512);
			float num3 = ((source != null) ? source.aspect : 1f);
			num = Mathf.Max(1, Mathf.RoundToInt((float)num2 * num3));
		}
		if (this.lightRT != null && this.lightRT.width == num && this.lightRT.height == num2)
		{
			return;
		}
		this.ReleaseRT();
		this.lightRT = new RenderTexture(num, num2, 0, RenderTextureFormat.ARGBHalf)
		{
			name = "_LightRT",
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Clamp,
			useMipMap = false
		};
		this.lightRT.Create();
		if (this.lightCamera != null)
		{
			this.lightCamera.targetTexture = this.lightRT;
			this.lightCamera.rect = LightRTManager.FullViewportRect;
		}
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x00166474 File Offset: 0x00164674
	private static bool TryGetWorldRenderTargetSize(Camera source, out int width, out int height)
	{
		RenderTexture renderTexture = LightRTManager.ResolveSourceRenderTexture(source);
		if (renderTexture != null)
		{
			width = renderTexture.width;
			height = renderTexture.height;
			return true;
		}
		width = 0;
		height = 0;
		return false;
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001664AC File Offset: 0x001646AC
	private static RenderTexture ResolveSourceRenderTexture(Camera source)
	{
		if (source != null && source.targetTexture != null)
		{
			return source.targetTexture;
		}
		if (CameraSystem.Instance != null)
		{
			RenderTexture renderTexture = CameraSystem.Instance.MainCamera.RenderTexture;
			if (renderTexture != null)
			{
				return renderTexture;
			}
		}
		return null;
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x00166500 File Offset: 0x00164700
	private void CreateLightCamera()
	{
		if (this.lightCamera != null)
		{
			return;
		}
		this.lightCamera = base.gameObject.AddComponent<Camera>();
		this.lightCamera.enabled = false;
		this.lightCamera.orthographic = true;
		this.lightCamera.orthographicSize = this.worldHalfExtent;
		this.lightCamera.clearFlags = CameraClearFlags.Color;
		this.lightCamera.backgroundColor = Color.black;
		this.lightCamera.cullingMask = 512;
		this.lightCamera.depth = -100f;
		this.lightCamera.rect = LightRTManager.FullViewportRect;
		this.lightCamera.nearClipPlane = 0.1f;
		this.lightCamera.farClipPlane = this.cameraHeight + 10f;
		this.lightCamera.allowHDR = true;
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001665D8 File Offset: 0x001647D8
	private void SyncLightCameraFromSource(Camera source)
	{
		if (this.lightCamera == null)
		{
			return;
		}
		if (source != null)
		{
			Transform transform = this.lightCamera.transform;
			Transform transform2 = source.transform;
			if (transform.parent != transform2)
			{
				transform.SetPositionAndRotation(transform2.position, transform2.rotation);
			}
			this.lightCamera.orthographic = source.orthographic;
			this.lightCamera.fieldOfView = source.fieldOfView;
			this.lightCamera.orthographicSize = source.orthographicSize;
			this.lightCamera.nearClipPlane = source.nearClipPlane;
			this.lightCamera.farClipPlane = source.farClipPlane;
			this.lightCamera.rect = LightRTManager.FullViewportRect;
			this.lightCamera.ResetWorldToCameraMatrix();
			this.lightCamera.worldToCameraMatrix = source.worldToCameraMatrix;
			this.lightCamera.ResetProjectionMatrix();
			this.lightCamera.projectionMatrix = source.projectionMatrix;
			LightRTManager.PublishCameraMatrices(source);
			return;
		}
		this.lightCamera.orthographicSize = this.worldHalfExtent;
		this.lightCamera.rect = LightRTManager.FullViewportRect;
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x001666F8 File Offset: 0x001648F8
	private static void PublishCameraMatrices(Camera source)
	{
		Matrix4x4 matrix4x = GL.GetGPUProjectionMatrix(source.projectionMatrix, true) * source.worldToCameraMatrix;
		Shader.SetGlobalMatrix(LightRTManager.idLightRTCameraVPMatrix, matrix4x);
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x00166728 File Offset: 0x00164928
	private void RenderLightRT()
	{
		if (this.lightCamera == null || this.lightRT == null)
		{
			return;
		}
		this.lightCamera.rect = LightRTManager.FullViewportRect;
		this.lightCamera.Render();
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x00166764 File Offset: 0x00164964
	private void PublishGlobals()
	{
		Shader.SetGlobalTexture(LightRTManager.idLightRT, this.lightRT);
		Shader.SetGlobalVector(LightRTManager.idLightRTViewDir, this.lightCamera.transform.forward);
		Shader.SetGlobalFloat(LightRTManager.idLightRTIntensity, 1f);
		this.UpdateKeyword();
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x001667B8 File Offset: 0x001649B8
	private void ClearGlobals()
	{
		Shader.SetGlobalTexture(LightRTManager.idLightRT, Texture2D.blackTexture);
		Shader.SetGlobalVector(LightRTManager.idLightRTViewDir, Vector4.zero);
		Shader.SetGlobalFloat(LightRTManager.idLightRTIntensity, 0f);
		Shader.SetGlobalMatrix(LightRTManager.idLightRTCameraVPMatrix, Matrix4x4.zero);
		this.SetKeyword(false);
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x00166808 File Offset: 0x00164A08
	private void UpdateKeyword()
	{
		bool flag = SwitchLightPolicy.UseLightRT && this.fakers.Count > 0;
		this.SetKeyword(flag);
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x00166835 File Offset: 0x00164A35
	private void SetKeyword(bool enable)
	{
		if (this.keywordApplied == enable)
		{
			return;
		}
		this.keywordApplied = enable;
		if (enable)
		{
			Shader.EnableKeyword("USE_LIGHT_RT");
			return;
		}
		Shader.DisableKeyword("USE_LIGHT_RT");
	}

	// Token: 0x06004BCF RID: 19407 RVA: 0x00166860 File Offset: 0x00164A60
	private void ReleaseRT()
	{
		if (this.lightRT != null)
		{
			this.lightRT.Release();
			global::UnityEngine.Object.Destroy(this.lightRT);
			this.lightRT = null;
		}
	}

	// Token: 0x06004BD0 RID: 19408 RVA: 0x0016688D File Offset: 0x00164A8D
	public void Register(LightFaker faker)
	{
		LightRTManager.EnsureInstance();
		this.fakers.Add(faker);
	}

	// Token: 0x06004BD1 RID: 19409 RVA: 0x001668A1 File Offset: 0x00164AA1
	public void Unregister(LightFaker faker)
	{
		this.fakers.Remove(faker);
		if (this.fakers.Count == 0)
		{
			this.ClearGlobals();
		}
	}

	// Token: 0x06004BD2 RID: 19410 RVA: 0x001668C4 File Offset: 0x00164AC4
	public static void ApplyPolicy()
	{
		if (LightRTManager.instance != null)
		{
			LightRTManager.instance.ApplyPolicyInternal();
			return;
		}
		if (SwitchLightPolicy.UseLightRT)
		{
			return;
		}
		Shader.SetGlobalTexture(LightRTManager.idLightRT, Texture2D.blackTexture);
		Shader.SetGlobalVector(LightRTManager.idLightRTViewDir, Vector4.zero);
		Shader.SetGlobalFloat(LightRTManager.idLightRTIntensity, 0f);
		Shader.SetGlobalMatrix(LightRTManager.idLightRTCameraVPMatrix, Matrix4x4.zero);
		Shader.DisableKeyword("USE_LIGHT_RT");
	}

	// Token: 0x06004BD3 RID: 19411 RVA: 0x00166937 File Offset: 0x00164B37
	private void ApplyPolicyInternal()
	{
		if (!SwitchLightPolicy.UseLightRT || this.fakers.Count == 0)
		{
			this.keywordApplied = true;
			this.ClearGlobals();
			return;
		}
		this.keywordApplied = false;
		this.UpdateKeyword();
	}

	// Token: 0x04003D17 RID: 15639
	public const int RT_RESOLUTION_SWITCH = 256;

	// Token: 0x04003D18 RID: 15640
	public const int RT_RESOLUTION_DEFAULT = 512;

	// Token: 0x04003D19 RID: 15641
	private static LightRTManager instance;

	// Token: 0x04003D1A RID: 15642
	private static readonly int idLightRT = Shader.PropertyToID("_LightRT");

	// Token: 0x04003D1B RID: 15643
	private static readonly int idLightRTViewDir = Shader.PropertyToID("_LightRTViewDir");

	// Token: 0x04003D1C RID: 15644
	private static readonly int idLightRTIntensity = Shader.PropertyToID("_LightRTIntensity");

	// Token: 0x04003D1D RID: 15645
	private static readonly int idLightRTCameraVPMatrix = Shader.PropertyToID("_LightRTCameraVPMatrix");

	// Token: 0x04003D1E RID: 15646
	[SerializeField]
	private float worldHalfExtent = 40f;

	// Token: 0x04003D1F RID: 15647
	[SerializeField]
	private float cameraHeight = 50f;

	// Token: 0x04003D20 RID: 15648
	[SerializeField]
	private Camera lightCamera;

	// Token: 0x04003D21 RID: 15649
	private RenderTexture lightRT;

	// Token: 0x04003D22 RID: 15650
	private readonly HashSet<LightFaker> fakers = new HashSet<LightFaker>();

	// Token: 0x04003D23 RID: 15651
	private bool keywordApplied;

	// Token: 0x04003D24 RID: 15652
	private bool preRenderHooked;

	// Token: 0x04003D25 RID: 15653
	private static readonly Rect FullViewportRect = new Rect(0f, 0f, 1f, 1f);
}
