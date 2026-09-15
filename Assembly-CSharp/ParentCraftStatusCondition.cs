using System;
using UnityEngine;

// Token: 0x020003EE RID: 1006
[Serializable]
public class ParentCraftStatusCondition : ConditionalDrawerConditionBase
{
	// Token: 0x1700048E RID: 1166
	// (get) Token: 0x06001A72 RID: 6770 RVA: 0x0007B346 File Offset: 0x00079546
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.CraftStatusChanged;
		}
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x0007B2EB File Offset: 0x000794EB
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.CraftComponent != null;
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x0007B894 File Offset: 0x00079A94
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		if (this.eventOnly && !context.IsCalledFromEvent)
		{
			return false;
		}
		bool flag = false;
		foreach (SGuid sguid in context.WgoData.WorkbenchParents)
		{
			WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
			if (wgoData != null && wgoData.CraftComponent != null && wgoData.CraftComponent.Status == CraftComponentStatus.Started && !wgoData.CraftComponent.IsDestroyingCraftActive)
			{
				flag = true;
				break;
			}
		}
		return this.expectedStatus == ExpectedParentCraftStatus.Started && flag;
	}

	// Token: 0x040019A1 RID: 6561
	[Tooltip("The expected craft status")]
	public ExpectedParentCraftStatus expectedStatus;

	// Token: 0x040019A2 RID: 6562
	public bool eventOnly;
}
