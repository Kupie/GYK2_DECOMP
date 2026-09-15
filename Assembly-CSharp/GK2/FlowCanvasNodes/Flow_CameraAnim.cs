using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B78 RID: 2936
	[Name("Camera Animation", 0)]
	[Category("Game/Cutscenes")]
	public class Flow_CameraAnim : GKCustomFlowNode
	{
		// Token: 0x06004D59 RID: 19801 RVA: 0x0016CF40 File Offset: 0x0016B140
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.DoAnimation), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinished = base.AddFlowOutput("onFinished".CapitalizeFirst(), "");
			this.animationName = base.AddValueInput<string>("animationName", "");
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x0016CFC0 File Offset: 0x0016B1C0
		private void DoAnimation(Flow flow)
		{
			Flow_CameraAnim.<>c__DisplayClass5_0 CS$<>8__locals1 = new Flow_CameraAnim.<>c__DisplayClass5_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.flow = flow;
			CS$<>8__locals1.<DoAnimation>g__Common|0();
		}

		// Token: 0x04003E3C RID: 15932
		private FlowInput @in;

		// Token: 0x04003E3D RID: 15933
		private FlowOutput @out;

		// Token: 0x04003E3E RID: 15934
		private FlowOutput onFinished;

		// Token: 0x04003E3F RID: 15935
		private ValueInput<string> animationName;
	}
}
