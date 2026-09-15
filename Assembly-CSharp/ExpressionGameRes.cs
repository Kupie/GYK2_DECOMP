using System;

// Token: 0x0200022F RID: 559
[Serializable]
public class ExpressionGameRes : IAutoParsable
{
	// Token: 0x06000D35 RID: 3381 RVA: 0x00044161 File Offset: 0x00042361
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"[ExpressionGameRes: ",
			this.name,
			" ",
			this.expression.ToString(),
			"]"
		});
	}

	// Token: 0x0400105B RID: 4187
	public string name;

	// Token: 0x0400105C RID: 4188
	public LazyExpression expression;
}
