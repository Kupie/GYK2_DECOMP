using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x0200008C RID: 140
	internal sealed class HoursBetweenFunction : FunctionBase
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000DBDB File Offset: 0x0000BDDB
		public override string Name
		{
			get
			{
				return "HoursBetween";
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			object obj = parameters[0].Evaluate(base.Variables);
			object obj2 = parameters[1].Evaluate(base.Variables);
			if (obj == null || obj2 == null)
			{
				return null;
			}
			DateTime dateTime = Convert.ToDateTime(obj, context.CurrentCulture);
			return (Convert.ToDateTime(obj2, context.CurrentCulture) - dateTime).TotalHours;
		}
	}
}
