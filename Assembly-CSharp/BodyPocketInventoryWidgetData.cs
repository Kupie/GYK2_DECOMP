using System;

// Token: 0x020008F1 RID: 2289
public class BodyPocketInventoryWidgetData : InventoryWidgetDataBase
{
	// Token: 0x17000904 RID: 2308
	// (get) Token: 0x06003BDC RID: 15324 RVA: 0x0011DF57 File Offset: 0x0011C157
	// (set) Token: 0x06003BDD RID: 15325 RVA: 0x0011DF5F File Offset: 0x0011C15F
	public WgoData InteractionWgo { get; private set; }

	// Token: 0x17000905 RID: 2309
	// (get) Token: 0x06003BDE RID: 15326 RVA: 0x0011DF68 File Offset: 0x0011C168
	// (set) Token: 0x06003BDF RID: 15327 RVA: 0x0011DF70 File Offset: 0x0011C170
	public ZombieWgoData ZombieWgoData { get; private set; }

	// Token: 0x17000906 RID: 2310
	// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x0011DF79 File Offset: 0x0011C179
	// (set) Token: 0x06003BE1 RID: 15329 RVA: 0x0011DF81 File Offset: 0x0011C181
	public bool IsActive { get; private set; }

	// Token: 0x17000907 RID: 2311
	// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x0011DF8A File Offset: 0x0011C18A
	// (set) Token: 0x06003BE3 RID: 15331 RVA: 0x0011DF92 File Offset: 0x0011C192
	public bool FirstEmptyIsInteractable { get; set; }

	// Token: 0x06003BE4 RID: 15332 RVA: 0x0011DF9C File Offset: 0x0011C19C
	public BodyPocketInventoryWidgetData()
		: this(Inventory.GetDefault(), null, null, null, null, null, null, null, ItemRelatedWidgetState.Default)
	{
	}

	// Token: 0x06003BE5 RID: 15333 RVA: 0x0011DFBC File Offset: 0x0011C1BC
	public BodyPocketInventoryWidgetData(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default)
		: base(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState)
	{
	}

	// Token: 0x06003BE6 RID: 15334 RVA: 0x0011DFE0 File Offset: 0x0011C1E0
	public BodyPocketInventoryWidgetData(bool isActive, WgoData wgoData, ZombieWgoData zombieWgoData, Inventory bodyItemInventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress = null)
		: this(bodyItemInventory, onItemCellOver, onItemCellOut, onItemCellPress, null, null, null, null, ItemRelatedWidgetState.Default)
	{
		this.InteractionWgo = wgoData;
		this.ZombieWgoData = zombieWgoData;
		this.IsActive = isActive;
	}
}
