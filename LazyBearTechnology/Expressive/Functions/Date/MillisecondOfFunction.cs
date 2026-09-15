using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x0200008D RID: 141
	internal sealed class MillisecondOfFunction : FunctionBase
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000DC54 File Offset: 0x0000BE54
		public override string Name
		{
			get
			{
				return "MillisecondOf";
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Millisecond;
		}
	}
}
