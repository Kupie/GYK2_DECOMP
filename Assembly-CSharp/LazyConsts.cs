using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x02000ADB RID: 2779
public static class LazyConsts
{
	// Token: 0x04003AEF RID: 15087
	public const string PLACEHOLDER_ITEM_ICON = "i_placeholder";

	// Token: 0x04003AF0 RID: 15088
	public const string PLACEHOLDER_CRAFT_RESULT_ICON = "i_b_blueprint_placeholder";

	// Token: 0x04003AF1 RID: 15089
	public const string STAR_ICON_PREFIX = "item_star_";

	// Token: 0x04003AF2 RID: 15090
	public const string ITEM_FROM_GAME_RES_ATOM_PREFIX = "game_res_";

	// Token: 0x04003AF3 RID: 15091
	public const string WORLD_ZONE_RES_ATOM_PREFIX = "wz_";

	// Token: 0x04003AF4 RID: 15092
	public const string TELEPORT_POINT_PREFIX = "tp_point_";

	// Token: 0x04003AF5 RID: 15093
	public const string TOWN_BUILDING_CRAFT_PREFIX = "town_building_craft:";

	// Token: 0x04003AF6 RID: 15094
	public const string ENERGY_KEY = "energy";

	// Token: 0x04003AF7 RID: 15095
	public const string INSANITY_KEY = "insanity";

	// Token: 0x04003AF8 RID: 15096
	public const string HAPPINESS_KEY = "happiness";

	// Token: 0x04003AF9 RID: 15097
	public const string INSANITY_LOCK_KEY = "insanity_lock";

	// Token: 0x04003AFA RID: 15098
	public const string TECH_RED_SPHERE = "tech_red";

	// Token: 0x04003AFB RID: 15099
	public const string TECH_GREEN_SPHERE = "tech_green";

	// Token: 0x04003AFC RID: 15100
	public const string TECH_BLUE_SPHERE = "tech_blue";

	// Token: 0x04003AFD RID: 15101
	public const string RUNE_RED = "rune_r";

	// Token: 0x04003AFE RID: 15102
	public const string RUNE_GREEN = "rune_g";

	// Token: 0x04003AFF RID: 15103
	public const string RUNE_BLUE = "rune_b";

	// Token: 0x04003B00 RID: 15104
	public const string MONEY_KEY = "money";

	// Token: 0x04003B01 RID: 15105
	public const string GLOBAL_PPL_KEY = "global_ppl";

	// Token: 0x04003B02 RID: 15106
	public const string CUR_PRAY_PPL_KEY = "cur_pray_ppl";

	// Token: 0x04003B03 RID: 15107
	public const string LAST_GO_TO_PATH_LENGTH_KEY = "lastGoToPathLength";

	// Token: 0x04003B04 RID: 15108
	public const string DURATION_KEY = "duration";

	// Token: 0x04003B05 RID: 15109
	public const string QUALITY_BONUS_KEY = "quality_bonus";

	// Token: 0x04003B06 RID: 15110
	public const string CROP_BONUS_KEY = "crop_bonus";

	// Token: 0x04003B07 RID: 15111
	public const string DURATION_BONUS_KEY = "duration_bonus";

	// Token: 0x04003B08 RID: 15112
	public const string FAILED_PROGRESS_TICKS_KEY = "failed_cells";

	// Token: 0x04003B09 RID: 15113
	public const string SUCCEDED_PROGRESS_TICKS_KEY = "succeded_cells";

	// Token: 0x04003B0A RID: 15114
	public const string GARDEN_FERTILIZERS_SLOTS_KEY = "g_garden_fertilizer_slots";

	// Token: 0x04003B0B RID: 15115
	public const string GARDEN_CRAFT_DECREASE_CRAFT_TIME_KEY = "g_garden_autocraft_dec";

	// Token: 0x04003B0C RID: 15116
	public const string GARDEN_BED_FARMING_MASTERY_KEY = "g_garden_farming_base";

	// Token: 0x04003B0D RID: 15117
	public const string GARDEN_BED_FARMING_LEVEL_KEY = "g_garden_lvl";

	// Token: 0x04003B0E RID: 15118
	public const string GARDEN_BED_VINEYARD_MASTERY_KEY = "g_vineyard_farming_base";

	// Token: 0x04003B0F RID: 15119
	public const string GARDEN_BED_VINEYARD_LEVEL_KEY = "g_vineyard_lvl";

	// Token: 0x04003B10 RID: 15120
	public const string GARDEN_PERK_SLOT_PREFIX = "perk_slot_";

	// Token: 0x04003B11 RID: 15121
	public const string GARDEN_SEED_MASTERY_LOCK_KEY = "seed_mastery_lock";

	// Token: 0x04003B12 RID: 15122
	public const string GARDEN_COMMON_CROP_OUTPUT_KEY = "crop";

	// Token: 0x04003B13 RID: 15123
	public const string GARDEN_BRONZE_CROP_OUTPUT_KEY = "crop_b";

	// Token: 0x04003B14 RID: 15124
	public const string GARDEN_SILVER_CROP_OUTPUT_KEY = "crop_s";

	// Token: 0x04003B15 RID: 15125
	public const string GARDEN_GOLDEN_CROP_OUTPUT_KEY = "crop_g";

	// Token: 0x04003B16 RID: 15126
	public const string MILESTONES_ACTIVATED_KEY = "milestones_activated";

	// Token: 0x04003B17 RID: 15127
	public const string GARDEN_FERTILIZER_PERK_PREFIX = "perk_fertilize_";

	// Token: 0x04003B18 RID: 15128
	public const string FAKE_CRAFT_PREFIX = "fake_";

	// Token: 0x04003B19 RID: 15129
	public const string ZOMBIE_CARETAKER_DEFAULT_GD_POINT_ID = "zombie_porter_station_gd_point";

	// Token: 0x04003B1A RID: 15130
	public const string ZOMBIE_CARETAKER_STATION_WGO_ID = "zombie_supplier_station";

