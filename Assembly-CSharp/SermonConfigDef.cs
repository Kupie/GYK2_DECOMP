using System;
using LazyBearTechnology;

// Token: 0x02000207 RID: 519
[Serializable]
public class SermonConfigDef : BalanceBaseObject
{
	// Token: 0x04000ED5 RID: 3797
	[AutoParse("cur_zone_id")]
	public string curWorldZoneId;

	// Token: 0x04000ED6 RID: 3798
	[AutoParse("attached_zone_id")]
	public string attachedWorldZoneId;

	// Token: 0x04000ED7 RID: 3799
	[AutoParse("reward_box_id")]
	public string rewardBoxId;
}
