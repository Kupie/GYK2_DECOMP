using System;
using UnityEngine;

// Token: 0x020003DE RID: 990
[Serializable]
public class CraftProgressCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000483 RID: 1155
	// (get) Token: 0x06001A47 RID: 6727 RVA: 0x0007B2E8 File Offset: 0x000794E8
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.CraftProgressChanged;
		}
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0007B2EB File Offset: 0x000794EB
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.CraftComponent != null;
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0007B304 File Offset: 0x00079504
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		CraftElementBase currentCraftElement = context.CraftComponent.CurrentCraftElement;
		float num = ((currentCraftElement != null) ? currentCraftElement.ProgressTimeNormalized : 0f);
		return base.CompareValues(num, this.comparison, this.targetProgress, this.targetProgress2);
	}

	// Token: 0x04001974 RID: 6516
	[Tooltip("The comparison operator to use")]
	public ComparisonOperator comparison;

	// Token: 0x04001975 RID: 6517
	[Tooltip("The target progress value (0-1)")]
	[Range(0f, 1f)]
	public float targetProgress;

	// Token: 0x04001976 RID: 6518
	[Tooltip("Second target progress value (used for Between comparisons)")]
	[Range(0f, 1f)]
	public float targetProgress2;
}
