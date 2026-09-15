using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B71 RID: 2929
	[Name("Add Phrase To BlackList", 0)]
	[Category("Game/Dialogue")]
	[Color("FFFFFF")]
	public class Flow_AddPhraseToBlackList : GKCustomFlowNode
	{
		// Token: 0x06004D40 RID: 19776 RVA: 0x0016C724 File Offset: 0x0016A924
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ProcessPhrase), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.phrase = base.AddValueInput<string>("phrase".CapitalizeFirst(), "");
		}

		// Token: 0x06004D41 RID: 19777 RVA: 0x0016C790 File Offset: 0x0016A990
		private void ProcessPhrase(Flow flow)
		{
			Flow_AddPhraseToBlackList.OperationType operationType = this.operationType;
			if (operationType != Flow_AddPhraseToBlackList.OperationType.Add)
			{
				if (operationType == Flow_AddPhraseToBlackList.OperationType.Remove)
				{
					MainGame.Instance.GameSave.knowledgeSystem.RemovePhraseFromBlackList(this.phrase.value);
				}
			}
			else
			{
				MainGame.Instance.GameSave.knowledgeSystem.AddPhraseToBlackList(this.phrase.value);
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000B87 RID: 2951
		// (get) Token: 0x06004D42 RID: 19778 RVA: 0x0016C7F9 File Offset: 0x0016A9F9
		public override string name
		{
			get
			{
				return string.Format("{0} Phrase {1} BlackList", this.operationType, (this.operationType == Flow_AddPhraseToBlackList.OperationType.Add) ? "to" : "from");
			}
		}

		// Token: 0x04003E1D RID: 15901
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_AddPhraseToBlackList.OperationType operationType;

		// Token: 0x04003E1E RID: 15902
		private FlowInput @in;

		// Token: 0x04003E1F RID: 15903
		private FlowOutput @out;

		// Token: 0x04003E20 RID: 15904
		private ValueInput<string> phrase;

		// Token: 0x02000B72 RID: 2930
		public enum OperationType
		{
			// Token: 0x04003E22 RID: 15906
			Add,
			// Token: 0x04003E23 RID: 15907
			Remove
		}
	}
}
