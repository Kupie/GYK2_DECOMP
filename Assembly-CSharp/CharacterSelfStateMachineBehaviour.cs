using System;
using UnityEngine;

// Token: 0x0200079D RID: 1949
public class CharacterSelfStateMachineBehaviour : StateMachineBehaviour
{
	// Token: 0x06003227 RID: 12839 RVA: 0x000F06E1 File Offset: 0x000EE8E1
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
		this.hasAnimationComponent = this.animationComponent != null;
		if (!this.hasAnimationComponent)
		{
			return;
		}
		MainGame.PlayerController.SetControlTakenType(TakenControlType.BySelf, false);
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x06003228 RID: 12840 RVA: 0x000F0720 File Offset: 0x000EE920
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.animationComponent.SetState(global::AnimationState.Idle);
		MainGame.PlayerController.SetControlTakenType(TakenControlType.BySelf, true);
	}

	// Token: 0x0400284A RID: 10314
	private AnimationComponentBase animationComponent;

	// Token: 0x0400284B RID: 10315
	private bool hasAnimationComponent;
}
