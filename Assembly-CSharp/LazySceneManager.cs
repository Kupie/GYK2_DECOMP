using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

// Token: 0x02000716 RID: 1814
public static class LazySceneManager
{
	// Token: 0x1700075E RID: 1886
	// (get) Token: 0x06002F9E RID: 12190 RVA: 0x000E48CA File Offset: 0x000E2ACA
	public static string LastLoadedSceneId
	{
		get
		{
			if (LazySceneManager.lazySceneInfos.Count == 0)
			{
				return "MainScene";
			}
			List<LazySceneInfo> list = LazySceneManager.lazySceneInfos;
			return list[list.Count - 1].sceneId;
		}
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x000E48F8 File Offset: 0x000E2AF8
	public static void LoadScene(string sceneId, Action<SceneInstance> loadCompleteCallback = null)
	{
		LazySceneInfo sceneInfo = new LazySceneInfo();
		sceneInfo.sceneId = sceneId;
		sceneInfo.status = SceneStatus.Loading;
		sceneInfo.onLoadedCallback = loadCompleteCallback;
		LazySceneManager.lazySceneInfos.Add(sceneInfo);
		Addressables.LoadSceneAsync(sceneId ?? "", LoadSceneMode.Additive, true, 100, SceneReleaseMode.ReleaseSceneWhenSceneUnloaded).Completed += delegate(AsyncOperationHandle<SceneInstance> handle)
		{
			LazySceneManager.OnSceneLoaded(handle, sceneInfo);
		};
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x000E4974 File Offset: 0x000E2B74
	public static UniTask<SceneInstance> LoadSceneAsync(string sceneId, IProgress<float> progress = null)
	{
		LazySceneManager.<LoadSceneAsync>d__4 <LoadSceneAsync>d__;
		<LoadSceneAsync>d__.<>t__builder = AsyncUniTaskMethodBuilder<SceneInstance>.Create();
		<LoadSceneAsync>d__.sceneId = sceneId;
		<LoadSceneAsync>d__.progress = progress;
		<LoadSceneAsync>d__.<>1__state = -1;
		<LoadSceneAsync>d__.<>t__builder.Start<LazySceneManager.<LoadSceneAsync>d__4>(ref <LoadSceneAsync>d__);
		return <LoadSceneAsync>d__.<>t__builder.Task;
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x000E49C0 File Offset: 0x000E2BC0
	public static AsyncOperationHandle UnloadScene(string sceneId, Action<SceneInstance> unloadStartCallback = null, Action unloadCompleteCallback = null)
	{
		AsyncOperationHandle asyncOperationHandle = default(AsyncOperationHandle);
		int i = 0;
		while (i < LazySceneManager.lazySceneInfos.Count)
		{
			LazySceneInfo sceneInfo = LazySceneManager.lazySceneInfos[i];
			if (sceneInfo.sceneId == sceneId)
			{
				if (!sceneInfo.sceneHandle.IsValid())
				{
					Debug.LogError("LazySceneManager: scene [" + sceneId + "] has no valid load handle, cannot unload.");
					break;
				}
				asyncOperationHandle = Addressables.UnloadSceneAsync(sceneInfo.sceneHandle, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects, true);
				if (unloadStartCallback != null)
				{
					unloadStartCallback(sceneInfo.sceneInstance);
				}
				asyncOperationHandle.Completed += delegate(AsyncOperationHandle handle)
				{
					LazySceneManager.OnSceneUnloaded(handle, sceneInfo, unloadCompleteCallback);
				};
				sceneInfo.status = SceneStatus.Unloading;
				break;
			}
			else
			{
				i++;
			}
		}
		return asyncOperationHandle;
	}

	// Token: 0x06002FA2 RID: 12194 RVA: 0x000E4AA8 File Offset: 0x000E2CA8
	public static bool IsSceneUnloading(string sceneId)
	{
		foreach (LazySceneInfo lazySceneInfo in LazySceneManager.lazySceneInfos)
		{
			if (lazySceneInfo.sceneId == sceneId)
			{
				return lazySceneInfo.status == SceneStatus.Unloading;
			}
		}
		return false;
	}

	// Token: 0x06002FA3 RID: 12195 RVA: 0x000E4B10 File Offset: 0x000E2D10
	public static bool IsSceneLoadingOrLoaded(string sceneId)
	{
		using (List<LazySceneInfo>.Enumerator enumerator = LazySceneManager.lazySceneInfos.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.sceneId == sceneId)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002FA4 RID: 12196 RVA: 0x000E4B70 File Offset: 0x000E2D70
	public static async UniTask<SceneInstance> AwaitSceneLoadedAsync(string sceneId)
	{
		for (;;)
		{
			foreach (LazySceneInfo lazySceneInfo in LazySceneManager.lazySceneInfos)
			{
				if (!(lazySceneInfo.sceneId != sceneId))
				{
					SceneStatus status = lazySceneInfo.status;
					if (status == SceneStatus.LoadingFailed)
					{
						throw new Exception("Scene [" + sceneId + "] failed to load");
					}
					if (status == SceneStatus.Loaded)
					{
						return lazySceneInfo.sceneInstance;
					}
				}
			}
			await UniTask.Yield();
		}
		SceneInstance sceneInstance;
		return sceneInstance;
	}

	// Token: 0x06002FA5 RID: 12197 RVA: 0x000E4BB4 File Offset: 0x000E2DB4
	private static void OnSceneLoaded(AsyncOperationHandle<SceneInstance> asyncOperationHandle, LazySceneInfo sceneInfo)
	{
		sceneInfo.sceneHandle = asyncOperationHandle;
		if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
		{
			Debug.LogError("LazySceneManager: error during scene [" + sceneInfo.sceneId + "] loading callback, unsuccessful.");
			sceneInfo.status = SceneStatus.LoadingFailed;
			sceneInfo.onLoadedCallback = null;
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<SceneInstance>(asyncOperationHandle);
			}
			LazySceneManager.lazySceneInfos.Remove(sceneInfo);
			return;
		}
		sceneInfo.sceneInstance = asyncOperationHandle.Result;
		sceneInfo.status = SceneStatus.Loaded;
		Action<SceneInstance> onLoadedCallback = sceneInfo.onLoadedCallback;
		if (onLoadedCallback != null)
		{
			onLoadedCallback(sceneInfo.sceneInstance);
		}
		sceneInfo.onLoadedCallback = null;
	}

	// Token: 0x06002FA6 RID: 12198 RVA: 0x000E4C50 File Offset: 0x000E2E50
	private static void OnSceneUnloaded(AsyncOperationHandle operationHandle, LazySceneInfo sceneInfo, Action OnUnloaded)
	{
		Debug.Log(string.Format("{0} Status: {1}", "OnSceneUnloaded", operationHandle.Status));
		if (operationHandle.Status == AsyncOperationStatus.Succeeded)
		{
			LazySceneManager.lazySceneInfos.Remove(sceneInfo);
		}
		else
		{
			sceneInfo.status = SceneStatus.Loaded;
		}
		if (OnUnloaded != null)
		{
			OnUnloaded();
		}
	}

	// Token: 0x0400268B RID: 9867
	private static List<LazySceneInfo> lazySceneInfos = new List<LazySceneInfo>();
}
