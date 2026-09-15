using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public static class SteamWorkshopCreatorStore
{
	// Token: 0x170007AD RID: 1965
	// (get) Token: 0x06003278 RID: 12920 RVA: 0x000F26BE File Offset: 0x000F08BE
	public static string FilePath
	{
		get
		{
			return Path.Combine(Application.persistentDataPath, "SteamWorkshop", "workshop_items.json");
		}
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x000F26D4 File Offset: 0x000F08D4
	public static List<SteamWorkshopItemRecord> Load()
	{
		List<SteamWorkshopItemRecord> list;
		try
		{
			if (!File.Exists(SteamWorkshopCreatorStore.FilePath))
			{
				list = new List<SteamWorkshopItemRecord>();
			}
			else
			{
				string text = File.ReadAllText(SteamWorkshopCreatorStore.FilePath);
				if (string.IsNullOrWhiteSpace(text))
				{
					list = new List<SteamWorkshopItemRecord>();
				}
				else
				{
					SteamWorkshopCreatorStore.FileData fileData = JsonUtility.FromJson<SteamWorkshopCreatorStore.FileData>(text);
					if (((fileData != null) ? fileData.items : null) == null)
					{
						list = new List<SteamWorkshopItemRecord>();
					}
					else
					{
						list = fileData.items;
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[SteamWorkshopCreator] Failed to load items: " + ex.Message);
			list = new List<SteamWorkshopItemRecord>();
		}
		return list;
	}

	// Token: 0x0600327A RID: 12922 RVA: 0x000F2768 File Offset: 0x000F0968
	public static void Save(List<SteamWorkshopItemRecord> items)
	{
		try
		{
			string directoryName = Path.GetDirectoryName(SteamWorkshopCreatorStore.FilePath);
			if (!string.IsNullOrEmpty(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			SteamWorkshopCreatorStore.FileData fileData = new SteamWorkshopCreatorStore.FileData
			{
				items = (items ?? new List<SteamWorkshopItemRecord>())
			};
			File.WriteAllText(SteamWorkshopCreatorStore.FilePath, JsonUtility.ToJson(fileData, true));
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[SteamWorkshopCreator] Failed to save items: " + ex.Message);
		}
	}

	// Token: 0x04002879 RID: 10361
	private const string FolderName = "SteamWorkshop";

	// Token: 0x0400287A RID: 10362
	private const string FileName = "workshop_items.json";

	// Token: 0x020007AE RID: 1966
	[Serializable]
	private class FileData
	{
		// Token: 0x0400287B RID: 10363
		public List<SteamWorkshopItemRecord> items = new List<SteamWorkshopItemRecord>();
	}
}