	// Token: 0x04003B1B RID: 15131
	public const string ZOMBIE_CARETAKER_STATION_NO_WORKER_ICON = "i_no_zombie_delivery";

	// Token: 0x04003B1C RID: 15132
	public const string RED_CROSS_BIG_ICON = "i_red_cross_big_icon";

	// Token: 0x04003B1D RID: 15133
	public const string ZOMBIE_GARDENER_DEFAULT_GD_POINT_ID = "zombie_garden_crafter_gd_point";

	// Token: 0x04003B1E RID: 15134
	public const string ZOMBIE_SAWMILL_WOOD_CRAFTER_GD_POINT = "zombie_sawmill_wood_crafter_gd_point";

	// Token: 0x04003B1F RID: 15135
	public const string ZOMBIE_SAWMILL_WOOD_CONTAINER_GD_POINT = "zombie_sawmill_wood_container_gd_point";

	// Token: 0x04003B20 RID: 15136
	public const string ZOMBIE_MINE_CRAFTER_GD_POINT = "zombie_mine_crafter_gd_point";

	// Token: 0x04003B21 RID: 15137
	public const string ZOMBIE_CLAY_CRAFTER_GD_POINT = "zombie_clay_sand_crafter_gd_point";

	// Token: 0x04003B22 RID: 15138
	public const string ZOMBIE_SAND_CRAFTER_GD_POINT = "zombie_clay_sand_crafter_gd_point";

	// Token: 0x04003B23 RID: 15139
	public const string STAMINA_KEY = "stamina";

	// Token: 0x04003B24 RID: 15140
	public const string LINKED_FIGHTERS_FLAG_KEY = "fighters_flag";

	// Token: 0x04003B25 RID: 15141
	public const string CHULK_BOARD_ENABLED_RES_NAME = "chalk_board_enabled";

	// Token: 0x04003B26 RID: 15142
	public const string DONKEY_BODY_DROP_CHANCE = "donkey_body_drop_chance";

	// Token: 0x04003B27 RID: 15143
	public const string NPC_LIFE_SIM_HOME_RES_ID = "npc_life_sim_home";

	// Token: 0x04003B28 RID: 15144
	public const string NPC_LIFE_SIM_DISABLED_FLAG = "npc_life_sim_disabled_flag";

	// Token: 0x04003B29 RID: 15145
	public const string CUR_BODIES_COUNT_KEY = "cur_bodies_count";

	// Token: 0x04003B2A RID: 15146
	public const string LIFT_TARGET_STORAGE = "target_storage_wgo";

	// Token: 0x04003B2B RID: 15147
	public const string CUR_ZOMBIES_COUNT_KEY = "cur_zombies_count";

	// Token: 0x04003B2C RID: 15148
	public const string ZOMBIE_LIMIT_MECHANIC_KEY = "zombies_limit_mechanic";

	// Token: 0x04003B2D RID: 15149
	public const string RESURRECTION_PREPARED_KEY = "resurrection_prepared";

	// Token: 0x04003B2E RID: 15150
	public const string RESURRECTION_HAS_POWER = "resurrection_has_power";

	// Token: 0x04003B2F RID: 15151
	public const string DONKEY_ADDITIONAL_BODY_DROP_KEY = "donkey_drop_additional";

	// Token: 0x04003B30 RID: 15152
	public const string NUN_MANY_BODY_DROP_KEY = "nun_many_body_drop";

	// Token: 0x04003B31 RID: 15153
	public const string DONKEY_DROP_CHANCE_REDUCE_AFTER_SUCCESS = "donkey_drop_chance_reduce_after_success";

	// Token: 0x04003B32 RID: 15154
	public const string DONKEY_DROP_CHANCE_INCREASE_AFTER_FAIL = "donkey_drop_chance_increase_after_fail";

	// Token: 0x04003B33 RID: 15155
	public const string DONKEY_DROP_CHANCE_INCREASE_AFTER_SKIP = "donkey_drop_chance_increase_after_skip";

	// Token: 0x04003B34 RID: 15156
	public const string NUN_MANY_DROP_CHANCE_REDUCE_AFTER_SUCCESS = "nun_many_drop_chance_reduce_after_success";

	// Token: 0x04003B35 RID: 15157
	public const string NUN_MANY_DROP_CHANCE_INCREASE_AFTER_FAIL = "nun_many_drop_chance_increase_after_fail";

	// Token: 0x04003B36 RID: 15158
	public const string NUN_MANY_DROP_CHANCE_INCREASE_AFTER_SKIP = "nun_many_drop_chance_increase_after_skip";

	// Token: 0x04003B37 RID: 15159
	public const string CRAFT_REACHED_GOLD_LEVEL_KEY = "reached_gold";

	// Token: 0x04003B38 RID: 15160
	public const string CRAFT_REACHED_SILVER_LEVEL_KEY = "reached_silver";

	// Token: 0x04003B39 RID: 15161
	public const string CRAFT_REACHED_BRONZE_LEVEL_KEY = "reached_bronze";

	// Token: 0x04003B3A RID: 15162
	public const string DO_NOT_CHECK_WORKER_DEPENDENT_VALUES_KEY = "do_not_check_worker_dependent_values";

	// Token: 0x04003B3B RID: 15163
	public const string DO_NOT_CHECK_MULTIINVENTORY_SPACE_KEY = "do_not_check_multiinventory_space";

	// Token: 0x04003B3C RID: 15164
	public const string AUTO_START_SAME_CRAFT_AFTER_PICKUP_KEY = "auto_start_same_craft_after_pickup";

	// Token: 0x04003B3D RID: 15165
	public const string ZOMBIE_PORTER_STAYING_AT_STATION_FLAG = "is_staying_at_porter_station";

	// Token: 0x04003B3E RID: 15166
	public const string ZOMBIE_PORTER_MOVING_FLAG = "is_moving_to_target_world_zone";

	// Token: 0x04003B3F RID: 15167
	public const float MORNING = 0.25f;

