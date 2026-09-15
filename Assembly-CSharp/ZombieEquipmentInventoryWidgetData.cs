using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008FB RID: 2299
public class ZombieEquipmentInventoryWidgetData : InventoryWidgetDataBase
{
	// Token: 0x17000910 RID: 2320
	// (get) Token: 0x06003C1E RID: 15390 RVA: 0x0011F274 File Offset: 0x0011D474
	public ZombieWgoData ZombieWgoData
	{
		get
		{
			return this.zombie;
		}
	}

	// Token: 0x06003C1F RID: 15391 RVA: 0x0011F27C File Offset: 0x0011D47C
	public ZombieEquipmentInventoryWidgetData()
		: base(Inventory.GetDefault(), null, null, null, null, null, null, null, ItemRelatedWidgetState.Default)
	{
	}

	// Token: 0x06003C20 RID: 15392 RVA: 0x0011F29C File Offset: 0x0011D49C
	public ZombieEquipmentInventoryWidgetData(Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, Action<UIItemCell> onItemCellPress, Action<UIItemCell> onItemCellPress2, Action<UIItemCell> onItemCellDown, Func<Item, bool> itemsAvailableCondition = null, Func<Item, bool> customItemsNotShowCondition = null, ItemRelatedWidgetState widgetState = ItemRelatedWidgetState.Default)
		: base(inventory, onItemCellOver, onItemCellOut, onItemCellPress, onItemCellPress2, onItemCellDown, itemsAvailableCondition, customItemsNotShowCondition, widgetState)
	{
	}

	// Token: 0x06003C21 RID: 15393 RVA: 0x0011F2C0 File Offset: 0x0011D4C0
	public ZombieEquipmentInventoryWidgetData(ZombieWgoData wgoData, Inventory inventory, Action<UIItemCell> onItemCellOver, Action<UIItemCell> onItemCellOut, bool isBlocked = false)
		: this(inventory, onItemCellOver, onItemCellOut, null, null, null, null, null, ItemRelatedWidgetState.Default)
	{
		this.zombie = wgoData;
	}

