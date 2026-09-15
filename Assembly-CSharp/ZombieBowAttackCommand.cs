using System;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020002EE RID: 750
public class ZombieBowAttackCommand : ZombieAttackCommand
{
	// Token: 0x060013C4 RID: 5060 RVA: 0x0006090B File Offset: 0x0005EB0B
	public ZombieBowAttackCommand()
		: base(MobCommand.CommandType.ZombieBowAttack)
	{
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x00060914 File Offset: 0x0005EB14
	public override void OnStart()
	{
		base.OnStart();
		this.attackRange = (float)this.agent.FighterDef.atkRange.EvaluateInt(this.agent.Wgo);
		base.SetDirectionToTarget();
		if (base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent4 == null)
		{
			base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent4 = new UnityEvent();
		}
		if (base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent7 == null)
		{
			base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent7 = new UnityEvent();
		}
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x000609CC File Offset: 0x0005EBCC
	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
		if (base.TargetEntity != null && this.agent.AttackComponent.weapon && !AgentAI.TryLineCastByRecast(this.agent.AttackComponent.weapon.transform.position, base.TargetEntity.CombatEntityPosition))
		{
			this.agent.StopCommandExecution(true);
			if (this.isAttackAnimPlaying)
			{
				this.isAttackAnimPlaying = false;
				this.agent.AttackComponent.CancelAttack();
			}
			return;
		}
		if (base.TargetEntity == null || base.TargetEntity.CombatEntityHpComponent.Hp <= 0)
		{
			if (this.isAttackAnimPlaying)
			{
				this.isAttackAnimPlaying = false;
				this.agent.AttackComponent.CancelAttack();
			}
			this.agent.StopCommandExecution(true);
			return;
		}
		if (this.isAttackAnimPlaying)
		{
			return;
		}
		if (this.customStopCondition != null && this.customStopCondition())
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if ((base.Wgo.Data.Position - base.Position).XZ2().magnitude > this.attackRange)
		{
			this.agent.StopCommandExecution(true);
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
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x00060B20 File Offset: 0x0005ED20
	public override void OnFinish()
	{
		base.OnFinish();
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		if (mainWgoPart != null)
		{
			AnimationComponentBase animationComponent = mainWgoPart.AnimationComponent;
			if (animationComponent != null)
			{
				animationComponent.CancelBowAimLoop();
			}
		}
		this.isAttackAnimPlaying = false;
		WgoPart mainWgoPart2 = base.Wgo.MainWgoPart;
		if (mainWgoPart2 != null)
		{
			AnimationComponentBase animationComponent2 = mainWgoPart2.AnimationComponent;
			if (animationComponent2 != null)
			{
				animationComponent2.SetState(global::AnimationState.Idle);
			}
		}
		WgoPart mainWgoPart3 = base.Wgo.MainWgoPart;
		if (mainWgoPart3 != null)
		{
			AnimationComponentBase animationComponent3 = mainWgoPart3.AnimationComponent;
			if (animationComponent3 != null)
			{
				AnimationEventReceiver animationEventReceiver = animationComponent3.AnimationEventReceiver;
				if (animationEventReceiver != null)
				{
					UnityEvent onEvent = animationEventReceiver.onEvent4;
					if (onEvent != null)
					{
						onEvent.RemoveAllListeners();
					}
				}
			}
		}
		WgoPart mainWgoPart4 = base.Wgo.MainWgoPart;
		if (mainWgoPart4 == null)
		{
			return;
		}
		AnimationComponentBase animationComponent4 = mainWgoPart4.AnimationComponent;
		if (animationComponent4 == null)
		{
			return;
		}
		AnimationEventReceiver animationEventReceiver2 = animationComponent4.AnimationEventReceiver;
		if (animationEventReceiver2 == null)
		{
			return;
		}
		UnityEvent onEvent2 = animationEventReceiver2.onEvent7;
		if (onEvent2 == null)
		{
			return;
		}
		onEvent2.RemoveAllListeners();
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x0005BAA9 File Offset: 0x00059CA9
	private Vector3 GetAimPosition(Vector3 targetPosition)
	{
		return targetPosition + Vector3.up * 1.666667f / 2f;
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x00060BE8 File Offset: 0x0005EDE8
	private void DoAttack()
	{
		this.isAttackAnimPlaying = true;
		this.agent.AttackComponent.PerformAttack(false, default(Vector3), true, delegate
		{
			if (!this.isAttackAnimPlaying)
			{
				return;
			}
			this.isAttackAnimPlaying = false;
			this.agent.StopCommandExecution(true);
		}, false);
		base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent4.AddListener(new UnityAction(this.EmitArrow));
		base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent7.AddListener(new UnityAction(this.HandleAnimationFinish));
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x00060C7C File Offset: 0x0005EE7C
	private void HandleAnimationFinish()
	{
		this.agent.AttackComponent.OnAttackAnimFinished(base.Wgo.MainWgoPart.AnimationComponent);
		base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent7.RemoveAllListeners();
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x00060CC8 File Offset: 0x0005EEC8
	private void EmitArrow()
	{
		base.Wgo.MainWgoPart.AnimationComponent.AnimationEventReceiver.onEvent4.RemoveAllListeners();
		BowWeapon bowWeapon = this.agent.AttackComponent.weapon as BowWeapon;
		if (bowWeapon == null)
		{
			this.agent.StopCommandExecution(false);
			return;
		}
		if (base.TargetEntity != null && base.TargetEntity.CombatEntityHpComponent.Hp > 0)
		{
			Vector3 normalized = (this.GetAimPosition(base.TargetEntity.CombatEntityPosition) - bowWeapon.transform.position).normalized;
			LazyAudio.PlayAtGameObject("bow_aim_shot", this.agent.transform, SpatialType.sound3D, true);
			this.agent.AttackComponent.ActivateWeapon(normalized);
		}
	}

	// Token: 0x040014FC RID: 5372
	private float attackRange;
}
