using System;
using System.Collections;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200006F RID: 111
	internal class CountFunction : FunctionBase
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000D062 File Offset: 0x0000B262
		public override string Name
		{
			get
			{
				return "Count";
			}
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D06C File Offset: 0x0000B26C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, -1, 1);
			int num = 0;
			foreach (IExpression expression in parameters)
			{
				int num2 = 1;
				ICollection collection = expression.Evaluate(base.Variables) as ICollection;
				if (collection != null)
				{
					num2 = collection.Count;
				}
				num += num2;
			}
			return num;
		}
	}
}
