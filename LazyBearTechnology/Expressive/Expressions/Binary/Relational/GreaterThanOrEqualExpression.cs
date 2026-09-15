using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Relational
{
	// Token: 0x020000A8 RID: 168
	internal class GreaterThanOrEqualExpression : BinaryExpressionBase
	{
		// Token: 0x0600029C RID: 668 RVA: 0x0000E61B File Offset: 0x0000C81B
		public GreaterThanOrEqualExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000E626 File Offset: 0x0000C826
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return Comparison.CompareUsingMostPreciseType(lhsResult, rightHandSide.Evaluate(variables), base.Context) >= 0;
		}
	}
}
