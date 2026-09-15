using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x0200020C RID: 524
[Serializable]
public class TalentExpLevelDef : BalanceBaseObject
{
	// Token: 0x06000CB3 RID: 3251 RVA: 0x0003FE69 File Offset: 0x0003E069
	public static TalentExpLevelBalanceData GetTalentLevelData(string talentId)
	{
		return GameBalance.Me.talentExpLevelsCache.GetValueOrDefault(talentId);
	}

	// Token: 0x04000EF3 RID: 3827
	public const string YELLOW_TALENT_ID = "talent_yellow";

	// Token: 0x04000EF4 RID: 3828
	public const string GREEN_TALENT_ID = "talent_green";

	// Token: 0x04000EF5 RID: 3829
	public const string RED_TALENT_ID = "talent_red";

	// Token: 0x04000EF6 RID: 3830
	public const string ORANGE_TALENT_ID = "talent_orange";

	// Token: 0x04000EF7 RID: 3831
	public const string BLUE_TALENT_ID = "talent_blue";

	// Token: 0x04000EF8 RID: 3832
	[AutoParse("talent_yellow")]
	public int yellow;

	// Token: 0x04000EF9 RID: 3833
	[AutoParse("talent_green")]
	public int green;

	// Token: 0x04000EFA RID: 3834
	[AutoParse("talent_red")]
	public int red;

	// Token: 0x04000EFB RID: 3835
	[AutoParse("talent_orange")]
	public int orange;

	// Token: 0x04000EFC RID: 3836
	[AutoParse("talent_blue")]
	public int blue;
}
