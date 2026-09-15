using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BAC RID: 2988
	[Name("Fire Event Trigger", 0)]
	[Category("Game/Script")]
	[Color("ff5c5c")]
	[Icon("FS", false, "")]
	public class Flow_FireEventTrigger : GKCustomFlowNode
	{
		// Token: 0x06004E00 RID: 19968 RVA: 0x001702EC File Offset: 0x0016E4EC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.FireEvent), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.eventType = base.AddValueInput<GlobalEventsSystem.Event.Type>("eventType", "");
			this.eventId = base.AddValueInput<string>("eventId", "");
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x00170367 File Offset: 0x0016E567
		private void FireEvent(Flow flow)
		{
			GlobalEventsSystem.FireTrigger(this.eventType.value, this.eventId.value ?? string.Empty);
			this.@out.Call(flow);
		}

		// Token: 0x04003F17 RID: 16151
		private FlowInput @in;

		// Token: 0x04003F18 RID: 16152
		private FlowOutput @out;

		// Token: 0x04003F19 RID: 16153
		private ValueInput<GlobalEventsSystem.Event.Type> eventType;

		// Token: 0x04003F1A RID: 16154
		private ValueInput<string> eventId;
	}
}
