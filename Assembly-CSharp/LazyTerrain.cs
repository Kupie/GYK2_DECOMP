using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006AE RID: 1710
public class LazyTerrain : MonoBehaviour
{
	// Token: 0x06002DAD RID: 11693 RVA: 0x000DAC28 File Offset: 0x000D8E28
	private void Awake()
	{
		if (Application.isPlaying && this.meshesData.Count > 0)
		{
			for (int i = 0; i < this.meshesData.Count; i++)
			{
				this.meshesData[i].Init(this);
			}
			LazySingleton<ChunkManager>.Instance.RegisterChunks<LazyTerrainMeshData>(this.meshesData, ChunkManagerLayerType.StaticObjects);
		}
	}

	// Token: 0x06002DAE RID: 11694 RVA: 0x000DAC84 File Offset: 0x000D8E84
	private void OnDestroy()
	{
		if (!Application.isPlaying || this.meshesData.Count == 0 || GameShutdown.IsQuitting)
		{
			return;
		}
		foreach (LazyTerrainMeshData lazyTerrainMeshData in this.meshesData)
		{
			lazyTerrainMeshData.UpdateChunkVisibility(false);
		}
		LazySingleton<ChunkManager>.Instance.UnregisterChunks<LazyTerrainMeshData>(this.meshesData, ChunkManagerLayerType.StaticObjects);
	}

	// Token: 0x040024A3 RID: 9379
	public const float Y_OFFSET = -0.005f;

	// Token: 0x040024A4 RID: 9380
	public int width = 100;

	// Token: 0x040024A5 RID: 9381
	public int height = 100;

	// Token: 0x040024A6 RID: 9382
	public int centerHor;

	// Token: 0x040024A7 RID: 9383
	public int centerVert;

	// Token: 0x040024A8 RID: 9384
	public int meshSize = 5;

	// Token: 0x040024A9 RID: 9385
	public Vector2 meshTileSize = new Vector2(0.96f, 1.2f);

	// Token: 0x040024AA RID: 9386
	public Material material;

	// Token: 0x040024AB RID: 9387
	public LazyTerrainConfig lazyTerrainConfig;

	// Token: 0x040024AC RID: 9388
	public List<LazyTerrainMesh> meshes = new List<LazyTerrainMesh>();

	// Token: 0x040024AD RID: 9389
	public List<LazyTerrainMeshData> meshesData = new List<LazyTerrainMeshData>();

	// Token: 0x040024AE RID: 9390
	public bool castShadows;
}
