using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000087 RID: 135
	internal sealed class AddSecondsFunction : FunctionBase
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000D9D3 File Offset: 0x0000BBD3
		public override string Name
		{
			get
			{
				return "AddSeconds";
			}
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000D9DC File Offset: 0x0000BBDC
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
			return dateTime.AddSeconds(num);
		}
	}
}
