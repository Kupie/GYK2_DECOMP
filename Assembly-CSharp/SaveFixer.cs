using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

// Token: 0x02000486 RID: 1158
public static class SaveFixer
{
	// Token: 0x06001EB7 RID: 7863 RVA: 0x000910EF File Offset: 0x0008F2EF
	public static bool Apply(GameSave gameSave, IList<GameSceneConfig> gameSceneConfigs)
	{
		return gameSave != null && !string.IsNullOrEmpty(gameSave.GameSaveVer) && SaveFixer.ApplyInternal(gameSave, gameSceneConfigs, null);
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x0009110B File Offset: 0x0008F30B
	public static void ApplyForce(GameSave gameSave, IList<GameSceneConfig> gameSceneConfigs, SaveFixData forceFix)
	{
		if (gameSave == null || string.IsNullOrEmpty(gameSave.GameSaveVer) || forceFix == null)
		{
			return;
		}
		SaveFixer.ApplyInternal(gameSave, gameSceneConfigs, forceFix);
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x00091130 File Offset: 0x0008F330
	public static void LogApplicableFixes(GameSave gameSave)
	{
		GameSaveVersion gameSaveVersion;
		if (!SaveFixer.TryParseSaveVersion(gameSave, out gameSaveVersion))
		{
			Debug.LogError("[SaveFixer] Save version [" + ((gameSave != null) ? gameSave.GameSaveVer : null) + "] is not parseable, skip applying fixes");
			return;
		}
		SaveFixer.LoadedFixes loadedFixes = SaveFixer.LoadFixesInRange(gameSave, null);
		try
		{
			if (loadedFixes.LoadFailed)
			{
				string[] array = new string[5];
				array[0] = "[SaveFixer] Failed to load fix assets. Save version [";
				array[1] = ((gameSave != null) ? gameSave.GameSaveVer : null);
				array[2] = "] client [";
				int num = 3;
				GameInfo instance = LazySingletonSO<GameInfo>.Instance;
				array[num] = ((instance != null) ? instance.Version : null);
				array[4] = "]";
				Debug.LogError(string.Concat(array));
			}
			else if (loadedFixes.Fixes.Count == 0)
			{
				string[] array2 = new string[5];
				array2[0] = "[SaveFixer] No fixes to apply. Save version [";
				array2[1] = ((gameSave != null) ? gameSave.GameSaveVer : null);
				array2[2] = "] client [";
				int num2 = 3;
				GameInfo instance2 = LazySingletonSO<GameInfo>.Instance;
				array2[num2] = ((instance2 != null) ? instance2.Version : null);
				array2[4] = "]";
				Debug.Log(string.Concat(array2));
			}
			else
			{
				string[] array3 = new string[5];
				array3[0] = "[SaveFixer] Fixes that would apply to save [";
				array3[1] = ((gameSave != null) ? gameSave.GameSaveVer : null);
				array3[2] = "] (client [";
				int num3 = 3;
				GameInfo instance3 = LazySingletonSO<GameInfo>.Instance;
				array3[num3] = ((instance3 != null) ? instance3.Version : null);
				array3[4] = "]):";
				Debug.Log(string.Concat(array3));
				foreach (SaveFixData saveFixData in loadedFixes.Fixes)
				{
					int num4 = SaveFixer.CountEnabledOperations(saveFixData);
					Debug.Log(string.Format("[SaveFixer]  - {0} ({1} operations)", saveFixData.name, num4));
				}
			}
		}
		finally
		{
			loadedFixes.Release();
		}
	}

	// Token: 0x06001EBA RID: 7866 RVA: 0x00091300 File Offset: 0x0008F500
	private static bool ApplyInternal(GameSave gameSave, IList<GameSceneConfig> gameSceneConfigs, SaveFixData forceFix)
	{
		GameSaveVersion gameSaveVersion;
		if (forceFix == null && !SaveFixer.TryParseSaveVersion(gameSave, out gameSaveVersion))
		{
			Debug.LogError("[SaveFixer] Save version [" + gameSave.GameSaveVer + "] is not parseable, keep save version");
			return false;
		}
		SaveFixer.LoadedFixes loadedFixes = SaveFixer.LoadFixesInRange(gameSave, forceFix);
		SaveFixContext saveFixContext = null;
		bool flag;
		try
		{
			if (loadedFixes.LoadFailed)
			{
				Debug.LogError("[SaveFixer] Failed to load fix assets, keep save version [" + gameSave.GameSaveVer + "]");
				flag = false;
			}
			else
			{
				bool flag2 = loadedFixes.Fixes.Count > 0;
				bool flag3 = forceFix == null && SaveFixer.ShouldApplyCodeFixes(gameSave);
				if (!flag2 && !flag3)
				{
					string[] array = new string[5];
					array[0] = "[SaveFixer] No fixes to apply. Save version [";
					array[1] = gameSave.GameSaveVer;
					array[2] = "] client [";
					int num = 3;
					GameInfo instance = LazySingletonSO<GameInfo>.Instance;
					array[num] = ((instance != null) ? instance.Version : null);
					array[4] = "]";
					Debug.Log(string.Concat(array));
					flag = true;
				}
				else
				{
					saveFixContext = new SaveFixContext(gameSave, gameSceneConfigs);
					bool flag4 = false;
					if (flag3)
					{
						SaveFixContext saveFixContext2 = saveFixContext;
						string[] array2 = new string[5];
						array2[0] = "Applying code fixes to save [";
						array2[1] = gameSave.GameSaveVer;
						array2[2] = "] → [";
						int num2 = 3;
						GameInfo instance2 = LazySingletonSO<GameInfo>.Instance;
						array2[num2] = ((instance2 != null) ? instance2.Version : null);
						array2[4] = "]";
						saveFixContext2.Log(string.Concat(array2));
						flag4 |= SaveFixer.ApplyCodeFixes(saveFixContext);
					}
					if (flag2)
					{
						SaveFixContext saveFixContext3 = saveFixContext;
						string text = "Applying {0} fix asset(s) to save [{1}] → [{2}]";
						object obj = loadedFixes.Fixes.Count;
						object gameSaveVer = gameSave.GameSaveVer;
						GameInfo instance3 = LazySingletonSO<GameInfo>.Instance;
						saveFixContext3.Log(string.Format(text, obj, gameSaveVer, (instance3 != null) ? instance3.Version : null));
						foreach (SaveFixData saveFixData in loadedFixes.Fixes)
						{
							flag4 |= SaveFixer.ApplyFix(saveFixContext, saveFixData);
						}
					}
					if (flag4)
					{
						Debug.LogError("[SaveFixer] One or more operations failed, keep save version [" + gameSave.GameSaveVer + "]");
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
			}
		}
		finally
		{
			if (saveFixContext != null)
			{
				saveFixContext.UnloadLoadedContent();
			}
			loadedFixes.Release();
		}
		return flag;
	}

	// Token: 0x06001EBB RID: 7867 RVA: 0x00091530 File Offset: 0x0008F730
	private static bool ApplyFix(SaveFixContext ctx, SaveFixData fix)
	{
		if (((fix != null) ? fix.Operations : null) == null)
		{
			return false;
		}
		bool flag = false;
		ctx.Log("Applying fix [" + fix.name + "]");
		foreach (SaveFixOperation saveFixOperation in fix.Operations)
		{
			if (saveFixOperation != null && saveFixOperation.isEnabled)
			{
				try
				{
					saveFixOperation.Apply(ctx);
				}
				catch (Exception ex)
				{
					flag = true;
					Debug.LogError(string.Concat(new string[] { "[SaveFixer] Operation [", saveFixOperation.Summary, "] in [", fix.name, "] failed" }));
					Debug.LogException(ex);
				}
			}
		}
		return flag;
	}

	// Token: 0x06001EBC RID: 7868 RVA: 0x0009160C File Offset: 0x0008F80C
	private static SaveFixer.LoadedFixes LoadFixesInRange(GameSave gameSave, SaveFixData forceFix)
	{
		if (forceFix != null)
		{
			return SaveFixer.LoadedFixes.FromExisting(forceFix);
		}
		GameSaveVersion? gameSaveVersion;
		GameSaveVersion? gameSaveVersion2;
		SaveFixer.TryGetVersionRange(gameSave, out gameSaveVersion, out gameSaveVersion2);
		SaveFixer.LoadedFixes loadedFixes = new SaveFixer.LoadedFixes
		{
			Fixes = new List<SaveFixData>()
		};
		SaveFixer.LoadedFixes loadedFixes2 = loadedFixes;
		try
		{
			loadedFixes2.LocationsHandle = Addressables.LoadResourceLocationsAsync("SaveFixerAssets", typeof(SaveFixData));
			IList<IResourceLocation> list = loadedFixes2.LocationsHandle.WaitForCompletion();
			if (loadedFixes2.LocationsHandle.Status != AsyncOperationStatus.Succeeded || list == null)
			{
				loadedFixes2.LoadFailed = true;
				Debug.LogError("[SaveFixer] Failed to load SaveFixer asset locations");
				return loadedFixes2;
			}
			List<IResourceLocation> list2 = new List<IResourceLocation>();
			foreach (IResourceLocation resourceLocation in list)
			{
				GameSaveVersion gameSaveVersion3;
				if (!SaveFixer.TryGetLocationVersion(resourceLocation, out gameSaveVersion3))
				{
					Debug.LogWarning("[SaveFixer] Skip location [" + ((resourceLocation != null) ? resourceLocation.PrimaryKey : null) + "] — version is not parseable");
				}
				else if (SaveFixer.IsInApplyRange(gameSaveVersion3, gameSaveVersion, gameSaveVersion2))
				{
					list2.Add(resourceLocation);
				}
			}
			if (list2.Count == 0)
			{
				return loadedFixes2;
			}
			list2.Sort(new Comparison<IResourceLocation>(SaveFixer.CompareLocationVersions));
			loadedFixes2.AssetsHandle = Addressables.LoadAssetsAsync<SaveFixData>(list2, null);
			IList<SaveFixData> list3 = loadedFixes2.AssetsHandle.WaitForCompletion();
			if (loadedFixes2.AssetsHandle.Status != AsyncOperationStatus.Succeeded || list3 == null)
			{
				loadedFixes2.LoadFailed = true;
				Debug.LogError("[SaveFixer] Failed to load filtered SaveFixer assets");
				return loadedFixes2;
			}
			foreach (SaveFixData saveFixData in list3)
			{
				if (saveFixData != null)
				{
					loadedFixes2.Fixes.Add(saveFixData);
				}
			}
			loadedFixes2.Fixes.Sort(new Comparison<SaveFixData>(SaveFixer.CompareFixVersions));
		}
		catch (Exception ex)
		{
			loadedFixes2.LoadFailed = true;
			Debug.LogError("[SaveFixer] Failed to load SaveFixer assets: " + ex.Message);
		}
		return loadedFixes2;
	}

	// Token: 0x06001EBD RID: 7869 RVA: 0x00091848 File Offset: 0x0008FA48
	private static bool TryParseSaveVersion(GameSave gameSave, out GameSaveVersion saveVersion)
	{
		saveVersion = default(GameSaveVersion);
		return gameSave != null && GameSaveVersion.TryParse(gameSave.GameSaveVer, out saveVersion);
	}

	// Token: 0x06001EBE RID: 7870 RVA: 0x00091864 File Offset: 0x0008FA64
	private static void TryGetVersionRange(GameSave gameSave, out GameSaveVersion? saveVersion, out GameSaveVersion? clientVersion)
	{
		saveVersion = null;
		clientVersion = null;
		GameSaveVersion gameSaveVersion;
		if (SaveFixer.TryParseSaveVersion(gameSave, out gameSaveVersion))
		{
			saveVersion = new GameSaveVersion?(gameSaveVersion);
		}
		GameSaveVersion gameSaveVersion2;
		if (LazySingletonSO<GameInfo>.Instance != null && GameSaveVersion.TryParse(LazySingletonSO<GameInfo>.Instance.Version, out gameSaveVersion2))
		{
			clientVersion = new GameSaveVersion?(gameSaveVersion2);
		}
	}

	// Token: 0x06001EBF RID: 7871 RVA: 0x000918C4 File Offset: 0x0008FAC4
	public static bool IsInApplyRange(GameSaveVersion fixVersion, GameSaveVersion? saveVersion, GameSaveVersion? clientVersion)
	{
		return saveVersion != null && (clientVersion == null || !(saveVersion.Value >= clientVersion.Value)) && fixVersion.NumberAsInt >= saveVersion.Value.NumberAsInt && (clientVersion == null || fixVersion.NumberAsInt <= clientVersion.Value.NumberAsInt);
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x0009193C File Offset: 0x0008FB3C
	private static bool ShouldApplyCodeFixes(GameSave gameSave)
	{
		GameSaveVersion? gameSaveVersion;
		GameSaveVersion? gameSaveVersion2;
		SaveFixer.TryGetVersionRange(gameSave, out gameSaveVersion, out gameSaveVersion2);
		return gameSaveVersion != null && (gameSaveVersion2 == null || !(gameSaveVersion.Value >= gameSaveVersion2.Value));
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x00091980 File Offset: 0x0008FB80
	private static bool ApplyCodeFixes(SaveFixContext ctx)
	{
		bool flag;
		try
		{
			SaveCodeFixes.Apply(ctx);
			flag = false;
		}
		catch (Exception ex)
		{
			Debug.LogError("[SaveFixer] Code fixes failed");
			Debug.LogException(ex);
			flag = true;
		}
		return flag;
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x000919BC File Offset: 0x0008FBBC
	private static bool TryGetLocationVersion(IResourceLocation location, out GameSaveVersion version)
	{
		version = default(GameSaveVersion);
		return location != null && !string.IsNullOrEmpty(location.PrimaryKey) && GameSaveVersion.TryParse(SaveFixer.GetAddressableAssetName(location.PrimaryKey), out version);
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x000919E8 File Offset: 0x0008FBE8
	private static string GetAddressableAssetName(string address)
	{
		int num = address.LastIndexOf('/');
		string text = ((num >= 0) ? address.Substring(num + 1) : address);
		if (text.EndsWith(".asset"))
		{
			text = text.Substring(0, text.Length - ".asset".Length);
		}
		return text;
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x00091A38 File Offset: 0x0008FC38
	private static int CompareLocationVersions(IResourceLocation a, IResourceLocation b)
	{
		GameSaveVersion gameSaveVersion;
		SaveFixer.TryGetLocationVersion(a, out gameSaveVersion);
		GameSaveVersion gameSaveVersion2;
		SaveFixer.TryGetLocationVersion(b, out gameSaveVersion2);
		return gameSaveVersion.CompareTo(gameSaveVersion2);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x00091A60 File Offset: 0x0008FC60
	private static int CompareFixVersions(SaveFixData a, SaveFixData b)
	{
		GameSaveVersion gameSaveVersion;
		a.TryGetVersion(out gameSaveVersion);
		GameSaveVersion gameSaveVersion2;
		b.TryGetVersion(out gameSaveVersion2);
		return gameSaveVersion.CompareTo(gameSaveVersion2);
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x00091A88 File Offset: 0x0008FC88
	private static int CountEnabledOperations(SaveFixData fix)
	{
		if (((fix != null) ? fix.Operations : null) == null)
		{
			return 0;
		}
		int num = 0;
		foreach (SaveFixOperation saveFixOperation in fix.Operations)
		{
			if (saveFixOperation != null && saveFixOperation.isEnabled)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x02000487 RID: 1159
	private struct LoadedFixes
	{
		// Token: 0x06001EC7 RID: 7879 RVA: 0x00091AF0 File Offset: 0x0008FCF0
		public static SaveFixer.LoadedFixes FromExisting(SaveFixData fix)
		{
			return new SaveFixer.LoadedFixes
			{
				Fixes = new List<SaveFixData> { fix }
			};
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x00091B19 File Offset: 0x0008FD19
		public void Release()
		{
			if (this.AssetsHandle.IsValid())
			{
				Addressables.Release<IList<SaveFixData>>(this.AssetsHandle);
			}
			if (this.LocationsHandle.IsValid())
			{
				Addressables.Release<IList<IResourceLocation>>(this.LocationsHandle);
			}
		}

		// Token: 0x04001BCB RID: 7115
		public List<SaveFixData> Fixes;

		// Token: 0x04001BCC RID: 7116
		public bool LoadFailed;

		// Token: 0x04001BCD RID: 7117
		public AsyncOperationHandle<IList<IResourceLocation>> LocationsHandle;

		// Token: 0x04001BCE RID: 7118
		public AsyncOperationHandle<IList<SaveFixData>> AssetsHandle;
	}
}
