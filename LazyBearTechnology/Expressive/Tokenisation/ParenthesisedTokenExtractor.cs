using System;
using Expressive.Exceptions;

namespace Expressive.Tokenisation
{
	// Token: 0x02000037 RID: 55
	internal class ParenthesisedTokenExtractor : ITokenExtractor
	{
		// Token: 0x06000120 RID: 288 RVA: 0x000072C5 File Offset: 0x000054C5
		public ParenthesisedTokenExtractor(char singleCharacter)
			: this(singleCharacter, singleCharacter)
		{
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000072CF File Offset: 0x000054CF
		public ParenthesisedTokenExtractor(char startingCharacter, char endingCharacter)
		{
			this.startingCharacter = startingCharacter;
			this.endingCharacter = endingCharacter;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000072E8 File Offset: 0x000054E8
		public Token ExtractToken(string expression, int currentIndex, Context context)
		{
			if (expression[currentIndex] != this.startingCharacter)
			{
				return null;
			}
			string @string = ParenthesisedTokenExtractor.GetString(expression, currentIndex, this.endingCharacter);
			if (string.IsNullOrWhiteSpace(@string))
			{
				throw new MissingTokenException(string.Format("Missing closing token '{0}'", this.endingCharacter), this.endingCharacter);
			}
			return new Token(@string, currentIndex);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007344 File Offset: 0x00005544
		private static string GetString(string expression, int startIndex, char expectedEndingCharacter)
		{
			int num = startIndex;
			bool flag = false;
			char c = expression[num];
			bool flag2 = false;
			while (num < expression.Length && !flag)
			{
				if (num != startIndex && c == expectedEndingCharacter && !flag2)
				{
					flag = true;
				}
				flag2 = c == '\\' && !flag2;
				num++;
				if (num == expression.Length)
				{
					break;
				}
				c = expression[num];
			}
			if (!flag)
			{
				return null;
			}
			return expression.Substring(startIndex, num - startIndex);
		}

		// Token: 0x04000093 RID: 147
		private readonly char endingCharacter;

		// Token: 0x04000094 RID: 148
		private readonly char startingCharacter;
	}
}
