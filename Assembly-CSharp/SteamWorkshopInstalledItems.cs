using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;
using Steamworks;
using UnityEngine;

// Token: 0x020007AF RID: 1967
public static class SteamWorkshopInstalledItems
{
	// Token: 0x170007AE RID: 1966
	// (get) Token: 0x0600327C RID: 12924 RVA: 0x000F27F3 File Offset: 0x000F09F3
	// (set) Token: 0x0600327D RID: 12925 RVA: 0x000F27FA File Offset: 0x000F09FA
	public static bool PendingRescan { get; set; }

	// Token: 0x0600327E RID: 12926 RVA: 0x000F2804 File Offset: 0x000F0A04
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatics()
	{
		SteamWorkshopInstalledItems.downloadCallback = null;
		SteamWorkshopInstalledItems.installedCallback = null;
		SteamWorkshopInstalledItems.queryResult = null;
		SteamWorkshopInstalledItems.subscribeResult = null;
		SteamWorkshopInstalledItems.downloadInFlight.Clear();
		SteamWorkshopInstalledItems.subscribeQueued.Clear();
		SteamWorkshopInstalledItems.subscribeAttempted.Clear();
		SteamWorkshopInstalledItems.ignoredIds.Clear();
		SteamWorkshopInstalledItems.subscribeQueue.Clear();
		SteamWorkshopInstalledItems.nextRetryUnscaledTime.Clear();
		SteamWorkshopInstalledItems.lastEnsureUnscaledTime = -100f;
		SteamWorkshopInstalledItems.detailsQueried = false;
		SteamWorkshopInstalledItems.detailsReady = false;
		SteamWorkshopInstalledItems.loggedAppInstall = false;
		SteamWorkshopInstalledItems.subscribeCallInFlight = false;
		SteamWorkshopInstalledItems.PendingRescan = false;
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x000F288D File Offset: 0x000F0A8D
	public static bool IsSteamReady()
	{
		return Time.frameCount > 0 && SteamManager.Initialized;
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x000F28A0 File Offset: 0x000F0AA0
	public static void Tick()
	{
		if (!SteamWorkshopInstalledItems.IsSteamReady())
		{
			return;
		}
		if (SteamWorkshopInstalledItems.downloadCallback == null)
		{
			SteamWorkshopInstalledItems.downloadCallback = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(SteamWorkshopInstalledItems.OnDownloadItem));
		}
		if (SteamWorkshopInstalledItems.installedCallback == null)
		{
			SteamWorkshopInstalledItems.installedCallback = Callback<ItemInstalled_t>.Create(new Callback<ItemInstalled_t>.DispatchDelegate(SteamWorkshopInstalledItems.OnItemInstalled));
		}
		if (SteamWorkshopInstalledItems.queryResult == null)
		{
			SteamWorkshopInstalledItems.queryResult = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(SteamWorkshopInstalledItems.OnQueryCompleted));
		}
		if (SteamWorkshopInstalledItems.subscribeResult == null)
		{
			SteamWorkshopInstalledItems.subscribeResult = CallResult<RemoteStorageSubscribePublishedFileResult_t>.Create(new CallResult<RemoteStorageSubscribePublishedFileResult_t>.APIDispatchDelegate(SteamWorkshopInstalledItems.OnSubscribed));
		}
		if (Time.unscaledTime - SteamWorkshopInstalledItems.lastEnsureUnscaledTime < 2f)
		{
			return;
		}
		SteamWorkshopInstalledItems.lastEnsureUnscaledTime = Time.unscaledTime;
		if (!SteamWorkshopInstalledItems.loggedAppInstall)
		{
			SteamWorkshopInstalledItems.LogAppInstall();
		}
		SteamWorkshopInstalledItems.EnsureDownloads();
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x000F2958 File Offset: 0x000F0B58
	public static IReadOnlyList<string> GetInstalledFolders()
	{
		List<string> list = new List<string>();
		if (!SteamWorkshopInstalledItems.IsSteamReady())
		{
			return list;
		}
		uint numSubscribedItems = SteamUGC.GetNumSubscribedItems(false);
		if (numSubscribedItems == 0U)
		{
			return list;
		}
		PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
		uint subscribedItems = SteamUGC.GetSubscribedItems(array, numSubscribedItems, false);
		for (uint num = 0U; num < subscribedItems; num += 1U)
		{
			PublishedFileId_t publishedFileId_t = array[(int)num];
			if (!SteamWorkshopInstalledItems.ignoredIds.Contains(publishedFileId_t.m_PublishedFileId) && (SteamUGC.GetItemState(publishedFileId_t) & 2U) == 0U)
			{
				ulong num2;
				string text;
				uint num3;
				if (SteamUGC.GetItemInstallInfo(publishedFileId_t, out num2, out text, 4096U, out num3) && !string.IsNullOrEmpty(text) && Directory.Exists(text))
				{
					SteamWorkshopInstalledItems.AddFolder(list, text);
				}
				else
				{
					SteamWorkshopInstalledItems.AddFolder(list, SteamWorkshopInstalledItems.FindWorkshopContentFolder(publishedFileId_t.m_PublishedFileId));
				}
			}
		}
		return list;
	}

	// Token: 0x06003282 RID: 12930 RVA: 0x000F2A0C File Offset: 0x000F0C0C
	private static void EnsureDownloads()
	{
		uint numSubscribedItems = SteamUGC.GetNumSubscribedItems(false);
		if (numSubscribedItems == 0U)
		{
			return;
		}
		PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
		uint subscribedItems = SteamUGC.GetSubscribedItems(array, numSubscribedItems, false);
		if (!SteamWorkshopInstalledItems.detailsQueried)
		{
			SteamWorkshopInstalledItems.detailsQueried = true;
			SteamWorkshopInstalledItems.QueryDetails(array, subscribedItems);
			return;
		}
		if (!SteamWorkshopInstalledItems.detailsReady)
		{
			return;
		}
		for (uint num = 0U; num < subscribedItems; num += 1U)
		{
			PublishedFileId_t publishedFileId_t = array[(int)num];
			ulong publishedFileId = publishedFileId_t.m_PublishedFileId;
			if (!SteamWorkshopInstalledItems.ignoredIds.Contains(publishedFileId))
			{
				uint itemState = SteamUGC.GetItemState(publishedFileId_t);
				bool flag = (itemState & 1U) > 0U;
				bool flag2 = (itemState & 4U) > 0U;
				bool flag3 = (itemState & 8U) > 0U;
				bool flag4 = (itemState & 16U) > 0U;
				bool flag5 = (itemState & 32U) > 0U;
				float num2;
				if ((!flag2 || flag3) && (!flag4 && !flag5) && !SteamWorkshopInstalledItems.downloadInFlight.Contains(publishedFileId) && (!SteamWorkshopInstalledItems.nextRetryUnscaledTime.TryGetValue(publishedFileId, out num2) || Time.unscaledTime >= num2))
				{
					if (!flag && !SteamWorkshopInstalledItems.subscribeAttempted.Contains(publishedFileId))
					{
						SteamWorkshopInstalledItems.EnqueueSubscribe(publishedFileId_t);
					}
					else
					{
						SteamWorkshopInstalledItems.QueueDownload(publishedFileId_t, itemState);
					}
				}
			}
		}
		SteamWorkshopInstalledItems.PumpSubscribeQueue();
	}

	// Token: 0x06003283 RID: 12931 RVA: 0x000F2B1C File Offset: 0x000F0D1C
	private static void EnqueueSubscribe(PublishedFileId_t id)
	{
		ulong publishedFileId = id.m_PublishedFileId;
		if (SteamWorkshopInstalledItems.subscribeQueued.Contains(publishedFileId) || SteamWorkshopInstalledItems.subscribeAttempted.Contains(publishedFileId))
		{
			return;
		}
		SteamWorkshopInstalledItems.subscribeQueued.Add(publishedFileId);
		SteamWorkshopInstalledItems.subscribeQueue.Enqueue(id);
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x000F2B64 File Offset: 0x000F0D64
	private static void PumpSubscribeQueue()
	{
		if (SteamWorkshopInstalledItems.subscribeCallInFlight || SteamWorkshopInstalledItems.subscribeQueue.Count == 0)
		{
			return;
		}
		PublishedFileId_t publishedFileId_t = SteamWorkshopInstalledItems.subscribeQueue.Dequeue();
		ulong publishedFileId = publishedFileId_t.m_PublishedFileId;
		SteamWorkshopInstalledItems.subscribeQueued.Remove(publishedFileId);
		SteamAPICall_t steamAPICall_t = SteamUGC.SubscribeItem(publishedFileId_t);
		if (steamAPICall_t == SteamAPICall_t.Invalid)
		{
			SteamWorkshopInstalledItems.nextRetryUnscaledTime[publishedFileId] = Time.unscaledTime + 60f;
			Debug.Log(string.Format("[SteamWorkshop] SubscribeItem returned invalid call for {0}. Retry in {1:0}s.", publishedFileId, 60f));
			return;
		}
		SteamWorkshopInstalledItems.subscribeCallInFlight = true;
		SteamWorkshopInstalledItems.subscribeAttempted.Add(publishedFileId);
		SteamWorkshopInstalledItems.subscribeResult.Set(steamAPICall_t, null);
		uint itemState = SteamUGC.GetItemState(publishedFileId_t);
		Debug.Log(string.Format("[SteamWorkshop] SubscribeItem {0} state={1}.", publishedFileId, SteamWorkshopInstalledItems.FormatState(itemState)));
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x000F2C30 File Offset: 0x000F0E30
	private static void QueueDownload(PublishedFileId_t id, uint state)
	{
		ulong publishedFileId = id.m_PublishedFileId;
		if (SteamUGC.DownloadItem(id, true))
		{
			SteamWorkshopInstalledItems.downloadInFlight.Add(publishedFileId);
			ulong num;
			ulong num2;
			SteamUGC.GetItemDownloadInfo(id, out num, out num2);
			Debug.Log(string.Format("[SteamWorkshop] Queued download for {0} state={1} downloaded={2}/{3}.", new object[]
			{
				publishedFileId,
				SteamWorkshopInstalledItems.FormatState(state),
				num,
				num2
			}));
			return;
		}
		SteamWorkshopInstalledItems.nextRetryUnscaledTime[publishedFileId] = Time.unscaledTime + 60f;
		Debug.Log(string.Format("[SteamWorkshop] DownloadItem returned false for {0} state={1}. Retry in {2:0}s.", publishedFileId, SteamWorkshopInstalledItems.FormatState(state), 60f));
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x000F2CDC File Offset: 0x000F0EDC
	private static void QueryDetails(PublishedFileId_t[] ids, uint count)
	{
		if (count == 0U)
		{
			return;
		}
		UGCQueryHandle_t ugcqueryHandle_t = SteamUGC.CreateQueryUGCDetailsRequest(ids, count);
		if (ugcqueryHandle_t == UGCQueryHandle_t.Invalid)
		{
			return;
		}
		SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(ugcqueryHandle_t);
		SteamWorkshopInstalledItems.queryResult.Set(steamAPICall_t, null);
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x000F2D18 File Offset: 0x000F0F18
	private static void OnQueryCompleted(SteamUGCQueryCompleted_t result, bool ioFailure)
	{
		SteamWorkshopInstalledItems.detailsReady = true;
		if (ioFailure || result.m_eResult != EResult.k_EResultOK)
		{
			Debug.Log("[SteamWorkshop] UGC details query failed: " + (ioFailure ? "IO failure" : result.m_eResult.ToString()));
			return;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[SteamWorkshop] Item details:");
		for (uint num = 0U; num < result.m_unNumResultsReturned; num += 1U)
		{
			SteamUGCDetails_t steamUGCDetails_t;
			if (SteamUGC.GetQueryUGCResult(result.m_handle, num, out steamUGCDetails_t))
			{
				ulong publishedFileId = steamUGCDetails_t.m_nPublishedFileId.m_PublishedFileId;
				stringBuilder.Append("\n  id=").Append(publishedFileId);
				stringBuilder.Append(" title='").Append(steamUGCDetails_t.m_rgchTitle).Append("'");
				stringBuilder.Append(" fileSize=").Append(steamUGCDetails_t.m_nFileSize);
				stringBuilder.Append(" totalFilesSize=").Append(steamUGCDetails_t.m_ulTotalFilesSize);
				stringBuilder.Append(" visibility=").Append(steamUGCDetails_t.m_eVisibility);
				stringBuilder.Append(" result=").Append(steamUGCDetails_t.m_eResult);
				if (steamUGCDetails_t.m_eResult == EResult.k_EResultFileNotFound)
				{
					SteamWorkshopInstalledItems.ignoredIds.Add(publishedFileId);
					SteamUGC.UnsubscribeItem(steamUGCDetails_t.m_nPublishedFileId);
					stringBuilder.Append(" (dropped)");
				}
			}
		}
		SteamUGC.ReleaseQueryUGCRequest(result.m_handle);
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x000F2E8C File Offset: 0x000F108C
	private static void OnSubscribed(RemoteStorageSubscribePublishedFileResult_t result, bool ioFailure)
	{
		SteamWorkshopInstalledItems.subscribeCallInFlight = false;
		ulong publishedFileId = result.m_nPublishedFileId.m_PublishedFileId;
		uint itemState = SteamUGC.GetItemState(result.m_nPublishedFileId);
		if (!ioFailure && result.m_eResult == EResult.k_EResultFileNotFound)
		{
			SteamWorkshopInstalledItems.ignoredIds.Add(publishedFileId);
			SteamUGC.UnsubscribeItem(result.m_nPublishedFileId);
			Debug.Log(string.Format("[SteamWorkshop] SubscribeItem FileNotFound id={0}. Dropping (deleted or never published).", publishedFileId));
			SteamWorkshopInstalledItems.PumpSubscribeQueue();
			return;
		}
		if (ioFailure || result.m_eResult != EResult.k_EResultOK)
		{
			SteamWorkshopInstalledItems.nextRetryUnscaledTime[publishedFileId] = Time.unscaledTime + 60f;
			SteamWorkshopInstalledItems.subscribeAttempted.Remove(publishedFileId);
			Debug.Log(string.Format("[SteamWorkshop] SubscribeItem failed id={0} result={1} state={2}. Retry in {3:0}s.", new object[]
			{
				publishedFileId,
				ioFailure ? "IO failure" : result.m_eResult.ToString(),
				SteamWorkshopInstalledItems.FormatState(itemState),
				60f
			}));
			SteamWorkshopInstalledItems.PumpSubscribeQueue();
			return;
		}
		Debug.Log(string.Format("[SteamWorkshop] SubscribeItem ok id={0} state={1}.", publishedFileId, SteamWorkshopInstalledItems.FormatState(itemState)));
		SteamWorkshopInstalledItems.QueueDownload(result.m_nPublishedFileId, itemState);
		SteamWorkshopInstalledItems.PumpSubscribeQueue();
	}

	// Token: 0x06003289 RID: 12937 RVA: 0x000F2FAC File Offset: 0x000F11AC
	private static void LogAppInstall()
	{
		SteamWorkshopInstalledItems.loggedAppInstall = true;
		AppId_t appID = SteamUtils.GetAppID();
		bool flag = SteamApps.BIsAppInstalled(appID);
		bool flag2 = SteamApps.BIsSubscribedApp(appID);
		string text;
		uint appInstallDir = SteamApps.GetAppInstallDir(appID, out text, 1024U);
		Debug.Log(string.Format("[SteamWorkshop] App {0} BIsAppInstalled={1} BIsSubscribedApp={2} ", appID, flag, flag2) + "installDir='" + ((appInstallDir > 0U) ? text : "") + "'.");
	}

	// Token: 0x0600328A RID: 12938 RVA: 0x000F3020 File Offset: 0x000F1220
	private static void OnDownloadItem(DownloadItemResult_t result)
	{
		ulong publishedFileId = result.m_nPublishedFileId.m_PublishedFileId;
		SteamWorkshopInstalledItems.downloadInFlight.Remove(publishedFileId);
		uint itemState = SteamUGC.GetItemState(result.m_nPublishedFileId);
		AppId_t appID = SteamUtils.GetAppID();
		if (result.m_eResult == EResult.k_EResultOK)
		{
			Debug.Log(string.Format("[SteamWorkshop] Download finished id={0} appId={1} localAppId={2} state={3}.", new object[]
			{
				publishedFileId,
				result.m_unAppID,
				appID,
				SteamWorkshopInstalledItems.FormatState(itemState)
			}));
			SteamWorkshopInstalledItems.PendingRescan = true;
			return;
		}
		SteamWorkshopInstalledItems.nextRetryUnscaledTime[publishedFileId] = Time.unscaledTime + 60f;
		Debug.Log(string.Format("[SteamWorkshop] Download failed id={0} result={1} appId={2} localAppId={3} state={4}. ", new object[]
		{
			publishedFileId,
			result.m_eResult,
			result.m_unAppID,
			appID,
			SteamWorkshopInstalledItems.FormatState(itemState)
		}) + "If state lacks Subscribed, the Steam client is skipping the item: check Steam/logs/workshop_log.txt for 'No workshop depot defined' (Steamworks App Admin > Workshop > General > Workshop Depot, then Publish and restart Steam). " + string.Format("Retry in {0:0}s.", 60f));
	}

	// Token: 0x0600328B RID: 12939 RVA: 0x000F3125 File Offset: 0x000F1325
	private static void OnItemInstalled(ItemInstalled_t result)
	{
		if (result.m_unAppID != SteamUtils.GetAppID())
		{
			return;
		}
		Debug.Log(string.Format("[SteamWorkshop] ItemInstalled id={0}.", result.m_nPublishedFileId.m_PublishedFileId));
		SteamWorkshopInstalledItems.PendingRescan = true;
	}

	// Token: 0x0600328C RID: 12940 RVA: 0x000F3160 File Offset: 0x000F1360
	private static string FormatState(uint state)
	{
		if (state == 0U)
		{
			return "None";
		}
		List<string> list = new List<string>();
		if ((state & 1U) != 0U)
		{
			list.Add("Subscribed");
		}
		if ((state & 2U) != 0U)
		{
			list.Add("Legacy");
		}
		if ((state & 4U) != 0U)
		{
			list.Add("Installed");
		}
		if ((state & 8U) != 0U)
		{
			list.Add("NeedsUpdate");
		}
		if ((state & 16U) != 0U)
		{
			list.Add("Downloading");
		}
		if ((state & 32U) != 0U)
		{
			list.Add("DownloadPending");
		}
		if ((state & 64U) != 0U)
		{
			list.Add("DisabledLocally");
		}
		return string.Format("{0}({1})", state, string.Join("|", list));
	}

	// Token: 0x0600328D RID: 12941 RVA: 0x000F320C File Offset: 0x000F140C
	private static string FindWorkshopContentFolder(ulong publishedFileId)
	{
		string text = SteamUtils.GetAppID().m_AppId.ToString();
		string text2 = publishedFileId.ToString();
		List<string> steamLibraryPaths = SteamWorkshopInstalledItems.GetSteamLibraryPaths();
		for (int i = 0; i < steamLibraryPaths.Count; i++)
		{
			string text3 = Path.Combine(new string[]
			{
				steamLibraryPaths[i],
				"steamapps",
				"workshop",
				"content",
				text,
				text2
			});
			if (Directory.Exists(text3))
			{
				return text3;
			}
		}
		return null;
	}

	// Token: 0x0600328E RID: 12942 RVA: 0x000F3294 File Offset: 0x000F1494
	private static List<string> GetSteamLibraryPaths()
	{
		List<string> list = new List<string>();
		string steamInstallPath = SteamWorkshopInstalledItems.GetSteamInstallPath();
		if (string.IsNullOrEmpty(steamInstallPath))
		{
			return list;
		}
		SteamWorkshopInstalledItems.AddFolder(list, steamInstallPath);
		string text = Path.Combine(steamInstallPath, "steamapps", "libraryfolders.vdf");
		if (!File.Exists(text))
		{
			return list;
		}
		try
		{
			foreach (string text2 in File.ReadAllLines(text))
			{
				int num = text2.IndexOf("\"path\"", StringComparison.OrdinalIgnoreCase);
				if (num >= 0)
				{
					int num2 = text2.IndexOf('"', num + 6);
					if (num2 >= 0)
					{
						int num3 = text2.IndexOf('"', num2 + 1);
						if (num3 >= 0)
						{
							SteamWorkshopInstalledItems.AddFolder(list, text2.Substring(num2 + 1, num3 - num2 - 1).Replace("\\\\", "\\"));
						}
					}
				}
			}
		}
		catch
		{
		}
		return list;
	}

	// Token: 0x0600328F RID: 12943 RVA: 0x000F3374 File Offset: 0x000F1574
	private static string GetSteamInstallPath()
	{
		try
		{
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam"))
			{
				string text = ((registryKey != null) ? registryKey.GetValue("SteamPath") : null) as string;
				if (text != null && !string.IsNullOrWhiteSpace(text))
				{
					return text.Replace('/', Path.DirectorySeparatorChar);
				}
			}
		}
		catch
		{
		}
		return null;
	}

	// Token: 0x06003290 RID: 12944 RVA: 0x000F33F4 File Offset: 0x000F15F4
	private static void AddFolder(List<string> folders, string path)
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
		for (int i = 0; i < folders.Count; i++)
		{
			if (string.Equals(folders[i], path, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
		}
		folders.Add(path);
	}

	// Token: 0x0400287C RID: 10364
	private const uint InstallFolderBufferBytes = 4096U;

	// Token: 0x0400287D RID: 10365
	private const float DownloadRetrySeconds = 60f;

	// Token: 0x0400287E RID: 10366
	private const float EnsureIntervalSeconds = 2f;

	// Token: 0x04002880 RID: 10368
	private static Callback<DownloadItemResult_t> downloadCallback;

	// Token: 0x04002881 RID: 10369
	private static Callback<ItemInstalled_t> installedCallback;

	// Token: 0x04002882 RID: 10370
	private static CallResult<SteamUGCQueryCompleted_t> queryResult;

	// Token: 0x04002883 RID: 10371
	private static CallResult<RemoteStorageSubscribePublishedFileResult_t> subscribeResult;

	// Token: 0x04002884 RID: 10372
	private static readonly HashSet<ulong> downloadInFlight = new HashSet<ulong>();

	// Token: 0x04002885 RID: 10373
	private static readonly HashSet<ulong> subscribeQueued = new HashSet<ulong>();

	// Token: 0x04002886 RID: 10374
	private static readonly HashSet<ulong> subscribeAttempted = new HashSet<ulong>();

	// Token: 0x04002887 RID: 10375
	private static readonly HashSet<ulong> ignoredIds = new HashSet<ulong>();

	// Token: 0x04002888 RID: 10376
	private static readonly Queue<PublishedFileId_t> subscribeQueue = new Queue<PublishedFileId_t>();

	// Token: 0x04002889 RID: 10377
	private static readonly Dictionary<ulong, float> nextRetryUnscaledTime = new Dictionary<ulong, float>();

	// Token: 0x0400288A RID: 10378
	private static float lastEnsureUnscaledTime = -100f;

	// Token: 0x0400288B RID: 10379
	private static bool detailsQueried;

	// Token: 0x0400288C RID: 10380
	private static bool detailsReady;

	// Token: 0x0400288D RID: 10381
	private static bool loggedAppInstall;

	// Token: 0x0400288E RID: 10382
	private static bool subscribeCallInFlight;
}
