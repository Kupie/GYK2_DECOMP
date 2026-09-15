using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020001FB RID: 507
[Serializable]
public class MercenariesDef : BalanceBaseObject
{
	// Token: 0x04000E81 RID: 3713
	[AutoParse("money")]
	public int money;

	// Token: 0x04000E82 RID: 3714
	[AutoParse("after_pay_expr")]
	public List<LazyExpression> afterPayExpr;

	// Token: 0x04000E83 RID: 3715
	[AutoParse("after_win_expr")]
	public List<LazyExpression> afterWinExpr;

	// Token: 0x04000E84 RID: 3716
	[AutoParse("items")]
	public List<NeedItemData> needItems = new List<NeedItemData>();
}
