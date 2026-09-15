using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

namespace LazyBearTechnology
{
	// Token: 0x02000C4D RID: 3149
	public static class LanguageModLoader
	{
		// Token: 0x06005037 RID: 20535 RVA: 0x0017AC18 File Offset: 0x00178E18
		static LanguageModLoader()
		{
			LanguageModLoader.BindHooks();
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x0017ACC0 File Offset: 0x00178EC0
		private static void BindHooks()
		{
			LanguageModHooks.TryGetLanguage = new LanguageModHooks.TryGetLanguageHandler(LanguageModLoader.TryGetLanguage);
			LanguageModHooks.TryGetFontAsset = new LanguageModHooks.TryGetFontAssetHandler(LanguageModLoader.TryGetFontAsset);
			LanguageModHooks.UsesOwnMaterial = new LanguageModHooks.UsesOwnMaterialHandler(LanguageModLoader.UsesOwnMaterial);
			LanguageModHooks.RequiresResize = new LanguageModHooks.RequiresResizeHandler(LanguageModLoader.RequiresResize);
			LanguageModHooks.ApplyDirection = new LanguageModHooks.ApplyDirectionHandler(LanguageModLoader.ApplyDirection);
			LanguageModHooks.AppendLanguages = new LanguageModHooks.AppendLanguagesHandler(LanguageModLoader.AppendLanguages);
			VoiceOverModLoader.SearchRootsProvider = new Func<IReadOnlyList<string>>(ModsPaths.GetModSearchRoots);
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x0017AD44 File Offset: 0x00178F44
		public static void Scan()
		{
			LanguageModLoader.BindHooks();
			List<TextStyle> list = LanguageModLoader.CollectRegisteredStringMaterialStyles();
			foreach (LanguageModLoader.PackedLanguage packedLanguage in LanguageModLoader.packs.Values)
			{
				if (packedLanguage.language != null)
				{
					global::UnityEngine.Object.Destroy(packedLanguage.language);
				}
				LanguageModLoader.DestroyRuntimeFontAsset(packedLanguage.customFontAsset);
			}
			LanguageModLoader.UnregisterDynamicStyleMaterials();
			TextStyle.ClearDynamicMaterialCaches();
			LanguageModLoader.PurgeDestroyedTmpLookups();
			LanguageModLoader.packs.Clear();
			LanguageModLoader.shippedMarkupCache.Clear();
			LanguageModLoader.resolvedMarkupCache.Clear();
			LL ll = LanguageModLoader.LoadEnglish();
			if (ll == null)
			{
				Debug.LogWarning("[LanguageMod] English locale asset is missing. Cannot load language packs.");
				LanguageModLoader.ReregisterStringMaterials(list);
				return;
			}
			Dictionary<string, string> dictionary = LanguageModLoader.BuildMarkupMap(ll);
			LanguageModLoader.shippedMarkupCache["en"] = dictionary;
			Dictionary<string, LanguageModLoader.PackSource> dictionary2 = new Dictionary<string, LanguageModLoader.PackSource>();
			LanguageModLoader.CollectSources(dictionary2);
			foreach (LanguageModLoader.PackSource packSource in dictionary2.Values)
			{
				LanguageModLoader.BuildPack(packSource, dictionary2, ll, dictionary);
			}
			Debug.Log(string.Format("[LanguageMod] Loaded {0} language pack(s). Search roots: {1}", LanguageModLoader.packs.Count, string.Join(" | ", ModsPaths.GetModSearchRoots())));
			foreach (LanguageModLoader.PackSource packSource2 in dictionary2.Values)
			{
				Debug.Log(string.Concat(new string[] { "[LanguageMod] Pack id='", packSource2.id, "' dir='", packSource2.directory, "'" }));
			}
			LanguageModLoader.ReregisterStringMaterials(list);
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x0017AF28 File Offset: 0x00179128
		private static void CollectSources(Dictionary<string, LanguageModLoader.PackSource> sources)
		{
			IReadOnlyList<string> modSearchRoots = ModsPaths.GetModSearchRoots();
			for (int i = 0; i < modSearchRoots.Count; i++)
			{
				string text = modSearchRoots[i];
				LanguageModLoader.TryAddPackDirectory(sources, text, false);
				string text2 = Path.Combine(text, "Languages");
				if (Directory.Exists(text2))
				{
					string[] directories = Directory.GetDirectories(text2);
					Array.Sort<string>(directories, StringComparer.OrdinalIgnoreCase);
					for (int j = 0; j < directories.Length; j++)
					{
						LanguageModLoader.TryAddPackDirectory(sources, directories[j], true);
					}
				}
			}
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x0017AFA4 File Offset: 0x001791A4
		private static void TryAddPackDirectory(Dictionary<string, LanguageModLoader.PackSource> sources, string directory, bool warnIfMissing)
		{
			string fileName = Path.GetFileName(directory);
			if (ModsPaths.IsDisabledFolderName(fileName))
			{
				return;
			}
			LanguageModLoader.PackSource packSource;
			if (!LanguageModLoader.TryReadSource(directory, fileName, warnIfMissing, out packSource))
			{
				return;
			}
			if (string.IsNullOrEmpty(packSource.id))
			{
				return;
			}
			if (LLBase.languages.ContainsKey(packSource.id))
			{
				Debug.LogWarning("[LanguageMod] Skipping '" + packSource.id + "': id collides with a shipped language.");
				return;
			}
			if (sources.ContainsKey(packSource.id))
			{
				Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] Skipping '", directory, "': id '", packSource.id, "' already loaded from another folder." }));
				return;
			}
			sources[packSource.id] = packSource;
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x0017B05C File Offset: 0x0017925C
		public static void AppendLanguages(Dictionary<string, LLBase.LanguageInfo> languages)
		{
			foreach (KeyValuePair<string, LanguageModLoader.PackedLanguage> keyValuePair in LanguageModLoader.packs)
			{
				if (!languages.ContainsKey(keyValuePair.Key))
				{
					languages.Add(keyValuePair.Key, new LLBase.LanguageInfo(keyValuePair.Value.id, keyValuePair.Value.id, keyValuePair.Value.displayName));
				}
			}
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x0017B0EC File Offset: 0x001792EC
		public static bool TryGetLanguage(string langId, out LL language)
		{
			language = null;
			if (string.IsNullOrEmpty(langId))
			{
				return false;
			}
			LanguageModLoader.PackedLanguage packedLanguage;
			if (!LanguageModLoader.packs.TryGetValue(langId, out packedLanguage))
			{
				return false;
			}
			language = packedLanguage.language;
			return language != null;
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x0017B128 File Offset: 0x00179328
		public static bool TryGetFontAsset(string lang, bool staticFont, out TMP_FontAsset fontAsset)
		{
			fontAsset = null;
			if (staticFont || string.IsNullOrEmpty(lang))
			{
				return false;
			}
			LanguageModLoader.PackedLanguage packedLanguage;
			if (!LanguageModLoader.packs.TryGetValue(lang, out packedLanguage))
			{
				return false;
			}
			if (LanguageModLoader.IsExternalFontFile(packedLanguage.font))
			{
				return LanguageModLoader.TryGetOrCreateExternalFont(packedLanguage, out fontAsset);
			}
			string text = LanguageModLoader.NormalizeFont(packedLanguage.font);
			if (text == "default")
			{
				return false;
			}
			string text2;
			if (!LanguageModLoader.FontResourcePaths.TryGetValue(text, out text2))
			{
				return false;
			}
			if (!LanguageModLoader.fontCache.TryGetValue(text2, out fontAsset) || fontAsset == null)
			{
				fontAsset = Resources.Load<TMP_FontAsset>(text2);
				LanguageModLoader.fontCache[text2] = fontAsset;
			}
			return fontAsset != null;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x0017B1CC File Offset: 0x001793CC
		public static bool UsesOwnMaterial(string lang)
		{
			LanguageModLoader.PackedLanguage packedLanguage;
			if (string.IsNullOrEmpty(lang) || !LanguageModLoader.packs.TryGetValue(lang, out packedLanguage))
			{
				return false;
			}
			if (LanguageModLoader.IsExternalFontFile(packedLanguage.font))
			{
				return packedLanguage.useOwnMaterial;
			}
			return LanguageModLoader.FontsUsingOwnMaterial.Contains(LanguageModLoader.NormalizeFont(packedLanguage.font));
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x0017B21C File Offset: 0x0017941C
		public static bool IsRightToLeft(string lang)
		{
			LanguageModLoader.PackedLanguage packedLanguage;
			return !string.IsNullOrEmpty(lang) && LanguageModLoader.packs.TryGetValue(lang, out packedLanguage) && packedLanguage.rtl;
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x0017B248 File Offset: 0x00179448
		public static bool RequiresResize(string lang)
		{
			LanguageModLoader.PackedLanguage packedLanguage;
			return !string.IsNullOrEmpty(lang) && LanguageModLoader.packs.TryGetValue(lang, out packedLanguage) && packedLanguage.requireResize;
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x0017B274 File Offset: 0x00179474
		public static void ApplyDirection(TMP_Text label, string lang, bool staticFont)
		{
			if (label == null)
			{
				return;
			}
			LanguageRtlLabelState languageRtlLabelState = label.GetComponent<LanguageRtlLabelState>();
			if (languageRtlLabelState == null)
			{
				languageRtlLabelState = label.gameObject.AddComponent<LanguageRtlLabelState>();
				languageRtlLabelState.hideFlags = HideFlags.HideAndDontSave;
			}
			if (!languageRtlLabelState.captured)
			{
				LanguageModLoader.DestroyRuntimePreprocessors(label, LanguageModLoader.NoPreprocessorTypes);
				languageRtlLabelState.originalRtl = label.isRightToLeftText;
				languageRtlLabelState.originalAlignment = label.alignment;
				languageRtlLabelState.originalPreprocessor = label.textPreprocessor;
				languageRtlLabelState.captured = true;
			}
			bool flag = !staticFont && LanguageModLoader.IsRightToLeft(lang);
			label.isRightToLeftText = flag || languageRtlLabelState.originalRtl;
			bool flag2 = false;
			LocalizedRtlAlign component = label.GetComponent<LocalizedRtlAlign>();
			if (component != null && component.DontChangeAlignOnRtl)
			{
				flag2 = true;
			}
			label.alignment = ((flag && !flag2) ? LanguageModLoader.FlipHorizontalAlignment(languageRtlLabelState.originalAlignment) : languageRtlLabelState.originalAlignment);
			Type[] array = (staticFont ? LanguageModLoader.NoPreprocessorTypes : LanguageModLoader.GetPreprocessorTypes(lang));
			LanguageModLoader.SyncPreprocessors(label, array, languageRtlLabelState);
		}

		// Token: 0x06005043 RID: 20547 RVA: 0x0017B364 File Offset: 0x00179564
		public static void EnsureExamplePack()
		{
			string exampleLanguageFolder = ModsPaths.ExampleLanguageFolder;
			if (Directory.Exists(exampleLanguageFolder))
			{
				return;
			}
			Directory.CreateDirectory(exampleLanguageFolder);
			File.WriteAllText(Path.Combine(exampleLanguageFolder, "language.json"), ModsDocumentation.Load("Mods/Languages/language"));
			LL ll = LanguageModLoader.LoadEnglish();
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			if (ll != null)
			{
				int textEntryCount = ll.TextEntryCount;
				for (int i = 0; i < textEntryCount; i++)
				{
					list.Add(new KeyValuePair<string, string>(ll.GetTextIdAt(i), ll.GetTextWithMarkup(i)));
				}
			}
			ModsCsv.WriteKeyValueFile(Path.Combine(exampleLanguageFolder, "strings.csv"), list);
			Debug.Log("[LanguageMod] Created example pack at '" + exampleLanguageFolder + "'.");
		}

		// Token: 0x06005044 RID: 20548 RVA: 0x0017B410 File Offset: 0x00179610
		private static bool TryReadSource(string directory, string folderName, bool warnIfMissing, out LanguageModLoader.PackSource source)
		{
			source = null;
			string text = Path.Combine(directory, "language.json");
			if (!File.Exists(text))
			{
				if (warnIfMissing)
				{
					Debug.LogWarning("[LanguageMod] '" + folderName + "' has no language.json. Pack ignored.");
				}
				return false;
			}
			LanguageModManifest languageModManifest;
			try
			{
				languageModManifest = JsonUtility.FromJson<LanguageModManifest>(ModsJsonComments.Strip(File.ReadAllText(text)));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[LanguageMod] Failed to parse '" + text + "': " + ex.Message);
				return false;
			}
			if (languageModManifest == null)
			{
				Debug.LogWarning("[LanguageMod] Failed to parse '" + text + "'.");
				return false;
			}
			if (string.IsNullOrEmpty(folderName))
			{
				return false;
			}
			source = new LanguageModLoader.PackSource
			{
				id = folderName,
				directory = directory,
				manifest = languageModManifest,
				overlay = ModsCsv.ReadKeyValueFile(Path.Combine(directory, "strings.csv"))
			};
			return true;
		}

		// Token: 0x06005045 RID: 20549 RVA: 0x0017B4F0 File Offset: 0x001796F0
		private static void BuildPack(LanguageModLoader.PackSource source, Dictionary<string, LanguageModLoader.PackSource> sources, LL english, Dictionary<string, string> englishMarkup)
		{
			string id = source.id;
			Dictionary<string, string> overlay = source.overlay;
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { id };
			Dictionary<string, string> dictionary = LanguageModLoader.ResolveFallbackMarkup(source.manifest.fallback, id, sources, hashSet, englishMarkup);
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
			LL ll = ScriptableObject.CreateInstance<LL>();
			ll.hideFlags = HideFlags.HideAndDontSave;
			ll.id = id;
			ll.aliases1.AddRange(english.aliases1);
			ll.aliases2.AddRange(english.aliases2);
			bool rtl = source.manifest.rtl;
			HashSet<string> hashSet2 = new HashSet<string>();
			int textEntryCount = english.TextEntryCount;
			for (int i = 0; i < textEntryCount; i++)
			{
				string textIdAt = english.GetTextIdAt(i);
				string text2;
				string text = (englishMarkup.TryGetValue(textIdAt, out text2) ? text2 : english.GetTextWithMarkup(i));
				string text3 = text;
				string text4;
				if (overlay.TryGetValue(textIdAt, out text4) && !string.IsNullOrWhiteSpace(text4))
				{
					text3 = text4;
				}
				else if (textIdAt == "ui_lang_name_loc")
				{
					text3 = (string.IsNullOrWhiteSpace(source.manifest.name) ? id : source.manifest.name.Trim());
				}
				else
				{
					string text5;
					if (dictionary.TryGetValue(textIdAt, out text5) && !string.IsNullOrWhiteSpace(text5))
					{
						text3 = text5;
					}
					list.Add(new KeyValuePair<string, string>(textIdAt, text));
				}
				string text6 = (rtl ? ArabicShaper.Shape(text3) : text3);
				ll.AddLangString(textIdAt, ref text6, false);
				hashSet2.Add(textIdAt);
			}
			foreach (KeyValuePair<string, string> keyValuePair in overlay)
			{
				if (!hashSet2.Contains(keyValuePair.Key) && !string.IsNullOrWhiteSpace(keyValuePair.Value))
				{
					string text7 = (rtl ? ArabicShaper.Shape(keyValuePair.Value) : keyValuePair.Value);
					ll.AddLangString(keyValuePair.Key, ref text7, false);
				}
			}
			ll.InitHashDictionary();
			if (ModsPaths.IsEditableModPath(source.directory))
			{
				ModsCsv.WriteKeyValueFile(Path.Combine(source.directory, "_missing_lines.csv"), list);
			}
			string text8 = (string.IsNullOrWhiteSpace(source.manifest.name) ? id : source.manifest.name.Trim());
			if (rtl)
			{
				text8 = ArabicShaper.Shape(text8);
			}
			string text9 = LanguageModLoader.NormalizeFont(source.manifest.font);
			LanguageModLoader.packs[id] = new LanguageModLoader.PackedLanguage
			{
				id = id,
				displayName = text8,
				font = text9,
				fontDirectory = source.directory,
				fontSettings = (LanguageModLoader.IsExternalFontFile(text9) ? source.manifest.fontSettings : null),
				useOwnMaterial = false,
				rtl = source.manifest.rtl,
				requireResize = source.manifest.requireResize,
				preprocessorTypes = LanguageModLoader.ResolvePreprocessorTypes(source.manifest.preprocessors, id),
				language = ll
			};
		}

		// Token: 0x06005046 RID: 20550 RVA: 0x0017B804 File Offset: 0x00179A04
		private static Dictionary<string, string> ResolveFallbackMarkup(string fallback, string packId, Dictionary<string, LanguageModLoader.PackSource> sources, HashSet<string> visiting, Dictionary<string, string> englishMarkup)
		{
			string text = LanguageModLoader.NormalizeFallbackId(fallback, packId);
			if (string.Equals(text, "en", StringComparison.OrdinalIgnoreCase))
			{
				return englishMarkup;
			}
			LanguageModLoader.PackSource packSource;
			if (LanguageModLoader.TryFindSource(sources, text, out packSource))
			{
				if (!visiting.Add(packSource.id))
				{
					Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] Fallback cycle involving '", packId, "' / '", packSource.id, "'. Using English." }));
					return englishMarkup;
				}
				Dictionary<string, string> dictionary;
				if (LanguageModLoader.resolvedMarkupCache.TryGetValue(packSource.id, out dictionary))
				{
					visiting.Remove(packSource.id);
					return dictionary;
				}
				Dictionary<string, string> dictionary2 = LanguageModLoader.OverlayMarkup(LanguageModLoader.ResolveFallbackMarkup(packSource.manifest.fallback, packSource.id, sources, visiting, englishMarkup), packSource.overlay);
				LanguageModLoader.resolvedMarkupCache[packSource.id] = dictionary2;
				visiting.Remove(packSource.id);
				return dictionary2;
			}
			else
			{
				Dictionary<string, string> dictionary3;
				if (LanguageModLoader.TryGetShippedMarkup(text, out dictionary3))
				{
					return dictionary3;
				}
				Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] '", packId, "' fallback '", text, "' was not found. Using English." }));
				return englishMarkup;
			}
		}

		// Token: 0x06005047 RID: 20551 RVA: 0x0017B928 File Offset: 0x00179B28
		private static Dictionary<string, string> OverlayMarkup(Dictionary<string, string> baseline, Dictionary<string, string> overlay)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(baseline);
			foreach (KeyValuePair<string, string> keyValuePair in overlay)
			{
				if (!string.IsNullOrWhiteSpace(keyValuePair.Value))
				{
					dictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return dictionary;
		}

		// Token: 0x06005048 RID: 20552 RVA: 0x0017B99C File Offset: 0x00179B9C
		private static bool TryFindSource(Dictionary<string, LanguageModLoader.PackSource> sources, string id, out LanguageModLoader.PackSource source)
		{
			if (sources.TryGetValue(id, out source))
			{
				return true;
			}
			foreach (KeyValuePair<string, LanguageModLoader.PackSource> keyValuePair in sources)
			{
				if (string.Equals(keyValuePair.Key, id, StringComparison.OrdinalIgnoreCase))
				{
					source = keyValuePair.Value;
					return true;
				}
			}
			source = null;
			return false;
		}

		// Token: 0x06005049 RID: 20553 RVA: 0x0017BA14 File Offset: 0x00179C14
		private static bool TryGetShippedMarkup(string langId, out Dictionary<string, string> markup)
		{
			if (LanguageModLoader.shippedMarkupCache.TryGetValue(langId, out markup))
			{
				return true;
			}
			foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in LanguageModLoader.shippedMarkupCache)
			{
				if (string.Equals(keyValuePair.Key, langId, StringComparison.OrdinalIgnoreCase))
				{
					markup = keyValuePair.Value;
					return true;
				}
			}
			LL ll = Resources.Load<LL>("Locales/lng_" + langId);
			if (ll == null && !string.Equals(langId, langId.ToLowerInvariant(), StringComparison.Ordinal))
			{
				ll = Resources.Load<LL>("Locales/lng_" + langId.ToLowerInvariant());
			}
			if (ll == null)
			{
				markup = null;
				return false;
			}
			if (ll.dictionary == null || ll.dictionary.Count == 0)
			{
				ll.InitHashDictionary();
			}
			markup = LanguageModLoader.BuildMarkupMap(ll);
			LanguageModLoader.shippedMarkupCache[ll.id ?? langId] = markup;
			return true;
		}

		// Token: 0x0600504A RID: 20554 RVA: 0x0017BB1C File Offset: 0x00179D1C
		private static Dictionary<string, string> BuildMarkupMap(LL language)
		{
			int textEntryCount = language.TextEntryCount;
			Dictionary<string, string> dictionary = new Dictionary<string, string>(textEntryCount);
			for (int i = 0; i < textEntryCount; i++)
			{
				string textIdAt = language.GetTextIdAt(i);
				if (!string.IsNullOrEmpty(textIdAt) && !dictionary.ContainsKey(textIdAt))
				{
					dictionary.Add(textIdAt, language.GetTextWithMarkup(i));
				}
			}
			return dictionary;
		}

		// Token: 0x0600504B RID: 20555 RVA: 0x0017BB6C File Offset: 0x00179D6C
		private static string NormalizeFallbackId(string fallback, string packId)
		{
			if (string.IsNullOrWhiteSpace(fallback))
			{
				return "en";
			}
			string text = fallback.Trim();
			if (string.Equals(text, packId, StringComparison.OrdinalIgnoreCase))
			{
				Debug.LogWarning("[LanguageMod] '" + packId + "' fallback points at itself. Using English.");
				return "en";
			}
			return text;
		}

		// Token: 0x0600504C RID: 20556 RVA: 0x0017BBB4 File Offset: 0x00179DB4
		private static Type[] GetPreprocessorTypes(string lang)
		{
			LanguageModLoader.PackedLanguage packedLanguage;
			if (string.IsNullOrEmpty(lang) || !LanguageModLoader.packs.TryGetValue(lang, out packedLanguage) || packedLanguage.preprocessorTypes == null)
			{
				return LanguageModLoader.NoPreprocessorTypes;
			}
			return packedLanguage.preprocessorTypes;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x0017BBEC File Offset: 0x00179DEC
		private static void SyncPreprocessors(TMP_Text label, Type[] desired, LanguageRtlLabelState state)
		{
			LanguageModLoader.DestroyRuntimePreprocessors(label, desired);
			foreach (Type type in desired)
			{
				if (!(type == null))
				{
					Component component = label.GetComponent(type);
					if (component != null)
					{
						Behaviour behaviour = component as Behaviour;
						if (behaviour != null)
						{
							behaviour.enabled = true;
						}
					}
					else
					{
						label.gameObject.AddComponent(type).hideFlags = HideFlags.HideAndDontSave;
					}
				}
			}
			if (desired.Length == 0)
			{
				label.textPreprocessor = state.originalPreprocessor;
			}
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x0017BC64 File Offset: 0x00179E64
		private static void DestroyRuntimePreprocessors(TMP_Text label, Type[] keep)
		{
			foreach (MonoBehaviour monoBehaviour in label.GetComponents<MonoBehaviour>())
			{
				if (!(monoBehaviour == null) && monoBehaviour is ITextPreprocessor && (monoBehaviour.hideFlags & HideFlags.HideAndDontSave) != HideFlags.None && !LanguageModLoader.ContainsType(keep, monoBehaviour.GetType()))
				{
					global::UnityEngine.Object.Destroy(monoBehaviour);
				}
			}
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x0017BCBC File Offset: 0x00179EBC
		private static bool ContainsType(Type[] types, Type type)
		{
			if (types == null)
			{
				return false;
			}
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i] == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x0017BCEC File Offset: 0x00179EEC
		private static bool ListContainsType(List<Type> types, Type type)
		{
			for (int i = 0; i < types.Count; i++)
			{
				if (types[i] == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005051 RID: 20561 RVA: 0x0017BD1C File Offset: 0x00179F1C
		private static Type[] ResolvePreprocessorTypes(string[] names, string packId)
		{
			if (names == null || names.Length == 0)
			{
				return LanguageModLoader.NoPreprocessorTypes;
			}
			List<Type> list = new List<Type>(names.Length);
			foreach (string text in names)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					text = text.Trim();
					Type type = LanguageModLoader.ResolvePreprocessorType(text);
					if (type == null)
					{
						Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] '", packId, "' preprocessor '", text, "' was not found." }));
					}
					else if (!typeof(MonoBehaviour).IsAssignableFrom(type) || type.IsAbstract)
					{
						Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] '", packId, "' preprocessor '", text, "' must be a MonoBehaviour." }));
					}
					else if (!typeof(ITextPreprocessor).IsAssignableFrom(type))
					{
						Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] '", packId, "' preprocessor '", text, "' must implement ITextPreprocessor." }));
					}
					else if (!LanguageModLoader.ListContainsType(list, type))
					{
						list.Add(type);
					}
				}
			}
			if (list.Count != 0)
			{
				return list.ToArray();
			}
			return LanguageModLoader.NoPreprocessorTypes;
		}

		// Token: 0x06005052 RID: 20562 RVA: 0x0017BE64 File Offset: 0x0017A064
		private static Type ResolvePreprocessorType(string name)
		{
			Type type;
			if (LanguageModLoader.preprocessorTypeCache.TryGetValue(name, out type))
			{
				return type;
			}
			Type type2 = LanguageModLoader.FindPreprocessorType(name);
			LanguageModLoader.preprocessorTypeCache[name] = type2;
			return type2;
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x0017BE98 File Offset: 0x0017A098
		private static Type FindPreprocessorType(string name)
		{
			Type type = Type.GetType(name);
			if (type != null)
			{
				return type;
			}
			Assembly assembly = typeof(LanguageModLoader).Assembly;
			type = assembly.GetType(name) ?? assembly.GetType("LazyBearTechnology." + name);
			if (type != null)
			{
				return type;
			}
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				try
				{
					type = assembly2.GetType(name);
					if (type != null)
					{
						return type;
					}
					if (name.IndexOf('.') < 0)
					{
						type = assembly2.GetType("LazyBearTechnology." + name);
						if (type != null)
						{
							return type;
						}
					}
				}
				catch (ReflectionTypeLoadException)
				{
				}
			}
			return null;
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0017BF6C File Offset: 0x0017A16C
		private static LL LoadEnglish()
		{
			LL ll = Resources.Load<LL>("Locales/lng_en");
			if (ll == null)
			{
				return null;
			}
			if (ll.dictionary == null || ll.dictionary.Count == 0)
			{
				ll.InitHashDictionary();
			}
			return ll;
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x0017BFAC File Offset: 0x0017A1AC
		private static string NormalizeFont(string font)
		{
			if (string.IsNullOrWhiteSpace(font))
			{
				return "default";
			}
			string text = font.Trim();
			if (LanguageModLoader.IsExternalFontFile(text))
			{
				return Path.GetFileName(text);
			}
			string text2 = text.ToLowerInvariant();
			if (text2 == "japanese" || text2 == "chinese" || text2 == "korean" || text2 == "handjet")
			{
				return text2;
			}
			return "default";
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0017C01F File Offset: 0x0017A21F
		private static bool IsExternalFontFile(string font)
		{
			return !string.IsNullOrEmpty(font) && font.IndexOf(".ttf", StringComparison.OrdinalIgnoreCase) >= 0;
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x0017C040 File Offset: 0x0017A240
		private static bool TryGetOrCreateExternalFont(LanguageModLoader.PackedLanguage pack, out TMP_FontAsset fontAsset)
		{
			fontAsset = pack.customFontAsset;
			if (fontAsset != null)
			{
				return true;
			}
			string fileName = Path.GetFileName(pack.font);
			string text = Path.Combine(pack.fontDirectory, fileName);
			if (!File.Exists(text))
			{
				Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] '", pack.id, "' font file '", fileName, "' was not found in the pack folder." }));
				return false;
			}
			LanguageModFontSettings fontSettings = pack.fontSettings;
			int num = ((fontSettings != null && fontSettings.pointSize > 0) ? fontSettings.pointSize : 16);
			int num2 = ((fontSettings != null && fontSettings.atlasWidth > 0) ? fontSettings.atlasWidth : 512);
			int num3 = ((fontSettings != null && fontSettings.atlasHeight > 0) ? fontSettings.atlasHeight : 512);
			int num4 = ((fontSettings != null) ? Math.Max(0, fontSettings.faceIndex) : 0);
			fontAsset = TMP_FontAsset.CreateFontAsset(text, num4, num, 3, GlyphRenderMode.RASTER_HINTED, num2, num3);
			if (fontAsset == null)
			{
				Debug.LogWarning(string.Concat(new string[] { "[LanguageMod] '", pack.id, "' failed to load font '", fileName, "'." }));
				return false;
			}
			fontAsset.hideFlags = HideFlags.HideAndDontSave;
			fontAsset.name = Path.GetFileNameWithoutExtension(fileName);
			if (fontAsset.material != null)
			{
				fontAsset.material.hideFlags = HideFlags.HideAndDontSave;
				fontAsset.material.name = fontAsset.name + " Material";
			}
			LanguageModLoader.ApplyFaceInfoOverrides(fontAsset, fontSettings);
			fontAsset.ReadFontAssetDefinition();
			LanguageModLoader.WarmupFontCharacters(fontAsset, pack);
			LanguageModLoader.ApplyAtlasFilter(fontAsset);
			MaterialReferenceManager.AddFontAsset(fontAsset);
			pack.customFontAsset = fontAsset;
			return true;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0017C1F8 File Offset: 0x0017A3F8
		private static void ApplyFaceInfoOverrides(TMP_FontAsset fontAsset, LanguageModFontSettings settings)
		{
			if (fontAsset == null)
			{
				return;
			}
			FaceInfo faceInfo = fontAsset.faceInfo;
			faceInfo.scale = 1f;
			if (settings != null && settings.overrideFaceInfo)
			{
				faceInfo.lineHeight = settings.lineHeight;
				faceInfo.ascentLine = settings.ascentLine;
				faceInfo.capLine = settings.capLine;
				faceInfo.meanLine = settings.meanLine;
				faceInfo.baseline = settings.baseline;
				faceInfo.descentLine = settings.descentLine;
				if (settings.pointSize > 0)
				{
					faceInfo.pointSize = (float)settings.pointSize;
				}
			}
			fontAsset.faceInfo = faceInfo;
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x0017C29C File Offset: 0x0017A49C
		private static void ApplyAtlasFilter(TMP_FontAsset fontAsset)
		{
			Texture2D[] atlasTextures = fontAsset.atlasTextures;
			if (atlasTextures == null)
			{
				return;
			}
			for (int i = 0; i < atlasTextures.Length; i++)
			{
				if (!(atlasTextures[i] == null))
				{
					atlasTextures[i].filterMode = FilterMode.Point;
					atlasTextures[i].wrapMode = TextureWrapMode.Clamp;
					atlasTextures[i].hideFlags = HideFlags.HideAndDontSave;
				}
			}
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0017C2EC File Offset: 0x0017A4EC
		private static void WarmupFontCharacters(TMP_FontAsset fontAsset, LanguageModLoader.PackedLanguage pack)
		{
			if (pack.language == null || pack.language.dictionary == null)
			{
				return;
			}
			HashSet<char> hashSet = new HashSet<char>();
			foreach (KeyValuePair<string, string> keyValuePair in pack.language.dictionary)
			{
				string value = keyValuePair.Value;
				if (!string.IsNullOrEmpty(value))
				{
					for (int i = 0; i < value.Length; i++)
					{
						hashSet.Add(value[i]);
					}
				}
			}
			for (char c = ' '; c <= '~'; c += '\u0001')
			{
				hashSet.Add(c);
			}
			if (hashSet.Count == 0)
			{
				return;
			}
			char[] array = new char[hashSet.Count];
			hashSet.CopyTo(array);
			fontAsset.TryAddCharacters(new string(array), true);
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x0017C3E0 File Offset: 0x0017A5E0
		private static void DestroyRuntimeFontAsset(TMP_FontAsset fontAsset)
		{
			if (fontAsset == null)
			{
				return;
			}
			LanguageModLoader.UnregisterTmpFont(fontAsset);
			global::UnityEngine.Object.DestroyImmediate(fontAsset, true);
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0017C3FC File Offset: 0x0017A5FC
		private static void UnregisterTmpFont(TMP_FontAsset fontAsset)
		{
			Dictionary<int, TMP_FontAsset> tmpFontLookup = LanguageModLoader.GetTmpFontLookup();
			if (tmpFontLookup != null)
			{
				tmpFontLookup.Remove(fontAsset.hashCode);
			}
			Dictionary<int, Material> tmpMaterialLookup = LanguageModLoader.GetTmpMaterialLookup();
			if (tmpMaterialLookup == null)
			{
				return;
			}
			tmpMaterialLookup.Remove(fontAsset.hashCode);
			tmpMaterialLookup.Remove(fontAsset.materialHashCode);
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x0017C444 File Offset: 0x0017A644
		private static void UnregisterDynamicStyleMaterials()
		{
			Dictionary<int, Material> tmpMaterialLookup = LanguageModLoader.GetTmpMaterialLookup();
			if (tmpMaterialLookup == null)
			{
				return;
			}
			TextStyle[] array = Resources.FindObjectsOfTypeAll<TextStyle>();
			foreach (string text in LanguageModLoader.packs.Keys)
			{
				for (int i = 0; i < array.Length; i++)
				{
					tmpMaterialLookup.Remove(LanguageModLoader.HashTmpMaterialName(text + ":" + array[i].name + "_dynamic"));
				}
			}
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0017C4DC File Offset: 0x0017A6DC
		private static List<TextStyle> CollectRegisteredStringMaterialStyles()
		{
			List<TextStyle> list = new List<TextStyle>();
			Dictionary<int, Material> tmpMaterialLookup = LanguageModLoader.GetTmpMaterialLookup();
			if (tmpMaterialLookup == null)
			{
				return list;
			}
			string currentLang = LLBase.CurrentLang;
			if (string.IsNullOrEmpty(currentLang))
			{
				return list;
			}
			TextStyle[] array = Resources.FindObjectsOfTypeAll<TextStyle>();
			for (int i = 0; i < array.Length; i++)
			{
				if (tmpMaterialLookup.ContainsKey(LanguageModLoader.HashTmpMaterialName(currentLang + ":" + array[i].name + "_dynamic")))
				{
					list.Add(array[i]);
				}
			}
			return list;
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0017C554 File Offset: 0x0017A754
		private static void ReregisterStringMaterials(List<TextStyle> styles)
		{
			for (int i = 0; i < styles.Count; i++)
			{
				TextStyle textStyle = styles[i];
				if (!(textStyle == null) && !(textStyle.Font == null) && textStyle.data != null)
				{
					try
					{
						textStyle.ApplyStyleToString(string.Empty, false, true);
					}
					catch (Exception ex)
					{
						Debug.LogWarning("[LanguageMod] Failed to re-register string material for style '" + textStyle.name + "': " + ex.Message);
					}
				}
			}
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x0017C5E0 File Offset: 0x0017A7E0
		private static void PurgeDestroyedTmpLookups()
		{
			Dictionary<int, TMP_FontAsset> tmpFontLookup = LanguageModLoader.GetTmpFontLookup();
			if (tmpFontLookup != null)
			{
				List<int> list = new List<int>();
				foreach (KeyValuePair<int, TMP_FontAsset> keyValuePair in tmpFontLookup)
				{
					if (keyValuePair.Value == null)
					{
						list.Add(keyValuePair.Key);
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					tmpFontLookup.Remove(list[i]);
				}
			}
			Dictionary<int, Material> tmpMaterialLookup = LanguageModLoader.GetTmpMaterialLookup();
			if (tmpMaterialLookup == null)
			{
				return;
			}
			List<int> list2 = new List<int>();
			foreach (KeyValuePair<int, Material> keyValuePair2 in tmpMaterialLookup)
			{
				if (keyValuePair2.Value == null)
				{
					list2.Add(keyValuePair2.Key);
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				tmpMaterialLookup.Remove(list2[j]);
			}
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x0017C700 File Offset: 0x0017A900
		private static Dictionary<int, TMP_FontAsset> GetTmpFontLookup()
		{
			FieldInfo field = typeof(MaterialReferenceManager).GetField("m_FontAssetReferenceLookup", BindingFlags.Instance | BindingFlags.NonPublic);
			return ((field != null) ? field.GetValue(MaterialReferenceManager.instance) : null) as Dictionary<int, TMP_FontAsset>;
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x0017C72E File Offset: 0x0017A92E
		private static Dictionary<int, Material> GetTmpMaterialLookup()
		{
			FieldInfo field = typeof(MaterialReferenceManager).GetField("m_FontMaterialReferenceLookup", BindingFlags.Instance | BindingFlags.NonPublic);
			return ((field != null) ? field.GetValue(MaterialReferenceManager.instance) : null) as Dictionary<int, Material>;
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x0017C75C File Offset: 0x0017A95C
		private static int HashTmpMaterialName(string matName)
		{
			int num = 0;
			for (int i = 0; i < matName.Length; i++)
			{
				num = ((num << 5) + num) ^ (int)TMP_TextParsingUtilities.ToUpperASCIIFast(matName[i]);
			}
			return num;
		}

		// Token: 0x06005064 RID: 20580 RVA: 0x0017C790 File Offset: 0x0017A990
		private static TextAlignmentOptions FlipHorizontalAlignment(TextAlignmentOptions alignment)
		{
			if (alignment <= TextAlignmentOptions.BottomRight)
			{
				if (alignment <= TextAlignmentOptions.Left)
				{
					if (alignment == TextAlignmentOptions.TopLeft)
					{
						return TextAlignmentOptions.TopRight;
					}
					if (alignment == TextAlignmentOptions.TopRight)
					{
						return TextAlignmentOptions.TopLeft;
					}
					if (alignment == TextAlignmentOptions.Left)
					{
						return TextAlignmentOptions.Right;
					}
				}
				else
				{
					if (alignment == TextAlignmentOptions.Right)
					{
						return TextAlignmentOptions.Left;
					}
					if (alignment == TextAlignmentOptions.BottomLeft)
					{
						return TextAlignmentOptions.BottomRight;
					}
					if (alignment == TextAlignmentOptions.BottomRight)
					{
						return TextAlignmentOptions.BottomLeft;
					}
				}
			}
			else if (alignment <= TextAlignmentOptions.MidlineLeft)
			{
				if (alignment == TextAlignmentOptions.BaselineLeft)
				{
					return TextAlignmentOptions.BaselineRight;
				}
				if (alignment == TextAlignmentOptions.BaselineRight)
				{
					return TextAlignmentOptions.BaselineLeft;
				}
				if (alignment == TextAlignmentOptions.MidlineLeft)
				{
					return TextAlignmentOptions.MidlineRight;
				}
			}
			else
			{
				if (alignment == TextAlignmentOptions.MidlineRight)
				{
					return TextAlignmentOptions.MidlineLeft;
				}
				if (alignment == TextAlignmentOptions.CaplineLeft)
				{
					return TextAlignmentOptions.CaplineRight;
				}
				if (alignment == TextAlignmentOptions.CaplineRight)
				{
					return TextAlignmentOptions.CaplineLeft;
				}
			}
			return alignment;
		}

		// Token: 0x040041C5 RID: 16837
		private const string FontDefault = "default";

		// Token: 0x040041C6 RID: 16838
		private const string FontJapanese = "japanese";

		// Token: 0x040041C7 RID: 16839
		private const string FontChinese = "chinese";

		// Token: 0x040041C8 RID: 16840
		private const string FontKorean = "korean";

		// Token: 0x040041C9 RID: 16841
		private const string FontHandjet = "handjet";

		// Token: 0x040041CA RID: 16842
		private const string TtfExtension = ".ttf";

		// Token: 0x040041CB RID: 16843
		private const string UiLangNameLocKey = "ui_lang_name_loc";

		// Token: 0x040041CC RID: 16844
		private const int DefaultSamplingPointSize = 16;

		// Token: 0x040041CD RID: 16845
		private const int DefaultAtlasPadding = 3;

		// Token: 0x040041CE RID: 16846
		private const int DefaultAtlasSize = 512;

		// Token: 0x040041CF RID: 16847
		private const float DefaultFaceScale = 1f;

		// Token: 0x040041D0 RID: 16848
		private const GlyphRenderMode DefaultRenderMode = GlyphRenderMode.RASTER_HINTED;

		// Token: 0x040041D1 RID: 16849
		private static readonly Dictionary<string, string> FontResourcePaths = new Dictionary<string, string>
		{
			{ "japanese", "Fonts & Materials/japanese" },
			{ "chinese", "Fonts & Materials/chinese" },
			{ "korean", "Fonts & Materials/korean" },
			{ "handjet", "Fonts & Materials/handjet" }
		};

		// Token: 0x040041D2 RID: 16850
		private static readonly HashSet<string> FontsUsingOwnMaterial = new HashSet<string>();

		// Token: 0x040041D3 RID: 16851
		private static readonly Type[] NoPreprocessorTypes = Array.Empty<Type>();

		// Token: 0x040041D4 RID: 16852
		private static readonly Dictionary<string, LanguageModLoader.PackedLanguage> packs = new Dictionary<string, LanguageModLoader.PackedLanguage>();

		// Token: 0x040041D5 RID: 16853
		private static readonly Dictionary<string, TMP_FontAsset> fontCache = new Dictionary<string, TMP_FontAsset>();

		// Token: 0x040041D6 RID: 16854
		private static readonly Dictionary<string, Dictionary<string, string>> shippedMarkupCache = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x040041D7 RID: 16855
		private static readonly Dictionary<string, Dictionary<string, string>> resolvedMarkupCache = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x040041D8 RID: 16856
		private static readonly Dictionary<string, Type> preprocessorTypeCache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x02000C4E RID: 3150
		private class PackedLanguage
		{
			// Token: 0x040041D9 RID: 16857
			public string id;

			// Token: 0x040041DA RID: 16858
			public string displayName;

			// Token: 0x040041DB RID: 16859
			public string font;

			// Token: 0x040041DC RID: 16860
			public string fontDirectory;

			// Token: 0x040041DD RID: 16861
			public LanguageModFontSettings fontSettings;

			// Token: 0x040041DE RID: 16862
			public bool useOwnMaterial;

			// Token: 0x040041DF RID: 16863
			public bool rtl;

			// Token: 0x040041E0 RID: 16864
			public bool requireResize;

			// Token: 0x040041E1 RID: 16865
			public Type[] preprocessorTypes;

			// Token: 0x040041E2 RID: 16866
			public LL language;

			// Token: 0x040041E3 RID: 16867
			public TMP_FontAsset customFontAsset;
		}

		// Token: 0x02000C4F RID: 3151
		private class PackSource
		{
			// Token: 0x040041E4 RID: 16868
			public string id;

			// Token: 0x040041E5 RID: 16869
			public string directory;

			// Token: 0x040041E6 RID: 16870
			public LanguageModManifest manifest;

			// Token: 0x040041E7 RID: 16871
			public Dictionary<string, string> overlay;
		}
	}
}
