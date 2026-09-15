using System;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public class FishGfxTransitionStateSMB : StateMachineBehaviour
{
	// Token: 0x0600322C RID: 12844 RVA: 0x000F07AC File Offset: 0x000EE9AC
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		this.isReversing = false;
		if (!this.fishGfx)
		{
			this.fishGfx = animator.GetComponentInParent<FishUnderwaterGfx>();
		}
		if (this.fishGfx)
		{
			this.stateOnEnter = this.fishGfx.FishState;
		}
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x000F0800 File Offset: 0x000EEA00
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		this.stateOnEnter = FishUnderwaterGfx.FishGfxState.None;
		if (this.isReversing)
		{
			animator.speed = 1f;
		}
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x000F0828 File Offset: 0x000EEA28
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.fishGfx && this.stateOnEnter != this.fishGfx.FishState)
		{
			this.stateOnEnter = this.fishGfx.FishState;
			if (!this.isReversing)
			{
				this.isReversing = true;
				animator.Play(stateInfo.fullPathHash, layerIndex, Mathf.Clamp01(this.reverseStartNormalizedTime));
				animator.speed = -1f;
			}
			else
			{
				this.isReversing = false;
				animator.speed = 1f;
			}
		}
		base.OnStateUpdate(animator, stateInfo, layerIndex);
	}

	// Token: 0x0400284D RID: 10317
	private FishUnderwaterGfx fishGfx;

	// Token: 0x0400284E RID: 10318
	private FishUnderwaterGfx.FishGfxState stateOnEnter;

	// Token: 0x0400284F RID: 10319
	[SerializeField]
	[Range(0f, 1f)]
	private float reverseStartNormalizedTime = 1f;

	// Token: 0x04002850 RID: 10320
	private bool isReversing;
}
