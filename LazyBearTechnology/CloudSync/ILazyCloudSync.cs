using System;

namespace LazyBearTechnology.CloudSync
{
	// Token: 0x020001A1 RID: 417
	public interface ILazyCloudSync
	{
		// Token: 0x06000972 RID: 2418
		string GetApplicationVersion();

		// Token: 0x06000973 RID: 2419
		LazyCloudSyncSaveData GetSavegameData();

		// Token: 0x06000974 RID: 2420
		void SetSavegameData(LazyCloudSyncSaveData data);

		// Token: 0x06000975 RID: 2421
		void ShowCloudSyncDialogWithEnterCodeButton();

		// Token: 0x06000976 RID: 2422
		void ShowCloudSyncDialogWithBreakSyncButton();

		// Token: 0x06000977 RID: 2423
		void ShowCloudSyncDialogWithCode(string code);

		// Token: 0x06000978 RID: 2424
		void OnCloudSyncCodeExpired();

		// Token: 0x06000979 RID: 2425
		void ShowCloudSyncErrorMessage(LazyCloudSync.CloudResult result);

		// Token: 0x0600097A RID: 2426
		void ShowCloudSyncSaveChooserDialog(LazyCloudSyncSaveData local_data, Action on_local_chosen, LazyCloudSyncSaveData remote_data, Action on_remote_chosen);
	}
}
