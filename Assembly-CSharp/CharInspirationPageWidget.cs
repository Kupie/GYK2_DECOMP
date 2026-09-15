using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x0200090E RID: 2318
public class CharInspirationPageWidget : LazyWidget<CharInspirationPageWidgetData>
{
	// Token: 0x06003CF2 RID: 15602 RVA: 0x00123280 File Offset: 0x00121480
	public override void Init()
	{
		base.Init();
		float num = 96f;
		GridLayoutGroup gridLayoutGroup = ((this.inspirationWidgetsParent != null) ? this.inspirationWidgetsParent.GetComponent<GridLayoutGroup>() : null);
		if (gridLayoutGroup != null)
		{
			num = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
		}
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, num * 0.5f);
		this.talentTabButtonsContainer.Init(delegate(string talendId)
		{
			CharacterWindow window = LazyUI.GetWindow<CharacterWindow>();
			if (window == null)
			{
				return;
			}
			window.SetInspirationPageWithSpecificTalent(talendId);
		}, this.canvas);
		this.inspirationWidgetPrefab.gameObject.SetActive(false);
		this.EnsureFlyingExpOverlay();
		if (this.flyingExpPointPrefab != null)
		{
			this.flyingExpPointPrefab.gameObject.SetActive(false);
			this.flyingExpPool = new Pool(this.flyingExpPointPrefab, this.flyingExpOverlay, 1, Pool.PoolType.ImmediateActivation, false, null);
		}
	}

	// Token: 0x06003CF3 RID: 15603 RVA: 0x0012336C File Offset: 0x0012156C
	protected override void SetData(CharInspirationPageWidgetData data)
	{
		base.SetData(data);
		data.onTalentExpChanged = null;
		data.onInspirationProgressChanged = new Action<string>(this.OnInspirationProgressChanged);
		data.onInspirationCompleted = new Action<string>(this.OnInspirationCompleted);
		data.onTalentLevelPurchased = new TalentSystemData.DelTalentLevelPurchased(this.OnTalentLevelPurchased);
		data.onInspirationPurchased = new Action<string>(this.OnInspirationPurchased);
	}

	// Token: 0x06003CF4 RID: 15604 RVA: 0x001233D0 File Offset: 0x001215D0
	public override void Redraw()
	{
		base.Redraw();
		this.pendingInspirationsRedraw = false;
		this.CancelFlyingExpAnimation();
		this.isShown = true;
		this.RedrawFaith();
		TalentExpProgressWidgetData talentExpProgressWidgetData = new TalentExpProgressWidgetData(this.data.TalentData);
		this.talentExpProgressWidget.Draw(talentExpProgressWidgetData);
		this.DrawInspirations(this.data.TalentData);
		this.RedrawTalentLevelUpsWidget();
		this.UpdateCanBuyTalentLevelUpEffect();
		this.RedrawTabs(true);
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x06003CF5 RID: 15605 RVA: 0x00123444 File Offset: 0x00121644
	public override void Hide()
	{
		if (!this.isShown)
		{
			return;
		}
		this.isShown = false;
		this.pendingInspirationsRedraw = false;
		this.ClearPendingGamepadFocusAfterPurchase();
		this.CancelFlyingExpAnimation();
		this.talentExpProgressWidget.Hide();
		this.HideInspirations();
		this.talentLevelUpsWidget.Hide();
		this.UpdateCanBuyTalentLevelUpEffect();
		if (this.data != null)
		{
			this.data.UnsubscribeEvents();
			this.data.onTalentExpChanged = null;
			this.data.onInspirationProgressChanged = null;
			this.data.onInspirationCompleted = null;
			this.data.onTalentLevelPurchased = null;
			this.data.onInspirationPurchased = null;
		}
		base.Hide();
	}

	// Token: 0x06003CF6 RID: 15606 RVA: 0x001234EB File Offset: 0x001216EB
	private void OnEnable()
	{
		if (this.isShown)
		{
			this.RedrawInspirationsIfPending();
		}
	}

	// Token: 0x06003CF7 RID: 15607 RVA: 0x001234FB File Offset: 0x001216FB
	private void OnDisable()
	{
		this.CancelFlyingExpAnimation();
	}

	// Token: 0x06003CF8 RID: 15608 RVA: 0x00123504 File Offset: 0x00121704
	public void RedrawFaith()
	{
		this.faithCell.Draw(new Item("faith", MainGame.PlayerController.PlayerData.Inventory.Data.GetTotalCountInInventory("faith", null, false)), false, -1, false, 1, false, 0, true, true, true, ItemRelatedWidgetState.NotSet, false);
	}

	// Token: 0x06003CF9 RID: 15609 RVA: 0x00123554 File Offset: 0x00121754
	public void UpdateGamepadDependentStuff()
	{
		bool isGamepadActive = LazyInput.IsGamepadActive;
		if (this.nextSubTabGamepadHelper != null)
		{
			this.nextSubTabGamepadHelper.gameObject.SetActive(isGamepadActive);
			if (isGamepadActive)
			{
				this.nextSubTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.NextSubTab, null, true);
			}
			this.nextSubTabGamepadHelper.transform.SetAsLastSibling();
		}
		if (this.prevSubTabGamepadHelper != null)
		{
			this.prevSubTabGamepadHelper.gameObject.SetActive(isGamepadActive);
			if (isGamepadActive)
			{
				this.prevSubTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.PrevSubTab, null, true);
			}
			this.prevSubTabGamepadHelper.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06003CFA RID: 15610 RVA: 0x001235F9 File Offset: 0x001217F9
	public bool OnPressedPrevTechTab(GamepadNavigationController gamepadNavigationController)
	{
		return this.talentTabButtonsContainer.OnPressedPrevTechTab(gamepadNavigationController, this.autoScroll);
	}

	// Token: 0x06003CFB RID: 15611 RVA: 0x0012360D File Offset: 0x0012180D
	public bool OnPressedNextTechTab(GamepadNavigationController gamepadNavigationController)
	{
		return this.talentTabButtonsContainer.OnPressedNextTechTab(gamepadNavigationController, this.autoScroll);
	}

	// Token: 0x06003CFC RID: 15612 RVA: 0x00123624 File Offset: 0x00121824
	private void DrawInspirations(TalentData talentData)
	{
		this.HideInspirations();
		for (int i = 0; i < talentData.activeInspirations.Count; i++)
		{
			if (!talentData.activeInspirations[i].isAllLevelsBought && !talentData.activeInspirations[i].IsHidden)
			{
				InspirationWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<InspirationWidget>(this.inspirationWidgetsParent);
				elementFromPool.Init();
				InspirationWidgetData inspirationWidgetData = new InspirationWidgetData(talentData.activeInspirations[i], this.talentExpProgressWidget.PointIconId);
				elementFromPool.Draw(inspirationWidgetData);
				this.displayedInspirations.Add(elementFromPool);
			}
		}
		this.displayedInspirations.Sort(delegate(InspirationWidget x, InspirationWidget y)
		{
			bool isCompleted = x.InspirationData.IsCompleted;
			bool isCompleted2 = y.InspirationData.IsCompleted;
			if (isCompleted == isCompleted2)
			{
				int completionPrice = InspirationDef.GetDataForLevel(x.InspirationData.id, x.InspirationData.curLevel).completionPrice;
				int completionPrice2 = InspirationDef.GetDataForLevel(y.InspirationData.id, y.InspirationData.curLevel).completionPrice;
				return completionPrice.CompareTo(completionPrice2);
			}
			if (!isCompleted)
			{
				return 1;
			}
			return -1;
		});
		for (int j = 0; j < talentData.activeInspirations.Count; j++)
		{
			for (int k = 0; k < talentData.activeInspirations[j].PurchasedInspirations.Count; k++)
			{
				InspirationWidgetFinished elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<InspirationWidgetFinished>(this.inspirationWidgetsFinishedParent);
				elementFromPool2.Init();
				InspirationWidgetData inspirationWidgetData2 = new InspirationWidgetData(talentData.activeInspirations[j].PurchasedInspirations[k]);
				elementFromPool2.Draw(inspirationWidgetData2);
				this.displayedInspirationsFinished.Add(elementFromPool2);
			}
		}
		for (int l = 0; l < this.displayedInspirations.Count; l++)
		{
			this.displayedInspirations[l].transform.SetSiblingIndex(l);
		}
		for (int m = 0; m < this.displayedInspirationsFinished.Count; m++)
		{
			this.displayedInspirationsFinished[m].transform.SetSiblingIndex(m);
		}
		if (this.displayedInspirationsFinished.Count > 0 && this.displayedInspirations.Count > 0)
		{
			this.finishedSeparator.SetActive(true);
			return;
		}
		this.finishedSeparator.SetActive(false);
	}

	// Token: 0x06003CFD RID: 15613 RVA: 0x0012380C File Offset: 0x00121A0C
	private void HideInspirations()
	{
		foreach (InspirationWidget inspirationWidget in this.displayedInspirations)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<InspirationWidget>(inspirationWidget);
		}
		this.displayedInspirations.Clear();
		foreach (InspirationWidgetFinished inspirationWidgetFinished in this.displayedInspirationsFinished)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<InspirationWidgetFinished>(inspirationWidgetFinished);
		}
		this.displayedInspirationsFinished.Clear();
	}

	// Token: 0x06003CFE RID: 15614 RVA: 0x001238C0 File Offset: 0x00121AC0
	private void OnInspirationProgressChanged(string inspirationId)
	{
		InspirationData inspirationData = this.data.TalentData.activeInspirations.Find((InspirationData x) => x.id == inspirationId);
		if (inspirationData == null)
		{
			return;
		}
		foreach (InspirationWidget inspirationWidget in this.displayedInspirations)
		{
			if (inspirationId == inspirationWidget.IdWithoutLevel && inspirationWidget.Level == inspirationData.curLevel)
			{
				inspirationWidget.Redraw();
			}
		}
	}

	// Token: 0x06003CFF RID: 15615 RVA: 0x00123968 File Offset: 0x00121B68
	private void OnInspirationCompleted(string inspirationId)
	{
		if (!this.isShown)
		{
			return;
		}
		this.RedrawTabs(true);
	}

	// Token: 0x06003D00 RID: 15616 RVA: 0x0012397C File Offset: 0x00121B7C
	private void OnTalentLevelPurchased(string talentId, string levelId)
	{
		if (this.data.TalentData.id != talentId)
		{
			return;
		}
		TalentSystemData.DelTalentLevelPurchased onTalentLevelPurchased = this.talentLevelUpsWidgetData.onTalentLevelPurchased;
		if (onTalentLevelPurchased != null)
		{
			onTalentLevelPurchased(talentId, levelId);
		}
		this.RedrawTalentExpProgressWidget();
		this.UpdateCanBuyTalentLevelUpEffect();
		this.RedrawTabs(true);
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003D01 RID: 15617 RVA: 0x001239E0 File Offset: 0x00121BE0
	private void RedrawTalentExpProgressWidget()
	{
		if (this.isPlayingExpFlyAnimation)
		{
			return;
		}
		TalentExpProgressWidgetData talentExpProgressWidgetData = new TalentExpProgressWidgetData(this.data.TalentData);
		this.talentExpProgressWidget.Draw(talentExpProgressWidgetData);
	}

	// Token: 0x06003D02 RID: 15618 RVA: 0x00123A13 File Offset: 0x00121C13
	private void RedrawTabs(bool blockCurrentInspirationActionIndicator = true)
	{
		this.talentTabButtonsContainer.Draw(this.data.TalentData.id, blockCurrentInspirationActionIndicator);
		LazyUI.GetWindow<CharacterWindow>().RefreshInspirationPageActionIndicatorStatus();
	}

	// Token: 0x06003D03 RID: 15619 RVA: 0x00123A3B File Offset: 0x00121C3B
	private void RedrawTalentLevelUpsWidget()
	{
		this.talentLevelUpsWidgetData = new TalentLevelUpsWidgetData(this.data.TalentData);
		this.talentLevelUpsWidget.Draw(this.talentLevelUpsWidgetData);
	}

	// Token: 0x06003D04 RID: 15620 RVA: 0x00123A64 File Offset: 0x00121C64
	private void UpdateCanBuyTalentLevelUpEffect()
	{
		if (this.canBuyTalentLevelUpEffect == null)
		{
			return;
		}
		bool flag = this.isShown && this.data != null && this.data.TalentData != null && this.data.TalentData.HasAvailableTalentLevelUpToPurchase();
		this.canBuyTalentLevelUpEffect.SetActive(flag);
	}

	// Token: 0x06003D05 RID: 15621 RVA: 0x00123AC0 File Offset: 0x00121CC0
	private void OnInspirationPurchased(string id)
	{
		if (!this.isShown || this.isHandlingInspirationPurchase)
		{
			return;
		}
		this.isHandlingInspirationPurchase = true;
		try
		{
			InspirationWidget inspirationWidget = this.displayedInspirations.Find((InspirationWidget widget) => widget.IdWithoutLevel == id);
			this.RegisterGamepadFocusAfterPurchase(id, inspirationWidget);
			int num = ((inspirationWidget != null) ? inspirationWidget.DisplayedCompletionExp : 0);
			Vector3 vector = ((inspirationWidget != null && inspirationWidget.BuyExpRect != null) ? inspirationWidget.BuyExpRect.GetWorldRect().center : Vector3.zero);
			string text = ((inspirationWidget != null) ? inspirationWidget.PointIconId : null);
			TextMeshProUGUI textMeshProUGUI = ((inspirationWidget != null) ? inspirationWidget.BuyExpLabel : null);
			if (inspirationWidget != null)
			{
				inspirationWidget.HideFlyingReward();
			}
			this.RedrawTalentLevelUpsWidget();
			this.UpdateCanBuyTalentLevelUpEffect();
			this.RedrawTabs(true);
			this.RedrawFaith();
			Vector3 vector2 = default(Vector3);
			if (num <= 0 || string.IsNullOrEmpty(text) || !(textMeshProUGUI != null) || !this.talentExpProgressWidget.TryGetFlyTarget(out vector2))
			{
				this.DrawInspirations(this.data.TalentData);
				((RectTransform)base.transform).RefreshContentFitter();
				this.RestoreGamepadFocusAfterPurchaseIfPending();
				this.RedrawTalentExpProgressWidget();
				this.TryShowInspirationTalentsTutorial();
			}
			else
			{
				this.pendingInspirationsRedraw = true;
				this.PlayFlyingExpAnimation(vector, vector2, num, text.FontIcon(), textMeshProUGUI);
			}
		}
		finally
		{
			this.isHandlingInspirationPurchase = false;
		}
	}

	// Token: 0x06003D06 RID: 15622 RVA: 0x00123C54 File Offset: 0x00121E54
	private void RegisterGamepadFocusAfterPurchase(string id, InspirationWidget sourceWidget)
	{
		if (!LazyInput.IsGamepadActive)
		{
			return;
		}
		this.pendingGamepadFocusAfterPurchase = true;
		this.pendingPurchasedInspirationId = id;
		this.pendingPurchasedInspirationIndex = ((sourceWidget != null) ? this.displayedInspirations.IndexOf(sourceWidget) : (-1));
		this.pendingGamepadFocusAnchorIndex = this.pendingPurchasedInspirationIndex;
		this.pendingFinishedInspirationId = null;
		this.pendingFinishedInspirationLevel = -1;
		this.pendingFinishedInspirationIndex = -1;
		this.pendingGamepadFocusSourceNavigationItem = ((sourceWidget != null) ? sourceWidget.GetComponent<GamepadNavigationItem>() : null);
		this.pendingGamepadFocusNavigationItem = this.pendingGamepadFocusSourceNavigationItem;
		this.pendingGamepadFocusMovedAfterPurchase = false;
		this.SubscribePendingGamepadFocusChanges();
	}

	// Token: 0x06003D07 RID: 15623 RVA: 0x00123CE8 File Offset: 0x00121EE8
	private void UpdatePendingGamepadFocusFromCurrentFocus()
	{
		if (!this.pendingGamepadFocusAfterPurchase || !LazyInput.IsGamepadActive)
		{
			return;
		}
		CharacterWindow window = LazyUI.GetWindow<CharacterWindow>();
		GamepadNavigationController gamepadNavigationController = ((window != null) ? window.NavigationController : null);
		this.UpdatePendingGamepadFocusFromItem((gamepadNavigationController != null) ? gamepadNavigationController.FocusedItem : null);
	}

	// Token: 0x06003D08 RID: 15624 RVA: 0x00123D36 File Offset: 0x00121F36
	private void OnPendingGamepadFocusedItemChanged(GamepadNavigationItem focusedItem)
	{
		if (this.isRestoringGamepadFocusAfterPurchase || !this.pendingGamepadFocusAfterPurchase)
		{
			return;
		}
		this.UpdatePendingGamepadFocusFromItem(focusedItem);
	}

	// Token: 0x06003D09 RID: 15625 RVA: 0x00123D50 File Offset: 0x00121F50
	private void UpdatePendingGamepadFocusFromItem(GamepadNavigationItem focusedItem)
	{
		if (focusedItem == null)
		{
			return;
		}
		if (!focusedItem.transform.IsChildOf(base.transform))
		{
			return;
		}
		if (focusedItem == this.pendingGamepadFocusSourceNavigationItem)
		{
			return;
		}
		this.pendingGamepadFocusMovedAfterPurchase = true;
		InspirationWidget inspirationWidget;
		InspirationWidgetFinished inspirationWidgetFinished;
		if (focusedItem.TryGetComponent<InspirationWidget>(out inspirationWidget) && this.displayedInspirations.Contains(inspirationWidget))
		{
			this.pendingPurchasedInspirationId = inspirationWidget.IdWithoutLevel;
			this.pendingPurchasedInspirationIndex = this.displayedInspirations.IndexOf(inspirationWidget);
			this.pendingFinishedInspirationId = null;
			this.pendingFinishedInspirationLevel = -1;
			this.pendingFinishedInspirationIndex = -1;
		}
		else if (focusedItem.TryGetComponent<InspirationWidgetFinished>(out inspirationWidgetFinished) && this.displayedInspirationsFinished.Contains(inspirationWidgetFinished))
		{
			this.pendingPurchasedInspirationId = null;
			this.pendingPurchasedInspirationIndex = -1;
			this.pendingFinishedInspirationId = inspirationWidgetFinished.IdWithoutLevel;
			this.pendingFinishedInspirationLevel = inspirationWidgetFinished.Level;
			this.pendingFinishedInspirationIndex = this.displayedInspirationsFinished.IndexOf(inspirationWidgetFinished);
		}
		else
		{
			this.pendingPurchasedInspirationId = null;
			this.pendingPurchasedInspirationIndex = -1;
			this.pendingFinishedInspirationId = null;
			this.pendingFinishedInspirationLevel = -1;
			this.pendingFinishedInspirationIndex = -1;
		}
		this.pendingGamepadFocusNavigationItem = focusedItem;
	}

	// Token: 0x06003D0A RID: 15626 RVA: 0x00123E5C File Offset: 0x0012205C
	private void SubscribePendingGamepadFocusChanges()
	{
		this.UnsubscribePendingGamepadFocusChanges();
		CharacterWindow window = LazyUI.GetWindow<CharacterWindow>();
		this.pendingGamepadFocusNavigationController = ((window != null) ? window.NavigationController : null);
		if (this.pendingGamepadFocusNavigationController != null)
		{
			this.pendingGamepadFocusNavigationController.OnFocusedItemChanged += this.OnPendingGamepadFocusedItemChanged;
		}
	}

	// Token: 0x06003D0B RID: 15627 RVA: 0x00123EB2 File Offset: 0x001220B2
	private void UnsubscribePendingGamepadFocusChanges()
	{
		if (this.pendingGamepadFocusNavigationController == null)
		{
			return;
		}
		this.pendingGamepadFocusNavigationController.OnFocusedItemChanged -= this.OnPendingGamepadFocusedItemChanged;
		this.pendingGamepadFocusNavigationController = null;
	}

	// Token: 0x06003D0C RID: 15628 RVA: 0x00123EE4 File Offset: 0x001220E4
	private void RestoreGamepadFocusAfterPurchaseIfPending()
	{
		if (!this.pendingGamepadFocusAfterPurchase)
		{
			return;
		}
		CharacterWindow window = LazyUI.GetWindow<CharacterWindow>();
		GamepadNavigationController gamepadNavigationController = ((window != null) ? window.NavigationController : null);
		if (!LazyInput.IsGamepadActive || gamepadNavigationController == null)
		{
			this.ClearPendingGamepadFocusAfterPurchase();
			return;
		}
		GamepadNavigationItem gamepadNavigationItem = (this.pendingGamepadFocusMovedAfterPurchase ? this.FindMovedGamepadFocusAfterPurchase() : this.FindAutomaticGamepadFocusAfterPurchase());
		if (gamepadNavigationItem != null)
		{
			this.isRestoringGamepadFocusAfterPurchase = true;
			try
			{
				if (this.autoScroll != null)
				{
					this.autoScroll.SkipNextAutoscroll = true;
				}
				gamepadNavigationController.ReinitItems(false, null, null);
				gamepadNavigationController.SetFocusedItem(gamepadNavigationItem);
				if (this.autoScroll != null)
				{
					this.autoScroll.ScrollToItem(gamepadNavigationItem);
					this.autoScroll.SkipNextAutoscroll = false;
				}
				goto IL_00C3;
			}
			finally
			{
				this.isRestoringGamepadFocusAfterPurchase = false;
			}
		}
		gamepadNavigationController.ReinitItems(true, null, null);
		IL_00C3:
		this.ClearPendingGamepadFocusAfterPurchase();
	}

	// Token: 0x06003D0D RID: 15629 RVA: 0x00123FCC File Offset: 0x001221CC
	private GamepadNavigationItem FindMovedGamepadFocusAfterPurchase()
	{
		InspirationWidget inspirationWidget = (string.IsNullOrEmpty(this.pendingPurchasedInspirationId) ? null : this.displayedInspirations.Find((InspirationWidget widget) => widget.IdWithoutLevel == this.pendingPurchasedInspirationId));
		GamepadNavigationItem gamepadNavigationItem = null;
		if (inspirationWidget != null)
		{
			inspirationWidget.TryGetComponent<GamepadNavigationItem>(out gamepadNavigationItem);
		}
		if (gamepadNavigationItem == null)
		{
			InspirationWidgetFinished inspirationWidgetFinished = this.FindPendingFinishedInspirationWidget();
			if (inspirationWidgetFinished != null)
			{
				inspirationWidgetFinished.TryGetComponent<GamepadNavigationItem>(out gamepadNavigationItem);
			}
		}
		if (gamepadNavigationItem == null && this.IsValidGamepadFocusNavigationItem(this.pendingGamepadFocusNavigationItem))
		{
			gamepadNavigationItem = this.pendingGamepadFocusNavigationItem;
		}
		if (gamepadNavigationItem == null)
		{
			gamepadNavigationItem = this.FindAnyGamepadFocusAfterPurchase();
		}
		return gamepadNavigationItem;
	}

	// Token: 0x06003D0E RID: 15630 RVA: 0x00124068 File Offset: 0x00122268
	private GamepadNavigationItem FindAutomaticGamepadFocusAfterPurchase()
	{
		GamepadNavigationItem gamepadNavigationItem = this.GetDisplayedInspirationNavigationItem(this.pendingGamepadFocusAnchorIndex, false);
		if (gamepadNavigationItem != null)
		{
			return gamepadNavigationItem;
		}
		gamepadNavigationItem = this.FindNearestDisplayedInspirationNavigationItem(this.pendingGamepadFocusAnchorIndex, 1, true);
		if (gamepadNavigationItem != null)
		{
			return gamepadNavigationItem;
		}
		gamepadNavigationItem = this.FindNearestDisplayedInspirationNavigationItem(this.pendingGamepadFocusAnchorIndex, -1, true);
		if (gamepadNavigationItem != null)
		{
			return gamepadNavigationItem;
		}
		gamepadNavigationItem = this.FindNearestDisplayedInspirationNavigationItem(this.pendingGamepadFocusAnchorIndex, 1, false);
		if (gamepadNavigationItem != null)
		{
			return gamepadNavigationItem;
		}
		gamepadNavigationItem = this.FindNearestDisplayedInspirationNavigationItem(this.pendingGamepadFocusAnchorIndex, -1, false);
		if (!(gamepadNavigationItem != null))
		{
			return this.FindAnyGamepadFocusAfterPurchase();
		}
		return gamepadNavigationItem;
	}

	// Token: 0x06003D0F RID: 15631 RVA: 0x001240FC File Offset: 0x001222FC
	private InspirationWidgetFinished FindPendingFinishedInspirationWidget()
	{
		if (string.IsNullOrEmpty(this.pendingFinishedInspirationId))
		{
			return null;
		}
		InspirationWidgetFinished inspirationWidgetFinished = this.displayedInspirationsFinished.Find((InspirationWidgetFinished widget) => widget.IdWithoutLevel == this.pendingFinishedInspirationId && widget.Level == this.pendingFinishedInspirationLevel);
		if (inspirationWidgetFinished != null)
		{
			return inspirationWidgetFinished;
		}
		if (this.pendingFinishedInspirationIndex >= 0 && this.pendingFinishedInspirationIndex < this.displayedInspirationsFinished.Count)
		{
			return this.displayedInspirationsFinished[this.pendingFinishedInspirationIndex];
		}
		return null;
	}

	// Token: 0x06003D10 RID: 15632 RVA: 0x0012416C File Offset: 0x0012236C
	private GamepadNavigationItem FindAnyGamepadFocusAfterPurchase()
	{
		foreach (InspirationWidget inspirationWidget in this.displayedInspirations)
		{
			GamepadNavigationItem gamepadNavigationItem;
			if (inspirationWidget != null && inspirationWidget.TryGetComponent<GamepadNavigationItem>(out gamepadNavigationItem) && this.IsValidGamepadFocusNavigationItem(gamepadNavigationItem))
			{
				return gamepadNavigationItem;
			}
		}
		foreach (InspirationWidgetFinished inspirationWidgetFinished in this.displayedInspirationsFinished)
		{
			GamepadNavigationItem gamepadNavigationItem2;
			if (inspirationWidgetFinished != null && inspirationWidgetFinished.TryGetComponent<GamepadNavigationItem>(out gamepadNavigationItem2) && this.IsValidGamepadFocusNavigationItem(gamepadNavigationItem2))
			{
				return gamepadNavigationItem2;
			}
		}
		return null;
	}

	// Token: 0x06003D11 RID: 15633 RVA: 0x0012423C File Offset: 0x0012243C
	private GamepadNavigationItem FindNearestDisplayedInspirationNavigationItem(int startIndex, int step, bool requireBuyable)
	{
		if (this.displayedInspirations.Count == 0)
		{
			return null;
		}
		int num = startIndex + step;
		if (startIndex < 0)
		{
			num = ((step > 0) ? 0 : (this.displayedInspirations.Count - 1));
		}
		while (num >= 0 && num < this.displayedInspirations.Count)
		{
			GamepadNavigationItem displayedInspirationNavigationItem = this.GetDisplayedInspirationNavigationItem(num, requireBuyable);
			if (displayedInspirationNavigationItem != null)
			{
				return displayedInspirationNavigationItem;
			}
			num += step;
		}
		return null;
	}

	// Token: 0x06003D12 RID: 15634 RVA: 0x001242A4 File Offset: 0x001224A4
	private GamepadNavigationItem GetDisplayedInspirationNavigationItem(int index, bool requireBuyable)
	{
		if (index < 0 || index >= this.displayedInspirations.Count)
		{
			return null;
		}
		InspirationWidget inspirationWidget = this.displayedInspirations[index];
		if (inspirationWidget == null)
		{
			return null;
		}
		if (requireBuyable && (inspirationWidget.IsBuyLocked || inspirationWidget.InspirationData == null || !inspirationWidget.InspirationData.IsAvailableToBuy))
		{
			return null;
		}
		GamepadNavigationItem gamepadNavigationItem;
		if (!inspirationWidget.TryGetComponent<GamepadNavigationItem>(out gamepadNavigationItem) || !this.IsValidGamepadFocusNavigationItem(gamepadNavigationItem))
		{
			return null;
		}
		return gamepadNavigationItem;
	}

	// Token: 0x06003D13 RID: 15635 RVA: 0x00124315 File Offset: 0x00122515
	private bool IsValidGamepadFocusNavigationItem(GamepadNavigationItem navigationItem)
	{
		return navigationItem != null && navigationItem.isActiveAndEnabled && navigationItem.Active && navigationItem.gameObject.activeInHierarchy && navigationItem.transform.IsChildOf(base.transform);
	}

	// Token: 0x06003D14 RID: 15636 RVA: 0x00124350 File Offset: 0x00122550
	private void ClearPendingGamepadFocusAfterPurchase()
	{
		this.pendingGamepadFocusAfterPurchase = false;
		this.pendingPurchasedInspirationId = null;
		this.pendingPurchasedInspirationIndex = -1;
		this.pendingFinishedInspirationId = null;
		this.pendingFinishedInspirationLevel = -1;
		this.pendingFinishedInspirationIndex = -1;
		this.pendingGamepadFocusNavigationItem = null;
		this.pendingGamepadFocusSourceNavigationItem = null;
		this.isRestoringGamepadFocusAfterPurchase = false;
		this.pendingGamepadFocusMovedAfterPurchase = false;
		this.pendingGamepadFocusAnchorIndex = -1;
		this.UnsubscribePendingGamepadFocusChanges();
	}

	// Token: 0x06003D15 RID: 15637 RVA: 0x001243B0 File Offset: 0x001225B0
	private void TryShowInspirationTalentsTutorial()
	{
		if (!this.isShown)
		{
			return;
		}
		if (MainGame.PlayerData.sawInspirationTalentsTutorialOnce)
		{
			return;
		}
		if (this.data == null || this.data.TalentData == null || this.data.TalentData.talentExpPoints <= 0)
		{
			return;
		}
		MainGame.PlayerData.sawInspirationTalentsTutorialOnce = true;
		LazyUI.GetWindow<UITutorialWindow>().Open(new UITutorialWindowData("tut_insp_talents_hdr_new", null, false));
	}

	// Token: 0x06003D16 RID: 15638 RVA: 0x00124420 File Offset: 0x00122620
	public InspirationWidget FindDisplayedInspiration(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.displayedInspirations.Find((InspirationWidget widget) => widget != null && widget.IdWithoutLevel == id);
	}

	// Token: 0x06003D17 RID: 15639 RVA: 0x00124460 File Offset: 0x00122660
	public bool CompleteFlyingExpAnimationImmediately()
	{
		if (!this.isPlayingExpFlyAnimation)
		{
			return false;
		}
		for (int i = 0; i < this.flyingExpPoints.Count; i++)
		{
			if (this.flyingExpPoints[i] != null)
			{
				this.flyingExpPoints[i].StopAndRelease();
			}
		}
		this.flyingExpPoints.Clear();
		this.remainingFlyingExpCount = 0;
		this.FinishFlyingExpAnimation();
		return true;
	}

	// Token: 0x06003D18 RID: 15640 RVA: 0x001244CC File Offset: 0x001226CC
	private void PlayFlyingExpAnimation(Vector3 startPos, Vector3 targetPos, int completionExp, string icon, TextMeshProUGUI sourceLabel)
	{
		this.EnsureFlyingExpPool(sourceLabel);
		if (this.flyingExpPool == null)
		{
			this.RedrawTalentExpProgressWidget();
			this.TryShowInspirationTalentsTutorial();
			return;
		}
		int num = this.remainingFlyingExpCount;
		if (!this.isPlayingExpFlyAnimation)
		{
			this.isPlayingExpFlyAnimation = true;
			this.talentExpProgressWidget.PrepareFillAnimation();
		}
		this.talentExpProgressWidget.SetFillAnimationTarget(new TalentExpProgressWidgetData(this.data.TalentData));
		this.remainingFlyingExpCount += completionExp;
		for (int i = 0; i < completionExp; i++)
		{
			Vector3 vector = targetPos;
			this.talentExpProgressWidget.TryGetFlyTarget(num + i, out vector);
			FlyingInspirationExpPoint orCreateObject = this.flyingExpPool.GetOrCreateObject<FlyingInspirationExpPoint>();
			orCreateObject.gameObject.SetActive(false);
			orCreateObject.transform.SetParent(this.flyingExpOverlay, false);
			orCreateObject.transform.SetAsLastSibling();
			this.flyingExpPoints.Add(orCreateObject);
			FlyingInspirationExpPoint capturedPoint = orCreateObject;
			capturedPoint.Fly(startPos, vector, icon, this.flyingExpPool, this.flyingExpPointDuration, delegate
			{
				this.OnFlyingExpPointReached(capturedPoint);
			});
		}
	}

	// Token: 0x06003D19 RID: 15641 RVA: 0x001245E8 File Offset: 0x001227E8
	private void OnFlyingExpPointReached(FlyingInspirationExpPoint flyingPoint)
	{
		this.flyingExpPoints.Remove(flyingPoint);
		if (!this.isShown || !this.isPlayingExpFlyAnimation)
		{
			return;
		}
		this.remainingFlyingExpCount--;
		this.talentExpProgressWidget.ApplyArrivedExpPoint(new Action(this.TryFinishFlyingExpAnimation));
	}

	// Token: 0x06003D1A RID: 15642 RVA: 0x00124638 File Offset: 0x00122838
	private void TryFinishFlyingExpAnimation()
	{
		if (!this.isShown || !this.isPlayingExpFlyAnimation || this.remainingFlyingExpCount > 0 || this.talentExpProgressWidget.HasPendingBarReset)
		{
			return;
		}
		this.FinishFlyingExpAnimation();
	}

	// Token: 0x06003D1B RID: 15643 RVA: 0x00124667 File Offset: 0x00122867
	private void FinishFlyingExpAnimation()
	{
		this.isPlayingExpFlyAnimation = false;
		this.remainingFlyingExpCount = 0;
		this.flyingExpPoints.Clear();
		this.talentExpProgressWidget.ResetFillAnimationState();
		if (this.isShown)
		{
			this.RedrawInspirationsIfPending();
			this.RedrawTalentExpProgressWidget();
			this.TryShowInspirationTalentsTutorial();
		}
	}

	// Token: 0x06003D1C RID: 15644 RVA: 0x001246A8 File Offset: 0x001228A8
	private void CancelFlyingExpAnimation()
	{
		this.isPlayingExpFlyAnimation = false;
		this.remainingFlyingExpCount = 0;
		for (int i = 0; i < this.flyingExpPoints.Count; i++)
		{
			if (this.flyingExpPoints[i] != null)
			{
				this.flyingExpPoints[i].StopAndRelease();
			}
		}
		this.flyingExpPoints.Clear();
		this.talentExpProgressWidget.ResetFillAnimationState();
		if (!this.isShown)
		{
			this.pendingInspirationsRedraw = false;
			return;
		}
		if (base.isActiveAndEnabled)
		{
			this.RedrawInspirationsIfPending();
		}
	}

	// Token: 0x06003D1D RID: 15645 RVA: 0x00124732 File Offset: 0x00122932
	private void RedrawInspirationsIfPending()
	{
		if (!this.pendingInspirationsRedraw)
		{
			return;
		}
		this.pendingInspirationsRedraw = false;
		this.UpdatePendingGamepadFocusFromCurrentFocus();
		this.DrawInspirations(this.data.TalentData);
		((RectTransform)base.transform).RefreshContentFitter();
		this.RestoreGamepadFocusAfterPurchaseIfPending();
	}

	// Token: 0x06003D1E RID: 15646 RVA: 0x00124774 File Offset: 0x00122974
	private void EnsureFlyingExpOverlay()
	{
		if (this.flyingExpOverlay != null)
		{
			return;
		}
		RectTransform component = new GameObject("FlyingInspirationExpOverlay", new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
		component.SetParent(base.transform, false);
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		component.SetAsLastSibling();
		this.flyingExpOverlay = component;
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x001247FC File Offset: 0x001229FC
	private void EnsureFlyingExpPool(TextMeshProUGUI sourceLabel)
	{
		if (this.flyingExpPool != null)
		{
			return;
		}
		this.EnsureFlyingExpOverlay();
		if (sourceLabel == null)
		{
			return;
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(sourceLabel.gameObject, this.flyingExpOverlay);
		gameObject.name = "FlyingInspirationExpPoint";
		gameObject.SetActive(false);
		EventTrigger component = gameObject.GetComponent<EventTrigger>();
		if (component != null)
		{
			component.enabled = false;
		}
		TextMeshProUGUI component2 = gameObject.GetComponent<TextMeshProUGUI>();
		if (component2 != null)
		{
			component2.raycastTarget = false;
			component2.overflowMode = TextOverflowModes.Overflow;
			component2.textWrappingMode = TextWrappingModes.NoWrap;
			component2.alignment = TextAlignmentOptions.Center;
		}
		FlyingInspirationExpPoint flyingInspirationExpPoint = gameObject.GetComponent<FlyingInspirationExpPoint>();
		if (flyingInspirationExpPoint == null)
		{
			flyingInspirationExpPoint = gameObject.AddComponent<FlyingInspirationExpPoint>();
		}
		this.flyingExpPool = new Pool(flyingInspirationExpPoint, this.flyingExpOverlay, 0, Pool.PoolType.ImmediateActivation, false, null);
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x001248BC File Offset: 0x00122ABC
	public override List<LazyGameKeyTip> GetTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (gamepadNavigationItem == null)
		{
			return list;
		}
		InspirationWidget inspirationWidget;
		if (gamepadNavigationItem.TryGetComponent<InspirationWidget>(out inspirationWidget) && inspirationWidget != null && inspirationWidget.InspirationData != null)
		{
			list.Add(LazyGameKeyTip.Select(!inspirationWidget.IsBuyLocked && inspirationWidget.InspirationData.IsAvailableToBuy && inspirationWidget.InspirationData.IsCompleted, true, true));
		}
		TalentLevelUpWidget talentLevelUpWidget;
		if (gamepadNavigationItem.TryGetComponent<TalentLevelUpWidget>(out talentLevelUpWidget) && talentLevelUpWidget != null)
		{
			TalentLevelUpWidgetData data = talentLevelUpWidget.Data;
			if (((data != null) ? data.Def : null) != null)
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
					flag = ((gameSave != null) ? gameSave.talentSystemData : null) != null;
				}
				if (flag)
				{
					TalentData talentData;
					list.Add(LazyGameKeyTip.Select(MainGame.Instance.GameSave.talentSystemData.CanPurchaseLevel(talentLevelUpWidget.Data.Def.id, out talentData), true, true));
				}
			}
		}
		return list;
	}

	// Token: 0x06003D21 RID: 15649 RVA: 0x0012499F File Offset: 0x00122B9F
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new CharInspirationPageWidgetData(MainGame.Instance.GameSave.talentSystemData, null));
	}

	// Token: 0x04002FDB RID: 12251
	[Space]
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04002FDC RID: 12252
	[Space]
	[SerializeField]
	private TalentTabButtonsContainer talentTabButtonsContainer;

	// Token: 0x04002FDD RID: 12253
	[SerializeField]
	private TalentExpProgressWidget talentExpProgressWidget;

	// Token: 0x04002FDE RID: 12254
	[SerializeField]
	private UIItemCell faithCell;

	// Token: 0x04002FDF RID: 12255
	[SerializeField]
	private TextMeshProUGUI nextSubTabGamepadHelper;

	// Token: 0x04002FE0 RID: 12256
	[SerializeField]
	private TextMeshProUGUI prevSubTabGamepadHelper;

	// Token: 0x04002FE1 RID: 12257
	[Space]
	[SerializeField]
	private InspirationWidget inspirationWidgetPrefab;

	// Token: 0x04002FE2 RID: 12258
	[SerializeField]
	private Transform inspirationWidgetsParent;

	// Token: 0x04002FE3 RID: 12259
	[SerializeField]
	private Transform inspirationWidgetsFinishedParent;

	// Token: 0x04002FE4 RID: 12260
	[SerializeField]
	private GameObject finishedSeparator;

	// Token: 0x04002FE5 RID: 12261
	private readonly List<InspirationWidget> displayedInspirations = new List<InspirationWidget>();

	// Token: 0x04002FE6 RID: 12262
	private readonly List<InspirationWidgetFinished> displayedInspirationsFinished = new List<InspirationWidgetFinished>();

	// Token: 0x04002FE7 RID: 12263
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04002FE8 RID: 12264
	[SerializeField]
	private AutoScroll autoScroll;

	// Token: 0x04002FE9 RID: 12265
	[SerializeField]
	private TalentLevelUpsWidget talentLevelUpsWidget;

	// Token: 0x04002FEA RID: 12266
	private TalentLevelUpsWidgetData talentLevelUpsWidgetData;

	// Token: 0x04002FEB RID: 12267
	[SerializeField]
	private FlyingInspirationExpPoint flyingExpPointPrefab;

	// Token: 0x04002FEC RID: 12268
	[SerializeField]
	private float flyingExpPointDuration = 0.6666667f;

	// Token: 0x04002FED RID: 12269
	[SerializeField]
	private GameObject canBuyTalentLevelUpEffect;

	// Token: 0x04002FEE RID: 12270
	private bool isShown;

	// Token: 0x04002FEF RID: 12271
	private Pool flyingExpPool;

	// Token: 0x04002FF0 RID: 12272
	private Transform flyingExpOverlay;

	// Token: 0x04002FF1 RID: 12273
	private bool isPlayingExpFlyAnimation;

	// Token: 0x04002FF2 RID: 12274
	private bool isHandlingInspirationPurchase;

	// Token: 0x04002FF3 RID: 12275
	private bool pendingInspirationsRedraw;

	// Token: 0x04002FF4 RID: 12276
	private bool pendingGamepadFocusAfterPurchase;

	// Token: 0x04002FF5 RID: 12277
	private string pendingPurchasedInspirationId;

	// Token: 0x04002FF6 RID: 12278
	private int pendingPurchasedInspirationIndex = -1;

	// Token: 0x04002FF7 RID: 12279
	private string pendingFinishedInspirationId;

	// Token: 0x04002FF8 RID: 12280
	private int pendingFinishedInspirationLevel = -1;

	// Token: 0x04002FF9 RID: 12281
	private int pendingFinishedInspirationIndex = -1;

	// Token: 0x04002FFA RID: 12282
	private GamepadNavigationItem pendingGamepadFocusNavigationItem;

	// Token: 0x04002FFB RID: 12283
	private GamepadNavigationItem pendingGamepadFocusSourceNavigationItem;

	// Token: 0x04002FFC RID: 12284
	private GamepadNavigationController pendingGamepadFocusNavigationController;

	// Token: 0x04002FFD RID: 12285
	private bool pendingGamepadFocusMovedAfterPurchase;

	// Token: 0x04002FFE RID: 12286
	private int pendingGamepadFocusAnchorIndex = -1;

	// Token: 0x04002FFF RID: 12287
	private bool isRestoringGamepadFocusAfterPurchase;

	// Token: 0x04003000 RID: 12288
	private int remainingFlyingExpCount;

	// Token: 0x04003001 RID: 12289
	private readonly List<FlyingInspirationExpPoint> flyingExpPoints = new List<FlyingInspirationExpPoint>();
}
