using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003A6 RID: 934
public class AttackBowDefaultPlayerState : SSMState
{
	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06001926 RID: 6438 RVA: 0x000775B2 File Offset: 0x000757B2
	public override bool IsActive
	{
		get
		{
			return this.isShooting;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06001927 RID: 6439 RVA: 0x000775BA File Offset: 0x000757BA
	// (set) Token: 0x06001928 RID: 6440 RVA: 0x000775C2 File Offset: 0x000757C2
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

	// Token: 0x06001929 RID: 6441 RVA: 0x000775CB File Offset: 0x000757CB
	public AttackBowDefaultPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x0600192A RID: 6442 RVA: 0x000775D4 File Offset: 0x000757D4
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.isShooting = true;
		this.hasEnteredShotState = false;
		this.timeSinceEnter = 0f;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.BowAttack, 1f);
		this.animator = this.playerController.View.PlayerAnimation.Animator;
		this.animator.SetBool(AttackBowDefaultPlayerState.AttackFocus, false);
		this.animator.SetBool(AttackBowDefaultPlayerState.BowPrepare, false);
		this.animator.SetBool(AttackBowDefaultPlayerState.BowShoot, true);
		Debug.Log("Entering AttackBowDefaultPlayerState");
	}

	// Token: 0x0600192B RID: 6443 RVA: 0x0007767E File Offset: 0x0007587E
	public override void Update()
	{
		this.timeSinceEnter += Time.deltaTime;
		if (this.IsPlayingSingleShot())
		{
			this.hasEnteredShotState = true;
			return;
		}
		if (this.hasEnteredShotState || this.timeSinceEnter > 1f)
		{
			this.isShooting = false;
		}
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x000776BE File Offset: 0x000758BE
	public override void FixedUpdate()
	{
		if (this.playerController.IsControlsEnabled)
		{
			this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
		}
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x000776E4 File Offset: 0x000758E4
	public override void OnExit()
	{
		this.staminaSystem.BeginRegenerationDelayIfSuspended();
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.BowAttack, 0f);
		this.playerController.View.PlayerAnimation.CancelBowAimLoop();
		this.hasEnteredShotState = false;
		this.animator.SetBool(AttackBowDefaultPlayerState.AttackFocus, false);
		this.animator.SetBool(AttackBowDefaultPlayerState.BowPrepare, false);
		this.animator.SetBool(AttackBowDefaultPlayerState.BowShoot, false);
		this.enteredByMouse = false;
		Debug.Log("Exiting AttackBowDefaultPlayerState");
	}

	// Token: 0x0600192E RID: 6446 RVA: 0x00077778 File Offset: 0x00075978
	private bool IsPlayingSingleShot()
	{
		int num = 12;
		return this.animator.GetCurrentAnimatorStateInfo(num).IsName("Single Shot") || this.animator.GetNextAnimatorStateInfo(num).IsName("Single Shot");
	}

	// Token: 0x0400188F RID: 6287
	private const string SINGLE_SHOT_STATE = "Single Shot";

	// Token: 0x04001890 RID: 6288
	private const float SHOT_START_TIMEOUT = 1f;

	// Token: 0x04001891 RID: 6289
	private static readonly int AttackFocus = Animator.StringToHash("attack_focus");

	// Token: 0x04001892 RID: 6290
	private static readonly int BowShoot = Animator.StringToHash("bow_shoot");

	// Token: 0x04001893 RID: 6291
	private static readonly int BowPrepare = Animator.StringToHash("bow_preparing");

	// Token: 0x04001894 RID: 6292
	private bool isShooting;

	// Token: 0x04001895 RID: 6293
	private bool hasEnteredShotState;

	// Token: 0x04001896 RID: 6294
	private float timeSinceEnter;

	// Token: 0x04001897 RID: 6295
	private bool enteredByMouse;

	// Token: 0x04001898 RID: 6296
	private Animator animator;

	// Token: 0x04001899 RID: 6297
	private StaminaSystem staminaSystem;
}
