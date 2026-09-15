using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C0E RID: 3086
	[Name("Show Vendor Window", 0)]
	[Category("Game/UI")]
	public class Flow_ShowVendorWindow : GKCustomFlowNode
	{
		// Token: 0x06004F47 RID: 20295 RVA: 0x00175A94 File Offset: 0x00173C94
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onClosed = base.AddFlowOutput("onClosed".CapitalizeFirst(), "");
			this.vendorIdInput = base.AddValueInput<string>("vendorIdInput", "");
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00175B14 File Offset: 0x00173D14
		private void Show(Flow flow)
		{
			if (!string.IsNullOrEmpty(this.vendorIdInput.value))
			{
				Trading trading = new Trading();
				UIVendorWindowData uivendorWindowData = new UIVendorWindowData();
				trading.FillVendorWindowData(uivendorWindowData, this.vendorIdInput.value, delegate
				{
					this.onClosed.Call(flow);
				});
				LazyUI.GetWindow<UIVendorWindow>().Open(uivendorWindowData);
			}
			else
			{
				Debug.LogError("Flow_ShowVendorWindow: vendor id is empty.");
			}
			this.@out.Call(flow);
		}

		// Token: 0x0400409F RID: 16543
		private FlowInput @in;

		// Token: 0x040040A0 RID: 16544
		private FlowOutput @out;

		// Token: 0x040040A1 RID: 16545
		private FlowOutput onClosed;

		// Token: 0x040040A2 RID: 16546
		private ValueInput<string> vendorIdInput;
	}
}
