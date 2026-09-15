using System;
using UnityEngine;

// Token: 0x020003E8 RID: 1000
[Serializable]
public class HPThresholdReachedCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000489 RID: 1161
	// (get) Token: 0x06001A5E RID: 6750 RVA: 0x0007B541 File Offset: 0x00079741
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.HPChanged;
		}
	}

	// Token: 0x06001A5F RID: 6751 RVA: 0x0007B548 File Offset: 0x00079748
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.WgoData != null && context.WgoData.HpComponent != null;
	}

	// Token: 0x06001A60 RID: 6752 RVA: 0x0007B5E4 File Offset: 0x000797E4
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		if (!context.IsCalledFromEvent)
		{
			return false;
		}
		HPComponent hpComponent = context.WgoData.HpComponent;
		int maxHpValue = hpComponent.MaxHpValue;
		if (this.usePercentage && maxHpValue <= 0)
		{
			return false;
		}
		float num = this.ToComparableValue(hpComponent.prevHp, maxHpValue);
		float num2 = this.ToComparableValue(hpComponent.Hp, maxHpValue);
		if (Mathf.Approximately(num, num2))
		{
			return false;
		}
		bool flag;
		switch (this.direction)
		{
		case HPThresholdCrossDirection.Decreasing:
			flag = num > this.targetValue && num2 <= this.targetValue;
			break;
		case HPThresholdCrossDirection.Increasing:
			flag = num < this.targetValue && num2 >= this.targetValue;
			break;
		case HPThresholdCrossDirection.Any:
			flag = (num > this.targetValue && num2 <= this.targetValue) || (num < this.targetValue && num2 >= this.targetValue);
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x0007B6CE File Offset: 0x000798CE
	private float ToComparableValue(int hp, int maxHp)
	{
		if (!this.usePercentage)
		{
			return (float)hp;
		}
		return (float)hp / (float)maxHp * 100f;
	}

	// Token: 0x04001991 RID: 6545
	[Tooltip("Whether to treat the target as a percentage of max HP instead of an absolute value")]
	public bool usePercentage;

	// Token: 0x04001992 RID: 6546
	[Tooltip("HP value (or %) that must be reached or passed")]
	public float targetValue;

	// Token: 0x04001993 RID: 6547
	[Tooltip("Which HP change direction can trigger the condition")]
	public HPThresholdCrossDirection direction;
}