	// Token: 0x06003C22 RID: 15394 RVA: 0x0011F2E4 File Offset: 0x0011D4E4
	public void OnArmorCellPressed(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem == null)
		{
			LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
			UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.InsertItem), new Func<Item, bool>(this.IsArmor), false, null, null);
			window.Open(uimultiInventoryWindowData);
			this.currentMultiInventory = uimultiInventoryWindowData.MultiInventory;
			return;
		}
		this.itemToReplace = itemCell.DisplayingItem;
		LazyWindow<UIMultiInventoryWindowData> window2 = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData2 = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.ReplaceItem), new Func<Item, bool>(this.IsArmor), false, null, null);
		window2.Open(uimultiInventoryWindowData2);
		this.currentMultiInventory = uimultiInventoryWindowData2.MultiInventory;
	}

	// Token: 0x06003C23 RID: 15395 RVA: 0x0011F380 File Offset: 0x0011D580
	public void OnArmorCellPressed2(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem != null)
		{
			Item displayingItem = itemCell.DisplayingItem;
			Item item = new Item(displayingItem.id, displayingItem.Count);
			if (this.zombie != null)
			{
				this.zombie.equippedArmor.SetGuid(SGuid.Empty);
			}
			base.Inventory.RemoveItemFromInventoryByUID(itemCell.DisplayingItem, -1);
			new MultiInventory(MainGame.PlayerData, true).AddItem(item);
			this.TryUpdateZombie();
		}
	}

	// Token: 0x06003C24 RID: 15396 RVA: 0x0011F3F8 File Offset: 0x0011D5F8
	public void OnInstrumentCellPressed(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem == null)
		{
			LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
			UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.InsertItem), new Func<Item, bool>(this.IsInstrumentOrWeapon), false, null, null);
			window.Open(uimultiInventoryWindowData);
			this.currentMultiInventory = uimultiInventoryWindowData.MultiInventory;
			return;
		}
		this.itemToReplace = itemCell.DisplayingItem;
		LazyWindow<UIMultiInventoryWindowData> window2 = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData2 = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.ReplaceItem), new Func<Item, bool>(this.IsInstrumentOrWeapon), false, null, null);
		window2.Open(uimultiInventoryWindowData2);
		this.currentMultiInventory = uimultiInventoryWindowData2.MultiInventory;
	}

	// Token: 0x06003C25 RID: 15397 RVA: 0x0011F494 File Offset: 0x0011D694
	public void OnInstrumentCellPressed2(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem != null)
		{
			Item displayingItem = itemCell.DisplayingItem;
			Item item = new Item(displayingItem.id, displayingItem.Count);
			if (this.zombie != null)
			{
				this.zombie.equippedHand.SetGuid(SGuid.Empty);
			}
			base.Inventory.RemoveItemFromInventoryByUID(itemCell.DisplayingItem, -1);
			new MultiInventory(MainGame.PlayerData, true).AddItem(item);
			this.TryUpdateZombie();
		}
	}

	// Token: 0x06003C26 RID: 15398 RVA: 0x0011F50C File Offset: 0x0011D70C
	public void OnCollarCellPressed(UIItemCell itemCell)
	{
		if (this.zombie == null || this.zombie.Collar.IsEmpty)
		{
			return;
		}
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.UpgradeCollar), new Func<Item, bool>(this.zombie.CanUpgradeCollarTo), false, "zombie_collar", null);
		window.Open(uimultiInventoryWindowData);
		this.currentMultiInventory = uimultiInventoryWindowData.MultiInventory;
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0011F57C File Offset: 0x0011D77C
	private void UpgradeCollar(UIItemCell itemCell)
	{
		Item displayingItem = itemCell.DisplayingItem;
		if (this.zombie != null && this.zombie.CanUpgradeCollarTo(displayingItem))
		{
			Item collar = this.zombie.Collar;
			List<Item> list;
			if (base.Inventory.TryAddItemToInventory(new Item(displayingItem.id, 1), out list, null, false))
			{
				this.zombie.equippedCollar.SetGuid(list[0].UniqueId);
				base.Inventory.RemoveItemFromInventoryByUID(collar, 1);
				this.currentMultiInventory.RemoveItemFromInventoryByUID(displayingItem, 1);
				base.Inventory.ForceTriggerOnItemsAddEventWithoutItems();
			}
		}
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x0011F61C File Offset: 0x0011D81C
	public void ReplaceItem(UIItemCell itemCell)
	{
		Item item = new Item(this.itemToReplace.id, this.itemToReplace.Count);
		Item item2 = new Item(itemCell.DisplayingItem.id, itemCell.DisplayingItem.Count);
		base.Inventory.RemoveItemFromInventoryByUID(this.itemToReplace, -1);
		this.currentMultiInventory.RemoveItemFromInventoryByUID(itemCell.DisplayingItem, -1);
		this.currentMultiInventory.AddItem(item);
		List<Item> list;
		if (base.Inventory.TryAddItemToInventory(item2, out list, null, false) && this.zombie != null)
		{
			Item item3 = list[0];
			if (this.IsArmor(item3))
			{
				this.zombie.equippedArmor.SetGuid(item3.UniqueId);
				base.Inventory.ForceTriggerOnItemsAddEventWithoutItems();
			}
			else if (this.IsInstrumentOrWeapon(item3))
			{
				this.zombie.equippedHand.SetGuid(item3.UniqueId);
				base.Inventory.ForceTriggerOnItemsAddEventWithoutItems();
			}
		}
		this.itemToReplace = null;
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		this.TryUpdateZombie();
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x0011F724 File Offset: 0x0011D924
	private void InsertItem(UIItemCell itemCell)
	{
		List<Item> list;
		if (base.Inventory.TryAddItemToInventory(itemCell.DisplayingItem, out list, null, false) && this.zombie != null)
		{
			Item item = list[0];
			if (this.IsArmor(item))
			{
				this.zombie.equippedArmor.SetGuid(item.UniqueId);
				base.Inventory.ForceTriggerOnItemsAddEventWithoutItems();
			}
			else if (this.IsInstrumentOrWeapon(item))
			{
				this.zombie.equippedHand.SetGuid(item.UniqueId);
				base.Inventory.ForceTriggerOnItemsAddEventWithoutItems();
			}
		}
		this.currentMultiInventory.RemoveItemFromInventoryByUID(itemCell.DisplayingItem, -1);
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		this.TryUpdateZombie();
	}

	// Token: 0x06003C2A RID: 15402 RVA: 0x0011F7D0 File Offset: 0x0011D9D0
	private void TryUpdateZombie()
	{
		if (this.zombie != null)
		{
			switch (this.zombie.ZombieType)
			{
			case ZombieType.Free:
			case ZombieType.Caretaker:
			case ZombieType.Porter:
				return;
			case ZombieType.Crafter:
				this.zombie.CrafterOnToolChanged();
				return;
			case ZombieType.ConveyorCrafter:
				this.zombie.ConveyorCrafterOnToolChanged();
				return;
			case ZombieType.Worker:
				this.zombie.CrafterOnToolChanged();
				return;
			case ZombieType.Gardener:
				this.zombie.GardenerOnToolChanged();
				return;
			case ZombieType.Fighter:
				this.zombie.FighterOnEquipmentChange();
				return;
			}
			this.zombie.FighterOnEquipmentChange();
		}
	}

	// Token: 0x06003C2B RID: 15403 RVA: 0x0011F864 File Offset: 0x0011DA64
	private bool IsInstrumentOrWeapon(Item item)
	{
		return item != null && (item.Definition.isTool || item.Definition.isWeapon) && item.Definition.type != ItemType.Sword;
	}

	// Token: 0x06003C2C RID: 15404 RVA: 0x0011F897 File Offset: 0x0011DA97
	private bool IsArmor(Item item)
	{
		return item != null && item.Definition.type == ItemType.BodyArmor;
	}

	// Token: 0x04002F50 RID: 12112
	private ZombieWgoData zombie;

	// Token: 0x04002F51 RID: 12113
	private MultiInventory currentMultiInventory;

	// Token: 0x04002F52 RID: 12114
	private Item itemToReplace;
}
