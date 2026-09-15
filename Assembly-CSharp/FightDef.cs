using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020001E5 RID: 485
[Serializable]
public class FightDef : BalanceBaseObject
{
	// Token: 0x04000DB6 RID: 3510
	[AutoParse("squads")]
	public int squads;

	// Token: 0x04000DB7 RID: 3511
	[AutoParse("defence_power_lock")]
	public int defencePowerLock;

	// Token: 0x04000DB8 RID: 3512
	[AutoParse("rewards")]
	public List<NeedItemData> rewards = new List<NeedItemData>();

	// Token: 0x04000DB9 RID: 3513
	[AutoParse("on_debug_start")]
	public List<LazyExpression> onDebugStartExpressions = new List<LazyExpression>();

	// Token: 0x04000DBA RID: 3514
	[AutoParse("on_win_next_fight_id")]
	public string onWinNextFightId;

	// Token: 0x04000DBB RID: 3515
	[AutoParse("is_barricades_unavailable")]
	public bool isBarricadesUnavailable;

	// Token: 0x04000DBC RID: 3516
	[AutoParse("is_towers_unavailable")]
	public bool isTowersUnavailable;
}
