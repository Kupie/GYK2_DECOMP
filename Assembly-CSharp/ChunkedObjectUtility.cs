using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000A96 RID: 2710
public static class ChunkedObjectUtility
{
	// Token: 0x0600498D RID: 18829 RVA: 0x0015B784 File Offset: 0x00159984
	public static void UpdateFlag(this IChunkableObject chunkableObject, ChunkingIgnoreType type, bool newValue)
	{
		if (chunkableObject != null)
		{
			global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
			if (@object == null || !(@object == null))
			{
				if (chunkableObject.IgnoreMultiFlag == null)
				{
					chunkableObject.IgnoreMultiFlag = new MultiFlagOR<ChunkingIgnoreType>();
				}
				chunkableObject.IgnoreMultiFlag.UpdateFlag(type, newValue);
				if (chunkableObject.IgnoreMultiFlag.ResultFlag)
				{
					chunkableObject.UpdateChunkVisibility(true);
					return;
				}
				LazySingleton<ChunkManager>.Instance.RequestVisibilityRecheck(chunkableObject);
				return;
			}
		}
	}
}
