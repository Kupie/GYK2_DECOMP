using System;

// Token: 0x020006EE RID: 1774
[Serializable]
public struct BurstableChunkBoundsPair
{
	// Token: 0x06002EDC RID: 11996 RVA: 0x000E0321 File Offset: 0x000DE521
	public BurstableBounds GetBounds()
	{
		if (PlatformFeatureConfig.Get(GamePlatformResolver.Current).shadowMode == PlatformShadowMode.Off)
		{
			return this.withoutShadows;
		}
		return this.withShadows;
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x000E0341 File Offset: 0x000DE541
	public BurstableChunkBoundsPair(BurstableBounds withShadows, BurstableBounds withoutShadows)
	{
		this.withShadows = withShadows;
		this.withoutShadows = withoutShadows;
	}

	// Token: 0x040025D5 RID: 9685
	public BurstableBounds withShadows;

	// Token: 0x040025D6 RID: 9686
	public BurstableBounds withoutShadows;
}
