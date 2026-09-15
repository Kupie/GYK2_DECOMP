using System;
using Cinemachine;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class CinemachineCorrectPixelExtension : CinemachineExtension
{
	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000902 RID: 2306 RVA: 0x0002E03B File Offset: 0x0002C23B
	private CameraController CameraController
	{
		get
		{
			if (!this.cameraController)
			{
				this.cameraController = base.GetComponent<CameraController>();
			}
			return this.cameraController;
		}
	}

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000903 RID: 2307 RVA: 0x0002E05C File Offset: 0x0002C25C
	// (set) Token: 0x06000904 RID: 2308 RVA: 0x0002E064 File Offset: 0x0002C264
	public bool SmoothWorldInsteadOfCharacter
	{
		get
		{
			return this.smoothWorldInsteadOfCharacter;
		}
		set
		{
			this.smoothWorldInsteadOfCharacter = value;
		}
	}

	// Token: 0x17000167 RID: 359
	// (get) Token: 0x06000905 RID: 2309 RVA: 0x0002E06D File Offset: 0x0002C26D
	// (set) Token: 0x06000906 RID: 2310 RVA: 0x0002E075 File Offset: 0x0002C275
	public float SmoothingFactor
	{
		get
		{
			return this.smoothingFactor;
		}
		set
		{
			this.smoothingFactor = value;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0002E080 File Offset: 0x0002C280
	protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
	{
		if (stage != this.applyCorrectionOnStage || base.VirtualCamera.Follow == null)
		{
			return;
		}
		if (!this.isActiveExtension && !this.applyMaxDeltaConstraint)
		{
			return;
		}
		bool flag = CameraSystem.Instance.ActiveCameraController == this.CameraController;
		Vector3 vector = state.CorrectedOrientation * Vector3.forward;
		float y = base.VirtualCamera.Follow.transform.position.y;
		if (this.isActiveExtension)
		{
			vcam.MoveToTopOfPrioritySubqueue();
			if (!this.useSmoothing)
			{
				Vector3 posOnYPlaneProjected = this.GetPosOnYPlaneProjected(state.CorrectedPosition, vector, y);
				Vector3 roundedCameraPosition = CameraController.GetRoundedCameraPosition(posOnYPlaneProjected, CameraSystem.Instance.ResolutionPiexelSize, flag);
				state.PositionCorrection += roundedCameraPosition - posOnYPlaneProjected;
			}
			else
			{
				Vector3 roundedCameraPosition2 = CameraController.GetRoundedCameraPosition(base.VirtualCamera.Follow.transform.position, CameraSystem.Instance.ResolutionPiexelSize, flag);
				Vector3 posOnYPlaneProjected2 = this.GetPosOnYPlaneProjected(state.CorrectedPosition, vector, roundedCameraPosition2.y);
				if (this.ShouldResetSmoothing(base.VirtualCamera.Follow, flag))
				{
					this.ResetSmoothingState(posOnYPlaneProjected2, roundedCameraPosition2);
				}
				this.UpdateSmoothingCache(base.VirtualCamera.Follow, flag);
				if (this.smoothWorldInsteadOfCharacter)
				{
					this.smoothedWorldPosition = Vector3.Lerp(this.smoothedWorldPosition, posOnYPlaneProjected2, this.smoothingFactor * deltaTime);
					Vector3 roundedCameraPosition3 = CameraController.GetRoundedCameraPosition(this.smoothedWorldPosition, CameraSystem.Instance.ResolutionPiexelSize, flag);
					state.PositionCorrection += roundedCameraPosition3 - posOnYPlaneProjected2;
				}
				else
				{
					Vector3 vector2 = posOnYPlaneProjected2 - roundedCameraPosition2;
					this.smoothedOffset = Vector3.Lerp(this.smoothedOffset, vector2, this.smoothingFactor * deltaTime);
					Vector3 roundedCameraPosition4 = CameraController.GetRoundedCameraPosition(roundedCameraPosition2 + this.smoothedOffset, CameraSystem.Instance.ResolutionPiexelSize, flag);
					state.PositionCorrection += roundedCameraPosition4 - posOnYPlaneProjected2;
				}
				if (this.applyMaxDistanceConstraint)
				{
					Vector3 posOnYPlaneProjected3 = this.GetPosOnYPlaneProjected(roundedCameraPosition2, vector, roundedCameraPosition2.y);
					Vector3 posOnYPlaneProjected4 = this.GetPosOnYPlaneProjected(state.FinalPosition, vector, roundedCameraPosition2.y);
					Vector2 vector3 = Vector2.zero;
					float num = posOnYPlaneProjected3.x - posOnYPlaneProjected4.x;
					float num2 = posOnYPlaneProjected3.z - posOnYPlaneProjected4.z;
					Vector2 vector4 = new Vector2(num, num2);
					if (vector4.sqrMagnitude.EqualsOrMore(this.maxDistance, 1E-05f))
					{
						vector3 = vector4 - vector4.normalized * this.maxDistance;
						Vector3 roundedPosXZ = VisualConsts.GetRoundedPosXZ(new Vector3(vector3.x, 0f, vector3.y), CameraSystem.Instance.ResolutionPiexelSize, 1);
						vector3.x = roundedPosXZ.x;
						vector3.y = roundedPosXZ.z;
					}
					state.PositionCorrection.x = state.PositionCorrection.x + vector3.x;
					state.PositionCorrection.z = state.PositionCorrection.z + vector3.y;
				}
			}
		}
		if (this.applyMaxDeltaConstraint)
		{
			this.ApplyMaxDeltaConstraint(ref state, vector, y, deltaTime, flag);
		}
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0002E3A6 File Offset: 0x0002C5A6
	protected override void Awake()
	{
		base.Awake();
		this.HandleMainCameraRenderTypeChanged(CameraSystem.Instance.MainCamera.GetRenderType());
		MainCamera.OnRenderModeChanged += this.HandleMainCameraRenderTypeChanged;
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x0002E3D4 File Offset: 0x0002C5D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		MainCamera.OnRenderModeChanged -= this.HandleMainCameraRenderTypeChanged;
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x0002E3F0 File Offset: 0x0002C5F0
	private void HandleMainCameraRenderTypeChanged(MainCamera.RenderMode renderType)
	{
		this.isActiveExtension = PlatformFeatures.Current.renderMode != PlatformRenderMode.Lightweight;
		this.useSmoothing = renderType == MainCamera.RenderMode.Lightweight;
		bool flag = renderType == MainCamera.RenderMode.Lightweight && this.CameraController != null && this.CameraController.CameraType == global::CameraType.BuildMode;
		this.applyMaxDeltaConstraint = flag;
		if (flag)
		{
			this.maxDeltaPerSecond = 8f;
		}
		this.cachedFollowTarget = null;
		this.hasLastClampedGroundPos = false;
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x0002E464 File Offset: 0x0002C664
	private bool ShouldResetSmoothing(Transform follow, bool isActiveCamera)
	{
		return follow != this.cachedFollowTarget || (isActiveCamera && !this.cachedWasActiveCamera);
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x0002E484 File Offset: 0x0002C684
	private void UpdateSmoothingCache(Transform follow, bool isActiveCamera)
	{
		this.cachedFollowTarget = follow;
		this.cachedWasActiveCamera = isActiveCamera;
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x0002E494 File Offset: 0x0002C694
	private void ResetSmoothingState(Vector3 curPos, Vector3 roundedCharacterPos)
	{
		this.smoothedOffset = curPos - roundedCharacterPos;
		this.smoothedWorldPosition = curPos;
		this.hasLastClampedGroundPos = false;
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x0002E4B4 File Offset: 0x0002C6B4
	private void ApplyMaxDeltaConstraint(ref CameraState state, Vector3 camDir, float yPlane, float deltaTime, bool isActiveCamera)
	{
		Vector3 posOnYPlaneProjected = this.GetPosOnYPlaneProjected(state.CorrectedPosition, camDir, yPlane);
		bool flag = !isActiveCamera || deltaTime < 0f || this.ShouldResetSmoothing(base.VirtualCamera.Follow, isActiveCamera);
		this.UpdateSmoothingCache(base.VirtualCamera.Follow, isActiveCamera);
		if (flag || !this.hasLastClampedGroundPos)
		{
			this.lastClampedGroundPos = posOnYPlaneProjected;
			this.hasLastClampedGroundPos = isActiveCamera && deltaTime >= 0f;
			return;
		}
		if (deltaTime <= 0f || this.maxDeltaPerSecond <= 0f)
		{
			this.lastClampedGroundPos = posOnYPlaneProjected;
			return;
		}
		Vector3 vector = posOnYPlaneProjected - this.lastClampedGroundPos;
		vector.y = 0f;
		float num = this.maxDeltaPerSecond * deltaTime;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude > num * num)
		{
			Vector3 vector2 = this.lastClampedGroundPos + vector / Mathf.Sqrt(sqrMagnitude) * num;
			vector2.y = posOnYPlaneProjected.y;
			state.PositionCorrection += vector2 - posOnYPlaneProjected;
			this.lastClampedGroundPos = vector2;
			return;
		}
		this.lastClampedGroundPos = posOnYPlaneProjected;
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x0002E5DC File Offset: 0x0002C7DC
	private Vector3 GetPosOnYPlaneProjected(Vector3 camPos, Vector3 camDir, float yPlane)
	{
		if (Mathf.Abs(camDir.y) < 1E-05f)
		{
			return new Vector3(camPos.x, yPlane, camPos.z);
		}
		float num = (yPlane - camPos.y) / camDir.y;
		return new Vector3(camPos.x + num * camDir.x, yPlane, camPos.z + num * camDir.z);
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x0002E644 File Offset: 0x0002C844
	private void OnDrawGizmosSelected()
	{
		if (!this.applyMaxDistanceConstraint || base.VirtualCamera.Follow == null)
		{
			return;
		}
		Gizmos.DrawWireCube(base.VirtualCamera.Follow.position, Vector3.one * this.maxDistance * 2f);
	}

	// Token: 0x04000AB6 RID: 2742
	private const float PLANE_INTERSECTION_DIR_EPSILON = 1E-05f;

	// Token: 0x04000AB7 RID: 2743
	private const float LightweightBuildModeMaxDeltaPerSecond = 8f;

	// Token: 0x04000AB8 RID: 2744
	[SerializeField]
	private CinemachineCore.Stage applyCorrectionOnStage;

	// Token: 0x04000AB9 RID: 2745
	[Space]
	[SerializeField]
	private bool useSmoothing;

	// Token: 0x04000ABA RID: 2746
	[SerializeField]
	private bool applyMaxDistanceConstraint;

	// Token: 0x04000ABB RID: 2747
	[SerializeField]
	private float maxDistance = 2f;

	// Token: 0x04000ABC RID: 2748
	[SerializeField]
	private float smoothingFactor = 10f;

	// Token: 0x04000ABD RID: 2749
	[SerializeField]
	private bool smoothWorldInsteadOfCharacter;

	// Token: 0x04000ABE RID: 2750
	[Space]
	[SerializeField]
	[Tooltip("Clamp per-frame camera travel on the follow-target ground plane. Runs even when pixel correction is off (Lightweight).")]
	private bool applyMaxDeltaConstraint;

	// Token: 0x04000ABF RID: 2751
	[SerializeField]
	[Min(0f)]
	[Tooltip("Maximum ground-plane travel in world units per second. Cuts and follow-target switches are not clamped.")]
	private float maxDeltaPerSecond = 4f;

	// Token: 0x04000AC0 RID: 2752
	private bool isActiveExtension = true;

	// Token: 0x04000AC1 RID: 2753
	private Vector3 smoothedOffset;

	// Token: 0x04000AC2 RID: 2754
	private Vector3 smoothedWorldPosition;

	// Token: 0x04000AC3 RID: 2755
	private Transform cachedFollowTarget;

	// Token: 0x04000AC4 RID: 2756
	private bool cachedWasActiveCamera;

	// Token: 0x04000AC5 RID: 2757
	private Vector3 lastClampedGroundPos;

	// Token: 0x04000AC6 RID: 2758
	private bool hasLastClampedGroundPos;

	// Token: 0x04000AC7 RID: 2759
	private CameraController cameraController;
}
