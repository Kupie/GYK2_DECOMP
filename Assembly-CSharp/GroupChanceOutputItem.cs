using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000401 RID: 1025
[Serializable]
public class GroupChanceOutputItem
{
	// Token: 0x06001AC2 RID: 6850 RVA: 0x0007C5C4 File Offset: 0x0007A7C4
	public List<ItemCount> MakePreOutput(WgoData wgoData, float resultForQualityRoll = 0f)
	{
		float num = global::UnityEngine.Random.Range(0f, this.GetChanceSum(wgoData));
		float num2 = 0f;
		List<ItemCount> list = null;
		foreach (ChanceOutputItem chanceOutputItem in this.chanceItems)
		{
			num2 += chanceOutputItem.chance.EvaluateFloat(wgoData);
			if (num2 >= num)
			{
				list = chanceOutputItem.MakePreOutputAsGroupItem(wgoData, resultForQualityRoll);
				break;
			}
		}
		return list;
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x0007C64C File Offset: 0x0007A84C
	private float GetChanceSum(WgoData wgoData)
	{
		float num = 0f;
		foreach (ChanceOutputItem chanceOutputItem in this.chanceItems)
		{
			num += chanceOutputItem.chance.EvaluateFloat(wgoData);
		}
		return num;
	}

	// Token: 0x040019EA RID: 6634
	public string outputGroupId;

	// Token: 0x040019EB RID: 6635
	public List<ChanceOutputItem> chanceItems = new List<ChanceOutputItem>();
}
