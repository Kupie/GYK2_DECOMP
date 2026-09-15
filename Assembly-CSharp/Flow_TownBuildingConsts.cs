using System;

// Token: 0x020004E0 RID: 1248
public static class Flow_TownBuildingConsts
{
	// Token: 0x060020B5 RID: 8373 RVA: 0x0009AF95 File Offset: 0x00099195
	public static string GetPlayerTpDuringFadeGdPointId(string gdPointTent)
	{
		if (string.IsNullOrEmpty(gdPointTent))
		{
			return null;
		}
		return gdPointTent + "_player_tp_during_fade";
	}

	// Token: 0x04001D76 RID: 7542
	public const string TOWN_BUILDING_TIME_AVAILABILITY_KEY = "available_by_time";

	// Token: 0x04001D77 RID: 7543
	public const string PLAYER_TP_DURING_FADE_POSTFIX = "_player_tp_during_fade";
}
