using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EB RID: 1003
[Serializable]
public class ItemGroupCountCondition : ConditionalDrawerConditionBase
{
	// Token: 0x1700048C RID: 1164
	// (get) Token: 0x06001A6A RID: 6762 RVA: 0x0007B70C File Offset: 0x0007990C
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ItemsChanged;
		}
	}

	// Token: 0x06001A6B RID: 6763 RVA: 0x0007B778 File Offset: 0x00079978
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.Inventory != null && !string.IsNullOrEmpty(this.itemGroupId);
	}

	// Token: 0x06001A6C RID: 6764 RVA: 0x0007B79C File Offset: 0x0007999C
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		if (context.Inventory == null)
		{
			Debug.LogError("[ItemGroupCountCondition]: Inventory is null");
			return false;
		}
		List<Item> itemsByGroupId = context.Inventory.GetItemsByGroupId(this.itemGroupId);
		int num = 0;
		foreach (Item item in itemsByGroupId)
		{
			num += item.Count;
		}
		return base.CompareValues(num, this.comparison, this.targetCount, this.targetCount2);
	}

	// Token: 0x04001999 RID: 6553
	[Tooltip("The group ID of items to check")]
	public string itemGroupId;

	// Token: 0x0400199A RID: 6554
	[Tooltip("The comparison operator to use")]
	public ComparisonOperator comparison;

	// Token: 0x0400199B RID: 6555
	[Tooltip("The target count to compare against")]
	public int targetCount;

	// Token: 0x0400199C RID: 6556
	[Tooltip("Second target count (used for Between comparisons)")]
	public int targetCount2;
}
