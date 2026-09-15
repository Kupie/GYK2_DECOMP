using System;
using LazyBearTechnology;

// Token: 0x02000667 RID: 1639
public class TownPaletteInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B4E RID: 11086 RVA: 0x000CD278 File Offset: 0x000CB478
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item2;
		if (MainGame.PlayerData.TryGetOverheadItem((Item item) => this.assignedWgo.Data.Inventory.CanAddItemToInventory(item), out item2))
		{
			MainGame.PlayerData.InsertOverheadItemTo(this.assignedWgo.Data, item2);
			this.assignedWgo.DrawWidgets();
		}
		return true;
	}

	// Token: 0x06002B4F RID: 11087 RVA: 0x000CD2CC File Offset: 0x000CB4CC
	public override bool HasInteraction(PlayerController interactor)
	{
		Item item2;
		return base.HasInteraction(interactor) || MainGame.PlayerData.TryGetOverheadItem((Item item) => this.assignedWgo.Data.Inventory.CanAddItemToInventory(item), out item2);
	}

	// Token: 0x06002B50 RID: 11088 RVA: 0x000CD304 File Offset: 0x000CB504
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		Item item2;
		if (MainGame.PlayerData.TryGetOverheadItem((Item item) => this.assignedWgo.Data.Inventory.CanAddItemToInventory(item), out item2))
		{
			return new InteractionInfos(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put", GameKey.Interaction)));
		}
		return new InteractionInfos(new InteractionInfo(""));
	}
}
