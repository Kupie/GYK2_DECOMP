using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B7E RID: 2942
	[Name("Change Wisp Type", 0)]
	[Category("Game/Cutscenes")]
	[Color("8a8a8a")]
	public class Flow_ChangeWispType : GKCustomFlowNode
	{
		// Token: 0x17000B8D RID: 2957
		// (get) Token: 0x06004D6F RID: 19823 RVA: 0x0016D49E File Offset: 0x0016B69E
		public override string name
		{
			get
			{
				return string.Format("Change Wisp Type to {0}", this.wispType.value);
			}
		}

		// Token: 0x06004D70 RID: 19824 RVA: 0x0016D4BC File Offset: 0x0016B6BC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ChangeWispType), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onAnimFinish = base.AddFlowOutput("onAnimFinish".CapitalizeFirst(), "");
			this.wispType = base.AddValueInput<WispType>("wispType".CapitalizeFirst(), "");
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x0016D544 File Offset: 0x0016B744
		private void ChangeWispType(Flow flow)
		{
			MainGame.PlayerController.WispController.ChangeWispType(this.wispType.value);
			LazyTimer.AddTimer(Flow_ChangeWispType.CHANGE_WISP_TYPE_TIME, delegate
			{
				this.onAnimFinish.Call(flow);
			}, null);
			this.@out.Call(flow);
		}

		// Token: 0x04003E56 RID: 15958
		private static float CHANGE_WISP_TYPE_TIME = 1f;

		// Token: 0x04003E57 RID: 15959
		private FlowInput @in;

		// Token: 0x04003E58 RID: 15960
		private FlowOutput @out;

		// Token: 0x04003E59 RID: 15961
		private FlowOutput onAnimFinish;

		// Token: 0x04003E5A RID: 15962
		private ValueInput<WispType> wispType;
	}
}
