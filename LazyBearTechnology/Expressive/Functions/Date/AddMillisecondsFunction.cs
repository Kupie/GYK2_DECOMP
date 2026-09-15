using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000084 RID: 132
	internal sealed class AddMillisecondsFunction : FunctionBase
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000D877 File Offset: 0x0000BA77
		public override string Name
		{
			get
			{
				return "AddMilliseconds";
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000D880 File Offset: 0x0000BA80
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
			return dateTime.AddMilliseconds(num);
		}
	}
}
