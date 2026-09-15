using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200058A RID: 1418
[Serializable]
public class GameSceneData
{
	// Token: 0x14000060 RID: 96
	// (add) Token: 0x0600244A RID: 9290 RVA: 0x000AA86C File Offset: 0x000A8A6C
	// (remove) Token: 0x0600244B RID: 9291 RVA: 0x000AA8A4 File Offset: 0x000A8AA4
	public event Action<WgoData, bool> OnWgoDataAdd;

	// Token: 0x14000061 RID: 97
	// (add) Token: 0x0600244C RID: 9292 RVA: 0x000AA8DC File Offset: 0x000A8ADC
	// (remove) Token: 0x0600244D RID: 9293 RVA: 0x000AA914 File Offset: 0x000A8B14
	public event Action<WgoData> OnWgoDataRemove;

	// Token: 0x14000062 RID: 98
	// (add) Token: 0x0600244E RID: 9294 RVA: 0x000AA94C File Offset: 0x000A8B4C
	// (remove) Token: 0x0600244F RID: 9295 RVA: 0x000AA984 File Offset: 0x000A8B84
	public event Action<WgoData> OnWgoDataPreRemove;

	// Token: 0x14000063 RID: 99
	// (add) Token: 0x06002450 RID: 9296 RVA: 0x000AA9BC File Offset: 0x000A8BBC
	// (remove) Token: 0x06002451 RID: 9297 RVA: 0x000AA9F0 File Offset: 0x000A8BF0
	public static event Action<string, WgoData> OnWgoDataOnScenePreRemove;

	// Token: 0x14000064 RID: 100
	// (add) Token: 0x06002452 RID: 9298 RVA: 0x000AAA24 File Offset: 0x000A8C24
	// (remove) Token: 0x06002453 RID: 9299 RVA: 0x000AAA5C File Offset: 0x000A8C5C
	public event Action<WsoData> OnWsoDataAdd;

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06002454 RID: 9300 RVA: 0x000AAA94 File Offset: 0x000A8C94
	// (remove) Token: 0x06002455 RID: 9301 RVA: 0x000AAACC File Offset: 0x000A8CCC
	public event Action<WsoData> OnWsoDataRemove;

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06002456 RID: 9302 RVA: 0x000AAB04 File Offset: 0x000A8D04
	// (remove) Token: 0x06002457 RID: 9303 RVA: 0x000AAB38 File Offset: 0x000A8D38
	public static event Action<string, WsoData> OnWsoDataOnScenePreRemove;

	// Token: 0x14000067 RID: 103
	// (add) Token: 0x06002458 RID: 9304 RVA: 0x000AAB6C File Offset: 0x000A8D6C
	// (remove) Token: 0x06002459 RID: 9305 RVA: 0x000AABA4 File Offset: 0x000A8DA4
	public event Action<FightingLevelData> OnFightingLevelDataAdded;

	// Token: 0x14000068 RID: 104
	// (add) Token: 0x0600245A RID: 9306 RVA: 0x000AABDC File Offset: 0x000A8DDC
	// (remove) Token: 0x0600245B RID: 9307 RVA: 0x000AAC14 File Offset: 0x000A8E14
	public event Action<FightingLevelData> OnFightingLevelDataRemoved;

	// Token: 0x14000069 RID: 105
	// (add) Token: 0x0600245C RID: 9308 RVA: 0x000AAC4C File Offset: 0x000A8E4C
	// (remove) Token: 0x0600245D RID: 9309 RVA: 0x000AAC84 File Offset: 0x000A8E84
	public event Action<DropData> OnDropAdd;

	// Token: 0x1400006A RID: 106
	// (add) Token: 0x0600245E RID: 9310 RVA: 0x000AACBC File Offset: 0x000A8EBC
	// (remove) Token: 0x0600245F RID: 9311 RVA: 0x000AACF4 File Offset: 0x000A8EF4
	public event Action<DropData> OnDropRemove;

