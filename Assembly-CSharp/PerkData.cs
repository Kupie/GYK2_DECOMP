using System;
using LazyBearTechnology;

// Token: 0x02000496 RID: 1174
[Serializable]
public class PerkData : ObjectLinkedToDefinition<PerkDef>
{
	// Token: 0x06001F46 RID: 8006 RVA: 0x0009434A File Offset: 0x0009254A
	public PerkData()
	{
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x00094352 File Offset: 0x00092552
	public PerkData(string id)
		: base(id)
	{
	}

	// Token: 0x04001C0F RID: 7183
	public float currentDuration;

	// Token: 0x04001C10 RID: 7184
	public float tickTimer;
}
