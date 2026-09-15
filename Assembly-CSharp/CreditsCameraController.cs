using System;
using UnityEngine;

// Token: 0x02000172 RID: 370
[DefaultExecutionOrder(-1)]
public class CreditsCameraController : MonoBehaviour
{
	// Token: 0x0600091A RID: 2330 RVA: 0x0002EA86 File Offset: 0x0002CC86
	private void Awake()
	{
		if (this.cameraController == null)
		{
			this.cameraController = base.GetComponent<CameraController>();
		}
	}

	// Token: 0x0600091B RID: 2331 RVA: 0x0002EAA4 File Offset: 0x0002CCA4
	public static void TryEnable(UICreditsWindow creditsWindow)
	{
		CreditsCameraController creditsCameraController = CreditsCameraController.FindController();
		if (creditsCameraController == null)
		{
			Debug.LogError(string.Format("{0} is not found on {1} camera.", "CreditsCameraController", global::CameraType.Credits));
			return;
		}
		creditsCameraController.Enable(creditsWindow);
	}

	// Token: 0x0600091C RID: 2332 RVA: 0x0002EAE2 File Offset: 0x0002CCE2
	public static void TryDisable()
	{
		CreditsCameraController creditsCameraController = CreditsCameraController.FindController();
		if (creditsCameraController == null)
		{
			return;
		}
		creditsCameraController.Disable();
	}

	// Token: 0x0600091D RID: 2333 RVA: 0x0002EAF4 File Offset: 0x0002CCF4
	public void Enable(UICreditsWindow creditsWindow)
	{
		if (creditsWindow == null)
		{
			return;
		}
		this.creditsWindow = creditsWindow;
		this.isSticking = false;
		this.trackTopScreenYAtContact = 0f;
		if (creditsWindow.CameraTrackRect == null)
		{
			Debug.LogError("CreditsCameraController: first credits element is not found.");
		}
		CameraSystem instance = CameraSystem.Instance;
		Vector3 liveFollowPosition = CreditsCameraController.GetLiveFollowPosition(instance.ActiveCameraController);
		instance.SetActiveCamera(global::CameraType.Credits);
		instance.ActiveCameraController.SetPosition(liveFollowPosition, 0f, null);
		this.cameraController.ResetCameraSpaceOffset();
		MainGame instance2 = MainGame.Instance;
		GDPointData gdpointData;
		if (instance2 == null)
		{
			gdpointData = null;
		}
		else
		{
			GameSave gameSave = instance2.GameSave;
			if (gameSave == null)
			{
				gdpointData = null;
			}
			else
			{
				WorldData worldData = gameSave.worldData;
				if (worldData == null)
				{
					gdpointData = null;
				}
				else
				{
					GdPointsData gdPointsData = worldData.gdPointsData;
					gdpointData = ((gdPointsData != null) ? gdPointsData.GetGDPointDataById(this.gdPointId) : null);
				}
			}
		}
		GDPointData gdpointData2 = gdpointData;
		if (gdpointData2 == null)
		{
			Debug.LogError("CreditsCameraController: GD point [" + this.gdPointId + "] is not found.");
			this.isActive = true;
			return;
		}
		this.gdPointPosition = gdpointData2.Position;
		this.isActive = true;
	}

	// Token: 0x0600091E RID: 2334 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
	public void Disable()
	{
		if (!this.isActive && this.creditsWindow == null)
		{
			return;
		}
		this.isActive = false;
		this.isSticking = false;
		this.creditsWindow = null;
		this.cameraController.ResetCameraSpaceOffset();
		CameraSystem instance = CameraSystem.Instance;
		if (instance == null)
		{
			return;
		}
		Vector3 liveFollowPosition = CreditsCameraController.GetLiveFollowPosition(this.cameraController);
		instance.SetActiveCamera(global::CameraType.Main);
		instance.ActiveCameraController.SetPosition(liveFollowPosition, 0f, null);
	}

