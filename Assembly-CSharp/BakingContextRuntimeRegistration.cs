using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006EA RID: 1770
public static class BakingContextRuntimeRegistration
{
	// Token: 0x06002ECF RID: 11983 RVA: 0x000E007C File Offset: 0x000DE27C
	public static void Register(IBakingContext context, List<BakedChunkableObjectComponentData> registrationCache)
	{
		if (context == null || !Application.isPlaying)
		{
			return;
		}
		BakingContextRuntimeRegistration.Unregister(registrationCache);
		foreach (BakedChunkableObjectComponentData bakedChunkableObjectComponentData in context.GetBakedData)
		{
			if (bakedChunkableObjectComponentData != null)
			{
				registrationCache.Add(bakedChunkableObjectComponentData);
				bakedChunkableObjectComponentData.UpdateChunkVisibility(false);
			}
		}
		if (registrationCache.Count == 0)
		{
			return;
		}
		ChunkManager instance = LazySingleton<ChunkManager>.Instance;
		if (instance == null)
		{
			return;
		}
		instance.RegisterChunks<BakedChunkableObjectComponentData>(registrationCache, ChunkManagerLayerType.FightingLevelStaticObjects);
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x000E010C File Offset: 0x000DE30C
	public static void Unregister(List<BakedChunkableObjectComponentData> registrationCache)
	{
		if (registrationCache == null || registrationCache.Count == 0)
		{
			return;
		}
		if (Application.isPlaying)
		{
			ChunkManager instance = LazySingleton<ChunkManager>.Instance;
			if (instance != null)
			{
				instance.UnregisterChunks<BakedChunkableObjectComponentData>(registrationCache, ChunkManagerLayerType.FightingLevelStaticObjects);
			}
		}
		foreach (BakedChunkableObjectComponentData bakedChunkableObjectComponentData in registrationCache)
		{
			bakedChunkableObjectComponentData.UpdateChunkVisibility(false);
		}
		registrationCache.Clear();
	}
}
