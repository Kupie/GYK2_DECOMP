using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BA8 RID: 2984
	[Name("Finish Sermon", 0)]
	[Category("Game/Cutscenes")]
	[Color("8a8a8a")]
	public class Flow_FinishSermon : GKCustomFlowNode
	{
		// Token: 0x06004DF4 RID: 19956 RVA: 0x0017004E File Offset: 0x0016E24E
		private bool GetCurrentSermonResult()
		{
			return MainGame.PlayerData.currentSermon.success;
		}

		// Token: 0x06004DF5 RID: 19957 RVA: 0x00170060 File Offset: 0x0016E260
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.FinishSermon), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinish = base.AddFlowOutput("onFinish".CapitalizeFirst(), "");
			this.result = base.AddValueOutput<bool>("result", new ValueHandler<bool>(this.GetCurrentSermonResult), "");
		}

		// Token: 0x06004DF6 RID: 19958 RVA: 0x001700EC File Offset: 0x0016E2EC
		private void FinishSermon(Flow flow)
		{
			MainGame.PlayerController.View.FinishSermon(this.GetCurrentSermonResult(), delegate
			{
				this.onFinish.Call(flow);
			});
			this.@out.Call(flow);
		}

		// Token: 0x04003F09 RID: 16137
		protected FlowInput @in;

		// Token: 0x04003F0A RID: 16138
		protected FlowOutput @out;

		// Token: 0x04003F0B RID: 16139
		protected FlowOutput onFinish;

		// Token: 0x04003F0C RID: 16140
		protected ValueOutput<bool> result;
	}
}
