using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Relational;

namespace Expressive.Operators.Relational
{
	// Token: 0x02000042 RID: 66
	internal class NotEqualOperator : OperatorBase
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007726 File Offset: 0x00005926
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "!=", "<>" };
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000773E File Offset: 0x0000593E
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new NotEqualExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000774C File Offset: 0x0000594C
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.NotEqual;
		}
	}
}
