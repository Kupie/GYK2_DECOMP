using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200025D RID: 605
[Serializable]
public class AnimationColliderTriggerData : ColliderTriggerDataBase
{
	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0004EC53 File Offset: 0x0004CE53
	public static List<string> Triggers
	{
		get
		{
			return AnimationColliderTriggerComponent.triggerNamesArray;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0004EC5A File Offset: 0x0004CE5A
	// (set) Token: 0x06000F50 RID: 3920 RVA: 0x0004EC62 File Offset: 0x0004CE62
	public Animator Animator { get; set; }

	// Token: 0x06000F51 RID: 3921 RVA: 0x0004EC6B File Offset: 0x0004CE6B
	protected override bool IsSetupCompleted()
	{
		return this.Animator != null && !string.IsNullOrEmpty(this.trigger);
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x0004EC8B File Offset: 0x0004CE8B
	protected override void TriggerSetAction()
	{
		Debug.Log("AnimationColliderTriggerData Trigger:[" + this.trigger + "] animation");
		this.Animator.SetTrigger(this.trigger);
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x0004ECB8 File Offset: 0x0004CEB8
	protected override void TriggerResetAction()
	{
		this.Animator.ResetTrigger(this.trigger);
	}

	// Token: 0x04001235 RID: 4661
	[SerializeField]
	private string trigger;
}
