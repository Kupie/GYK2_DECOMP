using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C33 RID: 3123
	[Name("Pay Mercenary", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_PayMercenary : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004FB9 RID: 20409 RVA: 0x00177AB8 File Offset: 0x00175CB8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.PayMercenary), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x00177B08 File Offset: 0x00175D08
		private void PayMercenary(Flow flow)
		{
			MainGame.Instance.GameSave.militaryBaseData.IsMercenaryPayed = true;
			MercenariesDef data = GameBalance.Me.GetData<MercenariesDef>(MainGame.Instance.GameSave.militaryBaseData.MercenariesPaymentId);
			if (data != null)
			{
				foreach (LazyExpression lazyExpression in data.afterPayExpr)
				{
					lazyExpression.Evaluate();
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004130 RID: 16688
		private FlowInput @in;

		// Token: 0x04004131 RID: 16689
		private FlowOutput @out;
	}
}
