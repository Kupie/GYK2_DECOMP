using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Binary.Multiplicative
{
	// Token: 0x020000AC RID: 172
	internal class DivideExpression : BinaryExpressionBase
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000E6EB File Offset: 0x0000C8EB
		public DivideExpression(IExpression lhs, IExpression rhs, Context context)
			: base(lhs, rhs, context)
		{
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000E6F6 File Offset: 0x0000C8F6
		protected override object EvaluateImpl(object lhsResult, IExpression rightHandSide, IDictionary<string, object> variables)
		{
			return BinaryExpressionBase.EvaluateAggregates(lhsResult, rightHandSide, variables, delegate(object l, object r)
			{
				if (l != null && r != null && !DivideExpression.IsReal(l) && !DivideExpression.IsReal(r))
				{
					return Numbers.Divide(Convert.ToDouble(l), r);
				}
				return Numbers.Divide(l, r);
			});
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000E720 File Offset: 0x0000C920
		private static bool IsReal(object value)
		{
			TypeCode typeCode = TypeHelper.GetTypeCode(value);
			return typeCode == TypeCode.Decimal || typeCode == TypeCode.Double || typeCode == TypeCode.Single;
		}
	}
}
