using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000082 RID: 130
	internal sealed class AddDaysFunction : FunctionBase
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000233 RID: 563 RVA: 0x0000D790 File Offset: 0x0000B990
		public override string Name
		{
			get
			{
				return "AddDays";
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000D798 File Offset: 0x0000B998
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
			double num = Convert.ToDouble(obj2, context.CurrentCulture);
			return dateTime.AddDays(num);
		}
	}
}
