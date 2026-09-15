using System;
using UnityEngine;

// Token: 0x020003EC RID: 1004
[Serializable]
public class ItemGroupInventoryIndexCondition : ConditionalDrawerConditionBase
{
	// Token: 0x1700048D RID: 1165
	// (get) Token: 0x06001A6E RID: 6766 RVA: 0x0007B70C File Offset: 0x0007990C
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ItemsChanged;
		}
	}

	// Token: 0x06001A6F RID: 6767 RVA: 0x0007B82C File Offset: 0x00079A2C
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.Inventory != null && !string.IsNullOrEmpty(this.itemGroupId);
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x0007B850 File Offset: 0x00079A50
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		Item itemByGroupId = context.Inventory.GetItemByGroupId(this.itemGroupId);
		return itemByGroupId != null && context.Inventory.Data.Inventory.IndexOf(itemByGroupId) == this.expectedIndex;
	}

	// Token: 0x0400199D RID: 6557
	[Tooltip("The group ID of items to check")]
	public string itemGroupId;

	// Token: 0x0400199E RID: 6558
	[Tooltip("The expected inventory index (0-based)")]
	public int expectedIndex;
}
