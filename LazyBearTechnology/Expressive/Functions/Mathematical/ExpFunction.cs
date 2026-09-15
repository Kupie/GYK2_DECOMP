using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000071 RID: 113
	internal class ExpFunction : FunctionBase
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000D0F1 File Offset: 0x0000B2F1
		public override string Name
		{
			get
			{
				return "Exp";
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000D0F8 File Offset: 0x0000B2F8
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Exp(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
