using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD4 RID: 3028
	[Name("Last Sermon Equals", 0)]
	[Category("Game/Sermon")]
	[Color("70f1ff")]
	public class Flow_LastSermonEquals : GKCustomFlowNode
	{
		// Token: 0x06004E84 RID: 20100 RVA: 0x00172174 File Offset: 0x00170374
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				if (MainGame.PlayerData.currentSermon.id == this.sermonId.value)
				{
					this.yes.Call(flow);
					return;
				}
				this.no.Call(flow);
			}, "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
			this.sermonId = base.AddValueInput<string>("sermonId".CapitalizeFirst(), "");
		}

		// Token: 0x04003FAF RID: 16303
		private FlowInput @in;

		// Token: 0x04003FB0 RID: 16304
		private FlowOutput yes;

		// Token: 0x04003FB1 RID: 16305
		private FlowOutput no;

		// Token: 0x04003FB2 RID: 16306
		private ValueInput<string> sermonId;
	}
}
