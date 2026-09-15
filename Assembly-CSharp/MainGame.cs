using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006D2 RID: 1746
public class MainGame : MonoBehaviour
{
	// Token: 0x1700072D RID: 1837
	// (get) Token: 0x06002E2A RID: 11818 RVA: 0x000DCC46 File Offset: 0x000DAE46
	// (set) Token: 0x06002E2B RID: 11819 RVA: 0x000DCC4E File Offset: 0x000DAE4E
	public MainGame.GameState gameState { get; private set; }

	// Token: 0x1700072E RID: 1838
	// (get) Token: 0x06002E2C RID: 11820 RVA: 0x000DCC57 File Offset: 0x000DAE57
	// (set) Token: 0x06002E2D RID: 11821 RVA: 0x000DCC5E File Offset: 0x000DAE5E
	public static MainGame Instance { get; private set; }

	// Token: 0x1700072F RID: 1839
	// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000DCC66 File Offset: 0x000DAE66
	public static UpdateManager UpdateManager
	{
		get
		{
			return MainGame.Instance.updateManager;
		}
	}

	// Token: 0x17000730 RID: 1840
	// (get) Token: 0x06002E2F RID: 11823 RVA: 0x00084092 File Offset: 0x00082292
	public static WorldData WorldData
	{
		get
		{
			return MainGame.Instance.GameSave.worldData;
		}
	}

