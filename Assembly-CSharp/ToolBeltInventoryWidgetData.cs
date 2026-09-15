using System;

// Token: 0x020008B8 RID: 2232
public class ToolBeltInventoryWidgetData : InventoryWidgetDataBase
{
	// Token: 0x06003A0A RID: 14858 RVA: 0x00116020 File Offset: 0x00114220
	public ToolBeltInventoryWidgetData(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default)
		: base(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState)
	{
	}
}
