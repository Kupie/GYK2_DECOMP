using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using FlowCanvas;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020004E1 RID: 1249
public class FlowScriptAssetCache : LazySingleton<FlowScriptAssetCache>
{
	// Token: 0x1700055E RID: 1374
	// (get) Token: 0x060020B6 RID: 8374 RVA: 0x0009AFAC File Offset: 0x000991AC
	public static bool IsLoaded
	{
		get
		{
			return LazySingleton<FlowScriptAssetCache>.Instance.isLoaded;
		}
	}

	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x060020B7 RID: 8375 RVA: 0x0009AFB8 File Offset: 0x000991B8
	public static float LocalProgress
	{
		get
		{
			return LazySingleton<FlowScriptAssetCache>.Instance.localProgress;
		}
	}

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x060020B8 RID: 8376 RVA: 0x0009AFC4 File Offset: 0x000991C4
	public static int LoadedCount
	{
		get
		{
			return LazySingleton<FlowScriptAssetCache>.Instance.graphsByAssetPath.Count;
		}
	}

	// Token: 0x060020B9 RID: 8377 RVA: 0x0009AFD8 File Offset: 0x000991D8
	public static async UniTask LoadAllAsync()
	{
		await LazySingleton<FlowScriptAssetCache>.Instance.LoadAllAsyncInternal();
	}

	// Token: 0x060020BA RID: 8378 RVA: 0x0009B013 File Offset: 0x00099213
	public static FlowGraph Get(string graphPathWithoutExtension)
	{
		return LazySingleton<FlowScriptAssetCache>.Instance.GetInternal(graphPathWithoutExtension);
	}

	// Token: 0x060020BB RID: 8379 RVA: 0x0009B020 File Offset: 0x00099220
	public static void EnsureLoaded(string graphPathWithoutExtension)
	{
		LazySingleton<FlowScriptAssetCache>.Instance.EnsureLoadedInternal(graphPathWithoutExtension);
	}

	// Token: 0x060020BC RID: 8380 RVA: 0x0009B02D File Offset: 0x0009922D
	public static bool IsCached(FlowGraph graph)
	{
		return LazySingleton<FlowScriptAssetCache>.Instance.IsCachedInternal(graph);
	}

	// Token: 0x060020BD RID: 8381 RVA: 0x0009B03A File Offset: 0x0009923A
	public static void ReleaseAll()
	{
		LazySingleton<FlowScriptAssetCache>.Instance.ReleaseAllInternal();
	}

	// Token: 0x060020BE RID: 8382 RVA: 0x0009B048 File Offset: 0x00099248
	private async UniTask LoadAllAsyncInternal()
	{
		if (!this.isLoaded)
		{
			GameShutdown.ThrowIfRequested();
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			List<string> addresses = FlowScriptAssetCache.CollectAddresses();
			if (addresses.Count == 0)
			{
				this.isLoaded = true;
				this.localProgress = 1f;
			}
			else
			{
				for (int i = 0; i < addresses.Count; i++)
				{
					GameShutdown.ThrowIfRequested();
					string text = addresses[i];
					if (!this.graphsByAssetPath.ContainsKey(text))
					{
						await this.LoadAndCacheAsync(text);
					}
					this.UpdateProgress(i + 1, addresses.Count);
					await BackgroundLoading.YieldIfNeeded(i + 1, 5);
				}
				this.isLoaded = true;
				Debug.Log(string.Format("FlowScriptAssetCache loaded {0}/{1} flow script assets.", this.graphsByAssetPath.Count, addresses.Count));
			}
		}
	}

	// Token: 0x060020BF RID: 8383 RVA: 0x0009B08C File Offset: 0x0009928C
	private FlowGraph GetInternal(string graphPathWithoutExtension)
	{
		string text = graphPathWithoutExtension + ".asset";
		FlowGraph flowGraph;
		if (!this.graphsByAssetPath.TryGetValue(text, out flowGraph))
		{
			Debug.LogError("Flow script asset is not preloaded in cache: " + text);
			return null;
		}
		return global::UnityEngine.Object.Instantiate<FlowGraph>(flowGraph);
	}

	// Token: 0x060020C0 RID: 8384 RVA: 0x0009B0D0 File Offset: 0x000992D0
	private void EnsureLoadedInternal(string graphPathWithoutExtension)
	{
		string text = graphPathWithoutExtension + ".asset";
		if (this.graphsByAssetPath.ContainsKey(text))
		{
			return;
		}
		this.LoadAndCacheAsync(text).GetAwaiter().GetResult();
	}