	// Token: 0x04003B40 RID: 15168
	public const float EVENING = 0.8f;

	// Token: 0x04003B41 RID: 15169
	public const string GARDEN_WORLD_ZONE_ID = "garden";

	// Token: 0x04003B42 RID: 15170
	public const string MILITARY_BASE_WORLD_ZONE_ID = "town_guard_barracks";

	// Token: 0x04003B43 RID: 15171
	public const string CONVEYOR_STORAGE_WORLD_ZONE_ID = "conveyor_storage";

	// Token: 0x04003B44 RID: 15172
	public const string CONVEYOR_BUILDING_NOT_REMOVABLE_KEY = "conveyor_build_is_not_removable";

	// Token: 0x04003B45 RID: 15173
	public const string LOCK_BUILDING_REMOVAL_RES = "lock_building_removal";

	// Token: 0x04003B46 RID: 15174
	public const string NULL_WGO_ID = "0";

	// Token: 0x04003B47 RID: 15175
	public const string TREES_WGO_GROUP = "trees";

	// Token: 0x04003B48 RID: 15176
	public const string GRAVEYARD_MODULES_WGO_GROUP = "graveyard_modules";

	// Token: 0x04003B49 RID: 15177
	public const string ITEM_PRICE_GLOBAL_MODIFICATOR_POSTFIX = "_base_price_global_mod";

	// Token: 0x04003B4A RID: 15178
	public const string ITEM_COUNT_GLOBAL_MODIFICATOR_POSTFIX = "_base_count_global_mod";

	// Token: 0x04003B4B RID: 15179
	public const string BOOST_CRAFT_POSTFIX = "_boost";

	// Token: 0x04003B4C RID: 15180
	public const string MIX_CRAFT_PREFIX = "mix";

	// Token: 0x04003B4D RID: 15181
	public const string PARISHIONER_CRIT_CHANCE_MOD = "parishioner_crit_chance_mod";

	// Token: 0x04003B4E RID: 15182
	public const string PARISHIONER_CRIT_POWER_MOD = "parishioner_crit_power_mod";

	// Token: 0x04003B4F RID: 15183
	public const string ITEM_FAITH_ID = "faith";

	// Token: 0x04003B50 RID: 15184
	public const string ITEM_TEMPTATION_ID = "temptation";

	// Token: 0x04003B51 RID: 15185
	public const string RESERVOIR_FISH_CAUGHT_POSTFIX = "_caught";

	// Token: 0x04003B52 RID: 15186
	public const string BUILDING_WINDOW_DEFAULT_TAB_ID = "tab_building_default";

	// Token: 0x04003B53 RID: 15187
	public const int CRAFT_INV_SIZE_FOR_ZOMBIE_INSERTABLE_WGO = 50;

	// Token: 0x04003B54 RID: 15188
	public const int MAX_PRODUCT_TIER = 3;

	// Token: 0x04003B55 RID: 15189
	public const int START_MONEY = 50;

	// Token: 0x04003B56 RID: 15190
	public const int MAX_INGREDIENTS_IN_ALCHEMY = 3;

	// Token: 0x04003B57 RID: 15191
	public const int MAX_RUNES_IN_ALCHEMY = 5;

	// Token: 0x04003B58 RID: 15192
	public const int MIN_RUNES_IN_ALCHEMY = 0;

	// Token: 0x04003B59 RID: 15193
	public const int CRAFT_MAX_DURATION = 18;

	// Token: 0x04003B5A RID: 15194
	public const int MAX_FIGHTER_CONTAINERS = 4;

	// Token: 0x04003B5B RID: 15195
	public const int MERCENARIES_AMOUNT = 4;

	// Token: 0x04003B5C RID: 15196
	public const string FIGHTBACK_PLAYER_SPAWN = "RT_fightback_player_spawn";

	// Token: 0x04003B5D RID: 15197
	public const float TALENT_COEFFICIENT_VALUE = 0.1f;

	// Token: 0x04003B5E RID: 15198
	public const float DOCK_SEARCH_RADIUS = 1.5f;

	// Token: 0x04003B5F RID: 15199
	public const float PLAYER_GRAPH_SIZE = 2.2f;

	// Token: 0x04003B60 RID: 15200
	public const float PLAYER_GRAPH_NODE_SIZE = 0.1f;

	// Token: 0x04003B61 RID: 15201
	public const float PLAYER_GRAPH_COLLIDER_DIAMETER = 0.28f;

	// Token: 0x04003B62 RID: 15202
	public const float MAGNETISM_DELAY_DURATION = 0.25f;

	// Token: 0x04003B63 RID: 15203
	public const float INTERACTIVE_DROPS_MAGNET_SPEED_K = 0.2f;

	// Token: 0x04003B64 RID: 15204
	public const float INTERACTIVE_DROPS_MAGNET_SPEED = 3f;

	// Token: 0x04003B65 RID: 15205
	public const float DROP_MOVEMENT_TO_POSITION_SPEED = 2f;

	// Token: 0x04003B66 RID: 15206
	public const float DROP_MIN_DISTANCE_TO_STOP = 0.1f;

	// Token: 0x04003B67 RID: 15207
	public const float DROP_MAX_DISTANCE_TO_STOP = 4f;

	// Token: 0x04003B68 RID: 15208
	public const float SLEEP_GAME_RES_DROP_COLLECT_DURATION = 1f;

	// Token: 0x04003B69 RID: 15209
	public const float PLAYER_DROP_OFFSET = 0.65f;

	// Token: 0x04003B6A RID: 15210
	public const float DROP_OFFSET_RIGHT = 0.3f;

	// Token: 0x04003B6B RID: 15211
	public const float DROP_OFFSET_FORWARD = 0.4f;

	// Token: 0x04003B6C RID: 15212
	public const float PLAYER_BACK_OFFSET = 0.5f;

	// Token: 0x04003B6D RID: 15213
	public const float PLAYER_COLLIDER_RADIUS = 0.22f;

