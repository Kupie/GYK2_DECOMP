using System;
using UnityEngine;

// Token: 0x020002F0 RID: 752
public class ZombiePikeAttackCommand : ZombieAttackCommand
{
	// Token: 0x060013DA RID: 5082 RVA: 0x0006125A File Offset: 0x0005F45A
	public ZombiePikeAttackCommand()
		: base(MobCommand.CommandType.ZombiePikeAttack)
	{
	}

	// Token: 0x060013DB RID: 5083 RVA: 0x00061263 File Offset: 0x0005F463
	public override void OnStart()
	{
		base.OnStart();
		base.SetFacingDirection((base.Position - base.Wgo.Data.Position).XZ2(), false);
	}

	// Token: 0x060013DC RID: 5084 RVA: 0x00061294 File Offset: 0x0005F494
	public override void OnUpdate(float deltaTime)
	{
		if (this.isAttackAnimPlaying)
		{
			this.attackAnimElapsed += deltaTime;
			if (this.attackAnimElapsed >= 1f)
			{
				this.isAttackAnimPlaying = false;
				this.agent.StopCommandExecution(true);
			}
			return;
		}
		base.OnUpdate(deltaTime);
		if (base.TargetEntity == null || base.TargetEntity.CombatEntityHpComponent.Hp <= 0)
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (this.customStopCondition != null && this.customStopCondition())
		{
			this.agent.StopCommandExecution(true);
			return;
		}
		if (!PikeCombatGeometry.IsAlignedForStrike(this.agent, base.Position, 0.55f))
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

	// Token: 0x060013DD RID: 5085 RVA: 0x0006135E File Offset: 0x0005F55E
	public override void OnFinish()
	{
		this.UnsubscribeSpearAttack();
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

	// Token: 0x060013DE RID: 5086 RVA: 0x0006138C File Offset: 0x0005F58C
	private void DoAttack()
	{
		this.isAttackAnimPlaying = true;
		this.attackAnimElapsed = 0f;
		base.SetFacingDirection((base.Position - base.Wgo.Data.Position).XZ2(), false);
		this.SubscribeSpearAttack();
		this.agent.AttackComponent.PerformAttack(false, default(Vector3), true, delegate
		{
			this.isAttackAnimPlaying = false;
			this.UnsubscribeSpearAttack();
			this.agent.StopCommandExecution(true);
		}, true);
	}

	// Token: 0x060013DF RID: 5087 RVA: 0x00061400 File Offset: 0x0005F600
	private void SubscribeSpearAttack()
	{
		this.UnsubscribeSpearAttack();
		WgoPart mainWgoPart = base.Wgo.MainWgoPart;
		this.subscribedAnimation = ((mainWgoPart != null) ? mainWgoPart.AnimationComponent : null);
		if (this.subscribedAnimation != null)
		{
			this.subscribedAnimation.SpearAttackFired += this.HandleSpearAttackFired;
		}
	}

	// Token: 0x060013E0 RID: 5088 RVA: 0x00061455 File Offset: 0x0005F655
	private void UnsubscribeSpearAttack()
	{
		if (this.subscribedAnimation == null)
		{
			return;
		}
		this.subscribedAnimation.SpearAttackFired -= this.HandleSpearAttackFired;
		this.subscribedAnimation = null;
	}

	// Token: 0x060013E1 RID: 5089 RVA: 0x00061484 File Offset: 0x0005F684
	private void HandleSpearAttackFired()
	{
		if (!this.isAttackAnimPlaying)
		{
			return;
		}
		AttackComponent attackComponent = this.agent.AttackComponent;
		Weapon weapon = ((attackComponent != null) ? attackComponent.weapon : null);
		if (weapon == null)
		{
			return;
		}
		Vector3 vector = base.Wgo.Data.Position + Vector3.up * 0.5f;
		Vector3 vector2 = base.Wgo.Data.direction.Value.XZ();
		float num = PikeCombatGeometry.EffectiveReach(this.agent);
		foreach (IDamageDealer damageDealer in weapon.Dealers)
		{
			PikeHitBox pikeHitBox = damageDealer as PikeHitBox;
			if (pikeHitBox != null)
			{
				pikeHitBox.SweepForTargets(vector, vector2, num, 0.4f);
			}
		}
	}

	// Token: 0x04001503 RID: 5379
	private const float ATTACK_ANIM_FALLBACK_DURATION = 1f;

	// Token: 0x04001504 RID: 5380
	private float attackAnimElapsed;

	// Token: 0x04001505 RID: 5381
	private AnimationComponentBase subscribedAnimation;
}
