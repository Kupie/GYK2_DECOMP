using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B7C RID: 2940
	[Name("Can Start Fight", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_CatStartFight : GKCustomFlowNode
	{
		// Token: 0x06004D68 RID: 19816 RVA: 0x0016D334 File Offset: 0x0016B534
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetFightingLevelName), "");
			this.trueOut = base.AddFlowOutput("<color=green>✔</color>", "");
			this.falseOut = base.AddFlowOutput("<color=red>✘</color>", "");
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x0016D394 File Offset: 0x0016B594
		private void SetFightingLevelName(Flow flow)
		{
			if (LazySingleton<FightingGameController>.Instance.CanStartFight())
			{
				this.trueOut.Call(flow);
				return;
			}
			this.falseOut.Call(flow);
		}

		// Token: 0x04003E50 RID: 15952
		private FlowInput @in;

		// Token: 0x04003E51 RID: 15953
		private FlowOutput trueOut;

		// Token: 0x04003E52 RID: 15954
		private FlowOutput falseOut;
	}
}
