using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B7 RID: 1719
[CreateAssetMenu(fileName = "LazyTerrainDeformingGrassBackup", menuName = "GK2/LazyTerrain/Deforming Grass Backup", order = 2)]
public class LazyTerrainDeformingGrassBackup : ScriptableObject
{
	// Token: 0x040024C6 RID: 9414
	public string terrainGameObjectName;

	// Token: 0x040024C7 RID: 9415
	public string sceneName;

	// Token: 0x040024C8 RID: 9416
	public List<LazyTerrainDeformingGrassTileBackup> tiles = new List<LazyTerrainDeformingGrassTileBackup>();
}
