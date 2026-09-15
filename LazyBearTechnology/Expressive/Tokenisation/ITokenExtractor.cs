using System;

namespace Expressive.Tokenisation
{
	// Token: 0x02000034 RID: 52
	public interface ITokenExtractor
	{
		// Token: 0x06000117 RID: 279
		Token ExtractToken(string expression, int currentIndex, Context context);
	}
}
