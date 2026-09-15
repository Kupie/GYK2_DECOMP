using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x020007A3 RID: 1955
public class MobAttackSMB : StateMachineBehaviour
{
	// Token: 0x06003236 RID: 12854 RVA: 0x000F0AA4 File Offset: 0x000EECA4
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		if (!this.animationComponent)
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
		}
		if (this.animationComponent != null)
		{
			if (!this.attackComponent)
			{
				this.attackComponent = this.animationComponent.GetComponentInParent<AttackComponent>();
			}
			if (!this.attackComponent || !this.attackComponent.IsInitialized)
			{
				return;
			}
			this.attackComponent.OnAttackAnimFinished(this.animationComponent);
		}
	}

	// Token: 0x04002853 RID: 10323
	private AnimationComponentBase animationComponent;

	// Token: 0x04002854 RID: 10324
	[CanBeNull]
	private AttackComponent attackComponent;
}
