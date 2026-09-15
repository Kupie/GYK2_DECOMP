using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000186 RID: 390
	public static class LazyTime
	{
		// Token: 0x0600089F RID: 2207 RVA: 0x0002A47D File Offset: 0x0002867D
		public static void OverrideUnscaledDeltaTime(float value)
		{
			LazyTime.isUnscaledDeltaTimeOverrode = true;
			LazyTime.overrodeUnscaledDeltaTime = value;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0002A48B File Offset: 0x0002868B
		public static void CancelOverrideUnscaledDeltaTime()
		{
			LazyTime.isUnscaledDeltaTimeOverrode = false;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x0002A493 File Offset: 0x00028693
		public static float GetUnscaledDeltaTime
		{
			get
			{
				if (!LazyTime.isUnscaledDeltaTimeOverrode)
				{
					return Time.unscaledDeltaTime;
				}
				return LazyTime.overrodeUnscaledDeltaTime;
			}
		}

		// Token: 0x04000546 RID: 1350
		private static float overrodeUnscaledDeltaTime;

		// Token: 0x04000547 RID: 1351
		private static bool isUnscaledDeltaTimeOverrode;
	}
}
