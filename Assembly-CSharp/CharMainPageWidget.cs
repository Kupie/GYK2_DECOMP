using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000936 RID: 2358
public class CharMainPageWidget : LazyWidget<CharMainPageWidgetData>
{
	// Token: 0x1700095A RID: 2394
	// (get) Token: 0x06003E19 RID: 15897 RVA: 0x00128937 File Offset: 0x00126B37
	private CharacterWindow CharacterWindow
	{
		get
		{
			return LazyUI.GetWindow<CharacterWindow>();
		}
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x00128940 File Offset: 0x00126B40
	public override void Init()
	{
		base.Init();
		if (this.perksWidget != null)
		{
			SmoothMouseWheelScroll.EnsureForItem(this.perksWidget.GetComponentInParent<ScrollRect>(), 48f);
		}
		this.closeBagButton.onClick.AddListener(new UnityAction(this.OnCloseBagBtnPressed));
		this.perksWidget.onContentChanged += this.RefreshContentFitter;
		this.buffsWidget.onContentChanged += this.RefreshContentFitter;
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x001289C4 File Offset: 0x00126BC4
	public override void Redraw()
	{
		this.multiInventoryWidget.Draw(this.data.MultiInventoryWidgetData);
		this.toolBeltInventoryWidget.Draw(this.data.ToolBeltInventoryWidgetData);
		if (this.data.IsBagShown)
		{
			this.bagInventoryWidget.Draw(this.data.BagInventoryWidgetData);
		}
		else
		{
			this.bagInventoryWidget.Hide();
		}
		this.perksWidget.Draw(this.data.PerksWidgetData);
		this.buffsWidget.Draw(this.data.BuffsWidgetData);
		this.moneyWidget.Draw(this.data.MoneyWidgetData);
		this.perksFooter.gameObject.SetActive(this.data.PerksWidgetData.Perks.Count > 0);
		this.DrawTalentToolRowWidgets();
		this.EnsureMasteryLevelsTooltip();
		this.TrySubscribe();
		this.multiInventoryWidget.OnMoveAllSimilarBtnInteractableChanged = new Action(this.OnMoveAllSimilarBtnInteractableChanged);
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x00128AC0 File Offset: 0x00126CC0
	public override void Hide()
	{
		if (this.multiInventoryWidget != null)
		{
			this.multiInventoryWidget.OnMoveAllSimilarBtnInteractableChanged = null;
		}
		this.TryUnsubscribe();
		CharMainPageWidgetData data = this.data;
		if (data != null)
		{
			data.HideBag();
		}
		this.multiInventoryWidget.Hide();
		this.toolBeltInventoryWidget.Hide();
		if (this.data != null && this.data.IsBagShown)
		{
			this.bagInventoryWidget.Hide();
		}
		this.perksWidget.Hide();
		this.buffsWidget.Hide();
		this.moneyWidget.Hide();
		base.Hide();
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x00128B5C File Offset: 0x00126D5C
	private void TrySubscribe()
	{
		if (!this.subscribed)
		{
			this.data.ToolBeltInventoryWidgetData.Inventory.OnItemsAdd += this.OnToolbeltItemsChanged;
			this.data.ToolBeltInventoryWidgetData.Inventory.OnItemsRemove += this.OnToolbeltItemsChanged;
			this.data.PlayerData.OnItemUsed += this.OnPlayerItemUsed;
			this.subscribed = true;
		}
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x00128BD8 File Offset: 0x00126DD8
	private void TryUnsubscribe()
	{
		if (this.subscribed)
		{
			this.data.ToolBeltInventoryWidgetData.Inventory.OnItemsAdd -= this.OnToolbeltItemsChanged;
			this.data.ToolBeltInventoryWidgetData.Inventory.OnItemsRemove -= this.OnToolbeltItemsChanged;
			this.data.PlayerData.OnItemUsed -= this.OnPlayerItemUsed;
			this.subscribed = false;
		}
	}

	// Token: 0x06003E1F RID: 15903 RVA: 0x00128C52 File Offset: 0x00126E52
	private void RefreshContentFitter()
	{
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x00128C64 File Offset: 0x00126E64
	private void OnToolbeltItemsChanged(List<Item> items)
	{
		this.DrawTalentToolRowWidgets();
		this.toolBeltInventoryWidget.Draw(this.data.ToolBeltInventoryWidgetData);
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x00128C82 File Offset: 0x00126E82
	private void OnPlayerItemUsed(Item item)
	{
		this.OnToolbeltItemsChanged(null);
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x00128C8C File Offset: 0x00126E8C
	private void DrawTalentToolRowWidgets()
	{
		for (int i = 0; i < this.talentWidgets.Count; i++)
		{
			this.talentWidgets[i].Draw(new TalentWidgetData(this.talents[i], MainGame.PlayerController.GetMasteryLevelForTalentBranch(this.talents[i], null)));
		}
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x00128CE8 File Offset: 0x00126EE8
	private void EnsureMasteryLevelsTooltip()
	{
		if (this.talentWidgets == null || this.talentWidgets.Count == 0 || this.talentWidgets[0] == null)
		{
			return;
		}
		RectTransform rectTransform = this.talentWidgets[0].transform.parent.parent.parent as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		if (this.masteryLevelsHoverArea == null)
		{
			this.masteryLevelsHoverArea = UIMouseTooltip.GetOrCreateOverlay(rectTransform, "MasteryLevelsHoverArea");
		}
		List<RectTransform> list = new List<RectTransform>();
		for (int i = 0; i < this.talentWidgets.Count; i++)
		{
			if (this.talentWidgets[i] != null)
			{
				list.Add((RectTransform)this.talentWidgets[i].transform);
			}
		}
		UIMouseTooltip.FitOverlayToWorldRects(this.masteryLevelsHoverArea, list);
		this.masteryLevelsHoverArea.SetAsLastSibling();
		UIMouseTooltip.Attach(this.masteryLevelsHoverArea.gameObject, "tt_your_mastery_levels", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x00128E00 File Offset: 0x00127000
	public void OnBagShown(Item bag)
	{
		GamepadNavigationItem[] componentsInChildren = this.toolBeltInventoryWidget.GetComponentsInChildren<GamepadNavigationItem>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = false;
		}
		this.perksWidget.DisableGamepadNavigation();
		this.buffsWidget.DisableGamepadNavigation();
		this.CharacterWindow.NavigationController.navigationGroupSources.Clear();
		int count = this.multiInventoryWidget.DrawnInventories.Count;
		List<GamepadNavigationController.NavigationGroupTarget> list = new List<GamepadNavigationController.NavigationGroupTarget>();
		List<GamepadNavigationController.NavigationGroupTarget> list2 = new List<GamepadNavigationController.NavigationGroupTarget>();
		list.Add(new GamepadNavigationController.NavigationGroupTarget(1000, GUIDirection.Right));
		for (int j = 0; j < count; j++)
		{
			list2.Add(new GamepadNavigationController.NavigationGroupTarget(j, GUIDirection.Left));
		}
		for (int k = 0; k < count; k++)
		{
			InventoryWidget inventoryWidget = this.multiInventoryWidget.DrawnInventories[k];
			List<GamepadNavigationController.NavigationGroupTarget> list3 = new List<GamepadNavigationController.NavigationGroupTarget>();
			list3.AddRange(list);
			list3.Add(new GamepadNavigationController.NavigationGroupTarget(k - 1, GUIDirection.Up));
			list3.Add(new GamepadNavigationController.NavigationGroupTarget(k + 1, GUIDirection.Down));
			this.CharacterWindow.NavigationController.navigationGroupSources.Add(new GamepadNavigationController.NavigationGroupSource(k, list3));
			for (int l = 0; l < inventoryWidget.Cells.Count; l++)
			{
				inventoryWidget.Cells[l].GamepadNavigationItem.group = k;
			}
		}
		List<GamepadNavigationController.NavigationGroupTarget> list4 = new List<GamepadNavigationController.NavigationGroupTarget>();
		list4.AddRange(list2);
		this.CharacterWindow.NavigationController.navigationGroupSources.Add(new GamepadNavigationController.NavigationGroupSource(1000, list4));
		this.bagInventoryWidget.Draw(this.data.BagInventoryWidgetData);
		for (int m = 0; m < this.bagInventoryWidget.Cells.Count; m++)
		{
			this.bagInventoryWidget.Cells[m].GamepadNavigationItem.group = 1000;
		}
		this.bagStateShading.SetActive(true);
		this.multiInventoryWidget.EnableBagMode(bag, this.data.OnMoveAllSimilarItemFromPlayerToBag);
		((RectTransform)this.bagInventoryWidget.transform).RefreshContentFitter();
		LazyAudio.PlayAndForget("bag_open");
		base.StartCoroutine(this.RebuildAfterBagShownNextFrame());
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x0012902F File Offset: 0x0012722F
	private IEnumerator RebuildAfterBagShownNextFrame()
	{
		yield return new WaitForEndOfFrame();
		((RectTransform)this.multiInventoryWidget.transform).RefreshContentFitter();
		yield break;
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x00129040 File Offset: 0x00127240
	public void OnBagHide(Item bag)
	{
		for (int i = 0; i < this.multiInventoryWidget.DrawnInventories.Count; i++)
		{
			InventoryWidget inventoryWidget = this.multiInventoryWidget.DrawnInventories[i];
			for (int j = 0; j < inventoryWidget.Cells.Count; j++)
			{
				inventoryWidget.Cells[j].GamepadNavigationItem.group = 0;
			}
		}
		this.CharacterWindow.NavigationController.navigationGroupSources.Clear();
		GamepadNavigationItem[] componentsInChildren = this.toolBeltInventoryWidget.GetComponentsInChildren<GamepadNavigationItem>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].Active = true;
		}
		this.perksWidget.EnableGamepadNavigation();
		this.buffsWidget.EnableGamepadNavigation();
		this.bagInventoryWidget.Hide();
		this.bagStateShading.SetActive(false);
		this.multiInventoryWidget.DisableBagMode(bag);
		LazyAudio.PlayAndForget("bag_close");
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x00129127 File Offset: 0x00127327
	public bool IsBagModeEnabled()
	{
		return this.data != null && this.multiInventoryWidget != null && this.multiInventoryWidget.IsBagSelected;
	}

	// Token: 0x06003E28 RID: 15912 RVA: 0x0012914C File Offset: 0x0012734C
	public void OnAllToChestPressed()
	{
		if (!this.IsBagModeEnabled())
		{
			return;
		}
		Action onMoveAllSimilarItemFromPlayerToBag = this.data.OnMoveAllSimilarItemFromPlayerToBag;
		if (onMoveAllSimilarItemFromPlayerToBag == null)
		{
			return;
		}
		onMoveAllSimilarItemFromPlayerToBag();
	}

	// Token: 0x06003E29 RID: 15913 RVA: 0x0012916C File Offset: 0x0012736C
	private void OnCloseBagBtnPressed()
	{
		this.data.HideBag();
	}

	// Token: 0x06003E2A RID: 15914 RVA: 0x00129179 File Offset: 0x00127379
	private void OnMoveAllSimilarBtnInteractableChanged()
	{
		if (this.CharacterWindow != null)
		{
			this.CharacterWindow.RefreshGamepadTips();
		}
	}

	// Token: 0x06003E2B RID: 15915 RVA: 0x00129194 File Offset: 0x00127394
	public override List<LazyGameKeyTip> GetTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		UIItemCell uiitemCell;
		if (gamepadNavigationItem != null && gamepadNavigationItem.TryGetComponent<UIItemCell>(out uiitemCell) && uiitemCell != null)
		{
			if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress != null)
			{
				list.Add(LazyGameKeyTip.Select(true, true, true));
			}
			if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress2 != null)
			{
				list.Add(new LazyGameKeyTip(GameKey.ItemMove, "tip_item_action", true, true, true));
			}
		}
		if (this.multiInventoryWidget != null)
		{
			MultiInventoryWidgetData data = this.multiInventoryWidget.Data;
			if (data != null && data.WidgetSelectionMode == MultiInventoryWidgetMode.BagMode && this.multiInventoryWidget.IsMoveAllSimilarBtnInteractable)
			{
				list.Add(new LazyGameKeyTip(GameKey.MoveAllItemsFromPlayer, "tip_move_similar_items", true, true, true));
			}
		}
		return list;
	}

	// Token: 0x06003E2C RID: 15916 RVA: 0x00129286 File Offset: 0x00127486
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new CharMainPageWidgetData(MainGame.Instance.GameSave));
	}

	// Token: 0x040030E7 RID: 12519
	[Space]
	[SerializeField]
	private MultiInventoryWidget multiInventoryWidget;

	// Token: 0x040030E8 RID: 12520
	[SerializeField]
	private ToolBeltInventoryWidget toolBeltInventoryWidget;

	// Token: 0x040030E9 RID: 12521
	[SerializeField]
	private BagInventoryWidget bagInventoryWidget;

	// Token: 0x040030EA RID: 12522
	[SerializeField]
	private GameObject bagStateShading;

	// Token: 0x040030EB RID: 12523
	[SerializeField]
	private LazyButton closeBagButton;

	// Token: 0x040030EC RID: 12524
	[SerializeField]
	private PerksWidget perksWidget;

	// Token: 0x040030ED RID: 12525
	[SerializeField]
	private PerksWidget buffsWidget;

	// Token: 0x040030EE RID: 12526
	[SerializeField]
	private GameObject perksFooter;

	// Token: 0x040030EF RID: 12527
	[SerializeField]
	private MoneyWidget moneyWidget;

	// Token: 0x040030F0 RID: 12528
	[Space]
	[SerializeField]
	private List<TalentWidget> talentWidgets = new List<TalentWidget>();

	// Token: 0x040030F1 RID: 12529
	[SerializeField]
	private List<string> talents = new List<string>();

	// Token: 0x040030F2 RID: 12530
	[SerializeField]
	private RectTransform masteryLevelsHoverArea;

	// Token: 0x040030F3 RID: 12531
	private bool subscribed;
}
