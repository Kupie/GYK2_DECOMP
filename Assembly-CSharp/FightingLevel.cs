using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000318 RID: 792
public class FightingLevel : SceneWgoContentPart, IBakingContext
{
	// Token: 0x1700039C RID: 924
	// (get) Token: 0x0600151F RID: 5407 RVA: 0x0006735C File Offset: 0x0006555C
	public FightingLevelPreset FightingLevelPreset
	{
		get
		{
			return this.fightingLevelPreset;
		}
	}

	// Token: 0x1700039D RID: 925
	// (get) Token: 0x06001520 RID: 5408 RVA: 0x00067364 File Offset: 0x00065564
	public IReadOnlyList<BoxCollider> ZoneDefineColliders
	{
		get
		{
			return this.zoneDefineColliders;
		}
	}

	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06001521 RID: 5409 RVA: 0x0006736C File Offset: 0x0006556C
	public IReadOnlyDictionary<int, List<EnemySpawnZone>> SpawnZones
	{
		get
		{
			return this.spawnZones;
		}
	}

	// Token: 0x1700039F RID: 927
	// (get) Token: 0x06001522 RID: 5410 RVA: 0x00067374 File Offset: 0x00065574
	public LazyConsts.Navigation.Graph NavigationGraph
	{
		get
		{
			return this.navigationGraph;
		}
	}

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x06001523 RID: 5411 RVA: 0x0006737C File Offset: 0x0006557C
	public FightingCapturePoint BaseCapturePoint
	{
		get
		{
			return this.baseCapturePoint;
		}
	}

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06001524 RID: 5412 RVA: 0x00067384 File Offset: 0x00065584
	public string LevelGdPointId
	{
		get
		{
			return this.levelGdPointId;
		}
	}

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06001525 RID: 5413 RVA: 0x0006738C File Offset: 0x0006558C
	public string LevelWorldZoneId
	{
		get
		{
			return this.levelWorldZoneId;
		}
	}

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06001526 RID: 5414 RVA: 0x00067394 File Offset: 0x00065594
	public string FightbackGdPointId
	{
		get
		{
			return this.fightbackGdPointId;
		}
	}

	// Token: 0x170003A4 RID: 932
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x0006739C File Offset: 0x0006559C
	public string FightbackAfterWinGdPointId
	{
		get
		{
			return this.fightbackAfterWinGdPointId;
		}
	}

	// Token: 0x170003A5 RID: 933
	// (get) Token: 0x06001528 RID: 5416 RVA: 0x000673A4 File Offset: 0x000655A4
	public bool IsInsideIndoor
	{
		get
		{
			return this.isInsideIndoor;
		}
	}

	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x06001529 RID: 5417 RVA: 0x000673AC File Offset: 0x000655AC
	public bool IsInsideDungeon
	{
		get
		{
			return this.isInsideDungeon;
		}
	}

