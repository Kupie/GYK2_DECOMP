using System;

// Token: 0x02000669 RID: 1641
public class WorkInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B59 RID: 11097 RVA: 0x000CD454 File Offset: 0x000CB654
	public override bool HasInteraction2(PlayerController interactor)
	{
		if (MainGame.PlayerData != null && MainGame.PlayerData.HasMultipleOverheadItems)
		{
			return false;
		}
		base.HasInteraction2(interactor);
		return true;
	}

	// Token: 0x06002B5A RID: 11098 RVA: 0x000CD474 File Offset: 0x000CB674
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		return new InteractionInfos(base.GetInteractionInfoByUsingTool(false));
	}
}
