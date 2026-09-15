using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000416 RID: 1046
[Serializable]
public class Inventory
{
	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06001B43 RID: 6979 RVA: 0x0007EA28 File Offset: 0x0007CC28
	// (remove) Token: 0x06001B44 RID: 6980 RVA: 0x0007EA60 File Offset: 0x0007CC60
	public event Action<List<Item>> OnItemsAdd;

	// Token: 0x14000042 RID: 66
	// (add) Token: 0x06001B45 RID: 6981 RVA: 0x0007EA98 File Offset: 0x0007CC98
	// (remove) Token: 0x06001B46 RID: 6982 RVA: 0x0007EAD0 File Offset: 0x0007CCD0
	public event Action<List<Item>> OnItemsRemove;

	// Token: 0x14000043 RID: 67
	// (add) Token: 0x06001B47 RID: 6983 RVA: 0x0007EB08 File Offset: 0x0007CD08
	// (remove) Token: 0x06001B48 RID: 6984 RVA: 0x0007EB40 File Offset: 0x0007CD40
	public event Action<Item> OnBagRemoved;

	// Token: 0x14000044 RID: 68
	// (add) Token: 0x06001B49 RID: 6985 RVA: 0x0007EB78 File Offset: 0x0007CD78
	// (remove) Token: 0x06001B4A RID: 6986 RVA: 0x0007EBB0 File Offset: 0x0007CDB0
	public event Action<Item> OnBagAdded;

	// Token: 0x14000045 RID: 69
	// (add) Token: 0x06001B4B RID: 6987 RVA: 0x0007EBE8 File Offset: 0x0007CDE8
	// (remove) Token: 0x06001B4C RID: 6988 RVA: 0x0007EC20 File Offset: 0x0007CE20
	public event Action OnInventoryFull;

