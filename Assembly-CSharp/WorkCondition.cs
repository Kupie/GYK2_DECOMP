using System;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
[Serializable]
public class WorkCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000493 RID: 1171
	// (get) Token: 0x06001A88 RID: 6792 RVA: 0x0007BCEC File Offset: 0x00079EEC
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.WorkStateChanged;
		}
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x0007BCF0 File Offset: 0x00079EF0
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		CraftComponent craftComponent = context.CraftComponent;
		bool flag = false;
		if (craftComponent != null)
		{
			flag = craftComponent.IsDestroyingCraftActive;
		}
		bool flag2;
		switch (this.conditionType)
		{
		case WorkConditionType.IsInWork:
			flag2 = context.IsInWork && !flag;
			break;
		case WorkConditionType.IsNotInWork:
			flag2 = !context.IsInWork || flag;
			break;
		case WorkConditionType.HasWorker:
			flag2 = context.HasWorker;
			break;
		case WorkConditionType.HasNoWorker:
			flag2 = !context.HasWorker;
			break;
		default:
			flag2 = false;
			break;
		}
		return flag2;
	}

	// Token: 0x040019B2 RID: 6578
	[Tooltip("The type of work condition to check")]
	public WorkConditionType conditionType;
}
