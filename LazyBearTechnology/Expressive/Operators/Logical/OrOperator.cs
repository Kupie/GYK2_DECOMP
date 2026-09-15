using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Logical;

namespace Expressive.Operators.Logical
{
	// Token: 0x02000049 RID: 73
	internal class OrOperator : OperatorBase
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007885 File Offset: 0x00005A85
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "||", "or" };
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000789D File Offset: 0x00005A9D
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new OrExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000078AB File Offset: 0x00005AAB
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Or;
		}
	}
}
