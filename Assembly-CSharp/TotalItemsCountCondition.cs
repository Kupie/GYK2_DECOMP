using System;
using UnityEngine;

// Token: 0x020003F3 RID: 1011
[Serializable]
public class TotalItemsCountCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000492 RID: 1170
	// (get) Token: 0x06001A83 RID: 6787 RVA: 0x0007B70C File Offset: 0x0007990C
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ItemsChanged;
		}
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x0007BB7A File Offset: 0x00079D7A
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.Inventory != null;
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x0007BB90 File Offset: 0x00079D90
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		int totalItemsCount = context.Inventory.Data.TotalItemsCount;
		if (!this.usePercentage)
		{
			return base.CompareValues(totalItemsCount, this.comparison, (int)this.targetValue, (int)this.targetValue2);
		}
		int num = this.CalculateTotalPossibleItemsCount(context);
		if (num == 0)
		{
			return false;
		}
		float num2 = (float)totalItemsCount / (float)num * 100f;
		return base.CompareValues(num2, this.comparison, this.targetValue, this.targetValue2);
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x0007BC04 File Offset: 0x00079E04
	private int CalculateTotalPossibleItemsCount(ConditionalDrawerContext context)
	{
		int num = 0;
		foreach (Item item in context.Inventory.Data.Inventory)
		{
			if (item.IsEmpty)
			{
				num += context.WgoData.Definition.emptyCellStackCount;
			}
			else
			{
				num += item.Definition.stackCount;
			}
		}
		if (context.Inventory.Data.Inventory.Count < context.Inventory.Data.InventorySize)
		{
			num += context.WgoData.Definition.emptyCellStackCount * (context.Inventory.Data.InventorySize - context.Inventory.Data.Inventory.Count);
		}
		return num;
	}

	// Token: 0x040019A9 RID: 6569
	[Tooltip("Whether to use percentage-based comparison instead of absolute count")]
	public bool usePercentage;

	// Token: 0x040019AA RID: 6570
	[Tooltip("The comparison operator to use")]
	public ComparisonOperator comparison;

	// Token: 0x040019AB RID: 6571
	[Tooltip("The target value to compare against (count or percentage)")]
	public float targetValue;

	// Token: 0x040019AC RID: 6572
	[Tooltip("Second target value (used for Between comparisons)")]
	public float targetValue2;
}
