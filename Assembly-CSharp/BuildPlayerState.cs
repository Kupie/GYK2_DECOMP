using System;

// Token: 0x020003AD RID: 941
public class BuildPlayerState : SSMState
{
	// Token: 0x06001967 RID: 6503 RVA: 0x000775CB File Offset: 0x000757CB
	public BuildPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x1700045A RID: 1114
	// (get) Token: 0x06001968 RID: 6504 RVA: 0x000787BB File Offset: 0x000769BB
	public override bool CanEnter
	{
		get
		{
			return this.IsActive;
		}
	}

	// Token: 0x1700045B RID: 1115
	// (get) Token: 0x06001969 RID: 6505 RVA: 0x000787C3 File Offset: 0x000769C3
	public override bool IsActive
	{
		get
		{
			return BuildController.Instance != null && BuildController.Instance.IsBuildModeActive;
		}
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x000787DE File Offset: 0x000769DE
	public override void OnEnter()
	{
		this.playerController.PhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByBuilding, false);
		this.playerController.PlayerInteractionComponent.SetPauseState(PlayerInteractionPauseType.ByBuild, true);
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x00078804 File Offset: 0x00076A04
	public override void OnExit()
	{
		this.playerController.PhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByBuilding, true);
		this.playerController.PlayerInteractionComponent.SetPauseState(PlayerInteractionPauseType.ByBuild, false);
	}
}
