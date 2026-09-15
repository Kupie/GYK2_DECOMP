using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000221 RID: 545
[Serializable]
public class WGODef : BalanceBaseObject
{
	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00040DFC File Offset: 0x0003EFFC
	public ItemDef FuelItemDef
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.fuelItemId);
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00040E0E File Offset: 0x0003F00E
	public ItemDef FuelItemDef2
	{
		get
		{
			return GameBalance.Me.GetData<ItemDef>(this.fuelItemId2);
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000CEB RID: 3307 RVA: 0x00040E20 File Offset: 0x0003F020
	public Sprite Portrait
	{
		get
		{
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.portrait, "portrait_icon_hero");
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000CEC RID: 3308 RVA: 0x00040E37 File Offset: 0x0003F037
	public bool OpenInMultiInventory
	{
		get
		{
			return this.openInMultiInventory;
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000CED RID: 3309 RVA: 0x00040E3F File Offset: 0x0003F03F
	public int MasteryLock
	{
		get
		{
			if (!this.masteryLock.HasExpression)
			{
				return 1;
			}
			return this.masteryLock.EvaluateInt();
		}
	}

	// Token: 0x06000CEE RID: 3310 RVA: 0x00040E5C File Offset: 0x0003F05C
	public string ResolveAssetId(string fallbackId, WgoData wgoData = null)
	{
		if (!this.hasCustomAssetId)
		{
			return fallbackId;
		}
		string text = ((wgoData != null) ? this.customAssetId.Evaluate(wgoData) : this.customAssetId.Evaluate());
		if (!string.IsNullOrEmpty(text))
		{
			return text;
		}
		return fallbackId;
	}

	// Token: 0x06000CEF RID: 3311 RVA: 0x00040E9C File Offset: 0x0003F09C
	public bool TryGetBuildingDefForWgo(out BuildingDef buildingDef)
	{
		buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.id + "_p");
		if (buildingDef == null)
		{
			buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.id + "_s");
			if (buildingDef == null)
			{
				buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.id + "_cp");
				if (buildingDef == null)
				{
					buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.id + "_fp");
					if (buildingDef == null)
					{
						buildingDef = GameBalance.Me.GetDataOrNull<BuildingDef>(this.id + "_fp");
					}
				}
			}
		}
		return buildingDef != null;
	}

	// Token: 0x04000F7B RID: 3963
	[AutoParse("custom_visual_id")]
	public string customVisualId;

	// Token: 0x04000F7C RID: 3964
	[AutoParse("custom_asset_id")]
	[LazyExpressionPureValueType(PureValueType.String)]
	public LazyExpression customAssetId = new LazyExpression();

	// Token: 0x04000F7D RID: 3965
	[AutoParse("zombie_roll_data_id")]
	public string zombieRollDataId;

	// Token: 0x04000F7E RID: 3966
	[AutoParse("interaction_type")]
	public WGODef.InteractionType interactionType;

	// Token: 0x04000F7F RID: 3967
	public CustomInteraction customInteraction = new CustomInteraction();

	// Token: 0x04000F80 RID: 3968
	public CustomInteraction customInteraction2 = new CustomInteraction();

	// Token: 0x04000F81 RID: 3969
	[AutoParse("autocraft_tick_duration")]
	public LazyExpression autocraftTickDuration = new LazyExpression();

	// Token: 0x04000F82 RID: 3970
	[AutoParse("attached_workbenches")]
	public List<string> attachedWorkbenchExtensionIds = new List<string>();

	// Token: 0x04000F83 RID: 3971
	[AutoParse("wgo_group")]
	public string wgoGroup;

	// Token: 0x04000F84 RID: 3972
	[AutoParse("attached_script")]
	public string attachedScript;

	// Token: 0x04000F85 RID: 3973
	[SerializeField]
	public List<string> teleportDestinationWgoIds = new List<string>();

	// Token: 0x04000F86 RID: 3974
	public ToolAction toolAction = new ToolAction();

	// Token: 0x04000F87 RID: 3975
	[AutoParse("inventory_size")]
	public int inventorySize;

	// Token: 0x04000F88 RID: 3976
	[SerializeField]
	[AutoParse("start_items")]
	public List<NeedItemData> startItems = new List<NeedItemData>();

	// Token: 0x04000F89 RID: 3977
	public WhiteListItemFilter inventoryWhiteList = new WhiteListItemFilter();

	// Token: 0x04000F8A RID: 3978
	public BlackListItemFilter inventoryBlackList = new BlackListItemFilter();

	// Token: 0x04000F8B RID: 3979
	[AutoParse("empty_cell_stack_count")]
	public int emptyCellStackCount = 1;

	// Token: 0x04000F8C RID: 3980
	[AutoParse("craft_inventory_size")]
	public int craftInventorySize;

	// Token: 0x04000F8D RID: 3981
	[AutoParse("hp")]
	public int hp;

	// Token: 0x04000F8E RID: 3982
	[AutoParse("start_hp_val")]
	public int startHpValue = -1;

	// Token: 0x04000F8F RID: 3983
	[AutoParse("run_le_on_hp_reached")]
	public HPAction hpAction = new HPAction();

	// Token: 0x04000F90 RID: 3984
	[AutoParse("player_hp_activity_mod")]
	public int playerHpActivityMod = 1;

	// Token: 0x04000F91 RID: 3985
	[AutoParse("dev_inf_hp")]
	public bool hasInfiniteHp;

	// Token: 0x04000F92 RID: 3986
	[AutoParse("mastery_lock")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression masteryLock = new LazyExpression();

	// Token: 0x04000F93 RID: 3987
	public bool noMasteryLock;

	// Token: 0x04000F94 RID: 3988
	[AutoParse("energy")]
	public LazyExpression energyPerTick;

	// Token: 0x04000F95 RID: 3989
	[AutoParse("insanity")]
	public LazyExpression insanityPerTick;

	// Token: 0x04000F96 RID: 3990
	public string worldFxOnHpActionTick;

	// Token: 0x04000F97 RID: 3991
	[AutoParse("sfx_on_action_tick")]
	public string sfxOnActionTick;

	// Token: 0x04000F98 RID: 3992
	public Vector3 worldFxOnHpActionSize = Vector3.one;

	// Token: 0x04000F99 RID: 3993
	public string worldFxOnHpFirstHit;

	// Token: 0x04000F9A RID: 3994
	public Vector3 worldFxOnHpFirstHitActionSize = Vector3.one;

	// Token: 0x04000F9B RID: 3995
	[AutoParse("replace_to_wgo_on_die")]
	[LazyExpressionPureValueType(PureValueType.String)]
	public LazyExpression replaceToWgoOnDie = new LazyExpression();

	// Token: 0x04000F9C RID: 3996
	[AutoParse("revive_on_die")]
	public bool reviveOnDie;

	// Token: 0x04000F9D RID: 3997
	[AutoParse("transfer_data")]
	public bool transferDataToNewWgo;

	// Token: 0x04000F9E RID: 3998
	[AutoParse("drop_res_on_die")]
	public OutputItems deathChanceItems;

	// Token: 0x04000F9F RID: 3999
	[AutoParse("drop_inventory_on_death")]
	public bool dropInventoryOnDeath;

	// Token: 0x04000FA0 RID: 4000
	[AutoParse("expression_on_die")]
	public List<LazyExpression> executeOnDeath = new List<LazyExpression>();

	// Token: 0x04000FA1 RID: 4001
	[AutoParse("expression_on_replace")]
	public List<LazyExpression> executeOnReplace = new List<LazyExpression>();

	// Token: 0x04000FA2 RID: 4002
	[AutoParse("rep_gameres")]
	public string repResName;

	// Token: 0x04000FA3 RID: 4003
	[AutoParse("portrait_asset_id")]
	public string portrait;

	// Token: 0x04000FA4 RID: 4004
	[AutoParse("use_portrait_in_dialogues")]
	public bool usePortraitInDialogues;

	// Token: 0x04000FA5 RID: 4005
	[AutoParse("tech_r")]
	public int techRed;

	// Token: 0x04000FA6 RID: 4006
	[AutoParse("tech_g")]
	public int techGreen;

	// Token: 0x04000FA7 RID: 4007
	[AutoParse("tech_b")]
	public int techBlue;

	// Token: 0x04000FA8 RID: 4008
	[AutoParse("inspiration_on_die")]
	public LazyExpression inspirationOnDeath = new LazyExpression();

	// Token: 0x04000FA9 RID: 4009
	[AutoParse("talent")]
	public string talent;

	// Token: 0x04000FAA RID: 4010
	public string worldFxOnDie;

	// Token: 0x04000FAB RID: 4011
	[AutoParse("sfx_on_die")]
	public string sfxOnDie;

	// Token: 0x04000FAC RID: 4012
	public bool getFxOnDieSizeFromBuildCollider;

	// Token: 0x04000FAD RID: 4013
	public Vector3 fxOnDieSize = Vector3.one;

	// Token: 0x04000FAE RID: 4014
	public float customDeathTime = -1f;

	// Token: 0x04000FAF RID: 4015
	[AutoParse("quality_display_type")]
	public WGODef.QualityDisplayType qualityDisplayType;

	// Token: 0x04000FB0 RID: 4016
	[AutoParse("quality")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression quality;

	// Token: 0x04000FB1 RID: 4017
	[AutoParse("town_quality")]
	public int townQuality;

	// Token: 0x04000FB2 RID: 4018
	[AutoParse("add_inventory_quality")]
	public bool considerInventoryQuality;

	// Token: 0x04000FB3 RID: 4019
	[AutoParse("dev_label")]
	public string devLabelStr;

	// Token: 0x04000FB4 RID: 4020
	[AutoParse("open_in_multi_inventory")]
	[SerializeField]
	private bool openInMultiInventory;

	// Token: 0x04000FB5 RID: 4021
	[AutoParse("movable")]
	public bool isMovable;

	// Token: 0x04000FB6 RID: 4022
	[AutoParse("can_insert_zombie")]
	public bool canInsertZombie;

	// Token: 0x04000FB7 RID: 4023
	[AutoParse("force_make_nav_hole")]
	public WGODef.ForceSetNavigationHoleType forceSetNavigationHoleType = WGODef.ForceSetNavigationHoleType.InsideWorldZone;

	// Token: 0x04000FB8 RID: 4024
	[AutoParse("conveyor_type")]
	public ConveyorElementType conveyorType;

	// Token: 0x04000FB9 RID: 4025
	[AutoParse("conveyor_connectors_setup")]
	public ConveyorConnectorsSetup conveyorConnectorsSetup = new ConveyorConnectorsSetup();

	// Token: 0x04000FBA RID: 4026
	[AutoParse("npc_life_sim_group")]
	public string npcLifeSimGroup;

	// Token: 0x04000FBB RID: 4027
	[AutoParse("npc_life_sim_home")]
	public string npcLifeSimHome;

	// Token: 0x04000FBC RID: 4028
	[AutoParse("fuel_item_id")]
	public string fuelItemId;

	// Token: 0x04000FBD RID: 4029
	[AutoParse("fuel_item_id2")]
	public string fuelItemId2;

	// Token: 0x04000FBE RID: 4030
	[AutoParse("ref_to_other_wgo_inventory")]
	public string refToOtherWgoInventory;

	// Token: 0x04000FBF RID: 4031
	[AutoParse("craft_icon")]
	public string craftIconId;

	// Token: 0x04000FC0 RID: 4032
	[AutoParse("craft_icon_color")]
	public int craftIconColor;

	// Token: 0x04000FC1 RID: 4033
	public bool isAutoCrafter;

	// Token: 0x04000FC2 RID: 4034
	public bool hasCustomVisualId;

	// Token: 0x04000FC3 RID: 4035
	public bool hasCustomAssetId;

	// Token: 0x04000FC4 RID: 4036
	public bool isFuelContainer;

	// Token: 0x04000FC5 RID: 4037
	public bool hasRefToOtherWgoInventory;

	// Token: 0x04000FC6 RID: 4038
	public VoiceID voiceId;

	// Token: 0x02000222 RID: 546
	public enum InteractionType
	{
		// Token: 0x04000FC8 RID: 4040
		None,
		// Token: 0x04000FC9 RID: 4041
		Work,
		// Token: 0x04000FCA RID: 4042
		Craft,
		// Token: 0x04000FCB RID: 4043
		Script,
		// Token: 0x04000FCC RID: 4044
		CustomInteraction,
		// Token: 0x04000FCD RID: 4045
		Builder,
		// Token: 0x04000FCE RID: 4046
		Chest,
		// Token: 0x04000FCF RID: 4047
		Grave,
		// Token: 0x04000FD0 RID: 4048
		Ladder,
		// Token: 0x04000FD1 RID: 4049
		PrayerStand = 10,
		// Token: 0x04000FD2 RID: 4050
		Autopsy,
		// Token: 0x04000FD3 RID: 4051
		Garden = 13,
		// Token: 0x04000FD4 RID: 4052
		Zombie = 15,
		// Token: 0x04000FD5 RID: 4053
		Survey = 17,
		// Token: 0x04000FD6 RID: 4054
		Alchemy,
		// Token: 0x04000FD7 RID: 4055
		Reservoir,
		// Token: 0x04000FD8 RID: 4056
		Barricade,
		// Token: 0x04000FD9 RID: 4057
		Flag,
		// Token: 0x04000FDA RID: 4058
		Station,
		// Token: 0x04000FDB RID: 4059
		TownBuildingPlace,
		// Token: 0x04000FDC RID: 4060
		ConveyorCell,
		// Token: 0x04000FDD RID: 4061
		PowerSource,
		// Token: 0x04000FDE RID: 4062
		TakeAll,
		// Token: 0x04000FDF RID: 4063
		FlagStand,
		// Token: 0x04000FE0 RID: 4064
		Embalm,
		// Token: 0x04000FE1 RID: 4065
		FighterContainer,
		// Token: 0x04000FE2 RID: 4066
		FightBuilder,
		// Token: 0x04000FE3 RID: 4067
		TownPalette,
		// Token: 0x04000FE4 RID: 4068
		ChoirPlace,
		// Token: 0x04000FE5 RID: 4069
		ZombieCarrier,
		// Token: 0x04000FE6 RID: 4070
		ZombieSawmill,
		// Token: 0x04000FE7 RID: 4071
		TeleportMilestone,
		// Token: 0x04000FE8 RID: 4072
		PorterStation,
		// Token: 0x04000FE9 RID: 4073
		ZombieMine,
		// Token: 0x04000FEA RID: 4074
		ZombieClay,
		// Token: 0x04000FEB RID: 4075
		ZombieSand,
		// Token: 0x04000FEC RID: 4076
		GardenStation,
		// Token: 0x04000FED RID: 4077
		CargoLift,
		// Token: 0x04000FEE RID: 4078
		Crematorium,
		// Token: 0x04000FEF RID: 4079
		ConveyorTransporterStation,
		// Token: 0x04000FF0 RID: 4080
		PanicReductionMachine,
		// Token: 0x04000FF1 RID: 4081
		ResurrectionTable,
		// Token: 0x04000FF2 RID: 4082
		WellUpgrade,
		// Token: 0x04000FF3 RID: 4083
		RiverDump
	}

	// Token: 0x02000223 RID: 547
	public enum QualityDisplayType
	{
		// Token: 0x04000FF5 RID: 4085
		Hidden,
		// Token: 0x04000FF6 RID: 4086
		Show
	}

	// Token: 0x02000224 RID: 548
	public enum ForceSetNavigationHoleType
	{
		// Token: 0x04000FF8 RID: 4088
		InsideWorldZone = -1,
		// Token: 0x04000FF9 RID: 4089
		DontSpawnHole,
		// Token: 0x04000FFA RID: 4090
		SpawnHoleForce
	}
}
