using System;
using UnityEngine;

// Token: 0x0200079E RID: 1950
public class ConveyorStorageSMB : StateMachineBehaviour
{
	// Token: 0x0600322A RID: 12842 RVA: 0x000F073C File Offset: 0x000EE93C
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Wgo componentInParent = animator.GetComponentInParent<Wgo>(true);
		if (componentInParent != null && this.animationIdToSet > -1 && componentInParent.Data.GetGameResInt("activateId") != this.animationIdToSet)
		{
			componentInParent.Data.SetGameRes("activateId", this.animationIdToSet);
		}
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x0400284C RID: 10316
	[SerializeField]
	private int animationIdToSet = -1;
}
