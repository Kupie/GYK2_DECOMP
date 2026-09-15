using System;

// Token: 0x020008A1 RID: 2209
public class BagInventoryWidgetData : InventoryWidgetData
{
	// Token: 0x1700087D RID: 2173
	// (get) Token: 0x0600390E RID: 14606 RVA: 0x00112766 File Offset: 0x00110966
	// (set) Token: 0x0600390F RID: 14607 RVA: 0x0011276E File Offset: 0x0011096E
	public Inventory ParentInventory { get; set; }

	// Token: 0x06003910 RID: 14608 RVA: 0x00112778 File Offset: 0x00110978
	public BagInventoryWidgetData(Inventory inventory, InventoryHeaderWidgetData inventoryHeaderWidgetData, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default, Action<InventoryWidget> onWidgetPressed = null)
		: base(inventory, inventoryHeaderWidgetData, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState, onWidgetPressed)
	{
	}
}
