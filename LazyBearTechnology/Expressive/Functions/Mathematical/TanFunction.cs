using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200007E RID: 126
	internal class TanFunction : FunctionBase
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000D610 File Offset: 0x0000B810
		public override string Name
		{
			get
			{
				return "Tan";
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000D617 File Offset: 0x0000B817
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Tan(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
