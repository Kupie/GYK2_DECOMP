using System;
using System.IO;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020007A9 RID: 1961
public static class SteamWorkshopCreatorConfig
{
	// Token: 0x170007AA RID: 1962
	// (get) Token: 0x06003249 RID: 12873 RVA: 0x000F0E5A File Offset: 0x000EF05A
	// (set) Token: 0x0600324A RID: 12874 RVA: 0x000F0E61 File Offset: 0x000EF061
	public static bool Enabled { get; private set; }

	// Token: 0x0600324B RID: 12875 RVA: 0x000F0E6C File Offset: 0x000EF06C
	public static void EnsureAndReload()
	{
		SteamWorkshopCreatorConfig.Enabled = false;
		string workshopConfigPath = ModsPaths.WorkshopConfigPath;
		try
		{
			string directoryName = Path.GetDirectoryName(workshopConfigPath);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (!File.Exists(workshopConfigPath))
			{
				File.WriteAllText(workshopConfigPath, "{\n  // Enables the Steam Workshop Creator window (Shift+F11).\n  // Keep this false unless you are publishing translations to the Workshop.\n  \"workshopCreatorMode\": false\n}\n");
			}
			string text = ModsJsonComments.Strip(File.ReadAllText(workshopConfigPath));
			if (!string.IsNullOrWhiteSpace(text))
			{
				SteamWorkshopCreatorConfig.FileData fileData = JsonUtility.FromJson<SteamWorkshopCreatorConfig.FileData>(text);
				SteamWorkshopCreatorConfig.Enabled = fileData != null && fileData.workshopCreatorMode;
				Debug.Log(string.Format("[SteamWorkshopCreator] workshopCreatorMode={0} ({1})", SteamWorkshopCreatorConfig.Enabled, workshopConfigPath));
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[SteamWorkshopCreator] Failed to read '" + workshopConfigPath + "': " + ex.Message);
			SteamWorkshopCreatorConfig.Enabled = false;
		}
	}

	// Token: 0x0600324C RID: 12876 RVA: 0x000F0F30 File Offset: 0x000EF130
	public static void Tick()
	{
		if (!SteamWorkshopCreatorConfig.Enabled)
		{
			return;
		}
		if (!Input.GetKeyDown(KeyCode.F11))
		{
			return;
		}
		if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
		{
			return;
		}
		UISteamWorkshopCreatorWindow.Toggle();
	}

	// Token: 0x04002861 RID: 10337
	private const string DefaultFileContents = "{\n  // Enables the Steam Workshop Creator window (Shift+F11).\n  // Keep this false unless you are publishing translations to the Workshop.\n  \"workshopCreatorMode\": false\n}\n";

	// Token: 0x020007AA RID: 1962
	[Serializable]
	private class FileData
	{
		// Token: 0x04002863 RID: 10339
		public bool workshopCreatorMode;
	}
}
