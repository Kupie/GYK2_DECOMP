using System;
using UnityEngine;

// Token: 0x020002B8 RID: 696
public class DebugDummyAgent : MonoBehaviour, ICombatEntity
{
	// Token: 0x170002E1 RID: 737
	// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00059115 File Offset: 0x00057315
	public SGuid CombatEntityUID
	{
		get
		{
			return this.combatEntityUid;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x060011C1 RID: 4545 RVA: 0x0005911D File Offset: 0x0005731D
	public LazyConsts.Fighting.TeamType TeamType
	{
		get
		{
			return this.teamType;
		}
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x060011C2 RID: 4546 RVA: 0x00028294 File Offset: 0x00026494
	public LazyConsts.Fighting.EntityType EntityType
	{
		get
		{
			return LazyConsts.Fighting.EntityType.None;
		}
	}

	// Token: 0x170002E4 RID: 740
	// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00059125 File Offset: 0x00057325
	public Vector3 CombatEntityPosition
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x060011C4 RID: 4548 RVA: 0x00059132 File Offset: 0x00057332
	public int ArmorValue { get; }

	// Token: 0x060011C5 RID: 4549 RVA: 0x0005913C File Offset: 0x0005733C
	public float GetCombatEntityDistance(Vector3 from, LazyConsts.Fighting.TeamType teamType)
	{
		return (from - this.CombatEntityPosition).magnitude;
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060011C6 RID: 4550 RVA: 0x0005915D File Offset: 0x0005735D
	public HPComponent CombatEntityHpComponent
	{
		get
		{
			return this.hpComponent;
		}
	}

	// Token: 0x170002E7 RID: 743
	// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00059165 File Offset: 0x00057365
	public int CombatEntityQuality
	{
		get
		{
			return this.combatEntityQuality;
		}
	}

	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0005916D File Offset: 0x0005736D
	// (set) Token: 0x060011C9 RID: 4553 RVA: 0x00059175 File Offset: 0x00057375
	public int AttackPriority
	{
		get
		{
			return this.attackPriority;
		}
		set
		{
			this.attackPriority = value;
		}
	}

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060011CA RID: 4554 RVA: 0x00028294 File Offset: 0x00026494
	public bool HasAnyDockPoint
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x060011CB RID: 4555 RVA: 0x0005917E File Offset: 0x0005737E
	// (set) Token: 0x060011CC RID: 4556 RVA: 0x00059186 File Offset: 0x00057386
	public bool IsActiveCombatant
	{
		get
		{
			return this.isActiveCombatant;
		}
		set
		{
			this.isActiveCombatant = value;
		}
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x0005918F File Offset: 0x0005738F
	private void Awake()
	{
		this.RefreshHpComponent();
		if (this.ensurePhysicsComponents)
		{
			this.EnsurePhysicsSetup();
		}
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x000591A5 File Offset: 0x000573A5
	private void Reset()
	{
		this.startHpValue = Mathf.Clamp(this.startHpValue, 0, this.maxHpValue);
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x000591BF File Offset: 0x000573BF
	private void OnDestroy()
	{
		FightingGameController fightingGameController = this.ownerController;
		if (fightingGameController == null)
		{
			return;
		}
		fightingGameController.OnDebugDummyDestroyed(this);
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x000591D4 File Offset: 0x000573D4
	public void Configure(LazyConsts.Fighting.TeamType newTeam, int newMaxHp, int newStartHp, int newQuality, int newPriority, bool newIsFightingMember)
	{
		this.teamType = newTeam;
		this.maxHpValue = Mathf.Max(1, newMaxHp);
		this.startHpValue = Mathf.Clamp(newStartHp, 0, this.maxHpValue);
		this.combatEntityQuality = Math.Max(0, newQuality);
		this.attackPriority = newPriority;
		this.isActiveCombatant = newIsFightingMember;
		this.combatEntityUid = new SGuid();
		this.RefreshHpComponent();
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x00059237 File Offset: 0x00057437
	public void AssignOwner(FightingGameController controller)
	{
		this.ownerController = controller;
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x00059240 File Offset: 0x00057440
	public void EnsureInitialized()
	{
		if (this.hpComponent == null)
		{
			this.RefreshHpComponent();
		}
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOtherCombatTargetReachedToMe(ICombatEntity other)
	{
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00002318 File Offset: 0x00000518
	public void OnOtherCombatTargetHitMe(AttackContext ctx)
	{
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x00059250 File Offset: 0x00057450
	public float GetCombatEntityGameRes(string resId)
	{
		return 0f;
	}

	// Token: 0x060011D6 RID: 4566 RVA: 0x00059257 File Offset: 0x00057457
	private void RefreshHpComponent()
	{
		this.startHpValue = Mathf.Clamp(this.startHpValue, 0, this.maxHpValue);
		this.hpComponent = new HPComponent(this.maxHpValue, this.startHpValue);
		this.hpComponent.Init(null, null);
	}

	// Token: 0x060011D7 RID: 4567 RVA: 0x00059298 File Offset: 0x00057498
	private void EnsurePhysicsSetup()
	{
		Collider collider;
		if (!base.TryGetComponent<Collider>(out collider))
		{
			CapsuleCollider capsuleCollider = base.gameObject.AddComponent<CapsuleCollider>();
			capsuleCollider.height = 1.8f;
			capsuleCollider.radius = 0.35f;
			capsuleCollider.center = Vector3.up * capsuleCollider.height * 0.5f;
		}
		Rigidbody rigidbody;
		if (!base.TryGetComponent<Rigidbody>(out rigidbody))
		{
			rigidbody = base.gameObject.AddComponent<Rigidbody>();
			rigidbody.isKinematic = true;
			rigidbody.useGravity = false;
		}
	}

	// Token: 0x040013A6 RID: 5030
	[SerializeField]
	private SGuid combatEntityUid = new SGuid();

	// Token: 0x040013A7 RID: 5031
	[SerializeField]
	private LazyConsts.Fighting.TeamType teamType;

	// Token: 0x040013A8 RID: 5032
	[SerializeField]
	[Min(1f)]
	private int maxHpValue = 10;

	// Token: 0x040013A9 RID: 5033
	[SerializeField]
	[Min(0f)]
	private int startHpValue = 10;

	// Token: 0x040013AA RID: 5034
	[SerializeField]
	[Min(0f)]
	private int combatEntityQuality = 1;

	// Token: 0x040013AB RID: 5035
	[SerializeField]
	private int attackPriority = 20;

	// Token: 0x040013AC RID: 5036
	[SerializeField]
	private bool isActiveCombatant = true;

	// Token: 0x040013AD RID: 5037
	[SerializeField]
	private bool ensurePhysicsComponents = true;

	// Token: 0x040013AE RID: 5038
	private HPComponent hpComponent;

	// Token: 0x040013AF RID: 5039
	private FightingGameController ownerController;
}
