using System;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
public class Object3DStaticSMB : StateMachineBehaviour
{
	// Token: 0x0600323D RID: 12861 RVA: 0x000F0CC3 File Offset: 0x000EEEC3
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.object3D == null)
		{
			this.object3D = animator.GetComponent<Object3D>();
		}
		if (this.object3D != null)
		{
			this.object3D.ResetToStaticState();
		}
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x04002859 RID: 10329
	private Object3D object3D;
}
