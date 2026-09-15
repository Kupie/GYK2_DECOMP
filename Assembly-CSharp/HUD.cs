using System;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000881 RID: 2177
[RequireComponent(typeof(Canvas))]
public class HUD : LazyWidget<HUDData>
{
	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x060037B7 RID: 14263 RVA: 0x0010C7EC File Offset: 0x0010A9EC
	// (remove) Token: 0x060037B8 RID: 14264 RVA: 0x0010C824 File Offset: 0x0010AA24
	public event Action onTechPointsPanelShown;

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x060037B9 RID: 14265 RVA: 0x0010C859 File Offset: 0x0010AA59
	public HUDMode Mode
	{
		get
		{
			return this.mode;
		}
	}

	// Token: 0x060037BA RID: 14266 RVA: 0x0010C864 File Offset: 0x0010AA64
	public override void Init()
	{
		this.canvas = base.GetComponent<Canvas>();
		this.canvas.overrideSorting = true;
		this.canvas.sortingOrder = 50;
		this.disableStateType = new MultiFlagAND<HudStateType>();
		this.disableStateType.Init(null, true);
		this.SnapTechPointsPanelToCurrentState();
		if (this.tutorialListBtn != null)
		{
			this.tutorialListBtn.onClick.AddListener(new UnityAction(this.OpenTutorialList));
		}
		GUIElements.OnWindowSizeTypeChanged += this.OnWindowSizeTypeChanged;
		UIGameBindingSettingsWindow.OnBtnUpdated += this.hotBarWidget.UpdateButtonsText;
		UIMouseTooltip.Attach(this.happinessLabel.gameObject, "tt_town_happiness", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
		this.energySanityBar.AttachHudIconTooltips();
		if (base.GetComponent<GraphicRaycaster>() == null)
		{
			base.gameObject.AddComponent<GraphicRaycaster>();
		}
		if (this.canvas.renderMode != RenderMode.ScreenSpaceOverlay && this.canvas.worldCamera == null && CameraSystem.Instance != null)
		{
			this.canvas.worldCamera = CameraSystem.Instance.WorldCamera;
		}
		if (this.magnifyingGlassWidgetPrefab != null)
		{
			this.magnifyingGlassWidgetPool = new Pool(this.magnifyingGlassWidgetPrefab, base.transform, 1, Pool.PoolType.ImmediateActivation, false, null);
			this.magnifyingGlassWidgetPrefab.gameObject.SetActive(false);
		}
	}

	// Token: 0x060037BB RID: 14267 RVA: 0x0010C9D2 File Offset: 0x0010ABD2
	protected override void SetData(HUDData data)
	{
		base.SetData(data);
		this.hotBarWidget.SetDataOutside(data.HotBarWidgetData);
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x0010C9EC File Offset: 0x0010ABEC
	public override void Draw()
	{
		base.Draw();
		if (this.data != null)
		{
			this.Redraw();
		}
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x0010CA04 File Offset: 0x0010AC04
	public override void Redraw()
	{
		base.Redraw();
		this.energySanityBar.Draw(this.data.EnergySanityBarData);
		this.gameResNotificatior.Draw(this.data.GameResNotificatiorData);
		this.buffsDisplay.Draw(this.data.BuffsDisplayData);
		this.hotBarWidget.Draw(this.data.HotBarWidgetData);
		this.wheel.forceTimeDependentChange = true;
		this.wheel.OnNewDayStarted(EnvironmentEngine.Instance.Data.Day, false);
		this.wheel.OnTimeOfDayChanged(EnvironmentEngine.Instance.timeOfDay, false);
		this.UpdateGamepadDependentStuff();
		this.UpdateTechPointsInstant();
		this.TrySubscribeGameEvents();
		this.TrySubscribeMagnifyingGlassTracking();
		this.UpdateHotBarEnabledState();
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x0010CACA File Offset: 0x0010ACCA
	public override void Hide()
	{
		base.Hide();
		this.energySanityBar.Hide();
		this.buffsDisplay.Hide();
		this.TryUnsubscribeGameEvents();
		this.TryUnsubscribeMagnifyingGlassTracking();
	}

	// Token: 0x060037BF RID: 14271 RVA: 0x0010CAF4 File Offset: 0x0010ACF4
	public void DrawFightingTimeline(UIFightingTimelineRendererData widgetData)
	{
		this.fightingTimelineWidget.Draw(widgetData);
		this.uiFightingSquadHudGroupWidget.Draw(new UIFightingSquadHudGroupWidgetData(widgetData.CurrentLevel));
	}

	// Token: 0x060037C0 RID: 14272 RVA: 0x0010CB18 File Offset: 0x0010AD18
	public void HideFightingTimeline()
	{
		this.fightingTimelineWidget.Hide();
		this.uiFightingSquadHudGroupWidget.Hide();
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x0010CB30 File Offset: 0x0010AD30
	public void SetMode(HUDMode mode)
	{
		this.mode = mode;
		RectTransform rectTransform = this.buffsDisplay.RectTransform;
		RectTransform content = this.buffsDisplay.Content;
		Vector2 vector;
		if (mode == HUDMode.Common)
		{
			this.leftUpGroup.SetActive(true);
			this.rightUpGroup.SetActive(true);
			this.fightingControlsWidget.gameObject.SetActive(false);
			this.fightingTimelineWidget.gameObject.SetActive(false);
			this.uiFightingSquadHudGroupWidget.gameObject.SetActive(false);
			RectTransform rectTransform2 = rectTransform;
			RectTransform rectTransform3 = rectTransform;
			vector = new Vector2(0.5f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform2.anchorMin = vector;
			rectTransform.anchoredPosition = Vector2.zero;
			content.pivot = new Vector2(0.5f, 1f);
			content.anchoredPosition = new Vector2(0f, -9.5f);
			return;
		}
		if (mode != HUDMode.Fight)
		{
			throw new ArgumentOutOfRangeException("mode", mode, null);
		}
		this.leftUpGroup.SetActive(false);
		this.rightUpGroup.SetActive(false);
		this.fightingControlsWidget.gameObject.SetActive(true);
		this.fightingControlsWidget.Draw(new UIFightingControlsWidgetData());
		this.fightingTimelineWidget.gameObject.SetActive(true);
		this.uiFightingSquadHudGroupWidget.gameObject.SetActive(true);
		RectTransform rectTransform4 = rectTransform;
		RectTransform rectTransform5 = rectTransform;
		vector = new Vector2(0f, 1f);
		rectTransform5.anchorMax = vector;
		rectTransform4.anchorMin = vector;
		rectTransform.anchoredPosition = Vector2.zero;
		content.pivot = new Vector2(0f, 1f);
		content.anchoredPosition = new Vector2(8f, -9.5f);
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x0010CCCC File Offset: 0x0010AECC
	private void Update()
	{
		if (this.isTechPointsPanelVisible)
		{
			this.hideTechPointsPanelTimer -= Time.deltaTime;
			if (this.hideTechPointsPanelTimer <= 0f)
			{
				this.TurnOffTechPointsPanel();
			}
		}
		if (this.mode == HUDMode.Fight)
		{
			this.fightingControlsWidget.CustomUpdate();
		}
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x0010CD1C File Offset: 0x0010AF1C
	public void SetDisableState(HudStateType type, bool isEnabled, HUDData dataToDraw = null)
	{
		this.disableStateType.UpdateFlag(type, isEnabled);
		Debug.Log(string.Format("HUD: SetDisableState:[{0}] isDisabled:[{1}] disableStateType.ResultFlag:[{2}]", type, isEnabled, this.disableStateType.ResultFlag));
		if (!this.disableStateType.ResultFlag)
		{
			if (dataToDraw != null)
			{
				this.SetData(dataToDraw);
			}
			this.Hide();
			return;
		}
		if (dataToDraw != null)
		{
			this.Draw(dataToDraw);
			return;
		}
		this.Draw(this.data);
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x0010CD98 File Offset: 0x0010AF98
	private void TrySubscribeGameEvents()
	{
		if (!this.subscribedGameEvents)
		{
			this.subscribedGameEvents = true;
			GK2GameResSystem system = GK2GameResSystem.GetSystem("happiness");
			system.onValueChanged = (Action<float>)Delegate.Combine(system.onValueChanged, new Action<float>(this.UpdateHappinessInstant));
			GK2GameResSystem system2 = GK2GameResSystem.GetSystem("tech_red");
			system2.onValueChanged = (Action<float>)Delegate.Combine(system2.onValueChanged, new Action<float>(this.UpdateRedTechInstant));
			GK2GameResSystem system3 = GK2GameResSystem.GetSystem("tech_green");
			system3.onValueChanged = (Action<float>)Delegate.Combine(system3.onValueChanged, new Action<float>(this.UpdateGreenTechInstant));
			GK2GameResSystem system4 = GK2GameResSystem.GetSystem("tech_blue");
			system4.onValueChanged = (Action<float>)Delegate.Combine(system4.onValueChanged, new Action<float>(this.UpdateBlueTechInstant));
			TownSystem.OnQualityChanged += this.UpdateHappinessInstant;
			KnowledgeSystem.OnTutorialViewed = (Action<string>)Delegate.Combine(KnowledgeSystem.OnTutorialViewed, new Action<string>(this.HandleTutorialViewed));
			LazyInput.OnInputChanged += this.UpdateGamepadDependentStuff;
			this.subscribedPlayerController = MainGame.PlayerController;
			this.subscribedPlayerController.OnControlStateChanged += this.HandlePlayerControlStateChanged;
			LazyWindowsStackController.OnWindowOpened += this.UpdateHotBarEnabledState;
			LazyWindowsStackController.OnWindowClosed += this.UpdateHotBarEnabledState;
			LazyWindowsStackController.OnAllWindowsClosed += this.UpdateHotBarEnabledState;
			CharacterWindow.OnTabChanged += this.UpdateHotBarEnabledState;
		}
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x0010CF0C File Offset: 0x0010B10C
	private void TryUnsubscribeGameEvents()
	{
		if (this.subscribedGameEvents)
		{
			this.subscribedGameEvents = false;
			GK2GameResSystem system = GK2GameResSystem.GetSystem("happiness");
			system.onValueChanged = (Action<float>)Delegate.Remove(system.onValueChanged, new Action<float>(this.UpdateHappinessInstant));
			GK2GameResSystem system2 = GK2GameResSystem.GetSystem("tech_red");
			system2.onValueChanged = (Action<float>)Delegate.Remove(system2.onValueChanged, new Action<float>(this.UpdateRedTechInstant));
			GK2GameResSystem system3 = GK2GameResSystem.GetSystem("tech_green");
			system3.onValueChanged = (Action<float>)Delegate.Remove(system3.onValueChanged, new Action<float>(this.UpdateGreenTechInstant));
			GK2GameResSystem system4 = GK2GameResSystem.GetSystem("tech_blue");
			system4.onValueChanged = (Action<float>)Delegate.Remove(system4.onValueChanged, new Action<float>(this.UpdateBlueTechInstant));
			TownSystem.OnQualityChanged -= this.UpdateHappinessInstant;
			KnowledgeSystem.OnTutorialViewed = (Action<string>)Delegate.Remove(KnowledgeSystem.OnTutorialViewed, new Action<string>(this.HandleTutorialViewed));
			LazyInput.OnInputChanged -= this.UpdateGamepadDependentStuff;
			this.UnsubscribePlayerControlStateChanged();
			LazyWindowsStackController.OnWindowOpened -= this.UpdateHotBarEnabledState;
			LazyWindowsStackController.OnWindowClosed -= this.UpdateHotBarEnabledState;
			LazyWindowsStackController.OnAllWindowsClosed -= this.UpdateHotBarEnabledState;
			CharacterWindow.OnTabChanged -= this.UpdateHotBarEnabledState;
		}
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x0010D064 File Offset: 0x0010B264
	private void TrySubscribeMagnifyingGlassTracking()
	{
		if (this.subscribedMagnifyingGlassTracking || this.magnifyingGlassWidgetPool == null)
		{
			return;
		}
		this.subscribedMagnifyingGlassTracking = true;
		WgoData.OnAnyInteractionEventChanged += this.HandleInteractionEventChangedAndRedraw;
		PlayerController.OnPlayerTeleported += this.HandlePlayerTeleportedMagnifyingGlasses;
		CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.UpdateMagnifyingGlasses));
		this.ScanExistingWorkEvents();
		this.UpdateMagnifyingGlasses(null);
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x0010D0D0 File Offset: 0x0010B2D0
	private void TryUnsubscribeMagnifyingGlassTracking()
	{
		if (!this.subscribedMagnifyingGlassTracking)
		{
			return;
		}
		this.subscribedMagnifyingGlassTracking = false;
		WgoData.OnAnyInteractionEventChanged -= this.HandleInteractionEventChangedAndRedraw;
		PlayerController.OnPlayerTeleported -= this.HandlePlayerTeleportedMagnifyingGlasses;
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.UpdateMagnifyingGlasses));
		this.UntrackAllMagnifyingGlasses();
	}

	// Token: 0x060037C8 RID: 14280 RVA: 0x0010D12C File Offset: 0x0010B32C
	private void ScanExistingWorkEvents()
	{
		if (MainGame.WorldData == null || !MainGame.WorldData.HasCache)
		{
			return;
		}
		foreach (WgoData wgoData in MainGame.WorldData.Cache.wgoDataByUidCache.Values)
		{
			this.HandleInteractionEventChanged(wgoData);
		}
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x0010D1A4 File Offset: 0x0010B3A4
	private void HandleInteractionEventChangedAndRedraw(WgoData wgoData)
	{
		this.HandleInteractionEventChanged(wgoData);
		this.UpdateMagnifyingGlasses(null);
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x0010D1B4 File Offset: 0x0010B3B4
	private void HandleInteractionEventChanged(WgoData wgoData)
	{
		if (wgoData == null)
		{
			return;
		}
		if (HUD.HasWorkInteractionEvent(wgoData))
		{
			this.TrackWorkEventTarget(wgoData);
			return;
		}
		this.UntrackWorkEventTarget(wgoData.UniqueId);
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x0010D1D8 File Offset: 0x0010B3D8
	private static bool HasWorkInteractionEvent(WgoData wgoData)
	{
		InteractionEvent interactionEvent = wgoData.PeekFirstAddedEvent();
		return interactionEvent != null && interactionEvent.type == InteractionEvent.Type.Work;
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x0010D1FC File Offset: 0x0010B3FC
	private void TrackWorkEventTarget(WgoData wgoData)
	{
		SGuid uniqueId = wgoData.UniqueId;
		if (this.trackedWorkEventTargets.ContainsKey(uniqueId))
		{
			this.trackedWorkEventTargets[uniqueId] = wgoData;
			this.RefreshMagnifyingGlassWorldOverride(uniqueId, wgoData);
			return;
		}
		this.trackedWorkEventTargets.Add(uniqueId, wgoData);
		this.drawingMagnifyingGlassData.Add(uniqueId, new UIMagnifyingGlassWidgetData(uniqueId, Vector2.zero, Vector2.zero));
		this.RefreshMagnifyingGlassWorldOverride(uniqueId, wgoData);
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x0010D268 File Offset: 0x0010B468
	private void UntrackWorkEventTarget(SGuid uniqueId)
	{
		if (SGuid.IsNullOrEmpty(uniqueId) || !this.trackedWorkEventTargets.ContainsKey(uniqueId))
		{
			return;
		}
		this.ReleaseMagnifyingGlassWidget(uniqueId);
		this.drawingMagnifyingGlassData.Remove(uniqueId);
		this.magnifyingGlassDoorOverride.Remove(uniqueId);
		this.trackedWorkEventTargets.Remove(uniqueId);
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x0010D2BC File Offset: 0x0010B4BC
	private void UntrackAllMagnifyingGlasses()
	{
		this.magnifyingGlassUntrackBuffer.Clear();
		foreach (SGuid sguid in this.trackedWorkEventTargets.Keys)
		{
			this.magnifyingGlassUntrackBuffer.Add(sguid);
		}
		foreach (SGuid sguid2 in this.magnifyingGlassUntrackBuffer)
		{
			this.UntrackWorkEventTarget(sguid2);
		}
	}

	// Token: 0x060037CF RID: 14287 RVA: 0x0010D368 File Offset: 0x0010B568
	private void UpdateMagnifyingGlasses(CinemachineBrain brain)
	{
		if (this.magnifyingGlassWidgetPool == null || this.trackedWorkEventTargets.Count == 0 || CameraSystem.Instance == null)
		{
			return;
		}
		Bounds screenBounds = LazyUI.GetScreenBounds();
		Vector2 vector = new Vector2(screenBounds.center.x, screenBounds.center.y);
		Rect rect = new Rect(screenBounds.min.x + this.clampWidgetOffsetFromScreenBorder, screenBounds.min.y + this.clampWidgetOffsetFromScreenBorder, screenBounds.size.x - this.clampWidgetOffsetFromScreenBorder * 2f, screenBounds.size.y - this.clampWidgetOffsetFromScreenBorder * 2f);
		this.magnifyingGlassUntrackBuffer.Clear();
		foreach (KeyValuePair<SGuid, WgoData> keyValuePair in this.trackedWorkEventTargets)
		{
			SGuid key = keyValuePair.Key;
			WgoData value = keyValuePair.Value;
			if (value != null)
			{
				WorldData worldData = MainGame.WorldData;
				if (((worldData != null) ? worldData.GetWgoData(key) : null) != null && HUD.HasWorkInteractionEvent(value))
				{
					this.RefreshMagnifyingGlassWorldOverride(key, value);
					Vector2 vector2 = CameraSystem.WorldToScreenPoint(this.GetMagnifyingGlassTrackedWorldPosition(key, value));
					bool flag = !rect.Contains(vector2);
					bool flag2 = this.magnifyingGlassDoorOverride.ContainsKey(key);
					UIObjectBubble uiobjectBubble;
					if ((!flag2 && UIObjectBubbleManager.Instance != null && UIObjectBubbleManager.Instance.TryGetDisplayedBubble(key, out uiobjectBubble) && !uiobjectBubble.IsOutOfScreen) || (!flag && !flag2))
					{
						this.ReleaseMagnifyingGlassWidget(key);
						continue;
					}
					UIMagnifyingGlassWidgetData uimagnifyingGlassWidgetData = this.drawingMagnifyingGlassData[key];
					uimagnifyingGlassWidgetData.IsOutOfScreen = flag;
					if (flag)
					{
						uimagnifyingGlassWidgetData.ScreenPosition = this.GetRectEdgeIntersection(vector, vector2, rect);
						uimagnifyingGlassWidgetData.DirectionToTarget = HUD.GetPointerDirectionFromEdgePosition(uimagnifyingGlassWidgetData.ScreenPosition, rect);
					}
					else
					{
						uimagnifyingGlassWidgetData.ScreenPosition = vector2;
						uimagnifyingGlassWidgetData.DirectionToTarget = Vector2.zero;
					}
					UIMagnifyingGlassWidget orCreateObject;
					if (!this.drawingMagnifyingGlasses.TryGetValue(key, out orCreateObject))
					{
						orCreateObject = this.magnifyingGlassWidgetPool.GetOrCreateObject<UIMagnifyingGlassWidget>();
						orCreateObject.transform.SetParent(base.transform);
						orCreateObject.transform.SetAsLastSibling();
						this.drawingMagnifyingGlasses.Add(key, orCreateObject);
					}
					orCreateObject.Draw(uimagnifyingGlassWidgetData);
					continue;
				}
			}
			this.magnifyingGlassUntrackBuffer.Add(key);
		}
		foreach (SGuid sguid in this.magnifyingGlassUntrackBuffer)
		{
			this.UntrackWorkEventTarget(sguid);
		}
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x0010D640 File Offset: 0x0010B840
	private void HandlePlayerTeleportedMagnifyingGlasses()
	{
		foreach (KeyValuePair<SGuid, WgoData> keyValuePair in this.trackedWorkEventTargets)
		{
			this.RefreshMagnifyingGlassWorldOverride(keyValuePair.Key, keyValuePair.Value);
		}
		this.UpdateMagnifyingGlasses(null);
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x0010D6A8 File Offset: 0x0010B8A8
	private void RefreshMagnifyingGlassWorldOverride(SGuid uniqueId, WgoData wgoData)
	{
		Vector3 magnifyingGlassObjectWorldPosition = HUD.GetMagnifyingGlassObjectWorldPosition(uniqueId, wgoData);
		Vector3 vector = ((MainGame.PlayerController != null) ? MainGame.PlayerController.transform.position : magnifyingGlassObjectWorldPosition);
		WgoData wgoData2;
		if (TeleportPointGraph.Instance != null && TeleportPointGraph.Instance.TryResolveDoor(vector, magnifyingGlassObjectWorldPosition, wgoData, out wgoData2))
		{
			this.magnifyingGlassDoorOverride[uniqueId] = wgoData2;
			return;
		}
		this.magnifyingGlassDoorOverride.Remove(uniqueId);
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0010D710 File Offset: 0x0010B910
	private Vector3 GetMagnifyingGlassTrackedWorldPosition(SGuid uniqueId, WgoData wgoData)
	{
		WgoData wgoData2;
		if (this.magnifyingGlassDoorOverride.TryGetValue(uniqueId, out wgoData2) && wgoData2 != null)
		{
			return HUD.GetMagnifyingGlassObjectWorldPosition(wgoData2.UniqueId, wgoData2);
		}
		return HUD.GetMagnifyingGlassObjectWorldPosition(uniqueId, wgoData);
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x0010D744 File Offset: 0x0010B944
	private static Vector3 GetMagnifyingGlassObjectWorldPosition(SGuid uniqueId, WgoData wgoData)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(uniqueId);
		if (wgoViewGlobal != null)
		{
			return wgoViewGlobal.BubbleDrawablePosition;
		}
		return wgoData.BubblePos;
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x0010D770 File Offset: 0x0010B970
	private void ReleaseMagnifyingGlassWidget(SGuid uniqueId)
	{
		UIMagnifyingGlassWidget uimagnifyingGlassWidget;
		if (!this.drawingMagnifyingGlasses.TryGetValue(uniqueId, out uimagnifyingGlassWidget))
		{
			return;
		}
		this.magnifyingGlassWidgetPool.ReleaseObject<UIMagnifyingGlassWidget>(uimagnifyingGlassWidget);
		this.drawingMagnifyingGlasses.Remove(uniqueId);
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x0010D7A8 File Offset: 0x0010B9A8
	private Vector2 GetRectEdgeIntersection(Vector2 from, Vector2 to, Rect rect)
	{
		Vector2 vector = to - from;
		float num = 0f;
		float num2 = 1f;
		if (Mathf.Abs(vector.x) > 0.0001f)
		{
			float num3 = (rect.xMin - from.x) / vector.x;
			float num4 = (rect.xMax - from.x) / vector.x;
			if (vector.x < 0f)
			{
				float num5 = num4;
				float num6 = num3;
				num3 = num5;
				num4 = num6;
			}
			num = Mathf.Max(num, num3);
			num2 = Mathf.Min(num2, num4);
		}
		if (Mathf.Abs(vector.y) > 0.0001f)
		{
			float num7 = (rect.yMin - from.y) / vector.y;
			float num8 = (rect.yMax - from.y) / vector.y;
			if (vector.y < 0f)
			{
				float num9 = num8;
				float num6 = num7;
				num7 = num9;
				num8 = num6;
			}
			num = Mathf.Max(num, num7);
			num2 = Mathf.Min(num2, num8);
		}
		float num10 = Mathf.Clamp(num2, 0f, 1f);
		return from + vector * num10;
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x0010D8C0 File Offset: 0x0010BAC0
	private static Vector2 GetPointerDirectionFromEdgePosition(Vector2 widgetPosition, Rect clampRect)
	{
		float num = Mathf.Abs(widgetPosition.x - clampRect.xMin);
		float num2 = Mathf.Abs(widgetPosition.x - clampRect.xMax);
		float num3 = Mathf.Abs(widgetPosition.y - clampRect.yMin);
		float num4 = Mathf.Abs(widgetPosition.y - clampRect.yMax);
		float num5 = Mathf.Min(num, num2);
		float num7;
		if (Mathf.Min(num3, num4) <= num5)
		{
			float num6 = ((clampRect.width > 0.0001f) ? Mathf.InverseLerp(clampRect.xMin, clampRect.xMax, widgetPosition.x) : 0.5f);
			num7 = ((num4 <= num3) ? Mathf.Lerp(-30f, 30f, num6) : (180f + Mathf.Lerp(30f, -30f, num6)));
		}
		else
		{
			float num8 = ((clampRect.height > 0.0001f) ? Mathf.InverseLerp(clampRect.yMin, clampRect.yMax, widgetPosition.y) : 0.5f);
			num7 = ((num <= num2) ? (270f + Mathf.Lerp(-30f, 30f, num8)) : (90f + Mathf.Lerp(30f, -30f, num8)));
		}
		float num9 = (90f - num7) * 0.017453292f;
		return new Vector2(Mathf.Cos(num9), Mathf.Sin(num9));
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0010DA1C File Offset: 0x0010BC1C
	private void OnDestroy()
	{
		GUIElements.OnWindowSizeTypeChanged -= this.OnWindowSizeTypeChanged;
		this.UnsubscribePlayerControlStateChanged();
		this.TryUnsubscribeMagnifyingGlassTracking();
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x0010DA3C File Offset: 0x0010BC3C
	private void UpdateGamepadDependentStuff()
	{
		if (LazyInput.IsGamepadActive)
		{
			this.hotBarWidget.transform.SetParent(this.hotBarGamepadPos);
			this.hotBarGamepadPos.gameObject.SetActive(true);
			this.hotBarKeyboardPos.gameObject.SetActive(false);
			this.fightingControlsWidget.transform.SetParent(this.controlsWidgetGamepad);
			this.fightingControlsWidget.DrawGamepadTips();
		}
		else
		{
			this.hotBarWidget.transform.SetParent(this.hotBarKeyboardPos);
			this.hotBarGamepadPos.gameObject.SetActive(false);
			this.hotBarKeyboardPos.gameObject.SetActive(true);
			this.fightingControlsWidget.transform.SetParent(this.controlsWidgetMouse);
		}
		LazyPlatformDependentElement[] componentsInChildren = base.GetComponentsInChildren<LazyPlatformDependentElement>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Init();
		}
		LazyGamepadDependentElement[] componentsInChildren2 = base.GetComponentsInChildren<LazyGamepadDependentElement>(true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].UpdateState();
		}
		this.UpdateTutorialListButton();
		((RectTransform)this.hotBarWidget.transform).anchoredPosition = Vector3.zero;
		((RectTransform)this.fightingControlsWidget.transform).anchoredPosition = Vector3.zero;
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0010DB7A File Offset: 0x0010BD7A
	private void UpdateHotBarEnabledState(CharacterWindowData.CharPage page)
	{
		this.UpdateHotBarEnabledState();
	}

	// Token: 0x060037DA RID: 14298 RVA: 0x0010DB7A File Offset: 0x0010BD7A
	private void UpdateHotBarEnabledState(LazyWidgetBase widgetBase)
	{
		this.UpdateHotBarEnabledState();
	}

	// Token: 0x060037DB RID: 14299 RVA: 0x0010DB82 File Offset: 0x0010BD82
	private void UpdateHotBarEnabledState()
	{
		this.hotBarWidget.gameObject.SetActive(LazyWindowsStackController.ActiveWindow == null);
	}

	// Token: 0x060037DC RID: 14300 RVA: 0x0010DB9F File Offset: 0x0010BD9F
	private void OpenTutorialList()
	{
		LazyUI.GetWindow<UITutorialListWindow>().Open(new UITutorialListWindowData(UITutorialListOpenSource.HUD));
	}

	// Token: 0x060037DD RID: 14301 RVA: 0x0010DBB1 File Offset: 0x0010BDB1
	private void HandleTutorialViewed(string _)
	{
		this.UpdateTutorialListButton();
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0010DBB1 File Offset: 0x0010BDB1
	private void HandlePlayerControlStateChanged()
	{
		this.UpdateTutorialListButton();
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x0010DBB9 File Offset: 0x0010BDB9
	private void UnsubscribePlayerControlStateChanged()
	{
		if (this.subscribedPlayerController == null)
		{
			return;
		}
		this.subscribedPlayerController.OnControlStateChanged -= this.HandlePlayerControlStateChanged;
		this.subscribedPlayerController = null;
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0010DBE8 File Offset: 0x0010BDE8
	private void UpdateTutorialListButton()
	{
		if (!(this.tutorialListBtn == null))
		{
			MainGame instance = MainGame.Instance;
			bool flag;
			if (instance == null)
			{
				flag = null != null;
			}
			else
			{
				GameSave gameSave = instance.GameSave;
				flag = ((gameSave != null) ? gameSave.knowledgeSystem : null) != null;
			}
			if (flag)
			{
				PlayerController playerController = MainGame.PlayerController;
				this.tutorialListBtn.gameObject.SetActive(MainGame.Instance.GameSave.knowledgeSystem.HasViewedTutorials() && !LazyInput.IsGamepadActive && playerController != null && playerController.IsControlsEnabled);
				return;
			}
		}
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x0010DC68 File Offset: 0x0010BE68
	public void UpdateHappinessInstant(float value)
	{
		if (MainGame.Instance.GameSave.townSystem.Quality > 0)
		{
			this.happinessLabel.text = string.Format("{0}{1}/{2}", "happiness".FontIcon(), value, MainGame.Instance.GameSave.townSystem.Quality);
			return;
		}
		this.happinessLabel.text = string.Format("{0}{1}", "happiness".FontIcon(), value);
	}

	// Token: 0x060037E2 RID: 14306 RVA: 0x0010DCF0 File Offset: 0x0010BEF0
	private void UpdateHappinessInstant()
	{
		this.UpdateHappinessInstant(MainGame.PlayerData.GetRes("happiness", 0f));
	}

	// Token: 0x060037E3 RID: 14307 RVA: 0x0010DD0C File Offset: 0x0010BF0C
	public void UpdateTechPointsInstant()
	{
		this.UpdateRedTechInstant(MainGame.PlayerData.GetRes("tech_red", 0f));
		this.UpdateGreenTechInstant(MainGame.PlayerData.GetRes("tech_green", 0f));
		this.UpdateBlueTechInstant(MainGame.PlayerData.GetRes("tech_blue", 0f));
		this.UpdateHappinessInstant(MainGame.PlayerData.GetRes("happiness", 0f));
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x0010DD84 File Offset: 0x0010BF84
	public bool TryTurnOnTechPointsPanel()
	{
		if (this.isTechPointsPanelVisible)
		{
			this.hideTechPointsPanelTimer = this.techPointsPanelShowTime;
			return this.isTechPointsPanelOnRightPlace;
		}
		this.isTechPointsPanelVisible = true;
		this.hideTechPointsPanelTimer = this.techPointsPanelShowTime;
		Tween tween = this.currentTechPointsPanelTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.UpdateTechPointsPanelPivot();
		this.techPointsPanel.anchoredPosition = this.GetTechPointsLeftAnchor().anchoredPosition;
		this.SetTechPointsPanelActive(true);
		this.currentTechPointsPanelTween = this.techPointsPanel.DOAnchorPos(this.GetTechPointsRightAnchor().anchoredPosition, this.techPointsPanelMoveTime, false).SetEase(Ease.OutCubic).OnComplete(delegate
		{
			this.isTechPointsPanelOnRightPlace = true;
			Action action = this.onTechPointsPanelShown;
			if (action == null)
			{
				return;
			}
			action();
		});
		return false;
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x0010DE30 File Offset: 0x0010C030
	private void TurnOffTechPointsPanel()
	{
		this.isTechPointsPanelOnRightPlace = false;
		this.isTechPointsPanelVisible = false;
		Tween tween = this.currentTechPointsPanelTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.UpdateTechPointsPanelPivot();
		this.currentTechPointsPanelTween = this.techPointsPanel.DOAnchorPos(this.GetTechPointsLeftAnchor().anchoredPosition, this.techPointsPanelMoveTime, false).SetEase(Ease.InCubic).OnComplete(delegate
		{
			if (!this.isTechPointsPanelVisible)
			{
				this.SetTechPointsPanelActive(false);
			}
		});
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x0010DE9D File Offset: 0x0010C09D
	public void RefreshTechPointsPanelForResolution()
	{
		this.SnapTechPointsPanelToCurrentState();
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x0010DE9D File Offset: 0x0010C09D
	private void OnWindowSizeTypeChanged(UIWindowSizeType _)
	{
		this.SnapTechPointsPanelToCurrentState();
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x0010DEA8 File Offset: 0x0010C0A8
	private void SnapTechPointsPanelToCurrentState()
	{
		if (this.techPointsPanel == null)
		{
			return;
		}
		Tween tween = this.currentTechPointsPanelTween;
		if (tween != null)
		{
			tween.Kill(false);
		}
		this.currentTechPointsPanelTween = null;
		this.UpdateTechPointsPanelPivot();
		RectTransform rectTransform = (this.isTechPointsPanelVisible ? this.GetTechPointsRightAnchor() : this.GetTechPointsLeftAnchor());
		this.techPointsPanel.anchoredPosition = rectTransform.anchoredPosition;
		this.isTechPointsPanelOnRightPlace = this.isTechPointsPanelVisible;
		this.SetTechPointsPanelActive(this.isTechPointsPanelVisible);
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x0010DF23 File Offset: 0x0010C123
	private void SetTechPointsPanelActive(bool active)
	{
		this.techPointsPanel.gameObject.SetActive(active);
		if (active)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.techPointsPanel);
		}
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x0010DF44 File Offset: 0x0010C144
	private void UpdateTechPointsPanelPivot()
	{
		this.techPointsPanel.pivot = ((GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Small) ? HUD.techPointsPanelSmallPivot : HUD.techPointsPanelBigPivot);
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x0010DF6A File Offset: 0x0010C16A
	private RectTransform GetTechPointsLeftAnchor()
	{
		if (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Small && this.techPointsSmallLeftAnchor != null)
		{
			return this.techPointsSmallLeftAnchor;
		}
		return this.techPointsLeftAnchor;
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x0010DF94 File Offset: 0x0010C194
	private RectTransform GetTechPointsRightAnchor()
	{
		if (GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Small && this.techPointsSmallRightAnchor != null)
		{
			return this.techPointsSmallRightAnchor;
		}
		return this.techPointsRightAnchor;
	}

	// Token: 0x060037ED RID: 14317 RVA: 0x0010DFBE File Offset: 0x0010C1BE
	private void UpdateRedTechInstant(float value)
	{
		this.redSpheresLabel.text = string.Format("{0}{1}", "tech_red".FontIcon(), value);
	}

	// Token: 0x060037EE RID: 14318 RVA: 0x0010DFE5 File Offset: 0x0010C1E5
	private void UpdateGreenTechInstant(float value)
	{
		this.greenSpheresLabel.text = string.Format("{0}{1}", "tech_green".FontIcon(), value);
	}

	// Token: 0x060037EF RID: 14319 RVA: 0x0010E00C File Offset: 0x0010C20C
	private void UpdateBlueTechInstant(float value)
	{
		this.blueSpheresLabel.text = string.Format("{0}{1}", "tech_blue".FontIcon(), value);
	}

	// Token: 0x060037F0 RID: 14320 RVA: 0x0010E034 File Offset: 0x0010C234
	public TextMeshProUGUI GetHudLabel(string type)
	{
		if (type == "tech_red")
		{
			return this.redSpheresLabel;
		}
		if (type == "tech_green")
		{
			return this.greenSpheresLabel;
		}
		if (type == "tech_blue")
		{
			return this.blueSpheresLabel;
		}
		if (type == "happiness")
		{
			return this.happinessLabel;
		}
		return null;
	}

	// Token: 0x060037F1 RID: 14321 RVA: 0x0010E092 File Offset: 0x0010C292
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new HUDData(MainGame.Instance.GameSave));
	}

	// Token: 0x04002C5C RID: 11356
	[SerializeField]
	private GameObject leftUpGroup;

	// Token: 0x04002C5D RID: 11357
	[SerializeField]
	private GameObject rightUpGroup;

	// Token: 0x04002C5E RID: 11358
	[SerializeField]
	private UIEnergySanityBar energySanityBar;

	// Token: 0x04002C5F RID: 11359
	[SerializeField]
	private UIHUDWheel wheel;

	// Token: 0x04002C60 RID: 11360
	[SerializeField]
	private UIGameResNotificator gameResNotificatior;

	// Token: 0x04002C61 RID: 11361
	[SerializeField]
	private UIBuffsDisplay buffsDisplay;

	// Token: 0x04002C62 RID: 11362
	[SerializeField]
	private UIHotBarWidget hotBarWidget;

	// Token: 0x04002C63 RID: 11363
	[SerializeField]
	private LazyButton tutorialListBtn;

	// Token: 0x04002C64 RID: 11364
	[SerializeField]
	private Transform hotBarKeyboardPos;

	// Token: 0x04002C65 RID: 11365
	[SerializeField]
	private Transform hotBarGamepadPos;

	// Token: 0x04002C66 RID: 11366
	[SerializeField]
	private UIFightingTimelineRendererWidget fightingTimelineWidget;

	// Token: 0x04002C67 RID: 11367
	[SerializeField]
	private UIFightingSquadHudGroupWidget uiFightingSquadHudGroupWidget;

	// Token: 0x04002C68 RID: 11368
	[SerializeField]
	private UIFightingControlsWidget fightingControlsWidget;

	// Token: 0x04002C69 RID: 11369
	[SerializeField]
	private Transform controlsWidgetMouse;

	// Token: 0x04002C6A RID: 11370
	[SerializeField]
	private Transform controlsWidgetGamepad;

	// Token: 0x04002C6B RID: 11371
	[SerializeField]
	private TextMeshProUGUI happinessLabel;

	// Token: 0x04002C6C RID: 11372
	[SerializeField]
	[Space]
	[Header("Tech Points")]
	private TextMeshProUGUI redSpheresLabel;

	// Token: 0x04002C6D RID: 11373
	[SerializeField]
	private TextMeshProUGUI greenSpheresLabel;

	// Token: 0x04002C6E RID: 11374
	[SerializeField]
	private TextMeshProUGUI blueSpheresLabel;

	// Token: 0x04002C6F RID: 11375
	[SerializeField]
	private RectTransform techPointsPanel;

	// Token: 0x04002C70 RID: 11376
	[SerializeField]
	private RectTransform techPointsLeftAnchor;

	// Token: 0x04002C71 RID: 11377
	[SerializeField]
	private RectTransform techPointsRightAnchor;

	// Token: 0x04002C72 RID: 11378
	[SerializeField]
	private RectTransform techPointsSmallLeftAnchor;

	// Token: 0x04002C73 RID: 11379
	[SerializeField]
	private RectTransform techPointsSmallRightAnchor;

	// Token: 0x04002C74 RID: 11380
	[SerializeField]
	private float techPointsPanelMoveTime = 0.25f;

	// Token: 0x04002C75 RID: 11381
	[SerializeField]
	private float techPointsPanelShowTime = 5f;

	// Token: 0x04002C76 RID: 11382
	[SerializeField]
	private UIMagnifyingGlassWidget magnifyingGlassWidgetPrefab;

	// Token: 0x04002C77 RID: 11383
	[SerializeField]
	private float clampWidgetOffsetFromScreenBorder = 20f;

	// Token: 0x04002C78 RID: 11384
	private Tween currentTechPointsPanelTween;

	// Token: 0x04002C79 RID: 11385
	private bool isTechPointsPanelVisible;

	// Token: 0x04002C7A RID: 11386
	private bool isTechPointsPanelOnRightPlace;

	// Token: 0x04002C7B RID: 11387
	private float hideTechPointsPanelTimer;

	// Token: 0x04002C7C RID: 11388
	private static readonly Vector2 techPointsPanelBigPivot = new Vector2(0f, 0.5f);

	// Token: 0x04002C7D RID: 11389
	private static readonly Vector2 techPointsPanelSmallPivot = new Vector2(0.5f, 0.5f);

	// Token: 0x04002C7E RID: 11390
	private Canvas canvas;

	// Token: 0x04002C7F RID: 11391
	private bool subscribedGameEvents;

	// Token: 0x04002C80 RID: 11392
	private bool subscribedMagnifyingGlassTracking;

	// Token: 0x04002C81 RID: 11393
	private PlayerController subscribedPlayerController;

	// Token: 0x04002C82 RID: 11394
	private MultiFlagAND<HudStateType> disableStateType;

	// Token: 0x04002C83 RID: 11395
	private Pool magnifyingGlassWidgetPool;

	// Token: 0x04002C84 RID: 11396
	private readonly Dictionary<SGuid, WgoData> trackedWorkEventTargets = new Dictionary<SGuid, WgoData>();

	// Token: 0x04002C85 RID: 11397
	private readonly Dictionary<SGuid, UIMagnifyingGlassWidgetData> drawingMagnifyingGlassData = new Dictionary<SGuid, UIMagnifyingGlassWidgetData>();

	// Token: 0x04002C86 RID: 11398
	private readonly Dictionary<SGuid, UIMagnifyingGlassWidget> drawingMagnifyingGlasses = new Dictionary<SGuid, UIMagnifyingGlassWidget>();

	// Token: 0x04002C87 RID: 11399
	private readonly Dictionary<SGuid, WgoData> magnifyingGlassDoorOverride = new Dictionary<SGuid, WgoData>();

	// Token: 0x04002C88 RID: 11400
	private readonly List<SGuid> magnifyingGlassUntrackBuffer = new List<SGuid>();

	// Token: 0x04002C8A RID: 11402
	private HUDMode mode;
}
