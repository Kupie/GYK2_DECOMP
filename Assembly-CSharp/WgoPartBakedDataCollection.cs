using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;

// Token: 0x020005CC RID: 1484
[CreateAssetMenu(menuName = "WgoPartBakedDataCollection")]
public class WgoPartBakedDataCollection : LazySingletonSerializedSO<WgoPartBakedDataCollection>
{
	// Token: 0x060026E5 RID: 9957 RVA: 0x000B6C03 File Offset: 0x000B4E03
	public void AddOrUpdate(WgoPartBakedData data)
	{
		if (data == null || string.IsNullOrEmpty(data.id))
		{
			Debug.LogWarning("Can not add null or empty id WgoPartBakedData.");
			return;
		}
		this.EnsureCacheLoaded();
		this.cachedData[data.id] = data;
	}

	// Token: 0x060026E6 RID: 9958 RVA: 0x000B6C38 File Offset: 0x000B4E38
	public WgoPartBakedData Get(string id)
	{
		this.EnsureCacheLoaded();
		WgoPartBakedData wgoPartBakedData;
		this.cachedData.TryGetValue(id, out wgoPartBakedData);
		if (wgoPartBakedData == null)
		{
			Debug.LogWarning("No WgoPartBakedData with id:[" + id + "]");
			return WgoPartBakedData.Empty;
		}
		return wgoPartBakedData;
	}

	// Token: 0x060026E7 RID: 9959 RVA: 0x000B6C79 File Offset: 0x000B4E79
	public void ClearCache()
	{
		if (this.cachedData == null)
		{
			this.cachedData = new Dictionary<string, WgoPartBakedData>();
		}
		this.cachedData.Clear();
		this.isCacheLoaded = false;
	}

	// Token: 0x060026E8 RID: 9960 RVA: 0x000B6CA0 File Offset: 0x000B4EA0
	public void Clear()
	{
		this.ClearCache();
	}

	// Token: 0x060026E9 RID: 9961 RVA: 0x000B6CA8 File Offset: 0x000B4EA8
	public void LoadCache()
	{
		this.EnsureCacheLoaded();
	}

	// Token: 0x060026EA RID: 9962 RVA: 0x000B6CB0 File Offset: 0x000B4EB0
	private void EnsureCacheLoaded()
	{
		if (this.isCacheLoaded)
		{
			return;
		}
		if (this.cachedData == null)
		{
			this.cachedData = new Dictionary<string, WgoPartBakedData>();
		}
		this.cachedData.Clear();
		foreach (WgoPartBakedData wgoPartBakedData in this.dataList)
		{
			if (wgoPartBakedData != null && !string.IsNullOrEmpty(wgoPartBakedData.id))
			{
				this.cachedData[wgoPartBakedData.id] = wgoPartBakedData;
			}
		}
		Debug.Log(string.Format("WgoPartBakedDataCollection loaded [{0}] items", this.cachedData.Count));
		this.isCacheLoaded = true;
	}

	// Token: 0x060026EB RID: 9963 RVA: 0x000B6D6C File Offset: 0x000B4F6C
	private void OnEnable()
	{
		this.isCacheLoaded = false;
	}

	// Token: 0x04002172 RID: 8562
	private const string DATA_DIRECTORY = "Assets/AddressableAssets/Configurations/WgoPartBakedData";

	// Token: 0x04002173 RID: 8563
	private const string ADDRESSABLE_GROUP = "WgoPartBakedData";

	// Token: 0x04002174 RID: 8564
	private const string ADDRESSABLE_LABEL = "WgoPartBakedData";

	// Token: 0x04002175 RID: 8565
	private const string ADDRESSABLE_ADDRESS_PREFIX = "WgoPartBakedData";

	// Token: 0x04002176 RID: 8566
	[OdinSerialize]
	private List<WgoPartBakedData> dataList = new List<WgoPartBakedData>();

	// Token: 0x04002177 RID: 8567
	private Dictionary<string, WgoPartBakedData> cachedData = new Dictionary<string, WgoPartBakedData>();

	// Token: 0x04002178 RID: 8568
	private bool isCacheLoaded;
}
