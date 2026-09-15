using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A9B RID: 2715
[Serializable]
public class ConstructorPartBoundsCollection
{
	// Token: 0x17000B27 RID: 2855
	// (get) Token: 0x0600499A RID: 18842 RVA: 0x0015BBA3 File Offset: 0x00159DA3
	public int Count
	{
		get
		{
			return this.entries.Count;
		}
	}

	// Token: 0x0600499B RID: 18843 RVA: 0x0015BBB0 File Offset: 0x00159DB0
	public void SetBounds(string assetPath, ChunkBoundsPair localBounds)
	{
		if (string.IsNullOrEmpty(assetPath))
		{
			return;
		}
		this.entries.RemoveAll((ConstructorPartBoundsCollection.Entry e) => e.assetPath == assetPath);
		this.entries.Add(new ConstructorPartBoundsCollection.Entry
		{
			assetPath = assetPath,
			localBounds = localBounds
		});
		this.lookup = null;
	}

	// Token: 0x0600499C RID: 18844 RVA: 0x0015BC1A File Offset: 0x00159E1A
	public bool TryGetBounds(string assetPath, out ChunkBoundsPair localBounds)
	{
		if (this.lookup == null)
		{
			this.BuildLookup();
		}
		return this.lookup.TryGetValue(assetPath, out localBounds);
	}

	// Token: 0x0600499D RID: 18845 RVA: 0x0015BC38 File Offset: 0x00159E38
	public bool TryGetBounds(string assetPath, Vector3 worldPosition, Vector3 lossyScale, out BurstableChunkBoundsPair worldBounds)
	{
		if (this.lookup == null)
		{
			this.BuildLookup();
		}
		ChunkBoundsPair chunkBoundsPair;
		if (!this.lookup.TryGetValue(assetPath, out chunkBoundsPair))
		{
			worldBounds = default(BurstableChunkBoundsPair);
			return false;
		}
		Vector3 vector = new Vector3(Mathf.Abs(lossyScale.x), Mathf.Abs(lossyScale.y), Mathf.Abs(lossyScale.z));
		Vector3 vector2 = Vector3.Scale(chunkBoundsPair.withShadows.size, vector);
		Vector3 vector3 = Vector3.Scale(chunkBoundsPair.withoutShadows.size, vector);
		worldBounds = new BurstableChunkBoundsPair(new BurstableBounds(chunkBoundsPair.withShadows.center + worldPosition, vector2), new BurstableBounds(chunkBoundsPair.withoutShadows.center + worldPosition, vector3));
		return true;
	}

	// Token: 0x0600499E RID: 18846 RVA: 0x0015BD0C File Offset: 0x00159F0C
	public void BuildLookup()
	{
		this.lookup = new Dictionary<string, ChunkBoundsPair>(this.entries.Count);
		foreach (ConstructorPartBoundsCollection.Entry entry in this.entries)
		{
			if (!string.IsNullOrEmpty(entry.assetPath))
			{
				this.lookup[entry.assetPath] = entry.localBounds;
			}
		}
	}

	// Token: 0x0600499F RID: 18847 RVA: 0x0015BD94 File Offset: 0x00159F94
	public void Clear()
	{
		this.entries.Clear();
		this.lookup = null;
	}

	// Token: 0x04003968 RID: 14696
	[SerializeField]
	private List<ConstructorPartBoundsCollection.Entry> entries = new List<ConstructorPartBoundsCollection.Entry>();

	// Token: 0x04003969 RID: 14697
	private Dictionary<string, ChunkBoundsPair> lookup;

	// Token: 0x02000A9C RID: 2716
	[Serializable]
	private class Entry
	{
		// Token: 0x0400396A RID: 14698
		[SerializeField]
		public string assetPath;

		// Token: 0x0400396B RID: 14699
		[SerializeField]
		public ChunkBoundsPair localBounds;
	}
}
