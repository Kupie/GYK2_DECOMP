using System;
using Steamworks;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000117 RID: 279
	public class LazyPlatformDefault : ILazyPlatform
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600059C RID: 1436 RVA: 0x0001CEFC File Offset: 0x0001B0FC
		// (remove) Token: 0x0600059D RID: 1437 RVA: 0x0001CF34 File Offset: 0x0001B134
		public event Action<PlayerInfo> OnUserDataChanged;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600059E RID: 1438 RVA: 0x0001CF6C File Offset: 0x0001B16C
		// (remove) Token: 0x0600059F RID: 1439 RVA: 0x0001CFA4 File Offset: 0x0001B1A4
		public event Action<string> OnDifferentUserFound;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060005A0 RID: 1440 RVA: 0x0001CFDC File Offset: 0x0001B1DC
		// (remove) Token: 0x060005A1 RID: 1441 RVA: 0x0001D014 File Offset: 0x0001B214
		public event Action OnNoUsersFound;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060005A2 RID: 1442 RVA: 0x0001D04C File Offset: 0x0001B24C
		// (remove) Token: 0x060005A3 RID: 1443 RVA: 0x0001D084 File Offset: 0x0001B284
		public event Action OnAllControllersDisabled;

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001D0B9 File Offset: 0x0001B2B9
		public void Init()
		{
			if (SteamManager.Initialized)
			{
				this.gamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create(new Callback<GamepadTextInputDismissed_t>.DispatchDelegate(this.OnGamepadTextInputRecieved));
			}
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001D0D9 File Offset: 0x0001B2D9
		public LazyPlatform GetPlatformId()
		{
			return LazyPlatform.PC;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001D0DC File Offset: 0x0001B2DC
		public void Update()
		{
			this.FlushSteamStatsIfDirty();
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x0001D0E4 File Offset: 0x0001B2E4
		public void AddUser(bool tryAddSilently = true)
		{
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
		public string GetUniqueUserId()
		{
			if (SteamManager.Initialized)
			{
				return SteamFriends.GetPersonaName() + " " + SteamUser.GetSteamID().ToString();
			}
			return null;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001D120 File Offset: 0x0001B320
		public string GetPlatformName()
		{
			if (SteamManager.Initialized)
			{
				return "Steam";
			}
			return "PC";
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001D134 File Offset: 0x0001B334
		public bool IsGuest()
		{
			return false;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001D137 File Offset: 0x0001B337
		public void UnlockAchievement(string achievementId)
		{
			if (!this.IsAchievementSystemActive())
			{
				return;
			}
			SteamUserStats.SetAchievement(achievementId);
			this.MarkSteamStatsDirty();
			this.FlushSteamStatsIfDirty();
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001D158 File Offset: 0x0001B358
		public void SetAchievementProgress(string id, int currentValue, int fullValue)
		{
			if (string.IsNullOrEmpty(id) || currentValue <= 0)
			{
				return;
			}
			if (!this.IsAchievementSystemActive())
			{
				return;
			}
			try
			{
				int num;
				if (!SteamUserStats.GetStat(id, out num) || num != currentValue)
				{
					if (SteamUserStats.SetStat(id, currentValue))
					{
						this.MarkSteamStatsDirty();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning(string.Format("#achievement# SetAchievementProgress failed id:[{0}] value:[{1}/{2}] {3}", new object[] { id, currentValue, fullValue, ex }));
			}
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
		public bool ClearAchievementById(string id)
		{
			return SteamUserStats.ClearAchievement(id);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x0001D1EC File Offset: 0x0001B3EC
		public bool ClearAllAchievements()
		{
			return SteamUserStats.ResetAllStats(true);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x0001D1F4 File Offset: 0x0001B3F4
		private void MarkSteamStatsDirty()
		{
			this.steamStatsDirty = true;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x0001D1FD File Offset: 0x0001B3FD
		private void FlushSteamStatsIfDirty()
		{
			if (!this.steamStatsDirty || !SteamManager.Initialized)
			{
				return;
			}
			SteamUserStats.StoreStats();
			this.steamStatsDirty = false;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001D21C File Offset: 0x0001B41C
		private bool IsAchievementSystemActive()
		{
			bool flag;
			try
			{
				flag = SteamManager.Initialized;
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001D248 File Offset: 0x0001B448
		public bool PrefsHasKey(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x0001D250 File Offset: 0x0001B450
		public string PrefsGetString(string key, string defaultValue = "")
		{
			return PlayerPrefs.GetString(key, defaultValue);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x0001D259 File Offset: 0x0001B459
		public int PrefsGetInt(string key, int defaultValue = 0)
		{
			return PlayerPrefs.GetInt(key, defaultValue);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x0001D262 File Offset: 0x0001B462
		public void PrefsSetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x0001D26B File Offset: 0x0001B46B
		public void PrefsSave()
		{
			PlayerPrefs.Save();
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x0001D274 File Offset: 0x0001B474
		public bool IsDLCAvailable(DLCInfo dlcInfo)
		{
			if (!dlcInfo.useFileCheck)
			{
				if (dlcInfo.steamAppId == 0U)
				{
					return false;
				}
				if (SteamManager.Initialized)
				{
					return SteamApps.BIsDlcInstalled(new AppId_t(dlcInfo.steamAppId));
				}
			}
			return LazyAPI.LazyFile.Exists(Application.dataPath + "/" + dlcInfo.dlcFilePath);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x0001D2CA File Offset: 0x0001B4CA
		public void OpenProductInStore(StoreProductInfo storeProductInfo)
		{
			Application.OpenURL(storeProductInfo.steamUrl);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x0001D2D7 File Offset: 0x0001B4D7
		public void ShowKeyboard(Action<string> callback, int textMaxLength, string headerText)
		{
			if (!SteamManager.Initialized)
			{
				return;
			}
			if (SteamUtils.ShowGamepadTextInput(EGamepadTextInputMode.k_EGamepadTextInputModeNormal, EGamepadTextInputLineMode.k_EGamepadTextInputLineModeSingleLine, headerText, (uint)textMaxLength, string.Empty))
			{
				this.gamepadTextInputCallback = callback;
				SteamAPI.RunCallbacks();
				return;
			}
			if (callback != null)
			{
				callback(string.Empty);
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x0001D30C File Offset: 0x0001B50C
		private void OnGamepadTextInputRecieved(GamepadTextInputDismissed_t callback)
		{
			if (!SteamManager.Initialized || !callback.m_bSubmitted)
			{
				Action<string> action = this.gamepadTextInputCallback;
				if (action == null)
				{
					return;
				}
				action(string.Empty);
				return;
			}
			else
			{
				uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
				string text;
				SteamUtils.GetEnteredGamepadTextInput(out text, enteredGamepadTextLength);
				Action<string> action2 = this.gamepadTextInputCallback;
				if (action2 == null)
				{
					return;
				}
				action2(text);
				return;
			}
		}

		// Token: 0x040002A3 RID: 675
		private Callback<GamepadTextInputDismissed_t> gamepadTextInputDismissed;

		// Token: 0x040002A4 RID: 676
		private Action<string> gamepadTextInputCallback;

		// Token: 0x040002A5 RID: 677
		private bool steamStatsDirty;
	}
}
