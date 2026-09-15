using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Bitwise;

namespace Expressive.Operators.Bitwise
{
	// Token: 0x02000051 RID: 81
	internal class RightShiftOperator : OperatorBase
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00007A8C File Offset: 0x00005C8C
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { ">>" };
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00007A9C File Offset: 0x00005C9C
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new RightShiftExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00007AAA File Offset: 0x00005CAA
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.RightShift;
		}
	}
}
