using System;

namespace Expressive
{
	// Token: 0x02000032 RID: 50
	public sealed class Token
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006F59 File Offset: 0x00005159
		public string CurrentToken { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006F61 File Offset: 0x00005161
		public int Length { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00006F69 File Offset: 0x00005169
		public int StartIndex { get; }

		// Token: 0x06000103 RID: 259 RVA: 0x00006F71 File Offset: 0x00005171
		public Token(string currentToken, int startIndex)
		{
			this.CurrentToken = currentToken;
			this.StartIndex = startIndex;
			string currentToken2 = this.CurrentToken;
			this.Length = ((currentToken2 != null) ? currentToken2.Length : 0);
		}
	}
}
