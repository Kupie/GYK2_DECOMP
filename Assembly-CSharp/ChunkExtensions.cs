using System;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006F6 RID: 1782
public static class ChunkExtensions
{
	// Token: 0x1700074F RID: 1871
	// (get) Token: 0x06002F10 RID: 12048 RVA: 0x00028294 File Offset: 0x00026494
	// (set) Token: 0x06002F11 RID: 12049 RVA: 0x000E0D0C File Offset: 0x000DEF0C
	public static bool DrawGizmos
	{
		get
		{
			return false;
		}
		set
		{
			PlayerPrefs.SetInt("draw_chink_gizmos", value.ToInt(0));
		}
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x000E0D20 File Offset: 0x000DEF20
	public static void DrawChunkGizmos(this IChunkableObject chunkableObject)
	{
		if (ChunkExtensions.DrawGizmos)
		{
			BurstableBounds chunkableData = chunkableObject.GetChunkableData();
			if (chunkableData.Extents.Equals(float3.zero))
			{
				Vector3 vector = chunkableData.center;
				Gizmos.color = Color.cyan;
				Gizmos.DrawLine(vector, vector + Vector3.up * 5f);
				Gizmos.DrawSphere(vector, 0.05f);
				return;
			}
			Gizmos.DrawWireCube(chunkableData.center, chunkableData.size);
		}
	}

	// Token: 0x040025FA RID: 9722
	public const string KEY_DRAW_GIZMOS = "draw_chink_gizmos";
}
