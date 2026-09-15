using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Multiplicative
{
	// Token: 0x020000AE RID: 174
	internal class ModulusExpression : BinaryExpressionBase
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0000E779 File Offset: 0x0000C979
		public ModulusExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000E784 File Offset: 0x0000C984
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, new Func<object, object, object>(Numbers.Modulus));
		}
	}
}
