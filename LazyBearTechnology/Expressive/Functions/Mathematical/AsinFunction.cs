using System;
using Expressive.Expressions;

namespace Expressive.Functions.Mathematical
{
	// Token: 0x0200006B RID: 107
	internal class AsinFunction : FunctionBase
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000CF45 File Offset: 0x0000B145
		public override string Name
		{
			get
			{
				return "Asin";
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000CF4C File Offset: 0x0000B14C
		public override object Evaluate(IExpression[] parameters, Context context)
		{
			base.ValidateParameterCount(parameters, 1, 1);
			return Math.Asin(Convert.ToDouble(parameters[0].Evaluate(base.Variables)));
		}
	}
}
