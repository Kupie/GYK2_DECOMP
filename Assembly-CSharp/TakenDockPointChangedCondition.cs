using System;
using UnityEngine;

// Token: 0x020003F0 RID: 1008
[Serializable]
public class TakenDockPointChangedCondition : ConditionalDrawerConditionBase
{
	// Token: 0x17000490 RID: 1168
	// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0007BA9C File Offset: 0x00079C9C
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.TakenDockPointChanged;
		}
	}

	// Token: 0x06001A7D RID: 6781 RVA: 0x0007BAA3 File Offset: 0x00079CA3
	public override bool IsValid(ConditionalDrawerContext context)
	{
		return base.IsValid(context) && context.WgoData != null;
	}

	// Token: 0x06001A7E RID: 6782 RVA: 0x0007BABC File Offset: 0x00079CBC
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		if (this.eventOnly && !context.IsCalledFromEvent)
		{
			return false;
		}
		string text = string.Empty;
		if (!SGuid.IsNullOrEmpty(context.WgoData.takenDockPointsParentSGuid))
		{
			WgoData wgoData = MainGame.WorldData.GetWgoData(context.WgoData.takenDockPointsParentSGuid);
			text = ((wgoData != null) ? wgoData.id : null);
		}
		return !string.IsNullOrEmpty(this.takenDockPointParentWgoId) && !string.IsNullOrEmpty(text) && text == this.takenDockPointParentWgoId;
	}

	// Token: 0x040019A3 RID: 6563
	[Tooltip("The expected parent wgo id")]
	public string takenDockPointParentWgoId;

	// Token: 0x040019A4 RID: 6564
	public bool eventOnly;
}