	// Token: 0x04003B6E RID: 15214
	public const int DROP_ITEM_COUNT_LIMIT_TO_STACKING = 5;

	// Token: 0x04003B6F RID: 15215
	public const float TELEPORT_FADE_DELAY = 0.3f;

	// Token: 0x04003B70 RID: 15216
	public static Vector3 BIG_DROP_COLLIDER_SIZE = new Vector3(0.8f, 0.2f, 0.4f);

	// Token: 0x04003B71 RID: 15217
	public static readonly Vector3 OVERHEAD_STACK_OFFSET = new Vector3(0f, 0.3f, -0.04f);

	// Token: 0x04003B72 RID: 15218
	public const string EXTRA_OVERHEAD_KEY = "extra_overhead";

	// Token: 0x04003B73 RID: 15219
	public const float BIG_DROP_MAX_ELEVATION_DELTA = 1.5f;

	// Token: 0x04003B74 RID: 15220
	public const float DISTANCE_ANGLE_COMPENSTATION_COEFF = 0.0026666666f;

	// Token: 0x04003B75 RID: 15221
	public const string EXTRACT_ORGAN_CRAFT_PREFIX = "extract_";

	// Token: 0x04003B76 RID: 15222
	public const string INSERT_ORGAN_CRAFT_PREFIX = "insert_";

	// Token: 0x04003B77 RID: 15223
	public const string CHANGE_ORGAN_CRAFT_PREFIX = "change_";

	// Token: 0x04003B78 RID: 15224
	public const string EMBALM_CRAFT_PREFIX = "embalm_";

	// Token: 0x04003B79 RID: 15225
	public const string EXTRACT_ITEM_FROM_POCKET_CRAFT = "pocket_extract_item";

	// Token: 0x04003B7A RID: 15226
	public static List<ItemType> MAIN_ORGANS_TYPES = new List<ItemType>
	{
		ItemType.Bones,
		ItemType.Brain,
		ItemType.Heart,
		ItemType.Guts,
		ItemType.Skin,
		ItemType.Skull
	};

	// Token: 0x04003B7B RID: 15227
	public const string ZOMBIE_DEFAULT_COLLAR_ID = "collar_bronze";

	// Token: 0x04003B7C RID: 15228
	public const string MORGUE_PALLETS_GROUP = "morgue_pallets";

	// Token: 0x04003B7D RID: 15229
	public const string DONATION_BOX_SERMON_EVENT_ID = "sermon_reward";

	// Token: 0x04003B7E RID: 15230
	public const string PALETTE_TRADING_REWARD = "palette_trading_reward";

	// Token: 0x04003B7F RID: 15231
	public const string CASHBOX_TRADE_EVENT_ID = "cashbox_reward";

	// Token: 0x04003B80 RID: 15232
	public const string HAPPINESSBOX_EVENT_ID = "happinessbox_reward";

	// Token: 0x04003B81 RID: 15233
	public const string SERMON_READY = "sermon_ready";

	// Token: 0x04003B82 RID: 15234
	public const string CHURCH_CHOIR_WGO_ID = "zmb_choir_place";

	// Token: 0x04003B83 RID: 15235
	public const string CHURCH_ORGAN_WGO_ID = "zmb_organ_place";

	// Token: 0x04003B84 RID: 15236
	public const string PS5_ACTIVITY_ID = "continue";

	// Token: 0x04003B85 RID: 15237
	public const float MAX_WIND_SPEED = 1f;

	// Token: 0x04003B86 RID: 15238
	public const string CONVEYOR_WORLD_ZONE_ID = "conveyor";

	// Token: 0x04003B87 RID: 15239
	public static string NO_LUT = "NO LUT";

	// Token: 0x02000ADC RID: 2780
	public static class Layers
	{
		// Token: 0x04003B88 RID: 15240
		public const int DEFAULT = 0;

		// Token: 0x04003B89 RID: 15241
		public const int INTERACTABLE = 6;

		// Token: 0x04003B8A RID: 15242
		public const int DOCK_POINT = 7;

		// Token: 0x04003B8B RID: 15243
		public const int OBSTACLES = 8;

		// Token: 0x04003B8C RID: 15244
		public const int FAKE_LIGHTING_OBJECT = 9;

		// Token: 0x04003B8D RID: 15245
		public const int PLAYER = 10;

		// Token: 0x04003B8E RID: 15246
		public const int GROUND = 11;

		// Token: 0x04003B8F RID: 15247
		public const int STEP = 12;

		// Token: 0x04003B90 RID: 15248
		public const int CUSTOM_GRAVITY_FIELD = 13;

		// Token: 0x04003B91 RID: 15249
		public const int PLAYER_INVISIBLE_WALLS = 14;

		// Token: 0x04003B92 RID: 15250
		public const int TRANSPARENCY_OCCLUDER = 15;

		// Token: 0x04003B93 RID: 15251
		public const int DROP = 16;

		// Token: 0x04003B94 RID: 15252
		public const int WORLD_ZONE = 17;

		// Token: 0x04003B95 RID: 15253
		public const int SOUND_ZONE = 18;

		// Token: 0x04003B96 RID: 15254
		public const int BUILD_AREA = 19;

		// Token: 0x04003B97 RID: 15255
		public const int GD_ZONE = 23;

		// Token: 0x04003B98 RID: 15256
		public const int PLAYER_DROP = 24;

		// Token: 0x04003B99 RID: 15257
		public const int IGNORE_COLLISION_WITH_PLAYER = 26;

		// Token: 0x04003B9A RID: 15258
		public const int CONVEYOR_CONNECTOR = 27;

		// Token: 0x04003B9B RID: 15259
		public const int BUFF_AREA = 28;

		// Token: 0x04003B9C RID: 15260
		public const int FIGHTER = 29;

		// Token: 0x04003B9D RID: 15261
		public const int WATER = 4;

		// Token: 0x04003B9E RID: 15262
		public const int DEFAULT_MASK = 1;

		// Token: 0x04003B9F RID: 15263
		public const int INTERACTABLE_MASK = 64;

