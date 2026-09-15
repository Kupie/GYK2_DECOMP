using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025C RID: 604
public class AnimationColliderTriggerComponent : ColliderTriggerComponentBase
{
	// Token: 0x06000F4B RID: 3915 RVA: 0x0004EB9E File Offset: 0x0004CD9E
	private void Awake()
	{
		this.onEnter.Animator = this.animator;
		this.onExit.Animator = this.animator;
		base.Init(this.onEnter, this.onExit);
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x0004EBD4 File Offset: 0x0004CDD4
	private void OnCustomInspectorGUI()
	{
		if (this.animator == null)
		{
			Debug.LogError("Animator not set!", this);
			return;
		}
		EditorAnimatorHelper.ScanAnimator(this.animator.gameObject, ref this.stateNames, ref this.triggerNames, ref this.paramNames, AnimatorControllerParameterType.Trigger);
		AnimationColliderTriggerComponent.triggerNamesArray = this.triggerNames;
	}

	// Token: 0x0400122E RID: 4654
	[SerializeField]
	private Animator animator;

	// Token: 0x0400122F RID: 4655
	[SerializeField]
	private AnimationColliderTriggerData onEnter;

	// Token: 0x04001230 RID: 4656
	[SerializeField]
	private AnimationColliderTriggerData onExit;

	// Token: 0x04001231 RID: 4657
	public static List<string> triggerNamesArray;

	// Token: 0x04001232 RID: 4658
	private List<string> stateNames = new List<string>();

	// Token: 0x04001233 RID: 4659
	private List<string> triggerNames = new List<string>();

	// Token: 0x04001234 RID: 4660
	private List<string> paramNames = new List<string>();
}
