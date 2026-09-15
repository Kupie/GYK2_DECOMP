using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD1 RID: 3025
	[Name("Is Demo", 0)]
	[Category("Game")]
	[Color("cf35c1")]
	public class Flow_IsDemo : GKCustomFlowNode
	{
		// Token: 0x06004E7B RID: 20091 RVA: 0x00171F24 File Offset: 0x00170124
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				this.no.Call(flow);
			}, "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
		}

		// Token: 0x04003FA3 RID: 16291
		private FlowInput @in;

		// Token: 0x04003FA4 RID: 16292
		private FlowOutput yes;

		// Token: 0x04003FA5 RID: 16293
		private FlowOutput no;
	}
}
