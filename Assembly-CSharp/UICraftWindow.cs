using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x0200099A RID: 2458
public class UICraftWindow : UIBaseCraftWindow, IUIWindowCustomOperable
{
	// Token: 0x170009FE RID: 2558
	// (get) Token: 0x0600417E RID: 16766 RVA: 0x00137C17 File Offset: 0x00135E17
	public UIBaseCraftWindowData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x0600417F RID: 16767 RVA: 0x00137C20 File Offset: 0x00135E20
	public override void Init()
	{
		base.Init();
		this.craftListContentTransform = this.craftListContent.GetComponent<RectTransform>();
		this.craftScrollRectTransform = ((this.craftScrollRect != null) ? ((RectTransform)this.craftScrollRect.transform) : null);
		SmoothMouseWheelScroll.EnsureForItem(this.craftScrollRect, 42f);
		if (this.queueAutoScroll != null)
		{
			SmoothMouseWheelScroll.EnsureForItem(this.queueAutoScroll.GetComponent<ScrollRect>(), 50f);
		}
		this.StoreBackMaskFullLayout();
	}

	// Token: 0x06004180 RID: 16768 RVA: 0x00137CA6 File Offset: 0x00135EA6
	public override void Open(UIBaseCraftWindowData data)
	{
		base.Open(data);
		this.isQueueSelectedOnGamepad = false;
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x00137CCC File Offset: 0x00135ECC
	public override void Redraw()
	{
		this.onCraftAddedToQueuePressed = this.data.OnAddToQueuePressed;
		this.onCraftStartPressed = this.data.OnStartCraftPressed;
		this.data.onCraftAddedToQueue = new CraftComponent.DelCraftAddedToQueue(this.OnAddToQeueButtonPressed);
		this.data.onCraftRemovedFromQueue = new CraftComponent.DelCraftRemovedFromQueue(this.OnRemovedFromQueue);
		this.data.SubscribeEvents();
		foreach (KeyValuePair<string, List<CraftDef>> keyValuePair in this.data.CraftsByTabs)
		{
			if (keyValuePair.Value.Count != 0)
			{
				this.DisplayTabWidget(keyValuePair.Key, keyValuePair.Value);
			}
		}
		foreach (KeyValuePair<string, List<CraftDef>> keyValuePair2 in this.data.ExtensionCrafts)
		{
			this.DisplayTabWidget(keyValuePair2.Key, keyValuePair2.Value);
		}
		this.isQueueUiEnabled = this.data == null || !this.data.IsAddToQueueDisabledForAllCrafts;
		if (this.isQueueUiEnabled)
		{
			this.data.CraftComponent.UpdateQueueElementsCraftStatus();
			this.ShowQueueElements();
			this.UpdateQueueVisualState();
		}
		else
		{
			this.HideQueueElements();
		}
		this.uiInfoWidget.Draw(this.data.InfoWidgetData);
		((RectTransform)base.transform).RefreshContentFitter();
		base.Redraw();
		this.ApplyQueueLayout();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004182 RID: 16770 RVA: 0x00137E84 File Offset: 0x00136084
	public override void Hide()
	{
		if (this.data != null)
		{
			this.data.UnsubscribeEvents();
		}
		foreach (UICraftsTabWidget uicraftsTabWidget in this.displayedTabs)
		{
			uicraftsTabWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftsTabWidget>(uicraftsTabWidget);
		}
		this.displayedTabs.Clear();
		foreach (UICraftsTabSeparatorWidget uicraftsTabSeparatorWidget in this.displayedTabSeparators)
		{
			uicraftsTabSeparatorWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftsTabSeparatorWidget>(uicraftsTabSeparatorWidget);
		}
		this.displayedTabSeparators.Clear();
		this.HideQueueElements();
		this.uiInfoWidget.Hide();
		this.RestoreLayoutIfCompact();
		this.RestoreBackMaskFullLayout();
		this.SetQueueObjectsActive(true);
		this.isQueueUiEnabled = true;
		base.Hide();
	}

	// Token: 0x06004183 RID: 16771 RVA: 0x00137F88 File Offset: 0x00136188
	private void RemoveQueueWidget(UICraftQueueElementWidget uiCraftQueueElementWidget)
	{
		this.craftQueueElements.Remove(uiCraftQueueElementWidget);
		uiCraftQueueElementWidget.DeInit();
		uiCraftQueueElementWidget.Hide();
		UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftQueueElementWidget>(uiCraftQueueElementWidget);
		this.UpdateQueueVisualState();
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x00137FB8 File Offset: 0x001361B8
	public UICraftPreviewItemCell GetCraftWidget(Transform parent, UICraftPreviewItemCellData data)
	{
		UICraftPreviewItemCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftPreviewItemCell>(this.otstoynik);
		elementFromPool.Draw(data);
		this.craftPreviewItemCells.Add(elementFromPool);
		if (data != null)
		{
			if (data.IsTab)
			{
				parent.transform.SetAsFirstSibling();
			}
			else if (data.IsUnknown)
			{
				parent.transform.SetAsLastSibling();
			}
			if (data.IsTab)
			{
				elementFromPool.ItemCellGamepadNavigationItem.Active = false;
			}
			else
			{
				elementFromPool.ItemCellGamepadNavigationItem.Active = true;
			}
		}
		else
		{
			elementFromPool.ItemCellGamepadNavigationItem.Active = false;
		}
		return elementFromPool;
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x00138045 File Offset: 0x00136245
	public void ReleaseCraftWidget(UICraftPreviewItemCell cellWidget)
	{
		cellWidget.Hide();
		UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftPreviewItemCell>(cellWidget);
		this.craftPreviewItemCells.Remove(cellWidget);
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x00138068 File Offset: 0x00136268
	private void OnCraftStartPressed(Action<CraftElement> action, CraftDef craftDefinition, List<NeedItemData> selectedNeedItems, CraftParamsData craftParams, int craftsCount = 1)
	{
		CraftElement craftElement = (craftDefinition.isConveyorCraft ? new ConveyorCraftElement(craftDefinition.id, craftsCount, selectedNeedItems, craftParams) : new CraftElement(craftDefinition.id, craftsCount, selectedNeedItems, craftParams));
		if (craftDefinition.isFuelCraft)
		{
			craftElement.DoBeforeStartCalculations(this.data.AssignedWgo.Data);
			ItemDef data = GameBalance.Me.GetData<ItemDef>(craftElement.Definition.addItemsToWgoOnFinish.chanceOutputItems[0].id);
			int num = Mathf.FloorToInt((float)this.data.AssignedWgo.Data.Inventory.Data.CanAddItemCountToInventory(data, 99999, true, null, false) / (float)craftElement.PreToWgoOnFinishItems[0].count);
			craftElement.Count = ((craftsCount > num) ? num : craftsCount);
		}
		if (action != null)
		{
			action(craftElement);
		}
		this.UpdateCraftsLocks();
	}

	// Token: 0x06004187 RID: 16775 RVA: 0x0013814C File Offset: 0x0013634C
	private void AddQueueItem(CraftElementBase queueElement)
	{
		UICraftQueueElementWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftQueueElementWidget>(this.queueListContent.transform);
		this.craftQueueElements.Add(elementFromPool);
		UICraftQueueElementWidgetData uicraftQueueElementWidgetData = new UICraftQueueElementWidgetData(this.data.AssignedWgo.Data, queueElement, this.data.CraftComponent.CraftElementsQueue, null, null, null, null, new Action<CraftElementBase>(this.TryUpToQueue), new Action<CraftElementBase>(this.TryDownToQueue), this.data.OnQueueElementRemovePressed);
		elementFromPool.Init();
		elementFromPool.Draw(uicraftQueueElementWidgetData);
	}

	// Token: 0x06004188 RID: 16776 RVA: 0x001381D8 File Offset: 0x001363D8
	private bool IsCraftItemCellFocusedByGamepad(out UICraftItemCell craftItemCell)
	{
		craftItemCell = null;
		if (!LazyInput.IsGamepadActive)
		{
			return false;
		}
		GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
		return focusedItem != null && focusedItem.TryGetComponent<UICraftItemCell>(out craftItemCell);
	}

	// Token: 0x06004189 RID: 16777 RVA: 0x00138214 File Offset: 0x00136414
	private void HideQueueElements()
	{
		foreach (UICraftQueueElementWidget uicraftQueueElementWidget in this.craftQueueElements)
		{
			uicraftQueueElementWidget.DeInit();
			uicraftQueueElementWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftQueueElementWidget>(uicraftQueueElementWidget);
		}
		this.craftQueueElements.Clear();
	}

	// Token: 0x0600418A RID: 16778 RVA: 0x00138284 File Offset: 0x00136484
	private void ShowQueueElements()
	{
		if (!this.isQueueUiEnabled || !this.UpdateQueueVisualState())
		{
			return;
		}
		foreach (CraftElementBase craftElementBase in this.data.CraftComponent.CraftElementsQueue)
		{
			this.AddQueueItem(craftElementBase);
		}
	}

	// Token: 0x0600418B RID: 16779 RVA: 0x001382F4 File Offset: 0x001364F4
	private void RedrawQueueElements()
	{
		this.HideQueueElements();
		this.ShowQueueElements();
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x00138302 File Offset: 0x00136502
	private void TryUpToQueue(CraftElementBase queueElement)
	{
		if (this.data.CraftComponent.TryElementUpToQueue(queueElement))
		{
			this.RedrawQueueElements();
		}
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x0013831D File Offset: 0x0013651D
	private void TryDownToQueue(CraftElementBase queueElement)
	{
		if (this.data.CraftComponent.TryElementDownToQueue(queueElement))
		{
			this.RedrawQueueElements();
		}
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x00138338 File Offset: 0x00136538
	private void OnAddToQeueButtonPressed(CraftElementBase queueElement)
	{
		if (!this.isQueueUiEnabled)
		{
			return;
		}
		this.AddQueueItem(queueElement);
		this.RedrawQueueElements();
		this.PrintTips();
		this.craftListContentTransform.RefreshContentFitter();
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x00138364 File Offset: 0x00136564
	private void OnRemovedFromQueue(CraftElementBase queueElement)
	{
		foreach (UICraftQueueElementWidget uicraftQueueElementWidget in this.craftQueueElements)
		{
			if (uicraftQueueElementWidget.Data.CraftQueueElement == queueElement)
			{
				bool flag = LazyInput.IsGamepadActive && uicraftQueueElementWidget.OutputItem.GamepadNavigationItem.IsFocused;
				this.RemoveQueueWidget(uicraftQueueElementWidget);
				if (flag)
				{
					if (this.craftQueueElements.Count > 0)
					{
						this.isQueueSelectedOnGamepad = true;
						base.GamepadNavigationController.FocusOnFirstActive(1);
					}
					else
					{
						this.isQueueSelectedOnGamepad = false;
						base.GamepadNavigationController.FocusOnFirstActive(0);
					}
				}
				this.PrintTips();
				break;
			}
		}
	}

	// Token: 0x06004190 RID: 16784 RVA: 0x00138424 File Offset: 0x00136624
	private bool HasQueueElements()
	{
		UIBaseCraftWindowData data = this.data;
		bool flag;
		if (data == null)
		{
			flag = null != null;
		}
		else
		{
			CraftComponent craftComponent = data.CraftComponent;
			flag = ((craftComponent != null) ? craftComponent.CraftElementsQueue : null) != null;
		}
		return flag && this.data.CraftComponent.CraftElementsQueue.Count > 0;
	}

	// Token: 0x06004191 RID: 16785 RVA: 0x00138460 File Offset: 0x00136660
	private bool UpdateQueueVisualState()
	{
		if (!this.isQueueUiEnabled)
		{
			return false;
		}
		if (!this.HasQueueElements())
		{
			this.noQueueElementsGo.SetActive(true);
			return false;
		}
		this.noQueueElementsGo.SetActive(false);
		return true;
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x00138490 File Offset: 0x00136690
	private void ApplyQueueLayout()
	{
		bool flag = this.data != null && this.data.IsAddToQueueDisabledForAllCrafts;
		this.isQueueUiEnabled = this.data == null || !flag;
		this.RestoreLayoutIfCompact();
		this.ApplyFullContentHeight();
		this.SetQueueObjectsActive(this.isQueueUiEnabled);
		if (!this.isQueueUiEnabled)
		{
			this.isQueueSelectedOnGamepad = false;
			this.ApplyCompactLayout();
			this.SetBackMaskBottomPadding(13f);
			return;
		}
		this.RestoreBackMaskFullLayout();
	}

	// Token: 0x06004193 RID: 16787 RVA: 0x00138508 File Offset: 0x00136708
	private void ApplyFullContentHeight()
	{
		float num = ((GUIElements.Instance.UIWindowSizeType == UIWindowSizeType.Big) ? this.contentHeightBig : this.contentHeightSmall);
		this.contentRectTransform.sizeDelta = new Vector2(this.contentRectTransform.sizeDelta.x, num);
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x00138554 File Offset: 0x00136754
	private void CaptureFullLayoutSnapshot()
	{
		this.fullContentSizeDelta = this.contentRectTransform.sizeDelta;
		if (this.craftScrollRectTransform != null)
		{
			this.fullCraftScrollSizeDelta = this.craftScrollRectTransform.sizeDelta;
			this.fullCraftScrollAnchoredPosition = this.craftScrollRectTransform.anchoredPosition;
		}
		if (this.fadeDownRect != null)
		{
			this.fullFadeDownAnchoredPosition = this.fadeDownRect.anchoredPosition;
		}
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x001385C4 File Offset: 0x001367C4
	private void RestoreLayoutIfCompact()
	{
		if (!this.isCompactLayoutApplied)
		{
			return;
		}
		this.contentRectTransform.sizeDelta = this.fullContentSizeDelta;
		if (this.craftScrollRectTransform != null)
		{
			this.craftScrollRectTransform.sizeDelta = this.fullCraftScrollSizeDelta;
			this.craftScrollRectTransform.anchoredPosition = this.fullCraftScrollAnchoredPosition;
		}
		if (this.fadeDownRect != null)
		{
			this.fadeDownRect.anchoredPosition = this.fullFadeDownAnchoredPosition;
		}
		this.isCompactLayoutApplied = false;
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x00138641 File Offset: 0x00136841
	private void SetQueueObjectsActive(bool active)
	{
		if (this.contentQueue != null)
		{
			this.contentQueue.SetActive(active);
		}
		if (this.frameQueue != null)
		{
			this.frameQueue.SetActive(active);
		}
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x00138678 File Offset: 0x00136878
	private void StoreBackMaskFullLayout()
	{
		if (this.hasStoredBackMaskLayout || this.backMaskRect == null)
		{
			return;
		}
		this.fullBackMaskSizeDelta = this.backMaskRect.sizeDelta;
		this.fullBackMaskAnchoredPosition = this.backMaskRect.anchoredPosition;
		this.hasStoredBackMaskLayout = true;
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x001386C5 File Offset: 0x001368C5
	private void RestoreBackMaskFullLayout()
	{
		if (this.backMaskRect == null || !this.hasStoredBackMaskLayout)
		{
			return;
		}
		this.backMaskRect.sizeDelta = this.fullBackMaskSizeDelta;
		this.backMaskRect.anchoredPosition = this.fullBackMaskAnchoredPosition;
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x00138700 File Offset: 0x00136900
	private void SetBackMaskBottomPadding(float padding)
	{
		if (this.backMaskRect == null)
		{
			return;
		}
		this.StoreBackMaskFullLayout();
		Vector2 offsetMin = this.backMaskRect.offsetMin;
		offsetMin.y = padding;
		this.backMaskRect.offsetMin = offsetMin;
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x00138744 File Offset: 0x00136944
	private void ApplyCompactLayout()
	{
		if (this.isCompactLayoutApplied || this.craftScrollRectTransform == null)
		{
			return;
		}
		float y = this.craftScrollRectTransform.offsetMin.y;
		if (y <= 0.01f)
		{
			return;
		}
		this.CaptureFullLayoutSnapshot();
		this.contentRectTransform.sizeDelta = new Vector2(this.contentRectTransform.sizeDelta.x, this.contentRectTransform.sizeDelta.y - y - 50f);
		Vector2 offsetMin = this.craftScrollRectTransform.offsetMin;
		offsetMin.y = 0f;
		this.craftScrollRectTransform.offsetMin = offsetMin;
		if (this.fadeDownRect != null)
		{
			Canvas.ForceUpdateCanvases();
			RectTransform rectTransform = (RectTransform)this.fadeDownRect.parent;
			this.fadeDownRect.anchoredPosition = new Vector2(this.fadeDownRect.anchoredPosition.x, rectTransform.rect.yMin + this.fadeDownRect.rect.height * this.fadeDownRect.pivot.y);
		}
		this.isCompactLayoutApplied = true;
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x00138864 File Offset: 0x00136A64
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.CraftWindowZoneSwitch, new Func<bool>(this.OnSwitchZonePressed));
		gameKeyDelegates.Add(GameKey.DpadDown, new Func<bool>(this.OnDpadDown));
		gameKeyDelegates.Add(GameKey.DpadUp, new Func<bool>(this.OnDpadUp));
		gameKeyDelegates.Add(GameKey.Down, new Func<bool>(this.OnDpadDown));
		gameKeyDelegates.Add(GameKey.Up, new Func<bool>(this.OnDpadUp));
		gameKeyDelegates.Add(GameKey.CraftWindowQueueRight, new Func<bool>(this.OnQueueRight));
		gameKeyDelegates.Add(GameKey.CraftWindowQueueLeft, new Func<bool>(this.OnQueueLeft));
		gameKeyDelegates.Add(GameKey.RightClick, new Func<bool>(this.OnPressedBack));
		return gameKeyDelegates;
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x00138930 File Offset: 0x00136B30
	private bool OnSwitchZonePressed()
	{
		if (!this.isQueueUiEnabled)
		{
			return false;
		}
		if (LazyInput.IsGamepadActive)
		{
			if (this.isQueueSelectedOnGamepad)
			{
				this.isQueueSelectedOnGamepad = false;
				base.GamepadNavigationController.FocusOnFirstActive(0);
			}
			else if (this.craftQueueElements.Count > 0)
			{
				this.isQueueSelectedOnGamepad = true;
				base.GamepadNavigationController.FocusOnFirstActive(1);
			}
		}
		return true;
	}

	// Token: 0x0600419D RID: 16797 RVA: 0x0013898F File Offset: 0x00136B8F
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		this.isQueueSelectedOnGamepad = false;
		base.GamepadNavigationController.FocusOnFirstActive(0);
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x001389AB File Offset: 0x00136BAB
	protected override void Update()
	{
		this.TickQueueCountHold();
		base.Update();
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x001389BC File Offset: 0x00136BBC
	private void TickQueueCountHold()
	{
		if (!base.IsShownAndTop || !this.isQueueUiEnabled || !this.isQueueSelectedOnGamepad || base.GamepadNavigationController.FocusedItem == null)
		{
			this.ResetQueueCountHold();
			return;
		}
		UICraftQueueElementWidget componentInParent = base.GamepadNavigationController.FocusedItem.GetComponentInParent<UICraftQueueElementWidget>();
		if (componentInParent == null)
		{
			this.ResetQueueCountHold();
			return;
		}
		if (this.queueCountHoldTarget != componentInParent)
		{
			this.queueCountHold.Reset();
			this.queueCountHoldTarget = componentInParent;
		}
		if (HoldRepeatValueChanger.GetPointerHoldDirection(componentInParent.PlusCraftButton, componentInParent.MinusCraftButton) != 0)
		{
			this.queueCountHold.Reset();
			return;
		}
		bool flag = (HoldRepeatValueChanger.IsAnyKeyHeld(GameKey.DpadUp, GameKey.Up) || HoldRepeatValueChanger.GetAxisHoldDirection(true) > 0) && componentInParent.PlusCraftButton != null && componentInParent.PlusCraftButton.interactable;
		bool flag2 = (HoldRepeatValueChanger.IsAnyKeyHeld(GameKey.DpadDown, GameKey.Down) || HoldRepeatValueChanger.GetAxisHoldDirection(true) < 0) && componentInParent.MinusCraftButton != null && componentInParent.MinusCraftButton.interactable;
		int num = 0;
		if (flag != flag2)
		{
			num = (flag ? 1 : (-1));
		}
		this.queueCountHold.Tick(num, new Action<int>(componentInParent.ChangeCount));
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x00138AEF File Offset: 0x00136CEF
	private void ResetQueueCountHold()
	{
		this.queueCountHold.Reset();
		this.queueCountHoldTarget = null;
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x00138B04 File Offset: 0x00136D04
	private bool OnDpadDown()
	{
		if (!LazyInput.IsGamepadActive || base.GamepadNavigationController.FocusedItem == null)
		{
			return false;
		}
		if (this.isQueueUiEnabled && this.isQueueSelectedOnGamepad && base.GamepadNavigationController.FocusedItem != null)
		{
			UICraftQueueElementWidget componentInParent = base.GamepadNavigationController.FocusedItem.GetComponentInParent<UICraftQueueElementWidget>();
			return !(componentInParent == null) && !(componentInParent.MinusCraftButton == null) && componentInParent.MinusCraftButton.interactable;
		}
		return this.lazyWindowInputController.OnPressedDownDpad();
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x00138B94 File Offset: 0x00136D94
	private bool OnDpadUp()
	{
		if (!LazyInput.IsGamepadActive || base.GamepadNavigationController.FocusedItem == null)
		{
			return false;
		}
		if (this.isQueueUiEnabled && this.isQueueSelectedOnGamepad && base.GamepadNavigationController.FocusedItem != null)
		{
			UICraftQueueElementWidget componentInParent = base.GamepadNavigationController.FocusedItem.GetComponentInParent<UICraftQueueElementWidget>();
			return !(componentInParent == null) && !(componentInParent.PlusCraftButton == null) && componentInParent.PlusCraftButton.interactable;
		}
		return this.lazyWindowInputController.OnPressedUpDpad();
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x00138C24 File Offset: 0x00136E24
	private bool OnQueueRight()
	{
		return this.TryMoveFocusedQueueItem(true);
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x00138C2D File Offset: 0x00136E2D
	private bool OnQueueLeft()
	{
		return this.TryMoveFocusedQueueItem(false);
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x00138C38 File Offset: 0x00136E38
	private bool TryMoveFocusedQueueItem(bool moveRight)
	{
		if (!this.isQueueUiEnabled || !this.isQueueSelectedOnGamepad || base.GamepadNavigationController.FocusedItem == null)
		{
			return false;
		}
		UICraftQueueElementWidget componentInParent = base.GamepadNavigationController.FocusedItem.GetComponentInParent<UICraftQueueElementWidget>();
		if (componentInParent == null || componentInParent.Data == null || componentInParent.Data.CraftQueueElement == null)
		{
			return false;
		}
		CraftElementBase craftQueueElement = componentInParent.Data.CraftQueueElement;
		if (moveRight)
		{
			if (componentInParent.QueueDownButton == null || !componentInParent.QueueDownButton.interactable)
			{
				return false;
			}
			componentInParent.QueueDownButton.onClick.Invoke();
		}
		else
		{
			if (componentInParent.QueueUpButton == null || !componentInParent.QueueUpButton.interactable)
			{
				return false;
			}
			componentInParent.QueueUpButton.onClick.Invoke();
		}
		this.FocusQueueElement(craftQueueElement);
		return true;
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x00138D10 File Offset: 0x00136F10
	private void FocusQueueElement(CraftElementBase craftElement)
	{
		foreach (UICraftQueueElementWidget uicraftQueueElementWidget in this.craftQueueElements)
		{
			if (!(uicraftQueueElementWidget == null) && uicraftQueueElementWidget.Data != null && uicraftQueueElementWidget.Data.CraftQueueElement == craftElement)
			{
				GamepadNavigationItem componentInChildren = uicraftQueueElementWidget.GetComponentInChildren<GamepadNavigationItem>();
				if (componentInChildren == null)
				{
					break;
				}
				((RectTransform)this.queueListContent.transform).RefreshContentFitter();
				if (this.queueAutoScroll != null)
				{
					this.queueAutoScroll.SkipNextAutoscroll = true;
				}
				base.GamepadNavigationController.SetFocusedItem(componentInChildren);
				if (this.queueAutoScroll != null)
				{
					this.queueAutoScroll.ScrollToItem(componentInChildren);
				}
				uicraftQueueElementWidget.OnOver();
				break;
			}
		}
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x00138DF4 File Offset: 0x00136FF4
	protected override void PrintTips()
	{
		this.PrintTips(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x060041A8 RID: 16808 RVA: 0x00138E08 File Offset: 0x00137008
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (this.isQueueUiEnabled && this.HasQueueElements())
		{
			list.Add(new LazyGameKeyTip(GameKey.CraftWindowZoneSwitch, "tip_switch", true, true, true));
		}
		if (gamepadNavigationItem != null)
		{
			UICraftQueueElementWidget uicraftQueueElementWidget = (this.isQueueUiEnabled ? gamepadNavigationItem.GetComponentInParent<UICraftQueueElementWidget>() : null);
			if (uicraftQueueElementWidget != null)
			{
				list.Add(new LazyGameKeyTip(GameKey.DpadUp, "+", true, true, true));
				list.Add(new LazyGameKeyTip(GameKey.DpadDown, "-", true, true, true));
				if (uicraftQueueElementWidget.QueueUpButton.interactable)
				{
					list.Add(new LazyGameKeyTip(GameKey.CraftWindowQueueLeft, "<", true, true, true));
				}
				if (uicraftQueueElementWidget.QueueDownButton.interactable)
				{
					list.Add(new LazyGameKeyTip(GameKey.CraftWindowQueueRight, ">", true, true, true));
				}
			}
			else
			{
				list.Add(LazyGameKeyTip.Select(true, true, true));
			}
		}
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x060041A9 RID: 16809 RVA: 0x00138F20 File Offset: 0x00137120
	private void UpdateCraftsLocks()
	{
		foreach (UICraftPreviewItemCell uicraftPreviewItemCell in this.craftPreviewItemCells)
		{
			uicraftPreviewItemCell.UpdateLocks();
		}
	}

	// Token: 0x060041AA RID: 16810 RVA: 0x00138F70 File Offset: 0x00137170
	private void DisplayTabWidget(string crafterId, List<CraftDef> crafts)
	{
		if (this.displayedTabs.Count != 0)
		{
			UICraftsTabSeparatorWidgetData uicraftsTabSeparatorWidgetData = new UICraftsTabSeparatorWidgetData();
			UICraftsTabSeparatorWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftsTabSeparatorWidget>(this.craftListContent.transform);
			elementFromPool.Draw(uicraftsTabSeparatorWidgetData);
			this.displayedTabSeparators.Add(elementFromPool);
		}
		UICraftsTabWidgetData uicraftsTabWidgetData = new UICraftsTabWidgetData(this.data.AssignedWgo.Data, crafts, crafterId, delegate(CraftDef craftDef, List<NeedItemData> needsData, CraftParamsData cpd, int count)
		{
			this.OnCraftStartPressed(this.onCraftAddedToQueuePressed, craftDef, needsData, cpd, count);
		}, delegate(CraftDef craftDef, List<NeedItemData> needsData, CraftParamsData cpd, int count)
		{
			this.OnCraftStartPressed(this.onCraftStartPressed, craftDef, needsData, cpd, count);
		}, this.data.IsGravePartRemove);
		UICraftsTabWidget elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<UICraftsTabWidget>(this.craftListContent.transform);
		elementFromPool2.Draw(uicraftsTabWidgetData);
		this.displayedTabs.Add(elementFromPool2);
	}

	// Token: 0x060041AB RID: 16811 RVA: 0x00139020 File Offset: 0x00137220
	[LazyUITest]
	protected override void TestDraw()
	{
		Wgo wgo = Wgo.Spawn(new WgoData("woodworking_workbench_1", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId), MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		CraftInteractionHandler craftInteractionHandler = new CraftInteractionHandler();
		craftInteractionHandler.Init(wgo);
		craftInteractionHandler.HasInteraction(MainGame.PlayerController);
		craftInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x060041AD RID: 16813 RVA: 0x001390D9 File Offset: 0x001372D9
	bool IUIWindowCustomOperable.get_IsShown()
	{
		return base.IsShown;
	}

	// Token: 0x0400332E RID: 13102
	[SerializeField]
	private UIInfoWidget uiInfoWidget;

	// Token: 0x0400332F RID: 13103
	[SerializeField]
	private RectTransform contentRectTransform;

	// Token: 0x04003330 RID: 13104
	[SerializeField]
	private float contentHeightBig;

	// Token: 0x04003331 RID: 13105
	[SerializeField]
	private float contentHeightSmall;

	// Token: 0x04003332 RID: 13106
	[SerializeField]
	[Space]
	private GameObject craftListContent;

	// Token: 0x04003333 RID: 13107
	[SerializeField]
	private GameObject queueListContent;

	// Token: 0x04003334 RID: 13108
	[SerializeField]
	private GameObject noQueueElementsGo;

	// Token: 0x04003335 RID: 13109
	[SerializeField]
	private GameObject contentQueue;

	// Token: 0x04003336 RID: 13110
	[SerializeField]
	private GameObject frameQueue;

	// Token: 0x04003337 RID: 13111
	[SerializeField]
	private LazyScrollRect craftScrollRect;

	// Token: 0x04003338 RID: 13112
	[SerializeField]
	private AutoScroll queueAutoScroll;

	// Token: 0x04003339 RID: 13113
	[SerializeField]
	private Transform otstoynik;

	// Token: 0x0400333A RID: 13114
	private UICraftQueueElementWidget currentCraftQueueElement;

	// Token: 0x0400333B RID: 13115
	private List<UICraftsTabWidget> displayedTabs = new List<UICraftsTabWidget>();

	// Token: 0x0400333C RID: 13116
	private List<UICraftsTabSeparatorWidget> displayedTabSeparators = new List<UICraftsTabSeparatorWidget>();

	// Token: 0x0400333D RID: 13117
	private List<UICraftQueueElementWidget> craftQueueElements = new List<UICraftQueueElementWidget>();

	// Token: 0x0400333E RID: 13118
	private RectTransform craftListContentTransform;

	// Token: 0x0400333F RID: 13119
	private List<UICraftPreviewItemCell> craftPreviewItemCells = new List<UICraftPreviewItemCell>();

	// Token: 0x04003340 RID: 13120
	private Action<CraftElement> onCraftAddedToQueuePressed;

	// Token: 0x04003341 RID: 13121
	private Action<CraftElement> onCraftStartPressed;

	// Token: 0x04003342 RID: 13122
	private bool isQueueSelectedOnGamepad;

	// Token: 0x04003343 RID: 13123
	private bool isQueueUiEnabled = true;

	// Token: 0x04003344 RID: 13124
	private bool isCompactLayoutApplied;

	// Token: 0x04003345 RID: 13125
	private Vector2 fullContentSizeDelta;

	// Token: 0x04003346 RID: 13126
	private Vector2 fullCraftScrollSizeDelta;

	// Token: 0x04003347 RID: 13127
	private Vector2 fullCraftScrollAnchoredPosition;

	// Token: 0x04003348 RID: 13128
	private Vector2 fullFadeDownAnchoredPosition;

	// Token: 0x04003349 RID: 13129
	private Vector2 fullBackMaskSizeDelta;

	// Token: 0x0400334A RID: 13130
	private Vector2 fullBackMaskAnchoredPosition;

	// Token: 0x0400334B RID: 13131
	private bool hasStoredBackMaskLayout;

	// Token: 0x0400334C RID: 13132
	private RectTransform craftScrollRectTransform;

	// Token: 0x0400334D RID: 13133
	[SerializeField]
	private RectTransform backMaskRect;

	// Token: 0x0400334E RID: 13134
	[SerializeField]
	private RectTransform fadeDownRect;

	// Token: 0x0400334F RID: 13135
	private const float BackMaskBottomPaddingCompact = 13f;

	// Token: 0x04003350 RID: 13136
	private const float CompactExtraHeightReduction = 50f;

	// Token: 0x04003351 RID: 13137
	private readonly HoldRepeatValueChanger queueCountHold = new HoldRepeatValueChanger();

	// Token: 0x04003352 RID: 13138
	private UICraftQueueElementWidget queueCountHoldTarget;
}
