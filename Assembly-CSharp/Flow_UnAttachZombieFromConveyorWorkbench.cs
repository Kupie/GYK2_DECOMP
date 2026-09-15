using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004DF RID: 1247
[Name("UnAttach Zombie To Conveyor Workbench", 0)]
[Category("Game/Environment")]
[Color("313c8f")]
public class Flow_UnAttachZombieFromConveyorWorkbench : GKCustomFlowNode
{
	// Token: 0x060020B2 RID: 8370 RVA: 0x0009AEBC File Offset: 0x000990BC
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetZombieToConveyorWorkbench), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.conveyorWorkbenchData = base.AddValueInput<WgoData>("conveyorWorkbenchData".CapitalizeFirst(), "");
	}

	// Token: 0x060020B3 RID: 8371 RVA: 0x0009AF28 File Offset: 0x00099128
	private void SetZombieToConveyorWorkbench(Flow flow)
	{
		if (this.conveyorWorkbenchData != null && this.conveyorWorkbenchData.value.Worker != null)
		{
			ZombieWgoData zombieWgoData = this.conveyorWorkbenchData.value.Worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				zombieWgoData.UnAttachFromWgoData(false);
				MainGame.Instance.GameSave.zombieSystemData.RemoveZombie(zombieWgoData.UniqueId);
			}
		}
		this.@out.Call(flow);
	}

	// Token: 0x04001D73 RID: 7539
	private FlowInput @in;

	// Token: 0x04001D74 RID: 7540
	private FlowOutput @out;

	// Token: 0x04001D75 RID: 7541
	private ValueInput<WgoData> conveyorWorkbenchData;
}
