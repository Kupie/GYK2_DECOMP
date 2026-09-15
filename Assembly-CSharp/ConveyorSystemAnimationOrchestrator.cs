using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class ConveyorSystemAnimationOrchestrator : LazySingleton<ConveyorSystemAnimationOrchestrator>
{
	// Token: 0x06002972 RID: 10610 RVA: 0x000C3415 File Offset: 0x000C1615
	public bool TryAddAnimator(ConveyorSystemAnimator animator)
	{
		if (!this.conveyorAnimatorSet.Add(animator))
		{
			return false;
		}
		this.conveyorAnimators.Add(animator);
		animator.OnOrchestratorRegistered(this.globalState, this.phase);
		animator.UpdateView();
		return true;
	}

	// Token: 0x06002973 RID: 10611 RVA: 0x000C344C File Offset: 0x000C164C
	public void RemoveAnimator(ConveyorSystemAnimator animator)
	{
		if (!this.conveyorAnimatorSet.Remove(animator))
		{
			return;
		}
		this.conveyorAnimators.Remove(animator);
	}

	// Token: 0x06002974 RID: 10612 RVA: 0x000C346C File Offset: 0x000C166C
	private void Update()
	{
		if (MainGame.IsGamePaused)
		{
			return;
		}
		for (int i = this.conveyorAnimators.Count - 1; i >= 0; i--)
		{
			ConveyorSystemAnimator conveyorSystemAnimator = this.conveyorAnimators[i];
			if (!conveyorSystemAnimator)
			{
				this.conveyorAnimatorSet.Remove(conveyorSystemAnimator);
				this.conveyorAnimators.RemoveAt(i);
			}
			else if (conveyorSystemAnimator.gameObject.activeInHierarchy)
			{
				if (this.requiresViewUpdate)
				{
					conveyorSystemAnimator.UpdateView();
				}
				conveyorSystemAnimator.CustomUpdate();
			}
		}
		this.requiresViewUpdate = false;
		this.clock += (this.unscaled ? Time.unscaledDeltaTime : Time.deltaTime) * this.speed;
		this.phase = this.clock % this.clipLength / this.clipLength;
		if (this.prevPhaseValue > this.phase)
		{
			if (this.globalState == "Out")
			{
				this.SetState("In");
			}
			else if (this.globalState == "In")
			{
				this.SetState("Idle");
			}
		}
		this.prevPhaseValue = this.phase;
		for (int j = 0; j < this.conveyorAnimators.Count; j++)
		{
			ConveyorSystemAnimator conveyorSystemAnimator2 = this.conveyorAnimators[j];
			if (conveyorSystemAnimator2 && conveyorSystemAnimator2.gameObject.activeInHierarchy)
			{
				conveyorSystemAnimator2.Play(this.phase);
			}
		}
	}

	// Token: 0x06002975 RID: 10613 RVA: 0x000C35D0 File Offset: 0x000C17D0
	public void SetState(string name)
	{
		this.globalState = name;
		if (this.globalState == "Idle")
		{
			this.clipLength = this.idleAnimationClip.length;
		}
		else if (this.globalState == "In")
		{
			this.clipLength = this.inAnimationClip.length;
		}
		else if (this.globalState == "Out")
		{
			this.clipLength = this.outAnimationClip.length;
		}
		this.phase = 0f;
		this.clock = 0f;
		this.prevPhaseValue = 0f;
		this.requiresViewUpdate = true;
		foreach (ConveyorSystemAnimator conveyorSystemAnimator in this.conveyorAnimators)
		{
			if (conveyorSystemAnimator)
			{
				conveyorSystemAnimator.CurrentState = name;
				conveyorSystemAnimator.MarkPlaybackDirty();
			}
		}
	}

	// Token: 0x04002266 RID: 8806
	public const string IDLE_STATE_NAME = "Idle";

	// Token: 0x04002267 RID: 8807
	public const string OUT_STATE_NAME = "Out";

	// Token: 0x04002268 RID: 8808
	public const string IN_STATE_NAME = "In";

	// Token: 0x04002269 RID: 8809
	[SerializeField]
	[ReadOnly]
	private List<ConveyorSystemAnimator> conveyorAnimators = new List<ConveyorSystemAnimator>();

	// Token: 0x0400226A RID: 8810
	private readonly HashSet<ConveyorSystemAnimator> conveyorAnimatorSet = new HashSet<ConveyorSystemAnimator>();

	// Token: 0x0400226B RID: 8811
	[SerializeField]
	private AnimationClip idleAnimationClip;

	// Token: 0x0400226C RID: 8812
	[SerializeField]
	private AnimationClip inAnimationClip;

	// Token: 0x0400226D RID: 8813
	[SerializeField]
	private AnimationClip outAnimationClip;

	// Token: 0x0400226E RID: 8814
	[SerializeField]
	private string globalState = "Idle";

	// Token: 0x0400226F RID: 8815
	[SerializeField]
	public bool unscaled;

	// Token: 0x04002270 RID: 8816
	[SerializeField]
	public float speed = 1f;

	// Token: 0x04002271 RID: 8817
	private float clock;

	// Token: 0x04002272 RID: 8818
	private float prevPhaseValue;

	// Token: 0x04002273 RID: 8819
	private float clipLength = 1f;

	// Token: 0x04002274 RID: 8820
	private float phase;

	// Token: 0x04002275 RID: 8821
	private bool requiresViewUpdate;
}
