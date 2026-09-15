using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000090 RID: 144
	internal sealed class MinutesBetweenFunction : FunctionBase
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000DD73 File Offset: 0x0000BF73
		public override string Name
		{
			get
			{
				return "MinutesBetween";
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000DD7C File Offset: 0x0000BF7C
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
			return (Convert.ToDateTime(obj2, context.CurrentCulture) - dateTime).TotalMinutes;
		}
	}
}
