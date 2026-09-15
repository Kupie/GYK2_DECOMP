using System;
using System.Collections.Generic;
using Expressive.Helpers;

namespace Expressive.Expressions.Unary.Additive
{
	// Token: 0x020000A3 RID: 163
	internal class MinusExpression : UnaryExpressionBase
	{
		// Token: 0x0600028F RID: 655 RVA: 0x0000E300 File Offset: 0x0000C500
		public MinusExpression(IExpression expression)
			: base(expression)
		{
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000E309 File Offset: 0x0000C509
		public override object Evaluate(IDictionary<string, object> variables)
		{
			return Numbers.Subtract(0, this.expression.Evaluate(variables));
		}
	}
}
