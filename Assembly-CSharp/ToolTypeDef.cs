using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000216 RID: 534
[Serializable]
public class ToolTypeDef : BalanceBaseObject
{
	// Token: 0x04000F4C RID: 3916
	[AutoParse("talent_ids")]
	public List<string> talentIds;
}
