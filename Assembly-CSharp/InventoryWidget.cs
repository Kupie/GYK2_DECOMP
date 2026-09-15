using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008AD RID: 2221
public class InventoryWidget : InventoryWidgetBase<InventoryWidgetData>
{
	// Token: 0x1700088C RID: 2188
	// (get) Token: 0x0600397E RID: 14718 RVA: 0x00114301 File Offset: 0x00112501
	private ItemRelatedWidgetState ItemRelatedWidgetState
	{
		get
		{
			if (this.data == null)
			{
				return ItemRelatedWidgetState.Default;
			}
			return this.data.ItemRelatedWidgetState;
		}
	}

	// Token: 0x1700088D RID: 2189
	// (get) Token: 0x0600397F RID: 14719 RVA: 0x00114318 File Offset: 0x00112518
	public virtual List<UIItemCell> Cells
	{
		get
		{
			return this.uiItemCells;
		}
	}

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x06003980 RID: 14720 RVA: 0x00114320 File Offset: 0x00112520
	// (remove) Token: 0x06003981 RID: 14721 RVA: 0x00114358 File Offset: 0x00112558
	public event Action onRedraw;

	// Token: 0x1700088E RID: 2190
	// (get) Token: 0x06003982 RID: 14722 RVA: 0x0011438D File Offset: 0x0011258D
	public InventoryHeaderWidget InventoryHeaderWidget
	{
		get
		{
			return this.inventoryHeaderWidget;
		}
	}

	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06003983 RID: 14723 RVA: 0x00114395 File Offset: 0x00112595
	public InventoryWidgetData Data
	{
		get
		{
			return this.data as InventoryWidgetData;
		}
	}

	// Token: 0x06003984 RID: 14724 RVA: 0x001143A4 File Offset: 0x001125A4
	public override void Init()
	{
		base.Init();
		this.inventoryHeaderWidget.Init();
		if (this.widgetBtn != null)
		{
			this.widgetBtn.onClick.RemoveAllListeners();
			this.widgetBtn.onClick.AddListener(new UnityAction(this.OnWidgetPress));
		}
	}

	// Token: 0x06003985 RID: 14725 RVA: 0x001143FC File Offset: 0x001125FC
	protected override void SetData(InventoryWidgetDataBase data)
	{
		this.UnsubscribeFromInventoryEvents();
		base.SetData(data);
		this.SubscribeToInventoryEvents();
	}

