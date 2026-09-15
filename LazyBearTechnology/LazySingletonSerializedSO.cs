using System;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;

namespace LazyBearTechnology
{
	// Token: 0x02000184 RID: 388
	public abstract class LazySingletonSerializedSO<T> : SerializedScriptableObject where T : SerializedScriptableObject
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0002A3D0 File Offset: 0x000285D0
		public static T Instance
		{
			get
			{
				if (LazySingletonSerializedSO<T>.instance == null)
				{
					LazySingletonSerializedSO<T>.instance = Addressables.LoadAssetAsync<T>(typeof(T).Name).WaitForCompletion();
				}
				return LazySingletonSerializedSO<T>.instance;
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0002A415 File Offset: 0x00028615
		public static void SetReference(T reference)
		{
			LazySingletonSerializedSO<T>.instance = reference;
		}

		// Token: 0x04000544 RID: 1348
		private static T instance;
	}
}
