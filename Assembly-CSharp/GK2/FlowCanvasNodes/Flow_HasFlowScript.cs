using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BCD RID: 3021
	[Name("Has Flow Script", 0)]
	[Category("Game/Script")]
	[Color("ff5c5c")]
	[Icon("FS", false, "")]
	public class Flow_HasFlowScript : GKCustomFlowNode
	{
		// Token: 0x06004E6C RID: 20076 RVA: 0x001719E4 File Offset: 0x0016FBE4
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				if (GlobalScriptsManager.HasFlowScript(this.scriptName.value))
				{
					this.yes.Call(flow);
					return;
				}
				this.no.Call(flow);
			}, "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
			this.scriptName = base.AddValueInput<string>("scriptName".CapitalizeFirst(), "");
		}

		// Token: 0x04003F90 RID: 16272
		private FlowInput @in;

		// Token: 0x04003F91 RID: 16273
		private FlowOutput yes;

		// Token: 0x04003F92 RID: 16274
		private FlowOutput no;

		// Token: 0x04003F93 RID: 16275
		private ValueInput<string> scriptName;
	}
}
