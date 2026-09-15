using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200007B RID: 123
	internal class SinFunction : FunctionBase
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000D4B6 File Offset: 0x0000B6B6
		public override string Name
		{
			get
			{
				return "Sin";
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000D4BD File Offset: 0x0000B6BD
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Sin(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
