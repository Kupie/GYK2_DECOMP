using System;

namespace LazyBearTechnology
{
	// Token: 0x0200018B RID: 395
	public static class Platform
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060008D0 RID: 2256 RVA: 0x0002A910 File Offset: 0x00028B10
		public static PlatformType Type
		{
			get
			{
				if (Platform.debugPlatformForced)
				{
					return Platform.forcedPlatformType;
				}
				return PlatformType.PС;
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002A920 File Offset: 0x00028B20
		public static void ForceDebugPlatform(PlatformType platformType)
		{
			Platform.debugPlatformForced = true;
			Platform.forcedPlatformType = platformType;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002A92E File Offset: 0x00028B2E
		public static void ClearForcedPlatform()
		{
			Platform.debugPlatformForced = false;
		}

		// Token: 0x0400054B RID: 1355
		private static bool debugPlatformForced;

		// Token: 0x0400054C RID: 1356
		private static PlatformType forcedPlatformType;

		// Token: 0x0400054D RID: 1357
		private const PlatformType CURRENT = PlatformType.PС;
	}
}
