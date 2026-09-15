using System;
using LazyBearTechnology;

// Token: 0x0200064E RID: 1614
public class GardenStationInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002ADD RID: 10973 RVA: 0x000CB228 File Offset: 0x000C9428
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		Item item;
		if (this.assignedWgo.Data.Worker == null && this.TryGetInsertableZombieOverhead(out item))
		{
			GDPointData gdpointData = this.assignedWgo.Data.GetGDPointData("zombie_garden_crafter_gd_point");
			ZombieWgoData zombieWgoData = MainGame.ZombieSystemData.PutZombieFromStoreToGameSceneAsGardener(interactor.PlayerData, item, interactor.PlayerData.currentGameSceneId, gdpointData.Position, gdpointData.Direction);
			Item zombieItem = zombieWgoData.ZombieItem;
			zombieWgoData.AttachToGardenStationWgoData(this.assignedWgo.Data.UniqueId, zombieItem, null);
			this.SetupZombieVisual(zombieWgoData);
		}
		interactor.PlayerInteractionComponent.ResetInteractionState();
		return true;
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x000CB2CD File Offset: 0x000C94CD
	public override bool HasInteraction(PlayerController interactor)
	{
		return this.assignedWgo.Data.Worker == null && base.HasInsertableZombieOverhead();
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x000CB2EC File Offset: 0x000C94EC
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
		return interactionInfos2;
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x000CB348 File Offset: 0x000C9548
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
			num = 1801;
			goto IL_0098;
		case 1052:
		case 1054:
			num = 1802;
			goto IL_0098;
		case 1058:
			num = 1803;
			goto IL_0098;
		}
		num = 1800;
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

	// Token: 0x04002342 RID: 9026
	private CraftComponent assignedCraftComponent;
}
