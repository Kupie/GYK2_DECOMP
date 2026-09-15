using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC4 RID: 3012
	[Name("Give Money Reward For Pray", 0)]
	[Category("Game/UI")]
	public class Flow_GiveMoneyRewardForPray : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E4B RID: 20043 RVA: 0x00171240 File Offset: 0x0016F440
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				base.GetWgoData().AddMoneyToPlayerAndClearSermonResult();
			}, "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x04003F6E RID: 16238
		private FlowInput @in;

		// Token: 0x04003F6F RID: 16239
		private FlowOutput @out;
	}
}
