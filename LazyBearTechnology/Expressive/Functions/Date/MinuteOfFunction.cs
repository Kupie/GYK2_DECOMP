using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x0200008F RID: 143
	internal sealed class MinuteOfFunction : FunctionBase
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000DD20 File Offset: 0x0000BF20
		public override string Name
		{
			get
			{
				return "MinuteOf";
			}
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000DD28 File Offset: 0x0000BF28
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDateTime(obj, context.CurrentCulture).Minute;
		}
	}
}
