using System;
using UnityEngine;

// Token: 0x020003E6 RID: 998
[Serializable]
public class HPChangedCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000488 RID: 1160
	// (get) Token: 0x06001A5A RID: 6746 RVA: 0x0007B541 File Offset: 0x00079741
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.HPChanged;
		}
	}

	// Token: 0x06001A5B RID: 6747 RVA: 0x0007B548 File Offset: 0x00079748
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.WgoData != null && context.WgoData.HpComponent != null;
	}

	// Token: 0x06001A5C RID: 6748 RVA: 0x0007B56C File Offset: 0x0007976C
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		int hp = context.WgoData.HpComponent.Hp;
		int maxHpValue = context.WgoData.HpComponent.MaxHpValue;
		if (!this.usePercentage)
		{
			return base.CompareValues(hp, this.comparison, (int)this.targetValue, (int)this.targetValue2);
		}
		float num = (float)hp / (float)maxHpValue * 100f;
		return base.CompareValues(num, this.comparison, this.targetValue, this.targetValue2);
	}

	// Token: 0x04001989 RID: 6537
	[Tooltip("Whether to use percentage-based comparison instead of absolute count")]
	public bool usePercentage;

	// Token: 0x0400198A RID: 6538
	[Tooltip("The comparison operator to use")]
	public ComparisonOperator comparison;

	// Token: 0x0400198B RID: 6539
	[Tooltip("The target value to compare against (count or percentage)")]
	public float targetValue;

	// Token: 0x0400198C RID: 6540
	[Tooltip("Second target value (used for Between comparisons)")]
	public float targetValue2;
}
