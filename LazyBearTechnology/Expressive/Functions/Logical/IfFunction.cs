using System;
using Expressive.Expressions;

namespace Expressive.Functions.Logical
{
	// Token: 0x02000080 RID: 128
	internal class IfFunction : FunctionBase
	{
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000D67E File Offset: 0x0000B87E
		public override string Name
		{
			get
			{
				return "If";
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000D685 File Offset: 0x0000B885
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 3, 3);
			if (!Convert.ToBoolean(parameters[0].Evaluate(base.Variables)))
			{
				return parameters[2].Evaluate(base.Variables);
			}
			return parameters[1].Evaluate(base.Variables);
		}
	}
}
