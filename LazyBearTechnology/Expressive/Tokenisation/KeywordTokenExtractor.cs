using System;
using System.Collections.Generic;

namespace Expressive.Tokenisation
{
	// Token: 0x02000035 RID: 53
	internal class KeywordTokenExtractor : ITokenExtractor
	{
		// Token: 0x06000118 RID: 280 RVA: 0x00007046 File Offset: 0x00005246
		public KeywordTokenExtractor(IEnumerable<string> keywords)
		{
			if (keywords == null)
			{
				throw new ArgumentNullException("keywords");
			}
			this.keywords = keywords;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007064 File Offset: 0x00005264
		public Token ExtractToken(string expression, int currentIndex, Context context)
		{
			int length = expression.Length;
			foreach (string text in this.keywords)
			{
				string text2 = expression.Substring(currentIndex, Math.Min(text.Length, length - currentIndex));
				if (string.Equals(text2, text, context.ParsingStringComparison))
				{
					return new Token(text2, currentIndex);
				}
			}
			return null;
		}

		// Token: 0x04000092 RID: 146
		private readonly IEnumerable<string> keywords;
	}
}
