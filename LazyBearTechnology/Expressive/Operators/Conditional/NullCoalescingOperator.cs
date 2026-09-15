using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Conditional;

namespace Expressive.Operators.Conditional
{
	// Token: 0x0200004C RID: 76
	internal class NullCoalescingOperator : OperatorBase
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000079BA File Offset: 0x00005BBA
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "??" };
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000079CA File Offset: 0x00005BCA
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new NullCoalescingExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000079D8 File Offset: 0x00005BD8
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.NullCoalescing;
		}
	}
}
