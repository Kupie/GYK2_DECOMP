using System;
using System.Collections.Generic;
using FlowCanvas;
using LinqTools;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BD8 RID: 3032
	[Name("Multi Talk", 0)]
	[Category("Game/Dialogue")]
	[Color("40addb")]
	[ParadoxNotion.Design.Icon("Dialogue", false, "")]
	public sealed class Flow_MultiTalk : GKMultiElementFlowNode<TalkElement>
	{
		// Token: 0x06004E8F RID: 20111 RVA: 0x00172370 File Offset: 0x00170570
		protected override void RegisterPorts()
		{
			base.RegisterWgoIdsPorts();
			this.flowInput = base.AddFlowInput("In", new FlowHandler(this.DoTalk), "");
			this.flowOutput = base.AddFlowOutput("Out", "");
			this.flowOnFinished = base.AddFlowOutput("On Finished", "");
		}

		// Token: 0x06004E90 RID: 20112 RVA: 0x001723D1 File Offset: 0x001705D1
		private void DoTalk(Flow flow)
		{
			this.DoTalkIteration(flow, 0);
			this.flowOutput.Call(flow);
		}

		// Token: 0x06004E91 RID: 20113 RVA: 0x001723E8 File Offset: 0x001705E8
		private void DoTalkIteration(Flow flow, int index)
		{
			if (index >= this.elements.value.Length)
			{
				this.flowOnFinished.Call(flow);
				return;
			}
			TalkElement el = this.elements.value[index];
			bool flag = el.wgoId == "[Player]";
			WgoData wgoData = null;
			if (!flag)
			{
				wgoData = ((el.wgoId == "[Self]") ? base.SelfWgoData : ((el.wgoId == "[Wisp]") ? MainGame.PlayerController.WispController.GetWispWgoData() : MainGame.Instance.GameSave.worldData.GetWgoData(el.wgoId)));
			}
			Bubble.Talk(new PhraseData
			{
				text = el.text,
				isPlayer = flag,
				npcWgoData = wgoData,
				onFinished = delegate
				{
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.SpeechSaid, el.text);
					this.DoTalkIteration(flow, index + 1);
				},
				cornerPosition = el.forceCornerPosition,
				speechType = SpeechBubbleType.Talk
			});
		}

		// Token: 0x06004E92 RID: 20114 RVA: 0x00172530 File Offset: 0x00170730
		protected override void SeparateNode(int i)
		{
			Flow_MultiTalk flow_MultiTalk = base.flowGraph.AddNode<Flow_MultiTalk>(base.position + Vector2.right * 200f);
			foreach (string text in this.wgosIds.value)
			{
				flow_MultiTalk.AddNewWgoIfAbsent(text);
			}
			List<TalkElement> list = this.elements.value.ToList<TalkElement>();
			for (int k = i + 1; k < this.elements.value.Length; k++)
			{
				flow_MultiTalk.AddNewElement(this.elements.value[k]);
				list.RemoveAt(i + 1);
			}
			this.elements.serializedValue = list.ToArray();
			base.RemoveUnusedWGOs();
			flow_MultiTalk.RemoveUnusedWGOs();
		}

		// Token: 0x04003FBB RID: 16315
		private FlowInput flowInput;

		// Token: 0x04003FBC RID: 16316
		private FlowOutput flowOutput;

		// Token: 0x04003FBD RID: 16317
		private FlowOutput flowOnFinished;
	}
}
