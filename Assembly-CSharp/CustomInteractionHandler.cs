using System;

// Token: 0x02000644 RID: 1604
public class CustomInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A9E RID: 10910 RVA: 0x000C9800 File Offset: 0x000C7A00
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (this.assignedWgo.Data.Definition.customInteraction.IsInteractable(this.assignedWgo.Data))
		{
			this.Interact(this.assignedWgo.Data.Definition.customInteraction);
			return true;
		}
		this.Interact(this.assignedWgo.Data.Definition.customInteraction2);
		return true;
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000C9878 File Offset: 0x000C7A78
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || (this.HasInteraction(this.assignedWgo.Data.Definition.customInteraction) || this.HasInteraction(this.assignedWgo.Data.Definition.customInteraction2));
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000C98D0 File Offset: 0x000C7AD0
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string empty = string.Empty;
		if (this.TryGetInteractionHint(this.assignedWgo.Data.Definition.customInteraction, out empty))
		{
			if (empty.StartsWith("[") && empty.EndsWith("]"))
			{
				return new InteractionInfos(base.GetInteractionInfoByUsingTool(false));
			}
			return new InteractionInfos(new InteractionInfo((!string.IsNullOrEmpty(empty)) ? base.LocalizeHintWithActionIcon(empty, this.assignedWgo.Data.Definition.customInteraction.gameKey) : string.Empty));
		}
		else
		{
			if (!this.TryGetInteractionHint(this.assignedWgo.Data.Definition.customInteraction2, out empty))
			{
				return new InteractionInfos(new InteractionInfo(empty));
			}
			if (empty.StartsWith("[") && empty.EndsWith("]"))
			{
				return new InteractionInfos(base.GetInteractionInfoByUsingTool(false));
			}
			return new InteractionInfos(new InteractionInfo((!string.IsNullOrEmpty(empty)) ? base.LocalizeHintWithActionIcon(empty, this.assignedWgo.Data.Definition.customInteraction2.gameKey) : string.Empty));
		}
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000C9A01 File Offset: 0x000C7C01
	private bool HasInteraction(CustomInteraction customInteraction)
	{
		return customInteraction.IsInteractable(this.assignedWgo.Data);
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000C9A1C File Offset: 0x000C7C1C
	private void Interact(CustomInteraction customInteraction)
	{
		Item item;
		customInteraction.TryGetInteractable(this.assignedWgo.Data, out item);
		Item overheadCandidate = LazyExpressionEvaluationScope.OverheadCandidate;
		LazyExpressionEvaluationScope.OverheadCandidate = item;
		try
		{
			foreach (LazyExpression lazyExpression in customInteraction.execution)
			{
				lazyExpression.EvaluateBool(this.assignedWgo.Data);
			}
		}
		finally
		{
			LazyExpressionEvaluationScope.OverheadCandidate = overheadCandidate;
		}
		if (this.assignedWgo.Data.Definition.customInteraction != null && this.assignedWgo.Data.Definition.customInteraction2 != null)
		{
			this.assignedWgo.DrawWidgets();
		}
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000C9AE4 File Offset: 0x000C7CE4
	private bool TryGetInteractionHint(CustomInteraction customInteraction, out string hint)
	{
		hint = string.Empty;
		if (customInteraction.IsInteractable(this.assignedWgo.Data))
		{
			hint = customInteraction.hint;
			return true;
		}
		return false;
	}
}
