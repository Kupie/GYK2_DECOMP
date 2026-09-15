using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C2F RID: 3119
	[Name("Is Mercenary Payed", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_IsMercenaryPayed : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004FAE RID: 20398 RVA: 0x00177870 File Offset: 0x00175A70
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.IsMercenaryPayed), "");
			this.trueOut = base.AddFlowOutput("<color=green>✔</color>", "");
			this.falseOut = base.AddFlowOutput("<color=red>✘</color>", "");
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x001778D0 File Offset: 0x00175AD0
		private void IsMercenaryPayed(Flow flow)
		{
			if (MainGame.Instance.GameSave.militaryBaseData.IsMercenaryPayed)
			{
				this.trueOut.Call(flow);
				return;
			}
			this.falseOut.Call(flow);
		}

		// Token: 0x04004128 RID: 16680
		private FlowInput @in;

		// Token: 0x04004129 RID: 16681
		private FlowOutput trueOut;

		// Token: 0x0400412A RID: 16682
		private FlowOutput falseOut;
	}
}
