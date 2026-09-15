using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000324 RID: 804
public class FightingSector : MonoBehaviour
{
	// Token: 0x14000023 RID: 35
	// (add) Token: 0x06001583 RID: 5507 RVA: 0x00068ECC File Offset: 0x000670CC
	// (remove) Token: 0x06001584 RID: 5508 RVA: 0x00068F04 File Offset: 0x00067104
	public event Action<FightingSector> OnCapturedByAllies;

	// Token: 0x170003B6 RID: 950
	// (get) Token: 0x06001585 RID: 5509 RVA: 0x00068F39 File Offset: 0x00067139
	public bool IsFirst
	{
		get
		{
			return this.sectorIdx == 0;
		}
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x00068F44 File Offset: 0x00067144
	public void Init(FightingLine fightingLine)
	{
		this.fightingLine = fightingLine;
		this.point.Init(this);
		this.point.SetOwnedByTeam(this.point.OwnedByTeamDefault);
		this.point.OnCapturedByTeam += this.HandleCapturePointCaptured;
		foreach (ICombatEntity combatEntity in this.point.allies)
		{
			LazySingleton<FightingGameController>.Instance.RegisterTargetNonPersistent(combatEntity, fightingLine.lineIdx, this.sectorIdx, true);
		}
		foreach (ICombatEntity combatEntity2 in this.point.enemies)
		{
			LazySingleton<FightingGameController>.Instance.RegisterTargetNonPersistent(combatEntity2, fightingLine.lineIdx, this.sectorIdx, true);
		}
		if (this.sectorTrigger)
		{
			this.sectorTrigger.enabled = true;
		}
		Debug.Log(string.Format("Sector [{0}, {1}]: registered allies: {2}, enemies: {3}", new object[]
		{
			fightingLine.lineIdx,
			this.sectorIdx,
			this.point.allies.Count,
			this.point.enemies.Count
		}));
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000690C0 File Offset: 0x000672C0
	public void DeInit()
	{
		if (this.sectorTrigger)
		{
			this.sectorTrigger.enabled = false;
		}
		this.point.OnCapturedByTeam -= this.HandleCapturePointCaptured;
		this.point.DeInit();
		this.sectorIdx = -1;
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x0006910F File Offset: 0x0006730F
	public void SetActiveState(bool isActive)
	{
		base.gameObject.SetActive(isActive);
		this.point.SetActiveState(isActive);
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x0006912C File Offset: 0x0006732C
	private void HandleCapturePointCaptured(FightingCapturePoint capturePoint)
	{
		LazyConsts.Fighting.TeamType ownedByTeam = capturePoint.OwnedByTeam;
		if (ownedByTeam == LazyConsts.Fighting.TeamType.Player)
		{
			this.fightingLine.HandleSectorWasCaptured(this, LazyConsts.Fighting.TeamType.Player);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightSectorCaptured, string.Format("{0}:{1}:{2}", LazySingleton<FightingGameController>.Instance.CurrentLevelId, this.fightingLine.lineIdx, this.sectorIdx));
			return;
		}
		if (ownedByTeam != LazyConsts.Fighting.TeamType.WildZombie)
		{
			return;
		}
		this.fightingLine.HandleSectorWasCaptured(this, LazyConsts.Fighting.TeamType.WildZombie);
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.FightSectorLost, string.Format("{0}:{1}:{2}", LazySingleton<FightingGameController>.Instance.CurrentLevelId, this.fightingLine.lineIdx, this.sectorIdx));
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x000691D0 File Offset: 0x000673D0
	private void Awake()
	{
		this.fightingLine = base.GetComponentInParent<FightingLine>();
		if (!this.sectorTrigger)
		{
			this.sectorTrigger = base.GetComponent<Collider>();
		}
		if (this.sectorTrigger)
		{
			Debug.Log("Sector [" + base.name + "]: sectorTrigger disabled", this);
			this.sectorTrigger.enabled = false;
		}
		if (this.keepActiveOnAwake)
		{
			return;
		}
		this.SetActiveState(false);
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x00069248 File Offset: 0x00067448
	private void OnTriggerEnter(Collider other)
	{
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent != null && componentInParent.IsActiveCombatant)
		{
			LazySingleton<FightingGameController>.Instance.RegisterTargetNonPersistent(componentInParent, this.fightingLine.lineIdx, this.sectorIdx, true);
		}
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x00069284 File Offset: 0x00067484
	private void OnTriggerExit(Collider other)
	{
		ICombatEntity componentInParent = other.GetComponentInParent<ICombatEntity>();
		if (componentInParent != null)
		{
			LazySingleton<FightingGameController>.Instance.UpdateTargetLocation(componentInParent, this.fightingLine.lineIdx, -1);
		}
	}

	// Token: 0x0400160D RID: 5645
	public FightingCapturePoint point;

	// Token: 0x0400160E RID: 5646
	public Collider sectorTrigger;

	// Token: 0x0400160F RID: 5647
	[SerializeField]
	private bool keepActiveOnAwake;

	// Token: 0x04001610 RID: 5648
	public FightingLine fightingLine;

	// Token: 0x04001611 RID: 5649
	[NonSerialized]
	public int sectorIdx = -1;
}
