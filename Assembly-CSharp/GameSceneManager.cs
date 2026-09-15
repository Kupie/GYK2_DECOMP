using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders;

// Token: 0x0200070C RID: 1804
public class GameSceneManager : LazySingleton<GameSceneManager>
{
	// Token: 0x06002F80 RID: 12160 RVA: 0x000E3CF0 File Offset: 0x000E1EF0
	protected override void Awake()
	{
		base.Awake();
		this.loadedGameSceneIds.Clear();
		this.loadedGameScenes.Clear();
	}

	// Token: 0x1700075C RID: 1884
	// (get) Token: 0x06002F81 RID: 12161 RVA: 0x000E3D0E File Offset: 0x000E1F0E
	public List<GameScene> LoadedGameScenes
	{
		get
		{
			return this.loadedGameScenes;
		}
	}

	// Token: 0x1700075D RID: 1885
	// (get) Token: 0x06002F82 RID: 12162 RVA: 0x000E3D16 File Offset: 0x000E1F16
	public List<string> LoadedGameSceneIds
	{
		get
		{
			return this.loadedGameSceneIds;
		}
	}

	// Token: 0x06002F83 RID: 12163 RVA: 0x000E3D20 File Offset: 0x000E1F20
	public void LoadScene(string sceneId, Action onSceneLoaded = null)
	{
		if (sceneId == "MainScene")
		{
			return;
		}
		if (LazySceneManager.IsSceneLoadingOrLoaded(sceneId))
		{
			Debug.LogWarning("[GameSceneManager]: trying to load already loading or loaded GameScene [" + sceneId + "]");
			this.InvokeWhenGameSceneReady(sceneId, onSceneLoaded);
			return;
		}
		LazySceneManager.LoadScene(sceneId, delegate(SceneInstance sceneInstance)
		{
			this.HandleGameSceneLoadCompleted(sceneInstance, onSceneLoaded);
		});
		this.loadedGameSceneIds.Add(sceneId);
		Debug.Log("LoadScene: [" + sceneId + "]");
	}

	// Token: 0x06002F84 RID: 12164 RVA: 0x000E3DB0 File Offset: 0x000E1FB0
	public async UniTask LoadSceneAsync(string sceneId, IProgress<float> progress = null)
	{
		if (!(sceneId == "MainScene"))
		{
			while (LazySceneManager.IsSceneUnloading(sceneId))
			{
				GameShutdown.ThrowIfRequested();
				await UniTask.Yield();
			}
			if (LazySceneManager.IsSceneLoadingOrLoaded(sceneId))
			{
				Debug.LogWarning("[GameSceneManager]: trying to load already loading or loaded GameScene [" + sceneId + "]");
			}
			else
			{
				this.loadedGameSceneIds.Add(sceneId);
				Debug.Log("LoadSceneAsync: [" + sceneId + "]");
				SceneInstance sceneInstance = await LazySceneManager.LoadSceneAsync(sceneId, progress);
				bool isRequested = GameShutdown.IsRequested;
				if (isRequested || !sceneInstance.Scene.IsValid())
				{
					this.UnloadScene(sceneId, null);
					if (isRequested)
					{
						throw new OperationCanceledException("[GameSceneManager] GameScene [" + sceneId + "] load was cancelled by shutdown");
					}
					throw new Exception("[GameSceneManager] GameScene [" + sceneId + "] loaded with an invalid SceneInstance");
				}
				else
				{
					GameObject[] rootGameObjects = sceneInstance.Scene.GetRootGameObjects();
					int i = 0;
					while (i < rootGameObjects.Length)
					{
						GameScene gameScene;
						if (rootGameObjects[i].TryGetComponent<GameScene>(out gameScene))
						{
							this.loadedGameScenes.Add(gameScene);
							Awaitable<bool>.Awaiter awaiter = gameScene.WaitForStartCompleted(30f).GetAwaiter();
							if (!awaiter.IsCompleted)
							{
								await awaiter;
								Awaitable<bool>.Awaiter awaiter2;
								awaiter = awaiter2;
								awaiter2 = default(Awaitable<bool>.Awaiter);
							}
							if (!awaiter.GetResult())
							{
								Debug.LogWarning("[GameSceneManager] GameScene start was not completed for scene [" + gameScene.Id + "]");
								break;
							}
							break;
						}
						else
						{
							gameScene = null;
							i++;
						}
					}
				}
			}
		}
	}

	// Token: 0x06002F85 RID: 12165 RVA: 0x000E3E04 File Offset: 0x000E2004
	public void UnloadScene(string sceneId, Action onSceneUnloaded = null)
	{
		if (sceneId == "MainScene")
		{
			return;
		}
		Debug.Log("UnloadScene: [" + sceneId + "]");
		LazySceneManager.UnloadScene(sceneId, new Action<SceneInstance>(this.HandleGameSceneUnloadStarted), onSceneUnloaded);
		this.loadedGameSceneIds.Remove(sceneId);
	}

