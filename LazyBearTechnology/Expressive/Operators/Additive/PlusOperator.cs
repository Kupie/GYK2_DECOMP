using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Expressions;
using Expressive.Expressions.Binary.Additive;
using Expressive.Expressions.Unary.Additive;

namespace Expressive.Operators.Additive
{
	// Token: 0x02000052 RID: 82
	internal class PlusOperator : OperatorBase
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00007AB6 File Offset: 0x00005CB6
		public override IEnumerable<string> Tags
		{
			get
			{
				return new string[] { "+" };
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00007AC6 File Offset: 0x00005CC6
		public override IExpression BuildExpression(Token previousToken, IExpression[] expressions, Context context)
		{
			if (PlusOperator.IsUnary(previousToken))
			{
				return new PlusExpression(expressions[0] ?? expressions[1]);
			}
			return new AddExpression(expressions[0], expressions[1], context);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00007AEC File Offset: 0x00005CEC
		public override bool CanGetCaptiveTokens(Token previousToken, Token token, Queue<Token> remainingTokens)
		{
			Queue<Token> queue = new Queue<Token>(remainingTokens.ToArray());
			return this.GetCaptiveTokens(previousToken, token, queue).Any<Token>();
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00007B13 File Offset: 0x00005D13
		public override Token[] GetInnerCaptiveTokens(Token[] allCaptiveTokens)
		{
			return allCaptiveTokens.Skip(1).ToArray<Token>();
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00007B21 File Offset: 0x00005D21
		public override OperatorPrecedence GetPrecedence(Token previousToken)
		{
			if (!PlusOperator.IsUnary(previousToken))
			{
				return OperatorPrecedence.Add;
			}
			return OperatorPrecedence.UnaryPlus;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00007B30 File Offset: 0x00005D30
		private static bool IsUnary(Token previousToken)
		{
			return string.IsNullOrEmpty((previousToken != null) ? previousToken.CurrentToken : null) || string.Equals(previousToken.CurrentToken, "(", StringComparison.Ordinal) || previousToken.CurrentToken.IsArithmeticOperator();
		}
	}
}
