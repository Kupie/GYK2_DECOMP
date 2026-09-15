using System;
using UnityEngine;

// Token: 0x020007A8 RID: 1960
public class ToolAnimationSMB : StateMachineBehaviour
{
	// Token: 0x06003243 RID: 12867 RVA: 0x000F0D58 File Offset: 0x000EEF58
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.animationComponent == null)
		{
			this.animationComponent = animator.GetComponentInParent<AnimationComponentBase>();
			this.hasAnimationComponent = this.animationComponent != null;
		}
		if (!this.hasAnimationComponent)
		{
			return;
		}
		base.OnStateEnter(animator, stateInfo, layerIndex);
		this.startedLoops = 0;
		this.completedLoops = 0;
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x000F0DB4 File Offset: 0x000EEFB4
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (!this.hasAnimationComponent)
		{
			return;
		}
		base.OnStateUpdate(animator, stateInfo, layerIndex);
		int num = Mathf.FloorToInt(stateInfo.normalizedTime);
		if (this.startedLoops <= num)
		{
			this.StartLoop();
		}
		if (num > this.completedLoops)
		{
			this.CompleteLoop();
		}
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x000F0DFE File Offset: 0x000EEFFE
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.startedLoops > this.completedLoops)
		{
			this.CompleteLoop();
		}
		this.animationComponent = null;
		this.hasAnimationComponent = false;
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x000F0E22 File Offset: 0x000EF022
	private void StartLoop()
	{
		this.startedLoops++;
		this.animationComponent.HandleLoopStarted(this);
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x000F0E3E File Offset: 0x000EF03E
	private void CompleteLoop()
	{
		this.completedLoops++;
		this.animationComponent.HandleLoopFinished(this);
	}

	// Token: 0x0400285C RID: 10332
	public ItemType itemType;

	// Token: 0x0400285D RID: 10333
	private int startedLoops;

	// Token: 0x0400285E RID: 10334
	private int completedLoops;

	// Token: 0x0400285F RID: 10335
	private AnimationComponentBase animationComponent;

	// Token: 0x04002860 RID: 10336
	private bool hasAnimationComponent;
}
