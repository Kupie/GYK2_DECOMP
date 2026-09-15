using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000091 RID: 145
	internal sealed class MonthOfFunction : FunctionBase
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000DDEC File Offset: 0x0000BFEC
		public override string Name
		{
			get
			{
				return "MonthOf";
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000DDF4 File Offset: 0x0000BFF4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Month;
		}
	}
}
