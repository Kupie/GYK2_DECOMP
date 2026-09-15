using System;
using LazyBearTechnology;

// Token: 0x0200064B RID: 1611
public class FlagInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x170006C3 RID: 1731
	// (get) Token: 0x06002AC1 RID: 10945 RVA: 0x000CA17C File Offset: 0x000C837C
	private string ItemId
	{
		get
		{
			return this.assignedWgo.Data.id + "_item";
		}
	}

	// Token: 0x06002AC2 RID: 10946 RVA: 0x000CA198 File Offset: 0x000C8398
	public override bool HasInteraction(PlayerController interactor)
	{
		return !AgentsGroupFlagController.IsInteractionLocked(this.assignedWgo) && interactor.PlayerData.Inventory.CanAddItemToInventory(this.ItemId, 1);
	}

	// Token: 0x06002AC3 RID: 10947 RVA: 0x000CA1C8 File Offset: 0x000C83C8
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		AgentsGroupFlagController componentInChildren = this.assignedWgo.GetComponentInChildren<AgentsGroupFlagController>();
		if (componentInChildren)
		{
			componentInChildren.Init();
			this.assignedWgo.UpdateFlag(ChunkingIgnoreType.Fighting, true);
			LazySingleton<FightingGameController>.Instance.AllDynamicObjectsInZone.Add(this.assignedWgo);
			LazySingleton<FightingGameController>.Instance.customFlagControllers.Add(componentInChildren);
		}
		interactor.AttachTheFlag(this.assignedWgo);
		return true;
	}

	// Token: 0x06002AC4 RID: 10948 RVA: 0x000CA23C File Offset: 0x000C843C
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
		interactionInfos2.Add(new InteractionInfo("", "icon-arrow_flag-take"));
		return interactionInfos2;
	}
}
