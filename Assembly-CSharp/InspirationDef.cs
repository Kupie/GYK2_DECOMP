using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001EE RID: 494
[Serializable]
public class InspirationDef : BalanceBaseObject
{
	// Token: 0x17000219 RID: 537
	// (get) Token: 0x06000C4B RID: 3147 RVA: 0x0003E43B File Offset: 0x0003C63B
	public Sprite Icon
	{
		get
		{
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.icon, "i_inspiration_anatomy_lvl_01");
		}
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0003E452 File Offset: 0x0003C652
	public static InspirationDef GetDataForLevel(string inspirationId, int level)
	{
		return GameBalance.Me.inspirationLevelsCache[inspirationId].GetDataForLevel(level);
	}

	// Token: 0x04000DF7 RID: 3575
	public string idWithoutLvl;

	// Token: 0x04000DF8 RID: 3576
	[AutoParse("level")]
	public int lvl;

	// Token: 0x04000DF9 RID: 3577
	[AutoParse("lvl_frame")]
	public int lvlFrame;

	// Token: 0x04000DFA RID: 3578
	[AutoParse("talent")]
	public string talentId;

	// Token: 0x04000DFB RID: 3579
	[AutoParse("completion_price")]
	public int completionPrice;

	// Token: 0x04000DFC RID: 3580
	[AutoParse("completion_goal_value")]
	public int completionGoalValue;

	// Token: 0x04000DFD RID: 3581
	[AutoParse("exp_to_talent")]
	public int completionExp;

	// Token: 0x04000DFE RID: 3582
	[AutoParse("unlock_after_inspiration")]
	public List<string> inpsirationLocks = new List<string>();

	// Token: 0x04000DFF RID: 3583
	[AutoParse("unlock_after_tech")]
	public List<string> techLocks = new List<string>();

	// Token: 0x04000E00 RID: 3584
	[SerializeField]
	[AutoParse("icon")]
	private string icon = string.Empty;

	// Token: 0x04000E01 RID: 3585
	[AutoParse("unlock_after_quest")]
	public List<string> questLocks = new List<string>();
}
