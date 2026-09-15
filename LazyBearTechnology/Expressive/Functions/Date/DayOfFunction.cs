using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000089 RID: 137
	internal sealed class DayOfFunction : FunctionBase
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000DABB File Offset: 0x0000BCBB
		public override string Name
		{
			get
			{
				return "DayOf";
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000DAC4 File Offset: 0x0000BCC4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Day;
		}
	}
}
