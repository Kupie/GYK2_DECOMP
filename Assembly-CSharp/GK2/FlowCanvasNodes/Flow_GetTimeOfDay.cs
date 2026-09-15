using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BBF RID: 3007
	[Name("Get Time Of Day", 0)]
	[Color("f47dff")]
	public class Flow_GetTimeOfDay : GKCustomFlowNode
	{
		// Token: 0x06004E3C RID: 20028 RVA: 0x001710AB File Offset: 0x0016F2AB
		protected override void RegisterPorts()
		{
			this.timeOfDay = base.AddValueOutput<float>("timeOfDay".CapitalizeFirst(), () => EnvironmentEngine.Instance.timeOfDay, "");
		}

		// Token: 0x04003F66 RID: 16230
		private ValueOutput<float> timeOfDay;
	}
}
