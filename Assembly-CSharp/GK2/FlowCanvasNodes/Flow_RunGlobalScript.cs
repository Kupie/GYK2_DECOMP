using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BEF RID: 3055
	[Name("Run Global Script", 0)]
	[Category("Game/Script")]
	[Color("ff5c5c")]
	[Icon("FS", false, "")]
	public class Flow_RunGlobalScript : GKCustomFlowNode
	{
		// Token: 0x06004EDC RID: 20188 RVA: 0x00173E64 File Offset: 0x00172064
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.RunScript), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
			this.scriptName = base.AddValueInput<string>("scriptName".CapitalizeFirst(), "");
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00173EEC File Offset: 0x001720EC
		private void RunScript(Flow flow)
		{
			GameScriptUtility.RunGlobalScript(this.scriptName.value, delegate
			{
				this.onFinished.Call(flow);
			});
			this.@out.Call(flow);
		}

		// Token: 0x04004022 RID: 16418
		private FlowInput @in;

		// Token: 0x04004023 RID: 16419
		private FlowOutput @out;

		// Token: 0x04004024 RID: 16420
		private FlowOutput onFinished;

		// Token: 0x04004025 RID: 16421
		private ValueInput<string> scriptName;
	}
}
