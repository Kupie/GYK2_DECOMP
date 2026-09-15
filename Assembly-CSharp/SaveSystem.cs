using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using Steamworks;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public static class SaveSystem
{
	// Token: 0x1700053F RID: 1343
	// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x00092460 File Offset: 0x00090660
	public static List<SaveSlotData> SaveSlotDataList
	{
		get
		{
			if (!SaveSystem.IsLimitedSaveSlotsEnabled)
			{
				return SaveSystem.ReadSaveSlotsData();
			}
			if (SaveSystem.saveSlotDataList == null)
			{
				SaveSystem.saveSlotDataList = SaveSystem.ReadSaveSlotsData();
				SaveSystem.saveDataList = new List<byte[]>();
				for (int i = 0; i < SaveSystem.saveSlotDataList.Count; i++)
				{
					SaveSystem.saveDataList.Add(null);
				}
			}
			List<SaveSlotData> list = new List<SaveSlotData>(new SaveSlotData[SaveSystem.saveDataList.Count]);
			for (int j = 0; j < list.Count; j++)
			{
				list[j] = SaveSystem.saveSlotDataList[j].Copy();
			}
			return list;
		}
	}

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x06001EF4 RID: 7924 RVA: 0x000924F4 File Offset: 0x000906F4
	// (remove) Token: 0x06001EF5 RID: 7925 RVA: 0x00092528 File Offset: 0x00090728
	public static event Action OnSaveSlotsReadStarted;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06001EF6 RID: 7926 RVA: 0x0009255C File Offset: 0x0009075C
	// (remove) Token: 0x06001EF7 RID: 7927 RVA: 0x00092590 File Offset: 0x00090790
	public static event Action OnSaveSlotsReadCompleted;

	// Token: 0x1400004E RID: 78
	// (add) Token: 0x06001EF8 RID: 7928 RVA: 0x000925C4 File Offset: 0x000907C4
	// (remove) Token: 0x06001EF9 RID: 7929 RVA: 0x000925F8 File Offset: 0x000907F8
	public static event Action OnSaveSlotsReadFailed;

	// Token: 0x1400004F RID: 79
	// (add) Token: 0x06001EFA RID: 7930 RVA: 0x0009262C File Offset: 0x0009082C
	// (remove) Token: 0x06001EFB RID: 7931 RVA: 0x00092660 File Offset: 0x00090860
	public static event Action OnSaveLoadingStarted;

	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06001EFC RID: 7932 RVA: 0x00092694 File Offset: 0x00090894
	// (remove) Token: 0x06001EFD RID: 7933 RVA: 0x000926C8 File Offset: 0x000908C8
	public static event Action OnSaveLoadingEnded;

	// Token: 0x14000051 RID: 81
	// (add) Token: 0x06001EFE RID: 7934 RVA: 0x000926FC File Offset: 0x000908FC
	// (remove) Token: 0x06001EFF RID: 7935 RVA: 0x00092730 File Offset: 0x00090930
	public static event Action OnSaveWriteStarted;

	// Token: 0x14000052 RID: 82
	// (add) Token: 0x06001F00 RID: 7936 RVA: 0x00092764 File Offset: 0x00090964
	// (remove) Token: 0x06001F01 RID: 7937 RVA: 0x00092798 File Offset: 0x00090998
	public static event Action OnSaveWriteStartedInstant;

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x06001F02 RID: 7938 RVA: 0x000927CC File Offset: 0x000909CC
	// (remove) Token: 0x06001F03 RID: 7939 RVA: 0x00092800 File Offset: 0x00090A00
	public static event Action OnSaveWriteEnded;

	// Token: 0x17000540 RID: 1344
	// (get) Token: 0x06001F04 RID: 7940 RVA: 0x00028294 File Offset: 0x00026494
	public static bool IsLimitedSaveSlotsEnabled
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000541 RID: 1345
	// (get) Token: 0x06001F05 RID: 7941 RVA: 0x00092833 File Offset: 0x00090A33
	public static string SaveFolder
	{
		get
		{
			return Application.persistentDataPath + "/";
		}
	}

	// Token: 0x17000542 RID: 1346
	// (get) Token: 0x06001F06 RID: 7942 RVA: 0x00092844 File Offset: 0x00090A44
	public static string DemoSaveFolder
	{
		get
		{
			string text = Application.persistentDataPath + "/";
			text = text.Replace('\\', '/').Replace("Graveyard Keeper 2", "Graveyard Keeper 2 Demo");
			string text2;
			if (SaveSystem.TryGetProtonDemoSaveFolder(out text2))
			{
				return text2;
			}
			return text;
		}
	}

	// Token: 0x06001F07 RID: 7943 RVA: 0x00092888 File Offset: 0x00090A88
	private static bool TryGetProtonDemoSaveFolder(out string demoSaveFolder)
	{
		if (!SaveSystem.protonDemoSaveFolderResolved)
		{
			if (!SaveSystem.ShouldUseProtonDemoSaveFolder())
			{
				if (SaveSystem.CanFinalizeProtonDemoSaveFolderDecision())
				{
					SaveSystem.protonDemoSaveFolderResolved = true;
					SaveSystem.cachedProtonDemoSaveFolder = null;
				}
				demoSaveFolder = null;
				return false;
			}
			SaveSystem.cachedProtonDemoSaveFolder = SaveSystem.ResolveProtonDemoSaveFolder();
			SaveSystem.protonDemoSaveFolderResolved = true;
			if (!string.IsNullOrEmpty(SaveSystem.cachedProtonDemoSaveFolder))
			{
				Debug.Log("[SaveSystem] Proton demo save folder: " + SaveSystem.cachedProtonDemoSaveFolder);
			}
			else
			{
				Debug.Log("[SaveSystem] Proton demo prefix not found for app 5075680");
			}
		}
		demoSaveFolder = SaveSystem.cachedProtonDemoSaveFolder;
		return !string.IsNullOrEmpty(demoSaveFolder);
	}

	// Token: 0x06001F08 RID: 7944 RVA: 0x00092908 File Offset: 0x00090B08
	private static bool ShouldUseProtonDemoSaveFolder()
	{
		return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("STEAM_COMPAT_DATA_PATH")) && SteamManager.Initialized && SteamUtils.IsSteamRunningOnSteamDeck();
	}

	// Token: 0x06001F09 RID: 7945 RVA: 0x0009292B File Offset: 0x00090B2B
	private static bool CanFinalizeProtonDemoSaveFolderDecision()
	{
		return string.IsNullOrEmpty(Environment.GetEnvironmentVariable("STEAM_COMPAT_DATA_PATH")) || SteamManager.Initialized;
	}

	// Token: 0x06001F0A RID: 7946 RVA: 0x00092948 File Offset: 0x00090B48
	private static string ResolveProtonDemoSaveFolder()
	{
		string text;
		if (!SaveSystem.TryGetDemoProtonPrefix(out text))
		{
			return null;
		}
		string text2 = SaveSystem.GetPersistentDataPathRelativeToDriveC().Replace("Graveyard Keeper 2", "Graveyard Keeper 2 Demo");
		if (!text2.StartsWith("/"))
		{
			text2 = "/" + text2;
		}
		string text3 = SaveSystem.ToProtonAccessiblePath(text + "/pfx/drive_c" + text2);
		if (!text3.EndsWith("/"))
		{
			text3 += "/";
		}
		return text3;
	}

	// Token: 0x06001F0B RID: 7947 RVA: 0x000929BC File Offset: 0x00090BBC
	private static bool TryGetDemoProtonPrefix(out string demoPrefix)
	{
		demoPrefix = null;
		string text = SaveSystem.NormalizeUnixPath(Environment.GetEnvironmentVariable("STEAM_COMPAT_DATA_PATH"));
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		List<string> list = new List<string>();
		SaveSystem.AddUniquePath(list, SaveSystem.ReplaceAppIdPathSegment(text, "4358690", "5075680"));
		string unixParent = SaveSystem.GetUnixParent(SaveSystem.GetUnixParent(text));
		SaveSystem.AddDemoPrefixCandidatesFromLibraryRoot(SaveSystem.GetUnixParent(unixParent), list);
		SaveSystem.AddDemoPrefixCandidatesFromVdf(unixParent + "/libraryfolders.vdf", list);
		string text2 = SaveSystem.NormalizeUnixPath(Environment.GetEnvironmentVariable("STEAM_COMPAT_CLIENT_INSTALL_PATH"));
		if (!string.IsNullOrEmpty(text2))
		{
			SaveSystem.AddDemoPrefixCandidatesFromLibraryRoot(text2, list);
			SaveSystem.AddDemoPrefixCandidatesFromVdf(text2 + "/steamapps/libraryfolders.vdf", list);
		}
		SaveSystem.TryAddDemoPrefixFromSteamInstallDir(list);
		for (int i = 0; i < list.Count; i++)
		{
			if (SaveSystem.ProtonDirectoryExists(list[i]))
			{
				demoPrefix = list[i];
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F0C RID: 7948 RVA: 0x00092A8A File Offset: 0x00090C8A
	private static void AddDemoPrefixCandidatesFromLibraryRoot(string libraryRoot, List<string> candidates)
	{
		libraryRoot = SaveSystem.NormalizeUnixPath(libraryRoot);
		if (string.IsNullOrEmpty(libraryRoot))
		{
			return;
		}
		SaveSystem.AddUniquePath(candidates, libraryRoot + "/steamapps/compatdata/5075680");
	}

	// Token: 0x06001F0D RID: 7949 RVA: 0x00092AB0 File Offset: 0x00090CB0
	private static void AddDemoPrefixCandidatesFromVdf(string vdfUnixPath, List<string> candidates)
	{
		string text = SaveSystem.ToProtonAccessiblePath(vdfUnixPath);
		if (string.IsNullOrEmpty(text) || !File.Exists(text))
		{
			return;
		}
		string text2;
		try
		{
			text2 = File.ReadAllText(text);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[SaveSystem] Failed to read Steam libraryfolders.vdf at [" + text + "]: " + ex.Message);
			return;
		}
		int num = 0;
		for (;;)
		{
			int num2 = text2.IndexOf("\"path\"", num, StringComparison.OrdinalIgnoreCase);
			if (num2 < 0)
			{
				break;
			}
			int num3 = text2.IndexOf('"', num2 + "\"path\"".Length);
			if (num3 < 0)
			{
				break;
			}
			int num4 = text2.IndexOf('"', num3 + 1);
			if (num4 < 0)
			{
				break;
			}
			SaveSystem.AddDemoPrefixCandidatesFromLibraryRoot(text2.Substring(num3 + 1, num4 - num3 - 1).Replace("\\\\", "\\"), candidates);
			num = num4 + 1;
		}
	}

	// Token: 0x06001F0E RID: 7950 RVA: 0x00092B80 File Offset: 0x00090D80
	private static void TryAddDemoPrefixFromSteamInstallDir(List<string> candidates)
	{
		try
		{
			if (SteamManager.Initialized)
			{
				AppId_t appId_t = new AppId_t(uint.Parse("5075680"));
				if (SteamApps.BIsAppInstalled(appId_t))
				{
					string text;
					if (SteamApps.GetAppInstallDir(appId_t, out text, 1024U) != 0U && !string.IsNullOrEmpty(text))
					{
						string text2 = SaveSystem.NormalizeUnixPath(text);
						if (text2.Length < 2 || char.ToUpperInvariant(text2[0]) != 'C' || text2[1] != ':')
						{
							SaveSystem.AddDemoPrefixCandidatesFromLibraryRoot(SaveSystem.GetUnixParent(SaveSystem.GetUnixParent(SaveSystem.GetUnixParent(text2))), candidates);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogWarning("[SaveSystem] Failed to resolve demo install dir via Steam API: " + ex.Message);
		}
	}

	// Token: 0x06001F0F RID: 7951 RVA: 0x00092C3C File Offset: 0x00090E3C
	private static string GetPersistentDataPathRelativeToDriveC()
	{
		string text = Application.persistentDataPath.Replace('\\', '/');
		int num = text.IndexOf(':');
		if (num >= 0 && num + 1 < text.Length)
		{
			text = text.Substring(num + 1);
		}
		if (!text.StartsWith("/"))
		{
			text = "/" + text;
		}
		return text.TrimEnd('/');
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x00092C9C File Offset: 0x00090E9C
	private static string ReplaceAppIdPathSegment(string path, string fromAppId, string toAppId)
	{
		path = SaveSystem.NormalizeUnixPath(path);
		if (path.EndsWith("/" + fromAppId, StringComparison.Ordinal))
		{
			return path.Substring(0, path.Length - fromAppId.Length) + toAppId;
		}
		return path.Replace(fromAppId, toAppId);
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x00092CE8 File Offset: 0x00090EE8
	private static void AddUniquePath(List<string> list, string path)
	{
		path = SaveSystem.NormalizeUnixPath(path);
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (string.Equals(list[i], path, StringComparison.Ordinal))
			{
				return;
			}
		}
		list.Add(path);
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x00092D30 File Offset: 0x00090F30
	private static string GetUnixParent(string path)
	{
		path = SaveSystem.NormalizeUnixPath(path);
		if (string.IsNullOrEmpty(path))
		{
			return path;
		}
		int num = path.LastIndexOf('/');
		if (num <= 0)
		{
			return path;
		}
		return path.Substring(0, num);
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x00092D68 File Offset: 0x00090F68
	private static string NormalizeUnixPath(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			return path;
		}
		path = path.Replace('\\', '/').TrimEnd('/');
		if (path.Length >= 2 && char.ToUpperInvariant(path[0]) == 'Z' && path[1] == ':')
		{
			path = path.Substring(2);
			if (string.IsNullOrEmpty(path))
			{
				path = "/";
			}
		}
		return path;
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x00092DD0 File Offset: 0x00090FD0
	private static string ToProtonAccessiblePath(string unixPath)
	{
		unixPath = SaveSystem.NormalizeUnixPath(unixPath);
		if (string.IsNullOrEmpty(unixPath))
		{
			return unixPath;
		}
		if (unixPath.Length >= 2 && unixPath[1] == ':')
		{
			return unixPath;
		}
		if (unixPath.StartsWith("/"))
		{
			return "Z:" + unixPath;
		}
		return unixPath;
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x00092E20 File Offset: 0x00091020
	private static bool ProtonDirectoryExists(string unixPath)
	{
		bool flag;
		try
		{
			flag = Directory.Exists(SaveSystem.ToProtonAccessiblePath(unixPath));
		}
		catch
		{
			flag = false;
		}
		return flag;
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x00092E54 File Offset: 0x00091054
	public static string GetNameForLimitedSaveSlot(int slotIndex)
	{
		if (slotIndex < 1 || slotIndex > 3)
		{
			Debug.LogError(string.Format("Error: limited save slot index [{0}] is out of range", slotIndex));
			return null;
		}
		return string.Format("{0}_{1}", LazyAPI.Platform.GetPlatformName(), slotIndex);
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x00092E90 File Offset: 0x00091090
	public static List<SaveSlotData> GetLimitedSaveSlotsData()
	{
		List<SaveSlotData> list = new List<SaveSlotData>(new SaveSlotData[3]);
		List<SaveSlotData> list2 = SaveSystem.SaveSlotDataList;
		for (int i = 0; i < 3; i++)
		{
			string nameForLimitedSaveSlot = SaveSystem.GetNameForLimitedSaveSlot(i + 1);
			for (int j = 0; j < list2.Count; j++)
			{
				if (SaveSystem.IsCurrentApplicationLimitedSaveSlot(list2[j]) && list2[j].slotName == nameForLimitedSaveSlot)
				{
					list[i] = list2[j];
					break;
				}
			}
		}
		return list;
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x00092F0F File Offset: 0x0009110F
	public static List<SaveSlotData> GetImportableSaveSlotsForLimitedSlots()
	{
		return SaveSystem.GetImportableSaveSlotsForLimitedSlots(SaveSystem.SaveSlotDataList);
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x00092F1C File Offset: 0x0009111C
	private static List<SaveSlotData> GetImportableSaveSlotsForLimitedSlots(List<SaveSlotData> allSlots)
	{
		List<SaveSlotData> list = new List<SaveSlotData>();
		if (allSlots == null)
		{
			SaveSystem.SaveImportLog("GetImportableSaveSlotsForLimitedSlots: allSlots is null");
			return list;
		}
		SaveSystem.SaveImportLog(string.Format("GetImportableSaveSlotsForLimitedSlots: checking [{0}] slots", allSlots.Count));
		for (int i = 0; i < allSlots.Count; i++)
		{
			SaveSlotData saveSlotData = allSlots[i];
			if (saveSlotData == null || SaveSystem.IsCurrentApplicationLimitedSaveSlot(saveSlotData))
			{
				SaveSystem.SaveImportLog(string.Format("GetImportableSaveSlotsForLimitedSlots: skip slot[{0}] slotName:[{1}] isDemoSave:[{2}] reason:[{3}]", new object[]
				{
					i,
					(saveSlotData != null) ? saveSlotData.slotName : null,
					(saveSlotData != null) ? new bool?(saveSlotData.isDemoSave) : null,
					(saveSlotData == null) ? "null" : "current limited slot"
				}));
			}
			else
			{
				SaveSystem.SaveImportLog(string.Format("GetImportableSaveSlotsForLimitedSlots: add slot[{0}] slotName:[{1}] platform:[{2}] isDemoSave:[{3}]", new object[] { i, saveSlotData.slotName, saveSlotData.platform, saveSlotData.isDemoSave }));
				list.Add(saveSlotData);
			}
		}
		list.Sort((SaveSlotData x, SaveSlotData y) => DateTime.Compare(y.GetSaveDateTime(), x.GetSaveDateTime()));
		SaveSystem.SaveImportLog(string.Format("GetImportableSaveSlotsForLimitedSlots: result count [{0}]", list.Count));
		return list;
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x0009306C File Offset: 0x0009126C
	public static bool HasImportableSaveSlotsForLimitedSlots()
	{
		bool flag = SaveSystem.GetImportableSaveSlotsForLimitedSlots().Count > 0;
		SaveSystem.SaveImportLog(string.Format("HasImportableSaveSlotsForLimitedSlots: [{0}]", flag));
		return flag;
	}

	// Token: 0x06001F1B RID: 7963 RVA: 0x0009309D File Offset: 0x0009129D
	public static string GetLimitedSaveSlotSourceLabel(SaveSlotData slotData)
	{
		if (slotData == null)
		{
			return string.Empty;
		}
		if (slotData.isDemoSave)
		{
			return "DEMO";
		}
		return string.Empty;
	}

	// Token: 0x17000543 RID: 1347
	// (get) Token: 0x06001F1C RID: 7964 RVA: 0x000930BB File Offset: 0x000912BB
	private static OdinBinaryFileSerializer OdinBinaryFileSerializer
	{
		get
		{
			if (SaveSystem.odinBinaryFileSerializer != null)
			{
				return SaveSystem.odinBinaryFileSerializer;
			}
			SaveSystem.odinBinaryFileSerializer = new OdinBinaryFileSerializer(".dat", null, null);
			return SaveSystem.odinBinaryFileSerializer;
		}
	}

	// Token: 0x06001F1D RID: 7965 RVA: 0x00002318 File Offset: 0x00000518
	private static void SaveImportLog(string message)
	{
	}

	// Token: 0x06001F1E RID: 7966 RVA: 0x000930E0 File Offset: 0x000912E0
	private static void PrepareLoadedSaveSlotInfos([TupleElementNames(new string[] { "data", "fileName" })] List<ValueTuple<SaveSlotData, string>> slots, string source, bool forceDemoSave)
	{
		if (slots == null)
		{
			SaveSystem.SaveImportLog(source + ": LoadAll returned null");
			return;
		}
		SaveSystem.SaveImportLog(string.Format("{0}: LoadAll returned [{1}] slot info files", source, slots.Count));
		for (int i = 0; i < slots.Count; i++)
		{
			SaveSlotData item = slots[i].Item1;
			string slotNameFromInfoFileName = SaveSystem.GetSlotNameFromInfoFileName(slots[i].Item2);
			if (item == null)
			{
				SaveSystem.SaveImportLog(string.Format("{0}: slot[{1}] file:[{2}] data is null", source, i, slots[i].Item2));
			}
			else
			{
				item.slotName = slotNameFromInfoFileName;
				if (forceDemoSave && !item.isDemoSave)
				{
					SaveSystem.SaveImportLog(string.Format("{0}: slot[{1}] file:[{2}] is not marked as demo in metadata, forcing demo source flag", source, i, slots[i].Item2));
					item.isDemoSave = true;
				}
				SaveSystem.SaveImportLog(string.Format("{0}: slot[{1}] file:[{2}] slotName:[{3}] platform:[{4}] isDemoSave:[{5}] repValue:[{6}] currentLimited:[{7}] canLoad:[{8}]", new object[]
				{
					source,
					i,
					slots[i].Item2,
					item.slotName,
					item.platform,
					item.isDemoSave,
					item.repValue,
					SaveSystem.IsCurrentApplicationLimitedSaveSlot(item),
					SaveSystem.CanLoadSaveSlot(item)
				}));
			}
		}
	}

	// Token: 0x06001F1F RID: 7967 RVA: 0x00093233 File Offset: 0x00091433
	private static string GetSlotNameFromInfoFileName(string fileName)
	{
		if (fileName == null)
		{
			return null;
		}
		return fileName.Replace(".info", "");
	}

	// Token: 0x06001F20 RID: 7968 RVA: 0x0009324A File Offset: 0x0009144A
	private static bool IsSameLoadedSaveSlot(SaveSlotData cachedSlotData, SaveSlotData requestedSlotData)
	{
		return cachedSlotData != null && requestedSlotData != null && cachedSlotData.slotName == requestedSlotData.slotName && cachedSlotData.isDemoSave == requestedSlotData.isDemoSave;
	}

	// Token: 0x06001F21 RID: 7969 RVA: 0x00093278 File Offset: 0x00091478
	private static List<SaveSlotData> ReadSaveSlotsData()
	{
		SaveSystem.<>c__DisplayClass73_0 CS$<>8__locals1 = new SaveSystem.<>c__DisplayClass73_0();
		CS$<>8__locals1.slotReadFinished = false;
		Action onSaveSlotsReadStarted = SaveSystem.OnSaveSlotsReadStarted;
		if (onSaveSlotsReadStarted != null)
		{
			onSaveSlotsReadStarted();
		}
		List<SaveSlotData> list2;
		try
		{
			CS$<>8__locals1.saveSlots = null;
			ApplicationSettings currentSetting = LazyApplicationSettings.GetCurrentSetting();
			SaveSystem.SaveImportLog(string.Format("ReadSaveSlotsData started. SaveFolder:[{0}] DemoSaveFolder:[{1}] currentSettingsNull:[{2}]", SaveSystem.SaveFolder, SaveSystem.DemoSaveFolder, currentSetting == null));
			SaveSystem.lazySaveSystem.LoadAll<SaveSlotData>(SaveSystem.SaveFolder, delegate([TupleElementNames(new string[] { "data", "fileName" })] List<ValueTuple<SaveSlotData, string>> list)
			{
				SaveSystem.<>c__DisplayClass73_1 CS$<>8__locals2 = new SaveSystem.<>c__DisplayClass73_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.list = list;
				SaveSystem.PrepareLoadedSaveSlotInfos(CS$<>8__locals2.list, "Current application", false);
				if (Directory.Exists(SaveSystem.DemoSaveFolder))
				{
					SaveSystem.lazySaveSystem.LoadAll<SaveSlotData>(SaveSystem.DemoSaveFolder, delegate([TupleElementNames(new string[] { "data", "fileName" })] List<ValueTuple<SaveSlotData, string>> list2)
					{
						SaveSystem.PrepareLoadedSaveSlotInfos(list2, "Standalone demo application", true);
						CS$<>8__locals2.list.AddRange(list2);
						base.<ReadSaveSlotsData>g__AfterLoadAll|2();
					});
					return;
				}
				CS$<>8__locals2.<ReadSaveSlotsData>g__AfterLoadAll|2();
			});
			list2 = CS$<>8__locals1.saveSlots;
		}
		catch (Exception ex)
		{
			Debug.Log(string.Format("Error during load save slots:[{0}]", ex));
			SaveSystem.SaveImportLog(string.Format("ReadSaveSlotsData failed with exception:[{0}]", ex));
			if (!CS$<>8__locals1.slotReadFinished)
			{
				Action onSaveSlotsReadCompleted = SaveSystem.OnSaveSlotsReadCompleted;
				if (onSaveSlotsReadCompleted != null)
				{
					onSaveSlotsReadCompleted();
				}
			}
			list2 = new List<SaveSlotData>();
		}
		return list2;
	}

	// Token: 0x06001F22 RID: 7970 RVA: 0x00093348 File Offset: 0x00091548
	private static string GetFolderForSlotData(SaveSlotData slotData)
	{
		string text = SaveSystem.SaveFolder;
		LazyApplicationSettings.GetCurrentSetting();
		if (slotData.isDemoSave)
		{
			text = SaveSystem.DemoSaveFolder;
		}
		SaveSystem.SaveImportLog(string.Format("GetFolderForSlotData slotName:[{0}] platform:[{1}] isDemoSave:[{2}] -> folder:[{3}]", new object[]
		{
			(slotData != null) ? slotData.slotName : null,
			(slotData != null) ? slotData.platform : null,
			(slotData != null) ? new bool?(slotData.isDemoSave) : null,
			text
		}));
		return text;
	}

	// Token: 0x06001F23 RID: 7971 RVA: 0x000933C8 File Offset: 0x000915C8
	private static bool IsLimitedSaveSlotName(string slotName)
	{
		for (int i = 1; i <= 3; i++)
		{
			if (slotName == SaveSystem.GetNameForLimitedSaveSlot(i))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001F24 RID: 7972 RVA: 0x000933F2 File Offset: 0x000915F2
	private static bool IsCurrentApplicationLimitedSaveSlot(SaveSlotData slotData)
	{
		return slotData != null && !slotData.isDemoSave && SaveSystem.IsLimitedSaveSlotName(slotData.slotName);
	}

	// Token: 0x06001F25 RID: 7973 RVA: 0x0009340E File Offset: 0x0009160E
	private static bool IsImportedSaveSlotDeleted(SaveSlotData slotData)
	{
		return slotData == null || (slotData.isDemoSave && slotData.IsDemoSlotDeleted);
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x00093428 File Offset: 0x00091628
	public static void Load(SaveSlotData slotData, Action<GameSave> callback)
	{
		SaveSystem.<>c__DisplayClass78_0 CS$<>8__locals1 = new SaveSystem.<>c__DisplayClass78_0();
		CS$<>8__locals1.callback = callback;
		CS$<>8__locals1.slotData = slotData;
		CS$<>8__locals1.slotReadFinished = false;
		Action onSaveLoadingStarted = SaveSystem.OnSaveLoadingStarted;
		if (onSaveLoadingStarted != null)
		{
			onSaveLoadingStarted();
		}
		try
		{
			string text = "Load started slotName:[{0}] platform:[{1}] isDemoSave:[{2}]";
			SaveSlotData slotData2 = CS$<>8__locals1.slotData;
			object obj = ((slotData2 != null) ? slotData2.slotName : null);
			SaveSlotData slotData3 = CS$<>8__locals1.slotData;
			object obj2 = ((slotData3 != null) ? slotData3.platform : null);
			SaveSlotData slotData4 = CS$<>8__locals1.slotData;
			SaveSystem.SaveImportLog(string.Format(text, obj, obj2, (slotData4 != null) ? new bool?(slotData4.isDemoSave) : null));
			if (!SaveSystem.CanLoadSaveSlot(CS$<>8__locals1.slotData))
			{
				string text2 = "Save slot [";
				SaveSlotData slotData5 = CS$<>8__locals1.slotData;
				Debug.LogWarning(text2 + ((slotData5 != null) ? slotData5.slotName : null) + "] can't be loaded");
				string text3 = "Load rejected by CanLoadSaveSlot slotName:[";
				SaveSlotData slotData6 = CS$<>8__locals1.slotData;
				SaveSystem.SaveImportLog(text3 + ((slotData6 != null) ? slotData6.slotName : null) + "]");
				Action<GameSave> callback2 = CS$<>8__locals1.callback;
				if (callback2 != null)
				{
					callback2(null);
				}
				Action onSaveLoadingEnded = SaveSystem.OnSaveLoadingEnded;
				if (onSaveLoadingEnded != null)
				{
					onSaveLoadingEnded();
				}
			}
			else
			{
				string folderForSlotData = SaveSystem.GetFolderForSlotData(CS$<>8__locals1.slotData);
				if (SaveSystem.IsLimitedSaveSlotsEnabled)
				{
					for (int i = 0; i < SaveSystem.saveSlotDataList.Count; i++)
					{
						if (SaveSystem.IsSameLoadedSaveSlot(SaveSystem.saveSlotDataList[i], CS$<>8__locals1.slotData) && SaveSystem.saveDataList[i] != null)
						{
							SaveSystem.SaveImportLog(string.Format("Load served from cache slotName:[{0}] isDemoSave:[{1}] index:[{2}] bytes:[{3}]", new object[]
							{
								CS$<>8__locals1.slotData.slotName,
								CS$<>8__locals1.slotData.isDemoSave,
								i,
								SaveSystem.saveDataList[i].Length
							}));
							CS$<>8__locals1.<Load>g__OnLoaded|0(SaveSystem.OdinBinaryFileSerializer.Deserialize<GameSave>(SaveSystem.saveDataList[i]));
							return;
						}
					}
				}
				SaveSystem.SaveImportLog(string.Concat(new string[]
				{
					"Load from folder:[",
					folderForSlotData,
					"] slotName:[",
					CS$<>8__locals1.slotData.slotName,
					"]"
				}));
				SaveSystem.lazySaveSystem.Load<GameSave>(folderForSlotData, CS$<>8__locals1.slotData.slotName, new Action<GameSave>(CS$<>8__locals1.<Load>g__OnLoaded|0));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during load save:[{0}]", ex));
			string text4 = "Load failed slotName:[{0}] exception:[{1}]";
			SaveSlotData slotData7 = CS$<>8__locals1.slotData;
			SaveSystem.SaveImportLog(string.Format(text4, (slotData7 != null) ? slotData7.slotName : null, ex));
			if (!CS$<>8__locals1.slotReadFinished)
			{
				CS$<>8__locals1.callback(null);
				Action onSaveLoadingEnded2 = SaveSystem.OnSaveLoadingEnded;
				if (onSaveLoadingEnded2 != null)
				{
					onSaveLoadingEnded2();
				}
			}
		}
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x000936DC File Offset: 0x000918DC
	public static void ApplyCustomActionsForDemoSaveLoadedInRelease(SaveSlotData slotData, GameSave loadedSave)
	{
		loadedSave.isDemoSaveLoadedInReleaseCustomActionsApplied = true;
		if (DLCEngine.IsDLCAvailable(DLCVersion.Preorder))
		{
			loadedSave.GivePreorderReward();
		}
		else
		{
			Debug.Log("No preorder available GivePreorderReward failed");
		}
		loadedSave.knowledgeSystem.ApplyDelayedDemoTechUnlocks();
		loadedSave.achievementsSystem.VerifyAndTryUnlockAchievement();
		if (loadedSave.questSystemData.IsQuestInStatus("15_guards_fightback_win_demo", QuestStatus.InProgress))
		{
			loadedSave.questSystemData.CancelQuest("15_guards_fightback_win_demo");
			loadedSave.questSystemData.StartQuest("15_guards_fightback_win", 0f);
		}
		if (loadedSave.questSystemData.IsQuestInStatus("26_port_foreman_reunite_demo", QuestStatus.InProgress))
		{
			loadedSave.questSystemData.CancelQuest("26_port_foreman_reunite_demo");
			loadedSave.questSystemData.StartQuest("26_port_foreman_reunite_if_demo_version_not_finished", 0f);
		}
		if (loadedSave.questSystemData.IsQuestInStatus("15_guards_fightback_win_demo", QuestStatus.Completed))
		{
			loadedSave.questSystemData.StartQuest("15_guards_fightback_win_demo_finish", 0f);
			loadedSave.questSystemData.CompleteQuest("15_guards_fightback_win_demo_finish", 0f);
		}
		Debug.Log(string.Format("#DEV# slotData.repValue:[{0}] 1", slotData.repValue));
		if (slotData.repValue)
		{
			Debug.Log(string.Format("#DEV# slotData.repValue:[{0}] 2", slotData.repValue));
			loadedSave.questSystemData.CancelQuest("26_port_foreman_reunite_demo");
			loadedSave.questSystemData.StartQuest("26_port_foreman_reunite", 0f);
		}
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x00093830 File Offset: 0x00091A30
	public static bool Remove(SaveSlotData slotData, Action callback)
	{
		string folderForSlotData = SaveSystem.GetFolderForSlotData(slotData);
		bool flag2;
		try
		{
			bool flag = SaveSystem.lazySaveSystem.Remove<GameSave>(folderForSlotData, slotData.slotName, callback);
			flag &= SaveSystem.lazySaveSystem.Remove<SaveSlotData>(folderForSlotData, slotData.slotName, null);
			if (flag && SaveSystem.IsLimitedSaveSlotsEnabled)
			{
				int num = -1;
				for (int i = 0; i < SaveSystem.saveSlotDataList.Count; i++)
				{
					if (SaveSystem.IsSameLoadedSaveSlot(SaveSystem.saveSlotDataList[i], slotData))
					{
						num = i;
						break;
					}
				}
				if (num != -1)
				{
					SaveSystem.saveSlotDataList.RemoveAt(num);
					SaveSystem.saveDataList.RemoveAt(num);
				}
			}
			flag2 = flag;
		}
		catch (Exception ex)
		{
			Debug.Log(string.Format("Error during deleting save:[{0}]", ex));
			flag2 = false;
		}
		return flag2;
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x000938F0 File Offset: 0x00091AF0
	public static void ImportSaveToLimitedSlot(SaveSlotData sourceSlotData, int targetSlotIndex, Action<bool> callback, bool triggerSaveWriteStarted = true)
	{
		string text = "ImportSaveToLimitedSlot started sourceSlotName:[{0}] sourcePlatform:[{1}] sourceIsDemo:[{2}] targetSlotIndex:[{3}]";
		object[] array = new object[4];
		int num = 0;
		SaveSlotData sourceSlotData2 = sourceSlotData;
		array[num] = ((sourceSlotData2 != null) ? sourceSlotData2.slotName : null);
		int num2 = 1;
		SaveSlotData sourceSlotData3 = sourceSlotData;
		array[num2] = ((sourceSlotData3 != null) ? sourceSlotData3.platform : null);
		int num3 = 2;
		SaveSlotData sourceSlotData4 = sourceSlotData;
		array[num3] = ((sourceSlotData4 != null) ? new bool?(sourceSlotData4.isDemoSave) : null);
		array[3] = targetSlotIndex;
		SaveSystem.SaveImportLog(string.Format(text, array));
		if (sourceSlotData == null)
		{
			Debug.LogError("Import save error: source slot is null");
			SaveSystem.SaveImportLog("ImportSaveToLimitedSlot failed: source slot is null");
			Action<bool> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false);
			return;
		}
		else
		{
			string targetSlotName = SaveSystem.GetNameForLimitedSaveSlot(targetSlotIndex);
			if (!string.IsNullOrEmpty(targetSlotName))
			{
				if (triggerSaveWriteStarted)
				{
					Action onSaveWriteStarted = SaveSystem.OnSaveWriteStarted;
					if (onSaveWriteStarted != null)
					{
						onSaveWriteStarted();
					}
				}
				Action <>9__2;
				Action <>9__3;
				SaveSystem.Load(sourceSlotData, delegate(GameSave loadedSave)
				{
					if (loadedSave == null)
					{
						SaveSystem.SaveImportLog("ImportSaveToLimitedSlot failed: loaded save is null for sourceSlotName:[" + sourceSlotData.slotName + "]");
						base.<ImportSaveToLimitedSlot>g__FinishImport|0(false);
						return;
					}
					SaveSlotData saveSlotData = sourceSlotData.Copy();
					saveSlotData.slotName = targetSlotName;
					saveSlotData.platform = LazyAPI.Platform.GetPlatformName();
					saveSlotData.importedFromDemo = sourceSlotData.isDemoSave || sourceSlotData.importedFromDemo;
					saveSlotData.isDemoSave = false;
					SaveSystem.SaveImportLog(string.Format("ImportSaveToLimitedSlot saving targetSlotName:[{0}] targetPlatform:[{1}] importedFromDemo:[{2}]", saveSlotData.slotName, saveSlotData.platform, saveSlotData.importedFromDemo));
					SaveSlotData saveSlotData2 = saveSlotData;
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							base.<ImportSaveToLimitedSlot>g__FinishImport|0(true);
						});
					}
					Action action2;
					if ((action2 = <>9__3) == null)
					{
						action2 = (<>9__3 = delegate
						{
							base.<ImportSaveToLimitedSlot>g__FinishImport|0(false);
						});
					}
					SaveSystem.Save(saveSlotData2, loadedSave, action, action2, false, null);
				});
				return;
			}
			SaveSystem.SaveImportLog(string.Format("ImportSaveToLimitedSlot failed: target slot name is empty for index:[{0}]", targetSlotIndex));
			Action<bool> callback3 = callback;
			if (callback3 == null)
			{
				return;
			}
			callback3(false);
			return;
		}
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x00093A18 File Offset: 0x00091C18
	public static void TriggerOnSaveStartEvent(bool instant = false)
	{
		if (instant)
		{
			Action onSaveWriteStartedInstant = SaveSystem.OnSaveWriteStartedInstant;
			if (onSaveWriteStartedInstant == null)
			{
				return;
			}
			onSaveWriteStartedInstant();
			return;
		}
		else
		{
			Action onSaveWriteStarted = SaveSystem.OnSaveWriteStarted;
			if (onSaveWriteStarted == null)
			{
				return;
			}
			onSaveWriteStarted();
			return;
		}
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x00093A3C File Offset: 0x00091C3C
	public static bool CanLoadSaveSlot(SaveSlotData slotData)
	{
		return !SaveSystem.IsImportedSaveSlotDeleted(slotData);
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x00093A4C File Offset: 0x00091C4C
	public static SaveSlotData GetLastSaveSlot()
	{
		List<SaveSlotData> list = new List<SaveSlotData>();
		list.AddRange(SaveSystem.IsLimitedSaveSlotsEnabled ? SaveSystem.GetLimitedSaveSlotsData() : SaveSystem.SaveSlotDataList);
		if (list.Count <= 0)
		{
			return null;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (!SaveSystem.CanLoadSaveSlot(list[i]))
			{
				list.RemoveAt(i);
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		list.Sort((SaveSlotData x, SaveSlotData y) => DateTime.Compare(y.GetSaveDateTime(), x.GetSaveDateTime()));
		return list[0];
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x00093AE4 File Offset: 0x00091CE4
	public static SaveSlotData GetActiveSaveData()
	{
		List<SaveSlotData> list = new List<SaveSlotData>();
		list.AddRange(SaveSystem.IsLimitedSaveSlotsEnabled ? SaveSystem.GetLimitedSaveSlotsData() : SaveSystem.SaveSlotDataList);
		if (list.Count <= 0)
		{
			return null;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (!SaveSystem.CanLoadSaveSlot(list[i]))
			{
				list.RemoveAt(i);
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		list.Sort((SaveSlotData x, SaveSlotData y) => DateTime.Compare(y.GetSaveDateTime(), x.GetSaveDateTime()));
		return list[0];
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x00093B7C File Offset: 0x00091D7C
	public static void Save(SaveSlotData slotData, GameSave gameSave, Action callbackSuccessful = null, Action callbackUnsuccessful = null, bool autoOnSaveStart = true, Action<SaveSlotData, GameSave> postPrepareToSave = null)
	{
		if (slotData == null)
		{
			Debug.LogError("Save Error: Cannot save to a null slot");
			if (callbackUnsuccessful != null)
			{
				callbackUnsuccessful();
			}
			return;
		}
		if (autoOnSaveStart)
		{
			Action onSaveWriteStarted = SaveSystem.OnSaveWriteStarted;
			if (onSaveWriteStarted != null)
			{
				onSaveWriteStarted();
			}
		}
		if (slotData.isDemoSave)
		{
			slotData.slotName = SaveSystem.GetNameForNewSlot(SaveSystem.SaveSlotDataList);
		}
		gameSave.PrepareToSave(slotData);
		if (postPrepareToSave != null)
		{
			postPrepareToSave(slotData, gameSave);
		}
		string saveFolder = SaveSystem.SaveFolder;
		Debug.Log("Save slotData.slotName:[" + slotData.slotName + "]");
		try
		{
			bool flag;
			if (SaveSystem.IsLimitedSaveSlotsEnabled)
			{
				gameSave.OnBeforeSerialize();
				byte[] array = SaveSystem.OdinBinaryFileSerializer.Serialize<GameSave>(gameSave);
				Debug.Log(string.Format("Serialized save length: {0}", array.Length));
				if (array.Length == 0)
				{
					flag = false;
				}
				else
				{
					flag = SaveSystem.OdinBinaryFileSerializer.SaveBytes(SaveSystem.SaveFolder, slotData.slotName, array);
					if (flag)
					{
						flag &= SaveSystem.lazySaveSystem.Save<SaveSlotData>(slotData, SaveSystem.SaveFolder, slotData.slotName, null);
					}
					if (flag)
					{
						SaveSystem.LimitedSaveSlotsActionsForSaveWrite(slotData, array);
					}
				}
			}
			else
			{
				flag = SaveSystem.lazySaveSystem.Save<GameSave>(gameSave, saveFolder, slotData.slotName, null);
				if (flag)
				{
					flag &= SaveSystem.lazySaveSystem.Save<SaveSlotData>(slotData, saveFolder, slotData.slotName, null);
				}
			}
			if (flag)
			{
				if (callbackSuccessful != null)
				{
					callbackSuccessful();
				}
			}
			else if (callbackUnsuccessful != null)
			{
				callbackUnsuccessful();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during save:[{0}]", ex));
			if (callbackUnsuccessful != null)
			{
				callbackUnsuccessful();
			}
		}
		if (autoOnSaveStart)
		{
			Action onSaveWriteEnded = SaveSystem.OnSaveWriteEnded;
			if (onSaveWriteEnded == null)
			{
				return;
			}
			onSaveWriteEnded();
		}
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x00093D00 File Offset: 0x00091F00
	private static void LimitedSaveSlotsActionsForSaveWrite(SaveSlotData slotData, byte[] gameSaveBytes)
	{
		bool flag = false;
		for (int i = 0; i < SaveSystem.saveSlotDataList.Count; i++)
		{
			if (SaveSystem.IsSameLoadedSaveSlot(SaveSystem.saveSlotDataList[i], slotData))
			{
				SaveSystem.saveSlotDataList[i] = slotData.Copy();
				SaveSystem.saveDataList[i] = gameSaveBytes;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			SaveSystem.saveSlotDataList.Add(slotData.Copy());
			SaveSystem.saveDataList.Add(gameSaveBytes);
		}
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x00093D78 File Offset: 0x00091F78
	public static string GetNameForNewSlot(List<SaveSlotData> existingSaveSlots)
	{
		int i = 1;
		while (i < 1000)
		{
			string text = i.ToString();
			bool flag = false;
			using (List<SaveSlotData>.Enumerator enumerator = existingSaveSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.slotName == text)
					{
						flag = true;
						break;
					}
				}
			}
			i++;
			if (!flag)
			{
				return LazyAPI.Platform.GetPlatformName() + "_" + text;
			}
		}
		Debug.LogError("Error: GetNameForNewSlot - too many iterations");
		return null;
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x00093E10 File Offset: 0x00092010
	public static GameSettings LoadGameSettings()
	{
		string gameSettingsPlayerPrefsKey = SaveSystem.GetGameSettingsPlayerPrefsKey();
		string text = string.Empty;
		if (!string.IsNullOrEmpty(gameSettingsPlayerPrefsKey) && LazyAPI.PlayerPrefs.HasKey(gameSettingsPlayerPrefsKey))
		{
			text = LazyAPI.PlayerPrefs.GetString(gameSettingsPlayerPrefsKey, "");
		}
		if (string.IsNullOrEmpty(text))
		{
			Debug.Log("[SaveSystem] No stored game settings found (PlayerPrefs key '" + gameSettingsPlayerPrefsKey + "'). Creating defaults (first run).");
			return new GameSettings();
		}
		GameSettings gameSettings = JsonUtility.FromJson<GameSettings>(text);
		if (!text.Contains("\"gpuGraphicsDefaultApplied\""))
		{
			gameSettings.gpuGraphicsDefaultApplied = true;
			Debug.Log(string.Format("[SaveSystem] Stored game settings predate GPU-based tier detection; keeping existing graphics tier {0}.", gameSettings.graphicsTier));
		}
		return gameSettings;
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x00093EA8 File Offset: 0x000920A8
	public static void SaveGameSettings()
	{
		string gameSettingsPlayerPrefsKey = SaveSystem.GetGameSettingsPlayerPrefsKey();
		if (string.IsNullOrEmpty(gameSettingsPlayerPrefsKey))
		{
			return;
		}
		string text = JsonUtility.ToJson(GameSettings.Instance);
		LazyAPI.PlayerPrefs.SetString(gameSettingsPlayerPrefsKey, text);
		try
		{
			LazyAPI.PlayerPrefs.Save();
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("Error during save GameSettings:[{0}]", ex));
		}
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x00093F0C File Offset: 0x0009210C
	private static string GetGameSettingsPlayerPrefsKey()
	{
		return "settings";
	}

	// Token: 0x04001BE9 RID: 7145
	private const string META_FILE_EXTENSION = ".info";

	// Token: 0x04001BEA RID: 7146
	private const string DATA_FILE_EXTENSION = ".dat";

	// Token: 0x04001BEB RID: 7147
	private const string RELEASE_FOLDER_NAME = "Graveyard Keeper 2";

	// Token: 0x04001BEC RID: 7148
	private const string DEMO_FOLDER_NAME = "Graveyard Keeper 2 Demo";

	// Token: 0x04001BED RID: 7149
	private const string STEAM_RELEASE_APP_ID = "4358690";

	// Token: 0x04001BEE RID: 7150
	private const string STEAM_DEMO_APP_ID = "5075680";

	// Token: 0x04001BEF RID: 7151
	private const string GAME_SETTINGS_PLAYER_PREFS_KEY = "settings";

	// Token: 0x04001BF0 RID: 7152
	public const int LIMITED_SAVE_SLOTS_COUNT = 3;

	// Token: 0x04001BF1 RID: 7153
	private static List<SaveSlotData> saveSlotDataList;

	// Token: 0x04001BF2 RID: 7154
	private static List<byte[]> saveDataList;

	// Token: 0x04001BFB RID: 7163
	private static string cachedProtonDemoSaveFolder;

	// Token: 0x04001BFC RID: 7164
	private static bool protonDemoSaveFolderResolved;

	// Token: 0x04001BFD RID: 7165
	private static OdinBinaryFileSerializer odinBinaryFileSerializer;

	// Token: 0x04001BFE RID: 7166
	public static LazySaveSystem lazySaveSystem = new LazySaveSystem(new ValueTuple<Type, IDataSerializer>[]
	{
		new ValueTuple<Type, IDataSerializer>(typeof(GameSave), SaveSystem.OdinBinaryFileSerializer),
		new ValueTuple<Type, IDataSerializer>(typeof(SaveSlotData), new JsonFileSerializer(".info")),
		new ValueTuple<Type, IDataSerializer>(typeof(GameSettings), new PlayerPrefsSerializer())
	});
}
