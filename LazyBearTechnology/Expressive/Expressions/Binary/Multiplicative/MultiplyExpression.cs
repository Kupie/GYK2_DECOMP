using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Multiplicative
{
	// Token: 0x020000AF RID: 175
	internal class MultiplyExpression : BinaryExpressionBase
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000E79A File Offset: 0x0000C99A
		public MultiplyExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000E7A5 File Offset: 0x0000C9A5
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, new Func<object, object, object>(Numbers.Multiply));
		}
	}
}
