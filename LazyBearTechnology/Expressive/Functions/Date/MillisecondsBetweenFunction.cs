using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x0200008E RID: 142
	internal sealed class MillisecondsBetweenFunction : FunctionBase
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000DCA7 File Offset: 0x0000BEA7
		public override string Name
		{
			get
			{
				return "MillisecondsBetween";
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000DCB0 File Offset: 0x0000BEB0
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
			return (Convert.ToDateTime(obj2, context.CurrentCulture) - dateTime).TotalMilliseconds;
		}
	}
}
