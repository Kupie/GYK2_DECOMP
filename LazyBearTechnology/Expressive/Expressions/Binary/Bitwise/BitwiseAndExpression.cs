using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Bitwise
{
	// Token: 0x020000B3 RID: 179
	internal class BitwiseAndExpression : BinaryExpressionBase
	{
		// Token: 0x060002B3 RID: 691 RVA: 0x0000E841 File Offset: 0x0000CA41
		public BitwiseAndExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E84C File Offset: 0x0000CA4C
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => (int)(Convert.ToUInt16(l) & Convert.ToUInt16(r)));
		}
	}
}
