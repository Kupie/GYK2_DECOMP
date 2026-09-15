using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200007C RID: 124
	internal class SqrtFunction : FunctionBase
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000D4ED File Offset: 0x0000B6ED
		public override string Name
		{
			get
			{
				return "Sqrt";
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000D4F4 File Offset: 0x0000B6F4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Sqrt(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
