using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using LazyBearTechnology;
using LinqTools;
using Pathfinding;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x02000624 RID: 1572
public class GameScene : SerializedMonoBehaviour
{
	// Token: 0x170006B8 RID: 1720
	// (get) Token: 0x060029D8 RID: 10712 RVA: 0x000C548E File Offset: 0x000C368E
	public bool DisableUnloadOnTeleport
	{
		get
		{
			GameSceneConfig gameSceneConfig = this.gameSceneConfig;
			return gameSceneConfig != null && gameSceneConfig.disableUnloadOnTeleport;
		}
	}

	// Token: 0x060029D9 RID: 10713 RVA: 0x000C54A1 File Offset: 0x000C36A1
	public static void ClearGlobalCaches()
	{
		GameScene.globalWgoViewCache = new Dictionary<SGuid, Wgo>();
		GameScene.globalWsoViewCache = new Dictionary<SGuid, Wso>();
	}

	// Token: 0x170006B9 RID: 1721
	// (get) Token: 0x060029DA RID: 10714 RVA: 0x000C54B7 File Offset: 0x000C36B7
	public bool IsStartCompleted
	{
		get
		{
			return this.isStartCompleted;
		}
	}

	// Token: 0x170006BA RID: 1722
	// (get) Token: 0x060029DB RID: 10715 RVA: 0x000C54BF File Offset: 0x000C36BF
	public bool IsInitialized
	{
		get
		{
			return this.gameSceneData != null;
		}
	}

	// Token: 0x170006BB RID: 1723
	// (get) Token: 0x060029DC RID: 10716 RVA: 0x000C54CA File Offset: 0x000C36CA
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170006BC RID: 1724
	// (get) Token: 0x060029DD RID: 10717 RVA: 0x000C54D2 File Offset: 0x000C36D2
	public GameSceneConfig GameSceneConfig
	{
		get
		{
			return this.gameSceneConfig;
		}
	}

	// Token: 0x170006BD RID: 1725
	// (get) Token: 0x060029DE RID: 10718 RVA: 0x000C54DA File Offset: 0x000C36DA
	public SceneWaypointContent SceneWaypointContent
	{
		get
		{
			return this.waypointContent;
		}
	}

	// Token: 0x170006BE RID: 1726
	// (get) Token: 0x060029DF RID: 10719 RVA: 0x000C54E2 File Offset: 0x000C36E2
	public GameSceneData GameSceneData
	{
		get
		{
			return this.gameSceneData;
		}
	}

	// Token: 0x170006BF RID: 1727
	// (get) Token: 0x060029E0 RID: 10720 RVA: 0x000C54EA File Offset: 0x000C36EA
	public GDPoint[] SceneGdPoints
	{
		get
		{
			return this.sceneGdPoints;
		}
	}

	// Token: 0x170006C0 RID: 1728
	// (get) Token: 0x060029E1 RID: 10721 RVA: 0x000C54F2 File Offset: 0x000C36F2
	public List<Wgo> Wgos
	{
		get
		{
			return this.wgoViewCache.Values.ToList<Wgo>();
		}
	}

	// Token: 0x060029E2 RID: 10722 RVA: 0x000C5504 File Offset: 0x000C3704
	public Wgo AddWgoData(WgoData data, bool recheckVisibilityOnSpawn = false)
	{
		MainGame.Instance.GameSave.worldData.AddWgoData(data, recheckVisibilityOnSpawn);
		return this.wgoViewCache.GetValueOrDefault(data.UniqueId);
	}

	// Token: 0x060029E3 RID: 10723 RVA: 0x000C5530 File Offset: 0x000C3730
	public Wgo AddWgoData(string id, Vector3 position)
	{
		WgoData wgoData = new WgoData(id, position, this.id);
		return this.AddWgoData(wgoData, false);
	}

	// Token: 0x060029E4 RID: 10724 RVA: 0x000C5553 File Offset: 0x000C3753
	public static void ClearGlobalWgoViewCache()
	{
		GameScene.globalWgoViewCache = new Dictionary<SGuid, Wgo>();
	}

