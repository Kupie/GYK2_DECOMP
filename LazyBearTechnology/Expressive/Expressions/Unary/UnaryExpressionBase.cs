using System;
using System.Collections.Generic;

namespace Expressive.Expressions.Unary
{
	// Token: 0x020000A1 RID: 161
	internal abstract class UnaryExpressionBase : IExpression
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000E29E File Offset: 0x0000C49E
		internal UnaryExpressionBase(IExpression expression)
		{
			this.expression = expression;
		}

		// Token: 0x0600028C RID: 652
		public abstract object Evaluate(IDictionary<string, object> variables);

		// Token: 0x040000BA RID: 186
		protected readonly IExpression expression;
	}
}
