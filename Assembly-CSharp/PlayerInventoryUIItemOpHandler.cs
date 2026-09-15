using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008A9 RID: 2217
public class PlayerInventoryUIItemOpHandler
{
	// Token: 0x17000888 RID: 2184
	// (get) Token: 0x06003956 RID: 14678 RVA: 0x0011398D File Offset: 0x00111B8D
	public bool IsBagShown
	{
		get
		{
			return this.isBagShown;
		}
	}

	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06003957 RID: 14679 RVA: 0x00113995 File Offset: 0x00111B95
	public BagInventoryWidgetData BagInventoryWidgetData
	{
		get
		{
			return this.bagInventoryWidgetData;
		}
	}

	// Token: 0x1700088A RID: 2186
	// (get) Token: 0x06003958 RID: 14680 RVA: 0x0011399D File Offset: 0x00111B9D
	// (set) Token: 0x06003959 RID: 14681 RVA: 0x001139A5 File Offset: 0x00111BA5
	public Action<Item> OnBagHide
	{
		get
		{
			return this.onBagHide;
		}
		set
		{
			this.onBagHide = value;
		}
	}

	// Token: 0x1700088B RID: 2187
	// (get) Token: 0x0600395A RID: 14682 RVA: 0x001139AE File Offset: 0x00111BAE
	// (set) Token: 0x0600395B RID: 14683 RVA: 0x001139B6 File Offset: 0x00111BB6
	public Action<Item> OnBagShow
	{
		get
		{
			return this.onBagShow;
		}
		set
		{
			this.onBagShow = value;
		}
	}

	// Token: 0x0600395C RID: 14684 RVA: 0x001139BF File Offset: 0x00111BBF
	public PlayerInventoryUIItemOpHandler(PlayerData playerData)
	{
		this.playerData = playerData;
	}

	// Token: 0x0600395D RID: 14685 RVA: 0x001139CE File Offset: 0x00111BCE
	public PlayerInventoryUIItemOpHandler(PlayerData playerData, MultiInventoryWidgetData playerMultiInventoryWidgetData)
	{
		this.playerData = playerData;
		this.playerMultiInventoryWidgetData = playerMultiInventoryWidgetData;
	}

	// Token: 0x0600395E RID: 14686 RVA: 0x001139E4 File Offset: 0x00111BE4
	public void OnPlayerInvItemPressed(UIItemCell cell)
	{
		if (this.isBagShown)
		{
			if (!cell.DisplayingItem.IsBag)
			{
				new InventoryUIItemMoveOpHandler(() => this.playerMultiInventoryWidgetData.SelectedWidgetData.Inventory, () => this.bagInventoryWidgetData.Inventory, this.shownBag)
				{
					Silent = true
				}.OnInventory1ItemPress1(cell);
				return;
			}
			if (this.IsShownBag(cell.DisplayingItem))
			{
				this.HideBag();
				return;
			}
			this.ShowBag(cell);
			return;
		}
		else
		{
			if (cell.DisplayingItem.IsBag)
			{
				this.ShowBag(cell);
				return;
			}
			UIContextMenuWindowData uicontextMenuWindowData = new UIContextMenuWindowData();
			uicontextMenuWindowData.Position = (LazyInput.IsGamepadActive ? cell.transform.position : Input.mousePosition);
			uicontextMenuWindowData.Options = new List<UIContextMenuWindowWidgetData>();
			this.FillContextPressData(cell, cell.DisplayingItem, uicontextMenuWindowData);
			if (uicontextMenuWindowData.Options.Count > 0)
			{
				Action callback = uicontextMenuWindowData.Options[0].callback;
				if (callback == null)
				{
					return;
				}
				callback();
			}
			return;
		}
	}

