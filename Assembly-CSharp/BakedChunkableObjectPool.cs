using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020006E6 RID: 1766
public class BakedChunkableObjectPool : LazySingleton<BakedChunkableObjectPool>, IProgress<float>
{
	// Token: 0x17000743 RID: 1859
	// (get) Token: 0x06002EB4 RID: 11956 RVA: 0x000DF803 File Offset: 0x000DDA03
	// (set) Token: 0x06002EB5 RID: 11957 RVA: 0x000DF80B File Offset: 0x000DDA0B
	public float LocalProgress { get; private set; }

	// Token: 0x06002EB6 RID: 11958 RVA: 0x000DF814 File Offset: 0x000DDA14
	public async UniTask InitAsync()
	{
		GameShutdown.ThrowIfRequested();
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		int total = LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>.Instance.initialSizeConfigs.List.Count;
		int index = 0;
		foreach (GameResAtom gameResAtom in LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>.Instance.initialSizeConfigs.List)
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

	// Token: 0x06002EB7 RID: 11959 RVA: 0x000DF857 File Offset: 0x000DDA57
	public static BakedChunkableObjectComponent Get(string pathToLoad)
	{
		return LazySingleton<BakedChunkableObjectPool>.Instance.GetInternal(pathToLoad);
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x000DF864 File Offset: 0x000DDA64
	public static void Release(string pathToLoad, BakedChunkableObjectComponent obj)
	{
		LazySingleton<BakedChunkableObjectPool>.Instance.ReleaseInternal(pathToLoad, obj);
	}

	// Token: 0x06002EB9 RID: 11961 RVA: 0x000DF872 File Offset: 0x000DDA72
	public static int TrimPaths(IEnumerable<string> paths, ScenePoolTrimPolicy policy)
	{
		return LazySingleton<BakedChunkableObjectPool>.Instance.TrimPathsInternal(paths, policy);
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x000DF880 File Offset: 0x000DDA80
	public void BakeSizesToInitialConfig()
	{
		foreach (string text in this.poolsDict.Keys)
		{
			LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>.Instance.SetSizeForPath(text, this.poolsDict[text].Objects.Count);
		}
	}

	// Token: 0x06002EBB RID: 11963 RVA: 0x000DF8F4 File Offset: 0x000DDAF4
	private BakedChunkableObjectComponent GetInternal(string pathToLoad)
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
		BakedChunkableObjectComponent orCreateObject = pool.GetOrCreateObject<BakedChunkableObjectComponent>();
		int num;
		this.borrowedCounts.TryGetValue(pathToLoad, out num);
		this.borrowedCounts[pathToLoad] = num + 1;
		return orCreateObject;
	}

	// Token: 0x06002EBC RID: 11964 RVA: 0x000DF944 File Offset: 0x000DDB44
	private Pool CreatePoolById(string pathToLoad)
	{
		if (GameShutdown.IsRequested)
		{
			return null;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(pathToLoad);
		asyncOperationHandle.WaitForCompletion();
		return this.FinishCreatePool(pathToLoad, asyncOperationHandle);
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x000DF974 File Offset: 0x000DDB74
	private UniTask<Pool> CreatePoolByIdAsync(string pathToLoad)
	{
		BakedChunkableObjectPool.<CreatePoolByIdAsync>d__15 <CreatePoolByIdAsync>d__;
		<CreatePoolByIdAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<Pool>.Create();
		<CreatePoolByIdAsync>d__.<>4__this = this;
		<CreatePoolByIdAsync>d__.pathToLoad = pathToLoad;
		<CreatePoolByIdAsync>d__.<>1__state = -1;
		<CreatePoolByIdAsync>d__.<>t__builder.Start<BakedChunkableObjectPool.<CreatePoolByIdAsync>d__15>(ref <CreatePoolByIdAsync>d__);
		return <CreatePoolByIdAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x000DF9C0 File Offset: 0x000DDBC0
	private Pool FinishCreatePool(string pathToLoad, AsyncOperationHandle<GameObject> handle)
	{
		Pool pool;
		if (this.poolsDict.TryGetValue(pathToLoad, out pool))
		{
			Addressables.Release<GameObject>(handle);
			return pool;
		}
		GameObject result = handle.Result;
		BakedChunkableObjectComponent bakedChunkableObjectComponent;
		if (!result.TryGetComponent<BakedChunkableObjectComponent>(out bakedChunkableObjectComponent))
		{
			bakedChunkableObjectComponent = result.AddComponent<BakedChunkableObjectComponentWithHorizontalSpr>();
		}
		Pool pool2 = LazyPooler.CreatePoolById(pathToLoad, bakedChunkableObjectComponent, 0, Pool.PoolType.ImmediateActivation, true, false, null);
		this.poolsDict.Add(pathToLoad, pool2);
		this.loadedHandles[pathToLoad] = handle;
		int sizeForPath = LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>.Instance.GetSizeForPath(pathToLoad);
		for (int i = 0; i < sizeForPath; i++)
		{
			pool2.AddObjectToPool();
		}
		return pool2;
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x000DFA4C File Offset: 0x000DDC4C
	private void ReleaseInternal(string pathToLoad, BakedChunkableObjectComponent obj)
	{
		int num;
		if (this.borrowedCounts.TryGetValue(pathToLoad, out num) && num > 0)
		{
			num--;
			if (num == 0)
			{
				this.borrowedCounts.Remove(pathToLoad);
			}
			else
			{
				this.borrowedCounts[pathToLoad] = num;
			}
		}
		Pool pool;
		if (!this.poolsDict.TryGetValue(pathToLoad, out pool))
		{
			if (obj != null)
			{
				global::UnityEngine.Object.Destroy(obj.gameObject);
			}
			return;
		}
		pool.ReleaseObject<BakedChunkableObjectComponent>(obj);
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x000DFABC File Offset: 0x000DDCBC
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
				int num2 = ((policy == ScenePoolTrimPolicy.Aggressive) ? 0 : LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>.Instance.GetSizeForPath(text));
				num += pool.TrimIdleToSize(num2);
				int num3;
				this.borrowedCounts.TryGetValue(text, out num3);
				if (policy == ScenePoolTrimPolicy.Aggressive && pool.Objects.Count == 0 && num3 <= 0)
				{
					this.UnloadAddressablePrefab(text);
				}
			}
		}
		return num;
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x000DFB68 File Offset: 0x000DDD68
	private void UnloadAddressablePrefab(string path)
	{
		this.poolsDict.Remove(path);
		this.borrowedCounts.Remove(path);
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

	// Token: 0x06002EC2 RID: 11970 RVA: 0x000DFBC3 File Offset: 0x000DDDC3
	public void Report(float value)
	{
		this.LocalProgress = Mathf.Clamp01(value);
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x000DFBD1 File Offset: 0x000DDDD1
	private void OnDestroy()
	{
		this.Clear();
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x000DFBDC File Offset: 0x000DDDDC
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
		this.borrowedCounts.Clear();
		foreach (KeyValuePair<string, AsyncOperationHandle<GameObject>> keyValuePair2 in this.loadedHandles)
		{
			if (keyValuePair2.Value.IsValid())
			{
				Addressables.Release<GameObject>(keyValuePair2.Value);
			}
		}
		this.loadedHandles.Clear();
	}

	// Token: 0x040025BF RID: 9663
	private const int YIELD_EVERY = 10;

	// Token: 0x040025C0 RID: 9664
	private Dictionary<string, Pool> poolsDict = new Dictionary<string, Pool>();

	// Token: 0x040025C1 RID: 9665
	private readonly Dictionary<string, AsyncOperationHandle<GameObject>> loadedHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();

	// Token: 0x040025C2 RID: 9666
	private readonly Dictionary<string, int> borrowedCounts = new Dictionary<string, int>();
}
