using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000074 RID: 116
	internal class Log10Function : FunctionBase
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000D1A9 File Offset: 0x0000B3A9
		public override string Name
		{
			get
			{
				return "Log10";
			}
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000D1B0 File Offset: 0x0000B3B0
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Log10(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
