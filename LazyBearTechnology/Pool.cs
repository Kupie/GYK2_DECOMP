using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x0200018F RID: 399
	public class Pool
	{
		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x0002AC8E File Offset: 0x00028E8E
		public MonoBehaviour Prefab
		{
			get
			{
				return this.prefab;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060008E3 RID: 2275 RVA: 0x0002AC96 File Offset: 0x00028E96
		public Stack<MonoBehaviour> Objects
		{
			get
			{
				return this.objects;
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0002ACA0 File Offset: 0x00028EA0
		public Pool(MonoBehaviour prefab, Transform poolParent, int initialSize, Pool.PoolType poolType = Pool.PoolType.ImmediateActivation, bool forceActivateAllNewObjects = false, Pool.MonoBehaviourDelegate onCreateNewObject = null)
		{
			this.prefab = prefab;
			this.poolParent = poolParent;
			this.poolType = poolType;
			this.forceActivateAllNewObjects = forceActivateAllNewObjects;
			this.onCreateNewObject = onCreateNewObject;
			for (int i = 0; i < initialSize; i++)
			{
				this.AddObjectToPool();
			}
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0002ACF8 File Offset: 0x00028EF8
		public void AddObjectToPool()
		{
			MonoBehaviour monoBehaviour = global::UnityEngine.Object.Instantiate<MonoBehaviour>(this.prefab, this.poolParent);
			this.objects.Push(monoBehaviour);
			if (this.poolType == Pool.PoolType.ImmediateActivation)
			{
				monoBehaviour.gameObject.SetActive(false);
			}
			if (this.forceActivateAllNewObjects)
			{
				monoBehaviour.gameObject.SetActive(true);
			}
			Pool.MonoBehaviourDelegate monoBehaviourDelegate = this.onCreateNewObject;
			if (monoBehaviourDelegate == null)
			{
				return;
			}
			monoBehaviourDelegate(monoBehaviour);
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0002AD5C File Offset: 0x00028F5C
		public void ReleaseObject<T>(T obj) where T : MonoBehaviour
		{
			this.objects.Push(obj);
			obj.transform.SetParent(this.poolParent);
			if (this.poolType == Pool.PoolType.ImmediateActivation)
			{
				obj.gameObject.SetActive(false);
			}
			IPoolable poolable = obj as IPoolable;
			if (poolable != null)
			{
				poolable.OnPoolableObjReleased();
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002ADC0 File Offset: 0x00028FC0
		public void ReleaseAllObjectsAndClearList<T>(ref List<T> objs) where T : MonoBehaviour
		{
			if (objs == null)
			{
				return;
			}
			foreach (T t in objs)
			{
				this.ReleaseObject<T>(t);
			}
			objs.Clear();
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002AE1C File Offset: 0x0002901C
		public T GetOrCreateObject<T>() where T : MonoBehaviour
		{
			if (this.objects.Count == 0)
			{
				this.AddObjectToPool();
			}
			MonoBehaviour monoBehaviour = this.objects.Pop();
			if (this.poolType == Pool.PoolType.ImmediateActivation || !monoBehaviour.gameObject.activeSelf)
			{
				monoBehaviour.gameObject.SetActive(true);
			}
			return monoBehaviour as T;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002AE74 File Offset: 0x00029074
		public T GetOrCreateInactiveObject<T>() where T : MonoBehaviour
		{
			if (this.objects.Count == 0)
			{
				this.AddInactiveObjectToPool();
			}
			return this.objects.Pop() as T;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002AEA0 File Offset: 0x000290A0
		public void AddInactiveObjectToPool()
		{
			Transform transform = Pool.GetInstantiateHideRoot();
			MonoBehaviour monoBehaviour = global::UnityEngine.Object.Instantiate<MonoBehaviour>(this.prefab, transform);
			monoBehaviour.gameObject.SetActive(false);
			monoBehaviour.transform.SetParent(this.poolParent, false);
			this.objects.Push(monoBehaviour);
			Pool.MonoBehaviourDelegate monoBehaviourDelegate = this.onCreateNewObject;
			if (monoBehaviourDelegate == null)
			{
				return;
			}
			monoBehaviourDelegate(monoBehaviour);
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002AEFC File Offset: 0x000290FC
		private static Transform GetInstantiateHideRoot()
		{
			if (Pool.instantiateHideRoot == null)
			{
				Pool.instantiateHideRoot = new GameObject("PoolInstantiateInactive");
				global::UnityEngine.Object.DontDestroyOnLoad(Pool.instantiateHideRoot);
				Pool.instantiateHideRoot.hideFlags = HideFlags.HideAndDontSave;
				Pool.instantiateHideRoot.SetActive(false);
			}
			return Pool.instantiateHideRoot.transform;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002AF50 File Offset: 0x00029150
		public void DeactivateUnnecessaryObjects()
		{
			if (this.poolType != Pool.PoolType.SmartActivation)
			{
				return;
			}
			bool flag = this.onDeactivate != null;
			foreach (MonoBehaviour monoBehaviour in this.objects)
			{
				if (flag)
				{
					this.onDeactivate(monoBehaviour);
				}
				else if (monoBehaviour.gameObject.activeSelf)
				{
					monoBehaviour.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002AFDC File Offset: 0x000291DC
		public void SetCustomSmartDeactivationMethod(Pool.MonoBehaviourDelegate onDeactivate)
		{
			this.onDeactivate = onDeactivate;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002AFE5 File Offset: 0x000291E5
		public void SetCustomOnCreateNewObjectDelegate(Pool.MonoBehaviourDelegate onCreateNewObject)
		{
			this.onCreateNewObject = onCreateNewObject;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002AFF0 File Offset: 0x000291F0
		public int TrimIdleToSize(int maxIdleCount)
		{
			int num = 0;
			while (this.objects.Count > maxIdleCount)
			{
				MonoBehaviour monoBehaviour = this.objects.Pop();
				if (monoBehaviour != null)
				{
					global::UnityEngine.Object.Destroy(monoBehaviour.gameObject);
				}
				num++;
			}
			return num;
		}

		// Token: 0x04000557 RID: 1367
		private Stack<MonoBehaviour> objects = new Stack<MonoBehaviour>();

		// Token: 0x04000558 RID: 1368
		private MonoBehaviour prefab;

		// Token: 0x04000559 RID: 1369
		private Transform poolParent;

		// Token: 0x0400055A RID: 1370
		private bool forceActivateAllNewObjects;

		// Token: 0x0400055B RID: 1371
		private Pool.PoolType poolType;

		// Token: 0x0400055C RID: 1372
		private Pool.MonoBehaviourDelegate onDeactivate;

		// Token: 0x0400055D RID: 1373
		private Pool.MonoBehaviourDelegate onCreateNewObject;

		// Token: 0x0400055E RID: 1374
		private static GameObject instantiateHideRoot;

		// Token: 0x0200020B RID: 523
		public enum PoolType
		{
			// Token: 0x040006F7 RID: 1783
			ImmediateActivation,
			// Token: 0x040006F8 RID: 1784
			SmartActivation
		}

		// Token: 0x0200020C RID: 524
		// (Invoke) Token: 0x06000A9F RID: 2719
		public delegate void MonoBehaviourDelegate(MonoBehaviour obj);
	}
}
