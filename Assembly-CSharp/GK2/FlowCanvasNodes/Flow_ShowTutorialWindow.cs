using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C0C RID: 3084
	[Name("Tutorial Window", 0)]
	[Category("Game/Quests")]
	public class Flow_ShowTutorialWindow : GKCustomFlowNode
	{
		// Token: 0x06004F42 RID: 20290 RVA: 0x00175990 File Offset: 0x00173B90
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ShowWindow), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onClose = base.AddFlowOutput("onClose".CapitalizeFirst(), "");
			this.pageId = base.AddValueInput<string>("pageId".CapitalizeFirst(), "");
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x00175A18 File Offset: 0x00173C18
		private void ShowWindow(Flow flow)
		{
			UITutorialWindowData uitutorialWindowData = new UITutorialWindowData(this.pageId.value, null, false)
			{
				OnCompleteCallback = delegate
				{
					this.onClose.Call(flow);
				}
			};
			LazyUI.GetWindow<UITutorialWindow>().Open(uitutorialWindowData);
			this.@out.Call(flow);
		}

		// Token: 0x04004099 RID: 16537
		private FlowInput @in;

		// Token: 0x0400409A RID: 16538
		private FlowOutput @out;

		// Token: 0x0400409B RID: 16539
		private FlowOutput onClose;

		// Token: 0x0400409C RID: 16540
		private ValueInput<string> pageId;
	}
}
