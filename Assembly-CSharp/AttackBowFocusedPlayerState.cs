using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003A7 RID: 935
public class AttackBowFocusedPlayerState : SSMState, IStaminaConsumer
{
	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06001930 RID: 6448 RVA: 0x000777ED File Offset: 0x000759ED
	public override bool IsActive
	{
		get
		{
			return this.isInFocus;
		}
	}

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06001931 RID: 6449 RVA: 0x000777F5 File Offset: 0x000759F5
	public Direction FocusedDirection
	{
		get
		{
			return this.focusedDirection;
		}
	}

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06001932 RID: 6450 RVA: 0x000777FD File Offset: 0x000759FD
	// (set) Token: 0x06001933 RID: 6451 RVA: 0x00077805 File Offset: 0x00075A05
	public bool EnteredByMouse
	{
		get
		{
			return this.enteredByMouse;
		}
		set
		{
			this.enteredByMouse = value;
		}
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0007780E File Offset: 0x00075A0E
	public AttackBowFocusedPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x06001935 RID: 6453 RVA: 0x00077838 File Offset: 0x00075A38
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.isInFocus = true;
		this.focusedDirection = this.playerController.PlayerData.Direction.ConvertFromVector2();
		this.animator = this.playerController.View.PlayerAnimation.Animator;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.BowAttack, 1f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.StanceWalk, 1f);
		this.animator.SetFloat(AttackBowFocusedPlayerState.WalkSpeed, this.walkAnimationSpeed);
		this.animator.SetBool(AttackBowFocusedPlayerState.BowShoot, false);
		this.animator.SetBool(AttackBowFocusedPlayerState.AttackFocus, true);
		this.animator.SetBool(AttackBowFocusedPlayerState.BowPrepare, true);
		this.playerController.PlayerData.SetDirectionLock(false);
		this.playerController.PhysicalBody.SetDirectionLock(false);
		this.playerController.PhysicalBody.SpeedMultiplier = this.movementSpeedMultiplier;
		Debug.Log("Entering AttackBowFocusedPlayerState");
	}

	// Token: 0x06001936 RID: 6454 RVA: 0x00077958 File Offset: 0x00075B58
	public override void Update()
	{
		if (!this.playerController.IsControlsEnabled)
		{
			this.isInFocus = false;
		}
		this.TryFollowCursorInMouseStance();
		float num = Mathf.Atan2(LazyInput.GetDirection().y, LazyInput.GetDirection().x) * 57.29578f;
		this.animator.SetFloat("MovementDirection", num);
		float x = this.playerController.MovableDirection.x;
		float @float = this.animator.GetFloat(AttackBowFocusedPlayerState.WalkSpeed);
		if (!Mathf.Sign(x).EqualsTo(Mathf.Sign(@float), 1E-05f) && x != 0f)
		{
			float num2 = Mathf.Abs(this.walkAnimationSpeed) * Mathf.Sign(x);
			this.animator.SetFloat(AttackBowFocusedPlayerState.WalkSpeed, num2);
		}
		if (this.IsAttackKeyDown() && !this.animator.GetBool(AttackBowFocusedPlayerState.BowShoot) && this.staminaSystem.CanPerformAttack())
		{
			this.animator.SetBool(AttackBowFocusedPlayerState.BowShoot, true);
		}
		if (!this.IsFocusKeyHeld())
		{
			this.isInFocus = false;
		}
		if (this.playerController.IsControlsEnabled)
		{
			this.playerController.PlayerInputHandler.UpdateHotBarInteraction();
		}
	}

	// Token: 0x06001937 RID: 6455 RVA: 0x000776BE File Offset: 0x000758BE
	public override void FixedUpdate()
	{
		if (this.playerController.IsControlsEnabled)
		{
			this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
		}
	}

	// Token: 0x06001938 RID: 6456 RVA: 0x00077A7C File Offset: 0x00075C7C
	public override void OnExit()
	{
		this.staminaSystem.BeginRegenerationDelayIfSuspended();
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.BowAttack, 0f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.StanceWalk, 0f);
		this.playerController.View.PlayerAnimation.CancelBowAimLoop();
		this.animator.SetBool(AttackBowFocusedPlayerState.AttackFocus, false);
		this.animator.SetBool(AttackBowFocusedPlayerState.BowPrepare, false);
		this.animator.SetBool(AttackBowFocusedPlayerState.BowShoot, false);
		this.animator.SetFloat(AttackBowFocusedPlayerState.WalkSpeed, 1f);
		this.playerController.PlayerData.SetDirectionLock(true);
		this.playerController.PhysicalBody.SetDirectionLock(true);
		this.playerController.PhysicalBody.SpeedMultiplier = 1f;
		this.enteredByMouse = false;
		Debug.Log("Exiting AttackBowFocusedPlayerState");
	}

	// Token: 0x06001939 RID: 6457 RVA: 0x00077B71 File Offset: 0x00075D71
	private bool IsAttackKeyDown()
	{
		if (this.enteredByMouse)
		{
			return LazyInput.GetKeyDown(GameKey.LeftClick) && !MouseAimHelper.IsPointerOverUI();
		}
		return LazyInput.GetKeyDown(GameKey.Attack);
	}

	// Token: 0x0600193A RID: 6458 RVA: 0x00077B9C File Offset: 0x00075D9C
	private bool IsFocusKeyHeld()
	{
		if (!this.enteredByMouse)
		{
			return LazyInput.GetKey(GameKey.AttackFocus);
		}
		return LazyInput.GetKey(GameKey.RightClick);
	}

	// Token: 0x0600193B RID: 6459 RVA: 0x00077BBC File Offset: 0x00075DBC
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

	// Token: 0x0400189A RID: 6298
	private const string MOVEMENT_DIR = "MovementDirection";

	// Token: 0x0400189B RID: 6299
	private static readonly int BowPrepare = Animator.StringToHash("bow_preparing");

	// Token: 0x0400189C RID: 6300
	private static readonly int AttackFocus = Animator.StringToHash("attack_focus");

	// Token: 0x0400189D RID: 6301
	private static readonly int BowShoot = Animator.StringToHash("bow_shoot");

	// Token: 0x0400189E RID: 6302
	private static readonly int WalkSpeed = Animator.StringToHash("walk_speed");

	// Token: 0x0400189F RID: 6303
	private float movementSpeedMultiplier = 0.5f;

	// Token: 0x040018A0 RID: 6304
	private float attackAnimationSpeed = 1.7f;

	// Token: 0x040018A1 RID: 6305
	private float walkAnimationSpeed = 0.5f;

	// Token: 0x040018A2 RID: 6306
	private bool isInFocus;

	// Token: 0x040018A3 RID: 6307
	private bool enteredByMouse;

	// Token: 0x040018A4 RID: 6308
	private Animator animator;

	// Token: 0x040018A5 RID: 6309
	private Direction focusedDirection;

	// Token: 0x040018A6 RID: 6310
	private StaminaSystem staminaSystem;
}
