using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F9 RID: 505
[Serializable]
public class NeedItemData : IAutoParsable
{
	// Token: 0x17000221 RID: 545
	// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0003F25C File Offset: 0x0003D45C
	public ItemDef ItemDef
	{
		get
		{
			if (this.groupType != ItemGroup.None)
			{
				Debug.LogError(string.Format("Need Item is a group! Can not get {0}", typeof(ItemDef)));
				return null;
			}
			if (this.itemDef == null)
			{
				this.itemDef = GameBalance.Me.GetDataOrNull<ItemDef>(this.id);
			}
			return this.itemDef;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0003F2B0 File Offset: 0x0003D4B0
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x06000C7D RID: 3197 RVA: 0x0003F2B8 File Offset: 0x0003D4B8
	public bool IsEmpty
	{
		get
		{
			return string.IsNullOrEmpty(this.id) || this.count == null || !this.count.HasExpression || !this.count.HasPureValue;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000C7E RID: 3198 RVA: 0x0003F2EC File Offset: 0x0003D4EC
	public bool IsGroup
	{
		get
		{
			return this.groupType > ItemGroup.None;
		}
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0003F2F7 File Offset: 0x0003D4F7
	public int GetCount(WgoData wgoData = null)
	{
		if (this.count == null || !this.count.HasExpression)
		{
			return 1;
		}
		if (wgoData == null)
		{
			return this.count.EvaluateInt();
		}
		return this.count.EvaluateInt(wgoData);
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0003F32C File Offset: 0x0003D52C
	public bool TryGetGroupItemDefs(out List<ItemDef> groupItemDefs)
	{
		groupItemDefs = null;
		ItemGroup itemGroup = this.groupType;
		if (itemGroup != ItemGroup.Common)
		{
			return itemGroup == ItemGroup.Star && GameBalance.Me.starGroupItemsCache.TryGetValue(this.id, out groupItemDefs);
		}
		return GameBalance.Me.groupItemsCache.TryGetValue(this.id, out groupItemDefs);
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0003F37C File Offset: 0x0003D57C
	public NeedItemData()
	{
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0003F38F File Offset: 0x0003D58F
	public NeedItemData(string id, int count)
	{
		this.Reinitialize(id, count);
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0003F3AA File Offset: 0x0003D5AA
	public NeedItemData(string id, LazyExpression count)
	{
		this.Reinitialize(id, count);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x0003F3C8 File Offset: 0x0003D5C8
	public static float GetQualitySum(List<NeedItemData> needItems)
	{
		float num = 0f;
		int num2 = 0;
		foreach (NeedItemData needItemData in needItems)
		{
			if (needItemData.groupType == ItemGroup.None && needItemData.ItemDef.qualityType == ItemDef.QualityType.Star)
			{
				num += (float)needItemData.ItemDef.quality;
				num2++;
			}
		}
		if (num2 > 0)
		{
			num /= (float)num2;
		}
		return num;
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0003F44C File Offset: 0x0003D64C
	public static int GetCraftStartTicksBonusValue(List<NeedItemData> needItems)
	{
		int num = 0;
		foreach (NeedItemData needItemData in needItems)
		{
			if (needItemData.groupType == ItemGroup.None && needItemData.ItemDef.qualityType == ItemDef.QualityType.Star)
			{
				num += needItemData.ItemDef.quality - 1;
			}
		}
		return num;
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0003F4BC File Offset: 0x0003D6BC
	public bool Equals(NeedItemData other)
	{
		if (this.id == other.id)
		{
			LazyExpression lazyExpression = this.count;
			string text = ((lazyExpression != null) ? lazyExpression.GetRawExpressionString() : null) ?? string.Empty;
			LazyExpression lazyExpression2 = other.count;
			if (text == (((lazyExpression2 != null) ? lazyExpression2.GetRawExpressionString() : null) ?? string.Empty))
			{
				return this.groupType == other.groupType;
			}
		}
		return false;
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0003F529 File Offset: 0x0003D729
	public void Reinitialize(string id, int count)
	{
		this.Reinitialize(id, new LazyExpression(count.ToString()));
	}

	// Token: 0x06000C88 RID: 3208 RVA: 0x0003F540 File Offset: 0x0003D740
	public void Reinitialize(string id, LazyExpression count)
	{
		this.id = id;
		this.count = count ?? new LazyExpression();
		if (GameBalance.Me.starGroupItemsCache.ContainsKey(id))
		{
			this.groupType = ItemGroup.Star;
			return;
		}
		if (GameBalance.Me.groupItemsCache.ContainsKey(id))
		{
			this.groupType = ItemGroup.Common;
		}
	}

	// Token: 0x06000C89 RID: 3209 RVA: 0x0003F597 File Offset: 0x0003D797
	public override string ToString()
	{
		return string.Format("{0}={1}", this.id, this.count);
	}

	// Token: 0x04000E78 RID: 3704
	public string id;

	// Token: 0x04000E79 RID: 3705
	public LazyExpression count = new LazyExpression();

	// Token: 0x04000E7A RID: 3706
	public ItemGroup groupType;

	// Token: 0x04000E7B RID: 3707
	[NonSerialized]
	private ItemDef itemDef;
}
