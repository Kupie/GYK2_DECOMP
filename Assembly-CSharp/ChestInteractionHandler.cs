using System;
using LazyBearTechnology;

// Token: 0x0200063B RID: 1595
public class ChestInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002A64 RID: 10852 RVA: 0x000C8198 File Offset: 0x000C6398
	public override bool Interact(PlayerController interactor)
	{
		if (base.Interact(interactor))
		{
			return true;
		}
		WorldZoneData currentWorldZoneData = interactor.PlayerData.CurrentWorldZoneData;
		UIBaseChestWindowData uibaseChestWindowData = new UIBaseChestWindowData(interactor.PlayerData.inventory, (currentWorldZoneData == null) ? null : new MultiInventory(currentWorldZoneData, this.assignedWgo.Data, false), this.assignedWgo.Data);
		if (ChestInteractionHandler.IsGardenBagsStorage(this.assignedWgo.Data.id))
		{
			LazyUI.GetWindow<UIConveyorVegetablesChestWindow>().Open(uibaseChestWindowData);
		}
		else if (ChestInteractionHandler.IsWineStorage(this.assignedWgo.Data.id))
		{
			LazyUI.GetWindow<UIConveyorWineChestWindow>().Open(uibaseChestWindowData);
		}
		else if (this.assignedWgo.Data.Definition.conveyorType == ConveyorElementType.Chest || this.assignedWgo.Data.Definition.conveyorType == ConveyorElementType.ChestOut)
		{
			LazyUI.GetWindow<UIConveyorChestWindow>().Open(uibaseChestWindowData);
		}
		else
		{
			LazyUI.GetWindow<UIChestWindow>().Open(uibaseChestWindowData);
		}
		return true;
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000C8282 File Offset: 0x000C6482
	private static bool IsGardenBagsStorage(string wgoId)
	{
		return wgoId == "garden_bags_storage_1" || wgoId == "garden_bags_storage_2" || wgoId == "garden_bags_storage_3";
	}

	// Token: 0x06002A66 RID: 10854 RVA: 0x000C82AB File Offset: 0x000C64AB
	private static bool IsWineStorage(string wgoId)
	{
		return wgoId == "conveyor_wine_beer_pallet";
	}

	// Token: 0x06002A67 RID: 10855 RVA: 0x000C7727 File Offset: 0x000C5927
	public override bool HasInteraction(PlayerController interactor)
	{
		base.HasInteraction(interactor);
		return true;
	}

	// Token: 0x06002A68 RID: 10856 RVA: 0x000C82B8 File Offset: 0x000C64B8
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		string text;
		return new InteractionInfos(new InteractionInfo(base.TryGetCustomInteractionStr(out text) ? text : base.LocalizeHintWithActionIcon("hint_open", GameKey.Interaction)));
	}

	// Token: 0x0400232F RID: 9007
	private const string GARDEN_BAGS_STORAGE1_ID = "garden_bags_storage_1";

	// Token: 0x04002330 RID: 9008
	private const string GARDEN_BAGS_STORAGE2_ID = "garden_bags_storage_2";

	// Token: 0x04002331 RID: 9009
	private const string GARDEN_BAGS_STORAGE3_ID = "garden_bags_storage_3";

	// Token: 0x04002332 RID: 9010
	private const string WINE_STORE_ID = "conveyor_wine_beer_pallet";
}
