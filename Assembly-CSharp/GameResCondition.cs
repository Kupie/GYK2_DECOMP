using System;
using UnityEngine;

// Token: 0x020003E5 RID: 997
[Serializable]
public class GameResCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000487 RID: 1159
	// (get) Token: 0x06001A56 RID: 6742 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.GameResChanged;
		}
	}

	// Token: 0x06001A57 RID: 6743 RVA: 0x0007B4EB File Offset: 0x000796EB
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && !string.IsNullOrEmpty(this.gameResId);
	}

	// Token: 0x06001A58 RID: 6744 RVA: 0x0007B508 File Offset: 0x00079708
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		float num = (float)context.WgoData.GetGameResInt(this.gameResId);
		return base.CompareValues(num, this.comparison, this.targetValue, this.targetValue2);
	}

	// Token: 0x04001985 RID: 6533
	[Tooltip("The ID of the game resource to check")]
	public string gameResId;

	// Token: 0x04001986 RID: 6534
	[Tooltip("The comparison operator to use")]
	public ComparisonOperator comparison;

	// Token: 0x04001987 RID: 6535
	[Tooltip("The target value to compare against")]
	public float targetValue;

	// Token: 0x04001988 RID: 6536
	[Tooltip("Second target value (used for Between comparisons)")]
	public float targetValue2;
}
