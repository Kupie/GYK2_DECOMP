using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000217 RID: 535
[Serializable]
public class TownBuildingDef : BalanceBaseObject
{
	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00040AF3 File Offset: 0x0003ECF3
	public string BuildResultIcon
	{
		get
		{
			if (!string.IsNullOrEmpty(this.iconId))
			{
				return this.iconId;
			}
			return "i_b_blueprint_placeholder";
		}
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x0003CD40 File Offset: 0x0003AF40
	public string GetHeader()
	{
		return LLBase.L(this.id);
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x0003CD4D File Offset: 0x0003AF4D
	public string GetHeaderPrefix()
	{
		return LLBase.L("ui_blueprint");
	}

	// Token: 0x04000F4D RID: 3917
	[AutoParse("crafts_in")]
	public List<string> craftsIn = new List<string>();

	// Token: 0x04000F4E RID: 3918
	[AutoParse("building_type")]
	public TownBuildingType townBuildingType;

	// Token: 0x04000F4F RID: 3919
	[AutoParse("is_locked")]
	public bool isNeedsUnlock;

	// Token: 0x04000F50 RID: 3920
	[AutoParse("building_cost")]
	public List<NeedItemData> needItems = new List<NeedItemData>();

	// Token: 0x04000F51 RID: 3921
	[AutoParse("execute_on_building_finished")]
	public List<LazyExpression> onCraftEndExpressions = new List<LazyExpression>();

	// Token: 0x04000F52 RID: 3922
	[AutoParse("variation_id")]
	public string variationId;

	// Token: 0x04000F53 RID: 3923
	[AutoParse("icon_id")]
	public string iconId;

	// Token: 0x04000F54 RID: 3924
	[AutoParse("vendor_id")]
	public string vendorId;

	// Token: 0x04000F55 RID: 3925
	[AutoParse("lvl_up_building_id")]
	public string lvlUpId;

	// Token: 0x04000F56 RID: 3926
	[AutoParse("character_id")]
	public string characterId;

	// Token: 0x04000F57 RID: 3927
	[AutoParse("expression_on_char")]
	public List<LazyExpression> expressionOnCharCreate = new List<LazyExpression>();

	// Token: 0x04000F58 RID: 3928
	[AutoParse("dont_place_building_on_wgo")]
	public bool dontPlaceBuildingOnWgo;

	// Token: 0x04000F59 RID: 3929
	[AutoParse("upgrade_requirements")]
	public List<ExpressionGameRes> upgradeRequirements = new List<ExpressionGameRes>();

	// Token: 0x04000F5A RID: 3930
	[AutoParse("drop_items_on_building_finished")]
	public OutputItems dropItemsOnBuildingFinished = new OutputItems();
}
