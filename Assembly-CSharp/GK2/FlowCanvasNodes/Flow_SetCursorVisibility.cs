using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF6 RID: 3062
	[Name("Set Cursor Visibility", 0)]
	[Category("Game/UI")]
	[Description("Shows or hides the mouse cursor using CursorController.")]
	[Color("313c8f")]
	public class Flow_SetCursorVisibility : GKCustomFlowNode
	{
		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x06004EF5 RID: 20213 RVA: 0x001744D1 File Offset: 0x001726D1
		public override string name
		{
			get
			{
				if (!this.hide)
				{
					return "Show Cursor";
				}
				return "Hide Cursor";
			}
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x001744E8 File Offset: 0x001726E8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetVisibility), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00174537 File Offset: 0x00172737
		private void SetVisibility(Flow flow)
		{
			CursorController.ChangeCursorVisibleState(!this.hide);
			this.@out.Call(flow);
		}

		// Token: 0x04004039 RID: 16441
		[FlowNode.GatherPortsCallbackAttribute]
		public bool hide = true;

		// Token: 0x0400403A RID: 16442
		private FlowInput @in;

		// Token: 0x0400403B RID: 16443
		private FlowOutput @out;
	}
}
