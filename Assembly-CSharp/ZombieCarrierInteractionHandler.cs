using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200066A RID: 1642
public class ZombieCarrierInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B5C RID: 11100 RVA: 0x000CD4A0 File Offset: 0x000CB6A0
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (this.assignedWgo.Data.Worker == null && this.TryGetInsertableZombieOverhead(out item))
		{
			DockPoint dockPoint = this.assignedWgo.TryGetDockPointForWorker(true, interactor.transform.position);
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, dockPoint.transform.position, dockPoint.Direction);
			DockPointData dockPointData = this.assignedWgo.GetDockPointData(dockPoint);
			zombieWgoData.AttachToCraftWgoData(this.assignedWgo.Data.UniqueId, item, dockPointData);
			this.assignedWgo.Data.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
			this.assignedWgo.DrawWidgets();
			if (this.assignedWgo.Data.CraftComponent.CurrentCraftElement == null)
			{
				CraftParamsData craftParamsData = new CraftParamsData(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, this.assignedWgo.Data, CraftParamsData.CraftParamsType.Common, -1);
				craftParamsData.customRes.Set("auto_start_same_craft_after_pickup", 1f);
				craftParamsData.customRes.Set("do_not_check_multiinventory_space", 1f);
				craftParamsData.customRes.Set("do_not_check_worker_dependent_values", 1f);
				this.assignedWgo.Data.CraftComponent.TryStartCraft(new CraftElement(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData));
			}
			zombieWgoData.CrafterStopCraftActivity();
			zombieWgoData.CrafterStartCraftActivity(false);
		}
		else
		{
			ZombieWgoData zombieWgoData2 = this.assignedWgo.Data.Worker as ZombieWgoData;
			if (zombieWgoData2 != null && MainGame.PlayerData.HasFreeOverheadSlot)
			{
				zombieWgoData2.UnAttachFromWgoData(false);
				this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
				MainGame.ZombieSystemData.PutZombieFromGameSceneToStoreForPlayer(MainGame.PlayerData, zombieWgoData2);
			}
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002B5D RID: 11101 RVA: 0x000CD6A4 File Offset: 0x000CB8A4
	public override bool Interact2(PlayerController interactor)
	{
		ZombieWgoData zombieWgoData = this.assignedWgo.Data.Worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			UIZombieWorkerWindowData uizombieWorkerWindowData = new UIZombieWorkerWindowData(zombieWgoData);
			LazyUI.GetWindow<UIZombieWorkerWindow>().Open(uizombieWorkerWindowData);
			return true;
		}
		return false;
	}

	// Token: 0x06002B5E RID: 11102 RVA: 0x000CBC10 File Offset: 0x000C9E10
	public override bool HasInteraction(PlayerController interactor)
	{
		return (this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead()) || (this.assignedWgo.Data.Worker != null && MainGame.PlayerData.HasFreeOverheadSlot);
	}

	// Token: 0x06002B5F RID: 11103 RVA: 0x000CD6DF File Offset: 0x000CB8DF
	public override bool HasInteraction2(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker != null;
	}

	// Token: 0x06002B60 RID: 11104 RVA: 0x000CD6F4 File Offset: 0x000CB8F4
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
		else if (this.assignedWgo.Data.Worker != null)
		{
			if (MainGame.PlayerData.HasFreeOverheadSlot)
			{
				interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("hint_take", GameKey.Interaction)));
			}
			interactionInfos2.Add(new InteractionInfo(base.LocalizeHintWithActionIcon("action_inspect", GameKey.Action)));
		}
		return interactionInfos2;
	}
}
