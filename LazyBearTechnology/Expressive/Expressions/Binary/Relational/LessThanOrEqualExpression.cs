using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Relational
{
	// Token: 0x020000AA RID: 170
	internal class LessThanOrEqualExpression : BinaryExpressionBase
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x0000E66E File Offset: 0x0000C86E
		public LessThanOrEqualExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000E679 File Offset: 0x0000C879
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return Comparison.CompareUsingMostPreciseType(lhsResult, rightHandSide.Evaluate(variables), base.Context) <= 0;
		}
	}
}
