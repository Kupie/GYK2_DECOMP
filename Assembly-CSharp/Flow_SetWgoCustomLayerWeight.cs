using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004DB RID: 1243
[Name("Set WGO Custom Layer Weight", 0)]
[Category("Game/Animation")]
public class Flow_SetWgoCustomLayerWeight : GKCustomFlowNodeWithWgoData
{
	// Token: 0x060020A4 RID: 8356 RVA: 0x0009AA58 File Offset: 0x00098C58
	protected override void RegisterPorts()
	{
		base.RegisterPorts();
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetCustomLayerWeight), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
	}

	// Token: 0x060020A5 RID: 8357 RVA: 0x0009AAB0 File Offset: 0x00098CB0
	private void SetCustomLayerWeight(Flow flow)
	{
		WgoData wgoData = base.GetWgoData();
		if (wgoData != null)
		{
			wgoData.SetLayerWeightToAnimator((int)this.layer, (float)Convert.ToInt32(this.enable));
		}
		else
		{
			Debug.LogError(string.Format("{0}: cannot set [{1}] layer on null WGO", "Flow_SetTriggerToAnimator", this.layer));
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001D5C RID: 7516
	[FlowNode.GatherPortsCallbackAttribute]
	public AnimationComponent.Layers layer;

	// Token: 0x04001D5D RID: 7517
	[FlowNode.GatherPortsCallbackAttribute]
	public bool enable;

	// Token: 0x04001D5E RID: 7518
	private FlowInput @in;

	// Token: 0x04001D5F RID: 7519
	private FlowOutput @out;
}
