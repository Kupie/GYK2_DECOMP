using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace LazyBearTechnology.CloudSync
{
	// Token: 0x020001A3 RID: 419
	public class LazyCloudSync : MonoBehaviour
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600097F RID: 2431 RVA: 0x0002D378 File Offset: 0x0002B578
		public static LazyCloudSync.State CurrentState
		{
			get
			{
				if (!(LazyCloudSync.instance == null))
				{
					return LazyCloudSync.instance.currentState;
				}
				return LazyCloudSync.State.NotInited;
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0002D393 File Offset: 0x0002B593
		public static void Init(ILazyCloudSync saver)
		{
			LazyCloudSync.Init<LazyCloudSyncSettingsPlayerPrefs>(saver);
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0002D39C File Offset: 0x0002B59C
		public static void Init<T>(ILazyCloudSync saver) where T : ILazyCloudSyncSettings, new()
		{
			if (LazyCloudSync.initialized)
			{
				Debug.LogWarning("Trying to call LBCloudSync.Init() for the 2nd time. Skipping.");
				return;
			}
			LazyCloudSync.instance = new GameObject("LBCloudSync").AddComponent<LazyCloudSync>();
			LazyCloudSync.initialized = true;
			LazyCloudSync.instance.settingsInterface = new T();
			LazyCloudSync.instance.syncInterface = saver;
			LazyCloudSync.instance.currentState = (LazyCloudSync.IsRegisteredInCloud() ? LazyCloudSync.State.Syncing : LazyCloudSync.State.NotSetUp);
			LazyCloudSync.Connect();
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0002D40E File Offset: 0x0002B60E
		public static void Connect()
		{
			LazyCloudSync.instance.DoRequest("ping", null);
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0002D420 File Offset: 0x0002B620
		public static void OnMainButtonClicked()
		{
			if (LazyCloudSync.instance.processingCoroutine)
			{
				Debug.LogWarning("LBCloudSync.OnMainButtonClicked() is ignored while other request is in progress.");
				return;
			}
			LazyCloudSync.State state = LazyCloudSync.instance.currentState;
			if (state - LazyCloudSync.State.NotSetUp > 1)
			{
				if (state != LazyCloudSync.State.Offline)
				{
					Debug.LogWarning("Wrong state = " + LazyCloudSync.instance.currentState.ToString());
					return;
				}
				LazyCloudSync.Connect();
				return;
			}
			else
			{
				if (LazyCloudSync.IsRegisteredInCloud())
				{
					LazyCloudSync.instance.syncInterface.ShowCloudSyncDialogWithBreakSyncButton();
					return;
				}
				LazyCloudSync.instance.syncInterface.ShowCloudSyncDialogWithEnterCodeButton();
				return;
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0002D4AC File Offset: 0x0002B6AC
		public static void StartSync()
		{
			if (!LazyCloudSync.initialized)
			{
				Debug.LogError("LB Cloud Sync error: library was not initializaed. Call Init() method first.");
				return;
			}
			if (LazyCloudSync.DoIHaveActiveSyncCode())
			{
				LazyCloudSync.instance.syncInterface.ShowCloudSyncDialogWithCode(LazyCloudSync.instance.code);
				return;
			}
			LazyCloudSync.instance.DoRequest("syncinit", null);
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0002D4FC File Offset: 0x0002B6FC
		public static void BreakSync()
		{
			Debug.Log("LB Cloud Sync: Break sync");
			LazyCloudSync.instance.currentState = LazyCloudSync.State.NotSetUp;
			LazyCloudSync.instance.settingsInterface.DeleteValue("LBCloud_id");
			LazyCloudSync.instance.settingsInterface.DeleteValue("LBCloud_pass");
			LazyCloudSync.instance.settingsInterface.DeleteValue("LBCloud_code");
			LazyCloudSync.instance.settingsInterface.DeleteValue("LBCloud_time");
			LazyCloudSync.instance.settingsInterface.DeleteValue("LBCloud_was_synced");
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0002D582 File Offset: 0x0002B782
		public static void ProcessCloudCode(string code)
		{
			LazyCloudSync.instance.DoRequest("linkme", new Dictionary<string, string> { { "code", code } });
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0002D5A4 File Offset: 0x0002B7A4
		private void DoRequest(string command, Dictionary<string, string> additionalFields = null)
		{
			Debug.Log("DoRequest " + command);
			if (this.processingCoroutine)
			{
				this.onCoroutineFinished.Enqueue(() => this.DoRequestCoroutine(command, additionalFields));
				return;
			}
			this.processingCoroutine = true;
			this.onCoroutineFinished.Clear();
			base.StartCoroutine(this.DoRequestCoroutine(command, additionalFields));
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0002D62C File Offset: 0x0002B82C
		private IEnumerator DoRequestCoroutine(string command, Dictionary<string, string> additionalFields = null)
		{
			Debug.Log("<color=cyan>LB Cloud command:</color> " + command);
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("ver", this.syncInterface.GetApplicationVersion());
			if (!this.settingsInterface.HasValue("LBCloud_clientid"))
			{
				this.GenerateNewCloudClientID();
			}
			if (command == "sync" || command == "syncinit")
			{
				LazyCloudSyncSaveData savegameData = this.syncInterface.GetSavegameData();
				wwwform.AddField("save", savegameData.CombinedBody);
				wwwform.AddField("stats", savegameData.header);
				if (command == "sync")
				{
					wwwform.AddField("was_synced", this.settingsInterface.GetValue("LBCloud_was_synced"));
				}
			}
			wwwform.AddField("client_id", this.settingsInterface.GetValue("LBCloud_clientid"));
			wwwform.AddField("server_id", this.settingsInterface.GetValue("LBCloud_id"));
			wwwform.AddField("password", this.settingsInterface.GetValue("LBCloud_pass"));
			wwwform.AddField("platform", Application.platform.ToString());
			if (additionalFields != null)
			{
				foreach (KeyValuePair<string, string> keyValuePair in additionalFields)
				{
					wwwform.AddField(keyValuePair.Key, keyValuePair.Value);
				}
			}
			UnityWebRequest webRequest = UnityWebRequest.Post("http://s2.lazybeargames.com/cloud/" + command, wwwform);
			yield return webRequest;
			while (!webRequest.isDone)
			{
				yield return new WaitForSecondsRealtime(0.1f);
			}
			Debug.Log(string.Format("<color=cyan>LB Cloud:</color> {0} request done", command));
			Debug.Log(webRequest.downloadHandler.text);
			LazyCloudSyncServerReply lazyCloudSyncServerReply;
			try
			{
				lazyCloudSyncServerReply = JsonUtility.FromJson<LazyCloudSyncServerReply>(webRequest.downloadHandler.text);
			}
			catch (Exception)
			{
				lazyCloudSyncServerReply = null;
			}
			if (lazyCloudSyncServerReply == null)
			{
				Debug.LogError("LB Cloud error parsing JSON result");
			}
			else if (lazyCloudSyncServerReply.CloudResult == LazyCloudSync.CloudResult.AuthenticationError)
			{
				LazyCloudSync.BreakSync();
			}
			else if (!(command == "ping"))
			{
				if (!(command == "syncinit"))
				{
					if (!(command == "linkme"))
					{
						if (!(command == "sync"))
						{
							if (!(command == "change_client_id"))
							{
								Debug.LogError("Unknown command: " + command);
							}
							else
							{
								yield return this.ProcessResultChangeClientID(lazyCloudSyncServerReply);
							}
						}
						else
						{
							yield return this.ProcessResultSync(lazyCloudSyncServerReply);
						}
					}
					else
					{
						yield return this.ProcessResultLinkMe(lazyCloudSyncServerReply);
					}
				}
				else
				{
					yield return this.ProcessResultSyncInit(lazyCloudSyncServerReply);
				}
			}
			else
			{
				yield return this.ProcessResultPing(lazyCloudSyncServerReply);
			}
			Debug.Log("_on_coroutine_finished.Count = " + this.onCoroutineFinished.Count.ToString());
			if (this.onCoroutineFinished.Count == 0)
			{
				this.processingCoroutine = false;
			}
			else
			{
				LazyCloudSync.EnumeratorDelegate enumeratorDelegate = this.onCoroutineFinished.Dequeue();
				yield return enumeratorDelegate();
			}
			yield break;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0002D649 File Offset: 0x0002B849
		private IEnumerator ProcessResultPing(LazyCloudSyncServerReply result)
		{
			if (this.currentState == LazyCloudSync.State.Syncing)
			{
				if (LazyCloudSync.DoIHaveActiveSyncCode() && !result.IsAuthenicated)
				{
					this.currentState = LazyCloudSync.State.NotSetUp;
					LazyCloudSync.BreakSync();
				}
				else
				{
					this.DoSyncRequest(false);
				}
			}
			yield return 0;
			yield break;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0002D65F File Offset: 0x0002B85F
		private IEnumerator ProcessResultLinkMe(LazyCloudSyncServerReply result)
		{
			LazyCloudSync.CloudResult cloudResult = result.CloudResult;
			if (cloudResult != LazyCloudSync.CloudResult.OK)
			{
				if (cloudResult - LazyCloudSync.CloudResult.WrongSyncCode <= 1)
				{
					this.syncInterface.ShowCloudSyncErrorMessage(result.CloudResult);
				}
			}
			else
			{
				this.settingsInterface.SetValue("LBCloud_id", result.serverId);
				this.settingsInterface.SetValue("LBCloud_pass", result.password);
				this.SetWasSynced(false);
				this.DoSyncRequest(false);
			}
			yield return 0;
			yield break;
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0002D675 File Offset: 0x0002B875
		private void DoSyncRequest(bool force = false)
		{
			string text = "sync";
			object obj;
			if (!force)
			{
				obj = null;
			}
			else
			{
				(obj = new Dictionary<string, string>()).Add("force", "1");
			}
			this.DoRequest(text, obj);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0002D69D File Offset: 0x0002B89D
		private IEnumerator ProcessResultSyncInit(LazyCloudSyncServerReply result)
		{
			if (result.IsError)
			{
				Debug.LogError("Sync error: " + result.result);
				yield break;
			}
			this.settingsInterface.SetValue("LBCloud_id", result.serverId);
			this.settingsInterface.SetValue("LBCloud_pass", result.password);
			this.time = LazyCloudSync.GetDateTicksInSeconds() + result.lifetime;
			this.code = result.code;
			this.settingsInterface.SetValue("LBCloud_code", this.code);
			this.settingsInterface.SetValue("LBCloud_time", this.time.ToString());
			this.SetWasSynced(true);
			this.syncInterface.ShowCloudSyncDialogWithCode(this.code);
			yield break;
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0002D6B3 File Offset: 0x0002B8B3
		private IEnumerator ProcessResultSync(LazyCloudSyncServerReply result)
		{
			LazyCloudSync.<>c__DisplayClass28_0 CS$<>8__locals1 = new LazyCloudSync.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			string result2 = result.result;
			UnityWebRequest webRequest;
			if (!(result2 == "save"))
			{
				if (!(result2 == "ok"))
				{
					this.SetWasSynced(false);
				}
				else
				{
					this.currentState = LazyCloudSync.State.Connected;
					this.SetWasSynced(true);
				}
			}
			else
			{
				webRequest = UnityWebRequest.Get(result.file);
				yield return webRequest;
				if (!string.IsNullOrEmpty(webRequest.error))
				{
					Debug.LogError("LB Cloud sync: Error downloading webRequest: " + webRequest.error);
					this.syncInterface.ShowCloudSyncErrorMessage(LazyCloudSync.CloudResult.ErrorDowloadingFile);
					yield break;
				}
				CS$<>8__locals1.remoteSave = new LazyCloudSyncSaveData(webRequest.downloadHandler.text);
				if (result.DoOverwrite)
				{
					this.OverwriteWithRemoteSave(CS$<>8__locals1.remoteSave);
					this.SetWasSynced(true);
					this.DoRequest("change_client_id", null);
				}
				else
				{
					this.syncInterface.ShowCloudSyncSaveChooserDialog(this.syncInterface.GetSavegameData(), delegate
					{
						this.DoSyncRequest(true);
					}, CS$<>8__locals1.remoteSave, delegate
					{
						CS$<>8__locals1.<>4__this.OverwriteWithRemoteSave(CS$<>8__locals1.remoteSave);
						CS$<>8__locals1.<>4__this.DoRequest("change_client_id", null);
					});
				}
			}
			CS$<>8__locals1 = null;
			webRequest = null;
			yield break;
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0002D6C9 File Offset: 0x0002B8C9
		private IEnumerator ProcessResultChangeClientID(LazyCloudSyncServerReply result)
		{
			if (!result.IsResultOK)
			{
				Debug.LogError("LB Cloud Sync change_client_id error: " + result.result);
				yield break;
			}
			this.currentState = LazyCloudSync.State.Connected;
			yield break;
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x0002D6E0 File Offset: 0x0002B8E0
		private void GenerateNewCloudClientID()
		{
			string text = global::UnityEngine.Random.Range(0, 999999999).ToString() + "_" + LazyCloudSync.GetDateTicksInSeconds().ToString();
			this.settingsInterface.SetValue("LBCloud_clientid", text);
			Debug.Log("Cloud client id: " + text);
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x0002D73C File Offset: 0x0002B93C
		private static int GetDateTicksInSeconds()
		{
			DateTime dateTime = new DateTime(2000, 1, 1, 8, 0, 0, DateTimeKind.Utc);
			return (int)(DateTime.UtcNow - dateTime).TotalSeconds;
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x0002D770 File Offset: 0x0002B970
		private void SetWasSynced(bool synced)
		{
			Debug.Log("<color=cyan>SetWasSynced:</color> " + synced.ToString());
			if (synced)
			{
				this.settingsInterface.SetValue("LBCloud_was_synced", "1");
				return;
			}
			this.settingsInterface.DeleteValue("LBCloud_was_synced");
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x0002D7BC File Offset: 0x0002B9BC
		public static bool IsRegisteredInCloud()
		{
			return LazyCloudSync.instance.settingsInterface.HasValue("LBCloud_id") && LazyCloudSync.instance.settingsInterface.HasValue("LBCloud_pass") && !string.IsNullOrEmpty(LazyCloudSync.instance.settingsInterface.GetValue("LBCloud_id"));
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x0002D813 File Offset: 0x0002BA13
		private static bool DoIHaveActiveSyncCode()
		{
			return LazyCloudSync.instance.settingsInterface.HasValue("LBCloud_code") && Convert.ToInt32(LazyCloudSync.instance.settingsInterface.GetValue("LBCloud_time")) - LazyCloudSync.GetDateTicksInSeconds() > 0;
		}

		// Token: 0x06000994 RID: 2452 RVA: 0x0002D850 File Offset: 0x0002BA50
		public static string GetSyncCodeTimeLeft()
		{
			int num = LazyCloudSync.instance.time - LazyCloudSync.GetDateTicksInSeconds();
			if (num <= 0)
			{
				LazyCloudSync.instance.syncInterface.OnCloudSyncCodeExpired();
				return "";
			}
			int num2 = num / 60;
			num -= num2 * 60;
			string text = ((num >= 10) ? "" : "0") + num.ToString();
			return string.Format("{0}:{1}", num2, text);
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x0002D8C2 File Offset: 0x0002BAC2
		private void OverwriteWithRemoteSave(LazyCloudSyncSaveData remoteSave)
		{
			Debug.LogError("Overwrite: " + remoteSave.header);
			this.syncInterface.SetSavegameData(remoteSave);
			this.currentState = LazyCloudSync.State.Connected;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0002D8EC File Offset: 0x0002BAEC
		public static void SaveToCloud()
		{
			if (LazyCloudSync.instance == null)
			{
				return;
			}
			if (!LazyCloudSync.IsRegisteredInCloud())
			{
				return;
			}
			LazyCloudSync.instance.DoSyncRequest(false);
			LazyCloudSync.instance.SetWasSynced(false);
		}

		// Token: 0x040005BC RID: 1468
		public const string SERVER_URL = "http://s2.lazybeargames.com/cloud/";

		// Token: 0x040005BD RID: 1469
		private static bool initialized;

		// Token: 0x040005BE RID: 1470
		private static LazyCloudSync instance;

		// Token: 0x040005BF RID: 1471
		private ILazyCloudSyncSettings settingsInterface;

		// Token: 0x040005C0 RID: 1472
		private ILazyCloudSync syncInterface;

		// Token: 0x040005C1 RID: 1473
		private LazyCloudSync.State currentState;

		// Token: 0x040005C2 RID: 1474
		private bool processingCoroutine;

		// Token: 0x040005C3 RID: 1475
		private Queue<LazyCloudSync.EnumeratorDelegate> onCoroutineFinished = new Queue<LazyCloudSync.EnumeratorDelegate>();

		// Token: 0x040005C4 RID: 1476
		private int time;

		// Token: 0x040005C5 RID: 1477
		private string code;

		// Token: 0x0200021C RID: 540
		public enum State
		{
			// Token: 0x04000727 RID: 1831
			NotInited,
			// Token: 0x04000728 RID: 1832
			Unknown,
			// Token: 0x04000729 RID: 1833
			NotSetUp,
			// Token: 0x0400072A RID: 1834
			Connected,
			// Token: 0x0400072B RID: 1835
			Syncing,
			// Token: 0x0400072C RID: 1836
			Offline
		}

		// Token: 0x0200021D RID: 541
		public enum CloudResult
		{
			// Token: 0x0400072E RID: 1838
			Unknown = -1,
			// Token: 0x0400072F RID: 1839
			OK,
			// Token: 0x04000730 RID: 1840
			WrongSyncCode,
			// Token: 0x04000731 RID: 1841
			CantLinkToSameDevice,
			// Token: 0x04000732 RID: 1842
			AuthenticationError,
			// Token: 0x04000733 RID: 1843
			ErrorDowloadingFile
		}

		// Token: 0x0200021E RID: 542
		// (Invoke) Token: 0x06000ACF RID: 2767
		private delegate IEnumerator EnumeratorDelegate();
	}
}
