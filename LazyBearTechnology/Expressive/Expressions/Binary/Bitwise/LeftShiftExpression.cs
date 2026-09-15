using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Bitwise
{
	// Token: 0x020000B6 RID: 182
	internal class LeftShiftExpression : BinaryExpressionBase
	{
		// Token: 0x060002B9 RID: 697 RVA: 0x0000E8DD File Offset: 0x0000CADD
		public LeftShiftExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => (int)Convert.ToUInt16(l) << (int)Convert.ToUInt16(r));
		}
	}
}
