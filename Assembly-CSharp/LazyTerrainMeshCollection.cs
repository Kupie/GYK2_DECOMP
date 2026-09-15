using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006BA RID: 1722
[CreateAssetMenu(fileName = "LazyTerrainMeshCollection", menuName = "LazyTerrainMeshCollection", order = 1)]
public class LazyTerrainMeshCollection : LazySingletonSO<LazyTerrainMeshCollection>
{
	// Token: 0x06002DC5 RID: 11717 RVA: 0x000DB02F File Offset: 0x000D922F
	public static void Runtime_StripCpuMeshData(Mesh mesh)
	{
		if (mesh == null || !mesh.isReadable)
		{
			return;
		}
		mesh.UploadMeshData(true);
	}

	// Token: 0x06002DC6 RID: 11718 RVA: 0x000DB04C File Offset: 0x000D924C
	public void Runtime_StripAllMeshCpuData()
	{
		for (int i = 0; i < this.meshes.Count; i++)
		{
			LazyTerrainMeshCollection.Runtime_StripCpuMeshData(this.meshes[i]);
		}
	}

	// Token: 0x040024CE RID: 9422
	public const string AssetPath = "Assets/AddressableAssets/Configurations/LazyTerrainMeshCollection.asset";

	// Token: 0x040024CF RID: 9423
	public const string LegacyAssetPath = "Assets/AddressableAssets/Configurations/LazyTerrainMeshCollection_Legacy.asset";

	// Token: 0x040024D0 RID: 9424
	public const string AddressableGroup = "Configurations";

	// Token: 0x040024D1 RID: 9425
	public const string AddressableAddress = "LazyTerrainMeshCollection";

	// Token: 0x040024D2 RID: 9426
	[SerializeField]
	private List<Mesh> meshes = new List<Mesh>();
}
