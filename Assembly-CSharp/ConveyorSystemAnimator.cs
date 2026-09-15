using System;
using UnityEngine;

// Token: 0x020005FF RID: 1535
public abstract class ConveyorSystemAnimator : MonoBehaviour
{
	// Token: 0x170006A8 RID: 1704
	// (get) Token: 0x0600296A RID: 10602 RVA: 0x000C33DC File Offset: 0x000C15DC
	// (set) Token: 0x0600296B RID: 10603 RVA: 0x000C33E4 File Offset: 0x000C15E4
	public string CurrentState
	{
		get
		{
			return this.currentState;
		}
		set
		{
			if (this.currentState == value)
			{
				return;
			}
			this.currentState = value;
			this.MarkPlaybackDirty();
		}
	}

	// Token: 0x0600296C RID: 10604 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnOrchestratorRegistered(string globalState, float phase)
	{
	}

	// Token: 0x0600296D RID: 10605 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void MarkPlaybackDirty()
	{
	}

	// Token: 0x0600296E RID: 10606
	public abstract void CustomUpdate();

	// Token: 0x0600296F RID: 10607
	public abstract void Play(float phase);

	// Token: 0x06002970 RID: 10608
	public abstract void UpdateView();

	// Token: 0x04002260 RID: 8800
	[SerializeField]
	protected Animator animator;

	// Token: 0x04002261 RID: 8801
	protected bool isItemIdleLocked;

	// Token: 0x04002262 RID: 8802
	protected bool isAdditionalItemIdleLocked;

	// Token: 0x04002263 RID: 8803
	private string currentState = "Idle";
}