	// Token: 0x1400006B RID: 107
	// (add) Token: 0x06002460 RID: 9312 RVA: 0x000AAD2C File Offset: 0x000A8F2C
	// (remove) Token: 0x06002461 RID: 9313 RVA: 0x000AAD60 File Offset: 0x000A8F60
	public static event Action<string, DropData> OnDropOnScenePreRemoved;

	// Token: 0x170005E5 RID: 1509
	// (get) Token: 0x06002462 RID: 9314 RVA: 0x000AAD93 File Offset: 0x000A8F93
	public bool HasCache
	{
		get
		{
			return this.cache != null;
		}
	}

	// Token: 0x170005E6 RID: 1510
	// (get) Token: 0x06002463 RID: 9315 RVA: 0x000AAD9E File Offset: 0x000A8F9E
	public QuadTreeRoot<WgoData> QuadTreeRoot
	{
		get
		{
			return this.quadTreeRoot;
		}
	}

	// Token: 0x170005E7 RID: 1511
	// (get) Token: 0x06002464 RID: 9316 RVA: 0x000AADA6 File Offset: 0x000A8FA6
	// (set) Token: 0x06002465 RID: 9317 RVA: 0x000AADAE File Offset: 0x000A8FAE
	public SortedDictionary<int, List<WorldZoneData>> WorldZonesByProcessingPriority { get; private set; } = new SortedDictionary<int, List<WorldZoneData>>();