	// Token: 0x170003A7 RID: 935
	// (get) Token: 0x0600152A RID: 5418 RVA: 0x000673B4 File Offset: 0x000655B4
	public string EnvironmentPreset
	{
		get
		{
			if (this.isInsideIndoor)
			{
				return "indoor";
			}
			if (!this.isInsideDungeon)
			{
				return "outdoor";
			}
			return "dungeons";
		}
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000673D7 File Offset: 0x000655D7
	public GDPointData GetLevelGdPointData()
	{
		if (string.IsNullOrEmpty(this.levelGdPointId))
		{
			return null;
		}
		return MainGame.WorldData.gdPointsData.GetGDPointDataById(this.levelGdPointId);
	}

	// Token: 0x170003A8 RID: 936
	// (get) Token: 0x0600152C RID: 5420 RVA: 0x000673FD File Offset: 0x000655FD
	public IReadOnlyList<FightingLine> FightingLines
	{
		get
		{
			return this.fightingLines;
		}
	}

	// Token: 0x170003A9 RID: 937
	// (get) Token: 0x0600152D RID: 5421 RVA: 0x00067405 File Offset: 0x00065605
	public IReadOnlyList<FightingStage> NonPrefabStages
	{
		get
		{
			return this.nonPrefabStages;
		}
	}

	// Token: 0x170003AA RID: 938
	// (get) Token: 0x0600152E RID: 5422 RVA: 0x0006740D File Offset: 0x0006560D
	public IReadOnlyList<FightingLevel.FightinStageWorldObject> StageWorldObjects
	{
		get
		{
			return this.stageWorldObjects;
		}
	}

	// Token: 0x170003AB RID: 939
	// (get) Token: 0x0600152F RID: 5423 RVA: 0x00067415 File Offset: 0x00065615
	public IReadOnlyList<FightingLevel.FightingStageWorldZone> StageWorldZones
	{
		get
		{
			return this.stageWorldZones;
		}
	}

	// Token: 0x170003AC RID: 940
	// (get) Token: 0x06001530 RID: 5424 RVA: 0x0006741D File Offset: 0x0006561D
	public HashSet<ZombieFog> ZombieFogs
	{
		get
		{
			return this.zombieFogs;
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x06001531 RID: 5425 RVA: 0x00067428 File Offset: 0x00065628
	public Bounds LevelBounds
	{
		get
		{
			if (this.zoneDefineColliders.Count == 0)
			{
				return default(Bounds);
			}
			Bounds bounds = this.zoneDefineColliders[0].bounds;
			for (int i = 1; i < this.zoneDefineColliders.Count; i++)
			{
				bounds.Encapsulate(this.zoneDefineColliders[i].bounds);
			}
			return bounds;
		}
	}

	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06001532 RID: 5426 RVA: 0x0006748D File Offset: 0x0006568D
	public List<AlliesSpawn> AlliesSpawns
	{
		get
		{
			if (this.alliesSpawns == null || this.alliesSpawns.Count == 0)
			{
				this.alliesSpawns = base.GetComponentsInChildren<AlliesSpawn>().ToList<AlliesSpawn>();
			}
			return this.alliesSpawns;
		}
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x000674BC File Offset: 0x000656BC
	public FightingCapturePoint FindCapturePointForFlagStand(Vector3 worldPosition)
	{
		FightingCapturePoint fightingCapturePoint = null;
		float num = float.MaxValue;
		for (int i = 0; i < this.fightingLines.Count; i++)
		{
			FightingLine fightingLine = this.fightingLines[i];
			for (int j = 0; j < fightingLine.sectors.Count; j++)
			{
				FightingCapturePoint point = fightingLine.sectors[j].point;
				if (point && point.MatchesAllyFlagStandPosition(worldPosition))
				{
					float sqrMagnitude = (worldPosition - point.transform.position).XZ().sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						fightingCapturePoint = point;
					}
				}
			}
		}
		return fightingCapturePoint;
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x00067566 File Offset: 0x00065766
	public IEnumerator Init()
	{
		this.baseCapturePoint.Init(null);
		this.baseCapturePoint.SetOwnedByTeam(LazyConsts.Fighting.TeamType.Player);
		for (int i = 0; i < this.fightingLines.Count; i++)
		{
			FightingLine fightingLine = this.fightingLines[i];
			fightingLine.lineIdx = i;
			for (int j = 0; j < fightingLine.sectors.Count; j++)
			{
				fightingLine.sectors[j].sectorIdx = j;
			}
		}
		foreach (FightingLine fightingLine2 in this.fightingLines)
		{
			if (!this.pointByLineDict.ContainsKey(fightingLine2))
			{
				this.spawnZones.TryAdd(fightingLine2.lineIdx, fightingLine2.spawnZones);
				this.pointByLineDict.TryAdd(fightingLine2, this.spawnZones.Count - 1);
			}
		}
		if (!this.graphWasInitialized)
		{
			this.graphWasInitialized = true;
			yield return this.InitGraphAsync();
		}
		yield break;
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x00067578 File Offset: 0x00065778
	public void OnPlay()
	{
		if (LazySingleton<FightingGameController>.Instance.UIFightingOverlayData == null)
		{
			return;
		}
		foreach (FightingLine fightingLine in this.fightingLines)
		{
			foreach (FightingSector fightingSector in fightingLine.sectors)
			{
				if (fightingSector.point.OwnedByTeam == LazyConsts.Fighting.TeamType.Player)
				{
					LazySingleton<FightingGameController>.Instance.UIFightingOverlayData.TrackCapturePoint(fightingSector.point);
				}
			}
		}
		LazySingleton<FightingGameController>.Instance.UIFightingOverlayData.TrackCapturePoint(this.baseCapturePoint);
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x00067644 File Offset: 0x00065844
	public void OnStop()
	{
		if (LazySingleton<FightingGameController>.Instance.UIFightingOverlayData == null)
		{
			return;
		}
		foreach (FightingLine fightingLine in this.fightingLines)
		{
			foreach (FightingSector fightingSector in fightingLine.sectors)
			{
				LazySingleton<FightingGameController>.Instance.UIFightingOverlayData.UntrackCapturePoint(fightingSector.point);
			}
		}
		LazySingleton<FightingGameController>.Instance.UIFightingOverlayData.UntrackCapturePoint(this.baseCapturePoint);
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x00067700 File Offset: 0x00065900
	public void InitGraph()
	{
		Bounds levelBounds = this.LevelBounds;
		LazySingleton<GlobalNavigationManager>.Instance.InitRecastGraph(this.navigationGraph, levelBounds.center, levelBounds.size.XZ2(), true, levelBounds.size.y, null);
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x00067748 File Offset: 0x00065948
	public void SetSpawnerActive(int lineId, string spawnZoneName, bool isActive)
	{
		List<EnemySpawnZone> list;
		if (this.spawnZones.TryGetValue(lineId, out list))
		{
			EnemySpawnZone enemySpawnZone = list.Find((EnemySpawnZone z) => z.id == spawnZoneName);
			if (!enemySpawnZone)
			{
				Debug.LogWarning(string.Format("SpawnZone with id {0} on line {1} not found!", spawnZoneName, lineId));
				return;
			}
			enemySpawnZone.gameObject.SetActive(isActive);
		}
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x000677B8 File Offset: 0x000659B8
	public void StartLevel()
	{
		LazySingleton<FightingGameController>.Instance.Play(this.id);
		this.baseCapturePoint.OnCapturedByTeam += this.HandleCaptureBy;
		foreach (ZombieFog zombieFog in this.zombieFogs)
		{
			zombieFog.ResetActiveState();
		}
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x00067830 File Offset: 0x00065A30
	public void StopLevel()
	{
		this.baseCapturePoint.OnCapturedByTeam -= this.HandleCaptureBy;
		LazySingleton<FightingGameController>.Instance.Stop(false, false);
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x00067855 File Offset: 0x00065A55
	private void Awake()
	{
		this.zombieFogs = new HashSet<ZombieFog>();
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x00067862 File Offset: 0x00065A62
	private IEnumerator InitGraphAsync()
	{
		Bounds levelBounds = this.LevelBounds;
		yield return LazySingleton<GlobalNavigationManager>.Instance.InitRecastGraphAsync(this.navigationGraph, levelBounds.center, levelBounds.size.XZ2(), true, levelBounds.size.y, null);
		yield break;
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x00067874 File Offset: 0x00065A74
	public void SetActive(bool isActive)
	{
		foreach (ZombieFog zombieFog in this.zombieFogs)
		{
			zombieFog.ResetActiveState();
		}
		foreach (FightingLine fightingLine in this.fightingLines)
		{
			fightingLine.SetActive(isActive);
		}
		if (!this.hideCapturePoint)
		{
			this.baseCapturePoint.SetActiveState(isActive);
			return;
		}
		this.baseCapturePoint.SetActiveState(false);
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x00067928 File Offset: 0x00065B28
	public void InitLines()
	{
		foreach (FightingLine fightingLine in this.fightingLines)
		{
			fightingLine.Init();
		}
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x00067978 File Offset: 0x00065B78
	public void Deactivate()
	{
		this.baseCapturePoint.DeInit();
		this.baseCapturePoint.gameObject.SetActive(false);
		foreach (FightingLine fightingLine in this.fightingLines)
		{
			fightingLine.Deinit();
		}
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x000679E4 File Offset: 0x00065BE4
	public bool GetAvailablePositionForSpawning(int lineIndex, out Vector3 position, out List<PathfindingPenalty> penalties)
	{
		penalties = new List<PathfindingPenalty>();
		position = Vector3.zero;
		if (this.spawnZones.Count == 0 || lineIndex < 0 || lineIndex >= this.spawnZones.Count)
		{
			return false;
		}
		IEnumerable<EnemySpawnZone> enumerable = this.spawnZones[lineIndex].Where((EnemySpawnZone zone) => zone.gameObject.activeSelf);
		if (!enumerable.Any<EnemySpawnZone>())
		{
			return false;
		}
		position = enumerable.ToList<EnemySpawnZone>().GetRandom<EnemySpawnZone>().GetRandomPosFromZone(out penalties);
		return true;
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x00067A77 File Offset: 0x00065C77
	public FightingLine GetFightingLine(int lineIndex)
	{
		if (lineIndex >= 0 && lineIndex < this.fightingLines.Count)
		{
			return this.fightingLines[lineIndex];
		}
		return null;
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x00067A9C File Offset: 0x00065C9C
	public void SetBorderObjectsActive(bool isActive)
	{
		foreach (GameObject gameObject in this.borderObjects)
		{
			if (gameObject)
			{
				gameObject.SetActive(isActive);
			}
		}
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x00067AF8 File Offset: 0x00065CF8
	private void HandleCaptureBy(FightingCapturePoint capturePoint)
	{
		if (capturePoint.OwnedByTeam == LazyConsts.Fighting.TeamType.WildZombie)
		{
			LazySingleton<FightingGameController>.Instance.FinishAsLost();
		}
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x00067B10 File Offset: 0x00065D10
	public void SpawnAllies()
	{
		List<WgoData> list = new List<WgoData>();
		foreach (SGuid sguid in MainGame.Instance.GameSave.militaryBaseData.fighterContainersSelectedForFight)
		{
			list.Add(MainGame.WorldData.GetWgoData(sguid));
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.AlliesSpawns[i].SpawnFromContainer(list[i]);
		}
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x00067BAC File Offset: 0x00065DAC
	public void DeSpawnAllies()
	{
		foreach (AlliesSpawn alliesSpawn in this.AlliesSpawns)
		{
			alliesSpawn.Clear();
		}
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x00067BFC File Offset: 0x00065DFC
	public void SetEnabledFlagControllers(bool isEnabled)
	{
		foreach (AlliesSpawn alliesSpawn in this.AlliesSpawns)
		{
			if (!(alliesSpawn.FlagController == null))
			{
				alliesSpawn.FlagController.SetEnabled(isEnabled);
			}
		}
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x00067C64 File Offset: 0x00065E64
	public void ClearFlagStands()
	{
		foreach (FightingLevel.FightinStageWorldObject fightinStageWorldObject in this.stageWorldObjects)
		{
			if (fightinStageWorldObject.type == FightingLevel.FightinStageWorldObject.ObjType.Wgo)
			{
				Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(fightinStageWorldObject.uniqueId);
				if (wgoViewGlobal)
				{
					FlagStandComponent componentInChildren = wgoViewGlobal.GetComponentInChildren<FlagStandComponent>(true);
					if (componentInChildren)
					{
						componentInChildren.DetachFlag();
					}
					wgoViewGlobal.Data.GameResStr.Set("flag_stand_sguid", string.Empty);
				}
			}
		}
		foreach (FlagStandComponent flagStandComponent in LazySingleton<FightingGameController>.Instance.FlagStandComponents)
		{
			flagStandComponent.DetachFlag();
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x00067D40 File Offset: 0x00065F40
	public bool IsValid()
	{
		GameSave gameSave = MainGame.Instance.GameSave;
		if (string.IsNullOrEmpty(this.levelWorldZoneId) || gameSave.worldData.GetWorldZoneDataById(this.levelWorldZoneId) == null)
		{
			Debug.LogError("WorldZoneId is not set for fighting level: " + this.id);
			return false;
		}
		int num = gameSave.militaryBaseData.fighterContainers.Count;
		if (gameSave.militaryBaseData.IsMercenaryPayed)
		{
			num++;
		}
		if (this.alliesSpawns == null || this.alliesSpawns.Count < num)
		{
			Debug.LogError("AlliesSpawns count is not set for fighting level: " + this.id);
			return false;
		}
		if (string.IsNullOrEmpty(this.levelGdPointId) || gameSave.WorldData.gdPointsData.GetGDPointDataById(this.levelGdPointId) == null)
		{
			Debug.LogError("GdPointId is not set for fighting level: " + this.id);
			return false;
		}
		return true;
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x00067E1C File Offset: 0x0006601C
	public void ApplyStageId(int stageId)
	{
		if (string.IsNullOrEmpty(base.WorldId))
		{
			Debug.LogWarning("WorldId is not set for fighting level: " + this.id);
			return;
		}
		this.currentStageId = stageId;
		GameSceneData gameSceneDataById = MainGame.WorldData.GetGameSceneDataById(base.WorldId);
		if (gameSceneDataById == null)
		{
			Debug.LogWarning("GameSceneData is not found for fighting level: " + this.id);
			return;
		}
		gameSceneDataById.ApplyStageForFightingLevel(this.id, stageId);
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x00067E8C File Offset: 0x0006608C
	public void ApplyStageIdFromData(int stageId)
	{
		foreach (FightingLevel.SerializedStagePrefabRef serializedStagePrefabRef in this.stagePrefabsRefs)
		{
			serializedStagePrefabRef.ApplyByStageId(stageId, base.transform);
		}
		foreach (FightingStage fightingStage in this.nonPrefabStages)
		{
			fightingStage.ApplyStage(stageId);
		}
		foreach (FightingLevel.FightinStageWorldObject fightinStageWorldObject in this.stageWorldObjects)
		{
			bool flag = stageId != 0 && (fightinStageWorldObject.stageMask & (1 << stageId)) != 0;
			FightingLevel.FightinStageWorldObject.ObjType type = fightinStageWorldObject.type;
			if (type != FightingLevel.FightinStageWorldObject.ObjType.Wgo)
			{
				if (type == FightingLevel.FightinStageWorldObject.ObjType.Wso)
				{
					WsoData wsoData = MainGame.WorldData.GetWsoData(fightinStageWorldObject.uniqueId);
					if (wsoData != null)
					{
						wsoData.IsHidden = !flag;
					}
				}
			}
			else
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(fightinStageWorldObject.uniqueId);
				if (wgoData != null)
				{
					wgoData.IsHidden = !flag;
				}
			}
		}
		foreach (FightingLevel.FightingStageWorldZone fightingStageWorldZone in this.stageWorldZones)
		{
			bool flag2 = stageId != 0 && (fightingStageWorldZone.stageMask & (1 << stageId)) != 0;
			WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById(fightingStageWorldZone.worldZoneId);
			if (worldZoneDataById != null)
			{
				worldZoneDataById.IsActive = flag2;
			}
			WorldZone worldZoneById = MainGame.Instance.fightingLevelSystem.GetWorldZoneById(fightingStageWorldZone.worldZoneId);
			if (worldZoneById != null)
			{
				worldZoneById.gameObject.SetActive(flag2);
			}
		}
		Debug.Log(string.Format("Apply stage id: {0} to fighting level: {1}", stageId, this.id));
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00068094 File Offset: 0x00066294
	public GameObject GetStageInstanceFromAssetReference(AssetReferenceGameObject assetReference)
	{
		foreach (FightingLevel.SerializedStagePrefabRef serializedStagePrefabRef in this.stagePrefabsRefs)
		{
			if (serializedStagePrefabRef.fightingStagePrefabRef.AssetGUID == assetReference.AssetGUID)
			{
				return serializedStagePrefabRef.fightingStagePrefab;
			}
		}
		return null;
	}

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x0600154C RID: 5452 RVA: 0x00068104 File Offset: 0x00066304
	public List<BakedChunkableObjectComponentData> GetBakedData
	{
		get
		{
			return this.bakedData;
		}
	}

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x0600154D RID: 5453 RVA: 0x00028294 File Offset: 0x00026494
	public int Editor_BakingContextPriority
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x0006810C File Offset: 0x0006630C
	public void Editor_SetContextEnableState(bool isActive)
	{
		this.disabledBakingContext = !isActive;
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x00068118 File Offset: 0x00066318
	public void CollectNonStageChunkableObjects()
	{
		this.nonStageChunkableObjects.Clear();
		foreach (ChunkableObjectComponent chunkableObjectComponent in base.GetComponentsInChildren<ChunkableObjectComponent>(true))
		{
			if (!(chunkableObjectComponent == null) && !(chunkableObjectComponent.GetComponentInParent<FightingStage>(true) != null) && !(chunkableObjectComponent.GetComponentInParent<Wgo>(true) != null) && !(chunkableObjectComponent.GetComponentInParent<Wso>(true) != null))
			{
				this.nonStageChunkableObjects.Add(chunkableObjectComponent);
			}
		}
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x0006818C File Offset: 0x0006638C
	private void OnEnable()
	{
		if (this.disabledBakingContext)
		{
			return;
		}
		BakingContextRuntimeRegistration.Register(this, this.registeredBakedChunkCaches);
		this.RegisterLiveChunkableObjects();
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x000681A9 File Offset: 0x000663A9
	private void OnDisable()
	{
		this.ForceUnregisterBakingContext();
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x000681B1 File Offset: 0x000663B1
	public void ForceUnregisterBakingContext()
	{
		if (this.disabledBakingContext)
		{
			return;
		}
		BakingContextRuntimeRegistration.Unregister(this.registeredBakedChunkCaches);
		this.UnregisterLiveChunkableObjects();
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x000681D0 File Offset: 0x000663D0
	private void RegisterLiveChunkableObjects()
	{
		this.UnregisterLiveChunkableObjects();
		if (this.nonStageChunkableObjects.Count == 0)
		{
			this.CollectNonStageChunkableObjects();
		}
		for (int i = 0; i < this.nonStageChunkableObjects.Count; i++)
		{
			ChunkableObjectComponent chunkableObjectComponent = this.nonStageChunkableObjects[i];
			if (!(chunkableObjectComponent == null))
			{
				chunkableObjectComponent.UpdateChunkVisibility(false);
				this.registeredLiveChunkables.Add(chunkableObjectComponent);
			}
		}
		if (this.registeredLiveChunkables.Count == 0)
		{
			return;
		}
		ChunkManager instance = LazySingleton<ChunkManager>.Instance;
		if (instance == null)
		{
			return;
		}
		instance.RegisterChunks<ChunkableObjectComponent>(this.registeredLiveChunkables, ChunkManagerLayerType.FightingLevelStaticObjects);
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x00068260 File Offset: 0x00066460
	private void UnregisterLiveChunkableObjects()
	{
		if (this.registeredLiveChunkables.Count == 0)
		{
			return;
		}
		ChunkManager instance = LazySingleton<ChunkManager>.Instance;
		if (instance != null)
		{
			instance.UnregisterChunks<ChunkableObjectComponent>(this.registeredLiveChunkables, ChunkManagerLayerType.FightingLevelStaticObjects);
		}
		for (int i = 0; i < this.registeredLiveChunkables.Count; i++)
		{
			ChunkableObjectComponent chunkableObjectComponent = this.registeredLiveChunkables[i];
			if (!(chunkableObjectComponent == null))
			{
				chunkableObjectComponent.UpdateChunkVisibility(false);
			}
		}
		this.registeredLiveChunkables.Clear();
	}

	// Token: 0x040015D0 RID: 5584
	public string id;

	// Token: 0x040015D1 RID: 5585
	[SerializeField]
	private FightingLevelPreset fightingLevelPreset;

	// Token: 0x040015D2 RID: 5586
	[SerializeField]
	private List<FightingLine> fightingLines = new List<FightingLine>();

	// Token: 0x040015D3 RID: 5587
	[SerializeField]
	private LazyConsts.Navigation.Graph navigationGraph = LazyConsts.Navigation.Graph.None;

	// Token: 0x040015D4 RID: 5588
	[SerializeField]
	private List<BoxCollider> zoneDefineColliders = new List<BoxCollider>();

	// Token: 0x040015D5 RID: 5589
	[SerializeField]
	private List<GameObject> borderObjects = new List<GameObject>();

	// Token: 0x040015D6 RID: 5590
	[SerializeField]
	private string levelGdPointId;

	// Token: 0x040015D7 RID: 5591
	[SerializeField]
	private string levelWorldZoneId;

	// Token: 0x040015D8 RID: 5592
	[SerializeField]
	private string fightbackGdPointId;

	// Token: 0x040015D9 RID: 5593
	[SerializeField]
	private string fightbackAfterWinGdPointId;

	// Token: 0x040015DA RID: 5594
	[SerializeField]
	private bool isInsideIndoor;

	// Token: 0x040015DB RID: 5595
	[SerializeField]
	private bool isInsideDungeon;

	// Token: 0x040015DC RID: 5596
	[Space]
	[SerializeField]
	private List<AlliesSpawn> alliesSpawns = new List<AlliesSpawn>();

	// Token: 0x040015DD RID: 5597
	[Space]
	public List<EnemySpawnZone> customSpawnZones = new List<EnemySpawnZone>();

	// Token: 0x040015DE RID: 5598
	[SerializeField]
	private FightingCapturePoint baseCapturePoint;

	// Token: 0x040015DF RID: 5599
	[SerializeField]
	private bool hideCapturePoint;

	// Token: 0x040015E0 RID: 5600
	public bool graphWasInitialized;

	// Token: 0x040015E1 RID: 5601
	private int currentStageId;

	// Token: 0x040015E2 RID: 5602
	private Dictionary<int, List<EnemySpawnZone>> spawnZones = new Dictionary<int, List<EnemySpawnZone>>();

	// Token: 0x040015E3 RID: 5603
	private Dictionary<FightingLine, int> pointByLineDict = new Dictionary<FightingLine, int>();

	// Token: 0x040015E4 RID: 5604
	private HashSet<ZombieFog> zombieFogs;

	// Token: 0x040015E5 RID: 5605
	[SerializeField]
	private List<FightingLevel.SerializedStagePrefabRef> stagePrefabsRefs = new List<FightingLevel.SerializedStagePrefabRef>();

	// Token: 0x040015E6 RID: 5606
	[SerializeField]
	private List<FightingStage> nonPrefabStages = new List<FightingStage>();

	// Token: 0x040015E7 RID: 5607
	[SerializeField]
	private List<FightingLevel.FightinStageWorldObject> stageWorldObjects = new List<FightingLevel.FightinStageWorldObject>();

	// Token: 0x040015E8 RID: 5608
	[SerializeField]
	private List<FightingLevel.FightingStageWorldZone> stageWorldZones = new List<FightingLevel.FightingStageWorldZone>();

	// Token: 0x040015E9 RID: 5609
	[SerializeField]
	private List<ChunkableObjectComponent> nonStageChunkableObjects = new List<ChunkableObjectComponent>();

	// Token: 0x040015EA RID: 5610
	[SerializeField]
	private List<BakedChunkableObjectComponentData> bakedData = new List<BakedChunkableObjectComponentData>();

	// Token: 0x040015EB RID: 5611
	[SerializeField]
	private bool disabledBakingContext;

	// Token: 0x040015EC RID: 5612
	[NonSerialized]
	private readonly List<BakedChunkableObjectComponentData> registeredBakedChunkCaches = new List<BakedChunkableObjectComponentData>();

	// Token: 0x040015ED RID: 5613
	[NonSerialized]
	private readonly List<ChunkableObjectComponent> registeredLiveChunkables = new List<ChunkableObjectComponent>();

	// Token: 0x02000319 RID: 793
	[Serializable]
	private class SerializedStagePrefabRef
	{
		// Token: 0x06001556 RID: 5462 RVA: 0x00068398 File Offset: 0x00066598
		public void ApplyByStageId(int stageId, Transform parent)
		{
			if (stageId == 0 || (this.stageMask & (1 << stageId)) == 0)
			{
				if (this.fightingStagePrefab && this.prefabHandle.IsValid())
				{
					global::UnityEngine.Object.Destroy(this.fightingStagePrefab);
					this.prefabHandle.Release();
					this.fightingStagePrefab = null;
				}
				return;
			}
			if (!this.fightingStagePrefab || !this.prefabHandle.IsValid())
			{
				this.prefabHandle = this.fightingStagePrefabRef.InstantiateAsync(parent, false);
				this.fightingStagePrefab = this.prefabHandle.WaitForCompletion();
			}
			FightingLevel.SerializedStagePrefabRef.ApplyChunkableVisibilityForInstantiatedStage(this.fightingStagePrefab, stageId);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x0006843C File Offset: 0x0006663C
		private static void ApplyChunkableVisibilityForInstantiatedStage(GameObject stageRoot, int stageId)
		{
			if (!stageRoot)
			{
				return;
			}
			FightingStage fightingStage = stageRoot.GetComponent<FightingStage>();
			if (!fightingStage)
			{
				fightingStage = stageRoot.GetComponentInChildren<FightingStage>(true);
			}
			if (fightingStage != null)
			{
				fightingStage.ApplyStageFromRuntimeInstance(stageId);
				return;
			}
			ChunkableObjectComponent[] componentsInChildren = stageRoot.GetComponentsInChildren<ChunkableObjectComponent>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].CustomVisibilityDisabled = false;
			}
		}

		// Token: 0x040015EE RID: 5614
		public AssetReferenceGameObject fightingStagePrefabRef;

		// Token: 0x040015EF RID: 5615
		public int stageMask;

		// Token: 0x040015F0 RID: 5616
		[NonSerialized]
		public AsyncOperationHandle<GameObject> prefabHandle;

		// Token: 0x040015F1 RID: 5617
		[NonSerialized]
		public GameObject fightingStagePrefab;
	}

	// Token: 0x0200031A RID: 794
	[Serializable]
	public class FightinStageWorldObject
	{
		// Token: 0x040015F2 RID: 5618
		public SGuid uniqueId;

		// Token: 0x040015F3 RID: 5619
		public FightingLevel.FightinStageWorldObject.ObjType type;

		// Token: 0x040015F4 RID: 5620
		public int stageMask;

		// Token: 0x0200031B RID: 795
		public enum ObjType
		{
			// Token: 0x040015F6 RID: 5622
			Wgo,
			// Token: 0x040015F7 RID: 5623
			Wso
		}
	}

	// Token: 0x0200031C RID: 796
	[Serializable]
	public class FightingStageWorldZone
	{
		// Token: 0x040015F8 RID: 5624
		public string worldZoneId;

		// Token: 0x040015F9 RID: 5625
		public int stageMask;
	}
}
