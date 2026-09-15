using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B9E RID: 2974
	[Name("Enable Pre Fight", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_EnablePreFight : GKCustomFlowNode
	{
		// Token: 0x06004DCD RID: 19917 RVA: 0x0016F3D8 File Offset: 0x0016D5D8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetFightingLevelName), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.levelName = base.AddValueInput<string>("levelName", "");
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0016F43D File Offset: 0x0016D63D
		private void SetFightingLevelName(Flow flow)
		{
			LazySingleton<FightingGameController>.Instance.StartPreFight(this.levelName.value, null);
			this.@out.Call(flow);
		}

		// Token: 0x04003ED8 RID: 16088
		private FlowInput @in;

		// Token: 0x04003ED9 RID: 16089
		private FlowOutput @out;

		// Token: 0x04003EDA RID: 16090
		private ValueInput<string> levelName;
	}
}
