using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200048E RID: 1166
[Serializable]
public class RemovedDemoSavesList
{
	// Token: 0x1700053C RID: 1340
	// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000921B8 File Offset: 0x000903B8
	public static RemovedDemoSavesList Instance
	{
		get
		{
			if (RemovedDemoSavesList.instance == null)
			{
				RemovedDemoSavesList.instance = RemovedDemoSavesList.Load();
			}
			return RemovedDemoSavesList.instance;
		}
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x000921D0 File Offset: 0x000903D0
	public static void TryAddSave(string save)
	{
		if (!RemovedDemoSavesList.IsDeleted(save))
		{
			RemovedDemoSavesList.Instance.removedSaves.Add(save);
			RemovedDemoSavesList.Save();
		}
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x000921EF File Offset: 0x000903EF
	public static void TryRemoveSave(string save)
	{
		if (RemovedDemoSavesList.IsDeleted(save))
		{
			RemovedDemoSavesList.Instance.removedSaves.Remove(save);
			RemovedDemoSavesList.Save();
		}
	}

	// Token: 0x06001EE7 RID: 7911 RVA: 0x0009220F File Offset: 0x0009040F
	public static bool IsDeleted(string save)
	{
		return RemovedDemoSavesList.Instance.removedSaves.Contains(save);
	}

	// Token: 0x06001EE8 RID: 7912 RVA: 0x00092224 File Offset: 0x00090424
	private static RemovedDemoSavesList Load()
	{
		string text = string.Empty;
		if (LazyAPI.PlayerPrefs.HasKey("removedDemoSaves"))
		{
			text = LazyAPI.PlayerPrefs.GetString("removedDemoSaves", "");
		}
		if (!string.IsNullOrEmpty(text))
		{
			return JsonUtility.FromJson<RemovedDemoSavesList>(text);
		}
		return new RemovedDemoSavesList();
	}

	// Token: 0x06001EE9 RID: 7913 RVA: 0x00092274 File Offset: 0x00090474
	public static void Save()
	{
		string text = JsonUtility.ToJson(RemovedDemoSavesList.Instance);
		LazyAPI.PlayerPrefs.SetString("removedDemoSaves", text);
		LazyAPI.PlayerPrefs.Save();
		try
		{
			LazyAPI.PlayerPrefs.Save();
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during save RemovedDemoSavesList :[{0}]", ex));
		}
	}

	// Token: 0x04001BD9 RID: 7129
	private const string PLAYER_PREFS_KEY = "removedDemoSaves";

	// Token: 0x04001BDA RID: 7130
	private static RemovedDemoSavesList instance;

	// Token: 0x04001BDB RID: 7131
	public List<string> removedSaves = new List<string>();
}
