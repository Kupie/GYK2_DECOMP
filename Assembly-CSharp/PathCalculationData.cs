using System;
using Pathfinding;
using UnityEngine;

// Token: 0x020002C9 RID: 713
public class PathCalculationData
{
	// Token: 0x17000302 RID: 770
	// (get) Token: 0x06001258 RID: 4696 RVA: 0x0005B19A File Offset: 0x0005939A
	public Vector3 Destination
	{
		get
		{
			ICombatEntity combatEntity = this.entity;
			if (combatEntity == null)
			{
				return this.targetPos;
			}
			return combatEntity.CombatEntityPosition;
		}
	}

	// Token: 0x06001259 RID: 4697 RVA: 0x0005B1B2 File Offset: 0x000593B2
	public PathCalculationData(SGuid agentGuid, RichAI richAI, ICombatEntity entity)
	{
		this.agentGuid = agentGuid;
		this.richAI = richAI;
		this.entity = entity;
	}

	// Token: 0x0600125A RID: 4698 RVA: 0x0005B1CF File Offset: 0x000593CF
	public PathCalculationData(SGuid agentGuid, RichAI richAI, Vector3 targetPos)
	{
		this.agentGuid = agentGuid;
		this.richAI = richAI;
		this.targetPos = targetPos;
	}

	// Token: 0x04001404 RID: 5124
	public SGuid agentGuid;

	// Token: 0x04001405 RID: 5125
	public RichAI richAI;

	// Token: 0x04001406 RID: 5126
	public bool isOutDated;

	// Token: 0x04001407 RID: 5127
	private ICombatEntity entity;

	// Token: 0x04001408 RID: 5128
	private Vector3 targetPos;
}
