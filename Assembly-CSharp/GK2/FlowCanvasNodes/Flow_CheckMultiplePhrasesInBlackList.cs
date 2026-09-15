using System;
using System.Collections.Generic;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B80 RID: 2944
	[Name("Check Phrase In BlackList", 0)]
	[Category("Game/Dialogue")]
	[Color("70f1ff")]
	public class Flow_CheckMultiplePhrasesInBlackList : GKCustomFlowNode
	{
		// Token: 0x06004D76 RID: 19830 RVA: 0x0016D5CC File Offset: 0x0016B7CC
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.Check), "");
			this.@true = base.AddFlowOutput("true".CapitalizeFirst(), "");
			this.@false = base.AddFlowOutput("false".CapitalizeFirst(), "");
			for (int i = 0; i < this.number; i++)
			{
				this.phrasesList.Add(base.AddValueInput<string>(string.Format("Phrase {0}", i + 1), ""));
			}
		}

		// Token: 0x06004D77 RID: 19831 RVA: 0x0016D670 File Offset: 0x0016B870
		private void Check(Flow flow)
		{
			foreach (ValueInput<string> valueInput in this.phrasesList)
			{
				string text = (T)valueInput;
				if (!MainGame.Instance.GameSave.knowledgeSystem.blackListPhrases.Contains(text))
				{
					this.@false.Call(flow);
					return;
				}
			}
			this.@true.Call(flow);
		}

		// Token: 0x04003E5D RID: 15965
		private FlowInput @in;

		// Token: 0x04003E5E RID: 15966
		private FlowOutput @true;

		// Token: 0x04003E5F RID: 15967
		private FlowOutput @false;

		// Token: 0x04003E60 RID: 15968
		private List<ValueInput<string>> phrasesList = new List<ValueInput<string>>();

		// Token: 0x04003E61 RID: 15969
		[FlowNode.GatherPortsCallbackAttribute]
		public int number = 1;
	}
}
