using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Unary.Logical;

namespace Expressive.Operators.Logical
{
	// Token: 0x02000048 RID: 72
	internal class NotOperator : OperatorBase
	{
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007850 File Offset: 0x00005A50
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "!", "not" };
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00007868 File Offset: 0x00005A68
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new NotExpression(expressions[0] ?? expressions[1]);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007879 File Offset: 0x00005A79
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Not;
		}
	}
}
