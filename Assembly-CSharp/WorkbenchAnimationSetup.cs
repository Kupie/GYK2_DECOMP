using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000002 RID: 2
public static class WorkbenchAnimationSetup
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public static ConditionalDrawerRule CreateCraftAnimationRule(Animator animator)
	{
		CraftStatusCondition craftStatusCondition = new CraftStatusCondition
		{
			expectedStatus = ExpectedCraftStatus.Started
		};
		SetAnimationStateAction setAnimationStateAction = new SetAnimationStateAction
		{
			animator = animator,
			parameterId = "state",
			stateValueOnTrue = 1,
			stateValueOnFalse = 0
		};
		return new ConditionalDrawerRule
		{
			condition = craftStatusCondition,
			actions = new List<IConditionalDrawerAction> { setAnimationStateAction }
		};
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000020B0 File Offset: 0x000002B0
	public static ConditionalDrawer SetupWorkbenchAnimation(WgoPart wgoPart, Animator animator)
	{
		if (wgoPart == null || animator == null)
		{
			Debug.LogError("[WorkbenchAnimationSetup] WgoPart or Animator is null");
			return null;
		}
		ConditionalDrawer conditionalDrawer = wgoPart.GetComponent<ConditionalDrawer>();
		if (conditionalDrawer == null)
		{
			conditionalDrawer = wgoPart.gameObject.AddComponent<ConditionalDrawer>();
		}
		ConditionalDrawerRule conditionalDrawerRule = WorkbenchAnimationSetup.CreateCraftAnimationRule(animator);
		conditionalDrawer.AddRule(conditionalDrawerRule);
		return conditionalDrawer;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002108 File Offset: 0x00000308
	public static bool ValidateAnimator(Animator animator)
	{
		if (animator == null || animator.runtimeAnimatorController == null)
		{
			return false;
		}
		foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
		{
			if (animatorControllerParameter.name == "state" && animatorControllerParameter.type == AnimatorControllerParameterType.Int)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000001 RID: 1
	public const string STATE_PARAMETER = "state";

	// Token: 0x04000002 RID: 2
	public const int STATE_IDLE = 0;

	// Token: 0x04000003 RID: 3
	public const int STATE_WORK = 1;
}
