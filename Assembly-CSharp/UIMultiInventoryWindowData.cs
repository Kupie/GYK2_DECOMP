using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A02 RID: 2562
public class UIMultiInventoryWindowData : LazyWidgetDataBase
{
	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x06004506 RID: 17670 RVA: 0x001469A2 File Offset: 0x00144BA2
	// (set) Token: 0x06004507 RID: 17671 RVA: 0x001469AA File Offset: 0x00144BAA
	public MultiInventoryWidgetData MultiInventoryWidgetData { get; private set; }

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x06004508 RID: 17672 RVA: 0x001469B3 File Offset: 0x00144BB3
	// (set) Token: 0x06004509 RID: 17673 RVA: 0x001469BB File Offset: 0x00144BBB
	public MultiInventory MultiInventory { get; private set; }

	// Token: 0x0600450A RID: 17674 RVA: 0x001469C4 File Offset: 0x00144BC4
	public UIMultiInventoryWindowData(PlayerData playerData, Action<UIItemCell> onCellClicked, Func<Item, bool> itemsAvailableCondition = null, bool addCurrentPlayerWorldZone = true, string customHeaderId = null, Func<Item, string> extraRedTooltipLocIdProvider = null)
	{
		this.MultiInventoryWidgetData = new MultiInventoryWidgetData(MultiInventoryWidgetMode.Default);
		MultiInventory multiInventory = new MultiInventory(playerData, addCurrentPlayerWorldZone);
		List<InventoryWidgetDataBase> widgetsDataForMultiInventory = InventoryWidgetDataHelper.GetWidgetsDataForMultiInventory(multiInventory, delegate(UIItemCell _)
		{
			LazyAudio.PlayAndForget("gui_hover_light");
		}, null, delegate(UIItemCell item)
		{
			Action<UIItemCell> onCellClicked2 = onCellClicked;
			if (onCellClicked2 != null)
			{
				onCellClicked2(item);
			}
			LazyAudio.PlayAndForget("gui_click");
		}, null, null, itemsAvailableCondition, true, false, ItemRelatedWidgetState.Default, ItemRelatedWidgetState.Default);
		if (!string.IsNullOrEmpty(customHeaderId))
		{
			this.MultiInventoryWidgetData.HeaderLocaleId = customHeaderId;
		}
		if (extraRedTooltipLocIdProvider != null)
		{
			for (int i = 0; i < widgetsDataForMultiInventory.Count; i++)
			{
				widgetsDataForMultiInventory[i].ExtraRedTooltipLocIdProvider = extraRedTooltipLocIdProvider;
			}
		}
		this.MultiInventoryWidgetData.AddRange(widgetsDataForMultiInventory);
		this.MultiInventory = multiInventory;
	}
}
