using System;
using Cysharp.Threading.Tasks;
using FlowCanvas;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020004E6 RID: 1254
public static class FlowScriptAssetLoadPolicy
{
	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x060020D0 RID: 8400 RVA: 0x0009B7DE File Offset: 0x000999DE
	// (set) Token: 0x060020D1 RID: 8401 RVA: 0x0009B7E5 File Offset: 0x000999E5
	public static FlowScriptAssetLoadPolicyType Current { get; private set; }

	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x060020D2 RID: 8402 RVA: 0x0009B7ED File Offset: 0x000999ED
	public static bool UsesBackgroundPreload
	{
		get
		{
			return FlowScriptAssetLoadPolicy.Current == FlowScriptAssetLoadPolicyType.Cached;
		}
	}

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x060020D3 RID: 8403 RVA: 0x0009B7F7 File Offset: 0x000999F7
	public static float PreloadProgress
	{
		get
		{
			return FlowScriptAssetCache.LocalProgress;
		}
	}

	// Token: 0x060020D4 RID: 8404 RVA: 0x0009B7FE File Offset: 0x000999FE
	public static void SetPolicy(FlowScriptAssetLoadPolicyType policy)
	{
		if (FlowScriptAssetLoadPolicy.Current == policy)
		{
			return;
		}
		if (FlowScriptAssetLoadPolicy.Current == FlowScriptAssetLoadPolicyType.Cached)
		{
			FlowScriptAssetCache.ReleaseAll();
		}
		FlowScriptAssetLoadPolicy.Current = policy;
		D.LogColor(string.Format("Flow script asset load policy set to [{0}]", policy), "yellow");
	}

	// Token: 0x060020D5 RID: 8405 RVA: 0x0009B838 File Offset: 0x00099A38
	public static async UniTask PreloadAsync()
	{
		if (FlowScriptAssetLoadPolicy.UsesBackgroundPreload)
		{
			await FlowScriptAssetCache.LoadAllAsync();
		}
	}

	// Token: 0x060020D6 RID: 8406 RVA: 0x0009B873 File Offset: 0x00099A73
	public static FlowGraph Load(string graphPathWithoutExtension)
	{
		if (FlowScriptAssetLoadPolicy.Current == FlowScriptAssetLoadPolicyType.Cached)
		{
			return FlowScriptAssetCache.Get(graphPathWithoutExtension);
		}
		return FlowScriptAssetLoadPolicy.LoadOnDemand(graphPathWithoutExtension);
	}

	// Token: 0x060020D7 RID: 8407 RVA: 0x0009B88A File Offset: 0x00099A8A
	public static void ReleaseGraph(FlowGraph graph)
	{
		if (graph == null)
		{
			return;
		}
		if (FlowScriptAssetLoadPolicy.Current != FlowScriptAssetLoadPolicyType.Cached)
		{
			Addressables.Release<FlowGraph>(graph);
			return;
		}
		if (FlowScriptAssetCache.IsCached(graph))
		{
			return;
		}
		global::UnityEngine.Object.Destroy(graph);
	}

	// Token: 0x060020D8 RID: 8408 RVA: 0x0009B8B4 File Offset: 0x00099AB4
	private static FlowGraph LoadOnDemand(string graphPathWithoutExtension)
	{
		string text = graphPathWithoutExtension + ".asset";
		FlowGraph flowGraph;
		try
		{
			AsyncOperationHandle<FlowGraph> asyncOperationHandle = Addressables.LoadAssetAsync<FlowGraph>(text);
			asyncOperationHandle.WaitForCompletion();
			if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded || asyncOperationHandle.Result == null)
			{
				if (asyncOperationHandle.IsValid())
				{
					Addressables.Release<FlowGraph>(asyncOperationHandle);
				}
				Debug.LogError("Not found flow graph: " + text);
				flowGraph = null;
			}
			else
			{
				flowGraph = asyncOperationHandle.Result;
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
			flowGraph = null;
		}
		return flowGraph;
	}
}
