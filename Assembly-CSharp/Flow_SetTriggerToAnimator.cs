using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
[Name("Set Trigger To Animator", 0)]
[Category("Game/Animation")]
public class Flow_SetTriggerToAnimator : GKCustomFlowNodeWithWgoData
{
	// Token: 0x0600209C RID: 8348 RVA: 0x0009A760 File Offset: 0x00098960
	protected override void RegisterPorts()
	{
		switch (this.animatorType)
		{
		case Flow_SetTriggerToAnimator.AnimatorType.Wgo:
			base.RegisterPorts();
			break;
		case Flow_SetTriggerToAnimator.AnimatorType.GameObject:
			this.gameObject = base.AddValueInput<GameObject>("gameObject".CapitalizeFirst(), "");
			break;
		}
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetTrigger), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		if (this.setState)
		{
			this.animationState = base.AddValueInput<global::AnimationState>("animationState".CapitalizeFirst(), "");
			return;
		}
		this.triggerName = base.AddValueInput<string>("triggerName".CapitalizeFirst(), "");
	}

	// Token: 0x0600209D RID: 8349 RVA: 0x0009A82C File Offset: 0x00098A2C
	private void SetTrigger(Flow flow)
	{
		switch (this.animatorType)
		{
		case Flow_SetTriggerToAnimator.AnimatorType.Wgo:
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				if (this.setState)
				{
					wgoData.SetStateToAnimator(this.animationState.value);
				}
				else
				{
					wgoData.SetTriggerToAnimator(this.triggerName.value);
				}
			}
			else
			{
				Debug.LogError("Flow_SetTriggerToAnimator: cannot trigger [" + this.triggerName.value + "] on null WGO");
			}
			break;
		}
		case Flow_SetTriggerToAnimator.AnimatorType.GameObject:
		{
			GameObject gameObject = base.ParamValueOrSelf(this.gameObject);
			if (gameObject != null)
			{
				Animator component = gameObject.GetComponent<Animator>();
				if (component != null)
				{
					component.SetTrigger(this.triggerName.value);
				}
				else
				{
					Debug.LogError("[Flow_SetTriggerToAnimator]: Animator not found");
				}
			}
			else
			{
				Debug.LogError("[Flow_SetTriggerToAnimator]: GameObject is null");
			}
			break;
		}
		case Flow_SetTriggerToAnimator.AnimatorType.Player:
			MainGame.PlayerController.View.PlayerAnimation.SetTrigger(this.triggerName.value);
			break;
		}
		this.@out.Call(flow);
	}

	// Token: 0x1700055C RID: 1372
	// (get) Token: 0x0600209E RID: 8350 RVA: 0x0009A932 File Offset: 0x00098B32
	public override string name
	{
		get
		{
			return "Set" + (this.setState ? " State In" : " Trigger To") + " Animator";
		}
	}

	// Token: 0x04001D4D RID: 7501
	[FlowNode.GatherPortsCallbackAttribute]
	public Flow_SetTriggerToAnimator.AnimatorType animatorType;

	// Token: 0x04001D4E RID: 7502
	[FlowNode.GatherPortsCallbackAttribute]
	[ShowIf("animatorType", 0)]
	public bool setState;

	// Token: 0x04001D4F RID: 7503
	private FlowInput @in;

	// Token: 0x04001D50 RID: 7504
	private FlowOutput @out;

	// Token: 0x04001D51 RID: 7505
	private ValueInput<GameObject> gameObject;

	// Token: 0x04001D52 RID: 7506
	private ValueInput<string> triggerName;

	// Token: 0x04001D53 RID: 7507
	private ValueInput<global::AnimationState> animationState;

	// Token: 0x020004D9 RID: 1241
	public enum AnimatorType
	{
		// Token: 0x04001D55 RID: 7509
		Wgo,
		// Token: 0x04001D56 RID: 7510
		GameObject,
		// Token: 0x04001D57 RID: 7511
		Player
	}
}
