using System;

namespace LazyBearTechnology.CloudSync
{
	// Token: 0x020001A2 RID: 418
	public interface ILazyCloudSyncSettings
	{
		// Token: 0x0600097B RID: 2427
		void SetValue(string key, string value);

		// Token: 0x0600097C RID: 2428
		string GetValue(string key);

		// Token: 0x0600097D RID: 2429
		bool HasValue(string key);

		// Token: 0x0600097E RID: 2430
		void DeleteValue(string key);
	}
}
