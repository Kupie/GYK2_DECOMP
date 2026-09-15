using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200006C RID: 108
	internal class AtanFunction : FunctionBase
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000CF7C File Offset: 0x0000B17C
		public override string Name
		{
			get
			{
				return "Atan";
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000CF83 File Offset: 0x0000B183
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Atan(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
