using System;
using LazyBearTechnology;

// Token: 0x0200064F RID: 1615
public class GraveInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AE2 RID: 10978 RVA: 0x000CB438 File Offset: 0x000C9638
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		LazyUI.GetWindow<UIGraveWindow>().Open(new UIGraveWindowData(this.assignedWgo));
		return true;
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x000CB45B File Offset: 0x000C965B
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || !this.assignedWgo.Data.CraftComponent.IsStarted;
	}

	// Token: 0x06002AE4 RID: 10980 RVA: 0x000CB482 File Offset: 0x000C9682
	public override bool HasInteraction2(PlayerController interactor)
	{
		return base.HasInteraction2(interactor) || this.assignedWgo.Data.CraftComponent.IsStarted;
	}

	// Token: 0x06002AE5 RID: 10981 RVA: 0x000CB4AC File Offset: 0x000C96AC
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		if (this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			return new InteractionInfos(base.GetInteractionInfoByUsingTool(false));
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_grave", GameKey.Interaction)));
	}

	// Token: 0x04002343 RID: 9027
	private CraftComponent assignedCraftComponent;
}
