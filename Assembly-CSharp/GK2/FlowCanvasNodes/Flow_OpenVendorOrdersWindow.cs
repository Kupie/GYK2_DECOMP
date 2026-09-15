using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE0 RID: 3040
	[Name("Show Vendor Orders Window", 0)]
	[Category("Game/UI")]
	public class Flow_OpenVendorOrdersWindow : GKCustomFlowNode
	{
		// Token: 0x06004EA7 RID: 20135 RVA: 0x00172950 File Offset: 0x00170B50
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x001729A0 File Offset: 0x00170BA0
		private void Show(Flow flow)
		{
			if (!MainGame.PlayerData.interactedWithChalkBoardOnce && MainGame.PlayerData.GetRes("commerce_tutorial_available", 0f) > 0f)
			{
				MainGame.PlayerData.interactedWithChalkBoardOnce = true;
				UITutorialWindowData uitutorialWindowData = new UITutorialWindowData("tut_commerce_hdr", null, false);
				LazyUI.GetWindow<UITutorialWindow>().Open(uitutorialWindowData, delegate(UITutorialWindowData _)
				{
					MainGame.PlayerData.SetRes("chalk_board_enabled", 1f);
					LazyUI.GetWindow<UIVendorOrdersWindow>().Open(new UIVendorOrdersWindowData());
				});
			}
			else
			{
				LazyUI.GetWindow<UIVendorOrdersWindow>().Open(new UIVendorOrdersWindowData());
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003FD0 RID: 16336
		private FlowInput @in;

		// Token: 0x04003FD1 RID: 16337
		private FlowOutput @out;
	}
}
