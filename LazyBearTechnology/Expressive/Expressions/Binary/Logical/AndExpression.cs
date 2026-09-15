using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Logical
{
	// Token: 0x020000B0 RID: 176
	internal class AndExpression : BinaryExpressionBase
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000E7BB File Offset: 0x0000C9BB
		public AndExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E7C6 File Offset: 0x0000C9C6
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return Convert.ToBoolean(lhsResult) && Convert.ToBoolean(rightHandSide.Evaluate(variables));
		}
	}
}
