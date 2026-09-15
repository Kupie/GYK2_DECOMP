using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	// Token: 0x02000099 RID: 153
	internal sealed class LongFunction : FunctionBase
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000E0C7 File Offset: 0x0000C2C7
		public override string Name
		{
			get
			{
				return "Long";
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000E0D0 File Offset: 0x0000C2D0
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToInt64(obj, context.CurrentCulture);
		}
	}
}
