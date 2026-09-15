using System;
using LazyBearTechnology;

// Token: 0x0200063D RID: 1597
public class ConveyorCellInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A74 RID: 10868 RVA: 0x000C8601 File Offset: 0x000C6801
	public override void OnInteractionTargetEnter(PlayerController interactor)
	{
		base.OnInteractionTargetEnter(interactor);
		WgoPart mainWgoPart = this.assignedWgo.MainWgoPart;
		if (mainWgoPart == null)
		{
			return;
		}
		mainWgoPart.SetDropViewInteractionState(true);
	}

	// Token: 0x06002A75 RID: 10869 RVA: 0x000C8620 File Offset: 0x000C6820
	public override void OnInteractionTargetExit()
	{
		WgoPart mainWgoPart = this.assignedWgo.MainWgoPart;
		if (mainWgoPart != null)
		{
			mainWgoPart.SetDropViewInteractionState(false);
		}
		base.OnInteractionTargetExit();
	}

	// Token: 0x06002A76 RID: 10870 RVA: 0x000C8640 File Offset: 0x000C6840
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		foreach (Item item in this.assignedWgo.Data.Inventory.RemoveItemById(this.assignedWgo.Data.Inventory.Data.Inventory[0].id, -1, null, null, false))
		{
			if (item.Definition.itemSize == ItemSize.Small)
			{
				this.assignedWgo.Data.MakeDrop(item);
			}
			else if (!MainGame.PlayerData.TryAddOverheadItemNoReplace(item))
			{
				this.assignedWgo.Data.MakeDrop(item);
			}
			this.assignedWgo.MainWgoPart.UpdateDropViewFromInventory(null);
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002A77 RID: 10871 RVA: 0x000C872C File Offset: 0x000C692C
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || (this.assignedWgo.Data.Inventory.Data.Inventory.Count > 0 && (this.assignedWgo.Data.Inventory.Data.Inventory[0].Definition.itemSize == ItemSize.Small || MainGame.PlayerData.HasFreeOverheadSlot));
	}

	// Token: 0x06002A78 RID: 10872 RVA: 0x000C87A4 File Offset: 0x000C69A4
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
	}
}
