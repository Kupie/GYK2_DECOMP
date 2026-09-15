using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x0200008B RID: 139
	internal sealed class HourOfFunction : FunctionBase
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000DB88 File Offset: 0x0000BD88
		public override string Name
		{
			get
			{
				return "HourOf";
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000DB90 File Offset: 0x0000BD90
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Hour;
		}
	}
}
