using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007A5 RID: 1957
public class Object3DAnimSMB : StateMachineBehaviour
{
	// Token: 0x0600323A RID: 12858 RVA: 0x000F0B60 File Offset: 0x000EED60
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.object3D == null)
		{
			this.object3D = animator.GetComponent<Object3D>();
		}
		if (this.object3D == null)
		{
			this.multiObject3DAnimatable = animator.GetComponent<MultiObject3DAnimatable>();
			this.object3Ds = new HashSet<Object3D>(this.multiObject3DAnimatable.Object3Ds);
		}
		if (this.object3D != null)
		{
			this.object3D.SetAnimatableState(true);
		}
		if (this.multiObject3DAnimatable != null)
		{
			foreach (Object3D object3D in this.object3Ds)
			{
				object3D.SetAnimatableState(true);
			}
		}
		base.OnStateEnter(animator, stateInfo, layerIndex);
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x000F0C2C File Offset: 0x000EEE2C
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.object3D != null)
		{
			this.object3D.SetAnimatableState(false);
		}
		if (this.multiObject3DAnimatable != null)
		{
			foreach (Object3D object3D in this.object3Ds)
			{
				object3D.SetAnimatableState(false);
			}
		}
		base.OnStateExit(animator, stateInfo, layerIndex);
	}

	// Token: 0x04002856 RID: 10326
	private Object3D object3D;

	// Token: 0x04002857 RID: 10327
	private MultiObject3DAnimatable multiObject3DAnimatable;

	// Token: 0x04002858 RID: 10328
	private HashSet<Object3D> object3Ds = new HashSet<Object3D>();
}
