using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000C55 RID: 3157
	public static class ModsPaths
	{
		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06005073 RID: 20595 RVA: 0x0017CEB4 File Offset: 0x0017B0B4
		public static string Root
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "Mods");
			}
		}

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x0017CEC5 File Offset: 0x0017B0C5
		public static string LanguagesRoot
		{
			get
			{
				return Path.Combine(ModsPaths.Root, "Languages");
			}
		}

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x06005075 RID: 20597 RVA: 0x0017CED6 File Offset: 0x0017B0D6
		public static string VoiceOversRoot
		{
			get
			{
				return Path.Combine(ModsPaths.Root, "VoiceOvers");
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x0017CEE7 File Offset: 0x0017B0E7
		public static string ExampleLanguageFolder
		{
			get
			{
				return Path.Combine(ModsPaths.LanguagesRoot, "~Example");
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06005077 RID: 20599 RVA: 0x0017CEF8 File Offset: 0x0017B0F8
		public static string WorkshopConfigPath
		{
			get
			{
				return Path.Combine(ModsPaths.Root, "workshop.json");
			}
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x0017CF09 File Offset: 0x0017B109
		public static string GetGameExecutableDirectory()
		{
			return Path.GetDirectoryName(Application.dataPath);
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x0017CF18 File Offset: 0x0017B118
		public static IReadOnlyList<string> GetModSearchRoots()
		{
			List<string> list = new List<string>();
			ModsPaths.AddRoot(list, ModsPaths.Root);
			ModsPaths.AddRoot(list, ModsPaths.GetGameExecutableDirectory());
			IReadOnlyList<string> installedFolders = SteamWorkshopInstalledItems.GetInstalledFolders();
			for (int i = 0; i < installedFolders.Count; i++)
			{
				ModsPaths.AddRoot(list, installedFolders[i]);
			}
			return list;
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x0017CF66 File Offset: 0x0017B166
		public static bool IsDisabledFolderName(string folderName)
		{
			return !string.IsNullOrEmpty(folderName) && folderName[0] == '~';
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x0017CF7D File Offset: 0x0017B17D
		public static bool IsEditableModPath(string path)
		{
			return !string.IsNullOrEmpty(path) && (ModsPaths.IsUnder(path, ModsPaths.Root) || ModsPaths.IsUnder(path, ModsPaths.GetGameExecutableDirectory()));
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x0017CFA4 File Offset: 0x0017B1A4
		private static void AddRoot(List<string> roots, string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				path = Path.GetFullPath(path);
			}
			catch
			{
				return;
			}
			if (!Directory.Exists(path))
			{
				return;
			}
			for (int i = 0; i < roots.Count; i++)
			{
				if (string.Equals(roots[i], path, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			roots.Add(path);
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x0017D00C File Offset: 0x0017B20C
		private static bool IsUnder(string path, string root)
		{
			if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(root))
			{
				return false;
			}
			bool flag;
			try
			{
				string text = Path.GetFullPath(path).TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
				string text2 = Path.GetFullPath(root).TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar,
					Path.AltDirectorySeparatorChar
				});
				if (string.Equals(text, text2, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
				else
				{
					string text3 = text2 + Path.DirectorySeparatorChar.ToString();
					flag = text.StartsWith(text3, StringComparison.OrdinalIgnoreCase);
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x040041F1 RID: 16881
		public const string ModsFolderName = "Mods";

		// Token: 0x040041F2 RID: 16882
		public const string LanguagesFolderName = "Languages";

		// Token: 0x040041F3 RID: 16883
		public const string VoiceOversFolderName = "VoiceOvers";

		// Token: 0x040041F4 RID: 16884
		public const string ExampleFolderName = "~Example";

		// Token: 0x040041F5 RID: 16885
		public const string ReadmeFileName = "README.txt";

		// Token: 0x040041F6 RID: 16886
		public const string LanguageJsonFileName = "language.json";

		// Token: 0x040041F7 RID: 16887
		public const string StringsCsvFileName = "strings.csv";

		// Token: 0x040041F8 RID: 16888
		public const string MissingLinesCsvFileName = "_missing_lines.csv";

		// Token: 0x040041F9 RID: 16889
		public const string VoiceOverManifestFileName = "mod.json";

		// Token: 0x040041FA RID: 16890
		public const string WorkshopConfigFileName = "workshop.json";
	}
}
