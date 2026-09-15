using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000B0F RID: 2831
public class WgoPartPool : LazySingleton<WgoPartPool>, IProgress<float>
{
	// Token: 0x17000B57 RID: 2903
	// (get) Token: 0x06004B5D RID: 19293 RVA: 0x00163FDE File Offset: 0x001621DE
	// (set) Token: 0x06004B5E RID: 19294 RVA: 0x00163FE6 File Offset: 0x001621E6
	public float LocalProgress { get; private set; }

	// Token: 0x06004B5F RID: 19295 RVA: 0x00163FF0 File Offset: 0x001621F0
	public async UniTask InitAsync()
	{
		GameShutdown.ThrowIfRequested();
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		float totalConfigsCount = (float)LazySingletonSO<WgoPartPoolInitialSizesConfig>.Instance.initialSizeConfigs.List.Count;
		int currentConfigIndex = 0;
		foreach (GameResAtom atom in LazySingletonSO<WgoPartPoolInitialSizesConfig>.Instance.initialSizeConfigs.List)
		{
			GameShutdown.ThrowIfRequested();
			string addressableKey = atom.type;
			GameObject prefab = this.LoadPrefabSync(addressableKey);
			this.Report((float)currentConfigIndex / totalConfigsCount);
			int num = currentConfigIndex;
			currentConfigIndex = num + 1;
			if (prefab == null)
			{
				Debug.LogError("[WgoPartPool] Failed to load prefab at [" + addressableKey + "]");
			}
			else
			{
				Stack<WgoPart> stack;
				int num2 = (this.pools.TryGetValue(addressableKey, out stack) ? stack.Count : 0);
				int i = num2;
				while ((float)i < atom.value)
				{
					GameShutdown.ThrowIfRequested();
					WgoPart wgoPart = global::UnityEngine.Object.Instantiate<WgoPart>(prefab.GetComponent<WgoPart>());
					wgoPart.CleanupChunkableComponents();
					wgoPart.PooledAddressableKey = addressableKey;
					this.Release(addressableKey, wgoPart);
					await BackgroundLoading.YieldIfNeeded(i + 1, 10);
					num = i;
					i = num + 1;
				}
				addressableKey = null;
				prefab = null;
				atom = null;
			}
		}
		List<GameResAtom>.Enumerator enumerator = default(List<GameResAtom>.Enumerator);
	}

	// Token: 0x06004B60 RID: 19296 RVA: 0x00164034 File Offset: 0x00162234
	public WgoPart GetSync(string addressableKey, Wgo wgo)
	{
		Stack<WgoPart> stack;
		GameObject gameObject;
		if (this.pools.TryGetValue(addressableKey, out stack) && stack.Count > 0)
		{
			WgoPart wgoPart = stack.Pop();
			wgoPart.transform.SetParent(wgo.transform);
			gameObject = this.LoadPrefabSync(addressableKey);
			if (gameObject == null)
			{
				Debug.LogError("[WgoPartPool] Failed to load prefab at [" + addressableKey + "]");
			}
			else
			{
				wgoPart.transform.localScale = gameObject.transform.localScale;
			}
			wgoPart.transform.localPosition = Vector3.zero;
			wgoPart.ReInitFromPool(wgo);
			return wgoPart;
		}
		gameObject = this.LoadPrefabSync(addressableKey);
		if (gameObject == null)
		{
			Debug.LogError("[WgoPartPool] Failed to load prefab at [" + addressableKey + "]");
			return null;
		}
		WgoPart wgoPart2 = global::UnityEngine.Object.Instantiate<WgoPart>(gameObject.GetComponent<WgoPart>(), wgo.transform);
		wgoPart2.transform.localScale = gameObject.transform.localScale;
		wgoPart2.CleanupChunkableComponents();
		wgoPart2.ReInitFromPool(wgo);
		wgoPart2.PooledAddressableKey = addressableKey;
		return wgoPart2;
	}

	// Token: 0x06004B61 RID: 19297 RVA: 0x0016412C File Offset: 0x0016232C
	public async Awaitable<WgoPart> GetAsync(string addressableKey, Wgo wgo)
	{
		Stack<WgoPart> stack;
		WgoPart wgoPart2;
		if (this.pools.TryGetValue(addressableKey, out stack) && stack.Count > 0)
		{
			WgoPart wgoPart = stack.Pop();
			wgoPart.transform.SetParent(wgo.transform);
			wgoPart.transform.localPosition = Vector3.zero;
			wgoPart.ReInitFromPool(wgo);
			wgoPart2 = wgoPart;
		}
		else
		{
			GameObject gameObject = await this.LoadPrefab(addressableKey);
			if (gameObject == null)
			{
				Debug.LogError("[WgoPartPool] Failed to load prefab at [" + addressableKey + "]");
				wgoPart2 = null;
			}
			else
			{
				WgoPart wgoPart3 = global::UnityEngine.Object.Instantiate<WgoPart>(gameObject.GetComponent<WgoPart>(), wgo.transform);
				wgoPart3.CleanupChunkableComponents();
				wgoPart3.ReInitFromPool(wgo);
				wgoPart3.PooledAddressableKey = addressableKey;
				wgoPart2 = wgoPart3;
			}
		}
		return wgoPart2;
	}

