using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C22 RID: 3106
	[Name("Test Fill Player Inventory", 0)]
	[Category("Game/UI")]
	public class Flow_TestFillPlayerInventory : GKCustomFlowNode
	{
		// Token: 0x06004F82 RID: 20354 RVA: 0x00176B9C File Offset: 0x00174D9C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x00176BEB File Offset: 0x00174DEB
		private void Show(Flow flow)
		{
			this.@out.Call(flow);
		}

		// Token: 0x040040F4 RID: 16628
		private FlowInput @in;

		// Token: 0x040040F5 RID: 16629
		private FlowOutput @out;
	}
}
