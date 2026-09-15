using System;
using LazyBearTechnology;

// Token: 0x0200063E RID: 1598
public class ConveyorTransporterStationInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A7A RID: 10874 RVA: 0x000C87EC File Offset: 0x000C69EC
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
				zombieWgoData.AttachToConveyorTransporterStationWgoData(this.assignedWgo.Data.UniqueId, zombieItem, null);
			}
			else
			{
				Bubble.Talk(new PhraseData(true, null, "zombie_supplier_station_no_overhead", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			}
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002A7B RID: 10875 RVA: 0x000C88A8 File Offset: 0x000C6AA8
	public override bool HasInteraction(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker == null;
	}

	// Token: 0x06002A7C RID: 10876 RVA: 0x000C88C0 File Offset: 0x000C6AC0
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
}
