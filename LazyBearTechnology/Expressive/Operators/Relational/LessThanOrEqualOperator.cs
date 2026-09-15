using System;
using System.Collections.Generic;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Relational;

namespace Expressive.Operators.Relational
{
	// Token: 0x02000041 RID: 65
	internal class LessThanOrEqualOperator : OperatorBase
	{
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000076FD File Offset: 0x000058FD
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "<=" };
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000770D File Offset: 0x0000590D
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new LessThanOrEqualExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000771B File Offset: 0x0000591B
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.LessThanOrEqual;
		}
	}
}
