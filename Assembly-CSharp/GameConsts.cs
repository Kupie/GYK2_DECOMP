using System;
using System.Collections.Generic;

// Token: 0x02000AD1 RID: 2769
public static class GameConsts
{
	// Token: 0x02000AD2 RID: 2770
	public static class Icons
	{
		// Token: 0x04003A7B RID: 14971
		public const string PLACEHOLDER_ITEM_ICON = "i_placeholder";

		// Token: 0x04003A7C RID: 14972
		public const string PLACEHOLDER_BODY_ICON = "i_body";

		// Token: 0x04003A7D RID: 14973
		public const string PLACEHOLDER_CRAFT_RESULT_ICON = "i_b_blueprint_placeholder";

		// Token: 0x04003A7E RID: 14974
		public const string UNKNOWN_MIX_ICON = "i_slot-question";

		// Token: 0x04003A7F RID: 14975
		public const string STAR_ICON_PREFIX = "item_star_";

		// Token: 0x04003A80 RID: 14976
		public const string WHITE_SKULL_ICON = "skull";

		// Token: 0x04003A81 RID: 14977
		public const string RED_SKULL_ICON = "rskull";

		// Token: 0x04003A82 RID: 14978
		public const string WREATH_ICON = "wr";

		// Token: 0x04003A83 RID: 14979
		public const string WREATH_ICON_RED = "wr_red";

		// Token: 0x04003A84 RID: 14980
		public const string TIME_PARAM_ICON = "icon_time";

		// Token: 0x04003A85 RID: 14981
		public const string STAR_PARAM_ICON = "icon_star";

		// Token: 0x04003A86 RID: 14982
		public const string INSPIRATION_HINT_ICON = "hint_inspiration";

		// Token: 0x04003A87 RID: 14983
		public const string ENERGY_HINT_ICON = "energy";

		// Token: 0x04003A88 RID: 14984
		public const string INSANITY_HINT_ICON = "insanity";

		// Token: 0x04003A89 RID: 14985
		public const string INSANITY_LOCK_HINT_ICON = "icon_sanity_lock";

		// Token: 0x04003A8A RID: 14986
		public const string NOT_ENOUGH_ENERGY_HINT_ICON = "no_energy";

		// Token: 0x04003A8B RID: 14987
		public const string NOT_ENOUGH_INSANITY_HINT_ICON = "no_insanity";

		// Token: 0x04003A8C RID: 14988
		public const string MASTERY_BONUS_GARDEN_ICON = "icon_shovel";

		// Token: 0x04003A8D RID: 14989
		public const string MASTERY_BONUS_GENERATOR_ICON = "icon_hand";

		// Token: 0x04003A8E RID: 14990
		public const string LOCK_ICON = "icon_lock";

		// Token: 0x04003A8F RID: 14991
		public const string INFINITY_HINT_ICON = "∞";

		// Token: 0x04003A90 RID: 14992
		public const string ENERGY_1 = "energy_1";

		// Token: 0x04003A91 RID: 14993
		public const string ENERGY_2 = "energy_2";

		// Token: 0x04003A92 RID: 14994
		public const string ENERGY_3 = "energy_3";

		// Token: 0x04003A93 RID: 14995
		public const string INSANITY_1 = "insanity_1";

		// Token: 0x04003A94 RID: 14996
		public const string INSANITY_2 = "insanity_2";

		// Token: 0x04003A95 RID: 14997
		public const string INSANITY_3 = "insanity_3";

		// Token: 0x04003A96 RID: 14998
		public const string TOWN_QUALITY_ICON = "reputation-citizens";
	}

	// Token: 0x02000AD3 RID: 2771
	public static class WGOs
	{
		// Token: 0x04003A97 RID: 14999
		public const string EXHUMATION_GRAVE_WGO_ID = "grave_exhume";

		// Token: 0x04003A98 RID: 15000
		public const string BODY_GRAVE_WGO_ID = "grave_body";

		// Token: 0x04003A99 RID: 15001
		public const string EMPTY_GRAVE_WGO_ID = "grave_empty";

		// Token: 0x04003A9A RID: 15002
		public const string GROUND_GRAVE_WGO_ID = "grave_ground";

		// Token: 0x04003A9B RID: 15003
		public const string SEED_GENERATOR_WGO_ID = "seed_generator";

		// Token: 0x04003A9C RID: 15004
		public const string MERCENARY_FIGHTER_CONTAINER_WGO_ID = "fighter_container_mercenary";

		// Token: 0x04003A9D RID: 15005
		public const string FIGHTER_CONTAINER_WGO_ID = "fighter_container";

		// Token: 0x04003A9E RID: 15006
		public const string MERCENARY_FIGHTER_PREFIX_ID = "npc_town_barracks_mercenary";

