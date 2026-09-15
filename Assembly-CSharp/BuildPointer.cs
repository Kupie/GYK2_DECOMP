using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200014D RID: 333
public class BuildPointer : MonoBehaviour, IBubbleDrawable
{
	// Token: 0x17000143 RID: 323
	// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00026E6D File Offset: 0x0002506D
	public IBuildPointerObject PointerObject
	{
		get
		{
			return this.pointerObject;
		}
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x060007E9 RID: 2025 RVA: 0x00026E75 File Offset: 0x00025075
	public Vector3 ShiftToVisualCenter
	{
		get
		{
			return this.shiftToVisualCenter;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x060007EA RID: 2026 RVA: 0x00026E7D File Offset: 0x0002507D
	public Transform VisualCenter
	{
		get
		{
			return this.visualCenter;
		}
	}

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x060007EB RID: 2027 RVA: 0x00026E85 File Offset: 0x00025085
	public SGuid BubbleDrawableUniqueId
	{
		get
		{
			return this.bubbleUniqueId;
		}
	}

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x060007EC RID: 2028 RVA: 0x00026E8D File Offset: 0x0002508D
	public List<LazyWidgetDataBase> BubbleDrawableWidgets
	{
		get
		{
			return this.bubbleWidgets;
		}
	}

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x060007ED RID: 2029 RVA: 0x00026E98 File Offset: 0x00025098
	public Vector3 BubbleDrawablePosition
	{
		get
		{
			Vector3 vector = ((this.visualCenter != null) ? this.visualCenter.position : base.transform.position);
			vector.z += this.modulesLimitsWidgetOffsetZ;
			return vector;
		}
	}

	// Token: 0x060007EE RID: 2030 RVA: 0x00026EE0 File Offset: 0x000250E0
	public void Enable(BuildData buildData, string worldZoneId, Vector3 startPos, List<NeedItemData> itemNeeds = null, MultiInventory multiInventory = null)
	{
		base.transform.localPosition = Vector3.zero;
		this.pointerObject = this.CreatePointerObject(buildData, itemNeeds, multiInventory);
		this.pointerObject.Init(buildData, worldZoneId);
		this.pointerObject.SetupSelectionCells(new BuildSelectionCell[] { this.selectionCellPrefab, this.buffCellPrefab, this.removeCellPrefab }, this.pointerObject.transform);
		this.pointerObject.UpdateCollider();
		this.pointerObject.ApplySelectionCellsVisuals();
		this.CalculateShiftToVisualCenter();
		this.UpdatePos(startPos);
		this.UpdateModulesLimitsWidget();
	}

	// Token: 0x060007EF RID: 2031 RVA: 0x00026F7B File Offset: 0x0002517B
	public void Disable()
	{
		this.HideModulesLimitsWidget();
		this.pointerObject.OnPointerDisable();
		this.pointerObject.ClearSelectionCells();
		global::UnityEngine.Object.Destroy(this.pointerObject.gameObject);
	}

	// Token: 0x060007F0 RID: 2032 RVA: 0x00026FAC File Offset: 0x000251AC
	public void UpdateModulesLimitsWidget()
	{
		this.bubbleWidgets.Clear();
		BuildPointerObject buildPointerObject = this.pointerObject;
		BuildingDef buildingDef;
		if (buildPointerObject == null)
		{
			buildingDef = null;
		}
		else
		{
			BuildData buildData = buildPointerObject.BuildData;
			buildingDef = ((buildData != null) ? buildData.Definition : null);
		}
		BuildingDef buildingDef2 = buildingDef;
		if (buildingDef2 != null && buildingDef2.HasLimits)
		{
			this.bubbleWidgets.Add(new UIModulesLimitsWidgetData(buildingDef2));
		}
		if (UIObjectBubbleManager.Instance == null)
		{
			return;
		}
		if (this.bubbleWidgets.Count > 0)
		{
			UIObjectBubbleManager.Instance.RequestDisplay(this);
			return;
		}
		UIObjectBubbleManager.Instance.Hide(this);
	}

	// Token: 0x060007F1 RID: 2033 RVA: 0x00027032 File Offset: 0x00025232
	private void HideModulesLimitsWidget()
	{
		this.bubbleWidgets.Clear();
		if (UIObjectBubbleManager.Instance != null)
		{
			UIObjectBubbleManager.Instance.Hide(this);
		}
	}

	// Token: 0x060007F2 RID: 2034 RVA: 0x00027057 File Offset: 0x00025257
	public void UpdatePos(Vector3 position)
	{
		this.pos = position;
		base.transform.position = this.pos;
		this.pointerObject.UpdatePosition(position);
	}

	// Token: 0x060007F3 RID: 2035 RVA: 0x0002707D File Offset: 0x0002527D
	public void UpdateAvailability()
	{
		this.pointerObject.UpdateSelectionCellsState();
	}

	// Token: 0x060007F4 RID: 2036 RVA: 0x0002708C File Offset: 0x0002528C
	public bool TryBuildActionInput()
	{
		bool flag = this.pointerObject.TryDoBuildAction();
		if (flag && this.pointerObject.BuildData.Definition != null && this.pointerObject.BuildData.Definition.buildingMode != BuildingDef.BuildingMode.Remove)
		{
			LazyAudio.PlayAndForget("build_place");
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.BuildBuilding, this.pointerObject.BuildData.WgoId);
		}
		return flag;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x000270F4 File Offset: 0x000252F4
	public void Rotate()
	{
		base.transform.localPosition = Vector3.zero;
		this.pointerObject.transform.localPosition = Vector3.zero;
		this.pointerObject.ClearSelectionCells();
		this.pointerObject.Rotate();
		this.pointerObject.SetupSelectionCells(new BuildSelectionCell[] { this.selectionCellPrefab, this.buffCellPrefab }, this.pointerObject.transform);
		this.pointerObject.UpdateCollider();
		this.pointerObject.ApplySelectionCellsVisuals();
		this.pointerObject.UpdateSelectionCellsState();
		this.CalculateShiftToVisualCenter();
		this.pointerObject.ShowHints();
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0002719C File Offset: 0x0002539C
	public void SetVisibleSelectionCells(bool isVisible)
	{
		this.pointerObject.SetVisibleSelectionCells(isVisible);
	}

	// Token: 0x060007F7 RID: 2039 RVA: 0x000271AA File Offset: 0x000253AA
	private void Awake()
	{
		this.selectionCellPrefab.gameObject.SetActive(false);
		this.buffCellPrefab.gameObject.SetActive(false);
		this.removeCellPrefab.gameObject.SetActive(false);
	}

	// Token: 0x060007F8 RID: 2040 RVA: 0x000271DF File Offset: 0x000253DF
	private void CalculateShiftToVisualCenter()
	{
		this.shiftToVisualCenter = this.pointerObject.GetCellsCenterLocal();
		this.shiftToVisualCenter.y = 0f;
		this.visualCenter.localPosition = this.shiftToVisualCenter;
	}

	// Token: 0x060007F9 RID: 2041 RVA: 0x00027214 File Offset: 0x00025414
	private BuildPointerObject CreatePointerObject(BuildData buildData, List<NeedItemData> itemNeeds = null, MultiInventory multiInventory = null)
	{
		GameObject gameObject = new GameObject("BuildPointerObject");
		gameObject.transform.SetParent(base.gameObject.transform);
		gameObject.transform.localPosition = Vector3.zero;
		GameScene currentGameScene = MainGame.PlayerController.CurrentGameScene;
		switch (buildData.BuildingMode)
		{
		case BuildingDef.BuildingMode.Place:
		{
			WgoBuildPointer wgoBuildPointer = gameObject.AddComponent<WgoBuildPointer>();
			return this.PrepareAndSpawnWgoBuildPointer(wgoBuildPointer, buildData, currentGameScene, itemNeeds, multiInventory);
		}
		case BuildingDef.BuildingMode.Remove:
			return gameObject.AddComponent<RemovePointer>();
		case BuildingDef.BuildingMode.ConveyorPlace:
		{
			WgoBuildPointer wgoBuildPointer2 = gameObject.AddComponent<ConveyorBuildPointer>();
			return this.PrepareAndSpawnWgoBuildPointer(wgoBuildPointer2, buildData, currentGameScene, itemNeeds, multiInventory);
		}
		case BuildingDef.BuildingMode.FightingPlace:
		{
			WgoBuildPointer wgoBuildPointer3 = gameObject.AddComponent<FightingBuildPointer>();
			return this.PrepareAndSpawnWgoBuildPointer(wgoBuildPointer3, buildData, currentGameScene, itemNeeds, multiInventory);
		}
		case BuildingDef.BuildingMode.FightBuilding:
		{
			WgoBuildPointer wgoBuildPointer4 = gameObject.AddComponent<MilitaryBaseBuildPointer>();
			return this.PrepareAndSpawnWgoBuildPointer(wgoBuildPointer4, buildData, currentGameScene, itemNeeds, multiInventory);
		}
		case BuildingDef.BuildingMode.Upgrade:
		{
			UpgradeBuildPointer upgradeBuildPointer = gameObject.AddComponent<UpgradeBuildPointer>();
			return this.PrepareAndSpawnWgoBuildPointer(upgradeBuildPointer, buildData, currentGameScene, itemNeeds, multiInventory);
		}
		}
		string text = "{0} for data type [{1}] wasn't implemented";
		object obj = "BuildingMode";
		BuildingDef definition = buildData.Definition;
		throw new NotImplementedException(string.Format(text, obj, (definition != null) ? new BuildingDef.BuildingMode?(definition.buildingMode) : null));
	}

	// Token: 0x060007FA RID: 2042 RVA: 0x0002734C File Offset: 0x0002554C
	private WgoBuildPointer PrepareAndSpawnWgoBuildPointer(WgoBuildPointer wgoBuildPointer, BuildData buildData, GameScene gameScene, List<NeedItemData> itemNeeds = null, MultiInventory multiInventory = null)
	{
		Wgo wgo = Wgo.Spawn(new WgoData(string.IsNullOrEmpty(buildData.Definition.customWgoPlacePreview) ? buildData.WgoId : buildData.Definition.customWgoPlacePreview, base.transform.position, gameScene.Id)
		{
			isTempObject = true
		}, gameScene.transform, true, true, true, false);
		wgo.UpdateChunkVisibility(true);
		WgoPart mainWgoPart = wgo.MainWgoPart;
		if (mainWgoPart != null)
		{
			mainWgoPart.TryApplyCustomRotationSequenceStart();
		}
		wgoBuildPointer.SetTarget(wgo, gameScene, buildData.Definition, itemNeeds, multiInventory);
		wgoBuildPointer.ShowHints();
		foreach (Collider collider in wgo.GetComponentsInChildren<Collider>(true))
		{
			int layer = collider.gameObject.layer;
			if (layer == 8 || layer == 11 || layer == 6)
			{
				collider.gameObject.SetActive(false);
			}
		}
		NavMeshCutBoxCustom[] componentsInChildren2 = wgo.GetComponentsInChildren<NavMeshCutBoxCustom>(true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].gameObject.SetActive(false);
		}
		return wgoBuildPointer;
	}

	// Token: 0x040009DC RID: 2524
	[SerializeField]
	private BuildSelectionCell selectionCellPrefab;

	// Token: 0x040009DD RID: 2525
	[SerializeField]
	private BuildSelectionCell buffCellPrefab;

	// Token: 0x040009DE RID: 2526
	[SerializeField]
	private BuildSelectionCell removeCellPrefab;

	// Token: 0x040009DF RID: 2527
	private Vector3 pos;

	// Token: 0x040009E0 RID: 2528
	private BuildPointerObject pointerObject;

	// Token: 0x040009E1 RID: 2529
	private Vector3 shiftToVisualCenter;

	// Token: 0x040009E2 RID: 2530
	[SerializeField]
	private Transform visualCenter;

	// Token: 0x040009E3 RID: 2531
	[SerializeField]
	private float modulesLimitsWidgetOffsetZ = 0.35f;

	// Token: 0x040009E4 RID: 2532
	private readonly SGuid bubbleUniqueId = new SGuid();

	// Token: 0x040009E5 RID: 2533
	private readonly List<LazyWidgetDataBase> bubbleWidgets = new List<LazyWidgetDataBase>();
}
