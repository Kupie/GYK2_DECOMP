using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004BE RID: 1214
[Name("Attach Zombie To Conveyor Workbench", 0)]
[Category("Game/Environment")]
[Color("313c8f")]
public class Flow_AttachZombieToConveyorWorkbench : GKCustomFlowNode
{
	// Token: 0x06002052 RID: 8274 RVA: 0x0009922C File Offset: 0x0009742C
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetZombieToConveyorWorkbench), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.zombieData = base.AddValueInput<ZombieWgoData>("zombieData".CapitalizeFirst(), "");
		this.conveyorWorkbenchData = base.AddValueInput<WgoData>("conveyorWorkbenchData".CapitalizeFirst(), "");
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000992B1 File Offset: 0x000974B1
	private void SetZombieToConveyorWorkbench(Flow flow)
	{
		this.zombieData.value.AttachToConveyorCraftWgoData(this.conveyorWorkbenchData.value.UniqueId, this.zombieData.value.ZombieItem, null);
		this.@out.Call(flow);
	}

	// Token: 0x04001CF0 RID: 7408
	private FlowInput @in;

	// Token: 0x04001CF1 RID: 7409
	private FlowOutput @out;

	// Token: 0x04001CF2 RID: 7410
	private ValueInput<ZombieWgoData> zombieData;

	// Token: 0x04001CF3 RID: 7411
	private ValueInput<WgoData> conveyorWorkbenchData;
}
