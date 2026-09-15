using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000C18 RID: 3096
	[Name("Start Fight By Id", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_StartFightById : GKCustomFlowNode
	{
		// Token: 0x06004F62 RID: 20322 RVA: 0x001761B4 File Offset: 0x001743B4
		protected override void RegisterPorts()
		{
			this.fightId = base.AddValueInput<string>("id", "");
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.StartFight), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x0017621C File Offset: 0x0017441C
		private void StartFight(Flow flow)
		{
			this.curLevel = MainGame.GetFightingLevel(this.fightId.value);
			if (this.curLevel == null)
			{
				Debug.LogError("Can't find fighting level with id [" + this.fightId.value + "]!");
				this.@out.Call(flow);
				return;
			}
			if (!this.curLevel.IsValid())
			{
				Debug.LogError("Fighting level [" + this.curLevel.id + "] is not valid!");
				this.@out.Call(flow);
				return;
			}
			if (base.PlayerData.HasMultipleOverheadItems)
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
			if (this.showPreFightWindow)
			{
				LazyUI.GetWindow<UIPrefightWindow>().Open(new UIPrefightWindowData(this.curLevel.id, new Action(this.Start), true));
			}
			else
			{
				this.Start();
			}
			this.@out.Call(flow);
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00176368 File Offset: 0x00174568
		private void Start()
		{
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.MultiAnswerSay, this.curLevel.id);
			GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.curLevel.LevelGdPointId);
			if (base.PlayerData.HasOverheadItem)
			{
				base.PlayerData.DropOverheadItem();
			}
			PlayerController.Teleport(new GDPointTeleportData(gdpointDataById, this.curLevel.EnvironmentPreset, "", null, false, 0.3f));
			LazySingleton<FightingGameController>.Instance.StartPreFight(this.curLevel.id, null);
		}

		// Token: 0x040040C8 RID: 16584
		public bool showPreFightWindow;

		// Token: 0x040040C9 RID: 16585
		private FlowInput @in;

		// Token: 0x040040CA RID: 16586
		private FlowOutput @out;

		// Token: 0x040040CB RID: 16587
		private ValueInput<string> fightId;

		// Token: 0x040040CC RID: 16588
		private FightingLevel curLevel;
	}
}
