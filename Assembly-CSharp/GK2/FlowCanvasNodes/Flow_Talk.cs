using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C1B RID: 3099
	[Name("Talk", 0)]
	[Category("Game/Dialogue")]
	[Color("40addb")]
	[Icon("Dialogue", false, "")]
	public class Flow_Talk : GKCustomFlowNodeWithWgoData
	{
		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06004F6E RID: 20334 RVA: 0x00176655 File Offset: 0x00174855
		public override string name
		{
			get
			{
				return string.Format("{0} {1}", this.speechType.value, this.isPlayer ? "Player" : "NPC");
			}
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x00176688 File Offset: 0x00174888
		protected override void RegisterPorts()
		{
			if (!this.isPlayer)
			{
				base.RegisterPorts();
			}
			this.flowInput = base.AddFlowInput("In", new FlowHandler(this.DoTalk), "");
			this.flowOutput = base.AddFlowOutput("Out", "");
			this.flowOnFinished = base.AddFlowOutput("On Finished", "");
			this.speechType = base.AddValueInput<SpeechBubbleType>("Speech type", "");
			this.speechType.SetDefaultAndSerializedValue(SpeechBubbleType.Talk);
			this.text = base.AddValueInput<string>("text", "");
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x0017672C File Offset: 0x0017492C
		protected virtual void DoTalk(Flow flow)
		{
			if (!this.fixedShowTime)
			{
				this.fixedShowTimeValue = 0f;
			}
			string text = this.text.value;
			Bubble.Talk(new PhraseData
			{
				text = text,
				isPlayer = this.isPlayer,
				npcWgoData = (this.isPlayer ? null : base.GetWgoData()),
				onFinished = delegate
				{
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.SpeechSaid, text);
					this.flowOnFinished.Call(flow);
				},
				cornerPosition = this.forceCornerPosition,
				fixedShowTimeValue = this.fixedShowTimeValue,
				isOverBlackout = this.isOverBlackout,
				speechType = this.speechType.value
			});
			this.flowOutput.Call(flow);
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06004F71 RID: 20337 RVA: 0x0003C7FE File Offset: 0x0003A9FE
		public override Alignment2x2 iconAlignment
		{
			get
			{
				return Alignment2x2.Left;
			}
		}

		// Token: 0x040040D3 RID: 16595
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isPlayer;

		// Token: 0x040040D4 RID: 16596
		public UIBasicBubble.ForceCornerPosition forceCornerPosition;

		// Token: 0x040040D5 RID: 16597
		[FlowNode.GatherPortsCallbackAttribute]
		public bool fixedShowTime;

		// Token: 0x040040D6 RID: 16598
		[ShowIf("fixedShowTime", 1)]
		public float fixedShowTimeValue = -1f;

		// Token: 0x040040D7 RID: 16599
		[FlowNode.GatherPortsCallbackAttribute]
		public bool isOverBlackout;

		// Token: 0x040040D8 RID: 16600
		protected FlowInput flowInput;

		// Token: 0x040040D9 RID: 16601
		protected FlowOutput flowOutput;

		// Token: 0x040040DA RID: 16602
		protected FlowOutput flowOnFinished;

		// Token: 0x040040DB RID: 16603
		protected ValueOutput<WgoData> wgoOutput;

		// Token: 0x040040DC RID: 16604
		protected ValueInput<SpeechBubbleType> speechType;

		// Token: 0x040040DD RID: 16605
		protected ValueInput<string> text;
	}
}
