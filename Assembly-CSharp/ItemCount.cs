using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F4 RID: 500
[Serializable]
public class ItemCount
{
	// Token: 0x1700021B RID: 539
	// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0003E6B0 File Offset: 0x0003C8B0
	public bool IsEmpty
	{
		get
		{
			return string.IsNullOrEmpty(this.itemId) || this.count == 0 || this.itemDef == null;
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x06000C59 RID: 3161 RVA: 0x0003E6D4 File Offset: 0x0003C8D4
	public ItemDef Def
	{
		get
		{
			ItemDef itemDef;
			if ((itemDef = this.itemDef) == null)
			{
				itemDef = (this.itemDef = GameBalance.Me.GetData<ItemDef>(this.itemId));
			}
			return itemDef;
		}
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00021B94 File Offset: 0x0001FD94
	public ItemCount()
	{
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0003E704 File Offset: 0x0003C904
	public ItemCount(string itemId, int count)
	{
		this.itemId = itemId;
		this.count = count;
		this.itemDef = GameBalance.Me.GetData<ItemDef>(itemId);
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0003E72B File Offset: 0x0003C92B
	public ItemCount(ItemDef itemDef, int count)
	{
		this.itemId = itemDef.id;
		this.count = count;
		this.itemDef = itemDef;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0003E74D File Offset: 0x0003C94D
	public ItemCount(Item item)
		: this(item.Definition, item.Count)
	{
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x0003E764 File Offset: 0x0003C964
	public List<Item> CreateItems()
	{
		List<Item> list = new List<Item>();
		int num;
		for (int i = this.count; i > 0; i -= num)
		{
			num = Mathf.Min(i, this.itemDef.stackCount);
			list.Add(new Item(this.itemId, num));
		}
		return list;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0003E7B0 File Offset: 0x0003C9B0
	public static List<Item> CreateItems(List<ItemCount> items)
	{
		List<Item> list = new List<Item>();
		foreach (ItemCount itemCount in items)
		{
			list.AddRange(itemCount.CreateItems());
		}
		return list;
	}

	// Token: 0x04000E0D RID: 3597
	public string itemId;

	// Token: 0x04000E0E RID: 3598
	public int count;

	// Token: 0x04000E0F RID: 3599
	[NonSerialized]
	private ItemDef itemDef;
}
