using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020005EC RID: 1516
[Serializable]
public class ZombieTalentData : ObjectLinkedToDefinition<TalentDef>
{
	// Token: 0x06002825 RID: 10277 RVA: 0x000BAEDB File Offset: 0x000B90DB
	public ZombieTalentData(string talentId)
		: base(talentId)
	{
		this.curTalentValue = 0;
	}

	// Token: 0x040021CB RID: 8651
	public int curTalentValue;

	// Token: 0x040021CC RID: 8652
	public List<string> studiedLevelUps = new List<string>();
}
