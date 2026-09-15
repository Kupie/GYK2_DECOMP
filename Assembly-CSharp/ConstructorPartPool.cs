using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000AA1 RID: 2721
public class ConstructorPartPool : LazySingleton<ConstructorPartPool>, IProgress<float>
{
	// Token: 0x17000B29 RID: 2857
	// (get) Token: 0x060049A8 RID: 18856 RVA: 0x0015BDFC File Offset: 0x00159FFC
	// (set) Token: 0x060049A9 RID: 18857 RVA: 0x0015BE04 File Offset: 0x0015A004
	public float LocalProgress { get; private set; }

	// Token: 0x060049AA RID: 18858 RVA: 0x0015BE10 File Offset: 0x0015A010
	public async UniTask InitAsync()
	{
		GameShutdown.ThrowIfRequested();
		Debug.Log("ConstructorPartPool InitAsync");
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		List<GameResAtom> list = LazySingletonSO<ConstructorPartPoolInitialSizesConfig>.Instance.initialSizeConfigs.List;
		int total = list.Count;
		int index = 0;
		foreach (GameResAtom gameResAtom in list)
		{
			GameShutdown.ThrowIfRequested();
			this.CreatePoolById(gameResAtom.type);
			int num = index;
			index = num + 1;
			if (total > 0)
			{
				this.Report((float)index / (float)total);
			}
			await BackgroundLoading.YieldIfNeeded(index, 10);
		}
		List<GameResAtom>.Enumerator enumerator = default(List<GameResAtom>.Enumerator);
	}

	// Token: 0x060049AB RID: 18859 RVA: 0x0015BE53 File Offset: 0x0015A053
	private void OnDestroy()
	{
		this.Clear();
	}

	// Token: 0x060049AC RID: 18860 RVA: 0x0015BE5B File Offset: 0x0015A05B
	public static ConstructorPartChildObject Get(string pathToLoad)
	{
		return LazySingleton<ConstructorPartPool>.Instance.GetInternal(pathToLoad);
	}

	// Token: 0x060049AD RID: 18861 RVA: 0x0015BE68 File Offset: 0x0015A068
	public static void Release(string pathToLoad, ConstructorPartChildObject obj)
	{
		LazySingleton<ConstructorPartPool>.Instance.ReleaseInternal(pathToLoad, obj);
	}

	// Token: 0x060049AE RID: 18862 RVA: 0x0015BE76 File Offset: 0x0015A076
	public static int TrimPaths(IEnumerable<string> paths, ScenePoolTrimPolicy policy)
	{
		return LazySingleton<ConstructorPartPool>.Instance.TrimPathsInternal(paths, policy);
	}

	// Token: 0x060049AF RID: 18863 RVA: 0x0015BE84 File Offset: 0x0015A084
	public void BakeSizesToInitialConfig()
	{
		foreach (string text in this.poolsDict.Keys)
		{
			LazySingletonSO<ConstructorPartPoolInitialSizesConfig>.Instance.SetSizeForPath(text, this.poolsDict[text].Objects.Count);
		}
	}

	// Token: 0x060049B0 RID: 18864 RVA: 0x0015BEF8 File Offset: 0x0015A0F8
	private ConstructorPartChildObject GetInternal(string pathToLoad)
	{
		Pool pool;
		if (!this.poolsDict.TryGetValue(pathToLoad, out pool))
		{
			pool = this.CreatePoolById(pathToLoad);
			if (pool == null)
			{
				return null;
			}
		}
		return pool.GetOrCreateObject<ConstructorPartChildObject>();
	}

