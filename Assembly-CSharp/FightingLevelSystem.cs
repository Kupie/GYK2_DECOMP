using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class FightingLevelSystem
{
	// Token: 0x06001C54 RID: 7252 RVA: 0x0008473F File Offset: 0x0008293F
	public FightingLevel GetFightingLevel(string id)
	{
		if (string.IsNullOrEmpty(id))
		{
			return null;
		}
		return this.fightingLevelsById.GetValueOrDefault(id);
	}

	// Token: 0x06001C55 RID: 7253 RVA: 0x00084758 File Offset: 0x00082958
	public WorldZone GetWorldZoneById(string zoneId)
	{
		if (string.IsNullOrEmpty(zoneId))
		{
			return null;
		}
		for (int i = 0; i < this.spawnedWorldZones.Count; i++)
		{
			WorldZone worldZone = this.spawnedWorldZones[i];
			if (worldZone != null && worldZone.Id == zoneId)
			{
				return worldZone;
			}
		}
		PlayerController playerController = MainGame.PlayerController;
		string text;
		if (playerController == null)
		{
			text = null;
		}
		else
		{
			GameScene currentGameScene = playerController.CurrentGameScene;
			text = ((currentGameScene != null) ? currentGameScene.Id : null);
		}
		string text2 = text;
		if (!string.IsNullOrEmpty(text2))
		{
			WorldZone worldZoneById = MainGame.PlayerController.CurrentGameScene.GetWorldZoneById(zoneId);
			if (worldZoneById != null)
			{
				return worldZoneById;
			}
		}
		GameSceneManager instance = LazySingleton<GameSceneManager>.Instance;
		if (instance == null)
		{
			return null;
		}
		foreach (GameScene gameScene in instance.LoadedGameScenes)
		{
			if (!(gameScene == null) && !(gameScene.Id == text2))
			{
				WorldZone worldZoneById2 = gameScene.GetWorldZoneById(zoneId);
				if (worldZoneById2 != null)
				{
					return worldZoneById2;
				}
			}
		}
		return null;
	}

	// Token: 0x06001C56 RID: 7254 RVA: 0x0008487C File Offset: 0x00082A7C
	public void PrepareForGame()
	{
		this.UnprepareFromGame();
		FightingGameController.RepairChainedFightsLeftAtWinAfterFollowUpReset(null);
		FightingLevelData.OnFightingLevelDataChanged += this.HandleFightingLevelDataChanged;
		this.isSubscribedToStageChanges = true;
		foreach (GameSceneData gameSceneData in MainGame.WorldData.gameSceneDataList)
		{
			this.SubscribeSceneData(gameSceneData);
			this.SpawnFightingLevelsFromData(gameSceneData);
		}
	}

	// Token: 0x06001C57 RID: 7255 RVA: 0x00084900 File Offset: 0x00082B00
	public void UnprepareFromGame()
	{
		if (this.isSubscribedToStageChanges)
		{
			FightingLevelData.OnFightingLevelDataChanged -= this.HandleFightingLevelDataChanged;
			this.isSubscribedToStageChanges = false;
		}
		for (int i = this.sceneBindings.Count - 1; i >= 0; i--)
		{
			FightingLevelSystem.UnsubscribeSceneData(this.sceneBindings[i]);
		}
		this.sceneBindings.Clear();
		this.fightingLevelsById.Clear();
		this.spawnedWorldZones.Clear();
	}

	// Token: 0x06001C58 RID: 7256 RVA: 0x00084977 File Offset: 0x00082B77
	public bool TryGetExistingWorldZone(string zoneId, out WorldZone worldZone)
	{
		worldZone = this.GetWorldZoneById(zoneId);
		return worldZone != null;
	}

	// Token: 0x06001C59 RID: 7257 RVA: 0x0008498C File Offset: 0x00082B8C
	public bool TryEnsureFightingLevelLoaded(string levelId)
	{
		if (string.IsNullOrEmpty(levelId))
		{
			return false;
		}
		if (this.GetFightingLevel(levelId) != null)
		{
			return true;
		}
		GameSceneData gameSceneData;
		if (!FightingLevelSystem.TryResolveOwnerSceneForLevel(levelId, out gameSceneData))
		{
			Debug.LogError("Can't find game scene data for fighting level: " + levelId);
			return false;
		}
		FightingLevelData fightingLevelData = gameSceneData.fightingLevels.Find((FightingLevelData level) => level.id == levelId);
		if (fightingLevelData != null)
		{
			this.LoadAndSpawnFightingLevel(gameSceneData, fightingLevelData, false);
		}
		else
		{
			gameSceneData.AddFightingLevelData(levelId);
		}
		return this.GetFightingLevel(levelId) != null;
	}

	// Token: 0x06001C5A RID: 7258 RVA: 0x00084A38 File Offset: 0x00082C38
	public bool TryRemoveFightingLevelData(string levelId)
	{
		if (string.IsNullOrEmpty(levelId))
		{
			return false;
		}
		GameSceneData gameSceneData;
		if (!FightingLevelSystem.TryResolveOwnerSceneForLevel(levelId, out gameSceneData))
		{
			Debug.LogError("Can't find game scene data for fighting level: " + levelId);
			return false;
		}
		gameSceneData.RemoveFightingLevelData(levelId);
		return true;
	}

	// Token: 0x06001C5B RID: 7259 RVA: 0x00084A74 File Offset: 0x00082C74
	private static bool TryResolveOwnerSceneForLevel(string levelId, out GameSceneData sceneData)
	{
		GameSceneConfig gameSceneConfig;
		if (MainGame.WorldData.TryGetGameSceneDataForContent(levelId, out sceneData, out gameSceneConfig))
		{
			return true;
		}
		sceneData = null;
		return false;
	}

	// Token: 0x06001C5C RID: 7260 RVA: 0x00084A98 File Offset: 0x00082C98
	private void SubscribeSceneData(GameSceneData sceneData)
	{
		if (sceneData == null)
		{
			return;
		}
		using (List<FightingLevelSystem.SceneFightingLevelBindings>.Enumerator enumerator = this.sceneBindings.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.SceneData == sceneData)
				{
					return;
				}
			}
		}
		FightingLevelSystem.SceneFightingLevelBindings sceneFightingLevelBindings = new FightingLevelSystem.SceneFightingLevelBindings(sceneData);
		sceneFightingLevelBindings.OnAdded = delegate(FightingLevelData data)
		{
			this.LoadAndSpawnFightingLevel(sceneData, data, true);
		};
		sceneFightingLevelBindings.OnRemoved = delegate(FightingLevelData data)
		{
			this.UnloadAndDespawnFightingLevel(sceneData, data);
		};
		sceneData.OnFightingLevelDataAdded += sceneFightingLevelBindings.OnAdded;
		sceneData.OnFightingLevelDataRemoved += sceneFightingLevelBindings.OnRemoved;
		this.sceneBindings.Add(sceneFightingLevelBindings);
	}

	// Token: 0x06001C5D RID: 7261 RVA: 0x00084B70 File Offset: 0x00082D70
	private static void UnsubscribeSceneData(FightingLevelSystem.SceneFightingLevelBindings bindings)
	{
		if (((bindings != null) ? bindings.SceneData : null) == null)
		{
			return;
		}
		bindings.SceneData.OnFightingLevelDataAdded -= bindings.OnAdded;
		bindings.SceneData.OnFightingLevelDataRemoved -= bindings.OnRemoved;
	}

	// Token: 0x06001C5E RID: 7262 RVA: 0x00084BA4 File Offset: 0x00082DA4
	private void SpawnFightingLevelsFromData(GameSceneData sceneData)
	{
		GameSceneConfig gameSceneConfig = FightingLevelSystem.ResolveGameSceneConfig(sceneData.id);
		if (gameSceneConfig == null)
		{
			Debug.LogError("Can't find game scene config with id: " + sceneData.id);
			return;
		}
		foreach (FightingLevelData fightingLevelData in sceneData.fightingLevels)
		{
			this.LoadAndSpawnFightingLevel(sceneData, gameSceneConfig, fightingLevelData, false);
		}
	}

	// Token: 0x06001C5F RID: 7263 RVA: 0x00084C28 File Offset: 0x00082E28
	private void LoadAndSpawnFightingLevel(GameSceneData sceneData, FightingLevelData fightingLevelData, bool addToGameSceneData)
	{
		GameSceneData gameSceneData;
		GameSceneConfig gameSceneConfig;
		GameSceneConfig gameSceneConfig2;
		if (MainGame.WorldData.TryGetGameSceneDataForContent(fightingLevelData.id, out gameSceneData, out gameSceneConfig))
		{
			sceneData = gameSceneData;
			gameSceneConfig2 = gameSceneConfig;
		}
		else
		{
			gameSceneConfig2 = FightingLevelSystem.ResolveGameSceneConfig(sceneData.id);
		}
		if (gameSceneConfig2 == null)
		{
			Debug.LogError("Can't find game scene config for fighting level: " + fightingLevelData.id);
			return;
		}
		this.LoadAndSpawnFightingLevel(sceneData, gameSceneConfig2, fightingLevelData, addToGameSceneData);
	}

	// Token: 0x06001C60 RID: 7264 RVA: 0x00084C88 File Offset: 0x00082E88
	private void LoadAndSpawnFightingLevel(GameSceneData sceneData, GameSceneConfig config, FightingLevelData fightingLevelData, bool addToGameSceneData)
	{
		SceneWgoContentData sceneWgoContentData;
		if (!config.TryLoadSceneDataContentByName(fightingLevelData.id, out sceneWgoContentData))
		{
			Debug.LogError("Can't load fighting level prefab: " + fightingLevelData.id + ", sceneWgoContentData is null");
			return;
		}
		if (addToGameSceneData)
		{
			MainGame.WorldData.AddContentDataFromConfig(sceneData, config, sceneWgoContentData);
		}
		FightingLevel component = sceneWgoContentData.GetComponent<FightingLevel>();
		if (component == null)
		{
			Debug.LogError("Can't load fighting level prefab: " + fightingLevelData.id + ", fighting level is null");
			return;
		}
		this.SpawnMissingWorldZonesForLoadedContent(sceneData, config, sceneWgoContentData);
		component.ApplyStageIdFromData(fightingLevelData.CurStageId);
		if (!this.fightingLevelsById.ContainsKey(component.id))
		{
			this.fightingLevelsById.Add(component.id, component);
		}
	}

	// Token: 0x06001C61 RID: 7265 RVA: 0x00084D38 File Offset: 0x00082F38
	private void UnloadAndDespawnFightingLevel(GameSceneData sceneData, FightingLevelData fightingLevelData)
	{
		FightingLevel fightingLevel;
		if (this.fightingLevelsById.TryGetValue(fightingLevelData.id, out fightingLevel))
		{
			this.DespawnWorldZonesForUnloadedContent(fightingLevel);
			this.fightingLevelsById.Remove(fightingLevelData.id);
		}
		else
		{
			Debug.LogWarning("Can't unload fighting level view, it wasn't found: " + fightingLevelData.id);
		}
		GameSceneData gameSceneData;
		GameSceneConfig gameSceneConfig;
		GameSceneConfig gameSceneConfig2;
		if (MainGame.WorldData.TryGetGameSceneDataForContent(fightingLevelData.id, out gameSceneData, out gameSceneConfig))
		{
			sceneData = gameSceneData;
			gameSceneConfig2 = gameSceneConfig;
		}
		else
		{
			gameSceneConfig2 = FightingLevelSystem.ResolveGameSceneConfig(sceneData.id);
		}
		if (gameSceneConfig2 == null)
		{
			Debug.LogError("Can't find game scene config for fighting level: " + fightingLevelData.id);
			return;
		}
		MainGame.WorldData.UnloadContentData(gameSceneConfig2, sceneData, fightingLevelData.id);
	}

	// Token: 0x06001C62 RID: 7266 RVA: 0x00084DE4 File Offset: 0x00082FE4
	private void HandleFightingLevelDataChanged(FightingLevelData fightingLevelData)
	{
		FightingLevel fightingLevel = this.GetFightingLevel(fightingLevelData.id);
		if (fightingLevel == null)
		{
			return;
		}
		fightingLevel.ApplyStageIdFromData(fightingLevelData.CurStageId);
	}

	// Token: 0x06001C63 RID: 7267 RVA: 0x00084E14 File Offset: 0x00083014
	private void SpawnMissingWorldZonesForLoadedContent(GameSceneData sceneData, GameSceneConfig config, SceneWgoContentData sceneWgoContentData)
	{
		SceneWgoContentPart component = sceneWgoContentData.GetComponent<SceneWgoContentPart>();
		if (component == null)
		{
			return;
		}
		IReadOnlyList<WorldZoneBakedData> worldZones = component.WorldZones;
		for (int i = 0; i < worldZones.Count; i++)
		{
			WorldZoneBakedData worldZoneBakedData = worldZones[i];
			WorldZone worldZone;
			if (!(worldZoneBakedData == null) && !this.TryGetExistingWorldZone(worldZoneBakedData.id, out worldZone))
			{
				WorldZoneData worldZoneDataById = sceneData.GetWorldZoneDataById(worldZoneBakedData.id);
				if (worldZoneDataById != null)
				{
					WorldZone worldZone2 = FightingLevelSystem.SpawnWorldZoneFromData(config, worldZoneDataById, component);
					if (!(worldZone2 == null))
					{
						this.spawnedWorldZones.Add(worldZone2);
						worldZone2.AddWgosOnGameSceneStart();
					}
				}
			}
		}
	}

	// Token: 0x06001C64 RID: 7268 RVA: 0x00084EA8 File Offset: 0x000830A8
	private void DespawnWorldZonesForUnloadedContent(SceneWgoContentPart contentPart)
	{
		if (contentPart == null)
		{
			return;
		}
		IReadOnlyList<WorldZoneBakedData> worldZones = contentPart.WorldZones;
		for (int i = this.spawnedWorldZones.Count - 1; i >= 0; i--)
		{
			WorldZone worldZone = this.spawnedWorldZones[i];
			if (worldZone == null)
			{
				this.spawnedWorldZones.RemoveAt(i);
			}
			else if (FightingLevelSystem.ContainsBakedZoneId(worldZones, worldZone.Id))
			{
				this.spawnedWorldZones.RemoveAt(i);
				global::UnityEngine.Object.Destroy(worldZone.gameObject);
			}
		}
	}

	// Token: 0x06001C65 RID: 7269 RVA: 0x00084F28 File Offset: 0x00083128
	private static WorldZone SpawnWorldZoneFromData(GameSceneConfig config, WorldZoneData worldZoneData, SceneWgoContentPart preferredContentPart)
	{
		Transform transform = FightingLevelSystem.ResolveWorldZoneParent(config, worldZoneData, preferredContentPart);
		return WorldZone.Spawn(worldZoneData, transform);
	}

	// Token: 0x06001C66 RID: 7270 RVA: 0x00084F48 File Offset: 0x00083148
	private static Transform ResolveWorldZoneParent(GameSceneConfig config, WorldZoneData worldZoneData, SceneWgoContentPart preferredContentPart)
	{
		if (preferredContentPart != null)
		{
			return preferredContentPart.transform;
		}
		GameObject gameObject;
		SceneWgoContentPart sceneWgoContentPart;
		if (config.TryGetLoadedContent(worldZoneData.contentPartName, out gameObject) && gameObject != null && gameObject.TryGetComponent<SceneWgoContentPart>(out sceneWgoContentPart))
		{
			return sceneWgoContentPart.transform;
		}
		GameSceneManager instance = LazySingleton<GameSceneManager>.Instance;
		if (instance != null)
		{
			GameScene gameScene = instance.LoadedGameScenes.Find((GameScene scene) => scene != null && scene.Id == worldZoneData.gameSceneId);
			if (gameScene != null)
			{
				return gameScene.transform;
			}
		}
		return null;
	}

	// Token: 0x06001C67 RID: 7271 RVA: 0x00084FDC File Offset: 0x000831DC
	private static bool ContainsBakedZoneId(IReadOnlyList<WorldZoneBakedData> bakedZones, string zoneId)
	{
		for (int i = 0; i < bakedZones.Count; i++)
		{
			WorldZoneBakedData worldZoneBakedData = bakedZones[i];
			if (worldZoneBakedData != null && worldZoneBakedData.id == zoneId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001C68 RID: 7272 RVA: 0x0008501C File Offset: 0x0008321C
	private static GameSceneConfig ResolveGameSceneConfig(string sceneId)
	{
		return MainGame.Instance.gameSceneConfigs.Find((GameSceneConfig config) => config.name == sceneId);
	}

	// Token: 0x04001AAC RID: 6828
	private readonly Dictionary<string, FightingLevel> fightingLevelsById = new Dictionary<string, FightingLevel>();

	// Token: 0x04001AAD RID: 6829
	private readonly List<WorldZone> spawnedWorldZones = new List<WorldZone>();

	// Token: 0x04001AAE RID: 6830
	private readonly List<FightingLevelSystem.SceneFightingLevelBindings> sceneBindings = new List<FightingLevelSystem.SceneFightingLevelBindings>();

	// Token: 0x04001AAF RID: 6831
	private bool isSubscribedToStageChanges;

	// Token: 0x0200042E RID: 1070
	private sealed class SceneFightingLevelBindings
	{
		// Token: 0x06001C6A RID: 7274 RVA: 0x0008507A File Offset: 0x0008327A
		public SceneFightingLevelBindings(GameSceneData sceneData)
		{
			this.SceneData = sceneData;
		}

		// Token: 0x04001AB0 RID: 6832
		public readonly GameSceneData SceneData;

		// Token: 0x04001AB1 RID: 6833
		public Action<FightingLevelData> OnAdded;

		// Token: 0x04001AB2 RID: 6834
		public Action<FightingLevelData> OnRemoved;
	}
}
