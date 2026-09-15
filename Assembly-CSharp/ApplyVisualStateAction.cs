using System;
using UnityEngine;

// Token: 0x020003CC RID: 972
[Serializable]
public class ApplyVisualStateAction : ConditionalDrawerActionBase
{
	// Token: 0x060019FB RID: 6651 RVA: 0x00079FD4 File Offset: 0x000781D4
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (string.IsNullOrEmpty(this.stateId))
		{
			return;
		}
		if (!(this.applyOnFalse ? (!conditionMet) : conditionMet))
		{
			return;
		}
		WgoPartState currentWgoPartState = context.WgoPart.CurrentWgoPartState;
		if (context.WgoPart.WgoPartData == null)
		{
			return;
		}
		int num = ((currentWgoPartState != null) ? currentWgoPartState.rotationIndex : (-1));
		context.WgoPart.ApplyWgoPartState(this.stateId, num);
	}

	// Token: 0x04001949 RID: 6473
	[Tooltip("The ID of the visual state to apply")]
	public string stateId;

	// Token: 0x0400194A RID: 6474
	[Tooltip("If true, only applies when condition is false")]
	public bool applyOnFalse;
}
