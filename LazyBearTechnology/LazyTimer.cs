using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000191 RID: 401
	public class LazyTimer : MonoBehaviour
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060008F9 RID: 2297 RVA: 0x0002B286 File Offset: 0x00029486
		private static LazyTimer Instance
		{
			get
			{
				if (LazyTimer.instance == null)
				{
					LazyTimer.instance = new GameObject("LazyTimer").AddComponent<LazyTimer>();
					global::UnityEngine.Object.DontDestroyOnLoad(LazyTimer.instance);
				}
				return LazyTimer.instance;
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002B2B8 File Offset: 0x000294B8
		private void Awake()
		{
			for (int i = 0; i < 15; i++)
			{
				this.inactiveTimers.Add(new Timer());
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002B2E4 File Offset: 0x000294E4
		private void Update()
		{
			float time = Time.time;
			float deltaTime = Time.deltaTime;
			for (int i = 0; i < this.activeTimers.Count; i++)
			{
				Timer timer = this.activeTimers[i];
				if (timer.isActive)
				{
					timer.Update(time, deltaTime);
				}
				else
				{
					this.activeTimers.Remove(timer);
					this.inactiveTimers.Add(timer);
					i--;
				}
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002B350 File Offset: 0x00029550
		public static int AddTimer(float seconds, Action onComplete, Action<float> onUpdate = null)
		{
			if (onComplete == null)
			{
				Debug.LogError("Trying to add timer without complete action");
				return -1;
			}
			Timer timer = LazyTimer.GetTimer();
			timer.Init(seconds, onComplete, onUpdate);
			LazyTimer.Instance.activeTimers.Add(timer);
			return timer.id;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002B394 File Offset: 0x00029594
		public static bool Stop(int id)
		{
			Timer timer = LazyTimer.Instance.activeTimers.Find((Timer x) => x.id == id);
			if (timer != null)
			{
				timer.Stop();
				return true;
			}
			return false;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0002B3D8 File Offset: 0x000295D8
		public static void StopAll()
		{
			foreach (Timer timer in LazyTimer.Instance.activeTimers)
			{
				timer.Stop();
			}
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0002B42C File Offset: 0x0002962C
		public static void ForceCompleteAll()
		{
			foreach (Timer timer in LazyTimer.Instance.activeTimers)
			{
				timer.OnComplete();
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002B480 File Offset: 0x00029680
		private static Timer GetTimer()
		{
			List<Timer> list = LazyTimer.Instance.inactiveTimers;
			Timer timer;
			if (list.Count != 0)
			{
				timer = list[0];
				list.RemoveAt(0);
			}
			else
			{
				timer = new Timer();
			}
			return timer;
		}

		// Token: 0x04000563 RID: 1379
		private const int POOL_SIZE = 15;

		// Token: 0x04000564 RID: 1380
		[SerializeField]
		private List<Timer> activeTimers = new List<Timer>();

		// Token: 0x04000565 RID: 1381
		private List<Timer> inactiveTimers = new List<Timer>();

		// Token: 0x04000566 RID: 1382
		private static LazyTimer instance;
	}
}
