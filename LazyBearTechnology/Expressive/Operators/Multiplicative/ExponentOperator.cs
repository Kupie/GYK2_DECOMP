using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Multiplicative;

namespace Expressive.Operators.Multiplicative
{
	// Token: 0x02000044 RID: 68
	internal class ExponentOperator : OperatorBase
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00007789 File Offset: 0x00005989
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "^", "‸" };
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000077A1 File Offset: 0x000059A1
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new ExponentExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x000077AF File Offset: 0x000059AF
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Multiply;
		}
	}
}