		// Token: 0x04003A9F RID: 15007
		public const string ZOMBIE_FIGHTER_ALLY = "zmb_wild_mob_allie";

		// Token: 0x04003AA0 RID: 15008
		public const string ALCHEMY_WORKBENCH = "alchemy_workbench";

		// Token: 0x04003AA1 RID: 15009
		public const string GARDEN_EMPTY_BED = "garden_empty";

		// Token: 0x04003AA2 RID: 15010
		public const string VINEYARD_EMPTY_BED = "vineyard_empty";

		// Token: 0x04003AA3 RID: 15011
		public const string GARDEN_PREFIX = "garden_";

		// Token: 0x04003AA4 RID: 15012
		public const string VINEYARD_PREFIX = "vineyard_";

		// Token: 0x04003AA5 RID: 15013
		public const string GARDEN_VINEYARD_READY_POSTFIX = "_ready";

		// Token: 0x04003AA6 RID: 15014
		public const string WOOD_CONTAINER = "wood_container";

		// Token: 0x04003AA7 RID: 15015
		public const string CRATES_SMALL = "crates_small";

		// Token: 0x04003AA8 RID: 15016
		public const string WAREHOUSE_CRANE = "warehouse_crane";

		// Token: 0x04003AA9 RID: 15017
		public const string BODY_DROP_GRAVE = "body_drop_grave_object";
	}

	// Token: 0x02000AD4 RID: 2772
	public static class Groups
	{
		// Token: 0x04003AAA RID: 15018
		public static List<string> fertylizerGroups = new List<string> { "fert_star", "fert_time", "fert_crop" };

		// Token: 0x04003AAB RID: 15019
		public const string BODY_ITEM_GROUP = "body";

		// Token: 0x04003AAC RID: 15020
		public const string CORPSE_ITEM_GROUP = "corpse";

		// Token: 0x04003AAD RID: 15021
		public const string ZOMBIE_ITEM_GROUP = "zombie";

		// Token: 0x04003AAE RID: 15022
		public const string WILD_ZOMBIE = "wild_zombie";

		// Token: 0x04003AAF RID: 15023
		public const string TOMBSTONE_ITEM_GROUP = "gravetop";

		// Token: 0x04003AB0 RID: 15024
		public const string FENCE_ITEM_GROUP = "gravebot";

		// Token: 0x04003AB1 RID: 15025
		public const string BURIAL_REWARD_ITEM_GROUP = "burial_reward";

		// Token: 0x04003AB2 RID: 15026
		public const string BODY_PART_ITEM_GROUP = "bodypart";

		// Token: 0x04003AB3 RID: 15027
		public const string TOOL_ITEM_GROUP = "tool";

		// Token: 0x04003AB4 RID: 15028
		public const string WEAPON_ITEM_GROUP = "weapon";

		// Token: 0x04003AB5 RID: 15029
		public const string FAT_ITEM_GROUP = "gr_fat";

		// Token: 0x04003AB6 RID: 15030
		public const string ORGAN_MISTAKE_ITEM_GROUP = "mistake";

		// Token: 0x04003AB7 RID: 15031
		public const string SEED_ITEM_GROUP = "seed";

		// Token: 0x04003AB8 RID: 15032
		public const string SEEDABLE_ITEM_GROUP = "seedable";

		// Token: 0x04003AB9 RID: 15033
		public const string FERTILIZER_ITEM_GROUP = "fertilizer";

		// Token: 0x04003ABA RID: 15034
		public const string FUEL_ITEM_GROUP = "fuel";

		// Token: 0x04003ABB RID: 15035
		public const string MERCENARY_WGO_GROUP = "mercenary";

		// Token: 0x04003ABC RID: 15036
		public const string MELEE_WEAPON_ITEM_GROUP = "melee";

		// Token: 0x04003ABD RID: 15037
		public const string RANGE_WEAPON_ITEM_GROUP = "range";

		// Token: 0x04003ABE RID: 15038
		public const string SPAWNGER_WGO_GROUP = "spawner";

		// Token: 0x04003ABF RID: 15039
		public const string VINEYARD_SEED_ITEM_GROUP = "vineyard_seed";

		// Token: 0x04003AC0 RID: 15040
		public const string VINEYARD_OBJECTS_GROUP = "vineyard_objects";

		// Token: 0x04003AC1 RID: 15041
		public const string GARDEN_BED_WGO_GROUP = "garden_bed";

		// Token: 0x04003AC2 RID: 15042
		public const string GARDEN_TABLETS_WGO_GROUP = "garden_tablets";

		// Token: 0x04003AC3 RID: 15043
		public const string BARRICADES_WGO_GROUP = "barricades";

		// Token: 0x04003AC4 RID: 15044
		public const string TOWERS_WGO_GROUP = "towers";

