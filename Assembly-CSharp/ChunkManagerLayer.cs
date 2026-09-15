using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;

// Token: 0x020006F9 RID: 1785
public class ChunkManagerLayer
{
	// Token: 0x17000751 RID: 1873
	// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000E2680 File Offset: 0x000E0880
	public bool IsDynamic
	{
		get
		{
			ChunkManagerLayerType chunkManagerLayerType = this.layerType;
			return chunkManagerLayerType == ChunkManagerLayerType.DynamicWgo || chunkManagerLayerType == ChunkManagerLayerType.DropView;
		}
	}

	// Token: 0x17000752 RID: 1874
	// (get) Token: 0x06002F35 RID: 12085 RVA: 0x000E26A5 File Offset: 0x000E08A5
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x06002F36 RID: 12086 RVA: 0x000E26B0 File Offset: 0x000E08B0
	public ChunkManagerLayer(ChunkManagerLayerType layerType)
	{
		this.layerType = layerType;
		this.count = 0;
		this.chunks = new Chunk[this.count];
	}

	// Token: 0x06002F37 RID: 12087 RVA: 0x000E2790 File Offset: 0x000E0990
	public void AddChunk(Chunk chunk)
	{
		Chunk[] array = new Chunk[this.count + 1];
		Array.Copy(this.chunks, array, this.count);
		float2 @float = new float2(chunk.chunkBounds.center.x, chunk.chunkBounds.center.z);
		this.chunkByCoords[@float] = chunk;
		array[this.count] = chunk;
		this.chunks = array;
		this.count = array.Length;
	}

	// Token: 0x06002F38 RID: 12088 RVA: 0x000E280C File Offset: 0x000E0A0C
	public void RemoveChunk(Chunk chunk)
	{
		chunk.Dispose();
		float2 @float = new float2(chunk.chunkBounds.center.x, chunk.chunkBounds.center.z);
		this.chunkByCoords.Remove(@float);
		int num = Array.IndexOf<Chunk>(this.chunks, chunk);
		Chunk[] array = new Chunk[this.count - 1];
		for (int i = 0; i < this.count - 1; i++)
		{
			array[i] = this.chunks[(i >= num) ? (i + 1) : i];
		}
		this.chunks = array;
		this.count = array.Length;
	}

	// Token: 0x06002F39 RID: 12089 RVA: 0x000E28A8 File Offset: 0x000E0AA8
	public void AddChunks(List<Chunk> newChunks)
	{
		Chunk[] array = new Chunk[this.count + newChunks.Count];
		Array.Copy(this.chunks, array, this.count);
		for (int i = 0; i < newChunks.Count; i++)
		{
			Chunk chunk = newChunks[i];
			array[this.count + i] = chunk;
			float2 @float = new float2(chunk.chunkBounds.center.x, chunk.chunkBounds.center.z);
			this.chunkByCoords[@float] = chunk;
		}
		this.chunks = array;
		this.count = array.Length;
	}

	// Token: 0x06002F3A RID: 12090 RVA: 0x000E2944 File Offset: 0x000E0B44
	public void RemoveChunks(List<Chunk> removeChunks)
	{
		foreach (Chunk chunk in removeChunks)
		{
			chunk.Dispose();
			float2 @float = new float2(chunk.chunkBounds.center.x, chunk.chunkBounds.center.z);
			this.chunkByCoords.Remove(@float);
		}
		Chunk[] array = new Chunk[this.count - removeChunks.Count];
		int num = 0;
		for (int i = 0; i < this.count; i++)
		{
			Chunk chunk2 = this.chunks[i];
			if (!removeChunks.Contains(chunk2))
			{
				array[num++] = chunk2;
			}
		}
		this.chunks = array;
		this.count = array.Length;
	}

	// Token: 0x06002F3B RID: 12091 RVA: 0x000E2A20 File Offset: 0x000E0C20
	public Chunk FindChunk(float3 chunkPos)
	{
		float2 @float = new float2(chunkPos.x, chunkPos.z);
		Chunk chunk;
		if (this.chunkByCoords.TryGetValue(@float, out chunk))
		{
			return chunk;
		}
		for (int i = 0; i < this.count; i++)
		{
			Chunk chunk2 = this.chunks[i];
			if (chunk2.chunkBounds.center.x == chunkPos.x && chunk2.chunkBounds.center.z == chunkPos.z)
			{
				this.chunkByCoords[@float] = chunk2;
				return chunk2;
			}
		}
		return null;
	}

