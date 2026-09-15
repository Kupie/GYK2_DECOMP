using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000419 RID: 1049
[Serializable]
public class Item : ObjectLinkedToDefinition<ItemDef>
{
	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x00080760 File Offset: 0x0007E960
	public int TotalItemsCount
	{
		get
		{
			int totalItemsCount = 0;
			this.inventory.ForEach(delegate(Item item)
			{
				totalItemsCount += item.count;
			});
			return totalItemsCount;
		}
	}

	// Token: 0x170004D3 RID: 1235
	// (get) Token: 0x06001BA4 RID: 7076 RVA: 0x00080797 File Offset: 0x0007E997
	// (set) Token: 0x06001BA5 RID: 7077 RVA: 0x0008079F File Offset: 0x0007E99F
	public int Count
	{
		get
		{
			return this.count;
		}
		set
		{
			this.count = value;
		}
	}

	// Token: 0x170004D4 RID: 1236
	// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x000807A8 File Offset: 0x0007E9A8
	public SGuid UniqueId
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x170004D5 RID: 1237
	// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x000807B0 File Offset: 0x0007E9B0
	public static Item Empty
	{
		get
		{
			return new Item("empty", 0);
		}
	}

	// Token: 0x170004D6 RID: 1238
	// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x000807BD File Offset: 0x0007E9BD
	public bool IsEmpty
	{
		get
		{
			return this.id == "empty" || string.IsNullOrEmpty(this.id) || this.count <= 0;
		}
	}

	// Token: 0x170004D7 RID: 1239
	// (get) Token: 0x06001BA9 RID: 7081 RVA: 0x000807EC File Offset: 0x0007E9EC
	public bool IsBag
	{
		get
		{
			return !string.IsNullOrEmpty(this.id) && this.id != "empty" && base.Definition.isBag;
		}
	}

	// Token: 0x170004D8 RID: 1240
	// (get) Token: 0x06001BAA RID: 7082 RVA: 0x0008081A File Offset: 0x0007EA1A
	public bool IsSeed
	{
		get
		{
			return !string.IsNullOrEmpty(this.id) && this.id != "empty" && base.Definition.isSeed;
		}
	}

	// Token: 0x170004D9 RID: 1241
	// (get) Token: 0x06001BAB RID: 7083 RVA: 0x00080848 File Offset: 0x0007EA48
	public bool IsFertilizer
	{
		get
		{
			return !string.IsNullOrEmpty(this.id) && this.id != "empty" && base.Definition.isFertilizer;
		}
	}

	// Token: 0x170004DA RID: 1242
	// (get) Token: 0x06001BAC RID: 7084 RVA: 0x00080876 File Offset: 0x0007EA76
	public int InventoryFillSize
	{
		get
		{
			if (this.inventoryFillSize != -1)
			{
				return this.inventoryFillSize;
			}
			this.CalculateInventoryFillSize();
			return this.inventoryFillSize;
		}
	}

	// Token: 0x170004DB RID: 1243
	// (get) Token: 0x06001BAE RID: 7086 RVA: 0x0008089D File Offset: 0x0007EA9D
	// (set) Token: 0x06001BAD RID: 7085 RVA: 0x00080894 File Offset: 0x0007EA94
	public int InventorySize
	{
		get
		{
			return this.inventorySize;
		}
		set
		{
			this.inventorySize = value;
		}
	}

	// Token: 0x170004DC RID: 1244
	// (get) Token: 0x06001BAF RID: 7087 RVA: 0x000808A5 File Offset: 0x0007EAA5
	public int InventoryCount
	{
		get
		{
			return this.inventory.Count;
		}
	}

