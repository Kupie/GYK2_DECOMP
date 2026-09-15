using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000591 RID: 1425
[Serializable]
public class GdPointsData
{
	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x060024AE RID: 9390 RVA: 0x000AC277 File Offset: 0x000AA477
	public List<GDPointData> Points
	{
		get
		{
			return this.waypoints.Concat(this.scenePoints).ToList<GDPointData>();
		}
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x000AC290 File Offset: 0x000AA490
	public void PrepareForGame(List<GameSceneConfig> configs)
	{
		this.cache = new GdPointsData.Cache();
		this.waypoints = new List<GDPointData>();
		foreach (GameSceneConfig gameSceneConfig in configs)
		{
			this.InitDataFromConfig(gameSceneConfig);
		}
		foreach (GDPointData gdpointData in this.scenePoints)
		{
			this.cache.AddToCache(gdpointData);
		}
		this.UpdateLinkGraphData();
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x000AC344 File Offset: 0x000AA544
	public void InitScenePointsFromGameScene(GameScene gameScene, GDPoint[] points)
	{
		List<GDPointData> list = new List<GDPointData>();
		foreach (GDPoint gdpoint in points)
		{
			GDPointData gdpointData = this.GetGDPointDataByView(gdpoint);
			if (gdpointData != null)
			{
				gdpoint.Init(gdpointData);
				gdpoint.gameObject.SetActive(gdpointData.Enabled);
			}
			else
			{
				gdpointData = new GDPointData(gdpoint, gameScene.Id, gameScene.GameSceneConfig.sceneGlobalPosition, false);
				list.Add(gdpointData);
				gdpoint.Init(gdpointData);
				gdpoint.gameObject.SetActive(gdpointData.Enabled);
			}
		}
		this.AddScenePoints(list);
		this.UpdateLinkGraphData();
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x000AC3DD File Offset: 0x000AA5DD
	public void AddScenePoint(GDPointData scenePoint)
	{
		this.scenePoints.Add(scenePoint);
		this.cache.AddToCache(scenePoint);
		this.UpdateLinkGraphData();
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x000AC400 File Offset: 0x000AA600
	public void AddScenePoints(List<GDPointData> scenePoints)
	{
		this.scenePoints.AddRange(scenePoints);
		foreach (GDPointData gdpointData in scenePoints)
		{
			this.cache.AddToCache(gdpointData);
		}
		this.UpdateLinkGraphData();
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x000AC468 File Offset: 0x000AA668
	public void RemoveScenePoints(List<GDPointData> scenePoints)
	{
		foreach (GDPointData gdpointData in scenePoints)
		{
			this.scenePoints.Remove(gdpointData);
			GdPointsData.Cache cache = this.cache;
			if (cache != null)
			{
				cache.RemoveFromCache(gdpointData);
			}
		}
		if (this.waypoints != null)
		{
			this.UpdateLinkGraphData();
		}
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x000AC4DC File Offset: 0x000AA6DC
	public GDPointData GetGDPointDataById(string gdPointId)
	{
		List<GDPointData> list;
		if (this.cache.allPointsByIdCache.TryGetValue(gdPointId, out list))
		{
			return list[0];
		}
		Debug.LogWarning("Can't find gd point with Id [" + gdPointId + "]");
		return null;
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000AC51C File Offset: 0x000AA71C
	public List<GDPointData> GetGDPointsDataById(string gdPointId)
	{
		List<GDPointData> list;
		if (this.cache.allPointsByIdCache.TryGetValue(gdPointId, out list))
		{
			return list;
		}
		return new List<GDPointData>();
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000AC548 File Offset: 0x000AA748
	public GDPointData GetGDPointDataByCustomTag(string customTag)
	{
		List<GDPointData> list;
		if (this.cache.allPointsByCustomTagCache.TryGetValue(customTag, out list))
		{
			return list[0];
		}
		Debug.LogWarning("Can't find gd point with customTag [" + customTag + "]");
		return null;
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000AC588 File Offset: 0x000AA788
	public List<GDPointData> GetGDPointsDataByCustomTag(string customTag)
	{
		List<GDPointData> list;
		if (this.cache.allPointsByCustomTagCache.TryGetValue(customTag, out list))
		{
			return list;
		}
		return new List<GDPointData>();
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000AC5B4 File Offset: 0x000AA7B4
	public GDPointData GetGDPointDataByView(GDPoint gdPoint)
	{
		List<GDPointData> list;
		if (this.cache.allPointsByIdCache.TryGetValue(gdPoint.Id, out list))
		{
			foreach (GDPointData gdpointData in list)
			{
				if ((gdPoint.transform.position - gdpointData.Position).sqrMagnitude < 0.001f)
				{
					return gdpointData;
				}
			}
			return list[0];
		}
		return null;
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x000AC64C File Offset: 0x000AA84C
	public GDPointData GetGDPointDataByInstanceId(int instanceId)
	{
		return this.cache.allPointsByInstanceIdCache.GetValueOrDefault(instanceId);
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000AC660 File Offset: 0x000AA860
	private void UpdateLinkGraphData()
	{
		foreach (GDPointData gdpointData in this.Points)
		{
			gdpointData.LinkNextGdPointsData();
		}
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000AC6B0 File Offset: 0x000AA8B0
	private void InitDataFromConfig(GameSceneConfig gameSceneConfig)
	{
		GDPointData[] gdPointsData = gameSceneConfig.GdPointsData;
		for (int i = 0; i < gdPointsData.Length; i++)
		{
			GDPointData gdpointData = new GDPointData(gdPointsData[i]);
			this.waypoints.Add(gdpointData);
			this.cache.AddToCache(gdpointData);
		}
	}

	// Token: 0x04002067 RID: 8295
	[SerializeField]
	private List<GDPointData> scenePoints = new List<GDPointData>();

	// Token: 0x04002068 RID: 8296
	private List<GDPointData> waypoints;

	// Token: 0x04002069 RID: 8297
	private GdPointsData.Cache cache;

	// Token: 0x02000592 RID: 1426
	private class Cache
	{
		// Token: 0x060024BD RID: 9405 RVA: 0x000AC708 File Offset: 0x000AA908
		public void AddToCache(GDPointData gdPointData)
		{
			List<GDPointData> list;
			if (this.allPointsByIdCache.TryGetValue(gdPointData.Id, out list))
			{
				list.Add(gdPointData);
			}
			else
			{
				this.allPointsByIdCache.Add(gdPointData.Id, new List<GDPointData> { gdPointData });
			}
			if (!string.IsNullOrEmpty(gdPointData.CustomTag))
			{
				if (this.allPointsByCustomTagCache.TryGetValue(gdPointData.CustomTag, out list))
				{
					list.Add(gdPointData);
				}
				else
				{
					this.allPointsByCustomTagCache.Add(gdPointData.CustomTag, new List<GDPointData> { gdPointData });
				}
			}
			this.allPointsByInstanceIdCache.TryAdd(gdPointData.InstanceId, gdPointData);
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000AC7AC File Offset: 0x000AA9AC
		public void RemoveFromCache(GDPointData gdPointData)
		{
			List<GDPointData> list;
			if (this.allPointsByIdCache.TryGetValue(gdPointData.Id, out list))
			{
				list.Remove(gdPointData);
				if (list.Count == 0)
				{
					this.allPointsByIdCache.Remove(gdPointData.Id);
				}
			}
			if (!string.IsNullOrEmpty(gdPointData.CustomTag) && this.allPointsByCustomTagCache.TryGetValue(gdPointData.CustomTag, out list))
			{
				list.Remove(gdPointData);
				if (list.Count == 0)
				{
					this.allPointsByCustomTagCache.Remove(gdPointData.Id);
				}
			}
			this.allPointsByInstanceIdCache.Remove(gdPointData.InstanceId);
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000AC846 File Offset: 0x000AAA46
		public void Clear()
		{
			this.allPointsByIdCache.Clear();
			this.allPointsByCustomTagCache.Clear();
			this.allPointsByInstanceIdCache.Clear();
		}

		// Token: 0x0400206A RID: 8298
		public Dictionary<string, List<GDPointData>> allPointsByIdCache = new Dictionary<string, List<GDPointData>>();

		// Token: 0x0400206B RID: 8299
		public Dictionary<string, List<GDPointData>> allPointsByCustomTagCache = new Dictionary<string, List<GDPointData>>();

		// Token: 0x0400206C RID: 8300
		public Dictionary<int, GDPointData> allPointsByInstanceIdCache = new Dictionary<int, GDPointData>();
	}
}
