using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Bitwise
{
	// Token: 0x020000B4 RID: 180
	internal class BitwiseExclusiveOrExpression : BinaryExpressionBase
	{
		// Token: 0x060002B5 RID: 693 RVA: 0x0000E875 File Offset: 0x0000CA75
		public BitwiseExclusiveOrExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E880 File Offset: 0x0000CA80
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => (int)(Convert.ToUInt16(l) ^ Convert.ToUInt16(r)));
		}
	}
}
