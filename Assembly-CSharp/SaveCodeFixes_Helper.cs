using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public static class SaveCodeFixes_Helper
{
	// Token: 0x06001E47 RID: 7751 RVA: 0x0008E568 File Offset: 0x0008C768
	public static List<WgoData> GetSignboardsWithSpawnedTent(GameSave gameSave)
	{
		List<WgoData> list = new List<WgoData>();
		List<GameSceneData> list2;
		if (gameSave == null)
		{
			list2 = null;
		}
		else
		{
			WorldData worldData = gameSave.worldData;
			list2 = ((worldData != null) ? worldData.gameSceneDataList : null);
		}
		List<GameSceneData> list3 = list2;
		if (list3 == null)
		{
			return list;
		}
		List<WgoData> list4 = new List<WgoData>();
		List<WgoData> list5 = new List<WgoData>();
		for (int i = 0; i < list3.Count; i++)
		{
			GameSceneData gameSceneData = list3[i];
			List<WgoData> list6 = ((gameSceneData != null) ? gameSceneData.wgoDataList : null);
			if (list6 != null)
			{
				for (int j = 0; j < list6.Count; j++)
				{
					WgoData wgoData = list6[j];
					if (wgoData != null)
					{
						if (SaveCodeFixes_Helper.IsSignboard(wgoData))
						{
							list4.Add(wgoData);
						}
						else if (SaveCodeFixes_Helper.IsTent(wgoData))
						{
							list5.Add(wgoData);
						}
					}
				}
			}
		}
		if (list5.Count == 0)
		{
			return list;
		}
		for (int k = 0; k < list4.Count; k++)
		{
			if (SaveCodeFixes_Helper.HasTentAtConfigurationCoordinates(list4[k], list5))
			{
				list.Add(list4[k]);
			}
		}
		return list;
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x0008E65C File Offset: 0x0008C85C
	public static List<string> GetSignboardsPostfixes(List<WgoData> signboards)
	{
		List<string> list = new List<string>();
		string text = "t_b_signboard_";
		for (int i = 0; i < signboards.Count; i++)
		{
			WgoData wgoData = signboards[i];
			if (wgoData != null && SaveCodeFixes_Helper.HasCustomTagPrefix(wgoData, text))
			{
				list.Add(wgoData.CustomTag.Replace(text, ""));
			}
		}
		return list;
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x0008E6B2 File Offset: 0x0008C8B2
	private static bool IsSignboard(WgoData wgo)
	{
		return SaveCodeFixes_Helper.HasCustomTagPrefix(wgo, "t_b_signboard_") || (GameBalance.Me != null && wgo.Definition != null && wgo.Definition.interactionType == WGODef.InteractionType.TownBuildingPlace);
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x0008E6E9 File Offset: 0x0008C8E9
	private static bool IsTent(WgoData wgo)
	{
		return SaveCodeFixes_Helper.HasCustomTagPrefix(wgo, "t_b_tent_");
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x0008E6F6 File Offset: 0x0008C8F6
	private static bool HasCustomTagPrefix(WgoData wgo, string prefix)
	{
		return !string.IsNullOrEmpty(wgo.CustomTag) && wgo.CustomTag.StartsWith(prefix);
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x0008E714 File Offset: 0x0008C914
	private static bool HasTentAtConfigurationCoordinates(WgoData signboard, List<WgoData> tents)
	{
		TownBuildingWgoComponent townBuildingWgoComponent = signboard.TownBuildingWgoComponent;
		List<TownBuildingTierSceneConfiguration> list;
		if (townBuildingWgoComponent == null)
		{
			list = null;
		}
		else
		{
			TownBuildingSceneConfiguration sceneConfiguration = townBuildingWgoComponent.SceneConfiguration;
			list = ((sceneConfiguration != null) ? sceneConfiguration.tierDataList : null);
		}
		List<TownBuildingTierSceneConfiguration> list2 = list;
		if (list2 == null)
		{
			return false;
		}
		for (int i = 0; i < list2.Count; i++)
		{
			if (SaveCodeFixes_Helper.HasTentAtConfigObject(tents, signboard.WorldId, list2[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x0008E770 File Offset: 0x0008C970
	private static bool HasTentAtConfigObject(List<WgoData> tents, string worldId, TownBuildingTierSceneConfiguration tier)
	{
		return tier != null && (SaveCodeFixes_Helper.HasTentAtConfigPosition(tents, worldId, tier.tent) || SaveCodeFixes_Helper.HasTentAtConfigPosition(tents, worldId, tier.sign) || SaveCodeFixes_Helper.HasTentAtConfigPosition(tents, worldId, tier.yard) || SaveCodeFixes_Helper.HasTentAtConfigPosition(tents, worldId, tier.decor1) || SaveCodeFixes_Helper.HasTentAtConfigPosition(tents, worldId, tier.decor2) || SaveCodeFixes_Helper.HasTentAtConfigPosition(tents, worldId, tier.decor3));
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x0008E7DC File Offset: 0x0008C9DC
	private static bool HasTentAtConfigPosition(List<WgoData> tents, string worldId, TownBuildingObjectConfiguration config)
	{
		if (config == null || string.IsNullOrEmpty(config.wgoId))
		{
			return false;
		}
		for (int i = 0; i < tents.Count; i++)
		{
			WgoData wgoData = tents[i];
			if (SaveCodeFixes_Helper.IsSameWorld(worldId, wgoData.WorldId) && (wgoData.Position - config.position).sqrMagnitude <= 0.0001f)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x0008E845 File Offset: 0x0008CA45
	private static bool IsSameWorld(string worldIdA, string worldIdB)
	{
		return string.IsNullOrEmpty(worldIdA) || string.IsNullOrEmpty(worldIdB) || worldIdA == worldIdB;
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x0008E860 File Offset: 0x0008CA60
	public static void UnlockOrHideTechByQuest(SaveFixContext ctx, string techId, string unlockQuestId)
	{
		TechDef dataOrNull = GameBalance.Me.GetDataOrNull<TechDef>(techId);
		if (dataOrNull == null)
		{
			ctx.LogError("Missing TechDef [" + techId + "]");
			return;
		}
		if (ctx.GameSave.questSystemData.IsQuestInStatus(unlockQuestId, QuestStatus.Completed))
		{
			if (!ctx.GameSave.knowledgeSystem.IsTechUnlocked(dataOrNull.id))
			{
				dataOrNull.Unlock(true);
			}
			return;
		}
		ctx.GameSave.knowledgeSystem.HideTech(dataOrNull.id);
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x0008E8E0 File Offset: 0x0008CAE0
	public static void CopyUnlockedTutorialsToViewed(KnowledgeSystem knowledge)
	{
		if (((knowledge != null) ? knowledge.unlockedTutorials : null) == null)
		{
			return;
		}
		for (int i = 0; i < knowledge.unlockedTutorials.Count; i++)
		{
			knowledge.AddViewedTutorial(knowledge.unlockedTutorials[i]);
		}
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x0008E924 File Offset: 0x0008CB24
	public static void ResetNpcLifeSimulator(GameSave save)
	{
		save.npcLifeSimulatorData.ClearData();
		foreach (NPCGroupPointOfInterestConfiguration npcgroupPointOfInterestConfiguration in LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.AllGroups)
		{
			save.npcLifeSimulatorData.AddGroup(new NPCGroupPointOfInterestData(npcgroupPointOfInterestConfiguration));
		}
		foreach (NPCPointOfInterestConfiguration npcpointOfInterestConfiguration in LazySingletonSO<NPCLifeSimulatorConfiguration>.Instance.AllPonts)
		{
			save.npcLifeSimulatorData.AddPoint(new NPCPointOfInterestData(npcpointOfInterestConfiguration));
		}
		save.npcLifeSimulatorData.PrepareForGame();
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x0008E9EC File Offset: 0x0008CBEC
	public static bool WasQuestStarted(QuestSystemData quests, string questId)
	{
		return quests.IsQuestInStatus(questId, QuestStatus.InProgress) || quests.IsQuestInStatus(questId, QuestStatus.Completed);
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x0008EA02 File Offset: 0x0008CC02
	public static void GiveItemIfQuestCompleted(SaveFixContext ctx, QuestSystemData quests, string questId, string itemId, int count)
	{
		if (quests.IsQuestInStatus(questId, QuestStatus.Completed))
		{
			SaveCodeFixes_Helper.GiveItem(ctx, itemId, count);
		}
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x0008EA18 File Offset: 0x0008CC18
	public static void GiveItem(SaveFixContext ctx, string itemId, int count)
	{
		if (count <= 0)
		{
			return;
		}
		PlayerData playerData = ctx.GameSave.playerData;
		Inventory inventory = playerData.Inventory;
		int num = inventory.Data.CanAddItemCountToInventory(new Item(itemId, count), true, null, false);
		int num2 = count - num;
		if (num > 0)
		{
			SaveCodeFixes_Helper.AddItemsToInventory(inventory, itemId, num);
			ctx.Log(string.Format("Gave [{0} x{1}]", itemId, num));
		}
		if (num2 <= 0)
		{
			return;
		}
		MainGame.Instance.dropSystem.DropItem(new Item(itemId, num2), playerData.currentGameSceneId, playerData.position.Value + new Vector3(playerData.Direction.x, 0f, playerData.Direction.y), null);
		ctx.Log(string.Format("Dropped leftover [{0} x{1}] (inventory full)", itemId, num2));
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x0008EAE4 File Offset: 0x0008CCE4
	private static void AddItemsToInventory(Inventory inventory, string itemId, int count)
	{
		ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(itemId);
		if (dataOrNull != null && dataOrNull.stackCount == 1)
		{
			for (int i = 0; i < count; i++)
			{
				inventory.AddItemToInventory(new Item(itemId, 1), null, false);
			}
			return;
		}
		inventory.AddItemToInventory(new Item(itemId, count), null, false);
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x0008EB38 File Offset: 0x0008CD38
	public static void HideInspiration(KnowledgeSystem knowledge, string inspirationId)
	{
		if (knowledge.hiddenInspirations == null)
		{
			knowledge.hiddenInspirations = new List<string>();
		}
		if (!knowledge.hiddenInspirations.Contains(inspirationId))
		{
			knowledge.hiddenInspirations.Add(inspirationId);
		}
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x0008EB74 File Offset: 0x0008CD74
	public static void UnlockOrHideInspirationByQuest(SaveFixContext ctx, KnowledgeSystem knowledge, QuestSystemData quests, string inspirationId, string unlockQuestId)
	{
		if (!quests.IsQuestInStatus(unlockQuestId, QuestStatus.Completed))
		{
			SaveCodeFixes_Helper.HideInspiration(knowledge, inspirationId);
			return;
		}
		if (!knowledge.IsInspirationHidden(inspirationId))
		{
			return;
		}
		knowledge.RevealInspiration(inspirationId);
		ctx.Log(string.Concat(new string[] { "Revealed inspiration [", inspirationId, "] from completed quest [", unlockQuestId, "]" }));
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x0008EBD7 File Offset: 0x0008CDD7
	public static void AddInspirationIfQuestCompleted(GameSave save, QuestSystemData quests, string questId, string inspirationId, int value)
	{
		if (quests.IsQuestInStatus(questId, QuestStatus.Completed))
		{
			save.talentSystemData.AddToInspiration(inspirationId, value);
		}
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x0008EBF4 File Offset: 0x0008CDF4
	public static void UnlockTechIfQuestCompleted(SaveFixContext ctx, QuestSystemData quests, KnowledgeSystem knowledge, string questId, string techId)
	{
		if (!quests.IsQuestInStatus(questId, QuestStatus.Completed) || knowledge.IsTechUnlocked(techId))
		{
			return;
		}
		TechDef dataOrNull = GameBalance.Me.GetDataOrNull<TechDef>(techId);
		if (dataOrNull == null)
		{
			ctx.LogError("Missing TechDef [" + techId + "]");
			return;
		}
		dataOrNull.Unlock(true);
		ctx.Log(string.Concat(new string[] { "Unlocked tech [", techId, "] from completed quest [", questId, "]" }));
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x0008EC78 File Offset: 0x0008CE78
	public static void RevealTechsFromCompletedQuests(SaveFixContext ctx)
	{
		KnowledgeSystem knowledgeSystem;
		if (ctx == null)
		{
			knowledgeSystem = null;
		}
		else
		{
			GameSave gameSave = ctx.GameSave;
			knowledgeSystem = ((gameSave != null) ? gameSave.knowledgeSystem : null);
		}
		KnowledgeSystem knowledgeSystem2 = knowledgeSystem;
		if (knowledgeSystem2 == null)
		{
			return;
		}
		List<string> hiddenTechs = knowledgeSystem2.hiddenTechs;
		int num = ((hiddenTechs != null) ? hiddenTechs.Count : 0);
		knowledgeSystem2.RevealTechsFromCompletedQuests();
		List<string> hiddenTechs2 = knowledgeSystem2.hiddenTechs;
		int num2 = num - ((hiddenTechs2 != null) ? hiddenTechs2.Count : 0);
		if (num2 > 0)
		{
			ctx.Log(string.Format("Revealed {0} tech(s) from completed quests", num2));
		}
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x0008ECE8 File Offset: 0x0008CEE8
	public static void RevealHiddenTechsIfParentRevealed(SaveFixContext ctx)
	{
		KnowledgeSystem knowledgeSystem;
		if (ctx == null)
		{
			knowledgeSystem = null;
		}
		else
		{
			GameSave gameSave = ctx.GameSave;
			knowledgeSystem = ((gameSave != null) ? gameSave.knowledgeSystem : null);
		}
		KnowledgeSystem knowledgeSystem2 = knowledgeSystem;
		if (knowledgeSystem2 == null)
		{
			return;
		}
		List<string> hiddenTechs = knowledgeSystem2.hiddenTechs;
		int num = ((hiddenTechs != null) ? hiddenTechs.Count : 0);
		knowledgeSystem2.CatchUpRevealedTechsFromTree();
		List<string> hiddenTechs2 = knowledgeSystem2.hiddenTechs;
		int num2 = num - ((hiddenTechs2 != null) ? hiddenTechs2.Count : 0);
		if (num2 > 0)
		{
			ctx.Log(string.Format("Revealed {0} tech(s) whose parent is already visible in the tech tree", num2));
		}
	}

	// Token: 0x06001E5D RID: 7773 RVA: 0x0008ED58 File Offset: 0x0008CF58
	public static void RevokeUnlockedCraft(SaveFixContext ctx, KnowledgeSystem knowledge, string craftId)
	{
		if (((knowledge != null) ? knowledge.unlockedCrafts : null) == null || !knowledge.unlockedCrafts.Contains(craftId))
		{
			return;
		}
		knowledge.RemoveUnlockedCraft(craftId);
		ctx.Log("Revoked unlocked craft [" + craftId + "]");
	}

	// Token: 0x06001E5E RID: 7774 RVA: 0x0008ED94 File Offset: 0x0008CF94
	public static void ResyncQuestVisualisation(SaveFixContext ctx, QuestSystemData quests, string questId)
	{
		QuestDef dataOrNull = GameBalance.Me.GetDataOrNull<QuestDef>(questId);
		if (dataOrNull == null)
		{
			ctx.LogError("Missing QuestDef [" + questId + "]");
			return;
		}
		bool flag = quests.IsQuestInStatus(questId, QuestStatus.InProgress) || quests.IsQuestInStatus(questId, QuestStatus.Completed);
		bool flag2 = (!dataOrNull.hasPosInBalance || !flag) && dataOrNull.isHidden;
		quests.ChangeQuestHiddenState(questId, flag2);
		quests.ChangeQuestUnknownState(questId, dataOrNull.isUnknown);
		ctx.Log(string.Format("Resynced visualisation of quest [{0}]: hidden={1}, unknown={2}", questId, flag2, dataOrNull.isUnknown));
	}

	// Token: 0x06001E5F RID: 7775 RVA: 0x0008EE28 File Offset: 0x0008D028
	public static void CompleteVisualQuestIfQuestCompleted(SaveFixContext ctx, QuestSystemData quests, string questId, string visualQuestId)
	{
		if (!quests.IsQuestInStatus(questId, QuestStatus.Completed))
		{
			return;
		}
		if (quests.IsQuestInStatus(visualQuestId, QuestStatus.Completed))
		{
			return;
		}
		QuestDef dataOrNull = GameBalance.Me.GetDataOrNull<QuestDef>(visualQuestId);
		if (dataOrNull == null)
		{
			ctx.LogError("Missing QuestDef [" + visualQuestId + "]");
			return;
		}
		quests.CompleteQuest(visualQuestId, 0f);
		if (dataOrNull.hasPosInBalance)
		{
			quests.ChangeQuestHiddenState(visualQuestId, false);
		}
		ctx.Log(string.Concat(new string[] { "Completed visual quest [", visualQuestId, "] because [", questId, "] is completed" }));
	}

	// Token: 0x06001E60 RID: 7776 RVA: 0x0008EEC0 File Offset: 0x0008D0C0
	public static void HideTech(SaveFixContext ctx, KnowledgeSystem knowledge, string techId)
	{
		if (GameBalance.Me.GetDataOrNull<TechDef>(techId) == null)
		{
			ctx.LogError("Missing TechDef [" + techId + "]");
			return;
		}
		knowledge.HideTech(techId);
		ctx.Log("Hid tech [" + techId + "]");
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x0008EF10 File Offset: 0x0008D110
	public static void RevealHiddenTechsFromWorldObjects(SaveFixContext ctx)
	{
		GameSave gameSave = ((ctx != null) ? ctx.GameSave : null);
		KnowledgeSystem knowledgeSystem = ((gameSave != null) ? gameSave.knowledgeSystem : null);
		if (knowledgeSystem == null)
		{
			return;
		}
		SaveCodeFixes_Helper.RevealTechIfAnyWgoPresent(ctx, knowledgeSystem, gameSave, "autopsy_resurection", new string[] { "power_switch_table" });
		SaveCodeFixes_Helper.RevealTechIfAnyWgoPresent(ctx, knowledgeSystem, gameSave, "kitchen_table_2", new string[] { "kitchen_table", "kitchen_table_t2" });
		SaveCodeFixes_Helper.RevealTechIfAnyWgoPresent(ctx, knowledgeSystem, gameSave, "kitchen_oven_2", new string[] { "kitchen_oven", "kitchen_oven_t2" });
		bool flag = gameSave.playerData != null && gameSave.playerData.GetResInt("g_garden_farming_base") >= 4;
		if (knowledgeSystem.IsTechHidden("garden_improve_2") && (flag || SaveCodeFixes_Helper.HasAnyWgoWithId(gameSave, new string[] { "garden_t1", "garden_t2" })))
		{
			knowledgeSystem.RevealTech("garden_improve_2");
			ctx.Log("Revealed tech [garden_improve_2]: garden farming base was upgraded");
		}
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x0008F004 File Offset: 0x0008D204
	private static void RevealTechIfAnyWgoPresent(SaveFixContext ctx, KnowledgeSystem knowledge, GameSave save, string techId, params string[] wgoIds)
	{
		if (!knowledge.IsTechHidden(techId) || !SaveCodeFixes_Helper.HasAnyWgoWithId(save, wgoIds))
		{
			return;
		}
		knowledge.RevealTech(techId);
		ctx.Log(string.Concat(new string[]
		{
			"Revealed tech [",
			techId,
			"]: world object [",
			wgoIds[0],
			"] is present"
		}));
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x0008F060 File Offset: 0x0008D260
	private static bool HasAnyWgoWithId(GameSave save, params string[] wgoIds)
	{
		List<GameSceneData> list;
		if (save == null)
		{
			list = null;
		}
		else
		{
			WorldData worldData = save.worldData;
			list = ((worldData != null) ? worldData.gameSceneDataList : null);
		}
		List<GameSceneData> list2 = list;
		if (list2 == null || wgoIds == null || wgoIds.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < list2.Count; i++)
		{
			GameSceneData gameSceneData = list2[i];
			List<WgoData> list3 = ((gameSceneData != null) ? gameSceneData.wgoDataList : null);
			if (list3 != null)
			{
				for (int j = 0; j < list3.Count; j++)
				{
					WgoData wgoData = list3[j];
					if (wgoData != null && !string.IsNullOrEmpty(wgoData.id))
					{
						for (int k = 0; k < wgoIds.Length; k++)
						{
							if (wgoData.id == wgoIds[k])
							{
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x0008F10D File Offset: 0x0008D30D
	public static void UnlockBuildingIfTechUnlocked(KnowledgeSystem knowledge, string techId, string buildingId)
	{
		if (knowledge == null || !knowledge.IsTechUnlocked(techId))
		{
			return;
		}
		knowledge.UnlockBuilding(buildingId);
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x0008F124 File Offset: 0x0008D324
	public static void CatchUpVendorTiers(SaveFixContext ctx, VendorSystem vendorSystem, QuestSystemData quests, string vendorId, params string[] completedQuestIds)
	{
		Vendor vendor = vendorSystem.GetVendor(vendorId);
		if (((vendor != null) ? vendor.Definition : null) == null)
		{
			ctx.LogWarning("Vendor [" + vendorId + "] not found, skip tier catch-up");
			return;
		}
		int num = 0;
		for (int i = 0; i < completedQuestIds.Length; i++)
		{
			if (quests.IsQuestInStatus(completedQuestIds[i], QuestStatus.Completed))
			{
				num++;
			}
		}
		int num2 = vendor.Definition.startTier + num;
		int num3 = 0;
		while (vendor.CurTier < num2)
		{
			int curTier = vendor.CurTier;
			vendor.ForceLevelUp();
			if (vendor.CurTier <= curTier)
			{
				break;
			}
			num3++;
		}
		if (num3 > 0)
		{
			ctx.Log(string.Format("Leveled vendor [{0}] +{1} to tier {2}", vendorId, num3, vendor.CurTier));
		}
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x0008F1E4 File Offset: 0x0008D3E4
	public static void AddMissingVendorOrders(SaveFixContext ctx, VendorSystem vendorSystem, string vendorId)
	{
		Vendor vendor = ((vendorSystem != null) ? vendorSystem.GetVendor(vendorId) : null);
		bool flag;
		if (vendor == null)
		{
			flag = null != null;
		}
		else
		{
			VendorDef definition = vendor.Definition;
			flag = ((definition != null) ? definition.tierDataList : null) != null;
		}
		if (!flag)
		{
			ctx.LogWarning("Vendor [" + vendorId + "] not found, skip missing-order catch-up");
			return;
		}
		int num = 0;
		List<VendorTierData> tierDataList = vendor.Definition.tierDataList;
		for (int i = 0; i < tierDataList.Count; i++)
		{
			VendorTierData vendorTierData = tierDataList[i];
			List<string> list = ((vendorTierData != null) ? vendorTierData.orders : null);
			if (list != null)
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (vendor.TryAddMissingOrder(list[j], i + 1))
					{
						num++;
					}
				}
			}
		}
		if (num > 0)
		{
			ctx.Log(string.Format("Added {0} missing order(s) to vendor [{1}]", num, vendorId));
		}
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x0008F2B1 File Offset: 0x0008D4B1
	public static void StartQuestIfAvailable(SaveFixContext ctx, QuestSystemData quests, string questId)
	{
		if (quests == null || string.IsNullOrEmpty(questId))
		{
			return;
		}
		if (!quests.IsQuestInStatus(questId, QuestStatus.Available))
		{
			return;
		}
		quests.StartQuest(questId, 0f);
		ctx.Log("Started quest [" + questId + "]");
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x0008F2EC File Offset: 0x0008D4EC
	public static void CatchUpDarkFinalBattleApproach(SaveFixContext ctx, GameSave save, QuestSystemData quests)
	{
		string[] array = new string[] { "157_base_palace_home", "158_astrologer_palace_help_dark", "159_crossroad_palace_help_dark", "160_linda_palace_help_dark" };
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (quests.IsQuestInStatus(array[i], QuestStatus.Completed))
			{
				num++;
			}
		}
		int resInt = save.playerData.GetResInt("dark_final_battle");
		if (num > resInt)
		{
			save.playerData.SetRes("dark_final_battle", (float)num);
			ctx.Log(string.Format("Set dark_final_battle {0} -> {1}", resInt, num));
		}
		if (num >= 4 || (quests.IsQuestInStatus("151_base_90yes_god", QuestStatus.Completed) && quests.IsQuestInStatus("157_base_palace_home", QuestStatus.Completed)))
		{
			SaveCodeFixes_Helper.StartQuestIfAvailable(ctx, quests, "126_pre-C1-fights_approach");
		}
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x0008F3B8 File Offset: 0x0008D5B8
	public static void ForceHideTalentLevelUp(KnowledgeSystem knowledge, string talentLevelUpId)
	{
		List<string> revealedTalentLevelUps = knowledge.revealedTalentLevelUps;
		if (revealedTalentLevelUps != null)
		{
			revealedTalentLevelUps.Remove(talentLevelUpId);
		}
		if (knowledge.hiddenTalentLevelUps == null)
		{
			knowledge.hiddenTalentLevelUps = new List<string>();
		}
		if (!knowledge.hiddenTalentLevelUps.Contains(talentLevelUpId))
		{
			knowledge.hiddenTalentLevelUps.Add(talentLevelUpId);
		}
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x0008F408 File Offset: 0x0008D608
	public static void MigrateDelayedSpawnWgoUniqueIds(SaveFixContext ctx)
	{
		GameSave gameSave = ((ctx != null) ? ctx.GameSave : null);
		List<SpawnDelayedObject> list;
		if (gameSave == null)
		{
			list = null;
		}
		else
		{
			WgoDelayedSpawnSystemData wgoDelayedSpawnSystemData = gameSave.wgoDelayedSpawnSystemData;
			list = ((wgoDelayedSpawnSystemData != null) ? wgoDelayedSpawnSystemData.spawnDelayedObjects : null);
		}
		List<SpawnDelayedObject> list2 = list;
		if (list2 == null)
		{
			return;
		}
		int num = 0;
		for (int i = list2.Count - 1; i >= 0; i--)
		{
			SpawnDelayedObject spawnDelayedObject = list2[i];
			if (spawnDelayedObject == null)
			{
				list2.RemoveAt(i);
			}
			else
			{
				if (SGuid.IsNullOrEmpty(spawnDelayedObject.wgoUniqueId) && spawnDelayedObject.wgoData != null)
				{
					spawnDelayedObject.wgoUniqueId = SGuid.Empty;
					spawnDelayedObject.wgoUniqueId.SetGuid(spawnDelayedObject.wgoData.UniqueId);
					num++;
				}
				if (SGuid.IsNullOrEmpty(spawnDelayedObject.wgoUniqueId) || !ctx.HasWgo(spawnDelayedObject.wgoUniqueId))
				{
					ctx.Log(string.Format("Dropped delayed spawn, WGO [{0}] not found in save", spawnDelayedObject.wgoUniqueId));
					list2.RemoveAt(i);
				}
				else
				{
					spawnDelayedObject.wgoData = null;
				}
			}
		}
		if (num > 0)
		{
			ctx.Log(string.Format("Migrated {0} delayed spawn SGuid(s)", num));
		}
	}

	// Token: 0x06001E6B RID: 7787 RVA: 0x0008F504 File Offset: 0x0008D704
	public static void RemoveDuplicateFloraAtSameCoordinates(SaveFixContext ctx)
	{
		if (((ctx != null) ? ctx.GameSave : null) == null)
		{
			return;
		}
		if (GameBalance.Me == null)
		{
			ctx.LogError("RemoveDuplicateFloraAtSameCoordinates: GameBalance is not loaded");
			return;
		}
		WorldData worldData = ctx.GameSave.worldData;
		List<GameSceneData> list = ((worldData != null) ? worldData.gameSceneDataList : null);
		if (list == null)
		{
			return;
		}
		int num = 0;
		List<WgoData> list2 = new List<WgoData>();
		for (int i = 0; i < list.Count; i++)
		{
			GameSceneData gameSceneData = list[i];
			List<WgoData> list3 = ((gameSceneData != null) ? gameSceneData.wgoDataList : null);
			if (list3 != null)
			{
				list2.Clear();
				for (int j = 0; j < list3.Count; j++)
				{
					WgoData wgoData = list3[j];
					SaveCodeFixes_Helper.FloraKind floraKind;
					if (SaveCodeFixes_Helper.TryGetFloraKind(wgoData, out floraKind))
					{
						list2.Add(wgoData);
					}
				}
				if (list2.Count >= 2)
				{
					num += SaveCodeFixes_Helper.RemoveDuplicateFloraClusters(ctx, list2);
				}
			}
		}
		if (num > 0)
		{
			ctx.Log(string.Format("Removed {0} stacked flora WGO(s) at duplicate coordinates", num));
		}
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x0008F5F0 File Offset: 0x0008D7F0
	private static int RemoveDuplicateFloraClusters(SaveFixContext ctx, List<WgoData> flora)
	{
		int num = 0;
		bool[] array = new bool[flora.Count];
		List<WgoData> list = new List<WgoData>();
		for (int i = 0; i < flora.Count; i++)
		{
			if (!array[i])
			{
				list.Clear();
				list.Add(flora[i]);
				array[i] = true;
				for (int j = 0; j < list.Count; j++)
				{
					for (int k = i + 1; k < flora.Count; k++)
					{
						if (!array[k] && SaveCodeFixes_Helper.DistanceXzSq(list[j].Position, flora[k].Position) <= 0.0001f)
						{
							array[k] = true;
							list.Add(flora[k]);
						}
					}
				}
				if (list.Count >= 2)
				{
					WgoData wgoData = SaveCodeFixes_Helper.PickFloraKeeper(list);
					if (wgoData != null)
					{
						for (int l = 0; l < list.Count; l++)
						{
							WgoData wgoData2 = list[l];
							if (wgoData2 != wgoData && SaveCodeFixes_Helper.RemoveFloraWgo(ctx, wgoData2))
							{
								num++;
							}
						}
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x0008F6FC File Offset: 0x0008D8FC
	private static WgoData PickFloraKeeper(List<WgoData> cluster)
	{
		WgoData wgoData = null;
		WgoData wgoData2 = null;
		WgoData wgoData3 = null;
		WgoData wgoData4 = null;
		WgoData wgoData5 = null;
		for (int i = 0; i < cluster.Count; i++)
		{
			WgoData wgoData6 = cluster[i];
			SaveCodeFixes_Helper.FloraKind floraKind;
			if (SaveCodeFixes_Helper.TryGetFloraKind(wgoData6, out floraKind))
			{
				switch (floraKind)
				{
				case SaveCodeFixes_Helper.FloraKind.Spawner:
					if (wgoData == null)
					{
						wgoData = wgoData6;
					}
					break;
				case SaveCodeFixes_Helper.FloraKind.Stump:
					if (wgoData2 == null)
					{
						wgoData2 = wgoData6;
					}
					break;
				case SaveCodeFixes_Helper.FloraKind.Tree:
					if (wgoData3 == null)
					{
						wgoData3 = wgoData6;
					}
					if (wgoData4 == null && !SaveCodeFixes_Helper.IsOnetimeId(wgoData6.id))
					{
						wgoData4 = wgoData6;
					}
					break;
				case SaveCodeFixes_Helper.FloraKind.Bush:
					if (wgoData5 == null)
					{
						wgoData5 = wgoData6;
					}
					break;
				}
			}
		}
		if (wgoData != null)
		{
			return wgoData;
		}
		if (wgoData2 != null)
		{
			return wgoData2;
		}
		if (wgoData4 != null)
		{
			return wgoData4;
		}
		if (wgoData3 != null)
		{
			return wgoData3;
		}
		return wgoData5;
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x0008F7A4 File Offset: 0x0008D9A4
	private static bool RemoveFloraWgo(SaveFixContext ctx, WgoData wgo)
	{
		if (wgo == null || SGuid.IsNullOrEmpty(wgo.UniqueId))
		{
			return false;
		}
		SGuid uniqueId = wgo.UniqueId;
		string id = wgo.id;
		if (!ctx.RemoveWgoData(uniqueId))
		{
			ctx.LogWarning(string.Format("RemoveDuplicateFloraAtSameCoordinates: failed to remove [{0}] [{1}]", id, uniqueId));
			return false;
		}
		SaveCodeFixes_Helper.RemoveDelayedSpawnReferences(ctx, uniqueId);
		SaveCodeFixes_Helper.RemoveDelayedEventReferences(ctx, uniqueId);
		SaveCodeFixes_Helper.RemoveZombieOnSceneReferences(ctx, uniqueId);
		ctx.WarnAboutDanglingReferences(uniqueId, id);
		return true;
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x0008F80C File Offset: 0x0008DA0C
	private static bool TryGetFloraKind(WgoData wgo, out SaveCodeFixes_Helper.FloraKind kind)
	{
		kind = SaveCodeFixes_Helper.FloraKind.Spawner;
		if (wgo == null || string.IsNullOrEmpty(wgo.id))
		{
			return false;
		}
		if (SaveCodeFixes_Helper.IsInWgoGroup(wgo, "spawner"))
		{
			kind = SaveCodeFixes_Helper.FloraKind.Spawner;
			return true;
		}
		if (SaveCodeFixes_Helper.IsInWgoGroup(wgo, "stumps"))
		{
			kind = SaveCodeFixes_Helper.FloraKind.Stump;
			return true;
		}
		if (SaveCodeFixes_Helper.IsInWgoGroup(wgo, "trees"))
		{
			kind = SaveCodeFixes_Helper.FloraKind.Tree;
			return true;
		}
		if (SaveCodeFixes_Helper.IsInWgoGroup(wgo, "bushes") || SaveCodeFixes_Helper.IsInWgoGroup(wgo, "collectable_bushes"))
		{
			kind = SaveCodeFixes_Helper.FloraKind.Bush;
			return true;
		}
		return false;
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x0008F884 File Offset: 0x0008DA84
	private static bool IsInWgoGroup(WgoData wgo, string wgoGroup)
	{
		return GameBalance.Me.HasWgoIdByGroup(wgoGroup, wgo.id) || (wgo.Definition != null && wgo.Definition.wgoGroup == wgoGroup);
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x0008F8B6 File Offset: 0x0008DAB6
	private static bool IsOnetimeId(string wgoId)
	{
		return !string.IsNullOrEmpty(wgoId) && wgoId.EndsWith("_onetime", StringComparison.Ordinal);
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x0008F8D0 File Offset: 0x0008DAD0
	private static float DistanceXzSq(Vector3 a, Vector3 b)
	{
		float num = a.x - b.x;
		float num2 = a.z - b.z;
		return num * num + num2 * num2;
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x0008F900 File Offset: 0x0008DB00
	private static void RemoveDelayedSpawnReferences(SaveFixContext ctx, SGuid uniqueId)
	{
		WgoDelayedSpawnSystemData wgoDelayedSpawnSystemData = ctx.GameSave.wgoDelayedSpawnSystemData;
		List<SpawnDelayedObject> list = ((wgoDelayedSpawnSystemData != null) ? wgoDelayedSpawnSystemData.spawnDelayedObjects : null);
		if (list == null)
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			SpawnDelayedObject spawnDelayedObject = list[i];
			if (spawnDelayedObject == null)
			{
				list.RemoveAt(i);
			}
			else
			{
				SGuid sguid = spawnDelayedObject.wgoUniqueId;
				if (SGuid.IsNullOrEmpty(sguid) && spawnDelayedObject.wgoData != null)
				{
					sguid = spawnDelayedObject.wgoData.UniqueId;
				}
				if (sguid == uniqueId)
				{
					list.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x0008F984 File Offset: 0x0008DB84
	private static void RemoveDelayedEventReferences(SaveFixContext ctx, SGuid uniqueId)
	{
		WgoDelayedEventSystemData wgoDelayedEventSystemData = ctx.GameSave.wgoDelayedEventSystemData;
		List<SGuid> list = ((wgoDelayedEventSystemData != null) ? wgoDelayedEventSystemData.wgoUniqueIds : null);
		if (list == null)
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i] == uniqueId)
			{
				list.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x0008F9D8 File Offset: 0x0008DBD8
	private static void RemoveZombieOnSceneReferences(SaveFixContext ctx, SGuid uniqueId)
	{
		ZombieSystemData zombieSystemData = ctx.GameSave.zombieSystemData;
		List<SGuid> list = ((zombieSystemData != null) ? zombieSystemData.zombieOnSceneWgoIds : null);
		if (list == null)
		{
			return;
		}
		for (int i = list.Count - 1; i >= 0; i--)
		{
			if (list[i] == uniqueId)
			{
				list.RemoveAt(i);
			}
		}
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x0008FA2C File Offset: 0x0008DC2C
	public static void ApplyActivePerkSetResOnAdd(SaveFixContext ctx, GameSave save, string[] perkIds)
	{
		PerkSystemData perkSystemData = save.perkSystemData;
		if (((perkSystemData != null) ? perkSystemData.activePerks : null) == null || perkIds == null)
		{
			return;
		}
		foreach (string text in perkIds)
		{
			if (perkSystemData.HasPerk(text))
			{
				PerkDef dataOrNull = GameBalance.Me.GetDataOrNull<PerkDef>(text);
				if (dataOrNull == null)
				{
					ctx.LogError("Missing PerkDef [" + text + "]");
				}
				else if (!(dataOrNull.setGameResOnAdd == null) && !dataOrNull.setGameResOnAdd.IsEmpty())
				{
					save.playerData.SetRes(dataOrNull.setGameResOnAdd);
					ctx.Log("Applied set_res_on_add for active perk [" + text + "]");
				}
			}
		}
	}

	// Token: 0x06001E77 RID: 7799 RVA: 0x0008FAD8 File Offset: 0x0008DCD8
	public static void DespawnSewrenaFightingLevelsForCompletedQuests(SaveFixContext ctx)
	{
		foreach (ValueTuple<string, string> valueTuple in new ValueTuple<string, string>[]
		{
			new ValueTuple<string, string>("59_town_sewrena_AR1", "fight_AR1_1"),
			new ValueTuple<string, string>("59_town_sewrena_AR2", "fight_AR2_1"),
			new ValueTuple<string, string>("59_town_sewrena_AR4", "fight_AR4_1"),
			new ValueTuple<string, string>("59_town_sewrena_AR5", "fight_AR5_1"),
			new ValueTuple<string, string>("59_town_sewrena_AR6", "fight_AR6_1"),
			new ValueTuple<string, string>("59_town_sewrena_AR7", "fight_AR7_1"),
			new ValueTuple<string, string>("59_town_sewrena_AR10", "fight_AR10_1")
		})
		{
			string item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			SaveCodeFixes_Helper.DespawnFightingLevelIfQuestCompleted(ctx, item, item2);
		}
	}

	// Token: 0x06001E78 RID: 7800 RVA: 0x0008FBB0 File Offset: 0x0008DDB0
	public static void DespawnFightingLevelIfQuestCompleted(SaveFixContext ctx, string questId, string fightingLevelId)
	{
		bool flag;
		if (ctx == null)
		{
			flag = null != null;
		}
		else
		{
			GameSave gameSave = ctx.GameSave;
			flag = ((gameSave != null) ? gameSave.questSystemData : null) != null;
		}
		if (!flag || string.IsNullOrEmpty(questId) || string.IsNullOrEmpty(fightingLevelId))
		{
			return;
		}
		if (!ctx.GameSave.questSystemData.IsQuestInStatus(questId, QuestStatus.Completed))
		{
			return;
		}
		if (SaveCodeFixes_Helper.TryRemoveFightingLevelFromSave(ctx, fightingLevelId))
		{
			ctx.Log(string.Concat(new string[] { "Despawned fighting level [", fightingLevelId, "] because quest [", questId, "] is completed" }));
		}
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x0008FC38 File Offset: 0x0008DE38
	private static bool TryRemoveFightingLevelFromSave(SaveFixContext ctx, string fightingLevelId)
	{
		GameSceneData gameSceneData = SaveCodeFixes_Helper.FindSceneWithFightingLevel(ctx, fightingLevelId);
		if (gameSceneData == null)
		{
			return false;
		}
		gameSceneData.RemoveFightingLevelData(fightingLevelId);
		SaveCodeFixes_Helper.TryUnloadFightingLevelContent(ctx, gameSceneData, fightingLevelId);
		return true;
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x0008FC64 File Offset: 0x0008DE64
	private static void TryUnloadFightingLevelContent(SaveFixContext ctx, GameSceneData ownerScene, string fightingLevelId)
	{
		GameSave gameSave = ctx.GameSave;
		WorldData worldData = ((gameSave != null) ? gameSave.worldData : null);
		if (worldData == null || ownerScene == null)
		{
			return;
		}
		MainGame instance = MainGame.Instance;
		GameSceneConfig gameSceneConfig;
		if (instance == null)
		{
			gameSceneConfig = null;
		}
		else
		{
			List<GameSceneConfig> gameSceneConfigs = instance.gameSceneConfigs;
			gameSceneConfig = ((gameSceneConfigs != null) ? gameSceneConfigs.Find((GameSceneConfig c) => c != null && c.name == ownerScene.id) : null);
		}
		GameSceneConfig gameSceneConfig2 = gameSceneConfig;
		if (gameSceneConfig2 == null || !gameSceneConfig2.IsSceneContentDataLoaded(fightingLevelId))
		{
			return;
		}
		worldData.UnloadContentData(gameSceneConfig2, ownerScene, fightingLevelId);
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x0008FCE8 File Offset: 0x0008DEE8
	private static GameSceneData FindSceneWithFightingLevel(SaveFixContext ctx, string fightingLevelId)
	{
		WorldData worldData = ctx.GameSave.worldData;
		List<GameSceneData> list = ((worldData != null) ? worldData.gameSceneDataList : null);
		if (list == null)
		{
			return null;
		}
		Predicate<FightingLevelData> <>9__0;
		for (int i = 0; i < list.Count; i++)
		{
			GameSceneData gameSceneData = list[i];
			if (((gameSceneData != null) ? gameSceneData.fightingLevels : null) != null)
			{
				List<FightingLevelData> fightingLevels = gameSceneData.fightingLevels;
				Predicate<FightingLevelData> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (FightingLevelData level) => level != null && level.id == fightingLevelId);
				}
				if (fightingLevels.Exists(predicate))
				{
					return gameSceneData;
				}
			}
		}
		return null;
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x0008FD78 File Offset: 0x0008DF78
	public static void RecalculateCorpseCount(SaveFixContext ctx)
	{
		GameSave gameSave = ((ctx != null) ? ctx.GameSave : null);
		PlayerData playerData = ((gameSave != null) ? gameSave.playerData : null);
		if (playerData == null)
		{
			return;
		}
		int num = SaveCodeFixes_Helper.CountBodyCorpses(gameSave.worldData);
		num += SaveCodeFixes_Helper.CountBodyCorpsesInItems(playerData.OverheadItems);
		int resInt = playerData.GetResInt("cur_bodies_count");
		playerData.SetRes("cur_bodies_count", (float)num);
		ctx.Log(string.Format("Recalculated [{0}] {1} → {2}", "cur_bodies_count", resInt, num));
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x0008FDF8 File Offset: 0x0008DFF8
	private static int CountBodyCorpses(WorldData worldData)
	{
		List<GameSceneData> list = ((worldData != null) ? worldData.gameSceneDataList : null);
		if (list == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < list.Count; i++)
		{
			GameSceneData gameSceneData = list[i];
			if (gameSceneData != null)
			{
				num += SaveCodeFixes_Helper.CountBodyCorpsesInDrops(gameSceneData.droppedItems);
				num += SaveCodeFixes_Helper.CountBodyCorpsesInDrops(gameSceneData.queuedDrops);
				num += SaveCodeFixes_Helper.CountBodyCorpsesInWgos(gameSceneData.wgoDataList);
			}
		}
		return num;
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x0008FE60 File Offset: 0x0008E060
	private static int CountBodyCorpsesInDrops(List<DropData> drops)
	{
		if (drops == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < drops.Count; i++)
		{
			DropData dropData = drops[i];
			Item item = ((dropData != null) ? dropData.Item : null);
			if (item != null && !(item.id != "body_corpse"))
			{
				num += item.Count;
			}
		}
		return num;
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x0008FEB8 File Offset: 0x0008E0B8
	private static int CountBodyCorpsesInWgos(List<WgoData> wgos)
	{
		if (wgos == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < wgos.Count; i++)
		{
			WgoData wgoData = wgos[i];
			if (wgoData != null && !string.IsNullOrEmpty(wgoData.id))
			{
				string id = wgoData.id;
				uint num2 = <PrivateImplementationDetails>.ComputeStringHash(id);
				if (num2 <= 2932501607U)
				{
					if (num2 <= 1017398685U)
					{
						if (num2 != 967065828U)
						{
							if (num2 != 1017398685U)
							{
								goto IL_0177;
							}
							if (!(id == "autopsy_table_1"))
							{
								goto IL_0177;
							}
						}
						else if (!(id == "autopsy_table_2"))
						{
							goto IL_0177;
						}
					}
					else if (num2 != 1206942117U)
					{
						if (num2 != 2932501607U)
						{
							goto IL_0177;
						}
						if (!(id == "embalm_table_2"))
						{
							goto IL_0177;
						}
					}
					else
					{
						if (!(id == "resurrection_table_1"))
						{
							goto IL_0177;
						}
						if (wgoData.GetGameResInt("resurrection_prepared") == 1)
						{
							num++;
							goto IL_0177;
						}
						num += SaveCodeFixes_Helper.CountBodyCorpsesInInventory(wgoData.Inventory);
						goto IL_0177;
					}
				}
				else if (num2 <= 3156798543U)
				{
					if (num2 != 2949279226U)
					{
						if (num2 != 3156798543U)
						{
							goto IL_0177;
						}
						if (!(id == "grave_body"))
						{
							goto IL_0177;
						}
						num++;
						goto IL_0177;
					}
					else if (!(id == "embalm_table_1"))
					{
						goto IL_0177;
					}
				}
				else if (num2 != 4172553396U)
				{
					if (num2 != 4222886253U)
					{
						goto IL_0177;
					}
					if (!(id == "pallet_corpse_2"))
					{
						goto IL_0177;
					}
				}
				else if (!(id == "pallet_corpse_1"))
				{
					goto IL_0177;
				}
				num += SaveCodeFixes_Helper.CountBodyCorpsesInInventory(wgoData.Inventory);
			}
			IL_0177:;
		}
		return num;
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x0009004D File Offset: 0x0008E24D
	private static int CountBodyCorpsesInInventory(Inventory inventory)
	{
		if (((inventory != null) ? inventory.Data : null) == null)
		{
			return 0;
		}
		return SaveCodeFixes_Helper.CountBodyCorpsesInItems(inventory.Data.Inventory);
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x00090070 File Offset: 0x0008E270
	private static int CountBodyCorpsesInItems(IReadOnlyList<Item> items)
	{
		if (items == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			Item item = items[i];
			if (item != null && !(item.id != "body_corpse"))
			{
				num += item.Count;
			}
		}
		return num;
	}

	// Token: 0x04001BA5 RID: 7077
	private const float ConfigPositionEpsilonSqr = 0.0001f;

	// Token: 0x04001BA6 RID: 7078
	private const string StumpsWgoGroup = "stumps";

	// Token: 0x04001BA7 RID: 7079
	private const string BushesWgoGroup = "bushes";

	// Token: 0x04001BA8 RID: 7080
	private const string CollectableBushesWgoGroup = "collectable_bushes";

	// Token: 0x04001BA9 RID: 7081
	private const string PalletCorpse1WgoId = "pallet_corpse_1";

	// Token: 0x04001BAA RID: 7082
	private const string PalletCorpse2WgoId = "pallet_corpse_2";

	// Token: 0x04001BAB RID: 7083
	private const string ResurrectionTable1WgoId = "resurrection_table_1";

	// Token: 0x04001BAC RID: 7084
	private const string AutopsyTable1WgoId = "autopsy_table_1";

	// Token: 0x04001BAD RID: 7085
	private const string AutopsyTable2WgoId = "autopsy_table_2";

	// Token: 0x04001BAE RID: 7086
	private const string EmbalmTable1WgoId = "embalm_table_1";

	// Token: 0x04001BAF RID: 7087
	private const string EmbalmTable2WgoId = "embalm_table_2";

	// Token: 0x0200047C RID: 1148
	private enum FloraKind
	{
		// Token: 0x04001BB1 RID: 7089
		Spawner,
		// Token: 0x04001BB2 RID: 7090
		Stump,
		// Token: 0x04001BB3 RID: 7091
		Tree,
		// Token: 0x04001BB4 RID: 7092
		Bush
	}
}
