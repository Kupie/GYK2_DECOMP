using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000086 RID: 134
	internal sealed class AddMonthsFunction : FunctionBase
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0000D95F File Offset: 0x0000BB5F
		public override string Name
		{
			get
			{
				return "AddMonths";
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000D968 File Offset: 0x0000BB68
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
			return dateTime.AddMonths(num);
		}
	}
}
