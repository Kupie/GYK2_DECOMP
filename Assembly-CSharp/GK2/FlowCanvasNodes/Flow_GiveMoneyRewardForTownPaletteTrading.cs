using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BC5 RID: 3013
	[Name("Give Money Reward For Town Palette Trading", 0)]
	[Category("Game/UI")]
	public class Flow_GiveMoneyRewardForTownPaletteTrading : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004E4E RID: 20046 RVA: 0x001712A4 File Offset: 0x0016F4A4
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				base.GetWgoData().AddMoneyToPlayerAndPaletteTradingResult();
			}, "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x04003F70 RID: 16240
		private FlowInput @in;

		// Token: 0x04003F71 RID: 16241
		private FlowOutput @out;
	}
}
