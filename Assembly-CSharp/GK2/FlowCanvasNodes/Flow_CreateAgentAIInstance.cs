using System;
using FlowCanvas;
using ParadoxNotion.Design;
using UnityEngine;

namespace GK2.FlowCanvasNodes
{
	// Token: 0x02000B86 RID: 2950
	[Name("Create Agent AI Instance", 0)]
	[Category("Game/Fighting")]
	[Description("Creates a runtime instance of a specific AgentAI type.")]
	public class Flow_CreateAgentAIInstance : GKCustomFlowNode
	{
		// Token: 0x06004D89 RID: 19849 RVA: 0x0016DBBC File Offset: 0x0016BDBC
		protected override void RegisterPorts()
		{
			if (this.selectedType == Flow_CreateAgentAIInstance.AIType.Event_03_GoToDir)
			{
				this.direction = base.AddValueInput<Direction>("Direction", "");
				this.distance = base.AddValueInput<float>("Distance", "");
			}
			this.aiInstance = base.AddValueOutput<AgentAI>("AI Instance", new ValueHandler<AgentAI>(this.CreateInstance), "");
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x0016DC1F File Offset: 0x0016BE1F
		private AgentAI CreateInstance()
		{
			if (this.selectedType == Flow_CreateAgentAIInstance.AIType.Event_03_GoToDir)
			{
				Event_03_GoToDir event_03_GoToDir = ScriptableObject.CreateInstance<Event_03_GoToDir>();
				event_03_GoToDir.Direction = this.direction.value.ConvertToVector3();
				event_03_GoToDir.Distance = this.distance.value;
				return event_03_GoToDir;
			}
			return null;
		}

		// Token: 0x04003E72 RID: 15986
		[FlowNode.GatherPortsCallbackAttribute]
		public Flow_CreateAgentAIInstance.AIType selectedType;

		// Token: 0x04003E73 RID: 15987
		private ValueInput<Direction> direction;

		// Token: 0x04003E74 RID: 15988
		private ValueInput<float> distance;

		// Token: 0x04003E75 RID: 15989
		private ValueOutput<AgentAI> aiInstance;

		// Token: 0x02000B87 RID: 2951
		public enum AIType
		{
			// Token: 0x04003E77 RID: 15991
			Event_03_GoToDir
		}
	}
}
