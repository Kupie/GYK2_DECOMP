using System;

// Token: 0x0200047A RID: 1146
public static class SaveCodeFixes
{
	// Token: 0x06001E46 RID: 7750 RVA: 0x0008DF04 File Offset: 0x0008C104
	public static void Apply(SaveFixContext ctx)
	{
		if (((ctx != null) ? ctx.GameSave : null) == null)
		{
			return;
		}
		GameSaveVersion saveVersion = ctx.GameSave.SaveVersion;
		if (saveVersion < 960)
		{
			SaveCodeFixes_Helper.UnlockOrHideTechByQuest(ctx, "tech_town_building_1", "29_port_linda_meet");
		}
		if (saveVersion < 961)
		{
			if (ctx.GameSave.questSystemData.IsQuestInStatus("92_doctor_cure_tell", QuestStatus.Completed))
			{
				ctx.GameSave.questSystemData.CompleteQuest("92_doctor_cure_exit", 0f);
			}
			SaveCodeFixes_Helper.CopyUnlockedTutorialsToViewed(ctx.GameSave.knowledgeSystem);
			GlobalScriptsManager.FireEvent("GameSaveFixer", "0961", null);
			SaveCodeFixes_Helper.ResetNpcLifeSimulator(ctx.GameSave);
			GlobalScriptsManager.FireEvent("GameSaveFixer", "0961_life_sim", null);
		}
		if (saveVersion < 1000)
		{
			SaveCodeFixes_Helper.MigrateDelayedSpawnWgoUniqueIds(ctx);
			ValueTuple<string, string, int>[] array = new ValueTuple<string, string, int>[]
			{
				new ValueTuple<string, string, int>("24_doctor_zombies_powder", "hero_clothes_9004", 1),
				new ValueTuple<string, string, int>("10_village_food_samples", "hero_clothes_9002", 1),
				new ValueTuple<string, string, int>("12_village_boat_tools", "hero_clothes_9005", 1),
				new ValueTuple<string, string, int>("19_base_ceremony_sermon", "hero_clothes_9008", 1),
				new ValueTuple<string, string, int>("14_guard_faith_bell", "hero_clothes_9006", 1)
			};
			string[] array2 = new string[] { "73_crossroad_better_upgraded", "73_crossroad_better_upgraded_t2", "144_crossroad_home_improve" };
			string[] array3 = new string[] { "insp_aghata_house", "insp_furniture_craft", "insp_zombie_woodcutter" };
			ValueTuple<string, string>[] array4 = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("18_base_doctor_encounter", "insp_blockages"),
				new ValueTuple<string, string>("4_intro_base_crossroad", "insp_bridge_cross")
			};
			GameSave gameSave = ctx.GameSave;
			QuestSystemData questSystemData = gameSave.questSystemData;
			KnowledgeSystem knowledgeSystem = gameSave.knowledgeSystem;
			foreach (ValueTuple<string, string, int> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				int item3 = valueTuple.Item3;
				SaveCodeFixes_Helper.GiveItemIfQuestCompleted(ctx, questSystemData, item, item2, item3);
			}
			if (SaveCodeFixes_Helper.WasQuestStarted(questSystemData, "71_doctor_sister_corpse"))
			{
				SaveCodeFixes_Helper.GiveItem(ctx, "resurrection_liquid", 2);
				SaveCodeFixes_Helper.GiveItem(ctx, "collar_gold", 2);
			}
			for (int j = 0; j < array3.Length; j++)
			{
				SaveCodeFixes_Helper.HideInspiration(knowledgeSystem, array3[j]);
			}
			foreach (ValueTuple<string, string> valueTuple2 in array4)
			{
				string item4 = valueTuple2.Item1;
				string item5 = valueTuple2.Item2;
				SaveCodeFixes_Helper.UnlockOrHideInspirationByQuest(ctx, knowledgeSystem, questSystemData, item5, item4);
			}
			for (int l = 0; l < array2.Length; l++)
			{
				SaveCodeFixes_Helper.AddInspirationIfQuestCompleted(gameSave, questSystemData, array2[l], "insp_aghata_house", 1);
			}
			knowledgeSystem.TryRevealInspirations();
			SaveCodeFixes_Helper.UnlockOrHideTechByQuest(ctx, "tech_town_building_1", "29_port_linda_meet");
			SaveCodeFixes_Helper.UnlockTechIfQuestCompleted(ctx, questSystemData, knowledgeSystem, "40_village_fishing_bring", "fishing_3");
			if (!knowledgeSystem.IsTechUnlocked("fishing_3"))
			{
				SaveCodeFixes_Helper.HideTech(ctx, knowledgeSystem, "fishing_3");
			}
			SaveCodeFixes_Helper.UnlockTechIfQuestCompleted(ctx, questSystemData, knowledgeSystem, "4_intro_base_waking_up", "wood_basic");
			SaveCodeFixes_Helper.UnlockBuildingIfTechUnlocked(knowledgeSystem, "firewood_shed_2", "firewood_shed_kitchen_2_place_p");
			SaveCodeFixes_Helper.CatchUpVendorTiers(ctx, gameSave.vendorSystem, questSystemData, "npc_head_of_the_village", new string[] { "90_village_elections_leave_yes", "124_village_money_money" });
			SaveCodeFixes_Helper.CatchUpVendorTiers(ctx, gameSave.vendorSystem, questSystemData, "npc_herm", new string[] { "39_village_frill_bring" });
			questSystemData.ChangeQuestHiddenState("151_base_90yes_god", true);
			SaveCodeFixes_Helper.ForceHideTalentLevelUp(knowledgeSystem, "talent_telling_tales");
			SaveCodeFixes_Helper.ForceHideTalentLevelUp(knowledgeSystem, "talent_marauder");
			string[] array5 = new string[]
			{
				"buff_beer", "perk_cook", "perk_cook_2", "perk_heat_saver", "perk_fisherman", "perk_fishcutter", "buff_combat_1", "buff_combat_2", "buff_combat_3", "buff_farm_1",
				"buff_farm_2", "buff_farm_3", "buff_hypno_1", "buff_hypno_2", "buff_hypno_3", "buff_sauna"
			};
			SaveCodeFixes_Helper.ApplyActivePerkSetResOnAdd(ctx, gameSave, array5);
		}
		if (saveVersion < GameSaveVersion.Parse("1.000.1a"))
		{
			SaveCodeFixes_Helper.RemoveDuplicateFloraAtSameCoordinates(ctx);
		}
		if (saveVersion < 1000.5f)
		{
			SaveCodeFixes_Helper.RevealTechsFromCompletedQuests(ctx);
			SaveCodeFixes_Helper.RevealHiddenTechsIfParentRevealed(ctx);
			SaveCodeFixes_Helper.RevealHiddenTechsFromWorldObjects(ctx);
		}
		if (saveVersion < 1001)
		{
			GlobalScriptsManager.FireEvent("GameSaveFixer", "1001_life_sim", null);
		}
		if (saveVersion < 1001.1f)
		{
			SaveCodeFixes_Helper.DespawnSewrenaFightingLevelsForCompletedQuests(ctx);
		}
		if (saveVersion < 1002)
		{
			GameSave gameSave2 = ctx.GameSave;
			QuestSystemData questSystemData2 = gameSave2.questSystemData;
			if (questSystemData2.IsQuestInStatus("120_pre_B2_2_fights_win", QuestStatus.InProgress))
			{
				gameSave2.WorldData.GetGameSceneDataById("RuinedTemple").ApplyStageForFightingLevel("fight_B2_1", 2);
			}
			if (questSystemData2.IsQuestInStatus("36_port_trademaster_lenses", QuestStatus.Completed))
			{
				gameSave2.knowledgeSystem.UnlockPhrase("trade_warehouse_manager");
			}
			if (gameSave2.knowledgeSystem.blackListPhrases.Contains("get_new_people_lock_10"))
			{
				gameSave2.knowledgeSystem.AddPhraseToBlackList("get_new_people_lock_10_1");
				gameSave2.knowledgeSystem.AddPhraseToBlackList("get_new_people_lock_10_2");
			}
			if (!gameSave2.knowledgeSystem.IsTechUnlocked("resurrection_advanced"))
			{
				SaveCodeFixes_Helper.RevokeUnlockedCraft(ctx, gameSave2.knowledgeSystem, "ingot_gold");
			}
			SaveCodeFixes_Helper.RevokeUnlockedCraft(ctx, gameSave2.knowledgeSystem, "prayer_likes");
			SaveCodeFixes_Helper.ResyncQuestVisualisation(ctx, questSystemData2, "61_after_clearing_A_complete");
			SaveCodeFixes_Helper.CompleteVisualQuestIfQuestCompleted(ctx, questSystemData2, "61_after_clearing_A_complete", "61_after_clearing_A_complete_visual");
		}
		if (saveVersion < 1003)
		{
			GlobalScriptsManager.FireEvent("GameSaveFixer", "1003", null);
			GameSave gameSave3 = ctx.GameSave;
			QuestSystemData questSystemData3 = gameSave3.questSystemData;
			SaveCodeFixes_Helper.UnlockTechIfQuestCompleted(ctx, questSystemData3, gameSave3.knowledgeSystem, "40_village_fishing_bring", "fishing_4");
			SaveCodeFixes_Helper.UnlockTechIfQuestCompleted(ctx, questSystemData3, gameSave3.knowledgeSystem, "41_forest_bonfire_food", "cooking_soldier");
			SaveCodeFixes_Helper.AddMissingVendorOrders(ctx, gameSave3.vendorSystem, "npc_workshop_foreman");
			SaveCodeFixes_Helper.CatchUpVendorTiers(ctx, gameSave3.vendorSystem, questSystemData3, "npc_workshop_foreman", new string[] { "58_port_supplies_order" });
			SaveCodeFixes_Helper.CatchUpDarkFinalBattleApproach(ctx, gameSave3, questSystemData3);
		}
		if (saveVersion < 1004)
		{
			SaveCodeFixes_Helper.RecalculateCorpseCount(ctx);
			SaveCodeFixes_Helper.UnlockBuildingIfTechUnlocked(ctx.GameSave.knowledgeSystem, "wood_basic", "wood_container_p");
		}
	}
}
