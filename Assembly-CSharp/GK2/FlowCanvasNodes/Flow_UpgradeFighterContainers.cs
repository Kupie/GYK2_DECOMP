using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C34 RID: 3124
	[Name("Upgrade Fighter Containers", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_UpgradeFighterContainers : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004FBC RID: 20412 RVA: 0x00177B9C File Offset: 0x00175D9C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.UpgradeFighterContainers), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x00177BEB File Offset: 0x00175DEB
		private void UpgradeFighterContainers(Flow flow)
		{
			MainGame.Instance.GameSave.militaryBaseData.UpgradeFighterContainers();
			this.@out.Call(flow);
		}

		// Token: 0x04004132 RID: 16690
		private FlowInput @in;

		// Token: 0x04004133 RID: 16691
		private FlowOutput @out;
	}
}
