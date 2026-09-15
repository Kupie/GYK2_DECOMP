using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003AC RID: 940
public class AttackSwordPlayerState : SSMState
{
	// Token: 0x0600195F RID: 6495 RVA: 0x000775CB File Offset: 0x000757CB
	public AttackSwordPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x17000459 RID: 1113
	// (get) Token: 0x06001960 RID: 6496 RVA: 0x0007856E File Offset: 0x0007676E
	public override bool IsActive
	{
		get
		{
			return this.isAttacking;
		}
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x00078578 File Offset: 0x00076778
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.isAttacking = true;
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 1f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 1f);
		this.playerController.SetControlTakenType(TakenControlType.ByAttack, false);
		this.dashDirection = LazyInput.GetDirection();
		if (this.dashDirection.magnitude < 0.1f)
		{
			this.dashDirection = this.playerController.PlayerData.Direction;
		}
		this.dashEndTime = Time.time + this.playerController.PhysicalBody.PhysicsConfig.attackDashDuration;
		this.dashCooldownEndTime = Time.time + this.playerController.PhysicalBody.PhysicsConfig.attackDashCooldown;
		this.playerController.AttackComponent.PerformAttackByTrigger(false, default(Vector3), "attack", delegate
		{
			this.isAttacking = false;
		}, true);
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x00078684 File Offset: 0x00076884
	public override void Update()
	{
		if (LazyInput.GetKeyDown(GameKey.Attack) && this.isAttacking && this.staminaSystem.CanPerformAttack())
		{
			this.playerController.AttackComponent.PerformAttackByTrigger(false, default(Vector3), "attack", delegate
			{
				this.isAttacking = false;
			}, true);
		}
	}

	// Token: 0x06001963 RID: 6499 RVA: 0x000786E0 File Offset: 0x000768E0
	public override void FixedUpdate()
	{
		if (Time.time < this.dashEndTime)
		{
			Vector3 vector = this.dashDirection.XZ().normalized * this.playerController.PhysicalBody.PhysicsConfig.attackDashForce;
			this.playerController.PhysicalBody.Rb.AddForce(vector, ForceMode.Impulse);
		}
	}

	// Token: 0x06001964 RID: 6500 RVA: 0x00078740 File Offset: 0x00076940
	public override void OnExit()
	{
		this.playerController.AttackComponent.OnAttackAnimFinished(this.playerController.View.PlayerAnimation);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttack, 0f);
		this.playerController.View.PlayerAnimation.SetLayerWeight(AnimationComponent.Layers.SwordAttackHitbox, 0f);
		this.playerController.SetControlTakenType(TakenControlType.ByAttack, true);
	}

	// Token: 0x040018C2 RID: 6338
	private const string ATTACK_TRIGGER = "attack";

	// Token: 0x040018C3 RID: 6339
	private bool isAttacking;

	// Token: 0x040018C4 RID: 6340
	private float dashEndTime;

	// Token: 0x040018C5 RID: 6341
	private float dashCooldownEndTime;

	// Token: 0x040018C6 RID: 6342
	private Vector2 dashDirection;

	// Token: 0x040018C7 RID: 6343
	private StaminaSystem staminaSystem;
}
