using System;
using LazyBearTechnology;

// Token: 0x02000668 RID: 1640
public class WellUpgradeInteractionHandler : WorkInteractionHandler
{
	// Token: 0x06002B55 RID: 11093 RVA: 0x000CD366 File Offset: 0x000CB566
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || this.assignedWgo.Data.Inventory.Data.GetTotalCountInInventory("water", null, false) >= 1;
	}

	// Token: 0x06002B56 RID: 11094 RVA: 0x000CD39C File Offset: 0x000CB59C
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		this.assignedWgo.Data.MakeDrop(this.assignedWgo.Data.Inventory.Data.RemoveItemFromInventoryById("water", -1, null, null, false));
		return true;
	}

	// Token: 0x06002B57 RID: 11095 RVA: 0x000CD3E8 File Offset: 0x000CB5E8
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = new InteractionInfos();
		if (this.HasInteraction(MainGame.PlayerController))
		{
			interactionInfos.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take_water", GameKey.Interaction)));
		}
		if (this.HasInteraction2(MainGame.PlayerController))
		{
			interactionInfos.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_draw_water", GameKey.Action)));
		}
		return interactionInfos;
	}
}
