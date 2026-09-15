using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B5 RID: 437
[Serializable]
public class NPCLifeSimulatorData
{
	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000B10 RID: 2832 RVA: 0x00037D60 File Offset: 0x00035F60
	public List<NPCGroupPointOfInterestData> AllGroups
	{
		get
		{
			return this.groupPointsOfInterestData;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000B11 RID: 2833 RVA: 0x00037D68 File Offset: 0x00035F68
	public List<NPCPointOfInterestData> AllPoints
	{
		get
		{
			return this.pointsOfInterest;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000B12 RID: 2834 RVA: 0x00037D70 File Offset: 0x00035F70
	public List<NPCPointOfInterestAnimationData> AnimationDatas
	{
		get
		{
			return this.animationsData;
		}
	}

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00037D78 File Offset: 0x00035F78
	public List<NPCLifeSimulatorActionData> ActionsData
	{
		get
		{
			return this.actionsData;
		}
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00037D80 File Offset: 0x00035F80
	public void PrepareForGame()
	{
		foreach (NPCPointOfInterestData npcpointOfInterestData in this.pointsOfInterest)
		{
			GDPointData gdpointData = npcpointOfInterestData.GDPointData;
			if (gdpointData != null)
			{
				gdpointData.SetEnabledStateSilent(npcpointOfInterestData.Enabled);
			}
		}
		if (this.cachedPoints == null)
		{
			this.cachedPoints = new Dictionary<string, NPCPointOfInterestData>();
		}
		foreach (NPCPointOfInterestData npcpointOfInterestData2 in this.pointsOfInterest)
		{
			this.cachedPoints.Add(npcpointOfInterestData2.Id, npcpointOfInterestData2);
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00037E44 File Offset: 0x00036044
	public void AddGroup(NPCGroupPointOfInterestData group)
	{
		this.groupPointsOfInterestData.Add(group);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00037E52 File Offset: 0x00036052
	public void AddPoint(NPCPointOfInterestData point)
	{
		this.pointsOfInterest.Add(point);
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00037E60 File Offset: 0x00036060
	public void ClearData()
	{
		this.groupPointsOfInterestData.Clear();
		this.pointsOfInterest.Clear();
		Dictionary<string, NPCPointOfInterestData> dictionary = this.cachedPoints;
		if (dictionary != null)
		{
			dictionary.Clear();
		}
		this.actionsData.Clear();
		this.animationsData.Clear();
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x00037EA0 File Offset: 0x000360A0
	public NPCPointOfInterestData RollPoint(string group)
	{
		NPCGroupPointOfInterestData groupById = this.GetGroupById(group);
		List<NPCPointOfInterestData> list = new List<NPCPointOfInterestData>();
		foreach (string text in groupById.Configuration.AllPonts)
		{
			NPCPointOfInterestData pointById = this.GetPointById(text);
			if (pointById.Enabled && !pointById.IsOccupied)
			{
				list.Add(pointById);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		float num = 0f;
		foreach (NPCPointOfInterestData npcpointOfInterestData in list)
		{
			num += npcpointOfInterestData.Weight;
		}
		float num2 = global::UnityEngine.Random.Range(0f, num);
		float num3 = 0f;
		foreach (NPCPointOfInterestData npcpointOfInterestData2 in list)
		{
			num3 += npcpointOfInterestData2.Weight;
			if (num2 <= num3)
			{
				return npcpointOfInterestData2;
			}
		}
		List<NPCPointOfInterestData> list2 = list;
		return list2[list2.Count - 1];
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x00037FE4 File Offset: 0x000361E4
	public NPCGroupPointOfInterestData GetGroupById(string groupId)
	{
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.groupPointsOfInterestData)
		{
			if (npcgroupPointOfInterestData.Id == groupId)
			{
				return npcgroupPointOfInterestData;
			}
		}
		return null;
	}

	// Token: 0x06000B1A RID: 2842 RVA: 0x00038048 File Offset: 0x00036248
	public NPCPointOfInterestData GetPointById(string pointId)
	{
		NPCPointOfInterestData npcpointOfInterestData;
		if (!this.cachedPoints.TryGetValue(pointId, out npcpointOfInterestData))
		{
			return null;
		}
		return npcpointOfInterestData;
	}

	// Token: 0x06000B1B RID: 2843 RVA: 0x00038068 File Offset: 0x00036268
	public NPCGroupPointOfInterestData GetGroupByWGOId(SGuid wgoId)
	{
		foreach (NPCGroupPointOfInterestData npcgroupPointOfInterestData in this.groupPointsOfInterestData)
		{
			using (List<SGuid>.Enumerator enumerator2 = npcgroupPointOfInterestData.Wgos.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.Guid == wgoId.Guid)
					{
						return npcgroupPointOfInterestData;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06000B1C RID: 2844 RVA: 0x00038108 File Offset: 0x00036308
	public void AddAnimationData(NPCPointOfInterestAnimationData animationData)
	{
		this.animationsData.Add(animationData);
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00038118 File Offset: 0x00036318
	public void TryRemoveAnimationData(SGuid wgoId)
	{
		for (int i = this.animationsData.Count - 1; i >= 0; i--)
		{
			if (this.animationsData[i].WgoId.Guid == wgoId.Guid)
			{
				this.animationsData.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x0003816C File Offset: 0x0003636C
	public void TryRemoveActionData(SGuid wgoId)
	{
		for (int i = this.actionsData.Count - 1; i >= 0; i--)
		{
			if (this.actionsData[i].WgoId.Guid == wgoId.Guid)
			{
				this.actionsData.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x000381C0 File Offset: 0x000363C0
	public void AddActionData(NPCLifeSimulatorActionData actionData)
	{
		this.TryRemoveActionData(actionData.WgoId);
		this.actionsData.Add(actionData);
	}

	// Token: 0x04000C61 RID: 3169
	[SerializeField]
	private List<NPCGroupPointOfInterestData> groupPointsOfInterestData = new List<NPCGroupPointOfInterestData>();

	// Token: 0x04000C62 RID: 3170
	[SerializeField]
	private List<NPCPointOfInterestData> pointsOfInterest = new List<NPCPointOfInterestData>();

	// Token: 0x04000C63 RID: 3171
	[SerializeField]
	private List<NPCPointOfInterestAnimationData> animationsData = new List<NPCPointOfInterestAnimationData>();

	// Token: 0x04000C64 RID: 3172
	[SerializeField]
	private List<NPCLifeSimulatorActionData> actionsData = new List<NPCLifeSimulatorActionData>();

	// Token: 0x04000C65 RID: 3173
	private Dictionary<string, NPCPointOfInterestData> cachedPoints;
}
