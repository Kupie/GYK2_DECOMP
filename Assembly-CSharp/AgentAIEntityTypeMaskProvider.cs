using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002CB RID: 715
[Serializable]
public class AgentAIEntityTypeMaskProvider
{
	// Token: 0x0600125C RID: 4700 RVA: 0x0005B1EC File Offset: 0x000593EC
	public bool TryGetAgentAI(LazyConsts.Fighting.EntityType entityType, out AgentAI ai)
	{
		for (int i = 0; i < this.entries.Count; i++)
		{
			AgentAIEntityTypeMaskEntry agentAIEntityTypeMaskEntry = this.entries[i];
			if (agentAIEntityTypeMaskEntry.entityTypeMask != LazyConsts.Fighting.EntityType.None && agentAIEntityTypeMaskEntry.agentAI && (entityType & agentAIEntityTypeMaskEntry.entityTypeMask) == agentAIEntityTypeMaskEntry.entityTypeMask)
			{
				ai = agentAIEntityTypeMaskEntry.agentAI;
				return true;
			}
		}
		ai = null;
		return false;
	}

	// Token: 0x0400140A RID: 5130
	[SerializeField]
	private List<AgentAIEntityTypeMaskEntry> entries = new List<AgentAIEntityTypeMaskEntry>();
}
