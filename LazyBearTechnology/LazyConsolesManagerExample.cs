using System;
using DG.Tweening;
using Rewired;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000110 RID: 272
	internal class LazyConsolesManagerExample : LazySingleton<LazyConsolesManagerExample>
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001CA64 File Offset: 0x0001AC64
		public static bool IsUserInitializationFinished
		{
			get
			{
				return LazySingleton<LazyConsolesManagerExample>.Instance.isUserInitializationFinished;
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001CA70 File Offset: 0x0001AC70
		protected override void Awake()
		{
			this.Init();
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001CA78 File Offset: 0x0001AC78
		private void Init()
		{
			base.Awake();
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			LazyAPI.Platform.Init();
			this.isUserInitializationFinished = true;
			this.OnUserAdded();
			LazyAPI.Platform.AddUser(true);
			LazyAPI.Platform.OnAllControllersDisabled += this.ShowNoControllersWarning;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001CACE File Offset: 0x0001ACCE
		private void OnUserAdded()
		{
			if (!this.triggerUserAdded)
			{
				this.triggerUserAdded = true;
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001CAE0 File Offset: 0x0001ACE0
		private void Update()
		{
			if (LazyConsolesManagerExample.showNoControllerWarning)
			{
				LazyConsolesManagerExample.showNoControllerWarningDelay -= LazyTime.GetUnscaledDeltaTime;
				if (LazyConsolesManagerExample.showNoControllerWarningDelay <= 0f)
				{
					LazyConsolesManagerExample.showNoControllerWarning = false;
					if (ReInput.controllers.joystickCount == 0)
					{
						this.ShowNoControllersWarning();
					}
				}
			}
			LazyAPI.Platform.Update();
			if (LazyConsolesManagerExample.unfreezeGame)
			{
				LazyConsolesManagerExample.unfreezeGame = false;
				this.UnfreezeGame();
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001CB45 File Offset: 0x0001AD45
		private bool IsGameFrozen()
		{
			return LazyConsolesManagerExample.isFrozen;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0001CB4C File Offset: 0x0001AD4C
		private void FreezeGame()
		{
			LazyConsolesManagerExample.isFrozen = true;
			LazyTime.OverrideUnscaledDeltaTime(0f);
			DOTween.PauseAll();
			LazyConsolesManagerExample.frozenTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0001CB78 File Offset: 0x0001AD78
		private void UnfreezeGame()
		{
			LazyConsolesManagerExample.isFrozen = false;
			LazyTime.CancelOverrideUnscaledDeltaTime();
			DOTween.PlayAll();
			Time.timeScale = LazyConsolesManagerExample.frozenTimeScale;
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0001CB95 File Offset: 0x0001AD95
		public void ShowNoControllersWarning()
		{
		}

		// Token: 0x04000281 RID: 641
		private const string PS5_ACTIVITY_ID = "continue";

		// Token: 0x04000282 RID: 642
		private static bool showNoControllerWarning;

		// Token: 0x04000283 RID: 643
		private static float showNoControllerWarningDelay;

		// Token: 0x04000284 RID: 644
		private static float frozenTimeScale;

		// Token: 0x04000285 RID: 645
		private static bool unfreezeGame;

		// Token: 0x04000286 RID: 646
		private static bool isFrozen;

		// Token: 0x04000287 RID: 647
		private bool isUserInitializationFinished;

		// Token: 0x04000288 RID: 648
		private bool triggerUserAdded;
	}
}
