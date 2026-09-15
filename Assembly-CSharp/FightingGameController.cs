using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using Pathfinding;
using Pathfinding.RVO;
using UnityEngine;

// Token: 0x020002F3 RID: 755
public class FightingGameController : LazySingleton<FightingGameController>
{
	// Token: 0x1400001F RID: 31
	// (add) Token: 0x060013FD RID: 5117 RVA: 0x00061E8C File Offset: 0x0006008C
	// (remove) Token: 0x060013FE RID: 5118 RVA: 0x00061EC4 File Offset: 0x000600C4
	public event Action<FightState> OnFightStateChanged;

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x060013FF RID: 5119 RVA: 0x00061EF9 File Offset: 0x000600F9
	public FightingTargetsDatabase TargetsDatabase { get; } = new FightingTargetsDatabase();

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001400 RID: 5120 RVA: 0x00061F01 File Offset: 0x00060101
	public FightingLevelPreset CurrentLevelPreset
	{
		get
		{
			FightingLevel fightingLevel = this.currentLevel;
			if (fightingLevel == null)
			{
				return null;
			}
			return fightingLevel.FightingLevelPreset;
		}
	}

	// Token: 0x1700036E RID: 878
	// (get) Token: 0x06001401 RID: 5121 RVA: 0x00061F14 File Offset: 0x00060114
	public FightState CurrentFightState
	{
		get
		{
			return this.fightState;
		}
	}

	// Token: 0x1700036F RID: 879
	// (get) Token: 0x06001402 RID: 5122 RVA: 0x00061F1C File Offset: 0x0006011C
	public AgentsGroupBehaviourController BaseDefenseAgentsController
	{
		get
		{
			return this.baseDefenseAgentsController;
		}
	}

	// Token: 0x17000370 RID: 880
	// (get) Token: 0x06001403 RID: 5123 RVA: 0x00061F24 File Offset: 0x00060124
	// (set) Token: 0x06001404 RID: 5124 RVA: 0x00061F2C File Offset: 0x0006012C
	public HashSet<IChunkableObject> AllStaticObjectsInZone { get; private set; }

