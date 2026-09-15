using System;
using System.Collections.Generic;
using System.Globalization;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000461 RID: 1121
[Serializable]
public class GameSave : ISerializableData
{
	// Token: 0x17000506 RID: 1286
	// (get) Token: 0x06001D6A RID: 7530 RVA: 0x0008A61C File Offset: 0x0008881C
	// (set) Token: 0x06001D6B RID: 7531 RVA: 0x0008A624 File Offset: 0x00088824
	public string GameSaveVer
	{
		get
		{
			return this.gameSaveVersion;
		}
		set
		{
			this.gameSaveVersion = value;
			if (string.IsNullOrWhiteSpace(value))
			{
				this.saveVersion = default(GameSaveVersion);
				return;
			}
			this.saveVersion = GameSaveVersion.Parse(value);
		}
	}

	// Token: 0x17000507 RID: 1287
	// (get) Token: 0x06001D6C RID: 7532 RVA: 0x0008A64E File Offset: 0x0008884E
	public GameSaveVersion SaveVersion
	{
		get
		{
			return this.saveVersion;
		}
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x0008A768 File Offset: 0x00088968
	public static void SetupNewGameSave(GameSave save)
	{
		save.playerData = PlayerData.CreatePlayerData();
		save.talentSystemData = new TalentSystemData(GameBalance.Me.talentDefs);
		foreach (TechDef techDef in GameBalance.Me.techDefs)
		{
			if (techDef.availableAtStart)
			{
				save.knowledgeSystem.UnlockTech(techDef.id, true);
			}
			if (techDef.hiddenAtStart)
			{
				save.knowledgeSystem.hiddenTechs.Add(techDef.id);
			}
		}
		foreach (TalentDef talentDef in GameBalance.Me.talentDefs)
		{
			save.knowledgeSystem.unlockedTalentIds.Add(talentDef.id);
		}
		save.knowledgeSystem.revealedTechs = new List<string>();
		save.knowledgeSystem.revealedTalentLevelUps = new List<string>();
		foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
		{
			if (talentLevelUpDef.isHidden)
			{
				save.knowledgeSystem.hiddenTalentLevelUps.Add(talentLevelUpDef.id);
			}
			if (talentLevelUpDef.isUnknown)
			{
				save.knowledgeSystem.unknownTalentLevelUps.Add(talentLevelUpDef.id);
			}
		}
		foreach (AlchemyFormulaDef alchemyFormulaDef in GameBalance.Me.alchemyFormulaDefs)
		{
			if (alchemyFormulaDef.hiddenAtStart)
			{
				save.knowledgeSystem.hiddenAlchemyFormulas.Add(alchemyFormulaDef.id);
			}
		}
		foreach (SurveyDef surveyDef in GameBalance.Me.surveyDefs)
		{
			if (surveyDef.surveyedAtStart && !save.knowledgeSystem.oneTimeCompletedCrafts.Contains(surveyDef.id))
			{
				save.knowledgeSystem.oneTimeCompletedCrafts.Add(surveyDef.id);
			}
		}
		for (int i = 0; i < GameBalance.Me.vendorDefs.Count; i++)
		{
			VendorDef vendorDef = GameBalance.Me.vendorDefs[i];
			save.vendorSystem.vendors.Add(new Vendor(vendorDef.id, 0));
			if (!vendorDef.lockedByDefaultInOrdersWindow)
			{
				save.knowledgeSystem.unlockedVendorsForOrders.Add(vendorDef.id);
			}
		}
		for (int j = 0; j < ConstDef.Get("start_orders_count").IntValue; j++)
		{
			save.vendorSystem.currentOrders.Add(SGuid.Empty);
		}
		foreach (NPCGroupPointOfInterestConfiguration npcgroupPointOfInterestConfiguration in LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.AllGroups)
		{
			save.npcLifeSimulatorData.AddGroup(new NPCGroupPointOfInterestData(npcgroupPointOfInterestConfiguration));
		}
		foreach (NPCPointOfInterestConfiguration npcpointOfInterestConfiguration in LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.AllPonts)
		{
			save.npcLifeSimulatorData.AddPoint(new NPCPointOfInterestData(npcpointOfInterestConfiguration));
		}
		save.worldData.InitFromSceneConfigs(MainGame.Instance.gameSceneConfigs);
		save.questSystemData = new QuestSystemData(GameBalance.Me.questDefs);
		for (int k = 0; k < 40; k++)
		{
			string text = string.Format("zombie_name_{0}", k + 1);
			string text2 = LLBase.L(text);
			if (!(text == text2))
			{
				save.knowledgeSystem.freeZombieNames.Add(text);
			}
		}
		for (int l = 0; l < GameBalance.Me.inspirationDefs.Count; l++)
		{
			InspirationDef inspirationDef = GameBalance.Me.inspirationDefs[l];
			if (inspirationDef.inpsirationLocks.Count > 0 || inspirationDef.techLocks.Count > 0 || inspirationDef.questLocks.Count > 0)
			{
				string idWithoutLvl = inspirationDef.idWithoutLvl;
				if (!save.knowledgeSystem.hiddenInspirations.Contains(idWithoutLvl))
				{
					save.knowledgeSystem.hiddenInspirations.Add(idWithoutLvl);
				}
			}
		}
	}

	// Token: 0x17000508 RID: 1288
	// (get) Token: 0x06001D6F RID: 7535 RVA: 0x0008AC30 File Offset: 0x00088E30
	public WorldData WorldData
	{
		get
		{
			return this.worldData;
		}
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x0008AC38 File Offset: 0x00088E38
	public void PrepareForGame()
	{
		this.environmentData.PrepareForGame(EnvironmentEngine.Instance.gameplayDayInMinutes);
		this.globalEventsSystem.PrepareForGame();
		this.questSystemData.PrepareForGame();
		this.movementSystemData.RestoreMovingObjects(this.worldData);
		this.worldData.PrepareForGame();
		this.craftSystemData.RestoreActiveCrafts(this.worldData);
		MainGame.Instance.fightingLevelSystem.PrepareForGame();
		this.talentSystemData.PrepareForGame();
		this.gameLogicSystemData.PrepareForGame();
		this.playerData.PrepareForGame();
		this.knowledgeSystem.PrepareForGame();
		this.vendorSystem.PrepareForGame();
		this.zombieSystemData.PrepareForGame();
		this.militaryBaseData.PrepareForGame();
		this.npcLifeSimulatorData.PrepareForGame();
		this.conveyorSystemData.PrepareForGame(this.worldData);
		if (this.riverDropSystemData == null)
		{
			this.riverDropSystemData = new RiverDropSystemData();
		}
		MainGame instance = MainGame.Instance;
		if (instance != null)
		{
			RiverDropSystem riverDropSystem = instance.riverDropSystem;
			if (riverDropSystem != null)
			{
				riverDropSystem.ClearRuntimeState();
			}
		}
		foreach (string text in this.knowledgeSystem.knownMixCrafts)
		{
			GameBalance.GetAlchemyMixCraftDef(text);
		}
		AchievementsSystem achievementsSystem;
		if ((achievementsSystem = this.achievementsSystem) == null)
		{
			achievementsSystem = (this.achievementsSystem = new AchievementsSystem());
		}
		achievementsSystem.PrepareForGame();
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x0008ADAC File Offset: 0x00088FAC
	public void UnPrepareFromGame()
	{
		MainGame instance = MainGame.Instance;
		if (instance != null)
		{
			instance.fightingLevelSystem.UnprepareFromGame();
		}
		MainGame instance2 = MainGame.Instance;
		if (instance2 != null)
		{
			RiverDropSystem riverDropSystem = instance2.riverDropSystem;
			if (riverDropSystem != null)
			{
				riverDropSystem.ClearRuntimeState();
			}
		}
		this.playerData.UnPrepareFromGame();
		this.worldData.UnPrepareFromGame();
		this.knowledgeSystem.UnPrepareForGame();
		TalentSystemData talentSystemData = this.talentSystemData;
		if (talentSystemData == null)
		{
			return;
		}
		talentSystemData.UnPrepareFromGame();
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x0008AE1C File Offset: 0x0008901C
	public NetworkPlayer CreateClient(int clientId)
	{
		PlayerData playerData = PlayerData.CreatePlayerData();
		playerData.TryApplyStartState();
		NetworkPlayer networkPlayer = new NetworkPlayer(clientId, playerData);
		this.clientPlayers.Add(networkPlayer);
		return networkPlayer;
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x0008AE4C File Offset: 0x0008904C
	public bool GetClient(int clientId, out NetworkPlayer clientPlayer, bool considerHost = false)
	{
		clientPlayer = null;
		if (considerHost && clientId == this.hostPlayer.clientId)
		{
			clientPlayer = this.hostPlayer;
			return true;
		}
		foreach (NetworkPlayer networkPlayer in this.clientPlayers)
		{
			if (networkPlayer.clientId == clientId)
			{
				clientPlayer = networkPlayer;
				return true;
			}
		}
		Debug.LogError(string.Format("Client with clientId [{0}] not found", clientId));
		return false;
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00002318 File Offset: 0x00000518
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x0008AEE0 File Offset: 0x000890E0
	public void OnAfterSerialize()
	{
		if (this.wgoDelayedEventSystemData == null)
		{
			this.wgoDelayedEventSystemData = new WgoDelayedEventSystemData();
		}
		WgoDelayedEventSystemData wgoDelayedEventSystemData = this.wgoDelayedEventSystemData;
		if (wgoDelayedEventSystemData.wgoUniqueIds == null)
		{
			wgoDelayedEventSystemData.wgoUniqueIds = new List<SGuid>();
		}
		if (!string.IsNullOrEmpty(this.gameSaveVersion))
		{
			this.saveVersion = GameSaveVersion.Parse(this.gameSaveVersion);
		}
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x0008AF38 File Offset: 0x00089138
	public void PrepareToSave(SaveSlotData slotData)
	{
		slotData.gameSaveVersion = this.GameSaveVer;
		slotData.day = this.environmentData.Day;
		slotData.platform = LazyAPI.Platform.GetPlatformName();
		slotData.serializedCulture = CultureInfo.CurrentCulture.Name;
		slotData.saveDateTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);
		WorldZoneData worldZoneDataById = this.WorldData.GetWorldZoneDataById("graveyard");
		if (worldZoneDataById != null)
		{
			slotData.graveyardQuality = (int)worldZoneDataById.GetTotalQuality();
		}
		WorldZoneData worldZoneDataById2 = this.WorldData.GetWorldZoneDataById("church");
		if (worldZoneDataById2 != null)
		{
			slotData.churchQuality = (int)worldZoneDataById2.GetTotalQuality();
		}
		slotData.villageRep = this.playerData.GetNPCRep("village_REP");
		slotData.isDemoSave = false;
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x0008AFFC File Offset: 0x000891FC
	public void GivePreorderReward()
	{
		string text = "9007_bdy_01_clr_01_palette";
		string text2 = "9007_bdy_02_clr_01_palette";
		string text3 = "9007_bdy_03_clr_01_palette";
		this.playerData.customization.UnlockCustomizationPart("bdy_9007", CustomizablePartType.Body);
		this.playerData.customization.UnlockCustomizationColor(PlayerColorCustomizationType.Bdy1, text, 9007);
		this.playerData.customization.UnlockCustomizationColor(PlayerColorCustomizationType.Bdy2, text2, 9007);
		this.playerData.customization.UnlockCustomizationColor(PlayerColorCustomizationType.Bdy3, text3, 9007);
		GameScriptUtility.RunGlobalScript("DLC_BathhouseSpawn", null);
		Debug.Log("GivePreorderReward success");
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x0008B0A4 File Offset: 0x000892A4
	public void TryRemovePreorderReward()
	{
		Debug.Log("No preorder available TryRemovePreorderReward");
		PlayerCustomizationData customization = this.playerData.customization;
		bool flag = GameSave.IsCustomizationPartSelected(customization, CustomizablePartType.Body, "bdy_9007");
		bool flag2 = GameSave.IsCustomizationPartSelected(customization, CustomizablePartType.Arms, "arm_9007");
		GameSave.RemoveUnlockedPart(customization, CustomizablePartType.Body, "bdy_9007");
		GameSave.RemoveUnlockedPart(customization, CustomizablePartType.Arms, "arm_9007");
		string[] preorderRewardPaletteNames = GameSave.GetPreorderRewardPaletteNames();
		for (int i = 0; i < preorderRewardPaletteNames.Length; i++)
		{
			GameSave.RemoveUnlockedColor(customization, PlayerColorCustomizationType.Bdy1, 9007, preorderRewardPaletteNames[i]);
		}
		if (flag)
		{
			GameSave.SetCustomizationPart(customization, CustomizablePartType.Body, GameSave.GetDefaultCustomizationPartId(CustomizablePartType.Body));
			GameSave.SetCustomizationPart(customization, CustomizablePartType.Arms, GameSave.GetDefaultCustomizationPartId(CustomizablePartType.Arms));
			customization.SetColorCustomizationIndexForType(PlayerColorCustomizationType.Bdy1, 0);
		}
		else if (flag2)
		{
			GameSave.SetCustomizationPart(customization, CustomizablePartType.Arms, GameSave.GetDefaultCustomizationPartId(CustomizablePartType.Arms));
		}
		WgoData wgoData;
		GameSceneData gameSceneData;
		if (this.WorldData.TryGetWgoData("bathhouse", out wgoData, out gameSceneData))
		{
			this.WorldData.RemoveWgoDataFromGameScene(wgoData, true);
			Debug.Log("RemovePreorderReward success");
		}
		WgoData wgoData2;
		if (this.WorldData.TryGetWgoData("tp_RT_bathhouse_enter", out wgoData2, out gameSceneData))
		{
			this.WorldData.RemoveWgoDataFromGameScene(wgoData2, true);
		}
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x0008B1AA File Offset: 0x000893AA
	private static string[] GetPreorderRewardPaletteNames()
	{
		return new string[] { "9007_bdy_01_clr_01_palette", "9007_bdy_02_clr_01_palette", "9007_bdy_03_clr_01_palette" };
	}

	// Token: 0x06001D7A RID: 7546 RVA: 0x0008B1CC File Offset: 0x000893CC
	private static void RemoveUnlockedPart(PlayerCustomizationData customization, CustomizablePartType type, string id)
	{
		UnlockedCustomizationPartData unlockedCustomizationPartData = customization.unlockedCustomizationPartsData.Find((UnlockedCustomizationPartData x) => x.type == type);
		if (unlockedCustomizationPartData == null)
		{
			return;
		}
		unlockedCustomizationPartData.unlockedIds.Remove(id);
		if (unlockedCustomizationPartData.unlockedIds.Count == 0)
		{
			customization.unlockedCustomizationPartsData.Remove(unlockedCustomizationPartData);
		}
	}

	// Token: 0x06001D7B RID: 7547 RVA: 0x0008B22C File Offset: 0x0008942C
	private static void RemoveUnlockedColor(PlayerCustomizationData customization, PlayerColorCustomizationType type, int partSkinId, string paletteName)
	{
		UnlockedColorCustomizationData unlockedColorCustomizationData = customization.unlockedColorCustomizationData.Find((UnlockedColorCustomizationData x) => x.type == type && x.partSkinId == partSkinId);
		if (unlockedColorCustomizationData == null)
		{
			return;
		}
		unlockedColorCustomizationData.unlockedNames.Remove(paletteName);
		if (unlockedColorCustomizationData.unlockedNames.Count == 0 && unlockedColorCustomizationData.unlockedIndices.Count == 0)
		{
			customization.unlockedColorCustomizationData.Remove(unlockedColorCustomizationData);
		}
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x0008B2A0 File Offset: 0x000894A0
	private static bool IsCustomizationPartSelected(PlayerCustomizationData customization, CustomizablePartType type, string id)
	{
		PlayerCustomizationPartData playerCustomizationPartData = customization.customizationPartsData.Find((PlayerCustomizationPartData x) => x.type == type);
		return playerCustomizationPartData != null && playerCustomizationPartData.id == id;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x0008B2E4 File Offset: 0x000894E4
	private static void SetCustomizationPart(PlayerCustomizationData customization, CustomizablePartType type, string id)
	{
		PlayerCustomizationPartData playerCustomizationPartData = customization.customizationPartsData.Find((PlayerCustomizationPartData x) => x.type == type);
		if (playerCustomizationPartData != null)
		{
			playerCustomizationPartData.id = id;
			return;
		}
		customization.customizationPartsData.Add(new PlayerCustomizationPartData(id, type));
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x0008B338 File Offset: 0x00089538
	private static string GetDefaultCustomizationPartId(CustomizablePartType type)
	{
		PlayerCustomizationPartData playerCustomizationPartData = PlayerSkinHelper.DefaultCustomizationParts.Find((PlayerCustomizationPartData x) => x.type == type);
		if (playerCustomizationPartData == null)
		{
			return string.Empty;
		}
		return playerCustomizationPartData.id;
	}

	// Token: 0x04001B24 RID: 6948
	private const string PREORDER_REWARD_BODY_PART_ID = "bdy_9007";

	// Token: 0x04001B25 RID: 6949
	private const string PREORDER_REWARD_ARMS_PART_ID = "arm_9007";

	// Token: 0x04001B26 RID: 6950
	private const int PREORDER_REWARD_PART_SKIN_ID = 9007;

	// Token: 0x04001B27 RID: 6951
	[SerializeField]
	private string gameSaveVersion;

	// Token: 0x04001B28 RID: 6952
	public bool isDemoSaveLoadedInReleaseCustomActionsApplied;

	// Token: 0x04001B29 RID: 6953
	public WorldData worldData = new WorldData();

	// Token: 0x04001B2A RID: 6954
	public EnvironmentData environmentData = new EnvironmentData();

	// Token: 0x04001B2B RID: 6955
	public GameLogicsSystemData gameLogicSystemData = new GameLogicsSystemData();

	// Token: 0x04001B2C RID: 6956
	public PlayerData playerData = new PlayerData();

	// Token: 0x04001B2D RID: 6957
	public TalentSystemData talentSystemData;

	// Token: 0x04001B2E RID: 6958
	public PerkSystemData perkSystemData = new PerkSystemData();

	// Token: 0x04001B2F RID: 6959
	public KnowledgeSystem knowledgeSystem = new KnowledgeSystem();

	// Token: 0x04001B30 RID: 6960
	public GlobalEventsSystem globalEventsSystem = new GlobalEventsSystem();

	// Token: 0x04001B31 RID: 6961
	public QuestSystemData questSystemData = new QuestSystemData();

	// Token: 0x04001B32 RID: 6962
	public VendorSystem vendorSystem = new VendorSystem();

	// Token: 0x04001B33 RID: 6963
	public TownSystem townSystem = new TownSystem();

	// Token: 0x04001B34 RID: 6964
	public WeatherData weatherData = new WeatherData();

	// Token: 0x04001B35 RID: 6965
	public CraftSystemData craftSystemData = new CraftSystemData();

	// Token: 0x04001B36 RID: 6966
	public MovementSystemData movementSystemData = new MovementSystemData();

	// Token: 0x04001B37 RID: 6967
	public ZombieSystemData zombieSystemData = new ZombieSystemData();

	// Token: 0x04001B38 RID: 6968
	public ConveyorSystemData conveyorSystemData = new ConveyorSystemData();

	// Token: 0x04001B39 RID: 6969
	public RiverDropSystemData riverDropSystemData = new RiverDropSystemData();

	// Token: 0x04001B3A RID: 6970
	public MilitaryBaseData militaryBaseData = new MilitaryBaseData();

	// Token: 0x04001B3B RID: 6971
	public WgoCustomDeathSystemData wgoCustomDeathSystemData = new WgoCustomDeathSystemData();

	// Token: 0x04001B3C RID: 6972
	public NPCLifeSimulatorData npcLifeSimulatorData = new NPCLifeSimulatorData();

	// Token: 0x04001B3D RID: 6973
	public WgoDelayedEventSystemData wgoDelayedEventSystemData = new WgoDelayedEventSystemData();

	// Token: 0x04001B3E RID: 6974
	public WgoDelayedSpawnSystemData wgoDelayedSpawnSystemData = new WgoDelayedSpawnSystemData();

	// Token: 0x04001B3F RID: 6975
	public AchievementsSystem achievementsSystem = new AchievementsSystem();

	// Token: 0x04001B40 RID: 6976
	public NetworkPlayer hostPlayer;

	// Token: 0x04001B41 RID: 6977
	public List<NetworkPlayer> clientPlayers = new List<NetworkPlayer>();

	// Token: 0x04001B42 RID: 6978
	[NonSerialized]
	private GameSaveVersion saveVersion;
}
