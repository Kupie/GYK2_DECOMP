using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000650 RID: 1616
public class LadderInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AE7 RID: 10983 RVA: 0x000CB515 File Offset: 0x000C9715
	public override IWGOInteractionHandler Init(Wgo wgo)
	{
		this.ladder = wgo.GetComponentInChildren<Ladder>();
		if (!this.ladder)
		{
			Debug.LogError("Ladder not found");
		}
		return base.Init(wgo);
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000CB541 File Offset: 0x000C9741
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		this.assignedWgo.Data.SetGameRes("ladder_busy", 1);
		interactor.Ssm.ForceEnterState<LadderPlayerState>();
		return true;
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000CB570 File Offset: 0x000C9770
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		if (!this.ladder)
		{
			return false;
		}
		Wgo assignedWgo = this.assignedWgo;
		return assignedWgo.Data.GetGameResInt("ladder_busy") == 0 && assignedWgo.Data.Definition.interactionType == WGODef.InteractionType.Ladder && interactor.LadderClimbController.CanUse;
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x000CB5D8 File Offset: 0x000C97D8
	public override void OnInteractionTargetEnter(PlayerController interactor)
	{
		if (!this.ladder)
		{
			return;
		}
		LadderEdgePart nearestLadderPart = this.ladder.GetNearestLadderPart(interactor.transform.position);
		Transform bubblePointToDisplay = nearestLadderPart.BubblePointToDisplay;
		if (bubblePointToDisplay != null)
		{
			this.assignedWgo.SetCustomBubblePoint(bubblePointToDisplay);
		}
		interactor.LadderClimbController.LadderUnderInteraction = nearestLadderPart.Ladder;
		base.OnInteractionTargetEnter(interactor);
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x000CB640 File Offset: 0x000C9840
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_climb", GameKey.Interaction)));
	}

	// Token: 0x04002344 RID: 9028
	public const string BUSY_GAMERES_KEY = "ladder_busy";

	// Token: 0x04002345 RID: 9029
	private Ladder ladder;
}
