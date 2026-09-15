using System;
using UnityEngine;

// Token: 0x02000448 RID: 1096
public class TownCluster : MonoBehaviour
{
	// Token: 0x04001ACC RID: 6860
	private static string PATH_TO_TOWN_CLUSTER_DATA = "Assets/AddressableAssets/TownClusters";

	// Token: 0x04001ACD RID: 6861
	public int id;

	// Token: 0x04001ACE RID: 6862
	[SerializeField]
	private TownClusterData data;

	// Token: 0x04001ACF RID: 6863
	[SerializeField]
	[HideInInspector]
	private string instanceHash;

	// Token: 0x02000449 RID: 1097
	public enum HandleDestroyedWsoMode
	{
		// Token: 0x04001AD1 RID: 6865
		Destroy,
		// Token: 0x04001AD2 RID: 6866
		HouseRepair
	}
}
