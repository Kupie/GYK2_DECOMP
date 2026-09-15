using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x02000070 RID: 112
	internal class EFunction : FunctionBase
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000D0C9 File Offset: 0x0000B2C9
		public override string Name
		{
			get
			{
				return "E";
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D0D0 File Offset: 0x0000B2D0
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 0, 0);
			return 2.718281828459045;
		}
	}
}
