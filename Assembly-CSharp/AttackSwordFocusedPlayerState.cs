using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class AttackSwordFocusedPlayerState : SSMState, IStaminaConsumer
{
	// Token: 0x17000457 RID: 1111
	// (get) Token: 0x06001955 RID: 6485 RVA: 0x0007819D File Offset: 0x0007639D
	public override bool IsActive
	{
		get
		{
			return this.isInFocus;
		}
	}

	// Token: 0x17000458 RID: 1112
	// (get) Token: 0x06001956 RID: 6486 RVA: 0x000781A5 File Offset: 0x000763A5
	public Direction FocusedDirection
	{
		get
		{
			return this.focusedDirection;
		}
	}

	// Token: 0x06001957 RID: 6487 RVA: 0x000781AD File Offset: 0x000763AD
	public AttackSwordFocusedPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000781CC File Offset: 0x000763CC
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.isInFocus = true;
		this.focusedDirection = this.playerController.PlayerData.Direction.ConvertFromVector2();
		this.animator = this.playerController.View.PlayerAnimation.Animator;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 1f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 1f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.StanceWalk, 1f);
		this.animator.SetBool(AttackSwordFocusedPlayerState.AttackFocus, true);
		this.animator.SetFloat(AttackSwordFocusedPlayerState.WalkSpeed, this.walkAnimationSpeed);
		this.playerController.PlayerData.SetDirectionLock(false);
		this.playerController.PhysicalBody.SetDirectionLock(false);
		this.playerController.PhysicalBody.SpeedMultiplier = this.movementSpeedMultiplier;
		Debug.Log("Entering AttackSwordFocusedPlayerState");
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000782E4 File Offset: 0x000764E4
	public override void Update()
	{
		if (!this.playerController.IsControlsEnabled)
		{
			this.isInFocus = false;
		}
		this.TryFollowCursorInMouseStance();
		float num = Mathf.Atan2(LazyInput.GetDirection().y, LazyInput.GetDirection().x) * 57.29578f;
		this.animator.SetFloat("MovementDirection", num);
		if (PlayerInputHandler.IsAttackDown() && this.isInFocus)
		{
			this.TryDoAttack();
		}
		float x = this.playerController.MovableDirection.x;
		float @float = this.animator.GetFloat(AttackSwordFocusedPlayerState.WalkSpeed);
		if (!Mathf.Sign(x).EqualsTo(Mathf.Sign(@float), 1E-05f) && x != 0f)
		{
			float num2 = Mathf.Abs(this.walkAnimationSpeed) * Mathf.Sign(x);
			this.animator.SetFloat(AttackSwordFocusedPlayerState.WalkSpeed, num2);
		}
		if (!PlayerInputHandler.IsAttackFocusHeld())
		{
			this.isInFocus = false;
		}
		if (this.playerController.IsControlsEnabled)
		{
			this.playerController.PlayerInputHandler.UpdateHotBarInteraction();
		}
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000776BE File Offset: 0x000758BE
	public override void FixedUpdate()
	{
		if (this.playerController.IsControlsEnabled)
		{
			this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x000783E4 File Offset: 0x000765E4
	private bool TryDoAttack()
	{
		if (!this.staminaSystem.CanPerformAttack())
		{
			return false;
		}
		this.playerController.AttackComponent.PerformAttackByTrigger(false, default(Vector3), "attack", null, true);
		return true;
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x00078424 File Offset: 0x00076624
	private void TryFollowCursorInMouseStance()
	{
		if (!LazyInput.GetKey(GameKey.RightClick))
		{
			return;
		}
		Vector2 vector;
		if (!MouseAimHelper.TryGetAimDirection(this.playerController.PhysicalBody.transform.position, out vector))
		{
			return;
		}
		this.playerController.PhysicalBody.SetAimDirection(vector);
		this.focusedDirection = vector.ConvertFromVector2();
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x0007847C File Offset: 0x0007667C
	public override void OnExit()
	{
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 0f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 0f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.StanceWalk, 0f);
		Animator animator = this.playerController.View.PlayerAnimation.Animator;
		animator.SetBool(AttackSwordFocusedPlayerState.AttackFocus, false);
		animator.SetFloat(AttackSwordFocusedPlayerState.WalkSpeed, 1f);
		this.playerController.PlayerData.SetDirectionLock(true);
		this.playerController.PhysicalBody.SetDirectionLock(true);
		this.playerController.PhysicalBody.SpeedMultiplier = 1f;
		Debug.Log("Exiting AttackSwordFocusedPlayerState");
	}

	// Token: 0x040018B8 RID: 6328
	private const string ATTACK_TRIGGER = "attack";

	// Token: 0x040018B9 RID: 6329
	private const string MOVEMENT_DIR = "MovementDirection";

	// Token: 0x040018BA RID: 6330
	private static readonly int WalkSpeed = Animator.StringToHash("walk_speed");

	// Token: 0x040018BB RID: 6331
	private static readonly int AttackFocus = Animator.StringToHash("attack_focus");

	// Token: 0x040018BC RID: 6332
	private float movementSpeedMultiplier = 0.5f;

	// Token: 0x040018BD RID: 6333
	private float walkAnimationSpeed = 0.5f;

	// Token: 0x040018BE RID: 6334
	private bool isInFocus;

	// Token: 0x040018BF RID: 6335
	private Animator animator;

	// Token: 0x040018C0 RID: 6336
	private Direction focusedDirection;

	// Token: 0x040018C1 RID: 6337
	private StaminaSystem staminaSystem;
}
