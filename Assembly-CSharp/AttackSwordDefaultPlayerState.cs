using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003AA RID: 938
public class AttackSwordDefaultPlayerState : SSMState
{
	// Token: 0x0600194C RID: 6476 RVA: 0x000775CB File Offset: 0x000757CB
	public AttackSwordDefaultPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x0600194D RID: 6477 RVA: 0x00078009 File Offset: 0x00076209
	public override bool IsActive
	{
		get
		{
			return this.isAttacking;
		}
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x00078014 File Offset: 0x00076214
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.animator = this.playerController.View.PlayerAnimation.Animator;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 1f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 1f);
		this.isAttacking = this.TryDoAttack();
		Debug.Log("Entering AttackSwordDefaultPlayerState");
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x0007809A File Offset: 0x0007629A
	public override void Update()
	{
		if (PlayerInputHandler.IsAttackDown() && this.isAttacking)
		{
			if (LazyInput.GetKeyDown(GameKey.LeftClick))
			{
				MouseAimHelper.TryApplyAim(this.playerController.PhysicalBody);
			}
			this.TryDoAttack();
		}
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x000776BE File Offset: 0x000758BE
	public override void FixedUpdate()
	{
		if (this.playerController.IsControlsEnabled)
		{
			this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
		}
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000780D0 File Offset: 0x000762D0
	private bool TryDoAttack()
	{
		if (!this.staminaSystem.CanPerformAttack())
		{
			this.isAttacking = false;
			return false;
		}
		AttackComponent attackComponent = this.playerController.AttackComponent;
		bool flag = false;
		Action action = new Action(this.OnAttackFinished);
		attackComponent.PerformAttackByTrigger(flag, default(Vector3), "attack", action, true);
		return true;
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x00078122 File Offset: 0x00076322
	private void OnAttackFinished()
	{
		this.isAttacking = false;
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x0007812C File Offset: 0x0007632C
	public override void OnExit()
	{
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 0f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 0f);
		this.animator.SetBool(AttackSwordDefaultPlayerState.AttackFocus, false);
		Debug.Log("Exiting AttackSwordDefaultPlayerState");
	}

	// Token: 0x040018B3 RID: 6323
	private const string ATTACK_TRIGGER = "attack";

	// Token: 0x040018B4 RID: 6324
	private static readonly int AttackFocus = Animator.StringToHash("attack_focus");

	// Token: 0x040018B5 RID: 6325
	private bool isAttacking;

	// Token: 0x040018B6 RID: 6326
	private Animator animator;

	// Token: 0x040018B7 RID: 6327
	private StaminaSystem staminaSystem;
}
