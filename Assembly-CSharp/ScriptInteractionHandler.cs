using System;
using LazyBearTechnology;

// Token: 0x0200065E RID: 1630
public class ScriptInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B2A RID: 11050 RVA: 0x000CC9A1 File Offset: 0x000CABA1
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		this.assignedWgo.FireEvent("interaction");
		return true;
	}

	// Token: 0x06002B2B RID: 11051 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002B2C RID: 11052 RVA: 0x000CC9C0 File Offset: 0x000CABC0
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_interact", GameKey.Interaction)));
	}
}
