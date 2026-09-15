using System;
using System.Collections.Generic;
using Expressive.Expressions;

namespace Expressive.Operators.Grouping
{
	// Token: 0x0200004A RID: 74
	internal class ParenthesisCloseOperator : OperatorBase
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600016C RID: 364 RVA: 0x000078B6 File Offset: 0x00005AB6
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { ")" };
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000078C6 File Offset: 0x00005AC6
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return expressions[0] ?? expressions[1];
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000078D2 File Offset: 0x00005AD2
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.ParenthesisClose;
		}
	}
}
