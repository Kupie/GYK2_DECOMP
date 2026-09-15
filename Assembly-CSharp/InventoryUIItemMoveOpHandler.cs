using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public class InventoryUIItemMoveOpHandler
{
	// Token: 0x17000887 RID: 2183
	// (get) Token: 0x06003948 RID: 14664 RVA: 0x00113472 File Offset: 0x00111672
	// (set) Token: 0x06003949 RID: 14665 RVA: 0x0011347A File Offset: 0x0011167A
	public bool Silent
	{
		get
		{
			return this.silent;
		}
		set
		{
			this.silent = value;
		}
	}

	// Token: 0x0600394A RID: 14666 RVA: 0x00113483 File Offset: 0x00111683
	public InventoryUIItemMoveOpHandler(Func<Inventory> inv1, Func<Inventory> inv2)
	{
		this.inv1 = inv1;
		this.inv2 = inv2;
	}

	// Token: 0x0600394B RID: 14667 RVA: 0x00113499 File Offset: 0x00111699
	public InventoryUIItemMoveOpHandler(Func<Inventory> inv1, Func<Inventory> inv2, Item ignoreBagWhenAdd)
	{
		this.inv1 = inv1;
		this.inv2 = inv2;
		this.ignoreBagWhenAdd = ignoreBagWhenAdd;
	}

	// Token: 0x0600394C RID: 14668 RVA: 0x001134B8 File Offset: 0x001116B8
	public void OnInventory1ItemPress1(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem == null)
		{
			return;
		}
		if (itemCell.DisplayingItem.Count <= 1)
		{
			this.OnInventory1ItemPress2(itemCell);
			return;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			if (this.TryMoveItem(itemCell, itemCell.DisplayingItem.Count, this.inv1(), this.inv2()))
			{
				LazyAudio.PlayAndForget("item_put");
			}
			return;
		}
		if (this.TryMoveHalfStackOnCtrlClick(itemCell, this.inv1(), this.inv2()))
		{
			return;
		}
		this.OpenItemCountWindow(itemCell, this.inv1(), this.inv2());
	}

	// Token: 0x0600394D RID: 14669 RVA: 0x0011356E File Offset: 0x0011176E
	public void OnInventory1ItemPress2(UIItemCell itemCell)
	{
		if (this.TryMoveItem(itemCell, 1, this.inv1(), this.inv2()))
		{
			LazyAudio.PlayAndForget("item_put");
		}
	}

	// Token: 0x0600394E RID: 14670 RVA: 0x0011359C File Offset: 0x0011179C
	public void OnInventory2ItemPress1(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem == null)
		{
			return;
		}
		if (itemCell.DisplayingItem.Count <= 1)
		{
			this.OnInventory2ItemPress2(itemCell);
			return;
		}
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			if (this.TryMoveItem(itemCell, itemCell.DisplayingItem.Count, this.inv2(), this.inv1()))
			{
				LazyAudio.PlayAndForget("item_put");
			}
			return;
		}
		if (this.TryMoveHalfStackOnCtrlClick(itemCell, this.inv2(), this.inv1()))
		{
			return;
		}
		this.OpenItemCountWindow(itemCell, this.inv2(), this.inv1());
	}

	// Token: 0x0600394F RID: 14671 RVA: 0x00113652 File Offset: 0x00111852
	public void OnInventory2ItemPress2(UIItemCell itemCell)
	{
		if (this.TryMoveItem(itemCell, 1, this.inv2(), this.inv1()))
		{
			LazyAudio.PlayAndForget("item_put");
		}
	}

	// Token: 0x06003950 RID: 14672 RVA: 0x00113680 File Offset: 0x00111880
	private bool TryMoveHalfStackOnCtrlClick(UIItemCell itemCell, Inventory from, Inventory to)
	{
		if (LazyInput.IsGamepadActive)
		{
			return false;
		}
		if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
		{
			return false;
		}
		if (this.TryMoveItem(itemCell, itemCell.DisplayingItem.Count / 2, from, to))
		{
			LazyAudio.PlayAndForget("item_put");
		}
		return true;
	}

	// Token: 0x06003951 RID: 14673 RVA: 0x001136D4 File Offset: 0x001118D4
	private void OpenItemCountWindow(UIItemCell itemCell, Inventory from, Inventory to)
	{
		if (itemCell.DisplayingItem == null)
		{
			return;
		}
		int totalCountInInventory = from.Data.GetTotalCountInInventory(itemCell.DisplayingItem.id, itemCell.DisplayingItem.IsBag ? null : this.ignoreBagWhenAdd, true);
		int num = to.Data.CanAddItemCountToInventory(itemCell.DisplayingItem.Definition, totalCountInInventory, true, itemCell.DisplayingItem.IsBag ? null : this.ignoreBagWhenAdd, false);
		if (num > 0)
		{
			LazyAudio.PlayAndForget("item_put");
			UIItemCountWindowData uiitemCountWindowData = new UIItemCountWindowData();
			uiitemCountWindowData.Item = new Item(itemCell.DisplayingItem.id, 1);
			uiitemCountWindowData.Min = 1;
			uiitemCountWindowData.Max = num;
			uiitemCountWindowData.OnConfirm = delegate(int count)
			{
				this.TryMoveItem(itemCell, count, from, to);
			};
			uiitemCountWindowData.IsForVendor = false;
			uiitemCountWindowData.OkBtnData = new UIDialogWindowData.ButtonData(null, LLBase.L("btn_ok"), null, true, GameKey.Select, "");
			uiitemCountWindowData.BackBtnData = new UIDialogWindowData.ButtonData(null, LLBase.L("btn_cancel"), null, true, GameKey.Back, "");
			LazyUI.GetWindow<UIItemCountWindow>().Open(uiitemCountWindowData);
			return;
		}
		if (!this.silent && to == MainGame.PlayerData.Inventory)
		{
			LazySingleton<UINotificator>.Instance.HandleInventoryFull();
		}
	}

	// Token: 0x06003952 RID: 14674 RVA: 0x0011385C File Offset: 0x00111A5C
	private bool TryMoveItem(UIItemCell itemCell, int count, Inventory from, Inventory to)
	{
		if (itemCell.DisplayingItem == null)
		{
			return false;
		}
		if (this.silent)
		{
			UINotificator.isSilent = true;
		}
		bool flag = false;
		List<Item> list;
		if (itemCell.DisplayingItem.IsBag)
		{
			if (to.AddItemToInventory(Item.Copy(itemCell.DisplayingItem), null, false))
			{
				from.RemoveItemFromInventoryByUID(itemCell.DisplayingItem, count);
				flag = true;
			}
		}
		else if (to.AddItemToInventory(new Item(itemCell.DisplayingItem.id, count), out list, this.ignoreBagWhenAdd, true))
		{
			int addedCount = InventoryUIItemMoveOpHandler.GetAddedCount(list);
			if (addedCount > 0)
			{
				from.RemoveItemById(itemCell.DisplayingItem.id, addedCount, this.ignoreBagWhenAdd, from.TryFindSourceBagForItem(itemCell.DisplayingItem), true);
				flag = true;
			}
		}
		if (this.silent)
		{
			UINotificator.isSilent = false;
		}
		if (MainGame.PlayerData != null && MainGame.PlayerData.HasInteractingItem)
		{
			MainGame.PlayerData.UpdateInteractingItem();
		}
		return flag;
	}

	// Token: 0x06003953 RID: 14675 RVA: 0x0011393C File Offset: 0x00111B3C
	private static int GetAddedCount(List<Item> addedItems)
	{
		int num = 0;
		for (int i = 0; i < addedItems.Count; i++)
		{
			num += addedItems[i].Count;
		}
		return num;
	}

	// Token: 0x04002D73 RID: 11635
	private Func<Inventory> inv1;

	// Token: 0x04002D74 RID: 11636
	private Func<Inventory> inv2;

	// Token: 0x04002D75 RID: 11637
	private Item ignoreBagWhenAdd;

	// Token: 0x04002D76 RID: 11638
	private bool silent;
}
