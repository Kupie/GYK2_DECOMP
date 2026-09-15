using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Relational;

namespace Expressive.Operators.Relational
{
	// Token: 0x0200003F RID: 63
	internal class GreaterThanOrEqualOperator : OperatorBase
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000140 RID: 320 RVA: 0x000076AB File Offset: 0x000058AB
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { ">=" };
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000076BB File Offset: 0x000058BB
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new GreaterThanOrEqualExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000076C9 File Offset: 0x000058C9
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.GreaterThanOrEqual;
		}
	}
}
