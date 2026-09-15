using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000083 RID: 131
	internal sealed class AddHoursFunction : FunctionBase
	{
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000D803 File Offset: 0x0000BA03
		public override string Name
		{
			get
			{
				return "AddHours";
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000D80C File Offset: 0x0000BA0C
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
			return dateTime.AddHours(num);
		}
	}
}
