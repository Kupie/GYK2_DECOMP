using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Bitwise
{
	// Token: 0x020000B5 RID: 181
	internal class BitwiseOrExpression : BinaryExpressionBase
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000E8A9 File Offset: 0x0000CAA9
		public BitwiseOrExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E8B4 File Offset: 0x0000CAB4
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => (int)(Convert.ToUInt16(l) | Convert.ToUInt16(r)));
		}
	}
}