	// Token: 0x06002F3C RID: 12092 RVA: 0x000E2AAC File Offset: 0x000E0CAC
	public void Dispose()
	{
		if (this.chunkDataArray.IsCreated)
		{
			this.chunkDataArray.Dispose();
		}
		if (this.chunkVisibilityStateResults.IsCreated)
		{
			this.chunkVisibilityStateResults.Dispose();
		}
		this.prewarmedObjects.Clear();
	}

	// Token: 0x06002F3D RID: 12093 RVA: 0x000E2AE9 File Offset: 0x000E0CE9
	public void DisposeVisibilityJobBuffers()
	{
		this.dynamicBatchJobBuffer.Dispose();
		this.visibleJobBuffer.Dispose();
		this.prewarmJobBuffer.Dispose();
	}

	// Token: 0x06002F3E RID: 12094 RVA: 0x000E2B0C File Offset: 0x000E0D0C
	public void DisposeChunks()
	{
		for (int i = 0; i < this.chunks.Length; i++)
		{
			this.chunks[i].Dispose();
		}
		this.chunkByCoords.Clear();
	}

	// Token: 0x06002F3F RID: 12095 RVA: 0x000E2B44 File Offset: 0x000E0D44
	public void Clear()
	{
		this.DisposeChunks();
		this.chunks = Array.Empty<Chunk>();
		this.count = 0;
		this.Dispose();
	}

	// Token: 0x06002F40 RID: 12096 RVA: 0x000E2B64 File Offset: 0x000E0D64
	public void EnsureCapacity()
	{
		if (!this.chunkDataArray.IsCreated || this.count != this.chunkDataArray.Length)
		{
			this.Dispose();
			this.chunkDataArray = new NativeArray<BurstableBounds>(this.count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.chunkVisibilityStateResults = new NativeArray<byte>(this.count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}
	}

	// Token: 0x0400260A RID: 9738
	public ChunkManagerLayerType layerType;

	// Token: 0x0400260B RID: 9739
	public Chunk[] chunks;

	// Token: 0x0400260C RID: 9740
	public NativeArray<BurstableBounds> chunkDataArray;

	// Token: 0x0400260D RID: 9741
	public NativeArray<byte> chunkVisibilityStateResults;

	// Token: 0x0400260E RID: 9742
	public float expandFactor = 1.05f;

	// Token: 0x0400260F RID: 9743
	public float prewarmPlanePadding = 1f;

	// Token: 0x04002610 RID: 9744
	private int count;

	// Token: 0x04002611 RID: 9745
	private Dictionary<float2, Chunk> chunkByCoords = new Dictionary<float2, Chunk>();

	// Token: 0x04002612 RID: 9746
	public readonly HashSet<IChunkableObject> prewarmedObjects = new HashSet<IChunkableObject>();

	// Token: 0x04002613 RID: 9747
	public readonly List<IChunkableObject> dynamicUpdateBuffer = new List<IChunkableObject>(256);

	// Token: 0x04002614 RID: 9748
	public readonly List<IChunkableObject> objectsLeavingVisibleBuffer = new List<IChunkableObject>(256);

	// Token: 0x04002615 RID: 9749
	public readonly List<IChunkableObject> visibleCandidatesBuffer = new List<IChunkableObject>(256);

	// Token: 0x04002616 RID: 9750
	public readonly List<IChunkableObject> prewarmCandidatesBuffer = new List<IChunkableObject>(256);

	// Token: 0x04002617 RID: 9751
	public readonly HashSet<IChunkableObject> currentlyVisibleObjectsBuffer = new HashSet<IChunkableObject>();

	// Token: 0x04002618 RID: 9752
	public readonly HashSet<IChunkableObject> currentlyPrewarmedObjectsBuffer = new HashSet<IChunkableObject>();

	// Token: 0x04002619 RID: 9753
	public readonly HashSet<IChunkableObject> objectsToHideBuffer = new HashSet<IChunkableObject>();

	// Token: 0x0400261A RID: 9754
	public readonly ChunkVisibilityJobBuffer dynamicBatchJobBuffer = new ChunkVisibilityJobBuffer();

	// Token: 0x0400261B RID: 9755
	public readonly ChunkVisibilityJobBuffer visibleJobBuffer = new ChunkVisibilityJobBuffer();

	// Token: 0x0400261C RID: 9756
	public readonly ChunkVisibilityJobBuffer prewarmJobBuffer = new ChunkVisibilityJobBuffer();
}
