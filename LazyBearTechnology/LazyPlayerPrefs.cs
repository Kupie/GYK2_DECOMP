using System;

namespace LazyBearTechnology
{
	// Token: 0x0200011A RID: 282
	public class LazyPlayerPrefs
	{
		// Token: 0x060005BD RID: 1469 RVA: 0x0001D375 File Offset: 0x0001B575
		public string GetString(string key, string defaultValue = "")
		{
			return LazyAPI.Platform.PrefsGetString(key, defaultValue);
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0001D383 File Offset: 0x0001B583
		public bool HasKey(string key)
		{
			return LazyAPI.Platform.PrefsHasKey(key);
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x0001D390 File Offset: 0x0001B590
		public int GetInt(string key, int defaultValue = 0)
		{
			return LazyAPI.Platform.PrefsGetInt(key, defaultValue);
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x0001D39E File Offset: 0x0001B59E
		public void SetString(string key, string value)
		{
			LazyAPI.Platform.PrefsSetString(key, value);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x0001D3AC File Offset: 0x0001B5AC
		public void Save()
		{
			LazyAPI.Platform.PrefsSave();
		}
	}
}
