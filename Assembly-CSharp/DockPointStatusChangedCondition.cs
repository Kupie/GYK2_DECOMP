using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003E2 RID: 994
[Serializable]
public class DockPointStatusChangedCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000485 RID: 1157
	// (get) Token: 0x06001A4F RID: 6735 RVA: 0x0007B3CA File Offset: 0x000795CA
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.DockPointStatusChanged;
		}
	}

	// Token: 0x06001A50 RID: 6736 RVA: 0x0007B3D1 File Offset: 0x000795D1
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.WgoData != null && context.WgoData.MainWgoPartData != null;
	}

	// Token: 0x06001A51 RID: 6737 RVA: 0x0007B3F4 File Offset: 0x000795F4
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		if (this.eventOnly && !context.IsCalledFromEvent)
		{
			return false;
		}
		bool flag = false;
		using (List<DockPointData>.Enumerator enumerator = context.WgoData.MainWgoPartData.DockPointDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.IsOccupied)
				{
					flag = true;
					break;
				}
			}
		}
		return this.expectedStatus == ExpectedDockPointStatus.IsAnyOccupied && flag;
	}

	// Token: 0x0400197F RID: 6527
	[Tooltip("The expected craft status")]
	public ExpectedDockPointStatus expectedStatus;

	// Token: 0x04001980 RID: 6528
	public bool eventOnly;
}
