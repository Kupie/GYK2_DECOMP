using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000946 RID: 2374
public class MapPageWidget : LazyWidget<MapPageWidgetData>
{
	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06003E93 RID: 16019 RVA: 0x0012A55E File Offset: 0x0012875E
	public UIMapMilestone CurrentSelected
	{
		get
		{
			return this.currentSelected;
		}
	}

	// Token: 0x06003E94 RID: 16020 RVA: 0x0012A568 File Offset: 0x00128768
	public override void Init()
	{
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			base.Init();
			this.createdMilestones = new Dictionary<string, UIMapMilestone>();
			this.zones = new Dictionary<string, UIMapZone>();
			this.sortComponentsOtherMap = this.onAllUnlockedObject.GetComponentsInChildren<UISortComponent>(true).ToList<UISortComponent>();
			foreach (UIMapZone uimapZone in base.GetComponentsInChildren<UIMapZone>(true))
			{
				uimapZone.Init();
				this.zones.Add(uimapZone.Id, uimapZone);
				foreach (UISortComponent uisortComponent in uimapZone.SortComponents)
				{
					uisortComponent.transform.SetParent(this.hierarchySorter.HierarchyTarget);
				}
			}
			foreach (UISortComponent uisortComponent2 in this.sortComponentsOtherMap)
			{
				uisortComponent2.transform.SetParent(this.hierarchySorter.HierarchyTarget);
			}
			this.onAllUnlockedObject.gameObject.SetActive(false);
			this.hierarchySorter.ReinitChildrenAndSortComponents();
		}
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0012A6AC File Offset: 0x001288AC
	public void UpdateGamepadDependentStuff()
	{
		if (this.mapVirtualCursor != null)
		{
			this.mapVirtualCursor.gameObject.SetActive(LazyInput.IsGamepadActive);
		}
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x0012A6D4 File Offset: 0x001288D4
	public override void Redraw()
	{
		base.Redraw();
		this.currentSelected = null;
		this.UpdateMilestones();
		this.UpdatePlayerPos();
		this.UpdateZones();
		this.UpdateFightIcons();
		if (this.mapVirtualCursor != null && this.scrollRect != null && this.scrollRect.viewport != null)
		{
			this.mapVirtualCursor.SetBoundsRectTransform(this.scrollRect.viewport);
			this.CenterMapVirtualCursor();
		}
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x0012A758 File Offset: 0x00128958
	private void Update()
	{
		if (this.scrollRect == null || this.scrollRect.viewport == null || this.scrollRect.content == null)
		{
			return;
		}
		if (!LazyInput.IsGamepadActive)
		{
			this.PanMapByKeyboard();
			return;
		}
		if (this.mapVirtualCursor == null)
		{
			return;
		}
		Vector2 vector = this.mapVirtualCursor.transform.position;
		Vector2 desiredMovementDelta = this.mapVirtualCursor.DesiredMovementDelta;
		if (desiredMovementDelta.sqrMagnitude <= 0.0001f)
		{
			return;
		}
		this.PanMapByCursorBorder(vector, desiredMovementDelta);
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x0012A7F0 File Offset: 0x001289F0
	private void PanMapByKeyboard()
	{
		Vector2 direction = LazyInput.GetDirection();
		if (direction.sqrMagnitude <= 0.0001f)
		{
			return;
		}
		this.scrollRect.StopMovement();
		Vector2 vector = direction * this.keyboardPanSpeed * LazyUI.ScaleFactor * Time.deltaTime;
		Vector2 vector2 = this.scrollRect.content.anchoredPosition - this.GetViewportLocalDelta(vector);
		this.scrollRect.content.anchoredPosition = this.ClampContentAnchoredPosition(vector2);
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0012A874 File Offset: 0x00128A74
	private void PanMapByCursorBorder(Vector2 cursorPos, Vector2 cursorDelta)
	{
		this.scrollRect.viewport.GetWorldCorners(MapPageWidget.viewportCornersBuffer);
		Vector2 vector = MapPageWidget.viewportCornersBuffer[0];
		Vector2 vector2 = MapPageWidget.viewportCornersBuffer[2];
		Vector2 viewportWorldDelta = this.GetViewportWorldDelta(new Vector2(this.cursorBorderMargin, this.cursorBorderMargin));
		viewportWorldDelta = new Vector2(Mathf.Abs(viewportWorldDelta.x), Mathf.Abs(viewportWorldDelta.y));
		Vector2 vector3 = vector + viewportWorldDelta;
		Vector2 vector4 = vector2 - viewportWorldDelta;
		bool flag = (cursorDelta.x < 0f && cursorPos.x < vector3.x) || (cursorDelta.x > 0f && cursorPos.x > vector4.x);
		bool flag2 = (cursorDelta.y < 0f && cursorPos.y < vector3.y) || (cursorDelta.y > 0f && cursorPos.y > vector4.y);
		if (!flag && !flag2)
		{
			return;
		}
		Vector2 vector5 = new Vector2(flag ? cursorDelta.x : 0f, flag2 ? cursorDelta.y : 0f);
		Vector2 vector6 = this.scrollRect.content.anchoredPosition - this.GetViewportLocalDelta(vector5);
		this.scrollRect.content.anchoredPosition = this.ClampContentAnchoredPosition(vector6);
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0012A9E0 File Offset: 0x00128BE0
	private Vector2 GetViewportLocalDelta(Vector2 worldDelta)
	{
		return this.scrollRect.viewport.InverseTransformVector(worldDelta);
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0012A9FD File Offset: 0x00128BFD
	private Vector2 GetViewportWorldDelta(Vector2 localDelta)
	{
		return this.scrollRect.viewport.TransformVector(localDelta);
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0012AA1C File Offset: 0x00128C1C
	private Vector2 ClampContentAnchoredPosition(Vector2 targetAnchoredPos)
	{
		Vector2 sizeDelta = this.scrollRect.content.sizeDelta;
		Vector2 size = this.scrollRect.viewport.rect.size;
		float num = -(sizeDelta.x - size.x) * 0.5f;
		float num2 = (sizeDelta.x - size.x) * 0.5f;
		float num3 = -(sizeDelta.y - size.y) * 0.5f;
		float num4 = (sizeDelta.y - size.y) * 0.5f;
		if (sizeDelta.x <= size.x)
		{
			targetAnchoredPos.x = 0f;
		}
		else
		{
			targetAnchoredPos.x = Mathf.Clamp(targetAnchoredPos.x, num, num2);
		}
		if (sizeDelta.y <= size.y)
		{
			targetAnchoredPos.y = 0f;
		}
		else
		{
			targetAnchoredPos.y = Mathf.Clamp(targetAnchoredPos.y, num3, num4);
		}
		return targetAnchoredPos;
	}

	// Token: 0x06003E9D RID: 16029 RVA: 0x0012AB08 File Offset: 0x00128D08
	private void CenterMapVirtualCursor()
	{
		if (this.mapVirtualCursor == null || this.scrollRect == null || this.scrollRect.viewport == null)
		{
			return;
		}
		this.scrollRect.viewport.GetWorldCorners(MapPageWidget.viewportCornersBuffer);
		Vector3 vector = (MapPageWidget.viewportCornersBuffer[0] + MapPageWidget.viewportCornersBuffer[2]) * 0.5f;
		vector.z = this.mapVirtualCursor.transform.position.z;
		this.mapVirtualCursor.SetPosition(vector);
	}

	// Token: 0x06003E9E RID: 16030 RVA: 0x0012ABA8 File Offset: 0x00128DA8
	public void UpdatePlayerPos()
	{
		this.playerIcon.gameObject.SetActive(true);
		this.mapRect.gameObject.SetActive(true);
		this.scrollRect.content.sizeDelta = this.mapRect.sizeDelta;
		this.playerIcon.SetParent(this.mapRect);
		Vector3 value = this.data.PlayerData.position.Value;
		float num = Mathf.Min(GUIElements.Instance.WorldMin.position.x, GUIElements.Instance.WorldMax.position.x);
		float num2 = Mathf.Max(GUIElements.Instance.WorldMin.position.x, GUIElements.Instance.WorldMax.position.x);
		float num3 = Mathf.Min(GUIElements.Instance.WorldMin.position.z, GUIElements.Instance.WorldMax.position.z);
		float num4 = Mathf.Max(GUIElements.Instance.WorldMin.position.z, GUIElements.Instance.WorldMax.position.z);
		Rect rect = new Rect(num, num3, num2 - num, num4 - num3);
		float num5 = 0.017453292f * GUIElements.Instance.WorldMin.rotation.x;
		float num6 = value.z + value.y * Mathf.Tan(num5);
		if (rect.Contains(new Vector2(value.x, num6)))
		{
			Debug.Log("#Map# UpdatePlayerPos In World");
			float num7 = Mathf.InverseLerp(GUIElements.Instance.WorldMin.position.x, GUIElements.Instance.WorldMax.position.x, value.x);
			float num8 = Mathf.InverseLerp(GUIElements.Instance.WorldMin.position.z, GUIElements.Instance.WorldMax.position.z, num6);
			Vector2 sizeDelta = this.mapRect.sizeDelta;
			Vector2 vector = new Vector2((num7 - 0.5f) * sizeDelta.x, (num8 - 0.5f) * sizeDelta.y);
			this.playerIcon.anchoredPosition = vector;
			this.FocusOnPlayer();
		}
		else if (MainGame.PlayerData.CurrentWorldZoneData != null)
		{
			for (int i = 0; i < this.worldZonePoints.Count; i++)
			{
				MapPageWidget.WorldZonePoint worldZonePoint = this.worldZonePoints[i];
				if (worldZonePoint.worldZoneId == MainGame.PlayerData.CurrentWorldZoneData.id)
				{
					this.playerIcon.anchoredPosition = worldZonePoint.mapPosition;
					Debug.Log("#Map# UpdatePlayerPos In WorldZone:[" + worldZonePoint.worldZoneId + "]");
					this.FocusOnPlayer();
					return;
				}
			}
			this.playerIcon.gameObject.SetActive(false);
			Debug.Log("#Map# player not in World, not in WorldZone:[" + MainGame.PlayerData.CurrentWorldZoneData.id + "]. But it is not added into MapPage Config");
		}
		else
		{
			this.scrollRect.content.anchoredPosition = Vector2.zero;
			this.playerIcon.gameObject.SetActive(false);
			Debug.Log(string.Format("#Map# player not in World, not in WorldZone. Pos:[{0}]", value));
		}
		this.playerIcon.SetAsLastSibling();
		this.hierarchySorter.HierarchyTarget.SetAsLastSibling();
	}

	// Token: 0x06003E9F RID: 16031 RVA: 0x0012AEFF File Offset: 0x001290FF
	public void OnEnterMapMilestone(UIMapMilestone mapMilestone)
	{
		if (this.currentSelected != null)
		{
			this.currentSelected.OnEnter();
		}
		this.currentSelected = mapMilestone;
		this.currentSelected.OnEnter();
	}

	// Token: 0x06003EA0 RID: 16032 RVA: 0x0012AF2C File Offset: 0x0012912C
	public void OnExitMapMilestone(UIMapMilestone mapMilestone)
	{
		this.currentSelected.OnExit();
		if (this.currentSelected == mapMilestone)
		{
			this.currentSelected = null;
		}
	}

	// Token: 0x06003EA1 RID: 16033 RVA: 0x0012AF50 File Offset: 0x00129150
	public void OnPressMapMilestone(UIMapMilestone mapMilestone)
	{
		UIMapWindow window = LazyUI.GetWindow<UIMapWindow>();
		if (window.IsShown)
		{
			window.Close();
		}
		PlayerController.Teleport((!mapMilestone.UIMapMilestoneData.isNeedApplyPreset) ? new GDPointTeleportData(mapMilestone.WgoData.GetGDPointData("milestone_teleport_point"), "outdoor", "", null, false, 0.3f) : new GDPointTeleportData(mapMilestone.WgoData.GetGDPointData("milestone_teleport_point"), mapMilestone.UIMapMilestoneData.presetNameToApply, "", null, false, 0.3f));
		if (!string.IsNullOrEmpty(mapMilestone.UIMapMilestoneData.lazyExpressionOnTeleporting))
		{
			new LazyExpression(mapMilestone.UIMapMilestoneData.lazyExpressionOnTeleporting).Evaluate();
		}
	}

	// Token: 0x06003EA2 RID: 16034 RVA: 0x0012B000 File Offset: 0x00129200
	public static List<string> GetALlMapZonesForUnlock()
	{
		MapPageWidget mapPageWidget = LazyUI.GetWindow<UIMapWindow>().MapPageWidget;
		if (!mapPageWidget.isInitialized)
		{
			mapPageWidget.Init();
		}
		return mapPageWidget.zones.Keys.ToList<string>();
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0012B038 File Offset: 0x00129238
	private void UpdateZones()
	{
		bool flag = true;
		foreach (KeyValuePair<string, UIMapZone> keyValuePair in this.zones)
		{
			UIMapZone value = keyValuePair.Value;
			bool flag2 = MainGame.Instance.GameSave.knowledgeSystem.knownMapZones.Contains(value.Id);
			value.Draw(!flag2);
			if (!flag2)
			{
				flag = false;
			}
		}
		foreach (UISortComponent uisortComponent in this.sortComponentsOtherMap)
		{
			uisortComponent.gameObject.SetActive(!flag);
		}
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x0012B10C File Offset: 0x0012930C
	private void UpdateMilestones()
	{
		Transform worldMin = GUIElements.Instance.WorldMin;
		Transform worldMax = GUIElements.Instance.WorldMax;
		float num = 0.017453292f * GUIElements.Instance.WorldMin.rotation.x;
		Vector2 sizeDelta = this.mapRect.sizeDelta;
		foreach (UIMapMilestoneData uimapMilestoneData in this.milestonesData)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(uimapMilestoneData.wgoId);
			UIMapMilestone elementFromPool;
			bool flag = this.createdMilestones.TryGetValue(uimapMilestoneData.wgoId, out elementFromPool);
			if (wgoData == null || wgoData.IsHidden)
			{
				if (flag)
				{
					elementFromPool.gameObject.SetActive(false);
				}
			}
			else
			{
				if (!flag)
				{
					elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIMapMilestone>(this.milestonesRect);
					this.createdMilestones.Add(uimapMilestoneData.wgoId, elementFromPool);
				}
				if (wgoData.GetGameRes("activated_milestone") > 0f)
				{
					elementFromPool.DrawAsActivated(uimapMilestoneData, wgoData, this.data.MilestonesInteractable && this.data.CurrentMilestone != uimapMilestoneData.wgoId, new Action<UIMapMilestone>(this.OnPressMapMilestone));
				}
				else
				{
					elementFromPool.DrawAsNotActivated(uimapMilestoneData, wgoData);
				}
				if (uimapMilestoneData.hasCustomMapPos)
				{
					elementFromPool.RectTransform.anchoredPosition = uimapMilestoneData.customMapPos;
				}
				else
				{
					Vector3 position = wgoData.Position;
					float num2 = position.z + position.y * Mathf.Tan(num);
					elementFromPool.gameObject.SetActive(true);
					float num3 = Mathf.InverseLerp(worldMin.position.x, worldMax.position.x, position.x);
					float num4 = Mathf.InverseLerp(worldMin.position.z, worldMax.position.z, num2);
					Vector2 vector = new Vector2((num3 - 0.5f) * sizeDelta.x, (num4 - 0.5f) * sizeDelta.y);
					elementFromPool.RectTransform.anchoredPosition = vector;
				}
			}
		}
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x0012B348 File Offset: 0x00129548
	private void UpdateFightIcons()
	{
		foreach (GameObject gameObject in this.mapFightIcons)
		{
			gameObject.SetActive(MainGame.Instance.GameSave.knowledgeSystem.activeMapFightIcons.Contains(gameObject.name));
		}
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x0012B3BC File Offset: 0x001295BC
	private void FocusOnPlayer()
	{
		Vector2 vector = -this.playerIcon.anchoredPosition;
		Vector2 sizeDelta = this.scrollRect.content.sizeDelta;
		Vector2 size = this.scrollRect.viewport.rect.size;
		float num = -(sizeDelta.x - size.x) * 0.5f;
		float num2 = (sizeDelta.x - size.x) * 0.5f;
		float num3 = -(sizeDelta.y - size.y) * 0.5f;
		float num4 = (sizeDelta.y - size.y) * 0.5f;
		vector.x = Mathf.Clamp(vector.x, num, num2);
		vector.y = Mathf.Clamp(vector.y, num3, num4);
		this.scrollRect.content.anchoredPosition = vector;
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003128 RID: 12584
	private static readonly Vector3[] viewportCornersBuffer = new Vector3[4];

	// Token: 0x04003129 RID: 12585
	[SerializeField]
	private RectTransform playerIcon;

	// Token: 0x0400312A RID: 12586
	[SerializeField]
	private RectTransform mapRect;

	// Token: 0x0400312B RID: 12587
	[SerializeField]
	private RectTransform milestonesRect;

	// Token: 0x0400312C RID: 12588
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x0400312D RID: 12589
	[SerializeField]
	private float cursorBorderMargin = 10f;

	// Token: 0x0400312E RID: 12590
	[SerializeField]
	private float keyboardPanSpeed = 350f;

	// Token: 0x0400312F RID: 12591
	[SerializeField]
	private MapVirtualCursor mapVirtualCursor;

	// Token: 0x04003130 RID: 12592
	[SerializeField]
	private List<MapPageWidget.WorldZonePoint> worldZonePoints;

	// Token: 0x04003131 RID: 12593
	[SerializeField]
	private List<UIMapMilestoneData> milestonesData;

	// Token: 0x04003132 RID: 12594
	[SerializeField]
	private List<GameObject> mapFightIcons;

	// Token: 0x04003133 RID: 12595
	[SerializeField]
	private GameObject onAllUnlockedObject;

	// Token: 0x04003134 RID: 12596
	[SerializeField]
	private UIHierarchySorter hierarchySorter;

	// Token: 0x04003135 RID: 12597
	private List<UISortComponent> sortComponentsOtherMap;

	// Token: 0x04003136 RID: 12598
	private Dictionary<string, UIMapMilestone> createdMilestones;

	// Token: 0x04003137 RID: 12599
	private UIMapMilestone currentSelected;

	// Token: 0x04003138 RID: 12600
	private Dictionary<string, UIMapZone> zones;

	// Token: 0x04003139 RID: 12601
	private bool isInitialized;

	// Token: 0x02000947 RID: 2375
	[Serializable]
	private class WorldZonePoint
	{
		// Token: 0x0400313A RID: 12602
		public Vector2 mapPosition;

		// Token: 0x0400313B RID: 12603
		public string worldZoneId;
	}
}
