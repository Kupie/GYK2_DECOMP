using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Relational
{
	// Token: 0x020000A7 RID: 167
	internal class GreaterThanExpression : BinaryExpressionBase
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000E5F3 File Offset: 0x0000C7F3
		public GreaterThanExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000E5FE File Offset: 0x0000C7FE
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return Comparison.CompareUsingMostPreciseType(lhsResult, rightHandSide.Evaluate(variables), base.Context) > 0;
		}
	}
}
