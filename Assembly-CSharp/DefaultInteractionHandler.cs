using System;

// Token: 0x02000672 RID: 1650
public class DefaultInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B89 RID: 11145 RVA: 0x000CEAB4 File Offset: 0x000CCCB4
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor);
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x000CEAC4 File Offset: 0x000CCCC4
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		return new InteractionInfos(new InteractionInfo(string.Empty));
	}
}
