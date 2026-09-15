using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class PlayerStaminaGameResSystem : GK2GameResSystem
{
	// Token: 0x06001C93 RID: 7315 RVA: 0x00085506 File Offset: 0x00083706
	public PlayerStaminaGameResSystem(string gameResAtomName, GameRes gameRes)
		: base(gameResAtomName, gameRes)
	{
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x00085718 File Offset: 0x00083918
	public bool CanChangeStamina(float value)
	{
		if (value > 0f)
		{
			return base.CanAddValue(value);
		}
		return base.IsEnoughValue(-value);
	}

	// Token: 0x06001C95 RID: 7317 RVA: 0x000858CC File Offset: 0x00083ACC
	public override void Set(float value, bool silent = false)
	{
		float res = GK2GameResSystem.PlayerData.GetRes(this.gameResAtomName, 0f);
		float num = Mathf.Clamp(value, 0f, base.Max);
		GK2GameResSystem.PlayerData.SetResWithoutSystemsCheck(this.gameResAtomName, num);
		Action<float> onValueChanged = this.onValueChanged;
		if (onValueChanged != null)
		{
			onValueChanged(num);
		}
		Action<float> onValueDeltaChanged = this.onValueDeltaChanged;
		if (onValueDeltaChanged == null)
		{
			return;
		}
		onValueDeltaChanged(num - res);
	}

	// Token: 0x06001C96 RID: 7318 RVA: 0x00085937 File Offset: 0x00083B37
	public override void Add(float value, bool silent = false)
	{
		this.Set(GK2GameResSystem.PlayerData.GetRes(this.gameResAtomName, 0f) + value, silent);
	}

	// Token: 0x06001C97 RID: 7319 RVA: 0x00085957 File Offset: 0x00083B57
	public static PlayerStaminaGameResSystem GetSystem()
	{
		return GK2GameResSystem.PlayerData.GetResSystem("stamina") as PlayerStaminaGameResSystem;
	}
}
