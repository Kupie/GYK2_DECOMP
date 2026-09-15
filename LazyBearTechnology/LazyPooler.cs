using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200018E RID: 398
	public class LazyPooler : LazySingleton<LazyPooler>
	{
		// Token: 0x060008D4 RID: 2260 RVA: 0x0002A938 File Offset: 0x00028B38
		public static Pool CreatePool<T>(T prefab, int initialPoolSize = 0, Pool.PoolType poolType = Pool.PoolType.ImmediateActivation, bool parentAllObjectsInPool = false, bool forceActivateAllNewObjects = false, Pool.MonoBehaviourDelegate onCreateNewObject = null) where T : MonoBehaviour
		{
			Type typeFromHandle = typeof(T);
			if (LazySingleton<LazyPooler>.Instance.pools.ContainsKey(typeFromHandle))
			{
				string text = "Can't create a second pool of: ";
				Type type = typeFromHandle;
				Debug.LogError(text + ((type != null) ? type.ToString() : null));
				return null;
			}
			Pool pool = LazyPooler.CreatePoolInternal(prefab, initialPoolSize, poolType, parentAllObjectsInPool, forceActivateAllNewObjects, onCreateNewObject);
			LazySingleton<LazyPooler>.Instance.pools.Add(typeFromHandle, pool);
			return pool;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002A9A8 File Offset: 0x00028BA8
		public static Pool CreatePoolById(string poolId, MonoBehaviour prefab, int initialPoolSize = 0, Pool.PoolType poolType = Pool.PoolType.ImmediateActivation, bool parentAllObjectsInPool = false, bool forceActivateAllNewObjects = false, Pool.MonoBehaviourDelegate onCreateNewObject = null)
		{
			Pool pool;
			if (LazySingleton<LazyPooler>.Instance.poolsById.TryGetValue(poolId, out pool))
			{
				Debug.LogWarning("Returning existing pool with id = " + poolId);
				return pool;
			}
			Pool pool2 = LazyPooler.CreatePoolInternal(prefab, initialPoolSize, poolType, parentAllObjectsInPool, forceActivateAllNewObjects, onCreateNewObject);
			LazySingleton<LazyPooler>.Instance.poolsById.Add(poolId, pool2);
			return pool2;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002A9FC File Offset: 0x00028BFC
		private static Pool CreatePoolInternal(MonoBehaviour prefab, int initialPoolSize = 0, Pool.PoolType poolType = Pool.PoolType.ImmediateActivation, bool parentAllObjectsInPool = false, bool forceActivateAllNewObjects = false, Pool.MonoBehaviourDelegate onCreateNewObject = null)
		{
			GameObject gameObject = null;
			if (parentAllObjectsInPool)
			{
				gameObject = new GameObject();
				gameObject.name = string.Format("Pool parent of type:[{0}]", prefab.GetType());
				gameObject.transform.parent = LazySingleton<LazyPooler>.Instance.transform;
			}
			return new Pool(prefab, parentAllObjectsInPool ? gameObject.transform : prefab.transform.parent, initialPoolSize, poolType, forceActivateAllNewObjects, onCreateNewObject);
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002AA64 File Offset: 0x00028C64
		public static T GetObject<T>() where T : MonoBehaviour
		{
			Pool poolByType = LazyPooler.GetPoolByType<T>(default(T));
			if (poolByType == null)
			{
				return default(T);
			}
			return poolByType.GetOrCreateObject<T>();
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002AA92 File Offset: 0x00028C92
		public static T GetObject<T>(Transform parent) where T : MonoBehaviour
		{
			T @object = LazyPooler.GetObject<T>();
			@object.transform.SetParent(parent);
			return @object;
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002AAAC File Offset: 0x00028CAC
		public static void ReleaseObject<T>(T obj) where T : MonoBehaviour
		{
			Pool poolByType = LazyPooler.GetPoolByType<T>(default(T));
			if (poolByType == null)
			{
				return;
			}
			poolByType.ReleaseObject<T>(obj);
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0002AAD4 File Offset: 0x00028CD4
		public static T GetObject<T>(string poolId) where T : MonoBehaviour
		{
			Pool poolById = LazyPooler.GetPoolById(poolId);
			if (poolById == null)
			{
				return default(T);
			}
			return poolById.GetOrCreateObject<T>();
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002AAFA File Offset: 0x00028CFA
		public static T GetObject<T>(string poolId, Transform parent) where T : MonoBehaviour
		{
			T @object = LazyPooler.GetObject<T>(poolId);
			@object.transform.SetParent(parent);
			return @object;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0002AB13 File Offset: 0x00028D13
		public static void ReleaseObject<T>(string poolId, T obj) where T : MonoBehaviour
		{
			Pool poolById = LazyPooler.GetPoolById(poolId);
			if (poolById == null)
			{
				return;
			}
			poolById.ReleaseObject<T>(obj);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0002AB28 File Offset: 0x00028D28
		public static Pool GetPoolByType<T>(T objectUsedOnlyForPoolReference = default(T)) where T : MonoBehaviour
		{
			Pool pool;
			if (LazySingleton<LazyPooler>.Instance.pools.TryGetValue(typeof(T), out pool))
			{
				return pool;
			}
			string text = "A pool for an object type [";
			Type typeFromHandle = typeof(T);
			Debug.LogError(text + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + "] was not yet created. Call CreatePool() first.");
			return null;
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0002AB80 File Offset: 0x00028D80
		public static Pool GetPoolById(string poolId)
		{
			Pool pool;
			if (LazySingleton<LazyPooler>.Instance.poolsById.TryGetValue(poolId, out pool))
			{
				return pool;
			}
			Debug.LogError("A pool with id = " + poolId + " was not yet created. Call CreatePool() first.");
			return null;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0002ABB9 File Offset: 0x00028DB9
		public static bool RemovePoolById(string poolId)
		{
			return LazySingleton<LazyPooler>.Instance.poolsById.Remove(poolId);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0002ABCC File Offset: 0x00028DCC
		public static void DeactivateUnnecessaryObjectsForAllPools()
		{
			foreach (Pool pool in LazySingleton<LazyPooler>.Instance.pools.Values)
			{
				pool.DeactivateUnnecessaryObjects();
			}
			foreach (Pool pool2 in LazySingleton<LazyPooler>.Instance.poolsById.Values)
			{
				pool2.DeactivateUnnecessaryObjects();
			}
		}

		// Token: 0x04000555 RID: 1365
		private Dictionary<Type, Pool> pools = new Dictionary<Type, Pool>();

		// Token: 0x04000556 RID: 1366
		private Dictionary<string, Pool> poolsById = new Dictionary<string, Pool>();
	}
}
