using System;
using UnityEngine;

// Token: 0x020007A0 RID: 1952
public class IdleSMB : StateMachineBehaviour
{
	// Token: 0x06003230 RID: 12848 RVA: 0x000F08CC File Offset: 0x000EEACC
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!this.isInitialized && !this.wgo)
		{
			this.wgo = animator.GetComponentInParent<Wgo>();
			this.isInitialized = true;
		}
		if (this.wgo && this.wgo.Data.wasCustomAnimationFired)
		{
			this.wgo.UpdateFlag(ChunkingIgnoreType.Animation, false);
		}
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x04002851 RID: 10321
	private Wgo wgo;

	// Token: 0x04002852 RID: 10322
	private bool isInitialized;
}
