using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Unary.Additive
{
	// Token: 0x020000A4 RID: 164
	internal class PlusExpression : UnaryExpressionBase
	{
		// Token: 0x06000291 RID: 657 RVA: 0x0000E322 File Offset: 0x0000C522
		public PlusExpression(IExpression expression)
			: base(expression)
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E32B File Offset: 0x0000C52B
		public override object Evaluate(IDictionary<string, object> variables)
		{
			return Numbers.Add(0, this.expression.Evaluate(variables));
		}
	}
}
