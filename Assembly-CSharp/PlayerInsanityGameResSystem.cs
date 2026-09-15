using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class PlayerInsanityGameResSystem : GK2GameResSystem
{
	// Token: 0x06001C89 RID: 7305 RVA: 0x00085506 File Offset: 0x00083706
	public PlayerInsanityGameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
	}

	// Token: 0x170004EE RID: 1262
	// (get) Token: 0x06001C8A RID: 7306 RVA: 0x00085510 File Offset: 0x00083710
	private PerkSystemData PerkSystemData
	{
		get
		{
			return MainGame.Instance.GameSave.perkSystemData;
		}
	}

	// Token: 0x06001C8B RID: 7307 RVA: 0x00085718 File Offset: 0x00083918
	public bool CanChangeInsanity(float value)
	{
		if (value > 0f)
		{
			return base.CanAddValue(value);
		}
		return base.IsEnoughValue(-value);
	}

	// Token: 0x06001C8C RID: 7308 RVA: 0x00085734 File Offset: 0x00083934
	public override void Set(float value, bool silent = false)
	{
		float res = GK2GameResSystem.PlayerData.GetRes(this.gameResAtomName, 0f);
		float num = Mathf.Clamp(value, 0f, base.Max);
		GK2GameResSystem.PlayerData.SetResWithoutSystemsCheck(this.gameResAtomName, num);
		float max = PlayerEnergyGameResSystem.GetSystem().Max;
		if (GK2GameResSystem.PlayerData.GetRes("energy", 0f) > max)
		{
			GK2GameResSystem.PlayerData.SetRes("energy", max);
		}
		if (!silent)
		{
			Action<float> onValueChanged = this.onValueChanged;
			if (onValueChanged != null)
			{
				onValueChanged(num);
			}
			Action<float> onValueDeltaChanged = this.onValueDeltaChanged;
			if (onValueDeltaChanged != null)
			{
				onValueDeltaChanged(num - res);
			}
		}
		int num2 = (int)num - (int)res;
		base.EvaluateGameResExpressions(num2);
	}

	// Token: 0x06001C8D RID: 7309 RVA: 0x000857E4 File Offset: 0x000839E4
	public override void Add(float value, bool silent = false)
	{
		if (value < 0f && this.PerkSystemData.HasPerk("lack_of_sleep_debuff"))
		{
			GK2GameResSystem.PlayerData.AddResWithoutSystemsCheck(this.gameResAtomName, -value / 2f);
		}
		this.Set(GK2GameResSystem.PlayerData.GetRes(this.gameResAtomName, 0f) + value, silent);
	}

	// Token: 0x06001C8E RID: 7310 RVA: 0x00085841 File Offset: 0x00083A41
	public static PlayerInsanityGameResSystem GetSystem()
	{
		return GK2GameResSystem.PlayerData.GetResSystem("insanity") as PlayerInsanityGameResSystem;
	}
}
