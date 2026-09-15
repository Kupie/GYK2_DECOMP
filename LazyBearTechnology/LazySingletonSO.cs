using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace LazyBearTechnology
{
	// Token: 0x02000185 RID: 389
	public abstract class LazySingletonSO<T> : ScriptableObject where T : ScriptableObject
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x0002A428 File Offset: 0x00028628
		public static T Instance
		{
			get
			{
				if (LazySingletonSO<T>.instance == null)
				{
					LazySingletonSO<T>.instance = Addressables.LoadAssetAsync<T>(typeof(T).Name).WaitForCompletion();
				}
				return LazySingletonSO<T>.instance;
			}
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0002A46D File Offset: 0x0002866D
		public static void SetReference(T reference)
		{
			LazySingletonSO<T>.instance = reference;
		}

		// Token: 0x04000545 RID: 1349
		private static T instance;
	}
}
