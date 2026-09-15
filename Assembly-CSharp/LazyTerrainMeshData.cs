using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020006BB RID: 1723
[Serializable]
public class LazyTerrainMeshData : IChunkableObject
{
	// Token: 0x06002DC8 RID: 11720 RVA: 0x000DB093 File Offset: 0x000D9293
	public LazyTerrainMeshData(Mesh mesh, Vector3 localPosition, Vector3 center, Bounds bounds)
	{
		this.mesh = mesh;
		this.localPosition = localPosition;
		this.center = center;
		this.bounds = bounds;
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x000DB0C0 File Offset: 0x000D92C0
	public void Init(LazyTerrain lazyTerrain)
	{
		this.lazyTerrain = lazyTerrain;
		this.chunkBounds = new BurstableBounds(this.localPosition + lazyTerrain.transform.position, new Vector3(7f, 0f, 7f) * ((float)lazyTerrain.meshSize / 5f));
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x000DB125 File Offset: 0x000D9325
	public BurstableBounds GetChunkableData()
	{
		return this.chunkBounds;
	}

	// Token: 0x17000724 RID: 1828
	// (get) Token: 0x06002DCB RID: 11723 RVA: 0x000DB12D File Offset: 0x000D932D
	// (set) Token: 0x06002DCC RID: 11724 RVA: 0x000DB135 File Offset: 0x000D9335
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x17000725 RID: 1829
	// (get) Token: 0x06002DCD RID: 11725 RVA: 0x00028294 File Offset: 0x00026494
	public bool IgnoreChunkVisibility
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x000DB140 File Offset: 0x000D9340
	public void UpdateChunkVisibility(bool isVisible)
	{
		if (this.isVisible == isVisible)
		{
			return;
		}
		this.isVisible = isVisible;
		if (!isVisible)
		{
			if (this.hasGrass)
			{
				LazyTerrainMeshPool.ReleaseGrass(this.grassView);
				this.grassView = null;
			}
			LazyTerrainMeshPool.ReleaseMesh(this.meshView);
			this.meshView = null;
			return;
		}
		this.meshView = LazyTerrainMeshPool.GetMesh();
		if (this.hasGrass)
		{
			this.grassView = LazyTerrainMeshPool.GetGrass();
			this.meshView.DrawFromData(this, this.lazyTerrain, this.grassView);
			return;
		}
		this.meshView.DrawFromData(this, this.lazyTerrain, null);
	}

	// Token: 0x040024D3 RID: 9427
	public Mesh mesh;

	// Token: 0x040024D4 RID: 9428
	public Vector3 localPosition;

	// Token: 0x040024D5 RID: 9429
	public Vector3 center;

	// Token: 0x040024D6 RID: 9430
	public Bounds bounds;

	// Token: 0x040024D7 RID: 9431
	public DeformingGrassData deformingGrassData;

	// Token: 0x040024D8 RID: 9432
	public bool hasGrass;

	// Token: 0x040024D9 RID: 9433
	[NonSerialized]
	public LazyTerrain lazyTerrain;

	// Token: 0x040024DA RID: 9434
	private bool isVisible = true;

	// Token: 0x040024DB RID: 9435
	private BurstableBounds chunkBounds;

	// Token: 0x040024DC RID: 9436
	private LazyTerrainMesh meshView;

	// Token: 0x040024DD RID: 9437
	private DeformingGrass grassView;
}
