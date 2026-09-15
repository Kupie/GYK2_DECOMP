using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Relational;

namespace Expressive.Operators.Relational
{
	// Token: 0x02000040 RID: 64
	internal class LessThanOperator : OperatorBase
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000076D4 File Offset: 0x000058D4
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "<" };
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000076E4 File Offset: 0x000058E4
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new LessThanExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000076F2 File Offset: 0x000058F2
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.LessThan;
		}
	}
}