	// Token: 0x170004CF RID: 1231
	// (get) Token: 0x06001B4D RID: 6989 RVA: 0x0007EC55 File Offset: 0x0007CE55
	public Item Data
	{
		get
		{
			return this.inventoryItem;
		}
	}

	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06001B4E RID: 6990 RVA: 0x0007EC5D File Offset: 0x0007CE5D
	public string ViewId
	{
		get
		{
			if (!string.IsNullOrEmpty(this.customViewId))
			{
				return this.customViewId;
			}
			return this.Data.id;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06001B4F RID: 6991 RVA: 0x0007EC7E File Offset: 0x0007CE7E
	// (set) Token: 0x06001B50 RID: 6992 RVA: 0x0007EC86 File Offset: 0x0007CE86
	public Inventory ParentInventory
	{
		get
		{
			return this.parentInventory;
		}
		set
		{
			this.parentInventory = value;
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x0007EC8F File Offset: 0x0007CE8F
	public Inventory()
	{
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x0007ECA2 File Offset: 0x0007CEA2
	public Inventory(string inventoryId, int inventorySize)
	{
		this.inventoryItem = new Item(inventoryId, 1);
		this.inventoryItem.InventorySize = inventorySize;
	}

	// Token: 0x06001B53 RID: 6995 RVA: 0x0007ECCE File Offset: 0x0007CECE
	public Inventory(string inventoryId, int inventorySize, string customViewId)
	{
		this.inventoryItem = new Item(inventoryId, 1);
		this.inventoryItem.InventorySize = inventorySize;
		this.customViewId = customViewId;
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x0007ED01 File Offset: 0x0007CF01
	public Inventory(string inventoryId, int inventorySize, bool autoExpand)
		: this(inventoryId, inventorySize)
	{
		if (autoExpand)
		{
			this.inventoryItem.AddProperty<AutoExpandSerializedItemProperty>(new AutoExpandSerializedItemProperty());
		}
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x0007ED20 File Offset: 0x0007CF20
	public static Inventory GetInventoryFromBag(Item item, Inventory parentInventory = null)
	{
		if (item == null || !item.IsBag)
		{
			Debug.LogError("Can't create inventory not from bag item!");
			return null;
		}
		return new Inventory
		{
			inventoryItem = item,
			inventoryItem = 
			{
				InventorySize = item.Definition.bagSize
			},
			ParentInventory = parentInventory
		};
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x0007ED6D File Offset: 0x0007CF6D
	public Inventory(Item item)
	{
		this.inventoryItem = item;
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x0007ED87 File Offset: 0x0007CF87
	public static Inventory GetEmpty()
	{
		return new Inventory("inventory", 0);
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x0007ED94 File Offset: 0x0007CF94
	public static Inventory GetDefault()
	{
		return new Inventory("inventory", 40);
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x0007EDA4 File Offset: 0x0007CFA4
	public static Inventory Create(int inventorySize, bool isFuelInventory = false, WhiteListItemFilter whiteList = null, BlackListItemFilter blackList = null, string customViewId = "")
	{
		Inventory inventory = new Inventory("inventory", inventorySize, customViewId);
		if (isFuelInventory)
		{
			inventory.Data.AddProperty<FuelContainerSerializedItemProperty>(new FuelContainerSerializedItemProperty());
		}
		if (whiteList != null && !whiteList.IsEmpty)
		{
			inventory.Data.AddProperty<WhiteListFilterSerializedItemProperty>(new WhiteListFilterSerializedItemProperty(whiteList));
		}
		if (blackList != null && !blackList.IsEmpty)
		{
			inventory.Data.AddProperty<BlackListFilterSerializedItemProperty>(new BlackListFilterSerializedItemProperty(blackList));
		}
		return inventory;
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x0007EE0B File Offset: 0x0007D00B
	public static Inventory GetCraftInventory(int size)
	{
		return new Inventory("craftInventory", size);
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x0007EE18 File Offset: 0x0007D018
	public float GetTotalQuality()
	{
		float num = 0f;
		foreach (Item item in this.inventoryItem.Inventory)
		{
			num += (float)item.Definition.quality;
		}
		return num;
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x0007EE80 File Offset: 0x0007D080
	public float GetTotalQualityGrave()
	{
		float num = 0f;
		int num2 = 0;
		int num3 = 0;
		foreach (Item item in this.inventoryItem.Inventory)
		{
			num += (float)item.Definition.quality;
			if (item.Definition.itemGroupIds.Contains("body"))
			{
				foreach (Item item2 in item.Inventory)
				{
					num3 += item2.Definition.redSkulls * item2.Count;
					num2 += item2.Definition.whiteSkulls * item2.Count;
				}
			}
		}
		num3 = Mathf.Clamp(num3, 0, 999);
		num2 = Mathf.Clamp(num2, 0, 999);
		num -= (float)num3;
		num = Mathf.Clamp(num, -999f, (float)num2);
		return num;
	}

	// Token: 0x06001B5D RID: 7005 RVA: 0x0007EFA4 File Offset: 0x0007D1A4
	public bool AddItemToInventory(Item item, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		List<Item> list;
		return this.TryAddItemToInventory(item, out list, ignoredBag, ignoreAllBags);
	}

	// Token: 0x06001B5E RID: 7006 RVA: 0x0007EFBC File Offset: 0x0007D1BC
	public Item TryFindSourceBagForItem(Item item)
	{
		return this.inventoryItem.TryFindSourceBagForItem(item);
	}

	// Token: 0x06001B5F RID: 7007 RVA: 0x0007EFCA File Offset: 0x0007D1CA
	public bool AddItemToInventory(Item item, out List<Item> addedItems, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		return this.TryAddItemToInventory(item, out addedItems, ignoredBag, ignoreAllBags);
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x0007EFD7 File Offset: 0x0007D1D7
	public bool CanAddItemToInventory(Item item)
	{
		return this.inventoryItem.CanAddItemToInventory(item, true, false);
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x0007EFE7 File Offset: 0x0007D1E7
	public bool CanAddItemToInventory(string itemId, int count)
	{
		return this.inventoryItem.CanAddItemToInventory(itemId, count, true, false);
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x0007EFF8 File Offset: 0x0007D1F8
	public bool AddItemsToInventory(Inventory other)
	{
		List<Item> list;
		if (this.inventoryItem.AddItemsToInventory(other.Data, out list))
		{
			foreach (Item item in list)
			{
				if (item.IsBag)
				{
					Action<Item> onBagAdded = this.OnBagAdded;
					if (onBagAdded != null)
					{
						onBagAdded(item);
					}
				}
			}
			this.NotifyItemsAdded(list);
			return true;
		}
		Action onInventoryFull = this.OnInventoryFull;
		if (onInventoryFull != null)
		{
			onInventoryFull();
		}
		return false;
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x0007F08C File Offset: 0x0007D28C
	public bool AddItemsToInventory(List<Item> items)
	{
		List<Item> list;
		if (this.inventoryItem.AddItemsToInventory(items, out list))
		{
			foreach (Item item in list)
			{
				if (item.IsBag)
				{
					Action<Item> onBagAdded = this.OnBagAdded;
					if (onBagAdded != null)
					{
						onBagAdded(item);
					}
				}
			}
			this.NotifyItemsAdded(list);
			return true;
		}
		Action onInventoryFull = this.OnInventoryFull;
		if (onInventoryFull != null)
		{
			onInventoryFull();
		}
		return false;
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x0007F118 File Offset: 0x0007D318
	public bool AddItemsToNestedItemById(string itemId, List<Item> itemsToAdd)
	{
		Item itemById = this.GetItemById(itemId);
		if (itemById == null)
		{
			Debug.LogError("Add to nested item failed, Can't find nested item [" + itemId + "]");
			return false;
		}
		return itemById.AddItemsToInventory(itemsToAdd);
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x0007F154 File Offset: 0x0007D354
	public bool AddItemsToNestedItemByGroupId(string groupId, List<Item> itemsToAdd)
	{
		Item itemByGroupId = this.GetItemByGroupId(groupId);
		if (itemByGroupId == null)
		{
			Debug.LogError("Add to nested item failed, Can't find nested item with group [" + groupId + "]");
			return false;
		}
		if (itemByGroupId.AddItemsToInventory(itemsToAdd))
		{
			if (groupId == "body")
			{
				ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(itemByGroupId.UniqueId);
				if (zombie != null)
				{
					foreach (Item item in itemsToAdd)
					{
						zombie.SetZombieItem(itemByGroupId);
						zombie.OnAddOrgan(item);
					}
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001B66 RID: 7014 RVA: 0x0007F1F8 File Offset: 0x0007D3F8
	public List<Item> RemoveItemsFromNestedItemById(string itemId, List<NeedItemData> items, WgoData wgoData = null)
	{
		List<Item> list = new List<Item>();
		Item itemById = this.GetItemById(itemId);
		if (itemById == null)
		{
			Debug.LogError("Remove from nested item failed, Can't find nested item [" + itemId + "]");
			return list;
		}
		foreach (NeedItemData needItemData in items)
		{
			switch (needItemData.groupType)
			{
			case ItemGroup.None:
				list.AddRange(itemById.RemoveItemFromInventoryById(needItemData.Id, needItemData.GetCount(wgoData), null, null, false));
				break;
			case ItemGroup.Common:
				list.AddRange(itemById.RemoveItemFromInventoryByGroup(needItemData.Id, needItemData.GetCount(wgoData)));
				break;
			case ItemGroup.Star:
				list.AddRange(itemById.RemoveItemFromInventoryByStarGroup(needItemData.Id, needItemData.GetCount(wgoData)));
				break;
			}
		}
		return list;
	}

	// Token: 0x06001B67 RID: 7015 RVA: 0x0007F2D8 File Offset: 0x0007D4D8
	public List<Item> RemoveItemsFromNestedItemByGroupId(string groupId, List<NeedItemData> items, WgoData wgoData = null)
	{
		List<Item> list = new List<Item>();
		Item itemByGroupId = this.GetItemByGroupId(groupId);
		if (itemByGroupId == null)
		{
			Debug.LogError("Remove from nested item failed, Can't find nested item with group [" + groupId + "]");
			return list;
		}
		foreach (NeedItemData needItemData in items)
		{
			switch (needItemData.groupType)
			{
			case ItemGroup.None:
				list.AddRange(itemByGroupId.RemoveItemFromInventoryById(needItemData.Id, needItemData.GetCount(wgoData), null, null, false));
				break;
			case ItemGroup.Common:
				list.AddRange(itemByGroupId.RemoveItemFromInventoryByGroup(needItemData.Id, needItemData.GetCount(wgoData)));
				break;
			case ItemGroup.Star:
				list.AddRange(itemByGroupId.RemoveItemFromInventoryByStarGroup(needItemData.Id, needItemData.GetCount(wgoData)));
				break;
			}
		}
		if (groupId == "body")
		{
			ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(itemByGroupId.UniqueId);
			if (zombie != null)
			{
				foreach (Item item in list)
				{
					zombie.SetZombieItem(itemByGroupId);
					zombie.OnRemoveOrgan(item);
				}
			}
		}
		return list;
	}

	// Token: 0x06001B68 RID: 7016 RVA: 0x0007F424 File Offset: 0x0007D624
	public bool CanAddItemsToInventory(Inventory other)
	{
		return this.inventoryItem.CanAddItemsToInventory(other.Data);
	}

	// Token: 0x06001B69 RID: 7017 RVA: 0x0007F437 File Offset: 0x0007D637
	public bool CanAddItemsToInventory(List<Item> items)
	{
		return this.inventoryItem.CanAddItemsToInventory(items);
	}

	// Token: 0x06001B6A RID: 7018 RVA: 0x0007F445 File Offset: 0x0007D645
	public bool CanAddItemsToInventory(List<ItemCount> itemCounts, out List<ItemCount> cantAddItemsCount)
	{
		cantAddItemsCount = new List<ItemCount>();
		return this.inventoryItem.CanAddItemsToInventory(itemCounts, out cantAddItemsCount);
	}

	// Token: 0x06001B6B RID: 7019 RVA: 0x0007F45C File Offset: 0x0007D65C
	public bool CanAddItemsToInventory(List<ItemCount> itemCounts)
	{
		List<ItemCount> list;
		return this.CanAddItemsToInventory(itemCounts, out list);
	}

	// Token: 0x06001B6C RID: 7020 RVA: 0x0007F474 File Offset: 0x0007D674
	public bool CanAddItemsToInventoryConsideringDestination(CraftElementBase craftEl, List<ItemCount> itemCounts)
	{
		CraftElement craftElement = craftEl as CraftElement;
		if (craftElement != null)
		{
			CraftDef definition = craftElement.Definition;
			switch (definition.transferDestinationEnd)
			{
			case TransferDestination.Wgo:
				return this.CanAddItemsToInventory(itemCounts);
			case TransferDestination.ItemInside:
			{
				string destinationItemEnd = definition.destinationItemEnd;
				Item itemById = this.GetItemById(destinationItemEnd);
				return itemById != null && itemById.CanAddItemsToInventory(itemCounts);
			}
			case TransferDestination.GroupItemInside:
			{
				string destinationItemEnd2 = definition.destinationItemEnd;
				Item itemByGroupId = this.GetItemByGroupId(destinationItemEnd2);
				return itemByGroupId != null && itemByGroupId.CanAddItemsToInventory(itemCounts);
			}
			}
		}
		return false;
	}

	// Token: 0x06001B6D RID: 7021 RVA: 0x0007F4F8 File Offset: 0x0007D6F8
	public bool RemoveItemFromInventoryByUID(Item item, int count = -1)
	{
		Item item2 = this.inventoryItem.RemoveItemFromInventoryByUID(item.UniqueId.Guid, count);
		if (!item2.IsEmpty)
		{
			if (item2.IsBag)
			{
				Action<Item> onBagRemoved = this.OnBagRemoved;
				if (onBagRemoved != null)
				{
					onBagRemoved(item);
				}
			}
			this.NotifyItemsRemoved(new List<Item> { item });
			return true;
		}
		return false;
	}

	// Token: 0x06001B6E RID: 7022 RVA: 0x0007F554 File Offset: 0x0007D754
	public Item GetItemByType(ItemType itemType)
	{
		return this.inventoryItem.GetItemByType(itemType);
	}

	// Token: 0x06001B6F RID: 7023 RVA: 0x0007F564 File Offset: 0x0007D764
	public Item GetItemByTypes(ItemType[] itemTypes)
	{
		foreach (ItemType itemType in itemTypes)
		{
			Item itemByType = this.GetItemByType(itemType);
			if (!itemByType.IsEmpty)
			{
				return itemByType;
			}
		}
		return Item.Empty;
	}

	// Token: 0x06001B70 RID: 7024 RVA: 0x0007F59C File Offset: 0x0007D79C
	public void Clear()
	{
		List<Item> list = this.Data.RemoveAllItems();
		if (list.Count > 0)
		{
			foreach (Item item in list)
			{
				if (item.IsBag)
				{
					Action<Item> onBagRemoved = this.OnBagRemoved;
					if (onBagRemoved != null)
					{
						onBagRemoved(item);
					}
				}
			}
			this.NotifyItemsRemoved(list);
		}
	}

	// Token: 0x06001B71 RID: 7025 RVA: 0x0007F61C File Offset: 0x0007D81C
	public List<Item> RemoveItems(List<NeedItemData> items, WgoData wgoData)
	{
		return this.RemoveItems(items, 1, wgoData);
	}

	// Token: 0x06001B72 RID: 7026 RVA: 0x0007F628 File Offset: 0x0007D828
	public List<Item> RemoveItems(List<NeedItemData> items, int multiplicator = 1, WgoData wgoData = null)
	{
		List<Item> list = new List<Item>();
		foreach (NeedItemData needItemData in items)
		{
			switch (needItemData.groupType)
			{
			case ItemGroup.None:
				list.AddRange(this.RemoveItemById(needItemData.Id, needItemData.GetCount(wgoData) * multiplicator, null, null, false));
				break;
			case ItemGroup.Common:
				list.AddRange(this.RemoveItemByGroup(needItemData.Id, needItemData.GetCount(wgoData) * multiplicator));
				break;
			case ItemGroup.Star:
				list.AddRange(this.RemoveItemByStarGroup(needItemData.Id, needItemData.GetCount(wgoData) * multiplicator));
				break;
			}
		}
		return list;
	}

	// Token: 0x06001B73 RID: 7027 RVA: 0x0007F6EC File Offset: 0x0007D8EC
	public List<Item> RemoveItemByGroup(string groupId, int count)
	{
		List<Item> list = this.inventoryItem.RemoveItemFromInventoryByGroup(groupId, count);
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsBag)
				{
					Action<Item> onBagRemoved = this.OnBagRemoved;
					if (onBagRemoved != null)
					{
						onBagRemoved(list[i]);
					}
				}
			}
			this.NotifyItemsRemoved(list);
		}
		return list;
	}

	// Token: 0x06001B74 RID: 7028 RVA: 0x0007F750 File Offset: 0x0007D950
	public List<Item> RemoveItemByStarGroup(string starGroupId, int count)
	{
		List<Item> list = this.inventoryItem.RemoveItemFromInventoryByStarGroup(starGroupId, count);
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsBag)
				{
					Action<Item> onBagRemoved = this.OnBagRemoved;
					if (onBagRemoved != null)
					{
						onBagRemoved(list[i]);
					}
				}
			}
			this.NotifyItemsRemoved(list);
		}
		return list;
	}

	// Token: 0x06001B75 RID: 7029 RVA: 0x0007F7B4 File Offset: 0x0007D9B4
	public List<Item> RemoveItemById(string itemId, int count = -1, Item ignoredBag = null, Item sourceBag = null, bool ignoreAllBags = false)
	{
		List<Item> list = this.inventoryItem.RemoveItemFromInventoryById(itemId, count, ignoredBag, sourceBag, ignoreAllBags);
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsBag)
				{
					Action<Item> onBagRemoved = this.OnBagRemoved;
					if (onBagRemoved != null)
					{
						onBagRemoved(list[i]);
					}
				}
			}
			this.NotifyItemsRemoved(list);
		}
		return list;
	}

	// Token: 0x06001B76 RID: 7030 RVA: 0x0007F81C File Offset: 0x0007DA1C
	public bool CanTakeAnyItemsExistingInMeFromOtherInventory(Inventory otherInventory, bool ignoreMyBags = true, bool ignoreOtherBags = true)
	{
		return otherInventory != null && this.inventoryItem.CanTakeAnyItemsExistingInMeFromOtherInventory(otherInventory.inventoryItem, ignoreMyBags, ignoreOtherBags);
	}

	// Token: 0x06001B77 RID: 7031 RVA: 0x0007F838 File Offset: 0x0007DA38
	public void TakeAllItemsExistingInMeFromOtherInventory(Inventory otherInventory, bool ignoreMyBags = true, bool ignoreOtherBags = true)
	{
		List<Item> list = this.inventoryItem.TakeAllItemsExistingInMeFromOtherInventory(otherInventory.inventoryItem, ignoreMyBags, ignoreOtherBags);
		otherInventory.NotifyItemsRemoved(list);
		this.NotifyItemsAdded(list);
	}

	// Token: 0x06001B78 RID: 7032 RVA: 0x0007F868 File Offset: 0x0007DA68
	public Item GetItemById(string itemId)
	{
		Item item;
		this.inventoryItem.TryGetItemInInventory(itemId, out item);
		return item;
	}

	// Token: 0x06001B79 RID: 7033 RVA: 0x0007F888 File Offset: 0x0007DA88
	public Item GetItemByUniqueId(string uniqueId)
	{
		Item item;
		this.inventoryItem.TryGetItemInInventoryByGUID(uniqueId, out item);
		return item;
	}

	// Token: 0x06001B7A RID: 7034 RVA: 0x0007F8A5 File Offset: 0x0007DAA5
	public void Sort(Comparison<Item> comparison)
	{
		this.Data.Sort(comparison);
	}

	// Token: 0x06001B7B RID: 7035 RVA: 0x0007F8B4 File Offset: 0x0007DAB4
	public Item GetItemByGroupId(string groupId)
	{
		Item item;
		this.inventoryItem.TryGetItemInInventoryByGroupId(groupId, out item);
		return item;
	}

	// Token: 0x06001B7C RID: 7036 RVA: 0x0007F8D4 File Offset: 0x0007DAD4
	public List<Item> GetItemsByGroupId(string groupId)
	{
		List<Item> list = new List<Item>();
		foreach (Item item in this.inventoryItem.Inventory)
		{
			if (item.Definition.itemGroupIds.Contains(groupId))
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x06001B7D RID: 7037 RVA: 0x0007F948 File Offset: 0x0007DB48
	public List<Item> GetItemsByType(ItemType itemType)
	{
		List<Item> list = new List<Item>();
		foreach (Item item in this.inventoryItem.Inventory)
		{
			if (item.Definition.type == itemType)
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x06001B7E RID: 7038 RVA: 0x0007F9B8 File Offset: 0x0007DBB8
	public bool TryAddItemToInventory(Item item, out List<Item> addedItems, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		if (this.inventoryItem.AddItemToInventory(item, out addedItems, ignoredBag, ignoreAllBags))
		{
			foreach (Item item2 in addedItems)
			{
				if (item2.IsBag)
				{
					Action<Item> onBagAdded = this.OnBagAdded;
					if (onBagAdded != null)
					{
						onBagAdded(item2);
					}
				}
			}
			this.NotifyItemsAdded(addedItems);
			return true;
		}
		Action onInventoryFull = this.OnInventoryFull;
		if (onInventoryFull != null)
		{
			onInventoryFull();
		}
		return false;
	}

	// Token: 0x06001B7F RID: 7039 RVA: 0x0007FA48 File Offset: 0x0007DC48
	public void ForceTriggerOnItemsAddEventWithoutItems()
	{
		Action<List<Item>> onItemsAdd = this.OnItemsAdd;
		if (onItemsAdd == null)
		{
			return;
		}
		onItemsAdd(new List<Item>());
	}

	// Token: 0x06001B80 RID: 7040 RVA: 0x0007FA5F File Offset: 0x0007DC5F
	public void NotifyItemsAdded(List<Item> addedItems)
	{
		Action<List<Item>> onItemsAdd = this.OnItemsAdd;
		if (onItemsAdd != null)
		{
			onItemsAdd(addedItems);
		}
		Inventory inventory = this.ParentInventory;
		if (inventory == null)
		{
			return;
		}
		inventory.NotifyItemsAdded(addedItems);
	}

	// Token: 0x06001B81 RID: 7041 RVA: 0x0007FA84 File Offset: 0x0007DC84
	public void NotifyItemsRemoved(List<Item> removedItems)
	{
		Action<List<Item>> onItemsRemove = this.OnItemsRemove;
		if (onItemsRemove != null)
		{
			onItemsRemove(removedItems);
		}
		Inventory inventory = this.ParentInventory;
		if (inventory == null)
		{
			return;
		}
		inventory.NotifyItemsRemoved(removedItems);
	}

	// Token: 0x04001A71 RID: 6769
	private const string INVENTORY_ITEM_NAME = "inventory";

	// Token: 0x04001A72 RID: 6770
	private const string CRAFT_INVENTORY_ITEM_NAME = "craftInventory";

	// Token: 0x04001A73 RID: 6771
	private const int DEFAULT_INVENTORY_SIZE = 40;

	// Token: 0x04001A74 RID: 6772
	[SerializeField]
	private Item inventoryItem = new Item();

	// Token: 0x04001A75 RID: 6773
	[SerializeField]
	private string customViewId;

	// Token: 0x04001A76 RID: 6774
	[NonSerialized]
	private Inventory parentInventory;
}
