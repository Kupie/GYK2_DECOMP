using System;
using UnityEngine;

// Token: 0x020003D7 RID: 983
[Serializable]
public abstract class ConditionalDrawerConditionBase : IConditionalDrawerCondition
{
	// Token: 0x1700047F RID: 1151
	// (get) Token: 0x06001A35 RID: 6709
	public abstract ConditionalEventType EventType { get; }

	// Token: 0x06001A36 RID: 6710
	public abstract bool Evaluate(ConditionalDrawerContext context);

	// Token: 0x06001A37 RID: 6711 RVA: 0x0007B020 File Offset: 0x00079220
	public virtual bool IsValid(ConditionalDrawerContext context)
	{
		return ((context != null) ? context.WgoData : null) != null;
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0007B034 File Offset: 0x00079234
	protected bool CompareValues(float value, ComparisonOperator op, float target, float target2 = 0f)
	{
		bool flag;
		switch (op)
		{
		case ComparisonOperator.Less:
			flag = value < target;
			break;
		case ComparisonOperator.LessOrEqual:
			flag = value <= target;
			break;
		case ComparisonOperator.Equal:
			flag = Mathf.Approximately(value, target);
			break;
		case ComparisonOperator.GreaterOrEqual:
			flag = value >= target;
			break;
		case ComparisonOperator.Greater:
			flag = value > target;
			break;
		case ComparisonOperator.Between:
			flag = value >= target && value <= target2;
			break;
		case ComparisonOperator.BetweenExcludeLeft:
			flag = value > target && value <= target2;
			break;
		case ComparisonOperator.BetweenExcludeRight:
			flag = value >= target && value < target2;
			break;
		case ComparisonOperator.BetweenExcludeBoth:
			flag = value > target && value < target2;
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0007B0E0 File Offset: 0x000792E0
	protected bool CompareValues(int value, ComparisonOperator op, int target, int target2 = 0)
	{
		bool flag;
		switch (op)
		{
		case ComparisonOperator.Less:
			flag = value < target;
			break;
		case ComparisonOperator.LessOrEqual:
			flag = value <= target;
			break;
		case ComparisonOperator.Equal:
			flag = value == target;
			break;
		case ComparisonOperator.GreaterOrEqual:
			flag = value >= target;
			break;
		case ComparisonOperator.Greater:
			flag = value > target;
			break;
		case ComparisonOperator.Between:
			flag = value >= target && value <= target2;
			break;
		case ComparisonOperator.BetweenExcludeLeft:
			flag = value > target && value <= target2;
			break;
		case ComparisonOperator.BetweenExcludeRight:
			flag = value >= target && value < target2;
			break;
		case ComparisonOperator.BetweenExcludeBoth:
			flag = value > target && value < target2;
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}
}
