using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004C3 RID: 1219
[Name("Display Cinematics Scene", 0)]
[Category("Game/Cutscenes")]
[Color("8a8a8a")]
public class Flow_DisplayCinematicsScene : GKCustomFlowNode
{
	// Token: 0x06002062 RID: 8290 RVA: 0x000995A8 File Offset: 0x000977A8
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DisplayCinematicsScene), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
		this.id = base.AddValueInput<string>("id", "");
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x00099628 File Offset: 0x00097828
	private void DisplayCinematicsScene(Flow flow)
	{
		if (string.IsNullOrEmpty(this.id.value))
		{
			Debug.LogError("Flow_DisplayCinematicsScene: id is null or empty");
			this.@out.Call(flow);
			this.onFinished.Call(flow);
			return;
		}
		LazySingleton<CinematicsSceneDisplayManager>.Instance.DisplayCinematicsScene(this.id.value, delegate
		{
			this.onFinished.Call(flow);
		});
		this.@out.Call(flow);
	}

	// Token: 0x04001D03 RID: 7427
	private FlowInput @in;

	// Token: 0x04001D04 RID: 7428
	private FlowOutput @out;

	// Token: 0x04001D05 RID: 7429
	private FlowOutput onFinished;

	// Token: 0x04001D06 RID: 7430
	private ValueInput<string> id;
}
