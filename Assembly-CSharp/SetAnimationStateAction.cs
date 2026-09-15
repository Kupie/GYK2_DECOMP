using System;
using UnityEngine;

// Token: 0x020003D2 RID: 978
[Serializable]
public class SetAnimationStateAction : ConditionalDrawerActionBase
{
	// Token: 0x06001A08 RID: 6664 RVA: 0x0007A1FC File Offset: 0x000783FC
	public override void Execute(ConditionalDrawerContext context, bool conditionMet)
	{
		if (!this.CanApplyAnimation())
		{
			return;
		}
		if (!conditionMet && this.skipOnConditionNotMet)
		{
			return;
		}
		int num = (conditionMet ? this.stateValueOnTrue : this.stateValueOnFalse);
		AnimationParamType animationParamType = this.parameterType;
		if (animationParamType != AnimationParamType.Integer)
		{
			if (animationParamType == AnimationParamType.Bool)
			{
				this.animator.SetBool(this.parameterId, conditionMet);
				return;
			}
		}
		else
		{
			this.animator.SetInteger(this.parameterId, num);
		}
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x0007A263 File Offset: 0x00078463
	public override void Reset(ConditionalDrawerContext context)
	{
		if (!this.CanApplyAnimation())
		{
			return;
		}
		this.animator.SetInteger(this.parameterId, this.stateValueOnFalse);
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x0007A285 File Offset: 0x00078485
	private bool CanApplyAnimation()
	{
		return this.animator != null && this.animator.isActiveAndEnabled && this.animator.runtimeAnimatorController != null && !string.IsNullOrEmpty(this.parameterId);
	}

	// Token: 0x04001956 RID: 6486
	[Tooltip("The Animator component to control")]
	public Animator animator;

	// Token: 0x04001957 RID: 6487
	[Tooltip("Name of parameter to set")]
	public string parameterId;

	// Token: 0x04001958 RID: 6488
	[Tooltip("Type of the parameter to set")]
	public AnimationParamType parameterType;

	// Token: 0x04001959 RID: 6489
	[Tooltip("Value to set when condition is true")]
	public int stateValueOnTrue;

	// Token: 0x0400195A RID: 6490
	[Tooltip("Value to set when condition is false")]
	public int stateValueOnFalse;

	// Token: 0x0400195B RID: 6491
	[Tooltip("Do nothing if condition not met")]
	public bool skipOnConditionNotMet;
}
