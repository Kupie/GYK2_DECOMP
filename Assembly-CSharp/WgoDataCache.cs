using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D0 RID: 1488
public class WgoDataCache
{
	// Token: 0x06002713 RID: 10003 RVA: 0x000B7B84 File Offset: 0x000B5D84
	public void AddWgoDataToCache(WgoData wgoData)
	{
		if (!this.wgoDataByUidCache.TryAdd(wgoData.UniqueId.Guid, wgoData))
		{
			Debug.LogError(string.Concat(new string[]
			{
				string.Format("WgoData with [{0}] already exists in cache, id: {1}, scene: {2}", wgoData.UniqueId, wgoData.id, wgoData.WorldId),
				", duplicates: ",
				this.wgoDataByUidCache[wgoData.UniqueId.Guid].id,
				" from scene: ",
				this.wgoDataByUidCache[wgoData.UniqueId.Guid].WorldId
			}));
			return;
		}
		List<WgoData> list;
		if (this.wgoDataByIdsCache.TryGetValue(wgoData.id, out list))
		{
			list.Add(wgoData);
		}
		else
		{
			this.wgoDataByIdsCache.Add(wgoData.id, new List<WgoData> { wgoData });
		}
		if (!string.IsNullOrEmpty(wgoData.CustomTag))
		{
			List<WgoData> list2;
			if (this.wgoDataByCustomTagsCache.TryGetValue(wgoData.CustomTag, out list2))
			{
				list2.Add(wgoData);
			}
			else
			{
				this.wgoDataByCustomTagsCache.Add(wgoData.CustomTag, new List<WgoData> { wgoData });
			}
		}
		if (!string.IsNullOrEmpty(wgoData.Definition.wgoGroup))
		{
			List<WgoData> list3;
			if (this.wgoDataByGroup.TryGetValue(wgoData.Definition.wgoGroup, out list3))
			{
				list3.Add(wgoData);
				return;
			}
			this.wgoDataByGroup.Add(wgoData.Definition.wgoGroup, new List<WgoData> { wgoData });
		}
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x000B7D00 File Offset: 0x000B5F00
	public bool RemoveWgoDataFromCache(WgoData wgoData)
	{
		if (!this.wgoDataByUidCache.Remove(wgoData.UniqueId.Guid))
		{
			return false;
		}
		WgoDataCache.RemoveFromListCache<WgoData>(this.wgoDataByIdsCache, wgoData.id, wgoData);
		WgoDataCache.RemoveFromListCache<WgoData>(this.wgoDataByCustomTagsCache, wgoData.CustomTag, wgoData);
		Dictionary<string, List<WgoData>> dictionary = this.wgoDataByGroup;
		WGODef definition = wgoData.Definition;
		WgoDataCache.RemoveFromListCache<WgoData>(dictionary, (definition != null) ? definition.wgoGroup : null, wgoData);
		return true;
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x000B7D6C File Offset: 0x000B5F6C
	public void AddWsoDataToCache(WsoData wsoData)
	{
		if (!this.wsoDataByUidCache.TryAdd(wsoData.UniqueId.Guid, wsoData))
		{
			WsoData wsoData2 = this.wsoDataByUidCache[wsoData.UniqueId.Guid];
			Debug.LogError(string.Concat(new string[]
			{
				string.Format("WsoData with [{0}] already exists in cache, id: {1}, scene: {2}", wsoData.UniqueId, wsoData.id, wsoData.WorldId),
				", duplicates: ",
				wsoData2.id,
				" from scene: ",
				wsoData2.WorldId
			}));
			return;
		}
		if (!string.IsNullOrEmpty(wsoData.id))
		{
			List<WsoData> list;
			if (this.wsoDataByIdCache.TryGetValue(wsoData.id, out list))
			{
				list.Add(wsoData);
			}
			else
			{
				this.wsoDataByIdCache.Add(wsoData.id, new List<WsoData> { wsoData });
			}
		}
		if (string.IsNullOrEmpty(wsoData.CustomTag))
		{
			return;
		}
		List<WsoData> list2;
		if (this.wsoDataByCustomTagsCache.TryGetValue(wsoData.CustomTag, out list2))
		{
			list2.Add(wsoData);
			return;
		}
		this.wsoDataByCustomTagsCache.Add(wsoData.CustomTag, new List<WsoData> { wsoData });
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x000B7E8C File Offset: 0x000B608C
	public bool RemoveWsoDataFromCache(WsoData wsoData)
	{
		if (!this.wsoDataByUidCache.Remove(wsoData.UniqueId.Guid))
		{
			return false;
		}
		WgoDataCache.RemoveFromListCache<WsoData>(this.wsoDataByIdCache, wsoData.id, wsoData);
		WgoDataCache.RemoveFromListCache<WsoData>(this.wsoDataByCustomTagsCache, wsoData.CustomTag, wsoData);
		return true;
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x000B7ED8 File Offset: 0x000B60D8
	private static void RemoveFromListCache<T>(Dictionary<string, List<T>> cache, string key, T item)
	{
		if (string.IsNullOrEmpty(key))
		{
			return;
		}
		List<T> list;
		if (!cache.TryGetValue(key, out list))
		{
			return;
		}
		list.Remove(item);
		if (list.Count == 0)
		{
			cache.Remove(key);
		}
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x000B7F14 File Offset: 0x000B6114
	public WsoData GetWsoDataFromCache(SGuid uniqueId)
	{
		if (SGuid.IsNullOrEmpty(uniqueId))
		{
			return null;
		}
		WsoData wsoData;
		this.wsoDataByUidCache.TryGetValue(uniqueId.Guid, out wsoData);
		return wsoData;
	}

	// Token: 0x04002186 RID: 8582
	public Dictionary<Guid, WgoData> wgoDataByUidCache = new Dictionary<Guid, WgoData>();

	// Token: 0x04002187 RID: 8583
	public Dictionary<string, List<WgoData>> wgoDataByIdsCache = new Dictionary<string, List<WgoData>>();

	// Token: 0x04002188 RID: 8584
	public Dictionary<string, List<WgoData>> wgoDataByCustomTagsCache = new Dictionary<string, List<WgoData>>();

	// Token: 0x04002189 RID: 8585
	public Dictionary<string, List<WgoData>> wgoDataByGroup = new Dictionary<string, List<WgoData>>();

	// Token: 0x0400218A RID: 8586
	public Dictionary<Guid, WsoData> wsoDataByUidCache = new Dictionary<Guid, WsoData>();

	// Token: 0x0400218B RID: 8587
	public Dictionary<string, List<WsoData>> wsoDataByIdCache = new Dictionary<string, List<WsoData>>();

	// Token: 0x0400218C RID: 8588
	public Dictionary<string, List<WsoData>> wsoDataByCustomTagsCache = new Dictionary<string, List<WsoData>>();
}
