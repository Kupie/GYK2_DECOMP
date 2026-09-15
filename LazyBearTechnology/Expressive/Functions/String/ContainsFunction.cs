using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x0200005A RID: 90
	internal class ContainsFunction : FunctionBase
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000C244 File Offset: 0x0000A444
		public override string Name
		{
			get
			{
				return "Contains";
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000C24C File Offset: 0x0000A44C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			string text = (string)parameters[0].Evaluate(base.Variables);
			string text2 = (string)parameters[1].Evaluate(base.Variables);
			if (text2 == null)
			{
				return false;
			}
			return text != null && text.IndexOf(text2, context.EqualityStringComparison) >= 0;
		}
	}
}
