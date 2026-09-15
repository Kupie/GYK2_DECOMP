using System;
using System.Collections.Generic;
using System.Linq;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B82 RID: 2946
	[Name("Choose Fight Level", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_ChooseFightLevel : GKCustomFlowNode
	{
		// Token: 0x06004D7E RID: 19838 RVA: 0x0016D888 File Offset: 0x0016BA88
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.ShowChooseLevelMultiAnswer), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x0016D8D8 File Offset: 0x0016BAD8
		private void ShowChooseLevelMultiAnswer(Flow flow)
		{
			List<AnswerVisualData> list = new List<AnswerVisualData>();
			FightingLevel[] levels = global::UnityEngine.Object.FindObjectsByType<FightingLevel>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
			for (int i = 0; i < levels.Length; i++)
			{
				if (levels[i].IsValid())
				{
					list.Add(new AnswerVisualData
					{
						answerData = new AnswerData(),
						hiddenByDefault = false,
						id = levels[i].name
					});
				}
			}
			AnswerVisualData answerVisualData = new AnswerVisualData
			{
				answerData = new AnswerData(),
				hiddenByDefault = false,
				id = "Exit"
			};
			list.Add(answerVisualData);
			Transform bubblePoint = MainGame.PlayerController.BubblePoint;
			Bubble.ShowMultiAnswer(list, bubblePoint, base.SelfWgoData, delegate(string chosen)
			{
				if (chosen == "Exit")
				{
					this.@out.Call(flow);
					return;
				}
				if (this.PlayerData.HasMultipleOverheadItems)
				{
					Bubble.Talk(new PhraseData(true, null, LLBase.L("fight_start_overhead_reason"), null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
					this.@out.Call(flow);
					return;
				}
				if (!LazySingleton<FightingGameController>.Instance.CanStartFight())
				{
					Bubble.Talk(new PhraseData(true, null, LLBase.L("fight_start_equipment_reason"), null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
					this.@out.Call(flow);
					return;
				}
				GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.MultiAnswerSay, chosen);
				this.curLevel = levels.FirstOrDefault((FightingLevel x) => x.id == chosen);
				LazyUI.GetWindow<UIPrefightWindow>().Open(new UIPrefightWindowData(this.curLevel.id, new Action(this.Start), true));
				this.@out.Call(flow);
			}, delegate
			{
			}, true);
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x0016D9D4 File Offset: 0x0016BBD4
		private void Start()
		{
			GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.curLevel.LevelGdPointId);
			if (base.PlayerData.HasOverheadItem)
			{
				base.PlayerData.DropOverheadItem();
			}
			PlayerController.Teleport(new GDPointTeleportData(gdpointDataById, this.curLevel.EnvironmentPreset, "", null, false, 0.3f));
			LazySingleton<FightingGameController>.Instance.StartPreFight(this.curLevel.id, null);
		}

		// Token: 0x04003E69 RID: 15977
		private FlowInput @in;

		// Token: 0x04003E6A RID: 15978
		private FlowOutput @out;

		// Token: 0x04003E6B RID: 15979
		private FightingLevel curLevel;
	}
}
