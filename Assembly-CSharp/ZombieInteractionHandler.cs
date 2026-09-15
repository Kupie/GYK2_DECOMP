using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200066D RID: 1645
public class ZombieInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B6C RID: 11116 RVA: 0x000CDBBC File Offset: 0x000CBDBC
	public override bool HasInteraction(PlayerController interactor)
	{
		ZombieWgoData zombieWgoData = this.assignedWgo.Data as ZombieWgoData;
		return zombieWgoData == null || zombieWgoData.AttachedWgoData == null || (!(zombieWgoData.AttachedWgoData.id == "sawmill_wood_crafter") && !(zombieWgoData.AttachedWgoData.id == "mine_ore_coal_crafter"));
	}

	// Token: 0x06002B6D RID: 11117 RVA: 0x000CDC18 File Offset: 0x000CBE18
	public override bool HasInteraction2(PlayerController interactor)
	{
		ZombieWgoData zombieWgoData = this.assignedWgo.Data as ZombieWgoData;
		return zombieWgoData == null || zombieWgoData.AttachedWgoData == null || (!(zombieWgoData.AttachedWgoData.id == "sawmill_wood_crafter") && !(zombieWgoData.AttachedWgoData.id == "mine_ore_coal_crafter"));
	}

	// Token: 0x06002B6E RID: 11118 RVA: 0x000CDC74 File Offset: 0x000CBE74
	public override bool Interact(PlayerController interactor)
	{
		ZombieWgoData zombieWgoData = this.assignedWgo.Data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			WgoData attachedWgoData = zombieWgoData.AttachedWgoData;
			if (attachedWgoData != null)
			{
				if (attachedWgoData.CraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
				{
					attachedWgoData.CraftComponent.ContinueAutoCraft();
					List<Item> list = new List<Item>();
					list.AddRange(attachedWgoData.CraftableObjectCraftInventory.Data.RemoveAllItems());
					if (list.Count > 0)
					{
						foreach (Item item in list)
						{
							MainGame.Instance.dropSystem.DropItem(item, attachedWgoData.WorldId, attachedWgoData.Position, null);
						}
					}
					attachedWgoData.DropStoredTechPoints();
				}
				zombieWgoData.UnAttachFromWgoData(false);
			}
			if (!SGuid.IsNullOrEmpty(zombieWgoData.takenDockPointsParentSGuid))
			{
				WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(zombieWgoData.takenDockPointsParentSGuid);
				if (wgoData != null)
				{
					DockPointData occupiedDockPointBy = wgoData.MainWgoPartData.GetOccupiedDockPointBy(zombieWgoData.UniqueId);
					if (occupiedDockPointBy != null)
					{
						occupiedDockPointBy.UnOccupy();
					}
				}
				zombieWgoData.takenDockPointsParentSGuid = null;
			}
			this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
			MainGame.ZombieSystemData.PutZombieFromGameSceneToStoreForPlayer(MainGame.PlayerData, zombieWgoData);
			return true;
		}
		return false;
	}

	// Token: 0x06002B6F RID: 11119 RVA: 0x000CDDC8 File Offset: 0x000CBFC8
	public override bool Interact2(PlayerController interactor)
	{
		ZombieWgoData zombieWgoData = this.assignedWgo.Data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			UIZombieWorkerWindowData uizombieWorkerWindowData = new UIZombieWorkerWindowData(zombieWgoData);
			LazyUI.GetWindow<UIZombieWorkerWindow>().Open(uizombieWorkerWindowData);
			return true;
		}
		return false;
	}

	// Token: 0x06002B70 RID: 11120 RVA: 0x000CDE00 File Offset: 0x000CC000
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		InteractionInfos interactionInfos2 = new InteractionInfos();
		if (this.assignedWgo.Data is ZombieWgoData)
		{
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("action_inspect", GameKey.Action)));
		}
		return interactionInfos2;
	}
}
