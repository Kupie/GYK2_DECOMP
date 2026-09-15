using System;
using LazyBearTechnology;

// Token: 0x02000660 RID: 1632
public class SurveyInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B32 RID: 11058 RVA: 0x000CCB18 File Offset: 0x000CAD18
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (!this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			this.assignedWgo.Data.TrySetWorker(interactor, null);
			LazyWindow<UIResourceBasedCraftWindowData> window = LazyUI.GetWindow<UIResourceBasedCraftWindow>();
			UIResourceBasedCraftWindowData uiresourceBasedCraftWindowData = new UIResourceBasedCraftWindowData(this.assignedWgo.Data);
			window.Open(uiresourceBasedCraftWindowData);
		}
		return true;
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x000CCB78 File Offset: 0x000CAD78
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		if (this.assignedWgo.Data.CraftComponent.IsStarted)
		{
			return new InteractionInfos(new InteractionInfo(base.WorkHint));
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_survey", GameKey.Interaction)));
	}
}