		// Token: 0x04003BA0 RID: 15264
		public const int DOCK_POINT_MASK = 128;

		// Token: 0x04003BA1 RID: 15265
		public const int OBSTACLES_MASK = 256;

		// Token: 0x04003BA2 RID: 15266
		public const int FAKE_LIGHTING_OBJECT_MASK = 512;

		// Token: 0x04003BA3 RID: 15267
		public const int GROUND_MASK = 2048;

		// Token: 0x04003BA4 RID: 15268
		public const int STEP_MASK = 4096;

		// Token: 0x04003BA5 RID: 15269
		public const int CUSTOM_GRAVITY_FIELD_MASK = 8192;

		// Token: 0x04003BA6 RID: 15270
		public const int TRANSPARENCY_OCCLUDER_MASK = 32768;

		// Token: 0x04003BA7 RID: 15271
		public const int DROP_MASK = 65536;

		// Token: 0x04003BA8 RID: 15272
		public const int PLAYER_MASK = 1024;

		// Token: 0x04003BA9 RID: 15273
		public const int PLAYER_DROP_MASK = 16777216;

		// Token: 0x04003BAA RID: 15274
		public const int WORLD_ZONE_MASK = 131072;

		// Token: 0x04003BAB RID: 15275
		public const int BUILD_AREA_MASK = 524288;

		// Token: 0x04003BAC RID: 15276
		public const int GD_ZONE_MASK = 8388608;

		// Token: 0x04003BAD RID: 15277
		public const int CONVEYOR_CONNECTOR_MASK = 134217728;

		// Token: 0x04003BAE RID: 15278
		public const int IGNORE_COLLISION_WITH_PLAYER_MASK = 67108864;

		// Token: 0x04003BAF RID: 15279
		public const int BUFF_AREA_MASK = 268435456;

		// Token: 0x04003BB0 RID: 15280
		public const int FIGHTER_MASK = 536870912;
	}

	// Token: 0x02000ADD RID: 2781
	public static class Tags
	{
		// Token: 0x04003BB1 RID: 15281
		public const string WATER = "Water";

		// Token: 0x04003BB2 RID: 15282
		public const string RECAST_FLOOR = "RecastFloor";
	}

	// Token: 0x02000ADE RID: 2782
	public static class WgoCustomTags
	{
		// Token: 0x04003BB3 RID: 15283
		public const string TOWN_BUILDING_TAG_POSTFIX_PART = "place_";

		// Token: 0x04003BB4 RID: 15284
		public const string TOWN_BUILDING_SIGNBOARD_TAG_PREFIX = "t_b_signboard_";

		// Token: 0x04003BB5 RID: 15285
		public const string TOWN_BUILDING_CHARACTER_TAG_PREFIX = "t_b_character_";

		// Token: 0x04003BB6 RID: 15286
		public const string TOWN_BUILDING_TENT_TAG_PREFIX = "t_b_tent_";

		// Token: 0x04003BB7 RID: 15287
		public const string TOWN_BUILDING_YARD_TAG_PREFIX = "t_b_yard_";

		// Token: 0x04003BB8 RID: 15288
		public const string TOWN_BUILDING_SIGN_TAG_PREFIX = "t_b_sign_";

		// Token: 0x04003BB9 RID: 15289
		public const string TOWN_BUILDING_DECOR_1_TAG_PREFIX = "t_b_decor_1_";

		// Token: 0x04003BBA RID: 15290
		public const string TOWN_BUILDING_DECOR_2_TAG_PREFIX = "t_b_decor_2_";

		// Token: 0x04003BBB RID: 15291
		public const string TOWN_BUILDING_DECOR_3_TAG_PREFIX = "t_b_decor_3_";

		// Token: 0x04003BBC RID: 15292
		public const string PANIC_REDUCTION_MACHINE_TAG = "panic_reduction_machine";
	}

	// Token: 0x02000ADF RID: 2783
	public static class SFX
	{
		// Token: 0x04003BBD RID: 15293
		public const string TOOL_AXE = "tool_axe";

		// Token: 0x04003BBE RID: 15294
		public const string TOOL_SHOVEL = "tool_shovel";

		// Token: 0x04003BBF RID: 15295
		public const string TOOL_PICKAXE = "tool_pickaxe";

		// Token: 0x04003BC0 RID: 15296
		public const string TOOL_HAMMER = "tool_hammer";

		// Token: 0x04003BC1 RID: 15297
		public const string TREE_FALL = "tree_fall";

		// Token: 0x04003BC2 RID: 15298
		public const string DOOR = "door";

		// Token: 0x04003BC3 RID: 15299
		public const string TECH_POINT = "tech_point_collect";

		// Token: 0x04003BC4 RID: 15300
		public const string TECH_POINT_FAILED = "tech_point_collect_failed";

		// Token: 0x04003BC5 RID: 15301
		public const string BELL = "donkey_bell";

		// Token: 0x04003BC6 RID: 15302
		public const string ITEM_PICKUP = "item_pickup";

		// Token: 0x04003BC7 RID: 15303
		public const string PLANTING = "planting";

		// Token: 0x04003BC8 RID: 15304
		public const string BAG_OPEN = "bag_open";

		// Token: 0x04003BC9 RID: 15305
		public const string BAG_CLOSE = "bag_close";

		// Token: 0x04003BCA RID: 15306
		public const string UNLOCK = "unlock";

		// Token: 0x04003BCB RID: 15307
		public const string EQUIP = "equip_tool";

		// Token: 0x04003BCC RID: 15308
		public const string UNEQUIP = "unequip_tool";

		// Token: 0x04003BCD RID: 15309
		public const string HOVER = "gui_hover";

		// Token: 0x04003BCE RID: 15310
		public const string HOVER_LIGHT = "gui_hover_light";

		// Token: 0x04003BCF RID: 15311
		public const string CLICK = "gui_click";

		// Token: 0x04003BD0 RID: 15312
		public const string ITEM_PUT = "item_put";

