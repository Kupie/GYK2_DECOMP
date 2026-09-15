using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006C0 RID: 1728
[CreateAssetMenu(fileName = "LazyTerrainSurfaceConfiguration", menuName = "GK2/LazyTerrainSurfaceConfiguration", order = 0)]
public class LazyTerrainSurfaceConfiguration : LazySingletonSO<LazyTerrainSurfaceConfiguration>
{
	// Token: 0x06002DDF RID: 11743 RVA: 0x000DB64C File Offset: 0x000D984C
	public SurfaceType GetSurfaceTypeByGroupId(string group)
	{
		LazyTerrainSurfaceConfiguration.TerrainGroupSurfacePair terrainGroupSurfacePair = this.surfacePairs.Find((LazyTerrainSurfaceConfiguration.TerrainGroupSurfacePair p) => p.group == group);
		if (terrainGroupSurfacePair == null)
		{
			return SurfaceType.None;
		}
		return terrainGroupSurfacePair.surfaceType;
	}

	// Token: 0x040024F0 RID: 9456
	[SerializeField]
	private List<LazyTerrainSurfaceConfiguration.TerrainGroupSurfacePair> surfacePairs;

	// Token: 0x020006C1 RID: 1729
	[Serializable]
	private class TerrainGroupSurfacePair
	{
		// Token: 0x040024F1 RID: 9457
		public string group;

		// Token: 0x040024F2 RID: 9458
		public SurfaceType surfaceType;
	}
}
