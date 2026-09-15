using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C07 RID: 3079
	[Name("Set WGO Movement Pause State", 0)]
	[Category("Game/Environment")]
	[Color("f47dff")]
	public class Flow_SetWgoMovementPauseState : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x06004F30 RID: 20272 RVA: 0x00175548 File Offset: 0x00173748
		public override string name
		{
			get
			{
				if (this.isPaused == null || !this.isPaused.value)
				{
					return "<color=#5dff5d>▶</color> Resume WGO Movement";
				}
				return "<color=#ff5d5d>⏸</color> Pause WGO Movement";
			}
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x0017556C File Offset: 0x0017376C
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetMovementPauseState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.isPaused = base.AddValueInput<bool>("isPaused", "");
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x001755D8 File Offset: 0x001737D8
		private void SetMovementPauseState(Flow flow)
		{
			WgoData wgoData = base.GetWgoData();
			if (wgoData != null)
			{
				wgoData.MovementComponent.IsPaused = this.isPaused.value;
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004083 RID: 16515
		private FlowInput @in;

		// Token: 0x04004084 RID: 16516
		private FlowOutput @out;

		// Token: 0x04004085 RID: 16517
		private ValueInput<bool> isPaused;
	}
}
