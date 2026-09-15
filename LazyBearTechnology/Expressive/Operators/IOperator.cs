using System;
using System.Collections.Generic;
using Expressive.Expressions;

namespace Expressive.Operators
{
	// Token: 0x0200003A RID: 58
	public interface IOperator
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600012B RID: 299
		IEnumerable<string> Tags { get; }

		// Token: 0x0600012C RID: 300
		IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context);

		// Token: 0x0600012D RID: 301
		bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens);

		// Token: 0x0600012E RID: 302
		Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens);

		// Token: 0x0600012F RID: 303
		Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens);

		// Token: 0x06000130 RID: 304
		OperatorPrecedence GetPrecedence(Token previousToken);
	}
}
