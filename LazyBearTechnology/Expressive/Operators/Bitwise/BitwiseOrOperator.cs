using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Bitwise;

namespace Expressive.Operators.Bitwise
{
	// Token: 0x0200004F RID: 79
	internal class BitwiseOrOperator : OperatorBase
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007A38 File Offset: 0x00005C38
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "|" };
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007A48 File Offset: 0x00005C48
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new BitwiseOrExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007A56 File Offset: 0x00005C56
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.BitwiseOr;
		}
	}
}
