using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DB RID: 987
[Serializable]
public class ConveyorCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000481 RID: 1153
	// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0007B1CD File Offset: 0x000793CD
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.ConveyorChanged;
		}
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0007B1D1 File Offset: 0x000793D1
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.ConveyorComponent != null;
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x0007B1E8 File Offset: 0x000793E8
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		bool flag = this.HasConnectedWgoToDirection(context);
		ConveyorConditionType conveyorConditionType = this.conditionType;
		bool flag2;
		if (conveyorConditionType != ConveyorConditionType.HasConnectedToDirection)
		{
			flag2 = conveyorConditionType == ConveyorConditionType.HasNotConnectedToDirection && !flag;
		}
		else
		{
			flag2 = flag;
		}
		return flag2;
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0007B21C File Offset: 0x0007941C
	private bool HasConnectedWgoToDirection(ConditionalDrawerContext context)
	{
		using (List<Direction>.Enumerator enumerator = context.ConveyorComponent.occupiedConnectorsDirections.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current == (Direction)this.direction)
				{
					if (!this.requiresInOutFlag)
					{
						return true;
					}
					return this.isIn ? context.ConveyorComponent.HasParentsInDirection((Direction)this.direction) : context.ConveyorComponent.HasChildsInDirection((Direction)this.direction);
				}
			}
		}
		return false;
	}

	// Token: 0x0400196D RID: 6509
	[Tooltip("The type of conveyor condition to check")]
	public ConveyorConditionType conditionType;

	// Token: 0x0400196E RID: 6510
	[Tooltip("The direction to check (as integer value of Direction enum)")]
	public int direction;

	// Token: 0x0400196F RID: 6511
	[Tooltip("Should check if connection leads to in(parent) element or out(child) element")]
	public bool requiresInOutFlag;

	// Token: 0x04001970 RID: 6512
	[Tooltip("True - Connection leads to in(parent) element. False - Connection leads to out(child) element")]
	public bool isIn;
}