	// Token: 0x06004B62 RID: 19298 RVA: 0x00164180 File Offset: 0x00162380
	public void Release(string addressableKey, WgoPart wgoPart)
	{
		if (wgoPart == null)
		{
			return;
		}
		wgoPart.DeInitForPool();
		wgoPart.gameObject.SetActive(false);
		wgoPart.transform.SetParent(base.transform);
		Stack<WgoPart> stack;
		if (!this.pools.TryGetValue(addressableKey, out stack))
		{
			stack = new Stack<WgoPart>();
			this.pools[addressableKey] = stack;
		}
		stack.Push(wgoPart);
	}

	// Token: 0x06004B63 RID: 19299 RVA: 0x001641E4 File Offset: 0x001623E4
	public static int TrimPaths(IEnumerable<string> paths, ScenePoolTrimPolicy policy)
	{
		return LazySingleton<WgoPartPool>.Instance.TrimPathsInternal(paths, policy);
	}

	// Token: 0x06004B64 RID: 19300 RVA: 0x001641F2 File Offset: 0x001623F2
	public static string GetAddressableKey(string partAssetId)
	{
		if (string.IsNullOrEmpty(partAssetId))
		{
			return null;
		}
		return "Assets/AddressableAssets/WGOs/" + partAssetId + ".prefab";
	}

	// Token: 0x06004B65 RID: 19301 RVA: 0x00164210 File Offset: 0x00162410
	public static void CollectAddressableKeysForWgoData(WgoData wgoData, HashSet<string> paths)
	{
		if (wgoData == null || paths == null)
		{
			return;
		}
		string addressableKey = WgoPartPool.GetAddressableKey((wgoData.Definition != null) ? wgoData.Definition.ResolveAssetId(wgoData.id, wgoData) : wgoData.id);
		if (!string.IsNullOrEmpty(addressableKey))
		{
			paths.Add(addressableKey);
		}
		List<WgoPartData> additionalWgoPartsData = wgoData.AdditionalWgoPartsData;
		for (int i = 0; i < additionalWgoPartsData.Count; i++)
		{
			string addressableKey2 = WgoPartPool.GetAddressableKey(additionalWgoPartsData[i].id);
			if (!string.IsNullOrEmpty(addressableKey2))
			{
				paths.Add(addressableKey2);
			}
		}
	}

	// Token: 0x06004B66 RID: 19302 RVA: 0x00164298 File Offset: 0x00162498
	public void Clear()
	{
		foreach (KeyValuePair<string, Stack<WgoPart>> keyValuePair in this.pools)
		{
			foreach (WgoPart wgoPart in keyValuePair.Value)
			{
				if (wgoPart != null)
				{
					global::UnityEngine.Object.Destroy(wgoPart.gameObject);
				}
			}
		}
		this.pools.Clear();
		foreach (KeyValuePair<string, AsyncOperationHandle<GameObject>> keyValuePair2 in this.loadedHandles)
		{
			if (keyValuePair2.Value.IsValid())
			{
				Addressables.Release<GameObject>(keyValuePair2.Value);
			}
		}
		this.loadedHandles.Clear();
	}

	// Token: 0x06004B67 RID: 19303 RVA: 0x001643A8 File Offset: 0x001625A8
	private GameObject LoadPrefabSync(string addressableKey)
	{
		if (GameShutdown.IsRequested)
		{
			return null;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle;
		if (this.loadedHandles.TryGetValue(addressableKey, out asyncOperationHandle) && asyncOperationHandle.IsValid())
		{
			if (!asyncOperationHandle.IsDone)
			{
				asyncOperationHandle.WaitForCompletion();
			}
			if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && asyncOperationHandle.Result != null)
			{
				return asyncOperationHandle.Result;
			}
			this.loadedHandles.Remove(addressableKey);
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<GameObject>(asyncOperationHandle);
			}
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle2 = this.LoadPrefabAsync(addressableKey);
		GameObject gameObject = asyncOperationHandle2.WaitForCompletion();
		if (asyncOperationHandle2.Status == AsyncOperationStatus.Failed || gameObject == null)
		{
			this.loadedHandles.Remove(addressableKey);
			if (asyncOperationHandle2.IsValid())
			{
				Addressables.Release<GameObject>(asyncOperationHandle2);
			}
			Debug.LogError("[WgoPartPool] Addressable load failed for [" + addressableKey + "]");
			return null;
		}
		this.loadedHandles[addressableKey] = asyncOperationHandle2;
		return gameObject;
	}

