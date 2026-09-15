using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000156 RID: 342
	[CreateAssetMenu(menuName = "LazyFont/Lazy Font Data", fileName = "LazyFontData")]
	[Serializable]
	public class LazyFontData : ScriptableObject
	{
		// Token: 0x0600075D RID: 1885 RVA: 0x00025AFC File Offset: 0x00023CFC
		public TMP_FontAsset GetFontAssetFor(string lang, bool staticFont, bool applyLanguagePackFont = true)
		{
			if (!staticFont)
			{
				TMP_FontAsset tmp_FontAsset;
				if (applyLanguagePackFont && LanguageModHooks.TryGetFontAsset != null && LanguageModHooks.TryGetFontAsset(lang, false, out tmp_FontAsset))
				{
					return tmp_FontAsset;
				}
				for (int i = 0; i < this.assetOverrideByLang.Count; i++)
				{
					if (this.assetOverrideByLang[i].langId == lang)
					{
						return this.assetOverrideByLang[i].GetFontAsset();
					}
				}
			}
			return LazyFontData.LoadFontAsset(this.fontAssetPath, ref this.fontAssetCache);
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x00025B7C File Offset: 0x00023D7C
		public bool IsFontUsesOwnMaterial(string lang)
		{
			if (LanguageModHooks.UsesOwnMaterial != null && LanguageModHooks.UsesOwnMaterial(lang))
			{
				return true;
			}
			for (int i = 0; i < this.assetOverrideByLang.Count; i++)
			{
				if (this.assetOverrideByLang[i].langId == lang)
				{
					return this.assetOverrideByLang[i].useOwnMaterial;
				}
			}
			return false;
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x00025BE1 File Offset: 0x00023DE1
		private static TMP_FontAsset LoadFontAsset(string path, ref TMP_FontAsset cache)
		{
			if (cache != null)
			{
				return cache;
			}
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			cache = Resources.Load<TMP_FontAsset>(path);
			return cache;
		}

		// Token: 0x0400047A RID: 1146
		[SerializeField]
		private string fontAssetPath;

		// Token: 0x0400047B RID: 1147
		[SerializeField]
		private List<LazyFontData.FontOverrideData> assetOverrideByLang = new List<LazyFontData.FontOverrideData>();

		// Token: 0x0400047C RID: 1148
		private TMP_FontAsset fontAssetCache;

		// Token: 0x020001FA RID: 506
		[Serializable]
		private class FontOverrideData
		{
			// Token: 0x06000A69 RID: 2665 RVA: 0x0002ED6D File Offset: 0x0002CF6D
			public TMP_FontAsset GetFontAsset()
			{
				return LazyFontData.LoadFontAsset(this.fontAssetPath, ref this.fontAssetCache);
			}

			// Token: 0x040006B4 RID: 1716
			public string langId;

			// Token: 0x040006B5 RID: 1717
			public string fontAssetPath;

			// Token: 0x040006B6 RID: 1718
			public bool useOwnMaterial;

			// Token: 0x040006B7 RID: 1719
			private TMP_FontAsset fontAssetCache;
		}
	}
}
