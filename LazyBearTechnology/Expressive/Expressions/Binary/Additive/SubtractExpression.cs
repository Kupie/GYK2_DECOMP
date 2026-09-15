using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Additive
{
	// Token: 0x020000B9 RID: 185
	internal class SubtractExpression : BinaryExpressionBase
	{
		// Token: 0x060002BF RID: 703 RVA: 0x0000E995 File Offset: 0x0000CB95
		public SubtractExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E9A0 File Offset: 0x0000CBA0
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, new Func<object, object, object>(Numbers.Subtract));
		}
	}
}
