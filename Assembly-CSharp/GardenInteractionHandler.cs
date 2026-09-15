using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200064D RID: 1613
public class GardenInteractionHandler : WGOInteractionHandlerBase
{
	// Token: 0x06002ACC RID: 10956 RVA: 0x000CA7B0 File Offset: 0x000C89B0
	public override bool Interact(PlayerController interactor)
	{
		Item interactingItem = MainGame.PlayerData.interactingItem;
		CraftComponent craftComponent = this.assignedWgo.Data.CraftComponent;
		WgoData data = this.assignedWgo.Data;
		if (craftComponent.IsStarted || interactingItem == null || (!interactingItem.IsSeed && !interactingItem.IsFertilizer))
		{
			CraftDef craftDef;
			if (this.assignedWgo.Data.CraftComponent.CurrentCraftElement == null || GameBalance.Me.gardenGrowingCrafts.TryGetValue(this.assignedWgo.Data.CraftComponent.CurrentCraftElement.Def.id, out craftDef))
			{
				LazyWindow<UIGardenBedWindowData> window = LazyUI.GetWindow<UIGardenBedWindow>();
				UIGardenBedWindowData uigardenBedWindowData = new UIGardenBedWindowData(data);
				window.Open(uigardenBedWindowData);
			}
			return true;
		}
		CraftDefBase craftDefBase = GardenInteractionHandler.TryFindGardenCraft(interactingItem, this.assignedWgo.Data, true);
		if (craftDefBase == null)
		{
			return false;
		}
		if (GardenInteractionHandler.HasAssignedGardenOrder(data))
		{
			return false;
		}
		bool flag = false;
		if (data.Worker == null)
		{
			data.TrySetWorker(interactor, null);
			flag = true;
		}
		if (interactor.GetMasteryLevelForTalentBranch("talent_green", null) <= 0)
		{
			Bubble.Talk(new PhraseData(true, null, "gardening_no_mastery", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return false;
		}
		craftComponent.Clear();
		if (interactingItem.IsSeed && GardenInteractionHandler.TryApplySeed(interactingItem, craftDefBase, data))
		{
			this.ClearWorkerAndUpdateVisuals(flag, data);
			return true;
		}
		if (interactingItem.IsFertilizer && GardenInteractionHandler.HasFreeFertilizerPerkSlot(data) && GardenInteractionHandler.TryApplyFertilizer(interactingItem, craftDefBase, data))
		{
			this.ClearWorkerAndUpdateVisuals(flag, data);
			this.TryAssignPerkSlotForNewestAddedPerk();
			return true;
		}
		if (flag)
		{
			data.ClearWorker();
		}
		Debug.Log("Can not start planting craft");
		return false;
	}

	// Token: 0x06002ACD RID: 10957 RVA: 0x000CA930 File Offset: 0x000C8B30
	public override bool HasInteraction(PlayerController interactor)
	{
		if (base.HasInteraction(interactor))
		{
			return true;
		}
		Item interactingItem = MainGame.PlayerData.interactingItem;
		CraftComponent craftComponent = this.assignedWgo.Data.CraftComponent;
		WgoData data = this.assignedWgo.Data;
		if (!craftComponent.IsStarted && interactingItem != null && (interactingItem.IsSeed || interactingItem.IsFertilizer) && !GardenInteractionHandler.HasAssignedGardenOrder(data))
		{
			return GardenInteractionHandler.TryFindGardenCraft(interactingItem, data, true) != null;
		}
		CraftDef craftDef;
		return this.assignedWgo.Data.CraftComponent.CurrentCraftElement == null || GameBalance.Me.gardenGrowingCrafts.TryGetValue(this.assignedWgo.Data.CraftComponent.CurrentCraftElement.Def.id, out craftDef) || ((craftComponent.IsAutoCraftable && craftComponent.IsStarted) || (interactingItem == null && craftComponent.CurrentCraftElement == null)) || (!craftComponent.IsAutoCraftable && craftComponent.IsStarted);
	}

	// Token: 0x06002ACE RID: 10958 RVA: 0x000CAA1C File Offset: 0x000C8C1C
	protected override InteractionInfos FormInteractionInfo()
	{
		InteractionInfos interactionInfos = base.FormInteractionInfo();
		if (!interactionInfos.IsEmpty)
		{
			return interactionInfos;
		}
		CraftComponent craftComponent = this.assignedWgo.Data.CraftComponent;
		InteractionInfos interactionInfos2 = new InteractionInfos();
		Item interactingItem = MainGame.PlayerData.interactingItem;
		string text4;
		if (!craftComponent.IsAutoCraftable && craftComponent.IsStarted)
		{
			interactionInfos2.Add(base.GetInteractionInfoByUsingTool(false));
		}
		else if ((craftComponent.IsAutoCraftable && craftComponent.IsStarted) || (interactingItem == null && craftComponent.CurrentCraftElement == null))
		{
			string text = base.LocalizeHintWithActionIcon("action_inspect", GameKey.Interaction);
			interactionInfos2.Add(new InteractionInfo(text));
		}
		else if (craftComponent.CurrentCraftElement == null && !GardenInteractionHandler.HasAssignedGardenOrder(this.assignedWgo.Data))
		{
			if (interactingItem != null && interactingItem.IsSeed)
			{
				string text2 = base.LocalizeHintWithActionIcon("hint_plant", GameKey.Interaction);
				interactionInfos2.Add(new InteractionInfo(text2));
			}
			else if (interactingItem != null && interactingItem.IsFertilizer && GardenInteractionHandler.HasFreeFertilizerPerkSlot(this.assignedWgo.Data))
			{
				string text3 = base.LocalizeHintWithActionIcon("hint_fertilize", GameKey.Interaction);
				interactionInfos2.Add(new InteractionInfo(text3));
			}
		}
		else if (base.TryGetCustomInteractionStr(out text4))
		{
			interactionInfos2.Add(new InteractionInfo(text4));
		}
		return interactionInfos2;
	}

	// Token: 0x06002ACF RID: 10959 RVA: 0x000CAB5C File Offset: 0x000C8D5C
	public static bool TryApplyFertilizer(Item fertilizer, CraftDefBase cropCraft, WgoData wgoData)
	{
		List<NeedItemData> list = GardenInteractionHandler.FormNeedItems(cropCraft, fertilizer);
		CraftParamsData craftParamsData = new CraftParamsData(cropCraft.id, wgoData, CraftParamsData.CraftParamsType.Common, fertilizer.Definition.talentValue);
		CraftParamsData craftParamsData2 = craftParamsData;
		List<NeedItemData> list2 = list;
		IWorker worker;
		if (wgoData.Worker != null)
		{
			worker = wgoData.Worker;
		}
		else
		{
			IWorker playerController = MainGame.PlayerController;
			worker = playerController;
		}
		craftParamsData2.RecalculateParams(list2, worker);
		CraftElement craftElement = new CraftElement(cropCraft.id, 1, list, craftParamsData);
		if (wgoData.CraftComponent.GetStartCraftStatus(craftElement, null) == CraftStatus.OK)
		{
			wgoData.CraftComponent.ProcessInstantCraft(wgoData, craftElement);
			return true;
		}
		return false;
	}

	// Token: 0x06002AD0 RID: 10960 RVA: 0x000CABD8 File Offset: 0x000C8DD8
	public static bool TryApplySeed(Item seed, CraftDefBase cropCraft, WgoData wgoData)
	{
		List<NeedItemData> list = GardenInteractionHandler.FormNeedItems(cropCraft, seed);
		CraftParamsData craftParamsData = new CraftParamsData(cropCraft.id, wgoData, CraftParamsData.CraftParamsType.GardenPlanting, seed.Definition.talentValue);
		CraftParamsData craftParamsData2 = craftParamsData;
		List<NeedItemData> list2 = list;
		IWorker worker;
		if (wgoData.Worker != null)
		{
			worker = wgoData.Worker;
		}
		else
		{
			IWorker playerController = MainGame.PlayerController;
			worker = playerController;
		}
		craftParamsData2.RecalculateParams(list2, worker);
		CraftElement craftElement = new CraftElement(cropCraft.id, 1, list, craftParamsData);
		if (wgoData.CraftComponent.GetStartCraftStatus(craftElement, null) == CraftStatus.OK)
		{
			wgoData.CraftComponent.TryStartCraft(craftElement);
			wgoData.SetGameRes("seed_mastery_lock", seed.Definition.talentValue);
			return true;
		}
		return false;
	}

	// Token: 0x06002AD1 RID: 10961 RVA: 0x000CAC68 File Offset: 0x000C8E68
	public static CraftDefBase TryFindGardenCraft(Item gardenItem, WgoData wgoData, bool logWarning = true)
	{
		List<CraftDef> list;
		if (!GameBalance.Me.gardenCraftsPerItemCache.TryGetValue(gardenItem.Definition, out list))
		{
			if (logWarning)
			{
				Debug.LogWarning("Can not find garden crafts for item [" + gardenItem.id + "]");
			}
			return null;
		}
		foreach (CraftDefBase craftDefBase in list)
		{
			if (craftDefBase.craftsIn.Contains("garden_empty") && wgoData.id == "garden_empty")
			{
				return craftDefBase;
			}
			if (craftDefBase.craftsIn.Contains("vineyard_empty") && wgoData.id == "vineyard_empty")
			{
				return craftDefBase;
			}
		}
		if (logWarning)
		{
			Debug.LogWarning("Can not find garden craft for item [" + gardenItem.id + "]");
		}
		return null;
	}

	// Token: 0x06002AD2 RID: 10962 RVA: 0x000CAD58 File Offset: 0x000C8F58
	public static List<NeedItemData> FormNeedItems(CraftDefBase craftDef, Item seed)
	{
		return new List<NeedItemData>
		{
			new NeedItemData(seed.id, craftDef.needItems[0].count)
		};
	}

	// Token: 0x06002AD3 RID: 10963 RVA: 0x000CAD84 File Offset: 0x000C8F84
	public static bool HasFreeFertilizerPerkSlot(WgoData wgoData)
	{
		int num = 0;
		using (List<PerkData>.Enumerator enumerator = wgoData.ActivePerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Definition.IsFertilizerPerk)
				{
					num++;
				}
			}
		}
		return num < MainGame.PlayerData.GetResInt("g_garden_fertilizer_slots");
	}

