using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B7D RID: 2941
	[Name("Change Wisp State", 0)]
	[Category("Game/Cutscenes")]
	[Color("8a8a8a")]
	public class Flow_ChangeWispState : GKCustomFlowNode
	{
		// Token: 0x17000B8C RID: 2956
		// (get) Token: 0x06004D6B RID: 19819 RVA: 0x0016D3BB File Offset: 0x0016B5BB
		public override string name
		{
			get
			{
				return (this.isEnable.value ? "Enable Wisp" : "Disable Wisp") ?? "";
			}
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x0016D3E0 File Offset: 0x0016B5E0
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.isEnable = base.AddValueInput<bool>("isEnable".CapitalizeFirst(), "");
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x0016D44C File Offset: 0x0016B64C
		private void ChangeState(Flow flow)
		{
			MainGame.Instance.GameSave.playerData.isWispEnabled = this.isEnable.value;
			MainGame.PlayerController.WispController.ChangeActiveState(this.isEnable.value);
			this.@out.Call(flow);
		}

		// Token: 0x04003E53 RID: 15955
		private FlowInput @in;

		// Token: 0x04003E54 RID: 15956
		private FlowOutput @out;

		// Token: 0x04003E55 RID: 15957
		private ValueInput<bool> isEnable;
	}
}
