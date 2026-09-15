using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using Pathfinding;
using Pathfinding.Graphs.Navmesh;
using UnityEngine;

// Token: 0x0200072B RID: 1835
public class GlobalNavigationManager : LazySingleton<GlobalNavigationManager>
{
	// Token: 0x06002FE9 RID: 12265 RVA: 0x000E5BB8 File Offset: 0x000E3DB8
	public void InitRecastGraph(LazyConsts.Navigation.Graph graph, Vector3 center, Vector2 size, bool dontUseRecastFloor = false, float height = 1f, IReadOnlyList<WorldZoneNavigationHoleBakedData> navigationHoles = null)
	{
		if (graph == LazyConsts.Navigation.Graph.None)
		{
			return;
		}
		if ((LazyConsts.Navigation.Graph)this.astarPath.graphs.Length < graph + 1)
		{
			Debug.LogWarning(string.Format("Graph {0} not specified in {1}", graph, this.astarPath), this.astarPath);
			return;
		}
		RecastGraph recastGraph = this.astarPath.graphs[(int)graph] as RecastGraph;
		if (recastGraph == null)
		{
			Debug.LogWarning(string.Format("Graph {0} is not RecastGraph, it's {1}", graph, this.astarPath.graphs[(int)graph].GetType()), this.astarPath);
			return;
		}
		recastGraph.forcedBoundsCenter = center;
		recastGraph.forcedBoundsSize = new Vector3(size.x, height, size.y);
		Action<RecastMeshGatherer> action = GlobalNavigationManager.CreateCollectMeshesCallback(center, size, !dontUseRecastFloor, navigationHoles);
		if (action != null)
		{
			RecastGraph.CollectionSettings collectionSettings = recastGraph.collectionSettings;
			collectionSettings.onCollectMeshes = (Action<RecastMeshGatherer>)Delegate.Combine(collectionSettings.onCollectMeshes, action);
		}
		try
		{
			this.astarPath.Scan(recastGraph);
		}
		finally
		{
			if (action != null)
			{
				RecastGraph.CollectionSettings collectionSettings2 = recastGraph.collectionSettings;
				collectionSettings2.onCollectMeshes = (Action<RecastMeshGatherer>)Delegate.Remove(collectionSettings2.onCollectMeshes, action);
			}
		}
		int walkableNodesCount = 0;
		recastGraph.GetNodes(delegate(GraphNode node)
		{
			if (node.Walkable)
			{
				int walkableNodesCount2 = walkableNodesCount;
				walkableNodesCount = walkableNodesCount2 + 1;
			}
		});
		Debug.Log(string.Format("[GlobalNavigationManager] InitRecastGraph graph: {0}, scanned: {1}, walkable: {2}, center: {3:F3}, size: {4:F3}", new object[] { graph, recastGraph.isScanned, walkableNodesCount, center, size }));
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x000E5D3C File Offset: 0x000E3F3C
	public IEnumerator InitRecastGraphAsync(LazyConsts.Navigation.Graph graph, Vector3 center, Vector2 size, bool dontUseRecastFloor = false, float height = 1f, IReadOnlyList<WorldZoneNavigationHoleBakedData> navigationHoles = null)
	{
		if (graph == LazyConsts.Navigation.Graph.None)
		{
			yield return null;
		}
		if ((LazyConsts.Navigation.Graph)this.astarPath.graphs.Length < graph + 1)
		{
			Debug.LogWarning(string.Format("Graph {0} not specified in {1}", graph, this.astarPath), this.astarPath);
			yield return null;
		}
		RecastGraph recastGraph = this.astarPath.graphs[(int)graph] as RecastGraph;
		if (recastGraph == null)
		{
			Debug.LogWarning(string.Format("Graph {0} is not RecastGraph, it's {1}", graph, this.astarPath.graphs[(int)graph].GetType()), this.astarPath);
			yield break;
		}
		recastGraph.forcedBoundsCenter = center;
		recastGraph.forcedBoundsSize = new Vector3(size.x, height, size.y);
		Action<RecastMeshGatherer> collectMeshes = GlobalNavigationManager.CreateCollectMeshesCallback(center, size, !dontUseRecastFloor, navigationHoles);
		if (collectMeshes != null)
		{
			RecastGraph.CollectionSettings collectionSettings = recastGraph.collectionSettings;
			collectionSettings.onCollectMeshes = (Action<RecastMeshGatherer>)Delegate.Combine(collectionSettings.onCollectMeshes, collectMeshes);
		}
		try
		{
			foreach (Progress progress in LazySingleton<AStarScanScheduler>.Instance.ScanGraphAsync(recastGraph))
			{
				Debug.Log(string.Format("Scanning Graph {0}: {1}", graph, progress));
				yield return null;
			}
			IEnumerator<Progress> enumerator = null;
		}
		finally
		{
			if (collectMeshes != null)
			{
				RecastGraph.CollectionSettings collectionSettings2 = recastGraph.collectionSettings;
				collectionSettings2.onCollectMeshes = (Action<RecastMeshGatherer>)Delegate.Remove(collectionSettings2.onCollectMeshes, collectMeshes);
			}
		}
		int walkableNodesCount = 0;
		recastGraph.GetNodes(delegate(GraphNode node)
		{
			if (node.Walkable)
			{
				int walkableNodesCount2 = walkableNodesCount;
				walkableNodesCount = walkableNodesCount2 + 1;
			}
		});
		Debug.Log(string.Format("[GlobalNavigationManager] InitRecastGraph graph: {0}, scanned: {1}, walkable: {2}, center: {3:F3}, size: {4:F3}", new object[] { graph, recastGraph.isScanned, walkableNodesCount, center, size }));
		yield break;
		yield break;
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x000E5D78 File Offset: 0x000E3F78
	private static Action<RecastMeshGatherer> CreateCollectMeshesCallback(Vector3 center, Vector2 size, bool addFloor, IReadOnlyList<WorldZoneNavigationHoleBakedData> navigationHoles)
	{
		bool flag = navigationHoles != null && navigationHoles.Count > 0;
		if (!addFloor && !flag)
		{
			return null;
		}
		return delegate(RecastMeshGatherer gatherer)
		{
			GlobalNavigationManager.AddNavigationGeometryToGatherer(gatherer, center, size, addFloor, navigationHoles);
		};
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x000E5DDC File Offset: 0x000E3FDC
	private static void AddNavigationGeometryToGatherer(RecastMeshGatherer gatherer, Vector3 center, Vector2 size, bool addFloor, IReadOnlyList<WorldZoneNavigationHoleBakedData> navigationHoles)
	{
		int num = gatherer.AddMeshBuffers(GlobalNavigationManager.UnitBoxVerts, GlobalNavigationManager.UnitBoxTris);
		if (addFloor)
		{
			GlobalNavigationManager.AddRecastFloorToGatherer(gatherer, num, center, size);
		}
		if (navigationHoles != null && navigationHoles.Count > 0)
		{
			GlobalNavigationManager.AddNavigationHolesToGatherer(gatherer, num, navigationHoles);
		}
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x000E5E20 File Offset: 0x000E4020
	private static void AddRecastFloorToGatherer(RecastMeshGatherer gatherer, int meshDataIndex, Vector3 center, Vector2 size)
	{
		Matrix4x4 matrix4x = Matrix4x4.TRS(center, Quaternion.identity, new Vector3(size.x * 0.5f, 0.05f, size.y * 0.5f));
		RecastMeshGatherer.GatheredMesh gatheredMesh = new RecastMeshGatherer.GatheredMesh
		{
			meshDataIndex = meshDataIndex,
			tagDataIndex = -1,
			area = 0,
			indexStart = 0,
			indexEnd = -1,
			bounds = default(Bounds),
			matrix = matrix4x,
			solid = false,
			doubleSided = true,
			flatten = false,
			areaIsTag = false
		};
		gatheredMesh.RecalculateBounds();
		gatherer.AddMesh(gatheredMesh);
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x000E5ED0 File Offset: 0x000E40D0
	private static void AddNavigationHolesToGatherer(RecastMeshGatherer gatherer, int meshDataIndex, IReadOnlyList<WorldZoneNavigationHoleBakedData> navigationHoles)
	{
		for (int i = 0; i < navigationHoles.Count; i++)
		{
			WorldZoneNavigationHoleBakedData worldZoneNavigationHoleBakedData = navigationHoles[i];
			Matrix4x4 matrix4x = Matrix4x4.TRS(worldZoneNavigationHoleBakedData.center, worldZoneNavigationHoleBakedData.rotation, worldZoneNavigationHoleBakedData.size * 0.5f);
			RecastMeshGatherer.GatheredMesh gatheredMesh = new RecastMeshGatherer.GatheredMesh
			{
				meshDataIndex = meshDataIndex,
				tagDataIndex = -1,
				area = -1,
				indexStart = 0,
				indexEnd = -1,
				bounds = default(Bounds),
				matrix = matrix4x,
				solid = true,
				doubleSided = false,
				flatten = false,
				areaIsTag = false
			};
			gatheredMesh.RecalculateBounds();
			gatherer.AddMesh(gatheredMesh);
		}
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x000E5F94 File Offset: 0x000E4194
	public void AddCutUnit(SGuid holder, LazyConsts.Navigation.Graph graph, Vector3 center, Vector2 size)
	{
		this.AddCutUnit(holder, NavigationGraphMaskUtils.ToCutGraphMask(graph), center, size);
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x000E5FA6 File Offset: 0x000E41A6
	public void AddCutUnit(SGuid holder, LazyConsts.Navigation.Graph graph, Rect rect, float y)
	{
		this.AddCutUnit(holder, graph, new Vector3(rect.center.x, y, rect.center.y), rect.size);
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x000E5FD6 File Offset: 0x000E41D6
	public void AddCutUnit(SGuid holder, Rect rect, float y)
	{
		this.AddCutUnit(holder, GraphMask.everything, new Vector3(rect.center.x, y, rect.center.y), rect.size);
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x000E600C File Offset: 0x000E420C
	public void RemoveCutUnit(SGuid holder)
	{
		bool flag = false;
		GraphUpdateSceneUnit graphUpdateSceneUnit;
		if (this.usedGraphUpdateSceneUnits.TryGetValue(holder, out graphUpdateSceneUnit))
		{
			graphUpdateSceneUnit.Release();
			this.usedGraphUpdateSceneUnits.Remove(holder);
			Pool pool = this.graphUpdateSceneUnitPool;
			if (pool != null)
			{
				pool.ReleaseObject<GraphUpdateSceneUnit>(graphUpdateSceneUnit);
			}
			flag = true;
		}
		GraphCutUnit graphCutUnit;
		if (this.usedUnits.TryGetValue(holder, out graphCutUnit))
		{
			graphCutUnit.Release();
			this.usedUnits.Remove(holder);
			this.obstacleUnitPool.ReleaseObject<GraphCutUnit>(graphCutUnit);
			flag = true;
		}
		GraphCustomNavMeshCutUnit graphCustomNavMeshCutUnit;
		if (this.usedCustomNavMeshCutUnits.TryGetValue(holder, out graphCustomNavMeshCutUnit))
		{
			graphCustomNavMeshCutUnit.Release();
			this.usedCustomNavMeshCutUnits.Remove(holder);
			Pool pool2 = this.customNavMeshCutUnitPool;
			if (pool2 != null)
			{
				pool2.ReleaseObject<GraphCustomNavMeshCutUnit>(graphCustomNavMeshCutUnit);
			}
			flag = true;
		}
		if (!flag)
		{
			Debug.LogWarning(string.Format("Holder {0} not found in active units", holder));
		}
	}

	// Token: 0x06002FF3 RID: 12275 RVA: 0x000E60D0 File Offset: 0x000E42D0
	public void UpdateCutUnit(SGuid holder, Vector3 center, Vector2 size)
	{
		GraphCutUnit graphCutUnit;
		if (this.usedUnits.TryGetValue(holder, out graphCutUnit))
		{
			graphCutUnit.UpdateParameters(center, size);
			return;
		}
		Debug.LogWarning(string.Format("Holder {0} not found in usedUnits", holder));
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x000E6106 File Offset: 0x000E4306
	public void UpdateCutUnit(SGuid holder, Rect rect, float y)
	{
		this.UpdateCutUnit(holder, new Vector3(rect.center.x, y, rect.center.y), rect.size);
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x000E6134 File Offset: 0x000E4334
	public void ClearAll()
	{
		foreach (GraphCutUnit graphCutUnit in new List<GraphCutUnit>(this.usedUnits.Values))
		{
			this.RemoveCutUnit(graphCutUnit.holder);
		}
		foreach (GraphUpdateSceneUnit graphUpdateSceneUnit in new List<GraphUpdateSceneUnit>(this.usedGraphUpdateSceneUnits.Values))
		{
			this.RemoveCutUnit(graphUpdateSceneUnit.holder);
		}
		foreach (GraphCustomNavMeshCutUnit graphCustomNavMeshCutUnit in new List<GraphCustomNavMeshCutUnit>(this.usedCustomNavMeshCutUnits.Values))
		{
			this.RemoveCutUnit(graphCustomNavMeshCutUnit.holder);
		}
	}

	// Token: 0x06002FF6 RID: 12278 RVA: 0x000E623C File Offset: 0x000E443C
	public void CalculatePath(LazyConsts.Navigation.Graph graph, Vector3 start, Vector3 end, Action<Path> onPathComplete)
	{
		if (graph == LazyConsts.Navigation.Graph.None)
		{
			return;
		}
		this.CalculatePath(GraphMask.FromGraphIndex((uint)graph), start, end, onPathComplete);
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x000E6254 File Offset: 0x000E4454
	public void CalculatePath(GraphMask graphMask, Vector3 start, Vector3 end, Action<Path> onPathComplete)
	{
		if (graphMask == default(GraphMask))
		{
			return;
		}
		PathCalculationUnit pathCalculationUnit = this.pathCalculationUnitPool.GetOrCreateObject<PathCalculationUnit>();
		pathCalculationUnit.transform.position = start;
		pathCalculationUnit.seeker.graphMask = graphMask;
		pathCalculationUnit.seeker.StartPath(start, end, delegate(Path p)
		{
			pathCalculationUnit.VectorPath = p.vectorPath;
			this.pathCalculationUnitPool.ReleaseObject<PathCalculationUnit>(pathCalculationUnit);
			Action<Path> onPathComplete2 = onPathComplete;
			if (onPathComplete2 == null)
			{
				return;
			}
			onPathComplete2(p);
		}, graphMask);
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x000E62DC File Offset: 0x000E44DC
	protected override void Awake()
	{
		base.Awake();
		this.astarPath = AstarPath.active;
		this.obstacleUnitPool = new Pool(this.graphCutUnitPrefab, this.graphCutUnitPrefab.transform.parent, 10, Pool.PoolType.ImmediateActivation, false, null);
		if (this.graphCustomNavMeshCutUnitPrefab != null)
		{
			this.customNavMeshCutUnitPool = new Pool(this.graphCustomNavMeshCutUnitPrefab, this.graphCustomNavMeshCutUnitPrefab.transform.parent, 10, Pool.PoolType.ImmediateActivation, false, null);
		}
		if (this.graphUpdateSceneUnitPrefab != null)
		{
			this.graphUpdateSceneUnitPool = new Pool(this.graphUpdateSceneUnitPrefab, this.graphUpdateSceneUnitPrefab.transform.parent, 10, Pool.PoolType.ImmediateActivation, false, null);
		}
		this.recastFloorPool = new Pool(this.recastFloorPrefab, this.recastFloorPrefab.transform.parent, 1, Pool.PoolType.ImmediateActivation, false, null);
		this.pathCalculationUnitPool = new Pool(this.pathCalculationUnitPrefab, this.pathCalculationUnitPrefab.transform.parent, 10, Pool.PoolType.ImmediateActivation, false, null);
		this.graphCutUnitPrefab.gameObject.SetActive(false);
		if (this.graphCustomNavMeshCutUnitPrefab != null)
		{
			this.graphCustomNavMeshCutUnitPrefab.gameObject.SetActive(false);
		}
		if (this.graphUpdateSceneUnitPrefab != null)
		{
			this.graphUpdateSceneUnitPrefab.gameObject.SetActive(false);
		}
		this.recastFloorPrefab.gameObject.SetActive(false);
		this.pathCalculationUnitPrefab.gameObject.SetActive(false);
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x000E6444 File Offset: 0x000E4644
	public void AddCutUnit(SGuid holder, LazyConsts.Navigation.Graph graph, Vector3 center, float radius)
	{
		this.AddCutUnit(holder, NavigationGraphMaskUtils.ToCutGraphMask(graph), center, radius);
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x000E6456 File Offset: 0x000E4656
	public void AddCutUnit(SGuid holder, LazyConsts.Navigation.Graph graph, Vector3 center, WgoPartBakedData.PlannerMeshData plannerMeshData)
	{
		this.AddCutUnit(holder, NavigationGraphMaskUtils.ToCutGraphMask(graph), center, plannerMeshData);
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x000E6468 File Offset: 0x000E4668
	public void AddCutUnit(SGuid holder, Vector3 center, WgoPartBakedData.PlannerMeshData plannerMeshData)
	{
		this.AddCutUnit(holder, GraphMask.everything, center, plannerMeshData);
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x000E6478 File Offset: 0x000E4678
	public void AddGraphSceneUpdateUnit(SGuid holder, LazyConsts.Navigation.Graph graph, Vector3 center, WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData)
	{
		this.AddGraphSceneUpdateUnit(holder, NavigationGraphMaskUtils.ToCutGraphMask(graph), center, graphUpdateSceneBoxData);
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x000E648A File Offset: 0x000E468A
	public void AddGraphSceneUpdateUnit(SGuid holder, Vector3 center, WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData)
	{
		this.AddGraphSceneUpdateUnit(holder, GraphMask.everything, center, graphUpdateSceneBoxData);
	}

	// Token: 0x06002FFE RID: 12286 RVA: 0x000E649C File Offset: 0x000E469C
	public void AddCustomNavMeshCutUnit(SGuid holder, Vector3 center, Vector3 scale, WgoData wgoData)
	{
		if (this.customNavMeshCutUnitPool == null)
		{
			Debug.LogWarning("CustomNavMeshCutUnit pool is not initialized. Assign graphCustomNavMeshCutUnitPrefab in GlobalNavigationManager.");
			return;
		}
		if (!GraphCustomNavMeshCutUnit.HasBakedCustomNavMeshCuts(wgoData))
		{
			return;
		}
		GraphCustomNavMeshCutUnit graphCustomNavMeshCutUnit;
		if (this.usedCustomNavMeshCutUnits.TryGetValue(holder, out graphCustomNavMeshCutUnit))
		{
			graphCustomNavMeshCutUnit.UpdateParameters(center, scale, wgoData);
			return;
		}
		GraphCustomNavMeshCutUnit orCreateObject = this.customNavMeshCutUnitPool.GetOrCreateObject<GraphCustomNavMeshCutUnit>();
		orCreateObject.Assign(holder);
		orCreateObject.UpdateParameters(center, scale, wgoData);
		if (!this.usedCustomNavMeshCutUnits.TryAdd(holder, orCreateObject))
		{
			Debug.LogWarning(string.Format("Holder {0} already exists in usedCustomNavMeshCutUnits", holder));
			this.customNavMeshCutUnitPool.ReleaseObject<GraphCustomNavMeshCutUnit>(orCreateObject);
		}
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x000E652C File Offset: 0x000E472C
	public void UpdateCustomNavMeshCutUnitTransform(SGuid holder, Vector3 center, Vector3 scale)
	{
		GraphCustomNavMeshCutUnit graphCustomNavMeshCutUnit;
		if (this.usedCustomNavMeshCutUnits.TryGetValue(holder, out graphCustomNavMeshCutUnit))
		{
			graphCustomNavMeshCutUnit.UpdateTransform(center, scale);
		}
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x000E6554 File Offset: 0x000E4754
	public void RemoveCustomNavMeshCutUnit(SGuid holder)
	{
		GraphCustomNavMeshCutUnit graphCustomNavMeshCutUnit;
		if (!this.usedCustomNavMeshCutUnits.TryGetValue(holder, out graphCustomNavMeshCutUnit))
		{
			return;
		}
		graphCustomNavMeshCutUnit.Release();
		this.usedCustomNavMeshCutUnits.Remove(holder);
		Pool pool = this.customNavMeshCutUnitPool;
		if (pool == null)
		{
			return;
		}
		pool.ReleaseObject<GraphCustomNavMeshCutUnit>(graphCustomNavMeshCutUnit);
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x000E6598 File Offset: 0x000E4798
	private void AddCutUnit(SGuid holder, GraphMask graphMask, Vector3 center, float radius)
	{
		GraphCutUnit orCreateObject = this.obstacleUnitPool.GetOrCreateObject<GraphCutUnit>();
		orCreateObject.Assign(holder);
		orCreateObject.UpdateParameters(graphMask, center, radius);
		if (!this.usedUnits.TryAdd(holder, orCreateObject))
		{
			Debug.LogWarning(string.Format("Holder {0} already exists in usedUnits", holder));
			this.obstacleUnitPool.ReleaseObject<GraphCutUnit>(orCreateObject);
		}
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x000E65F0 File Offset: 0x000E47F0
	private void AddCutUnit(SGuid holder, GraphMask graphMask, Vector3 center, WgoPartBakedData.PlannerMeshData plannerMeshData)
	{
		GraphCutUnit orCreateObject = this.obstacleUnitPool.GetOrCreateObject<GraphCutUnit>();
		orCreateObject.Assign(holder);
		orCreateObject.UpdateParameters(graphMask, center, plannerMeshData);
		if (!this.usedUnits.TryAdd(holder, orCreateObject))
		{
			Debug.LogWarning(string.Format("Holder {0} already exists in usedUnits", holder));
			this.obstacleUnitPool.ReleaseObject<GraphCutUnit>(orCreateObject);
		}
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x000E6648 File Offset: 0x000E4848
	private void AddGraphSceneUpdateUnit(SGuid holder, GraphMask graphMask, Vector3 center, WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData)
	{
		if (this.graphUpdateSceneUnitPool == null)
		{
			Debug.LogWarning("GraphUpdateSceneUnit pool is not initialized. Assign graphUpdateSceneUnitPrefab in GlobalNavigationManager.");
			return;
		}
		GraphUpdateSceneUnit orCreateObject = this.graphUpdateSceneUnitPool.GetOrCreateObject<GraphUpdateSceneUnit>();
		orCreateObject.Assign(holder);
		orCreateObject.UpdateParameters(graphMask, center, graphUpdateSceneBoxData);
		if (!this.usedGraphUpdateSceneUnits.TryAdd(holder, orCreateObject))
		{
			Debug.LogWarning(string.Format("Holder {0} already exists in usedUnits", holder));
			this.graphUpdateSceneUnitPool.ReleaseObject<GraphUpdateSceneUnit>(orCreateObject);
		}
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x000E66B0 File Offset: 0x000E48B0
	private void AddCutUnit(SGuid holder, GraphMask graphMask, Vector3 center, Vector2 size)
	{
		GraphCutUnit orCreateObject = this.obstacleUnitPool.GetOrCreateObject<GraphCutUnit>();
		orCreateObject.Assign(holder);
		orCreateObject.UpdateParameters(graphMask, center, size);
		if (!this.usedUnits.TryAdd(holder, orCreateObject))
		{
			Debug.LogWarning(string.Format("Holder {0} already exists in usedUnits", holder));
			this.obstacleUnitPool.ReleaseObject<GraphCutUnit>(orCreateObject);
		}
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x000E6708 File Offset: 0x000E4908
	private string GraphMaskToReadableString(GraphMask graphMask)
	{
		AstarPath astarPath = this.astarPath;
		if (((astarPath != null) ? astarPath.graphs : null) == null)
		{
			return graphMask.ToString();
		}
		List<string> list = new List<string>();
		for (int i = 0; i < this.astarPath.graphs.Length; i++)
		{
			NavGraph navGraph = this.astarPath.graphs[i];
			if (navGraph != null && graphMask.Contains((uint)i))
			{
				string text;
				if (!string.IsNullOrEmpty(navGraph.name))
				{
					text = navGraph.name;
				}
				else
				{
					LazyConsts.Navigation.Graph graph = (LazyConsts.Navigation.Graph)i;
					text = graph.ToString();
				}
				string text2 = text;
				list.Add(string.Format("{0}:{1}", i, text2));
			}
		}
		if (list.Count != 0)
		{
			return string.Join(", ", list);
		}
		return "<none>";
	}

	// Token: 0x040026D3 RID: 9939
	private AstarPath astarPath;

	// Token: 0x040026D4 RID: 9940
	[SerializeField]
	private GraphCutUnit graphCutUnitPrefab;

	// Token: 0x040026D5 RID: 9941
	[SerializeField]
	private GraphCustomNavMeshCutUnit graphCustomNavMeshCutUnitPrefab;

	// Token: 0x040026D6 RID: 9942
	[SerializeField]
	private GraphUpdateSceneUnit graphUpdateSceneUnitPrefab;

	// Token: 0x040026D7 RID: 9943
	[SerializeField]
	private RecastFloor recastFloorPrefab;

	// Token: 0x040026D8 RID: 9944
	[SerializeField]
	private PathCalculationUnit pathCalculationUnitPrefab;

	// Token: 0x040026D9 RID: 9945
	private Pool obstacleUnitPool;

	// Token: 0x040026DA RID: 9946
	private Dictionary<SGuid, GraphCutUnit> usedUnits = new Dictionary<SGuid, GraphCutUnit>();

	// Token: 0x040026DB RID: 9947
	private Pool graphUpdateSceneUnitPool;

	// Token: 0x040026DC RID: 9948
	private Dictionary<SGuid, GraphUpdateSceneUnit> usedGraphUpdateSceneUnits = new Dictionary<SGuid, GraphUpdateSceneUnit>();

	// Token: 0x040026DD RID: 9949
	private Pool customNavMeshCutUnitPool;

	// Token: 0x040026DE RID: 9950
	private Dictionary<SGuid, GraphCustomNavMeshCutUnit> usedCustomNavMeshCutUnits = new Dictionary<SGuid, GraphCustomNavMeshCutUnit>();

	// Token: 0x040026DF RID: 9951
	private Pool recastFloorPool;

	// Token: 0x040026E0 RID: 9952
	private Pool pathCalculationUnitPool;

	// Token: 0x040026E1 RID: 9953
	private static readonly Vector3[] UnitBoxVerts = new Vector3[]
	{
		new Vector3(-1f, -1f, -1f),
		new Vector3(1f, -1f, -1f),
		new Vector3(1f, -1f, 1f),
		new Vector3(-1f, -1f, 1f),
		new Vector3(-1f, 1f, -1f),
		new Vector3(1f, 1f, -1f),
		new Vector3(1f, 1f, 1f),
		new Vector3(-1f, 1f, 1f)
	};

	// Token: 0x040026E2 RID: 9954
	private static readonly int[] UnitBoxTris = new int[]
	{
		0, 1, 2, 0, 2, 3, 6, 5, 4, 7,
		6, 4, 0, 5, 1, 0, 4, 5, 1, 6,
		2, 1, 5, 6, 2, 7, 3, 2, 6, 7,
		3, 4, 0, 3, 7, 4
	};

	// Token: 0x040026E3 RID: 9955
	private const float RecastFloorHalfHeight = 0.05f;
}
