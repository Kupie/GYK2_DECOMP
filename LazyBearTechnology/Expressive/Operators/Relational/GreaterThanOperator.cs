using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Relational;

namespace Expressive.Operators.Relational
{
	// Token: 0x0200003E RID: 62
	internal class GreaterThanOperator : OperatorBase
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00007682 File Offset: 0x00005882
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { ">" };
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007692 File Offset: 0x00005892
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new GreaterThanExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000076A0 File Offset: 0x000058A0
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.GreaterThan;
		}
	}
}
