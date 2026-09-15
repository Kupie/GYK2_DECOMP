using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Relational
{
	// Token: 0x020000A6 RID: 166
	internal class EqualExpression : BinaryExpressionBase
	{
		// Token: 0x06000298 RID: 664 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public EqualExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000E5AC File Offset: 0x0000C7AC
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			if (lhsResult == null)
			{
				return rightHandSide.Evaluate(variables) == null;
			}
			object obj = rightHandSide.Evaluate(variables);
			if (obj == null)
			{
				return false;
			}
			return Comparison.CompareUsingMostPreciseType(lhsResult, obj, base.Context) == 0;
		}
	}
}
