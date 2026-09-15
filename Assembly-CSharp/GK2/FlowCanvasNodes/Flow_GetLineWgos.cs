using System;
using System.Collections.Generic;
using FlowCanvas;
using LazyBearTechnology;
using ParadoxNotion.Design;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000BB9 RID: 3001
	[Name("Get Fighting Line WGOs", 0)]
	[Category("Game/Fighting")]
	[Description("Retrieves all WGO Data objects belonging to agents on a specific line.")]
	[Color("f47dff")]
	public class Flow_GetLineWgos : GKCustomFlowNode
	{
		// Token: 0x06004E2F RID: 20015 RVA: 0x00170D68 File Offset: 0x0016EF68
		protected override void RegisterPorts()
		{
			this.lineId = base.AddValueInput<int>("Line ID", "");
			this.wgos = base.AddValueOutput<List<WgoData>>("WGOs", new ValueHandler<List<WgoData>>(this.GetWgos), "");
			this.outAgentsGroupBehaviourController = base.AddValueOutput<AgentsGroupBehaviourController>("Group Controller", () => this.agentsGroupBehaviourController, "");
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x00170DD0 File Offset: 0x0016EFD0
		private List<WgoData> GetWgos()
		{
			List<WgoData> list = new List<WgoData>();
			FightingLevel currentLevel = LazySingleton<FightingGameController>.Instance.CurrentLevel;
			if (currentLevel == null)
			{
				return list;
			}
			FightingLine fightingLine = currentLevel.GetFightingLine(this.lineId.value);
			if (fightingLine == null)
			{
				return list;
			}
			this.agentsGroupBehaviourController = fightingLine.GetComponent<AgentsGroupBehaviourController>();
			if (this.agentsGroupBehaviourController == null)
			{
				return list;
			}
			foreach (FightingAgent fightingAgent in this.agentsGroupBehaviourController.Agents)
			{
				if (fightingAgent != null && fightingAgent.Wgo != null)
				{
					list.Add(fightingAgent.Wgo.Data);
				}
			}
			return list;
		}

		// Token: 0x04003F54 RID: 16212
		private ValueInput<int> lineId;

		// Token: 0x04003F55 RID: 16213
		private ValueOutput<List<WgoData>> wgos;

		// Token: 0x04003F56 RID: 16214
		private ValueOutput<AgentsGroupBehaviourController> outAgentsGroupBehaviourController;

		// Token: 0x04003F57 RID: 16215
		private AgentsGroupBehaviourController agentsGroupBehaviourController;
	}
}