	// Token: 0x0600091F RID: 2335 RVA: 0x0002EC60 File Offset: 0x0002CE60
	private void LateUpdate()
	{
		if (!this.isActive || this.creditsWindow == null || !this.creditsWindow.IsShown)
		{
			return;
		}
		float num;
		if (!CreditsCameraController.TryGetCreditsTrackTopScreenY(this.creditsWindow, out num))
		{
			return;
		}
		float y = CameraSystem.WorldToScreenPoint(this.gdPointPosition).y;
		if (!this.isSticking)
		{
			if (num < y)
			{
				return;
			}
			this.isSticking = true;
			this.trackTopScreenYAtContact = num;
			return;
		}
		else
		{
			float num2 = num - this.trackTopScreenYAtContact;
			if (num2 <= 0f)
			{
				this.cameraController.ResetCameraSpaceOffset();
				return;
			}
			Vector3 vector;
			if (!CreditsCameraController.TryGetCameraLocalOffsetForScreenYDelta(num2, this.gdPointPosition, out vector))
			{
				return;
			}
			this.cameraController.SetCameraSpaceOffset(vector);
			return;
		}
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x0002ED08 File Offset: 0x0002CF08
	private static Vector3 GetLiveFollowPosition(CameraController source)
	{
		if (source != null && source.VirtualCamera != null && source.VirtualCamera.Follow != null)
		{
			return source.VirtualCamera.Follow.position;
		}
		if (source != null && source.Target != null)
		{
			return source.Target.position;
		}
		if (MainGame.PlayerController != null)
		{
			return MainGame.PlayerController.PhysicalBody.PlayerView.transform.position;
		}
		return Vector3.zero;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0002EDA0 File Offset: 0x0002CFA0
	private static CreditsCameraController FindController()
	{
		CameraController cameraController = ((CameraSystem.Instance != null) ? CameraSystem.Instance.GetCameraController(global::CameraType.Credits) : null);
		if (!(cameraController != null))
		{
			return null;
		}
		return cameraController.GetComponent<CreditsCameraController>();
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0002EDDC File Offset: 0x0002CFDC
	private static bool TryGetCreditsTrackTopScreenY(UICreditsWindow creditsWindow, out float screenY)
	{
		screenY = 0f;
		RectTransform cameraTrackRect = creditsWindow.CameraTrackRect;
		if (cameraTrackRect == null)
		{
			return false;
		}
		cameraTrackRect.GetWorldCorners(CreditsCameraController.cornersBuffer);
		Vector3 vector = (CreditsCameraController.cornersBuffer[1] + CreditsCameraController.cornersBuffer[2]) * 0.5f;
		screenY = vector.y;
		return true;
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x0002EE3C File Offset: 0x0002D03C
	private static bool TryGetCameraLocalOffsetForScreenYDelta(float screenYDelta, Vector3 worldPoint, out Vector3 cameraLocalOffset)
	{
		cameraLocalOffset = Vector3.zero;
		if (CameraSystem.Instance == null)
		{
			return false;
		}
		Camera worldCamera = CameraSystem.Instance.WorldCamera;
		if (worldCamera == null)
		{
			return false;
		}
		Vector3 vector2;
		Vector3 vector = (vector2 = CameraSystem.WorldToScreenPoint(worldPoint));
		vector2.y += screenYDelta;
		Ray ray = CameraSystem.ScreenPointToRay(vector);
		Ray ray2 = CameraSystem.ScreenPointToRay(vector2);
		Plane plane = new Plane(-worldCamera.transform.forward, worldPoint);
		float num;
		float num2;
		if (!plane.Raycast(ray, out num) || !plane.Raycast(ray2, out num2))
		{
			return false;
		}
		Vector3 point = ray.GetPoint(num);
		Vector3 point2 = ray2.GetPoint(num2);
		Vector3 vector3 = point - point2;
		cameraLocalOffset = Quaternion.Inverse(worldCamera.transform.rotation) * vector3;
		return true;
	}

	// Token: 0x04000ACA RID: 2762
	private const string DefaultGdPointId = "gd_astrologer_tower_blackout";

	// Token: 0x04000ACB RID: 2763
	private static readonly Vector3[] cornersBuffer = new Vector3[4];

	// Token: 0x04000ACC RID: 2764
	[SerializeField]
	private CameraController cameraController;

	// Token: 0x04000ACD RID: 2765
	[SerializeField]
	private string gdPointId = "gd_astrologer_tower_blackout";

	// Token: 0x04000ACE RID: 2766
	private bool isActive;

	// Token: 0x04000ACF RID: 2767
	private bool isSticking;

	// Token: 0x04000AD0 RID: 2768
	private Vector3 gdPointPosition;

	// Token: 0x04000AD1 RID: 2769
	private float trackTopScreenYAtContact;

	// Token: 0x04000AD2 RID: 2770
	private UICreditsWindow creditsWindow;
}
