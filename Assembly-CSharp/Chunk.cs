using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x020006F0 RID: 1776
public class Chunk : IChunkableObject
{
	// Token: 0x17000748 RID: 1864
	// (get) Token: 0x06002EDF RID: 11999 RVA: 0x000E036D File Offset: 0x000DE56D
	public int Count
	{
		get
		{
			return this.chunkableObjects.Count;
		}
	}

	// Token: 0x06002EE0 RID: 12000 RVA: 0x000E037A File Offset: 0x000DE57A
	public Chunk(float3 position, float3 size)
	{
		this.chunkBounds = new BurstableBounds(position, size);
		this.chunkableObjects = new List<IChunkableObject>(100);
		this.chunkableObjectsSet = new HashSet<IChunkableObject>(100);
	}

	// Token: 0x06002EE1 RID: 12001 RVA: 0x000E03B0 File Offset: 0x000DE5B0
	public void AddChunkableObject(IChunkableObject chunkableObject)
	{
		this.chunkableObjects.Add(chunkableObject);
		this.chunkableObjectsSet.Add(chunkableObject);
	}

	// Token: 0x06002EE2 RID: 12002 RVA: 0x000E03CB File Offset: 0x000DE5CB
	public void RemoveChunkableObject(IChunkableObject chunkableObject)
	{
		this.chunkableObjects.Remove(chunkableObject);
		this.chunkableObjectsSet.Remove(chunkableObject);
	}

	// Token: 0x06002EE3 RID: 12003 RVA: 0x000E03E7 File Offset: 0x000DE5E7
	public void SetChunkableObjects(List<IChunkableObject> newChunkableObjects)
	{
		this.chunkableObjects = newChunkableObjects;
		this.chunkableObjectsSet = new HashSet<IChunkableObject>(this.chunkableObjects);
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x000E0401 File Offset: 0x000DE601
	public void ClearChunkableObjects()
	{
		this.chunkableObjects.Clear();
		this.chunkableObjectsSet.Clear();
	}

	// Token: 0x06002EE5 RID: 12005 RVA: 0x000E0419 File Offset: 0x000DE619
	public void AddChunkableObjects(List<IChunkableObject> newChunkableObjects)
	{
		this.chunkableObjects.AddRange(newChunkableObjects);
		this.chunkableObjectsSet.UnionWith(newChunkableObjects);
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x000E0433 File Offset: 0x000DE633
	public void RemoveChunkableObjects(List<IChunkableObject> removeChunkableObjects)
	{
		this.chunkableObjects.RemoveAll(new Predicate<IChunkableObject>(removeChunkableObjects.Contains));
		this.chunkableObjectsSet.ExceptWith(removeChunkableObjects);
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x000E045A File Offset: 0x000DE65A
	public bool ContainsChunkableObject(IChunkableObject chunkableObject)
	{
		return this.chunkableObjectsSet.Contains(chunkableObject);
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x000E0468 File Offset: 0x000DE668
	public void Dispose()
	{
		if (this.chunkableDataArray.IsCreated)
		{
			this.chunkableDataArray.Dispose();
		}
		if (this.visibilityResults.IsCreated)
		{
			this.visibilityResults.Dispose();
		}
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x000E049C File Offset: 0x000DE69C
	public void EnsureCapacity()
	{
		if (this.chunkableObjects.Count != this.chunkableDataArray.Length)
		{
			this.Dispose();
			this.chunkableDataArray = new NativeArray<BurstableBounds>(this.chunkableObjects.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			this.visibilityResults = new NativeArray<bool>(this.chunkableObjects.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x000E04F7 File Offset: 0x000DE6F7
	public BurstableBounds GetChunkableData()
	{
		return this.chunkBounds;
	}

	// Token: 0x17000749 RID: 1865
	// (get) Token: 0x06002EEB RID: 12011 RVA: 0x000E04FF File Offset: 0x000DE6FF
	// (set) Token: 0x06002EEC RID: 12012 RVA: 0x000E0507 File Offset: 0x000DE707
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x1700074A RID: 1866
	// (get) Token: 0x06002EED RID: 12013 RVA: 0x00028294 File Offset: 0x00026494
	public bool IgnoreChunkVisibility
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x000E0510 File Offset: 0x000DE710
	public void UpdateChunkVisibility(bool isVisible)
	{
		for (int i = 0; i < this.chunkableObjects.Count; i++)
		{
			IChunkableObject chunkableObject = this.chunkableObjects[i];
			if (chunkableObject != null)
			{
				global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
				if (@object == null || !(@object == null))
				{
					chunkableObject.UpdateChunkVisibility(isVisible);
				}
			}
		}
	}

	// Token: 0x040025D9 RID: 9689
	public BurstableBounds chunkBounds;

	// Token: 0x040025DA RID: 9690
	public bool isVisible = true;

	// Token: 0x040025DB RID: 9691
	public List<IChunkableObject> chunkableObjects;

	// Token: 0x040025DC RID: 9692
	public NativeArray<BurstableBounds> chunkableDataArray;

	// Token: 0x040025DD RID: 9693
	public NativeArray<bool> visibilityResults;

	// Token: 0x040025DE RID: 9694
	private HashSet<IChunkableObject> chunkableObjectsSet;
}
