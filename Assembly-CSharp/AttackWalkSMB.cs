using System;
using UnityEngine;

// Token: 0x0200079B RID: 1947
public class AttackWalkSMB : StateMachineBehaviour
{
	// Token: 0x06003221 RID: 12833 RVA: 0x000F0696 File Offset: 0x000EE896
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x06003222 RID: 12834 RVA: 0x000F06A1 File Offset: 0x000EE8A1
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);
		this.wasInterrupted = animator.IsInTransition(layerIndex);
	}

	// Token: 0x04002846 RID: 10310
	private AnimationComponentBase animationComponent;

	// Token: 0x04002847 RID: 10311
	private bool hasAnimationComponent;

	// Token: 0x04002848 RID: 10312
	private bool wasInterrupted;
}
