using System;
using System.Collections.Generic;
using System.Text;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200017A RID: 378
public static class DLCEngine
{
	// Token: 0x17000171 RID: 369
	// (get) Token: 0x06000966 RID: 2406 RVA: 0x0002FDEF File Offset: 0x0002DFEF
	public static IReadOnlyDictionary<DLCVersion, DLCInfo> DLCInfos
	{
		get
		{
			return DLCEngine.dlcInfoByVersion;
		}
	}

	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06000967 RID: 2407 RVA: 0x0002FDF6 File Offset: 0x0002DFF6
	public static IReadOnlyDictionary<DLCVersion, StoreProductInfo> StoreProductInfos
	{
		get
		{
			return DLCEngine.storeProductInfoByVersion;
		}
	}

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002FDFD File Offset: 0x0002DFFD
	public static IEnumerable<DLCVersion> ConfiguredDLCVersions
	{
		get
		{
			return DLCEngine.dlcInfoByVersion.Keys;
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0002FE09 File Offset: 0x0002E009
	public static bool IsDLCAvailable(DLCVersion dlcVersion)
	{
		return dlcVersion == DLCVersion.None || DLCEngine.IsDLCAvailableReal(dlcVersion);
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0002FE18 File Offset: 0x0002E018
	public static void LogDLCStates()
	{
		StringBuilder stringBuilder = new StringBuilder("[DLCEngine] DLC states:");
		foreach (object obj in Enum.GetValues(typeof(DLCVersion)))
		{
			DLCVersion dlcversion = (DLCVersion)obj;
			if (dlcversion != DLCVersion.None)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(string.Format("[{0}]: {1}", dlcversion, DLCEngine.IsDLCAvailable(dlcversion)));
			}
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0002FEB8 File Offset: 0x0002E0B8
	private static bool IsDLCAvailableReal(DLCVersion dlcVersion)
	{
		bool flag = true;
		if (DLCEngine.dlcInfoByVersion.ContainsKey(dlcVersion))
		{
			DLCEngine.SetDLCStates();
			bool flag2;
			flag = DLCEngine.dlcStateByVersion.TryGetValue(dlcVersion, out flag2) && flag2;
		}
		return flag;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0002FEEC File Offset: 0x0002E0EC
	private static void SetDLCStates()
	{
		if (!DLCEngine.isDLCStateCached)
		{
			foreach (KeyValuePair<DLCVersion, DLCInfo> keyValuePair in DLCEngine.dlcInfoByVersion)
			{
				DLCEngine.dlcStateByVersion[keyValuePair.Key] = LazyAPI.Platform.IsDLCAvailable(keyValuePair.Value);
			}
			DLCEngine.isDLCStateCached = true;
			DLCEngine.LogDLCStates();
		}
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x0002FF6C File Offset: 0x0002E16C
	public static void ResetDLCStateCached()
	{
		DLCEngine.isDLCStateCached = false;
		DLCEngine.dlcStateByVersion.Clear();
		DLCEngine.LogDLCStates();
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x0002FF84 File Offset: 0x0002E184
	public static void OpenDlcInStore(DLCVersion dlcVersion)
	{
		StoreProductInfo storeProductInfo;
		if (!DLCEngine.storeProductInfoByVersion.TryGetValue(dlcVersion, out storeProductInfo))
		{
			Debug.LogError(string.Format("There is no store product info for DLC [{0}]", dlcVersion));
			return;
		}
		LazyAPI.Platform.OpenProductInStore(storeProductInfo);
		LazyAudio.PlayAndForget("gui_click");
	}

	// Token: 0x04000B04 RID: 2820
	private static readonly Dictionary<DLCVersion, DLCInfo> dlcInfoByVersion = new Dictionary<DLCVersion, DLCInfo> { 
	{
		DLCVersion.Preorder,
		new DLCInfo
		{
			steamAppId = 5021030U,
			gogAppId = 0UL,
			epicGamesAppId = "",
			dlcIndex = 1,
			xboxAppId = "9N9SDNT9112W",
			ps4EntitlementLabel = "GK2PREORDERDLC00",
			ps5EntitlementLabel = "GK2PREORDERDLC00"
		}
	} };

	// Token: 0x04000B05 RID: 2821
	private static readonly Dictionary<DLCVersion, StoreProductInfo> storeProductInfoByVersion = new Dictionary<DLCVersion, StoreProductInfo> { 
	{
		DLCVersion.Preorder,
		new StoreProductInfo
		{
			steamUrl = "steam://advertise/5021030",
			epicGamesUrl = "",
			gogUrl = "",
			xboxProductId = "9N9SDNT9112W",
			nintendoApplicationId = "0100005027a18000",
			nintendo2ApplicationId = "0400543027ffc000",
			ps4 = new PlayStationStoreProductInfo
			{
				productLabel = "GK2PREORDERDLC00",
				serviceLabel = 0U,
				service = PlayStationStoreService.PlayStationStoreDeliveredContent
			},
			ps5 = new PlayStationStoreProductInfo
			{
				productLabel = "GK2PREORDERDLC00",
				serviceLabel = 0U,
				service = PlayStationStoreService.PlayStationStoreDeliveredContent
			}
		}
	} };

	// Token: 0x04000B06 RID: 2822
	private static readonly Dictionary<DLCVersion, bool> dlcStateByVersion = new Dictionary<DLCVersion, bool>();

	// Token: 0x04000B07 RID: 2823
	private static bool isDLCStateCached;
}
