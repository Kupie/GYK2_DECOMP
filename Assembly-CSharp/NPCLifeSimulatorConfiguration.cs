using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020001B4 RID: 436
[CreateAssetMenu(menuName = "GK2/NPCLifeSimulatorConfiguration", fileName = "NPCLifeSimulatorConfiguration")]
public class NPCLifeSimulatorConfiguration : LazySingletonSO<NPCLifeSimulatorConfiguration>
{
	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00037C6F File Offset: 0x00035E6F
	public float MinActionDelay
	{
		get
		{
			return this.minActionDelay;
		}
	}

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000B0A RID: 2826 RVA: 0x00037C77 File Offset: 0x00035E77
	public float MaxActionDelay
	{
		get
		{
			return this.maxActionDelay;
		}
	}

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00037C7F File Offset: 0x00035E7F
	public List<NPCGroupPointOfInterestConfiguration> AllGroups
	{
		get
		{
			return this.groupPointsOfInterestData;
		}
	}

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00037C87 File Offset: 0x00035E87
	public List<NPCPointOfInterestConfiguration> AllPonts
	{
		get
		{
			return this.pointsOfInterestData;
		}
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00037C90 File Offset: 0x00035E90
	public NPCGroupPointOfInterestConfiguration GetGroupById(string groupId)
	{
		foreach (NPCGroupPointOfInterestConfiguration npcgroupPointOfInterestConfiguration in this.groupPointsOfInterestData)
		{
			if (npcgroupPointOfInterestConfiguration.Id == groupId)
			{
				return npcgroupPointOfInterestConfiguration;
			}
		}
		return null;
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00037CF4 File Offset: 0x00035EF4
	public NPCPointOfInterestConfiguration GetPointById(string id)
	{
		foreach (NPCPointOfInterestConfiguration npcpointOfInterestConfiguration in this.pointsOfInterestData)
		{
			if (npcpointOfInterestConfiguration.Id == id)
			{
				return npcpointOfInterestConfiguration;
			}
		}
		return null;
	}

	// Token: 0x04000C5D RID: 3165
	[SerializeField]
	private List<NPCGroupPointOfInterestConfiguration> groupPointsOfInterestData;

	// Token: 0x04000C5E RID: 3166
	[SerializeField]
	private List<NPCPointOfInterestConfiguration> pointsOfInterestData;

	// Token: 0x04000C5F RID: 3167
	[SerializeField]
	private float minActionDelay;

	// Token: 0x04000C60 RID: 3168
	[SerializeField]
	private float maxActionDelay;
}
