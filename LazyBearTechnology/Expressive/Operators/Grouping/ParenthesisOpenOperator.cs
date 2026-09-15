using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;

namespace Expressive.Operators.Grouping
{
	// Token: 0x0200004B RID: 75
	internal class ParenthesisOpenOperator : IOperator
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000170 RID: 368 RVA: 0x000078DE File Offset: 0x00005ADE
		public IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "(" };
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x000078EE File Offset: 0x00005AEE
		public IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			return new ParenthesisedExpression(expressions[0] ?? expressions[1]);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00007900 File Offset: 0x00005B00
		public bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			Queue<Token> queue = new Queue<Token>(remainingTokens.ToArray());
			return this.GetCaptiveTokens(previousToken, token, queue).Any<Token>();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00007928 File Offset: 0x00005B28
		public Token[] GetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			IList<Token> list = new List<Token>();
			list.Add(token);
			int num = 1;
			while (remainingTokens.Any<Token>())
			{
				Token token2 = remainingTokens.Dequeue();
				list.Add(token2);
				if (string.Equals(token2.CurrentToken, "(", StringComparison.Ordinal))
				{
					num++;
				}
				else if (string.Equals(token2.CurrentToken, ")", StringComparison.Ordinal))
				{
					num--;
				}
				if (num <= 0)
				{
					break;
				}
			}
			return list.ToArray<Token>();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007996 File Offset: 0x00005B96
		public Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return allCaptiveTokens.Skip(1).Take(allCaptiveTokens.Length - 2).ToArray<Token>();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000079AE File Offset: 0x00005BAE
		public OperatorPrecedence GetPrecedence(Token previousToken)
		{
			return OperatorPrecedence.ParenthesisOpen;
		}
	}
}
