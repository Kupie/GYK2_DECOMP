using System;
using LazyBearTechnology;

// Token: 0x02000634 RID: 1588
public class AlchemyInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A46 RID: 10822 RVA: 0x000C76E0 File Offset: 0x000C58E0
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (!this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			UIAlchemyWindowData uialchemyWindowData = new UIAlchemyWindowData(this.assignedWgo);
			LazyUI.GetWindow<UIAlchemyWindow>().Open(uialchemyWindowData);
		}
		return true;
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x000C7734 File Offset: 0x000C5934
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
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_alchemy", GameKey.Interaction)));
	}
}
