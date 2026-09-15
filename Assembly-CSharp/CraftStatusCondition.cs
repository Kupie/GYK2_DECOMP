using System;
using UnityEngine;

// Token: 0x020003E0 RID: 992
[Serializable]
public class CraftStatusCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000484 RID: 1156
	// (get) Token: 0x06001A4B RID: 6731 RVA: 0x0007B346 File Offset: 0x00079546
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.CraftStatusChanged;
		}
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x0007B2EB File Offset: 0x000794EB
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.CraftComponent != null;
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0007B34C File Offset: 0x0007954C
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		CraftComponentStatus status = context.CraftComponent.Status;
		if (context.CraftComponent.IsDestroyingCraftActive)
		{
			return false;
		}
		if (this.eventOnly && !context.IsCalledFromEvent)
		{
			return false;
		}
		bool flag;
		switch (this.expectedStatus)
		{
		case ExpectedCraftStatus.Started:
			flag = status == CraftComponentStatus.Started;
			break;
		case ExpectedCraftStatus.Finished:
			flag = status == CraftComponentStatus.Finished && !context.CraftComponent.IsRemovingDestroyCraft;
			break;
		case ExpectedCraftStatus.ReadyToFinishAutoCraft:
			flag = status == CraftComponentStatus.ReadyToFinishAutoCraft;
			break;
		default:
			flag = false;
			break;
		}
		return flag;
	}

	// Token: 0x0400197B RID: 6523
	[Tooltip("The expected craft status")]
	public ExpectedCraftStatus expectedStatus;

	// Token: 0x0400197C RID: 6524
	public bool eventOnly;
}
