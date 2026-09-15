using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200044F RID: 1103
[Serializable]
public class TownClusterRepairWgoComponent : IComponent
{
	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x00086D64 File Offset: 0x00084F64
	// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x00086D6C File Offset: 0x00084F6C
	public int Id
	{
		get
		{
			return this.id;
		}
		set
		{
			this.id = value;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x00086D75 File Offset: 0x00084F75
	// (set) Token: 0x06001CDA RID: 7386 RVA: 0x00086D7D File Offset: 0x00084F7D
	public List<TownClusterData.TownClusterDataDto> Configurations
	{
		get
		{
			return this.configurations;
		}
		set
		{
			this.configurations = value;
		}
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x00086D88 File Offset: 0x00084F88
	public void DoRepairLogic()
	{
		string currentGameSceneId = MainGame.PlayerData.currentGameSceneId;
		if (string.IsNullOrEmpty(currentGameSceneId))
		{
			Debug.LogWarning("[TownClusterRepairWgoComponent] Current game scene id is empty.");
			return;
		}
		foreach (TownClusterData.TownClusterDataDto townClusterDataDto in this.configurations)
		{
			foreach (SGuid sguid in townClusterDataDto.destroyedWgoUniqueIds)
			{
				MainGame.WorldData.RemoveWgoDataFromGameScene(sguid);
			}
			TownCluster.HandleDestroyedWsoMode handleDestroyedWsoMode = townClusterDataDto.handleDestroyedWsoMode;
			if (handleDestroyedWsoMode != TownCluster.HandleDestroyedWsoMode.Destroy)
			{
				if (handleDestroyedWsoMode != TownCluster.HandleDestroyedWsoMode.HouseRepair)
				{
					continue;
				}
			}
			else
			{
				using (List<SGuid>.Enumerator enumerator2 = townClusterDataDto.destroyedWsoUniqueIds.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						SGuid sguid2 = enumerator2.Current;
						MainGame.WorldData.RemoveWsoDataFromGameScene(sguid2);
					}
					continue;
				}
			}
			foreach (SGuid sguid3 in townClusterDataDto.destroyedWsoUniqueIds)
			{
				TownUtils.RepairHouse(MainGame.WorldData.GetWsoData(sguid3), -1);
			}
		}
		foreach (TownClusterData.TownClusterDataDto townClusterDataDto2 in this.configurations)
		{
			foreach (TownClusterData.ClusterWgoData clusterWgoData in townClusterDataDto2.repairedWgoData)
			{
				string wgoId = clusterWgoData.wgoId;
				if (!string.IsNullOrEmpty(wgoId))
				{
					MainGame.WorldData.AddWgoData(new WgoData(wgoId, clusterWgoData.position, currentGameSceneId));
				}
			}
			foreach (TownClusterData.ClusterWgoData clusterWgoData2 in townClusterDataDto2.repairedWsoData)
			{
				string wgoId2 = clusterWgoData2.wgoId;
				if (!string.IsNullOrEmpty(wgoId2))
				{
					WsoData wsoData;
					MainGame.WorldData.AddWsoData(wgoId2, clusterWgoData2.position, currentGameSceneId, out wsoData);
				}
			}
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.RepairTownCluster, "");
	}

	// Token: 0x04001AE3 RID: 6883
	[SerializeField]
	private int id;

	// Token: 0x04001AE4 RID: 6884
	[SerializeField]
	private List<TownClusterData.TownClusterDataDto> configurations = new List<TownClusterData.TownClusterDataDto>();
}
