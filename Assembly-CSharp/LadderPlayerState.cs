using System;
using LazyBearTechnology;

// Token: 0x020003AF RID: 943
public class LadderPlayerState : SSMState
{
	// Token: 0x06001970 RID: 6512 RVA: 0x00078859 File Offset: 0x00076A59
	public LadderPlayerState(PlayerController playerController, LadderClimbController ladderClimbController)
		: base(playerController)
	{
		this.ladderClimbController = ladderClimbController;
	}

	// Token: 0x1700045D RID: 1117
	// (get) Token: 0x06001971 RID: 6513 RVA: 0x00078869 File Offset: 0x00076A69
	public override bool CanEnter
	{
		get
		{
			return this.CanUse();
		}
	}

	// Token: 0x1700045E RID: 1118
	// (get) Token: 0x06001972 RID: 6514 RVA: 0x00078871 File Offset: 0x00076A71
	public override bool IsActive
	{
		get
		{
			return this.ladderClimbController.IsClimbActive;
		}
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x00078880 File Offset: 0x00076A80
	public override void OnEnter()
	{
		this.playerController.SetControlTakenType(TakenControlType.ByLadder, false);
		this.playerController.PhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByLadder, false);
		this.playerController.PlayerInteractionComponent.SetPauseState(PlayerInteractionPauseType.ByLadder, true);
		this.playerController.PhysicalBody.enabled = false;
		this.ladderClimbController.StartClimb(0f, () => LazyInput.GetKey(GameKey.Up) || LazyInput.GetKey(GameKey.DpadUp), () => LazyInput.GetKey(GameKey.Down) || LazyInput.GetKey(GameKey.DpadDown), this.ladderClimbController.LadderUnderInteraction, this.playerController.PhysicalBody.Rb);
		this.ladderWgo = this.ladderClimbController.LadderUnderUse.GetComponentInParent<Wgo>();
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x0007894E File Offset: 0x00076B4E
	public override void Update()
	{
		if (this.ladderClimbController.AutoLeaveLadderEnabled)
		{
			return;
		}
		if (!this.ladderClimbController.CanLeaveLadder)
		{
			return;
		}
		if (LazyInput.GetKeyDown(GameKey.Interaction) && this.IsActive)
		{
			this.ladderClimbController.StopClimb();
		}
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x0007898C File Offset: 0x00076B8C
	public override void OnExit()
	{
		this.playerController.PhysicalBody.SetNonKinematicFlag(PlayerDynamicType.ByLadder, true);
		this.playerController.PhysicalBody.enabled = true;
		this.playerController.SetControlTakenType(TakenControlType.ByLadder, true);
		if (this.ladderWgo)
		{
			this.ladderWgo.Data.SetGameRes("ladder_busy", 0);
			this.ladderWgo = null;
		}
		this.playerController.PlayerInteractionComponent.SetPauseState(PlayerInteractionPauseType.ByLadder, false);
		this.playerController.PlayerInteractionComponent.ResetInteractionState();
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x00028294 File Offset: 0x00026494
	private bool CanUse()
	{
		return false;
	}

	// Token: 0x040018C9 RID: 6345
	private LadderClimbController ladderClimbController;

	// Token: 0x040018CA RID: 6346
	private Wgo ladderWgo;
}
