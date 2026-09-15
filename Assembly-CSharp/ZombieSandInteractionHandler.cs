using System;
using GK2.FlowCanvasNodes;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200066F RID: 1647
public class ZombieSandInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B79 RID: 11129 RVA: 0x000CE2FC File Offset: 0x000CC4FC
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (this.assignedWgo.Data.Worker == null && this.TryGetInsertableZombieOverhead(out item))
		{
			GDPointData gdpointData = this.assignedWgo.Data.GetGDPointData("zombie_clay_sand_crafter_gd_point");
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsCommon(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, gdpointData.Position, Direction.Down);
			this.assignedWgo.Data.TrySetWorker(zombieWgoData, null);
			zombieWgoData.IsInteractable = false;
			zombieWgoData.AttachToCraftWgoData(this.assignedWgo.Data.UniqueId, item, null);
			this.assignedWgo.Data.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
			this.assignedWgo.DrawWidgets();
			if (this.assignedWgo.Data.CraftComponent.CurrentCraftElement == null)
			{
				CraftParamsData craftParamsData = new CraftParamsData(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, this.assignedWgo.Data, CraftParamsData.CraftParamsType.Common, -1);
				craftParamsData.customRes.Set("wait_for_zombie_at_sand", 1f);
				craftParamsData.customRes.Set("ignore_handle_output", 1f);
				this.assignedWgo.Data.CraftComponent.AddToQueue(new CraftElement(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
			}
		}
		else
		{
			ZombieWgoData zombieWgoData2 = this.assignedWgo.Data.Worker as ZombieWgoData;
			if (zombieWgoData2 != null && MainGame.PlayerData.HasFreeOverheadSlot)
			{
				if (zombieWgoData2.GameResStr.Has("sand_point"))
				{
					MainGame.WorldData.GetWgoData("builder_clay_sand").SetGameRes(zombieWgoData2.GameResStr.Get("sand_point", ""), 0);
					zombieWgoData2.GameResStr.Remove("sand_point");
					zombieWgoData2.FireEvent("sand_craft_end");
				}
				else if (zombieWgoData2.CaretakerPortableItem != null && !zombieWgoData2.CaretakerPortableItem.IsEmpty)
				{
					Flow_FinishZombieSandCraft.FinishCraft(zombieWgoData2.AttachedWgoData, false);
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

	// Token: 0x06002B7A RID: 11130 RVA: 0x000CE5A0 File Offset: 0x000CC7A0
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

	// Token: 0x06002B7B RID: 11131 RVA: 0x000CBC10 File Offset: 0x000C9E10
	public override bool HasInteraction(PlayerController interactor)
	{
		return (this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead()) || (this.assignedWgo.Data.Worker != null && MainGame.PlayerData.HasFreeOverheadSlot);
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x000CE5DC File Offset: 0x000CC7DC
	protected override bool TryGetInsertableZombieOverhead(out Item zombieItem)
	{
		zombieItem = null;
		if (this.interactor == null || !this.assignedWgo.Data.Definition.canInsertZombie || this.assignedWgo.Data.Worker != null)
		{
			return false;
		}
		return this.interactor.PlayerData.TryGetOverheadItem((Item item) => item.Definition.itemGroupIds.Contains("zombie"), out zombieItem);
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x000CD6DF File Offset: 0x000CB8DF
	public override bool HasInteraction2(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker != null;
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x000CE658 File Offset: 0x000CC858
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
