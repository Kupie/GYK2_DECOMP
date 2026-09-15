using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004D6 RID: 1238
[Name("Set State In Animator", 0)]
[Category("Game/Animation")]
public class Flow_SetStateInAnimator : GKCustomFlowNodeWithWgoData
{
	// Token: 0x06002099 RID: 8345 RVA: 0x0009A600 File Offset: 0x00098800
	protected override void RegisterPorts()
	{
		switch (this.animatorType)
		{
		case Flow_SetStateInAnimator.AnimatorType.Wgo:
			base.RegisterPorts();
			break;
		case Flow_SetStateInAnimator.AnimatorType.GameObject:
			this.gameObject = base.AddValueInput<GameObject>("gameObject".CapitalizeFirst(), "");
			break;
		}
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetTrigger), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.state = base.AddValueInput<global::AnimationState>("state".CapitalizeFirst(), "");
	}

	// Token: 0x0600209A RID: 8346 RVA: 0x0009A6A8 File Offset: 0x000988A8
	private void SetTrigger(Flow flow)
	{
		switch (this.animatorType)
		{
		case Flow_SetStateInAnimator.AnimatorType.Wgo:
			Debug.LogError("Set state doesn't work for WGO (WgoData).");
			break;
		case Flow_SetStateInAnimator.AnimatorType.GameObject:
		{
			GameObject gameObject = base.ParamValueOrSelf(this.gameObject);
			if (gameObject != null)
			{
				Animator component = gameObject.GetComponent<Animator>();
				if (component != null)
				{
					component.SetInteger(AnimationComponentBase.idStateAnimator, (int)this.state.value);
				}
				else
				{
					Debug.LogError("[Flow_SetStateInAnimator]: Animator not found");
				}
			}
			else
			{
				Debug.LogError("[Flow_SetStateInAnimator]: GameObject is null");
			}
			break;
		}
		case Flow_SetStateInAnimator.AnimatorType.Player:
			MainGame.PlayerController.View.PlayerAnimation.SetState(this.state.value);
			break;
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001D44 RID: 7492
	[FlowNode.GatherPortsCallbackAttribute]
	public Flow_SetStateInAnimator.AnimatorType animatorType;

	// Token: 0x04001D45 RID: 7493
	private FlowInput @in;

	// Token: 0x04001D46 RID: 7494
	private FlowOutput @out;

	// Token: 0x04001D47 RID: 7495
	private ValueInput<GameObject> gameObject;

	// Token: 0x04001D48 RID: 7496
	private ValueInput<global::AnimationState> state;

	// Token: 0x020004D7 RID: 1239
	public enum AnimatorType
	{
		// Token: 0x04001D4A RID: 7498
		Wgo,
		// Token: 0x04001D4B RID: 7499
		GameObject,
		// Token: 0x04001D4C RID: 7500
		Player
	}
}
