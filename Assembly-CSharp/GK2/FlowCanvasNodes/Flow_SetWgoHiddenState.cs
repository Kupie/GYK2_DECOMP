using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C05 RID: 3077
	[Name("Set WGO Hidden State", 0)]
	[Category("Game/Environment")]
	[Color("f47dff")]
	public class Flow_SetWgoHiddenState : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06004F29 RID: 20265 RVA: 0x001753FA File Offset: 0x001735FA
		public override string name
		{
			get
			{
				if (!this.hide)
				{
					return "Show Wgo";
				}
				return "Hide Wgo";
			}
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x00175410 File Offset: 0x00173610
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetHiddenState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x00175468 File Offset: 0x00173668
		private void SetHiddenState(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				wgoData.IsHidden = this.hide;
			}
			this.@out.Call(flow);
		}

		// Token: 0x0400407D RID: 16509
		[FlowNode.GatherPortsCallbackAttribute]
		public bool hide = true;

		// Token: 0x0400407E RID: 16510
		private FlowInput @in;

		// Token: 0x0400407F RID: 16511
		private FlowOutput @out;
	}
}
