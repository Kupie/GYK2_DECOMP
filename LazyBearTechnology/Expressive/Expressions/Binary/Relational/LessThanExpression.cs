using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Relational
{
	// Token: 0x020000A9 RID: 169
	internal class LessThanExpression : BinaryExpressionBase
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000E646 File Offset: 0x0000C846
		public LessThanExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000E651 File Offset: 0x0000C851
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return Comparison.CompareUsingMostPreciseType(lhsResult, rightHandSide.Evaluate(variables), base.Context) < 0;
		}
	}
}
