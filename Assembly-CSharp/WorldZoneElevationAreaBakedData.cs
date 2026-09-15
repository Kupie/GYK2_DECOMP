using System;
using UnityEngine;

// Token: 0x020001C9 RID: 457
[Serializable]
public struct WorldZoneElevationAreaBakedData
{
	// Token: 0x06000BC3 RID: 3011 RVA: 0x0003B5B7 File Offset: 0x000397B7
	public bool ContainsXZ(Vector2 xz)
	{
		return this.xzRect.Contains(xz);
	}

	// Token: 0x04000CC4 RID: 3268
	public Rect xzRect;

	// Token: 0x04000CC5 RID: 3269
	public float elevationY;
}
