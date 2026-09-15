using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Binary.Conditional
{
	// Token: 0x020000B2 RID: 178
	internal class NullCoalescingExpression : BinaryExpressionBase
	{
		// Token: 0x060002B1 RID: 689 RVA: 0x0000E80D File Offset: 0x0000CA0D
		public NullCoalescingExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E818 File Offset: 0x0000CA18
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, (object l, object r) => l ?? r);
		}
	}
}
