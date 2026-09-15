using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B8E RID: 2958
	[Name("Drop Happiness On Wgo", 0)]
	[Category("Game/Item")]
	[Color("FFFFFF")]
	public class Flow_DropHappiness : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004DA3 RID: 19875 RVA: 0x0016E234 File Offset: 0x0016C434
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Drop), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x0016E289 File Offset: 0x0016C489
		private void Drop(Flow flow)
		{
			base.GetWgoData().DropHappiness();
			this.@out.Call(flow);
		}

		// Token: 0x04003E91 RID: 16017
		private FlowInput @in;

		// Token: 0x04003E92 RID: 16018
		private FlowOutput @out;
	}
}
