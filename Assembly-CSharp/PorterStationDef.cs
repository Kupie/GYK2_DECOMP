using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020001FF RID: 511
[Serializable]
public class PorterStationDef : BalanceBaseObject
{
	// Token: 0x04000EA3 RID: 3747
	[AutoParse("items")]
	public List<NeedItemData> items = new List<NeedItemData>();

	// Token: 0x04000EA4 RID: 3748
	[AutoParse("target_world_zone")]
	public string targetWorldZone;

	// Token: 0x04000EA5 RID: 3749
	[AutoParse("custom_targets")]
	public List<string> customTargets = new List<string>();
}
