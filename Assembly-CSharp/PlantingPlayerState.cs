using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020003B1 RID: 945
public class PlantingPlayerState : SSMState
{
	// Token: 0x0600197B RID: 6523 RVA: 0x000775CB File Offset: 0x000757CB
	public PlantingPlayerState(PlayerController playerController)
		: base(playerController)
	{
	}

	// Token: 0x1700045F RID: 1119
	// (get) Token: 0x0600197C RID: 6524 RVA: 0x00078A55 File Offset: 0x00076C55
	public override bool CanEnter
	{
		get
		{
			PlayerData playerData = this.playerController.PlayerData;
			return ((playerData != null) ? playerData.interactingItem : null) != null;
		}
	}

	// Token: 0x17000460 RID: 1120
	// (get) Token: 0x0600197D RID: 6525 RVA: 0x00078A55 File Offset: 0x00076C55
	public override bool IsActive
	{
		get
		{
			PlayerData playerData = this.playerController.PlayerData;
			return ((playerData != null) ? playerData.interactingItem : null) != null;
		}
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x00078A74 File Offset: 0x00076C74
	public override void Update()
	{
		this.playerController.PlayerInputHandler.UpdateInputInteractionOnly();
		if (LazyInput.GetKeyDown(GameKey.Action) || LazyInput.GetKeyDown(GameKey.InGameMenu) || Input.GetKeyDown(KeyCode.Tab))
		{
			this.playerController.PlayerData.RemoveInteractingItem();
		}
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x00078AC2 File Offset: 0x00076CC2
	public override void FixedUpdate()
	{
		if (!MainGame.PlayerController.IsControlsEnabled)
		{
			return;
		}
		this.playerController.PhysicalBody.MoveByDirection(LazyInput.GetDirection());
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x00078AE6 File Offset: 0x00076CE6
	public override void OnExit()
	{
		this.playerController.PlayerData.RemoveInteractingItem();
		this.playerController.PlayerInteractionComponent.ResetInteractionState();
	}
}
