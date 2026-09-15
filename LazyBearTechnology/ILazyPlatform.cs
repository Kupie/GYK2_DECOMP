using System;

namespace LazyBearTechnology
{
	// Token: 0x0200010F RID: 271
	public interface ILazyPlatform
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600056A RID: 1386
		// (remove) Token: 0x0600056B RID: 1387
		event Action<PlayerInfo> OnUserDataChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600056C RID: 1388
		// (remove) Token: 0x0600056D RID: 1389
		event Action<string> OnDifferentUserFound;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600056E RID: 1390
		// (remove) Token: 0x0600056F RID: 1391
		event Action OnNoUsersFound;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000570 RID: 1392
		// (remove) Token: 0x06000571 RID: 1393
		event Action OnAllControllersDisabled;

		// Token: 0x06000572 RID: 1394
		void Init();

		// Token: 0x06000573 RID: 1395
		LazyPlatform GetPlatformId();

		// Token: 0x06000574 RID: 1396
		string GetPlatformName();

		// Token: 0x06000575 RID: 1397
		void AddUser(bool tryAddSilently = true);

		// Token: 0x06000576 RID: 1398
		void Update();

		// Token: 0x06000577 RID: 1399
		string GetUniqueUserId();

		// Token: 0x06000578 RID: 1400
		bool IsGuest();

		// Token: 0x06000579 RID: 1401
		void UnlockAchievement(string id);

		// Token: 0x0600057A RID: 1402
		void SetAchievementProgress(string id, int currentValue, int fullValue);

		// Token: 0x0600057B RID: 1403
		bool ClearAchievementById(string id);

		// Token: 0x0600057C RID: 1404
		bool ClearAllAchievements();

		// Token: 0x0600057D RID: 1405
		bool PrefsHasKey(string key);

		// Token: 0x0600057E RID: 1406
		string PrefsGetString(string key, string defaultValue = "");

		// Token: 0x0600057F RID: 1407
		int PrefsGetInt(string key, int defaultValue = 0);

		// Token: 0x06000580 RID: 1408
		void PrefsSetString(string key, string value);

		// Token: 0x06000581 RID: 1409
		void PrefsSave();

		// Token: 0x06000582 RID: 1410
		void ShowKeyboard(Action<string> callback, int textMaxLength, string headerText);

		// Token: 0x06000583 RID: 1411
		bool IsDLCAvailable(DLCInfo dlcInfo);

		// Token: 0x06000584 RID: 1412
		void OpenProductInStore(StoreProductInfo storeProductInfo);
	}
}
