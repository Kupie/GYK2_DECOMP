using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000092 RID: 146
	internal sealed class SecondOfFunction : FunctionBase
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0000DE3F File Offset: 0x0000C03F
		public override string Name
		{
			get
			{
				return "SecondOf";
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000DE48 File Offset: 0x0000C048
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Second;
		}
	}
}