	// Token: 0x06003986 RID: 14726 RVA: 0x00114414 File Offset: 0x00112614
	public override void Redraw()
	{
		string viewId = this.Data.Inventory.ViewId;
		base.Redraw();
		int num = this.data.Inventory.Data.InventorySize;
		while (this.uiItemCells.Count > num)
		{
			int num2 = this.uiItemCells.Count - 1;
			UIItemCell uiitemCell = this.uiItemCells[num2];
			uiitemCell.Flush(true);
			this.ReleaseCell(uiitemCell);
			this.uiItemCells.RemoveAt(num2);
		}
		int count = this.uiItemCells.Count;
		while (num - count > 0)
		{
			this.uiItemCells.Add(this.GetNewCell());
			num--;
		}
		int inventoryFillSize = this.data.Inventory.Data.InventoryFillSize;
		for (int i = 0; i < inventoryFillSize; i++)
		{
			Item item = this.data.Inventory.Data.Inventory[i];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(item);
			UIItemCell uiitemCell2 = this.uiItemCells[i];
			Func<Item, string> extraRedTooltipLocIdProvider = this.data.ExtraRedTooltipLocIdProvider;
			uiitemCell2.ExtraRedTooltipLocId = ((extraRedTooltipLocIdProvider != null) ? extraRedTooltipLocIdProvider(item) : null);
			ItemRelatedWidgetState cellRelatedWidgetState = this.GetCellRelatedWidgetState(item, flag);
			this.uiItemCells[i].Draw(item, false, -1, false, 1, cellRelatedWidgetState == ItemRelatedWidgetState.Disabled, 0, true, false, false, cellRelatedWidgetState, false);
			this.uiItemCells[i].OnItemCellOver = this.onItemCellOver;
			this.uiItemCells[i].OnItemCellOut = this.onItemCellOut;
			this.uiItemCells[i].OnItemCellPress = this.onItemCellPress;
			this.uiItemCells[i].OnItemCellPress2 = this.onItemCellPress2;
			this.uiItemCells[i].OnItemCellDown = this.onItemCellDown;
			this.uiItemCells[i].OnWidgetPress = new Action(this.OnWidgetPress);
			this.uiItemCells[i].gameObject.SetActive(true);
		}
		for (int j = inventoryFillSize; j < this.data.Inventory.Data.InventorySize; j++)
		{
			bool flag2 = this.data.CustomItemsAvailableCondition != null && this.data.CustomItemsAvailableCondition(null);
			ItemRelatedWidgetState itemRelatedWidgetState = ((this.data.DrawEmptyCellsAsDisabledWhenUnavailable && !flag2) ? ItemRelatedWidgetState.Disabled : this.data.ItemRelatedWidgetState);
			this.uiItemCells[j].DrawEmptyWithState(itemRelatedWidgetState, !flag2, false);
			this.uiItemCells[j].OnWidgetPress = new Action(this.OnWidgetPress);
			this.uiItemCells[j].gameObject.SetActive(true);
		}
		for (int k = 0; k < this.uiItemCells.Count; k++)
		{
			if (this.uiItemCells[k].gameObject.activeSelf && this.uiItemCells[k].DisplayingItem != null && !this.uiItemCells[k].DisplayingItem.IsEmpty && this.data.CustomItemsNotShowCondition != null && this.data.CustomItemsNotShowCondition(this.uiItemCells[k].DisplayingItem))
			{
				this.uiItemCells[k].gameObject.SetActive(false);
			}
		}
		this.inventoryHeaderWidget.Draw(this.Data.InventoryHeaderWidgetData);
		this.UpdateItemRelatedWidgetStateForWidget();
		this.OnRedraw();
	}

	// Token: 0x06003987 RID: 14727 RVA: 0x001147C8 File Offset: 0x001129C8
	public override void Hide()
	{
		base.Hide();
		foreach (UIItemCell uiitemCell in this.uiItemCells)
		{
			uiitemCell.Flush(true);
			this.ReleaseCell(uiitemCell);
		}
		this.uiItemCells.Clear();
		this.ChangeMoveAllBtnState(false, null, null);
	}

