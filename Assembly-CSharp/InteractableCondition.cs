using System;
using UnityEngine;

// Token: 0x020003E9 RID: 1001
[Serializable]
public class InteractableCondition : ConditionalDrawerConditionBase
{
	// Token: 0x1700048A RID: 1162
	// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0007B6E6 File Offset: 0x000798E6
	public override ConditionalEventType EventType
	{
		get
		{
			return ConditionalEventType.InteractableStateChanged;
		}
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x0007B6ED File Offset: 0x000798ED
	public override bool Evaluate(ConditionalDrawerContext context)
	{
		return context.WgoData != null && this.isInteractable == context.WgoData.IsInteractable;
	}

	// Token: 0x04001994 RID: 6548
	[Tooltip("The type of interact state condition to check")]
	public bool isInteractable;
}