	// Token: 0x06002AD4 RID: 10964 RVA: 0x000CADF4 File Offset: 0x000C8FF4
	public static bool IsSeedableSeed(string wgoId, Item item)
	{
		if (wgoId.StartsWith("garden_"))
		{
			return item.Definition.isSeed && !item.Definition.itemGroupIds.Contains("vineyard_seed");
		}
		return wgoId.StartsWith("vineyard_") && item.Definition.isSeed && item.Definition.itemGroupIds.Contains("vineyard_seed");
	}

	// Token: 0x06002AD5 RID: 10965 RVA: 0x000CAE6C File Offset: 0x000C906C
	private void TryAssignPerkSlotForNewestAddedPerk()
	{
		List<int> list = new List<int> { 1, 2, 3 };
		PerkData perkData = null;
		foreach (PerkData perkData2 in this.assignedWgo.Data.ActivePerks)
		{
			if (perkData2.Definition.IsFertilizerPerk)
			{
				int gameResInt = this.assignedWgo.Data.GetGameResInt("perk_fertilize_" + perkData2.Definition.id);
				if (gameResInt > 0)
				{
					list.Remove(gameResInt);
				}
				else
				{
					perkData = perkData2;
				}
			}
		}
		if (perkData != null && list.Count > 0)
		{
			this.assignedWgo.Data.SetGameRes("perk_fertilize_" + perkData.Definition.id, list[0]);
			return;
		}
		if (perkData == null)
		{
			Debug.LogError("Gardening: Can not assign perk slot for newest added perk: No new perk");
		}
		if (list.Count > 0)
		{
			Debug.LogError("Gardening: Can not assign perk slot for newest added perk: No free slots");
		}
	}

