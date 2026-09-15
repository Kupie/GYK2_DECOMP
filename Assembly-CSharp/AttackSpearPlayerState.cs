using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class AttackSpearPlayerState : SSMState
{
	// Token: 0x0600193D RID: 6461 RVA: 0x000775CB File Offset: 0x000757CB
	public AttackSpearPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x0600193E RID: 6462 RVA: 0x00077C50 File Offset: 0x00075E50
	public override bool IsActive
	{
		get
		{
			return this.isAttacking;
		}
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x00077C58 File Offset: 0x00075E58
	public override void OnEnter()
	{
		this.staminaSystem = MainGame.PlayerData.staminaSystem;
		this.isAttacking = true;
		this.playerController.SetControlTakenType(TakenControlType.ByAttack, false);
		this.playerController.AttackComponent.PerformAttack(false, default(Vector3), true, delegate
		{
			this.isAttacking = false;
		}, true);
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x00077CB4 File Offset: 0x00075EB4
	public override void Update()
	{
		if (LazyInput.GetKeyDown(GameKey.Attack) && this.isAttacking && this.staminaSystem.CanPerformAttack())
		{
			this.playerController.AttackComponent.PerformAttack(false, default(Vector3), true, delegate
			{
				this.isAttacking = false;
			}, true);
		}
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x00077D0A File Offset: 0x00075F0A
	public override void OnExit()
	{
		this.playerController.AttackComponent.OnAttackAnimFinished(this.playerController.View.PlayerAnimation);
		this.playerController.SetControlTakenType(TakenControlType.ByAttack, true);
	}

	// Token: 0x040018A7 RID: 6311
	private bool isAttacking;

	// Token: 0x040018A8 RID: 6312
	private StaminaSystem staminaSystem;
}
