using System;
using System.Collections.Generic;
using System.Linq;

namespace Expressive.Tokenisation
{
	// Token: 0x02000038 RID: 56
	internal sealed class Tokeniser
	{
		// Token: 0x06000124 RID: 292 RVA: 0x000073AC File Offset: 0x000055AC
		public Tokeniser(Context context, IEnumerable<ITokenExtractor> tokenExtractors)
		{
			this.context = context;
			this.tokenExtractors = tokenExtractors;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000073C4 File Offset: 0x000055C4
		internal IList<Token> Tokenise(string expression)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				return null;
			}
			int length = expression.Length;
			List<Token> list = new List<Token>();
			IList<char> list2 = null;
			int index;
			Func<ITokenExtractor, Token> <>9__0;
			Token token;
			for (index = 0; index < length; index += ((token != null) ? token.Length : 1))
			{
				IEnumerable<ITokenExtractor> enumerable = this.tokenExtractors;
				Func<ITokenExtractor, Token> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = (ITokenExtractor t) => t.ExtractToken(expression, index, this.context));
				}
				token = enumerable.Select(func).FirstOrDefault((Token t) => t != null);
				if (token != null)
				{
					Tokeniser.CheckForUnrecognised(list2, list, index);
					list2 = null;
					list.Add(token);
				}
				else
				{
					char c = expression[index];
					if (!char.IsWhiteSpace(c))
					{
						if (list2 == null)
						{
							list2 = new List<char>();
						}
						list2.Add(c);
					}
					else
					{
						Tokeniser.CheckForUnrecognised(list2, list, index);
						list2 = null;
					}
				}
			}
			Tokeniser.CheckForUnrecognised(list2, list, index);
			return list;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000074F8 File Offset: 0x000056F8
		private static void CheckForUnrecognised(IList<char> unrecognised, ICollection<Token> tokens, int index)
		{
			if (unrecognised == null)
			{
				return;
			}
			string text = new string(unrecognised.ToArray<char>());
			tokens.Add(new Token(text, index - text.Length));
		}

		// Token: 0x04000095 RID: 149
		private readonly Context context;

		// Token: 0x04000096 RID: 150
		private readonly IEnumerable<ITokenExtractor> tokenExtractors;
	}
}
