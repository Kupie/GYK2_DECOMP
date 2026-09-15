using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
[Serializable]
public class WorldData
{
	// Token: 0x14000089 RID: 137
	// (add) Token: 0x0600271A RID: 10010 RVA: 0x000B7FA0 File Offset: 0x000B61A0
	// (remove) Token: 0x0600271B RID: 10011 RVA: 0x000B7FD4 File Offset: 0x000B61D4
	public static event Action<SGuid, SGuid> OnDockPointHasToBeDisabled;

	// Token: 0x1400008A RID: 138
	// (add) Token: 0x0600271C RID: 10012 RVA: 0x000B8008 File Offset: 0x000B6208
	// (remove) Token: 0x0600271D RID: 10013 RVA: 0x000B803C File Offset: 0x000B623C
	public static event Action<SGuid, SGuid> OnDockPointFreed;

	// Token: 0x1700064F RID: 1615
	// (get) Token: 0x0600271E RID: 10014 RVA: 0x000B806F File Offset: 0x000B626F
	public WgoDataCache Cache
	{
		get
		{
			this.TryInitCache();
			return this.cache;
		}
	}

	// Token: 0x17000650 RID: 1616
	// (get) Token: 0x0600271F RID: 10015 RVA: 0x000B807D File Offset: 0x000B627D
	public bool HasCache
	{
		get
		{
			return this.cache != null;
		}
	}

