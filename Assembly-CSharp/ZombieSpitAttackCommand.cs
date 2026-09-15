using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002F1 RID: 753
public class ZombieSpitAttackCommand : ZombieAttackCommand
{
	// Token: 0x060013E3 RID: 5091 RVA: 0x0006157F File Offset: 0x0005F77F
	public ZombieSpitAttackCommand(float attackRange)
		: base(MobCommand.CommandType.ZombieSpitAttack)
	{
		this.attackRange = attackRange;
	}

	// Token: 0x060013E4 RID: 5092 RVA: 0x00061590 File Offset: 0x0005F790
	public override void OnStart()
	{
		base.OnStart();
		base.SetFacingDirection(this.DirectionToTarget, false);
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		AnimationEventReceiver animationEventReceiver;
		if (mainWgoPart == null)
		{
			animationEventReceiver = null;
		}
		else
		{
			AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
			animationEventReceiver = ((animationComponent != null) ? animationComponent.AnimationEventReceiver : null);
		}
		AnimationEventReceiver animationEventReceiver2 = animationEventReceiver;
		if (animationEventReceiver2 == null)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (animationEventReceiver2.onEvent4 == null)
		{
			animationEventReceiver2.onEvent4 = new UnityEvent();
		}
		if (animationEventReceiver2.onEvent7 == null)
		{
			animationEventReceiver2.onEvent7 = new UnityEvent();
		}
		this.ClearAnimationEventListeners();
	}