		// Token: 0x04003BD1 RID: 15313
		public const string COINS = "coins_sound";

		// Token: 0x04003BD2 RID: 15314
		public const string CLIMB_START = "ladder_climb_start";

		// Token: 0x04003BD3 RID: 15315
		public const string CLIMB_STEP = "ladder_climb";

		// Token: 0x04003BD4 RID: 15316
		public const string CLIMB_FINISH = "ladder_climb_finish";

		// Token: 0x04003BD5 RID: 15317
		public const string SERMON_SUCCESS = "sermon_success";

		// Token: 0x04003BD6 RID: 15318
		public const string SERMON_FAIL = "sermon_fail";

		// Token: 0x04003BD7 RID: 15319
		public const string PLAYER_ATTACK = "sword_attack";

		// Token: 0x04003BD8 RID: 15320
		public const string SWORD_HIT = "sword_hit";

		// Token: 0x04003BD9 RID: 15321
		public const string BOW_AIM_START = "bow_aim_start";

		// Token: 0x04003BDA RID: 15322
		public const string BOW_AIM_LOOP = "bow_aim_loop";

		// Token: 0x04003BDB RID: 15323
		public const string BOW_AIM_SHOT = "bow_aim_shot";

		// Token: 0x04003BDC RID: 15324
		public const string BOW_HIT = "bow_hit_zombie";

		// Token: 0x04003BDD RID: 15325
		public const string SPEAR_ATTACK = "spear_attack";

		// Token: 0x04003BDE RID: 15326
		public const string SPEAR_HIT = "spear_hit_zombie";

		// Token: 0x04003BDF RID: 15327
		public const string ZOMBIE_ATTACK = "zombie_attack";

		// Token: 0x04003BE0 RID: 15328
		public const string ZOMBIE_HIT_WOOD = "zombie_hit_wood";

		// Token: 0x04003BE1 RID: 15329
		public const string ZOMBIE_HIT = "zombie_hit_player";

		// Token: 0x04003BE2 RID: 15330
		public const string SPITTER_ATTACK = "spitter_attack";

		// Token: 0x04003BE3 RID: 15331
		public const string SPITTER_HIT = "spitter_hit";

		// Token: 0x04003BE4 RID: 15332
		public const string ZOMBIE_IDLE = "zombie_idle";

		// Token: 0x04003BE5 RID: 15333
		public const string FIGHT_LOSE = "fight_lose";

		// Token: 0x04003BE6 RID: 15334
		public const string FIGHT_WIN = "fight_win";

		// Token: 0x04003BE7 RID: 15335
		public const string FIGHT_START = "fight_start";

		// Token: 0x04003BE8 RID: 15336
		public const string WPN_HIT_DOOR_ZOMBIE = "hit_door_zombie";

		// Token: 0x04003BE9 RID: 15337
		public const string ARROW_HIT_DOOR_ZOMBIE = "arrow_hit_door_zombie";

		// Token: 0x04003BEA RID: 15338
		public const string ALLY_DEATH_HUMAN = "ally_death_human";

		// Token: 0x04003BEB RID: 15339
		public const string ALLY_DEATH_ZOMBIE = "ally_death_zombie";

		// Token: 0x04003BEC RID: 15340
		public const string ALLY_ARMOR_FOOTSTEPS = "ally_armor_footsteps";

		// Token: 0x04003BED RID: 15341
		public const string FISHING_START = "fishing_start";

		// Token: 0x04003BEE RID: 15342
		public const string FISHING_CAST = "fishing_cast_swoosh";

		// Token: 0x04003BEF RID: 15343
		public const string FISHING_BLOP = "fishing_blop";

		// Token: 0x04003BF0 RID: 15344
		public const string FISHING_BITE_SUCCESS = "fishing_bite_success";

		// Token: 0x04003BF1 RID: 15345
		public const string FISHING_LINE_BREAK = "fishing_line_break";

		// Token: 0x04003BF2 RID: 15346
		public const string FISHING_BITE = "fishing_bite";

		// Token: 0x04003BF3 RID: 15347
		public const string FISHING_SUCCESS = "fishing_success";

		// Token: 0x04003BF4 RID: 15348
		public const string FISHING_FAIL = "fishing_fail";

		// Token: 0x04003BF5 RID: 15349
		public const string FISHING_REEL_SHORT = "fishing_reel_short";

		// Token: 0x04003BF6 RID: 15350
		public const string FISHING_REEL_LONG = "fishing_reel_long";

		// Token: 0x04003BF7 RID: 15351
		public const string FISHING_SPLASHES = "fishing_floundering";

		// Token: 0x04003BF8 RID: 15352
		public const string RIVER_DUMP_WATER = "fishing_blop";

		// Token: 0x04003BF9 RID: 15353
		public const string OH_ZOMBIE_TAKE = "oh_zombie_grab";

		// Token: 0x04003BFA RID: 15354
		public const string OH_ZOMBIE_DROP = "oh_zombie_drop";

		// Token: 0x04003BFB RID: 15355
		public const string OH_CORPSE_TAKE = "oh_corpse_grab";

		// Token: 0x04003BFC RID: 15356
		public const string OH_CORPSE_DROP = "oh_corpse_drop";

		// Token: 0x04003BFD RID: 15357
		public const string OH_CORPSE_GRAVE_DROP = "oh_corpse_grave_drop";

		// Token: 0x04003BFE RID: 15358
		public const string OH_WOOD_TAKE = "oh_wood_grab";

		// Token: 0x04003BFF RID: 15359
		public const string OH_WOOD_DROP = "oh_wood_drop";

		// Token: 0x04003C00 RID: 15360
		public const string OH_WOOD_CONTAINER_DROP = "oh_wood_container_drop";

		// Token: 0x04003C01 RID: 15361
		public const string ZOMBIE_CHEST_RUMMAGE = "zombie_chest_rummage";

		// Token: 0x04003C02 RID: 15362
		public const string TAB_CLICK = "tab_click";

