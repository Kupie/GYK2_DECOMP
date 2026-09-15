using System;
using UnityEngine;
using UnityEngine.Animations;

// Token: 0x020007A7 RID: 1959
public class SwordHitBoxNotifierSMB : StateMachineBehaviour
{
	// Token: 0x0600323F RID: 12863 RVA: 0x000F0D01 File Offset: 0x000EEF01
	private HitStatesAccumulator GetHitStatesAccumulator(Animator animator)
	{
		if (this.hitStatesAccumulator != null)
		{
			return this.hitStatesAccumulator;
		}
		this.hitStatesAccumulator = animator.GetComponentInChildren<HitStatesAccumulator>(true);
		return this.hitStatesAccumulator;
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x000F0D2B File Offset: 0x000EEF2B
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex, controller);
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x000F0D38 File Offset: 0x000EEF38
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		HitStatesAccumulator hitStatesAccumulator = this.GetHitStatesAccumulator(animator);
		if (hitStatesAccumulator != null)
		{
			hitStatesAccumulator.Clear();
		}
		base.OnStateExit(animator, stateInfo, layerIndex);
	}

	// Token: 0x0400285A RID: 10330
	[SerializeField]
	private bool isReverseAnimationPlay;

	// Token: 0x0400285B RID: 10331
	private HitStatesAccumulator hitStatesAccumulator;
}