	// Token: 0x17000651 RID: 1617
	// (get) Token: 0x06002720 RID: 10016 RVA: 0x000B8088 File Offset: 0x000B6288
	public List<string> LoadedScenes
	{
		get
		{
			return LazySingleton<GameSceneManager>.Instance.LoadedGameSceneIds;
		}
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x000B80C8 File Offset: 0x000B62C8
	public void PrepareForGame()
	{
		for (int i = 0; i < this.gameSceneDataList.Count; i++)
		{
			this.gameSceneDataList[i].PrepareForGame(this.Cache);
		}
		this.gdPointsData.PrepareForGame(MainGame.Instance.gameSceneConfigs);
		this.TryInitCache();
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x000B8120 File Offset: 0x000B6320
	public void InitFromSceneConfigs(List<GameSceneConfig> configs)
	{
		this.gameSceneDataList.Clear();
		foreach (GameSceneConfig gameSceneConfig in configs)
		{
			List<SceneWgoContentData> list;
			if (gameSceneConfig.TryLoadSceneDataContent(out list))
			{
				GameSceneData gameSceneData = new GameSceneData(gameSceneConfig.name, gameSceneConfig.sceneGlobalPosition, this.Cache);
				this.gameSceneDataList.Add(gameSceneData);
				this.CreateGameSceneDataFromConfig(gameSceneData, gameSceneConfig, list);
				foreach (SceneWgoContentData sceneWgoContentData in list)
				{
					SceneWgoContentPart component = sceneWgoContentData.GetComponent<SceneWgoContentPart>();
					if (!(component == null) && !(component is FightingLevel))
					{
						gameSceneConfig.UnloadSceneDataContentByObject(component.gameObject);
					}
				}
			}
		}
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x000B820C File Offset: 0x000B640C
	public void UnloadContentData(GameSceneConfig config, GameSceneData sceneData, string contentName)
	{
		GameObject gameObject;
		if (!config.TryGetLoadedContent(contentName, out gameObject))
		{
			Debug.LogError("Can't get loaded content data ref: " + contentName);
			return;
		}
		SceneWgoContentData sceneWgoContentData;
		if (gameObject == null || !gameObject.TryGetComponent<SceneWgoContentData>(out sceneWgoContentData))
		{
			Debug.Log("Can't get content data instance: " + contentName);
			return;
		}
		this.DeInitDataFromConfig(sceneData, sceneWgoContentData);
		config.TryUnloadSceneDataContent(contentName);
		Debug.Log("Unloaded content data: " + contentName);
	}

	// Token: 0x06002725 RID: 10021 RVA: 0x000B827C File Offset: 0x000B647C
	public void UnPrepareFromGame()
	{
		foreach (GameSceneConfig gameSceneConfig in MainGame.Instance.gameSceneConfigs)
		{
			gameSceneConfig.UnloadSceneDataContents();
		}
	}

	// Token: 0x06002726 RID: 10022 RVA: 0x000B82D0 File Offset: 0x000B64D0
	public WgoData GetWgoDataForWisp(WispController wispController)
	{
		if (!this.HasCache)
		{
			return null;
		}
		WispData orCreateWispData = this.GetOrCreateWispData(wispController, "");
		return this.GetWgoData(orCreateWispData.linkedWgoId);
	}

	// Token: 0x06002727 RID: 10023 RVA: 0x000B8300 File Offset: 0x000B6500
	public WispData GetOrCreateWispData(WispController wispController, string gameSceneId = "")
	{
		if (string.IsNullOrEmpty(gameSceneId))
		{
			gameSceneId = MainGame.EntrySceneToLoad;
		}
		WispData wispData = this.wispDataList.Find((WispData d) => d.wispId == wispController.Id);
		if (wispData == null)
		{
			wispData = new WispData();
			wispData.wispId = wispController.Id;
			WgoData wgoData = new WgoData(wispController.Id, Vector3.zero, gameSceneId);
			wgoData.direction.Value = Direction.Left.ConvertToVector3();
			MainGame.Instance.GameSave.worldData.AddWgoData(wgoData);
			wispData.linkedWgoId = wgoData.UniqueId;
			this.wispDataList.Add(wispData);
		}
		return wispData;
	}

	// Token: 0x06002728 RID: 10024 RVA: 0x000B83B6 File Offset: 0x000B65B6
	public void AddGameSceneData(GameSceneData data)
	{
		this.gameSceneDataList.Add(data);
	}

	// Token: 0x06002729 RID: 10025 RVA: 0x000B83C4 File Offset: 0x000B65C4
	[NetworkMethod(typeof(WorldDataCommand), "AddWgoDataToGameScene", new object[] { })]
	public virtual void AddWgoData(WgoData data)
	{
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(data.WorldId, out gameSceneData))
		{
			gameSceneData.AddWgoData(data, false);
		}
	}

	// Token: 0x0600272A RID: 10026 RVA: 0x000B83EC File Offset: 0x000B65EC
	public void AddWgoData(WgoData data, bool recheckVisibilityOnSpawn)
	{
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(data.WorldId, out gameSceneData))
		{
			gameSceneData.AddWgoData(data, recheckVisibilityOnSpawn);
		}
	}

	// Token: 0x0600272B RID: 10027 RVA: 0x000B8414 File Offset: 0x000B6614
	public bool AddWgoData(string wgoId, Vector3 position, string gameSceneId, string customTag, out WgoData wgoData, bool recheckVisibilityOnSpawn = false)
	{
		wgoData = null;
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(gameSceneId, out gameSceneData))
		{
			wgoData = gameSceneData.AddWgoData(wgoId, position, customTag, recheckVisibilityOnSpawn);
		}
		return wgoData != null;
	}

