using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200044A RID: 1098
public class TownClusterData : ScriptableObject
{
	// Token: 0x06001CD1 RID: 7377 RVA: 0x00086BDC File Offset: 0x00084DDC
	public TownClusterData.TownClusterDataDto ToDto()
	{
		TownClusterData.TownClusterDataDto townClusterDataDto = new TownClusterData.TownClusterDataDto
		{
			id = this.id,
			handleDestroyedWsoMode = this.handleDestroyedWsoMode
		};
		townClusterDataDto.destroyedWgoUniqueIds.AddRange(this.destroyedWgoUniqueIds);
		townClusterDataDto.destroyedWsoUniqueIds.AddRange(this.destroyedWsoUniqueIds);
		foreach (TownClusterData.ClusterWgoData clusterWgoData in this.repairedWgoData)
		{
			townClusterDataDto.repairedWgoData.Add(new TownClusterData.ClusterWgoData
			{
				wgoId = clusterWgoData.wgoId,
				position = clusterWgoData.position
			});
		}
		foreach (TownClusterData.ClusterWsoData clusterWsoData in this.repairedWsoData)
		{
			string empty = string.Empty;
			townClusterDataDto.repairedWsoData.Add(new TownClusterData.ClusterWgoData
			{
				wgoId = empty,
				position = clusterWsoData.position
			});
		}
		return townClusterDataDto;
	}

	// Token: 0x04001AD3 RID: 6867
	public int id;

	// Token: 0x04001AD4 RID: 6868
	[Space]
	public List<SGuid> destroyedWgoUniqueIds = new List<SGuid>();

	// Token: 0x04001AD5 RID: 6869
	public List<TownClusterData.ClusterWgoData> repairedWgoData = new List<TownClusterData.ClusterWgoData>();

	// Token: 0x04001AD6 RID: 6870
	[Space]
	public TownCluster.HandleDestroyedWsoMode handleDestroyedWsoMode;

	// Token: 0x04001AD7 RID: 6871
	public List<SGuid> destroyedWsoUniqueIds = new List<SGuid>();

	// Token: 0x04001AD8 RID: 6872
	public List<TownClusterData.ClusterWsoData> repairedWsoData = new List<TownClusterData.ClusterWsoData>();

	// Token: 0x0200044B RID: 1099
	[Serializable]
	public class ClusterWgoData
	{
		// Token: 0x04001AD9 RID: 6873
		public string wgoId;

		// Token: 0x04001ADA RID: 6874
		public Vector3 position;
	}

	// Token: 0x0200044C RID: 1100
	[Serializable]
	public class ClusterWsoData
	{
		// Token: 0x04001ADB RID: 6875
		public AssetReferenceGameObject assetReference;

		// Token: 0x04001ADC RID: 6876
		public Vector3 position;
	}

	// Token: 0x0200044D RID: 1101
	[Serializable]
	public class TownClusterDataDto
	{
		// Token: 0x04001ADD RID: 6877
		public int id;

		// Token: 0x04001ADE RID: 6878
		public TownCluster.HandleDestroyedWsoMode handleDestroyedWsoMode;

		// Token: 0x04001ADF RID: 6879
		public List<SGuid> destroyedWgoUniqueIds = new List<SGuid>();

		// Token: 0x04001AE0 RID: 6880
		public List<TownClusterData.ClusterWgoData> repairedWgoData = new List<TownClusterData.ClusterWgoData>();

		// Token: 0x04001AE1 RID: 6881
		public List<SGuid> destroyedWsoUniqueIds = new List<SGuid>();

		// Token: 0x04001AE2 RID: 6882
		public List<TownClusterData.ClusterWgoData> repairedWsoData = new List<TownClusterData.ClusterWgoData>();
	}
}
