using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000321 RID: 801
[RequireComponent(typeof(AgentsGroupBehaviourController))]
public class FightingLine : MonoBehaviour
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x0600156C RID: 5484 RVA: 0x000686C0 File Offset: 0x000668C0
	public bool AreAllSpawnZonesDisabled
	{
		get
		{
			using (List<EnemySpawnZone>.Enumerator enumerator = this.spawnZones.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.gameObject.activeSelf)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x00068720 File Offset: 0x00066920
	public void SetActive(bool isActive)
	{
		foreach (FightingSector fightingSector in this.sectors)
		{
			fightingSector.SetActiveState(isActive);
		}
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x00068774 File Offset: 0x00066974
	public void Init()
	{
		foreach (FightingSector fightingSector in this.sectors)
		{
			fightingSector.Init(this);
		}
		this.currentSectorToCaptureIdx = 0;
		this.agentsController.FightingLine = this;
		this.UpdateSectorsCapturability();
		this.agentsController.Init();
		this.agentsController.SetTargetLineIdx(-1);
		foreach (EnemySpawnZone enemySpawnZone in this.spawnZones)
		{
			enemySpawnZone.ResetActiveState();
		}
		base.enabled = true;
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x0006883C File Offset: 0x00066A3C
	public void Deinit()
	{
		base.enabled = false;
		this.agentsController.DeInit();
		foreach (FightingSector fightingSector in this.sectors)
		{
			fightingSector.DeInit();
		}
		this.lineIdx = -1;
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x000688A8 File Offset: 0x00066AA8
	public void AddSpawnedEnemy(ICombatEntity agent)
	{
		Wgo wgo = agent as Wgo;
		FightingAgent fightingAgent;
		if (wgo != null && wgo.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
		{
			this.agentsController.AddAgent(fightingAgent);
		}
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x000688DC File Offset: 0x00066ADC
	public void HandleSectorWasCaptured(FightingSector sector, LazyConsts.Fighting.TeamType team)
	{
		int num = this.FindNearestEnemySectorIdxBy(team);
		this.UpdateSectorsCapturability();
		if (team != LazyConsts.Fighting.TeamType.Player)
		{
			if (team == LazyConsts.Fighting.TeamType.WildZombie)
			{
				if (num != -1)
				{
					this.currentSectorToCaptureIdx = num;
				}
				else
				{
					this.HandleLineBreachedByEnemies();
				}
				LazySingleton<FightingGameController>.Instance.UIFightingOverlayData.UntrackCapturePoint(sector.point);
			}
		}
		else
		{
			if (this.isLineBreached)
			{
				this.isLineBreached = false;
				this.agentsController.SetTargetLineIdx(this.lineIdx);
			}
			if (num != -1)
			{
				this.currentSectorToCaptureIdx = num;
			}
			else
			{
				this.HandleAllSectorsCapturedByPlayer();
			}
			LazySingleton<FightingGameController>.Instance.UIFightingOverlayData.TrackCapturePoint(sector.point);
		}
		this.UpdateAgentTargets();
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x00068978 File Offset: 0x00066B78
	public FightingSector FindNearestEnemySectorBy(LazyConsts.Fighting.TeamType teamType)
	{
		int num = this.FindNearestEnemySectorIdxBy(teamType);
		if (num == -1)
		{
			return null;
		}
		return this.sectors[num];
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000689A0 File Offset: 0x00066BA0
	public ICombatEntity FindNearestTargetOnLine(Vector3 from, LazyConsts.Fighting.TeamType targetTeamType, Func<IEnumerable<ICombatEntity>, ICombatEntity> customNearestFunc = null)
	{
		ValueTuple<ICombatEntity, float> valueTuple = new ValueTuple<ICombatEntity, float>(null, float.MaxValue);
		IEnumerable<ICombatEntity> enumerable = from e in LazySingleton<FightingGameController>.Instance.TargetsDatabase.GetTargets(this.lineIdx, -1)
			where e.TeamType == targetTeamType
			select e;
		if (customNearestFunc != null)
		{
			return customNearestFunc(enumerable);
		}
		foreach (ICombatEntity combatEntity in enumerable)
		{
			float magnitude = (combatEntity.CombatEntityPosition - from).XZ().magnitude;
			if (magnitude < valueTuple.Item2)
			{
				valueTuple.Item1 = combatEntity;
				valueTuple.Item2 = magnitude;
			}
		}
		return valueTuple.Item1;
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x00068A70 File Offset: 0x00066C70
	private int FindNearestEnemySectorIdxBy(LazyConsts.Fighting.TeamType teamType)
	{
		if (this.sectors.Count == 0)
		{
			return -1;
		}
		int num = ((teamType == LazyConsts.Fighting.TeamType.Player) ? 0 : (this.sectors.Count - 1));
		int num2 = ((teamType == LazyConsts.Fighting.TeamType.Player) ? (this.sectors.Count - 1) : 0);
		int num3 = ((teamType == LazyConsts.Fighting.TeamType.Player) ? 1 : (-1));
		int num4 = num;
		while ((num3 > 0) ? (num4 <= num2) : (num4 >= num2))
		{
			if (this.sectors[num4].point.OwnedByTeam != teamType)
			{
				return num4;
			}
			num4 += num3;
		}
		if (teamType != LazyConsts.Fighting.TeamType.Player)
		{
			if (teamType == LazyConsts.Fighting.TeamType.WildZombie)
			{
				List<FightingSector> list = this.sectors;
				if (list[list.Count - 1].point.OwnedByTeam == LazyConsts.Fighting.TeamType.Player)
				{
					return this.sectors.Count - 1;
				}
			}
		}
		else if (this.sectors[0].point.OwnedByTeam == LazyConsts.Fighting.TeamType.WildZombie)
		{
			return 0;
		}
		return -1;
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x00068B48 File Offset: 0x00066D48
	private void UpdateSectorsCapturability()
	{
		if (this.sectors.Count == 0)
		{
			return;
		}
		if (this.sectors.Count == 1)
		{
			this.sectors[0].point.LockedForCapture = false;
			return;
		}
		bool flag = false;
		for (int i = 0; i < this.sectors.Count - 1; i++)
		{
			FightingSector fightingSector = this.sectors[i];
			FightingSector fightingSector2 = this.sectors[i + 1];
			if (i == 0)
			{
				fightingSector.point.LockedForCapture = fightingSector.point.OwnedByTeam == fightingSector2.point.OwnedByTeam;
			}
			if (fightingSector.point.OwnedByTeam == fightingSector2.point.OwnedByTeam)
			{
				fightingSector2.point.LockedForCapture = true;
			}
			else
			{
				fightingSector2.point.LockedForCapture = false;
				flag = true;
			}
		}
		if (!flag)
		{
			if (this.sectors[0].point.OwnedByTeam == LazyConsts.Fighting.TeamType.WildZombie)
			{
				this.sectors[0].point.LockedForCapture = false;
			}
			List<FightingSector> list = this.sectors;
			if (list[list.Count - 1].point.OwnedByTeam == LazyConsts.Fighting.TeamType.Player)
			{
				List<FightingSector> list2 = this.sectors;
				list2[list2.Count - 1].point.LockedForCapture = false;
			}
		}
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x00068C8C File Offset: 0x00066E8C
	private void HandleAllSectorsCapturedByPlayer()
	{
		Debug.Log("Line " + base.gameObject.name + " captured all sectors!");
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightLineCaptured, string.Format("{0}:{1}", LazySingleton<FightingGameController>.Instance.CurrentLevelId, this.lineIdx));
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x00068CDE File Offset: 0x00066EDE
	private void UpdateAgentTargets()
	{
		this.agentsController.TryRetargetAgents();
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x00068CEC File Offset: 0x00066EEC
	private void HandleLineBreachedByEnemies()
	{
		if (this.isLineBreached)
		{
			return;
		}
		this.isLineBreached = true;
		LazySingleton<FightingGameController>.Instance.BreachedLines.Add(this);
		this.agentsController.SetTargetLineIdx(-1);
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightLineLost, string.Format("{0}:{1}", LazySingleton<FightingGameController>.Instance.CurrentLevelId, this.lineIdx));
		Debug.LogWarning(string.Format("Line {0} has been breached by enemies!", this.lineIdx));
		foreach (ICombatEntity combatEntity in (from t in LazySingleton<FightingGameController>.Instance.TargetsDatabase.AllTargets
			where t.Team == LazyConsts.Fighting.TeamType.WildZombie && t.LineId == this.lineIdx
			select t.entity).ToList<ICombatEntity>())
		{
			LazySingleton<FightingGameController>.Instance.UpdateTargetLocation(combatEntity, -1, -1);
			Wgo wgo = combatEntity as Wgo;
			FightingAgent fightingAgent;
			if (wgo != null && wgo.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
			{
				LazySingleton<FightingGameController>.Instance.BaseDefenseAgentsController.AddAgent(fightingAgent);
			}
		}
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x00068E20 File Offset: 0x00067020
	private void OnTriggerEnter(Collider other)
	{
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent == null)
		{
			return;
		}
		if (componentInParent.TeamType != this.agentsController.TargetTeam)
		{
			return;
		}
		this.UpdateAgentTargets();
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x00068E52 File Offset: 0x00067052
	public void CustomUpdate(float deltaTime)
	{
		this.agentsController.CustomUpdate(deltaTime);
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x00068E60 File Offset: 0x00067060
	private void Awake()
	{
		base.enabled = false;
	}

	// Token: 0x04001603 RID: 5635
	public List<EnemySpawnZone> spawnZones = new List<EnemySpawnZone>();

	// Token: 0x04001604 RID: 5636
	public List<FightingSector> sectors = new List<FightingSector>();

	// Token: 0x04001605 RID: 5637
	[SerializeField]
	private AgentsGroupBehaviourController agentsController;

	// Token: 0x04001606 RID: 5638
	[NonSerialized]
	public int lineIdx = -1;

	// Token: 0x04001607 RID: 5639
	private int currentSectorToCaptureIdx = -1;

	// Token: 0x04001608 RID: 5640
	private bool isLineBreached;
}