		// Token: 0x04003AC5 RID: 15045
		public const string SUPPLY_BOX_ITEM_GROUP = "town_box";

		// Token: 0x04003AC6 RID: 15046
		public const string BATTLE_POTION_ITEM_GROUP = "battle_potion";
	}

	// Token: 0x02000AD5 RID: 2773
	public static class GameLogic
	{
		// Token: 0x04003AC7 RID: 15047
		public const string FISHING_RESTORE_POSTFIX = "_restore";

		// Token: 0x04003AC8 RID: 15048
		public const int SEED_PURCHASE_STACK_SIZE = 4;
	}

	// Token: 0x02000AD6 RID: 2774
	public static class Items
	{
		// Token: 0x04003AC9 RID: 15049
		public const string BODY_CORPSE_ITEM = "body_corpse";

		// Token: 0x04003ACA RID: 15050
		public const string BODY_ZOMBIE_ITEM = "body_zombie";

		// Token: 0x04003ACB RID: 15051
		public const string EXHUME_CERTIFICATE_ITEM = "exhume_certificate";

		// Token: 0x04003ACC RID: 15052
		public const string NO_BAIT = "no_bait";

		// Token: 0x04003ACD RID: 15053
		public const string WOOD = "wood";

		// Token: 0x04003ACE RID: 15054
		public const string WATER = "water";
	}

	// Token: 0x02000AD7 RID: 2775
	public static class Crafts
	{
		// Token: 0x04003ACF RID: 15055
		public const string CORPSE_TO_ZOMBIE_CRAFT = "corpse_zombie_transition";
	}

	// Token: 0x02000AD8 RID: 2776
	public static class Buildings
	{
		// Token: 0x04003AD0 RID: 15056
		public const string GRAVEYARD_MODULE_PLACE = "graveyard_module_p";
	}

	// Token: 0x02000AD9 RID: 2777
	public static class Hints
	{
		// Token: 0x04003AD1 RID: 15057
		public const string CRAFT = "hint_craft";

		// Token: 0x04003AD2 RID: 15058
		public const string OPEN = "hint_open";

		// Token: 0x04003AD3 RID: 15059
		public const string INSPECT = "action_inspect";

		// Token: 0x04003AD4 RID: 15060
		public const string TAKE = "hint_take";

		// Token: 0x04003AD5 RID: 15061
		public const string PUT = "hint_put";

		// Token: 0x04003AD6 RID: 15062
		public const string TAKE_ALL = "hint_take_all";

		// Token: 0x04003AD7 RID: 15063
		public const string WORK = "hint_work";

		// Token: 0x04003AD8 RID: 15064
		public const string PLACE_BODY = "hint_place_body";

		// Token: 0x04003AD9 RID: 15065
		public const string BUILD = "hint_build";

		// Token: 0x04003ADA RID: 15066
		public const string CLIMB = "hint_climb";

		// Token: 0x04003ADB RID: 15067
		public const string PRAY = "hint_pray";

		// Token: 0x04003ADC RID: 15068
		public const string INTERACT = "hint_interact";

		// Token: 0x04003ADD RID: 15069
		public const string INTERACTION = "hint_interaction";

		// Token: 0x04003ADE RID: 15070
		public const string FISHING = "ui_submit_bait";

		// Token: 0x04003ADF RID: 15071
		public const string PUT_ZOMBIE = "hint_put_zombie";

		// Token: 0x04003AE0 RID: 15072
		public const string SURVEY = "hint_survey";

		// Token: 0x04003AE1 RID: 15073
		public const string SET_TO_HOT_BAR = "hint_set_to_hot_bar";

		// Token: 0x04003AE2 RID: 15074
		public const string MIX = "hint_alchemy";

		// Token: 0x04003AE3 RID: 15075
		public const string PLACE_FLAG = "hint_put_flag";

		// Token: 0x04003AE4 RID: 15076
		public const string TAKE_FLAG = "hint_take_flag";

		// Token: 0x04003AE5 RID: 15077
		public const string PLANT = "hint_plant";

		// Token: 0x04003AE6 RID: 15078
		public const string FERTILIZE = "hint_fertilize";

		// Token: 0x04003AE7 RID: 15079
		public const string CREMATE_BODY = "hint_cremate_body";

		// Token: 0x04003AE8 RID: 15080
		public const string DROP_BODY = "hint_drop_body";

		// Token: 0x04003AE9 RID: 15081
		public const string GRAVE = "hint_grave";

		// Token: 0x04003AEA RID: 15082
		public const string USE_CLOTHES = "hint_use_clothes";

		// Token: 0x04003AEB RID: 15083
		public const string USE_SEEDS = "hint_use_seeds";

		// Token: 0x04003AEC RID: 15084
		public const string USE_READ = "hint_use_read";
	}
}
