using System;

// Token: 0x020001F0 RID: 496
[Serializable]
public class BlackListItemFilter : ItemFilter
{
	// Token: 0x06000C50 RID: 3152 RVA: 0x0003E504 File Offset: 0x0003C704
	public override bool Contains(ItemDef itemDef)
	{
		if (this.itemsIds.Count > 0 && this.itemsIds.Contains(itemDef.id))
		{
			return true;
		}
		if (this.groupsIds.Count > 0)
		{
			bool flag = false;
			foreach (string text in itemDef.itemGroupIds)
			{
				if (this.groupsIds.Contains(text))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}
		return false;
	}
}
