using System;
using LazyBearTechnology;

// Token: 0x0200096B RID: 2411
public class UIBaseChestWindowData : LazyWidgetDataBase
{
	// Token: 0x1700098E RID: 2446
	// (get) Token: 0x06003F86 RID: 16262 RVA: 0x00130798 File Offset: 0x0012E998
	// (set) Token: 0x06003F87 RID: 16263 RVA: 0x001307A0 File Offset: 0x0012E9A0
	public WgoData Chest { get; private set; }

	// Token: 0x1700098F RID: 2447
	// (get) Token: 0x06003F88 RID: 16264 RVA: 0x001307A9 File Offset: 0x0012E9A9
	// (set) Token: 0x06003F89 RID: 16265 RVA: 0x001307B1 File Offset: 0x0012E9B1
	public MultiInventoryWidgetData FirstMultiInventoryData { get; private set; }

	// Token: 0x17000990 RID: 2448
	// (get) Token: 0x06003F8A RID: 16266 RVA: 0x001307BA File Offset: 0x0012E9BA
	// (set) Token: 0x06003F8B RID: 16267 RVA: 0x001307C2 File Offset: 0x0012E9C2
	public MultiInventoryWidgetData SecondMultiInventoryData { get; private set; }

	// Token: 0x17000991 RID: 2449
	// (get) Token: 0x06003F8C RID: 16268 RVA: 0x001307CB File Offset: 0x0012E9CB
	// (set) Token: 0x06003F8D RID: 16269 RVA: 0x001307D3 File Offset: 0x0012E9D3
	public Action OnMoveAllSimilarItemFromPlayerToChest { get; private set; }

	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x06003F8E RID: 16270 RVA: 0x001307DC File Offset: 0x0012E9DC
	// (set) Token: 0x06003F8F RID: 16271 RVA: 0x001307E4 File Offset: 0x0012E9E4
	public MoneyWidgetData MoneyWidgetData { get; private set; }

	// Token: 0x06003F90 RID: 16272 RVA: 0x001307F0 File Offset: 0x0012E9F0
	public UIBaseChestWindowData(Inventory playerInventory, MultiInventory worldZoneMultiInventory, WgoData wgoData)
	{
		UIBaseChestWindowData.<>c__DisplayClass22_0 CS$<>8__locals1 = new UIBaseChestWindowData.<>c__DisplayClass22_0();
		CS$<>8__locals1.playerInventory = playerInventory;
		base..ctor();
		CS$<>8__locals1.<>4__this = this;
		UIBaseChestWindowData.<>c__DisplayClass22_1 CS$<>8__locals2 = new UIBaseChestWindowData.<>c__DisplayClass22_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		this.Chest = wgoData;
		this.playerInventory = CS$<>8__locals2.CS$<>8__locals1.playerInventory;
		this.wgoInventory = wgoData.Inventory;
		this.FirstMultiInventoryData = new MultiInventoryWidgetData(MultiInventoryWidgetMode.SelectionWithMoveBtn);
		this.SecondMultiInventoryData = new MultiInventoryWidgetData(MultiInventoryWidgetMode.SelectionWithoutMoveBtn);
		CS$<>8__locals2.moveOpHandler = new InventoryUIItemMoveOpHandler(() => CS$<>8__locals2.CS$<>8__locals1.<>4__this.FirstMultiInventoryData.SelectedWidgetData.Inventory, () => CS$<>8__locals2.CS$<>8__locals1.<>4__this.SecondMultiInventoryData.SelectedWidgetData.Inventory);
		this.FirstMultiInventoryData.AddRange(InventoryWidgetDataHelper.GetWidgetsDataForInventory(CS$<>8__locals2.CS$<>8__locals1.playerInventory, delegate(UIItemCell _)
		{
			LazyAudio.PlayAndForget("gui_hover_light");
		}, null, new Action<UIItemCell>(CS$<>8__locals2.moveOpHandler.OnInventory1ItemPress1), new Action<UIItemCell>(CS$<>8__locals2.moveOpHandler.OnInventory1ItemPress2), null, new Func<Item, bool>(this.ItemAvailabilityConditionLeft), true, false, ItemRelatedWidgetState.Default, ItemRelatedWidgetState.Inactive));
		if (worldZoneMultiInventory != null)
		{
			this.FirstMultiInventoryData.AddRange(InventoryWidgetDataHelper.GetWidgetsDataForMultiInventory(worldZoneMultiInventory, delegate(UIItemCell _)
			{
				LazyAudio.PlayAndForget("gui_hover_light");
			}, null, null, null, null, (Item _) => false, true, false, ItemRelatedWidgetState.Disabled, ItemRelatedWidgetState.Disabled));
		}
		this.SecondMultiInventoryData.AddRange(InventoryWidgetDataHelper.GetWidgetsDataForInventory(this.wgoInventory, delegate(UIItemCell _)
		{
			LazyAudio.PlayAndForget("gui_hover_light");
		}, null, new Action<UIItemCell>(CS$<>8__locals2.moveOpHandler.OnInventory2ItemPress1), new Action<UIItemCell>(CS$<>8__locals2.moveOpHandler.OnInventory2ItemPress2), null, new Func<Item, bool>(this.ItemAvailabilityConditionRight), true, false, ItemRelatedWidgetState.Default, ItemRelatedWidgetState.Default));
		InventoryWidgetData inventoryWidgetData = this.SecondMultiInventoryData.inventoriesData[0] as InventoryWidgetData;
		if (inventoryWidgetData != null)
		{
			inventoryWidgetData.InventoryHeaderWidgetData.HeaderIconId = "comm-header_2-type_icon-simple_chest";
		}
		this.OnMoveAllSimilarItemFromPlayerToChest = delegate
		{
			CS$<>8__locals2.CS$<>8__locals1.<>4__this.SecondMultiInventoryData.SelectedWidgetData.Inventory.TakeAllItemsExistingInMeFromOtherInventory(CS$<>8__locals2.CS$<>8__locals1.<>4__this.FirstMultiInventoryData.SelectedWidgetData.Inventory, true, true);
		};
		this.FirstMultiInventoryData.OnMoveAllSimilarPressed = this.OnMoveAllSimilarItemFromPlayerToChest;
		this.FirstMultiInventoryData.GetMoveAllSimilarTargetInventory = delegate
		{
			InventoryWidgetDataBase selectedWidgetData = CS$<>8__locals2.CS$<>8__locals1.<>4__this.SecondMultiInventoryData.SelectedWidgetData;
			if (selectedWidgetData == null)
			{
				return null;
			}
			return selectedWidgetData.Inventory;
		};
		this.SecondMultiInventoryData.OnSelectedWidgetChanged = delegate
		{
			Action onMoveAllSimilarTargetChanged = CS$<>8__locals2.CS$<>8__locals1.<>4__this.FirstMultiInventoryData.OnMoveAllSimilarTargetChanged;
			if (onMoveAllSimilarTargetChanged == null)
			{
				return;
			}
			onMoveAllSimilarTargetChanged();
		};
		CS$<>8__locals2.CS$<>8__locals1.playerInventory.OnBagRemoved += this.FirstMultiInventoryData.OnBagRemoved;
		this.wgoInventory.OnBagRemoved += this.SecondMultiInventoryData.OnBagRemoved;
		CS$<>8__locals2.CS$<>8__locals1.playerInventory.OnBagAdded += CS$<>8__locals2.<.ctor>g__OnBagAddedToPlayer|9;
		this.wgoInventory.OnBagAdded += CS$<>8__locals2.<.ctor>g__OnBagAddedToWgo|10;
		this.FirstMultiInventoryData.onWidgetHide = delegate
		{
			CS$<>8__locals2.CS$<>8__locals1.playerInventory.OnBagRemoved -= CS$<>8__locals2.CS$<>8__locals1.<>4__this.FirstMultiInventoryData.OnBagRemoved;
			CS$<>8__locals2.CS$<>8__locals1.playerInventory.OnBagAdded -= base.<.ctor>g__OnBagAddedToPlayer|9;
		};
		this.SecondMultiInventoryData.onWidgetHide = delegate
		{
			CS$<>8__locals2.CS$<>8__locals1.<>4__this.wgoInventory.OnBagRemoved -= CS$<>8__locals2.CS$<>8__locals1.<>4__this.SecondMultiInventoryData.OnBagRemoved;
			CS$<>8__locals2.CS$<>8__locals1.<>4__this.wgoInventory.OnBagAdded -= base.<.ctor>g__OnBagAddedToWgo|10;
		};
		this.MoneyWidgetData = new MoneyWidgetData();
		this.MoneyWidgetData.Money = () => MainGame.PlayerData.GetResInt("money");
	}

