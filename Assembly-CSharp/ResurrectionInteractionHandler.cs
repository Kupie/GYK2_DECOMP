using System;
using LazyBearTechnology;

// Token: 0x0200065A RID: 1626
public class ResurrectionInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B14 RID: 11028 RVA: 0x000CC424 File Offset: 0x000CA624
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (this.assignedWgo.Data.CraftComponent.IsDestroyingCraftActive)
		{
			return false;
		}
		Item item;
		if (this.TryGetInsertableCorpseOverhead(out item))
		{
			MainGame.PlayerData.InsertOverheadItemTo(this.assignedWgo.Data, item);
			interactor.PlayerInteractionComponent.ResetInteractionState();
			return true;
		}
		if (this.HasTakableZombieOverhead() && this.assignedWgo.Data.GetGameResInt("resurrection_prepared") == 0)
		{
			Item itemByGroupId = this.assignedWgo.Data.Inventory.GetItemByGroupId("zombie");
			if (itemByGroupId == null || itemByGroupId.IsEmpty)
			{
				return false;
			}
			PlayerData playerData = MainGame.PlayerData;
			if (!playerData.HasFreeOverheadSlot)
			{
				return false;
			}
			playerData.TryAddOverheadItemNoReplace(itemByGroupId);
			this.assignedWgo.Data.Inventory.RemoveItemFromInventoryByUID(itemByGroupId, -1);
			interactor.PlayerInteractionComponent.ResetInteractionState();
			return true;
		}
		else
		{
			if (this.assignedWgo.Data.GetGameResInt("resurrection_prepared") == 0)
			{
				LazyUI.GetWindow<UIResurrectionWindow>().Open(new UIResurrectionWindowData(this.assignedWgo.Data));
				return true;
			}
			return false;
		}
	}

	// Token: 0x06002B15 RID: 11029 RVA: 0x000CC53C File Offset: 0x000CA73C
	public override bool HasInteraction(PlayerController interactor)
	{
		return base.HasInteraction(interactor) || this.HasInsertableCorpseOverhead() || (this.HasTakableZombieOverhead() && this.assignedWgo.Data.GetGameResInt("resurrection_prepared") == 0) || this.assignedWgo.Data.GetGameResInt("resurrection_prepared") == 0;
	}

	// Token: 0x06002B16 RID: 11030 RVA: 0x000CC59C File Offset: 0x000CA79C
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (this.assignedWgo.Data.CraftComponent.IsDestroyingCraftActive)
		{
			interactionInfos2.Add(base.GetInteractionInfoByUsingTool(true));
			return interactionInfos2;
		}
		if (this.HasInsertableCorpseOverhead())
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_place_body", GameKey.Interaction)));
			return interactionInfos2;
		}
		if (this.HasTakableZombieOverhead() && this.assignedWgo.Data.GetGameResInt("resurrection_prepared") == 0)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
			return interactionInfos2;
		}
		if (this.assignedWgo.Data.GetGameResInt("resurrection_prepared") == 0)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("action_inspect", GameKey.Interaction)));
			return interactionInfos2;
		}
		return interactionInfos2;
	}

	// Token: 0x06002B17 RID: 11031 RVA: 0x000CC67C File Offset: 0x000CA87C
	private bool HasInsertableCorpseOverhead()
	{
		Item item;
		return this.TryGetInsertableCorpseOverhead(out item);
	}

	// Token: 0x06002B18 RID: 11032 RVA: 0x000CC694 File Offset: 0x000CA894
	private bool TryGetInsertableCorpseOverhead(out Item corpseItem)
	{
		corpseItem = null;
		if (this.interactor == null)
		{
			return false;
		}
		if (!this.assignedWgo.Data.Inventory.GetItemByGroupId("body").IsEmpty)
		{
			return false;
		}
		return this.interactor.PlayerData.TryGetOverheadItem((Item item) => item.Definition.itemGroupIds.Contains("corpse"), out corpseItem);
	}

	// Token: 0x06002B19 RID: 11033 RVA: 0x000CC708 File Offset: 0x000CA908
	private bool HasTakableZombieOverhead()
	{
		return this.interactor != null && this.interactor.PlayerData.HasFreeOverheadSlot && !this.assignedWgo.Data.Inventory.GetItemByGroupId("zombie").IsEmpty;
	}
}
