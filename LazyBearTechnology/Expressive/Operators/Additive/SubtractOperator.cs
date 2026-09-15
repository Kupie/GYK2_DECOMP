using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Additive;
using Expressive.Expressions.Unary.Additive;

namespace Expressive.Operators.Additive
{
	// Token: 0x02000053 RID: 83
	internal class SubtractOperator : OperatorBase
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00007B6D File Offset: 0x00005D6D
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "-", "−" };
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00007B85 File Offset: 0x00005D85
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			if (SubtractOperator.IsUnary(previousToken))
			{
				return new MinusExpression(expressions[0] ?? expressions[1]);
			}
			return new SubtractExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00007BAC File Offset: 0x00005DAC
		public override bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			Queue<Token> queue = new Queue<Token>(remainingTokens.ToArray());
			return this.GetCaptiveTokens(previousToken, token, queue).Any<Token>();
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00007BD3 File Offset: 0x00005DD3
		public override Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return allCaptiveTokens.Skip(1).ToArray<Token>();
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00007BE1 File Offset: 0x00005DE1
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			if (!SubtractOperator.IsUnary(previousToken))
			{
				return OperatorPrecedence.Subtract;
			}
			return OperatorPrecedence.UnaryMinus;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00007BF0 File Offset: 0x00005DF0
		private static bool IsUnary(Token previousToken)
		{
			return string.IsNullOrEmpty((previousToken != null) ? previousToken.CurrentToken : null) || string.Equals(previousToken.CurrentToken, "(", StringComparison.Ordinal) || previousToken.CurrentToken.IsArithmeticOperator();
		}
	}
}