	// Token: 0x0600272C RID: 10028 RVA: 0x000B8448 File Offset: 0x000B6648
	[NetworkMethod(typeof(WorldDataCommand), "RemoveWgoDataFromGameScene", new object[] { })]
	public virtual void RemoveWgoDataFromGameScene(WgoData wgoData, bool clearCraftComponent = true)
	{
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(wgoData.WorldId, out gameSceneData))
		{
			string id = wgoData.id;
			gameSceneData.RemoveWgoData(wgoData, clearCraftComponent);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.RemoveWgoDataFromScene, id);
		}
	}

	// Token: 0x0600272D RID: 10029 RVA: 0x000B847C File Offset: 0x000B667C
	public void RemoveWgoDataFromGameScene(SGuid sGuid)
	{
		WgoData wgoData = this.GetWgoData(sGuid);
		GameSceneData gameSceneData;
		if (wgoData != null && this.TryGetGameSceneDataById(wgoData.WorldId, out gameSceneData))
		{
			string id = wgoData.id;
			gameSceneData.RemoveWgoData(wgoData, true);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.RemoveWgoDataFromScene, id);
			return;
		}
		Debug.LogError(string.Format("WgoData by uniqueId [{0}] wasn't found", sGuid));
	}

	// Token: 0x0600272E RID: 10030 RVA: 0x000B84CC File Offset: 0x000B66CC
	public WgoData ReplaceWgoData(WgoData wgoData, string newWgoId)
	{
		this.RemoveWgoDataFromGameScene(wgoData, true);
		WgoData wgoData2 = new WgoData(newWgoId, wgoData.Position, wgoData.WorldId);
		wgoData2.SpawnWGOComponent.SpawnStages = wgoData.SpawnWGOComponent.SpawnStages;
		this.AddWgoData(wgoData2);
		return wgoData2;
	}

	// Token: 0x0600272F RID: 10031 RVA: 0x000B8512 File Offset: 0x000B6712
	public void ChangeWgoData(WgoData wgoData, string newWgoId)
	{
		this.RemoveWgoDataFromGameScene(wgoData, false);
		wgoData.ChangeId(newWgoId);
		this.AddWgoData(wgoData);
	}

	// Token: 0x06002730 RID: 10032 RVA: 0x000B852A File Offset: 0x000B672A
	public void MoveWgoDataToAnotherGameScene(WgoData wgoData, string gameSceneIdTo)
	{
		this.RemoveWgoDataFromGameScene(wgoData, true);
		wgoData.WorldId = gameSceneIdTo;
		this.AddWgoData(wgoData);
	}

	// Token: 0x06002731 RID: 10033 RVA: 0x000B8544 File Offset: 0x000B6744
	public GameSceneData GetGameSceneDataById(string id)
	{
		return this.gameSceneDataList.Find((GameSceneData x) => x.id == id);
	}

	// Token: 0x06002732 RID: 10034 RVA: 0x000B8578 File Offset: 0x000B6778
	public bool TryGetGameSceneDataForContent(string contentName, out GameSceneData sceneData, out GameSceneConfig config)
	{
		sceneData = null;
		config = null;
		if (string.IsNullOrEmpty(contentName))
		{
			return false;
		}
		foreach (GameSceneConfig gameSceneConfig in MainGame.Instance.gameSceneConfigs)
		{
			if (gameSceneConfig.GetContentDataRef(contentName) != null)
			{
				config = gameSceneConfig;
				sceneData = this.GetGameSceneDataById(gameSceneConfig.name);
				return sceneData != null;
			}
		}
		return false;
	}

	// Token: 0x06002733 RID: 10035 RVA: 0x000B8600 File Offset: 0x000B6800
	public bool TryGetGameSceneDataForContentGuid(string assetGuid, out GameSceneData sceneData, out GameSceneConfig config)
	{
		return this.TryGetGameSceneDataForContentGuid(assetGuid, MainGame.Instance.gameSceneConfigs, out sceneData, out config);
	}

	// Token: 0x06002734 RID: 10036 RVA: 0x000B8618 File Offset: 0x000B6818
	public bool TryGetGameSceneDataForContentGuid(string assetGuid, IList<GameSceneConfig> configs, out GameSceneData sceneData, out GameSceneConfig config)
	{
		sceneData = null;
		config = null;
		if (string.IsNullOrEmpty(assetGuid) || configs == null)
		{
			return false;
		}
		GameSceneConfig gameSceneConfig = null;
		foreach (GameSceneConfig gameSceneConfig2 in configs)
		{
			if (!(gameSceneConfig2 == null) && gameSceneConfig2.GetContentDataRefByGuid(assetGuid) != null)
			{
				GameSceneData gameSceneDataById = this.GetGameSceneDataById(gameSceneConfig2.name);
				if (gameSceneDataById != null)
				{
					config = gameSceneConfig2;
					sceneData = gameSceneDataById;
					return true;
				}
				if (gameSceneConfig == null)
				{
					gameSceneConfig = gameSceneConfig2;
				}
			}
		}
		config = gameSceneConfig;
		return false;
	}

	// Token: 0x06002735 RID: 10037 RVA: 0x000B86AC File Offset: 0x000B68AC
	public WgoData GetWgoData(SGuid sGuid)
	{
		if (sGuid == null)
		{
			return null;
		}
		return this.Cache.wgoDataByUidCache.GetValueOrDefault(sGuid.Guid);
	}

	// Token: 0x06002736 RID: 10038 RVA: 0x000B86D0 File Offset: 0x000B68D0
	public WgoData GetWgoData(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		List<WgoData> list;
		if (this.Cache.wgoDataByIdsCache.TryGetValue(id, out list))
		{
			return list[0];
		}
		return null;
	}

	// Token: 0x06002737 RID: 10039 RVA: 0x000B8708 File Offset: 0x000B6908
	public List<WgoData> GetWgoDataList(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return new List<WgoData>();
		}
		List<WgoData> list;
		if (this.Cache.wgoDataByIdsCache.TryGetValue(id, out list))
		{
			return list;
		}
		return new List<WgoData>();
	}

	// Token: 0x06002738 RID: 10040 RVA: 0x000B8740 File Offset: 0x000B6940
	public WgoData GetWgoDataByCustomTag(string customTag)
	{
		if (string.IsNullOrEmpty(customTag))
		{
			return null;
		}
		List<WgoData> list;
		if (this.Cache.wgoDataByCustomTagsCache.TryGetValue(customTag, out list))
		{
			return list[0];
		}
		return null;
	}

	// Token: 0x06002739 RID: 10041 RVA: 0x000B8778 File Offset: 0x000B6978
	public List<WgoData> GetWgoDataListByCustomTag(string customTag)
	{
		if (string.IsNullOrEmpty(customTag))
		{
			return null;
		}
		List<WgoData> list;
		if (this.Cache.wgoDataByCustomTagsCache.TryGetValue(customTag, out list))
		{
			return list;
		}
		return null;
	}

	// Token: 0x0600273A RID: 10042 RVA: 0x000B87A8 File Offset: 0x000B69A8
	public List<WgoData> GetWgoDataListByGroup(string wgoGroup)
	{
		if (string.IsNullOrEmpty(wgoGroup))
		{
			return null;
		}
		List<WgoData> list;
		if (this.Cache.wgoDataByGroup.TryGetValue(wgoGroup, out list))
		{
			return list;
		}
		return null;
	}

	// Token: 0x0600273B RID: 10043 RVA: 0x000B87D8 File Offset: 0x000B69D8
	public WsoData GetWsoDataByCustomTag(string customTag)
	{
		if (string.IsNullOrEmpty(customTag))
		{
			return null;
		}
		List<WsoData> list;
		if (this.Cache.wsoDataByCustomTagsCache.TryGetValue(customTag, out list))
		{
			return list[0];
		}
		return null;
	}

	// Token: 0x0600273C RID: 10044 RVA: 0x000B8810 File Offset: 0x000B6A10
	public List<WsoData> GetWsoDataListByCustomTag(string customTag)
	{
		if (string.IsNullOrEmpty(customTag))
		{
			return null;
		}
		List<WsoData> list;
		if (this.Cache.wsoDataByCustomTagsCache.TryGetValue(customTag, out list))
		{
			return list;
		}
		return null;
	}

	// Token: 0x0600273D RID: 10045 RVA: 0x000B8840 File Offset: 0x000B6A40
	public bool TryGetWgoData(string wgoId, out WgoData foundWgoData, out GameSceneData foundGameScene)
	{
		foundWgoData = null;
		foundGameScene = null;
		List<WgoData> list;
		if (this.Cache.wgoDataByIdsCache.TryGetValue(wgoId, out list))
		{
			foundWgoData = list[0];
			foundGameScene = this.GetGameSceneDataById(list[0].WorldId);
			return true;
		}
		return false;
	}

	// Token: 0x0600273E RID: 10046 RVA: 0x000B8888 File Offset: 0x000B6A88
	public WorldZoneData GetWorldZoneDataById(string id)
	{
		foreach (GameSceneData gameSceneData in this.gameSceneDataList)
		{
			WorldZoneData worldZoneDataById = gameSceneData.GetWorldZoneDataById(id);
			if (worldZoneDataById != null)
			{
				return worldZoneDataById;
			}
		}
		return null;
	}

	// Token: 0x0600273F RID: 10047 RVA: 0x000B88E4 File Offset: 0x000B6AE4
	public void NotifyDockPointHasToBeDisabled(SGuid parent, SGuid whoWasStayed)
	{
		Debug.Log(string.Format("{0}: [{1}], [{2}]", "NotifyDockPointHasToBeDisabled", parent, whoWasStayed));
		Action<SGuid, SGuid> onDockPointHasToBeDisabled = WorldData.OnDockPointHasToBeDisabled;
		if (onDockPointHasToBeDisabled == null)
		{
			return;
		}
		onDockPointHasToBeDisabled(parent, whoWasStayed);
	}

	// Token: 0x06002740 RID: 10048 RVA: 0x000B890D File Offset: 0x000B6B0D
	public void NotifyDockPointFreed(SGuid parent, SGuid whoFreesIt)
	{
		Action<SGuid, SGuid> onDockPointFreed = WorldData.OnDockPointFreed;
		if (onDockPointFreed == null)
		{
			return;
		}
		onDockPointFreed(parent, whoFreesIt);
	}

	// Token: 0x06002741 RID: 10049 RVA: 0x000B8920 File Offset: 0x000B6B20
	public void AddWsoData(WsoData data)
	{
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(data.WorldId, out gameSceneData))
		{
			gameSceneData.AddWsoData(data);
		}
	}

	// Token: 0x06002742 RID: 10050 RVA: 0x000B8944 File Offset: 0x000B6B44
	public bool AddWsoData(string defId, Vector3 position, string gameSceneId, out WsoData wsoData)
	{
		wsoData = null;
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(gameSceneId, out gameSceneData))
		{
			wsoData = gameSceneData.AddWsoData(defId, position);
		}
		return wsoData != null;
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x000B8974 File Offset: 0x000B6B74
	public void RemoveWsoDataFromGameScene(WsoData wsoData)
	{
		GameSceneData gameSceneData;
		if (this.TryGetGameSceneDataById(wsoData.WorldId, out gameSceneData))
		{
			gameSceneData.RemoveWsoData(wsoData);
		}
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x000B8998 File Offset: 0x000B6B98
	public void RemoveWsoDataFromGameScene(SGuid sGuid)
	{
		WsoData wsoData = this.GetWsoData(sGuid);
		GameSceneData gameSceneData;
		if (wsoData != null && this.TryGetGameSceneDataById(wsoData.WorldId, out gameSceneData))
		{
			gameSceneData.RemoveWsoData(wsoData);
			return;
		}
		Debug.LogError(string.Format("WsoData by uniqueId [{0}] wasn't found", sGuid));
	}

	// Token: 0x06002745 RID: 10053 RVA: 0x000B89D8 File Offset: 0x000B6BD8
	public WsoData GetWsoData(SGuid sGuid)
	{
		if (sGuid == null)
		{
			return null;
		}
		return this.Cache.wsoDataByUidCache.GetValueOrDefault(sGuid.Guid);
	}

	// Token: 0x06002746 RID: 10054 RVA: 0x000B89FC File Offset: 0x000B6BFC
	public List<WsoData> GetWsoDataByDefId(string defId)
	{
		if (string.IsNullOrEmpty(defId))
		{
			return new List<WsoData>();
		}
		List<WsoData> list;
		if (this.Cache.wsoDataByIdCache.TryGetValue(defId, out list))
		{
			return new List<WsoData>(list);
		}
		return new List<WsoData>();
	}

	// Token: 0x06002747 RID: 10055 RVA: 0x000B8A38 File Offset: 0x000B6C38
	public List<WsoData> GetWsoDataForScene(string worldId)
	{
		GameSceneData gameSceneDataById = this.GetGameSceneDataById(worldId);
		return ((gameSceneDataById != null) ? gameSceneDataById.wsoDataList : null) ?? new List<WsoData>();
	}

	// Token: 0x06002748 RID: 10056 RVA: 0x000B8A58 File Offset: 0x000B6C58
	public void TryInitCache()
	{
		if (!this.HasCache)
		{
			this.cache = new WgoDataCache();
			foreach (GameSceneData gameSceneData in this.gameSceneDataList)
			{
				foreach (WgoData wgoData in gameSceneData.wgoDataList)
				{
					this.cache.AddWgoDataToCache(wgoData);
				}
				foreach (WsoData wsoData in gameSceneData.wsoDataList)
				{
					this.cache.AddWsoDataToCache(wsoData);
				}
			}
		}
	}

	// Token: 0x06002749 RID: 10057 RVA: 0x000B8B54 File Offset: 0x000B6D54
	public void AddContentDataFromConfig(GameSceneData sceneData, GameSceneConfig gameSceneConfig, SceneWgoContentData sceneWgoContentData)
	{
		this.AddDataToSceneFromContentData(sceneData, sceneWgoContentData, gameSceneConfig.sceneGlobalPosition);
	}

	// Token: 0x0600274A RID: 10058 RVA: 0x000B8B64 File Offset: 0x000B6D64
	public bool TryExecutePrebuiltWgoAfterBuildingExpressions(WgoData wgoData, BuildingDef buildingDef)
	{
		if (wgoData == null || buildingDef == null || buildingDef.expressionAfterBuilding.Count == 0)
		{
			return false;
		}
		if (this.prebuiltWgoAfterBuildingExpressionExecuted == null)
		{
			this.prebuiltWgoAfterBuildingExpressionExecuted = new HashSet<SGuid>();
		}
		if (!this.prebuiltWgoAfterBuildingExpressionExecuted.Add(wgoData.UniqueId))
		{
			return false;
		}
		foreach (LazyExpression lazyExpression in buildingDef.expressionAfterBuilding)
		{
			lazyExpression.EvaluateBool(wgoData);
		}
		return true;
	}

	// Token: 0x0600274B RID: 10059 RVA: 0x000B8BF4 File Offset: 0x000B6DF4
	public bool TryExecutePrebuiltWgoAfterBuildingExpressions(WgoData wgoData, WorldZonePrebuiltWgoParams prebuiltWgoParam)
	{
		BuildingDef buildingDef;
		return wgoData != null && prebuiltWgoParam != null && prebuiltWgoParam.execExpressionAfterBuilding && wgoData.Definition != null && wgoData.Definition.TryGetBuildingDefForWgo(out buildingDef) && this.TryExecutePrebuiltWgoAfterBuildingExpressions(wgoData, buildingDef);
	}

	// Token: 0x0600274C RID: 10060 RVA: 0x000B8C34 File Offset: 0x000B6E34
	private void CreateGameSceneDataFromConfig(GameSceneData sceneData, GameSceneConfig gameSceneConfig, List<SceneWgoContentData> sceneWgoContentDatas)
	{
		foreach (SceneWgoContentData sceneWgoContentData in sceneWgoContentDatas)
		{
			this.AddDataToSceneFromContentData(sceneData, sceneWgoContentData, gameSceneConfig.sceneGlobalPosition);
			FightingLevel fightingLevel = sceneWgoContentData.GetComponent<SceneWgoContentPart>() as FightingLevel;
			if (fightingLevel != null)
			{
				Debug.Log("Adding fighting level data: " + fightingLevel.id);
				FightingLevelData fightingLevelData = new FightingLevelData(fightingLevel.id);
				sceneData.fightingLevels.Add(fightingLevelData);
			}
			Debug.Log("Loaded SceneWgoContent: " + sceneWgoContentData.name);
		}
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000B8CDC File Offset: 0x000B6EDC
	private void AddDataToSceneFromContentData(GameSceneData sceneData, SceneWgoContentData sceneWgoContentData, Vector3 offset)
	{
		SceneWgoContentPart component = sceneWgoContentData.GetComponent<SceneWgoContentPart>();
		foreach (WgoData wgoData in component.Wgos)
		{
			if (!string.IsNullOrEmpty(wgoData.id))
			{
				WgoData wgoData2 = wgoData.CreateDataFromMe(offset, sceneData.id, true);
				if (wgoData.startReses != null)
				{
					foreach (StartReses.StartItemData startItemData in wgoData.startReses.startItems)
					{
						Item item = new Item(startItemData.id, startItemData.count);
						wgoData2.Inventory.AddItemToInventory(item, null, false);
					}
					wgoData2.SetGameRes(wgoData.startReses.startGameRes);
					wgoData2.GameResStr.Set(wgoData.startReses.startGameResStr);
				}
				sceneData.AddWgoData(wgoData2, false);
			}
		}
		foreach (WsoData wsoData in component.Wsos)
		{
			if (wsoData != null)
			{
				WsoData wsoData2 = wsoData.CreateDataFromMe(offset, sceneData.id, true);
				sceneData.AddWsoData(wsoData2);
			}
		}
		IReadOnlyList<WorldZoneBakedData> worldZones = component.WorldZones;
		List<WorldZoneBakedData> list = new List<WorldZoneBakedData>(worldZones.Count);
		for (int i = 0; i < worldZones.Count; i++)
		{
			WorldZoneBakedData worldZoneBakedData = worldZones[i];
			if (worldZoneBakedData != null)
			{
				list.Add(worldZoneBakedData);
			}
		}
		list.Sort((WorldZoneBakedData a, WorldZoneBakedData b) => a.processingPriority.CompareTo(b.processingPriority));
		foreach (WorldZoneBakedData worldZoneBakedData2 in list)
		{
			WorldZoneData worldZoneData = WorldZoneData.CreateFromBaked(worldZoneBakedData2, sceneData.id, sceneData.offset);
			if (worldZoneData != null)
			{
				sceneData.AddWorldZoneData(worldZoneData, worldZoneBakedData2);
			}
		}
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x000B8F20 File Offset: 0x000B7120
	private void DeInitDataFromConfig(GameSceneData sceneData, SceneWgoContentData sceneWgoContentData)
	{
		SceneWgoContentPart component = sceneWgoContentData.GetComponent<SceneWgoContentPart>();
		IReadOnlyList<SGuid> wgoUniqueIds = component.WgoUniqueIds;
		Debug.Log(string.Format("Removing {0} WgoData from scene: {1}", wgoUniqueIds.Count, sceneData.id));
		foreach (SGuid sguid in wgoUniqueIds)
		{
			WgoData wgoData = this.GetWgoData(sguid);
			if (wgoData != null)
			{
				sceneData.RemoveWgoData(wgoData, true);
			}
		}
		IReadOnlyList<SGuid> wsoUniqueIds = component.WsoUniqueIds;
		Debug.Log(string.Format("Removing {0} WsoData from scene: {1}", wsoUniqueIds.Count, sceneData.id));
		foreach (SGuid sguid2 in wsoUniqueIds)
		{
			WsoData wsoData = this.GetWsoData(sguid2);
			if (wsoData != null)
			{
				sceneData.RemoveWsoData(wsoData);
			}
		}
		IReadOnlyList<WorldZoneBakedData> worldZones = component.WorldZones;
		Debug.Log(string.Format("Removing {0} WorldZoneData from scene: {1}", worldZones.Count, sceneData.id));
		foreach (WorldZoneBakedData worldZoneBakedData in worldZones)
		{
			if (!(worldZoneBakedData == null))
			{
				WorldZoneData worldZoneDataById = sceneData.GetWorldZoneDataById(worldZoneBakedData.id);
				if (worldZoneDataById != null)
				{
					sceneData.RemoveWorldZoneData(worldZoneDataById);
				}
			}
		}
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x000B90A4 File Offset: 0x000B72A4
	private bool TryGetGameSceneDataById(string id, out GameSceneData gameSceneData)
	{
		gameSceneData = this.gameSceneDataList.Find((GameSceneData x) => x.id == id);
		if (gameSceneData == null)
		{
			Debug.LogError("[WorldData]: incorrect game scene id was provided [" + id + "]");
		}
		return gameSceneData != null;
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x000B90FA File Offset: 0x000B72FA
	public float GetGameRes(string id)
	{
		return this.worldGameRes.Get(id, 0f);
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000B910D File Offset: 0x000B730D
	public int GetGameResInt(string id)
	{
		return this.worldGameRes.GetInt(id);
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000B911B File Offset: 0x000B731B
	public void SetGameRes(string id, int value)
	{
		this.worldGameRes.Set(id, (float)value);
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000B912B File Offset: 0x000B732B
	public void SetGameRes(GameRes gameRes)
	{
		this.worldGameRes.Set(gameRes);
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x000B9139 File Offset: 0x000B7339
	public void SetGameRes(string id, float value)
	{
		this.worldGameRes.Set(id, value);
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x000B9148 File Offset: 0x000B7348
	public bool IsGameResEmpty()
	{
		return this.worldGameRes.IsEmpty();
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000B9155 File Offset: 0x000B7355
	public void AddGameRes(GameRes gameRes)
	{
		this.worldGameRes.Add(gameRes);
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x000B9163 File Offset: 0x000B7363
	public void SubGameRes(string id, int value)
	{
		this.worldGameRes.Sub(id, (float)value);
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x000B9173 File Offset: 0x000B7373
	public void SubGameRes(string id, float value)
	{
		this.worldGameRes.Sub(id, value);
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x000B9182 File Offset: 0x000B7382
	public void AddGameRes(string id, int value)
	{
		this.worldGameRes.Add(id, (float)value);
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x000B9192 File Offset: 0x000B7392
	public void AddGameRes(string id, float value)
	{
		this.worldGameRes.Add(id, value);
	}

	// Token: 0x0600275B RID: 10075 RVA: 0x000B91A1 File Offset: 0x000B73A1
	public void MultiplyGameRes(string id, float value)
	{
		this.worldGameRes.Multiply(id, value);
	}

	// Token: 0x0400218F RID: 8591
	public List<GameSceneData> gameSceneDataList = new List<GameSceneData>();

	// Token: 0x04002190 RID: 8592
	public GdPointsData gdPointsData = new GdPointsData();

	// Token: 0x04002191 RID: 8593
	public List<WispData> wispDataList = new List<WispData>();

	// Token: 0x04002192 RID: 8594
	[SerializeField]
	private GameRes worldGameRes = new GameRes();

	// Token: 0x04002193 RID: 8595
	private HashSet<SGuid> prebuiltWgoAfterBuildingExpressionExecuted;

	// Token: 0x04002194 RID: 8596
	private WgoDataCache cache;
}
