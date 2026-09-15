using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000655 RID: 1621
public class PowerSourceInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B00 RID: 11008 RVA: 0x000CBD00 File Offset: 0x000C9F00
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (this.TryGetInsertableZombieOverhead(out item) && this.HasNotOccupiedDockPoints())
		{
			DockPointData dockPointData = this.assignedWgo.Data.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.OnlyZombie)[0];
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, dockPointData.GetPosFrom(this.assignedWgo.Data.Position), dockPointData.Direction);
			zombieWgoData.OccupyDockPoint(dockPointData, this.assignedWgo.Data.UniqueId);
			zombieWgoData.IsInteractable = false;
			this.assignedWgo.DrawWidgets();
			this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
		}
		else if (MainGame.PlayerData.HasFreeOverheadSlot && this.HasOccupiedDockPoints())
		{
			List<DockPointData> dockPoints = this.assignedWgo.Data.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyOccupied, DockPointData.Filter.OnlyZombie);
			DockPointData dockPointData2 = dockPoints[dockPoints.Count - 1];
			SGuid occupiedBy = dockPointData2.OccupiedBy;
			ZombieWgoData zombie = MainGame.Instance.GameSave.zombieSystemData.GetZombie(occupiedBy);
			zombie.UnOccupyDockPoint(dockPointData2);
			zombie.IsInteractable = true;
			MainGame.ZombieSystemData.PutZombieFromGameSceneToStoreForPlayer(MainGame.PlayerData, zombie);
			this.assignedWgo.DrawWidgets();
			this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x000CBE72 File Offset: 0x000CA072
	public override bool HasInteraction(PlayerController interactor)
	{
		return (base.HasInsertableZombieOverhead() && this.HasNotOccupiedDockPoints()) || (MainGame.PlayerData.HasFreeOverheadSlot && this.HasOccupiedDockPoints());
	}

	// Token: 0x06002B02 RID: 11010 RVA: 0x000CBEA0 File Offset: 0x000CA0A0
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (base.HasInsertableZombieOverhead() && this.HasNotOccupiedDockPoints())
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_zombie", GameKey.Interaction)));
		}
		if (MainGame.PlayerData.HasFreeOverheadSlot && this.HasOccupiedDockPoints())
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
		}
		return interactionInfos2;
	}

	// Token: 0x06002B03 RID: 11011 RVA: 0x000CBF1F File Offset: 0x000CA11F
	private bool HasOccupiedDockPoints()
	{
		return this.assignedWgo.Data.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyOccupied, DockPointData.Filter.OnlyZombie).Count > 0;
	}

	// Token: 0x06002B04 RID: 11012 RVA: 0x000CBF40 File Offset: 0x000CA140
	private bool HasNotOccupiedDockPoints()
	{
		return this.assignedWgo.Data.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.OnlyZombie).Count > 0;
	}
}