	// Token: 0x060020C1 RID: 8385 RVA: 0x0009B10F File Offset: 0x0009930F
	private bool IsCachedInternal(FlowGraph graph)
	{
		return graph != null && this.cachedGraphs.Contains(graph);
	}

	// Token: 0x060020C2 RID: 8386 RVA: 0x0009B128 File Offset: 0x00099328
	private void ReleaseAllInternal()
	{
		foreach (AsyncOperationHandle<FlowGraph> asyncOperationHandle in this.handles)
		{
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<FlowGraph>(asyncOperationHandle);
			}
		}
		this.handles.Clear();
		this.graphsByAssetPath.Clear();
		this.cachedGraphs.Clear();
		this.isLoaded = false;
		this.localProgress = 0f;
	}

	// Token: 0x060020C3 RID: 8387 RVA: 0x0009B1B8 File Offset: 0x000993B8
	private UniTask LoadAndCacheAsync(string address)
	{
		FlowScriptAssetCache.<LoadAndCacheAsync>d__23 <LoadAndCacheAsync>d__;
		<LoadAndCacheAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder.Create();
		<LoadAndCacheAsync>d__.<>4__this = this;
		<LoadAndCacheAsync>d__.address = address;
		<LoadAndCacheAsync>d__.<>1__state = -1;
		<LoadAndCacheAsync>d__.<>t__builder.Start<FlowScriptAssetCache.<LoadAndCacheAsync>d__23>(ref <LoadAndCacheAsync>d__);
		return <LoadAndCacheAsync>d__.<>t__builder.Task;
	}

	// Token: 0x060020C4 RID: 8388 RVA: 0x0009B203 File Offset: 0x00099403
	private void RegisterLoadedAsset(string address, AsyncOperationHandle<FlowGraph> handle)
	{
		this.handles.Add(handle);
		this.graphsByAssetPath[address] = handle.Result;
		this.cachedGraphs.Add(handle.Result);
	}

	// Token: 0x060020C5 RID: 8389 RVA: 0x0009B238 File Offset: 0x00099438
	private static List<string> CollectAddresses()
	{
		List<string> list = new List<string>();
		foreach (IResourceLocator resourceLocator in Addressables.ResourceLocators)
		{
			foreach (object obj in resourceLocator.Keys)
			{
				string text = obj as string;
				if (text != null && text.EndsWith(".asset", StringComparison.Ordinal) && FlowScriptAssetCache.IsPreloadPath(text) && !list.Contains(text))
				{
					list.Add(text);
				}
			}
		}
		return list;
	}

	// Token: 0x060020C6 RID: 8390 RVA: 0x0009B2E8 File Offset: 0x000994E8
	private static bool IsPreloadPath(string address)
	{
		foreach (string text in FlowScriptAssetCache.PreloadPathPrefixes)
		{
			if (address.StartsWith(text, StringComparison.Ordinal))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x0009B31A File Offset: 0x0009951A
	private void UpdateProgress(int loaded, int total)
	{
		this.localProgress = ((total > 0) ? ((float)loaded / (float)total) : 1f);
	}

	// Token: 0x04001D78 RID: 7544
	private const int YIELD_EVERY = 5;

	// Token: 0x04001D79 RID: 7545
	private static readonly string[] PreloadPathPrefixes = new string[] { "Assets/AddressableAssets/VisualScripts/GlobalScripts/", "Assets/AddressableAssets/VisualScripts/WGODataScripts/" };

	// Token: 0x04001D7A RID: 7546
	private readonly Dictionary<string, FlowGraph> graphsByAssetPath = new Dictionary<string, FlowGraph>(StringComparer.Ordinal);

	// Token: 0x04001D7B RID: 7547
	private readonly HashSet<FlowGraph> cachedGraphs = new HashSet<FlowGraph>();

	// Token: 0x04001D7C RID: 7548
	private readonly List<AsyncOperationHandle<FlowGraph>> handles = new List<AsyncOperationHandle<FlowGraph>>();

	// Token: 0x04001D7D RID: 7549
	private bool isLoaded;

	// Token: 0x04001D7E RID: 7550
	private float localProgress;
}
