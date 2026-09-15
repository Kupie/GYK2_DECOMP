using System;
using GK2.FlowCanvasNodes;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public class ZombieMineInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002B72 RID: 11122 RVA: 0x000CDE70 File Offset: 0x000CC070
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
			this.SetupZombieVisual(zombieWgoData);
			this.assignedWgo.Data.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
			this.assignedWgo.DrawWidgets();
			if (this.assignedWgo.Data.CraftComponent.CurrentCraftElement == null)
			{
				CraftParamsData craftParamsData = new CraftParamsData(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, this.assignedWgo.Data, CraftParamsData.CraftParamsType.Common, -1);
				craftParamsData.customRes.Set("wait_for_zombie_at_mine", 1f);
				craftParamsData.customRes.Set("ignore_handle_output", 1f);
				this.assignedWgo.Data.CraftComponent.AddToQueue(new CraftElement(this.assignedWgo.Data.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData), false, -1);
			}
		}
		else
		{
			ZombieWgoData zombieWgoData2 = this.assignedWgo.Data.Worker as ZombieWgoData;
			if (zombieWgoData2 != null && MainGame.PlayerData.HasFreeOverheadSlot)
			{
				if (zombieWgoData2.GameResStr.Has("mine_point"))
				{
					MainGame.WorldData.GetWgoData("builder_mine").SetGameRes(zombieWgoData2.GameResStr.Get("mine_point", ""), 0);
					zombieWgoData2.GameResStr.Remove("mine_point");
					zombieWgoData2.FireEvent("mine_craft_end");
				}
				else if (zombieWgoData2.CaretakerPortableItem != null && !zombieWgoData2.CaretakerPortableItem.IsEmpty)
				{
					Flow_FinishZombieMineCraft.FinishCraft(zombieWgoData2.AttachedWgoData, false);
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

	// Token: 0x06002B73 RID: 11123 RVA: 0x000CE120 File Offset: 0x000CC320
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

	// Token: 0x06002B74 RID: 11124 RVA: 0x000CBC10 File Offset: 0x000C9E10
	public override bool HasInteraction(PlayerController interactor)
	{
		return (this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead()) || (this.assignedWgo.Data.Worker != null && MainGame.PlayerData.HasFreeOverheadSlot);
	}

	// Token: 0x06002B75 RID: 11125 RVA: 0x000CD6DF File Offset: 0x000CB8DF
	public override bool HasInteraction2(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker != null;
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x000CE15C File Offset: 0x000CC35C
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

	// Token: 0x06002B77 RID: 11127 RVA: 0x000CE20C File Offset: 0x000CC40C
	private void SetupZombieVisual(ZombieWgoData zombieData)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(zombieData.UniqueId);
		if (wgoViewGlobal == null)
		{
			return;
		}
		int gameResInt = zombieData.GetGameResInt("zombie_head_id");
		int gameResInt2 = zombieData.GetGameResInt("zombie_body_id");
		string text = zombieData.GameResStr.Get("zombie_head_lut", "");
		int num;
		switch (gameResInt)
		{
		case 1050:
		case 1056:
			num = 1701;
			goto IL_0098;
		case 1052:
		case 1054:
			num = 1702;
			goto IL_0098;
		case 1058:
			num = 1703;
			goto IL_0098;
		}
		num = 1700;
		IL_0098:
		int num2 = num;
		SkinPresetGK2 presetForCustomizationData = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", gameResInt2, num2, string.Empty, text);
		if (presetForCustomizationData != null)
		{
			AnimationComponent animationComponent = wgoViewGlobal.MainWgoPart.AnimationComponent as AnimationComponent;
			if (animationComponent != null)
			{
				animationComponent.SetSkinPreset(presetForCustomizationData);
				animationComponent.ChangeSkinPreset(presetForCustomizationData);
			}
		}
	}
}
