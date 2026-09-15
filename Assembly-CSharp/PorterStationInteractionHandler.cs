using System;
using LazyBearTechnology;

// Token: 0x02000654 RID: 1620
public class PorterStationInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AFA RID: 11002 RVA: 0x000CBAFC File Offset: 0x000C9CFC
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (this.assignedWgo.Data.Worker == null && this.TryGetInsertableZombieOverhead(out item))
		{
			GDPointData gdpointData = this.assignedWgo.Data.GetGDPointData("zombie_porter_station_gd_point");
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, gdpointData.Position, Direction.Down);
			this.assignedWgo.Data.TrySetWorker(zombieWgoData, null);
			zombieWgoData.AttachToPorterStation(this.assignedWgo.Data.UniqueId, item, null);
			zombieWgoData.direction.Value = Direction.Down.ConvertToVector2XZ();
		}
		else
		{
			ZombieWgoData zombieWgoData2 = this.assignedWgo.Data.Worker as ZombieWgoData;
			if (zombieWgoData2 != null && MainGame.PlayerData.HasFreeOverheadSlot)
			{
				zombieWgoData2.UnAttachFromWgoData(true);
				MainGame.ZombieSystemData.PutZombieFromGameSceneToStoreForPlayer(MainGame.PlayerData, zombieWgoData2);
			}
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x000CBBF3 File Offset: 0x000C9DF3
	public override bool Interact2(PlayerController interactor)
	{
		LazyUI.GetWindow<UIPorterStationWindow>().Open(new UIPorterStationWindowData(this.assignedWgo.Data));
		return true;
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x000CBC10 File Offset: 0x000C9E10
	public override bool HasInteraction(PlayerController interactor)
	{
		return (this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead()) || (this.assignedWgo.Data.Worker != null && MainGame.PlayerData.HasFreeOverheadSlot);
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override bool HasInteraction2(PlayerController interactor)
	{
		return true;
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x000CBC50 File Offset: 0x000C9E50
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead())
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_zombie", GameKey.Interaction)));
		}
		else if (this.assignedWgo.Data.Worker != null && MainGame.PlayerData.HasFreeOverheadSlot)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
		}
		interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_open", GameKey.Action)));
		return interactionInfos2;
	}
}