	// Token: 0x17000731 RID: 1841
	// (get) Token: 0x06002E30 RID: 11824 RVA: 0x000DCC72 File Offset: 0x000DAE72
	public static ZombieSystemData ZombieSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.zombieSystemData;
		}
	}

	// Token: 0x17000732 RID: 1842
	// (get) Token: 0x06002E31 RID: 11825 RVA: 0x000835B1 File Offset: 0x000817B1
	public static ConveyorSystemData ConveyorSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.conveyorSystemData;
		}
	}

	// Token: 0x17000733 RID: 1843
	// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000DCC83 File Offset: 0x000DAE83
	public static PlayerController PlayerController
	{
		get
		{
			return MainGame.Instance.playerController;
		}
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x000DCC8F File Offset: 0x000DAE8F
	public static FightingLevel GetFightingLevel(string id)
	{
		return MainGame.Instance.fightingLevelSystem.GetFightingLevel(id);
	}

	// Token: 0x17000734 RID: 1844
	// (get) Token: 0x06002E34 RID: 11828 RVA: 0x000DCCA1 File Offset: 0x000DAEA1
	// (set) Token: 0x06002E35 RID: 11829 RVA: 0x000DCCA8 File Offset: 0x000DAEA8
	public static PlayerData PlayerData { get; private set; }

	// Token: 0x17000735 RID: 1845
	// (get) Token: 0x06002E36 RID: 11830 RVA: 0x000DCCB0 File Offset: 0x000DAEB0
	// (set) Token: 0x06002E37 RID: 11831 RVA: 0x000DCCB7 File Offset: 0x000DAEB7
	public static bool IsGamePaused { get; private set; }

	// Token: 0x17000736 RID: 1846
	// (get) Token: 0x06002E38 RID: 11832 RVA: 0x000DCCBF File Offset: 0x000DAEBF
	public static string ConveyorPresetLoadOnNewGame
	{
		get
		{
			return MainGame.Instance.conveyorPresetLoadOnNewGame;
		}
	}

	// Token: 0x17000737 RID: 1847
	// (get) Token: 0x06002E39 RID: 11833 RVA: 0x000DCCCB File Offset: 0x000DAECB
	public static GameSaveVersion GameSaveVersion
	{
		get
		{
			return MainGame.Instance.GameSave.SaveVersion;
		}
	}

	// Token: 0x17000738 RID: 1848
	// (get) Token: 0x06002E3A RID: 11834 RVA: 0x000DCCDC File Offset: 0x000DAEDC
	public static string EntrySceneToLoad
	{
		get
		{
			return "RuinedTemple";
		}
	}

	// Token: 0x17000739 RID: 1849
	// (get) Token: 0x06002E3B RID: 11835 RVA: 0x000DCCE3 File Offset: 0x000DAEE3
	public static HashSet<string> FirstQuestSceneIds
	{
		get
		{
			return new HashSet<string> { "NorthRuinedTemple", "Prison" };
		}
	}

	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x06002E3C RID: 11836 RVA: 0x000DCD02 File Offset: 0x000DAF02
	public GameSave GameSave
	{
		get
		{
			return this.gameSave;
		}
	}

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x06002E3D RID: 11837 RVA: 0x000DCD0A File Offset: 0x000DAF0A
	public GraphHelper GraphHelper
	{
		get
		{
			return this.graphHelper;
		}
	}

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x06002E3E RID: 11838 RVA: 0x000DCD12 File Offset: 0x000DAF12
	public SaveSlotData SaveSlotData
	{
		get
		{
			return this.saveSlotData;
		}
	}

	// Token: 0x06002E3F RID: 11839 RVA: 0x000DCD1C File Offset: 0x000DAF1C
	private void Awake()
	{
		Debug.Log(string.Format("#shutdown# MainGame.Awake (shutdownRequested:[{0}] isQuitting:[{1}])", GameShutdown.IsRequested, GameShutdown.IsQuitting));
		this.gameState = MainGame.GameState.MainMenu;
		MainGame.Instance = this;
		if (DevUtils.IsDemoBitsummitActive)
		{
			VoiceOverSettings.IsEnabled = false;
		}
		this.playerController = this.GetPlayerController();
		this.playerController.Initialize();
		MainGame.PlayerController.SetDisabledStateType(DisabledStateType.ByMainMenu, false);
		ResolutionConfig.InitAvailableResolutions();
		GameSettings.OnResolutionChanged += this.OnResolutionChanged;
		LLBase.InitReplacementRuleSet(new LocaleReplacementRuleSet());
		ModsBootstrap.EnsureOnStartup();
		GameSettings.Instance.ApplySettings();
		VoiceOverSettings.IsEnabled = GameSettings.Instance.voiceOverMode == VoiceOverMode.VoiceOver;
		this.ApplyMainMenuViewScaleByCurrentResolution();
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x000DCDD0 File Offset: 0x000DAFD0
	private void OnDestroy()
	{
		GameSettings.OnResolutionChanged -= this.OnResolutionChanged;
		InWorldSfxFilterController.Shutdown();
	}

	// Token: 0x06002E41 RID: 11841 RVA: 0x000DCDE8 File Offset: 0x000DAFE8
	private void Start()
	{
		Debug.Log(string.Format("#shutdown# MainGame.Start begin (shutdownRequested:[{0}] isQuitting:[{1}])", GameShutdown.IsRequested, GameShutdown.IsQuitting));
		if (GameShutdown.IsQuitting)
		{
			return;
		}
		Debug.Log("#shutdown# MainGame.Start: WgoPartBakedDataCollection.LoadCache");
		LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.LoadCache();
		if (this.gameState == MainGame.GameState.MainMenu)
		{
			this.SetMainMenuEnabledState(true);
		}
		Debug.Log("#shutdown# MainGame.Start: GameBalance.LoadGameBalance");
		GameBalance.LoadGameBalance();
		Debug.Log("#shutdown# MainGame.Start: LazyInput.TryInit");
		LazyInput.TryInit();
		Debug.Log("#shutdown# MainGame.Start: LazyUI.Init");
		LazyUI.Init(null);
		Debug.Log("#shutdown# MainGame.Start: guiElements.Initialize");
		this.guiElements.Initialize();
		Debug.Log("#shutdown# MainGame.Start: GameSceneConfig.LoadAllConfigs");
		this.gameSceneConfigs = GameSceneConfig.LoadAllConfigs();
		Debug.Log("#shutdown# MainGame.Start: systems init");
		this.conveyorSystem.Init();
		this.updateManager.AddScheduledUpdate(new ScheduledUpdate(new List<ICustomUpdatable> { this.craftSystem, this.dropSystem, this.conveyorSystem }, 0.2f));
		this.updateManager.AddScheduledUpdate(new ScheduledUpdate(new List<ICustomUpdatable> { this.movementSystem, this.wgoDelayedEventsSystem, this.riverDropSystem }, 0f));
		this.updateManager.AddScheduledUpdate(new ScheduledUpdate(new List<ICustomUpdatable> { this.perkSystem, this.questSystem, this.wgoDelayedSpawnSystem }, 1f));
		this.updateManager.AddScheduledUpdate(new ScheduledUpdate(new List<ICustomUpdatable>
		{
			EnvironmentEngine.Instance,
			this.gameLogicsSystem,
			this.zombieSystem,
			this.wgoCustomDeathSystem,
			this.npcLifeSimulator
		}, 0.04f));
		this.updateManager.AddScheduledUpdate(new ScheduledUpdate(new List<ICustomUpdatable> { this.zombiePorterSystem }, 2f));
		MainGame.OnGoToMainMenu = (Action)Delegate.Combine(MainGame.OnGoToMainMenu, new Action(delegate
		{
			LazyAudio.StopAllPlaylistsImmediately();
			LazyAudio.StopAll();
			LazyAudio.PlayPlaylist("main_menu");
		}));
		LazyWindowsStackController.OnWindowOpened += this.OnWindowOpened;
		LazyWindowsStackController.OnWindowClosed += this.OnWindowClosed;
		LazyWindowsStackController.OnAllWindowsClosed += this.OnAllWindowsClosed;
		InWorldSfxFilterController.Init();
		Debug.Log("#shutdown# MainGame.Start: EnvironmentEngine.PreloadTimeOfDayPresets");
		EnvironmentEngine.Instance.PreloadTimeOfDayPresets();
		LazyInput.OnInputChanged += delegate
		{
			CursorController.ChangeCursorVisibleState(!LazyInput.IsGamepadActive);
		};
		CursorController.ChangeCursorVisibleState(!LazyInput.IsGamepadActive);
		VoiceOverPlayer.MuteCheck = new Func<string, bool>(DialogDataContainer.IsVoiceOverMuted);
		VoiceOverModLoader.Refresh(GameSettings.Instance.language);
		Debug.Log("#shutdown# MainGame.Start: guiElements.PreloadWindows");
		this.guiElements.PreloadWindows();
		Debug.Log("#shutdown# MainGame.Start: InitNetwork");
		this.InitNetwork();
		if (LazyUITester.isTesting)
		{
			LazyUITester.OnGameStart();
			return;
		}
		Debug.Log("#shutdown# MainGame.Start: HUD + main menu music");
		LazyUI.Get<HUD>().SetDisableState(HudStateType.MainMenu, false, null);
		LazyAudio.StopAllPlaylistsImmediately();
		LazyAudio.PlayPlaylist("main_menu");
		MainGame.SetMainMenuMixer(true);
		if (DevUtils.IsDemoBitsummitActive)
		{
			DevUtils.isMainSceneSkipped = false;
			this.StartNewGame(false);
		}
		else
		{
			this.TryOpenStartupMainMenu();
		}
		GameSettings.Instance.ApplySettings();
		if (!this.deferStartupMainMenu)
		{
			this.RegisterBackgroundPreloadTasks();
		}
		base.StartCoroutine(this.UnloadAstarCacheWhenAstarPathIsReady());
		Debug.Log(string.Format("#shutdown# MainGame.Start end (deferStartupMainMenu:[{0}])", this.deferStartupMainMenu));
	}

	// Token: 0x06002E42 RID: 11842 RVA: 0x000DD170 File Offset: 0x000DB370
	private void TryOpenStartupMainMenu()
	{
		if (ConsolesManager.IsStartupPreloadOverlayActive)
		{
			this.deferStartupMainMenu = true;
			return;
		}
		this.OpenStartupMainMenu(false);
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x000DD188 File Offset: 0x000DB388
	public void CompleteStartupAfterPreloadOverlay()
	{
		if (!this.deferStartupMainMenu || GameShutdown.IsRequested)
		{
			return;
		}
		this.deferStartupMainMenu = false;
		this.OpenStartupMainMenu(true);
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x000DD1A8 File Offset: 0x000DB3A8
	private void OpenStartupMainMenu(bool fromPreloader = false)
	{
		UIMainMenuWindow window = LazyUI.GetWindow<UIMainMenuWindow>();
		if (fromPreloader)
		{
			window.OpenFromPreloader(LazySingleton<ConsolesManager>.Instance.PreloadOverlay, new Action(this.OnPreloaderIntroComplete));
			return;
		}
		window.Open(null);
		this.TryShowDLCPopUpOnStartUp();
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x000DD1E8 File Offset: 0x000DB3E8
	private void OnPreloaderIntroComplete()
	{
		if (GameShutdown.IsRequested)
		{
			return;
		}
		this.RegisterBackgroundPreloadTasks();
		this.TryShowDLCPopUpOnStartUp();
	}

	// Token: 0x06002E46 RID: 11846 RVA: 0x000DD200 File Offset: 0x000DB400
	private void TryShowDLCPopUpOnStartUp()
	{
		DLCVersion dlcversion = DLCVersion.Preorder;
		GameSettings instance = GameSettings.Instance;
		if (DLCEngine.IsDLCAvailable(dlcversion) && !instance.IsDlcStartupPopUpShown(dlcversion))
		{
			LazyUI.GetWindow<UIPreorderWindow>().Open(null);
			instance.MarkDlcStartupPopUpShown(dlcversion);
		}
	}

	// Token: 0x06002E47 RID: 11847 RVA: 0x000DD238 File Offset: 0x000DB438
	private IEnumerator UnloadAstarCacheWhenAstarPathIsReady()
	{
		while (AstarPath.active == null)
		{
			yield return null;
		}
		while (AstarPath.active.graphs == null || AstarPath.active.graphs.Length == 0)
		{
			yield return null;
		}
		TextAsset file_cachedStartup = AstarPath.active.data.file_cachedStartup;
		AstarPath.active.data.file_cachedStartup = null;
		Debug.Log("Astar cache unloaded");
		yield break;
	}

	// Token: 0x06002E48 RID: 11848 RVA: 0x000DD240 File Offset: 0x000DB440
	private void RegisterBackgroundPreloadTasks()
	{
		if (this.backgroundPreloadRegistered || GameShutdown.IsRequested)
		{
			return;
		}
		this.backgroundPreloadRegistered = true;
		BackgroundLoading.IsActive = true;
		LoadingPipeline instance = LoadingPipeline.Instance;
		LazyTerrainMeshPool terrainPool = LazySingleton<LazyTerrainMeshPool>.Instance;
		instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("LazyTerrainMeshPool.Instance.InitAsync()", terrainPool.InitAsync()), () => terrainPool.LocalProgress, 0.1f);
		ConstructorPartPool constructorPool = LazySingleton<ConstructorPartPool>.Instance;
		instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("ConstructorPartPool.Instance.InitAsync()", constructorPool.InitAsync()), () => constructorPool.LocalProgress, 0.2f);
		BakedChunkableObjectPool bakedPool = LazySingleton<BakedChunkableObjectPool>.Instance;
		instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("BakedChunkableObjectPool.Instance.InitAsync()", bakedPool.InitAsync()), () => bakedPool.LocalProgress, 0.3f);
		WgoPartPool wgoPool = LazySingleton<WgoPartPool>.Instance;
		instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("WgoPartPool.Instance.InitAsync()", wgoPool.InitAsync()), () => wgoPool.LocalProgress, 0.4f);
		WorldFXPool fxPool = LazySingleton<WorldFXPool>.Instance;
		instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("WorldFXPool.Instance.InitAsync()", fxPool.InitAsync()), () => fxPool.LocalProgress, 0.3f);
		instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("DropView.PreloadAsync()", DropView.PreloadAsync()), () => DropView.PreloadProgress, 0.05f);
		if (FlowScriptAssetLoadPolicy.UsesBackgroundPreload)
		{
			instance.RegisterTask(LoadingStage.BackgroundPreload, MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTask|92_0("FlowScriptAssetLoadPolicy.PreloadAsync()", FlowScriptAssetLoadPolicy.PreloadAsync()), () => FlowScriptAssetLoadPolicy.PreloadProgress, 0.2f);
		}
	}

	// Token: 0x06002E49 RID: 11849 RVA: 0x000DD409 File Offset: 0x000DB609
	public void Update()
	{
		ModsBootstrap.Tick();
		SteamWorkshopCreatorConfig.Tick();
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x000DD415 File Offset: 0x000DB615
	public void StartNewGame(bool skipMainScene = false)
	{
		this.StartNewGameWithSlotName(SaveSystem.GetNameForNewSlot(SaveSystem.SaveSlotDataList), skipMainScene);
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x000DD428 File Offset: 0x000DB628
	public void StartNewGameInLimitedSaveSlot(int slotIndex, bool skipMainScene = false)
	{
		this.StartNewGameWithSlotName(SaveSystem.GetNameForLimitedSaveSlot(slotIndex), skipMainScene);
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x000DD437 File Offset: 0x000DB637
	private void StartNewGameWithSlotName(string slotName, bool skipMainScene = false)
	{
		DLCEngine.ResetDLCStateCached();
		if (!skipMainScene)
		{
			LazySingleton<GlobalNavigationManager>.Instance.ClearAll();
		}
		this.saveSlotData = new SaveSlotData
		{
			slotName = slotName
		};
		this.CreateGameSaveAndStart(PlayerSkinHelper.playerStandardCustomizationData, !skipMainScene);
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x000DD46C File Offset: 0x000DB66C
	public void CompleteGame(bool shouldGoToMenuOnReturn, Action onCreditsWindowClosed = null)
	{
		Debug.Log("#DEV# CompleteGame, show credits");
		UICinematic uicinematic = LazyUI.Get<UICinematic>();
		if (uicinematic != null && uicinematic.gameObject.activeSelf)
		{
			uicinematic.DisableCinematic(null, true);
		}
		this.SetMainMenuInfoPanelEnabled(false);
		LazyUI.GetWindow<UICreditsWindow>().OpenAfterGameComplete(shouldGoToMenuOnReturn, onCreditsWindowClosed);
	}

	// Token: 0x06002E4E RID: 11854 RVA: 0x000DD4BC File Offset: 0x000DB6BC
	public void CreateGameSaveAndStart(PlayerCustomizationData customizationData, bool startQuest = true)
	{
		MainGame.entrySceneToLoadCached = MainGame.EntrySceneToLoad;
		LoadingWindowData loadingWindowData = new LoadingWindowData(MainGame.entrySceneToLoadCached, delegate
		{
			this.gameSave = new GameSave();
			GameSave.SetupNewGameSave(this.gameSave);
			MainGame.PlayerData = this.gameSave.playerData;
			MainGame.PlayerData.TryApplyStartState();
			MainGame.PlayerData.ApplyCustomization(customizationData);
			this.startingNewGame = true;
			if (DLCEngine.IsDLCAvailable(DLCVersion.Preorder))
			{
				this.gameSave.GivePreorderReward();
			}
			else
			{
				Debug.Log("No preorder available GivePreorderReward failed");
			}
			this.StartGame(true);
			this.gameSave.questSystemData.AwaitQuest(ConstDef.Get("new_game_start_quest").StringValue, 0f);
		}, false);
		LoadingPipeline.Instance.ClearStage(LoadingStage.GameplaySceneLoad);
		LazyUI.Get<UILoadingOverlay>().Draw(loadingWindowData);
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x000DD518 File Offset: 0x000DB718
	public void ContinueGame(SaveSlotData saveSlotData, GameSave gameSave)
	{
		MainGame.<>c__DisplayClass99_0 CS$<>8__locals1 = new MainGame.<>c__DisplayClass99_0();
		CS$<>8__locals1.saveSlotData = saveSlotData;
		CS$<>8__locals1.gameSave = gameSave;
		CS$<>8__locals1.<>4__this = this;
		MainGame.entrySceneToLoadCached = MainGame.EntrySceneToLoad;
		LoadingPipeline.Instance.ClearStage(LoadingStage.GameplaySceneLoad);
		UILoadingOverlay uiloadingOverlay = LazyUI.Get<UILoadingOverlay>();
		if (uiloadingOverlay.IsShown)
		{
			CS$<>8__locals1.<ContinueGame>g__OnOverlayReady|0();
			return;
		}
		uiloadingOverlay.Draw(new LoadingWindowData(MainGame.entrySceneToLoadCached, new Action(CS$<>8__locals1.<ContinueGame>g__OnOverlayReady|0), false));
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x000DD588 File Offset: 0x000DB788
	private async void StartGame(bool isNewGame = false)
	{
		if (!GameShutdown.IsRequested)
		{
			this.SetMainMenuEnabledState(false);
			MainGame.SetMainMenuMixer(false);
			this.gameSave.PrepareForGame();
			if (isNewGame || SaveFixer.Apply(this.gameSave, this.gameSceneConfigs))
			{
				this.gameSave.GameSaveVer = LazySingletonSO<GameInfo>.Instance.Version;
			}
			if (!isNewGame && this.saveSlotData != null && this.saveSlotData.ShouldConvertDemoProgressOnLoad && !this.gameSave.isDemoSaveLoadedInReleaseCustomActionsApplied)
			{
				SaveSystem.ApplyCustomActionsForDemoSaveLoadedInRelease(this.saveSlotData, this.gameSave);
			}
			this.playerController.PreparePlayerForGame(this.gameSave.playerData);
			UniTask<bool>.Awaiter awaiter = this.AwaitBackgroundPreloadAsync().GetAwaiter();
			UniTask<bool>.Awaiter awaiter2;
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				awaiter = awaiter2;
				awaiter2 = default(UniTask<bool>.Awaiter);
			}
			if (awaiter.GetResult())
			{
				if (!GameShutdown.IsRequested)
				{
					this.gameState = MainGame.GameState.InGame;
					this.graphHelper.ScanGDPointGraph();
					TeleportPointGraph.Build(this.gameSave.worldData);
					this.SetSystemsPauseState(false);
					this.gameSave.zombieSystemData.ResumeCrafterWorkAfterLoad();
					MainGame.PlayerController.SetDisabledStateType(DisabledStateType.ByMainMenu, true);
					LazyUI.Get<HUD>().SetDisableState(HudStateType.MainMenu, true, new HUDData(this.gameSave));
					FlyingTechPoint.Clear();
					LazySingleton<ChunkManager>.Instance.IsActive = true;
					awaiter = this.AwaitGameplaySceneLoadAsync(isNewGame).GetAwaiter();
					if (!awaiter.IsCompleted)
					{
						await awaiter;
						awaiter = awaiter2;
						awaiter2 = default(UniTask<bool>.Awaiter);
					}
					if (awaiter.GetResult())
					{
						if (!GameShutdown.IsRequested)
						{
							if (!isNewGame && !DLCEngine.IsDLCAvailable(DLCVersion.Preorder))
							{
								this.gameSave.TryRemovePreorderReward();
							}
							Action onGameStarted = MainGame.OnGameStarted;
							if (onGameStarted != null)
							{
								onGameStarted();
							}
						}
					}
				}
			}
		}
	}

	// Token: 0x06002E51 RID: 11857 RVA: 0x000DD5C8 File Offset: 0x000DB7C8
	private async UniTask<bool> AwaitBackgroundPreloadAsync()
	{
		for (;;)
		{
			this.RegisterBackgroundPreloadTasks();
			BackgroundLoading.IsActive = false;
			if (this.backgroundPreloadRegistered)
			{
				try
				{
					await LoadingPipeline.Instance.AwaitStage(LoadingStage.BackgroundPreload);
					return 1;
				}
				catch (OperationCanceledException)
				{
				}
			}
			UniTask<bool>.Awaiter awaiter = GameShutdown.WaitForResumeAsync().GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				UniTask<bool>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(UniTask<bool>.Awaiter);
			}
			if (!awaiter.GetResult())
			{
				break;
			}
			this.backgroundPreloadRegistered = false;
			LoadingPipeline.Instance.ClearStage(LoadingStage.BackgroundPreload);
			await UniTask.Yield();
		}
		return 0;
	}

	// Token: 0x06002E52 RID: 11858 RVA: 0x000DD60C File Offset: 0x000DB80C
	private async UniTask<bool> AwaitGameplaySceneLoadAsync(bool isNewGame)
	{
		LoadingPipeline lp = LoadingPipeline.Instance;
		for (;;)
		{
			lp.ClearStage(LoadingStage.GameplaySceneLoad);
			lp.RegisterTask(LoadingStage.GameplaySceneLoad, this.LoadGameScene(isNewGame), () => this.gameSceneLoadProgress, 1f);
			try
			{
				await lp.AwaitStage(LoadingStage.GameplaySceneLoad);
				return 1;
			}
			catch (OperationCanceledException)
			{
			}
			UniTask<bool>.Awaiter awaiter = GameShutdown.WaitForResumeAsync().GetAwaiter();
			if (!awaiter.IsCompleted)
			{
				await awaiter;
				UniTask<bool>.Awaiter awaiter2;
				awaiter = awaiter2;
				awaiter2 = default(UniTask<bool>.Awaiter);
			}
			if (!awaiter.GetResult())
			{
				break;
			}
			await UniTask.Yield();
		}
		return 0;
	}

	// Token: 0x06002E53 RID: 11859 RVA: 0x000DD658 File Offset: 0x000DB858
	public void PrepareGameForNetwork(GameSave gameSave, NetworkPlayer networkPlayer, bool isHost)
	{
		gameSave.PrepareForGame();
		gameSave.zombieSystemData.ResumeCrafterWorkAfterLoad();
		if (isHost)
		{
			gameSave.hostPlayer = networkPlayer;
		}
		this.PlayerUniqueCommandHolder.RegisterData(networkPlayer);
		this.playerController.PreparePlayerForGame(gameSave.playerData);
		if (!isHost)
		{
			this.SpawnPlayer(gameSave.hostPlayer);
		}
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x000DD6AC File Offset: 0x000DB8AC
	public async UniTask LoadGameScene(bool isNewGame = false)
	{
		GameShutdown.ThrowIfRequested();
		this.gameSceneLoadProgress = 0f;
		MainGame.PlayerController.SetDisabledStateType(DisabledStateType.BySceneLoading, false);
		Debug.Log("LoadGameScene");
		Progress<float> progress = new Progress<float>(delegate(float p)
		{
			this.gameSceneLoadProgress = p * 0.8f;
		});
		await LazySingleton<GameSceneManager>.Instance.LoadSceneAsync(MainGame.entrySceneToLoadCached, progress);
		GameShutdown.ThrowIfRequested();
		this.gameSceneLoadProgress = 0.8f;
		if (isNewGame)
		{
			MainGame.<>c__DisplayClass104_0 CS$<>8__locals1 = new MainGame.<>c__DisplayClass104_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.questSceneIndex = 0;
			CS$<>8__locals1.questSceneCount = MainGame.FirstQuestSceneIds.Count - 1;
			foreach (string text in MainGame.FirstQuestSceneIds)
			{
				Action<float> action;
				if ((action = CS$<>8__locals1.<>9__1) == null)
				{
					MainGame.<>c__DisplayClass104_0 CS$<>8__locals2 = CS$<>8__locals1;
					Action<float> action2 = delegate(float p)
					{
						CS$<>8__locals1.<>4__this.gameSceneLoadProgress = 0.8f + (float)CS$<>8__locals1.questSceneIndex / (float)CS$<>8__locals1.questSceneCount * 0.1f;
					};
					CS$<>8__locals2.<>9__1 = action2;
					action = action2;
				}
				progress = new Progress<float>(action);
				await LazySingleton<GameSceneManager>.Instance.LoadSceneAsync(text, progress);
				GameShutdown.ThrowIfRequested();
				CS$<>8__locals1.questSceneIndex++;
			}
			HashSet<string>.Enumerator enumerator = default(HashSet<string>.Enumerator);
			CS$<>8__locals1 = null;
		}
		this.gameSceneLoadProgress = 0.9f;
		UILoadingOverlay uiloadingOverlay = LazyUI.Get<UILoadingOverlay>();
		if (uiloadingOverlay != null)
		{
			uiloadingOverlay.NotifyAfterSceneWorkStarted();
		}
		await this.AfterSceneHasLoaded();
		this.gameSceneLoadProgress = 1f;
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x000DD6F7 File Offset: 0x000DB8F7
	public void SetGameSave(GameSave gameSave)
	{
		this.gameSave = gameSave;
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x000DD700 File Offset: 0x000DB900
	public void SpawnPlayer(NetworkPlayer player)
	{
		PlayerPhysicalBody component = global::UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab).GetComponent<PlayerPhysicalBody>();
		component.InitNetworkPlayer(player);
		player.SubscribeToPlayerDataChanges(component);
		component.gameObject.SetActive(true);
		Debug.Log("Spawned player for network player");
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x000DD744 File Offset: 0x000DB944
	public void GoToMenu(Action onFadeInComplete = null, bool skipFadeIn = false, FadeFlag fadeOutFlag = FadeFlag.Common)
	{
		UIFade fadeElement = LazyUI.Get<UIFade>();
		if (skipFadeIn)
		{
			this.GoToMenuAfterFadeIn(fadeElement, onFadeInComplete, fadeOutFlag);
			return;
		}
		fadeElement.FadeIn(0.5f, delegate
		{
			this.GoToMenuAfterFadeIn(fadeElement, onFadeInComplete, fadeOutFlag);
		}, FadeFlag.Common, false);
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x000DD7B4 File Offset: 0x000DB9B4
	private async void GoToMenuAfterFadeIn(UIFade fadeElement, Action onFadeInComplete, FadeFlag fadeOutFlag)
	{
		if (onFadeInComplete != null)
		{
			onFadeInComplete();
		}
		this.gameState = MainGame.GameState.MainMenu;
		FightingGameController instance = LazySingleton<FightingGameController>.Instance;
		if (instance.CurrentFightState == FightState.InPreFight)
		{
			instance.CancelPreFight();
			instance.TryCloseFightEndWindows();
		}
		if (instance.CurrentFightState == FightState.ActiveFight)
		{
			instance.Stop(false, false);
			instance.TryCloseFightEndWindows();
		}
		FlyingTechPoint.Clear();
		LazySingleton<GlobalNavigationManager>.Instance.ClearAll();
		this.SetMainMenuEnabledState(true);
		this.guiElements.Clear();
		Action onGoToMainMenu = MainGame.OnGoToMainMenu;
		if (onGoToMainMenu != null)
		{
			onGoToMainMenu();
		}
		this.SetSystemsPauseState(true);
		this.gameSave.UnPrepareFromGame();
		this.playerController.UnPreparePlayerFromGame();
		Debug.Log("MainGame.GoToMenu");
		LazySingleton<ChunkManager>.Instance.IsActive = false;
		this.playerController.ClearCurrentGameScene();
		LazySingleton<GameSceneManager>.Instance.UnloadAllScenes();
		MainGame.PlayerController.SetDisabledStateType(DisabledStateType.ByMainMenu, false);
		LazyUI.Get<HUD>().SetDisableState(HudStateType.MainMenu, false, null);
		this.playerController.WispController.ChangeActiveState(false);
		WeatherSystem.Instance.ClearWeather();
		MainGame.SetMainMenuMixer(true);
		GUIElements.Instance.NpcWidget.Hide();
		this.ResetAllGameData();
		if (DevUtils.IsDemoBitsummitActive)
		{
			DevUtils.isMainSceneSkipped = false;
			this.StartNewGame(false);
		}
		else
		{
			LazyUI.GetWindow<UIMainMenuWindow>().Open(null);
		}
		await Awaitable.NextFrameAsync(default(CancellationToken));
		fadeElement.FadeOut(0.2f, null, fadeOutFlag);
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x000DD803 File Offset: 0x000DBA03
	private void PauseGame()
	{
		this.SetSystemsPauseState(true);
		LazySingleton<FightingGameController>.Instance.PauseAgentsMovement();
		Object3DMesh.SetDestructionTweenPauseState(true);
		MainGame.IsGamePaused = true;
		Action onGamePaused = MainGame.OnGamePaused;
		if (onGamePaused == null)
		{
			return;
		}
		onGamePaused();
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x000DD831 File Offset: 0x000DBA31
	private void UnpauseGame()
	{
		this.SetSystemsPauseState(false);
		LazySingleton<FightingGameController>.Instance.UnpauseAgentsMovement();
		Object3DMesh.SetDestructionTweenPauseState(false);
		MainGame.IsGamePaused = false;
		Action onGameUnpaused = MainGame.OnGameUnpaused;
		if (onGameUnpaused == null)
		{
			return;
		}
		onGameUnpaused();
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x000DD85F File Offset: 0x000DBA5F
	private void ResetAllGameData()
	{
		GameScene.ClearGlobalCaches();
		GlobalScriptsManager.TerminateAllRunningScripts();
		WgoDataScriptsManager.DestroyAll();
		UIMultiAnswer.ForceDisableAll();
		UISpeechBubble.ForceRemoveAll();
		Object3DOptimizedPart.ClearAtlasTextureCache();
		LazySingleton<ChunkManager>.Instance.ClearAll();
		Debug.Log("ResetAllGameData");
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x000DD894 File Offset: 0x000DBA94
	private async UniTask AfterSceneHasLoaded()
	{
		MainGame.PlayerController.SetDisabledStateType(DisabledStateType.BySceneLoading, true);
		UIFade uifade = LazyUI.Get<UIFade>();
		this.playerController.WispController.ChangeActiveState(this.GameSave.playerData.isWispEnabled);
		this.TrySetPlayerPosition();
		if (this.continueGame)
		{
			if (MainGame.PlayerData.HasOverheadItem)
			{
				MainGame.PlayerController.SetOverheadItems(MainGame.PlayerData.OverheadItems);
			}
			if (MainGame.PlayerData.tutorialArrowWgoId != null && !string.IsNullOrEmpty(MainGame.PlayerData.tutorialArrowWgoId.Id))
			{
				LazySingleton<UITutorialArrow>.Instance.Attach(this.GameSave.worldData.GetWgoData(MainGame.PlayerData.tutorialArrowWgoId));
			}
		}
		if (this.startingNewGame)
		{
			this.startingNewGame = false;
			uifade.FadeIn(0f, null, FadeFlag.FlowScript, false);
			await Awaitable.NextFrameAsync(default(CancellationToken));
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.StartNewGame, "");
		}
		CPDirectShadowsBlur.Initialize();
		LazyAudio.StopAllPlaylistsImmediately();
		LazyAudio.PlayPlaylist("gameplay");
		if (this.continueGame)
		{
			EnvironmentEngine.Instance.SetTimeOfDayPreset("indoor");
			await Awaitable.NextFrameAsync(default(CancellationToken));
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.AfterSleep, "");
			this.continueGame = false;
		}
		await Resources.UnloadUnusedAssets();
		GC.Collect();
		await Awaitable.NextFrameAsync(default(CancellationToken));
		await Awaitable.NextFrameAsync(default(CancellationToken));
		LazyUI.Get<UILoadingOverlay>().Hide();
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x000DD8D8 File Offset: 0x000DBAD8
	private void TrySetPlayerPosition()
	{
		if (!this.startingNewGame || this.continueGame)
		{
			return;
		}
		PlayerSpawnPoint playerSpawnPoint = global::UnityEngine.Object.FindObjectOfType<PlayerSpawnPoint>(true);
		if (playerSpawnPoint == null)
		{
			return;
		}
		this.playerController.SetPosition(playerSpawnPoint.transform.position, false, true);
		EnvironmentEngine.Instance.SetTimeOfDayPreset(playerSpawnPoint.environmentPreset);
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x000DD92F File Offset: 0x000DBB2F
	private PlayerController GetPlayerController()
	{
		PlayerController playerController = global::UnityEngine.Object.FindObjectOfType<PlayerController>(true);
		if (playerController == null)
		{
			throw new Exception("Can't find [PlayerController]");
		}
		return playerController;
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x000DD94C File Offset: 0x000DBB4C
	private void OnWindowOpened(LazyWidgetBase window)
	{
		if (LazyWindowsStackController.HasAnyModalWindowOpened && this.gameState == MainGame.GameState.InGame)
		{
			this.playerController.SetControlTakenType(TakenControlType.ByUI, false);
			if (!(window is UIFishingWindow) && !(window is UICreditsWindow))
			{
				this.PauseGame();
			}
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.OpenUIWindow, window.GetType().Name);
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x000DD9A0 File Offset: 0x000DBBA0
	private void OnWindowClosed(LazyWidgetBase window)
	{
		if (!LazyWindowsStackController.HasAnyModalWindowOpened && this.gameState == MainGame.GameState.InGame)
		{
			this.playerController.SetControlTakenType(TakenControlType.ByUI, true);
			if (!(window is UIFishingWindow) && !(window is UICreditsWindow))
			{
				this.UnpauseGame();
			}
		}
		if (window is UICraftWindow && !this.gameSave.playerData.openedCraftWindowOnce)
		{
			this.gameSave.playerData.openedCraftWindowOnce = true;
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FirstCloseCraftWindow, "");
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.CloseUIWindow, window.GetType().Name);
	}

	// Token: 0x06002E61 RID: 11873 RVA: 0x000DDA29 File Offset: 0x000DBC29
	private void OnAllWindowsClosed()
	{
		if (this.gameState == MainGame.GameState.InGame)
		{
			this.playerController.SetControlTakenType(TakenControlType.ByUI, true);
		}
	}

	// Token: 0x06002E62 RID: 11874 RVA: 0x00002318 File Offset: 0x00000518
	private void InitNetwork()
	{
	}

	// Token: 0x06002E63 RID: 11875 RVA: 0x000DDA41 File Offset: 0x000DBC41
	private void SetSystemsPauseState(bool isPaused)
	{
		Debug.Log(string.Format("Set systems pause state to {0}", isPaused));
		this.updateManager.IsActive = !isPaused;
	}

	// Token: 0x06002E64 RID: 11876 RVA: 0x000DDA67 File Offset: 0x000DBC67
	public void SetMainMenuInfoPanelEnabled(bool isEnabled)
	{
		if (isEnabled)
		{
			this.uiMainMenuInfoPanel.ShowFullPanel();
			return;
		}
		this.uiMainMenuInfoPanel.Hide();
	}

	// Token: 0x06002E65 RID: 11877 RVA: 0x000DDA84 File Offset: 0x000DBC84
	private void SetMainMenuEnabledState(bool isEnabled)
	{
		if (isEnabled)
		{
			this.uiMainMenuInfoPanel.ShowFullPanel();
		}
		else
		{
			this.uiMainMenuInfoPanel.ShowVersionInGameIfNeeded();
		}
		this.mainMenuViewObject.SetActive(isEnabled);
		this.ApplyMainMenuViewScaleByCurrentResolution();
		if (isEnabled)
		{
			CameraSystem.Instance.ActiveCameraController.SetTargetInstant(this.mainMenuViewObject.transform);
		}
		else
		{
			CameraSystem.Instance.ActiveCameraController.SetTargetInstant(this.playerController.PhysicalBody.PlayerView.transform);
		}
		CameraSystem.Instance.ActiveCameraController.UpdateTargetPosInstant();
	}

	// Token: 0x06002E66 RID: 11878 RVA: 0x000DDB10 File Offset: 0x000DBD10
	private static void SetMainMenuMixer(bool active)
	{
		if (WeatherSystem.Instance == null)
		{
			return;
		}
		WeatherSystem.Instance.AudioMixerStateController.SetLayerActive(AudioMixerSnapshotLayer.MainMenu, active);
	}

	// Token: 0x06002E67 RID: 11879 RVA: 0x000DDB31 File Offset: 0x000DBD31
	private void OnResolutionChanged(IntVector2 resolution)
	{
		this.ApplyMainMenuViewScaleByCurrentResolution();
	}

	// Token: 0x06002E68 RID: 11880 RVA: 0x000DDB3C File Offset: 0x000DBD3C
	private void ApplyMainMenuViewScaleByCurrentResolution()
	{
		if (this.mainMenuViewObject == null)
		{
			return;
		}
		bool flag = ResolutionConfig.currentResolution != null && ResolutionConfig.currentResolution.UseMainMenuScaleX2;
		this.mainMenuViewObject.transform.localScale = (flag ? this.mainMenuViewScaleX2 : this.mainMenuViewScaleX1);
	}

	// Token: 0x06002E6A RID: 11882 RVA: 0x000DDC7E File Offset: 0x000DBE7E
	[CompilerGenerated]
	internal static UniTask <RegisterBackgroundPreloadTasks>g__MeasureTask|92_0(string taskName, UniTask task)
	{
		return MainGame.<RegisterBackgroundPreloadTasks>g__MeasureTaskInternal|92_1(taskName, task);
	}

	// Token: 0x06002E6B RID: 11883 RVA: 0x000DDC88 File Offset: 0x000DBE88
	[CompilerGenerated]
	internal static async UniTask <RegisterBackgroundPreloadTasks>g__MeasureTaskInternal|92_1(string taskName, UniTask task)
	{
		await task;
	}

	// Token: 0x04002538 RID: 9528
	public static Action OnGameStarted;

	// Token: 0x04002539 RID: 9529
	public static Action OnGoToMainMenu;

	// Token: 0x0400253A RID: 9530
	public static Action OnGamePaused;

	// Token: 0x0400253B RID: 9531
	public static Action OnGameUnpaused;

	// Token: 0x0400253C RID: 9532
	public const string MAIN_SCENE = "MainScene";

	// Token: 0x0400253D RID: 9533
	public const string LOGO_SCENE = "Logos";

	// Token: 0x0400253E RID: 9534
	[SerializeField]
	private GUIElements guiElements;

	// Token: 0x0400253F RID: 9535
	[SerializeField]
	private GameSave gameSave;

	// Token: 0x04002540 RID: 9536
	public PlayerUniqueCommandHolder PlayerUniqueCommandHolder = new PlayerUniqueCommandHolder();

	// Token: 0x04002541 RID: 9537
	[SerializeField]
	private GameInfo gameInfo;

	// Token: 0x04002542 RID: 9538
	[SerializeField]
	private UIMainMenuInfoPanel uiMainMenuInfoPanel;

	// Token: 0x04002543 RID: 9539
	[SerializeField]
	private GameObject mainMenuViewObject;

	// Token: 0x04002544 RID: 9540
	[SerializeField]
	private Vector3 mainMenuViewScaleX1 = Vector3.one;

	// Token: 0x04002545 RID: 9541
	[SerializeField]
	private Vector3 mainMenuViewScaleX2 = Vector3.one * 2f;

	// Token: 0x04002546 RID: 9542
	private PlayerController playerController;

	// Token: 0x04002547 RID: 9543
	[SerializeField]
	private GraphHelper graphHelper;

	// Token: 0x04002548 RID: 9544
	[SerializeField]
	private GameObject playerPrefab;

	// Token: 0x04002549 RID: 9545
	[SerializeField]
	private UpdateManager updateManager;

	// Token: 0x0400254A RID: 9546
	[SerializeField]
	private string conveyorPresetLoadOnNewGame;

	// Token: 0x0400254B RID: 9547
	public MovementSystem movementSystem = new MovementSystem();

	// Token: 0x0400254C RID: 9548
	public CraftSystem craftSystem = new CraftSystem();

	// Token: 0x0400254D RID: 9549
	public DropSystem dropSystem = new DropSystem();

	// Token: 0x0400254E RID: 9550
	public FightingLevelSystem fightingLevelSystem = new FightingLevelSystem();

	// Token: 0x0400254F RID: 9551
	public PerkSystem perkSystem = new PerkSystem();

	// Token: 0x04002550 RID: 9552
	public GameLogicsSystem gameLogicsSystem = new GameLogicsSystem();

	// Token: 0x04002551 RID: 9553
	public QuestSystem questSystem = new QuestSystem();

	// Token: 0x04002552 RID: 9554
	public ZombieSystem zombieSystem = new ZombieSystem();

	// Token: 0x04002553 RID: 9555
	public ZombiePorterSystem zombiePorterSystem = new ZombiePorterSystem();

	// Token: 0x04002554 RID: 9556
	public ConveyorSystem conveyorSystem = new ConveyorSystem();

	// Token: 0x04002555 RID: 9557
	public RiverDropSystem riverDropSystem = new RiverDropSystem();

	// Token: 0x04002556 RID: 9558
	public WgoCustomDeathSystem wgoCustomDeathSystem = new WgoCustomDeathSystem();

	// Token: 0x04002557 RID: 9559
	public WgoDelayedEventsSystem wgoDelayedEventsSystem = new WgoDelayedEventsSystem();

	// Token: 0x04002558 RID: 9560
	public WgoDelayedSpawnSystem wgoDelayedSpawnSystem = new WgoDelayedSpawnSystem();

	// Token: 0x04002559 RID: 9561
	public NPCLifeSimulator npcLifeSimulator = new NPCLifeSimulator();

	// Token: 0x0400255A RID: 9562
	private SaveSlotData saveSlotData;

	// Token: 0x0400255B RID: 9563
	public List<GameSceneConfig> gameSceneConfigs = new List<GameSceneConfig>();

	// Token: 0x0400255C RID: 9564
	private bool startingNewGame;

	// Token: 0x0400255D RID: 9565
	private bool continueGame;

	// Token: 0x0400255E RID: 9566
	private bool deferStartupMainMenu;

	// Token: 0x0400255F RID: 9567
	private bool backgroundPreloadRegistered;

	// Token: 0x04002560 RID: 9568
	private float gameSceneLoadProgress;

	// Token: 0x04002565 RID: 9573
	private static string entrySceneToLoadCached;

	// Token: 0x020006D3 RID: 1747
	public enum GameState
	{
		// Token: 0x04002567 RID: 9575
		MainMenu,
		// Token: 0x04002568 RID: 9576
		InGame
	}
}
