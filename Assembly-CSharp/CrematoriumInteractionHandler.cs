using System;
using LazyBearTechnology;

// Token: 0x02000642 RID: 1602
public class CrematoriumInteractionHandler : CraftInteractionHandler
{
	// Token: 0x06002A93 RID: 10899 RVA: 0x000C9448 File Offset: 0x000C7648
	public override bool Interact(PlayerController interactor)
	{
		Item item;
		if (!this.TryGetInsertableOverheadCorpse(out item))
		{
			return false;
		}
		this.assignedWgo.Data.Inventory.AddItemToInventory(item, null, false);
		MainGame.Instance.GameSave.playerData.RemoveOverheadItem(item);
		CraftComponent craftComponent = this.assignedWgo.Data.CraftComponent;
		CraftDefBase craftDefBase = craftComponent.AvailableCrafts[0];
		CraftParamsData craftParamsData = new CraftParamsData(craftDefBase.id, this.assignedWgo.Data, CraftParamsData.CraftParamsType.Common, -1);
		craftComponent.TryStartCraft(new CraftElement(craftDefBase.id, 1, craftParamsData));
		MainGame.PlayerData.SubRes("cur_bodies_count", 1f);
		if (MainGame.PlayerData.CurrentWorldZoneData != null && MainGame.PlayerData.CurrentWorldZoneData.Definition.id == "morgue")
		{
			GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
		}
		this.assignedWgo.DrawWidgets();
		return true;
	}

	// Token: 0x06002A94 RID: 10900 RVA: 0x000C9538 File Offset: 0x000C7738
	public override bool HasInteraction(PlayerController interactor)
	{
		Item item;
		if (this.assignedWgo != null && this.assignedWgo.Data != null && this.assignedWgo.Data.CraftComponent != null && !this.assignedWgo.Data.CraftComponent.IsStarted && this.assignedWgo.Data.CraftComponent.Status != CraftComponentStatus.ReadyToFinishAutoCraft && !this.HasCorpseItemInside(out item) && this.HasInsertableOverheadBodyItem())
		{
			this.assignedCraftComponent = this.assignedWgo.Data.CraftComponent;
			return true;
		}
		return false;
	}

	// Token: 0x06002A95 RID: 10901 RVA: 0x000C95CC File Offset: 0x000C77CC
	public override bool HasInteraction2(PlayerController interactor)
	{
		if (this.assignedWgo != null && this.assignedWgo.Data != null && this.assignedWgo.Data.CraftComponent != null && this.assignedWgo.Data.CraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
		{
			this.assignedCraftComponent = this.assignedWgo.Data.CraftComponent;
			return base.HasInteraction2(interactor);
		}
		return false;
	}

	// Token: 0x06002A96 RID: 10902 RVA: 0x000C9640 File Offset: 0x000C7840
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = new InteractionInfos();
		if (this.assignedWgo != null && this.assignedWgo.Data != null && this.assignedWgo.Data.CraftComponent != null)
		{
			this.assignedCraftComponent = this.assignedWgo.Data.CraftComponent;
			Item item;
			if (!this.assignedWgo.Data.CraftComponent.IsStarted && this.assignedWgo.Data.CraftComponent.Status != CraftComponentStatus.ReadyToFinishAutoCraft && !this.HasCorpseItemInside(out item) && this.HasInsertableOverheadBodyItem())
			{
				interactionInfos.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_cremate_body", GameKey.Interaction)));
			}
			else if (this.assignedWgo.Data.CraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
			{
				interactionInfos.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take_all", GameKey.Action)));
			}
			else
			{
				interactionInfos = base.FormInteractionInfo();
			}
		}
		else
		{
			interactionInfos = base.FormInteractionInfo();
		}
		return interactionInfos;
	}

	// Token: 0x06002A97 RID: 10903 RVA: 0x000C9744 File Offset: 0x000C7944
	private bool HasInsertableOverheadBodyItem()
	{
		Item item;
		return !this.HasCorpseItemInside(out item) && this.TryGetInsertableOverheadCorpse(out item);
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x000C9765 File Offset: 0x000C7965
	private bool TryGetInsertableOverheadCorpse(out Item overheadItem)
	{
		return MainGame.Instance.GameSave.playerData.TryGetOverheadItem((Item item) => !item.HasItemsByItemType(ItemType.Demon) && item.Definition.itemGroupIds.Contains("corpse"), out overheadItem);
	}

	// Token: 0x06002A99 RID: 10905 RVA: 0x000C979B File Offset: 0x000C799B
	private bool HasCorpseItemInside(out Item bodyItem)
	{
		if (this.assignedWgo.Data.Inventory.Data.TryGetItemInInventoryByGroupId("corpse", out bodyItem))
		{
			return true;
		}
		bodyItem = null;
		return false;
	}
}
