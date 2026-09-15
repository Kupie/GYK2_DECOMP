using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Bitwise;

namespace Expressive.Operators.Bitwise
{
	// Token: 0x0200004E RID: 78
	internal class BitwiseExclusiveOrOperator : OperatorBase
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00007A0E File Offset: 0x00005C0E
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "^" };
			}
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007A1E File Offset: 0x00005C1E
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new BitwiseExclusiveOrExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007A2C File Offset: 0x00005C2C
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.BitwiseXOr;
		}
	}
}
