using System;

// Token: 0x0200021F RID: 543
[Serializable]
public class HPAction : IAutoParsable
{
	// Token: 0x06000CE6 RID: 3302 RVA: 0x00040DED File Offset: 0x0003EFED
	public void EvaluateExpression(WgoData data)
	{
		this.expression.Evaluate(data);
	}

	// Token: 0x04000F77 RID: 3959
	public bool isValidAction;

	// Token: 0x04000F78 RID: 3960
	public int hp;

	// Token: 0x04000F79 RID: 3961
	public LazyExpression expression;
}
