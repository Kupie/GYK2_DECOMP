using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003A5 RID: 933
public class AttackBowAutoPlayerState : SSMState
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x0600191F RID: 6431 RVA: 0x00077289 File Offset: 0x00075489
	public override bool IsActive
	{
		get
		{
			return this.isHoldingKey;
		}
	}

	// Token: 0x06001920 RID: 6432 RVA: 0x00077291 File Offset: 0x00075491
	public AttackBowAutoPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x06001921 RID: 6433 RVA: 0x000772B0 File Offset: 0x000754B0
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.wasShoot = false;
		this.isHoldingKey = true;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.BowAttack, 1f);
		this.animator = this.playerController.View.PlayerAnimation.Animator;
		this.animator.SetBool(AttackBowAutoPlayerState.AttackFocus, true);
		this.animator.SetBool(AttackBowAutoPlayerState.BowPrepare, true);
		this.animator.SetFloat(AttackBowAutoPlayerState.WalkSpeed, this.walkAnimationSpeed);
		this.playerController.PlayerData.SetDirectionLock(false);
		this.playerController.PhysicalBody.SetDirectionLock(false);
		this.playerController.PhysicalBody.SpeedMultiplier = this.movementSpeedMultiplier;
		Debug.Log("Entering AttackBowDefaultPlayerState");
	}

	// Token: 0x06001922 RID: 6434 RVA: 0x0007738C File Offset: 0x0007558C
	public override void Update()
	{
		float x = this.playerController.MovableDirection.x;
		float @float = this.animator.GetFloat(AttackBowAutoPlayerState.WalkSpeed);
		if (!Mathf.Sign(x).EqualsTo(Mathf.Sign(@float), 1E-05f) && x != 0f)
		{
			float num = Mathf.Abs(this.walkAnimationSpeed) * Mathf.Sign(x);
			this.animator.SetFloat(AttackBowAutoPlayerState.WalkSpeed, num);
		}
		if (!this.animator.GetBool(AttackBowAutoPlayerState.BowPrepare) && this.staminaSystem.CanPerformAttack())
		{
			this.animator.SetBool(AttackBowAutoPlayerState.BowPrepare, true);
		}
		if (!this.animator.GetBool(AttackBowAutoPlayerState.BowShoot) && this.animator.GetBool(AttackBowAutoPlayerState.BowPrepare))
		{
			this.animator.SetBool(AttackBowAutoPlayerState.BowShoot, true);
		}
		if (!LazyInput.GetKey(GameKey.Attack))
		{
			this.isHoldingKey = false;
		}
	}

	// Token: 0x06001923 RID: 6435 RVA: 0x00077477 File Offset: 0x00075677
	public override void FixedUpdate()
	{
		this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
	}

	// Token: 0x06001924 RID: 6436 RVA: 0x00077490 File Offset: 0x00075690
	public override void OnExit()
	{
		this.staminaSystem.BeginRegenerationDelayIfSuspended();
		this.wasShoot = false;
		this.playerController.AttackComponent.OnAttackAnimFinished(this.playerController.View.PlayerAnimation);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.BowAttack, 0f);
		this.animator.SetBool(AttackBowAutoPlayerState.AttackFocus, false);
		this.animator.SetBool(AttackBowAutoPlayerState.BowPrepare, false);
		this.animator.SetBool(AttackBowAutoPlayerState.BowShoot, false);
		this.animator.SetFloat(AttackBowAutoPlayerState.WalkSpeed, 1f);
		this.playerController.PlayerData.SetDirectionLock(true);
		this.playerController.PhysicalBody.SetDirectionLock(true);
		this.playerController.PhysicalBody.SpeedMultiplier = 1f;
		Debug.Log("Exiting AttackBowDefaultPlayerState");
	}

	// Token: 0x04001885 RID: 6277
	private static readonly int BowShoot = Animator.StringToHash("bow_shoot");

	// Token: 0x04001886 RID: 6278
	private static readonly int BowPrepare = Animator.StringToHash("bow_preparing");

	// Token: 0x04001887 RID: 6279
	private static readonly int AttackFocus = Animator.StringToHash("attack_focus");

	// Token: 0x04001888 RID: 6280
	private static readonly int WalkSpeed = Animator.StringToHash("walk_speed");

	// Token: 0x04001889 RID: 6281
	private float movementSpeedMultiplier = 0.5f;

	// Token: 0x0400188A RID: 6282
	private float walkAnimationSpeed = 0.5f;

	// Token: 0x0400188B RID: 6283
	private bool isHoldingKey;

	// Token: 0x0400188C RID: 6284
	private bool wasShoot;

	// Token: 0x0400188D RID: 6285
	private Animator animator;

	// Token: 0x0400188E RID: 6286
	private StaminaSystem staminaSystem;
}
