using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006B6 RID: 1718
[Serializable]
public class LazyTerrainDeformingGrassTileBackup
{
	// Token: 0x040024C1 RID: 9409
	public int tileX;

	// Token: 0x040024C2 RID: 9410
	public int tileY;

	// Token: 0x040024C3 RID: 9411
	public Mesh grassMesh;

	// Token: 0x040024C4 RID: 9412
	public List<Vector3> coords = new List<Vector3>();

	// Token: 0x040024C5 RID: 9413
	public DeformingGrassAtlas atlas;
}