	// Token: 0x06002F86 RID: 12166 RVA: 0x000E3E58 File Offset: 0x000E2058
	public void UnloadAllScenes()
	{
		for (int i = this.loadedGameSceneIds.Count - 1; i >= 0; i--)
		{
			this.UnloadScene(this.loadedGameSceneIds[i], null);
		}
	}

	// Token: 0x06002F87 RID: 12167 RVA: 0x000E3E90 File Offset: 0x000E2090
	public bool DisabledUnloadOnTeleport(string sceneId)
	{
		if (LazySingleton<GameSceneManager>.Instance)
		{
			GameScene gameScene = LazySingleton<GameSceneManager>.Instance.LoadedGameScenes.FirstOrDefault((GameScene x) => x.Id == sceneId);
			return gameScene != null && gameScene.DisableUnloadOnTeleport;
		}
		return false;
	}

	// Token: 0x06002F88 RID: 12168 RVA: 0x000E3EE0 File Offset: 0x000E20E0
	private async void InvokeWhenGameSceneReady(string sceneId, Action onSceneLoadedCallback)
	{
		try
		{
			SceneInstance sceneInstance = await LazySceneManager.AwaitSceneLoadedAsync(sceneId);
			if (!this.loadedGameSceneIds.Contains(sceneId))
			{
				this.loadedGameSceneIds.Add(sceneId);
			}
			UniTask<GameScene>.Awaiter awaiter = this.RegisterGameSceneWhenReady(sceneId, sceneInstance).GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				UniTask<GameScene>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(UniTask<GameScene>.Awaiter);
			}
			if (awaiter.GetResult() == null)
			{
				Debug.LogError("[GameSceneManager] GameScene component not found for scene [" + sceneId + "]");
			}
			else if (onSceneLoadedCallback != null)
			{
				onSceneLoadedCallback();
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[GameSceneManager] Failed to wait for scene [" + sceneId + "]: " + ex.Message);
		}
	}

	// Token: 0x06002F89 RID: 12169 RVA: 0x000E3F28 File Offset: 0x000E2128
	private async void HandleGameSceneLoadCompleted(SceneInstance sceneInstance, Action onSceneLoadedCallback)
	{
		GameScene gameScene = null;
		GameObject[] rootGameObjects = sceneInstance.Scene.GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			if (rootGameObjects[i].TryGetComponent<GameScene>(out gameScene))
			{
				this.loadedGameScenes.Add(gameScene);
				break;
			}
		}
		bool flag = gameScene != null;
		if (flag)
		{
			Awaitable<bool>.Awaiter awaiter = gameScene.WaitForStartCompleted(30f).GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				Awaitable<bool>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(Awaitable<bool>.Awaiter);
			}
			flag = !awaiter.GetResult();
		}
		if (flag)
		{
			Debug.LogWarning("[GameSceneManager] GameScene start was not completed for scene [" + gameScene.Id + "]");
		}
		if (onSceneLoadedCallback != null)
		{
			onSceneLoadedCallback();
		}
	}

	// Token: 0x06002F8A RID: 12170 RVA: 0x000E3F70 File Offset: 0x000E2170
	private async UniTask<GameScene> RegisterGameSceneWhenReady(string sceneId, SceneInstance sceneInstance)
	{
		GameScene gameScene = this.loadedGameScenes.FirstOrDefault((GameScene x) => x.Id == sceneId);
		if (gameScene == null)
		{
			GameObject[] rootGameObjects = sceneInstance.Scene.GetRootGameObjects();
			int i = 0;
			while (i < rootGameObjects.Length)
			{
				if (rootGameObjects[i].TryGetComponent<GameScene>(out gameScene))
				{
					if (!this.loadedGameScenes.Contains(gameScene))
					{
						this.loadedGameScenes.Add(gameScene);
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}
		bool flag = gameScene != null;
		if (flag)
		{
			Awaitable<bool>.Awaiter awaiter = gameScene.WaitForStartCompleted(30f).GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				Awaitable<bool>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(Awaitable<bool>.Awaiter);
			}
			flag = !awaiter.GetResult();
		}
		if (flag)
		{
			Debug.LogWarning("[GameSceneManager] GameScene start was not completed for scene [" + gameScene.Id + "]");
		}
		return gameScene;
	}

	// Token: 0x06002F8B RID: 12171 RVA: 0x000E3FC4 File Offset: 0x000E21C4
	private void HandleGameSceneUnloadStarted(SceneInstance sceneInstance)
	{
		GameObject[] rootGameObjects = sceneInstance.Scene.GetRootGameObjects();
		for (int i = 0; i < rootGameObjects.Length; i++)
		{
			GameScene gameScene;
			if (rootGameObjects[i].TryGetComponent<GameScene>(out gameScene))
			{
				this.loadedGameScenes.Remove(gameScene);
				return;
			}
		}
	}

	// Token: 0x04002662 RID: 9826
	private List<string> loadedGameSceneIds = new List<string>();

	// Token: 0x04002663 RID: 9827
	private List<GameScene> loadedGameScenes = new List<GameScene>();
}
