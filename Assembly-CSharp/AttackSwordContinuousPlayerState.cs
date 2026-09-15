using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class AttackSwordContinuousPlayerState : SSMState
{
	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06001944 RID: 6468 RVA: 0x00077D42 File Offset: 0x00075F42
	public override bool IsActive
	{
		get
		{
			return this.isAttacking;
		}
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x00077D4A File Offset: 0x00075F4A
	public AttackSwordContinuousPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x00077D6C File Offset: 0x00075F6C
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.isAttacking = true;
		this.animator = this.playerController.View.PlayerAnimation.Animator;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 1f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 1f);
		this.animator.SetFloat(AttackSwordContinuousPlayerState.WalkSpeed, this.walkAnimationSpeed);
		this.playerController.PlayerData.SetDirectionLock(false);
		this.playerController.PhysicalBody.SetDirectionLock(false);
		this.playerController.PhysicalBody.SpeedMultiplier = this.movementSpeedMultiplier;
		this.isAttackAnimPlaying = true;
		AttackComponent attackComponent = this.playerController.AttackComponent;
		bool flag = false;
		Action action = new Action(this.OnAttackFinished);
		attackComponent.PerformAttackByTrigger(flag, default(Vector3), "attack", action, true);
		Debug.Log("Entering ContinuousPlayerState");
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x00077E70 File Offset: 0x00076070
	public override void Update()
	{
		if (!LazyInput.GetKey(GameKey.Attack) && !this.isAttackAnimPlaying)
		{
			this.isAttacking = false;
			return;
		}
		if (!this.isAttackAnimPlaying && this.staminaSystem.CanPerformAttack())
		{
			this.playerController.PlayerData.SetDirectionLock(false);
			this.playerController.PhysicalBody.SetDirectionLock(false);
			this.isAttackAnimPlaying = true;
			AttackComponent attackComponent = this.playerController.AttackComponent;
			bool flag = false;
			Action action = new Action(this.OnAttackFinished);
			attackComponent.PerformAttackByTrigger(flag, default(Vector3), "attack", action, true);
		}
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x00077477 File Offset: 0x00075677
	public override void FixedUpdate()
	{
		this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x00077F05 File Offset: 0x00076105
	private void OnAttackFinished()
	{
		this.isAttackAnimPlaying = false;
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x00077F10 File Offset: 0x00076110
	public override void OnExit()
	{
		this.playerController.AttackComponent.OnAttackAnimFinished(this.playerController.View.PlayerAnimation);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 0f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 0f);
		this.playerController.View.PlayerAnimation.Animator.SetFloat(AttackSwordContinuousPlayerState.WalkSpeed, 1f);
		Debug.Log("Exiting ContinuousPlayerState");
		this.playerController.PlayerData.SetDirectionLock(true);
		this.playerController.PhysicalBody.SetDirectionLock(true);
		this.playerController.PhysicalBody.SpeedMultiplier = 1f;
	}

	// Token: 0x040018A9 RID: 6313
	private const string ATTACK_TRIGGER = "attack";

	// Token: 0x040018AA RID: 6314
	private static readonly int WalkSpeed = Animator.StringToHash("walk_speed");

	// Token: 0x040018AB RID: 6315
	private static readonly int AttackFocus = Animator.StringToHash("attack_focus");

	// Token: 0x040018AC RID: 6316
	private static readonly int AttackSpeed = Animator.StringToHash("attack_speed");

	// Token: 0x040018AD RID: 6317
	private float movementSpeedMultiplier = 0.5f;

	// Token: 0x040018AE RID: 6318
	private float walkAnimationSpeed = 0.5f;

	// Token: 0x040018AF RID: 6319
	private bool isAttacking;

	// Token: 0x040018B0 RID: 6320
	private bool isAttackAnimPlaying;

	// Token: 0x040018B1 RID: 6321
	private Animator animator;

	// Token: 0x040018B2 RID: 6322
	private StaminaSystem staminaSystem;
}
