using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C3C RID: 3132
	[Name("Show Vendor Window On Town Building Char", 0)]
	[Category("Game/UI")]
	public class Flow_ShowVendorWindowOnTownBuildingChar : GKCustomFlowNode
	{
		// Token: 0x06004FD9 RID: 20441 RVA: 0x001782C4 File Offset: 0x001764C4
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onClosed = base.AddFlowOutput("onClosed".CapitalizeFirst(), "");
		}

		// Token: 0x06004FDA RID: 20442 RVA: 0x00178330 File Offset: 0x00176530
		private void Show(Flow flow)
		{
			Trading trading = new Trading();
			UIVendorWindowData uivendorWindowData = new UIVendorWindowData();
			trading.FillVendorWindowData(uivendorWindowData, MainGame.WorldData.GetWgoData(base.SelfWgoData.LinkedToTownBuildingUniqueId).TownBuildingWgoComponent.TownBuildingDef.vendorId, delegate
			{
				this.onClosed.Call(flow);
			});
			LazyUI.GetWindow<UIVendorWindow>().Open(uivendorWindowData);
			this.@out.Call(flow);
		}

		// Token: 0x0400414D RID: 16717
		private FlowInput @in;

		// Token: 0x0400414E RID: 16718
		private FlowOutput @out;

		// Token: 0x0400414F RID: 16719
		private FlowOutput onClosed;
	}
}
