using System;
using LazyBearTechnology;

// Token: 0x0200021B RID: 539
[Serializable]
public class VendorOrderDef : BalanceBaseObject
{
	// Token: 0x04000F69 RID: 3945
	[AutoParse("item_id")]
	public string itemId;

	// Token: 0x04000F6A RID: 3946
	[AutoParse("count")]
	public int count;

	// Token: 0x04000F6B RID: 3947
	[AutoParse("is_urgent")]
	public bool isUrgent;

	// Token: 0x04000F6C RID: 3948
	[AutoParse("is_renewable")]
	public bool isRenewable;

	// Token: 0x04000F6D RID: 3949
	[AutoParse("happiness_reward")]
	public LazyExpression happinessReward;
}
