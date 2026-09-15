using System;
using System.Collections.Generic;
using Expressive.Exceptions;

namespace Expressive.Expressions
{
	// Token: 0x0200009E RID: 158
	internal class ParenthesisedExpression : IExpression
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000E1D6 File Offset: 0x0000C3D6
		internal ParenthesisedExpression(IExpression innerExpression)
		{
			this.innerExpression = innerExpression;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000E1E5 File Offset: 0x0000C3E5
		public object Evaluate(IDictionary<string, object> variables)
		{
			if (this.innerExpression == null)
			{
				throw new MissingParticipantException("Missing contents inside ().");
			}
			return this.innerExpression.Evaluate(variables);
		}

		// Token: 0x040000B8 RID: 184
		private readonly IExpression innerExpression;
	}
}
