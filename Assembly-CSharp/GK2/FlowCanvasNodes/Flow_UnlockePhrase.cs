using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C2A RID: 3114
	[Name("Unlock Phrase", 0)]
	[Category("Game/Dialogue")]
	[Color("FFFFFF")]
	public class Flow_UnlockePhrase : GKCustomFlowNode
	{
		// Token: 0x06004F9E RID: 20382 RVA: 0x00177570 File Offset: 0x00175770
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ProcessPhrase), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			this.phrase = base.AddValueInput<string>("phrase".CapitalizeFirst(), "");
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x001775DA File Offset: 0x001757DA
		private void ProcessPhrase(Flow flow)
		{
			MainGame.Instance.GameSave.knowledgeSystem.UnlockPhrase(this.phrase.value);
			this.@out.Call(flow);
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06004FA0 RID: 20384 RVA: 0x00177607 File Offset: 0x00175807
		public override string name
		{
			get
			{
				return "<color=#010101>Unlock Phrase</color>";
			}
		}

		// Token: 0x04004119 RID: 16665
		private FlowInput @in;

		// Token: 0x0400411A RID: 16666
		private FlowOutput @out;

		// Token: 0x0400411B RID: 16667
		private ValueInput<string> phrase;
	}
}
