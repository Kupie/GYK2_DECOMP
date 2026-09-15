using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000227 RID: 551
[Serializable]
public class WorldZoneDef : BalanceBaseObject
{
	// Token: 0x04000FFE RID: 4094
	[AutoParse("builder_id")]
	public string builderId;

	// Token: 0x04000FFF RID: 4095
	[AutoParse("has_custom_quality_zones")]
	public bool hasCustomQualityZones;

	// Token: 0x04001000 RID: 4096
	[AutoParse("display_type")]
	public WorldZoneDef.DisplayType displayType;

	// Token: 0x04001001 RID: 4097
	[AutoParse("quality_icon")]
	public string qualityIcon;

	// Token: 0x04001002 RID: 4098
	[AutoParse("build_desk_icon")]
	public string buildDeskIcon;

	// Token: 0x04001003 RID: 4099
	[AutoParse("string_format")]
	public string stringFormat;

	// Token: 0x04001004 RID: 4100
	[AutoParse("porter_start_point")]
	public string porterStartPoint;

	// Token: 0x04001005 RID: 4101
	[AutoParse("porter_end_point")]
	public string porterEndPoint;

	// Token: 0x04001006 RID: 4102
	[AutoParse("on_enter_expressions")]
	public List<LazyExpression> onEnterExpressions = new List<LazyExpression>();

	// Token: 0x04001007 RID: 4103
	[AutoParse("on_exit_expressions")]
	public List<LazyExpression> onExitExpressions = new List<LazyExpression>();

	// Token: 0x04001008 RID: 4104
	[AutoParse("expressions_on_max_quality_increased")]
	public List<LazyExpression> expressionsOnMaxQualityIncreased = new List<LazyExpression>();

	// Token: 0x02000228 RID: 552
	public enum DisplayType
	{
		// Token: 0x0400100A RID: 4106
		Hidden = -1,
		// Token: 0x0400100B RID: 4107
		None,
		// Token: 0x0400100C RID: 4108
		Sum
	}
}
