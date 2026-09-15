using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000533 RID: 1331
public class WorldFXPool : LazySingleton<WorldFXPool>, IProgress<float>
{
	// Token: 0x17000591 RID: 1425
	// (get) Token: 0x06002243 RID: 8771 RVA: 0x000A0F35 File Offset: 0x0009F135
	// (set) Token: 0x06002244 RID: 8772 RVA: 0x000A0F3D File Offset: 0x0009F13D
	public float LocalProgress { get; private set; }

	// Token: 0x06002245 RID: 8773 RVA: 0x000A0F48 File Offset: 0x0009F148
	public static bool TryLoadPrefab(string fxName, out GameObject prefab)
	{
		prefab = null;
		if (string.IsNullOrEmpty(fxName))
		{
			return false;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle;
		if (WorldFXPool.loadedPrefabHandles.TryGetValue(fxName, out asyncOperationHandle))
		{
			if (asyncOperationHandle.IsValid() && asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && asyncOperationHandle.Result != null)
			{
				prefab = asyncOperationHandle.Result;
				return true;
			}
			WorldFXPool.loadedPrefabHandles.Remove(fxName);
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<GameObject>(asyncOperationHandle);
			}
		}
		if (GameShutdown.IsRequested)
		{
			return false;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle2 = Addressables.LoadAssetAsync<GameObject>(fxName + ".prefab");
		asyncOperationHandle2.WaitForCompletion();
		if (asyncOperationHandle2.Status != AsyncOperationStatus.Succeeded || asyncOperationHandle2.Result == null)
		{
			if (asyncOperationHandle2.IsValid())
			{
				Addressables.Release<GameObject>(asyncOperationHandle2);
			}
			return false;
		}
		WorldFXPool.loadedPrefabHandles[fxName] = asyncOperationHandle2;
		prefab = asyncOperationHandle2.Result;
		return true;
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x000A101C File Offset: 0x0009F21C
	public async UniTask InitAsync()
	{
		GameShutdown.ThrowIfRequested();
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		int total = LazySingletonSO<WorldFXPoolInitialSizeConfig>.Instance.initialSizeConfigs.List.Count;
		int index = 0;
		foreach (GameResAtom gameResAtom in LazySingletonSO<WorldFXPoolInitialSizeConfig>.Instance.initialSizeConfigs.List)
		{
			GameShutdown.ThrowIfRequested();
			this.CreatePoolById(gameResAtom.type);
			this.Report((float)index / (float)total);
			int num = index;
			index = num + 1;
			await BackgroundLoading.YieldIfNeeded(index, 10);
		}
		List<GameResAtom>.Enumerator enumerator = default(List<GameResAtom>.Enumerator);
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000A105F File Offset: 0x0009F25F
	public static WorldFX Get(string fxName)
	{
		return LazySingleton<WorldFXPool>.Instance.GetInternal(fxName);
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000A106C File Offset: 0x0009F26C
	public static void Release(string fxName, WorldFX obj)
	{
		LazySingleton<WorldFXPool>.Instance.ReleaseInternal(fxName, obj);
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000A107C File Offset: 0x0009F27C
	public void BakeSizesToInitialConfig()
	{
		foreach (string text in this.poolsDict.Keys)
		{
			LazySingletonSO<WorldFXPoolInitialSizeConfig>.Instance.SetSizeForName(text, this.poolsDict[text].Objects.Count);
		}
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000A10F0 File Offset: 0x0009F2F0
	private WorldFX GetInternal(string fxName)
	{
		Pool pool;
		if (!this.poolsDict.TryGetValue(fxName, out pool))
		{
			pool = this.CreatePoolById(fxName);
			if (pool == null)
			{
				return null;
			}
		}
		return pool.GetOrCreateInactiveObject<WorldFX>();
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x000A1120 File Offset: 0x0009F320
	private Pool CreatePoolById(string fxName)
	{
		GameObject gameObject;
		if (!WorldFXPool.TryLoadPrefab(fxName, out gameObject))
		{
			Debug.LogError("WorldFX prefab not found: [" + fxName + "]");
			return null;
		}
		return this.FinishCreatePool(fxName, gameObject);
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000A1158 File Offset: 0x0009F358
	private Pool FinishCreatePool(string fxName, GameObject obj)
	{
		Pool pool;
		if (this.poolsDict.TryGetValue(fxName, out pool))
		{
			return pool;
		}
		WorldFX worldFX;
		if (!obj.TryGetComponent<WorldFX>(out worldFX))
		{
			worldFX = obj.AddComponent<WorldFX>();
		}
		Pool pool2 = LazyPooler.CreatePoolById(fxName, worldFX, 0, Pool.PoolType.ImmediateActivation, true, false, null);
		int sizeForName = LazySingletonSO<WorldFXPoolInitialSizeConfig>.Instance.GetSizeForName(fxName);
		for (int i = 0; i < sizeForName; i++)
		{
			pool2.AddInactiveObjectToPool();
		}
		this.poolsDict.Add(fxName, pool2);
		return pool2;
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000A11C5 File Offset: 0x0009F3C5
	private void ReleaseInternal(string fxName, WorldFX obj)
	{
		this.poolsDict[fxName].ReleaseObject<WorldFX>(obj);
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000A11D9 File Offset: 0x0009F3D9
	public void Report(float value)
	{
		this.LocalProgress = Mathf.Clamp01(value);
	}

	// Token: 0x04001ED3 RID: 7891
	private const int YIELD_EVERY = 10;

	// Token: 0x04001ED4 RID: 7892
	private Dictionary<string, Pool> poolsDict = new Dictionary<string, Pool>();

	// Token: 0x04001ED5 RID: 7893
	private static readonly Dictionary<string, AsyncOperationHandle<GameObject>> loadedPrefabHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
}
