using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F1 RID: 497
[Serializable]
public class ItemFilter
{
	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0003E5A0 File Offset: 0x0003C7A0
	public bool IsEmpty
	{
		get
		{
			return this.itemsIds.Count == 0 && this.groupsIds.Count == 0;
		}
	}

	// Token: 0x06000C53 RID: 3155 RVA: 0x0003E5BF File Offset: 0x0003C7BF
	public void AddElement(string id, ItemFilter.ItemFilterElementType elementType)
	{
		if (elementType != ItemFilter.ItemFilterElementType.Item)
		{
			if (elementType != ItemFilter.ItemFilterElementType.Group)
			{
				return;
			}
			if (!this.groupsIds.Contains(id))
			{
				this.groupsIds.Add(id);
			}
		}
		else if (!this.itemsIds.Contains(id))
		{
			this.itemsIds.Add(id);
			return;
		}
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool Contains(ItemDef itemDef)
	{
		return false;
	}

	// Token: 0x04000E08 RID: 3592
	[SerializeField]
	public List<string> itemsIds = new List<string>();

	// Token: 0x04000E09 RID: 3593
	[SerializeField]
	public List<string> groupsIds = new List<string>();

	// Token: 0x020001F2 RID: 498
	public enum ItemFilterElementType
	{
		// Token: 0x04000E0B RID: 3595
		Item,
		// Token: 0x04000E0C RID: 3596
		Group
	}
}
