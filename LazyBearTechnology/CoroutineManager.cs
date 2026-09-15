using System;
using System.Collections;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000174 RID: 372
	public class CoroutineManager : MonoBehaviour
	{
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00028EB8 File Offset: 0x000270B8
		private static CoroutineManager Instance
		{
			get
			{
				if (CoroutineManager.instance == null)
				{
					CoroutineManager.instance = global::UnityEngine.Object.FindObjectOfType<CoroutineManager>();
					if (CoroutineManager.instance == null)
					{
						GameObject gameObject = new GameObject(typeof(CoroutineManager).ToString());
						CoroutineManager.instance = gameObject.AddComponent<CoroutineManager>();
						global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
					}
				}
				return CoroutineManager.instance;
			}
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00028F12 File Offset: 0x00027112
		public static Coroutine StartEnumerator(IEnumerator enumerator)
		{
			return CoroutineManager.Instance.StartCoroutine(enumerator);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00028F1F File Offset: 0x0002711F
		public static void StopEnumerator(IEnumerator enumerator)
		{
			CoroutineManager.Instance.StopCoroutine(enumerator);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00028F2C File Offset: 0x0002712C
		public static void StopAll()
		{
			CoroutineManager.Instance.StopAllCoroutines();
		}

		// Token: 0x0400052A RID: 1322
		private static CoroutineManager instance;
	}
}
