using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200047F RID: 1151
public class SaveFixContext
{
	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x06001E86 RID: 7814 RVA: 0x000900F7 File Offset: 0x0008E2F7
	public GameSave GameSave
	{
		get
		{
			return this.gameSave;
		}
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x00090100 File Offset: 0x0008E300
	public SaveFixContext(GameSave gameSave, IList<GameSceneConfig> gameSceneConfigs)
	{
		this.gameSave = gameSave;
		this.gameSceneConfigs = gameSceneConfigs ?? Array.Empty<GameSceneConfig>();
		this.RebuildWgoIndex();
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x00090151 File Offset: 0x0008E351
	public void Log(string message)
	{
		Debug.Log("[SaveFixer] " + message);
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x00090163 File Offset: 0x0008E363
	public void LogWarning(string message)
	{
		Debug.LogWarning("[SaveFixer] " + message);
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x00090175 File Offset: 0x0008E375
	public void LogError(string message)
	{
		Debug.LogError("[SaveFixer] " + message);
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x00090188 File Offset: 0x0008E388
	public bool TryGetWgo(SGuid uniqueId, out WgoData wgoData, out GameSceneData sceneData)
	{
		wgoData = null;
		sceneData = null;
		if (SGuid.IsNullOrEmpty(uniqueId))
		{
			return false;
		}
		SaveFixContext.WgoIndexEntry wgoIndexEntry;
		if (!this.wgoByUid.TryGetValue(uniqueId.Guid, out wgoIndexEntry))
		{
			return false;
		}
		wgoData = wgoIndexEntry.wgoData;
		sceneData = wgoIndexEntry.sceneData;
		return true;
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x000901CD File Offset: 0x0008E3CD
	public bool HasWgo(SGuid uniqueId)
	{
		return !SGuid.IsNullOrEmpty(uniqueId) && this.wgoByUid.ContainsKey(uniqueId.Guid);
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x000901EC File Offset: 0x0008E3EC
	public bool TryFindWgoByIdNear(string wgoId, Vector3 origin, float radius, out WgoData wgoData, out GameSceneData sceneData)
	{
		return this.TryFindWgoNear(origin, radius, out wgoData, out sceneData, (WgoData candidate) => candidate.id == wgoId);
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x00090220 File Offset: 0x0008E420
	public bool TryFindWgoByGroupNear(string wgoGroup, Vector3 origin, float radius, out WgoData wgoData, out GameSceneData sceneData)
	{
		wgoData = null;
		sceneData = null;
		return !string.IsNullOrEmpty(wgoGroup) && !(GameBalance.Me == null) && this.TryFindWgoNear(origin, radius, out wgoData, out sceneData, (WgoData candidate) => GameBalance.Me.HasWgoIdByGroup(wgoGroup, candidate.id));
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x00090278 File Offset: 0x0008E478
	private bool TryFindWgoNear(Vector3 origin, float radius, out WgoData wgoData, out GameSceneData sceneData, Func<WgoData, bool> match)
	{
		wgoData = null;
		sceneData = null;
		if (match == null || radius < 0f)
		{
			return false;
		}
		WorldData worldData = this.gameSave.worldData;
		List<GameSceneData> list = ((worldData != null) ? worldData.gameSceneDataList : null);
		if (list == null)
		{
			return false;
		}
		float num = radius * radius;
		float num2 = float.MaxValue;
		for (int i = 0; i < list.Count; i++)
		{
			GameSceneData gameSceneData = list[i];
			if (((gameSceneData != null) ? gameSceneData.wgoDataList : null) != null)
			{
				for (int j = 0; j < gameSceneData.wgoDataList.Count; j++)
				{
					WgoData wgoData2 = gameSceneData.wgoDataList[j];
					if (wgoData2 != null && !string.IsNullOrEmpty(wgoData2.id) && match(wgoData2))
					{
						float num3 = SaveFixContext.DistanceXzSq(wgoData2.Position, origin);
						if (num3 <= num && num3 < num2)
						{
							num2 = num3;
							wgoData = wgoData2;
							sceneData = gameSceneData;
						}
					}
				}
			}
		}
		return wgoData != null;
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x00090364 File Offset: 0x0008E564
	private static float DistanceXzSq(Vector3 a, Vector3 b)
	{
		float num = a.x - b.x;
		float num2 = a.z - b.z;
		return num * num + num2 * num2;
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x00090394 File Offset: 0x0008E594
	public void AddWgoData(GameSceneData sceneData, WgoData wgoData)
	{
		if (sceneData == null || wgoData == null)
		{
			return;
		}
		if (!sceneData.HasCache)
		{
			this.LogError("AddWgoData: scene [" + sceneData.id + "] has no cache, run after PrepareForGame");
			return;
		}
		wgoData.WorldId = sceneData.id;
		sceneData.AddWgoData(wgoData, false);
		this.wgoByUid[wgoData.UniqueId.Guid] = new SaveFixContext.WgoIndexEntry(sceneData, wgoData);
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x00090400 File Offset: 0x0008E600
	public bool RemoveWgoData(SGuid uniqueId)
	{
		WgoData wgoData;
		GameSceneData gameSceneData;
		if (!this.TryGetWgo(uniqueId, out wgoData, out gameSceneData))
		{
			return false;
		}
		if (!gameSceneData.HasCache)
		{
			this.LogError("RemoveWgoData: scene [" + gameSceneData.id + "] has no cache, run after PrepareForGame");
			return false;
		}
		this.RemoveConveyorSystemLinks(wgoData);
		this.PrepareDataLayerForRemove(gameSceneData, wgoData);
		gameSceneData.RemoveWgoData(wgoData, true);
		this.wgoByUid.Remove(uniqueId.Guid);
		return true;
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x0009046C File Offset: 0x0008E66C
	public bool MoveWgo(SGuid uniqueId, Vector3 newPosition)
	{
		WgoData wgoData;
		GameSceneData gameSceneData;
		if (!this.TryGetWgo(uniqueId, out wgoData, out gameSceneData))
		{
			return false;
		}
		if (!gameSceneData.HasCache)
		{
			this.LogError("MoveWgo: scene [" + gameSceneData.id + "] has no cache, run after PrepareForGame");
			return false;
		}
		SaveFixContext.RemoveFromWorldZonesLive(gameSceneData, wgoData);
		wgoData.Position = newPosition;
		SaveFixContext.AssignWorldZoneLive(gameSceneData, wgoData);
		return true;
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x000904C4 File Offset: 0x0008E6C4
	public bool TryGetContentPart(string assetGuid, out SceneWgoContentPart part, out GameSceneData sceneData, out GameSceneConfig config)
	{
		part = null;
		sceneData = null;
		config = null;
		if (string.IsNullOrEmpty(assetGuid))
		{
			return false;
		}
		SaveFixContext.LoadedContent loadedContent;
		if (this.contentByGuid.TryGetValue(assetGuid, out loadedContent))
		{
			part = loadedContent.part;
			sceneData = loadedContent.sceneData;
			config = loadedContent.config;
			return part != null && sceneData != null && config != null;
		}
		if (!this.gameSave.worldData.TryGetGameSceneDataForContentGuid(assetGuid, this.gameSceneConfigs, out sceneData, out config))
		{
			if (config != null)
			{
				this.LogError(string.Concat(new string[] { "GameSceneConfig [", config.name, "] contains content guid [", assetGuid, "] but scene is not in the save" }));
			}
			else
			{
				this.LogError("No GameSceneConfig contains content guid [" + assetGuid + "]");
			}
			return false;
		}
		AssetReference contentDataRefByGuid = config.GetContentDataRefByGuid(assetGuid);
		if (contentDataRefByGuid == null)
		{
			this.LogError(string.Concat(new string[] { "GameSceneConfig [", config.name, "] has no AssetReference for guid [", assetGuid, "]" }));
			return false;
		}
		bool flag = config.IsSceneContentDataLoaded(contentDataRefByGuid);
		SceneWgoContentData sceneWgoContentData;
		if (!config.TryLoadSceneDataContentByRef(contentDataRefByGuid, out sceneWgoContentData) || sceneWgoContentData == null)
		{
			this.LogError("Failed to load content for guid [" + assetGuid + "]");
			return false;
		}
		part = sceneWgoContentData.GetComponent<SceneWgoContentPart>();
		if (part == null)
		{
			this.LogError("Loaded content [" + sceneWgoContentData.name + "] has no SceneWgoContentPart");
			return false;
		}
		this.contentByGuid[assetGuid] = new SaveFixContext.LoadedContent(part, sceneData, config);
		if (!flag)
		{
			this.contentLoadedByFixer.Add(new ValueTuple<GameSceneConfig, AssetReference>(config, contentDataRefByGuid));
		}
		return true;
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x00090680 File Offset: 0x0008E880
	public void UnloadLoadedContent()
	{
		for (int i = 0; i < this.contentLoadedByFixer.Count; i++)
		{
			ValueTuple<GameSceneConfig, AssetReference> valueTuple = this.contentLoadedByFixer[i];
			GameSceneConfig item = valueTuple.Item1;
			AssetReference item2 = valueTuple.Item2;
			if (!(item == null) && item2 != null)
			{
				item.TryUnloadSceneDataContent(item2);
			}
		}
		this.contentLoadedByFixer.Clear();
		this.contentByGuid.Clear();
	}

	// Token: 0x06001E96 RID: 7830 RVA: 0x000906E6 File Offset: 0x0008E8E6
	public void WarnAboutDanglingReferences(SGuid uniqueId, string wgoId)
	{
		if (SGuid.IsNullOrEmpty(uniqueId))
		{
			return;
		}
		this.WarnIfDelayedSpawnReferences(uniqueId, wgoId);
		this.WarnIfNpcLifeSimulatorReferences(uniqueId, wgoId);
		this.WarnIfZombieSystemReferences(uniqueId, wgoId);
		this.WarnIfConveyorSystemReferences(uniqueId, wgoId);
	}

	// Token: 0x06001E97 RID: 7831 RVA: 0x00090714 File Offset: 0x0008E914
	private void RebuildWgoIndex()
	{
		this.wgoByUid.Clear();
		List<GameSceneData> gameSceneDataList = this.gameSave.worldData.gameSceneDataList;
		if (gameSceneDataList == null)
		{
			return;
		}
		foreach (GameSceneData gameSceneData in gameSceneDataList)
		{
			if (((gameSceneData != null) ? gameSceneData.wgoDataList : null) != null)
			{
				foreach (WgoData wgoData in gameSceneData.wgoDataList)
				{
					if (!(((wgoData != null) ? wgoData.UniqueId : null) == null) && !this.wgoByUid.TryAdd(wgoData.UniqueId.Guid, new SaveFixContext.WgoIndexEntry(gameSceneData, wgoData)))
					{
						this.LogWarning(string.Format("Duplicate uniqueId [{0}] id [{1}] scene [{2}]", wgoData.UniqueId, wgoData.id, gameSceneData.id));
					}
				}
			}
		}
	}

	// Token: 0x06001E98 RID: 7832 RVA: 0x0009082C File Offset: 0x0008EA2C
	private static void AssignWorldZoneLive(GameSceneData sceneData, WgoData wgoData)
	{
		if (((sceneData != null) ? sceneData.worldZones : null) == null || wgoData == null)
		{
			return;
		}
		List<WorldZoneData> zonesByDescendingPriority = SaveFixContext.GetZonesByDescendingPriority(sceneData);
		for (int i = 0; i < zonesByDescendingPriority.Count; i++)
		{
			WorldZoneData worldZoneData = zonesByDescendingPriority[i];
			if (worldZoneData != null && worldZoneData.TryAddWgoData(wgoData))
			{
				return;
			}
		}
	}

	// Token: 0x06001E99 RID: 7833 RVA: 0x00090878 File Offset: 0x0008EA78
	private void PrepareDataLayerForRemove(GameSceneData sceneData, WgoData wgoData)
	{
		SaveFixContext.RemoveOrdersTargetingWgo(sceneData, wgoData);
		this.RemoveFromAllNpcLifeSimGroups(wgoData);
		this.UnlinkRemainingWorkbenchExtensions(wgoData);
	}

	// Token: 0x06001E9A RID: 7834 RVA: 0x00090890 File Offset: 0x0008EA90
	private static void RemoveOrdersTargetingWgo(GameSceneData sceneData, WgoData wgoData)
	{
		if (((sceneData != null) ? sceneData.worldZones : null) == null || wgoData == null)
		{
			return;
		}
		foreach (WorldZoneData worldZoneData in sceneData.worldZones)
		{
			if (worldZoneData != null)
			{
				worldZoneData.RemoveOrdersByTarget(wgoData.UniqueId);
			}
		}
	}

	// Token: 0x06001E9B RID: 7835 RVA: 0x00090900 File Offset: 0x0008EB00
	private void RemoveFromAllNpcLifeSimGroups(WgoData wgoData)
	{
		NPCLifeSimulatorData npcLifeSimulatorData = this.gameSave.npcLifeSimulatorData;
		if (((npcLifeSimulatorData != null) ? npcLifeSimulatorData.AllGroups : null) == null || wgoData == null)
		{
			return;
		}
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in npcLifeSimulatorData.AllGroups)
		{
			if (npcgroupPointOfInterestData != null)
			{
				npcgroupPointOfInterestData.RemoveWgoFromGroup(wgoData);
			}
		}
	}

	// Token: 0x06001E9C RID: 7836 RVA: 0x00090978 File Offset: 0x0008EB78
	private void UnlinkRemainingWorkbenchExtensions(WgoData wgoData)
	{
		if (((wgoData != null) ? wgoData.AttachedWorkbenchExtensions : null) == null)
		{
			return;
		}
		SGuid uniqueId = wgoData.UniqueId;
		IReadOnlyList<SGuid> attachedWorkbenchExtensions = wgoData.AttachedWorkbenchExtensions;
		for (int i = 0; i < attachedWorkbenchExtensions.Count; i++)
		{
			WgoData wgoData2;
			GameSceneData gameSceneData;
			if (this.TryGetWgo(attachedWorkbenchExtensions[i], out wgoData2, out gameSceneData))
			{
				wgoData2.RemoveWorkbenchParent(uniqueId);
			}
		}
	}

	// Token: 0x06001E9D RID: 7837 RVA: 0x000909D0 File Offset: 0x0008EBD0
	private void RemoveConveyorSystemLinks(WgoData wgoData)
	{
		ConveyorSystemData conveyorSystemData = this.gameSave.conveyorSystemData;
		if (((conveyorSystemData != null) ? conveyorSystemData.conveyorComponents : null) == null)
		{
			return;
		}
		SGuid uniqueId = wgoData.UniqueId;
		List<ConveyorComponent> conveyorComponents = conveyorSystemData.conveyorComponents;
		for (int i = conveyorComponents.Count - 1; i >= 0; i--)
		{
			ConveyorComponent conveyorComponent = conveyorComponents[i];
			if (conveyorComponent == null || conveyorComponent.wgoDataUniqueId == uniqueId)
			{
				conveyorComponents.RemoveAt(i);
				SaveFixContext.RemoveFromConveyorCache<ConveyorComponent>(conveyorSystemData.graphStartElements, conveyorComponent);
				SaveFixContext.RemoveFromConveyorCache<ConveyorComponent>(conveyorSystemData.graphEndElements, conveyorComponent);
				SaveFixContext.RemoveFromConveyorCache<ConveyorWorkbenchComponent>(conveyorSystemData.workbenchElements, conveyorComponent);
				SaveFixContext.RemoveFromConveyorCache<ConveyorSplitterComponent>(conveyorSystemData.splitterElements, conveyorComponent);
			}
			else
			{
				conveyorComponent.RemoveParentLink(uniqueId);
				WgoData wgoData2;
				GameSceneData gameSceneData;
				if (this.TryGetWgo(conveyorComponent.wgoDataUniqueId, out wgoData2, out gameSceneData))
				{
					ConveyorWgoData conveyorWgoData = wgoData2 as ConveyorWgoData;
					if (conveyorWgoData != null)
					{
						SaveFixContext.RemoveUid(conveyorWgoData.HardConnectedWGOs, uniqueId);
					}
				}
			}
		}
	}

	// Token: 0x06001E9E RID: 7838 RVA: 0x00090AB0 File Offset: 0x0008ECB0
	private static void RemoveFromConveyorCache<T>(List<T> cache, ConveyorComponent component) where T : ConveyorComponent
	{
		if (cache == null)
		{
			return;
		}
		for (int i = cache.Count - 1; i >= 0; i--)
		{
			if (cache[i] == component)
			{
				cache.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001E9F RID: 7839 RVA: 0x00090AEC File Offset: 0x0008ECEC
	private static void RemoveFromWorldZonesLive(GameSceneData sceneData, WgoData wgoData)
	{
		if (((sceneData != null) ? sceneData.worldZones : null) == null || wgoData == null)
		{
			return;
		}
		foreach (WorldZoneData worldZoneData in sceneData.worldZones)
		{
			if (worldZoneData != null)
			{
				worldZoneData.RemoveWgoData(wgoData);
			}
		}
	}

	// Token: 0x06001EA0 RID: 7840 RVA: 0x00090B58 File Offset: 0x0008ED58
	private static List<WorldZoneData> GetZonesByDescendingPriority(GameSceneData sceneData)
	{
		List<WorldZoneData> list = new List<WorldZoneData>(sceneData.worldZones);
		list.Sort(delegate(WorldZoneData a, WorldZoneData b)
		{
			int num = ((a != null) ? a.processingPriority : int.MinValue);
			return ((b != null) ? b.processingPriority : int.MinValue).CompareTo(num);
		});
		return list;
	}

	// Token: 0x06001EA1 RID: 7841 RVA: 0x00090B8C File Offset: 0x0008ED8C
	private static bool ContainsUid(List<SGuid> list, SGuid uniqueId)
	{
		if (list == null || SGuid.IsNullOrEmpty(uniqueId))
		{
			return false;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == uniqueId)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EA2 RID: 7842 RVA: 0x00090BCC File Offset: 0x0008EDCC
	private static void RemoveUid(List<SGuid> list, SGuid uniqueId)
	{
		if (list == null || SGuid.IsNullOrEmpty(uniqueId))
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i] == uniqueId)
			{
				list.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001EA3 RID: 7843 RVA: 0x00090C10 File Offset: 0x0008EE10
	private void WarnIfDelayedSpawnReferences(SGuid uniqueId, string wgoId)
	{
		WgoDelayedSpawnSystemData wgoDelayedSpawnSystemData = this.gameSave.wgoDelayedSpawnSystemData;
		List<SpawnDelayedObject> list = ((wgoDelayedSpawnSystemData != null) ? wgoDelayedSpawnSystemData.spawnDelayedObjects : null);
		if (list == null)
		{
			return;
		}
		foreach (SpawnDelayedObject spawnDelayedObject in list)
		{
			if (spawnDelayedObject != null)
			{
				SGuid sguid = spawnDelayedObject.wgoUniqueId;
				if (SGuid.IsNullOrEmpty(sguid) && spawnDelayedObject.wgoData != null)
				{
					sguid = spawnDelayedObject.wgoData.UniqueId;
				}
				if (sguid == uniqueId)
				{
					this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is still referenced by wgoDelayedSpawnSystemData", wgoId, uniqueId));
					break;
				}
			}
		}
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x00090CB8 File Offset: 0x0008EEB8
	private void WarnIfNpcLifeSimulatorReferences(SGuid uniqueId, string wgoId)
	{
		NPCLifeSimulatorData npcLifeSimulatorData = this.gameSave.npcLifeSimulatorData;
		if (npcLifeSimulatorData == null)
		{
			return;
		}
		if (npcLifeSimulatorData.GetGroupByWGOId(uniqueId) != null)
		{
			this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is still referenced by npcLifeSimulatorData groups", wgoId, uniqueId));
			return;
		}
		foreach (NPCPointOfInterestAnimationData npcpointOfInterestAnimationData in npcLifeSimulatorData.AnimationDatas)
		{
			if (npcpointOfInterestAnimationData != null && npcpointOfInterestAnimationData.WgoId == uniqueId)
			{
				this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is still referenced by npcLifeSimulatorData animations", wgoId, uniqueId));
				return;
			}
		}
		foreach (NPCLifeSimulatorActionData npclifeSimulatorActionData in npcLifeSimulatorData.ActionsData)
		{
			if (npclifeSimulatorActionData != null && npclifeSimulatorActionData.WgoId == uniqueId)
			{
				this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is still referenced by npcLifeSimulatorData actions", wgoId, uniqueId));
				break;
			}
		}
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x00090DB8 File Offset: 0x0008EFB8
	private void WarnIfZombieSystemReferences(SGuid uniqueId, string wgoId)
	{
		ZombieSystemData zombieSystemData = this.gameSave.zombieSystemData;
		List<SGuid> list = ((zombieSystemData != null) ? zombieSystemData.zombieOnSceneWgoIds : null);
		if (list == null)
		{
			return;
		}
		if (SaveFixContext.ContainsUid(list, uniqueId))
		{
			this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is still referenced by zombieSystemData.zombieOnSceneWgoIds", wgoId, uniqueId));
		}
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x00090DFC File Offset: 0x0008EFFC
	private void WarnIfConveyorSystemReferences(SGuid uniqueId, string wgoId)
	{
		ConveyorSystemData conveyorSystemData = this.gameSave.conveyorSystemData;
		if (conveyorSystemData == null)
		{
			return;
		}
		if (conveyorSystemData.conveyorComponents != null)
		{
			foreach (ConveyorComponent conveyorComponent in conveyorSystemData.conveyorComponents)
			{
				if (conveyorComponent != null && conveyorComponent.wgoDataUniqueId == uniqueId)
				{
					this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is still referenced by conveyorSystemData.conveyorComponents", wgoId, uniqueId));
					break;
				}
			}
		}
		if (conveyorSystemData.zombieCraftActivities == null)
		{
			return;
		}
		foreach (ZombieCraftActivity zombieCraftActivity in conveyorSystemData.zombieCraftActivities)
		{
			if (zombieCraftActivity != null && zombieCraftActivity.WgoUniqueId == uniqueId)
			{
				this.LogWarning(string.Format("Removed Wgo [{0}] [{1}] is used by a zombie craft activity in conveyorSystemData", wgoId, uniqueId));
				break;
			}
		}
	}

	// Token: 0x04001BB8 RID: 7096
	private readonly GameSave gameSave;

	// Token: 0x04001BB9 RID: 7097
	private readonly IList<GameSceneConfig> gameSceneConfigs;

	// Token: 0x04001BBA RID: 7098
	private readonly Dictionary<Guid, SaveFixContext.WgoIndexEntry> wgoByUid = new Dictionary<Guid, SaveFixContext.WgoIndexEntry>();

	// Token: 0x04001BBB RID: 7099
	private readonly Dictionary<string, SaveFixContext.LoadedContent> contentByGuid = new Dictionary<string, SaveFixContext.LoadedContent>();

	// Token: 0x04001BBC RID: 7100
	[TupleElementNames(new string[] { "config", "contentRef" })]
	private readonly List<ValueTuple<GameSceneConfig, AssetReference>> contentLoadedByFixer = new List<ValueTuple<GameSceneConfig, AssetReference>>();

	// Token: 0x02000480 RID: 1152
	private readonly struct WgoIndexEntry
	{
		// Token: 0x06001EA7 RID: 7847 RVA: 0x00090EF4 File Offset: 0x0008F0F4
		public WgoIndexEntry(GameSceneData sceneData, WgoData wgoData)
		{
			this.sceneData = sceneData;
			this.wgoData = wgoData;
		}

		// Token: 0x04001BBD RID: 7101
		public readonly GameSceneData sceneData;

		// Token: 0x04001BBE RID: 7102
		public readonly WgoData wgoData;
	}

	// Token: 0x02000481 RID: 1153
	private readonly struct LoadedContent
	{
		// Token: 0x06001EA8 RID: 7848 RVA: 0x00090F04 File Offset: 0x0008F104
		public LoadedContent(SceneWgoContentPart part, GameSceneData sceneData, GameSceneConfig config)
		{
			this.part = part;
			this.sceneData = sceneData;
			this.config = config;
		}

		// Token: 0x04001BBF RID: 7103
		public readonly SceneWgoContentPart part;

		// Token: 0x04001BC0 RID: 7104
		public readonly GameSceneData sceneData;

		// Token: 0x04001BC1 RID: 7105
		public readonly GameSceneConfig config;
	}
}
