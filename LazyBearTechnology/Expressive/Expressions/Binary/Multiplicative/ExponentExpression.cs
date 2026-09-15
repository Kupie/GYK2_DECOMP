using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Multiplicative
{
	// Token: 0x020000AD RID: 173
	internal class ExponentExpression : BinaryExpressionBase
	{
		// Token: 0x060002A7 RID: 679 RVA: 0x0000E745 File Offset: 0x0000C945
		public ExponentExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000E750 File Offset: 0x0000C950
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => Math.Pow(Convert.ToDouble(l), Convert.ToDouble(r)));
		}
	}
}
