using System;
using UnityEngine;

namespace LazyBearTechnology.CloudSync
{
	// Token: 0x020001A0 RID: 416
	public class LazyCloudSyncSettingsPlayerPrefs : ILazyCloudSyncSettings
	{
		// Token: 0x0600096D RID: 2413 RVA: 0x0002D34F File Offset: 0x0002B54F
		public void SetValue(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002D358 File Offset: 0x0002B558
		public string GetValue(string key)
		{
			return PlayerPrefs.GetString(key);
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002D360 File Offset: 0x0002B560
		public bool HasValue(string key)
		{
			return PlayerPrefs.HasKey(key);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0002D368 File Offset: 0x0002B568
		public void DeleteValue(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}
	}
}
