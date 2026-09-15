using System;
using FlowCanvas;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C1D RID: 3101
	[Name("Talk Wisp", 0)]
	[Category("Game/Dialogue")]
	[Color("40addb")]
	[Icon("Dialogue", false, "")]
	public class Flow_TalkWisp : Flow_Talk
	{
		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06004F75 RID: 20341 RVA: 0x00176846 File Offset: 0x00174A46
		public override string name
		{
			get
			{
				return "Talk Wisp";
			}
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x0017684D File Offset: 0x00174A4D
		protected override void RegisterPorts()
		{
			base.RegisterPorts();
			this.wispInput = base.AddValueInput<WispController>("wispInput".CapitalizeFirst(), "");
			this.wgoDataInput = null;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x00176878 File Offset: 0x00174A78
		protected override void DoTalk(Flow flow)
		{
			WispController wispController = this.wispInput.value;
			if (wispController == null)
			{
				wispController = MainGame.PlayerController.WispController;
			}
			string text = this.text.value;
			Bubble.Talk(new PhraseData
			{
				text = text,
				isPlayer = false,
				npcWgoData = wispController.GetWispWgoData(),
				onFinished = delegate
				{
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.SpeechSaid, text);
					this.flowOnFinished.Call(flow);
				},
				cornerPosition = this.forceCornerPosition,
				isOverBlackout = this.isOverBlackout,
				speechType = this.speechType.value
			});
			this.flowOutput.Call(flow);
		}

		// Token: 0x040040E1 RID: 16609
		private ValueInput<WispController> wispInput;
	}
}
