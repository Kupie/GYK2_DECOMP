using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004DA RID: 1242
[Name("Set WGO Custom Animation Trigger", 0)]
[Category("Game/Animation")]
public class Flow_SetWgoCustomAnimationState : GKCustomFlowNodeWithWgoData
{
	// Token: 0x060020A0 RID: 8352 RVA: 0x0009A958 File Offset: 0x00098B58
	protected override void RegisterPorts()
	{
		base.RegisterPorts();
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetTrigger), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		if (!this.remove)
		{
			this.triggerName = base.AddValueInput<string>("triggerName".CapitalizeFirst(), "");
		}
	}

	// Token: 0x060020A1 RID: 8353 RVA: 0x0009A9D0 File Offset: 0x00098BD0
	private void SetTrigger(Flow flow)
	{
		WgoData wgoData = base.GetWgoData();
		if (wgoData != null)
		{
			string text = (this.remove ? "" : this.triggerName.value);
			wgoData.SetCustomAnimationTrigger(text);
		}
		else
		{
			Debug.LogError("Flow_SetTriggerToAnimator: cannot trigger [" + this.triggerName.value + "] on null WGO");
		}
		this.@out.Call(flow);
	}

	// Token: 0x1700055D RID: 1373
	// (get) Token: 0x060020A2 RID: 8354 RVA: 0x0009AA36 File Offset: 0x00098C36
	public override string name
	{
		get
		{
			return (this.remove ? "Remove" : "Set") + " WGO Custom Animation Trigger";
		}
	}

	// Token: 0x04001D58 RID: 7512
	[FlowNode.GatherPortsCallbackAttribute]
	public bool remove;

	// Token: 0x04001D59 RID: 7513
	private FlowInput @in;

	// Token: 0x04001D5A RID: 7514
	private FlowOutput @out;

	// Token: 0x04001D5B RID: 7515
	private ValueInput<string> triggerName;
}
