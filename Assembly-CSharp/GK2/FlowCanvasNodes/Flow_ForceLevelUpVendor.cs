using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BAD RID: 2989
	[Name("Force level up vendor", 0)]
	[Category("Game/UI")]
	public class Flow_ForceLevelUpVendor : GKCustomFlowNode
	{
		// Token: 0x06004E03 RID: 19971 RVA: 0x0017039C File Offset: 0x0016E59C
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Show), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.vendorIdInput = base.AddValueInput<string>("vendorIdInput", "");
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x00170404 File Offset: 0x0016E604
		private void Show(Flow flow)
		{
			if (!string.IsNullOrEmpty(this.vendorIdInput.value))
			{
				MainGame.Instance.GameSave.vendorSystem.ForceLevelUpVendor(this.vendorIdInput.value);
			}
			else
			{
				Debug.LogError("Flow_ForceLevelUpVendor: vendor id is empty.");
			}
			this.@out.Call(flow);
		}

		// Token: 0x04003F1B RID: 16155
		private FlowInput @in;

		// Token: 0x04003F1C RID: 16156
		private FlowOutput @out;

		// Token: 0x04003F1D RID: 16157
		private ValueInput<string> vendorIdInput;
	}
}
