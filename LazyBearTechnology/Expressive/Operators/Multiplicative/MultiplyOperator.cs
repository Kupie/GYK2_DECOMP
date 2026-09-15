using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Multiplicative;

namespace Expressive.Operators.Multiplicative
{
	// Token: 0x02000046 RID: 70
	internal class MultiplyOperator : OperatorBase
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000077ED File Offset: 0x000059ED
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "*", "×" };
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00007805 File Offset: 0x00005A05
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new MultiplyExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007813 File Offset: 0x00005A13
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Multiply;
		}
	}
}
