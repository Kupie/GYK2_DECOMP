using System;
using LazyBearTechnology;

// Token: 0x0200063A RID: 1594
public class CargoLiftInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A60 RID: 10848 RVA: 0x000C80F8 File Offset: 0x000C62F8
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		LazyWindow<UIPorterStationWindowData> window = LazyUI.GetWindow<UIPorterStationWindow>();
		UIPorterStationWindowData uiporterStationWindowData = new UIPorterStationWindowData(this.assignedWgo.Data);
		window.Open(uiporterStationWindowData);
		return true;
	}

	// Token: 0x06002A61 RID: 10849 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override bool HasInteraction(PlayerController interactor)
	{
		return true;
	}

	// Token: 0x06002A62 RID: 10850 RVA: 0x000C8130 File Offset: 0x000C6330
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_open", GameKey.Interaction)));
		if (this.HasInteraction2(MainGame.PlayerController))
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put", GameKey.Action)));
		}
		return interactionInfos2;
	}
}
