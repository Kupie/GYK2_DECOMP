using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C09 RID: 3081
	[Name("Set Wisp Direction", 0)]
	[Category("Game/Environment")]
	[Color("313c8f")]
	public class Flow_SetWispDirection : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x06004F39 RID: 20281 RVA: 0x00175780 File Offset: 0x00173980
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetDirection), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.direction = base.AddValueInput<Direction>("direction", "");
			this.wispInput = base.AddValueInput<WispController>("wispInput".CapitalizeFirst(), "");
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x00175800 File Offset: 0x00173A00
		private void SetDirection(Flow flow)
		{
			WispController wispController = this.wispInput.value;
			if (wispController == null)
			{
				wispController = MainGame.PlayerController.WispController;
			}
			wispController.SetDirection(this.direction.value);
			this.@out.Call(flow);
		}

		// Token: 0x0400408E RID: 16526
		private FlowInput @in;

		// Token: 0x0400408F RID: 16527
		private FlowOutput @out;

		// Token: 0x04004090 RID: 16528
		private ValueInput<Direction> direction;

		// Token: 0x04004091 RID: 16529
		private ValueInput<WispController> wispInput;
	}
}
