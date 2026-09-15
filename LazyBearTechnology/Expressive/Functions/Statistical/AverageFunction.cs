using System;
using Expressive.Expressions;

namespace Expressive.Functions.Statistical
{
	// Token: 0x02000063 RID: 99
	internal class AverageFunction : FunctionBase
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000C805 File Offset: 0x0000AA05
		public override string Name
		{
			get
			{
				return "Average";
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C80C File Offset: 0x0000AA0C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			return MeanFunction.Evaluate(parameters, base.Variables);
		}
	}
}
