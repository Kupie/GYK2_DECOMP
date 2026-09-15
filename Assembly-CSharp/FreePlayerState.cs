using System;
using LazyBearTechnology;

// Token: 0x020003AE RID: 942
public class FreePlayerState : SSMState
{
	// Token: 0x1700045C RID: 1116
	// (get) Token: 0x0600196C RID: 6508 RVA: 0x0007882A File Offset: 0x00076A2A
	public override bool IsActive
	{
		get
		{
			return this.playerController.IsControlsEnabled;
		}
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x00078837 File Offset: 0x00076A37
	public FreePlayerState(PlayerController playerController)
		: base(playerController)
	{
		this.playerInputHandler = playerController.PlayerInputHandler;
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x0007884C File Offset: 0x00076A4C
	public override void Update()
	{
		this.playerInputHandler.UpdateInput();
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x00077477 File Offset: 0x00075677
	public override void FixedUpdate()
	{
		this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
	}

	// Token: 0x040018C8 RID: 6344
	private PlayerInputHandler playerInputHandler;
}