	// Token: 0x06004B68 RID: 19304 RVA: 0x0016448C File Offset: 0x0016268C
	private UniTask<GameObject> LoadPrefab(string addressableKey)
	{
		WgoPartPool.<LoadPrefab>d__17 <LoadPrefab>d__;
		<LoadPrefab>d__.<>t__builder = AsyncUniTaskMethodBuilder<GameObject>.Create();
		<LoadPrefab>d__.<>4__this = this;
		<LoadPrefab>d__.addressableKey = addressableKey;
		<LoadPrefab>d__.<>1__state = -1;
		<LoadPrefab>d__.<>t__builder.Start<WgoPartPool.<LoadPrefab>d__17>(ref <LoadPrefab>d__);
		return <LoadPrefab>d__.<>t__builder.Task;
	}

	// Token: 0x06004B69 RID: 19305 RVA: 0x001644D8 File Offset: 0x001626D8
	private AsyncOperationHandle<GameObject> LoadPrefabAsync(string addressableKey)
	{
		AsyncOperationHandle<GameObject> asyncOperationHandle;
		if (this.loadedHandles.TryGetValue(addressableKey, out asyncOperationHandle) && asyncOperationHandle.IsValid())
		{
			return asyncOperationHandle;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle2 = Addressables.LoadAssetAsync<GameObject>(addressableKey);
		this.loadedHandles[addressableKey] = asyncOperationHandle2;
		return asyncOperationHandle2;
	}

	// Token: 0x06004B6A RID: 19306 RVA: 0x00164515 File Offset: 0x00162715
	private void OnDestroy()
	{
		this.Clear();
	}

	// Token: 0x06004B6B RID: 19307 RVA: 0x00164520 File Offset: 0x00162720
	private int TrimPathsInternal(IEnumerable<string> paths, ScenePoolTrimPolicy policy)
	{
		if (paths == null)
		{
			return 0;
		}
		int num = 0;
		foreach (string text in paths)
		{
			Stack<WgoPart> stack;
			if (!string.IsNullOrEmpty(text) && this.pools.TryGetValue(text, out stack))
			{
				int num2 = ((policy == ScenePoolTrimPolicy.Aggressive) ? 0 : LazySingletonSO<WgoPartPoolInitialSizesConfig>.Instance.GetSizeForPath(text));
				while (stack.Count > num2)
				{
					WgoPart wgoPart = stack.Pop();
					if (wgoPart != null)
					{
						global::UnityEngine.Object.Destroy(wgoPart.gameObject);
					}
					num++;
				}
				if (policy == ScenePoolTrimPolicy.Aggressive && stack.Count == 0)
				{
					this.UnloadAddressablePrefab(text);
				}
			}
		}
		return num;
	}

	// Token: 0x06004B6C RID: 19308 RVA: 0x001645D4 File Offset: 0x001627D4
	private void UnloadAddressablePrefab(string path)
	{
		this.pools.Remove(path);
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

	// Token: 0x06004B6D RID: 19309 RVA: 0x0016461C File Offset: 0x0016281C
	public void BakeSizesToInitialConfig()
	{
		foreach (string text in this.pools.Keys)
		{
			LazySingletonSO<WgoPartPoolInitialSizesConfig>.Instance.SetSizeForPath(text, this.pools[text].Count);
		}
	}

	// Token: 0x06004B6E RID: 19310 RVA: 0x0016468C File Offset: 0x0016288C
	public void Report(float value)
	{
		this.LocalProgress = Mathf.Clamp01(value);
	}

	// Token: 0x04003CB9 RID: 15545
	private const int YIELD_EVERY = 10;

	// Token: 0x04003CBA RID: 15546
	private const string ADDRESSABLE_KEY_PREFIX = "Assets/AddressableAssets/WGOs/";

	// Token: 0x04003CBB RID: 15547
	private readonly Dictionary<string, Stack<WgoPart>> pools = new Dictionary<string, Stack<WgoPart>>();

	// Token: 0x04003CBC RID: 15548
	private readonly Dictionary<string, AsyncOperationHandle<GameObject>> loadedHandles = new Dictionary<string, AsyncOperationHandle<GameObject>>();
}
