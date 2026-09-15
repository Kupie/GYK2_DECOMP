using System;
using System.Collections.Generic;

// Token: 0x020008B2 RID: 2226
public static class InventoryWidgetDataHelper
{
	// Token: 0x060039C5 RID: 14789 RVA: 0x00114E5C File Offset: 0x0011305C
	public static List<InventoryWidgetDataBase> GetWidgetsDataForInventory(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown = null, Func<Item, bool> itemsAvailableCondition = null, bool addBags = true, bool disableHeaderForFirstWidget = false, ItemRelatedWidgetState customState = ItemRelatedWidgetState.Default, ItemRelatedWidgetState customStateBags = ItemRelatedWidgetState.Default)
	{
		List<InventoryWidgetDataBase> list = new List<InventoryWidgetDataBase>();
		InventoryWidgetData inventoryWidgetData = new InventoryWidgetData(inventory, disableHeaderForFirstWidget ? new InventoryHeaderWidgetData() : new InventoryHeaderWidgetData(inventory, "comm-header_2-type_icon-main_inventory", "", true, null), onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, null, customState, null);
		list.Add(inventoryWidgetData);
		if (addBags)
		{
			foreach (Item item in inventory.Data.Inventory)
			{
				if (item.IsBag)
				{
					Inventory inventoryFromBag = Inventory.GetInventoryFromBag(item, inventory);
					list.Add(new BagInventoryWidgetData(inventoryFromBag, new InventoryHeaderWidgetData(inventoryFromBag, "comm-header_2-type_icon-simple_bag", inventoryFromBag.Data.id, true, null), onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, null, customStateBags, null)
					{
						ParentInventory = inventory
					});
				}
			}
		}
		return list;
	}

	// Token: 0x060039C6 RID: 14790 RVA: 0x00114F44 File Offset: 0x00113144
	public static List<InventoryWidgetDataBase> GetWidgetsDataForMultiInventory(MultiInventory multiInventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, bool addBags = true, bool disableHeaderForFirstWidget = false, ItemRelatedWidgetState customState = ItemRelatedWidgetState.Default, ItemRelatedWidgetState customStateBags = ItemRelatedWidgetState.Default)
	{
		List<InventoryWidgetDataBase> list = new List<InventoryWidgetDataBase>();
		foreach (Inventory inventory in multiInventory.inventoryList)
		{
			list.AddRange(InventoryWidgetDataHelper.GetWidgetsDataForInventory(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, addBags, disableHeaderForFirstWidget, customState, customStateBags));
			disableHeaderForFirstWidget = false;
		}
		return list;
	}
}
