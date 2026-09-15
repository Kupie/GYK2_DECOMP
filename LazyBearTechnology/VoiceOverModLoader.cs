using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace LazyBearTechnology
{
	// Token: 0x02000195 RID: 405
	public static class VoiceOverModLoader
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0002C095 File Offset: 0x0002A295
		public static string RootPath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "Mods", "VoiceOvers");
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0002C0AB File Offset: 0x0002A2AB
		public static bool IsActive
		{
			get
			{
				return VoiceOverModLoader.isActive;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0002C0B2 File Offset: 0x0002A2B2
		public static string ActiveLanguage
		{
			get
			{
				return VoiceOverModLoader.activeLanguage;
			}
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x0002C0B9 File Offset: 0x0002A2B9
		public static void Refresh()
		{
			VoiceOverModLoader.Refresh(LLBase.CurrentLang);
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x0002C0C8 File Offset: 0x0002A2C8
		public static void Refresh(string language)
		{
			VoiceOverModLoader.isActive = false;
			VoiceOverModLoader.activeLanguage = language;
			VoiceOverModLoader.activePackDirectory = null;
			VoiceOverSettings.LanguageId = "en";
			if (string.IsNullOrEmpty(language))
			{
				return;
			}
			string text;
			VoiceOverModLoader.VoiceOverModManifest voiceOverModManifest;
			if (!VoiceOverModLoader.TryFindPackDirectory(language, out text, out voiceOverModManifest))
			{
				return;
			}
			VoiceOverModLoader.isActive = true;
			VoiceOverModLoader.activePackDirectory = text;
			VoiceOverSettings.LanguageId = language;
			Debug.Log(string.Concat(new string[]
			{
				"[VoiceOverMod] Active pack for '",
				language,
				"' at '",
				text,
				"'",
				string.IsNullOrEmpty(voiceOverModManifest.name) ? "." : (": " + voiceOverModManifest.name)
			}));
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x0002C170 File Offset: 0x0002A370
		private static IReadOnlyList<string> GetSearchRoots()
		{
			if (VoiceOverModLoader.SearchRootsProvider != null)
			{
				IReadOnlyList<string> readOnlyList = VoiceOverModLoader.SearchRootsProvider();
				if (readOnlyList != null && readOnlyList.Count > 0)
				{
					return readOnlyList;
				}
			}
			return new string[] { Path.Combine(Application.persistentDataPath, "Mods") };
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x0002C1B8 File Offset: 0x0002A3B8
		private static bool TryFindPackDirectory(string language, out string packDir, out VoiceOverModLoader.VoiceOverModManifest manifest)
		{
			packDir = null;
			manifest = null;
			IReadOnlyList<string> searchRoots = VoiceOverModLoader.GetSearchRoots();
			for (int i = 0; i < searchRoots.Count; i++)
			{
				if (VoiceOverModLoader.TryValidatePack(searchRoots[i], language, out manifest))
				{
					packDir = searchRoots[i];
					return true;
				}
				string text = Path.Combine(searchRoots[i], "VoiceOvers", language);
				if (VoiceOverModLoader.TryValidatePack(text, language, out manifest))
				{
					packDir = text;
					return true;
				}
				string text2 = Path.Combine(searchRoots[i], "VoiceOvers");
				if (Directory.Exists(text2))
				{
					string[] directories = Directory.GetDirectories(text2);
					for (int j = 0; j < directories.Length; j++)
					{
						string fileName = Path.GetFileName(directories[j]);
						if ((string.IsNullOrEmpty(fileName) || fileName[0] != '~') && VoiceOverModLoader.TryValidatePack(directories[j], language, out manifest))
						{
							packDir = directories[j];
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x0002C294 File Offset: 0x0002A494
		private static bool TryValidatePack(string packDir, string language, out VoiceOverModLoader.VoiceOverModManifest manifest)
		{
			manifest = null;
			if (string.IsNullOrEmpty(packDir) || !Directory.Exists(packDir))
			{
				return false;
			}
			string text = Path.Combine(packDir, "mod.json");
			if (!File.Exists(text))
			{
				return false;
			}
			try
			{
				manifest = JsonUtility.FromJson<VoiceOverModLoader.VoiceOverModManifest>(File.ReadAllText(text));
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[VoiceOverMod] Failed to parse '" + text + "': " + ex.Message);
				return false;
			}
			if (manifest == null)
			{
				return false;
			}
			if (manifest.schemaVersion > 1)
			{
				Debug.LogWarning(string.Format("[VoiceOverMod] Unsupported schemaVersion {0} in '{1}'.", manifest.schemaVersion, text));
				return false;
			}
			return manifest.enabled && string.Equals(manifest.language, language, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x0002C35C File Offset: 0x0002A55C
		public static bool TryLoadClip(string id, out AudioClip clip)
		{
			clip = null;
			if (!VoiceOverModLoader.isActive || string.IsNullOrEmpty(VoiceOverModLoader.activePackDirectory) || string.IsNullOrEmpty(id))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(id);
			if (string.IsNullOrEmpty(fileNameWithoutExtension))
			{
				return false;
			}
			string text;
			AudioType audioType;
			if (!VoiceOverModLoader.TryResolveClipPath(fileNameWithoutExtension, out text, out audioType))
			{
				return false;
			}
			bool flag;
			try
			{
				using (UnityWebRequest audioClip = UnityWebRequestMultimedia.GetAudioClip(new Uri(text).AbsoluteUri, audioType))
				{
					((DownloadHandlerAudioClip)audioClip.downloadHandler).streamAudio = false;
					audioClip.SendWebRequest();
					while (!audioClip.isDone)
					{
					}
					if (audioClip.result != UnityWebRequest.Result.Success)
					{
						Debug.LogWarning("[VoiceOverMod] Failed to load '" + text + "': " + audioClip.error);
						flag = false;
					}
					else
					{
						clip = DownloadHandlerAudioClip.GetContent(audioClip);
						if (clip == null)
						{
							flag = false;
						}
						else
						{
							clip.name = fileNameWithoutExtension;
							flag = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[VoiceOverMod] Exception loading '" + fileNameWithoutExtension + "': " + ex.Message);
				clip = null;
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0002C478 File Offset: 0x0002A678
		public static void ReleaseClip(AudioClip clip)
		{
			if (clip == null)
			{
				return;
			}
			global::UnityEngine.Object.Destroy(clip);
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x0002C48C File Offset: 0x0002A68C
		private static bool TryResolveClipPath(string fileId, out string clipPath, out AudioType audioType)
		{
			for (int i = 0; i < VoiceOverModLoader.ClipExtensions.Length; i++)
			{
				string text = VoiceOverModLoader.ClipExtensions[i];
				string text2 = Path.Combine(VoiceOverModLoader.activePackDirectory, fileId + text);
				if (File.Exists(text2))
				{
					clipPath = text2;
					audioType = VoiceOverModLoader.GetAudioType(text);
					return true;
				}
			}
			clipPath = null;
			audioType = AudioType.UNKNOWN;
			return false;
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0002C4E4 File Offset: 0x0002A6E4
		private static AudioType GetAudioType(string extension)
		{
			string text = extension.ToLowerInvariant();
			if (text == ".wav")
			{
				return AudioType.WAV;
			}
			if (text == ".ogg")
			{
				return AudioType.OGGVORBIS;
			}
			if (!(text == ".mp3"))
			{
				return AudioType.UNKNOWN;
			}
			return AudioType.MPEG;
		}

		// Token: 0x04000573 RID: 1395
		private const string ModsFolder = "Mods";

		// Token: 0x04000574 RID: 1396
		private const string VoiceOversFolder = "VoiceOvers";

		// Token: 0x04000575 RID: 1397
		private const string ManifestFileName = "mod.json";

		// Token: 0x04000576 RID: 1398
		private const int SupportedSchemaVersion = 1;

		// Token: 0x04000577 RID: 1399
		private static readonly string[] ClipExtensions = new string[] { ".wav", ".ogg", ".mp3" };

		// Token: 0x04000578 RID: 1400
		private static string activeLanguage;

		// Token: 0x04000579 RID: 1401
		private static bool isActive;

		// Token: 0x0400057A RID: 1402
		private static string activePackDirectory;

		// Token: 0x0400057B RID: 1403
		public static Func<IReadOnlyList<string>> SearchRootsProvider;

		// Token: 0x0200020F RID: 527
		[Serializable]
		private class VoiceOverModManifest
		{
			// Token: 0x040006FB RID: 1787
			public int schemaVersion = 1;

			// Token: 0x040006FC RID: 1788
			public string language = "";

			// Token: 0x040006FD RID: 1789
			public string name = "";

			// Token: 0x040006FE RID: 1790
			public bool enabled = true;
		}
	}
}
