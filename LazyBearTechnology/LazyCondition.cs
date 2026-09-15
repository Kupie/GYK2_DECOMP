using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000190 RID: 400
	public class LazyCondition : MonoBehaviour
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0002B034 File Offset: 0x00029234
		public static LazyCondition Instance
		{
			get
			{
				if (LazyCondition.instance == null)
				{
					LazyCondition.instance = new GameObject("LazyCondition").AddComponent<LazyCondition>();
					global::UnityEngine.Object.DontDestroyOnLoad(LazyCondition.instance);
				}
				return LazyCondition.instance;
			}
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002B068 File Offset: 0x00029268
		private void Awake()
		{
			for (int i = 0; i < 5; i++)
			{
				this.inactiveCheckers.Add(new ConditionalChecker());
			}
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002B094 File Offset: 0x00029294
		private void Update()
		{
			for (int i = 0; i < this.activeCheckers.Count; i++)
			{
				ConditionalChecker conditionalChecker = this.activeCheckers[i];
				if (conditionalChecker.isActive)
				{
					conditionalChecker.Update();
				}
				else
				{
					this.activeCheckers.Remove(conditionalChecker);
					this.inactiveCheckers.Add(conditionalChecker);
					i--;
				}
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002B0F4 File Offset: 0x000292F4
		public static int AddConditionalChecker(Func<bool> condition, Action onUpdate, Action onComplete)
		{
			if (condition == null)
			{
				Debug.LogError("Trying to add conditional checker without condition");
				return -1;
			}
			if (onComplete == null)
			{
				Debug.LogError("Trying to add conditional checker without complete action");
				return -1;
			}
			ConditionalChecker conditionalChecker = LazyCondition.GetConditionalChecker();
			conditionalChecker.Init(condition, onUpdate, onComplete);
			LazyCondition.Instance.activeCheckers.Add(conditionalChecker);
			return conditionalChecker.id;
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002B144 File Offset: 0x00029344
		public static bool Stop(int id)
		{
			ConditionalChecker conditionalChecker = LazyCondition.Instance.activeCheckers.Find((ConditionalChecker x) => x.id == id);
			if (conditionalChecker != null)
			{
				conditionalChecker.Stop();
				return true;
			}
			return false;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002B188 File Offset: 0x00029388
		public static void StopAll()
		{
			foreach (ConditionalChecker conditionalChecker in LazyCondition.Instance.activeCheckers)
			{
				conditionalChecker.Stop();
			}
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0002B1DC File Offset: 0x000293DC
		public static void ForceCompleteAll()
		{
			foreach (ConditionalChecker conditionalChecker in LazyCondition.Instance.activeCheckers)
			{
				conditionalChecker.OnComplete();
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002B230 File Offset: 0x00029430
		private static ConditionalChecker GetConditionalChecker()
		{
			List<ConditionalChecker> list = LazyCondition.Instance.inactiveCheckers;
			ConditionalChecker conditionalChecker;
			if (list.Count != 0)
			{
				conditionalChecker = list[0];
				list.RemoveAt(0);
			}
			else
			{
				conditionalChecker = new ConditionalChecker();
			}
			return conditionalChecker;
		}

		// Token: 0x0400055F RID: 1375
		private const int POOL_SIZE = 5;

		// Token: 0x04000560 RID: 1376
		[SerializeField]
		private List<ConditionalChecker> activeCheckers = new List<ConditionalChecker>();

		// Token: 0x04000561 RID: 1377
		private List<ConditionalChecker> inactiveCheckers = new List<ConditionalChecker>();

		// Token: 0x04000562 RID: 1378
		private static LazyCondition instance;
	}
}
