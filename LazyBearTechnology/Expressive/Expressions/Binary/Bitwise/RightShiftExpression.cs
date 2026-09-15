using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Bitwise
{
	// Token: 0x020000B7 RID: 183
	internal class RightShiftExpression : BinaryExpressionBase
	{
		// Token: 0x060002BB RID: 699 RVA: 0x0000E911 File Offset: 0x0000CB11
		public RightShiftExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000E91C File Offset: 0x0000CB1C
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => Convert.ToUInt16(l) >> (int)Convert.ToUInt16(r));
		}
	}
}
