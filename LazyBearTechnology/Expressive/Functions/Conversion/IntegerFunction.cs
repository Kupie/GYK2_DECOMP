using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	// Token: 0x02000098 RID: 152
	internal sealed class IntegerFunction : FunctionBase
	{
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0000E07B File Offset: 0x0000C27B
		public override string Name
		{
			get
			{
				return "Integer";
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000E084 File Offset: 0x0000C284
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToInt32(obj, context.CurrentCulture);
		}
	}
}
