using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BDC RID: 3036
	[Name("Show Folio Window", 0)]
	[Category("Game/UI")]
	public class Flow_OpenFolioWindow : GKCustomFlowNode
	{
		// Token: 0x06004E9C RID: 20124 RVA: 0x0017276C File Offset: 0x0017096C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x001727BC File Offset: 0x001709BC
		private void Show(Flow flow)
		{
			UIAlchemyFolioWindowData uialchemyFolioWindowData = new UIAlchemyFolioWindowData();
			uialchemyFolioWindowData.FillFromGaveSave(base.SelfWgoData);
			LazyUI.GetWindow<UIAlchemyFolioWindow>().Open(uialchemyFolioWindowData);
			this.@out.Call(flow);
		}

		// Token: 0x04003FC7 RID: 16327
		private FlowInput @in;

		// Token: 0x04003FC8 RID: 16328
		private FlowOutput @out;
	}
}
