using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200006E RID: 110
	internal class CosFunction : FunctionBase
	{
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000D02B File Offset: 0x0000B22B
		public override string Name
		{
			get
			{
				return "Cos";
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D032 File Offset: 0x0000B232
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Cos(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
