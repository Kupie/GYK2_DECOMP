using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020001ED RID: 493
[Serializable]
public class GameResSystemDef : BalanceBaseObject
{
	// Token: 0x17000218 RID: 536
	// (get) Token: 0x06000C49 RID: 3145 RVA: 0x0003E401 File Offset: 0x0003C601
	public string ResId
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(this.overrodeResName))
			{
				return this.overrodeResName;
			}
			return this.id;
		}
	}

	// Token: 0x04000DF1 RID: 3569
	[AutoParse("res_name")]
	public string overrodeResName;

	// Token: 0x04000DF2 RID: 3570
	[AutoParse("start")]
	public LazyExpression start;

	// Token: 0x04000DF3 RID: 3571
	[AutoParse("min")]
	public LazyExpression min;

	// Token: 0x04000DF4 RID: 3572
	[AutoParse("max")]
	public LazyExpression max;

	// Token: 0x04000DF5 RID: 3573
	[AutoParse("expressions_on_increase")]
	public List<LazyExpression> expressionsOnIncrease = new List<LazyExpression>();

	// Token: 0x04000DF6 RID: 3574
	[AutoParse("expressions_on_decrease")]
	public List<LazyExpression> expressionsOnDecrease = new List<LazyExpression>();
}