	// Token: 0x060029E5 RID: 10725 RVA: 0x000C555F File Offset: 0x000C375F
	public static Wgo GetWgoViewGlobal(SGuid uniqueId)
	{
		return GameScene.globalWgoViewCache.GetValueOrDefault(uniqueId);
	}

	// Token: 0x060029E6 RID: 10726 RVA: 0x000C556C File Offset: 0x000C376C
	public static void RedrawWgosWithInteractionEvents()
	{
		MainGame instance = MainGame.Instance;
		QuestSystemData questSystemData;
		if (instance == null)
		{
			questSystemData = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			questSystemData = ((gameSave != null) ? gameSave.questSystemData : null);
		}
		QuestSystemData questSystemData2 = questSystemData;
		foreach (Wgo wgo in GameScene.globalWgoViewCache.Values)
		{
			if (!(wgo == null) && wgo.Data != null)
			{
				bool flag = wgo.Data.Events.Count > 0;
				bool flag2 = questSystemData2 != null && questSystemData2.WgoHasReadyToFinishQuest(wgo.Id);
				if (flag || flag2)
				{
					wgo.DrawWidgets();
				}
			}
		}
	}

	// Token: 0x060029E7 RID: 10727 RVA: 0x000C561C File Offset: 0x000C381C
	public DropView GetDropView(Item item)
	{
		DropView dropView;
		if (this.TryGetDropView(item, out dropView))
		{
			return dropView;
		}
		Debug.LogWarning("No drop view found for item: [" + item.id + "]");
		return null;
	}

	// Token: 0x060029E8 RID: 10728 RVA: 0x000C5654 File Offset: 0x000C3854
	public bool TryGetDropView(Item item, out DropView dropView)
	{
		dropView = null;
		if (item == null)
		{
			return false;
		}
		if (!this.dropViewCache.TryGetValue(item.UniqueId, out dropView))
		{
			return false;
		}
		if (dropView == null || dropView.Data == null || dropView.IsDespawning)
		{
			dropView = null;
			return false;
		}
		return true;
	}

	// Token: 0x060029E9 RID: 10729 RVA: 0x000C56A1 File Offset: 0x000C38A1
	public FightingLevel GetFightingLevel(string id)
	{
		return MainGame.GetFightingLevel(id);
	}

	// Token: 0x060029EA RID: 10730 RVA: 0x000C56AC File Offset: 0x000C38AC
	public WorldZone GetWorldZoneById(string zoneId)
	{
		if (string.IsNullOrEmpty(zoneId) || this.worldZones == null)
		{
			return null;
		}
		return this.worldZones.Find((WorldZone zone) => zone != null && zone.Id == zoneId);
	}

