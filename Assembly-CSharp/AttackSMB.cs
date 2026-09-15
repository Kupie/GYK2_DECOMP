using System;
using UnityEngine;

// Token: 0x02000799 RID: 1945
public class AttackSMB : StateMachineBehaviour
{
	// Token: 0x140000B0 RID: 176
	// (add) Token: 0x06003217 RID: 12823 RVA: 0x000F051C File Offset: 0x000EE71C
	// (remove) Token: 0x06003218 RID: 12824 RVA: 0x000F0550 File Offset: 0x000EE750
	public static event Action<AnimationComponentBase> OnAttackFinished;

	// Token: 0x06003219 RID: 12825 RVA: 0x000F0583 File Offset: 0x000EE783
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		if (this.animationComponent == null)
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
			this.hasAnimationComponent = this.animationComponent != null;
		}
	}

	// Token: 0x0600321A RID: 12826 RVA: 0x000F05BA File Offset: 0x000EE7BA
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);
		this.wasInterrupted = animator.IsInTransition(layerIndex);
	}

	// Token: 0x0600321B RID: 12827 RVA: 0x000F05D2 File Offset: 0x000EE7D2
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		if (!this.hasAnimationComponent)
		{
			return;
		}
		if (stateInfo.normalizedTime >= 1f)
		{
			bool flag = !this.wasInterrupted;
		}
	}

	// Token: 0x04002840 RID: 10304
	private AnimationComponentBase animationComponent;

	// Token: 0x04002841 RID: 10305
	private bool hasAnimationComponent;

	// Token: 0x04002842 RID: 10306
	private bool wasInterrupted;
}
