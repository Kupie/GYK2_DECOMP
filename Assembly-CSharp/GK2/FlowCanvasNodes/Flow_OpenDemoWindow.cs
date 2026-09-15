using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BDB RID: 3035
	[Name("Open Demo Window", 0)]
	[Category("Game/UI")]
	public class Flow_OpenDemoWindow : GKCustomFlowNode
	{
		// Token: 0x06004E99 RID: 20121 RVA: 0x00172704 File Offset: 0x00170904
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Open), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004E9A RID: 20122 RVA: 0x00172753 File Offset: 0x00170953
		private void Open(Flow flow)
		{
			LazyUI.GetWindow<UIDemoEndWindow>().Open(null);
			this.@out.Call(flow);
		}

		// Token: 0x04003FC5 RID: 16325
		private FlowInput @in;

		// Token: 0x04003FC6 RID: 16326
		private FlowOutput @out;
	}
}
