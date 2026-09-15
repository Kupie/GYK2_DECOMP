using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B81 RID: 2945
	[Name("Check Phrase In BlackList", 0)]
	[Category("Game/Dialogue")]
	[Color("70f1ff")]
	public class Flow_CheckPhraseInBlackList : GKCustomFlowNode
	{
		// Token: 0x06004D79 RID: 19833 RVA: 0x0016D714 File Offset: 0x0016B914
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Check), "");
			this.@true = base.AddFlowOutput("true".CapitalizeFirst(), "");
			this.@false = base.AddFlowOutput("false".CapitalizeFirst(), "");
			this.phrase = base.AddValueInput<string>("phrase".CapitalizeFirst(), "");
			this.condition = base.AddValueOutput<bool>("condition".CapitalizeFirst(), () => this.innerCondition, "");
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x0016D7C0 File Offset: 0x0016B9C0
		private void Check(Flow flow)
		{
			if (this.checkInUnlockedList)
			{
				this.innerCondition = MainGame.Instance.GameSave.knowledgeSystem.unlockedPhrases.Contains(this.phrase.value);
			}
			else
			{
				this.innerCondition = MainGame.Instance.GameSave.knowledgeSystem.blackListPhrases.Contains(this.phrase.value);
			}
			if (this.innerCondition)
			{
				this.@true.Call(flow);
				return;
			}
			this.@false.Call(flow);
		}

		// Token: 0x17000B8E RID: 2958
		// (get) Token: 0x06004D7B RID: 19835 RVA: 0x0016D84C File Offset: 0x0016BA4C
		public override string name
		{
			get
			{
				return "<color=010101>" + (this.checkInUnlockedList ? "Check Phrase In UnlockedList" : "Check Phrase In BlackList") + "</color>";
			}
		}

		// Token: 0x04003E62 RID: 15970
		[FlowNode.GatherPortsCallbackAttribute]
		public bool checkInUnlockedList = true;

		// Token: 0x04003E63 RID: 15971
		private FlowInput @in;

		// Token: 0x04003E64 RID: 15972
		private FlowOutput @true;

		// Token: 0x04003E65 RID: 15973
		private FlowOutput @false;

		// Token: 0x04003E66 RID: 15974
		private ValueInput<string> phrase;

		// Token: 0x04003E67 RID: 15975
		private ValueOutput<bool> condition;

		// Token: 0x04003E68 RID: 15976
		private bool innerCondition;
	}
}
