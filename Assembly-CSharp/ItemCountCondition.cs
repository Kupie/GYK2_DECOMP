using System;
using UnityEngine;

// Token: 0x020003EA RID: 1002
[Serializable]
public class ItemCountCondition : ConditionalDrawerConditionBase
{
	// Token: 0x1700048B RID: 1163
	// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0007B70C File Offset: 0x0007990C
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ItemsChanged;
		}
	}

	// Token: 0x06001A67 RID: 6759 RVA: 0x0007B70F File Offset: 0x0007990F
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.Inventory != null && !string.IsNullOrEmpty(this.itemId);
	}

	// Token: 0x06001A68 RID: 6760 RVA: 0x0007B734 File Offset: 0x00079934
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		Item itemById = context.Inventory.GetItemById(this.itemId);
		int num = ((itemById != null) ? itemById.Count : 0);
		return base.CompareValues(num, this.comparison, this.targetCount, this.targetCount2);
	}

	// Token: 0x04001995 RID: 6549
	[Tooltip("The ID of the item to check")]
	public string itemId;

	// Token: 0x04001996 RID: 6550
	[Tooltip("The comparison operator to use")]
	public ComparisonOperator comparison;

	// Token: 0x04001997 RID: 6551
	[Tooltip("The target count to compare against")]
	public int targetCount;

	// Token: 0x04001998 RID: 6552
	[Tooltip("Second target count (used for Between comparisons)")]
	public int targetCount2;
}
