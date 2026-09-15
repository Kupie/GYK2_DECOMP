using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BDE RID: 3038
	[Name("Open Pray Reward Window", 0)]
	[Category("Game/UI")]
	public class Flow_OpenPrayRewardWindow : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004EA2 RID: 20130 RVA: 0x00172878 File Offset: 0x00170A78
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				base.GetWgoData();
				UIPrayReportWindowData uiprayReportWindowData = new UIPrayReportWindowData(MainGame.PlayerData.currentSermon, delegate
				{
					this.onClosed.Call(flow);
				});
				LazyUI.GetWindow<UIPrayReportWindow>().Open(uiprayReportWindowData);
			}, "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onClosed = base.AddFlowOutput("onClosed".CapitalizeFirst(), "");
		}

		// Token: 0x04003FCB RID: 16331
		private FlowInput @in;

		// Token: 0x04003FCC RID: 16332
		private FlowOutput @out;

		// Token: 0x04003FCD RID: 16333
		private FlowOutput onClosed;
	}
}
