using System;

// Token: 0x020001F3 RID: 499
[Serializable]
public class WhiteListItemFilter : ItemFilter
{
	// Token: 0x06000C56 RID: 3158 RVA: 0x0003E61C File Offset: 0x0003C81C
	public override bool Contains(ItemDef itemDef)
	{
		if (this.itemsIds.Count > 0 && !this.itemsIds.Contains(itemDef.id))
		{
			return false;
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
		return true;
	}
}
