using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BEB RID: 3051
	[Name("Quest Status Equals", 0)]
	[Category("Game/Quest")]
	[Color("FFBE3B")]
	public class Flow_QuestStatusEquals : GKCustomFlowNode
	{
		// Token: 0x06004ECE RID: 20174 RVA: 0x001738E0 File Offset: 0x00171AE0
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), delegate(Flow flow)
			{
				if (MainGame.Instance.GameSave.questSystemData.IsQuestInStatus(this.questId.value, this.questStatus.value))
				{
					this.yes.Call(flow);
					return;
				}
				this.no.Call(flow);
			}, "");
			this.yes = base.AddFlowOutput("yes".CapitalizeFirst(), "");
			this.no = base.AddFlowOutput("no".CapitalizeFirst(), "");
			this.questId = base.AddValueInput<string>("questId".CapitalizeFirst(), "");
			this.questStatus = base.AddValueInput<QuestStatus>("questStatus".CapitalizeFirst(), "");
		}

		// Token: 0x0400400D RID: 16397
		private FlowInput @in;

		// Token: 0x0400400E RID: 16398
		private FlowOutput yes;

		// Token: 0x0400400F RID: 16399
		private FlowOutput no;

		// Token: 0x04004010 RID: 16400
		private ValueInput<string> questId;

		// Token: 0x04004011 RID: 16401
		private ValueInput<QuestStatus> questStatus;
	}
}
