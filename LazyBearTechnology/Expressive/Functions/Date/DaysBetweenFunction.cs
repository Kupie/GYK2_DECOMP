using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x0200008A RID: 138
	internal sealed class DaysBetweenFunction : FunctionBase
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000DB0F File Offset: 0x0000BD0F
		public override string Name
		{
			get
			{
				return "DaysBetween";
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000DB18 File Offset: 0x0000BD18
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
			return (Convert.ToDateTime(obj2, context.CurrentCulture) - dateTime).TotalDays;
		}
	}
}
