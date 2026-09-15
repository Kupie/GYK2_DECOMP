using System;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BF2 RID: 3058
	[Name("Set All Capture Points Team", 0)]
	[Category("Game/Fighting")]
	[Description("Sets the owner team for all capture points in the current fighting level (base point + all sector points).")]
	[Color("313c8f")]
	public class Flow_SetCapturePointTeam : GKCustomFlowNode
	{
		// Token: 0x06004EE6 RID: 20198 RVA: 0x001740A0 File Offset: 0x001722A0
		protected override void RegisterPorts()
		{
			this.teamType = base.AddValueInput<LazyConsts.Fighting.TeamType>("Team Type", "");
			base.AddFlowInput("In", delegate(Flow f)
			{
				this.Execute();
				f.Call(this.outFlow);
			}, "");
			this.outFlow = base.AddFlowOutput("Out", "");
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x001740F8 File Offset: 0x001722F8
		private void Execute()
		{
			FightingLevel currentLevel = LazySingleton<FightingGameController>.Instance.CurrentLevel;
			if (currentLevel == null)
			{
				Debug.LogError("[Flow_SetCapturePointTeam]: No active fighting level found.");
				return;
			}
			foreach (FightingLine fightingLine in currentLevel.FightingLines)
			{
				if (!(fightingLine == null))
				{
					foreach (FightingSector fightingSector in fightingLine.sectors)
					{
						if (fightingSector != null && fightingSector.point != null)
						{
							fightingSector.point.SetOwnedByTeam(this.teamType.value);
						}
					}
				}
			}
		}

		// Token: 0x0400402C RID: 16428
		private ValueInput<LazyConsts.Fighting.TeamType> teamType;

		// Token: 0x0400402D RID: 16429
		private FlowOutput outFlow;
	}
}
