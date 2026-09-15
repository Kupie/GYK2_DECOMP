using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;

// Token: 0x0200016F RID: 367
[ExecuteAlways]
[DefaultExecutionOrder(-10)]
public class CameraSystem : MonoBehaviour
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x060008EA RID: 2282 RVA: 0x0002DAEA File Offset: 0x0002BCEA
	public static CameraSystem Instance
	{
		get
		{
			if (CameraSystem.instance == null)
			{
				CameraSystem.instance = global::UnityEngine.Object.FindObjectOfType<CameraSystem>();
			}
			return CameraSystem.instance;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x060008EB RID: 2283 RVA: 0x0002DB08 File Offset: 0x0002BD08
	public Camera WorldCamera
	{
		get
		{
			return this.worldCamera.Camera;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x060008EC RID: 2284 RVA: 0x0002DB15 File Offset: 0x0002BD15
	public CameraController ActiveCameraController
	{
		get
		{
			return this.activeCameraController;
		}
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x060008ED RID: 2285 RVA: 0x0002DB1D File Offset: 0x0002BD1D
	public Vector3 CameraGroundPos
	{
		get
		{
			return this.groundPointTransform.position;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x060008EE RID: 2286 RVA: 0x0002DB2A File Offset: 0x0002BD2A
	public Transform GroundPointTransform
	{
		get
		{
			return this.groundPointTransform;
		}
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x060008EF RID: 2287 RVA: 0x0002DB32 File Offset: 0x0002BD32
	public MainCamera MainCamera
	{
		get
		{
			return this.worldCamera;
		}
	}

	// Token: 0x17000164 RID: 356
	// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0002DB3A File Offset: 0x0002BD3A
	// (set) Token: 0x060008F1 RID: 2289 RVA: 0x0002DB42 File Offset: 0x0002BD42
	public int ResolutionPiexelSize { get; private set; }

	// Token: 0x060008F2 RID: 2290 RVA: 0x0002DB4C File Offset: 0x0002BD4C
	public void SetActiveCamera(global::CameraType cameraType)
	{
		foreach (CameraController cameraController in this.cameraControllers)
		{
			if (cameraController.CameraType == cameraType)
			{
				CameraController cameraController2 = this.activeCameraController;
				if (cameraController2 != null)
				{
					cameraController2.SetActive(false);
				}
				this.activeCameraController = cameraController;
				this.activeCameraController.VirtualCamera.PreviousStateIsValid = false;
				this.activeCameraController.SetActive(true);
				return;
			}
		}
		Debug.LogError(string.Format("Error activating camera with type {0}", cameraType));
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x0002DBF0 File Offset: 0x0002BDF0
	public static float CalculateOrthographicSize(int resolutionHeight, int resolutionPixelSize)
	{
		return (float)resolutionHeight / 2f / 100f / ((float)resolutionPixelSize / 2f);
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x0002DC0C File Offset: 0x0002BE0C
	public void SetOrthographicSize(float size)
	{
		foreach (CameraController cameraController in this.cameraControllers)
		{
			cameraController.VirtualCamera.m_Lens.OrthographicSize = size;
			cameraController.VirtualCamera.PreviousStateIsValid = false;
		}
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x0002DC74 File Offset: 0x0002BE74
	public CameraController GetCameraController(global::CameraType cameraType)
	{
		foreach (CameraController cameraController in this.cameraControllers)
		{
			if (cameraController.CameraType == cameraType)
			{
				return cameraController;
			}
		}
		return null;
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x0002DCD0 File Offset: 0x0002BED0
	private void Awake()
	{
		this.cameraControllers = base.GetComponentsInChildren<CameraController>().ToList<CameraController>();
		this.SetActiveCamera(global::CameraType.Main);
		this.ApplyRenderActiveState();
		GameSettings.OnResolutionChanged += this.OnResolutionChanged;
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0002DD01 File Offset: 0x0002BF01
	private void Start()
	{
		this.ApplyInitialResolution();
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0002DD09 File Offset: 0x0002BF09
	private void OnDestroy()
	{
		GameSettings.OnResolutionChanged -= this.OnResolutionChanged;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x0002DD1C File Offset: 0x0002BF1C
	private void ApplyInitialResolution()
	{
		this.OnResolutionChanged(GameSettings.Instance.GetResolutionIntVector2());
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x0002DD30 File Offset: 0x0002BF30
	public void OnResolutionChanged(IntVector2 res)
	{
		CameraSystem.curScreenWidth = res.x;
		CameraSystem.curScreenHeight = res.y;
		this.ResolutionPiexelSize = ResolutionConfig.PixelSize;
		float num = CameraSystem.CalculateOrthographicSize(res.y, this.ResolutionPiexelSize);
		this.SetOrthographicSize(num);
		Camera camera = ((this.worldCamera != null) ? this.worldCamera.Camera : null);
		if (camera != null)
		{
			camera.orthographicSize = num;
		}
		if (MainGame.Instance != null && MainGame.Instance.gameState == MainGame.GameState.InGame)
		{
			CameraController cameraController = this.activeCameraController;
			if (cameraController != null)
			{
				cameraController.UpdateTargetPosInstant();
			}
			CameraSystem.ProcessChunkVisibilityForUpcomingResolution(camera, num);
		}
		if (this.worldCamera != null)
		{
			this.worldCamera.OnResolutionChanged(res.x, res.y);
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x0002DDFC File Offset: 0x0002BFFC
	private static void ProcessChunkVisibilityForUpcomingResolution(Camera cam, float orthoSize)
	{
		if (LazySingleton<ChunkManager>.Instance == null)
		{
			return;
		}
		if (cam == null)
		{
			LazySingleton<ChunkManager>.Instance.ForceProcessVisibility();
			return;
		}
		float num = ((ResolutionConfig.Height > 0) ? ((float)ResolutionConfig.Width / (float)ResolutionConfig.Height) : cam.aspect);
		bool flag = num > 0f;
		if (flag)
		{
			cam.projectionMatrix = Matrix4x4.Ortho(-orthoSize * num, orthoSize * num, -orthoSize, orthoSize, cam.nearClipPlane, cam.farClipPlane);
		}
		try
		{
			LazySingleton<ChunkManager>.Instance.ForceProcessVisibility();
		}
		finally
		{
			if (flag)
			{
				cam.ResetProjectionMatrix();
			}
		}
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x0002DEA0 File Offset: 0x0002C0A0
	private void LateUpdate()
	{
		if (GameSettings.Instance.GraphicSettingsAppliedThisFrame)
		{
			return;
		}
		bool flag = CameraSystem.curScreenWidth != Screen.width || CameraSystem.curScreenHeight != Screen.height;
		bool flag2 = GameSettings.Instance.SyncScreenModeFromHardware(false);
		if (flag || flag2)
		{
			GameSettings.Instance.ApplyGraphicSettings(true, false);
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x0002DEF6 File Offset: 0x0002C0F6
	public void ApplyRenderActiveState()
	{
		this.SetCameraRenderActiveState();
	}

	// Token: 0x060008FE RID: 2302 RVA: 0x0002DEFE File Offset: 0x0002C0FE
	private void SetCameraRenderActiveState()
	{
		this.worldCamera.SetRenderType(PlatformFeatures.Current.GetMainCameraRenderMode());
	}

	// Token: 0x060008FF RID: 2303 RVA: 0x0002DF18 File Offset: 0x0002C118
	public static Vector3 WorldToScreenPoint(Vector3 worldPoint)
	{
		Vector3 vector = CameraSystem.Instance.WorldCamera.WorldToScreenPoint(worldPoint);
		if (CameraSystem.Instance.MainCamera.GetRenderType() == MainCamera.RenderMode.Lightweight)
		{
			float num = (float)Screen.width / (float)CameraSystem.Instance.MainCamera.RenderTexture.width;
			float num2 = (float)Screen.height / (float)CameraSystem.Instance.MainCamera.RenderTexture.height;
			vector.x *= num;
			vector.y *= num2;
		}
		return vector;
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x0002DF9C File Offset: 0x0002C19C
	public static Ray ScreenPointToRay(Vector3 screenPoint)
	{
		if (CameraSystem.Instance.MainCamera.GetRenderType() == MainCamera.RenderMode.Lightweight)
		{
			float num = (float)Screen.width / (float)CameraSystem.Instance.MainCamera.RenderTexture.width;
			float num2 = (float)Screen.height / (float)CameraSystem.Instance.MainCamera.RenderTexture.height;
			screenPoint.x /= num;
			screenPoint.y /= num2;
		}
		return CameraSystem.Instance.WorldCamera.ScreenPointToRay(screenPoint);
	}

	// Token: 0x04000AAC RID: 2732
	private const float SIZE_MULTIPLIER = 100f;

	// Token: 0x04000AAD RID: 2733
	private static CameraSystem instance;

	// Token: 0x04000AAE RID: 2734
	[SerializeField]
	private MainCamera worldCamera;

	// Token: 0x04000AAF RID: 2735
	[SerializeField]
	private Transform groundPointTransform;

	// Token: 0x04000AB0 RID: 2736
	private CameraController activeCameraController;

	// Token: 0x04000AB1 RID: 2737
	private List<CameraController> cameraControllers = new List<CameraController>();

	// Token: 0x04000AB2 RID: 2738
	[SerializeField]
	protected float followTime = 5f;

	// Token: 0x04000AB3 RID: 2739
	private static int curScreenWidth;

	// Token: 0x04000AB4 RID: 2740
	private static int curScreenHeight;
}
