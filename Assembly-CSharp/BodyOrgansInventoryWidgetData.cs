using System;

// Token: 0x020008ED RID: 2285
public class BodyOrgansInventoryWidgetData : InventoryWidgetDataBase
{
	// Token: 0x17000900 RID: 2304
	// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x0011D908 File Offset: 0x0011BB08
	// (set) Token: 0x06003BC7 RID: 15303 RVA: 0x0011D910 File Offset: 0x0011BB10
	public WgoData InteractionWgo { get; private set; }

	// Token: 0x17000901 RID: 2305
	// (get) Token: 0x06003BC8 RID: 15304 RVA: 0x0011D919 File Offset: 0x0011BB19
	// (set) Token: 0x06003BC9 RID: 15305 RVA: 0x0011D921 File Offset: 0x0011BB21
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x17000902 RID: 2306
	// (get) Token: 0x06003BCA RID: 15306 RVA: 0x0011D92A File Offset: 0x0011BB2A
	// (set) Token: 0x06003BCB RID: 15307 RVA: 0x0011D932 File Offset: 0x0011BB32
	public bool IsActive { get; private set; }

	// Token: 0x06003BCC RID: 15308 RVA: 0x0011D93C File Offset: 0x0011BB3C
	public BodyOrgansInventoryWidgetData()
		: this(Inventory.GetDefault(), null, null, null, null, null, null, null, ItemRelatedWidgetState.Default)
	{
	}

	// Token: 0x06003BCD RID: 15309 RVA: 0x0011D95C File Offset: 0x0011BB5C
	public BodyOrgansInventoryWidgetData(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default)
		: base(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState)
	{
	}

	// Token: 0x06003BCE RID: 15310 RVA: 0x0011D980 File Offset: 0x0011BB80
	public BodyOrgansInventoryWidgetData(bool isActive, WgoData wgoData, ZombieWgoData zombieWgoData, Inventory bodyItemInventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress = null)
		: this(bodyItemInventory, onItemCellOver, onItemCellOut, onItemCellPress, null, null, null, null, ItemRelatedWidgetState.Default)
	{
		this.InteractionWgo = wgoData;
		this.ZombieWgoData = zombieWgoData;
		this.IsActive = isActive;
	}

	// Token: 0x06003BCF RID: 15311 RVA: 0x0011D9B8 File Offset: 0x0011BBB8
	public bool HasAllMainOrgans()
	{
		for (int i = 0; i < LazyConsts.MAIN_ORGANS_TYPES.Count; i++)
		{
			if (!base.Inventory.Data.HasItemsByItemType(LazyConsts.MAIN_ORGANS_TYPES[i]))
			{
				return false;
			}
			if (base.Inventory.Data.GetItemByType(LazyConsts.MAIN_ORGANS_TYPES[i]).Definition.isOrganMistake)
			{
				return false;
			}
		}
		return true;
	}
}
