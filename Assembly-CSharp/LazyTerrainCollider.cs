using System;
using UnityEngine;

// Token: 0x020006B2 RID: 1714
public class LazyTerrainCollider : MonoBehaviour, IOrderedSurface
{
	// Token: 0x17000720 RID: 1824
	// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x000DAEA3 File Offset: 0x000D90A3
	public int Depth
	{
		get
		{
			return this.depth;
		}
	}

	// Token: 0x17000721 RID: 1825
	// (get) Token: 0x06002DB6 RID: 11702 RVA: 0x000DAEAB File Offset: 0x000D90AB
	public SurfaceType SurfaceType
	{
		get
		{
			return this.surfaceType;
		}
	}

	// Token: 0x040024BD RID: 9405
	public int depth;

	// Token: 0x040024BE RID: 9406
	public SurfaceType surfaceType;

	// Token: 0x040024BF RID: 9407
	public Collider collider;
}
