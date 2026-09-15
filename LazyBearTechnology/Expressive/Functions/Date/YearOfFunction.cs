using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000094 RID: 148
	internal sealed class YearOfFunction : FunctionBase
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public override string Name
		{
			get
			{
				return "YearOf";
			}
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000DF14 File Offset: 0x0000C114
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Year;
		}
	}
}
