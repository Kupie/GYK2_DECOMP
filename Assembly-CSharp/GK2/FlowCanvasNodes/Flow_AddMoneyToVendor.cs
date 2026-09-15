using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B70 RID: 2928
	[Name("Add money to vendor", 0)]
	[Category("Game/UI")]
	public class Flow_AddMoneyToVendor : GKCustomFlowNode
	{
		// Token: 0x06004D3D RID: 19773 RVA: 0x0016C644 File Offset: 0x0016A844
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.vendorIdInput = base.AddValueInput<string>("vendorIdInput", "");
			this.moneyInput = base.AddValueInput<int>("moneyInput", "");
		}

		// Token: 0x06004D3E RID: 19774 RVA: 0x0016C6C0 File Offset: 0x0016A8C0
		private void Show(Flow flow)
		{
			if (!string.IsNullOrEmpty(this.vendorIdInput.value))
			{
				MainGame.Instance.GameSave.vendorSystem.AddMoneyToVendor(this.vendorIdInput.value, this.moneyInput.value);
			}
			else
			{
				Debug.LogError("Flow_AddMoneyToVendor: vendor id is empty.");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003E19 RID: 15897
		private FlowInput @in;

		// Token: 0x04003E1A RID: 15898
		private FlowOutput @out;

		// Token: 0x04003E1B RID: 15899
		private ValueInput<string> vendorIdInput;

		// Token: 0x04003E1C RID: 15900
		private ValueInput<int> moneyInput;
	}
}
