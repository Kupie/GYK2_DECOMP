using System;
using UnityEngine;

// Token: 0x020001B2 RID: 434
[Serializable]
public class NPCLifeSimulatorActionData
{
	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000B04 RID: 2820 RVA: 0x00037C31 File Offset: 0x00035E31
	public SGuid WgoId
	{
		get
		{
			return this.wgoId;
		}
	}

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00037C39 File Offset: 0x00035E39
	public NPCLifeSimulatorActionType ActionType
	{
		get
		{
			return this.actionType;
		}
	}

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00037C41 File Offset: 0x00035E41
	// (set) Token: 0x06000B07 RID: 2823 RVA: 0x00037C49 File Offset: 0x00035E49
	public float RemainingTimeToAction
	{
		get
		{
			return this.remainingTimeToAction;
		}
		set
		{
			this.remainingTimeToAction = value;
		}
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00037C52 File Offset: 0x00035E52
	public NPCLifeSimulatorActionData(SGuid wgoId, float rolledTime, NPCLifeSimulatorActionType actionType)
	{
		this.wgoId = wgoId;
		this.remainingTimeToAction = rolledTime;
		this.actionType = actionType;
	}

	// Token: 0x04000C57 RID: 3159
	[SerializeField]
	private float remainingTimeToAction;

	// Token: 0x04000C58 RID: 3160
	[SerializeField]
	private SGuid wgoId;

	// Token: 0x04000C59 RID: 3161
	[SerializeField]
	private NPCLifeSimulatorActionType actionType;
}