		// Token: 0x04003C03 RID: 15363
		public const string BLIMP_GROW = "blimp_grow";

		// Token: 0x04003C04 RID: 15364
		public const string BUILD_PLACE = "build_place";
	}

	// Token: 0x02000AE0 RID: 2784
	public static class Music
	{
		// Token: 0x04003C05 RID: 15365
		public const string MAIN_MENU = "main_menu";

		// Token: 0x04003C06 RID: 15366
		public const string GAMEPLAY = "gameplay";

		// Token: 0x04003C07 RID: 15367
		public const string FIGHT = "fight";

		// Token: 0x04003C08 RID: 15368
		public const string SEWER_FIGHT = "sewer_fight";
	}

	// Token: 0x02000AE1 RID: 2785
	public static class Perks
	{
		// Token: 0x04003C09 RID: 15369
		public const string LACK_OF_SLEEP_DEBUFF = "lack_of_sleep_debuff";

		// Token: 0x04003C0A RID: 15370
		public const string EXCESSIVE_ZOMBIE_DEBUFF = "debuff_excessive_zombie";
	}

	// Token: 0x02000AE2 RID: 2786
	public static class ConstDefs
	{
		// Token: 0x04003C0B RID: 15371
		public const string TOWN_BUY_COEFFICIENT = "town_buy_k";

		// Token: 0x04003C0C RID: 15372
		public const string PRODUCT_GROUP_1_COEFFICIENT = "product_group_1";

		// Token: 0x04003C0D RID: 15373
		public const string PRODUCT_GROUP_2_COEFFICIENT = "product_group_2";

		// Token: 0x04003C0E RID: 15374
		public const string PRODUCT_GROUP_3_COEFFICIENT = "product_group_3";

		// Token: 0x04003C0F RID: 15375
		public const string PRODUCT_GROUP_HAPPIINESS_1_COEFFICIENT = "product_group_happiness_1";

		// Token: 0x04003C10 RID: 15376
		public const string PRODUCT_GROUP_HAPPIINESS_2_COEFFICIENT = "product_group_happiness_2";

		// Token: 0x04003C11 RID: 15377
		public const string PRODUCT_GROUP_HAPPIINESS_3_COEFFICIENT = "product_group_happiness_3";

		// Token: 0x04003C12 RID: 15378
		public const string NEW_GAME_START_QUEST = "new_game_start_quest";

		// Token: 0x04003C13 RID: 15379
		public const string ZOMBIE_CRAFT_SUB_TICKS_COUNT = "zombie_craft_sub_ticks_count";

		// Token: 0x04003C14 RID: 15380
		public const string ZOMBIE_CARETAKER_PICKING_UP_TIME = "zombie_caretaker_picking_up_time";

		// Token: 0x04003C15 RID: 15381
		public const string CONVEYOR_SYSTEM_UPDATE_INTERVAL = "conveyor_system_update_interval";

		// Token: 0x04003C16 RID: 15382
		public const string BASE_PARISHIONER_CRIT_POWER = "base_parishioner_crit_power";

		// Token: 0x04003C17 RID: 15383
		public const string MAX_CRAFT_CELLS_PER_ONE_HIT = "max_cells_per_one_hit";

		// Token: 0x04003C18 RID: 15384
		public const string ENERGY_CRAFT_BORDER_1 = "energy_craft_border_1";

		// Token: 0x04003C19 RID: 15385
		public const string ENERGY_CRAFT_BORDER_2 = "energy_craft_border_2";

		// Token: 0x04003C1A RID: 15386
		public const string INSANITY_CRAFT_BORDER_1 = "insanity_craft_border_1";

		// Token: 0x04003C1B RID: 15387
		public const string INSANITY_CRAFT_BORDER_2 = "insanity_craft_border_2";

		// Token: 0x04003C1C RID: 15388
		public const string STAMINA_REGENERATION = "stamina_regeneration";

		// Token: 0x04003C1D RID: 15389
		public const string STAMINA_REGENERATION_STANCE = "stamina_regeneration_stance";

		// Token: 0x04003C1E RID: 15390
		public const string STAMINA_REGENERATION_DELAY = "stamina_regeneration_delay";

		// Token: 0x04003C1F RID: 15391
		public const string START_ORDERS_COUNT = "start_orders_count";

		// Token: 0x04003C20 RID: 15392
		public const string CORPSE_AUTO_DESTROY_TIMER = "corpse_auto_destroy_timer";

		// Token: 0x04003C21 RID: 15393
		public const string MIN_SPAWN_DISTANCE = "spawn_distance";

		// Token: 0x04003C22 RID: 15394
		public const string DAY_PRIDE = "day_pride";

		// Token: 0x04003C23 RID: 15395
		public const string DAY_LUST = "day_lust";

		// Token: 0x04003C24 RID: 15396
		public const string DAY_GLUTTONY = "day_gluttony";

		// Token: 0x04003C25 RID: 15397
		public const string DAY_ENVY = "day_envy";

		// Token: 0x04003C26 RID: 15398
		public const string DAY_WRATH = "day_wrath";

		// Token: 0x04003C27 RID: 15399
		public const string DAY_SLOTH = "day_sloth";

		// Token: 0x04003C28 RID: 15400
		public static string[] AllDays = new string[] { "day_pride", "day_lust", "day_gluttony", "day_envy", "day_wrath", "day_sloth" };
	}

	// Token: 0x02000AE3 RID: 2787
	public static class Fighting
	{
		// Token: 0x04003C29 RID: 15401
		public const int FIGHTING_STAGE_VISIBLE = 1;

		// Token: 0x04003C2A RID: 15402
		public const int FIGHTING_STAGE_AVAILABLE_FOR_START = 2;

		// Token: 0x04003C2B RID: 15403
		public const int FIGHTING_STAGE_PRE_FIGHT = 3;

		// Token: 0x04003C2C RID: 15404
		public const int FIGHTING_STAGE_FIGHT = 4;

		// Token: 0x04003C2D RID: 15405
		public const int FIGHTING_STAGE_FIGHT_WIN = 5;

