using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LinqTools;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000104 RID: 260
	public abstract class LLBase : ScriptableObject
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000511 RID: 1297 RVA: 0x0001AD94 File Offset: 0x00018F94
		public static string CurrentLang
		{
			get
			{
				LL ll = LLBase.currentLang;
				return ((ll != null) ? ll.id : null) ?? "en";
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000512 RID: 1298 RVA: 0x0001ADB0 File Offset: 0x00018FB0
		public static bool IsCurrentLangLoaded
		{
			get
			{
				return LLBase.currentLang != null;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001ADBD File Offset: 0x00018FBD
		public static Dictionary<string, LLBase.LanguageInfo> AvailableLanguages
		{
			get
			{
				if (LLBase.currentLang == null)
				{
					LLBase.currentLang = new LL();
				}
				return LLBase.currentLang.GetAvailableLanguages();
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0001ADE0 File Offset: 0x00018FE0
		public static void InitReplacementRuleSet(BaseReplacementRuleSet replacementRuleSet)
		{
			LLBase.replacementRuleSet = replacementRuleSet;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0001ADE8 File Offset: 0x00018FE8
		public static void LoadLanguageResource(string langId)
		{
			LL ll;
			if (LanguageModHooks.TryGetLanguage != null && LanguageModHooks.TryGetLanguage(langId, out ll) && ll != null)
			{
				LLBase.currentLang = ll;
				LLBase.currentLang.InitHashDictionary();
				Debug.Log(string.Concat(new string[]
				{
					"LoadLanguageResource(\"",
					langId,
					"\") [mod], ",
					LLBase.currentLang.txts.Count.ToString(),
					" lines loaded"
				}));
				return;
			}
			LLBase.currentLang = Resources.Load<LL>("Locales/lng_" + langId);
			if (LLBase.currentLang == null && langId != "en")
			{
				LLBase.LoadLanguageResource("en");
			}
			Debug.Log(string.Concat(new string[]
			{
				"LoadLanguageResource(\"",
				langId,
				"\"), ",
				LLBase.currentLang.txts.Count.ToString(),
				" lines loaded"
			}));
			LLBase.currentLang.InitHashDictionary();
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001AEF4 File Offset: 0x000190F4
		public int TextEntryCount
		{
			get
			{
				return this.txtIds.Count;
			}
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0001AF01 File Offset: 0x00019101
		public string GetTextIdAt(int index)
		{
			return this.txtIds[index];
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0001AF10 File Offset: 0x00019110
		public string GetTextWithMarkup(int index)
		{
			string text = this.txts[index];
			if (index < this.nestedLocalesMetaInfos.Count && this.nestedLocalesMetaInfos[index].hasMetaInfo)
			{
				int num = 0;
				for (int i = 0; i < this.nestedLocalesMetaInfos[index].idsToInsert.Count; i++)
				{
					string text2 = "#(" + this.nestedLocalesMetaInfos[index].localeIDsToInsert[i] + ")";
					text = text.Insert(this.nestedLocalesMetaInfos[index].idsToInsert[i] + num, text2);
					num += text2.Length;
				}
			}
			ReplacementKeysMetadata replacementKeysMetadata = this.replacementKeysMetaInfoList.Find((ReplacementKeysMetadata x) => x.id == this.txtIds[index]);
			if (replacementKeysMetadata != null)
			{
				int num2 = 0;
				for (int j = 0; j < replacementKeysMetadata.keys.Count; j++)
				{
					string text3 = "@(" + replacementKeysMetadata.keys[j] + ")";
					text = text.Insert(replacementKeysMetadata.keysIndexes[j] + num2, text3);
					num2 += text3.Length;
				}
			}
			return text;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0001B080 File Offset: 0x00019280
		public void InitHashDictionary()
		{
			this.dictionary.Clear();
			this.idsToMetaInfo.Clear();
			for (int i = 0; i < this.txtIds.Count; i++)
			{
				this.dictionary.Add(this.txtIds[i], this.txts[i]);
				this.idsToMetaInfo.Add(this.txtIds[i], this.nestedLocalesMetaInfos[i]);
			}
			this.replacementIdMetaInfo.Clear();
			foreach (ReplacementKeysMetadata replacementKeysMetadata in this.replacementKeysMetaInfoList)
			{
				this.replacementIdMetaInfo.Add(replacementKeysMetadata.id, replacementKeysMetadata);
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0001B15C File Offset: 0x0001935C
		public static string GetCurrentLocaleCode()
		{
			bool flag = false;
			if (LLBase.currentLang == null)
			{
				flag = true;
				LLBase.currentLang = new LL();
			}
			string text = LLBase.currentLang.GetLocaleCode().ToLower();
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
			if (!LLBase.languages.ContainsKey(text))
			{
				Debug.LogWarning("Language '" + text + "' not found. Loading EN...");
				text = "en";
			}
			return text;
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0001B200 File Offset: 0x00019400
		protected virtual Dictionary<string, LLBase.LanguageInfo> GetAvailableLanguages()
		{
			return new Dictionary<string, LLBase.LanguageInfo>
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
					"it",
					new LLBase.LanguageInfo("it", "it-IT", "Italiano")
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
					new LLBase.LanguageInfo("zh_cn", "zh-CN", "Chinese")
				},
				{
					"ko",
					new LLBase.LanguageInfo("ko", "ko-KR", "Korean")
				}
			};
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0001B368 File Offset: 0x00019568
		protected virtual string GetLocaleCode()
		{
			SystemLanguage systemLanguage = Application.systemLanguage;
			if (systemLanguage <= SystemLanguage.German)
			{
				if (systemLanguage <= SystemLanguage.Chinese)
				{
					if (systemLanguage != SystemLanguage.Belarusian)
					{
						if (systemLanguage != SystemLanguage.Chinese)
						{
							goto IL_00BF;
						}
						return "zh_cn";
					}
				}
				else
				{
					if (systemLanguage == SystemLanguage.French)
					{
						return "fr";
					}
					if (systemLanguage != SystemLanguage.German)
					{
						goto IL_00BF;
					}
					return "de";
				}
			}
			else if (systemLanguage <= SystemLanguage.Spanish)
			{
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
					goto IL_00BF;
				case SystemLanguage.Polish:
					return "pl";
				case SystemLanguage.Portuguese:
					return "pt-br";
				case SystemLanguage.Russian:
					break;
				default:
					if (systemLanguage != SystemLanguage.Spanish)
					{
						goto IL_00BF;
					}
					return "es";
				}
			}
			else
			{
				if (systemLanguage == SystemLanguage.Ukrainian)
				{
					return "uk-ua";
				}
				if (systemLanguage != SystemLanguage.ChineseTraditional)
				{
					goto IL_00BF;
				}
				return "zh_cht";
			}
			return "ru";
			IL_00BF:
			return "en";
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001B43C File Offset: 0x0001963C
		public void AddLangString(string id, ref string txt, bool isEllipsisFriendly = false)
		{
			txt = txt.Replace("(*", "<sprite name=\"").Replace("*)", "\">");
			txt = txt.Replace('“', '"');
			txt = txt.Replace('„', '"');
			txt = txt.Replace('・', '⊙');
			if (isEllipsisFriendly)
			{
				txt = txt.Replace("...", "…");
			}
			NestedLocalesMetaInfo nestedLocalesMetaInfo = this.GetNestedLocalesMetaInfo(ref txt);
			ReplacementKeysMetadata replacementKeysMetadata = this.FormReplacementMetadataFor(id, ref txt);
			int num = this.txtIds.IndexOf(id);
			if (num == -1)
			{
				this.txtIds.Add(id);
				this.txts.Add(txt);
				this.nestedLocalesMetaInfos.Add(nestedLocalesMetaInfo);
				if (replacementKeysMetadata != null)
				{
					this.replacementKeysMetaInfoList.Add(replacementKeysMetadata);
					return;
				}
			}
			else
			{
				this.txtIds[num] = id;
				this.txts[num] = txt;
				this.nestedLocalesMetaInfos[num] = nestedLocalesMetaInfo;
				if (replacementKeysMetadata != null)
				{
					this.replacementKeysMetaInfoList[num] = replacementKeysMetadata;
				}
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0001B544 File Offset: 0x00019744
		public static LLBase.LanguageInfo GetAvailableLanguageInfoByIndex(int index)
		{
			index = Mathf.Clamp(index, 0, LLBase.AvailableLanguages.Count - 1);
			return LLBase.AvailableLanguages.ElementAt(index).Value;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001B57C File Offset: 0x0001977C
		public static LLBase.LanguageInfo GetLanguageInfoById(string languageID)
		{
			LLBase.LanguageInfo languageInfo;
			LLBase.AvailableLanguages.TryGetValue(languageID, out languageInfo);
			return languageInfo;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001B598 File Offset: 0x00019798
		public static int GetIndexByLanguage(string languageCode)
		{
			int num = 0;
			foreach (KeyValuePair<string, LLBase.LanguageInfo> keyValuePair in LLBase.AvailableLanguages)
			{
				if (keyValuePair.Value.id == languageCode)
				{
					return num;
				}
				num++;
			}
			return num;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001B604 File Offset: 0x00019804
		public static string[] GetLanguagesRange()
		{
			return LLBase.GetLanguagesRange(LLBase.languages);
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001B610 File Offset: 0x00019810
		public static string[] GetAvailableLanguagesRange()
		{
			return LLBase.GetLanguagesRange(LLBase.AvailableLanguages);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001B61C File Offset: 0x0001981C
		public static string[] GetAvailableLanguageNamesRange()
		{
			string[] array = new string[LLBase.AvailableLanguages.Count];
			int num = 0;
			foreach (KeyValuePair<string, LLBase.LanguageInfo> keyValuePair in LLBase.AvailableLanguages)
			{
				array[num] = keyValuePair.Value.name;
				num++;
			}
			return array;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0001B690 File Offset: 0x00019890
		private static string[] GetLanguagesRange(Dictionary<string, LLBase.LanguageInfo> languageCollection)
		{
			string[] array = new string[languageCollection.Count];
			int num = 0;
			foreach (KeyValuePair<string, LLBase.LanguageInfo> keyValuePair in languageCollection)
			{
				array[num] = keyValuePair.Value.id;
				num++;
			}
			return array;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001B6FC File Offset: 0x000198FC
		public static string L(string lngId)
		{
			if (lngId == null)
			{
				Debug.LogError("lng_id is null");
				return "";
			}
			if (LLBase.currentLang != null)
			{
				int num = LLBase.currentLang.aliases1.IndexOf(lngId);
				if (num != -1)
				{
					return LLBase.L(LLBase.currentLang.aliases2[num]);
				}
			}
			string text;
			if (LLBase.currentLang == null || !LLBase.currentLang.dictionary.ContainsKey(lngId))
			{
				text = lngId;
			}
			else
			{
				text = LLBase.currentLang.dictionary[lngId];
				NestedLocalesMetaInfo nestedLocalesMetaInfo = LLBase.currentLang.idsToMetaInfo[lngId];
				if (nestedLocalesMetaInfo.hasMetaInfo)
				{
					int num2 = 0;
					for (int i = 0; i < nestedLocalesMetaInfo.idsToInsert.Count; i++)
					{
						string text2 = LLBase.L(nestedLocalesMetaInfo.localeIDsToInsert[i]);
						text = text.Insert(nestedLocalesMetaInfo.idsToInsert[i] + num2, text2);
						num2 += text2.Length;
					}
				}
				if (LLBase.currentLang.replacementIdMetaInfo.ContainsKey(lngId))
				{
					ReplacementKeysMetadata replacementKeysMetadata = LLBase.currentLang.replacementIdMetaInfo[lngId];
					int num3 = 0;
					for (int j = 0; j < replacementKeysMetadata.keys.Count; j++)
					{
						string text3 = LLBase.replacementRuleSet.Replace(replacementKeysMetadata.keys[j]);
						text = text.Insert(replacementKeysMetadata.keysIndexes[j] + num3, text3);
						num3 += text3.Length;
					}
				}
			}
			text = text.Replace("&#xA;", "\n");
			text = text.Replace("’", "'");
			text = text.Replace('”', '"');
			text = text.Replace('“', '"');
			return text.Replace("\\u00A0", "\u00a0");
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001B8C8 File Offset: 0x00019AC8
		public static bool HasL(string lngId)
		{
			return lngId != null && ((LLBase.currentLang != null && LLBase.currentLang.aliases1.IndexOf(lngId) != -1) || (!(LLBase.currentLang == null) && LLBase.currentLang.dictionary.ContainsKey(lngId)));
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001B920 File Offset: 0x00019B20
		public static string ResolveAlias(string lngId)
		{
			if (string.IsNullOrEmpty(lngId) || LLBase.currentLang == null)
			{
				return lngId;
			}
			string text = lngId;
			for (int i = 0; i < 16; i++)
			{
				int num = LLBase.currentLang.aliases1.IndexOf(text);
				if (num < 0)
				{
					return text;
				}
				string text2 = LLBase.currentLang.aliases2[num];
				if (string.IsNullOrEmpty(text2))
				{
					return text;
				}
				text = text2;
			}
			return text;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001B988 File Offset: 0x00019B88
		public static string L(string id, string value)
		{
			return LLBase.L(id).Replace("%1", value);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001B99B File Offset: 0x00019B9B
		public static string L(string id, string value1, string value2)
		{
			return LLBase.L(id).Replace("%1", value1).Replace("%2", value2);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001B9B9 File Offset: 0x00019BB9
		public static string L(string id, string value1, string value2, string value3)
		{
			return LLBase.L(id).Replace("%1", value1).Replace("%2", value2)
				.Replace("%3", value3);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001B9E2 File Offset: 0x00019BE2
		public static string L(string id, int value)
		{
			return LLBase.L(id, value.ToString());
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001B9F1 File Offset: 0x00019BF1
		public static string L(string id, float value)
		{
			return LLBase.L(id, value.ToString());
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001BA00 File Offset: 0x00019C00
		public static string L(string id, int value1, int value2)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString());
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001BA16 File Offset: 0x00019C16
		public static string L(string id, string value1, int value2)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString());
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001BA2B File Offset: 0x00019C2B
		public static string L(string id, int value1, string value2)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString());
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001BA40 File Offset: 0x00019C40
		public static string L(string id, float value1, float value2)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString());
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001BA56 File Offset: 0x00019C56
		public static string L(string id, string value1, float value2)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString());
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001BA6B File Offset: 0x00019C6B
		public static string L(string id, float value1, string value2)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString());
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001BA80 File Offset: 0x00019C80
		public static string L(string id, string value1, int value2, int value3)
		{
			return LLBase.L(id, value1.ToString(), value2.ToString(), value3.ToString());
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001BA9C File Offset: 0x00019C9C
		public static bool IsEastern()
		{
			return !(LLBase.currentLang == null) && LLBase.easternLangIds.Contains(LLBase.currentLang.id);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001BAC1 File Offset: 0x00019CC1
		public static bool IsEllipsisFriendly(string langId)
		{
			return LLBase.ellipsisFriendlyLangIds.Contains(langId);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001BAD0 File Offset: 0x00019CD0
		public static string GetLocaleNameByCode(string localeCode)
		{
			foreach (KeyValuePair<string, LLBase.LanguageInfo> keyValuePair in LLBase.AvailableLanguages)
			{
				if (keyValuePair.Value.id == localeCode)
				{
					return keyValuePair.Value.name;
				}
			}
			return "?";
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001BB48 File Offset: 0x00019D48
		private NestedLocalesMetaInfo GetNestedLocalesMetaInfo(ref string s)
		{
			NestedLocalesMetaInfo nestedLocalesMetaInfo = new NestedLocalesMetaInfo();
			MatchCollection matchCollection = new Regex("\\#\\(.*?\\)").Matches(s);
			int num = 0;
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				s = s.Replace(match.Value, "");
				string text = match.Value.Substring(2, match.Value.Length - 3);
				nestedLocalesMetaInfo.AddEntry(match.Index - num, text);
				num += match.Value.Length;
			}
			return nestedLocalesMetaInfo;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001BC00 File Offset: 0x00019E00
		private ReplacementKeysMetadata FormReplacementMetadataFor(string id, ref string inputStr)
		{
			ReplacementKeysMetadata replacementKeysMetadata = new ReplacementKeysMetadata();
			MatchCollection matchCollection = new Regex("\\@\\(.*?\\)").Matches(inputStr);
			if (matchCollection.Count == 0)
			{
				return null;
			}
			replacementKeysMetadata.id = id;
			int num = 0;
			foreach (object obj in matchCollection)
			{
				Match match = (Match)obj;
				inputStr = inputStr.Replace(match.Value, "");
				string text = match.Value.Substring(2, match.Value.Length - 3);
				replacementKeysMetadata.keysIndexes.Add(match.Index - num);
				replacementKeysMetadata.keys.Add(text);
				num += match.Value.Length;
			}
			return replacementKeysMetadata;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001BCE0 File Offset: 0x00019EE0
		public static void AddAliases(List<string> alias1, List<string> alias2)
		{
			foreach (KeyValuePair<string, LLBase.LanguageInfo> keyValuePair in LLBase.languages)
			{
				LL ll = Resources.Load<LL>("Locales/lng_" + keyValuePair.Value.id);
				if (!(ll == null))
				{
					for (int i = 0; i < alias1.Count; i++)
					{
						if (!ll.aliases1.Contains(alias1[i]))
						{
							ll.aliases1.Add(alias1[i]);
							ll.aliases2.Add(alias2[i]);
						}
					}
				}
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001BD9C File Offset: 0x00019F9C
		public string ToJSON()
		{
			LLBase.JsonData jsonData = new LLBase.JsonData();
			for (int i = 0; i < this.txtIds.Count; i++)
			{
				jsonData.data.Add(new LLBase.JsonEntry
				{
					key = this.txtIds[i],
					value = this.txts[i]
				});
			}
			return JsonUtility.ToJson(jsonData, true);
		}

		// Token: 0x04000255 RID: 597
		public const string DEFAULT_LANGUAGE = "en";

		// Token: 0x04000256 RID: 598
		public static readonly Dictionary<string, LLBase.LanguageInfo> languages = new Dictionary<string, LLBase.LanguageInfo>
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
				"es-mx",
				new LLBase.LanguageInfo("es", "es-MX", "Español")
			},
			{
				"ru",
				new LLBase.LanguageInfo("ru", "ru-RU", "Русский")
			},
			{
				"uk-ua",
				new LLBase.LanguageInfo("uk-ua", "uk-UA", "Україньска")
			},
			{
				"it",
				new LLBase.LanguageInfo("it", "it-IT", "Italiano")
			},
			{
				"pl",
				new LLBase.LanguageInfo("pl", "pl-PL", "Polski")
			},
			{
				"tr",
				new LLBase.LanguageInfo("tr", "tr-TR", "Türkçe")
			},
			{
				"ja",
				new LLBase.LanguageInfo("ja", "ja-JP", "Japanese")
			},
			{
				"zh_cn",
				new LLBase.LanguageInfo("zh_cn", "zh-CN", "Chinese")
			},
			{
				"zh_cht",
				new LLBase.LanguageInfo("zh_cht", "zh-CHT", "Chinese-Traditional")
			},
			{
				"ko",
				new LLBase.LanguageInfo("ko", "ko-KR", "Korean")
			},
			{
				"th",
				new LLBase.LanguageInfo("th", "th-TH", "Siamese")
			},
			{
				"vn",
				new LLBase.LanguageInfo("vn", "vn-VN", "tiếng Việt")
			}
		};

		// Token: 0x04000257 RID: 599
		private static readonly HashSet<string> easternLangIds = new HashSet<string> { "ja", "zh_cn", "zh_cht", "ko", "th", "vn" };

		// Token: 0x04000258 RID: 600
		private static readonly HashSet<string> ellipsisFriendlyLangIds = new HashSet<string> { "ja", "zh_cn", "ko" };

		// Token: 0x04000259 RID: 601
		[NonSerialized]
		public Dictionary<string, string> dictionary = new Dictionary<string, string>();

		// Token: 0x0400025A RID: 602
		[NonSerialized]
		public Dictionary<string, NestedLocalesMetaInfo> idsToMetaInfo = new Dictionary<string, NestedLocalesMetaInfo>();

		// Token: 0x0400025B RID: 603
		[NonSerialized]
		public Dictionary<string, ReplacementKeysMetadata> replacementIdMetaInfo = new Dictionary<string, ReplacementKeysMetadata>();

		// Token: 0x0400025C RID: 604
		public List<string> aliases1 = new List<string>();

		// Token: 0x0400025D RID: 605
		public List<string> aliases2 = new List<string>();

		// Token: 0x0400025E RID: 606
		public string id = "en";

		// Token: 0x0400025F RID: 607
		[SerializeField]
		protected List<string> txtIds = new List<string>();

		// Token: 0x04000260 RID: 608
		[SerializeField]
		protected List<string> txts = new List<string>();

		// Token: 0x04000261 RID: 609
		[SerializeField]
		public List<NestedLocalesMetaInfo> nestedLocalesMetaInfos = new List<NestedLocalesMetaInfo>();

		// Token: 0x04000262 RID: 610
		[SerializeField]
		public List<ReplacementKeysMetadata> replacementKeysMetaInfoList = new List<ReplacementKeysMetadata>();

		// Token: 0x04000263 RID: 611
		protected static BaseReplacementRuleSet replacementRuleSet = new BaseReplacementRuleSet();

		// Token: 0x04000264 RID: 612
		protected static LL currentLang = null;

		// Token: 0x020001E3 RID: 483
		public class LanguageInfo
		{
			// Token: 0x06000A50 RID: 2640 RVA: 0x0002EBFB File Offset: 0x0002CDFB
			public LanguageInfo(string id, string iso, string name)
			{
				this.id = id;
				this.iso = iso;
				this.name = name;
			}

			// Token: 0x04000657 RID: 1623
			public string id;

			// Token: 0x04000658 RID: 1624
			public string iso;

			// Token: 0x04000659 RID: 1625
			public string name;
		}

		// Token: 0x020001E4 RID: 484
		[Serializable]
		public class JsonEntry
		{
			// Token: 0x0400065A RID: 1626
			public string key;

			// Token: 0x0400065B RID: 1627
			public string value;
		}

		// Token: 0x020001E5 RID: 485
		[Serializable]
		public class JsonData
		{
			// Token: 0x0400065C RID: 1628
			public List<LLBase.JsonEntry> data = new List<LLBase.JsonEntry>();
		}
	}
}
