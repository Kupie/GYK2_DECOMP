using System;

namespace LazyBearTechnology
{
	// Token: 0x0200010D RID: 269
	public static class LazyAPI
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0001C9A9 File Offset: 0x0001ABA9
		public static ILazyPlatform Platform
		{
			get
			{
				if (LazyAPI.platform == null)
				{
					LazyAPI.platform = new LazyPlatformDefault();
					LazyPlatformUpdater.Init();
				}
				return LazyAPI.platform;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600055D RID: 1373 RVA: 0x0001C9C6 File Offset: 0x0001ABC6
		public static LazyPlayerPrefs PlayerPrefs
		{
			get
			{
				LazyPlayerPrefs lazyPlayerPrefs;
				if ((lazyPlayerPrefs = LazyAPI.playerPrefs) == null)
				{
					lazyPlayerPrefs = (LazyAPI.playerPrefs = new LazyPlayerPrefs());
				}
				return lazyPlayerPrefs;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001C9DC File Offset: 0x0001ABDC
		public static LazyFile LazyFile
		{
			get
			{
				LazyFile lazyFile;
				if ((lazyFile = LazyAPI.lazyFile) == null)
				{
					lazyFile = (LazyAPI.lazyFile = new LazyFile());
				}
				return lazyFile;
			}
		}

		// Token: 0x0400027D RID: 637
		private static ILazyPlatform platform;

		// Token: 0x0400027E RID: 638
		private static LazyPlayerPrefs playerPrefs;

		// Token: 0x0400027F RID: 639
		private static LazyFile lazyFile;
	}
}