		// Token: 0x04003C2E RID: 15406
		public const int FIGHTING_STAGE_FINAL = 6;

		// Token: 0x04003C2F RID: 15407
		public const string DEV_GENERIC_FIGHTERS_TIER_RES_ID = "dev_generic_fighters_tier";

		// Token: 0x04003C30 RID: 15408
		public const float MAIN_HERO_RVO_PRIORITY = 0.85f;

		// Token: 0x04003C31 RID: 15409
		public const float MAIN_HERO_PUSH_ALLY_RVO_PRIORITY = 0.15f;

		// Token: 0x04003C32 RID: 15410
		public const float MAIN_HERO_PUSH_HOLD_TIME = 0.35f;

		// Token: 0x02000AE4 RID: 2788
		public enum TeamType
		{
			// Token: 0x04003C34 RID: 15412
			Player,
			// Token: 0x04003C35 RID: 15413
			WildZombie
		}

		// Token: 0x02000AE5 RID: 2789
		public enum TargetAttackPriority
		{
			// Token: 0x04003C37 RID: 15415
			Low,
			// Token: 0x04003C38 RID: 15416
			Medium = 10,
			// Token: 0x04003C39 RID: 15417
			High = 20,
			// Token: 0x04003C3A RID: 15418
			Critical = 30
		}

		// Token: 0x02000AE6 RID: 2790
		[Flags]
		public enum EntityType
		{
			// Token: 0x04003C3C RID: 15420
			None = 0,
			// Token: 0x04003C3D RID: 15421
			Player = 1,
			// Token: 0x04003C3E RID: 15422
			Soldier = 2,
			// Token: 0x04003C3F RID: 15423
			Zombie = 4
		}
	}

	// Token: 0x02000AE7 RID: 2791
	public static class Navigation
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004ACB RID: 19147 RVA: 0x00160EB4 File Offset: 0x0015F0B4
		public static global::Pathfinding.GraphMask GraphMaskFromAllRecastGraphs
		{
			get
			{
				global::Pathfinding.GraphMask graphMask = default(global::Pathfinding.GraphMask);
				foreach (NavGraph navGraph in AstarPath.active.graphs)
				{
					if (navGraph is RecastGraph)
					{
						graphMask |= new global::Pathfinding.GraphMask(1U << (int)navGraph.graphIndex);
					}
				}
				return graphMask;
			}
		}

		// Token: 0x02000AE8 RID: 2792
		public enum Graph
		{
			// Token: 0x04003C41 RID: 15425
			None = -1,
			// Token: 0x04003C42 RID: 15426
			PortArea = 1,
			// Token: 0x04003C43 RID: 15427
			DriedGatewayArea = 4,
			// Token: 0x04003C44 RID: 15428
			GdPointGraph = 2,
			// Token: 0x04003C45 RID: 15429
			PlayerGraph,
			// Token: 0x04003C46 RID: 15430
			DevPlayground = 5,
			// Token: 0x04003C47 RID: 15431
			Church,
			// Token: 0x04003C48 RID: 15432
			ZombieFighters,
			// Token: 0x04003C49 RID: 15433
			WZYard,
			// Token: 0x04003C4A RID: 15434
			TestZombieZone,
			// Token: 0x04003C4B RID: 15435
			VillageForestArea,
			// Token: 0x04003C4C RID: 15436
			RuinedTemple,
			// Token: 0x04003C4D RID: 15437
			Fighting_Recast,
			// Token: 0x04003C4E RID: 15438
			Carrier,
			// Token: 0x04003C4F RID: 15439
			Sawmill,
			// Token: 0x04003C50 RID: 15440
			Mine,
			// Token: 0x04003C51 RID: 15441
			SandClay,
			// Token: 0x04003C52 RID: 15442
			Garden,
			// Token: 0x04003C53 RID: 15443
			Vineyard,
			// Token: 0x04003C54 RID: 15444
			Conveyors,
			// Token: 0x04003C55 RID: 15445
			VineyardBasement,
			// Token: 0x04003C56 RID: 15446
			BasementWriting,
			// Token: 0x04003C57 RID: 15447
			AlchemyLab,
			// Token: 0x04003C58 RID: 15448
			PlayerHouse
		}

		// Token: 0x02000AE9 RID: 2793
		public enum GraphMask
		{
			// Token: 0x04003C5A RID: 15450
			PortAreaMask = 2,
			// Token: 0x04003C5B RID: 15451
			DriedGatewayAreaMask = 16,
			// Token: 0x04003C5C RID: 15452
			GdPointGraphAreaMask = 4,
			// Token: 0x04003C5D RID: 15453
			PlayerGraphMask = 8,
			// Token: 0x04003C5E RID: 15454
			DevPlaygroundMask = 32,
			// Token: 0x04003C5F RID: 15455
			VillageForesAreaMask = 1024,
			// Token: 0x04003C60 RID: 15456
			ChurchMask = 64,
			// Token: 0x04003C61 RID: 15457
			ZombieFightersMask = 128,
			// Token: 0x04003C62 RID: 15458
			WZYardMask = 256,
			// Token: 0x04003C63 RID: 15459
			TestZombieZoneMask = 512,
			// Token: 0x04003C64 RID: 15460
			RuinedTempleMask = 2048,
			// Token: 0x04003C65 RID: 15461
			Fighting_RecastMask = 4096
		}
	}

	// Token: 0x02000AEA RID: 2794
	public static class MovementComponent
	{
		// Token: 0x04003C66 RID: 15462
		public const float SPEED_DEFAULT = 1.5f;

		// Token: 0x04003C67 RID: 15463
		public const float NPC_SIM_SPEED_DEFAULT = 1.125f;
	}

	// Token: 0x02000AEB RID: 2795
	public static class WorldZones
	{
		// Token: 0x04003C68 RID: 15464
		public const string MORGUE = "morgue";

		// Token: 0x04003C69 RID: 15465
		public const string RESURRECTION = "resurrection";
	}
}
