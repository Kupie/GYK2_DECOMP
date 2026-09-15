using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000193 RID: 403
	[Serializable]
	public class Timer
	{
		// Token: 0x06000908 RID: 2312 RVA: 0x0002B598 File Offset: 0x00029798
		public void Init(float seconds, Action onComplete, Action<float> onUpdate = null)
		{
			this.Refresh();
			this.endTime = Time.time + seconds;
			this.onComplete = onComplete;
			this.onUpdate = onUpdate;
			this.id = global::UnityEngine.Random.Range(int.MinValue, int.MaxValue);
			this.justStarted = true;
			this.isActive = true;
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x0002B5E9 File Offset: 0x000297E9
		public void Update(float currentTime, float deltaTime)
		{
			if (this.justStarted)
			{
				this.justStarted = false;
				return;
			}
			Action<float> action = this.onUpdate;
			if (action != null)
			{
				action(deltaTime);
			}
			if (this.isActive && currentTime >= this.endTime)
			{
				this.OnComplete();
			}
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x0002B624 File Offset: 0x00029824
		public void Stop()
		{
			this.isActive = false;
			this.onComplete = null;
			this.onUpdate = null;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x0002B63B File Offset: 0x0002983B
		public void OnComplete()
		{
			this.onComplete();
			this.isActive = false;
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x0002B64F File Offset: 0x0002984F
		private void Refresh()
		{
			this.onComplete = null;
			this.isActive = false;
			this.justStarted = true;
			this.endTime = 0f;
		}

		// Token: 0x0400056D RID: 1389
		public int id;

		// Token: 0x0400056E RID: 1390
		public bool isActive;

		// Token: 0x0400056F RID: 1391
		private bool justStarted;

		// Token: 0x04000570 RID: 1392
		private float endTime;

		// Token: 0x04000571 RID: 1393
		private Action onComplete;

		// Token: 0x04000572 RID: 1394
		private Action<float> onUpdate;
	}
}
