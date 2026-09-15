using System;
using System.Collections.Generic;

// Token: 0x0200026E RID: 622
[Serializable]
public abstract class CraftElementT<T> : CraftElementBase where T : CraftDefBase
{
	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06001039 RID: 4153 RVA: 0x000521B5 File Offset: 0x000503B5
	public T Definition
	{
		get
		{
			return GameBalance.GetCraftDef<T>(this.craftId);
		}
	}

	// Token: 0x0600103A RID: 4154 RVA: 0x000521C2 File Offset: 0x000503C2
	public CraftElementT(string craftId, int count, List<NeedItemData> requirements, CraftParamsData paramsData)
		: base(craftId, count, requirements, paramsData)
	{
		this.craftId = craftId;
		this.count = count;
		this.requirements = requirements;
		this.paramsData = paramsData;
	}

	// Token: 0x0600103B RID: 4155 RVA: 0x000521EC File Offset: 0x000503EC
	public CraftElementT(CraftDefBase definition)
	{
		this.craftId = definition.id;
		this.count = 1;
		this.requirements = definition.needItems;
		this.paramsData = null;
	}

	// Token: 0x0600103C RID: 4156 RVA: 0x0005221A File Offset: 0x0005041A
	public CraftElementT(CraftDefBase definition, CraftParamsData paramsData)
	{
		this.craftId = definition.id;
		this.count = 1;
		this.requirements = definition.needItems;
		this.paramsData = paramsData;
	}

	// Token: 0x0600103D RID: 4157 RVA: 0x00052248 File Offset: 0x00050448
	protected CraftElementT(CraftElementT<T> other, int count = 1)
		: base(other, count)
	{
	}
}
