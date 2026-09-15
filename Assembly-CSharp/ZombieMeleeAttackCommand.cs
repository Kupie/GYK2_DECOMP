using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020002EF RID: 751
public class ZombieMeleeAttackCommand : ZombieAttackCommand
{
	// Token: 0x17000367 RID: 871
	// (get) Token: 0x060013CD RID: 5069 RVA: 0x00060DAC File Offset: 0x0005EFAC
	public DockPointData CustomDockPoint
	{
		get
		{
			return this.customDockPoint;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060013CE RID: 5070 RVA: 0x00060DB4 File Offset: 0x0005EFB4
	public override Vector2 DirectionToTarget
	{
		get
		{
			if (this.customDockPoint == null)
			{
				return base.DirectionToTarget;
			}
			return this.customDockPoint.Direction.ConvertToVector2XZ().normalized;
		}
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x00060DE8 File Offset: 0x0005EFE8
	public ZombieMeleeAttackCommand(float attackRange)
		: base(MobCommand.CommandType.ZombieMeleeAttack)
	{
		this.attackRange = attackRange;
	}

	// Token: 0x060013D0 RID: 5072 RVA: 0x00060E06 File Offset: 0x0005F006
	public override void OnStart()
	{
		base.OnStart();
		base.SetFacingDirection(this.DirectionToTarget, false);
	}

	// Token: 0x060013D1 RID: 5073 RVA: 0x00060E1C File Offset: 0x0005F01C
	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		this.hasDamage = true;
		global::UnityEngine.Object @object = base.TargetEntity as global::UnityEngine.Object;
		if (@object == null || @object == null || base.TargetEntity.CombatEntityHpComponent.Hp <= 0)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (this.customStopCondition != null && this.customStopCondition())
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		Wgo wgo = base.TargetEntity as Wgo;
		Vector3 vector = ((wgo != null) ? wgo.Data.Position : base.Position);
		DockPointData dockPointData = this.customDockPoint;
		Vector3 vector2 = ((dockPointData != null) ? dockPointData.GetPosFrom(vector) : base.Position);
		if ((base.Wgo.Data.Position - vector2).XZ2().magnitude > this.attackRange + 0.06666668f)
		{
			if (this.isAttackAnimPlaying)
			{
				this.hasDamage = false;
				return;
			}
			this.agent.StopCommandExecution(true);
			return;
		}
		else
		{
			if (this.isAttackAnimPlaying)
			{
				return;
			}
			if (base.ShouldPause(deltaTime))
			{
				return;
			}
			if (!this.isAttackAnimPlaying)
			{
				this.DoAttack();
			}
			return;
		}
	}

	// Token: 0x060013D2 RID: 5074 RVA: 0x00060F3B File Offset: 0x0005F13B
	public override void OnFinish()
	{
		base.OnFinish();
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		if (mainWgoPart == null)
		{
			return;
		}
		AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
		if (animationComponent == null)
		{
			return;
		}
		animationComponent.SetState(global::AnimationState.Idle);
	}

	// Token: 0x060013D3 RID: 5075 RVA: 0x00060F64 File Offset: 0x0005F164
	public void HandleAttackHit()
	{
		global::UnityEngine.Object @object = base.TargetEntity as global::UnityEngine.Object;
		if (@object == null || @object == null)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (base.TargetEntity.CombatEntityHpComponent.Hp <= 0)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (this.hasDamage && FightingWgoTarget.IsBarricadeOrTower(base.TargetEntity))
		{
			LazyAudio.PlayAtGameObject("zombie_hit_wood", this.agent.Wgo.transform, SpatialType.sound3D, true);
		}
		this.damage = (this.hasDamage ? Mathf.Clamp(this.damage - base.TargetEntity.ArmorValue, 0, int.MaxValue) : 0);
		if (this.damage == 0)
		{
			return;
		}
		if (base.TargetEntity is PlayerPhysicalBody)
		{
			LazyAudio.PlayAtGameObject("zombie_hit_player", this.agent.Wgo.transform, SpatialType.sound3D, true);
		}
		if (base.TargetEntity.IsActiveCombatant)
		{
			base.TargetEntity.CombatEntityHpComponent.ApplyDamage(this.damage);
		}
		Vector3 attackHitPosition = this.GetAttackHitPosition();
		Vector3 vector = this.agent.Wgo.Data.direction.Value.XZ();
		DamageEffectComponent.TryPlayEffect(base.TargetEntity.CombatEntityUID, attackHitPosition, vector, null);
		ICombatEntity wgo = this.agent.Wgo;
		LazyConsts.Fighting.TeamType teamType = this.agent.Wgo.TeamType;
		FighterDef fighterDef = this.agent.FighterDef;
		Weapon weapon = this.agent.Weapon;
		AttackContext attackContext = new AttackContext(wgo, teamType, fighterDef, (weapon != null) ? weapon.ItemDef : null, this.agent.Wgo.CombatEntityPosition, vector, attackHitPosition, -1, false, this.agent.AttackComponent, null);
		base.TargetEntity.OnOtherCombatTargetHitMe(attackContext);
	}

	// Token: 0x060013D4 RID: 5076 RVA: 0x00061118 File Offset: 0x0005F318
	private Vector3 GetAttackHitPosition()
	{
		Vector3 combatEntityPosition = this.agent.Wgo.CombatEntityPosition;
		Vector3 vector = (this.GetAttackHitTargetPoint() - combatEntityPosition).XZ();
		float magnitude = vector.magnitude;
		if (magnitude < 0.0001f)
		{
			return combatEntityPosition + Vector3.up * 0.5f;
		}
		float num = ((this.customDockPoint != null) ? magnitude : Mathf.Min(0.6f, magnitude));
		return combatEntityPosition + vector / magnitude * num + Vector3.up * 0.5f;
	}

	// Token: 0x060013D5 RID: 5077 RVA: 0x000611AC File Offset: 0x0005F3AC
	private Vector3 GetAttackHitTargetPoint()
	{
		if (this.customDockPoint != null)
		{
			Wgo wgo = base.TargetEntity as Wgo;
			if (wgo != null)
			{
				return wgo.Data.GetDockPointDataWorldPosition(this.customDockPoint) + this.customDockPoint.Direction.ConvertToVector3() * 0.6f;
			}
		}
		return base.TargetEntity.CombatEntityPosition;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x0006120C File Offset: 0x0005F40C
	private void DoAttack()
	{
		this.isAttackAnimPlaying = true;
		this.agent.AttackComponent.PerformAttackMelee(delegate
		{
			this.isAttackAnimPlaying = false;
			this.agent.StopCommandExecution(true);
		});
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x00061231 File Offset: 0x0005F431
	public ZombieMeleeAttackCommand WithCustomDockPoint(DockPointData dockPoint)
	{
		this.customDockPoint = dockPoint;
		return this;
	}

	// Token: 0x060013D8 RID: 5080 RVA: 0x0006123B File Offset: 0x0005F43B
	public ZombieMeleeAttackCommand WithDamage(int damage)
	{
		this.damage = damage;
		return this;
	}

	// Token: 0x040014FD RID: 5373
	private const float HitOffset = 0.6f;

	// Token: 0x040014FE RID: 5374
	private const float HitHeight = 0.5f;

	// Token: 0x040014FF RID: 5375
	private readonly float attackRange;

	// Token: 0x04001500 RID: 5376
	private DockPointData customDockPoint;

	// Token: 0x04001501 RID: 5377
	private int damage = 1;

	// Token: 0x04001502 RID: 5378
	private bool hasDamage = true;
}
