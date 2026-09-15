using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000088 RID: 136
	internal sealed class AddYearsFunction : FunctionBase
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000DA47 File Offset: 0x0000BC47
		public override string Name
		{
			get
			{
				return "AddYears";
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000DA50 File Offset: 0x0000BC50
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
			int num = Convert.ToInt32(obj2, context.CurrentCulture);
			return dateTime.AddYears(num);
		}
	}
}
