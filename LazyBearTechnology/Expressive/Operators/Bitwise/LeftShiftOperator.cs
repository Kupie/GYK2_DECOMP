using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Bitwise;

namespace Expressive.Operators.Bitwise
{
	// Token: 0x02000050 RID: 80
	internal class LeftShiftOperator : OperatorBase
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00007A62 File Offset: 0x00005C62
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "<<" };
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007A72 File Offset: 0x00005C72
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new LeftShiftExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00007A80 File Offset: 0x00005C80
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.LeftShift;
		}
	}
}
