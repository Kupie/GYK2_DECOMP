using System;
using LazyBearTechnology;

// Token: 0x02000639 RID: 1593
public class BuildInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A5C RID: 10844 RVA: 0x000C808D File Offset: 0x000C628D
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		LazySingleton<BuildManager>.Instance.TryEnable(this.assignedWgo, null);
		return true;
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002A5E RID: 10846 RVA: 0x000C80B0 File Offset: 0x000C62B0
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_build", GameKey.Interaction)));
	}
}
