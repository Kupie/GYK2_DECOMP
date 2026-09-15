using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Additive
{
	// Token: 0x020000B8 RID: 184
	internal class AddExpression : BinaryExpressionBase
	{
		// Token: 0x060002BD RID: 701 RVA: 0x0000E945 File Offset: 0x0000CB45
		public AddExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E950 File Offset: 0x0000CB50
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			string text = lhsResult as string;
			if (text != null)
			{
				string text2 = text;
				object obj = rightHandSide.Evaluate(variables);
				return text2 + ((obj != null) ? obj.ToString() : null);
			}
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, new Func<object, object, object>(Numbers.Add));
		}
	}
}
