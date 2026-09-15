using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Relational
{
	// Token: 0x020000AB RID: 171
	internal class NotEqualExpression : BinaryExpressionBase
	{
		// Token: 0x060002A2 RID: 674 RVA: 0x0000E699 File Offset: 0x0000C899
		public NotEqualExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			if (lhsResult == null)
			{
				return rightHandSide.Evaluate(variables) != null;
			}
			object obj = rightHandSide.Evaluate(variables);
			if (obj == null)
			{
				return true;
			}
			return Comparison.CompareUsingMostPreciseType(lhsResult, obj, base.Context) != 0;
		}
	}
}
