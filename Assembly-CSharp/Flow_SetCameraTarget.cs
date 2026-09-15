using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004D3 RID: 1235
[Name("Set Camera Target", 0)]
[Category("Game/Camera")]
[Color("8a8a8a")]
public class Flow_SetCameraTarget : GKCustomFlowNode
{
	// Token: 0x06002091 RID: 8337 RVA: 0x0009A3F4 File Offset: 0x000985F4
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetCameraTarget), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.duration = base.AddValueInput<float>("duration".CapitalizeFirst(), "");
		this.target = base.AddValueInput<Transform>("target".CapitalizeFirst(), "");
		this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x0009A494 File Offset: 0x00098694
	private void SetCameraTarget(Flow flow)
	{
		if (this.target.value == null)
		{
			Debug.LogError("Flow_SetCameraTarget: No camera target was set.");
			this.onFinished.Call(flow);
		}
		else if (this.duration.value == 0f)
		{
			CameraController.SetFollowTargetInstant(this.target.value);
			this.onFinished.Call(flow);
		}
		else
		{
			CameraController.SetFollowTarget(this.target.value, this.duration.value, delegate
			{
				this.onFinished.Call(flow);
			});
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001D3A RID: 7482
	private FlowInput @in;

	// Token: 0x04001D3B RID: 7483
	private FlowOutput @out;

	// Token: 0x04001D3C RID: 7484
	private FlowOutput onFinished;

	// Token: 0x04001D3D RID: 7485
	private ValueInput<float> duration;

	// Token: 0x04001D3E RID: 7486
	private ValueInput<Transform> target;
}
