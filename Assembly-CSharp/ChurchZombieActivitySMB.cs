using System;
using UnityEngine;

// Token: 0x02000187 RID: 391
public class ChurchZombieActivitySMB : StateMachineBehaviour
{
	// Token: 0x0600098A RID: 2442 RVA: 0x00030800 File Offset: 0x0002EA00
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		this.churchZombieActivity = animator.GetComponent<ChurchZombieActivity>();
		if (this.churchZombieActivity != null)
		{
			this.churchZombieActivity.StartAction();
		}
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x00030827 File Offset: 0x0002EA27
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		ChurchZombieActivity churchZombieActivity = this.churchZombieActivity;
		if (churchZombieActivity == null)
		{
			return;
		}
		churchZombieActivity.StopAction();
	}

	// Token: 0x04000B2A RID: 2858
	private ChurchZombieActivity churchZombieActivity;
}
