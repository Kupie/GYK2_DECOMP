using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

// Token: 0x020004DC RID: 1244
[Name("Start Craft At Conveyor Workbench", 0)]
[Category("Game/Environment")]
[Color("313c8f")]
public class Flow_StartCraftAtConveyorWorkbench : GKCustomFlowNode
{
	// Token: 0x060020A7 RID: 8359 RVA: 0x0009AB0C File Offset: 0x00098D0C
	protected override void RegisterPorts()
	{
		this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetZombieToConveyorWorkbench), "");
		this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		this.coneyorWorkbenchData = base.AddValueInput<WgoData>("coneyorWorkbenchData".CapitalizeFirst(), "");
		this.craftId = base.AddValueInput<string>("craftId".CapitalizeFirst(), "");
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x0009AB94 File Offset: 0x00098D94
	private void SetZombieToConveyorWorkbench(Flow flow)
	{
		if (this.coneyorWorkbenchData == null)
		{
			return;
		}
		this.coneyorWorkbenchData.value.CraftComponent.AddToQueue(new CraftElement(GameBalance.GetCraftDef(this.craftId.value), new CraftParamsData(this.craftId.value, this.coneyorWorkbenchData.value, CraftParamsData.CraftParamsType.Common, -1)), false, -1);
		this.@out.Call(flow);
	}

	// Token: 0x04001D60 RID: 7520
	private FlowInput @in;

	// Token: 0x04001D61 RID: 7521
	private FlowOutput @out;

	// Token: 0x04001D62 RID: 7522
	private ValueInput<WgoData> coneyorWorkbenchData;

	// Token: 0x04001D63 RID: 7523
	private ValueInput<string> craftId;
}
