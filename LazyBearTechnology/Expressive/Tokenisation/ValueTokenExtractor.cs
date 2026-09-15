using System;
using System.Linq;

namespace Expressive.Tokenisation
{
	// Token: 0x02000039 RID: 57
	internal class ValueTokenExtractor : ITokenExtractor
	{
		// Token: 0x06000127 RID: 295 RVA: 0x00007529 File Offset: 0x00005729
		public ValueTokenExtractor(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.value = value;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007548 File Offset: 0x00005748
		public Token ExtractToken(string expression, int currentIndex, Context context)
		{
			char c = expression[currentIndex];
			int length = expression.Length;
			char c2 = this.value.First<char>();
			if (string.Equals(c.ToString(), c2.ToString(), context.ParsingStringComparison) && ValueTokenExtractor.CanExtractValue(expression, length, currentIndex, this.value, context))
			{
				return new Token(this.value, currentIndex);
			}
			return null;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000075AA File Offset: 0x000057AA
		private static bool CanExtractValue(string expression, int expressionLength, int index, string expectedValue, Context context)
		{
			return string.Equals(expectedValue, ValueTokenExtractor.ExtractValue(expression, expressionLength, index, expectedValue, context), context.ParsingStringComparison);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000075C4 File Offset: 0x000057C4
		private static string ExtractValue(string expression, int expressionLength, int index, string expectedValue, Context context)
		{
			string text = null;
			int length = expectedValue.Length;
			if (expressionLength >= index + length)
			{
				string text2 = expression.Substring(index, length);
				bool flag = true;
				if (expressionLength > index + length)
				{
					flag = !char.IsLetterOrDigit(expression[index + length]) || string.Equals(expectedValue, ','.ToString(), context.ParsingStringComparison);
				}
				if (string.Equals(text2, expectedValue, context.ParsingStringComparison) && flag)
				{
					text = text2;
				}
			}
			return text;
		}

		// Token: 0x04000097 RID: 151
		private readonly string value;
	}
}
