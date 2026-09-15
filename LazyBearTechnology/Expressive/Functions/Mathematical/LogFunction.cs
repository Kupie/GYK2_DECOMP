using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000075 RID: 117
	internal class LogFunction : FunctionBase
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600020C RID: 524 RVA: 0x0000D1E0 File Offset: 0x0000B3E0
		public override string Name
		{
			get
			{
				return "Log";
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000D1E7 File Offset: 0x0000B3E7
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return Math.Log(Convert.ToDouble(parameters[0].Evaluate(base.Variables)), Convert.ToDouble(parameters[1].Evaluate(base.Variables)));
		}
	}
}
