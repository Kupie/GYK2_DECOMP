using System;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000183 RID: 387
	public abstract class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x0002A338 File Offset: 0x00028538
		public static T Instance
		{
			get
			{
				if (LazySingleton<T>.instance == null)
				{
					LazySingleton<T>.instance = global::UnityEngine.Object.FindObjectOfType<T>(true);
					if (LazySingleton<T>.instance == null)
					{
						new GameObject(string.Format("{0} Instance", typeof(T))).AddComponent<T>();
					}
				}
				return LazySingleton<T>.instance;
			}
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002A398 File Offset: 0x00028598
		public static void SetReference(T reference)
		{
			LazySingleton<T>.instance = reference;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002A3A0 File Offset: 0x000285A0
		protected virtual void Awake()
		{
			if (LazySingleton<T>.instance != null)
			{
				return;
			}
			LazySingleton<T>.SetReference(this as T);
		}

		// Token: 0x04000543 RID: 1347
		private static T instance;
	}
}
