using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	// Token: 0x02000097 RID: 151
	internal sealed class DoubleFunction : FunctionBase
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000E02F File Offset: 0x0000C22F
		public override string Name
		{
			get
			{
				return "Double";
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000E038 File Offset: 0x0000C238
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDouble(obj, context.CurrentCulture);
		}
	}
}
