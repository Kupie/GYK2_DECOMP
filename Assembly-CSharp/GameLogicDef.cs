using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020001EB RID: 491
[Serializable]
public class GameLogicDef : BalanceBaseObject
{
	// Token: 0x04000DE5 RID: 3557
	public GameLogicStartType gameLogicStartType;

	// Token: 0x04000DE6 RID: 3558
	[AutoParse("start_time")]
	public float startTime;

	// Token: 0x04000DE7 RID: 3559
	[AutoParse("period_time")]
	public float periodTime;

	// Token: 0x04000DE8 RID: 3560
	[AutoParse("day_number")]
	public string dayNumber;

	// Token: 0x04000DE9 RID: 3561
	[AutoParse("day_time")]
	public float dayTime;

	// Token: 0x04000DEA RID: 3562
	[AutoParse("condition")]
	public LazyExpression condition;

	// Token: 0x04000DEB RID: 3563
	[AutoParse("exec_expressions")]
	public List<LazyExpression> execExpressions = new List<LazyExpression>();

	// Token: 0x04000DEC RID: 3564
	[AutoParse("exec_fs")]
	public string execFlowscriptName;
}
