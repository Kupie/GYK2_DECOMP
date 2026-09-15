using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000649 RID: 1609
public class FightersContainerInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002AB7 RID: 10935 RVA: 0x000C9F18 File Offset: 0x000C8118
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (!this.TryGetInsertableZombieOverhead(out item))
		{
			return true;
		}
		Vector3 value = MainGame.PlayerData.position.Value;
		DockPointData nearestDockPointData = this.assignedWgo.Data.GetNearestDockPointData(value, DockPointData.Availability.OnlyNotOccupied);
		if (nearestDockPointData == null)
		{
			return false;
		}
		ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, this.assignedWgo.Data.GetDockPointDataWorldPosition(nearestDockPointData), nearestDockPointData.Direction);
		zombieWgoData.AttachToFightersContainer();
		nearestDockPointData.Occupy(zombieWgoData.UniqueId);
		zombieWgoData.takenDockPointsParentSGuid = this.assignedWgo.Data.UniqueId;
		zombieWgoData.GameResStr.Set("fighters_flag", this.assignedWgo.Data.GameResStr.Get("fighters_flag", ""));
		GameScene.GetWgoViewGlobal(zombieWgoData.UniqueId).InitZombieFighter();
		this.assignedWgo.DrawWidgets();
		if (Vector3.Distance(this.assignedWgo.Data.GetDockPointDataWorldPosition(nearestDockPointData), interactor.PlayerData.position.Value) <= 1f)
		{
			interactor.TryTeleportPlayerToAnyFreePlace();
		}
		WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.WorldData.GetWorldZoneDataById("town_guard_barracks");
		if (worldZoneDataById != null)
		{
			worldZoneDataById.NotifyWgoDataChanged();
		}
		return true;
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x00028294 File Offset: 0x00026494
	public override bool Interact2(PlayerController interactor)
	{
		return false;
	}

	// Token: 0x06002AB9 RID: 10937 RVA: 0x000CA065 File Offset: 0x000C8265
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		base.HasInsertableZombieOverhead();
		return true;
	}

	// Token: 0x06002ABA RID: 10938 RVA: 0x00028294 File Offset: 0x00026494
	public override bool HasInteraction2(PlayerController interactor)
	{
		return false;
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000CA07C File Offset: 0x000C827C
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (base.HasInsertableZombieOverhead())
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_put_zombie", GameKey.Interaction)));
			return interactionInfos2;
		}
		return interactionInfos2;
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x000CA0C8 File Offset: 0x000C82C8
	protected override bool TryGetInsertableZombieOverhead(out Item zombieItem)
	{
		zombieItem = null;
		if (this.interactor == null || !this.assignedWgo.Data.Definition.canInsertZombie || this.assignedWgo.DockPoints == null || this.assignedWgo.DockPoints.Count == 0 || this.assignedWgo.Data.GetNearestDockPointData(MainGame.PlayerData.position.Value, DockPointData.Availability.OnlyNotOccupied) == null)
		{
			return false;
		}
		return this.interactor.PlayerData.TryGetOverheadItem((Item item) => item.Definition.itemGroupIds.Contains("zombie"), out zombieItem);
	}

	// Token: 0x0400233E RID: 9022
	private CraftComponent assignedCraftComponent;
}
