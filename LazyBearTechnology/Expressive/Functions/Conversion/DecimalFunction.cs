using System;
using Expressive.Expressions;

namespace Expressive.Functions.Conversion
{
	// Token: 0x02000096 RID: 150
	internal sealed class DecimalFunction : FunctionBase
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000DFE5 File Offset: 0x0000C1E5
		public override string Name
		{
			get
			{
				return "Decimal";
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			object obj = parameters[0].Evaluate(base.Variables);
			if (obj == null)
			{
				return null;
			}
			return Convert.ToDecimal(obj, context.CurrentCulture);
		}
	}
}