	// Token: 0x06003F91 RID: 16273 RVA: 0x00130B1C File Offset: 0x0012ED1C
	private bool ItemAvailabilityConditionLeft(Item item)
	{
		BlackListFilterSerializedItemProperty blackListFilterSerializedItemProperty;
		WhiteListFilterSerializedItemProperty whiteListFilterSerializedItemProperty;
		return item == null || ((!item.Definition.isBag || !this.SecondMultiInventoryData.SelectedWidgetData.Inventory.Data.IsBag) && (!this.wgoInventory.Data.TryGetProperty<BlackListFilterSerializedItemProperty>(out blackListFilterSerializedItemProperty) || !blackListFilterSerializedItemProperty.BlackList.Contains(item.Definition)) && (!this.wgoInventory.Data.TryGetProperty<WhiteListFilterSerializedItemProperty>(out whiteListFilterSerializedItemProperty) || whiteListFilterSerializedItemProperty.WhiteList.IsEmpty || whiteListFilterSerializedItemProperty.WhiteList.Contains(item.Definition)));
	}

	// Token: 0x06003F92 RID: 16274 RVA: 0x00130BB9 File Offset: 0x0012EDB9
	private bool ItemAvailabilityConditionRight(Item item)
	{
		return item == null || !item.Definition.isBag || !this.FirstMultiInventoryData.SelectedWidgetData.Inventory.Data.IsBag;
	}

	// Token: 0x06003F93 RID: 16275 RVA: 0x00130BEC File Offset: 0x0012EDEC
	private void OnBagAdded(Item bag, MultiInventoryWidgetData targetData, Inventory parentInventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Func<Item, bool> itemsAvailableCondition = null)
	{
		Inventory inventoryFromBag = Inventory.GetInventoryFromBag(bag, parentInventory);
		targetData.OnBagAdded(new BagInventoryWidgetData(inventoryFromBag, new InventoryHeaderWidgetData(inventoryFromBag, "comm-header_2-type_icon-simple_bag", inventoryFromBag.Data.id, true, null), onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, null, itemsAvailableCondition, null, ItemRelatedWidgetState.Default, null)
		{
			ParentInventory = parentInventory,
			ItemRelatedWidgetState = ItemRelatedWidgetState.Inactive
		});
	}

	// Token: 0x04003210 RID: 12816
	private Inventory playerInventory;

	// Token: 0x04003211 RID: 12817
	private Inventory wgoInventory;
}
