using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C01 RID: 3073
	[Name("Set Time Of Day Preset", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SetTimeOfDayPreset : GKCustomFlowNode
	{
		// Token: 0x06004F1B RID: 20251 RVA: 0x0017511C File Offset: 0x0017331C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetTimeOfDayPreset), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.presetName = base.AddValueInput<string>("presetName".CapitalizeFirst(), "");
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x00175186 File Offset: 0x00173386
		protected void SetTimeOfDayPreset(Flow flow)
		{
			EnvironmentEngine.Instance.SetTimeOfDayPreset(this.presetName.value);
			this.@out.Call(flow);
		}

		// Token: 0x0400406F RID: 16495
		protected FlowInput @in;

		// Token: 0x04004070 RID: 16496
		protected FlowOutput @out;

		// Token: 0x04004071 RID: 16497
		private ValueInput<string> presetName;
	}
}
