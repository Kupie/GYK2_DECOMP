using System;

// Token: 0x02000329 RID: 809
public static class FightingWgoTarget
{
	// Token: 0x060015A3 RID: 5539 RVA: 0x000696C0 File Offset: 0x000678C0
	public static bool IsBarricadeOrTower(ICombatEntity entity)
	{
		Wgo wgo = entity as Wgo;
		return wgo != null && FightingWgoTarget.IsBarricadeOrTower(wgo);
	}

	// Token: 0x060015A4 RID: 5540 RVA: 0x000696DF File Offset: 0x000678DF
	public static bool IsBarricadeOrTower(Wgo wgo)
	{
		return wgo != null && FightingWgoTarget.IsBarricadeOrTower(wgo.Data);
	}

	// Token: 0x060015A5 RID: 5541 RVA: 0x000696F8 File Offset: 0x000678F8
	public static bool IsBarricadeOrTower(WgoData data)
	{
		return ((data != null) ? data.Definition : null) != null && (data.Definition.interactionType == WGODef.InteractionType.Barricade || (!(GameBalance.Me == null) && !string.IsNullOrEmpty(data.id) && (GameBalance.Me.HasWgoIdByGroup("barricades", data.id) || GameBalance.Me.HasWgoIdByGroup("towers", data.id))));
	}
}
