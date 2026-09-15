using System;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
public class MobWalkIdle : StateMachineBehaviour
{
	// Token: 0x06003238 RID: 12856 RVA: 0x000F0B2B File Offset: 0x000EED2B
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!this.animationComponent)
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
		}
		if (this.animationComponent != null)
		{
			this.animationComponent.StartZombieIdleSound();
		}
	}

	// Token: 0x04002855 RID: 10325
	private AnimationComponentBase animationComponent;
}
