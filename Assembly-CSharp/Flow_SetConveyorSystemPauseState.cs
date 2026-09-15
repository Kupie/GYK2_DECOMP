using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004D5 RID: 1237
[Name("Set Conveyor System Pause State", 0)]
[Category("Game/Environment")]
[Color("313c8f")]
public class Flow_SetConveyorSystemPauseState : GKCustomFlowNode
{
	// Token: 0x06002096 RID: 8342 RVA: 0x0009A56C File Offset: 0x0009876C
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetZombieToConveyorWorkbench), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.isPaused = base.AddValueInput<bool>("isPaused".CapitalizeFirst(), "");
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x0009A5D6 File Offset: 0x000987D6
	private void SetZombieToConveyorWorkbench(Flow flow)
	{
		MainGame.Instance.conveyorSystem.IsPaused = this.isPaused.value;
		this.@out.Call(flow);
	}

	// Token: 0x04001D41 RID: 7489
	private FlowInput @in;

	// Token: 0x04001D42 RID: 7490
	private FlowOutput @out;

	// Token: 0x04001D43 RID: 7491
	private ValueInput<bool> isPaused;
}
