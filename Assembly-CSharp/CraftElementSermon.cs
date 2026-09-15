using System;
using System.Collections.Generic;

// Token: 0x0200026C RID: 620
public class CraftElementSermon : CraftElementT<SermonDef>
{
	// Token: 0x0600102B RID: 4139 RVA: 0x00052051 File Offset: 0x00050251
	public CraftElementSermon(string craftId, int count, List<NeedItemData> requirements, CraftParamsData paramsData)
		: base(craftId, count, requirements, paramsData)
	{
	}

	// Token: 0x0600102C RID: 4140 RVA: 0x0005205E File Offset: 0x0005025E
	public CraftElementSermon(CraftDefBase definition)
		: base(definition)
	{
	}

	// Token: 0x0600102D RID: 4141 RVA: 0x00052067 File Offset: 0x00050267
	protected CraftElementSermon(CraftElementSermon other, int count = 1)
		: base(other, count)
	{
	}

	// Token: 0x0600102E RID: 4142 RVA: 0x00052071 File Offset: 0x00050271
	public override CraftElementBase Clone(int count = 1)
	{
		return new CraftElementSermon(this, count);
	}

	// Token: 0x0600102F RID: 4143 RVA: 0x0005207A File Offset: 0x0005027A
	protected override CraftDefBase GetCraftDef()
	{
		return GameBalance.GetSermonDef(this.craftId);
	}
}
