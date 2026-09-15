using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004C5 RID: 1221
[Name("Enable Lazy Animation Controller", 0)]
[Category("Game/Animation")]
public class Flow_EnableLazyAnimationController : GKCustomFlowNodeWithWgoData
{
	// Token: 0x06002067 RID: 8295 RVA: 0x000996D4 File Offset: 0x000978D4
	protected override void RegisterPorts()
	{
		base.RegisterPorts();
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Toggle), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
	}

	// Token: 0x06002068 RID: 8296 RVA: 0x0009972C File Offset: 0x0009792C
	private void Toggle(Flow flow)
	{
		WgoData wgoData = base.GetWgoData();
		if (wgoData != null)
		{
			LazyAnimationController componentInChildren = GameScene.GetWgoViewGlobal(wgoData.UniqueId).gameObject.GetComponentInChildren<LazyAnimationController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = this.isEnable;
			}
			else
			{
				Debug.LogError("Tried to toggle LazyAnimationController: not found component");
			}
		}
		this.@out.Call(flow);
	}

	// Token: 0x17000558 RID: 1368
	// (get) Token: 0x06002069 RID: 8297 RVA: 0x00099787 File Offset: 0x00097987
	public override string name
	{
		get
		{
			return (this.isEnable ? "Enable" : "Disable") + " LazyAnimationController";
		}
	}

	// Token: 0x04001D09 RID: 7433
	[FlowNode.GatherPortsCallbackAttribute]
	public bool isEnable;

	// Token: 0x04001D0A RID: 7434
	private FlowInput @in;

	// Token: 0x04001D0B RID: 7435
	private FlowOutput @out;
}
