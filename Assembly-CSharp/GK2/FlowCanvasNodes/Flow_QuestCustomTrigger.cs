using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BE9 RID: 3049
	[Name("Quest Custom Trigger", 0)]
	[Category("Game/Quest")]
	[Color("FFBE3B")]
	public class Flow_QuestCustomTrigger : GKCustomFlowNode
	{
		// Token: 0x06004EC9 RID: 20169 RVA: 0x001737A8 File Offset: 0x001719A8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Process), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.onFinish = base.AddFlowOutput("onFinish".CapitalizeFirst(), "");
			this.triggerId = base.AddValueInput<string>("triggerId", "");
		}

		// Token: 0x06004ECA RID: 20170 RVA: 0x00173828 File Offset: 0x00171A28
		private void Process(Flow flow)
		{
			if (!MainGame.Instance.GameSave.questSystemData.RaiseCustomQuestTrigger(this.triggerId.value, delegate
			{
				this.onFinish.Call(flow);
			}) && this.onFinish.isConnected)
			{
				base.Error("No callback for quest trigger " + this.triggerId.value + ".");
				this.onFinish.Call(flow);
			}
			this.@out.Call(flow);
		}

		// Token: 0x04004007 RID: 16391
		private FlowInput @in;

		// Token: 0x04004008 RID: 16392
		private FlowOutput @out;

		// Token: 0x04004009 RID: 16393
		private FlowOutput onFinish;

		// Token: 0x0400400A RID: 16394
		private ValueInput<string> triggerId;
	}
}
