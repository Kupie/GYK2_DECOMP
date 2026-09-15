using System;
using FlowCanvas;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BFD RID: 3069
	[Name("Change GD Point State", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SetGdPointState : GKCustomFlowNode
	{
		// Token: 0x06004F0C RID: 20236 RVA: 0x00174D54 File Offset: 0x00172F54
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in", new FlowHandler(this.SetGdPointState), "");
			this.@out = base.AddFlowOutput("out", "");
			this.gdPoint = base.AddValueInput<GDPointData>("gdPoint", "");
			this.enabled = base.AddValueInput<bool>("enabled", "");
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x00174DC5 File Offset: 0x00172FC5
		private void SetGdPointState(Flow flow)
		{
			this.gdPoint.value.Enabled = this.enabled.value;
			this.@out.Call(flow);
		}

		// Token: 0x0400405D RID: 16477
		private FlowInput @in;

		// Token: 0x0400405E RID: 16478
		private FlowOutput @out;

		// Token: 0x0400405F RID: 16479
		private ValueInput<GDPointData> gdPoint;

		// Token: 0x04004060 RID: 16480
		private ValueInput<bool> enabled;
	}
}