	// Token: 0x06002466 RID: 9318 RVA: 0x000AADB8 File Offset: 0x000A8FB8
	public GameSceneData()
	{
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x000AAE30 File Offset: 0x000A9030
	public GameSceneData(string id, Vector3 offset, WgoDataCache cache)
	{
		this.id = id;
		this.offset = offset;
		this.cache = cache;
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x000AAEBC File Offset: 0x000A90BC
	public void PrepareForGame(WgoDataCache cache)
	{
		this.cache = cache;
		this.RebuildWorldZonesByProcessingPriorityCache();
		foreach (WorldZoneData worldZoneData in this.worldZones)
		{
			worldZoneData.PrepareForGame();
		}
		List<WgoData> list = new List<WgoData>();
		foreach (WgoData wgoData in this.wgoDataList)
		{
			if (wgoData is ZombieWgoData)
			{
				list.Add(wgoData);
			}
			else
			{
				wgoData.PrepareForGame();
			}
		}
		foreach (WgoData wgoData2 in list)
		{
			wgoData2.PrepareForGame();
		}
		foreach (WsoData wsoData in this.wsoDataList)
		{
			wsoData.PrepareForGame();
		}
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x000AAFEC File Offset: 0x000A91EC
	public void AddWgoData(WgoData wgoData, bool recheckVisibilityOnSpawn = false)
	{
		wgoData.WorldId = this.id;
		this.cache.AddWgoDataToCache(wgoData);
		this.wgoDataList.Add(wgoData);
		Action<WgoData, bool> onWgoDataAdd = this.OnWgoDataAdd;
		if (onWgoDataAdd != null)
		{
			onWgoDataAdd(wgoData, recheckVisibilityOnSpawn);
		}
		this.HandleWgoDataAddWorldZone(wgoData);
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000AB02C File Offset: 0x000A922C
	public WgoData AddWgoData(string wgoId, Vector3 position, string customTag = "", bool recheckVisibilityOnSpawn = false)
	{
		WGODef wgodef;
		WgoData wgoData;
		if (GameBalance.Me.conveyorWgosCache.TryGetValue(wgoId, out wgodef))
		{
			wgoData = new ConveyorWgoData(wgodef.conveyorType, wgoId, position, this.id);
			if (!string.IsNullOrEmpty(customTag))
			{
				wgoData.CustomTag = customTag;
			}
		}
		else
		{
			wgoData = ((customTag == "") ? new WgoData(wgoId, position, this.id) : new WgoData(wgoId, position, this.id, customTag));
		}
		this.cache.AddWgoDataToCache(wgoData);
		this.wgoDataList.Add(wgoData);
		Action<WgoData, bool> onWgoDataAdd = this.OnWgoDataAdd;
		if (onWgoDataAdd != null)
		{
			onWgoDataAdd(wgoData, recheckVisibilityOnSpawn);
		}
		this.HandleWgoDataAddWorldZone(wgoData);
		return wgoData;
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000AB0D4 File Offset: 0x000A92D4
	public void RemoveWgoData(WgoData wgoData, bool clearCraftComponent = true)
	{
		if (!this.cache.RemoveWgoDataFromCache(wgoData))
		{
			return;
		}
		this.wgoDataList.Remove(wgoData);
		wgoData.OnRemove(clearCraftComponent);
		Action<string, WgoData> onWgoDataOnScenePreRemove = GameSceneData.OnWgoDataOnScenePreRemove;
		if (onWgoDataOnScenePreRemove != null)
		{
			onWgoDataOnScenePreRemove(this.id, wgoData);
		}
		Action<WgoData> onWgoDataPreRemove = this.OnWgoDataPreRemove;
		if (onWgoDataPreRemove != null)
		{
			onWgoDataPreRemove(wgoData);
		}
		Action<WgoData> onWgoDataRemove = this.OnWgoDataRemove;
		if (onWgoDataRemove != null)
		{
			onWgoDataRemove(wgoData);
		}
		this.HandleWgoDataRemoveWorldZone(wgoData);
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000AB148 File Offset: 0x000A9348
	public DropData AddDrop(Item droppableItem, Vector3 pos)
	{
		DropData dropData = new DropData(droppableItem, pos, this.id);
		return this.AddDrop(dropData);
	}

	// Token: 0x0600246D RID: 9325 RVA: 0x000AB16A File Offset: 0x000A936A
	public DropData AddDrop(DropData drop)
	{
		this.droppedItems.Add(drop);
		Action<DropData> onDropAdd = this.OnDropAdd;
		if (onDropAdd != null)
		{
			onDropAdd(drop);
		}
		drop.TryStartAutoDestroyTimer();
		return drop;
	}

	// Token: 0x0600246E RID: 9326 RVA: 0x000AB194 File Offset: 0x000A9394
	public void AddDropToQueue(Item droppableItem, Vector3 pos, bool getPosDropFromDockPoint = false)
	{
		for (int i = 0; i < this.queuedDrops.Count; i++)
		{
			if (!(this.queuedDrops[i].Id != droppableItem.id))
			{
				int num = this.queuedDrops[i].CanAddItemCount(droppableItem);
				if (num > 0)
				{
					Item item = droppableItem.Split(num, false);
					this.queuedDrops[i].AddItem(item);
				}
				if (droppableItem.Count == 0)
				{
					return;
				}
			}
		}
		DropData dropData = new DropData(droppableItem, pos, this.id);
		this.queuedDrops.Add(dropData);
	}

	// Token: 0x0600246F RID: 9327 RVA: 0x000AB22C File Offset: 0x000A942C
	public void RemoveDrop(DropData drop)
	{
		drop.MarkAsRemoving();
		this.droppedItems.Remove(drop);
		this.queuedDrops.Remove(drop);
		Action<string, DropData> onDropOnScenePreRemoved = GameSceneData.OnDropOnScenePreRemoved;
		if (onDropOnScenePreRemoved != null)
		{
			onDropOnScenePreRemoved(this.id, drop);
		}
		Action<DropData> onDropRemove = this.OnDropRemove;
		if (onDropRemove == null)
		{
			return;
		}
		onDropRemove(drop);
	}

	// Token: 0x06002470 RID: 9328 RVA: 0x000AB281 File Offset: 0x000A9481
	public void AddTechPointDrop(TechPointDropData drop)
	{
		this.techPointDrops.Add(drop);
	}

	// Token: 0x06002471 RID: 9329 RVA: 0x000AB28F File Offset: 0x000A948F
	public void RemoveTechPointDrop(TechPointDropData drop)
	{
		this.techPointDrops.Remove(drop);
	}

	// Token: 0x06002472 RID: 9330 RVA: 0x000AB2A0 File Offset: 0x000A94A0
	public void ProcessQueuedDrops()
	{
		foreach (DropData dropData in this.queuedDrops)
		{
			this.AddDrop(dropData);
		}
		this.queuedDrops.Clear();
	}

	// Token: 0x06002473 RID: 9331 RVA: 0x000AB300 File Offset: 0x000A9500
	public void AddWorldZoneData(WorldZoneData worldZoneData, WorldZoneBakedData bakedData)
	{
		this.worldZones.Add(worldZoneData);
		this.RegisterWorldZoneInProcessingPriorityCache(worldZoneData);
		foreach (WgoData wgoData in this.wgoDataList)
		{
			worldZoneData.TryAddWgoData(wgoData);
		}
		if (bakedData.prebuiltWgoParams != null)
		{
			WorldData worldData = MainGame.Instance.GameSave.worldData;
			foreach (WorldZonePrebuiltWgoParams worldZonePrebuiltWgoParams in bakedData.prebuiltWgoParams)
			{
				if (worldZonePrebuiltWgoParams.execExpressionAfterBuilding)
				{
					WgoData wgoData2 = worldData.GetWgoData(worldZonePrebuiltWgoParams.wgoUniqueId);
					BuildingDef buildingDef;
					if (wgoData2 != null && wgoData2.Definition.TryGetBuildingDefForWgo(out buildingDef))
					{
						worldData.TryExecutePrebuiltWgoAfterBuildingExpressions(wgoData2, buildingDef);
					}
				}
			}
		}
		Debug.Log(string.Concat(new string[] { "Adding world zone data with id:[", worldZoneData.id, "] to scene:[", this.id, "]" }));
	}

	// Token: 0x06002474 RID: 9332 RVA: 0x000AB42C File Offset: 0x000A962C
	public WorldZoneData GetWorldZoneDataById(string id)
	{
		return this.worldZones.Find((WorldZoneData worldZone) => worldZone.id == id);
	}

	// Token: 0x06002475 RID: 9333 RVA: 0x000AB45D File Offset: 0x000A965D
	public void RemoveWorldZoneData(WorldZoneData worldZoneData)
	{
		if (!this.worldZones.Remove(worldZoneData))
		{
			return;
		}
		this.DetachWgosFromRemovedWorldZone(worldZoneData);
		this.RebuildWorldZonesByProcessingPriorityCache();
	}

	// Token: 0x06002476 RID: 9334 RVA: 0x000AB47C File Offset: 0x000A967C
	private void DetachWgosFromRemovedWorldZone(WorldZoneData worldZoneData)
	{
		if (((worldZoneData != null) ? worldZoneData.wgoDataList : null) == null)
		{
			return;
		}
		for (int i = 0; i < worldZoneData.wgoDataList.Count; i++)
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(worldZoneData.wgoDataList[i]);
			if (wgoData != null && wgoData.WorldZoneData == worldZoneData)
			{
				wgoData.WorldZoneData = null;
			}
		}
	}

	// Token: 0x06002477 RID: 9335 RVA: 0x000AB4D8 File Offset: 0x000A96D8
	private void RebuildWorldZonesByProcessingPriorityCache()
	{
		if (this.WorldZonesByProcessingPriority == null)
		{
			this.WorldZonesByProcessingPriority = new SortedDictionary<int, List<WorldZoneData>>();
		}
		this.WorldZonesByProcessingPriority.Clear();
		foreach (WorldZoneData worldZoneData in this.worldZones)
		{
			this.RegisterWorldZoneInProcessingPriorityCache(worldZoneData);
		}
	}

	// Token: 0x06002478 RID: 9336 RVA: 0x000AB54C File Offset: 0x000A974C
	private void RegisterWorldZoneInProcessingPriorityCache(WorldZoneData worldZoneData)
	{
		List<WorldZoneData> list;
		if (!this.WorldZonesByProcessingPriority.TryGetValue(worldZoneData.processingPriority, out list))
		{
			list = new List<WorldZoneData>();
			this.WorldZonesByProcessingPriority[worldZoneData.processingPriority] = list;
		}
		list.Add(worldZoneData);
	}

	// Token: 0x06002479 RID: 9337 RVA: 0x000AB590 File Offset: 0x000A9790
	private void HandleWgoDataAddWorldZone(WgoData wgoData)
	{
		foreach (KeyValuePair<int, List<WorldZoneData>> keyValuePair in this.WorldZonesByProcessingPriority.Reverse<KeyValuePair<int, List<WorldZoneData>>>())
		{
			int num;
			List<WorldZoneData> list;
			keyValuePair.Deconstruct(out num, out list);
			using (List<WorldZoneData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.TryAddWgoData(wgoData))
					{
						return;
					}
				}
			}
		}
	}

	// Token: 0x0600247A RID: 9338 RVA: 0x000AB628 File Offset: 0x000A9828
	private void HandleWgoDataRemoveWorldZone(WgoData wgoData)
	{
		foreach (KeyValuePair<int, List<WorldZoneData>> keyValuePair in this.WorldZonesByProcessingPriority.Reverse<KeyValuePair<int, List<WorldZoneData>>>())
		{
			int num;
			List<WorldZoneData> list;
			keyValuePair.Deconstruct(out num, out list);
			foreach (WorldZoneData worldZoneData in list)
			{
				worldZoneData.RemoveWgoData(wgoData);
			}
		}
	}

	// Token: 0x0600247B RID: 9339 RVA: 0x000AB6BC File Offset: 0x000A98BC
	public void AddWsoData(WsoData wsoData)
	{
		wsoData.WorldId = this.id;
		this.cache.AddWsoDataToCache(wsoData);
		this.wsoDataList.Add(wsoData);
		Action<WsoData> onWsoDataAdd = this.OnWsoDataAdd;
		if (onWsoDataAdd == null)
		{
			return;
		}
		onWsoDataAdd(wsoData);
	}

	// Token: 0x0600247C RID: 9340 RVA: 0x000AB6F4 File Offset: 0x000A98F4
	public WsoData AddWsoData(string defId, Vector3 position)
	{
		WsoData wsoData = new WsoData(GameBalance.Me.GetData<WSODef>(defId), new SGuid(), position, this.id);
		this.cache.AddWsoDataToCache(wsoData);
		this.wsoDataList.Add(wsoData);
		Action<WsoData> onWsoDataAdd = this.OnWsoDataAdd;
		if (onWsoDataAdd != null)
		{
			onWsoDataAdd(wsoData);
		}
		return wsoData;
	}

	// Token: 0x0600247D RID: 9341 RVA: 0x000AB74C File Offset: 0x000A994C
	public void RemoveWsoData(WsoData wsoData)
	{
		if (!this.cache.RemoveWsoDataFromCache(wsoData))
		{
			return;
		}
		this.wsoDataList.Remove(wsoData);
		wsoData.Cleanup();
		Action<string, WsoData> onWsoDataOnScenePreRemove = GameSceneData.OnWsoDataOnScenePreRemove;
		if (onWsoDataOnScenePreRemove != null)
		{
			onWsoDataOnScenePreRemove(this.id, wsoData);
		}
		Action<WsoData> onWsoDataRemove = this.OnWsoDataRemove;
		if (onWsoDataRemove == null)
		{
			return;
		}
		onWsoDataRemove(wsoData);
	}

	// Token: 0x0600247E RID: 9342 RVA: 0x000AB7A4 File Offset: 0x000A99A4
	public void AddFightingLevelData(string id)
	{
		if (this.fightingLevels.Find((FightingLevelData fightingLevel) => fightingLevel.id == id) != null)
		{
			Debug.LogWarning(string.Concat(new string[] { "Fighting level with id:[", id, "] already exists in scene:[", this.id, "]" }));
			return;
		}
		if (MainGame.Instance.gameSceneConfigs.Find((GameSceneConfig c) => c.name == this.id) == null)
		{
			Debug.LogError("Can't find game scene config with id:[" + this.id + "] in gameSceneConfigs");
			return;
		}
		FightingLevelData fightingLevelData = new FightingLevelData(id);
		this.fightingLevels.Add(fightingLevelData);
		Debug.Log(string.Concat(new string[] { "Added fighting level data with id:[", id, "] to scene:[", this.id, "]" }));
		Action<FightingLevelData> onFightingLevelDataAdded = this.OnFightingLevelDataAdded;
		if (onFightingLevelDataAdded == null)
		{
			return;
		}
		onFightingLevelDataAdded(fightingLevelData);
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x000AB8BC File Offset: 0x000A9ABC
	public void RemoveFightingLevelData(string id)
	{
		int num = this.fightingLevels.FindIndex((FightingLevelData fightingLevel) => fightingLevel.id == id);
		if (num == -1)
		{
			Debug.LogWarning(string.Concat(new string[] { "Fighting level with id:[", id, "] not found in scene:[", this.id, "]" }));
			return;
		}
		FightingLevelData fightingLevelData = this.fightingLevels[num];
		this.fightingLevels.RemoveAt(num);
		Debug.Log(string.Concat(new string[] { "Removed fighting level data with id:[", id, "] from scene:[", this.id, "]" }));
		Action<FightingLevelData> onFightingLevelDataRemoved = this.OnFightingLevelDataRemoved;
		if (onFightingLevelDataRemoved == null)
		{
			return;
		}
		onFightingLevelDataRemoved(fightingLevelData);
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000AB994 File Offset: 0x000A9B94
	public void ApplyStageForFightingLevel(string id, int stageId)
	{
		int num = this.fightingLevels.FindIndex((FightingLevelData fightingLevel) => fightingLevel.id == id);
		if (num == -1)
		{
			Debug.LogWarning(string.Concat(new string[] { "Fighting level with id:[", id, "] not found in scene:[", this.id, "]" }));
			return;
		}
		this.fightingLevels[num].CurStageId = stageId;
	}

	// Token: 0x04002042 RID: 8258
	public string id;

	// Token: 0x04002043 RID: 8259
	public Vector3 offset;

	// Token: 0x04002044 RID: 8260
	public List<string> loadedContentsList = new List<string>();

	// Token: 0x04002045 RID: 8261
	public List<WgoData> wgoDataList = new List<WgoData>();

	// Token: 0x04002046 RID: 8262
	public List<WsoData> wsoDataList = new List<WsoData>();

	// Token: 0x04002047 RID: 8263
	public List<DropData> droppedItems = new List<DropData>();

	// Token: 0x04002048 RID: 8264
	public List<DropData> queuedDrops = new List<DropData>();

	// Token: 0x04002049 RID: 8265
	public List<TechPointDropData> techPointDrops = new List<TechPointDropData>();

	// Token: 0x0400204A RID: 8266
	public List<WorldZoneData> worldZones = new List<WorldZoneData>();

	// Token: 0x0400204B RID: 8267
	public List<FightingLevelData> fightingLevels = new List<FightingLevelData>();

	// Token: 0x0400204C RID: 8268
	private WgoDataCache cache;

	// Token: 0x0400204D RID: 8269
	[NonSerialized]
	private QuadTreeRoot<WgoData> quadTreeRoot;
}
