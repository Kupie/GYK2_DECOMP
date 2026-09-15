using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000208 RID: 520
[Serializable]
public class SermonDef : CraftDefBase
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0003FB2B File Offset: 0x0003DD2B
	public Sprite PrayIcon
	{
		get
		{
			return LazySingletonSO<EasySpritesCollection>.Instance.GetSprite(this.prayIcon, "i_b_base_act");
		}
	}

	// Token: 0x06000CA5 RID: 3237 RVA: 0x0003FB42 File Offset: 0x0003DD42
	public override OutputPreview GetOutputPreview(WgoData wgoData = null)
	{
		if (this.outputPreview == null)
		{
			this.outputPreview = new OutputPreview(this.id, "", false, 1, -1, "");
		}
		return this.outputPreview;
	}

	// Token: 0x06000CA6 RID: 3238 RVA: 0x0003FB70 File Offset: 0x0003DD70
	public PerkWidgetData GetBuffWidgetData()
	{
		if (this.perkWidgetData == null && !string.IsNullOrEmpty(this.successRewardBuff))
		{
			this.perkWidgetData = new PerkWidgetData(new PerkData(this.successRewardBuff), true, null, null, null);
		}
		return this.perkWidgetData;
	}

	// Token: 0x04000ED8 RID: 3800
	[AutoParse("min_parishioners")]
	public int minParishioners;

	// Token: 0x04000ED9 RID: 3801
	[AutoParse("sermon_difficulty")]
	public int sermonDifficulty;

	// Token: 0x04000EDA RID: 3802
	[AutoParse("base_reward_faith")]
	public LazyExpression baseFaithReward;

	// Token: 0x04000EDB RID: 3803
	[AutoParse("base_reward_money")]
	public LazyExpression baseMoneyReward;

	// Token: 0x04000EDC RID: 3804
	[AutoParse("success_reward_buff")]
	public string successRewardBuff;

	// Token: 0x04000EDD RID: 3805
	[AutoParse("success_reward_item")]
	public OutputItems successRewardItem = new OutputItems();

	// Token: 0x04000EDE RID: 3806
	[AutoParse("success_reward_cemetery_faith")]
	public LazyExpression successRewardCemeteryFaith;

	// Token: 0x04000EDF RID: 3807
	[AutoParse("success_reward_cemetery_money")]
	public LazyExpression successRewardCemeteryMoney;

	// Token: 0x04000EE0 RID: 3808
	[AutoParse("success_reward_church_faith")]
	public LazyExpression successRewardChurchFaith;

	// Token: 0x04000EE1 RID: 3809
	[AutoParse("success_reward_church_money")]
	public LazyExpression successRewardChurchMoney;

	// Token: 0x04000EE2 RID: 3810
	[AutoParse("success_reward_smile_faith")]
	public LazyExpression successRewardSmileFaith;

	// Token: 0x04000EE3 RID: 3811
	[AutoParse("success_reward_smile_money")]
	public LazyExpression successRewardSmileMoney;

	// Token: 0x04000EE4 RID: 3812
	[AutoParse("pray_icon")]
	public string prayIcon;

	// Token: 0x04000EE5 RID: 3813
	[AutoParse("pray_on_end_expression")]
	public List<LazyExpression> prayOnEndExpressions = new List<LazyExpression>();

	// Token: 0x04000EE6 RID: 3814
	private OutputPreview outputPreview;

	// Token: 0x04000EE7 RID: 3815
	private PerkWidgetData perkWidgetData;
}
