using System;

// Token: 0x020007FA RID: 2042
[Serializable]
public class AnswerData
{
	// Token: 0x06003466 RID: 13414 RVA: 0x000FC00E File Offset: 0x000FA20E
	public void AddLockRes(SmartRes res)
	{
		this.AddRes(ref this.lockRes, res);
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x000FC01D File Offset: 0x000FA21D
	public void AddCostRes(SmartRes res)
	{
		this.AddRes(ref this.costRes, res);
	}

	// Token: 0x06003468 RID: 13416 RVA: 0x000FC02C File Offset: 0x000FA22C
	public void AddRewardRes(SmartRes res)
	{
		this.AddRes(ref this.rewardRes, res);
	}

	// Token: 0x06003469 RID: 13417 RVA: 0x000FC03B File Offset: 0x000FA23B
	public void AddDay(string dayNumber)
	{
		this.dayNumber = dayNumber;
	}

	// Token: 0x0600346A RID: 13418 RVA: 0x000FC044 File Offset: 0x000FA244
	public void AddOrder(string order)
	{
		this.order = order;
	}

	// Token: 0x0600346B RID: 13419 RVA: 0x000FC04D File Offset: 0x000FA24D
	private void AddRes(ref SmartRes data, SmartRes res)
	{
		if (data == null)
		{
			data = res;
			return;
		}
		data.items.AddRange(res.items);
		data.gameRes.Add(res.gameRes);
	}

	// Token: 0x040029D3 RID: 10707
	public SmartRes lockRes;

	// Token: 0x040029D4 RID: 10708
	public SmartRes costRes;

	// Token: 0x040029D5 RID: 10709
	public SmartRes rewardRes;

	// Token: 0x040029D6 RID: 10710
	public SmartRes fakeRewardRes;

	// Token: 0x040029D7 RID: 10711
	public string dayNumber = string.Empty;

	// Token: 0x040029D8 RID: 10712
	public string order;

	// Token: 0x040029D9 RID: 10713
	public bool customHideCondition;

	// Token: 0x040029DA RID: 10714
	public bool notAvailable;
}