	// Token: 0x060013E5 RID: 5093 RVA: 0x00061618 File Offset: 0x0005F818
	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (base.TargetEntity == null || base.TargetEntity.CombatEntityHpComponent.Hp <= 0)
		{
			this.StopAttackAndCommand();
			return;
		}
		if ((base.Wgo.Data.Position - base.TargetEntity.CombatEntityPosition).XZ().magnitude > this.attackRange + 0.06666668f)
		{
			this.StopAttackAndCommand();
			return;
		}
		Transform transform = (this.agent.AttackComponent.weapon ? this.agent.AttackComponent.weapon.transform : this.agent.transform);
		if (!FightingWgoTarget.IsBarricadeOrTower(base.TargetEntity) && !AgentAI.TryLineCastByRecast(transform.position, base.TargetEntity.CombatEntityPosition + Vector3.up * 0.5f))
		{
			this.StopAttackAndCommand();
			return;
		}
		if (this.isAttackAnimPlaying)
		{
			if (!this.spitEmitted && Time.time - this.attackAnimStartTime >= 0.75f)
			{
				this.EmitSpit();
			}
			return;
		}
		if (this.customStopCondition != null && this.customStopCondition())
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (base.ShouldPause(deltaTime))
		{
			return;
		}
		this.DoAttack();
	}

	// Token: 0x060013E6 RID: 5094 RVA: 0x00061766 File Offset: 0x0005F966
	public override void OnFinish()
	{
		base.OnFinish();
		this.ClearAnimationEventListeners();
		this.isAttackAnimPlaying = false;
		this.spitEmitted = false;
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

	// Token: 0x060013E7 RID: 5095 RVA: 0x000617A2 File Offset: 0x0005F9A2
	public ZombieSpitAttackCommand WithDebugLogs(bool enabled)
	{
		this.debugLogs = enabled;
		return this;
	}

	// Token: 0x060013E8 RID: 5096 RVA: 0x000617AC File Offset: 0x0005F9AC
	private void DoAttack()
	{
		if (!this.agent.AttackComponent.weapon)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		this.isAttackAnimPlaying = true;
		this.spitEmitted = false;
		this.attackAnimStartTime = Time.time;
		this.agent.AttackComponent.PerformAttack(false, default(Vector3), true, delegate
		{
			if (!this.isAttackAnimPlaying)
			{
				return;
			}
			this.isAttackAnimPlaying = false;
			this.agent.StopCommandExecution(true);
		}, false);
		LazyAudio.PlayAtGameObject("spitter_attack", base.Wgo.transform, SpatialType.sound3D, true);
		AnimationEventReceiver animationEventReceiver = base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver;
		animationEventReceiver.onEvent4.AddListener(new UnityAction(this.EmitSpit));
		animationEventReceiver.onEvent7.AddListener(new UnityAction(this.HandleAnimationFinish));
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x00061878 File Offset: 0x0005FA78
	private void EmitSpit()
	{
		if (this.spitEmitted)
		{
			return;
		}
		this.spitEmitted = true;
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		object obj;
		if (mainWgoPart == null)
		{
			obj = null;
		}
		else
		{
			AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
			obj = ((animationComponent != null) ? animationComponent.AnimationEventReceiver : null);
		}
		object obj2 = obj;
		if (obj2 != null)
		{
			UnityEvent onEvent = obj2.onEvent4;
			if (onEvent != null)
			{
				onEvent.RemoveListener(new UnityAction(this.EmitSpit));
			}
		}
		if (this.agent.AttackComponent.weapon == null)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (base.TargetEntity == null || base.TargetEntity.CombatEntityHpComponent.Hp <= 0)
		{
			return;
		}
		float num = 0.8333335f;
		if (FightingWgoTarget.IsBarricadeOrTower(base.TargetEntity))
		{
			num *= 0.5f;
		}
		Vector3 normalized = (base.TargetEntity.CombatEntityPosition + Vector3.up * num - this.agent.AttackComponent.weapon.transform.position).normalized;
		if (normalized.sqrMagnitude <= 0f)
		{
			return;
		}
		this.agent.AttackComponent.ActivateWeapon(normalized);
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x0006199C File Offset: 0x0005FB9C
	private void HandleAnimationFinish()
	{
		if (!this.isAttackAnimPlaying)
		{
			return;
		}
		base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent7.RemoveListener(new UnityAction(this.HandleAnimationFinish));
		this.agent.AttackComponent.OnAttackAnimFinished(base.Wgo.MainWgoPart.AnimationComponent);
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x000619FD File Offset: 0x0005FBFD
	private void StopAttackAndCommand()
	{
		if (this.isAttackAnimPlaying)
		{
			this.isAttackAnimPlaying = false;
			this.spitEmitted = false;
			this.agent.AttackComponent.CancelAttack();
		}
		this.ClearAnimationEventListeners();
		this.agent.StopCommandExecution(true);
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x00061A38 File Offset: 0x0005FC38
	private void ClearAnimationEventListeners()
	{
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		AnimationEventReceiver animationEventReceiver;
		if (mainWgoPart == null)
		{
			animationEventReceiver = null;
		}
		else
		{
			AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
			animationEventReceiver = ((animationComponent != null) ? animationComponent.AnimationEventReceiver : null);
		}
		AnimationEventReceiver animationEventReceiver2 = animationEventReceiver;
		if (animationEventReceiver2 == null)
		{
			return;
		}
		UnityEvent onEvent = animationEventReceiver2.onEvent4;
		if (onEvent != null)
		{
			onEvent.RemoveListener(new UnityAction(this.EmitSpit));
		}
		UnityEvent onEvent2 = animationEventReceiver2.onEvent7;
		if (onEvent2 == null)
		{
			return;
		}
		onEvent2.RemoveListener(new UnityAction(this.HandleAnimationFinish));
	}

	// Token: 0x04001506 RID: 5382
	private const float SPIT_EMIT_FALLBACK_DELAY = 0.75f;

	// Token: 0x04001507 RID: 5383
	private float attackRange;

	// Token: 0x04001508 RID: 5384
	private bool spitEmitted;

	// Token: 0x04001509 RID: 5385
	private float attackAnimStartTime;

	// Token: 0x0400150A RID: 5386
	private bool debugLogs;
}
