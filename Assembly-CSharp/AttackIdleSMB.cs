using System;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02000798 RID: 1944
public class AttackIdleSMB : StateMachineBehaviour
{
	// Token: 0x06003213 RID: 12819 RVA: 0x000F044C File Offset: 0x000EE64C
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		if (!this.animationComponent)
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
		}
		SSMState curState = MainGame.PlayerController.Ssm.CurState;
		bool flag = curState is AttackSwordFocusedPlayerState || curState is AttackBowFocusedPlayerState;
		PlayerAnimation playerAnimation = this.animationComponent as PlayerAnimation;
		if (playerAnimation != null)
		{
			this.attackComponent = ((this.attackComponent == null) ? playerAnimation.GetComponentInParent<AttackComponent>() : this.attackComponent);
			if (!flag && this.attackComponent != null)
			{
				this.attackComponent.OnAttackAnimFinished(this.animationComponent);
			}
		}
	}

	// Token: 0x06003214 RID: 12820 RVA: 0x000F04F9 File Offset: 0x000EE6F9
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateUpdate(animator, stateInfo, layerIndex);
		this.wasInterrupted = animator.IsInTransition(layerIndex);
	}

	// Token: 0x06003215 RID: 12821 RVA: 0x000F0511 File Offset: 0x000EE711
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
	}

	// Token: 0x0400283C RID: 10300
	private AnimationComponentBase animationComponent;

	// Token: 0x0400283D RID: 10301
	[CanBeNull]
	private AttackComponent attackComponent;

	// Token: 0x0400283E RID: 10302
	private bool wasInterrupted;
}
