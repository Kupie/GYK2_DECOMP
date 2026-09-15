using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001DC RID: 476
[Serializable]
public class BuildingDef : BalanceBaseObject
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0003CCD1 File Offset: 0x0003AED1
	public bool HasLimits
	{
		get
		{
			return this.limitMax > 0;
		}
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x0003CCDC File Offset: 0x0003AEDC
	public bool IsAlwaysBlockedByWgoGroup(string wgoGroup)
	{
		return !string.IsNullOrEmpty(wgoGroup) && this.alwaysBlockedByWgoGroups.Count > 0 && this.alwaysBlockedByWgoGroups.Contains(wgoGroup);
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x0003CD02 File Offset: 0x0003AF02
	public bool ShouldIgnoreWgoGroupAsObstacle(string wgoGroup)
	{
		return !this.IsAlwaysBlockedByWgoGroup(wgoGroup) && !string.IsNullOrEmpty(wgoGroup) && this.excludeWgoGroups.Contains(wgoGroup);
	}

	// Token: 0x17000207 RID: 519
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x0003CD25 File Offset: 0x0003AF25
	public string BuildResultIcon
	{
		get
		{
			if (!string.IsNullOrEmpty(this.buildResultIcon))
			{
				return this.buildResultIcon;
			}
			return "i_b_blueprint_placeholder";
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x0003CD40 File Offset: 0x0003AF40
	public string GetHeader()
	{
		return LLBase.L(this.id);
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x0003CD4D File Offset: 0x0003AF4D
	public string GetHeaderPrefix()
	{
		return LLBase.L("ui_blueprint");
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x0003CD5C File Offset: 0x0003AF5C
	public static List<BuildData> GetBuildingsInBuilder(Wgo builder)
	{
		bool flag = false;
		bool flag2 = false;
		if (builder != null && builder.Data.Definition.interactionType == WGODef.InteractionType.FightBuilder && LazySingleton<FightingGameController>.Instance.CurrentFightState != FightState.Disabled)
		{
			FightDef data = GameBalance.Me.GetData<FightDef>(LazySingleton<FightingGameController>.Instance.CurrentLevel.id);
			flag = data.isBarricadesUnavailable;
			flag2 = data.isTowersUnavailable;
		}
		List<BuildData> list = new List<BuildData>();
		foreach (BuildingDef buildingDef in GameBalance.Me.buildDefsInBuilder[builder.Id])
		{
			BuildingDef.BuildingMode buildingMode = buildingDef.buildingMode;
			if (buildingMode != BuildingDef.BuildingMode.None && buildingMode != BuildingDef.BuildingMode.Remove && (!buildingDef.isNeedsUnlock || MainGame.Instance.GameSave.knowledgeSystem.unlockedBuildings.Contains(buildingDef.id)) && !MainGame.Instance.GameSave.knowledgeSystem.lockedBuildings.Contains(buildingDef.id) && (!flag || !GameBalance.Me.HasWgoIdByGroup("barricades", buildingDef.wgoId)) && (!flag2 || !GameBalance.Me.HasWgoIdByGroup("towers", buildingDef.wgoId)))
			{
				list.Add(BuildData.GetDataForBuild(buildingDef));
			}
		}
		return list;
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x0003CEC0 File Offset: 0x0003B0C0
	public string GetLimitsString()
	{
		return string.Format("{0}/{1}", this.currentLimitExpression.EvaluateInt(), this.limitMax);
	}

	// Token: 0x04000D35 RID: 3381
	public string wgoId;

	// Token: 0x04000D36 RID: 3382
	public BuildingDef.BuildingMode buildingMode;

	// Token: 0x04000D37 RID: 3383
	[AutoParse("instant_destroy")]
	public bool deleteInstantly;

	// Token: 0x04000D38 RID: 3384
	[AutoParse("tab")]
	public string tab;

	// Token: 0x04000D39 RID: 3385
	[AutoParse("start_craft")]
	public string startCraft = "destroy_basic";

	// Token: 0x04000D3A RID: 3386
	[AutoParse("is_needs_unlock")]
	public bool isNeedsUnlock;

	// Token: 0x04000D3B RID: 3387
	[AutoParse("custom_grid_step")]
	public int customGridStep = 1;

	// Token: 0x04000D3C RID: 3388
	public BuildingDef.BuildAreaChoosingType chooseCustomBuildAreaType;

	// Token: 0x04000D3D RID: 3389
	[AutoParse("custom_build_area_id")]
	public string customBuildAreaId;

	// Token: 0x04000D3E RID: 3390
	[AutoParse("exclude_wgo_groups")]
	public List<string> excludeWgoGroups = new List<string>();

	// Token: 0x04000D3F RID: 3391
	[SerializeField]
	[AutoParse("icon_id")]
	private string buildResultIcon;

	// Token: 0x04000D40 RID: 3392
	[AutoParse("custom_wgo_p_preview")]
	public string customWgoPlacePreview;

	// Token: 0x04000D41 RID: 3393
	[AutoParse("builds_in")]
	public List<string> buildsIn = new List<string>();

	// Token: 0x04000D42 RID: 3394
	[AutoParse("need_item")]
	public List<NeedItemData> needItems = new List<NeedItemData>();

	// Token: 0x04000D43 RID: 3395
	[AutoParse("out_item")]
	public OutputItems outputItems = new OutputItems();

	// Token: 0x04000D44 RID: 3396
	[AutoParse("exec_expressions_after_building")]
	public List<LazyExpression> expressionAfterBuilding = new List<LazyExpression>();

	// Token: 0x04000D45 RID: 3397
	[AutoParse("current_limit_expression")]
	[LazyExpressionPureValueType(PureValueType.Float)]
	public LazyExpression currentLimitExpression = new LazyExpression();

	// Token: 0x04000D46 RID: 3398
	[AutoParse("limit_max")]
	public int limitMax;

	// Token: 0x04000D47 RID: 3399
	[AutoParse("always_blocked_by_wgo_groups")]
	public List<string> alwaysBlockedByWgoGroups = new List<string>();

	// Token: 0x04000D48 RID: 3400
	public int fullCoveringCount;

	// Token: 0x020001DD RID: 477
	public enum BuildingMode
	{
		// Token: 0x04000D4A RID: 3402
		None,
		// Token: 0x04000D4B RID: 3403
		Place,
		// Token: 0x04000D4C RID: 3404
		Remove,
		// Token: 0x04000D4D RID: 3405
		ConveyorPlace,
		// Token: 0x04000D4E RID: 3406
		FightingPlace,
		// Token: 0x04000D4F RID: 3407
		FightBuilding,
		// Token: 0x04000D50 RID: 3408
		Script,
		// Token: 0x04000D51 RID: 3409
		Upgrade
	}

	// Token: 0x020001DE RID: 478
	public enum BuildAreaChoosingType
	{
		// Token: 0x04000D53 RID: 3411
		None = -1,
		// Token: 0x04000D54 RID: 3412
		Soft,
		// Token: 0x04000D55 RID: 3413
		Strict,
		// Token: 0x04000D56 RID: 3414
		FullCoverWithCount,
		// Token: 0x04000D57 RID: 3415
		FullCoverSoft
	}
}
