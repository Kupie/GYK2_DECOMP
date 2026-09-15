using System;
using System.Collections.Generic;
using System.Text;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

// Token: 0x020004C7 RID: 1223
[Name("Multi Answer", 0)]
[Category("Game/Dialogue")]
[ParadoxNotion.Design.Icon("Dialogue", false, "")]
[Color("32f08e")]
public class Flow_MultiAnswer : GKCustomFlowNode
{
	// Token: 0x17000559 RID: 1369
	// (get) Token: 0x0600206E RID: 8302 RVA: 0x00099838 File Offset: 0x00097A38
	public List<Flow_MultiAnswer.AnswerOption> AnswerOptions
	{
		get
		{
			return this.answerOptions;
		}
	}

	// Token: 0x1700055A RID: 1370
	// (get) Token: 0x0600206F RID: 8303 RVA: 0x00099840 File Offset: 0x00097A40
	public override int MinWidth
	{
		get
		{
			return 200;
		}
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x00099848 File Offset: 0x00097A48
	protected override void RegisterPorts()
	{
		FlowOutput flow_out = base.AddFlowOutput("Out", "");
		int num = -1;
		this.SyncLists();
		foreach (Flow_MultiAnswer.AnswerOption answerOption in this.answerOptions)
		{
			num++;
			string text = answerOption.id;
			StringBuilder stringBuilder = new StringBuilder("□□□");
			if (false)
			{
				string text2 = "<color=#4040FF>";
				StringBuilder stringBuilder2 = stringBuilder;
				text = text2 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null) + "</color> " + text;
			}
			string text3 = "out_" + num.ToString();
			text = (answerOption.isLockedByDefault ? ("[Ô]" + text) : text);
			answerOption.output = base.AddFlowOutput(text, text3);
			string text4 = "<color=#A08030>#";
			int num2 = num;
			answerOption.valueInput = base.AddValueInput<AnswerData>(text4 + num2.ToString() + "</color>", "");
		}
		ValueInput<WgoData> wgoTalker = base.AddValueInput<WgoData>("Talker", "");
		base.AddFlowInput("In", delegate(Flow f)
		{
			List<AnswerVisualData> list = new List<AnswerVisualData>();
			this.SyncLists();
			int i = 0;
			while (i < this.answerOptions.Count)
			{
				string id = this.answerOptions[i].id;
				AnswerData answerData = null;
				if (this.answerOptions[i].valueInput.isConnected)
				{
					answerData = this.answerOptions[i].valueInput.value;
					if (!answerData.customHideCondition)
					{
						goto IL_00B0;
					}
				}
				else
				{
					QuestDef questDef;
					if (GameBalance.Me.questDefByReqPhrase.TryGetValue(id, out questDef))
					{
						answerData = questDef.finishCheck.GetAnswerDataByReqs();
						goto IL_00B0;
					}
					goto IL_00B0;
				}
				IL_00E7:
				i++;
				continue;
				IL_00B0:
				list.Add(new AnswerVisualData
				{
					answerData = answerData,
					hiddenByDefault = this.answerOptions[i].isLockedByDefault,
					id = id
				});
				goto IL_00E7;
			}
			Transform bubblePoint = MainGame.PlayerController.BubblePoint;
			Bubble.ShowMultiAnswer(list, bubblePoint, this.WgoDataParamOrSelf(wgoTalker), delegate(string chosen)
			{
				GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.MultiAnswerSay, chosen);
				this.answerOptions.Find((Flow_MultiAnswer.AnswerOption x) => x.id == chosen).output.Call(f);
			}, delegate
			{
			}, this.isOverBlackout);
			flow_out.Call(f);
		}, "");
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x00028294 File Offset: 0x00026494
	private bool SyncLists()
	{
		return false;
	}

	// Token: 0x1700055B RID: 1371
	// (get) Token: 0x06002072 RID: 8306 RVA: 0x00099994 File Offset: 0x00097B94
	public override string name
	{
		get
		{
			foreach (FlowOutput flowOutput in base.GetOutputFlowPorts())
			{
				if (!(flowOutput.name == "Out") && !flowOutput.isConnected)
				{
					return "Multi Answer \n<color=#FF2020>!!!EMPTY OUT!!!</color>";
				}
			}
			return "Multi Answer";
		}
	}

	// Token: 0x04001D10 RID: 7440
	[SerializeField]
	private List<Flow_MultiAnswer.AnswerOption> answerOptions = new List<Flow_MultiAnswer.AnswerOption>();

	// Token: 0x04001D11 RID: 7441
	[SerializeField]
	private bool isOverBlackout;

	// Token: 0x020004C8 RID: 1224
	[Serializable]
	public class AnswerOption
	{
		// Token: 0x04001D12 RID: 7442
		public string id;

		// Token: 0x04001D13 RID: 7443
		public bool isLockedByDefault;

		// Token: 0x04001D14 RID: 7444
		public ValueInput<AnswerData> valueInput;

		// Token: 0x04001D15 RID: 7445
		public FlowOutput output;
	}
}