	// Token: 0x06003988 RID: 14728 RVA: 0x0011483C File Offset: 0x00112A3C
	public override void UpdateItemRelatedWidgetStateForCells()
	{
		for (int i = 0; i < this.uiItemCells.Count; i++)
		{
			UIItemCell uiitemCell = this.uiItemCells[i];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(uiitemCell.DisplayingItem);
			uiitemCell.SetWidgetState(this.GetCellRelatedWidgetState(uiitemCell.DisplayingItem, flag));
		}
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x001148A6 File Offset: 0x00112AA6
	private ItemRelatedWidgetState GetCellRelatedWidgetState(Item item, bool drawAsInteractable)
	{
		if (!drawAsInteractable)
		{
			return ItemRelatedWidgetState.Disabled;
		}
		if (item != null && !item.IsEmpty && this.data.CustomItemSelectedCondition != null && this.data.CustomItemSelectedCondition(item))
		{
			return ItemRelatedWidgetState.Selected;
		}
		return this.data.ItemRelatedWidgetState;
	}

	// Token: 0x0600398A RID: 14730 RVA: 0x001148E8 File Offset: 0x00112AE8
	public override void UpdateItemRelatedWidgetStateForWidget()
	{
		if (this.data.ItemRelatedWidgetState == ItemRelatedWidgetState.Default || this.data.ItemRelatedWidgetState == ItemRelatedWidgetState.Selected)
		{
			this.background.sprite = this.backgroundActiveSprite;
			this.inventoryHeaderWidget.ChangeActiveViewState(true);
			return;
		}
		this.background.sprite = this.backgroundInactiveSprite;
		this.inventoryHeaderWidget.ChangeActiveViewState(false);
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x0011494C File Offset: 0x00112B4C
	protected override void ClearCallbacks()
	{
		this.UnsubscribeFromInventoryEvents();
		base.ClearCallbacks();
		foreach (UIItemCell uiitemCell in this.uiItemCells)
		{
			uiitemCell.ClearCallbacks();
		}
	}

	// Token: 0x0600398C RID: 14732 RVA: 0x001149A8 File Offset: 0x00112BA8
	protected virtual UIItemCell GetNewCell()
	{
		return UIPrefabsPooler.Instance.GetElementFromPool<UIItemCell>(this.grid.transform);
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x001149BF File Offset: 0x00112BBF
	protected virtual void ReleaseCell(UIItemCell cell)
	{
		UIPrefabsPooler.Instance.ReleaseElementToPool<UIItemCell>(cell);
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x001149CC File Offset: 0x00112BCC
	private void Awake()
	{
		this.Init();
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x001149D4 File Offset: 0x00112BD4
	public virtual void UpdatePrices(InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate priceDelegate, int countModificator)
	{
		if (priceDelegate == null)
		{
			return;
		}
		foreach (UIItemCell uiitemCell in this.uiItemCells)
		{
			if (uiitemCell.DisplayingItem == null || uiitemCell.DisplayingItem.IsEmpty)
			{
				uiitemCell.ClearPriceLabel();
			}
			else
			{
				uiitemCell.UpdatePriceLabel(priceDelegate(uiitemCell.DisplayingItem, countModificator));
			}
		}
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x00114A54 File Offset: 0x00112C54
	public void UpdateHappinessStatusIcons(Vendor vendor, Func<string, int> getExtraSoldCount = null, float extraUsedHappiness = 0f)
	{
		foreach (UIItemCell uiitemCell in this.Cells)
		{
			int num = 0;
			if (getExtraSoldCount != null && uiitemCell.DisplayingItem != null)
			{
				num = getExtraSoldCount(uiitemCell.DisplayingItem.id);
			}
			uiitemCell.UpdateHappinessStatusIcons(vendor, num, extraUsedHappiness);
		}
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x00114AC8 File Offset: 0x00112CC8
	public void OnRedraw()
	{
		if (this.doNotTriggerOnRedraw)
		{
			return;
		}
		Action action = this.onRedraw;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x00114AE3 File Offset: 0x00112CE3
	public void ChangeMoveAllBtnState(bool isActive, Action onPress = null, Func<Inventory> getTargetInventory = null)
	{
		this.inventoryHeaderWidget.SetMoveAllSimilarBtnState(isActive, onPress, getTargetInventory);
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x00114AF3 File Offset: 0x00112CF3
	private void OnWidgetPress()
	{
		if (this.Data.ItemRelatedWidgetState != ItemRelatedWidgetState.Disabled)
		{
			Action<InventoryWidget> onWidgetPressed = this.Data.OnWidgetPressed;
			if (onWidgetPressed == null)
			{
				return;
			}
			onWidgetPressed(this);
		}
	}

	// Token: 0x06003994 RID: 14740 RVA: 0x00114B1C File Offset: 0x00112D1C
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new InventoryWidgetDataBase(MainGame.PlayerData.Inventory, null, null, null, null, null, null, null, ItemRelatedWidgetState.Default));
	}

	// Token: 0x04002D88 RID: 11656
	[SerializeField]
	protected InventoryHeaderWidget inventoryHeaderWidget;

	// Token: 0x04002D89 RID: 11657
	[SerializeField]
	protected Image background;

	// Token: 0x04002D8A RID: 11658
	[SerializeField]
	protected LazyButton widgetBtn;

	// Token: 0x04002D8B RID: 11659
	[SerializeField]
	protected GridLayoutGroup grid;

	// Token: 0x04002D8C RID: 11660
	[SerializeField]
	private Sprite backgroundActiveSprite;

	// Token: 0x04002D8D RID: 11661
	[SerializeField]
	private Sprite backgroundInactiveSprite;

	// Token: 0x04002D8E RID: 11662
	protected List<UIItemCell> uiItemCells = new List<UIItemCell>();

	// Token: 0x04002D90 RID: 11664
	public bool doNotTriggerOnRedraw;
}
