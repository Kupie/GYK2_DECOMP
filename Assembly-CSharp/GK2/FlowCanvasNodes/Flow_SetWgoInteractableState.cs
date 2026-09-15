using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C06 RID: 3078
	[Name("Set Wgo Interactable State", 0)]
	[Category("Game/Environment")]
	[Color("f47dff")]
	public class Flow_SetWgoInteractableState : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004F2D RID: 20269 RVA: 0x001754A8 File Offset: 0x001736A8
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetInteractableState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.isInteractable = base.AddValueInput<bool>("isInteractable?", "");
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x00175514 File Offset: 0x00173714
		private void SetInteractableState(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				wgoData.IsInteractable = this.isInteractable.value;
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004080 RID: 16512
		private FlowInput @in;

		// Token: 0x04004081 RID: 16513
		private FlowOutput @out;

		// Token: 0x04004082 RID: 16514
		private ValueInput<bool> isInteractable;
	}
}
