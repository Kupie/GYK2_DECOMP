using System;
using System.Globalization;

namespace Expressive.Tokenisation
{
	// Token: 0x02000036 RID: 54
	internal class NumericTokenExtractor : ITokenExtractor
	{
		// Token: 0x0600011A RID: 282 RVA: 0x000070E8 File Offset: 0x000052E8
		public Token ExtractToken(string expression, int currentIndex, Context context)
		{
			if (!NumericTokenExtractor.IsValidStart(expression[currentIndex], context, NumberStyles.Any))
			{
				return null;
			}
			NumericTokenExtractor.Location numberLocation = NumericTokenExtractor.GetNumberLocation(expression, currentIndex, context, NumberStyles.Any);
			return new Token(expression.Substring(numberLocation.Start, numberLocation.Length), currentIndex);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00007134 File Offset: 0x00005334
		private static NumericTokenExtractor.Location GetNumberLocation(string expression, int startIndex, Context context, NumberStyles numberStyles)
		{
			int num = startIndex;
			char c = expression[num];
			int length = expression.Length;
			NumericTokenExtractor.Location location;
			while (NumericTokenExtractor.IsAllowableCharacter(c, expression, num, startIndex, length, context, ref numberStyles, out location) && num < length)
			{
				if (location != null)
				{
					return new NumericTokenExtractor.Location(startIndex, location.End);
				}
				num++;
				if (num == expression.Length)
				{
					break;
				}
				c = expression[num];
			}
			return new NumericTokenExtractor.Location(startIndex, num);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00007198 File Offset: 0x00005398
		private static bool IsAllowableCharacter(char character, string expression, int index, int startIndex, int expressionLength, Context context, ref NumberStyles numberStyles, out NumericTokenExtractor.Location exponentialLocation)
		{
			exponentialLocation = null;
			if (char.IsDigit(character) || (NumericTokenExtractor.IsValidStart(character, context, numberStyles) && index == startIndex))
			{
				return true;
			}
			if (!numberStyles.HasFlag(NumberStyles.AllowExponent))
			{
				return false;
			}
			if ((character == 'e' || character == 'E') && char.IsDigit(expression[index - 1]) && index + 1 < expressionLength && NumericTokenExtractor.IsValidStart(expression[index + 1], context, NumberStyles.Integer))
			{
				exponentialLocation = NumericTokenExtractor.GetNumberLocation(expression, index + 1, context, NumberStyles.Integer);
				if (exponentialLocation != null)
				{
					return true;
				}
			}
			if (numberStyles.HasFlag(NumberStyles.AllowDecimalPoint) && character == context.DecimalSeparator)
			{
				numberStyles &= ~NumberStyles.AllowDecimalPoint;
				return true;
			}
			return false;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007255 File Offset: 0x00005455
		private static bool IsSignCharacter(char character)
		{
			return character == '-' || character == '−' || character == '+';
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000726C File Offset: 0x0000546C
		private static bool IsValidStart(char character, Context context, NumberStyles numberStyles)
		{
			return char.IsDigit(character) || (numberStyles.HasFlag(NumberStyles.AllowLeadingSign) && NumericTokenExtractor.IsSignCharacter(character)) || (numberStyles.HasFlag(NumberStyles.AllowDecimalPoint) && character == context.DecimalSeparator);
		}

		// Token: 0x020001AF RID: 431
		private class Location
		{
			// Token: 0x1700014E RID: 334
			// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0002DBDC File Offset: 0x0002BDDC
			public int End { get; }

			// Token: 0x1700014F RID: 335
			// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0002DBE4 File Offset: 0x0002BDE4
			public int Length
			{
				get
				{
					return this.End - this.Start;
				}
			}

			// Token: 0x17000150 RID: 336
			// (get) Token: 0x060009B7 RID: 2487 RVA: 0x0002DBF3 File Offset: 0x0002BDF3
			public int Start { get; }

			// Token: 0x060009B8 RID: 2488 RVA: 0x0002DBFB File Offset: 0x0002BDFB
			public Location(int start, int end)
			{
				this.Start = start;
				this.End = end;
			}
		}
	}
}
