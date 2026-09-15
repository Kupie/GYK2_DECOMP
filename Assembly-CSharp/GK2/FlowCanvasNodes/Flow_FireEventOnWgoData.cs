using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BAB RID: 2987
	[Name("Fire Event On WGO Data", 0)]
	[Category("Game/Script")]
	[Color("ff5c5c")]
	[Icon("FS", false, "")]
	public class Flow_FireEventOnWgoData : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004DFD RID: 19965 RVA: 0x001701F8 File Offset: 0x0016E3F8
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.FireEvent), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.eventName = base.AddValueInput<string>("eventName", "");
			this.delayTime = base.AddValueInput<float>("delayTime", "");
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0017027C File Offset: 0x0016E47C
		private void FireEvent(Flow flow)
		{
			if (this.delayTime.value.More(0f, 1E-05f))
			{
				base.GetWgoData().AddDelayedEvent(this.eventName.value, this.delayTime.value);
			}
			else
			{
				base.GetWgoData().FireEvent(this.eventName.value);
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003F13 RID: 16147
		private FlowInput @in;

		// Token: 0x04003F14 RID: 16148
		private FlowOutput @out;

		// Token: 0x04003F15 RID: 16149
		private ValueInput<string> eventName;

		// Token: 0x04003F16 RID: 16150
		private ValueInput<float> delayTime;
	}
}
