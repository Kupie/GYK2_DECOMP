using System;

// Token: 0x020008EF RID: 2287
public class BodyPartsAndInstrumentsWidgetData : InventoryWidgetDataBase
{
	// Token: 0x06003BD2 RID: 15314 RVA: 0x0011DA2C File Offset: 0x0011BC2C
	public BodyPartsAndInstrumentsWidgetData(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default)
		: base(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState)
	{
	}
}
