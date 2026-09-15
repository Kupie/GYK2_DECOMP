using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BA0 RID: 2976
	[Name("End Game", 0)]
	[Category("Game")]
	public class Flow_EndGame : GKCustomFlowNode
	{
		// Token: 0x06004DD4 RID: 19924 RVA: 0x0016F58C File Offset: 0x0016D78C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				MainGame.Instance.CompleteGame(this.shouldGoToMenuOnReturn, delegate
				{
					this.onWindowClosed.Call(flow);
				});
				this.@out.Call(flow);
			}, "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onWindowClosed = base.AddFlowOutput("onWindowClosed".CapitalizeFirst(), "");
		}

		// Token: 0x04003EDF RID: 16095
		[FlowNode.GatherPortsCallbackAttribute]
		public bool shouldGoToMenuOnReturn;

		// Token: 0x04003EE0 RID: 16096
		private FlowInput @in;

		// Token: 0x04003EE1 RID: 16097
		private FlowOutput @out;

		// Token: 0x04003EE2 RID: 16098
		private FlowOutput onWindowClosed;
	}
}
