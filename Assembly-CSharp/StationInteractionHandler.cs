using System;
using LazyBearTechnology;

// Token: 0x0200065F RID: 1631
public class StationInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B2E RID: 11054 RVA: 0x000CCA08 File Offset: 0x000CAC08
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		if (this.assignedWgo.Data.Worker == null)
		{
			Item item;
			if (this.TryGetInsertableZombieOverhead(out item))
			{
				GDPointData gdpointData = this.assignedWgo.Data.GetGDPointData("zombie_porter_station_gd_point");
				ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsAssistant(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, gdpointData.Position, gdpointData.Direction);
				Item zombieItem = zombieWgoData.ZombieItem;
				zombieWgoData.AttachToStationWgoData(this.assignedWgo.Data.UniqueId, zombieItem, null);
			}
			else
			{
				Bubble.Talk(new PhraseData(true, null, "zombie_supplier_station_no_overhead", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			}
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x000C88A8 File Offset: 0x000C6AA8
	public override bool HasInteraction(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker == null;
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x000CCAC4 File Offset: 0x000CACC4
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (this.assignedWgo.Data.Worker == null)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_zombie", GameKey.Interaction)));
		}
		return interactionInfos2;
	}

	// Token: 0x04002353 RID: 9043
	private CraftComponent assignedCraftComponent;
}
