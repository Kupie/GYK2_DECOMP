using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Bitwise;

namespace Expressive.Operators.Bitwise
{
	// Token: 0x0200004D RID: 77
	internal class BitwiseAndOperator : OperatorBase
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600017B RID: 379 RVA: 0x000079E4 File Offset: 0x00005BE4
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "&" };
			}
		}

		// Token: 0x0600017C RID: 380 RVA: 0x000079F4 File Offset: 0x00005BF4
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new BitwiseAndExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007A02 File Offset: 0x00005C02
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.BitwiseAnd;
		}
	}
}
