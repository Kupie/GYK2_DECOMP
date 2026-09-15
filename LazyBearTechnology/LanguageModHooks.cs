using System;
using System.Collections.Generic;
using TMPro;

namespace LazyBearTechnology
{
	// Token: 0x02000102 RID: 258
	public static class LanguageModHooks
	{
		// Token: 0x0400024A RID: 586
		public static LanguageModHooks.TryGetLanguageHandler TryGetLanguage;

		// Token: 0x0400024B RID: 587
		public static LanguageModHooks.TryGetFontAssetHandler TryGetFontAsset;

		// Token: 0x0400024C RID: 588
		public static LanguageModHooks.UsesOwnMaterialHandler UsesOwnMaterial;

		// Token: 0x0400024D RID: 589
		public static LanguageModHooks.RequiresResizeHandler RequiresResize;

		// Token: 0x0400024E RID: 590
		public static LanguageModHooks.ApplyDirectionHandler ApplyDirection;

		// Token: 0x0400024F RID: 591
		public static LanguageModHooks.AppendLanguagesHandler AppendLanguages;

		// Token: 0x020001DC RID: 476
		// (Invoke) Token: 0x06000A39 RID: 2617
		public delegate bool TryGetLanguageHandler(string langId, out LL language);

		// Token: 0x020001DD RID: 477
		// (Invoke) Token: 0x06000A3D RID: 2621
		public delegate bool TryGetFontAssetHandler(string lang, bool staticFont, out TMP_FontAsset fontAsset);

		// Token: 0x020001DE RID: 478
		// (Invoke) Token: 0x06000A41 RID: 2625
		public delegate bool UsesOwnMaterialHandler(string lang);

		// Token: 0x020001DF RID: 479
		// (Invoke) Token: 0x06000A45 RID: 2629
		public delegate bool RequiresResizeHandler(string lang);

		// Token: 0x020001E0 RID: 480
		// (Invoke) Token: 0x06000A49 RID: 2633
		public delegate void ApplyDirectionHandler(TMP_Text label, string lang, bool staticFont);

		// Token: 0x020001E1 RID: 481
		// (Invoke) Token: 0x06000A4D RID: 2637
		public delegate void AppendLanguagesHandler(Dictionary<string, LLBase.LanguageInfo> languages);
	}
}
