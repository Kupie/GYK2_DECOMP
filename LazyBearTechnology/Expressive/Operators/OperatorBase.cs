using System;
using System.Collections.Generic;
using Expressive.Expressions;

namespace Expressive.Operators
{
	// Token: 0x0200003B RID: 59
	public abstract class OperatorBase : IOperator
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000131 RID: 305
		public abstract IEnumerable<string> Tags { get; }

		// Token: 0x06000132 RID: 306
		public abstract IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context);

		// Token: 0x06000133 RID: 307 RVA: 0x00007632 File Offset: 0x00005832
		public virtual bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			return true;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007635 File Offset: 0x00005835
		public virtual Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			return new Token[] { token };
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007641 File Offset: 0x00005841
		public virtual Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return new Token[0];
		}

		// Token: 0x06000136 RID: 310
		public abstract OperatorPrecedence GetPrecedence(Token previousToken);
	}
}
