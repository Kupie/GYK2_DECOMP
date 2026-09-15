using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
using DG.Tweening;
using LazyBearTechnology.Preloader;
using Rewired;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000C40 RID: 3136
	public class ConsolesManager : LazySingleton<ConsolesManager>
	{
		// Token: 0x17000BC2 RID: 3010
		// (get) Token: 0x06004FE7 RID: 20455 RVA: 0x001786B1 File Offset: 0x001768B1
		public static bool IsUserInitializationFinished
		{
			get
			{
				return LazySingleton<ConsolesManager>.Instance.isUserInitializationFinished;
			}
		}

		// Token: 0x17000BC3 RID: 3011
		// (get) Token: 0x06004FE8 RID: 20456 RVA: 0x001786BD File Offset: 0x001768BD
		// (set) Token: 0x06004FE9 RID: 20457 RVA: 0x001786C4 File Offset: 0x001768C4
		public static bool IsStartupPreloadOverlayActive { get; private set; }

		// Token: 0x17000BC4 RID: 3012
		// (get) Token: 0x06004FEA RID: 20458 RVA: 0x001786CC File Offset: 0x001768CC
		public UIPreloadOverlay PreloadOverlay
		{
			get
			{
				return this.preloadOverlay;
			}
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x001786D4 File Offset: 0x001768D4
		protected override void Awake()
		{
			UserEnvironment.LogUserEnvironment();
			this.lazyPreloader.Run(this.OnFinishedLazyPreloaderCoroutine(), null, false);
			LazySingletonSO<LazyApplicationSettings>.SetReference(this.LoadConfig<LazyApplicationSettings>(this.lazyApplicationSettingsRef));
			LazySingletonSO<TMPShaderSetup>.SetReference(this.LoadConfig<TMPShaderSetup>(this.tmpShaderSetupRef));
			LazySingletonSO<GlobalResources>.SetReference(this.LoadConfig<GlobalResources>(this.globalResourcesRef));
			LazySingletonSO<GameResDisplayConfig>.SetReference(this.LoadConfig<GameResDisplayConfig>(this.gameResDisplayConfigRef));
			LazySingletonSO<GamepadTypeData>.SetReference(this.LoadConfig<GamepadTypeData>(this.gamepadTypeDataRef));
			LazySingletonSO<GameInfo>.SetReference(this.LoadConfig<GameInfo>(this.gameInfoRef));
			LazySingletonSO<GameBindings>.SetReference(this.LoadConfig<GameBindings>(this.gameBindingsRef));
			LazySingletonSO<EasySpritesCollection>.SetReference(this.LoadConfig<EasySpritesCollection>(this.easySpritesCollectionRef));
			LazyAudio.BindAudioConfig(this.LoadConfig<AudioConfig>(this.audioConfigRef));
			this.Init();
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x00178798 File Offset: 0x00176998
		private T LoadConfig<T>(AssetReferenceT<T> assetReference) where T : ScriptableObject
		{
			AsyncOperationHandle<T> asyncOperationHandle = default(AsyncOperationHandle<T>);
			T t = AddressableUtils.LoadAssetReferenceSync<T>(assetReference, ref asyncOperationHandle);
			if (asyncOperationHandle.IsValid())
			{
				this.configHandles.Add(asyncOperationHandle);
			}
			return t;
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001787CF File Offset: 0x001769CF
		private UniTask<T> LoadConfigAsync<T>(AssetReferenceT<T> assetReference) where T : ScriptableObject
		{
			return this.LoadConfigAsyncInternal<T>(assetReference);
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x001787D8 File Offset: 0x001769D8
		private UniTask<T> LoadConfigAsyncInternal<T>(AssetReferenceT<T> assetReference) where T : ScriptableObject
		{
			ConsolesManager.<LoadConfigAsyncInternal>d__53<T> <LoadConfigAsyncInternal>d__;
			<LoadConfigAsyncInternal>d__.<>t__builder = AsyncUniTaskMethodBuilder<T>.Create();
			<LoadConfigAsyncInternal>d__.<>4__this = this;
			<LoadConfigAsyncInternal>d__.assetReference = assetReference;
			<LoadConfigAsyncInternal>d__.<>1__state = -1;
			<LoadConfigAsyncInternal>d__.<>t__builder.Start<ConsolesManager.<LoadConfigAsyncInternal>d__53<T>>(ref <LoadConfigAsyncInternal>d__);
			return <LoadConfigAsyncInternal>d__.<>t__builder.Task;
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x00178824 File Offset: 0x00176A24
		private async UniTask LoadConfigAndSetReference<T>(AssetReferenceT<T> assetReference, Action<T> setReference) where T : ScriptableObject
		{
			T t = await this.LoadConfigAsync<T>(assetReference);
			GameShutdown.ThrowIfRequested();
			setReference(t);
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x00178878 File Offset: 0x00176A78
		private async UniTask LoadLazyTerrainMeshCollectionAsync()
		{
			object obj = await this.LoadConfigAsync<LazyTerrainMeshCollection>(this.lazyTerrainMeshCollectionRef);
			GameShutdown.ThrowIfRequested();
			LazySingletonSO<LazyTerrainMeshCollection>.SetReference(obj);
			if (obj != null)
			{
				obj.Runtime_StripAllMeshCpuData();
			}
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001788BC File Offset: 0x00176ABC
		private UniTask LoadDeferredConfigsAsync()
		{
			return UniTask.WhenAll(new UniTask[]
			{
				this.LoadConfigAndSetReference<ZombieCustomizationConfig>(this.zombieCustomizationConfigRef, new Action<ZombieCustomizationConfig>(LazySingletonSO<ZombieCustomizationConfig>.SetReference)),
				this.LoadConfigAndSetReference<WgoPartPoolInitialSizesConfig>(this.wgoPartPoolInitialSizesConfigRef, new Action<WgoPartPoolInitialSizesConfig>(LazySingletonSO<WgoPartPoolInitialSizesConfig>.SetReference)),
				this.LoadConfigAndSetReference<WgoPartBakedDataCollection>(this.wgoPartBakedDataCollectionRef, new Action<WgoPartBakedDataCollection>(LazySingletonSerializedSO<WgoPartBakedDataCollection>.SetReference)),
				this.LoadConfigAndSetReference<VoiceOverSettings>(this.voiceOverSettingsRef, new Action<VoiceOverSettings>(LazySingletonSO<VoiceOverSettings>.SetReference)),
				this.LoadConfigAndSetReference<SurfaceStepSoundSettings>(this.stepSoundSettingsRef, new Action<SurfaceStepSoundSettings>(LazySingletonSO<SurfaceStepSoundSettings>.SetReference)),
				this.LoadConfigAndSetReference<NPCLifeSimulatorConfiguration>(this.npcLifeSimulatorConfigurationRef, new Action<NPCLifeSimulatorConfiguration>(LazySingletonSO<NPCLifeSimulatorConfiguration>.SetReference)),
				this.LoadLazyTerrainMeshCollectionAsync(),
				this.LoadConfigAndSetReference<DeformingGrassSettings>(this.deformingGrassSettingsRef, new Action<DeformingGrassSettings>(LazySingletonSO<DeformingGrassSettings>.SetReference)),
				this.LoadConfigAndSetReference<ConstructorPartPoolInitialSizesConfig>(this.constructorPartPoolInitialSizesConfigRef, new Action<ConstructorPartPoolInitialSizesConfig>(LazySingletonSO<ConstructorPartPoolInitialSizesConfig>.SetReference)),
				this.LoadConfigAndSetReference<ConstructorPartBoundsConfig>(this.constructorPartBoundsConfigRef, new Action<ConstructorPartBoundsConfig>(LazySingletonSO<ConstructorPartBoundsConfig>.SetReference)),
				this.LoadConfigAndSetReference<BakedChunkableObjectPoolInitialSizesConfig>(this.bakedChunkableObjectPoolInitialSizesConfigRef, new Action<BakedChunkableObjectPoolInitialSizesConfig>(LazySingletonSO<BakedChunkableObjectPoolInitialSizesConfig>.SetReference))
			});
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x00178A1C File Offset: 0x00176C1C
		private void OnDestroy()
		{
			Debug.Log(string.Format("#shutdown# ConsolesManager.OnDestroy (isQuitting:[{0}])", GameShutdown.IsQuitting));
			ConsolesManager.IsStartupPreloadOverlayActive = false;
			GameShutdown.Resumed -= this.OnGameShutdownResumed;
			GameShutdown.SetQuitBlocker(null);
			if (!GameShutdown.IsQuitting)
			{
				this.ReleaseStartupHandles();
				this.ReleaseConfigHandles();
			}
			Debug.Log("#shutdown# ConsolesManager.OnDestroy finished");
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x00178A7C File Offset: 0x00176C7C
		private bool IsStartupSceneLoadInFlight()
		{
			return this.startupSceneHandle.IsValid() && !this.startupSceneHandle.IsDone;
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x00178A9C File Offset: 0x00176C9C
		private void ReleaseStartupHandles()
		{
			if (this.startupDownloadHandle.IsValid())
			{
				Debug.Log(string.Format("#shutdown# ReleaseStartupHandles: releasing download handle (isDone:[{0}] status:[{1}] percent:[{2}])", this.startupDownloadHandle.IsDone, this.startupDownloadHandle.Status, this.startupDownloadHandle.PercentComplete));
				Addressables.Release(this.startupDownloadHandle);
				this.startupDownloadHandle = default(AsyncOperationHandle);
				Debug.Log("#shutdown# ReleaseStartupHandles: download handle released");
			}
			if (this.startupSceneHandle.IsValid())
			{
				Debug.Log(string.Format("#shutdown# ReleaseStartupHandles: releasing scene handle (isDone:[{0}] status:[{1}] percent:[{2}])", this.startupSceneHandle.IsDone, this.startupSceneHandle.Status, this.startupSceneHandle.PercentComplete));
				Addressables.Release(this.startupSceneHandle);
				this.startupSceneHandle = default(AsyncOperationHandle);
				Debug.Log("#shutdown# ReleaseStartupHandles: scene handle released");
			}
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x00178B84 File Offset: 0x00176D84
		private void ReleaseConfigHandles()
		{
			for (int i = this.configHandles.Count - 1; i >= 0; i--)
			{
				AsyncOperationHandle asyncOperationHandle = this.configHandles[i];
				if (asyncOperationHandle.IsValid())
				{
					Addressables.Release(asyncOperationHandle);
				}
			}
			this.configHandles.Clear();
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x00178BD0 File Offset: 0x00176DD0
		private void Init()
		{
			base.Awake();
			global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			LazyAPI.Platform.Init();
			GameShutdown.Resumed += this.OnGameShutdownResumed;
			GameShutdown.SetQuitBlocker(new Func<bool>(this.IsStartupSceneLoadInFlight));
			ResolutionConfig.InitAvailableResolutions();
			LLBase.InitReplacementRuleSet(new LocaleReplacementRuleSet());
			GameSettings.Instance.ApplySettings();
			LazySingleton<CursorController>.Instance.Init();
			this.isUserInitializationFinished = true;
			this.OnUserAdded();
			LazyAPI.Platform.AddUser(true);
			LazyAPI.Platform.OnAllControllersDisabled += this.ShowNoControllersWarning;
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x00178C6B File Offset: 0x00176E6B
		private void OnUserAdded()
		{
			if (!this.triggerUserAdded)
			{
				GameSettings.Instance.ApplyAudioSettings();
				VoiceOverSettings.IsEnabled = GameSettings.Instance.voiceOverMode == VoiceOverMode.VoiceOver;
				this.triggerUserAdded = true;
				if (this.isLazyPreloaderFinished)
				{
					this.LoadingAction();
				}
			}
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x00178CA8 File Offset: 0x00176EA8
		private async void LoadingAction()
		{
			if (!this.isLoadingActionCalled)
			{
				this.isLoadingActionCalled = true;
				int num = this.startupAttemptId + 1;
				this.startupAttemptId = num;
				int attemptId = num;
				this.lazyPreloader.gameObject.SetActive(false);
				BackgroundLoading.IsActive = false;
				ConsolesManager.IsStartupPreloadOverlayActive = true;
				LoadingPipeline.Instance.ClearStage(LoadingStage.PreMainMenu);
				try
				{
					ConsolesManager.<>c__DisplayClass63_0 CS$<>8__locals1 = new ConsolesManager.<>c__DisplayClass63_0();
					GameShutdown.ThrowIfRequested();
					this.mainCamera.gameObject.SetActive(true);
					this.ApplyPreloadOverlayCanvasScale();
					await this.preloadOverlay.Open().AttachExternalCancellation(GameShutdown.Token);
					await this.preloadOverlay.ShowInitialProgressAndWaitFrame().AttachExternalCancellation(GameShutdown.Token);
					await this.LoadDeferredConfigsAsync();
					LoadingPipeline lp = LoadingPipeline.Instance;
					this.startupDownloadHandle = Addressables.DownloadDependenciesAsync("Preload", false);
					CS$<>8__locals1.downloadProgress = 0f;
					IProgress<float> progress = Progress.Create<float>(delegate(float p)
					{
						CS$<>8__locals1.downloadProgress = p;
					});
					lp.RegisterTask(LoadingStage.PreMainMenu, ConsolesManager.<LoadingAction>g__MeasureTask|63_0("Addressables.DownloadDependenciesAsync", this.startupDownloadHandle.ToUniTask(progress, PlayerLoopTiming.Update, GameShutdown.Token, true, false)), () => CS$<>8__locals1.downloadProgress, 0.3f);
					this.startupSceneHandle = Addressables.LoadSceneAsync("MainScene", LoadSceneMode.Additive, true, 100, SceneReleaseMode.ReleaseSceneWhenSceneUnloaded);
					CS$<>8__locals1.sceneLoadProgress = 0f;
					IProgress<float> progress2 = Progress.Create<float>(delegate(float p)
					{
						CS$<>8__locals1.sceneLoadProgress = p;
					});
					lp.RegisterTask(LoadingStage.PreMainMenu, ConsolesManager.<LoadingAction>g__MeasureTask|63_0("Addressables.LoadSceneAsync MAIN_SCENE", this.startupSceneHandle.ToUniTask(progress2, PlayerLoopTiming.Update, GameShutdown.Token, true, false)), () => CS$<>8__locals1.sceneLoadProgress, 0.75f);
					await UniTask.DelayFrame(1, PlayerLoopTiming.Update, GameShutdown.Token, false);
					this.preloadOverlay.JumpToDownloadDependenciesLoaded();
					await lp.AwaitStage(LoadingStage.PreMainMenu);
					GameShutdown.ThrowIfRequested();
					this.startupLoadCompleted = true;
					this.preloadOverlay.JumpToMainSceneLoaded();
					if (this.startupDownloadHandle.IsValid())
					{
						Addressables.Release(this.startupDownloadHandle);
						this.startupDownloadHandle = default(AsyncOperationHandle);
					}
					this.mainCamera.gameObject.SetActive(false);
					await UniTask.NextFrame(GameShutdown.Token, false);
					await UniTask.NextFrame(GameShutdown.Token, false);
					await this.preloadOverlay.CompleteProgressBar().AttachExternalCancellation(GameShutdown.Token);
					ConsolesManager.IsStartupPreloadOverlayActive = false;
					if (!(MainGame.Instance == null))
					{
						MainGame.Instance.CompleteStartupAfterPreloadOverlay();
						await this.preloadOverlay.FadeOut().AttachExternalCancellation(GameShutdown.Token);
						CS$<>8__locals1 = null;
						lp = null;
					}
				}
				catch (OperationCanceledException)
				{
					Debug.Log(string.Format("#shutdown# LoadingAction cancelled (attempt:[{0}] current:[{1}] startupLoadCompleted:[{2}] isQuitting:[{3}])", new object[]
					{
						attemptId,
						this.startupAttemptId,
						this.startupLoadCompleted,
						GameShutdown.IsQuitting
					}));
					if (attemptId == this.startupAttemptId)
					{
						if (GameShutdown.IsQuitting)
						{
							ConsolesManager.IsStartupPreloadOverlayActive = false;
						}
						else
						{
							this.AbortStartup();
							this.TryContinueStartupAfterResume();
							Debug.Log("#shutdown# LoadingAction cancellation handled");
						}
					}
				}
			}
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x00178CDF File Offset: 0x00176EDF
		private void OnGameShutdownResumed()
		{
			this.TryContinueStartupAfterResume();
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x00178CE8 File Offset: 0x00176EE8
		private void AbortStartup()
		{
			Debug.Log(string.Format("#shutdown# AbortStartup (startupLoadCompleted:[{0}] isQuitting:[{1}])", this.startupLoadCompleted, GameShutdown.IsQuitting));
			ConsolesManager.IsStartupPreloadOverlayActive = false;
			if (this == null)
			{
				return;
			}
			if (this.startupLoadCompleted)
			{
				return;
			}
			this.ReleaseStartupHandles();
			LoadingPipeline.Instance.ClearStage(LoadingStage.PreMainMenu);
			this.isLoadingActionCalled = false;
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x00178D4C File Offset: 0x00176F4C
		private void TryContinueStartupAfterResume()
		{
			if (GameShutdown.IsRequested || this == null)
			{
				return;
			}
			if (!this.startupLoadCompleted)
			{
				if (!this.isLoadingActionCalled && this.isLazyPreloaderFinished)
				{
					this.LoadingAction();
				}
				return;
			}
			if (MainGame.Instance == null)
			{
				return;
			}
			this.FinishStartupOverlayAfterResume();
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x00178DA0 File Offset: 0x00176FA0
		private async void FinishStartupOverlayAfterResume()
		{
			if (!this.isFinishingStartupOverlay)
			{
				this.isFinishingStartupOverlay = true;
				try
				{
					if (this.mainCamera != null)
					{
						this.mainCamera.gameObject.SetActive(false);
					}
					bool hasOverlay = this.preloadOverlay != null && this.preloadOverlay.gameObject.activeSelf;
					if (hasOverlay)
					{
						await this.preloadOverlay.CompleteProgressBar().AttachExternalCancellation(GameShutdown.Token);
					}
					ConsolesManager.IsStartupPreloadOverlayActive = false;
					if (MainGame.Instance != null)
					{
						MainGame.Instance.CompleteStartupAfterPreloadOverlay();
					}
					if (hasOverlay)
					{
						await this.preloadOverlay.FadeOut().AttachExternalCancellation(GameShutdown.Token);
					}
				}
				catch (OperationCanceledException)
				{
					if (this.preloadOverlay != null)
					{
						this.preloadOverlay.gameObject.SetActive(false);
					}
				}
				finally
				{
					this.isFinishingStartupOverlay = false;
				}
			}
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x00178DD7 File Offset: 0x00176FD7
		private void ApplyPreloadOverlayCanvasScale()
		{
			if (this.preloadOverlayCanvasScaler == null)
			{
				return;
			}
			this.preloadOverlayCanvasScaler.scaleFactor = ResolutionConfig.GetUiScaleFactor();
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x00178DF8 File Offset: 0x00176FF8
		private void ShowUnsupportedResolutionWindow()
		{
			this.isLoadingActionCalled = true;
			if (this.lazyPreloader != null)
			{
				this.lazyPreloader.gameObject.SetActive(false);
			}
			if (this.mainCamera != null)
			{
				this.mainCamera.gameObject.SetActive(true);
			}
			this.ApplyPreloadOverlayCanvasScale();
			if (this.preloadOverlay != null)
			{
				this.preloadOverlay.gameObject.SetActive(false);
			}
			if (this.unsupportedResolutionWindow == null)
			{
				Debug.LogError("UIUnsupportedResolutionWindow is not assigned on ConsolesManager.");
				return;
			}
			this.unsupportedResolutionWindow.Open();
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x00178E94 File Offset: 0x00177094
		private void Update()
		{
			if (ConsolesManager.showNoControllerWarning)
			{
				ConsolesManager.showNoControllerWarningDelay -= LazyTime.GetUnscaledDeltaTime;
				if (ConsolesManager.showNoControllerWarningDelay <= 0f)
				{
					ConsolesManager.showNoControllerWarning = false;
					if (ReInput.controllers.joystickCount == 0)
					{
						this.ShowNoControllersWarning();
					}
				}
			}
			LazyAPI.Platform.Update();
			if (ConsolesManager.unfreezeGame)
			{
				ConsolesManager.unfreezeGame = false;
				this.UnfreezeGame();
			}
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x00178EF9 File Offset: 0x001770F9
		private bool IsGameFrozen()
		{
			return ConsolesManager.isFrozen;
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x00178F00 File Offset: 0x00177100
		private void FreezeGame()
		{
			ConsolesManager.isFrozen = true;
			LazyTime.OverrideUnscaledDeltaTime(0f);
			DOTween.PauseAll();
			ConsolesManager.frozenTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x00178F2C File Offset: 0x0017712C
		private void UnfreezeGame()
		{
			ConsolesManager.isFrozen = false;
			LazyTime.CancelOverrideUnscaledDeltaTime();
			DOTween.PlayAll();
			Time.timeScale = ConsolesManager.frozenTimeScale;
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x00178F49 File Offset: 0x00177149
		private IEnumerator OnFinishedLazyPreloaderCoroutine()
		{
			this.isLazyPreloaderFinished = true;
			if (this.triggerUserAdded)
			{
				this.LoadingAction();
			}
			yield break;
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x00002318 File Offset: 0x00000518
		public void ShowNoControllersWarning()
		{
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x00178F6B File Offset: 0x0017716B
		[CompilerGenerated]
		internal static UniTask <LoadingAction>g__MeasureTask|63_0(string taskName, UniTask task)
		{
			return ConsolesManager.<LoadingAction>g__MeasureTaskInternal|63_1(taskName, task);
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x00178F74 File Offset: 0x00177174
		[CompilerGenerated]
		internal static async UniTask <LoadingAction>g__MeasureTaskInternal|63_1(string taskName, UniTask task)
		{
			await task;
		}

		// Token: 0x0400415C RID: 16732
		private const string PS5_ACTIVITY_ID = "continue";

		// Token: 0x0400415D RID: 16733
		private static bool showNoControllerWarning;

		// Token: 0x0400415E RID: 16734
		private static float showNoControllerWarningDelay;

		// Token: 0x0400415F RID: 16735
		private static float frozenTimeScale;

		// Token: 0x04004160 RID: 16736
		private static bool unfreezeGame;

		// Token: 0x04004161 RID: 16737
		private static bool isFrozen;

		// Token: 0x04004162 RID: 16738
		private bool isUserInitializationFinished;

		// Token: 0x04004163 RID: 16739
		private bool triggerUserAdded;

		// Token: 0x04004164 RID: 16740
		[SerializeField]
		private UIPreloadOverlay preloadOverlay;

		// Token: 0x04004165 RID: 16741
		[SerializeField]
		private CanvasScaler preloadOverlayCanvasScaler;

		// Token: 0x04004166 RID: 16742
		[SerializeField]
		private UIUnsupportedResolutionWindow unsupportedResolutionWindow;

		// Token: 0x04004167 RID: 16743
		[SerializeField]
		private LazyPreloader lazyPreloader;

		// Token: 0x04004168 RID: 16744
		[SerializeField]
		private Camera mainCamera;

		// Token: 0x04004169 RID: 16745
		[SerializeField]
		private bool isPreloadWindowInitialized;

		// Token: 0x0400416B RID: 16747
		[SerializeField]
		private AssetReferenceT<LazyApplicationSettings> lazyApplicationSettingsRef;

		// Token: 0x0400416C RID: 16748
		[SerializeField]
		private AssetReferenceT<ZombieCustomizationConfig> zombieCustomizationConfigRef;

		// Token: 0x0400416D RID: 16749
		[SerializeField]
		private AssetReferenceT<WgoPartPoolInitialSizesConfig> wgoPartPoolInitialSizesConfigRef;

		// Token: 0x0400416E RID: 16750
		[SerializeField]
		private AssetReferenceT<WgoPartBakedDataCollection> wgoPartBakedDataCollectionRef;

		// Token: 0x0400416F RID: 16751
		[SerializeField]
		private AssetReferenceT<VoiceOverSettings> voiceOverSettingsRef;

		// Token: 0x04004170 RID: 16752
		[SerializeField]
		private AssetReferenceT<TMPShaderSetup> tmpShaderSetupRef;

		// Token: 0x04004171 RID: 16753
		[SerializeField]
		private AssetReferenceT<SurfaceStepSoundSettings> stepSoundSettingsRef;

		// Token: 0x04004172 RID: 16754
		[SerializeField]
		private AssetReferenceT<NPCLifeSimulatorConfiguration> npcLifeSimulatorConfigurationRef;

		// Token: 0x04004173 RID: 16755
		[SerializeField]
		private AssetReferenceT<LazyTerrainMeshCollection> lazyTerrainMeshCollectionRef;

		// Token: 0x04004174 RID: 16756
		[SerializeField]
		private AssetReferenceT<GlobalResources> globalResourcesRef;

		// Token: 0x04004175 RID: 16757
		[SerializeField]
		private AssetReferenceT<GameResDisplayConfig> gameResDisplayConfigRef;

		// Token: 0x04004176 RID: 16758
		[SerializeField]
		private AssetReferenceT<GamepadTypeData> gamepadTypeDataRef;

		// Token: 0x04004177 RID: 16759
		[SerializeField]
		private AssetReferenceT<GameInfo> gameInfoRef;

		// Token: 0x04004178 RID: 16760
		[SerializeField]
		private AssetReferenceT<GameBindings> gameBindingsRef;

		// Token: 0x04004179 RID: 16761
		[SerializeField]
		private AssetReferenceT<EasySpritesCollection> easySpritesCollectionRef;

		// Token: 0x0400417A RID: 16762
		[SerializeField]
		private AssetReferenceT<DeformingGrassSettings> deformingGrassSettingsRef;

		// Token: 0x0400417B RID: 16763
		[SerializeField]
		private AssetReferenceT<ConstructorPartPoolInitialSizesConfig> constructorPartPoolInitialSizesConfigRef;

		// Token: 0x0400417C RID: 16764
		[SerializeField]
		private AssetReferenceT<ConstructorPartBoundsConfig> constructorPartBoundsConfigRef;

		// Token: 0x0400417D RID: 16765
		[SerializeField]
		private AssetReferenceT<BakedChunkableObjectPoolInitialSizesConfig> bakedChunkableObjectPoolInitialSizesConfigRef;

		// Token: 0x0400417E RID: 16766
		[SerializeField]
		private AssetReferenceT<AudioConfig> audioConfigRef;

		// Token: 0x0400417F RID: 16767
		private readonly List<AsyncOperationHandle> configHandles = new List<AsyncOperationHandle>();

		// Token: 0x04004180 RID: 16768
		private AsyncOperationHandle startupDownloadHandle;

		// Token: 0x04004181 RID: 16769
		private AsyncOperationHandle startupSceneHandle;

		// Token: 0x04004182 RID: 16770
		private bool isLazyPreloaderFinished;

		// Token: 0x04004183 RID: 16771
		private bool isLoadingActionCalled;

		// Token: 0x04004184 RID: 16772
		private bool startupLoadCompleted;

		// Token: 0x04004185 RID: 16773
		private int startupAttemptId;

		// Token: 0x04004186 RID: 16774
		private bool isFinishingStartupOverlay;
	}
}
