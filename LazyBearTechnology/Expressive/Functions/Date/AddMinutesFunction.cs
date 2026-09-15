using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000085 RID: 133
	internal sealed class AddMinutesFunction : FunctionBase
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000D8EB File Offset: 0x0000BAEB
		public override string Name
		{
			get
			{
				return "AddMinutes";
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
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
			return dateTime.AddMinutes(num);
		}
	}
}
