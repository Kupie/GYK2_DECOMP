using System;

namespace Expressive
{
	// Token: 0x0200002F RID: 47
	[Flags]
	public enum ExpressiveOptions
	{
		// Token: 0x04000086 RID: 134
		None = 1,
		// Token: 0x04000087 RID: 135
		[Obsolete("This will be removed in future versions with IgnoreCaseForParsing, IgnoreCaseForEquality and IgnoreCaseAll replacing it.")]
		IgnoreCase = 2,
		// Token: 0x04000088 RID: 136
		NoCache = 4,
		// Token: 0x04000089 RID: 137
		RoundAwayFromZero = 8,
		// Token: 0x0400008A RID: 138
		IgnoreCaseForParsing = 16,
		// Token: 0x0400008B RID: 139
		IgnoreCaseForEquality = 32,
		// Token: 0x0400008C RID: 140
		IgnoreCaseAll = 48,
		// Token: 0x0400008D RID: 141
		All = 62
	}
}
