using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BAA RID: 2986
	[Name("Fire Event On Global Script", 0)]
	[Category("Game/Script")]
	[Color("ff5c5c")]
	[Icon("FS", false, "")]
	public class Flow_FireEventOnGlobalScript : GKCustomFlowNode
	{
		// Token: 0x06004DFA RID: 19962 RVA: 0x00170158 File Offset: 0x0016E358
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in", delegate(Flow flow)
			{
				GlobalScriptsManager.FireEvent(this.scriptName.value, this.eventName.value, null);
				this.@out.Call(flow);
			}, "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.scriptName = base.AddValueInput<string>("scriptName", "");
			this.eventName = base.AddValueInput<string>("eventName", "");
		}

		// Token: 0x04003F0F RID: 16143
		private FlowInput @in;

		// Token: 0x04003F10 RID: 16144
		private FlowOutput @out;

		// Token: 0x04003F11 RID: 16145
		private ValueInput<string> scriptName;

		// Token: 0x04003F12 RID: 16146
		private ValueInput<string> eventName;
	}
}
