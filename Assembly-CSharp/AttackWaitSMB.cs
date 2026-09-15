using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200079A RID: 1946
public class AttackWaitSMB : StateMachineBehaviour
{
	// Token: 0x0600321D RID: 12829 RVA: 0x000F0604 File Offset: 0x000EE804
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		if (!this.animationComponent)
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
		}
		PlayerAnimation playerAnimation = this.animationComponent as PlayerAnimation;
		if (playerAnimation != null && MainGame.PlayerController.PlayerInputHandler.meleeMode == MeleeMode.Continuous)
		{
			if (!this.attackComponent)
			{
				this.attackComponent = playerAnimation.GetComponentInParent<AttackComponent>();
			}
			this.attackComponent.OnAttackAnimFinished(this.animationComponent);
		}
	}

	// Token: 0x0600321E RID: 12830 RVA: 0x000F067E File Offset: 0x000EE87E
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);
		this.wasInterrupted = animator.IsInTransition(layerIndex);
	}

	// Token: 0x0600321F RID: 12831 RVA: 0x000F0511 File Offset: 0x000EE711
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
	}

	// Token: 0x04002843 RID: 10307
	private AnimationComponentBase animationComponent;

	// Token: 0x04002844 RID: 10308
	[CanBeNull]
	private AttackComponent attackComponent;

	// Token: 0x04002845 RID: 10309
	private bool wasInterrupted;
}
