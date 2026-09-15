using System;

// Token: 0x020008B0 RID: 2224
public class InventoryWidgetData : InventoryWidgetDataBase
{
	// Token: 0x17000890 RID: 2192
	// (get) Token: 0x060039A5 RID: 14757 RVA: 0x00114CCE File Offset: 0x00112ECE
	// (set) Token: 0x060039A6 RID: 14758 RVA: 0x00114CD6 File Offset: 0x00112ED6
	public Action<InventoryWidget> OnWidgetPressed { get; set; }

	// Token: 0x17000891 RID: 2193
	// (get) Token: 0x060039A7 RID: 14759 RVA: 0x00114CDF File Offset: 0x00112EDF
	// (set) Token: 0x060039A8 RID: 14760 RVA: 0x00114CE7 File Offset: 0x00112EE7
	public InventoryHeaderWidgetData InventoryHeaderWidgetData { get; set; }

	// Token: 0x060039A9 RID: 14761 RVA: 0x00114CF0 File Offset: 0x00112EF0
	public InventoryWidgetData(Inventory inventory, InventoryHeaderWidgetData inventoryHeaderWidgetData, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default, Action<InventoryWidget> onWidgetPressed = null)
		: base(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState)
	{
		this.InventoryHeaderWidgetData = inventoryHeaderWidgetData;
		this.OnWidgetPressed = onWidgetPressed;
	}
}