	// Token: 0x17000371 RID: 881
	// (get) Token: 0x06001405 RID: 5125 RVA: 0x00061F35 File Offset: 0x00060135
	// (set) Token: 0x06001406 RID: 5126 RVA: 0x00061F3D File Offset: 0x0006013D
	public HashSet<IChunkableObject> AllDynamicObjectsInZone { get; private set; }

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001407 RID: 5127 RVA: 0x00061F46 File Offset: 0x00060146
	public string CurrentLevelId
	{
		get
		{
			return this.currentLevel.id;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x06001408 RID: 5128 RVA: 0x00061F53 File Offset: 0x00060153
	public FightingLevel CurrentLevel
	{
		get
		{
			return this.currentLevel;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06001409 RID: 5129 RVA: 0x00061F5B File Offset: 0x0006015B
	public HashSet<FightingLine> BreachedLines { get; } = new HashSet<FightingLine>();

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x0600140A RID: 5130 RVA: 0x00061F63 File Offset: 0x00060163
	// (set) Token: 0x0600140B RID: 5131 RVA: 0x00061F6B File Offset: 0x0006016B
	private List<ICombatEntity> CustomDecoyTargets { get; set; } = new List<ICombatEntity>();

	// Token: 0x17000376 RID: 886
	// (get) Token: 0x0600140C RID: 5132 RVA: 0x00061F74 File Offset: 0x00060174
	// (set) Token: 0x0600140D RID: 5133 RVA: 0x00061F7C File Offset: 0x0006017C
	public HashSet<FlagStandComponent> FlagStandComponents { get; set; } = new HashSet<FlagStandComponent>();

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x0600140E RID: 5134 RVA: 0x00061F85 File Offset: 0x00060185
	public bool IsPaused
	{
		get
		{
			return this.isPaused;
		}
	}

	// Token: 0x17000378 RID: 888
	// (get) Token: 0x0600140F RID: 5135 RVA: 0x00061F8D File Offset: 0x0006018D
	public FightEffectsManager FightEffectsManager
	{
		get
		{
			return this.fightEffectsManager;
		}
	}

	// Token: 0x17000379 RID: 889
	// (get) Token: 0x06001410 RID: 5136 RVA: 0x00061F95 File Offset: 0x00060195
	public UIFightingOverlayData UIFightingOverlayData
	{
		get
		{
			return this.uiFightingOverlayData;
		}
	}

	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06001411 RID: 5137 RVA: 0x00061F9D File Offset: 0x0006019D
	public bool IsClearingFightEnvironment
	{
		get
		{
			return this.isClearingFightEnvironment;
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001412 RID: 5138 RVA: 0x00061FA5 File Offset: 0x000601A5
	public RecastGraph RecastGraph
	{
		get
		{
			if (this.recastGraph == null)
			{
				this.recastGraph = AstarPath.active.graphs[12] as RecastGraph;
			}
			return this.recastGraph;
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x00061FD0 File Offset: 0x000601D0
	public void Play()
	{
		if (this.fightState == FightState.ActiveFight)
		{
			this.Stop(false, false);
		}
		if (this.currentLevel == null)
		{
			return;
		}
		SpearDestinationModifier.ClearAttachedEntities();
		this.SetFightState(FightState.ActiveFight);
		this.lastPlayedLevelId = this.currentLevel.id;
		this.gameLoopCoroutine = base.StartCoroutine(this.GameLoopCoroutine());
		this.lastPlayedFightPlaylist = (this.currentLevel.IsInsideDungeon ? "sewer_fight" : "fight");
		LazyAudio.StopPlaylist("gameplay");
		LazyAudio.PlayPlaylist(this.lastPlayedFightPlaylist);
		Debug.Log(string.Format("{0}.Play: {1}, {2} lines.", "FightingGameController", this.CurrentLevelPreset.name, this.CurrentLevelPreset.lines.Count));
		MainGame.PlayerController.PhysicalBody.PlayerView.PlayerAnimation.SetCustomDeathAnimationFinishedCallback(null);
		this.wasFinishedOnce = false;
		MainGame.PlayerController.View.PlayerAnimation.SetCustomDeathAnimationFinishedCallback(new Action(this.OnPlayerDeathAnimationFinished));
		MainGame.PlayerController.View.UpdateStaminaBarState();
		this.uiFightingOverlayData = new UIFightingOverlayData(this);
		LazyUI.GetElement<UIFightingOverlay>().Draw(this.uiFightingOverlayData);
		this.currentLevel.OnPlay();
		this.currentLevel.ApplyStageId(4);
		LazyAudio.PlayAndForget("fight_start");
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x00062122 File Offset: 0x00060322
	public void Play(string id)
	{
		this.StartPreFight(id, null);
		this.Play();
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x00062134 File Offset: 0x00060334
	public void Stop(bool hasCustomAfterFightPos = false, bool stopAsWon = false)
	{
		if (this.customFinishCallback != null)
		{
			Action action = this.customFinishCallback;
			if (action != null)
			{
				action();
			}
			this.customFinishCallback = null;
		}
		MainGame.PlayerController.View.PlayerAnimation.SetCustomDeathAnimationFinishedCallback(null);
		SpearDestinationModifier.ClearAttachedEntities();
		this.SetFightState(FightState.Disabled);
		HUD hud = LazyUI.Get<HUD>();
		if (hud.Mode == HUDMode.Fight)
		{
			hud.HideFightingTimeline();
			hud.SetMode(HUDMode.Common);
		}
		this.presetProcessor.StopPreset();
		this.presetProcessor.OnEnemiesSpawn -= this.HandleEnemiesSpawn;
		this.presetProcessor.OnPresetFinished -= this.OnPresetFinished;
		if (this.gameLoopCoroutine != null)
		{
			base.StopCoroutine(this.gameLoopCoroutine);
			this.gameLoopCoroutine = null;
		}
		if (this.spawnQueueCoroutine != null)
		{
			base.StopCoroutine(this.spawnQueueCoroutine);
			this.spawnQueueCoroutine = null;
		}
		if (this.victoryEnemyKillCoroutine != null)
		{
			base.StopCoroutine(this.victoryEnemyKillCoroutine);
			this.victoryEnemyKillCoroutine = null;
		}
		this.spawnQueue.Clear();
		if (!this.wasFinishedOnce)
		{
			this.pendingChainedFightRewards.Clear();
		}
		this.ClearDebugDummyAgents();
		this.TargetsDatabase.PruneDeadTargets(new Action<ICombatEntity>(this.TryRemoveChunkableObjectFromIgnore));
		using (List<TargetInfo>.Enumerator enumerator = new List<TargetInfo>(this.TargetsDatabase.AllTargets).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TargetInfo targetInfo = enumerator.Current;
				if (targetInfo != null && !targetInfo.IsPersistent)
				{
					if (!FightingTargetsDatabase.IsCombatEntityAlive(targetInfo.entity))
					{
						this.TargetsDatabase.RemoveEntry(targetInfo);
					}
					else
					{
						if (MainGame.PlayerData.Guid.Guid != targetInfo.entity.CombatEntityUID.Guid && MainGame.Instance.GameSave.militaryBaseData.fighters.Find((MilitaryBaseData.MilitaryBaseFighter x) => x.uniqueId.Guid == targetInfo.entity.CombatEntityUID.Guid) == null)
						{
							MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(targetInfo.entity.CombatEntityUID);
						}
						Wgo wgo = targetInfo.entity as Wgo;
						if (wgo != null)
						{
							FightingGameController.CleanupCustomDecoyComponents(wgo);
						}
						this.UnregisterTarget(targetInfo.entity);
					}
				}
			}
		}
		this.TargetsDatabase.Clear();
		FightingLevel fightingLevel = this.currentLevel;
		string text = ((fightingLevel != null) ? fightingLevel.id : null);
		if (this.currentLevel)
		{
			this.currentLevel.graphWasInitialized = false;
			this.currentLevel.SetActive(false);
			this.currentLevel.SetBorderObjectsActive(false);
			this.currentLevel.Deactivate();
			this.currentLevel.SetEnabledFlagControllers(false);
			this.currentLevel.ClearFlagStands();
			foreach (AgentsGroupFlagController agentsGroupFlagController in this.customFlagControllers)
			{
				agentsGroupFlagController.DeInit();
				agentsGroupFlagController.SetEnabled(false);
			}
			this.customFlagControllers.Clear();
			this.ClearEnvironment();
		}
		this.ResetNotIgnoredStateForDynamicChunkableObjects();
		this.DeactivateAllies();
		this.baseDefenseAgentsController.DeInit();
		this.groupViewController.DespawnAllZombies();
		FightingLevel fightingLevel2 = this.currentLevel;
		if (fightingLevel2 != null)
		{
			fightingLevel2.OnStop();
		}
		this.SetPlayerDefault(hasCustomAfterFightPos, stopAsWon);
		this.currentLevel = null;
		this.ClearTemporaryWgos();
		this.DestroyFlagStandComponents();
		LazyAudio.StopPlaylist(this.lastPlayedFightPlaylist);
		LazyAudio.PlayPlaylist("gameplay");
		Debug.Log("FightingGameController.Stop");
		if (this.wasLevelLoaded && !string.IsNullOrEmpty(text))
		{
			GameSceneData gameSceneData;
			GameSceneConfig gameSceneConfig;
			if (MainGame.WorldData.TryGetGameSceneDataForContent(text, out gameSceneData, out gameSceneConfig))
			{
				gameSceneData.RemoveFightingLevelData(text);
			}
			this.wasLevelLoaded = false;
		}
		MainGame.PlayerData.hpComponent.RestoreFullHp();
		MainGame.PlayerController.PhysicalBody.PlayerView.PlayerAnimation.SetCustomDeathAnimationFinishedCallback(null);
		MainGame.PlayerController.View.UpdateStaminaBarState();
		BuildController instance = BuildController.Instance;
		UIBuildingWindow window = LazyUI.GetWindow<UIBuildingWindow>();
		if (instance.IsBuildModeActive)
		{
			instance.DisableBuildMode();
		}
		if (window.IsShown)
		{
			window.Close();
		}
		if (this.uiFightingOverlayData != null)
		{
			LazyUI.GetWindow<UIFightingOverlay>().Hide();
			this.uiFightingOverlayData = null;
		}
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x000625AC File Offset: 0x000607AC
	public void SetPauseState(bool isPaused)
	{
		this.isPaused = isPaused;
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000625B8 File Offset: 0x000607B8
	public void AddWgoAsCustomDecoy(Wgo wgo, bool setAsAgent = false, LazyConsts.Fighting.TargetAttackPriority priority = LazyConsts.Fighting.TargetAttackPriority.High)
	{
		wgo.IsActiveCombatant = true;
		TargetInfo targetInfo = this.TargetsDatabase.GetTargetInfo(wgo);
		DecoyComponent decoyComponent = null;
		if (targetInfo != null)
		{
			global::UnityEngine.Object @object = targetInfo.entity as global::UnityEngine.Object;
			if (@object != null)
			{
				MonoBehaviour monoBehaviour = @object as MonoBehaviour;
				if (monoBehaviour != null)
				{
					decoyComponent = monoBehaviour.GetComponentInChildren<DecoyComponent>();
					if (!decoyComponent)
					{
						decoyComponent = wgo.MainWgoPart.gameObject.AddComponent<DecoyComponent>();
						decoyComponent.Initialize(wgo);
					}
				}
			}
		}
		else
		{
			decoyComponent = wgo.MainWgoPart.GetComponentInChildren<DecoyComponent>();
			if (!decoyComponent)
			{
				decoyComponent = wgo.MainWgoPart.gameObject.AddComponent<DecoyComponent>();
				decoyComponent.Initialize(wgo);
			}
			if (wgo.MainWgoPart.GetComponentInChildren<AnimationComponentBase>() && setAsAgent)
			{
				this.baseDefenseAgentsController.AddWgoAsAgent(wgo, null, true);
			}
			int num = -1;
			int num2 = -1;
			int num3 = Physics.OverlapBoxNonAlloc(decoyComponent.transform.position, Vector3.one, this.overlapResults);
			if (num3 > 0)
			{
				for (int i = 0; i < num3; i++)
				{
					Collider collider = this.overlapResults[i];
					FightingSector fightingSector;
					if (!(collider == null) && collider.TryGetComponent<FightingSector>(out fightingSector))
					{
						num = fightingSector.fightingLine.lineIdx;
						num2 = fightingSector.sectorIdx;
						break;
					}
				}
			}
			NavmeshCut navmeshCut = wgo.gameObject.AddComponent<NavmeshCut>();
			navmeshCut.type = NavmeshCut.MeshType.Capsule;
			navmeshCut.radiusExpansionMode = NavmeshCut.RadiusExpansionMode.DontExpand;
			navmeshCut.center = Vector3.zero;
			navmeshCut.circleRadius = 0.25f;
			wgo.AttackPriority = (int)priority;
			this.RegisterTargetNonPersistent(wgo, num, num2, true);
		}
		if (wgo.Data.HpComponent.MaxHpValue == 0)
		{
			wgo.Data.HpComponent.SetCustomHpValue(10, true);
		}
		this.TryForceRetargetAgentsByAddedDecoy(decoyComponent);
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x0006275A File Offset: 0x0006095A
	public void RemoveWgFromCustomDecoy(Wgo wgo)
	{
		this.TryForceRetargetAgentsByRemovedDecoy(wgo);
		this.CustomDecoyTargets.Remove(wgo);
		FightingGameController.CleanupCustomDecoyComponents(wgo);
		this.UnregisterTarget(wgo);
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x00062780 File Offset: 0x00060980
	private static void CleanupCustomDecoyComponents(Wgo wgo)
	{
		if (!wgo)
		{
			return;
		}
		if (wgo.MainWgoPart)
		{
			DecoyComponent componentInChildren = wgo.MainWgoPart.GetComponentInChildren<DecoyComponent>();
			if (componentInChildren)
			{
				global::UnityEngine.Object.Destroy(componentInChildren);
			}
		}
		NavmeshCut component = wgo.GetComponent<NavmeshCut>();
		if (component && FightingGameController.IsRuntimeCustomDecoyNavmeshCut(component))
		{
			global::UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x000627DA File Offset: 0x000609DA
	private static bool IsRuntimeCustomDecoyNavmeshCut(NavmeshCut navmeshCut)
	{
		return navmeshCut.type == NavmeshCut.MeshType.Capsule && navmeshCut.radiusExpansionMode == NavmeshCut.RadiusExpansionMode.DontExpand && navmeshCut.circleRadius == 0.25f;
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x000627FC File Offset: 0x000609FC
	public void SpawnDebugDummyAgent()
	{
		if (!Application.isPlaying)
		{
			Debug.LogWarning("Cannot spawn debug dummy agent outside of play mode.", this);
			return;
		}
		if (!this.currentLevel)
		{
			Debug.LogWarning("Cannot spawn debug dummy agent without an active fighting level.", this);
			return;
		}
		this.debugDummyStartHp = Mathf.Clamp(this.debugDummyStartHp, 0, this.debugDummyMaxHp);
		this.debugDummyQuality = Mathf.Max(0, this.debugDummyQuality);
		Vector3 debugDummySpawnPosition = this.GetDebugDummySpawnPosition();
		DebugDummyAgent debugDummyAgent = this.CreateDebugDummyAgent(debugDummySpawnPosition);
		debugDummyAgent.AssignOwner(this);
		debugDummyAgent.Configure(this.debugDummyTeam, this.debugDummyMaxHp, this.debugDummyStartHp, this.debugDummyQuality, (int)this.debugDummyAttackPriority, this.debugDummyIsFightingMember);
		debugDummyAgent.EnsureInitialized();
		this.debugDummyAgents.Add(debugDummyAgent);
		this.RegisterTarget(debugDummyAgent, -1, -1, true);
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000628BB File Offset: 0x00060ABB
	public void DestroyAllDebugDummyAgents()
	{
		this.ClearDebugDummyAgents();
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000628C4 File Offset: 0x00060AC4
	private Vector3 GetDebugDummySpawnPosition()
	{
		Vector3 vector = base.transform.position;
		if (MainGame.PlayerController != null)
		{
			vector = MainGame.PlayerController.transform.position;
		}
		else if (this.currentLevel)
		{
			FightingCapturePoint baseCapturePoint = this.currentLevel.BaseCapturePoint;
			vector = ((baseCapturePoint != null) ? baseCapturePoint.transform.position : this.currentLevel.transform.position);
		}
		return vector + this.debugDummySpawnOffset;
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x00062948 File Offset: 0x00060B48
	private DebugDummyAgent CreateDebugDummyAgent(Vector3 spawnPosition)
	{
		GameObject gameObject = new GameObject(string.Format("DebugDummyAgent_{0}", this.debugDummyAgents.Count + 1));
		Transform transform = base.transform;
		if (this.parentDebugDummyToLevel && this.currentLevel)
		{
			transform = this.currentLevel.transform;
		}
		gameObject.transform.SetParent(transform, false);
		gameObject.transform.position = spawnPosition;
		gameObject.transform.rotation = Quaternion.identity;
		return gameObject.AddComponent<DebugDummyAgent>();
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000629CC File Offset: 0x00060BCC
	private void ClearDebugDummyAgents()
	{
		if (this.debugDummyAgents.Count == 0)
		{
			return;
		}
		for (int i = this.debugDummyAgents.Count - 1; i >= 0; i--)
		{
			DebugDummyAgent debugDummyAgent = this.debugDummyAgents[i];
			this.TryUnregisterDebugDummy(debugDummyAgent);
			if (debugDummyAgent)
			{
				global::UnityEngine.Object.Destroy(debugDummyAgent.gameObject);
			}
		}
		this.debugDummyAgents.Clear();
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x00062A31 File Offset: 0x00060C31
	private void TryUnregisterDebugDummy(DebugDummyAgent dummy)
	{
		if (!dummy)
		{
			return;
		}
		if (this.TargetsDatabase.GetTargetInfo(dummy) != null)
		{
			this.TargetsDatabase.Unregister(dummy);
		}
	}

	// Token: 0x06001421 RID: 5153 RVA: 0x00062A56 File Offset: 0x00060C56
	internal void OnDebugDummyDestroyed(DebugDummyAgent dummy)
	{
		if (!dummy)
		{
			return;
		}
		this.debugDummyAgents.Remove(dummy);
		if (this.TargetsDatabase.GetTargetInfo(dummy) != null)
		{
			this.TargetsDatabase.Unregister(dummy);
		}
	}

	// Token: 0x06001422 RID: 5154 RVA: 0x00062A88 File Offset: 0x00060C88
	public void RegisterTarget(ICombatEntity entity, int lineId = -1, int sectorId = -1, bool updateInfoIfExists = true)
	{
		this.TryAddChunkableObjectToIgnore(entity);
		this.TargetsDatabase.Register(entity, lineId, sectorId, updateInfoIfExists, true);
		this.TryActivateAddedAlly(entity);
	}

	// Token: 0x06001423 RID: 5155 RVA: 0x00062AAC File Offset: 0x00060CAC
	public void RegisterCombatantTarget(Wgo wgo, bool updateInfoIfExists = true)
	{
		int num = -1;
		int num2 = -1;
		if (FightingGameController.IsTowerOrTurretCombatant(wgo))
		{
			this.TryResolveLineSectorAt(wgo.Data.Position, out num, out num2);
		}
		this.RegisterTarget(wgo, num, num2, updateInfoIfExists);
	}

	// Token: 0x06001424 RID: 5156 RVA: 0x00062AE8 File Offset: 0x00060CE8
	public bool TryResolveLineSectorAt(Vector3 worldPos, out int lineId, out int sectorId)
	{
		lineId = -1;
		sectorId = -1;
		if (this.currentLevel == null)
		{
			return false;
		}
		IReadOnlyList<FightingLine> fightingLines = this.currentLevel.FightingLines;
		for (int i = 0; i < fightingLines.Count; i++)
		{
			FightingLine fightingLine = fightingLines[i];
			if (!(fightingLine == null))
			{
				for (int j = 0; j < fightingLine.sectors.Count; j++)
				{
					FightingSector fightingSector = fightingLine.sectors[j];
					if (!(fightingSector == null))
					{
						if (fightingSector.sectorTrigger && FightingGameController.ContainsPositionXZ(fightingSector.sectorTrigger.bounds, worldPos))
						{
							lineId = fightingLine.lineIdx;
							sectorId = fightingSector.sectorIdx;
							return true;
						}
						if (fightingSector.point && fightingSector.point.ContainsPosition(worldPos))
						{
							lineId = fightingLine.lineIdx;
							sectorId = fightingSector.sectorIdx;
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001425 RID: 5157 RVA: 0x00062BDC File Offset: 0x00060DDC
	private static bool IsTowerOrTurretCombatant(Wgo wgo)
	{
		bool flag;
		if (wgo == null)
		{
			flag = null != null;
		}
		else
		{
			WgoData data = wgo.Data;
			flag = ((data != null) ? data.Definition : null) != null;
		}
		if (!flag)
		{
			return false;
		}
		string wgoGroup = wgo.Data.Definition.wgoGroup;
		AutomaticTurret automaticTurret;
		return (!string.IsNullOrEmpty(wgoGroup) && wgoGroup.Contains("towers")) || (wgo.MainWgoPart != null && wgo.MainWgoPart.TryGetComponent<AutomaticTurret>(out automaticTurret));
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x00062C4C File Offset: 0x00060E4C
	private static bool ContainsPositionXZ(Bounds bounds, Vector3 worldPos)
	{
		return worldPos.x >= bounds.min.x && worldPos.x <= bounds.max.x && worldPos.z >= bounds.min.z && worldPos.z <= bounds.max.z;
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x00062CAE File Offset: 0x00060EAE
	public void RegisterTargetNonPersistent(ICombatEntity entity, int lineId = -1, int sectorId = -1, bool updateInfoIfExists = true)
	{
		this.TryAddChunkableObjectToIgnore(entity);
		this.TargetsDatabase.Register(entity, lineId, sectorId, updateInfoIfExists, false);
		this.TryActivateAddedAlly(entity);
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x00062CCF File Offset: 0x00060ECF
	public void UnregisterTarget(ICombatEntity entity)
	{
		if (!FightingTargetsDatabase.IsCombatEntityAlive(entity))
		{
			this.TargetsDatabase.Unregister(entity);
			return;
		}
		entity.IsActiveCombatant = false;
		this.TargetsDatabase.Unregister(entity);
		this.TryRemoveChunkableObjectFromIgnore(entity);
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x00062D00 File Offset: 0x00060F00
	public void UpdateTargetLocation(ICombatEntity entity, int newLineId = -1, int newSectorId = -1)
	{
		TargetInfo targetInfo = this.TargetsDatabase.GetTargetInfo(entity);
		if (targetInfo != null)
		{
			targetInfo.LineId = newLineId;
			targetInfo.SectorId = newSectorId;
			return;
		}
		Debug.LogWarning(string.Format("Trying to update location for a non-registered target: {0}.", entity));
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x00062D3C File Offset: 0x00060F3C
	public void TryForceRetargetAgentsByAddedDecoy(DecoyComponent decoy)
	{
		foreach (TargetInfo targetInfo in this.TargetsDatabase.AllTargets)
		{
			if ((targetInfo.entity.CombatEntityPosition - decoy.transform.position).XZ().magnitude <= decoy.enemyRetargetRange)
			{
				global::UnityEngine.Object @object = targetInfo.entity as global::UnityEngine.Object;
				if (@object != null)
				{
					Wgo wgo = @object as Wgo;
					if (wgo != null)
					{
						FightingAgent componentInChildren = wgo.GetComponentInChildren<FightingAgent>();
						if (componentInChildren)
						{
							componentInChildren.StopCommandExecution(true);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x00062DEC File Offset: 0x00060FEC
	public void TryForceRetargetAgentsByRemovedDecoy(ICombatEntity removedEntity)
	{
		foreach (TargetInfo targetInfo in this.TargetsDatabase.AllTargets)
		{
			global::UnityEngine.Object @object = targetInfo.entity as global::UnityEngine.Object;
			if (@object != null)
			{
				GameObject gameObject = @object as GameObject;
				FightingAgent fightingAgent;
				if (gameObject != null && gameObject.TryGetComponent<FightingAgent>(out fightingAgent))
				{
					MobCommand mobCommand = fightingAgent.MobCommand;
					if (((mobCommand != null) ? mobCommand.TargetEntity : null) == removedEntity)
					{
						fightingAgent.StopCommandExecution(true);
					}
				}
			}
		}
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x00062E74 File Offset: 0x00061074
	public void ActivateCustomSpawner(string spawnZoneName)
	{
		EnemySpawnZone enemySpawnZone = this.CurrentLevel.customSpawnZones.Find((EnemySpawnZone z) => z.name == spawnZoneName);
		if (!enemySpawnZone)
		{
			return;
		}
		foreach (EnemyData enemyData in enemySpawnZone.customEnemyData)
		{
			for (int i = 0; i < enemyData.count; i++)
			{
				List<PathfindingPenalty> list;
				Vector3 randomPosFromZone = enemySpawnZone.GetRandomPosFromZone(out list);
				WgoData wgoData = new WgoData(enemyData.id, randomPosFromZone, MainGame.PlayerData.currentGameSceneId);
				Wgo wgo = this.groupViewController.SpawnZombie(wgoData);
				this.RegisterTargetNonPersistent(wgo, -1, -1, true);
				this.groupViewController.Init(wgo, list);
				wgo.IsActiveCombatant = GameBalance.Me.fighterWgoIdsCache.Contains(wgo.Id);
				bool isActiveCombatant = wgo.IsActiveCombatant;
			}
		}
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x00062F84 File Offset: 0x00061184
	public void StartPreFight(string levelId, Action onAfterStageApplied = null)
	{
		if (this.fightState == FightState.InPreFight)
		{
			return;
		}
		this.EnsureFightingLevelLoaded(levelId);
		this.currentLevel = MainGame.GetFightingLevel(levelId);
		if (this.currentLevel == null)
		{
			Debug.LogError("Can't get fighting level with id: " + levelId);
			return;
		}
		this.currentLevel.ApplyStageId(3);
		if (onAfterStageApplied != null)
		{
			onAfterStageApplied();
		}
		MainGame.Instance.GameSave.environmentData.EnvironmentEngine.IsPaused = true;
		this.currentLevel.SetActive(true);
		this.CurrentLevel.SetBorderObjectsActive(true);
		if (!this.currentLevel.graphWasInitialized)
		{
			this.SetNotIgnoredStateForChunkableObjects();
			this.currentLevel.graphWasInitialized = true;
			this.currentLevel.InitGraph();
			this.ResetNotIgnoredStateForStaticChunkableObjects();
		}
		this.SetFightState(FightState.InPreFight);
		LazySingleton<FightingGameController>.Instance.CurrentLevel.SpawnAllies();
		MainGame.PlayerData.RemoveInteractingItem();
		MainGame.PlayerController.SetArmorView(true, null, true);
		bool flag = MainGame.PlayerController.Sword.id != "empty";
		bool flag2 = MainGame.PlayerController.Bow.id != "empty";
		ItemDef itemDef = null;
		if (flag)
		{
			itemDef = MainGame.PlayerController.Sword.Definition;
		}
		else if (flag2)
		{
			itemDef = MainGame.PlayerController.Bow.Definition;
		}
		else
		{
			Debug.LogError("Player did not equipped sword or bow, cant set current weapon!!!");
		}
		if (itemDef != null)
		{
			MainGame.PlayerController.AttackComponent.EquipWeapon(itemDef);
		}
		else
		{
			MainGame.PlayerController.AttackComponent.EquipWeapon(ItemType.Sword);
		}
		MainGame.PlayerData.staminaSystem.SetMax();
		MainGame.PlayerController.View.UpdateStaminaBarState();
		HUD hud = LazyUI.Get<HUD>();
		hud.DrawFightingTimeline(new UIFightingTimelineRendererData(this.currentLevel.FightingLevelPreset, this.presetProcessor, this.currentLevel));
		hud.SetMode(HUDMode.Fight);
		if (!MainGame.PlayerData.sawFightTutorialOnce && MainGame.PlayerData.GetRes("battle_tutorial_available", 0f) > 0f)
		{
			MainGame.PlayerData.sawFightTutorialOnce = true;
			UITutorialWindowData uitutorialWindowData = new UITutorialWindowData("tut_battle_2_hdr", null, false);
			LazyUI.GetWindow<UITutorialWindow>().Open(uitutorialWindowData);
		}
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x0006319C File Offset: 0x0006139C
	public void CancelPreFight()
	{
		this.SetFightState(FightState.Disabled);
		this.currentLevel.ApplyStageId(2);
		this.currentLevel.SetBorderObjectsActive(false);
		this.currentLevel.SetActive(false);
		this.currentLevel.graphWasInitialized = false;
		HUD hud = LazyUI.Get<HUD>();
		if (hud.Mode == HUDMode.Fight)
		{
			hud.HideFightingTimeline();
			hud.SetMode(HUDMode.Common);
		}
		foreach (AgentsGroupFlagController agentsGroupFlagController in this.customFlagControllers)
		{
			if (agentsGroupFlagController)
			{
				agentsGroupFlagController.DeInit();
				agentsGroupFlagController.SetEnabled(false);
			}
		}
		this.customFlagControllers.Clear();
		this.SetPlayerDefault(false, false);
		this.ClearEnvironment();
		this.DestroyFlagStandComponents();
		MainGame.PlayerController.View.UpdateStaminaBarState();
		this.ResetNotIgnoredStateForDynamicChunkableObjects();
	}

	// Token: 0x0600142F RID: 5167 RVA: 0x00063284 File Offset: 0x00061484
	public bool CanStartFight()
	{
		if (MainGame.PlayerData.HasMultipleOverheadItems)
		{
			return false;
		}
		Item itemByType = MainGame.PlayerData.toolBeltInventory.GetItemByType(ItemType.BodyArmor);
		Item itemByType2 = MainGame.PlayerData.toolBeltInventory.GetItemByType(ItemType.Sword);
		return !itemByType.IsEmpty && !itemByType2.IsEmpty;
	}

	// Token: 0x06001430 RID: 5168 RVA: 0x000632D4 File Offset: 0x000614D4
	public void RegisterCurrentGameSceneTargets()
	{
		foreach (Wgo wgo in MainGame.PlayerController.CurrentGameScene.Wgos)
		{
			wgo.IsActiveCombatant = GameBalance.Me.fighterWgoIdsCache.Contains(wgo.Id);
			if (wgo.IsActiveCombatant && !wgo.Data.IsHidden && !MainGame.Instance.GameSave.militaryBaseData.ContainsFighter(wgo.Data, true))
			{
				this.RegisterCombatantTarget(wgo, false);
			}
		}
		foreach (AlliesSpawn alliesSpawn in this.CurrentLevel.AlliesSpawns)
		{
			IReadOnlyList<FightingAgent> readOnlyList;
			if (alliesSpawn == null)
			{
				readOnlyList = null;
			}
			else
			{
				AgentsGroupFlagController flagController = alliesSpawn.FlagController;
				if (flagController == null)
				{
					readOnlyList = null;
				}
				else
				{
					AgentsGroupBehaviourController agentsController = flagController.AgentsController;
					readOnlyList = ((agentsController != null) ? agentsController.Agents : null);
				}
			}
			IReadOnlyList<FightingAgent> readOnlyList2 = readOnlyList;
			if (readOnlyList2 != null)
			{
				foreach (FightingAgent fightingAgent in readOnlyList2)
				{
					fightingAgent.Wgo.IsActiveCombatant = GameBalance.Me.fighterWgoIdsCache.Contains(fightingAgent.Wgo.Id);
					if (fightingAgent.Wgo.IsActiveCombatant && !fightingAgent.Wgo.Data.IsHidden)
					{
						this.RegisterTarget(fightingAgent.Wgo, -1, -1, true);
					}
				}
			}
		}
		MainGame.PlayerController.PhysicalBody.IsActiveCombatant = true;
		this.RegisterTarget(MainGame.PlayerController.PhysicalBody, -1, -1, true);
		Debug.Log(string.Format("SetTargetsForEnemies: AllyTargets: {0}, ", this.TargetsDatabase.GetTargetCountByTeam(LazyConsts.Fighting.TeamType.Player)) + string.Format("EnemyTargets: {0}.", this.TargetsDatabase.GetTargetCountByTeam(LazyConsts.Fighting.TeamType.WildZombie)));
	}

	// Token: 0x06001431 RID: 5169 RVA: 0x000634E0 File Offset: 0x000616E0
	public void FinishAsLost()
	{
		if (this.wasFinishedOnce)
		{
			return;
		}
		this.wasFinishedOnce = true;
		this.pendingChainedFightRewards.Clear();
		Debug.Log("GameIsLost");
		FightEndWindowData fightEndWindowData = new FightEndWindowData(GameBalance.Me.GetData<FightDef>(this.currentLevel.id));
		LazyUI.GetWindow<FightLoseWindow>().Open(fightEndWindowData, delegate(FightEndWindowData _)
		{
			LazyUI.Get<UIFade>().Fade(1f, delegate
			{
				this.ApplyLostFightStages();
				this.Stop(false, false);
				GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightLost, this.lastPlayedLevelId);
			}, null);
		});
		LazyAudio.PlayAndForget("fight_lose");
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x00063550 File Offset: 0x00061750
	public void FinishAsWon(int customStageid = -1)
	{
		if (this.wasFinishedOnce)
		{
			return;
		}
		this.wasFinishedOnce = true;
		this.gameLoopCoroutine = null;
		this.spawnQueue.Clear();
		Debug.Log("GameIsWon");
		if (MainGame.Instance.GameSave.militaryBaseData.IsMercenaryPayed)
		{
			if (!string.IsNullOrEmpty(MainGame.Instance.GameSave.militaryBaseData.MercenariesPaymentId))
			{
				MercenariesDef data = GameBalance.Me.GetData<MercenariesDef>(MainGame.Instance.GameSave.militaryBaseData.MercenariesPaymentId);
				if (data != null)
				{
					foreach (LazyExpression lazyExpression in data.afterWinExpr)
					{
						lazyExpression.Evaluate();
					}
				}
			}
			MainGame.Instance.GameSave.militaryBaseData.IsMercenaryPayed = false;
		}
		List<Item> list = new List<Item>();
		foreach (NeedItemData needItemData in GameBalance.Me.GetData<FightDef>(this.currentLevel.id).rewards)
		{
			list.Add(new Item(needItemData.id, needItemData.GetCount(null)));
		}
		FightDef data2 = GameBalance.Me.GetData<FightDef>(this.currentLevel.id);
		FightEndWindowData fightEndWindowData = new FightEndWindowData(data2);
		string chainedNextFightId = data2.onWinNextFightId;
		bool hasChainedFight = !string.IsNullOrEmpty(chainedNextFightId);
		this.AccumulateFightRewards(list);
		LazyUI.GetWindow<FightWinWindow>().Open(fightEndWindowData, delegate(FightEndWindowData _)
		{
			if (hasChainedFight)
			{
				if (!this.TryOpenChainedPrefightWindow(chainedNextFightId, (customStageid != -1) ? customStageid : 5))
				{
					base.<FinishAsWon>g__Fade|1(true);
					return;
				}
			}
			else
			{
				base.<FinishAsWon>g__Fade|1(true);
			}
		});
		this.victoryEnemyKillCoroutine = base.StartCoroutine(this.KillRemainingEnemiesOnVictory(0.8f));
		LazyAudio.PlayAndForget("fight_win");
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x00063740 File Offset: 0x00061940
	public void SetCustomFinishCallback(Action callback)
	{
		this.customFinishCallback = callback;
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x00063749 File Offset: 0x00061949
	public void AddTemporaryWgoData(WgoData wgoData)
	{
		this.destroyOnStopWgos.Add(wgoData);
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x00063758 File Offset: 0x00061958
	public void SetWasInPreFightState(bool wasInPreFight)
	{
		this.SetFightState(wasInPreFight ? FightState.InPreFight : FightState.Disabled);
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x00063768 File Offset: 0x00061968
	public void TryCloseFightEndWindows()
	{
		FightDeadWindow window = LazyUI.GetWindow<FightDeadWindow>();
		FightLoseWindow window2 = LazyUI.GetWindow<FightLoseWindow>();
		FightWinWindow window3 = LazyUI.GetWindow<FightWinWindow>();
		if (window.IsShown)
		{
			window.CloseWithoutCallback();
		}
		if (window2.IsShown)
		{
			window2.CloseWithoutCallback();
		}
		if (window3.IsShown)
		{
			window3.CloseWithoutCallback();
		}
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000637B4 File Offset: 0x000619B4
	public void PauseAgentsMovement()
	{
		if (this.currentLevel == null)
		{
			return;
		}
		foreach (TargetInfo targetInfo in this.TargetsDatabase.AllTargets)
		{
			global::UnityEngine.Object @object = targetInfo.entity as global::UnityEngine.Object;
			if (@object != null && @object != null)
			{
				Wgo wgo = @object as Wgo;
				if (wgo != null)
				{
					FightingAgent componentInChildren = wgo.GetComponentInChildren<FightingAgent>();
					if (componentInChildren != null)
					{
						componentInChildren.PauseMovement();
					}
				}
			}
		}
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x00063844 File Offset: 0x00061A44
	public void UnpauseAgentsMovement()
	{
		if (this.currentLevel == null)
		{
			return;
		}
		foreach (TargetInfo targetInfo in this.TargetsDatabase.AllTargets)
		{
			global::UnityEngine.Object @object = targetInfo.entity as global::UnityEngine.Object;
			if (@object != null && @object != null)
			{
				Wgo wgo = @object as Wgo;
				if (wgo != null)
				{
					FightingAgent componentInChildren = wgo.GetComponentInChildren<FightingAgent>();
					if (componentInChildren != null)
					{
						componentInChildren.UnpauseMovement();
					}
				}
			}
		}
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000638D4 File Offset: 0x00061AD4
	private void DoRewardForTheFight(List<Item> items)
	{
		foreach (Item item in items)
		{
			PlayerData playerData = MainGame.PlayerData;
			Vector2 direction = playerData.Direction;
			MainGame.Instance.dropSystem.DropItem(item, MainGame.PlayerData.currentGameSceneId, playerData.position.Value + new Vector3(direction.x, 0f, direction.y), null);
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x0006396C File Offset: 0x00061B6C
	private void EnsureFightingLevelLoaded(string levelId)
	{
		GameSceneData gameSceneData;
		GameSceneConfig gameSceneConfig;
		if (!MainGame.WorldData.TryGetGameSceneDataForContent(levelId, out gameSceneData, out gameSceneConfig))
		{
			Debug.LogError("Can't find game scene data for fighting level: " + levelId);
			return;
		}
		if (!gameSceneConfig.IsSceneContentDataLoaded(levelId))
		{
			gameSceneData.AddFightingLevelData(levelId);
			this.wasLevelLoaded = true;
		}
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x000639B4 File Offset: 0x00061BB4
	private void AccumulateFightRewards(List<Item> rewards)
	{
		using (List<Item>.Enumerator enumerator = rewards.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Item reward = enumerator.Current;
				Item item = this.pendingChainedFightRewards.Find((Item i) => i.id == reward.id);
				if (item != null)
				{
					item.Count += reward.Count;
				}
				else
				{
					this.pendingChainedFightRewards.Add(new Item(reward.id, reward.Count));
				}
			}
		}
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x00063A64 File Offset: 0x00061C64
	private void GrantPendingChainedFightRewards()
	{
		if (this.pendingChainedFightRewards.Count == 0)
		{
			return;
		}
		this.DoRewardForTheFight(this.pendingChainedFightRewards);
		this.pendingChainedFightRewards.Clear();
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x00063A8C File Offset: 0x00061C8C
	private void OnPlayerDeathAnimationFinished()
	{
		if (this.wasFinishedOnce)
		{
			return;
		}
		this.wasFinishedOnce = true;
		this.pendingChainedFightRewards.Clear();
		FightEndWindowData fightEndWindowData = new FightEndWindowData(GameBalance.Me.GetData<FightDef>(this.currentLevel.id));
		LazyUI.GetWindow<FightDeadWindow>().Open(fightEndWindowData, delegate(FightEndWindowData _)
		{
			LazyUI.Get<UIFade>().Fade(1f, delegate
			{
				this.ApplyLostFightStages();
				this.Stop(false, false);
				GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightLost, this.lastPlayedLevelId);
				MainGame.PlayerController.View.PlayerAnimation.SetTrigger(MainGame.PlayerController.View.PlayerAnimation.PlayerDeathExitTriggerId);
			}, delegate
			{
				MainGame.PlayerController.SetControlTakenType(TakenControlType.ByDeath, true);
			});
		});
		LazyAudio.PlayAndForget("fight_lose");
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x00063AF0 File Offset: 0x00061CF0
	private void SetFightState(FightState newState)
	{
		if (this.fightState == newState)
		{
			return;
		}
		this.fightState = newState;
		Action<FightState> onFightStateChanged = this.OnFightStateChanged;
		if (onFightStateChanged == null)
		{
			return;
		}
		onFightStateChanged(newState);
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x00063B14 File Offset: 0x00061D14
	private new void Awake()
	{
		this.rvoSimulator = base.GetComponent<RVOSimulator>();
		if (this.rvoSimulator)
		{
			this.rvoSimulator.desiredSimulationFPS = 30;
		}
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x00063B3C File Offset: 0x00061D3C
	private IEnumerator GameLoopCoroutine()
	{
		this.SetNotIgnoredStateForChunkableObjects();
		yield return this.currentLevel.Init();
		this.baseDefenseAgentsController.Init();
		this.currentLevel.InitLines();
		this.currentLevel.SetActive(true);
		this.currentLevel.SetBorderObjectsActive(true);
		this.currentLevel.SetEnabledFlagControllers(true);
		foreach (AgentsGroupFlagController agentsGroupFlagController in this.customFlagControllers)
		{
			agentsGroupFlagController.Init();
			agentsGroupFlagController.SetEnabled(true);
		}
		this.RegisterCurrentGameSceneTargets();
		this.ActivateAllies();
		this.presetProcessor.OnEnemiesSpawn += this.HandleEnemiesSpawn;
		this.presetProcessor.OnPresetFinished += this.OnPresetFinished;
		this.presetProcessor.StartPreset(this.CurrentLevelPreset);
		yield return null;
		yield return null;
		this.ResetNotIgnoredStateForStaticChunkableObjects();
		this.spawnQueueCoroutine = base.StartCoroutine(this.ProcessSpawnQueue());
		while (this.presetProcessor.IsPlaying)
		{
			if (this.WereWeLost())
			{
				this.FinishAsLost();
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x00063B4C File Offset: 0x00061D4C
	private void OnPresetFinished()
	{
		MainGame.Instance.GameSave.militaryBaseData.fighterContainersSelectedForFight.Clear();
		foreach (FightingLine fightingLine in this.currentLevel.FightingLines)
		{
			foreach (FightingSector fightingSector in fightingLine.sectors)
			{
				if (fightingSector.point.isBasePoint && fightingSector.point.OwnedByTeam != LazyConsts.Fighting.TeamType.Player)
				{
					this.FinishAsLost();
					return;
				}
			}
		}
		this.FinishAsWon(-1);
		this.gameLoopCoroutine = base.StartCoroutine(this.CheckWinConditionAfterPreset());
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x00063C24 File Offset: 0x00061E24
	private IEnumerator CheckWinConditionAfterPreset()
	{
		while (this.TargetsDatabase.GetTargetCountByTeam(LazyConsts.Fighting.TeamType.WildZombie) > 0)
		{
			if (this.WereWeLost())
			{
				this.FinishAsLost();
				yield break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x00063C33 File Offset: 0x00061E33
	private IEnumerator KillRemainingEnemiesOnVictory(float totalDuration)
	{
		List<FightingAgent> enemies = this.CollectLivingEnemyAgents();
		if (enemies.Count == 0)
		{
			this.victoryEnemyKillCoroutine = null;
			yield break;
		}
		foreach (FightingAgent fightingAgent in enemies)
		{
			fightingAgent.ClearCommand();
			fightingAgent.PauseMovement();
		}
		float step = totalDuration / (float)enemies.Count;
		int num;
		for (int i = 0; i < enemies.Count; i = num + 1)
		{
			FightingAgent fightingAgent2 = enemies[i];
			if (!(fightingAgent2 == null))
			{
				AgentsGroupBehaviourController parentController = fightingAgent2.ParentController;
				if (parentController != null)
				{
					parentController.RemoveAgent(fightingAgent2.Wgo.Data.UniqueId);
				}
				fightingAgent2.ClearCommand();
				fightingAgent2.PlayDying();
				if (i < enemies.Count - 1 && step > 0f)
				{
					yield return new WaitForSeconds(step);
				}
			}
			num = i;
		}
		this.victoryEnemyKillCoroutine = null;
		yield break;
	}

	// Token: 0x06001444 RID: 5188 RVA: 0x00063C4C File Offset: 0x00061E4C
	private List<FightingAgent> CollectLivingEnemyAgents()
	{
		List<FightingAgent> list = new List<FightingAgent>();
		foreach (ICombatEntity combatEntity in this.TargetsDatabase.GetTargetsByTeam(LazyConsts.Fighting.TeamType.WildZombie))
		{
			Wgo wgo = combatEntity as Wgo;
			FightingAgent fightingAgent;
			if (wgo != null && wgo.MainWgoPart && wgo.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
			{
				list.Add(fightingAgent);
			}
		}
		return list;
	}

	// Token: 0x06001445 RID: 5189 RVA: 0x00063CCC File Offset: 0x00061ECC
	private IEnumerator ProcessSpawnQueue()
	{
		for (;;)
		{
			if (this.spawnQueue.Count > 0)
			{
				ValueTuple<string, FightingLevelPreset.FightingLineData> valueTuple = this.spawnQueue.Peek();
				int num = this.CurrentLevelPreset.lines.IndexOf(valueTuple.Item2);
				if (num < 0)
				{
					Debug.LogError("Could not find line index for the spawn request. Something is wrong.");
					this.spawnQueue.Dequeue();
					continue;
				}
				if (this.TargetsDatabase.GetEnemyCountOnLine(num) < valueTuple.Item2.maxSpawnedCountAtOnce)
				{
					this.spawnQueue.Dequeue();
					Vector3 vector;
					List<PathfindingPenalty> list;
					if (this.currentLevel.GetAvailablePositionForSpawning(num, out vector, out list))
					{
						WgoData wgoData = new WgoData(valueTuple.Item1, vector, MainGame.PlayerData.currentGameSceneId);
						Wgo wgo = this.groupViewController.SpawnZombie(wgoData);
						this.RegisterTargetNonPersistent(wgo, num, -1, true);
						this.groupViewController.Init(wgo, list);
						wgo.IsActiveCombatant = GameBalance.Me.fighterWgoIdsCache.Contains(wgo.Id);
						if (!wgo.IsActiveCombatant)
						{
							continue;
						}
						this.currentLevel.GetFightingLine(num).AddSpawnedEnemy(wgo);
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001446 RID: 5190 RVA: 0x00063CDC File Offset: 0x00061EDC
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		if (this.fightState != FightState.Disabled)
		{
			this.UpdateLines();
			MainGame.PlayerData.staminaSystem.UpdateStaminaLogic(Time.deltaTime);
		}
		if (this.uiFightingOverlayData != null)
		{
			this.uiFightingOverlayData.UpdateData(MainGame.PlayerController.PhysicalBody.transform.position);
		}
		if (this.fightState == FightState.ActiveFight)
		{
			this.UpdateTargets();
		}
	}

	// Token: 0x06001447 RID: 5191 RVA: 0x00063D4C File Offset: 0x00061F4C
	private void HandleEnemiesSpawn(string id, int count, FightingLevelPreset.FightingLineData line)
	{
		for (int i = 0; i < count; i++)
		{
			this.spawnQueue.Enqueue(new ValueTuple<string, FightingLevelPreset.FightingLineData>(id, line));
		}
	}

	// Token: 0x06001448 RID: 5192 RVA: 0x00063D78 File Offset: 0x00061F78
	private void ActivateAllies()
	{
		AutomaticTurret[] array = global::UnityEngine.Object.FindObjectsByType<AutomaticTurret>(FindObjectsSortMode.None);
		Debug.Log(string.Format("Turrets is going to be activated in count: {0}", array.Length));
		AutomaticTurret[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Activate();
		}
	}

	// Token: 0x06001449 RID: 5193 RVA: 0x00063DBC File Offset: 0x00061FBC
	private void TryActivateAddedAlly(ICombatEntity combatEntity)
	{
		Wgo wgo = combatEntity as Wgo;
		AutomaticTurret automaticTurret;
		if (wgo != null && wgo.IsActiveCombatant && wgo.MainWgoPart != null && wgo.MainWgoPart.TryGetComponent<AutomaticTurret>(out automaticTurret))
		{
			automaticTurret.Activate();
		}
	}

	// Token: 0x0600144A RID: 5194 RVA: 0x00063E00 File Offset: 0x00062000
	private void DeactivateAllies()
	{
		AutomaticTurret[] array = global::UnityEngine.Object.FindObjectsByType<AutomaticTurret>(FindObjectsSortMode.None);
		Debug.Log(string.Format("Turrets is going to be deactivated in count: {0}", array.Length));
		AutomaticTurret[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i].Deactivate();
		}
	}

	// Token: 0x0600144B RID: 5195 RVA: 0x00063E44 File Offset: 0x00062044
	private void UpdateLines()
	{
		if (this.currentLevel == null)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		foreach (FightingLine fightingLine in this.currentLevel.FightingLines)
		{
			fightingLine.CustomUpdate(deltaTime);
		}
		foreach (AlliesSpawn alliesSpawn in this.currentLevel.AlliesSpawns)
		{
			AgentsGroupFlagController flagController = alliesSpawn.FlagController;
			if (flagController != null && !this.customFlagControllers.Contains(flagController))
			{
				flagController.CustomUpdate(deltaTime);
			}
		}
		foreach (AgentsGroupFlagController agentsGroupFlagController in this.customFlagControllers)
		{
			agentsGroupFlagController.CustomUpdate(deltaTime);
		}
	}

	// Token: 0x0600144C RID: 5196 RVA: 0x00063F50 File Offset: 0x00062150
	private void UpdateTargets()
	{
		this.TargetsDatabase.PruneDeadTargets(new Action<ICombatEntity>(this.TryRemoveChunkableObjectFromIgnore));
	}

	// Token: 0x0600144D RID: 5197 RVA: 0x00063F6C File Offset: 0x0006216C
	private void TryAddChunkableObjectToIgnore(ICombatEntity entity)
	{
		IChunkableObject chunkableObject = entity as IChunkableObject;
		if (chunkableObject != null)
		{
			this.AllDynamicObjectsInZone.Add(chunkableObject);
			chunkableObject.UpdateFlag(ChunkingIgnoreType.Fighting, true);
		}
	}

	// Token: 0x0600144E RID: 5198 RVA: 0x00063F98 File Offset: 0x00062198
	private void TryRemoveChunkableObjectFromIgnore(ICombatEntity entity)
	{
		IChunkableObject chunkableObject = entity as IChunkableObject;
		if (chunkableObject != null)
		{
			this.AllDynamicObjectsInZone.Remove(chunkableObject);
			chunkableObject.UpdateFlag(ChunkingIgnoreType.Fighting, false);
		}
	}

	// Token: 0x0600144F RID: 5199 RVA: 0x00063FC4 File Offset: 0x000621C4
	private void SetNotIgnoredStateForChunkableObjects()
	{
		if (this.wasDisabledChunkerForFighting)
		{
			return;
		}
		this.wasDisabledChunkerForFighting = true;
		this.AllStaticObjectsInZone = new HashSet<IChunkableObject>();
		this.AllDynamicObjectsInZone = new HashSet<IChunkableObject>();
		foreach (BoxCollider boxCollider in this.currentLevel.ZoneDefineColliders)
		{
			HashSet<IChunkableObject> allChunkableObjectsInBoundsForSelectedLayers = LazySingleton<ChunkManager>.Instance.GetAllChunkableObjectsInBoundsForSelectedLayers(new List<ChunkManagerLayerType>
			{
				ChunkManagerLayerType.DynamicWgo,
				ChunkManagerLayerType.StaticWgo
			}, boxCollider.bounds);
			foreach (IChunkableObject chunkableObject in LazySingleton<ChunkManager>.Instance.GetAllChunkableObjectsInBoundsForSelectedLayers((from ChunkManagerLayerType layerType in Enum.GetValues(typeof(ChunkManagerLayerType))
				where layerType != ChunkManagerLayerType.DynamicWgo && layerType != ChunkManagerLayerType.StaticWgo
				select layerType).ToList<ChunkManagerLayerType>(), boxCollider.bounds))
			{
				if (FightingGameController.IsChunkableObjectAlive(chunkableObject) && this.AllStaticObjectsInZone.Add(chunkableObject))
				{
					chunkableObject.UpdateFlag(ChunkingIgnoreType.Fighting, true);
				}
			}
			foreach (IChunkableObject chunkableObject2 in allChunkableObjectsInBoundsForSelectedLayers)
			{
				if (FightingGameController.IsChunkableObjectAlive(chunkableObject2) && this.AllDynamicObjectsInZone.Add(chunkableObject2))
				{
					chunkableObject2.UpdateFlag(ChunkingIgnoreType.Fighting, true);
				}
			}
		}
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x00064180 File Offset: 0x00062380
	private void ResetNotIgnoredStateForStaticChunkableObjects()
	{
		if (this.AllStaticObjectsInZone == null)
		{
			return;
		}
		foreach (IChunkableObject chunkableObject in this.AllStaticObjectsInZone)
		{
			if (FightingGameController.IsChunkableObjectAlive(chunkableObject))
			{
				chunkableObject.UpdateFlag(ChunkingIgnoreType.Fighting, false);
			}
		}
		this.AllStaticObjectsInZone.Clear();
		this.wasDisabledChunkerForFighting = false;
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x000641F8 File Offset: 0x000623F8
	private void ResetNotIgnoredStateForDynamicChunkableObjects()
	{
		if (this.AllDynamicObjectsInZone == null)
		{
			return;
		}
		foreach (IChunkableObject chunkableObject in this.AllDynamicObjectsInZone)
		{
			if (FightingGameController.IsChunkableObjectAlive(chunkableObject))
			{
				chunkableObject.UpdateFlag(ChunkingIgnoreType.Fighting, false);
			}
		}
		this.AllDynamicObjectsInZone.Clear();
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x00064268 File Offset: 0x00062468
	private static bool IsChunkableObjectAlive(IChunkableObject chunkableObject)
	{
		if (chunkableObject != null)
		{
			global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
			return @object == null || @object != null;
		}
		return false;
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x0006428D File Offset: 0x0006248D
	private bool WereWeLost()
	{
		return this.CurrentLevel.BaseCapturePoint.OwnedByTeam == LazyConsts.Fighting.TeamType.WildZombie;
	}

	// Token: 0x06001454 RID: 5204 RVA: 0x000642A4 File Offset: 0x000624A4
	private void ClearEnvironment()
	{
		this.isClearingFightEnvironment = true;
		try
		{
			this.PrepareFightBuildingsForTeardown();
			MainGame.Instance.GameSave.militaryBaseData.ReturnFightBuildingsToBase();
			LazySingleton<FightingGameController>.Instance.CurrentLevel.DeSpawnAllies();
			MainGame.Instance.GameSave.environmentData.EnvironmentEngine.IsPaused = false;
			Wgo wgo = MainGame.PlayerController.RemoveTheFlag();
			if (wgo)
			{
				wgo.Data.Position = MainGame.PlayerData.position.Value;
				AgentsGroupFlagController.SetInteractionLocked(wgo, false);
			}
		}
		finally
		{
			this.isClearingFightEnvironment = false;
		}
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x0006434C File Offset: 0x0006254C
	private void PrepareFightBuildingsForTeardown()
	{
		foreach (MilitaryBaseData.MilitaryBaseFightBuilding militaryBaseFightBuilding in MainGame.Instance.GameSave.militaryBaseData.fightBuildings)
		{
			Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(militaryBaseFightBuilding.uniqueId);
			if (wgoViewGlobal)
			{
				foreach (FlagPlacementPoint flagPlacementPoint in wgoViewGlobal.GetComponentsInChildren<FlagPlacementPoint>(true))
				{
					Wgo flagWgo = flagPlacementPoint.FlagWgo;
					flagPlacementPoint.FlagWgo = null;
					flagPlacementPoint.DeInit();
					if (flagWgo)
					{
						MainGame.WorldData.RemoveWgoDataFromGameScene(flagWgo.Data, true);
					}
				}
			}
		}
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x00064400 File Offset: 0x00062600
	private void SetPlayerDefault(bool hasCustomAfterFightPos = false, bool stopAsWon = false)
	{
		PlayerController playerController = MainGame.PlayerController;
		playerController.AttackComponent.UnequipWeapon();
		playerController.SetArmorView(false, null, true);
		if (!hasCustomAfterFightPos)
		{
			string text = (stopAsWon ? this.currentLevel.FightbackAfterWinGdPointId : this.currentLevel.FightbackGdPointId);
			if (string.IsNullOrEmpty(text))
			{
				text = "RT_fightback_player_spawn";
			}
			PlayerController.Teleport(new GDPointTeleportData(text, false, "outdoor", "", null, true, 0.3f));
		}
		if (!playerController.IsControlEnabledByType(TakenControlType.ByDeath))
		{
			playerController.SetControlTakenType(TakenControlType.ByDeath, true);
			playerController.View.PlayerAnimation.SetState(global::AnimationState.Idle);
		}
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x0006449C File Offset: 0x0006269C
	private void ClearTemporaryWgos()
	{
		foreach (WgoData wgoData in this.destroyOnStopWgos)
		{
			if (wgoData != null)
			{
				MainGame.WorldData.RemoveWgoDataFromGameScene(wgoData, true);
			}
		}
		this.destroyOnStopWgos.Clear();
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x00064504 File Offset: 0x00062704
	private void DestroyFlagStandComponents()
	{
		foreach (FlagStandComponent flagStandComponent in this.FlagStandComponents)
		{
			if (flagStandComponent != null)
			{
				flagStandComponent.DestroyStandWgo();
			}
		}
		this.FlagStandComponents.Clear();
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x00064568 File Offset: 0x00062768
	private void ApplyLostFightStages()
	{
		this.CurrentLevel.ApplyStageId(2);
		FightingGameController.RepairChainedFightsLeftAtWinAfterFollowUpReset(null);
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x0006457C File Offset: 0x0006277C
	public static void RepairChainedFightsLeftAtWinAfterFollowUpReset(WorldData worldData = null)
	{
		if (worldData == null)
		{
			worldData = MainGame.WorldData;
		}
		GameBalance me = GameBalance.Me;
		List<FightDef> list = ((me != null) ? me.fightDefinitions : null);
		if (((worldData != null) ? worldData.gameSceneDataList : null) == null || list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			FightDef fightDef = list[i];
			FightingLevelData fightingLevelData;
			FightingLevelData fightingLevelData2;
			if (fightDef != null && !string.IsNullOrEmpty(fightDef.onWinNextFightId) && FightingGameController.TryGetFightingLevelData(worldData, fightDef.id, out fightingLevelData) && FightingGameController.TryGetFightingLevelData(worldData, fightDef.onWinNextFightId, out fightingLevelData2) && (fightingLevelData.CurStageId == 5 || fightingLevelData.CurStageId == 6) && fightingLevelData2.CurStageId == 2)
			{
				FightingGameController.ApplyFightingLevelStage(worldData, fightDef.id, 2);
			}
		}
	}

	// Token: 0x0600145B RID: 5211 RVA: 0x00064630 File Offset: 0x00062830
	private static bool TryGetFightingLevelData(WorldData worldData, string fightId, out FightingLevelData data)
	{
		data = null;
		if (worldData == null || string.IsNullOrEmpty(fightId))
		{
			return false;
		}
		GameSceneData gameSceneData;
		GameSceneConfig gameSceneConfig;
		if (MainGame.Instance != null && worldData.TryGetGameSceneDataForContent(fightId, out gameSceneData, out gameSceneConfig) && ((gameSceneData != null) ? gameSceneData.fightingLevels : null) != null)
		{
			data = gameSceneData.fightingLevels.Find((FightingLevelData level) => level.id == fightId);
			if (data != null)
			{
				return true;
			}
		}
		List<GameSceneData> gameSceneDataList = worldData.gameSceneDataList;
		Predicate<FightingLevelData> <>9__1;
		for (int i = 0; i < gameSceneDataList.Count; i++)
		{
			GameSceneData gameSceneData2 = gameSceneDataList[i];
			if (((gameSceneData2 != null) ? gameSceneData2.fightingLevels : null) != null)
			{
				List<FightingLevelData> fightingLevels = gameSceneData2.fightingLevels;
				Predicate<FightingLevelData> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = (FightingLevelData level) => level.id == fightId);
				}
				data = fightingLevels.Find(predicate);
				if (data != null)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600145C RID: 5212 RVA: 0x00064718 File Offset: 0x00062918
	private static void ApplyFightingLevelStage(WorldData worldData, string fightId, int stageId)
	{
		if (MainGame.Instance != null)
		{
			FightingLevel fightingLevel = MainGame.GetFightingLevel(fightId);
			if (fightingLevel != null)
			{
				fightingLevel.ApplyStageId(stageId);
				return;
			}
		}
		FightingLevelData fightingLevelData;
		if (FightingGameController.TryGetFightingLevelData(worldData, fightId, out fightingLevelData))
		{
			fightingLevelData.CurStageId = stageId;
		}
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x0006475C File Offset: 0x0006295C
	private bool TryOpenChainedPrefightWindow(string nextFightId, int customStageid)
	{
		this.EnsureFightingLevelLoaded(nextFightId);
		FightingLevel nextLevel = MainGame.GetFightingLevel(nextFightId);
		if (nextLevel == null)
		{
			Debug.LogError("Can't get fighting level with id: " + nextFightId);
			return false;
		}
		if (!nextLevel.IsValid())
		{
			Debug.LogError("Fighting level [" + nextLevel.id + "] is not valid!");
			return false;
		}
		string levelGdPointId = nextLevel.LevelGdPointId;
		if (MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(levelGdPointId) == null)
		{
			Debug.LogError("Can't find GD point data with id [" + levelGdPointId + "]!");
			return false;
		}
		Action <>9__1;
		LazyUI.GetWindow<UIPrefightWindow>().Open(new UIPrefightWindowData(nextFightId, delegate
		{
			UIBasicFade uibasicFade = LazyUI.Get<UIFade>();
			float num = 1f;
			Action action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate
				{
					this.CurrentLevel.ApplyStageId(customStageid);
					this.Stop(false, false);
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightWon, this.lastPlayedLevelId);
					GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(levelGdPointId);
					if (gdpointDataById == null)
					{
						return;
					}
					if (MainGame.PlayerData.HasOverheadItem)
					{
						MainGame.PlayerData.DropOverheadItem();
					}
					PlayerController.Teleport(new GDPointTeleportData(gdpointDataById, nextLevel.EnvironmentPreset, "", null, true, 0.3f));
					this.StartPreFight(nextFightId, null);
				});
			}
			uibasicFade.Fade(num, action, delegate
			{
			});
		}, false));
		return true;
	}

	// Token: 0x0400150F RID: 5391
	private const float VICTORY_ENEMY_KILL_DURATION = 0.8f;

	// Token: 0x04001510 RID: 5392
	private Action customFinishCallback;

	// Token: 0x04001512 RID: 5394
	[SerializeField]
	private ZombieGroupViewController groupViewController;

	// Token: 0x04001513 RID: 5395
	[SerializeField]
	private FightingLevelPresetProcessor presetProcessor;

	// Token: 0x04001514 RID: 5396
	[SerializeField]
	private FightingLevel currentLevel;

	// Token: 0x04001515 RID: 5397
	[Tooltip("A general controller for agents that don't belong to any specific line (e.g., have broken through).")]
	[SerializeField]
	private AgentsGroupBehaviourController baseDefenseAgentsController;

	// Token: 0x04001516 RID: 5398
	[SerializeField]
	private FightEffectsManager fightEffectsManager;

	// Token: 0x04001518 RID: 5400
	public RVOSimulator rvoSimulator;

	// Token: 0x04001519 RID: 5401
	private Coroutine gameLoopCoroutine;

	// Token: 0x0400151A RID: 5402
	private Coroutine spawnQueueCoroutine;

	// Token: 0x0400151B RID: 5403
	private Coroutine victoryEnemyKillCoroutine;

	// Token: 0x0400151C RID: 5404
	private bool isPaused;

	// Token: 0x0400151D RID: 5405
	[TupleElementNames(new string[] { "id", "line" })]
	private readonly Queue<ValueTuple<string, FightingLevelPreset.FightingLineData>> spawnQueue = new Queue<ValueTuple<string, FightingLevelPreset.FightingLineData>>();

	// Token: 0x0400151E RID: 5406
	private Collider[] overlapResults = new Collider[20];

	// Token: 0x0400151F RID: 5407
	private FightState fightState;

	// Token: 0x04001520 RID: 5408
	public HashSet<AgentsGroupFlagController> customFlagControllers = new HashSet<AgentsGroupFlagController>();

	// Token: 0x04001521 RID: 5409
	[SerializeField]
	private LazyConsts.Fighting.TeamType debugDummyTeam;

	// Token: 0x04001522 RID: 5410
	[SerializeField]
	private int debugDummyMaxHp = 25;

	// Token: 0x04001523 RID: 5411
	[SerializeField]
	private int debugDummyStartHp = 25;

	// Token: 0x04001524 RID: 5412
	[SerializeField]
	private int debugDummyQuality = 1;

	// Token: 0x04001525 RID: 5413
	[SerializeField]
	private LazyConsts.Fighting.TargetAttackPriority debugDummyAttackPriority = LazyConsts.Fighting.TargetAttackPriority.High;

	// Token: 0x04001526 RID: 5414
	[SerializeField]
	private bool debugDummyIsFightingMember = true;

	// Token: 0x04001527 RID: 5415
	[SerializeField]
	private Vector3 debugDummySpawnOffset = new Vector3(0f, 0f, 2f);

	// Token: 0x04001528 RID: 5416
	[SerializeField]
	private bool parentDebugDummyToLevel = true;

	// Token: 0x04001529 RID: 5417
	[SerializeField]
	private bool selectDebugDummyOnSpawn = true;

	// Token: 0x0400152A RID: 5418
	private string lastPlayedLevelId;

	// Token: 0x0400152B RID: 5419
	private string lastPlayedFightPlaylist;

	// Token: 0x0400152C RID: 5420
	private bool wasDisabledChunkerForFighting;

	// Token: 0x0400152D RID: 5421
	private bool wasFinishedOnce;

	// Token: 0x0400152E RID: 5422
	private bool wasLevelLoaded;

	// Token: 0x0400152F RID: 5423
	private bool isClearingFightEnvironment;

	// Token: 0x04001530 RID: 5424
	private readonly List<Item> pendingChainedFightRewards = new List<Item>();

	// Token: 0x04001531 RID: 5425
	private RecastGraph recastGraph;

	// Token: 0x04001532 RID: 5426
	private UIFightingOverlayData uiFightingOverlayData;

	// Token: 0x04001533 RID: 5427
	private FightingLevel[] allLevels;

	// Token: 0x04001534 RID: 5428
	private HashSet<WgoData> destroyOnStopWgos = new HashSet<WgoData>();

	// Token: 0x0400153A RID: 5434
	private readonly List<DebugDummyAgent> debugDummyAgents = new List<DebugDummyAgent>();
}
