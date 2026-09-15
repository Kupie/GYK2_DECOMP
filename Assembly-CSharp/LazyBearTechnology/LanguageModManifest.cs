using System;

namespace LazyBearTechnology
{
	// Token: 0x02000C4C RID: 3148
	[Serializable]
	public class LanguageModManifest
	{
		// Token: 0x040041BE RID: 16830
		public string name;

		// Token: 0x040041BF RID: 16831
		public string font;

		// Token: 0x040041C0 RID: 16832
		public LanguageModFontSettings fontSettings;

		// Token: 0x040041C1 RID: 16833
		public bool rtl;

		// Token: 0x040041C2 RID: 16834
		public string[] preprocessors;

		// Token: 0x040041C3 RID: 16835
		public string fallback;

		// Token: 0x040041C4 RID: 16836
		public bool requireResize;
	}
}