	// Token: 0x170004DD RID: 1245
	// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x000808B2 File Offset: 0x0007EAB2
	public List<Item> Inventory
	{
		get
		{
			return this.inventory;
		}
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x000808BA File Offset: 0x0007EABA
	public Item()
	{
		this.id = "empty";
		this.count = -1;
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x000808F4 File Offset: 0x0007EAF4
	public Item(string id, int value = 1)
		: base(id)
	{
		this.id = id;
		this.count = value;
		this.uniqueId = new SGuid();
		if (base.Definition == null)
		{
			return;
		}
		if (base.Definition.inventorySize > 0)
		{
			this.inventorySize = base.Definition.inventorySize;
		}
		if (base.Definition.isBag)
		{
			this.inventorySize = base.Definition.bagSize;
		}
		if (base.Definition.hasDurability)
		{
			this.AddProperty<DurabilitySerializedItemProperty>(new DurabilitySerializedItemProperty(1f));
		}
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x000809A4 File Offset: 0x0007EBA4
	public static Item Copy(Item other)
	{
		Item item = new Item();
		item.id = other.id;
		item.count = other.count;
		item.uniqueId = new SGuid();
		item.uniqueId.Id = other.uniqueId.Id;
		item.inventorySize = other.inventorySize;
		item.inventoryFillSize = other.inventoryFillSize;
		item.inventory = new List<Item>(other.inventory);
		item.properties = new Dictionary<Type, SerializedItemProperty>();
		foreach (KeyValuePair<Type, SerializedItemProperty> keyValuePair in other.properties)
		{
			item.properties.Add(keyValuePair.Key, keyValuePair.Value.Clone());
		}
		return item;
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x00080A84 File Offset: 0x0007EC84
	public override string ToString()
	{
		return string.Format("{0}={1}", this.id, this.Count);
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x00080AA4 File Offset: 0x0007ECA4
	public static float GetQualityAverage(List<Item> items)
	{
		float num = 0f;
		int num2 = 0;
		foreach (Item item in items)
		{
			num += (float)item.Definition.quality;
			num2++;
		}
		if (num2 > 0)
		{
			num /= (float)num2;
		}
		return num;
	}

	// Token: 0x06001BB6 RID: 7094 RVA: 0x00080B10 File Offset: 0x0007ED10
	public static int GetCraftStartTicksBonusValue(List<Item> items)
	{
		int num = 0;
		foreach (Item item in items)
		{
			num += item.Definition.quality - 1;
		}
		return num;
	}

	// Token: 0x06001BB7 RID: 7095 RVA: 0x00080B6C File Offset: 0x0007ED6C
	public void AddProperty<T>(T property) where T : SerializedItemProperty
	{
		this.CheckPropertiesCreated();
		Type typeFromHandle = typeof(T);
		if (this.properties.ContainsKey(typeFromHandle))
		{
			Debug.LogWarning("Item already has property of type " + typeFromHandle.Name);
			return;
		}
		this.properties[typeFromHandle] = property;
	}

	// Token: 0x06001BB8 RID: 7096 RVA: 0x00080BC0 File Offset: 0x0007EDC0
	public bool RemoveProperty<T>() where T : SerializedItemProperty
	{
		this.CheckPropertiesCreated();
		return this.properties.Remove(typeof(T));
	}

	// Token: 0x06001BB9 RID: 7097 RVA: 0x00080BDD File Offset: 0x0007EDDD
	public bool HasProperty<T>() where T : SerializedItemProperty
	{
		this.CheckPropertiesCreated();
		return this.properties.ContainsKey(typeof(T));
	}

	// Token: 0x06001BBA RID: 7098 RVA: 0x00080BFC File Offset: 0x0007EDFC
	public bool TryGetProperty<T>(out T property) where T : SerializedItemProperty
	{
		this.CheckPropertiesCreated();
		SerializedItemProperty serializedItemProperty;
		if (this.properties.TryGetValue(typeof(T), out serializedItemProperty))
		{
			property = (T)((object)serializedItemProperty);
			return true;
		}
		property = default(T);
		return false;
	}

	// Token: 0x06001BBB RID: 7099 RVA: 0x00080C3E File Offset: 0x0007EE3E
	private void CheckPropertiesCreated()
	{
		if (this.properties == null)
		{
			this.properties = new Dictionary<Type, SerializedItemProperty>();
		}
	}

	// Token: 0x06001BBC RID: 7100 RVA: 0x00080C53 File Offset: 0x0007EE53
	public bool TryAddItem(Item sourceItem)
	{
		if (this.CanAddItemCount(sourceItem) != sourceItem.Count || sourceItem.IsEmpty || this.Count == 0)
		{
			return false;
		}
		this.Count += sourceItem.Count;
		sourceItem.Count = 0;
		return true;
	}

	// Token: 0x06001BBD RID: 7101 RVA: 0x00080C91 File Offset: 0x0007EE91
	public bool TryAddItemNoChangeSource(in Item sourceItem)
	{
		if (this.CanAddItemCount(sourceItem) != sourceItem.Count || sourceItem.IsEmpty || this.Count == 0)
		{
			return false;
		}
		this.Count += sourceItem.Count;
		return true;
	}

	// Token: 0x06001BBE RID: 7102 RVA: 0x00080CCC File Offset: 0x0007EECC
	public bool TryAddItemPartial(Item sourceItem)
	{
		int num = this.CanAddItemCount(sourceItem);
		if (num == 0 || sourceItem.IsEmpty)
		{
			return false;
		}
		this.Count += num;
		sourceItem.Count -= num;
		return true;
	}

	// Token: 0x06001BBF RID: 7103 RVA: 0x00080D0B File Offset: 0x0007EF0B
	public bool CanAddItem(Item sourceItem)
	{
		return this.CanAddItemCount(sourceItem) > 0;
	}

	// Token: 0x06001BC0 RID: 7104 RVA: 0x00080D18 File Offset: 0x0007EF18
	public int CanAddItemCount(Item sourceItem, int countToAdd)
	{
		if (sourceItem == null || sourceItem.IsEmpty)
		{
			return 0;
		}
		if (this.id != sourceItem.id)
		{
			return 0;
		}
		int num = base.Definition.stackCount - this.Count;
		if (countToAdd < num)
		{
			return countToAdd;
		}
		return num;
	}

	// Token: 0x06001BC1 RID: 7105 RVA: 0x00080D61 File Offset: 0x0007EF61
	public int CanAddItemCount(Item sourceItem)
	{
		return this.CanAddItemCount(sourceItem, sourceItem.Count);
	}

	// Token: 0x06001BC2 RID: 7106 RVA: 0x00080D70 File Offset: 0x0007EF70
	public Item Split(int newPartCount, bool transferInternalData = false)
	{
		if (newPartCount <= 0)
		{
			Debug.LogError(string.Format("Item.Split: incorrect splitting value: [{0}] for item [{1}]", newPartCount, this.id));
			return Item.Empty;
		}
		if (newPartCount > this.Count)
		{
			Debug.LogWarning(string.Format("Item.Split: incorrect splitting value: [{0}] for item [{1}]", newPartCount, this.id));
			newPartCount = Mathf.Min(newPartCount, this.Count);
		}
		Item item = new Item(this.id, newPartCount);
		this.Count -= newPartCount;
		if (transferInternalData)
		{
			item.inventory = this.inventory;
			this.inventory = new List<Item>();
			foreach (KeyValuePair<Type, SerializedItemProperty> keyValuePair in this.properties)
			{
				item.properties.Add(keyValuePair.Key, keyValuePair.Value.Clone());
			}
		}
		return item;
	}

	// Token: 0x06001BC3 RID: 7107 RVA: 0x00080E68 File Offset: 0x0007F068
	public Item GetItemByType(ItemType toolType)
	{
		foreach (Item item in this.inventory)
		{
			if (item.Definition.type == toolType)
			{
				return item;
			}
		}
		return Item.Empty;
	}

	// Token: 0x06001BC4 RID: 7108 RVA: 0x00080ED0 File Offset: 0x0007F0D0
	public void Sort(Comparison<Item> comparison)
	{
		this.inventory.Sort(comparison);
	}

	// Token: 0x06001BC5 RID: 7109 RVA: 0x00080EE0 File Offset: 0x0007F0E0
	public bool AddItemToInventory(Item sourceItem, bool ignoreAllBags = false)
	{
		List<Item> list;
		return this.AddItemToInventory(sourceItem, out list, null, ignoreAllBags);
	}

	// Token: 0x06001BC6 RID: 7110 RVA: 0x00080EF8 File Offset: 0x0007F0F8
	public bool AddItemToInventory(Item sourceItem, out List<Item> addedItems, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		addedItems = new List<Item>();
		int num = this.CanAddItemCountToInventory(sourceItem, true, ignoredBag, ignoreAllBags);
		if (num == 0 || sourceItem.Count == 0)
		{
			return false;
		}
		int num2 = sourceItem.Count;
		int stackCount = sourceItem.Definition.stackCount;
		if (stackCount == 1)
		{
			Item item = sourceItem.Split(1, true);
			item.UniqueId.SetGuid(sourceItem.UniqueId);
			bool flag = false;
			if (!ignoreAllBags)
			{
				for (int i = 0; i < this.inventory.Count; i++)
				{
					if (this.inventory[i].IsBag && sourceItem.Definition.CanBeInsertedInBag(this.inventory[i].Definition) && this.inventory[i].uniqueId != ((ignoredBag != null) ? ignoredBag.uniqueId : null) && this.inventory[i].CanAddItemToInventory(item, true, false) && this.inventory[i].GetTotalCountInInventory(item.id, null, false) > 0)
					{
						this.inventory[i].InsertItemToInventory(item);
						flag = true;
						break;
					}
				}
			}
			if (this.HasProperty<AutoExpandSerializedItemProperty>() && this.InventorySize < 1000 && this.InventoryFillSize == this.InventorySize)
			{
				this.inventorySize++;
			}
			if (!flag && this.InventoryFillSize != this.InventorySize)
			{
				this.InsertItemToInventory(item);
				flag = true;
			}
			if (!ignoreAllBags && !flag)
			{
				for (int j = 0; j < this.inventory.Count; j++)
				{
					if (this.inventory[j].IsBag && sourceItem.Definition.CanBeInsertedInBag(this.inventory[j].Definition) && this.inventory[j].uniqueId != ((ignoredBag != null) ? ignoredBag.uniqueId : null) && this.inventory[j].CanAddItemToInventory(item, true, false))
					{
						this.inventory[j].InsertItemToInventory(item);
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				addedItems.Add(item);
				this.CalculateInventoryFillSize();
				Item.KeepUnaddedCountOnSource(sourceItem, num2, addedItems);
				return true;
			}
			return false;
		}
		else
		{
			int num3 = num;
			if (!ignoreAllBags)
			{
				for (int k = 0; k < this.inventory.Count; k++)
				{
					if (this.inventory[k].IsBag && sourceItem.Definition.CanBeInsertedInBag(this.inventory[k].Definition) && this.inventory[k].uniqueId != ((ignoredBag != null) ? ignoredBag.uniqueId : null) && this.inventory[k].GetTotalCountInInventory(sourceItem.id, null, false) > 0)
					{
						int num4 = this.inventory[k].CanAddItemCountToInventory(sourceItem.Definition, num3, true, null, true);
						addedItems.Add(new Item(sourceItem.id, num4));
						this.inventory[k].AddItemToInventory(new Item(sourceItem.id, num4), false);
						num3 -= num4;
					}
					if (num3 <= 0)
					{
						Item.KeepUnaddedCountOnSource(sourceItem, num2, addedItems);
						return true;
					}
				}
			}
			foreach (Item item2 in this.inventory)
			{
				if (!item2.IsEmpty)
				{
					int num5 = item2.CanAddItemCount(sourceItem, num3);
					if (num5 > 0)
					{
						Item item3 = new Item(sourceItem.id, num5);
						addedItems.Add(item3);
						item2.TryAddItemNoChangeSource(in item3);
						num3 -= num5;
					}
					else if (num3 <= 0)
					{
						break;
					}
				}
			}
			if (num3 <= 0)
			{
				Item.KeepUnaddedCountOnSource(sourceItem, num2, addedItems);
				this.CalculateInventoryFillSize();
				return true;
			}
			int num6 = (this.HasProperty<AutoExpandSerializedItemProperty>() ? 1000 : (this.inventorySize - this.inventoryFillSize));
			int num7;
			for (int l = Mathf.Min(num3, num6 * stackCount); l > 0; l -= num7)
			{
				num7 = Mathf.Min(num3, stackCount);
				Item item4 = sourceItem.Split(num7, false);
				num3 -= num7;
				addedItems.Add(item4);
				if (this.HasProperty<AutoExpandSerializedItemProperty>())
				{
					this.CalculateInventoryFillSize();
					if (this.InventoryFillSize == this.InventorySize)
					{
						this.inventorySize++;
					}
				}
				if (this.InventoryFillSize != this.InventorySize)
				{
					this.InsertItemToInventory(item4);
					this.CalculateInventoryFillSize();
				}
			}
			if (num3 <= 0)
			{
				Item.KeepUnaddedCountOnSource(sourceItem, num2, addedItems);
				this.CalculateInventoryFillSize();
				return true;
			}
			if (!ignoreAllBags)
			{
				for (int m = 0; m < this.inventory.Count; m++)
				{
					if (this.inventory[m].IsBag && sourceItem.Definition.CanBeInsertedInBag(this.inventory[m].Definition) && this.inventory[m].uniqueId != ((ignoredBag != null) ? ignoredBag.uniqueId : null))
					{
						int num8 = this.inventory[m].CanAddItemCountToInventory(sourceItem.Definition, num3, true, null, true);
						addedItems.Add(new Item(sourceItem.id, num8));
						this.inventory[m].AddItemToInventory(new Item(sourceItem.id, num8), false);
						num3 -= num8;
					}
					if (num3 <= 0)
					{
						Item.KeepUnaddedCountOnSource(sourceItem, num2, addedItems);
						return true;
					}
				}
			}
			Item.KeepUnaddedCountOnSource(sourceItem, num2, addedItems);
			return true;
		}
	}

	// Token: 0x06001BC7 RID: 7111 RVA: 0x000814A8 File Offset: 0x0007F6A8
	private static int GetItemsTotalCount(List<Item> items)
	{
		int num = 0;
		if (items == null)
		{
			return 0;
		}
		for (int i = 0; i < items.Count; i++)
		{
			num += items[i].Count;
		}
		return num;
	}

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000814DD File Offset: 0x0007F6DD
	private static void KeepUnaddedCountOnSource(Item sourceItem, int originalCount, List<Item> addedItems)
	{
		sourceItem.Count = Mathf.Max(0, originalCount - Item.GetItemsTotalCount(addedItems));
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000814F3 File Offset: 0x0007F6F3
	public bool CanAddItemToInventory(Item sourceItem, bool considerEmptySlots = true, bool ignoreAllBags = false)
	{
		return this.CanAddItemCountToInventory(sourceItem, considerEmptySlots, null, ignoreAllBags) == sourceItem.count;
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x00081508 File Offset: 0x0007F708
	public bool CanAddItemToInventory(string itemId, int count, bool considerEmptySlots = true, bool ignoreAllBags = false)
	{
		if (string.IsNullOrEmpty(itemId) || count == 0)
		{
			return false;
		}
		ItemDef data = GameBalance.Me.GetData<ItemDef>(itemId);
		return data != null && this.CanAddItemCountToInventory(data, count, considerEmptySlots, null, ignoreAllBags) == count;
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x00081542 File Offset: 0x0007F742
	public int CanAddItemCountToInventory(Item sourceItem, int countToAdd, bool considerEmptySlots = true, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		if (sourceItem == null || sourceItem.IsEmpty)
		{
			return 0;
		}
		return this.CanAddItemCountToInventory(sourceItem.Definition, countToAdd, considerEmptySlots, ignoredBag, ignoreAllBags);
	}

	// Token: 0x06001BCC RID: 7116 RVA: 0x00081564 File Offset: 0x0007F764
	public int CanAddItemCountToInventory(ItemDef itemDef, int countToAdd, bool considerEmptySlots = true, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		WhiteListFilterSerializedItemProperty whiteListFilterSerializedItemProperty;
		BlackListFilterSerializedItemProperty blackListFilterSerializedItemProperty;
		if ((this.TryGetProperty<WhiteListFilterSerializedItemProperty>(out whiteListFilterSerializedItemProperty) && !whiteListFilterSerializedItemProperty.WhiteList.Contains(itemDef)) || (this.TryGetProperty<BlackListFilterSerializedItemProperty>(out blackListFilterSerializedItemProperty) && blackListFilterSerializedItemProperty.BlackList.Contains(itemDef)))
		{
			return 0;
		}
		int num = 0;
		int stackCount = itemDef.stackCount;
		List<Item> list = new List<Item>(this.inventory);
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.inventory.Count; i++)
		{
			if (!ignoreAllBags && this.inventory[i].IsBag && itemDef.CanBeInsertedInBag(this.inventory[i].Definition) && this.inventory[i].uniqueId != ((ignoredBag != null) ? ignoredBag.uniqueId : null))
			{
				list.AddRange(this.inventory[i].inventory);
				if (considerEmptySlots)
				{
					num2 += this.inventory[i].inventorySize;
					num3 += this.inventory[i].inventoryFillSize;
				}
			}
		}
		foreach (Item item in list)
		{
			if (item.id == itemDef.id)
			{
				num += stackCount - item.Count;
			}
		}
		if (considerEmptySlots)
		{
			int num4 = (this.HasProperty<AutoExpandSerializedItemProperty>() ? 1000 : (this.inventorySize + num2));
			num += (num4 - (this.inventoryFillSize + num3)) * stackCount;
		}
		if (countToAdd < num)
		{
			return countToAdd;
		}
		return num;
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x0008171C File Offset: 0x0007F91C
	public int CanAddItemCountToInventory(Item sourceItem, bool considerEmptySlots = true, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		return this.CanAddItemCountToInventory(sourceItem, sourceItem.Count, considerEmptySlots, ignoredBag, ignoreAllBags);
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x00081730 File Offset: 0x0007F930
	public bool TryGetItemInInventory(string itemId, out Item itemResult)
	{
		itemResult = Item.Empty;
		foreach (Item item in this.inventory)
		{
			if (itemId == item.id)
			{
				itemResult = item;
				return true;
			}
		}
		foreach (Item item2 in this.inventory)
		{
			if (item2.IsBag && item2.TryGetItemInInventory(itemId, out itemResult))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000817EC File Offset: 0x0007F9EC
	public Item TryFindSourceBagForItem(Item item)
	{
		for (int i = 0; i < this.inventory.Count; i++)
		{
			Item item2 = this.inventory[i];
			if (item2.IsBag)
			{
				for (int j = 0; j < item2.inventory.Count; j++)
				{
					if (item2.inventory[j].UniqueId == item.UniqueId)
					{
						return item2;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x0008185C File Offset: 0x0007FA5C
	public bool TryGetItemInInventoryByGUID(string uniqueId, out Item itemResult)
	{
		itemResult = Item.Empty;
		for (int i = 0; i < this.inventory.Count; i++)
		{
			Item item = this.inventory[i];
			if (item.uniqueId.Id == uniqueId)
			{
				itemResult = item;
				return true;
			}
		}
		for (int j = 0; j < this.inventory.Count; j++)
		{
			Item item2 = this.inventory[j];
			if (item2.IsBag && item2.TryGetItemInInventoryByGUID(uniqueId, out itemResult))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000818E4 File Offset: 0x0007FAE4
	public bool TryGetItemInInventoryByGroupId(string groupId, out Item itemResult)
	{
		itemResult = Item.Empty;
		for (int i = 0; i < this.inventory.Count; i++)
		{
			Item item = this.inventory[i];
			if (item.Definition.itemGroupIds.Contains(groupId))
			{
				itemResult = item;
				return true;
			}
		}
		for (int j = 0; j < this.inventory.Count; j++)
		{
			Item item2 = this.inventory[j];
			if (item2.IsBag && item2.TryGetItemInInventoryByGroupId(groupId, out itemResult))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x0008196B File Offset: 0x0007FB6B
	public List<Item> RemoveItemFromInventoryById(string itemId, int count = -1, Item ignoredBag = null, Item sourceBag = null, bool ignoreAllBags = false)
	{
		return this.RemoveItemFromInventory(itemId, count, new Func<Item, string, bool>(Item.IsItemTheSameByItemId), ignoredBag, sourceBag, ignoreAllBags);
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x00081986 File Offset: 0x0007FB86
	public List<Item> RemoveItemFromInventoryByGroup(string groupId, int count = -1)
	{
		return this.RemoveItemFromInventory(groupId, count, new Func<Item, string, bool>(Item.IsItemTheSameByGroupId), null, null, false);
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x0008199F File Offset: 0x0007FB9F
	public List<Item> RemoveItemFromInventoryByStarGroup(string groupId, int count = -1)
	{
		return this.RemoveItemFromInventory(groupId, count, new Func<Item, string, bool>(Item.IsItemTheSameByStarGroupId), null, null, false);
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000819B8 File Offset: 0x0007FBB8
	public Item RemoveItemFromInventoryByUID(Guid uniqueId, int count = -1)
	{
		Item item = Item.Empty;
		for (int i = 0; i < this.inventory.Count; i++)
		{
			Item item2 = this.inventory[i];
			if (Item.IsItemTheSameByUId(item2, uniqueId))
			{
				int num = ((count == -1) ? item2.count : count);
				if (item2.count <= num)
				{
					item = item2;
					this.inventory.RemoveAt(i);
				}
				else
				{
					item = item2.Split(num, false);
				}
				this.CalculateInventoryFillSize();
				return item;
			}
			if (item2.IsBag)
			{
				for (int j = 0; j < item2.inventory.Count; j++)
				{
					Item item3 = item2.inventory[j];
					if (Item.IsItemTheSameByUId(item3, uniqueId))
					{
						int num2 = ((count == -1) ? item3.count : count);
						if (item3.count <= num2)
						{
							item = item3;
							item2.inventory.RemoveAt(j);
						}
						else
						{
							item = item3.Split(num2, false);
						}
						item2.CalculateInventoryFillSize();
						return item;
					}
				}
			}
		}
		return item;
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x00081AB4 File Offset: 0x0007FCB4
	private List<Item> RemoveItemFromInventory(string itemId, int count = -1, Func<Item, string, bool> removeCondition = null, Item ignoredBag = null, Item sourceBag = null, bool ignoreAllBags = false)
	{
		Item.<>c__DisplayClass70_0 CS$<>8__locals1;
		CS$<>8__locals1.itemId = itemId;
		CS$<>8__locals1.removeCondition = removeCondition;
		CS$<>8__locals1.removedItems = new List<Item>();
		CS$<>8__locals1.leftToRemoveCount = count;
		CS$<>8__locals1.removeFullCount = count == -1;
		ItemDef data = GameBalance.Me.GetData<ItemDef>(CS$<>8__locals1.itemId);
		if (!ignoreAllBags && sourceBag != null)
		{
			Item.<RemoveItemFromInventory>g__BagIteration|70_0(sourceBag, ref CS$<>8__locals1);
		}
		List<Item> list = new List<Item>();
		int num = this.inventory.Count - 1;
		while (num >= 0 && CS$<>8__locals1.leftToRemoveCount != 0)
		{
			Item item = this.inventory[num];
			if (sourceBag == null || item != sourceBag)
			{
				if (!ignoreAllBags && item.IsBag && data != null && data.CanBeInsertedInBag(item.Definition) && ((ignoredBag != null) ? ignoredBag.uniqueId : null) != item.uniqueId)
				{
					if (ignoredBag == null)
					{
						list.Add(item);
						goto IL_0198;
					}
					if (ignoredBag.uniqueId != item.uniqueId)
					{
						list.Add(item);
						goto IL_0198;
					}
				}
				Func<Item, string, bool> removeCondition2 = CS$<>8__locals1.removeCondition;
				if (removeCondition2 == null || removeCondition2(item, CS$<>8__locals1.itemId))
				{
					int num2 = (CS$<>8__locals1.removeFullCount ? item.count : CS$<>8__locals1.leftToRemoveCount);
					int num3;
					if (item.count <= num2)
					{
						CS$<>8__locals1.removedItems.Add(item);
						num3 = item.count;
						this.inventory.RemoveAt(num);
					}
					else
					{
						CS$<>8__locals1.removedItems.Add(item.Split(num2, false));
						num3 = num2;
					}
					if (!CS$<>8__locals1.removeFullCount)
					{
						CS$<>8__locals1.leftToRemoveCount -= num3;
					}
				}
			}
			IL_0198:
			num--;
		}
		if (!ignoreAllBags && (CS$<>8__locals1.removeFullCount || CS$<>8__locals1.leftToRemoveCount > 0))
		{
			int num4 = 0;
			while (num4 < list.Count && (CS$<>8__locals1.removeFullCount || CS$<>8__locals1.leftToRemoveCount > 0))
			{
				Item.<RemoveItemFromInventory>g__BagIteration|70_0(list[num4], ref CS$<>8__locals1);
				num4++;
			}
		}
		this.CalculateInventoryFillSize();
		return CS$<>8__locals1.removedItems;
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x00081CC0 File Offset: 0x0007FEC0
	private int GetLastItemIndexInInventory(string itemId)
	{
		for (int i = this.inventory.Count - 1; i >= 0; i--)
		{
			if (!(this.inventory[i].id != itemId))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00081D04 File Offset: 0x0007FF04
	private Item GetLastItemInInventory(string itemId)
	{
		int lastItemIndexInInventory = this.GetLastItemIndexInInventory(itemId);
		if (lastItemIndexInInventory != -1)
		{
			return this.inventory[lastItemIndexInInventory];
		}
		return null;
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x00081D2C File Offset: 0x0007FF2C
	public int GetTotalCountInInventory(string itemId, Item ignoredBag = null, bool ignoreAllBags = false)
	{
		if (string.IsNullOrEmpty(itemId))
		{
			return 0;
		}
		int num = 0;
		foreach (Item item in this.inventory)
		{
			if (item != ignoredBag)
			{
				if (Item.IsItemTheSameByItemId(item, itemId))
				{
					num += item.count;
				}
				if (item.IsBag && !ignoreAllBags)
				{
					foreach (Item item2 in item.inventory)
					{
						if (Item.IsItemTheSameByItemId(item2, itemId))
						{
							num += item2.count;
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x00081DF8 File Offset: 0x0007FFF8
	public int GetTotalCountInInventory(Guid uniqueId)
	{
		int num = 0;
		foreach (Item item in this.inventory)
		{
			if (Item.IsItemTheSameByUId(item, uniqueId))
			{
				num += item.count;
			}
			if (item.IsBag)
			{
				foreach (Item item2 in item.inventory)
				{
					if (Item.IsItemTheSameByUId(item2, uniqueId))
					{
						num += item2.count;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x00081EB4 File Offset: 0x000800B4
	public bool IsInventoryEmpty()
	{
		using (List<Item>.Enumerator enumerator = this.inventory.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.count > 0)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x00081F10 File Offset: 0x00080110
	public bool HasItemQuantityInInventory(string itemId, int count)
	{
		return this.GetTotalCountInInventory(itemId, null, false) >= count;
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00081F24 File Offset: 0x00080124
	public bool HasItemWithEnoughDurability(string itemId, float needDurability)
	{
		for (int i = 0; i < this.inventory.Count; i++)
		{
			DurabilitySerializedItemProperty durabilitySerializedItemProperty;
			if (this.inventory[i].id == itemId && this.inventory[i].TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty) && durabilitySerializedItemProperty.Durability >= needDurability)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x00081F84 File Offset: 0x00080184
	public Item GetAndRemoveItemWithEnoughDurability(string itemId, float needDurability)
	{
		Item item = null;
		for (int i = 0; i < this.inventory.Count; i++)
		{
			DurabilitySerializedItemProperty durabilitySerializedItemProperty;
			if (this.inventory[i].id == itemId && this.inventory[i].TryGetProperty<DurabilitySerializedItemProperty>(out durabilitySerializedItemProperty) && durabilitySerializedItemProperty.Durability >= needDurability)
			{
				item = this.inventory[i];
				this.inventory.RemoveAt(i);
				break;
			}
			ItemDef data = GameBalance.Me.GetData<ItemDef>(itemId);
			if (this.inventory[i].IsBag && data != null && data.CanBeInsertedInBag(this.inventory[i].Definition))
			{
				item = this.inventory[i].GetAndRemoveItemWithEnoughDurability(itemId, needDurability);
				if (item != null)
				{
					break;
				}
			}
		}
		this.CalculateInventoryFillSize();
		return item;
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x0008205C File Offset: 0x0008025C
	public bool CanTakeAnyItemsExistingInMeFromOtherInventory(Item otherInventory, bool ignoreMyBags, bool ignoreOtherBags)
	{
		if (otherInventory == null)
		{
			return false;
		}
		HashSet<string> hashSet = this.CollectUniqueItemIds(ignoreMyBags);
		for (int i = 0; i < otherInventory.inventory.Count; i++)
		{
			Item item = otherInventory.inventory[i];
			if (item.Definition.stackCount > 1)
			{
				if (hashSet.Contains(item.id) && this.CanAddItemCountToInventory(item, true, null, ignoreMyBags) > 0)
				{
					return true;
				}
			}
			else if (!ignoreOtherBags && item.IsBag)
			{
				for (int j = 0; j < item.inventory.Count; j++)
				{
					Item item2 = item.inventory[j];
					if (item2.Definition.stackCount > 1 && hashSet.Contains(item2.id) && this.CanAddItemCountToInventory(item2, true, null, ignoreMyBags) > 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00082128 File Offset: 0x00080328
	public List<Item> TakeAllItemsExistingInMeFromOtherInventory(Item otherInventory, bool ignoreMyBags, bool ignoreOtherBags)
	{
		List<Item> list = new List<Item>();
		HashSet<string> hashSet = this.CollectUniqueItemIds(ignoreMyBags);
		for (int i = otherInventory.inventory.Count - 1; i >= 0; i--)
		{
			Item item = otherInventory.inventory[i];
			if (item.Definition.stackCount > 1)
			{
				List<Item> list2;
				if (hashSet.Contains(item.id) && this.AddItemToInventory(new Item(item.id, item.Count), out list2, null, ignoreMyBags))
				{
					int itemsTotalCount = Item.GetItemsTotalCount(list2);
					if (itemsTotalCount > 0)
					{
						otherInventory.RemoveItemFromInventory(item.id, itemsTotalCount, new Func<Item, string, bool>(Item.IsItemTheSameByItemId), null, null, ignoreOtherBags);
						if (!list.Contains(otherInventory))
						{
							list.Add(otherInventory);
						}
					}
				}
			}
			else if (!ignoreOtherBags && item.IsBag)
			{
				for (int j = item.inventory.Count - 1; j >= 0; j--)
				{
					Item item2 = item.inventory[j];
					List<Item> list3;
					if (item2.Definition.stackCount > 1 && hashSet.Contains(item2.id) && this.AddItemToInventory(new Item(item2.id, item2.Count), out list3, null, ignoreMyBags))
					{
						int itemsTotalCount2 = Item.GetItemsTotalCount(list3);
						if (itemsTotalCount2 > 0)
						{
							item.RemoveItemFromInventory(item2.id, itemsTotalCount2, new Func<Item, string, bool>(Item.IsItemTheSameByItemId), null, null, ignoreOtherBags);
							if (!list.Contains(item2))
							{
								list.Add(item2);
							}
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x000822B4 File Offset: 0x000804B4
	private HashSet<string> CollectUniqueItemIds(bool ignoreBags)
	{
		HashSet<string> hashSet = new HashSet<string>();
		for (int i = 0; i < this.inventory.Count; i++)
		{
			Item item = this.inventory[i];
			hashSet.Add(item.id);
			if (!ignoreBags && item.IsBag)
			{
				for (int j = 0; j < item.inventory.Count; j++)
				{
					hashSet.Add(item.inventory[j].id);
				}
			}
		}
		return hashSet;
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x00082331 File Offset: 0x00080531
	public bool AddItemsToInventory(Item inventoryItem, out List<Item> addedItems)
	{
		return this.AddItemsToInventory(inventoryItem.inventory, out addedItems);
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x00082340 File Offset: 0x00080540
	public bool AddItemsToInventory(List<Item> itemsToAdd)
	{
		List<Item> list;
		return this.AddItemsToInventory(itemsToAdd, out list);
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x00082358 File Offset: 0x00080558
	public bool AddItemsToInventory(List<Item> itemsToAdd, out List<Item> addedItems)
	{
		addedItems = new List<Item>();
		if (!this.CanAddItemsToInventory(itemsToAdd))
		{
			return false;
		}
		foreach (Item item in itemsToAdd)
		{
			List<Item> list;
			this.AddItemToInventory(item, out list, null, false);
			addedItems.AddRange(list);
		}
		return true;
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000823C8 File Offset: 0x000805C8
	public bool CanAddItemsToInventory(Item inventoryItem)
	{
		return this.CanAddItemsToInventory(inventoryItem.inventory);
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x000823D8 File Offset: 0x000805D8
	public bool CanAddItemsToInventory(List<Item> itemsToAdd)
	{
		List<ItemCount> list = new List<ItemCount>();
		foreach (Item item in itemsToAdd)
		{
			if (!item.IsEmpty)
			{
				list.Add(new ItemCount(item.Definition, item.count));
			}
		}
		return this.CanAddItemsToInventory(list);
	}

	// Token: 0x06001BE7 RID: 7143 RVA: 0x0008244C File Offset: 0x0008064C
	public bool CanAddItemsToInventory(List<ItemCount> itemCounts, out List<ItemCount> cantAddItemsCount)
	{
		cantAddItemsCount = new List<ItemCount>();
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Dictionary<string, ItemDef> dictionary2 = new Dictionary<string, ItemDef>();
		foreach (ItemCount itemCount in itemCounts)
		{
			WhiteListFilterSerializedItemProperty whiteListFilterSerializedItemProperty;
			BlackListFilterSerializedItemProperty blackListFilterSerializedItemProperty;
			if (!itemCount.IsEmpty && (!this.TryGetProperty<WhiteListFilterSerializedItemProperty>(out whiteListFilterSerializedItemProperty) || whiteListFilterSerializedItemProperty.WhiteList.Contains(itemCount.Def)) && (!this.TryGetProperty<BlackListFilterSerializedItemProperty>(out blackListFilterSerializedItemProperty) || !blackListFilterSerializedItemProperty.BlackList.Contains(itemCount.Def)))
			{
				if (!dictionary.TryAdd(itemCount.itemId, itemCount.count))
				{
					Dictionary<string, int> dictionary3 = dictionary;
					string text = itemCount.itemId;
					dictionary3[text] += itemCount.count;
				}
				dictionary2.TryAdd(itemCount.itemId, itemCount.Def);
			}
		}
		foreach (string text2 in new List<string>(dictionary.Keys))
		{
			int num = dictionary[text2];
			ItemDef itemDef;
			if (num != 0 && dictionary2.TryGetValue(text2, out itemDef))
			{
				int num2 = this.CanAddItemCountToInventory(itemDef, num, false, null, false);
				if (num2 != 0 && num2 <= num)
				{
					Dictionary<string, int> dictionary3 = dictionary;
					string text = text2;
					dictionary3[text] -= num2;
				}
			}
		}
		int num3 = (this.HasProperty<AutoExpandSerializedItemProperty>() ? 1000 : this.inventorySize) - this.InventoryFillSize;
		if (num3 > 0)
		{
			foreach (string text3 in new List<string>(dictionary.Keys))
			{
				if (num3 == 0)
				{
					break;
				}
				ItemDef itemDef2;
				if (dictionary2.TryGetValue(text3, out itemDef2))
				{
					while (dictionary[text3] > 0 && num3 > 0)
					{
						Dictionary<string, int> dictionary3 = dictionary;
						string text = text3;
						dictionary3[text] -= Mathf.Min(itemDef2.stackCount, dictionary[text3]);
						num3--;
					}
				}
			}
		}
		bool flag = true;
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			string key = keyValuePair.Key;
			int value = keyValuePair.Value;
			if (value > 0)
			{
				flag = false;
				ItemDef itemDef3;
				if (dictionary2.TryGetValue(key, out itemDef3))
				{
					cantAddItemsCount.Add(new ItemCount(itemDef3, value));
				}
			}
		}
		return flag;
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x00082704 File Offset: 0x00080904
	public bool CanAddItemsToInventory(List<ItemCount> itemCounts)
	{
		List<ItemCount> list;
		return this.CanAddItemsToInventory(itemCounts, out list);
	}

	// Token: 0x06001BE9 RID: 7145 RVA: 0x0008271A File Offset: 0x0008091A
	public bool HasItemsWithIds(List<NeedItemData> items, WgoData wgoData)
	{
		return this.HasItemsWithIds(items, 1, wgoData);
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x00082728 File Offset: 0x00080928
	public bool HasItemsWithIds(List<NeedItemData> items, int multiplicator = 1, WgoData wgoData = null)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>(items.Count);
		for (int i = 0; i < items.Count; i++)
		{
			int num;
			if (!dictionary.TryGetValue(items[i].Id, out num))
			{
				dictionary.Add(items[i].Id, items[i].GetCount(wgoData) * multiplicator);
			}
			else
			{
				dictionary[items[i].Id] = num + items[i].GetCount(wgoData) * multiplicator;
			}
		}
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			if (!this.HasItemQuantityInInventory(keyValuePair.Key, keyValuePair.Value))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001BEB RID: 7147 RVA: 0x00082808 File Offset: 0x00080A08
	public bool HasItemsByItemType(ItemType type)
	{
		return this.inventory.Exists((Item x) => x.Definition.type == type);
	}

	// Token: 0x06001BEC RID: 7148 RVA: 0x00081F10 File Offset: 0x00080110
	public bool HasItemByItemId(string itemId, int count)
	{
		return this.GetTotalCountInInventory(itemId, null, false) >= count;
	}

	// Token: 0x06001BED RID: 7149 RVA: 0x0008283C File Offset: 0x00080A3C
	public bool HasItemsByItemId(List<Item> items)
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (Item item in items)
		{
			if (dictionary.ContainsKey(item.id))
			{
				Dictionary<string, int> dictionary2 = dictionary;
				string id = item.id;
				dictionary2[id] += item.Count;
			}
			else
			{
				dictionary.TryAdd(item.id, item.Count);
			}
		}
		foreach (KeyValuePair<string, int> keyValuePair in dictionary)
		{
			if (keyValuePair.Value - this.GetTotalCountInInventory(keyValuePair.Key, null, false) > 0)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x00082928 File Offset: 0x00080B28
	public bool HasItemsByUniqueId(List<Item> items)
	{
		foreach (Item item in items)
		{
			if (item.count > this.GetTotalCountInInventory(item.UniqueId.Guid))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x00082990 File Offset: 0x00080B90
	public List<Item> RemoveAllItems()
	{
		List<Item> list = new List<Item>();
		for (int i = this.inventory.Count - 1; i >= 0; i--)
		{
			list.Add(this.RemoveItemFromInventoryByUID(this.inventory[i].UniqueId.Guid, -1));
		}
		this.inventory.Clear();
		this.CalculateInventoryFillSize();
		return list;
	}

	// Token: 0x06001BF0 RID: 7152 RVA: 0x000829F0 File Offset: 0x00080BF0
	public Item RemoveItemAt(int index)
	{
		if (index < 0 || index >= this.inventory.Count)
		{
			return null;
		}
		Item item = this.inventory[index];
		this.inventory.RemoveAt(index);
		this.CalculateInventoryFillSize();
		return item;
	}

	// Token: 0x06001BF1 RID: 7153 RVA: 0x00082A24 File Offset: 0x00080C24
	public bool RemoveItemsFromInventory(List<Item> items)
	{
		if (!this.HasItemsByItemId(items))
		{
			return false;
		}
		foreach (Item item in items)
		{
			this.RemoveItemFromInventory(item.id, item.count, null, null, null, false);
		}
		return false;
	}

	// Token: 0x06001BF2 RID: 7154 RVA: 0x00082A90 File Offset: 0x00080C90
	public bool RemoveItemsFromInventoryByUniqueId(List<Item> items)
	{
		if (!this.HasItemsByUniqueId(items))
		{
			return false;
		}
		foreach (Item item in items)
		{
			this.RemoveItemFromInventoryByUID(item.UniqueId.Guid, -1);
		}
		return true;
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x00082AF8 File Offset: 0x00080CF8
	private static bool IsItemTheSameByItemId(Item item, string itemId)
	{
		return item.id == itemId;
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x00082B06 File Offset: 0x00080D06
	private static bool IsItemTheSameByUId(Item item, Guid uniqueId)
	{
		return uniqueId == item.UniqueId.Guid;
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00082B19 File Offset: 0x00080D19
	private static bool IsItemTheSameByGroupId(Item item, string groupId)
	{
		return item.Definition.itemGroupIds.Contains(groupId);
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x00082B2C File Offset: 0x00080D2C
	private static bool IsItemTheSameByStarGroupId(Item item, string starGroupId)
	{
		List<ItemDef> list;
		return GameBalance.Me.starGroupItemsCache.TryGetValue(starGroupId, out list) && list.Contains(item.Definition);
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x00082B5C File Offset: 0x00080D5C
	private void CalculateInventoryFillSize()
	{
		this.inventoryFillSize = 0;
		foreach (Item item in this.inventory)
		{
			if (!item.IsEmpty)
			{
				this.inventoryFillSize++;
			}
			if (item.IsBag)
			{
				item.CalculateInventoryFillSize();
			}
		}
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x00082BD4 File Offset: 0x00080DD4
	private void InsertItemToInventory(Item item)
	{
		int num = this.inventory.BinarySearch(item, Comparer<Item>.Create(delegate(Item a, Item b)
		{
			int num2 = a.Definition.sortOrder.CompareTo(b.Definition.sortOrder);
			if (num2 != 0)
			{
				return num2;
			}
			int num3 = string.Compare(a.id, b.id, StringComparison.Ordinal);
			if (num3 != 0)
			{
				return num3;
			}
			return b.count.CompareTo(a.count);
		}));
		if (num < 0)
		{
			num = ~num;
		}
		this.inventory.Insert(num, item);
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x00082C28 File Offset: 0x00080E28
	[CompilerGenerated]
	internal static void <RemoveItemFromInventory>g__BagIteration|70_0(Item bag, ref Item.<>c__DisplayClass70_0 A_1)
	{
		if (!A_1.removeFullCount && A_1.leftToRemoveCount <= 0)
		{
			return;
		}
		int num = (A_1.removeFullCount ? (-1) : A_1.leftToRemoveCount);
		List<Item> list = bag.RemoveItemFromInventory(A_1.itemId, num, A_1.removeCondition, null, null, false);
		A_1.removedItems.AddRange(list);
		if (A_1.removeFullCount)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			A_1.leftToRemoveCount -= list[i].count;
		}
	}

	// Token: 0x04001A7A RID: 6778
	private const int AUTO_EXPAND_SIZE_LIMIT = 1000;

	// Token: 0x04001A7B RID: 6779
	[SerializeField]
	private int count;

	// Token: 0x04001A7C RID: 6780
	[SerializeField]
	private SGuid uniqueId;

	// Token: 0x04001A7D RID: 6781
	[SerializeField]
	private List<Item> inventory = new List<Item>();

	// Token: 0x04001A7E RID: 6782
	[SerializeField]
	private int inventorySize;

	// Token: 0x04001A7F RID: 6783
	[SerializeField]
	private int inventoryFillSize = -1;

	// Token: 0x04001A80 RID: 6784
	[SerializeReference]
	private Dictionary<Type, SerializedItemProperty> properties = new Dictionary<Type, SerializedItemProperty>();
}