	// Token: 0x0600395F RID: 14687 RVA: 0x00113AD8 File Offset: 0x00111CD8
	public void OnPlayerInvItemPressed2(UIItemCell cell)
	{
		Item displayingItem = cell.DisplayingItem;
		if (!this.isBagShown)
		{
			UIContextMenuWindowData uicontextMenuWindowData = new UIContextMenuWindowData();
			uicontextMenuWindowData.Position = (LazyInput.IsGamepadActive ? cell.transform.position : Input.mousePosition);
			uicontextMenuWindowData.Options = new List<UIContextMenuWindowWidgetData>();
			this.FillContextPressData(cell, displayingItem, uicontextMenuWindowData);
			uicontextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_destroy"), delegate
			{
				this.TryDestroyItem(cell);
				LazyUI.GetWindow<UIContextMenuWindow>().Close();
			}, !displayingItem.Definition.CanNotBeDestroyed));
			LazyAudio.PlayAndForget("gui_click");
			LazyUI.GetWindow<UIContextMenuWindow>().Open(uicontextMenuWindowData);
			return;
		}
		if (displayingItem.IsBag)
		{
			return;
		}
		new InventoryUIItemMoveOpHandler(() => this.playerMultiInventoryWidgetData.SelectedWidgetData.Inventory, () => this.bagInventoryWidgetData.Inventory, this.shownBag)
		{
			Silent = true
		}.OnInventory1ItemPress2(cell);
	}

	// Token: 0x06003960 RID: 14688 RVA: 0x00002318 File Offset: 0x00000518
	public void OnPlayerInventoryPressedDown(UIItemCell cell)
	{
	}

	// Token: 0x06003961 RID: 14689 RVA: 0x00113BE0 File Offset: 0x00111DE0
	private void FillContextPressData(UIItemCell cell, Item item, UIContextMenuWindowData contextMenuWindowData)
	{
		bool flag = item.Definition.CanItemBeEquipped();
		bool canBeUsed = item.Definition.CanBeUsed;
		bool flag2 = LazySingleton<FightingGameController>.Instance.CurrentFightState == FightState.Disabled;
		if (!flag)
		{
			if (item.IsBag)
			{
				contextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_open"), delegate
				{
					this.ShowBag(cell);
					LazyUI.GetWindow<UIContextMenuWindow>().Close();
				}, true));
			}
			else if (item.IsSeed)
			{
				contextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_plant"), delegate
				{
					this.TrySetInteractingItem(cell);
					LazyUI.GetWindow<UIContextMenuWindow>().Close();
				}, flag2));
			}
			else if (item.IsFertilizer)
			{
				contextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_fertilize"), delegate
				{
					this.TrySetInteractingItem(cell);
					LazyUI.GetWindow<UIContextMenuWindow>().Close();
				}, flag2));
			}
			else
			{
				contextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_use"), delegate
				{
					this.TryUseItem(cell);
					LazyUI.GetWindow<UIContextMenuWindow>().Close();
				}, canBeUsed));
			}
			if (item.Definition.CanBePinnedToHotBar)
			{
				contextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_pin_hot_bar"), delegate
				{
					this.TryPinItemToHotBar(cell);
					LazyUI.GetWindow<UIContextMenuWindow>().Close();
				}, true));
				return;
			}
		}
		else
		{
			contextMenuWindowData.Options.Add(new UIContextMenuWindowWidgetData(LLBase.L("ui_equip"), delegate
			{
				this.TryEquipItem(cell);
				LazyUI.GetWindow<UIContextMenuWindow>().Close();
			}, true));
		}
	}

	// Token: 0x06003962 RID: 14690 RVA: 0x00113D44 File Offset: 0x00111F44
	public bool PlayerItemsAvailabilityCondition(Item item)
	{
		return item == null || !this.isBagShown || item.IsBag || item.Definition.CanBeInsertedInBag(this.bagInventoryWidgetData.Inventory.Data.Definition);
	}

	// Token: 0x06003963 RID: 14691 RVA: 0x00113D7F File Offset: 0x00111F7F
	public bool ToolBeltItemsAvailabilityCondition(Item item)
	{
		return item == null || (!this.isBagShown && (LazySingleton<FightingGameController>.Instance.CurrentFightState == FightState.Disabled || !item.Definition.IsFightingEquipment()));
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x00113DAC File Offset: 0x00111FAC
	public void TryUnEquipItem(UIItemCell cell)
	{
		if (cell.DisplayingItem == null || cell.DisplayingItem.IsEmpty)
		{
			return;
		}
		UINotificator.isSilent = true;
		Inventory toolBeltInventory = MainGame.PlayerData.toolBeltInventory;
		Inventory inventory = MainGame.PlayerData.inventory;
		Item displayingItem = cell.DisplayingItem;
		if (inventory.CanAddItemToInventory(displayingItem))
		{
			toolBeltInventory.RemoveItemFromInventoryByUID(displayingItem, -1);
			inventory.AddItemToInventory(displayingItem, null, false);
			LazyAudio.PlayAndForget("unequip_tool");
		}
		UINotificator.isSilent = false;
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x00113E20 File Offset: 0x00112020
	public void TryEquipItem(UIItemCell cell)
	{
		if (cell.DisplayingItem == null || !cell.DisplayingItem.Definition.CanItemBeEquipped())
		{
			return;
		}
		UINotificator.isSilent = true;
		Inventory toolBeltInventory = this.playerData.toolBeltInventory;
		Inventory inventory = this.playerData.Inventory;
		Item itemByType = toolBeltInventory.GetItemByType(cell.DisplayingItem.Definition.type);
		Item displayingItem = cell.DisplayingItem;
		inventory.RemoveItemFromInventoryByUID(cell.DisplayingItem, -1);
		if (itemByType.IsEmpty)
		{
			toolBeltInventory.AddItemToInventory(displayingItem, null, false);
		}
		else
		{
			toolBeltInventory.RemoveItemFromInventoryByUID(itemByType, -1);
			toolBeltInventory.AddItemToInventory(displayingItem, null, false);
			inventory.AddItemToInventory(itemByType, null, false);
		}
		PlayerInventoryUIItemOpHandler.SyncFightEquipment(displayingItem.Definition);
		LazyAudio.PlayAndForget("equip_tool");
		UINotificator.isSilent = false;
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x00113EE0 File Offset: 0x001120E0
	private static void SyncFightEquipment(ItemDef equippedDef)
	{
		if (LazySingleton<FightingGameController>.Instance.CurrentFightState == FightState.Disabled)
		{
			return;
		}
		ItemType type = equippedDef.type;
		if (type != ItemType.Sword)
		{
			if (type == ItemType.BodyArmor)
			{
				MainGame.PlayerController.SetArmorView(true, null, true);
				return;
			}
			if (type != ItemType.Bow)
			{
				return;
			}
		}
		MainGame.PlayerController.AttackComponent.EquipWeapon(equippedDef);
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x00113F38 File Offset: 0x00112138
	private void TrySetInteractingItem(UIItemCell cell)
	{
		if (cell.DisplayingItem == null || cell.DisplayingItem.IsEmpty || (!cell.DisplayingItem.IsSeed && !cell.DisplayingItem.IsFertilizer))
		{
			return;
		}
		this.playerData.SetInteractingItem(cell.DisplayingItem);
		LazyAudio.PlayAndForget("equip_tool");
		LazyUI.GetWindow<CharacterWindow>().Close();
	}

	// Token: 0x06003968 RID: 14696 RVA: 0x00113F9A File Offset: 0x0011219A
	private void TryUseItem(UIItemCell cell)
	{
		if (cell.DisplayingItem == null || cell.DisplayingItem.IsEmpty || !cell.DisplayingItem.Definition.CanBeUsed)
		{
			return;
		}
		this.playerData.UseItem(cell.DisplayingItem);
	}

	// Token: 0x06003969 RID: 14697 RVA: 0x00113FD5 File Offset: 0x001121D5
	private void TryDestroyItem(UIItemCell cell)
	{
		if (cell.DisplayingItem == null || cell.DisplayingItem.Definition.CanNotBeDestroyed)
		{
			return;
		}
		this.playerData.Inventory.RemoveItemFromInventoryByUID(cell.DisplayingItem, -1);
	}

	// Token: 0x0600396A RID: 14698 RVA: 0x0011400C File Offset: 0x0011220C
	private bool TryPinItemToHotBar(UIItemCell cell)
	{
		if (cell.DisplayingItem == null || cell.DisplayingItem.IsEmpty || !cell.DisplayingItem.Definition.CanBePinnedToHotBar)
		{
			return false;
		}
		LazyUI.GetWindow<UIHotBarSelectionWindow>().Open(new UIHotBarSelectionWindowData(MainGame.Instance.GameSave, cell.DisplayingItem));
		return true;
	}

	// Token: 0x0600396B RID: 14699 RVA: 0x00114062 File Offset: 0x00112262
	public bool IsShownBag(Item item)
	{
		return this.isBagShown && item != null && this.shownBag != null && item.UniqueId == this.shownBag.UniqueId;
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x00114090 File Offset: 0x00112290
	private void ShowBag(UIItemCell cell)
	{
		this.isBagShown = true;
		this.shownBag = cell.DisplayingItem;
		Inventory bagInventory = ((this.playerMultiInventoryWidgetData != null) ? this.playerMultiInventoryWidgetData.FindBagInventory(cell.DisplayingItem) : null);
		if (bagInventory == null)
		{
			bagInventory = Inventory.GetInventoryFromBag(cell.DisplayingItem, this.playerData.Inventory);
		}
		else
		{
			bagInventory.ParentInventory = this.playerData.Inventory;
		}
		InventoryUIItemMoveOpHandler inventoryUIItemMoveOpHandler = new InventoryUIItemMoveOpHandler(() => this.playerMultiInventoryWidgetData.SelectedWidgetData.Inventory, () => bagInventory, cell.DisplayingItem);
		inventoryUIItemMoveOpHandler.Silent = true;
		this.bagInventoryWidgetData = new BagInventoryWidgetData(bagInventory, new InventoryHeaderWidgetData(bagInventory, "comm-header_2-type_icon-simple_bag", bagInventory.Data.id, true, null), null, null, new Action<UIItemCell>(inventoryUIItemMoveOpHandler.OnInventory2ItemPress1), new Action<UIItemCell>(inventoryUIItemMoveOpHandler.OnInventory2ItemPress2), null, null, null, ItemRelatedWidgetState.Default, null);
		this.bagInventoryWidgetData.ParentInventory = this.playerData.Inventory;
		Action<Item> action = this.onBagShow;
		if (action == null)
		{
			return;
		}
		action(this.shownBag);
	}

	// Token: 0x0600396D RID: 14701 RVA: 0x001141C4 File Offset: 0x001123C4
	public void HideBag()
	{
		if (this.isBagShown)
		{
			this.isBagShown = false;
			Action<Item> action = this.onBagHide;
			if (action == null)
			{
				return;
			}
			action(this.shownBag);
		}
	}

	// Token: 0x04002D7B RID: 11643
	private PlayerData playerData;

	// Token: 0x04002D7C RID: 11644
	private MultiInventoryWidgetData playerMultiInventoryWidgetData;

	// Token: 0x04002D7D RID: 11645
	private bool isBagShown;

	// Token: 0x04002D7E RID: 11646
	private BagInventoryWidgetData bagInventoryWidgetData;

	// Token: 0x04002D7F RID: 11647
	private Action<Item> onBagHide;

	// Token: 0x04002D80 RID: 11648
	private Action<Item> onBagShow;

	// Token: 0x04002D81 RID: 11649
	private Item shownBag;
}
