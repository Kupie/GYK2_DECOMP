using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000192 RID: 402
	[Serializable]
	public class ConditionalChecker
	{
		// Token: 0x06000902 RID: 2306 RVA: 0x0002B4D6 File Offset: 0x000296D6
		public void Init(Func<bool> condition, Action onUpdate, Action onComplete)
		{
			this.Refresh();
			this.onComplete = onComplete;
			this.onUpdate = onUpdate;
			this.condition = condition;
			this.id = global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			this.isActive = true;
			this.justStarted = true;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x0002B516 File Offset: 0x00029716
		public void Update()
		{
			if (this.justStarted)
			{
				this.justStarted = false;
				return;
			}
			Action action = this.onUpdate;
			if (action != null)
			{
				action();
			}
			if (this.condition())
			{
				this.OnComplete();
			}
		}

		// Token: 0x06000904 RID: 2308 RVA: 0x0002B54C File Offset: 0x0002974C
		public void Stop()
		{
			this.isActive = false;
		}

		// Token: 0x06000905 RID: 2309 RVA: 0x0002B555 File Offset: 0x00029755
		public void OnComplete()
		{
			this.onComplete();
			this.isActive = false;
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x0002B569 File Offset: 0x00029769
		private void Refresh()
		{
			this.onComplete = null;
			this.onUpdate = null;
			this.condition = null;
			this.isActive = false;
			this.justStarted = true;
		}

		// Token: 0x04000567 RID: 1383
		public int id;

		// Token: 0x04000568 RID: 1384
		public bool isActive;

		// Token: 0x04000569 RID: 1385
		public bool justStarted;

		// Token: 0x0400056A RID: 1386
		private Action onComplete;

		// Token: 0x0400056B RID: 1387
		private Action onUpdate;

		// Token: 0x0400056C RID: 1388
		private Func<bool> condition;
	}
}
