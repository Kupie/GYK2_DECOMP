using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD3 RID: 3027
	[Name("Is Time In Between", 0)]
	[Category("Game/Environment")]
	public class Flow_IsTimeInBetween : GKCustomFlowNode
	{
		// Token: 0x06004E81 RID: 20097 RVA: 0x0017205C File Offset: 0x0017025C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.IsTimeInBetween), "");
			this.timeFrom = base.AddValueInput<float>("timeFrom", "");
			this.timeTo = base.AddValueInput<float>("timeTo", "");
			this.trueOut = base.AddFlowOutput("<color=green>✔</color>", "");
			this.falseOut = base.AddFlowOutput("<color=red>✘</color>", "");
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x001720E8 File Offset: 0x001702E8
		private void IsTimeInBetween(Flow flow)
		{
			float timeOfDay = EnvironmentEngine.Instance.timeOfDay;
			if ((this.timeFrom.value > this.timeTo.value) ? (timeOfDay > this.timeFrom.value || timeOfDay < this.timeTo.value) : (timeOfDay > this.timeFrom.value && timeOfDay < this.timeTo.value))
			{
				this.trueOut.Call(flow);
				return;
			}
			this.falseOut.Call(flow);
		}

		// Token: 0x04003FAA RID: 16298
		private FlowInput @in;

		// Token: 0x04003FAB RID: 16299
		private ValueInput<float> timeFrom;

		// Token: 0x04003FAC RID: 16300
		private ValueInput<float> timeTo;

		// Token: 0x04003FAD RID: 16301
		private FlowOutput trueOut;

		// Token: 0x04003FAE RID: 16302
		private FlowOutput falseOut;
	}
}
