using System;
using LazyBearTechnology;

// Token: 0x02000662 RID: 1634
public class TeleportMilestoneInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B3A RID: 11066 RVA: 0x000CC9A1 File Offset: 0x000CABA1
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		this.assignedWgo.FireEvent("interaction");
		return true;
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x000CCD89 File Offset: 0x000CAF89
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || MainGame.PlayerController.PlayerData.GetResInt("milestones_activated") > 0;
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x000CCDB0 File Offset: 0x000CAFB0
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		return new InteractionInfos(new InteractionInfo(base.LocalizeHintWithActionIcon("ui_use", GameKey.Interaction)));
	}
}
