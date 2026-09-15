using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000648 RID: 1608
public class FightBuildInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AB3 RID: 10931 RVA: 0x000C9E88 File Offset: 0x000C8088
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		MilitaryBaseData militaryBaseData = MainGame.Instance.GameSave.militaryBaseData;
		LazySingleton<BuildManager>.Instance.TryEnable(this.assignedWgo, new Func<List<Inventory>>(militaryBaseData.CreateBaseBuildingsInventory));
		return true;
	}

	// Token: 0x06002AB4 RID: 10932 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002AB5 RID: 10933 RVA: 0x000C9ED0 File Offset: 0x000C80D0
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
