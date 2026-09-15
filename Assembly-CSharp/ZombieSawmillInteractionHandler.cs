using System;
using GK2.FlowCanvasNodes;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000671 RID: 1649
public class ZombieSawmillInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B83 RID: 11139 RVA: 0x000CE714 File Offset: 0x000CC914
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
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, dockPoint.transform.position, Direction.Down);
			this.assignedWgo.Data.TrySetWorker(zombieWgoData, null);
			zombieWgoData.IsInteractable = false;
			zombieWgoData.AttachToCraftWgoData(this.assignedWgo.Data.UniqueId, item, null);
			this.assignedWgo.Data.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
			this.assignedWgo.DrawWidgets();
			if (this.assignedWgo.Data.CraftComponent.CurrentCraftElement == null)
			{
				CraftParamsData craftParamsData = new CraftParamsData(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, this.assignedWgo.Data, CraftParamsData.CraftParamsType.Common, -1);
				craftParamsData.customRes.Set("wait_for_zombie_at_sawmill", 1f);
				craftParamsData.customRes.Set("ignore_handle_output", 1f);
				this.assignedWgo.Data.CraftComponent.AddToQueue(new CraftElement(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
			}
		}
		else
		{
			ZombieWgoData zombieWgoData2 = this.assignedWgo.Data.Worker as ZombieWgoData;
			if (zombieWgoData2 != null && MainGame.PlayerData.HasFreeOverheadSlot)
			{
				if (zombieWgoData2.GameResStr.Has("sawmill_point"))
				{
					MainGame.WorldData.GetWgoData("builder_sawmill").SetGameRes(zombieWgoData2.GameResStr.Get("sawmill_point", ""), 0);
					zombieWgoData2.GameResStr.Remove("sawmill_point");
					zombieWgoData2.FireEvent("sawmill_craft_end");
				}
				else if (zombieWgoData2.CaretakerPortableItem != null && !zombieWgoData2.CaretakerPortableItem.IsEmpty)
				{
					Flow_FinishZombieSawmillCraft.FinishCraft(zombieWgoData2.AttachedWgoData, false);
					zombieWgoData2.CaretakerPortableItem = Item.Empty;
				}
				this.assignedWgo.Data.SetGameRes("stuff_disabled", 0);
				this.assignedWgo.Data.CraftComponent.Clear();
				zombieWgoData2.UnAttachFromWgoData(false);
				zombieWgoData2.IsInteractable = true;
				this.assignedWgo.Data.WorldZoneData.NotifyWgoDataChanged();
				MainGame.ZombieSystemData.PutZombieFromGameSceneToStoreForPlayer(MainGame.PlayerData, zombieWgoData2);
			}
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x000CE9C8 File Offset: 0x000CCBC8
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

	// Token: 0x06002B85 RID: 11141 RVA: 0x000CBC10 File Offset: 0x000C9E10
	public override bool HasInteraction(PlayerController interactor)
	{
		return (this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead()) || (this.assignedWgo.Data.Worker != null && MainGame.PlayerData.HasFreeOverheadSlot);
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x000CD6DF File Offset: 0x000CB8DF
	public override bool HasInteraction2(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker != null;
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x000CEA04 File Offset: 0x000CCC04
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