	// Token: 0x060029EB RID: 10731 RVA: 0x000C56F4 File Offset: 0x000C38F4
	private async void Start()
	{
		Debug.Log("#time_test# [GameScene] Start entered for id=" + this.id);
		try
		{
			if (this.gameSceneData == null)
			{
				this.gameSceneData = MainGame.Instance.GameSave.worldData.GetGameSceneDataById(this.id);
				MainGame.PlayerData.currentGameSceneId = this.id;
				MainGame.PlayerController.CurrentGameScene = this;
				if (this.gameSceneData == null)
				{
					Debug.LogError("Not found game scene with id: " + this.id);
				}
				else
				{
					this.sceneGdPoints = this.GetGDPointsExcludingEditorContent();
					MainGame.Instance.GameSave.worldData.gdPointsData.InitScenePointsFromGameScene(this, this.sceneGdPoints);
					List<WorldZone> list = await this.SpawnWorldZonesFromData();
					this.worldZones = list;
					await this.SpawnWgoViewsFromData();
					await this.SpawnWsoViewsFromData();
					await this.SpawnDropsFromData();
					await this.SpawnTechPointsFromData();
					this.gameSceneData.OnWgoDataAdd += this.HandleWgoAdd;
					this.gameSceneData.OnWgoDataRemove += this.HandleWgoRemove;
					this.gameSceneData.OnWsoDataAdd += this.HandleWsoAdd;
					this.gameSceneData.OnWsoDataRemove += this.HandleWsoRemoved;
					this.gameSceneData.OnDropAdd += this.HandleDropAdd;
					this.gameSceneData.OnDropRemove += this.HandleDropRemove;
					await this.ScanChurchGraph();
					this.AddWgosToWorldZones();
					this.gameSceneData.ProcessQueuedDrops();
					this.chunks = new List<IChunkableObject>();
					for (int i = 0; i < this.chunkComponents.Length; i++)
					{
						if (!(this.chunkComponents[i] == null))
						{
							this.chunkComponents[i].UpdateChunkVisibility(false);
							this.chunks.Add(this.chunkComponents[i]);
						}
					}
					for (int j = this.chunks.Count - 1; j >= 0; j--)
					{
						if (this.chunks[j] == null)
						{
							this.chunks.RemoveAt(j);
						}
					}
					for (int k = 0; k < this.constructorParts.Count; k++)
					{
						if (!(this.constructorParts[k] == null))
						{
							this.chunks.Add(this.constructorParts[k]);
						}
					}
					for (int l = 0; l < this.bakedChunkableObjectComponentDatas.Count; l++)
					{
						if (this.bakedChunkableObjectComponentDatas[l] != null)
						{
							this.chunks.Add(this.bakedChunkableObjectComponentDatas[l]);
						}
					}
					LazySingleton<ChunkManager>.Instance.RegisterChunks<IChunkableObject>(this.chunks, ChunkManagerLayerType.StaticObjects);
					LazySingleton<ScenePoolPathRegistry>.Instance.RegisterScenePaths(this.id, this.CollectScenePoolPaths());
					if (!string.IsNullOrEmpty(MainGame.ConveyorPresetLoadOnNewGame) && !MainGame.Instance.GameSave.conveyorSystemData.isInitialized)
					{
						ConveyorPreset.LoadPreset(MainGame.ConveyorPresetLoadOnNewGame);
						MainGame.Instance.GameSave.conveyorSystemData.isInitialized = true;
					}
					if (this.gameSceneLinksCollection)
					{
						this.gameSceneLinksCollection.RefreshLinks();
					}
					RiverDropSystem riverDropSystem = MainGame.Instance.riverDropSystem;
					if (riverDropSystem != null)
					{
						riverDropSystem.HandleSceneReady(this);
					}
				}
			}
		}
		finally
		{
			this.isStartCompleted = true;
		}
	}

	// Token: 0x060029EC RID: 10732 RVA: 0x000C572C File Offset: 0x000C392C
	public async Awaitable<bool> WaitForStartCompleted(float timeoutSeconds = 30f)
	{
		float timeoutAt = Time.realtimeSinceStartup + timeoutSeconds;
		while (!this.isStartCompleted)
		{
			bool flag;
			if (this == null)
			{
				flag = false;
			}
			else
			{
				if (Time.realtimeSinceStartup < timeoutAt)
				{
					await Awaitable.NextFrameAsync(default(CancellationToken));
					continue;
				}
				Debug.LogWarning("[GameScene] WaitForStartCompleted timeout for id=" + this.id);
				flag = false;
			}
			return flag;
		}
		return 1;
	}

