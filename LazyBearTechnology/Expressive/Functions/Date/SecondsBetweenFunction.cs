using System;
using Expressive.Expressions;

namespace Expressive.Functions.Date
{
	// Token: 0x02000093 RID: 147
	internal sealed class SecondsBetweenFunction : FunctionBase
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000DE93 File Offset: 0x0000C093
		public override string Name
		{
			get
			{
				return "SecondsBetween";
			}
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000DE9C File Offset: 0x0000C09C
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
			return (Convert.ToDateTime(obj2, context.CurrentCulture) - dateTime).TotalSeconds;
		}
	}
}
