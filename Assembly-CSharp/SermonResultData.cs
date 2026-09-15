using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A10 RID: 2576
[Serializable]
public class SermonResultData : ObjectLinkedToDefinition<SermonDef>
{
	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06004546 RID: 17734 RVA: 0x00147AD4 File Offset: 0x00145CD4
	public int Money
	{
		get
		{
			int num = this.parishionersCount * this.baseMoneyReward;
			if (this.success)
			{
				num += this.parishionersCount * this.successRewardSmileMoney;
				num += this.graveyardQuality * this.successRewardCemeteryMoney;
				num += this.churchQuality * this.successRewardChurchMoney;
			}
			return num;
		}
	}

	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06004547 RID: 17735 RVA: 0x00147B28 File Offset: 0x00145D28
	public int FaithOnlyParishioners
	{
		get
		{
			return (int)Math.Round((double)((float)this.parishionersCount * this.baseFaithReward), MidpointRounding.AwayFromZero);
		}
	}

	// Token: 0x17000A93 RID: 2707
	// (get) Token: 0x06004548 RID: 17736 RVA: 0x00147B40 File Offset: 0x00145D40
	public int FaithOnlyBonus
	{
		get
		{
			float num = 0f;
			if (this.success)
			{
				num += (float)this.parishionersCount * this.successRewardSmileFaith;
				num += (float)this.graveyardQuality * this.successRewardCemeteryFaith;
				num += (float)this.churchQuality * this.successRewardChurchFaith;
			}
			return (int)Math.Round((double)num, MidpointRounding.AwayFromZero);
		}
	}

	// Token: 0x17000A94 RID: 2708
	// (get) Token: 0x06004549 RID: 17737 RVA: 0x00147B97 File Offset: 0x00145D97
	public int MoneyOnlyParishioners
	{
		get
		{
			return this.parishionersCount * this.baseMoneyReward;
		}
	}

	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x0600454A RID: 17738 RVA: 0x00147BA8 File Offset: 0x00145DA8
	public int MoneyOnlyBonus
	{
		get
		{
			int num = 0;
			if (this.success)
			{
				num += this.parishionersCount * this.successRewardSmileMoney;
				num += this.graveyardQuality * this.successRewardCemeteryMoney;
				num += this.churchQuality * this.successRewardChurchMoney;
			}
			return num;
		}
	}

	// Token: 0x0600454B RID: 17739 RVA: 0x00147BF0 File Offset: 0x00145DF0
	public SermonResultData(string sermonId, string sermonConfigId, int parishionersCount, bool success, int churchQuality, int graveyardQuality)
		: base(sermonId)
	{
		this.sermonConfigId = sermonConfigId;
		this.parishionersCount = parishionersCount;
		this.success = success;
		this.churchQuality = churchQuality;
		this.graveyardQuality = graveyardQuality;
		this.successRewardCemeteryMoney = base.Definition.successRewardCemeteryMoney.EvaluateInt();
		this.successRewardChurchMoney = base.Definition.successRewardChurchMoney.EvaluateInt();
		this.successRewardSmileMoney = base.Definition.successRewardSmileMoney.EvaluateInt();
		this.baseMoneyReward = base.Definition.baseMoneyReward.EvaluateInt();
		this.successRewardCemeteryFaith = base.Definition.successRewardCemeteryFaith.EvaluateFloat();
		this.successRewardChurchFaith = base.Definition.successRewardChurchFaith.EvaluateFloat();
		this.successRewardSmileFaith = base.Definition.successRewardSmileFaith.EvaluateFloat();
		this.baseFaithReward = base.Definition.baseFaithReward.EvaluateFloat();
		this.parishionerDatas.Clear();
		if (parishionersCount <= 0)
		{
			return;
		}
		int num = (int)Math.Round((double)((float)parishionersCount * this.baseFaithReward), MidpointRounding.AwayFromZero);
		float num2 = 0f;
		if (success)
		{
			num2 += (float)parishionersCount * this.successRewardSmileFaith;
			num2 += (float)graveyardQuality * this.successRewardCemeteryFaith;
			num2 += (float)churchQuality * this.successRewardChurchFaith;
		}
		int num3 = (int)Math.Round((double)num2, MidpointRounding.AwayFromZero);
		int num4 = num + num3;
		int num5 = num4 / parishionersCount;
		int num6 = num4 % parishionersCount;
		for (int i = 0; i < parishionersCount; i++)
		{
			int num7 = num5;
			if (i < num6)
			{
				num7++;
			}
			this.parishionerDatas.Add(new ParishionerData(base.Definition, num7));
		}
	}

	// Token: 0x0600454C RID: 17740 RVA: 0x00147D80 File Offset: 0x00145F80
	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"SermonResultData sermonConfigId:[",
			this.sermonConfigId,
			"],",
			string.Format(" {0}:[{1}],", "parishionersCount", this.parishionersCount),
			string.Format(" {0}:[{1}],", "success", this.success),
			string.Format(" {0}:[{1}],", "churchQuality", this.churchQuality),
			string.Format(" {0}:[{1}],", "graveyardQuality", this.graveyardQuality),
			string.Format(" {0}:[{1}],", this.Money, this.Money),
			string.Format(" {0}:[{1}],", "FaithOnlyParishioners", this.FaithOnlyParishioners),
			string.Format(" {0}:[{1}],", "FaithOnlyBonus", this.FaithOnlyBonus),
			string.Format(" {0}:[{1}],", "MoneyOnlyParishioners", this.MoneyOnlyParishioners),
			string.Format(" {0}:[{1}],", "MoneyOnlyBonus", this.MoneyOnlyBonus)
		});
	}

	// Token: 0x0400361A RID: 13850
	public readonly string sermonConfigId;

	// Token: 0x0400361B RID: 13851
	public readonly int parishionersCount;

	// Token: 0x0400361C RID: 13852
	public readonly List<ParishionerData> parishionerDatas = new List<ParishionerData>();

	// Token: 0x0400361D RID: 13853
	public readonly bool success;

	// Token: 0x0400361E RID: 13854
	public readonly int churchQuality;

	// Token: 0x0400361F RID: 13855
	public readonly int graveyardQuality;

	// Token: 0x04003620 RID: 13856
	public readonly int successRewardCemeteryMoney;

	// Token: 0x04003621 RID: 13857
	public readonly int successRewardChurchMoney;

	// Token: 0x04003622 RID: 13858
	public readonly int successRewardSmileMoney;

	// Token: 0x04003623 RID: 13859
	public readonly int baseMoneyReward;

	// Token: 0x04003624 RID: 13860
	public readonly float successRewardCemeteryFaith;

	// Token: 0x04003625 RID: 13861
	public readonly float successRewardChurchFaith;

	// Token: 0x04003626 RID: 13862
	public readonly float successRewardSmileFaith;

	// Token: 0x04003627 RID: 13863
	public readonly float baseFaithReward;
}
