using System;
using UnityEngine;

// Token: 0x020007A2 RID: 1954
public class LiftCraneIdleSMB : StateMachineBehaviour
{
	// Token: 0x06003234 RID: 12852 RVA: 0x000F0A44 File Offset: 0x000EEC44
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		base.OnStateEnter(animator, stateInfo, layerIndex);
		Wgo componentInParent = animator.gameObject.GetComponentInParent<Wgo>();
		if (componentInParent == null)
		{
			Debug.LogError("[LiftCraneIdleSMB] No WGO found for crane " + animator.gameObject.name);
			return;
		}
		componentInParent.Data.GameResStr.Set("target_storage_wgo", string.Empty);
	}
}
