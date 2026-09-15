using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Relational;

namespace Expressive.Operators.Relational
{
	// Token: 0x0200003D RID: 61
	internal class EqualOperator : OperatorBase
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00007651 File Offset: 0x00005851
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "=", "==" };
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007669 File Offset: 0x00005869
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new EqualExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007677 File Offset: 0x00005877
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Equal;
		}
	}
}
