using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003FA RID: 1018
[Serializable]
public class ConditionalDrawerRule
{
	// Token: 0x170004A5 RID: 1189
	// (get) Token: 0x06001AAC RID: 6828 RVA: 0x0007C1A3 File Offset: 0x0007A3A3
	public ConditionalEventType EventType
	{
		get
		{
			IConditionalDrawerCondition conditionalDrawerCondition = this.condition;
			if (conditionalDrawerCondition == null)
			{
				return ConditionalEventType.None;
			}
			return conditionalDrawerCondition.EventType;
		}
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x0007C1B8 File Offset: 0x0007A3B8
	public void Evaluate(ConditionalDrawerContext context)
	{
		if (this.condition == null || !this.condition.IsValid(context))
		{
			return;
		}
		bool flag = this.condition.Evaluate(context);
		foreach (IConditionalDrawerAction conditionalDrawerAction in this.actions)
		{
			if (conditionalDrawerAction != null)
			{
				conditionalDrawerAction.Execute(context, flag);
			}
		}
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x0007C234 File Offset: 0x0007A434
	public void Reset(ConditionalDrawerContext context)
	{
		foreach (IConditionalDrawerAction conditionalDrawerAction in this.actions)
		{
			if (conditionalDrawerAction != null)
			{
				conditionalDrawerAction.Reset(context);
			}
		}
	}

	// Token: 0x040019C3 RID: 6595
	[SerializeReference]
	public IConditionalDrawerCondition condition;

	// Token: 0x040019C4 RID: 6596
	[SerializeReference]
	public List<IConditionalDrawerAction> actions = new List<IConditionalDrawerAction>();
}
