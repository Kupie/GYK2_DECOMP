using System;
using UnityEngine;

// Token: 0x0200079C RID: 1948
public class BowShootSMB : StateMachineBehaviour
{
	// Token: 0x06003224 RID: 12836 RVA: 0x000F06B9 File Offset: 0x000EE8B9
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateExit(animator, stateInfo, layerIndex);
		animator.SetBool(BowShootSMB.BowShoot, false);
	}

	// Token: 0x04002849 RID: 10313
	private static readonly int BowShoot = Animator.StringToHash("bow_shoot");
}
