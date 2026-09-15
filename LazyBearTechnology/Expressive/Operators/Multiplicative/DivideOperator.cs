using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Multiplicative;

namespace Expressive.Operators.Multiplicative
{
	// Token: 0x02000043 RID: 67
	internal class DivideOperator : OperatorBase
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007757 File Offset: 0x00005957
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "/", "÷" };
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000776F File Offset: 0x0000596F
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new DivideExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000777D File Offset: 0x0000597D
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.Divide;
		}
	}
}
