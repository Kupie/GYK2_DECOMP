using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x0200005B RID: 91
	internal class EndsWithFunction : FunctionBase
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0000C2BA File Offset: 0x0000A4BA
		public override string Name
		{
			get
			{
				return "EndsWith";
			}
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000C2C4 File Offset: 0x0000A4C4
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			string text = (string)parameters[0].Evaluate(base.Variables);
			string text2 = (string)parameters[1].Evaluate(base.Variables);
			if (text2 == null)
			{
				return false;
			}
			return text != null && text.EndsWith(text2, context.EqualityStringComparison);
		}
	}
}
