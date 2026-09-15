using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000103 RID: 259
	public class LL : LLBase
	{
		// Token: 0x06000505 RID: 1285 RVA: 0x0001A860 File Offset: 0x00018A60
		protected override Dictionary<string, LLBase.LanguageInfo> GetAvailableLanguages()
		{
			Dictionary<string, LLBase.LanguageInfo> dictionary = new Dictionary<string, LLBase.LanguageInfo>
			{
				{
					"en",
					new LLBase.LanguageInfo("en", "en-US", "English")
				},
				{
					"de",
					new LLBase.LanguageInfo("de", "de-DE", "Deutsch")
				},
				{
					"fr",
					new LLBase.LanguageInfo("fr", "fr-FR", "Français")
				},
				{
					"pt-br",
					new LLBase.LanguageInfo("pt-br", "pt-BR", "Português do Brasil")
				},
				{
					"es",
					new LLBase.LanguageInfo("es", "es-ES", "Español")
				},
				{
					"ru",
					new LLBase.LanguageInfo("ru", "ru-RU", "Русский")
				},
				{
					"pl",
					new LLBase.LanguageInfo("pl", "pl-PL", "Polski")
				},
				{
					"ja",
					new LLBase.LanguageInfo("ja", "ja-JP", "Japanese")
				},
				{
					"zh_cn",
					new LLBase.LanguageInfo("zh_cn", "zh-CN", "Chinese (Simplified)")
				},
				{
					"ko",
					new LLBase.LanguageInfo("ko", "ko-KR", "Korean")
				},
				{
					"tr",
					new LLBase.LanguageInfo("tr", "tr-TR", "Türkçe")
				}
			};
			LanguageModHooks.AppendLanguagesHandler appendLanguages = LanguageModHooks.AppendLanguages;
			if (appendLanguages != null)
			{
				appendLanguages(dictionary);
			}
			return dictionary;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001A9DC File Offset: 0x00018BDC
		public static string GetLocaleCodeId()
		{
			Debug.Log(string.Format("[{0}]: Detected System Language = {1}", "LL", Application.systemLanguage));
			SystemLanguage systemLanguage = Application.systemLanguage;
			if (systemLanguage <= SystemLanguage.French)
			{
				if (systemLanguage != SystemLanguage.Belarusian)
				{
					if (systemLanguage == SystemLanguage.Chinese)
					{
						goto IL_00C3;
					}
					if (systemLanguage != SystemLanguage.French)
					{
						goto IL_00E7;
					}
					return "fr";
				}
			}
			else if (systemLanguage <= SystemLanguage.Russian)
			{
				if (systemLanguage == SystemLanguage.German)
				{
					return "de";
				}
				switch (systemLanguage)
				{
				case SystemLanguage.Italian:
					return "it";
				case SystemLanguage.Japanese:
					return "ja";
				case SystemLanguage.Korean:
					return "ko";
				case SystemLanguage.Latvian:
				case SystemLanguage.Lithuanian:
				case SystemLanguage.Norwegian:
				case SystemLanguage.Romanian:
					goto IL_00E7;
				case SystemLanguage.Polish:
					return "pl";
				case SystemLanguage.Portuguese:
					return "pt-br";
				case SystemLanguage.Russian:
					break;
				default:
					goto IL_00E7;
				}
			}
			else
			{
				if (systemLanguage == SystemLanguage.Spanish)
				{
					return "es";
				}
				switch (systemLanguage)
				{
				case SystemLanguage.Turkish:
					return "tr";
				case SystemLanguage.Ukrainian:
				case SystemLanguage.Vietnamese:
					goto IL_00E7;
				case SystemLanguage.ChineseSimplified:
					goto IL_00C3;
				case SystemLanguage.ChineseTraditional:
					return "zh_cht";
				default:
					goto IL_00E7;
				}
			}
			return "ru";
			IL_00C3:
			return "zh_cn";
			IL_00E7:
			return "en";
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001AAD8 File Offset: 0x00018CD8
		public static bool IsCurrentLangAsian
		{
			get
			{
				return LLBase.CurrentLang == "zh_cn" || LLBase.CurrentLang == "ja" || LLBase.CurrentLang == "ko" || LLBase.CurrentLang == "zh_cht";
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001AB2C File Offset: 0x00018D2C
		public new static string GetCurrentLocaleCode()
		{
			bool flag = false;
			if (LLBase.currentLang == null)
			{
				flag = true;
				LLBase.currentLang = new LL();
			}
			string text = LLBase.currentLang.GetLocaleCode().ToLower();
			Dictionary<string, LLBase.LanguageInfo> availableLanguages = LLBase.currentLang.GetAvailableLanguages();
			if (flag)
			{
				LLBase.currentLang = null;
			}
			if (!(text == "ptbr"))
			{
				if (text == "es-es" || text == "es-mx")
				{
					text = "es";
				}
			}
			else
			{
				text = "pt-br";
			}
			if (!availableLanguages.ContainsKey(text))
			{
				Debug.LogWarning("Language '" + text + "' not found. Loading EN...");
				text = "en";
			}
			return text;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001ABD4 File Offset: 0x00018DD4
		protected override string GetLocaleCode()
		{
			return LL.GetLocaleCodeId();
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001ABDB File Offset: 0x00018DDB
		public static string GetSpace()
		{
			if (LLBase.currentLang.id == "zh_cht")
			{
				return "<space=6>";
			}
			return " ";
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001ABFE File Offset: 0x00018DFE
		public static string GetHintsSeparator()
		{
			return LL.GetSpace() + LL.GetSpace() + LL.GetSpace() + LL.GetSpace();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001AC1C File Offset: 0x00018E1C
		public static bool HasLocalizedValueForCurrentLang(string lngId)
		{
			if (string.IsNullOrEmpty(lngId) || LLBase.currentLang == null)
			{
				return false;
			}
			string text = LLBase.ResolveAlias(lngId);
			string text2;
			if (!LLBase.currentLang.dictionary.TryGetValue(text, out text2))
			{
				return false;
			}
			if (LLBase.currentLang.id == "en")
			{
				return true;
			}
			string text3 = LL.NormalizeForTranslationCompare(text2);
			if (string.IsNullOrEmpty(text3))
			{
				return false;
			}
			LL ll = LL.GetEnglishLang();
			string text4;
			return !(ll != null) || !ll.dictionary.TryGetValue(text, out text4) || !(text3 == LL.NormalizeForTranslationCompare(text4));
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001ACB8 File Offset: 0x00018EB8
		private static string NormalizeForTranslationCompare(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			s = LL.nobrTagRegex.Replace(s, string.Empty);
			s = LL.spaceTagRegex.Replace(s, string.Empty);
			s = s.Replace("\u200b", string.Empty);
			s = LL.whitespaceRegex.Replace(s, " ").Trim();
			return s;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001AD21 File Offset: 0x00018F21
		private static LL GetEnglishLang()
		{
			if (LL.englishLang == null)
			{
				LL.englishLang = Resources.Load<LL>("Locales/lng_en");
				if (LL.englishLang != null)
				{
					LL.englishLang.InitHashDictionary();
				}
			}
			return LL.englishLang;
		}

		// Token: 0x04000250 RID: 592
		private const string LATIN_AMERICA_CODE = "es-419";

		// Token: 0x04000251 RID: 593
		private static LL englishLang;

		// Token: 0x04000252 RID: 594
		private static readonly Regex nobrTagRegex = new Regex("</?nobr>", RegexOptions.IgnoreCase);

		// Token: 0x04000253 RID: 595
		private static readonly Regex spaceTagRegex = new Regex("<space=\\d+>", RegexOptions.IgnoreCase);

		// Token: 0x04000254 RID: 596
		private static readonly Regex whitespaceRegex = new Regex("\\s+");

		// Token: 0x020001E2 RID: 482
		public enum LocModificator
		{
			// Token: 0x04000655 RID: 1621
			None,
			// Token: 0x04000656 RID: 1622
			VoiceOverMuted
		}
	}
}
