using System;
using UnityEngine;

namespace LazyBearTechnology.Preloader
{
	// Token: 0x0200019D RID: 413
	public class LazyPreloaderController : MonoBehaviour
	{
		// Token: 0x06000960 RID: 2400 RVA: 0x0002D208 File Offset: 0x0002B408
		public void OnAnimationStarted()
		{
			LazyPreloader.OnAnimationStarted();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x0002D20F File Offset: 0x0002B40F
		public void OnAnimationStopped()
		{
			LazyPreloader.OnAnimationStopped();
		}
	}
}
