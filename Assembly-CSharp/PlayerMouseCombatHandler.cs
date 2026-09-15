using System;
using LazyBearTechnology;

// Token: 0x02000398 RID: 920
public class PlayerMouseCombatHandler
{
	// Token: 0x0600189D RID: 6301 RVA: 0x000748A1 File Offset: 0x00072AA1
	public PlayerMouseCombatHandler(PlayerController playerController)
	{
		this.playerController = playerController;
	}

	// Token: 0x0600189E RID: 6302 RVA: 0x000748B0 File Offset: 0x00072AB0
	public void Update()
	{
		if (!this.CanProcess())
		{
			this.playerController.PhysicalBody.ClearAimDirection();
			return;
		}
		SSMState curState = this.playerController.Ssm.CurState;
		bool flag = curState is AttackSwordFocusedPlayerState || curState is AttackBowFocusedPlayerState;
		bool key = LazyInput.GetKey(GameKey.RightClick);
		if (!flag && !key)
		{
			this.playerController.PhysicalBody.ClearAimDirection();
		}
		if (MouseAimHelper.IsPointerOverUI())
		{
			return;
		}
		if (this.playerController.AttackComponent.IsRangedWeapon)
		{
			this.UpdateRanged();
			return;
		}
		this.UpdateMelee();
	}

	// Token: 0x0600189F RID: 6303 RVA: 0x00074948 File Offset: 0x00072B48
	private bool CanProcess()
	{
		return this.playerController.IsControlsEnabled && this.playerController.AttackComponent.HasEquippedWeapon && !LazyInput.IsGamepadActive;
	}

	// Token: 0x060018A0 RID: 6304 RVA: 0x00074974 File Offset: 0x00072B74
	private void UpdateMelee()
	{
		if (LazyInput.GetKey(GameKey.RightClick))
		{
			MouseAimHelper.TryApplyAim(this.playerController.PhysicalBody);
			this.playerController.Ssm.ForceEnterState<AttackSwordFocusedPlayerState>();
		}
		if (LazyInput.GetKeyDown(GameKey.LeftClick) && !(this.playerController.Ssm.CurState is AttackSwordFocusedPlayerState) && MainGame.PlayerData.staminaSystem.CanPerformAttack())
		{
			MouseAimHelper.TryApplyAim(this.playerController.PhysicalBody);
			this.playerController.Ssm.ForceEnterState<AttackSwordDefaultPlayerState>();
		}
	}

	// Token: 0x060018A1 RID: 6305 RVA: 0x00074A04 File Offset: 0x00072C04
	private void UpdateRanged()
	{
		if (LazyInput.GetKey(GameKey.RightClick))
		{
			if (!(this.playerController.Ssm.CurState is AttackBowFocusedPlayerState))
			{
				AttackBowFocusedPlayerState state = this.playerController.Ssm.GetState<AttackBowFocusedPlayerState>();
				if (state != null)
				{
					state.EnteredByMouse = true;
				}
			}
			MouseAimHelper.TryApplyAim(this.playerController.PhysicalBody);
			this.playerController.Ssm.ForceEnterState<AttackBowFocusedPlayerState>();
		}
		if (LazyInput.GetKeyDown(GameKey.LeftClick) && !(this.playerController.Ssm.CurState is AttackBowFocusedPlayerState) && MainGame.PlayerData.staminaSystem.CanPerformAttack())
		{
			if (!(this.playerController.Ssm.CurState is AttackBowDefaultPlayerState))
			{
				AttackBowDefaultPlayerState state2 = this.playerController.Ssm.GetState<AttackBowDefaultPlayerState>();
				if (state2 != null)
				{
					state2.EnteredByMouse = true;
				}
			}
			MouseAimHelper.TryApplyAim(this.playerController.PhysicalBody);
			this.playerController.Ssm.ForceEnterState<AttackBowDefaultPlayerState>();
		}
	}

	// Token: 0x04001826 RID: 6182
	private readonly PlayerController playerController;
}
