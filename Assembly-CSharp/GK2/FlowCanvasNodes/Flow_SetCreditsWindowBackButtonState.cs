using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF5 RID: 3061
	[Name("Set Credits Window Back Button State", 0)]
	[Category("Game/UI")]
	public class Flow_SetCreditsWindowBackButtonState : GKCustomFlowNode
	{
		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x06004EF1 RID: 20209 RVA: 0x00174429 File Offset: 0x00172629
		public override string name
		{
			get
			{
				return (this.hide ? "Hide" : "Show") + " Credits Window Back Button";
			}
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x0017444C File Offset: 0x0017264C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetHiddenState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x0017449C File Offset: 0x0017269C
		private void SetHiddenState(Flow flow)
		{
			UICreditsWindow window = LazyUI.GetWindow<UICreditsWindow>();
			if (this.hide)
			{
				window.HideBackButton();
			}
			else
			{
				window.ShowBackButton();
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004036 RID: 16438
		[FlowNode.GatherPortsCallbackAttribute]
		public bool hide;

		// Token: 0x04004037 RID: 16439
		private FlowInput @in;

		// Token: 0x04004038 RID: 16440
		private FlowOutput @out;
	}
}
