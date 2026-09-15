using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000114 RID: 276
	public class LazyPlatformUpdater : MonoBehaviour
	{
		// Token: 0x06000597 RID: 1431 RVA: 0x0001CEAB File Offset: 0x0001B0AB
		private void Update()
		{
			LazyAPI.Platform.Update();
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0001CEB7 File Offset: 0x0001B0B7
		public static void Init()
		{
			if (LazyPlatformUpdater.isInited || !Application.isPlaying)
			{
				return;
			}
			LazyPlatformUpdater.isInited = true;
			GameObject gameObject = new GameObject("LazyPlatformUpdater");
			gameObject.AddComponent<LazyPlatformUpdater>();
			global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		}

		// Token: 0x0400029C RID: 668
		private static bool isInited;
	}
}
