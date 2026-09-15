using System;
using Expressive.Expressions;

namespace Expressive.Functions.String
{
	// Token: 0x02000061 RID: 97
	internal class StartsWithFunction : FunctionBase
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000C70D File Offset: 0x0000A90D
		public override string Name
		{
			get
			{
				return "StartsWith";
			}
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000C714 File Offset: 0x0000A914
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 2, 2);
			string text = (string)parameters[0].Evaluate(base.Variables);
			string text2 = (string)parameters[1].Evaluate(base.Variables);
			if (text2 == null)
			{
				return false;
			}
			return text != null && text.StartsWith(text2, context.EqualityStringComparison);
		}
	}
}
