using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000079 RID: 121
	internal class RoundFunction : FunctionBase
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000D355 File Offset: 0x0000B555
		public override string Name
		{
			get
			{
				return "Round";
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000D35C File Offset: 0x0000B55C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return Math.Round(Convert.ToDouble(parameters[0].Evaluate(base.Variables)), Convert.ToInt32(parameters[1].Evaluate(base.Variables)));
		}
	}
}
