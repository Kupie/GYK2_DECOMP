using System;
using LazyBearTechnology;

// Token: 0x020008B1 RID: 2225
public class InventoryWidgetDataBase : LazyWidgetDataBase
{
	// Token: 0x17000892 RID: 2194
	// (get) Token: 0x060039AA RID: 14762 RVA: 0x00114D22 File Offset: 0x00112F22
	// (set) Token: 0x060039AB RID: 14763 RVA: 0x00114D2A File Offset: 0x00112F2A
	public Inventory Inventory { get; protected set; }

	// Token: 0x17000893 RID: 2195
	// (get) Token: 0x060039AC RID: 14764 RVA: 0x00114D33 File Offset: 0x00112F33
	// (set) Token: 0x060039AD RID: 14765 RVA: 0x00114D3B File Offset: 0x00112F3B
	public Action<UIItemCell> OnItemCellOver { get; protected set; }

	// Token: 0x17000894 RID: 2196
	// (get) Token: 0x060039AE RID: 14766 RVA: 0x00114D44 File Offset: 0x00112F44
	// (set) Token: 0x060039AF RID: 14767 RVA: 0x00114D4C File Offset: 0x00112F4C
	public Action<UIItemCell> OnItemCellOut { get; protected set; }

	// Token: 0x17000895 RID: 2197
	// (get) Token: 0x060039B0 RID: 14768 RVA: 0x00114D55 File Offset: 0x00112F55
	// (set) Token: 0x060039B1 RID: 14769 RVA: 0x00114D5D File Offset: 0x00112F5D
	public Action<UIItemCell> OnItemCellPress { get; protected set; }

	// Token: 0x17000896 RID: 2198
	// (get) Token: 0x060039B2 RID: 14770 RVA: 0x00114D66 File Offset: 0x00112F66
	// (set) Token: 0x060039B3 RID: 14771 RVA: 0x00114D6E File Offset: 0x00112F6E
	public Action<UIItemCell> OnItemCellPress2 { get; protected set; }

	// Token: 0x17000897 RID: 2199
	// (get) Token: 0x060039B4 RID: 14772 RVA: 0x00114D77 File Offset: 0x00112F77
	// (set) Token: 0x060039B5 RID: 14773 RVA: 0x00114D7F File Offset: 0x00112F7F
	public Action<UIItemCell> OnItemCellDown { get; protected set; }

	// Token: 0x17000898 RID: 2200
	// (get) Token: 0x060039B6 RID: 14774 RVA: 0x00114D88 File Offset: 0x00112F88
	// (set) Token: 0x060039B7 RID: 14775 RVA: 0x00114D90 File Offset: 0x00112F90
	public Func<Item, bool> CustomItemsAvailableCondition { get; protected set; }

	// Token: 0x17000899 RID: 2201
	// (get) Token: 0x060039B8 RID: 14776 RVA: 0x00114D99 File Offset: 0x00112F99
	// (set) Token: 0x060039B9 RID: 14777 RVA: 0x00114DA1 File Offset: 0x00112FA1
	public Func<Item, bool> CustomItemsNotShowCondition { get; protected set; }

	// Token: 0x1700089A RID: 2202
	// (get) Token: 0x060039BA RID: 14778 RVA: 0x00114DAA File Offset: 0x00112FAA
	// (set) Token: 0x060039BB RID: 14779 RVA: 0x00114DB2 File Offset: 0x00112FB2
	public Func<Item, string> ExtraRedTooltipLocIdProvider { get; set; }

	// Token: 0x1700089B RID: 2203
	// (get) Token: 0x060039BC RID: 14780 RVA: 0x00114DBB File Offset: 0x00112FBB
	// (set) Token: 0x060039BD RID: 14781 RVA: 0x00114DC3 File Offset: 0x00112FC3
	public Func<Item, bool> CustomItemSelectedCondition { get; set; }

	// Token: 0x1700089C RID: 2204
	// (get) Token: 0x060039BE RID: 14782 RVA: 0x00114DCC File Offset: 0x00112FCC
	// (set) Token: 0x060039BF RID: 14783 RVA: 0x00114DD4 File Offset: 0x00112FD4
	public bool DrawEmptyCellsAsDisabledWhenUnavailable { get; set; }

	// Token: 0x1700089D RID: 2205
	// (get) Token: 0x060039C0 RID: 14784 RVA: 0x00114DDD File Offset: 0x00112FDD
	// (set) Token: 0x060039C1 RID: 14785 RVA: 0x00114DE5 File Offset: 0x00112FE5
	public ItemRelatedWidgetState ItemRelatedWidgetState
	{
		get
		{
			return this.currentWidgetState;
		}
		set
		{
			this.currentWidgetState = value;
		}
	}

	// Token: 0x060039C2 RID: 14786 RVA: 0x00114DF0 File Offset: 0x00112FF0
	public InventoryWidgetDataBase(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default)
	{
		this.Inventory = inventory;
		this.OnItemCellOver = onItemCellOver;
		this.OnItemCellOut = onItemCellOut;
		this.OnItemCellPress = onItemCellPress;
		this.OnItemCellPress2 = onItemCellPress2;
		this.OnItemCellDown = onItemCellDown;
		this.CustomItemsAvailableCondition = itemsAvailableCondition;
		this.CustomItemsNotShowCondition = customItemsNotShowCondition;
		this.ItemRelatedWidgetState = widgetState;
	}

	// Token: 0x060039C3 RID: 14787 RVA: 0x00114E48 File Offset: 0x00113048
	public void SetCustomOnCellPressedAction(Action<UIItemCell> action)
	{
		this.OnItemCellPress = action;
	}

	// Token: 0x060039C4 RID: 14788 RVA: 0x00114E51 File Offset: 0x00113051
	public void SetCustomOnCellPressed2Action(Action<UIItemCell> action)
	{
		this.OnItemCellPress2 = action;
	}

	// Token: 0x04002DA4 RID: 11684
	private ItemRelatedWidgetState currentWidgetState;
}
