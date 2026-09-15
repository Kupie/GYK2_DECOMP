using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000404 RID: 1028
public class OutputPreview
{
	// Token: 0x170004AA RID: 1194
	// (get) Token: 0x06001AD2 RID: 6866 RVA: 0x0007CBE4 File Offset: 0x0007ADE4
	public string IconId
	{
		get
		{
			if (!string.IsNullOrEmpty(this.customIconId))
			{
				return this.customIconId;
			}
			ItemDef itemDef = this.ResolveItemDef();
			if (itemDef != null && !string.IsNullOrEmpty(itemDef.iconId))
			{
				return itemDef.iconId;
			}
			return this.craftId;
		}
	}

	// Token: 0x170004AB RID: 1195
	// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0007CC2C File Offset: 0x0007AE2C
	public bool IsBigItemOutput
	{
		get
		{
			if (!string.IsNullOrEmpty(this.customIconId))
			{
				Sprite sprite = LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.customIconId, null);
				if (sprite != null)
				{
					return sprite.rect.width / sprite.rect.height > 1.5f;
				}
			}
			ItemDef itemDef = this.ResolveItemDef();
			return itemDef != null && itemDef.itemSize == ItemSize.Big;
		}
	}

	// Token: 0x06001AD4 RID: 6868 RVA: 0x0007CC9C File Offset: 0x0007AE9C
	private ItemDef ResolveItemDef()
	{
		if (this.itemDef != null)
		{
			return this.itemDef;
		}
		if (string.IsNullOrEmpty(this.itemId) || GameBalance.Me == null)
		{
			return null;
		}
		List<ItemDef> list;
		if (this.isStarOutput && GameBalance.Me.starGroupItemsCache.TryGetValue(this.itemId, out list) && list != null && list.Count > 0)
		{
			this.itemDef = list[0];
			return this.itemDef;
		}
		this.itemDef = GameBalance.Me.GetDataOrNull<ItemDef>(this.itemId);
		return this.itemDef;
	}

	// Token: 0x06001AD5 RID: 6869 RVA: 0x0007CD31 File Offset: 0x0007AF31
	public OutputPreview(string craftId, string itemId, bool isStarOutput, int count, int quality = -1, string customIconId = "")
	{
		this.craftId = craftId;
		this.itemId = itemId;
		this.isStarOutput = isStarOutput;
		this.count = count;
		this.quality = quality;
		this.customIconId = customIconId;
	}

	// Token: 0x040019EF RID: 6639
	public string craftId;

	// Token: 0x040019F0 RID: 6640
	public string itemId;

	// Token: 0x040019F1 RID: 6641
	public bool isStarOutput;

	// Token: 0x040019F2 RID: 6642
	public int count;

	// Token: 0x040019F3 RID: 6643
	public int quality;

	// Token: 0x040019F4 RID: 6644
	public string customIconId;

	// Token: 0x040019F5 RID: 6645
	private ItemDef itemDef;
}
