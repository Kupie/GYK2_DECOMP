using System;
using LazyBearTechnology;

// Token: 0x0200063C RID: 1596
public class ChoirPlaceInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A6A RID: 10858 RVA: 0x000C82FD File Offset: 0x000C64FD
	private bool CanAddToInventory(Item item)
	{
		return this.assignedWgo.Data.Inventory.CanAddItemToInventory(item);
	}

	// Token: 0x170006C2 RID: 1730
	// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000C8315 File Offset: 0x000C6515
	private bool HasInventorySpace
	{
		get
		{
			return this.assignedWgo.Data.Inventory.Data.InventorySize > this.assignedWgo.Data.Inventory.Data.InventoryFillSize;
		}
	}

	// Token: 0x06002A6C RID: 10860 RVA: 0x000C8350 File Offset: 0x000C6550
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (this.TryGetInsertableZombieOverhead(out item) && this.CanAddToInventory(item))
		{
			this.InsertOverheadItem(item);
			this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
		}
		else if (this.HasInventorySpace)
		{
			Bubble.Talk(new PhraseData(true, null, "zombie_supplier_station_no_overhead", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
		}
		return true;
	}

	// Token: 0x06002A6D RID: 10861 RVA: 0x000C83C0 File Offset: 0x000C65C0
	public override bool Interact2(PlayerController interactor)
	{
		if (base.Interact2(interactor))
		{
			return true;
		}
		Item itemByGroupId = this.assignedWgo.Data.Inventory.GetItemByGroupId("zombie");
		if (itemByGroupId != null && itemByGroupId.Count > 0)
		{
			Item itemByGroupId2 = this.assignedWgo.Data.Inventory.GetItemByGroupId("zombie");
			if (!interactor.PlayerData.TryAddOverheadItemNoReplace(itemByGroupId2))
			{
				return true;
			}
			this.assignedWgo.Data.Inventory.RemoveItemFromInventoryByUID(itemByGroupId2, 1);
			this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
		}
		return true;
	}

	// Token: 0x06002A6E RID: 10862 RVA: 0x000C845C File Offset: 0x000C665C
	public override bool HasInteraction(PlayerController interactor)
	{
		Item item;
		return (this.TryGetInsertableZombieOverhead(out item) && this.CanAddToInventory(item)) || this.HasInventorySpace;
	}

	// Token: 0x06002A6F RID: 10863 RVA: 0x000C8487 File Offset: 0x000C6687
	public override bool HasInteraction2(PlayerController interactor)
	{
		return interactor != null && this.assignedWgo.Data.Inventory.GetItemsByGroupId("zombie").Count > 0 && interactor.PlayerData.HasFreeOverheadSlot;
	}

	// Token: 0x06002A70 RID: 10864 RVA: 0x000C84C4 File Offset: 0x000C66C4
	private void InsertOverheadItem(Item zombieItem)
	{
		if (zombieItem == null)
		{
			return;
		}
		MainGame.Instance.GameSave.playerData.RemoveOverheadItem(zombieItem);
		this.assignedWgo.Data.Inventory.AddItemToInventory(zombieItem, null, false);
		this.LinkZombie(zombieItem);
	}

	// Token: 0x06002A71 RID: 10865 RVA: 0x000C8500 File Offset: 0x000C6700
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		Item item;
		if ((this.TryGetInsertableZombieOverhead(out item) && this.CanAddToInventory(item)) || this.HasInventorySpace)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_zombie", GameKey.Interaction)));
		}
		Item itemByGroupId = this.assignedWgo.Data.Inventory.GetItemByGroupId("zombie");
		if (itemByGroupId != null && itemByGroupId.Count > 0 && MainGame.PlayerData.HasFreeOverheadSlot)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Action)));
		}
		return interactionInfos2;
	}

	// Token: 0x06002A72 RID: 10866 RVA: 0x000C85B0 File Offset: 0x000C67B0
	private void LinkZombie(Item sourceZombieItem)
	{
		ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(sourceZombieItem.UniqueId);
		if (zombie != null)
		{
			Item itemByUniqueId = this.assignedWgo.Data.Inventory.GetItemByUniqueId(sourceZombieItem.UniqueId.Id);
			if (!itemByUniqueId.IsEmpty)
			{
				zombie.SetZombieItem(itemByUniqueId);
			}
		}
	}
}