	// Token: 0x06002AD6 RID: 10966 RVA: 0x000CAF80 File Offset: 0x000C9180
	private void ClearWorkerAndUpdateVisuals(bool wasPlayerSetAsWorker, WgoData wgoData)
	{
		if (wasPlayerSetAsWorker)
		{
			wgoData.ClearWorker();
		}
		MainGame.PlayerData.UpdateInteractingItem();
		GardenInteractionHandler.PlayPlantingFeedback();
	}

	// Token: 0x06002AD7 RID: 10967 RVA: 0x000CAF9C File Offset: 0x000C919C
	public static void PlayPlantingFeedback()
	{
		MainGame.PlayerController.View.PlayerAnimation.SetState(global::AnimationState.Planting);
		WorldFX.Spawn(MainGame.PlayerData.position.Value, "planting", null, default(Vector3));
		LazyAudio.Play("planting");
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x000C9A01 File Offset: 0x000C7C01
	private bool HasInteraction(CustomInteraction customInteraction)
	{
		return customInteraction.IsInteractable(this.assignedWgo.Data);
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x000CAFEE File Offset: 0x000C91EE
	public static bool HasAssignedGardenOrder(WgoData wgoData)
	{
		return wgoData.WorldZoneData != null && wgoData.WorldZoneData.HasAssignedGardenOrder(wgoData.UniqueId);
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x000CB00C File Offset: 0x000C920C
	public static bool TryPlacePlantOrder(WgoData wgoData)
	{
		using (IEnumerator<SGuid> enumerator = wgoData.AttachedWorkbenchExtensions.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				SGuid sguid = enumerator.Current;
				WgoData wgoData2 = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
				string text;
				if (!GardenTabletWorldIconLogic.IsGardenTablet(wgoData2) || !GardenTabletWorldIconLogic.TryGetCropIdFromTabletWgoId(wgoData2.id, out text))
				{
					return false;
				}
				string text2 = text + "_seed";
				string text3 = text2;
				List<ItemDef> list;
				if (GameBalance.Me.starGroupItemsCache.TryGetValue(text2, out list))
				{
					text3 = list[0].id;
				}
				CraftDefBase craftDefBase = GardenInteractionHandler.TryFindGardenCraft(new Item(text3, 1), wgoData, true);
				if (craftDefBase == null)
				{
					return false;
				}
				if (wgoData.WorldZoneData.FindOrdersByTarget(wgoData.UniqueId, typeof(PlantOrder)).Count > 0)
				{
					return false;
				}
				Debug.Log("Placed PLANT garden order for seed [" + text2 + "]");
				wgoData.WorldZoneData.PlaceNewOrder(new PlantOrder(wgoData.UniqueId, new Item(text2, craftDefBase.needItems[0].GetCount(wgoData)), text3 != text2));
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x000CB16C File Offset: 0x000C936C
	public static bool TryPlaceGatherOrder(WgoData wgoData)
	{
		using (IEnumerator<SGuid> enumerator = wgoData.AttachedWorkbenchExtensions.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				SGuid sguid = enumerator.Current;
				if (!GardenTabletWorldIconLogic.IsGardenTablet(MainGame.Instance.GameSave.WorldData.GetWgoData(sguid)))
				{
					return false;
				}
				if (wgoData.WorldZoneData.FindOrdersByTarget(wgoData.UniqueId, typeof(GatherOrder)).Count > 0)
				{
					return false;
				}
				Debug.Log("Placed GATHER garden order");
				wgoData.WorldZoneData.PlaceNewOrder(new GatherOrder(wgoData.UniqueId, Item.Empty));
				return true;
			}
		}
		return false;
	}
}
