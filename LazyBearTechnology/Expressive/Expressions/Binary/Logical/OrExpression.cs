using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Logical
{
	// Token: 0x020000B1 RID: 177
	internal class OrExpression : BinaryExpressionBase
	{
		// Token: 0x060002AF RID: 687 RVA: 0x0000E7E4 File Offset: 0x0000C9E4
		public OrExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E7EF File Offset: 0x0000C9EF
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return Convert.ToBoolean(lhsResult) || Convert.ToBoolean(rightHandSide.Evaluate(variables));
		}
	}
}