	// Token: 0x060049B1 RID: 18865 RVA: 0x0015BF28 File Offset: 0x0015A128
	private GameObject LoadPrefab(string pathToLoad)
	{
		if (GameShutdown.IsRequested)
		{
			return null;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(pathToLoad);
		asyncOperationHandle.WaitForCompletion();
		this.loadedHandles[pathToLoad] = asyncOperationHandle;
		return asyncOperationHandle.Result;
	}

	// Token: 0x060049B2 RID: 18866 RVA: 0x0015BF64 File Offset: 0x0015A164
	private UniTask<GameObject> LoadPrefabAsync(string pathToLoad)
	{
		ConstructorPartPool.<LoadPrefabAsync>d__15 <LoadPrefabAsync>d__;
		<LoadPrefabAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<GameObject>.Create();
		<LoadPrefabAsync>d__.<>4__this = this;
		<LoadPrefabAsync>d__.pathToLoad = pathToLoad;
		<LoadPrefabAsync>d__.<>1__state = -1;
		<LoadPrefabAsync>d__.<>t__builder.Start<ConstructorPartPool.<LoadPrefabAsync>d__15>(ref <LoadPrefabAsync>d__);
		return <LoadPrefabAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060049B3 RID: 18867 RVA: 0x0015BFB0 File Offset: 0x0015A1B0
	private Pool CreatePoolById(string pathToLoad)
	{
		Pool pool;
		if (this.poolsDict.TryGetValue(pathToLoad, out pool))
		{
			return pool;
		}
		GameObject gameObject = this.LoadPrefab(pathToLoad);
		if (gameObject == null)
		{
			return null;
		}
		return this.FinishCreatePool(pathToLoad, gameObject);
	}

	// Token: 0x060049B4 RID: 18868 RVA: 0x0015BFEC File Offset: 0x0015A1EC
	private async UniTask<Pool> CreatePoolByIdAsync(string pathToLoad)
	{
		GameObject gameObject = await this.LoadPrefabAsync(pathToLoad);
		return this.FinishCreatePool(pathToLoad, gameObject);
	}

	// Token: 0x060049B5 RID: 18869 RVA: 0x0015C038 File Offset: 0x0015A238
	private Pool FinishCreatePool(string pathToLoad, GameObject obj)
	{
		Pool pool;
		if (this.poolsDict.TryGetValue(pathToLoad, out pool))
		{
			return pool;
		}
		ConstructorPartChildObject constructorPartChildObject;
		if (!obj.TryGetComponent<ConstructorPartChildObject>(out constructorPartChildObject))
		{
			constructorPartChildObject = obj.AddComponent<ConstructorPartChildObject>();
		}
		Pool pool2 = LazyPooler.CreatePoolById(pathToLoad, constructorPartChildObject, LazySingletonSO<ConstructorPartPoolInitialSizesConfig>.Instance.GetSizeForPath(pathToLoad), Pool.PoolType.ImmediateActivation, true, false, null);
		this.poolsDict.Add(pathToLoad, pool2);
		return pool2;
	}

	// Token: 0x060049B6 RID: 18870 RVA: 0x0015C090 File Offset: 0x0015A290
	private void ReleaseInternal(string pathToLoad, ConstructorPartChildObject obj)
	{
		if (obj == null)
		{
			return;
		}
		Pool pool;
		if (!this.poolsDict.TryGetValue(pathToLoad, out pool))
		{
			Debug.LogError(string.Format("Pool not found for path: {0}, total pool count:[{1}]", pathToLoad, this.poolsDict.Count));
			return;
		}
		pool.ReleaseObject<ConstructorPartChildObject>(obj);
	}

	// Token: 0x060049B7 RID: 18871 RVA: 0x0015C0E0 File Offset: 0x0015A2E0
	private int TrimPathsInternal(IEnumerable<string> paths, ScenePoolTrimPolicy policy)
	{
		if (paths == null)
		{
			return 0;
		}
		int num = 0;
		foreach (string text in paths)
		{
			Pool pool;
			if (!string.IsNullOrEmpty(text) && this.poolsDict.TryGetValue(text, out pool))
			{
				int num2 = ((policy == ScenePoolTrimPolicy.Aggressive) ? 0 : LazySingletonSO<ConstructorPartPoolInitialSizesConfig>.Instance.GetSizeForPath(text));
				num += pool.TrimIdleToSize(num2);
				if (policy == ScenePoolTrimPolicy.Aggressive && pool.Objects.Count == 0)
				{
					this.UnloadAddressablePrefab(text);
				}
			}
		}
		return num;
	}

	// Token: 0x060049B8 RID: 18872 RVA: 0x0015C178 File Offset: 0x0015A378
	private void UnloadAddressablePrefab(string path)
	{
		this.poolsDict.Remove(path);
		LazyPooler.RemovePoolById(path);
		AsyncOperationHandle<GameObject> asyncOperationHandle;
		if (!this.loadedHandles.TryGetValue(path, out asyncOperationHandle))
		{
			return;
		}
		this.loadedHandles.Remove(path);
		if (asyncOperationHandle.IsValid())
		{
			Addressables.Release<GameObject>(asyncOperationHandle);
		}
	}

	// Token: 0x060049B9 RID: 18873 RVA: 0x0015C1C6 File Offset: 0x0015A3C6
	public void Report(float value)
	{
		this.LocalProgress = Mathf.Clamp01(value);
	}

	// Token: 0x060049BA RID: 18874 RVA: 0x0015C1D4 File Offset: 0x0015A3D4
	private void Clear()
	{
		foreach (KeyValuePair<string, Pool> keyValuePair in this.poolsDict)
		{
			foreach (MonoBehaviour monoBehaviour in keyValuePair.Value.Objects)
			{
				if (monoBehaviour != null)
				{
					global::UnityEngine.Object.Destroy(monoBehaviour.gameObject);
				}
			}
		}
		this.poolsDict.Clear();
		foreach (KeyValuePair<string, AsyncOperationHandle<GameObject>> keyValuePair2 in this.loadedHandles)
		{
			if (keyValuePair2.Value.IsValid())
			{
				Addressables.Release<GameObject>(keyValuePair2.Value);
			}
		}
		this.loadedHandles.Clear();
	}

	// Token: 0x04003976 RID: 14710
	private const int YIELD_EVERY = 10;

	// Token: 0x04003977 RID: 14711
	private Dictionary<string, Pool> poolsDict = new Dictionary<string, Pool>();

	// Token: 0x04003978 RID: 14712
	private readonly Dictionary<string, AsyncOperationHandle<GameObject>> loadedHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
}
