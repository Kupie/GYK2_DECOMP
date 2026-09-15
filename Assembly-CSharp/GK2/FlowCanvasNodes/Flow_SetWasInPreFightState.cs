using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C02 RID: 3074
	[Name("Set Was In PreFight State", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_SetWasInPreFightState : GKCustomFlowNode
	{
		// Token: 0x06004F1E RID: 20254 RVA: 0x001751AC File Offset: 0x001733AC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetWasInPreFightState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x001751FB File Offset: 0x001733FB
		private void SetWasInPreFightState(Flow flow)
		{
			LazySingleton<FightingGameController>.Instance.SetWasInPreFightState(this.wasInPreFightState);
			this.@out.Call(flow);
		}

		// Token: 0x04004072 RID: 16498
		[FlowNode.GatherPortsCallbackAttribute]
		public bool wasInPreFightState;

		// Token: 0x04004073 RID: 16499
		private FlowInput @in;

		// Token: 0x04004074 RID: 16500
		private FlowOutput @out;
	}
}
