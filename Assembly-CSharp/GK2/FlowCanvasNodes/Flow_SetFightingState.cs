using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF9 RID: 3065
	[Name("Set Fighting State", 0)]
	[Category("Game/Fighting")]
	[Color("313c8f")]
	public class Flow_SetFightingState : GKCustomFlowNode
	{
		// Token: 0x06004F01 RID: 20225 RVA: 0x001749C8 File Offset: 0x00172BC8
		protected override void RegisterPorts()
		{
			this.@in = base.AddFlowInput("in".CapitalizeFirst(), new FlowHandler(this.SetFightingPlayState), "");
			this.@out = base.AddFlowOutput("out".CapitalizeFirst(), "");
			if (this.state == Flow_SetFightingState.State.Start)
			{
				this.levelId = base.AddValueInput<string>("levelId", "");
			}
			if (this.customFinishTransition)
			{
				Flow_SetFightingState.State state = this.state;
				if (state == Flow_SetFightingState.State.Start || state == Flow_SetFightingState.State.Stop)
				{
					this.customOut = base.AddFlowOutput("customOut".CapitalizeFirst(), "");
				}
			}
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x00174A68 File Offset: 0x00172C68
		private void SetFightingPlayState(Flow flow)
		{
			switch (this.state)
			{
			case Flow_SetFightingState.State.Start:
				LazySingleton<FightingGameController>.Instance.Play(this.levelId.value);
				break;
			case Flow_SetFightingState.State.Pause:
				LazySingleton<FightingGameController>.Instance.SetPauseState(true);
				break;
			case Flow_SetFightingState.State.Continue:
				LazySingleton<FightingGameController>.Instance.SetPauseState(false);
				break;
			case Flow_SetFightingState.State.Stop:
				LazySingleton<FightingGameController>.Instance.Stop(this.customFinishTransition, false);
				break;
			}
			if (this.customFinishTransition)
			{
				Flow_SetFightingState.State state = this.state;
				if (state == Flow_SetFightingState.State.Start || state == Flow_SetFightingState.State.Stop)
				{
					LazySingleton<FightingGameController>.Instance.SetCustomFinishCallback(delegate
					{
						FlowOutput flowOutput = this.customOut;
						if (flowOutput == null)
						{
							return;
						}
						flowOutput.Call(flow);
					});
				}
			}
			this.@out.Call(flow);
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06004F03 RID: 20227 RVA: 0x00174B28 File Offset: 0x00172D28
		public override string name
		{
			get
			{
				string text;
				switch (this.state)
				{
				case Flow_SetFightingState.State.Start:
					text = "Start Fighting";
					break;
				case Flow_SetFightingState.State.Pause:
					text = "Pause Fighting";
					break;
				case Flow_SetFightingState.State.Continue:
					text = "Continue Fighting";
					break;
				case Flow_SetFightingState.State.Stop:
					text = "Stop Fighting";
					break;
				default:
					text = "Set Fighting State";
					break;
				}
				return text;
			}
		}

		// Token: 0x04004049 RID: 16457
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_SetFightingState.State state;

		// Token: 0x0400404A RID: 16458
		[FlowNode.GatherPortsCallbackAttribute]
		public bool customFinishTransition;

		// Token: 0x0400404B RID: 16459
		private FlowInput @in;

		// Token: 0x0400404C RID: 16460
		private FlowOutput @out;

		// Token: 0x0400404D RID: 16461
		private FlowOutput customOut;

		// Token: 0x0400404E RID: 16462
		private ValueInput<string> levelId;

		// Token: 0x02000BFA RID: 3066
		public enum State
		{
			// Token: 0x04004050 RID: 16464
			Start,
			// Token: 0x04004051 RID: 16465
			Pause,
			// Token: 0x04004052 RID: 16466
			Continue,
			// Token: 0x04004053 RID: 16467
			Stop
		}
	}
}
