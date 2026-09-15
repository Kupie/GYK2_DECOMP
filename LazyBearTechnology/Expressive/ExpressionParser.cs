using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Expressive.Exceptions;
using Expressive.Expressions;
using Expressive.Operators;
using Expressive.Tokenisation;

namespace Expressive
{
	// Token: 0x0200002E RID: 46
	internal sealed class ExpressionParser
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x000064DC File Offset: 0x000046DC
		internal ExpressionParser(Context context)
		{
			this.context = context;
			this.tokeniser = new Tokeniser(this.context, new List<ITokenExtractor>
			{
				new KeywordTokenExtractor(this.context.FunctionNames),
				new KeywordTokenExtractor(this.context.OperatorNames),
				new ParenthesisedTokenExtractor('[', ']'),
				new NumericTokenExtractor(),
				new ParenthesisedTokenExtractor('#'),
				new ValueTokenExtractor(","),
				new ParenthesisedTokenExtractor('"'),
				new ParenthesisedTokenExtractor('\''),
				new ValueTokenExtractor("true"),
				new ValueTokenExtractor("TRUE"),
				new ValueTokenExtractor("false"),
				new ValueTokenExtractor("FALSE"),
				new ValueTokenExtractor("null"),
				new ValueTokenExtractor("NULL")
			});
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000065EC File Offset: 0x000047EC
		internal IExpression CompileExpression(string expression, IList<string> variables)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				throw new ExpressiveException("An Expression cannot be empty.");
			}
			IList<Token> list = this.tokeniser.Tokenise(expression);
			int num = list.Select((Token t) => t.CurrentToken).Count((string t) => string.Equals(t, "(", StringComparison.Ordinal));
			int num2 = list.Select((Token t) => t.CurrentToken).Count((string t) => string.Equals(t, ")", StringComparison.Ordinal));
			if (num > num2)
			{
				throw new ArgumentException("There aren't enough ')' symbols. Expected " + num.ToString() + " but there is only " + num2.ToString());
			}
			if (num < num2)
			{
				throw new ArgumentException("There are too many ')' symbols. Expected " + num.ToString() + " but there is " + num2.ToString());
			}
			return this.CompileExpression(new Queue<Token>(list), OperatorPrecedence.Minimum, variables, false);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000670C File Offset: 0x0000490C
		private IExpression CompileExpression(Queue<Token> tokens, OperatorPrecedence minimumPrecedence, IList<string> variables, bool isWithinFunction)
		{
			if (tokens == null)
			{
				throw new ArgumentNullException("tokens", "You must call Tokenise before compiling");
			}
			IExpression expression = null;
			Token token = tokens.PeekOrDefault<Token>();
			Token token2 = null;
			while (token != null)
			{
				IOperator @operator;
				Func<IExpression[], IDictionary<string, object>, object> func;
				if (this.context.TryGetOperator(token.CurrentToken, out @operator))
				{
					OperatorPrecedence precedence = @operator.GetPrecedence(token2);
					if (precedence <= minimumPrecedence)
					{
						break;
					}
					tokens.Dequeue();
					if (!@operator.CanGetCaptiveTokens(token2, token, tokens))
					{
						@operator.GetCaptiveTokens(token2, token, tokens);
						break;
					}
					Token[] captiveTokens = @operator.GetCaptiveTokens(token2, token, tokens);
					IExpression expression2;
					if (captiveTokens.Length > 1)
					{
						Token[] innerCaptiveTokens = @operator.GetInnerCaptiveTokens(captiveTokens);
						expression2 = this.CompileExpression(new Queue<Token>(innerCaptiveTokens), OperatorPrecedence.Minimum, variables, isWithinFunction);
						token = captiveTokens[captiveTokens.Length - 1];
					}
					else
					{
						expression2 = this.CompileExpression(tokens, precedence, variables, isWithinFunction);
						token = new Token(")", -1);
					}
					expression = @operator.BuildExpression(token2, new IExpression[] { expression, expression2 }, this.context);
				}
				else if (this.context.TryGetFunction(token.CurrentToken, out func))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					List<IExpression> list = new List<IExpression>();
					Queue<Token> queue = new Queue<Token>();
					int num = 0;
					tokens.Dequeue();
					while (tokens.Count > 0)
					{
						Token token3 = tokens.Dequeue();
						if (string.Equals(token3.CurrentToken, "(", StringComparison.Ordinal))
						{
							num++;
						}
						else if (string.Equals(token3.CurrentToken, ")", StringComparison.Ordinal))
						{
							num--;
						}
						if ((num != 1 || !(token3.CurrentToken == "(")) && (num != 0 || !(token3.CurrentToken == ")")))
						{
							queue.Enqueue(token3);
						}
						if (num == 0 && queue.Any<Token>())
						{
							list.Add(this.CompileExpression(queue, OperatorPrecedence.Minimum, variables, true));
							queue.Clear();
						}
						else if (string.Equals(token3.CurrentToken, ','.ToString(), StringComparison.Ordinal) && num == 1)
						{
							list.Add(this.CompileExpression(queue, OperatorPrecedence.Minimum, variables, true));
							queue.Clear();
						}
						if (num <= 0)
						{
							break;
						}
					}
					expression = new FunctionExpression(token.CurrentToken, func, list.ToArray());
				}
				else if (token.CurrentToken.IsNumeric(this.context.DecimalCurrentCulture))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					int num2;
					decimal num3;
					double num4;
					float num5;
					long num6;
					if (int.TryParse(token.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out num2))
					{
						expression = new ConstantValueExpression(num2);
					}
					else if (decimal.TryParse(token.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out num3))
					{
						expression = new ConstantValueExpression(num3);
					}
					else if (double.TryParse(token.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out num4))
					{
						expression = new ConstantValueExpression(num4);
					}
					else if (float.TryParse(token.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out num5))
					{
						expression = new ConstantValueExpression(num5);
					}
					else if (long.TryParse(token.CurrentToken, NumberStyles.Any, this.context.DecimalCurrentCulture, out num6))
					{
						expression = new ConstantValueExpression(num6);
					}
				}
				else if (token.CurrentToken.StartsWith("[") && token.CurrentToken.EndsWith("]"))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					string text = token.CurrentToken.Replace("[", "").Replace("]", "");
					expression = new VariableExpression(text);
					if (!variables.Contains(text, this.context.ParsingStringComparer))
					{
						variables.Add(text);
					}
				}
				else if (string.Equals(token.CurrentToken, "true", StringComparison.OrdinalIgnoreCase))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					expression = new ConstantValueExpression(true);
				}
				else if (string.Equals(token.CurrentToken, "false", StringComparison.OrdinalIgnoreCase))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					expression = new ConstantValueExpression(false);
				}
				else if (string.Equals(token.CurrentToken, "null", StringComparison.OrdinalIgnoreCase))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					expression = new ConstantValueExpression(null);
				}
				else if (token.CurrentToken.StartsWith('#'.ToString()) && token.CurrentToken.EndsWith('#'.ToString()))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					string text2 = token.CurrentToken.Replace('#'.ToString(), "");
					DateTime dateTime;
					if (!DateTime.TryParse(text2, out dateTime))
					{
						if (string.Equals("TODAY", text2, StringComparison.OrdinalIgnoreCase))
						{
							dateTime = DateTime.Today;
						}
						else
						{
							if (!string.Equals("NOW", text2, StringComparison.OrdinalIgnoreCase))
							{
								throw new UnrecognisedTokenException(text2);
							}
							dateTime = DateTime.Now;
						}
					}
					expression = new ConstantValueExpression(dateTime);
				}
				else if ((token.CurrentToken.StartsWith("'") && token.CurrentToken.EndsWith("'")) || (token.CurrentToken.StartsWith("\"") && token.CurrentToken.EndsWith("\"")))
				{
					ExpressionParser.CheckForExistingParticipant(expression, token, isWithinFunction);
					tokens.Dequeue();
					expression = new ConstantValueExpression(ExpressionParser.CleanString(token.CurrentToken.Substring(1, token.Length - 2)));
				}
				else
				{
					if (!string.Equals(token.CurrentToken, ','.ToString(), StringComparison.Ordinal))
					{
						tokens.Dequeue();
						throw new UnrecognisedTokenException(token.CurrentToken);
					}
					if (!isWithinFunction)
					{
						throw new ExpressiveException(string.Format("Unexpected token '{0}'", token));
					}
					tokens.Dequeue();
				}
				token2 = token;
				token = tokens.PeekOrDefault<Token>();
			}
			return expression;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00006D0C File Offset: 0x00004F0C
		private static string CleanString(string input)
		{
			if (input.Length <= 1)
			{
				return input;
			}
			char[] array = new char[input.Length];
			int num = 0;
			int i = 0;
			while (i < input.Length)
			{
				char c = input[i];
				if (c != '\\' || i >= input.Length - 1)
				{
					goto IL_00D1;
				}
				char c2 = input[i + 1];
				if (c2 <= '\\')
				{
					if (c2 != '"')
					{
						if (c2 != '\'')
						{
							if (c2 != '\\')
							{
								goto IL_00D1;
							}
							array[num++] = '\\';
							i++;
						}
						else
						{
							array[num++] = '\'';
							i++;
						}
					}
					else
					{
						array[num++] = '"';
						i++;
					}
				}
				else if (c2 != 'n')
				{
					if (c2 != 'r')
					{
						if (c2 != 't')
						{
							goto IL_00D1;
						}
						array[num++] = '\t';
						i++;
					}
					else
					{
						array[num++] = '\r';
						i++;
					}
				}
				else
				{
					array[num++] = '\n';
					i++;
				}
				IL_00D9:
				i++;
				continue;
				IL_00D1:
				array[num++] = c;
				goto IL_00D9;
			}
			return new string(array, 0, num);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00006E0A File Offset: 0x0000500A
		private static void CheckForExistingParticipant(IExpression participant, Token token, bool isWithinFunction)
		{
			if (participant == null)
			{
				return;
			}
			if (isWithinFunction)
			{
				throw new MissingTokenException("Missing token, expecting ','.", ',');
			}
			throw new ExpressiveException(string.Format("Unexpected token '{0}' at index {1}", token.CurrentToken, token.StartIndex));
		}

		// Token: 0x04000083 RID: 131
		private readonly Context context;

		// Token: 0x04000084 RID: 132
		private readonly Tokeniser tokeniser;
	}
}