	// Token: 0x060029ED RID: 10733 RVA: 0x000C5778 File Offset: 0x000C3978
	private HashSet<string> CollectScenePoolPaths()
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (this.bakedChunkableObjectComponentDatas != null)
		{
			for (int i = 0; i < this.bakedChunkableObjectComponentDatas.Count; i++)
			{
				BakedChunkableObjectComponentData bakedChunkableObjectComponentData = this.bakedChunkableObjectComponentDatas[i];
				if (bakedChunkableObjectComponentData != null && !string.IsNullOrEmpty(bakedChunkableObjectComponentData.pathToObject))
				{
					hashSet.Add(bakedChunkableObjectComponentData.pathToObject);
				}
			}
		}
		if (this.constructorParts != null)
		{
			for (int j = 0; j < this.constructorParts.Count; j++)
			{
				ConstructorPart constructorPart = this.constructorParts[j];
				if (constructorPart != null && constructorPart.HasChildPath)
				{
					hashSet.Add(constructorPart.constructorPartChildData.pathToObject);
				}
			}
		}
		if (this.gameSceneData != null)
		{
			for (int k = 0; k < this.gameSceneData.wgoDataList.Count; k++)
			{
				WgoPartPool.CollectAddressableKeysForWgoData(this.gameSceneData.wgoDataList[k], hashSet);
			}
		}
		return hashSet;
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x000C5868 File Offset: 0x000C3A68
	public void RestoreStaticObjectsAfterTeleport()
	{
		if (!this.IsInitialized || this.chunks == null || this.chunks.Count == 0)
		{
			return;
		}
		ChunkManager instance = LazySingleton<ChunkManager>.Instance;
		if (instance == null)
		{
			return;
		}
		instance.RegisterChunks<IChunkableObject>(this.chunks, ChunkManagerLayerType.StaticObjects);
		foreach (IChunkableObject chunkableObject in this.chunks)
		{
			if (chunkableObject != null)
			{
				global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
				if (@object == null || !(@object == null))
				{
					instance.RequestVisibilityRecheck(chunkableObject);
				}
			}
		}
		LazyTerrain componentInChildren = base.GetComponentInChildren<LazyTerrain>(true);
		if (componentInChildren != null && componentInChildren.meshesData.Count > 0)
		{
			instance.RegisterChunks<LazyTerrainMeshData>(componentInChildren.meshesData, ChunkManagerLayerType.StaticObjects);
			foreach (LazyTerrainMeshData lazyTerrainMeshData in componentInChildren.meshesData)
			{
				instance.RequestVisibilityRecheck(lazyTerrainMeshData);
			}
		}
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x000C5980 File Offset: 0x000C3B80
	private void OnDestroy()
	{
		Debug.Log("[GameScene] OnDestroy for id=" + this.id);
		if (!Application.isPlaying)
		{
			return;
		}
		if (this.gameSceneData == null)
		{
			return;
		}
		this.gameSceneData.OnWgoDataAdd -= this.HandleWgoAdd;
		this.gameSceneData.OnWgoDataRemove -= this.HandleWgoRemove;
		this.gameSceneData.OnWsoDataAdd -= this.HandleWsoAdd;
		this.gameSceneData.OnWsoDataRemove -= this.HandleWsoRemoved;
		this.gameSceneData.OnDropAdd -= this.HandleDropAdd;
		this.gameSceneData.OnDropRemove -= this.HandleDropRemove;
		foreach (DropView dropView in this.dropViewCache.Values)
		{
			if (dropView != null)
			{
				dropView.DespawnView();
			}
		}
		this.dropViewCache.Clear();
		LazySingleton<ChunkManager>.Instance.UnregisterChunks<IChunkableObject>(this.chunks, ChunkManagerLayerType.StaticObjects);
		foreach (ConstructorPart constructorPart in this.constructorParts)
		{
			constructorPart.UpdateChunkVisibility(false);
		}
		foreach (BakedChunkableObjectComponentData bakedChunkableObjectComponentData in this.bakedChunkableObjectComponentDatas)
		{
			bakedChunkableObjectComponentData.UpdateChunkVisibility(false);
		}
		List<IChunkableObject> list = new List<IChunkableObject>();
		List<IChunkableObject> list2 = new List<IChunkableObject>();
		foreach (SGuid sguid in new List<SGuid>(this.wgoViewCache.Keys))
		{
			Wgo valueOrDefault = this.wgoViewCache.GetValueOrDefault(sguid);
			if (valueOrDefault != null)
			{
				valueOrDefault.UpdateChunkVisibilityState(ChunkVisibilityState.OutOfRange);
				valueOrDefault.UpdateChunkVisibility(false);
				if (valueOrDefault.RegisteredInChunker)
				{
					WgoData data = valueOrDefault.Data;
					bool? flag;
					if (data == null)
					{
						flag = null;
					}
					else
					{
						WGODef definition = data.Definition;
						flag = ((definition != null) ? new bool?(definition.isMovable) : null);
					}
					bool? flag2 = flag;
					if (flag2.GetValueOrDefault())
					{
						list2.Add(valueOrDefault);
					}
					else
					{
						list.Add(valueOrDefault);
					}
					valueOrDefault.RegisteredInChunker = false;
				}
			}
			GameScene.globalWgoViewCache.Remove(sguid);
		}
		LazySingleton<ChunkManager>.Instance.UnregisterChunks<IChunkableObject>(list, ChunkManagerLayerType.StaticWgo);
		LazySingleton<ChunkManager>.Instance.UnregisterChunks<IChunkableObject>(list2, ChunkManagerLayerType.DynamicWgo);
		this.wgoViewCache.Clear();
		List<IChunkableObject> list3 = new List<IChunkableObject>();
		foreach (SGuid sguid2 in new List<SGuid>(this.wsoViewCache.Keys))
		{
			Wso valueOrDefault2 = this.wsoViewCache.GetValueOrDefault(sguid2);
			if (valueOrDefault2 != null && valueOrDefault2.RegisteredInChunker)
			{
				list3.Add(valueOrDefault2);
				valueOrDefault2.RegisteredInChunker = false;
			}
			GameScene.globalWsoViewCache.Remove(sguid2);
		}
		LazySingleton<ChunkManager>.Instance.UnregisterChunks<IChunkableObject>(list3, ChunkManagerLayerType.StaticWso);
		this.wsoViewCache.Clear();
		this.ReleaseFightingBakingContexts();
		LazySingleton<ScenePoolPathRegistry>.Instance.UnregisterScenePaths(this.id);
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x000C5D00 File Offset: 0x000C3F00
	private void ReleaseFightingBakingContexts()
	{
		FightingLevel[] componentsInChildren = base.GetComponentsInChildren<FightingLevel>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != null)
			{
				componentsInChildren[i].ForceUnregisterBakingContext();
			}
		}
		FightingStage[] componentsInChildren2 = base.GetComponentsInChildren<FightingStage>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			if (componentsInChildren2[j] != null)
			{
				componentsInChildren2[j].ForceUnregisterBakingContext();
			}
		}
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x000C5D60 File Offset: 0x000C3F60
	private async Awaitable<List<WorldZone>> SpawnWorldZonesFromData()
	{
		List<WorldZone> spawnedZones = new List<WorldZone>();
		foreach (WorldZoneData worldZoneData in this.gameSceneData.worldZones)
		{
			WorldZone worldZone = this.TrySpawnWorldZoneFromData(worldZoneData, null);
			if (worldZone)
			{
				spawnedZones.Add(worldZone);
			}
		}
		await Awaitable.NextFrameAsync(default(CancellationToken));
		return spawnedZones;
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x000C5DA4 File Offset: 0x000C3FA4
	private async Awaitable SpawnWgoViewsFromData()
	{
		List<IChunkableObject> staticViews = new List<IChunkableObject>();
		List<IChunkableObject> dynamicViews = new List<IChunkableObject>();
		int spawnCount = 0;
		List<WgoData> list = new List<WgoData>(this.gameSceneData.wgoDataList);
		Debug.Log(string.Format("SpawnWgoViewsFromData: {0}", list.Count));
		foreach (WgoData wgoData in list)
		{
			if (spawnCount >= 50)
			{
				spawnCount = 0;
				await Awaitable.NextFrameAsync(default(CancellationToken));
			}
			Wgo wgo = this.CreateWgoView(wgoData, true, false);
			WGODef definition = wgoData.Definition;
			if (definition != null && definition.isMovable)
			{
				dynamicViews.Add(wgo);
			}
			else
			{
				staticViews.Add(wgo);
			}
			wgo.RegisteredInChunker = true;
			spawnCount++;
			wgoData = null;
		}
		List<WgoData>.Enumerator enumerator = default(List<WgoData>.Enumerator);
		LazySingleton<ChunkManager>.Instance.RegisterChunks<IChunkableObject>(staticViews, ChunkManagerLayerType.StaticWgo);
		LazySingleton<ChunkManager>.Instance.RegisterChunks<IChunkableObject>(dynamicViews, ChunkManagerLayerType.DynamicWgo);
	}

	// Token: 0x060029F3 RID: 10739 RVA: 0x000C5DE8 File Offset: 0x000C3FE8
	private async Awaitable SpawnWsoViewsFromData()
	{
		if (this.gameSceneData.wsoDataList != null && this.gameSceneData.wsoDataList.Count != 0)
		{
			List<IChunkableObject> createdWsos = new List<IChunkableObject>();
			int spawnCount = 0;
			List<WsoData> list = new List<WsoData>(this.gameSceneData.wsoDataList);
			foreach (WsoData wsoData in list)
			{
				if (spawnCount >= 50)
				{
					spawnCount = 0;
					await Awaitable.NextFrameAsync(default(CancellationToken));
				}
				Wso wso = this.CreateWsoView(wsoData);
				if (wso != null && !wso.RegisteredInChunker)
				{
					wso.RegisteredInChunker = true;
					createdWsos.Add(wso);
				}
				wsoData = null;
			}
			List<WsoData>.Enumerator enumerator = default(List<WsoData>.Enumerator);
			if (createdWsos.Count > 0)
			{
				LazySingleton<ChunkManager>.Instance.RegisterChunks<IChunkableObject>(createdWsos, ChunkManagerLayerType.StaticWso);
			}
		}
	}

	// Token: 0x060029F4 RID: 10740 RVA: 0x000C5E2C File Offset: 0x000C402C
	private async Awaitable SpawnDropsFromData()
	{
		foreach (DropData dropData in this.gameSceneData.droppedItems)
		{
			this.CreateDropView(dropData);
		}
		await Awaitable.NextFrameAsync(default(CancellationToken));
	}

	// Token: 0x060029F5 RID: 10741 RVA: 0x000C5E70 File Offset: 0x000C4070
	private async Awaitable SpawnTechPointsFromData()
	{
		foreach (TechPointDropData techPointDropData in this.gameSceneData.techPointDrops)
		{
			TechPointDrop.Spawn(techPointDropData, base.transform);
		}
		await Awaitable.NextFrameAsync(default(CancellationToken));
	}

	// Token: 0x060029F6 RID: 10742 RVA: 0x000C5EB4 File Offset: 0x000C40B4
	private async Awaitable ScanChurchGraph()
	{
		RecastGraph recastGraph = AstarPath.active.graphs[6] as RecastGraph;
		if (recastGraph != null)
		{
			HashSet<IChunkableObject> allChunkableObjectsInBounds = LazySingleton<ChunkManager>.Instance.GetAllChunkableObjectsInBounds(recastGraph.bounds);
			foreach (IChunkableObject chunkableObject in allChunkableObjectsInBounds)
			{
				if (chunkableObject != null)
				{
					chunkableObject.UpdateFlag(ChunkingIgnoreType.Building, true);
				}
			}
			recastGraph.Scan();
			foreach (IChunkableObject chunkableObject2 in allChunkableObjectsInBounds)
			{
				if (chunkableObject2 != null)
				{
					chunkableObject2.UpdateFlag(ChunkingIgnoreType.Building, false);
				}
			}
		}
		await Awaitable.NextFrameAsync(default(CancellationToken));
	}

	// Token: 0x060029F7 RID: 10743 RVA: 0x000C5EF0 File Offset: 0x000C40F0
	private void HandleWgoAdd(WgoData data, bool recheckVisibilityOnSpawn)
	{
		HashSet<string> hashSet = new HashSet<string>();
		WgoPartPool.CollectAddressableKeysForWgoData(data, hashSet);
		LazySingleton<ScenePoolPathRegistry>.Instance.RegisterScenePaths(this.id, hashSet);
		this.CreateWgoView(data, false, recheckVisibilityOnSpawn);
	}

	// Token: 0x060029F8 RID: 10744 RVA: 0x000C5F28 File Offset: 0x000C4128
	private Wgo CreateWgoView(WgoData data, bool ignoreChunkRegistration = false, bool recheckVisibilityOnSpawn = false)
	{
		Wgo wgo = Wgo.Spawn(data, base.transform, true, ignoreChunkRegistration, false, recheckVisibilityOnSpawn);
		this.wgoViewCache.Add(data.UniqueId, wgo);
		GameScene.globalWgoViewCache.Add(data.UniqueId, wgo);
		return wgo;
	}

	// Token: 0x060029F9 RID: 10745 RVA: 0x000C5F6C File Offset: 0x000C416C
	private void HandleWgoRemove(WgoData data)
	{
		GameScene.globalWgoViewCache.Remove(data.UniqueId);
		Wgo wgo;
		if (this.wgoViewCache.Remove(data.UniqueId, out wgo))
		{
			if (wgo == null || wgo.IsDespawning)
			{
				return;
			}
			wgo.DespawnAfterDataWasRemoved();
		}
	}

	// Token: 0x060029FA RID: 10746 RVA: 0x000C5FB7 File Offset: 0x000C41B7
	private void HandleWsoAdd(WsoData data)
	{
		this.CreateWsoView(data);
	}

	// Token: 0x060029FB RID: 10747 RVA: 0x000C5FC4 File Offset: 0x000C41C4
	private Wso CreateWsoView(WsoData data)
	{
		Wso wso = Wso.Spawn(data, base.transform);
		this.wsoViewCache.Add(data.UniqueId, wso);
		GameScene.globalWsoViewCache.Add(data.UniqueId, wso);
		return wso;
	}

	// Token: 0x060029FC RID: 10748 RVA: 0x000C6004 File Offset: 0x000C4204
	private void HandleWsoRemoved(WsoData data)
	{
		GameScene.globalWsoViewCache.Remove(data.UniqueId);
		Wso wso;
		if (this.wsoViewCache.Remove(data.UniqueId, out wso))
		{
			global::UnityEngine.Object.Destroy(wso.gameObject);
		}
	}

	// Token: 0x060029FD RID: 10749 RVA: 0x000C6042 File Offset: 0x000C4242
	private void HandleDropAdd(DropData drop)
	{
		this.CreateDropView(drop);
	}

	// Token: 0x060029FE RID: 10750 RVA: 0x000C604C File Offset: 0x000C424C
	private void HandleDropRemove(DropData drop)
	{
		DropView dropView;
		if (this.dropViewCache.Remove(drop.UniqueId, out dropView))
		{
			dropView.DespawnView();
		}
	}

	// Token: 0x060029FF RID: 10751 RVA: 0x000C6074 File Offset: 0x000C4274
	private DropView CreateDropView(DropData drop)
	{
		DropView dropView = DropView.SpawnDrop(drop, base.transform, false);
		if (dropView == null)
		{
			return null;
		}
		this.dropViewCache[drop.UniqueId] = dropView;
		return dropView;
	}

	// Token: 0x06002A00 RID: 10752 RVA: 0x000C60B0 File Offset: 0x000C42B0
	private void AddWgosToWorldZones()
	{
		foreach (WorldZone worldZone in this.worldZones)
		{
			worldZone.AddWgosOnGameSceneStart();
		}
	}

	// Token: 0x06002A01 RID: 10753 RVA: 0x000C6100 File Offset: 0x000C4300
	private GDPoint[] GetGDPointsExcludingEditorContent()
	{
		return base.GetComponentsInChildren<GDPoint>(true);
	}

	// Token: 0x06002A02 RID: 10754 RVA: 0x000C6109 File Offset: 0x000C4309
	private GameSceneConfig GetResolvedGameSceneConfig()
	{
		return MainGame.Instance.gameSceneConfigs.Find((GameSceneConfig c) => c.name == this.gameSceneData.id);
	}

	// Token: 0x06002A03 RID: 10755 RVA: 0x000C6128 File Offset: 0x000C4328
	private WorldZone TrySpawnWorldZoneFromData(WorldZoneData worldZoneData, SceneWgoContentPart preferredContentPart = null)
	{
		WorldZone worldZone;
		if (MainGame.Instance.fightingLevelSystem.TryGetExistingWorldZone(worldZoneData.id, out worldZone))
		{
			return worldZone;
		}
		Transform transform = this.ResolveWorldZoneParent(worldZoneData, preferredContentPart);
		return WorldZone.Spawn(worldZoneData, transform);
	}

	// Token: 0x06002A04 RID: 10756 RVA: 0x000C6160 File Offset: 0x000C4360
	private Transform ResolveWorldZoneParent(WorldZoneData worldZoneData, SceneWgoContentPart preferredContentPart)
	{
		SceneWgoContentPart sceneWgoContentPart = preferredContentPart;
		GameObject gameObject;
		SceneWgoContentPart sceneWgoContentPart2;
		if (sceneWgoContentPart == null && this.gameSceneConfig.TryGetLoadedContent(worldZoneData.contentPartName, out gameObject) && gameObject != null && gameObject.TryGetComponent<SceneWgoContentPart>(out sceneWgoContentPart2))
		{
			sceneWgoContentPart = sceneWgoContentPart2;
		}
		if (!(sceneWgoContentPart != null))
		{
			return base.transform;
		}
		return sceneWgoContentPart.transform;
	}

	// Token: 0x040022E2 RID: 8930
	private const int MAX_OBJ_SPAWN_COUNT_PER_FRAME = 50;

	// Token: 0x040022E3 RID: 8931
	[SerializeField]
	private string id;

	// Token: 0x040022E4 RID: 8932
	private GameSceneData gameSceneData;

	// Token: 0x040022E5 RID: 8933
	[SerializeField]
	[CanBeNull]
	private GameSceneLinksCollection gameSceneLinksCollection;

	// Token: 0x040022E6 RID: 8934
	private SceneWaypointContent waypointContent;

	// Token: 0x040022E7 RID: 8935
	[SerializeField]
	private GameSceneConfig gameSceneConfig;

	// Token: 0x040022E8 RID: 8936
	[SerializeField]
	[HideInInspector]
	private ChunkableObjectComponent[] chunkComponents;

	// Token: 0x040022E9 RID: 8937
	[SerializeField]
	[HideInInspector]
	private List<ConstructorPart> constructorParts;

	// Token: 0x040022EA RID: 8938
	[OdinSerialize]
	[HideInInspector]
	public List<BakedChunkableObjectComponentData> bakedChunkableObjectComponentDatas = new List<BakedChunkableObjectComponentData>();

	// Token: 0x040022EB RID: 8939
	private Dictionary<SGuid, Wgo> wgoViewCache = new Dictionary<SGuid, Wgo>();

	// Token: 0x040022EC RID: 8940
	private static Dictionary<SGuid, Wgo> globalWgoViewCache = new Dictionary<SGuid, Wgo>();

	// Token: 0x040022ED RID: 8941
	private Dictionary<SGuid, DropView> dropViewCache = new Dictionary<SGuid, DropView>();

	// Token: 0x040022EE RID: 8942
	private Dictionary<SGuid, Wso> wsoViewCache = new Dictionary<SGuid, Wso>();

	// Token: 0x040022EF RID: 8943
	private static Dictionary<SGuid, Wso> globalWsoViewCache = new Dictionary<SGuid, Wso>();

	// Token: 0x040022F0 RID: 8944
	private GDPoint[] sceneGdPoints;

	// Token: 0x040022F1 RID: 8945
	private List<IChunkableObject> chunks;

	// Token: 0x040022F2 RID: 8946
	private List<WorldZone> worldZones;

	// Token: 0x040022F3 RID: 8947
	private bool isStartCompleted;
}
