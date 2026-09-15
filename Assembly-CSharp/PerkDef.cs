using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001FC RID: 508
[Serializable]
public class PerkDef : BalanceBaseObject
{
	// Token: 0x17000225 RID: 549
	// (get) Token: 0x06000C8B RID: 3211 RVA: 0x0003F5C2 File Offset: 0x0003D7C2
	public Sprite Icon
	{
		get
		{
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(string.IsNullOrEmpty(this.customIcon) ? this.id : this.customIcon, "b_sleep");
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000C8C RID: 3212 RVA: 0x0003F5EE File Offset: 0x0003D7EE
	public string IconId
	{
		get
		{
			if (!string.IsNullOrEmpty(this.customIcon))
			{
				return this.customIcon;
			}
			return this.id;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003F60A File Offset: 0x0003D80A
	public bool IsFertilizerPerk
	{
		get
		{
			return this.id.StartsWith("perk_fertilize_");
		}
	}

	// Token: 0x06000C8E RID: 3214 RVA: 0x0003CD40 File Offset: 0x0003AF40
	public string GetHeader()
	{
		return LLBase.L(this.id);
	}

	// Token: 0x06000C8F RID: 3215 RVA: 0x0003F61C File Offset: 0x0003D81C
	public string GetHeaderPrefix()
	{
		return LLBase.L("ui_perk");
	}

	// Token: 0x04000E85 RID: 3717
	[AutoParse("icon")]
	[SerializeField]
	private string customIcon;

	// Token: 0x04000E86 RID: 3718
	[AutoParse("perk_type")]
	public PerkType perkType;

	// Token: 0x04000E87 RID: 3719
	[AutoParse("energy_add")]
	public float energyAdd;

	// Token: 0x04000E88 RID: 3720
	[AutoParse("insanity_add")]
	public float insanityAdd;

	// Token: 0x04000E89 RID: 3721
	[AutoParse("craft_done_hits")]
	public int craftStartTicks;

	// Token: 0x04000E8A RID: 3722
	[AutoParse("craft_add_duration")]
	public int craftTotalProgressTicksBonus;

	// Token: 0x04000E8B RID: 3723
	[AutoParse("craft_add_talent")]
	public int craftMasteryBonus;

	// Token: 0x04000E8C RID: 3724
	[AutoParse("duration")]
	public float duration;

	// Token: 0x04000E8D RID: 3725
	[AutoParse("has_hidden_timer")]
	public bool hiddenTimer;

	// Token: 0x04000E8E RID: 3726
	[AutoParse("perk_add_type")]
	public PerkAddType perkAddType;

	// Token: 0x04000E8F RID: 3727
	[AutoParse("is_hidden")]
	public bool isHidden;

	// Token: 0x04000E90 RID: 3728
	[AutoParse("set_res_on_add")]
	public GameRes setGameResOnAdd;

	// Token: 0x04000E91 RID: 3729
	[AutoParse("add_res_on_add")]
	public GameRes addGameResOnAdd;

	// Token: 0x04000E92 RID: 3730
	[AutoParse("exp_on_add")]
	public List<LazyExpression> onAddExpressions;

	// Token: 0x04000E93 RID: 3731
	[AutoParse("set_res_on_remove")]
	public GameRes setGameResOnRemove;

	// Token: 0x04000E94 RID: 3732
	[AutoParse("add_res_on_remove")]
	public GameRes addGameResOnRemove;

	// Token: 0x04000E95 RID: 3733
	[AutoParse("exp_on_remove")]
	public List<LazyExpression> onRemoveExpressions;

	// Token: 0x04000E96 RID: 3734
	[AutoParse("tick_rate")]
	public float tickRate;

	// Token: 0x04000E97 RID: 3735
	[AutoParse("add_res_per_tick")]
	public GameRes addGameResPerTick;

	// Token: 0x04000E98 RID: 3736
	[AutoParse("exp_per_tick")]
	public List<LazyExpression> onPerTickExpressions;

	// Token: 0x04000E99 RID: 3737
	[AutoParse("fertilizer_item")]
	public string fertilizerItemId;

	// Token: 0x04000E9A RID: 3738
	[AutoParse("world_fx_prefab")]
	public string worldFxPrefabId;

	// Token: 0x04000E9B RID: 3739
	[AutoParse("hud_fx_prefab")]
	public string hudFxPrefabId;
}
