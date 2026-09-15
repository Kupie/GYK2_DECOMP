using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C2E RID: 3118
	[Name("Is Max Fighter Containers", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_IsMaxFighterContainers : GKCustomFlowNode
	{
		// Token: 0x06004FAB RID: 20395 RVA: 0x001777DC File Offset: 0x001759DC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.IsMaxFighterContainers), "");
			this.trueOut = base.AddFlowOutput("<color=green>✔</color>", "");
			this.falseOut = base.AddFlowOutput("<color=red>✘</color>", "");
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x0017783C File Offset: 0x00175A3C
		private void IsMaxFighterContainers(Flow flow)
		{
			if (MainGame.Instance.GameSave.militaryBaseData.IsMaxFighterContainers)
			{
				this.trueOut.Call(flow);
				return;
			}
			this.falseOut.Call(flow);
		}

		// Token: 0x04004125 RID: 16677
		private FlowInput @in;

		// Token: 0x04004126 RID: 16678
		private FlowOutput trueOut;

		// Token: 0x04004127 RID: 16679
		private FlowOutput falseOut;
	}
}
