using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000077 RID: 119
	internal class PowFunction : FunctionBase
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000D252 File Offset: 0x0000B452
		public override string Name
		{
			get
			{
				return "Pow";
			}
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000D259 File Offset: 0x0000B459
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			return Math.Pow(Convert.ToDouble(parameters[0].Evaluate(base.Variables)), Convert.ToDouble(parameters[1].Evaluate(base.Variables)));
		}
	}
}
