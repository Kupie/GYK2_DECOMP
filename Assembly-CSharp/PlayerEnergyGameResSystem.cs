using System;
using LazyBearTechnology;

// Token: 0x02000435 RID: 1077
public class PlayerEnergyGameResSystem : GK2GameResSystem
{
	// Token: 0x06001C83 RID: 7299 RVA: 0x00085506 File Offset: 0x00083706
	public PlayerEnergyGameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
	}

	// Token: 0x170004ED RID: 1261
	// (get) Token: 0x06001C84 RID: 7300 RVA: 0x00085510 File Offset: 0x00083710
	private PerkSystemData PerkSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.perkSystemData;
		}
	}

	// Token: 0x06001C85 RID: 7301 RVA: 0x00085524 File Offset: 0x00083724
	public override void Add(float value, bool silent = false)
	{
		if (value < 0f && this.PerkSystemData.HasPerk("lack_of_sleep_debuff"))
		{
			GK2GameResSystem.PlayerData.AddRes("insanity", -value / 2f);
		}
		this.Set(GK2GameResSystem.PlayerData.GetRes(this.gameResAtomName, 0f) + value, silent);
	}

	// Token: 0x06001C86 RID: 7302 RVA: 0x00085580 File Offset: 0x00083780
	public static PlayerEnergyGameResSystem GetSystem()
	{
		return GK2GameResSystem.PlayerData.GetResSystem("energy") as PlayerEnergyGameResSystem;
	}
}
