using System;
using UnityEngine;

// Token: 0x020006F5 RID: 1781
[Serializable]
public struct ChunkBoundsPair
{
	// Token: 0x06002F0E RID: 12046 RVA: 0x000E0CDC File Offset: 0x000DEEDC
	public Bounds GetBounds()
	{
		if (PlatformFeatureConfig.Get(GamePlatformResolver.Current).shadowMode == PlatformShadowMode.Off)
		{
			return this.withoutShadows;
		}
		return this.withShadows;
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x000E0CFC File Offset: 0x000DEEFC
	public ChunkBoundsPair(Bounds withShadows, Bounds withoutShadows)
	{
		this.withShadows = withShadows;
		this.withoutShadows = withoutShadows;
	}

	// Token: 0x040025F8 RID: 9720
	public Bounds withShadows;

	// Token: 0x040025F9 RID: 9721
	public Bounds withoutShadows;
}
