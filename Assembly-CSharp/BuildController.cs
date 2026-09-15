using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000140 RID: 320
public class BuildController : MonoBehaviour
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x06000778 RID: 1912 RVA: 0x000234C0 File Offset: 0x000216C0
	// (set) Token: 0x06000779 RID: 1913 RVA: 0x0002350F File Offset: 0x0002170F
	public static BuildController Instance
	{
		get
		{
			if (BuildController.cachedInstance == null)
			{
				BuildController.cachedInstance = global::UnityEngine.Object.FindFirstObjectByType<BuildController>();
				if (BuildController.cachedInstance == null)
				{
					Debug.LogError(string.Format("Cannot find instance of {0} on current scene.", typeof(BuildController)));
				}
			}
			return BuildController.cachedInstance;
		}
		set
		{
			BuildController.cachedInstance = value;
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x0600077A RID: 1914 RVA: 0x00023517 File Offset: 0x00021717
	// (set) Token: 0x0600077B RID: 1915 RVA: 0x0002351F File Offset: 0x0002171F
	public bool IsBuildModeActive
	{
		get
		{
			return this.isBuildModeActive;
		}
		set
		{
			this.isBuildModeActive = value;
			Action<bool> onBuildModeStateChanged = this.OnBuildModeStateChanged;
			if (onBuildModeStateChanged == null)
			{
				return;
			}
			onBuildModeStateChanged(value);
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x0600077C RID: 1916 RVA: 0x00023539 File Offset: 0x00021739
	public WorldZone CurrentWorldZone
	{
		get
		{
			return this.currentWorldZone;
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x0600077D RID: 1917 RVA: 0x00023541 File Offset: 0x00021741
	public BuildLayout BuildLayout
	{
		get
		{
			return this.buildLayout;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x0600077E RID: 1918 RVA: 0x00023549 File Offset: 0x00021749
	public BuildArea CurrentFullCoverSoftHintArea
	{
		get
		{
			return this.currentFullCoverSoftHintArea;
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x0600077F RID: 1919 RVA: 0x00023551 File Offset: 0x00021751
	public Vector3 LastCursorScreenPosition
	{
		get
		{
			return this.lastCursorPos;
		}
	}

	// Token: 0x17000130 RID: 304
	// (get) Token: 0x06000780 RID: 1920 RVA: 0x00023559 File Offset: 0x00021759
	private bool IsRemoveMode
	{
		get
		{
			return this.currentBuildData != null && this.currentBuildData.BuildingMode == BuildingDef.BuildingMode.Remove;
		}
	}

	// Token: 0x14000001 RID: 1
	// (add) Token: 0x06000781 RID: 1921 RVA: 0x00023574 File Offset: 0x00021774
	// (remove) Token: 0x06000782 RID: 1922 RVA: 0x000235AC File Offset: 0x000217AC
	public event Action<bool> OnBuildModeStateChanged;

	// Token: 0x06000783 RID: 1923 RVA: 0x000235E4 File Offset: 0x000217E4
	private void Awake()
	{
		if (this.dockPointHintPrefab == null)
		{
			return;
		}
		this.dockPointHintPrefab.gameObject.SetActive(false);
		Transform transform = ((this.dockPointHintPoolParent != null) ? this.dockPointHintPoolParent : base.transform);
		this.dockPointHintsPool = new Pool(this.dockPointHintPrefab, transform, 0, Pool.PoolType.ImmediateActivation, false, null);
		this.buildLayout.BuildGrid3D.Init();
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x00023654 File Offset: 0x00021854
	public void EnableBuildMode(BuildData buildData, WorldZone worldZone, List<NeedItemData> itemNeeds = null, MultiInventory multiInventory = null)
	{
		this.currentWorldZone = worldZone;
		this.currentBuildData = buildData;
		this.isBuildModeInputLocked = false;
		this.gridStep = ((buildData.Definition != null) ? (BuildConsts.BUILD_GRID_SIZE * buildData.Definition.customGridStep) : BuildConsts.BUILD_GRID_SIZE);
		LazyUI.Get<HUD>().SetDisableState(HudStateType.BuildController, false, null);
		BuildingHUDData buildingHUDData = new BuildingHUDData(null, false);
		LazyUI.Get<BuildingHUD>().Draw(buildingHUDData);
		this.IsBuildModeActive = true;
		this.SetIgnoredStateForChunkableObjectsInWorldZone(worldZone);
		Physics.SyncTransforms();
		Rect wholeZoneRect = worldZone.Data.wholeZoneRect;
		Vector3 buildPos = worldZone.GetBuildPos();
		this.buildPointer.Enable(buildData, worldZone.Id, buildPos, itemNeeds, multiInventory);
		WgoBuildPointer wgoBuildPointer = this.buildPointer.PointerObject as WgoBuildPointer;
		bool flag = wgoBuildPointer != null && wgoBuildPointer.DrawBuffAreas;
		List<BuildElevationArea> buildElevationAreas = worldZone.GetBuildElevationAreas();
		Debug.Log(string.Format("BUILDING: elevationAreas.Count = {0}", buildElevationAreas.Count));
		this.buildLayout.EnableBuildingMode(buildPos, worldZone.Id, wholeZoneRect, buildData.Definition, flag, buildElevationAreas);
		this.gamepadCursorSpeed = this.GetGamepadCursorMinSpeed();
		this.lastCursorPos = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
		this.fullCoverSoftCacheBuilt = false;
		this.UpdateFullCoverSoftBuildAreaHints();
		this.UpdateDockPointsHints(true);
		Debug.Log(string.Format("BUILDING: WorldZone.GetBuildPos() = {0}", worldZone.GetBuildPos()));
		Debug.Log("BUILDING: worldZone.Id = " + worldZone.Id);
		this.cameraController.Enable(this.buildPointer.VisualCenter, worldZone.ZoneCollider);
		base.StartCoroutine(this.CenterPointerOnScreenAfterCameraUpdate());
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000237FC File Offset: 0x000219FC
	public void Update()
	{
		if (!this.IsBuildModeActive)
		{
			return;
		}
		if (this.isBuildModeInputLocked)
		{
			return;
		}
		if (MainGame.PlayerController != null && !MainGame.PlayerController.IsControlsEnabledExcept(new TakenControlType[] { TakenControlType.ByBuilding }))
		{
			return;
		}
		this.UpdatePointerAtPos(this.GetCursorPosition(Time.deltaTime), false);
		this.UpdateBuildModeInput();
		this.UpdateBuildModeTreeTransparencyOccluders();
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x00023860 File Offset: 0x00021A60
	public void DisableBuildMode()
	{
		LazyUI.Get<BuildingHUD>().Hide();
		LazyUI.Get<HUD>().SetDisableState(HudStateType.BuildController, true, null);
		this.IsBuildModeActive = false;
		this.isBuildModeInputLocked = false;
		Object3DTransparencyOccluder.Shared.Clear();
		this.ClearDockPointsHints();
		this.ClearFullCoverSoftBuildAreaHints();
		this.currentBuildData = null;
		this.buildLayout.DisableBuildingMode();
		this.cameraController.Disable();
		this.buildPointer.Disable();
		this.SetNotIgnoredStateForChunkableObjectsInCurrentWorldZone();
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x000238D8 File Offset: 0x00021AD8
	public void UpdatePointerObjectPosition(Vector3 snappedCursorPos)
	{
		this.curPosVisualCenter = snappedCursorPos;
		this.curPosActual = this.curPosVisualCenter - this.buildPointer.ShiftToVisualCenter;
		this.buildPointer.UpdatePos(this.curPosActual);
		this.buildPointer.UpdateAvailability();
		this.UpdateFullCoverSoftBuildAreaHints();
	}

	// Token: 0x06000788 RID: 1928 RVA: 0x0002392C File Offset: 0x00021B2C
	private void SetIgnoredStateForChunkableObjectsInWorldZone(WorldZone worldZone)
	{
		foreach (Wgo wgo in worldZone.Wgos)
		{
			wgo.UpdateFlag(ChunkingIgnoreType.Building, true);
		}
		if (this.currentWorldZone.AllStaticObjectsInZone == null)
		{
			BoxCollider zoneCollider = worldZone.ZoneCollider;
			Bounds bounds = new Bounds(zoneCollider.transform.TransformPoint(zoneCollider.center), zoneCollider.size);
			worldZone.AllStaticObjectsInZone = LazySingleton<ChunkManager>.Instance.GetAllChunkableObjectsInBoundsForSelectedLayers(new List<ChunkManagerLayerType> { ChunkManagerLayerType.StaticObjects }, bounds);
		}
		foreach (IChunkableObject chunkableObject in worldZone.AllStaticObjectsInZone)
		{
			chunkableObject.UpdateFlag(ChunkingIgnoreType.Building, true);
		}
	}

	// Token: 0x06000789 RID: 1929 RVA: 0x00023A10 File Offset: 0x00021C10
	private void SetNotIgnoredStateForChunkableObjectsInCurrentWorldZone()
	{
		foreach (IChunkableObject chunkableObject in this.currentWorldZone.AllStaticObjectsInZone)
		{
			chunkableObject.UpdateFlag(ChunkingIgnoreType.Building, false);
		}
		foreach (Wgo wgo in this.currentWorldZone.Wgos)
		{
			wgo.UpdateFlag(ChunkingIgnoreType.Building, false);
		}
	}

	// Token: 0x0600078A RID: 1930 RVA: 0x00023AB0 File Offset: 0x00021CB0
	private void UpdateBuildModeTreeTransparencyOccluders()
	{
		Object3DTransparencyOccluder shared = Object3DTransparencyOccluder.Shared;
		shared.BeginFrame();
		PlayerController playerController = MainGame.PlayerController;
		PlayerView playerView = ((playerController != null) ? playerController.View : null);
		if (playerView != null)
		{
			Vector3 position = playerView.transform.position;
			shared.AddOccludersFromCapsule(position, position + Vector3.up * 1.4f, 0.24f, 10f);
		}
		BuildPointer buildPointer = this.buildPointer;
		if (((buildPointer != null) ? buildPointer.VisualCenter : null) != null)
		{
			BuildPointerObject buildPointerObject = this.buildPointer.PointerObject as BuildPointerObject;
			if (buildPointerObject != null)
			{
				Bounds worldRoundedBounds = buildPointerObject.GetWorldRoundedBounds();
				Vector3 position2 = this.buildPointer.VisualCenter.position;
				float num = Mathf.Max(1.4f, worldRoundedBounds.size.y);
				float num2 = Mathf.Max(0.24f, Mathf.Max(worldRoundedBounds.extents.x, worldRoundedBounds.extents.z));
				Vector3 vector = new Vector3(position2.x, worldRoundedBounds.min.y, position2.z);
				shared.AddOccludersFromCapsule(vector, vector + Vector3.up * num, num2, 10f);
			}
		}
		shared.EndFrame();
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00023BF0 File Offset: 0x00021DF0
	private void UpdatePointerAtPos(Vector3 pos, bool forceUpdate = false)
	{
		this.lastCursorPos = pos;
		Ray ray = CameraSystem.ScreenPointToRay(this.lastCursorPos);
		Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, 100f, 2048))
		{
			float num = (float)Math.Round((double)(raycastHit.point.z / 0.0125f), MidpointRounding.AwayFromZero) * 0.0125f;
			Vector3 vector = new Vector3(raycastHit.point.x, raycastHit.point.y, num);
			if (this.currentWorldZone != null)
			{
				vector = VisualConsts.ProjectElevationPointToGround(vector, this.currentWorldZone.GroundPlaneY);
			}
			Vector3 vector2 = VisualConsts.GetRoundedPosXZ(vector - this.buildPointer.ShiftToVisualCenter, this.gridStep);
			vector2 += -Vector3.up * -0.006f;
			vector2 += this.buildPointer.ShiftToVisualCenter;
			this.lastSnappedCursorPos = vector2;
			float num2;
			if (this.currentWorldZone != null && this.currentWorldZone.TryGetBuildElevationY(vector2.x, vector2.z, out num2))
			{
				this.lastSnappedCursorPos = VisualConsts.ProjectGroundPointToElevation(vector2, num2);
			}
			if (!this.lastSnappedCursorPos.x.EqualsTo(this.curPosVisualCenter.x, 1E-05f) || !this.lastSnappedCursorPos.z.EqualsTo(this.curPosVisualCenter.z, 1E-05f) || !this.lastSnappedCursorPos.y.EqualsTo(this.curPosVisualCenter.y, 0.001f) || forceUpdate)
			{
				this.UpdatePointerObjectPosition(this.lastSnappedCursorPos);
			}
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00023DB8 File Offset: 0x00021FB8
	private void UpdateBuildModeInput()
	{
		if (this.isBuildModeInputLocked)
		{
			return;
		}
		if (LazyInput.GetKeyDown(GameKey.Back) || LazyInput.GetKeyDown(GameKey.RightClick))
		{
			this.DisableBuildMode();
			LazySingleton<BuildManager>.Instance.Disable();
			return;
		}
		if (LazyInput.GetKeyDown(GameKey.Rotate) && this.buildPointer.PointerObject.HasRotation())
		{
			this.buildPointer.Rotate();
			this.UpdatePointerObjectPosition(this.lastSnappedCursorPos);
			base.StartCoroutine(this.LockBuildInputAndDoDelayedAction(delegate
			{
				this.buildLayout.UpdateBuildingMode();
				this.buildPointer.UpdateAvailability();
				this.UpdateFullCoverSoftBuildAreaHints();
				this.UpdateDockPointsHints(true);
			}));
		}
		if (LazyInput.GetKeyDown(GameKey.Build) && this.buildPointer.TryBuildActionInput())
		{
			base.StartCoroutine(this.LockBuildInputAndDoDelayedAction(delegate
			{
				this.buildLayout.UpdateBuildingMode();
				this.buildPointer.UpdateAvailability();
				this.buildPointer.UpdateModulesLimitsWidget();
				this.UpdateFullCoverSoftBuildAreaHints();
				this.UpdateDockPointsHints(true);
			}));
			return;
		}
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00023E78 File Offset: 0x00022078
	private Vector3 GetCursorPosition(float deltaTime)
	{
		if (!LazyInput.IsGamepadActive)
		{
			return Input.mousePosition;
		}
		Vector3 vector = default(Vector3);
		if (LazyInput.GetKey(GameKey.DpadUp))
		{
			vector += Vector3.up;
		}
		else if (LazyInput.GetKey(GameKey.DpadDown))
		{
			vector += Vector3.down;
		}
		else if (LazyInput.GetKey(GameKey.DpadLeft))
		{
			vector += Vector3.left;
		}
		else if (LazyInput.GetKey(GameKey.DpadRight))
		{
			vector += Vector3.right;
		}
		if (!vector.sqrMagnitude.EqualsTo(0f, 1E-05f))
		{
			if (!this.IsRemoveMode && !this.dpadPressedOnce)
			{
				this.dpadPressedOnce = true;
				Vector2 vector2 = Vector2.Scale(BuildConsts.BUILD_GRID_SIZE_WORLD_UNIT, vector);
				Vector3 vector3 = CameraSystem.WorldToScreenPoint(this.curPosVisualCenter + new Vector3(vector2.x, 0f, vector2.y));
				return this.SnapToBounds(vector3);
			}
			return this.ProcessCursorContiniousMoving(vector, deltaTime, false);
		}
		else
		{
			this.dpadPressedOnce = false;
			vector = LazyInput.GetDirection();
			if (!vector.sqrMagnitude.EqualsTo(0f, 1E-05f))
			{
				return this.ProcessCursorContiniousMoving(vector, deltaTime, true);
			}
			this.gamepadCursorSpeed = this.GetGamepadCursorMinSpeed();
			if (this.IsRemoveMode)
			{
				return this.SnapToBounds(this.lastCursorPos);
			}
			return this.SnapToBounds(CameraSystem.WorldToScreenPoint(this.lastSnappedCursorPos));
		}
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x00023FE2 File Offset: 0x000221E2
	private IEnumerator CenterPointerOnScreenAfterCameraUpdate()
	{
		this.isBuildModeInputLocked = true;
		yield return new WaitForEndOfFrame();
		if (!this.IsBuildModeActive)
		{
			yield break;
		}
		this.lastCursorPos = new Vector3((float)Screen.width / 2f, (float)Screen.height / 2f, 0f);
		Debug.Log(string.Format("BUILDING: lastCursorPos = {0}", this.lastCursorPos));
		this.UpdatePointerAtPos(this.lastCursorPos, true);
		CameraSystem.Instance.ActiveCameraController.SetTargetInstant(this.buildPointer.VisualCenter);
		Debug.Log(string.Format("BUILDING: buildPointer.VisualCenter = {0}", this.buildPointer.VisualCenter.position));
		this.isBuildModeInputLocked = false;
		yield break;
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00023FF1 File Offset: 0x000221F1
	private IEnumerator LockBuildInputAndDoDelayedAction(Action delayedAction = null)
	{
		this.isBuildModeInputLocked = true;
		this.buildPointer.SetVisibleSelectionCells(false);
		yield return this.waitForFixedUpdate;
		this.buildPointer.SetVisibleSelectionCells(true);
		this.isBuildModeInputLocked = false;
		if (delayedAction != null)
		{
			delayedAction();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00024008 File Offset: 0x00022208
	private Vector3 ProcessCursorContiniousMoving(Vector3 inputDirection, float deltaTime, bool isStick = false)
	{
		if (this.IsRemoveMode)
		{
			this.gamepadCursorSpeed += this.gamepadRemoveCursorAcceleration * deltaTime;
			this.gamepadCursorSpeed = Mathf.Clamp(this.gamepadCursorSpeed, this.gamepadRemoveCursorMinSpeed, this.gamepadRemoveCursorMaxSpeed);
			float num = this.gamepadCursorSpeed * LazyUI.ScaleFactor * deltaTime;
			return this.SnapToBounds(this.lastCursorPos + inputDirection * num);
		}
		float num2 = (isStick ? (this.gamepadCursorAcceleration / inputDirection.magnitude) : this.gamepadCursorAcceleration);
		num2 = Mathf.Clamp(num2, this.gamepadCursorAcceleration, this.gamepadCursorMaxAcceleration);
		this.gamepadCursorSpeed += num2 * deltaTime;
		this.gamepadCursorSpeed = Mathf.Clamp(this.gamepadCursorSpeed, this.gamepadCursorMinSpeed, this.gamepadCursorMaxSpeed);
		Vector3 vector = this.lastCursorPos + inputDirection * this.gamepadCursorSpeed;
		return this.SnapToBounds(vector);
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x000240F0 File Offset: 0x000222F0
	private float GetGamepadCursorMinSpeed()
	{
		if (!this.IsRemoveMode)
		{
			return this.gamepadCursorMinSpeed;
		}
		return this.gamepadRemoveCursorMinSpeed;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00024108 File Offset: 0x00022308
	private Vector3 SnapToBounds(Vector3 pos)
	{
		pos.x = Mathf.Clamp(pos.x, this.gamepadCursorScreenOffsetX, (float)Screen.width - this.gamepadCursorScreenOffsetX);
		pos.y = Mathf.Clamp(pos.y, this.gamepadCursorScreenOffsetY, (float)Screen.height - this.gamepadCursorScreenOffsetY);
		return pos;
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00024160 File Offset: 0x00022360
	private void ClearDockPointsHints()
	{
		for (int i = 0; i < this.displayedDockPointHints.Count; i++)
		{
			DockPointHint dockPointHint = this.displayedDockPointHints[i];
			if (!(dockPointHint == null))
			{
				dockPointHint.Remove();
				if (this.dockPointHintsPool != null)
				{
					this.dockPointHintsPool.ReleaseObject<DockPointHint>(dockPointHint);
				}
				else
				{
					global::UnityEngine.Object.Destroy(dockPointHint.gameObject);
				}
			}
		}
		this.displayedDockPointHints.Clear();
	}

	// Token: 0x06000794 RID: 1940 RVA: 0x000241CC File Offset: 0x000223CC
	private void ShowDockPointsHints()
	{
		if (this.dockPointHintPrefab == null)
		{
			return;
		}
		BuildPointer buildPointer = this.buildPointer;
		WgoBuildPointer wgoBuildPointer = ((buildPointer != null) ? buildPointer.PointerObject : null) as WgoBuildPointer;
		if (wgoBuildPointer == null)
		{
			return;
		}
		Wgo target = wgoBuildPointer.Target;
		IReadOnlyList<DockPoint> readOnlyList = ((target != null) ? target.DockPoints : null);
		if (readOnlyList == null || readOnlyList.Count == 0)
		{
			return;
		}
		Transform transform = ((this.dockPointHintPoolParent != null) ? this.dockPointHintPoolParent : base.transform);
		for (int i = 0; i < readOnlyList.Count; i++)
		{
			DockPoint dockPoint = readOnlyList[i];
			if (!(dockPoint == null) && dockPoint.gameObject.activeInHierarchy && !dockPoint.HideInFighting && !dockPoint.IsForZombie)
			{
				DockPointHint dockPointHint = ((this.dockPointHintsPool != null) ? this.dockPointHintsPool.GetOrCreateObject<DockPointHint>() : global::UnityEngine.Object.Instantiate<DockPointHint>(this.dockPointHintPrefab));
				dockPointHint.transform.SetParent(transform);
				this.displayedDockPointHints.Add(dockPointHint);
				dockPointHint.Display(dockPoint);
			}
		}
	}

	// Token: 0x06000795 RID: 1941 RVA: 0x000242CC File Offset: 0x000224CC
	private void UpdateDockPointsHints(bool force)
	{
		if (!force)
		{
			return;
		}
		this.ClearDockPointsHints();
		this.ShowDockPointsHints();
	}

	// Token: 0x06000796 RID: 1942 RVA: 0x000242E0 File Offset: 0x000224E0
	private bool IsFullCoverSoftMode()
	{
		BuildingDef definition = this.currentBuildData.Definition;
		return definition != null && definition.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft && !string.IsNullOrEmpty(definition.customBuildAreaId) && this.buildPointer != null && this.buildPointer.PointerObject is WgoBuildPointer;
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x00024335 File Offset: 0x00022535
	private static Collider GetBuildAreaCollider(BuildArea buildArea)
	{
		if (buildArea == null)
		{
			return null;
		}
		if (buildArea.Collider != null)
		{
			return buildArea.Collider;
		}
		return buildArea.GetComponent<Collider>();
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00024360 File Offset: 0x00022560
	private static Vector3 GetColliderCenterWorld(Collider col)
	{
		if (col == null)
		{
			return Vector3.zero;
		}
		BoxCollider boxCollider = col as BoxCollider;
		if (boxCollider != null)
		{
			return boxCollider.transform.TransformPoint(boxCollider.center);
		}
		SphereCollider sphereCollider = col as SphereCollider;
		if (sphereCollider != null)
		{
			return sphereCollider.transform.TransformPoint(sphereCollider.center);
		}
		CapsuleCollider capsuleCollider = col as CapsuleCollider;
		if (capsuleCollider == null)
		{
			return col.bounds.center;
		}
		return capsuleCollider.transform.TransformPoint(capsuleCollider.center);
	}

	// Token: 0x06000799 RID: 1945 RVA: 0x000243E0 File Offset: 0x000225E0
	private void EnsureFullCoverSoftBuildAreasCacheBuilt()
	{
		if (this.fullCoverSoftCacheBuilt)
		{
			return;
		}
		this.fullCoverSoftCacheBuilt = true;
		this.fullCoverSoftBuildAreasCache.Clear();
		if (!this.IsFullCoverSoftMode())
		{
			return;
		}
		if (this.currentWorldZone == null || this.currentWorldZone.ZoneCollider == null)
		{
			return;
		}
		string customBuildAreaId = this.currentBuildData.Definition.customBuildAreaId;
		Bounds bounds = this.currentWorldZone.ZoneCollider.bounds;
		Collider[] array = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, 524288);
		if (array == null || array.Length == 0)
		{
			return;
		}
		foreach (Collider collider in array)
		{
			BuildArea buildArea;
			if (!(collider == null) && collider.TryGetComponent<BuildArea>(out buildArea) && !(buildArea.Id != customBuildAreaId) && !this.fullCoverSoftBuildAreasCache.Contains(buildArea))
			{
				this.fullCoverSoftBuildAreasCache.Add(buildArea);
			}
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000244D0 File Offset: 0x000226D0
	private void ClearFullCoverSoftBuildAreaHints()
	{
		foreach (KeyValuePair<BuildArea, Wgo> keyValuePair in this.fullCoverSoftHintWgos)
		{
			Wgo value = keyValuePair.Value;
			if (value != null)
			{
				global::UnityEngine.Object.Destroy(value.gameObject);
			}
		}
		this.fullCoverSoftHintWgos.Clear();
		this.fullCoverSoftHintBounds.Clear();
		this.fullCoverSoftBuildAreasCache.Clear();
		this.currentFullCoverSoftHintArea = null;
		this.fullCoverSoftCacheBuilt = false;
	}

	// Token: 0x0600079B RID: 1947 RVA: 0x00024568 File Offset: 0x00022768
	private static bool TryGetWgoBuildAreaBounds(Wgo wgo, out Bounds bounds)
	{
		bounds = default(Bounds);
		if (wgo == null)
		{
			return false;
		}
		bool flag = false;
		foreach (Collider collider in wgo.GetComponentsInChildren<Collider>(true))
		{
			if (!(collider == null) && collider.gameObject.activeInHierarchy && collider.gameObject.layer == 19)
			{
				Debug.Log(string.Format("col: {0}, bounds: {1}", collider.gameObject.name, collider.bounds));
				if (!flag)
				{
					flag = true;
					bounds = collider.bounds;
				}
				else
				{
					bounds.Encapsulate(collider.bounds);
				}
			}
		}
		return flag;
	}

	// Token: 0x0600079C RID: 1948 RVA: 0x00024610 File Offset: 0x00022810
	private Wgo GetOrCreateFullCoverSoftHint(BuildArea buildArea)
	{
		if (buildArea == null)
		{
			return null;
		}
		Wgo wgo;
		if (this.fullCoverSoftHintWgos.TryGetValue(buildArea, out wgo) && wgo != null)
		{
			return wgo;
		}
		GameScene currentGameScene = MainGame.PlayerController.CurrentGameScene;
		if (currentGameScene == null)
		{
			return null;
		}
		string text = (string.IsNullOrEmpty(this.currentBuildData.Definition.customWgoPlacePreview) ? this.currentBuildData.WgoId : this.currentBuildData.Definition.customWgoPlacePreview);
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		WgoData wgoData = new WgoData(text, Vector3.zero, currentGameScene.Id)
		{
			isTempObject = true
		};
		WgoBuildPointer wgoBuildPointer = this.buildPointer.PointerObject as WgoBuildPointer;
		if (wgoBuildPointer != null)
		{
			wgoData.MainWgoPartData.variationId = wgoBuildPointer.Target.MainWgoPart.WgoPartData.variationId;
			wgoData.MainWgoPartData.rotationIndex = (buildArea.HasRotationRequirement ? buildArea.RotationRequirement : wgoBuildPointer.Target.MainWgoPart.WgoPartData.rotationIndex);
		}
		Wgo wgo2 = Wgo.Spawn(wgoData, currentGameScene.transform, true, true, true, false);
		wgo2.UpdateChunkVisibility(true);
		Bounds bounds;
		if (BuildController.TryGetWgoBuildAreaBounds(wgo2, out bounds))
		{
			this.fullCoverSoftHintBounds[buildArea] = bounds;
		}
		foreach (Collider collider in wgo2.GetComponentsInChildren<Collider>(true))
		{
			if (collider != null)
			{
				collider.gameObject.SetActive(false);
			}
		}
		foreach (NavMeshCutBoxCustom navMeshCutBoxCustom in wgo2.GetComponentsInChildren<NavMeshCutBoxCustom>(true))
		{
			if (navMeshCutBoxCustom != null)
			{
				navMeshCutBoxCustom.gameObject.SetActive(false);
			}
		}
		wgo2.SetSelectionTint(Color.green, 0.35f);
		foreach (Collider collider2 in wgo2.GetComponentsInChildren<Collider>(true))
		{
			if (collider2 != null)
			{
				collider2.gameObject.SetActive(false);
			}
		}
		this.fullCoverSoftHintWgos[buildArea] = wgo2;
		return wgo2;
	}

	// Token: 0x0600079D RID: 1949 RVA: 0x00024820 File Offset: 0x00022A20
	private bool IsFullCoverSoftBuildAreaFree(BuildArea buildArea)
	{
		if (buildArea == null)
		{
			return false;
		}
		Collider buildAreaCollider = BuildController.GetBuildAreaCollider(buildArea);
		if (buildAreaCollider == null)
		{
			return false;
		}
		Wgo componentInParent = buildArea.GetComponentInParent<Wgo>();
		BuildingDef definition = this.currentBuildData.Definition;
		Bounds bounds = buildAreaCollider.bounds;
		Vector3 vector = Vector3.Max(Vector3.zero, bounds.extents - VisualConsts.XYZ_STEP);
		int num = Physics.OverlapBoxNonAlloc(bounds.center, vector, this.fullCoverSoftOverlapColliders, Quaternion.identity, 590080);
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.fullCoverSoftOverlapColliders[i];
			BuildArea buildArea2;
			ModuleSlotArea moduleSlotArea;
			if (!(collider == null) && !collider.TryGetComponent<BuildArea>(out buildArea2) && !collider.TryGetComponent<ModuleSlotArea>(out moduleSlotArea))
			{
				PlacementBlockingArea placementBlockingArea;
				if (PlacementBlockingArea.TryGet(collider, out placementBlockingArea))
				{
					if (PlacementBlockingArea.IsBlockingFor(collider, definition, null))
					{
						return false;
					}
				}
				else
				{
					Wgo componentInParent2 = collider.GetComponentInParent<Wgo>();
					if (componentInParent2 != null)
					{
						if (!(componentInParent2 == componentInParent) && componentInParent2.Data != null && !componentInParent2.Data.isTempObject && (definition == null || !definition.ShouldIgnoreWgoGroupAsObstacle(componentInParent2.Data.Definition.wgoGroup)))
						{
							return false;
						}
					}
					else
					{
						int layer = collider.gameObject.layer;
						if (layer == 8 || layer == 16)
						{
							return false;
						}
					}
				}
			}
		}
		return true;
	}

	// Token: 0x0600079E RID: 1950 RVA: 0x00024974 File Offset: 0x00022B74
	private static bool ContainsXZ(Bounds bounds, Vector3 point)
	{
		return point.x >= bounds.min.x && point.x <= bounds.max.x && point.z >= bounds.min.z && point.z <= bounds.max.z;
	}

	// Token: 0x0600079F RID: 1951 RVA: 0x000249D8 File Offset: 0x00022BD8
	private static bool FullyContainsXZ(Bounds container, Bounds inner, float eps = 0.001f)
	{
		return inner.min.x >= container.min.x - eps && inner.max.x <= container.max.x + eps && inner.min.z >= container.min.z - eps && inner.max.z <= container.max.z + eps;
	}

	// Token: 0x060007A0 RID: 1952 RVA: 0x00024A5C File Offset: 0x00022C5C
	private void UpdateFullCoverSoftBuildAreaHints()
	{
		if (!this.IsFullCoverSoftMode())
		{
			if (this.fullCoverSoftHintWgos.Count > 0)
			{
				this.ClearFullCoverSoftBuildAreaHints();
			}
			return;
		}
		this.EnsureFullCoverSoftBuildAreasCacheBuilt();
		if (this.fullCoverSoftBuildAreasCache.Count == 0)
		{
			return;
		}
		Bounds bounds = default(Bounds);
		if (this.buildPointer != null)
		{
			BuildPointerObject buildPointerObject = this.buildPointer.PointerObject as BuildPointerObject;
			if (buildPointerObject != null)
			{
				bounds = buildPointerObject.GetWorldRoundedBounds();
			}
		}
		BuildArea buildArea = null;
		for (int i = 0; i < this.fullCoverSoftBuildAreasCache.Count; i++)
		{
			BuildArea buildArea2 = this.fullCoverSoftBuildAreasCache[i];
			Collider buildAreaCollider = BuildController.GetBuildAreaCollider(buildArea2);
			if (!(buildAreaCollider == null) && BuildController.FullyContainsXZ(bounds, buildAreaCollider.bounds, 0.001f))
			{
				buildArea = buildArea2;
				break;
			}
		}
		this.currentFullCoverSoftHintArea = buildArea;
		for (int j = 0; j < this.fullCoverSoftBuildAreasCache.Count; j++)
		{
			BuildArea buildArea3 = this.fullCoverSoftBuildAreasCache[j];
			Collider buildAreaCollider2 = BuildController.GetBuildAreaCollider(buildArea3);
			if (!(buildAreaCollider2 == null))
			{
				if (!this.IsFullCoverSoftBuildAreaFree(buildArea3))
				{
					Wgo wgo;
					if (this.fullCoverSoftHintWgos.TryGetValue(buildArea3, out wgo) && wgo != null)
					{
						global::UnityEngine.Object.Destroy(wgo.gameObject);
						this.fullCoverSoftHintWgos.Remove(buildArea3);
						this.fullCoverSoftHintBounds.Remove(buildArea3);
					}
				}
				else
				{
					Wgo orCreateFullCoverSoftHint = this.GetOrCreateFullCoverSoftHint(buildArea3);
					if (!(orCreateFullCoverSoftHint == null))
					{
						Vector3 center = buildAreaCollider2.bounds.center;
						float y = buildArea3.transform.position.y;
						Debug.DrawLine(center, center + Vector3.up * 2.5f, Color.green, 5f);
						Vector3 vector = Vector3.zero;
						Bounds bounds2;
						if (this.fullCoverSoftHintBounds.TryGetValue(buildArea3, out bounds2))
						{
							Vector3 vector2 = -bounds2.center;
							vector2.y = 0f;
							vector = center + vector2;
							vector.y = y;
						}
						orCreateFullCoverSoftHint.Data.Position = vector;
						orCreateFullCoverSoftHint.transform.position = vector;
						orCreateFullCoverSoftHint.gameObject.SetActive(buildArea3 != buildArea);
					}
				}
			}
		}
	}

	// Token: 0x0400096D RID: 2413
	[SerializeField]
	private BuildPointer buildPointer;

	// Token: 0x0400096E RID: 2414
	[SerializeField]
	private BuildLayout buildLayout;

	// Token: 0x0400096F RID: 2415
	[SerializeField]
	private BuildModeCameraController cameraController;

	// Token: 0x04000970 RID: 2416
	[SerializeField]
	private DockPointHint dockPointHintPrefab;

	// Token: 0x04000971 RID: 2417
	[SerializeField]
	private Transform dockPointHintPoolParent;

	// Token: 0x04000972 RID: 2418
	[SerializeField]
	private float gamepadCursorMinSpeed = 2f;

	// Token: 0x04000973 RID: 2419
	[SerializeField]
	private float gamepadCursorMaxSpeed = 8f;

	// Token: 0x04000974 RID: 2420
	[SerializeField]
	private float gamepadCursorAcceleration = 6f;

	// Token: 0x04000975 RID: 2421
	[SerializeField]
	private float gamepadCursorMaxAcceleration = 12f;

	// Token: 0x04000976 RID: 2422
	[SerializeField]
	private float gamepadRemoveCursorMinSpeed = 250f;

	// Token: 0x04000977 RID: 2423
	[SerializeField]
	private float gamepadRemoveCursorMaxSpeed = 400f;

	// Token: 0x04000978 RID: 2424
	[SerializeField]
	private float gamepadRemoveCursorAcceleration = 75f;

	// Token: 0x04000979 RID: 2425
	[SerializeField]
	private float gamepadCursorScreenOffsetX = 50f;

	// Token: 0x0400097A RID: 2426
	[SerializeField]
	private float gamepadCursorScreenOffsetY = 50f;

	// Token: 0x0400097B RID: 2427
	private Vector3 curPosVisualCenter = Vector3.zero;

	// Token: 0x0400097C RID: 2428
	private Vector3 curPosActual = Vector3.zero;

	// Token: 0x0400097D RID: 2429
	private bool isBuildModeActive;

	// Token: 0x0400097E RID: 2430
	private Vector3 lastCursorPos;

	// Token: 0x0400097F RID: 2431
	private Vector3 lastSnappedCursorPos;

	// Token: 0x04000980 RID: 2432
	private float gamepadCursorSpeed;

	// Token: 0x04000981 RID: 2433
	private bool dpadPressedOnce;

	// Token: 0x04000982 RID: 2434
	private Vector2Int gridStep;

	// Token: 0x04000983 RID: 2435
	private bool isBuildModeInputLocked;

	// Token: 0x04000984 RID: 2436
	private readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

	// Token: 0x04000985 RID: 2437
	private WorldZone currentWorldZone;

	// Token: 0x04000986 RID: 2438
	private BuildData currentBuildData;

	// Token: 0x04000987 RID: 2439
	private readonly List<DockPointHint> displayedDockPointHints = new List<DockPointHint>();

	// Token: 0x04000988 RID: 2440
	private Pool dockPointHintsPool;

	// Token: 0x04000989 RID: 2441
	private readonly List<BuildArea> fullCoverSoftBuildAreasCache = new List<BuildArea>();

	// Token: 0x0400098A RID: 2442
	private readonly Dictionary<BuildArea, Wgo> fullCoverSoftHintWgos = new Dictionary<BuildArea, Wgo>();

	// Token: 0x0400098B RID: 2443
	private readonly Dictionary<BuildArea, Bounds> fullCoverSoftHintBounds = new Dictionary<BuildArea, Bounds>();

	// Token: 0x0400098C RID: 2444
	private readonly Collider[] fullCoverSoftOverlapColliders = new Collider[64];

	// Token: 0x0400098D RID: 2445
	private bool fullCoverSoftCacheBuilt;

	// Token: 0x0400098E RID: 2446
	private BuildArea currentFullCoverSoftHintArea;

	// Token: 0x0400098F RID: 2447
	private static BuildController cachedInstance;
}
