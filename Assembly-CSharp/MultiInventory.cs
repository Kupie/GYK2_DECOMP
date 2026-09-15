using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000417 RID: 1047
[Serializable]
public class MultiInventory
{
	// Token: 0x06001B82 RID: 7042 RVA: 0x0007FAA9 File Offset: 0x0007DCA9
	public MultiInventory()
	{
	}

	// Token: 0x06001B83 RID: 7043 RVA: 0x0007FABC File Offset: 0x0007DCBC
	public MultiInventory(Inventory inventory)
	{
		this.inventoryList.Add(inventory);
	}

	// Token: 0x06001B84 RID: 7044 RVA: 0x0007FADB File Offset: 0x0007DCDB
	public MultiInventory(IEnumerable<Inventory> inventoryList)
	{
		this.inventoryList.AddRange(inventoryList);
		this.Sort();
	}

	// Token: 0x06001B85 RID: 7045 RVA: 0x0007FADB File Offset: 0x0007DCDB
	public MultiInventory(params Inventory[] inventoryList)
	{
		this.inventoryList.AddRange(inventoryList);
		this.Sort();
	}

	// Token: 0x06001B86 RID: 7046 RVA: 0x0007FB00 File Offset: 0x0007DD00
	public MultiInventory(WorldZoneData worldZoneData, WgoData excludeWgoData = null, bool includePlayerInventory = false)
	{
		if (includePlayerInventory)
		{
			foreach (PlayerData playerData in worldZoneData.playerDataList)
			{
				this.inventoryList.Add(playerData.inventory);
			}
		}
		foreach (SGuid sguid in worldZoneData.wgoDataList)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null && wgoData.Definition.inventorySize != 0 && (excludeWgoData == null || !(wgoData.UniqueId == excludeWgoData.UniqueId)) && wgoData.Definition.OpenInMultiInventory)
			{
				this.inventoryList.Add(wgoData.Inventory);
			}
		}
		this.Sort();
	}

	// Token: 0x06001B87 RID: 7047 RVA: 0x0007FC14 File Offset: 0x0007DE14
	public MultiInventory(PlayerData playerData, bool addCurrentPlayerWorldZone = true)
	{
		if (addCurrentPlayerWorldZone && playerData.CurrentWorldZoneData != null)
		{
			this.inventoryList.AddRange(new MultiInventory(playerData.CurrentWorldZoneData, null, false).inventoryList);
		}
		this.inventoryList.Insert(0, playerData.inventory);
		this.Sort();
	}

	// Token: 0x06001B88 RID: 7048 RVA: 0x0007FC72 File Offset: 0x0007DE72
	public void Add(MultiInventory multiInventory)
	{
		this.inventoryList.AddRange(multiInventory.inventoryList);
		this.Sort();
	}

	// Token: 0x06001B89 RID: 7049 RVA: 0x0007FC8B File Offset: 0x0007DE8B
	public void Add(Inventory inventory)
	{
		this.inventoryList.Add(inventory);
		this.Sort();
	}

	// Token: 0x06001B8A RID: 7050 RVA: 0x0007FCA0 File Offset: 0x0007DEA0
	public int GetTotalCount(string itemId)
	{
		int num = 0;
		foreach (Inventory inventory in this.inventoryList)
		{
			num += inventory.Data.GetTotalCountInInventory(itemId, null, false);
		}
		return num;
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x0007FD00 File Offset: 0x0007DF00
	public bool HasItemWithEnoughDurability(string itemId, float needDurability)
	{
		using (List<Inventory>.Enumerator enumerator = this.inventoryList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Data.HasItemWithEnoughDurability(itemId, needDurability))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x0007FD60 File Offset: 0x0007DF60
	public bool HasItemsById(List<Item> items)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(items.Count);
		foreach (Item item in items)
		{
			int num;
			if (dictionary.TryGetValue(item.id, out num))
			{
				dictionary[item.id] = num + item.Count;
			}
			else
			{
				dictionary.Add(item.id, item.Count);
			}
		}
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			if (!this.HasItemQuantity(keyValuePair.Key, keyValuePair.Value))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x0007FE44 File Offset: 0x0007E044
	public bool HasItemsById(List<NeedItemData> needItems, WgoData wgoData)
	{
		return this.HasItemsById(needItems, 1, wgoData);
	}

	// Token: 0x06001B8E RID: 7054 RVA: 0x0007FE50 File Offset: 0x0007E050
	public bool HasItemsById(List<NeedItemData> needItems, int multiplicator = 1, WgoData wgoData = null)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(needItems.Count);
		foreach (NeedItemData needItemData in needItems)
		{
			int num;
			if (!dictionary.TryGetValue(needItemData.Id, out num))
			{
				dictionary.Add(needItemData.id, needItemData.GetCount(wgoData) * multiplicator);
			}
			else
			{
				dictionary[needItemData.Id] = num + needItemData.GetCount(wgoData);
			}
		}
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			if (!this.HasItemQuantity(keyValuePair.Key, keyValuePair.Value))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001B8F RID: 7055 RVA: 0x0007FF38 File Offset: 0x0007E138
	public bool HasItemQuantity(string itemId, int count)
	{
		return this.GetTotalCount(itemId) >= count;
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x0007FF47 File Offset: 0x0007E147
	public bool CanAddItems(List<Item> items)
	{
		return MultiInventory.CanAddItemsToInventories(items, this.inventoryList);
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x0007FF58 File Offset: 0x0007E158
	public bool CanAddItems(List<ItemCount> itemCounts)
	{
		List<ItemCount> list = new List<ItemCount>(itemCounts);
		foreach (Inventory inventory in this.inventoryList)
		{
			inventory.CanAddItemsToInventory(list, out list);
			if (list.Count == 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x0007FFC4 File Offset: 0x0007E1C4
	public bool CanAddItem(Item item, int count = -1)
	{
		if (count == -1)
		{
			count = item.Count;
		}
		int num = 0;
		foreach (Inventory inventory in this.inventoryList)
		{
			num += inventory.Data.CanAddItemCountToInventory(item, true, null, false);
			if (num >= count)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001B93 RID: 7059 RVA: 0x0008003C File Offset: 0x0007E23C
	public bool TryAddItems(List<Item> items)
	{
		if (!this.CanAddItems(items))
		{
			return false;
		}
		foreach (Item item in items)
		{
			this.AddItem(item);
		}
		return true;
	}

	// Token: 0x06001B94 RID: 7060 RVA: 0x00080098 File Offset: 0x0007E298
	public int TryAddItem(Item item)
	{
		int count = item.Count;
		int num = item.Count;
		foreach (Inventory inventory in this.inventoryList)
		{
			int num2 = inventory.Data.CanAddItemCountToInventory(item, true, null, false);
			if (num2 > num)
			{
				num2 = num;
			}
			if (num2 > 0)
			{
				Item item2 = Item.Copy(item);
				item2.Count = num2;
				inventory.AddItemToInventory(item2, null, false);
				num -= num2;
				if (num <= 0)
				{
					break;
				}
			}
		}
		return count - num;
	}

	// Token: 0x06001B95 RID: 7061 RVA: 0x0008013C File Offset: 0x0007E33C
	public void TryAddItemsPerOne(List<Item> items)
	{
		foreach (Item item in items)
		{
			this.TryAddItem(item);
		}
	}

	// Token: 0x06001B96 RID: 7062 RVA: 0x0008018C File Offset: 0x0007E38C
	public void AddItems(List<Item> items)
	{
		foreach (Inventory inventory in this.inventoryList)
		{
			for (int i = 0; i < items.Count; i++)
			{
				Item item = items[i];
				if (inventory.Data.CanAddItemCountToInventory(item, true, null, false) != 0)
				{
					inventory.AddItemToInventory(item, null, false);
					if (item.Count == 0)
					{
						items.RemoveAt(i);
						i--;
					}
				}
			}
		}
	}

	// Token: 0x06001B97 RID: 7063 RVA: 0x00080220 File Offset: 0x0007E420
	public int AddItem(Item item)
	{
		if (item == null || item.Count <= 0)
		{
			return 0;
		}
		int num = item.Count;
		int num2 = 0;
		foreach (Inventory inventory in this.inventoryList)
		{
			int num3 = inventory.Data.CanAddItemCountToInventory(item, true, null, false);
			if (num3 > num)
			{
				num3 = num;
			}
			if (num3 > 0)
			{
				Item item2 = Item.Copy(item);
				item2.Count = num3;
				if (inventory.AddItemToInventory(item2, null, false))
				{
					num2 += num3;
					num -= num3;
					if (num <= 0)
					{
						break;
					}
				}
			}
		}
		return num2;
	}

	// Token: 0x06001B98 RID: 7064 RVA: 0x000802D0 File Offset: 0x0007E4D0
	public List<Item> RemoveItemByCount(string itemId, int count)
	{
		List<Item> list = new List<Item>();
		int num = count;
		foreach (Inventory inventory in this.inventoryList)
		{
			if (num <= 0)
			{
				break;
			}
			List<Item> list2 = inventory.RemoveItemById(itemId, num, null, null, false);
			list.AddRange(list2);
			foreach (Item item in list2)
			{
				num -= item.Count;
			}
		}
		return list;
	}

	// Token: 0x06001B99 RID: 7065 RVA: 0x00080384 File Offset: 0x0007E584
	public List<Item> RemoveItems(List<NeedItemData> items, WgoData wgoData)
	{
		return this.RemoveItems(items, 1, wgoData);
	}

	// Token: 0x06001B9A RID: 7066 RVA: 0x00080390 File Offset: 0x0007E590
	public List<Item> RemoveItems(List<NeedItemData> items, int multiplicator = 1, WgoData wgoData = null)
	{
		List<Item> list = new List<Item>();
		foreach (NeedItemData needItemData in items)
		{
			int num = needItemData.GetCount(wgoData) * multiplicator;
			bool flag = false;
			foreach (Inventory inventory in this.inventoryList)
			{
				List<Item> list2 = inventory.RemoveItemById(needItemData.Id, num, null, null, false);
				list.AddRange(list2);
				foreach (Item item in list2)
				{
					num -= item.Count;
					if (num <= 0)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06001B9B RID: 7067 RVA: 0x00080498 File Offset: 0x0007E698
	public void RemoveItemFromInventoryByUID(Item item, int count = -1)
	{
		using (List<Inventory>.Enumerator enumerator = this.inventoryList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.RemoveItemFromInventoryByUID(item, count))
				{
					break;
				}
			}
		}
	}

	// Token: 0x06001B9C RID: 7068 RVA: 0x000804F0 File Offset: 0x0007E6F0
	public void RemoveItem(Item item)
	{
		using (List<Inventory>.Enumerator enumerator = this.inventoryList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.RemoveItemById(item.id, item.Count, null, null, false).Count > 0)
				{
					break;
				}
			}
		}
	}

	// Token: 0x06001B9D RID: 7069 RVA: 0x0008055C File Offset: 0x0007E75C
	public bool IsEmpty()
	{
		using (List<Inventory>.Enumerator enumerator = this.inventoryList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.Data.IsInventoryEmpty())
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000805BC File Offset: 0x0007E7BC
	private static bool CanAddItemsToInventories(List<Item> items, List<Inventory> inventories)
	{
		if (items.Count == 0)
		{
			return true;
		}
		int num = 0;
		foreach (Inventory inventory in inventories)
		{
			num += inventory.Data.InventorySize - inventory.Data.InventoryCount;
		}
		for (int i = 0; i < items.Count; i++)
		{
			Item item = items[i];
			int num2 = 0;
			foreach (Inventory inventory2 in inventories)
			{
				num2 += inventory2.Data.CanAddItemCountToInventory(item, false, null, false);
			}
			if (num2 < item.Count)
			{
				int num3 = Mathf.CeilToInt((float)(item.Count - num2) / (float)item.Definition.stackCount);
				if (num < num3)
				{
					return false;
				}
				num -= num3;
			}
		}
		return true;
	}

	// Token: 0x06001B9F RID: 7071 RVA: 0x000806D4 File Offset: 0x0007E8D4
	private void Sort()
	{
		this.inventoryList.Sort(delegate(Inventory x, Inventory y)
		{
			if (!x.Data.HasProperty<FuelContainerSerializedItemProperty>())
			{
				if (y.Data.HasProperty<FuelContainerSerializedItemProperty>())
				{
					return -1;
				}
			}
			else
			{
				if (!y.Data.HasProperty<FuelContainerSerializedItemProperty>())
				{
					return 1;
				}
				if (x.Data.InventorySize < y.Data.InventorySize)
				{
					return -1;
				}
			}
			return 0;
		});
	}

	// Token: 0x04001A77 RID: 6775
	public List<Inventory> inventoryList = new List<Inventory>();
}
