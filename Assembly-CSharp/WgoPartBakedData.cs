using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020005BE RID: 1470
[Serializable]
public class WgoPartBakedData
{
	// Token: 0x1700063D RID: 1597
	// (get) Token: 0x060026B4 RID: 9908 RVA: 0x000B6309 File Offset: 0x000B4509
	public IReadOnlyList<WgoPartBakedData.WgoPartDockPointsBakedData> PointsList
	{
		get
		{
			return this.pointsList;
		}
	}

	// Token: 0x1700063E RID: 1598
	// (get) Token: 0x060026B5 RID: 9909 RVA: 0x000B6311 File Offset: 0x000B4511
	public GameRes ModuleBuildingTypes
	{
		get
		{
			return this.moduleBuildingTypes;
		}
	}

	// Token: 0x1700063F RID: 1599
	// (get) Token: 0x060026B6 RID: 9910 RVA: 0x000B6319 File Offset: 0x000B4519
	public static WgoPartBakedData Empty
	{
		get
		{
			return new WgoPartBakedData(string.Empty);
		}
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x060026B7 RID: 9911 RVA: 0x000B6325 File Offset: 0x000B4525
	// (set) Token: 0x060026B8 RID: 9912 RVA: 0x000B632D File Offset: 0x000B452D
	public float RadiusSpehereCutter
	{
		get
		{
			return this.radiusSpehereCutter;
		}
		set
		{
			this.radiusSpehereCutter = value;
		}
	}

	// Token: 0x17000641 RID: 1601
	// (get) Token: 0x060026B9 RID: 9913 RVA: 0x000B6336 File Offset: 0x000B4536
	public bool HasChunkBounds
	{
		get
		{
			return this.hasChunkBounds;
		}
	}

	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x060026BA RID: 9914 RVA: 0x000B633E File Offset: 0x000B453E
	public ChunkBoundsPair ChunkBounds
	{
		get
		{
			return this.chunkBounds;
		}
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x060026BB RID: 9915 RVA: 0x000B6348 File Offset: 0x000B4548
	public IReadOnlyDictionary<int, List<WgoPartBakedData.GDPointBakedData>> VariationGDPointsDict
	{
		get
		{
			if (this.variationGDPointsDict == null)
			{
				this.variationGDPointsDict = new Dictionary<int, List<WgoPartBakedData.GDPointBakedData>>();
				if (this.gdPointsBakedData == null)
				{
					this.gdPointsBakedData = new List<WgoPartBakedData.GDPointsVariationBakedData>();
				}
				foreach (WgoPartBakedData.GDPointsVariationBakedData gdpointsVariationBakedData in this.gdPointsBakedData)
				{
					if (((gdpointsVariationBakedData != null) ? gdpointsVariationBakedData.gdPoints : null) != null)
					{
						this.variationGDPointsDict[gdpointsVariationBakedData.hash] = gdpointsVariationBakedData.gdPoints;
					}
				}
			}
			return this.variationGDPointsDict;
		}
	}

	// Token: 0x060026BC RID: 9916 RVA: 0x000B63E8 File Offset: 0x000B45E8
	public void SetChunkBounds(ChunkBoundsPair bounds)
	{
		this.chunkBounds = bounds;
		this.hasChunkBounds = bounds.withShadows.size.sqrMagnitude > 0.0001f || bounds.withoutShadows.size.sqrMagnitude > 0.0001f;
	}

	// Token: 0x060026BD RID: 9917 RVA: 0x000B643C File Offset: 0x000B463C
	public void SetGDPointsBakedData(List<WgoPartBakedData.GDPointBakedData> data, int hash)
	{
		if (this.gdPointsBakedData == null)
		{
			this.gdPointsBakedData = new List<WgoPartBakedData.GDPointsVariationBakedData>();
		}
		this.gdPointsBakedData.RemoveAll((WgoPartBakedData.GDPointsVariationBakedData el) => el.hash == hash);
		if (data != null && data.Count > 0)
		{
			this.gdPointsBakedData.Add(new WgoPartBakedData.GDPointsVariationBakedData
			{
				hash = hash,
				gdPoints = data
			});
		}
		this.variationGDPointsDict = null;
	}

	// Token: 0x17000644 RID: 1604
	// (get) Token: 0x060026BE RID: 9918 RVA: 0x000B64B8 File Offset: 0x000B46B8
	public IReadOnlyDictionary<int, Rect> VariationCollisionBoundsRectDict
	{
		get
		{
			if (this.variationCollisionBoundsRectDict == null)
			{
				this.variationCollisionBoundsRectDict = new Dictionary<int, Rect>();
				foreach (WgoPartBakedData.InternalBoundsData internalBoundsData in this.boundsData)
				{
					this.variationCollisionBoundsRectDict.Add(internalBoundsData.hash, internalBoundsData.rect);
				}
			}
			return this.variationCollisionBoundsRectDict;
		}
	}

	// Token: 0x17000645 RID: 1605
	// (get) Token: 0x060026BF RID: 9919 RVA: 0x000B6534 File Offset: 0x000B4734
	public IReadOnlyDictionary<int, List<DockPointData.Baked>> VariationDockPointsDict
	{
		get
		{
			if (this.variationDockPointsDict == null)
			{
				this.variationDockPointsDict = new Dictionary<int, List<DockPointData.Baked>>();
				foreach (WgoPartBakedData.WgoPartDockPointsBakedData wgoPartDockPointsBakedData in this.pointsList)
				{
					this.variationDockPointsDict.Add(wgoPartDockPointsBakedData.hash, wgoPartDockPointsBakedData.dockPoints);
				}
			}
			return this.variationDockPointsDict;
		}
	}

	// Token: 0x17000646 RID: 1606
	// (get) Token: 0x060026C0 RID: 9920 RVA: 0x000B65B0 File Offset: 0x000B47B0
	public IReadOnlyDictionary<int, WgoPartBakedData.PlannerMeshData> VariationPlannerMeshDict
	{
		get
		{
			if (this.variationPlannerMeshDict != null)
			{
				if (this.variationPlannerMeshDict.Count != 0)
				{
					goto IL_008A;
				}
				List<WgoPartBakedData.PlannerMeshData> list = this.plannerMeshesData;
				if (list == null || list.Count <= 0)
				{
					goto IL_008A;
				}
			}
			this.variationPlannerMeshDict = new Dictionary<int, WgoPartBakedData.PlannerMeshData>();
			if (this.plannerMeshesData == null)
			{
				this.plannerMeshesData = new List<WgoPartBakedData.PlannerMeshData>();
			}
			foreach (WgoPartBakedData.PlannerMeshData plannerMeshData in this.plannerMeshesData)
			{
				if (plannerMeshData != null)
				{
					this.variationPlannerMeshDict[plannerMeshData.hash] = plannerMeshData;
				}
			}
			IL_008A:
			return this.variationPlannerMeshDict;
		}
	}

	// Token: 0x17000647 RID: 1607
	// (get) Token: 0x060026C1 RID: 9921 RVA: 0x000B6660 File Offset: 0x000B4860
	public IReadOnlyDictionary<int, List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData>> VariationCustomNavMeshCutPrefabsDict
	{
		get
		{
			if (this.variationCustomNavMeshCutPrefabsDict == null)
			{
				this.variationCustomNavMeshCutPrefabsDict = new Dictionary<int, List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData>>();
				if (this.customNavMeshCutPrefabsBakedData == null)
				{
					this.customNavMeshCutPrefabsBakedData = new List<WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData>();
				}
				foreach (WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData customNavMeshCutPrefabsVariationBakedData in this.customNavMeshCutPrefabsBakedData)
				{
					if (((customNavMeshCutPrefabsVariationBakedData != null) ? customNavMeshCutPrefabsVariationBakedData.entries : null) != null)
					{
						this.variationCustomNavMeshCutPrefabsDict[customNavMeshCutPrefabsVariationBakedData.hash] = customNavMeshCutPrefabsVariationBakedData.entries;
					}
				}
			}
			return this.variationCustomNavMeshCutPrefabsDict;
		}
	}

	// Token: 0x17000648 RID: 1608
	// (get) Token: 0x060026C2 RID: 9922 RVA: 0x000B6700 File Offset: 0x000B4900
	public IReadOnlyDictionary<int, WgoPartBakedData.GraphUpdateSceneBoxData> VariationGraphUpdateSceneBoxDict
	{
		get
		{
			if (this.variationGraphUpdateSceneBoxDict != null)
			{
				if (this.variationGraphUpdateSceneBoxDict.Count != 0)
				{
					goto IL_008A;
				}
				List<WgoPartBakedData.GraphUpdateSceneBoxData> list = this.graphUpdateSceneBoxesData;
				if (list == null || list.Count <= 0)
				{
					goto IL_008A;
				}
			}
			this.variationGraphUpdateSceneBoxDict = new Dictionary<int, WgoPartBakedData.GraphUpdateSceneBoxData>();
			if (this.graphUpdateSceneBoxesData == null)
			{
				this.graphUpdateSceneBoxesData = new List<WgoPartBakedData.GraphUpdateSceneBoxData>();
			}
			foreach (WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData in this.graphUpdateSceneBoxesData)
			{
				if (graphUpdateSceneBoxData != null)
				{
					this.variationGraphUpdateSceneBoxDict[graphUpdateSceneBoxData.hash] = graphUpdateSceneBoxData;
				}
			}
			IL_008A:
			return this.variationGraphUpdateSceneBoxDict;
		}
	}

	// Token: 0x060026C3 RID: 9923 RVA: 0x000B67B0 File Offset: 0x000B49B0
	public WgoPartBakedData(string id)
	{
		this.id = id;
		this.pointsList = new List<WgoPartBakedData.WgoPartDockPointsBakedData>();
		this.boundsData = new List<WgoPartBakedData.InternalBoundsData>();
		this.plannerMeshesData = new List<WgoPartBakedData.PlannerMeshData>();
		this.graphUpdateSceneBoxesData = new List<WgoPartBakedData.GraphUpdateSceneBoxData>();
		this.variationCollisionBoundsRectDict = new Dictionary<int, Rect>();
		this.variationDockPointsDict = new Dictionary<int, List<DockPointData.Baked>>();
		this.variationPlannerMeshDict = new Dictionary<int, WgoPartBakedData.PlannerMeshData>();
		this.variationGraphUpdateSceneBoxDict = new Dictionary<int, WgoPartBakedData.GraphUpdateSceneBoxData>();
		this.moduleBuildingTypes = new GameRes();
		this.radiusSpehereCutter = -1f;
	}

	// Token: 0x060026C4 RID: 9924 RVA: 0x000B684E File Offset: 0x000B4A4E
	public void SetCollisionBoundsRect(Rect rect, int hash)
	{
		this.boundsData.Add(new WgoPartBakedData.InternalBoundsData
		{
			hash = hash,
			rect = rect
		});
	}

	// Token: 0x060026C5 RID: 9925 RVA: 0x000B686E File Offset: 0x000B4A6E
	public void SetDockPointsData(DockPointData.Baked[] pointDatas, int hash)
	{
		this.pointsList.Add(new WgoPartBakedData.WgoPartDockPointsBakedData
		{
			hash = hash,
			dockPoints = pointDatas.ToList<DockPointData.Baked>()
		});
	}

	// Token: 0x060026C6 RID: 9926 RVA: 0x000B6894 File Offset: 0x000B4A94
	public void SetPlannerMeshData(Vector3[] vertices, int[] triangles, int hash)
	{
		if (vertices == null || triangles == null || vertices.Length == 0 || triangles.Length < 3)
		{
			return;
		}
		if (this.plannerMeshesData == null)
		{
			this.plannerMeshesData = new List<WgoPartBakedData.PlannerMeshData>();
		}
		WgoPartBakedData.PlannerMeshData plannerMeshData = new WgoPartBakedData.PlannerMeshData
		{
			hash = hash,
			vertices = vertices.ToList<Vector3>(),
			triangles = triangles.ToList<int>()
		};
		this.plannerMeshesData.RemoveAll((WgoPartBakedData.PlannerMeshData el) => el.hash == hash);
		this.plannerMeshesData.Add(plannerMeshData);
		this.variationPlannerMeshDict = null;
	}

	// Token: 0x060026C7 RID: 9927 RVA: 0x000B6927 File Offset: 0x000B4B27
	public bool TryGetPlannerMeshData(int hash, out WgoPartBakedData.PlannerMeshData data)
	{
		return this.VariationPlannerMeshDict.TryGetValue(hash, out data);
	}

	// Token: 0x060026C8 RID: 9928 RVA: 0x000B693C File Offset: 0x000B4B3C
	public void SetGraphUpdateSceneBoxData(WgoPartBakedData.GraphUpdateSceneBoxData data, int hash)
	{
		if (data == null || data.size.sqrMagnitude <= 0f)
		{
			return;
		}
		if (this.graphUpdateSceneBoxesData == null)
		{
			this.graphUpdateSceneBoxesData = new List<WgoPartBakedData.GraphUpdateSceneBoxData>();
		}
		data.hash = hash;
		this.graphUpdateSceneBoxesData.RemoveAll((WgoPartBakedData.GraphUpdateSceneBoxData el) => el.hash == hash);
		this.graphUpdateSceneBoxesData.Add(data);
		this.variationGraphUpdateSceneBoxDict = null;
	}

	// Token: 0x060026C9 RID: 9929 RVA: 0x000B69B6 File Offset: 0x000B4BB6
	public bool TryGetGraphUpdateSceneBoxData(int hash, out WgoPartBakedData.GraphUpdateSceneBoxData data)
	{
		if (this.VariationGraphUpdateSceneBoxDict.TryGetValue(hash, out data))
		{
			return true;
		}
		data = null;
		return false;
	}

	// Token: 0x060026CA RID: 9930 RVA: 0x000B69D0 File Offset: 0x000B4BD0
	public void SetCustomNavMeshCutPrefabs(List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData> entries, int hash)
	{
		if (this.customNavMeshCutPrefabsBakedData == null)
		{
			this.customNavMeshCutPrefabsBakedData = new List<WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData>();
		}
		this.customNavMeshCutPrefabsBakedData.RemoveAll((WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData el) => el.hash == hash);
		if (entries != null && entries.Count > 0)
		{
			this.customNavMeshCutPrefabsBakedData.Add(new WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData
			{
				hash = hash,
				entries = entries
			});
		}
		this.variationCustomNavMeshCutPrefabsDict = null;
	}

	// Token: 0x060026CB RID: 9931 RVA: 0x000B6A4B File Offset: 0x000B4C4B
	public bool TryGetCustomNavMeshCutPrefabs(int hash, out List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData> entries)
	{
		if (this.VariationCustomNavMeshCutPrefabsDict.TryGetValue(hash, out entries))
		{
			return true;
		}
		entries = null;
		return false;
	}

	// Token: 0x060026CC RID: 9932 RVA: 0x000B6A64 File Offset: 0x000B4C64
	public static bool TryCreateMesh(WgoPartBakedData.PlannerMeshData data, string meshName, out Mesh mesh)
	{
		mesh = null;
		if (data == null || data.vertices == null || data.triangles == null || data.vertices.Count == 0 || data.triangles.Count < 3)
		{
			return false;
		}
		mesh = new Mesh
		{
			name = meshName
		};
		mesh.SetVertices(data.vertices);
		mesh.SetTriangles(data.triangles, 0);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		return true;
	}

	// Token: 0x060026CD RID: 9933 RVA: 0x000B6ADC File Offset: 0x000B4CDC
	public void SetModuleBuildingTypes(GameRes moduleBuildingTypes)
	{
		this.moduleBuildingTypes = moduleBuildingTypes;
	}

	// Token: 0x060026CE RID: 9934 RVA: 0x000B6AE5 File Offset: 0x000B4CE5
	public WgoPartBakedData.WgoPartDockPointsBakedData GetDockPointsData(int idx)
	{
		return this.pointsList[idx];
	}

	// Token: 0x060026CF RID: 9935 RVA: 0x000B6AF3 File Offset: 0x000B4CF3
	public Rect GetCollisionBoundsRect(int idx)
	{
		return this.boundsData[idx].rect;
	}

	// Token: 0x0400213D RID: 8509
	public string id;

	// Token: 0x0400213E RID: 8510
	[SerializeField]
	public string prefabGuid;

	// Token: 0x0400213F RID: 8511
	[SerializeField]
	private List<WgoPartBakedData.WgoPartDockPointsBakedData> pointsList;

	// Token: 0x04002140 RID: 8512
	[SerializeField]
	private List<WgoPartBakedData.InternalBoundsData> boundsData;

	// Token: 0x04002141 RID: 8513
	[OdinSerialize]
	private List<WgoPartBakedData.PlannerMeshData> plannerMeshesData;

	// Token: 0x04002142 RID: 8514
	[OdinSerialize]
	private List<WgoPartBakedData.GraphUpdateSceneBoxData> graphUpdateSceneBoxesData;

	// Token: 0x04002143 RID: 8515
	[OdinSerialize]
	private List<WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData> customNavMeshCutPrefabsBakedData = new List<WgoPartBakedData.CustomNavMeshCutPrefabsVariationBakedData>();

	// Token: 0x04002144 RID: 8516
	[SerializeField]
	private GameRes moduleBuildingTypes;

	// Token: 0x04002145 RID: 8517
	[SerializeField]
	private float radiusSpehereCutter;

	// Token: 0x04002146 RID: 8518
	[SerializeField]
	private ChunkBoundsPair chunkBounds;

	// Token: 0x04002147 RID: 8519
	[SerializeField]
	private bool hasChunkBounds;

	// Token: 0x04002148 RID: 8520
	[SerializeField]
	private List<WgoPartBakedData.GDPointsVariationBakedData> gdPointsBakedData = new List<WgoPartBakedData.GDPointsVariationBakedData>();

	// Token: 0x04002149 RID: 8521
	private Dictionary<int, Rect> variationCollisionBoundsRectDict;

	// Token: 0x0400214A RID: 8522
	private Dictionary<int, List<DockPointData.Baked>> variationDockPointsDict;

	// Token: 0x0400214B RID: 8523
	private Dictionary<int, WgoPartBakedData.PlannerMeshData> variationPlannerMeshDict;

	// Token: 0x0400214C RID: 8524
	private Dictionary<int, WgoPartBakedData.GraphUpdateSceneBoxData> variationGraphUpdateSceneBoxDict;

	// Token: 0x0400214D RID: 8525
	private Dictionary<int, List<WgoPartBakedData.GDPointBakedData>> variationGDPointsDict;

	// Token: 0x0400214E RID: 8526
	private Dictionary<int, List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData>> variationCustomNavMeshCutPrefabsDict;

	// Token: 0x020005BF RID: 1471
	[Serializable]
	public class InternalBoundsData
	{
		// Token: 0x0400214F RID: 8527
		public int hash;

		// Token: 0x04002150 RID: 8528
		public Rect rect;
	}

	// Token: 0x020005C0 RID: 1472
	[Serializable]
	public class WgoPartDockPointsBakedData
	{
		// Token: 0x04002151 RID: 8529
		public int hash;

		// Token: 0x04002152 RID: 8530
		public List<DockPointData.Baked> dockPoints;
	}

	// Token: 0x020005C1 RID: 1473
	[Serializable]
	public class PlannerMeshData
	{
		// Token: 0x04002153 RID: 8531
		[OdinSerialize]
		public int hash;

		// Token: 0x04002154 RID: 8532
		[OdinSerialize]
		public List<Vector3> vertices = new List<Vector3>();

		// Token: 0x04002155 RID: 8533
		[OdinSerialize]
		public List<int> triangles = new List<int>();
	}

	// Token: 0x020005C2 RID: 1474
	[Serializable]
	public class GraphUpdateSceneBoxData
	{
		// Token: 0x060026D3 RID: 9939 RVA: 0x000B6B24 File Offset: 0x000B4D24
		public void SetPenaltyDelta(int value)
		{
			this.penaltyDelta = value;
		}

		// Token: 0x04002156 RID: 8534
		[OdinSerialize]
		public int hash;

		// Token: 0x04002157 RID: 8535
		[OdinSerialize]
		public Vector3 localCenter;

		// Token: 0x04002158 RID: 8536
		[OdinSerialize]
		public Vector3 size = Vector3.one;

		// Token: 0x04002159 RID: 8537
		[OdinSerialize]
		public bool setWalkability;

		// Token: 0x0400215A RID: 8538
		[OdinSerialize]
		public bool updatePhysics;

		// Token: 0x0400215B RID: 8539
		[OdinSerialize]
		public int penaltyDelta;
	}

	// Token: 0x020005C3 RID: 1475
	[Serializable]
	public class GDPointsVariationBakedData
	{
		// Token: 0x0400215C RID: 8540
		public int hash;

		// Token: 0x0400215D RID: 8541
		public List<WgoPartBakedData.GDPointBakedData> gdPoints = new List<WgoPartBakedData.GDPointBakedData>();
	}

	// Token: 0x020005C4 RID: 1476
	[Serializable]
	public class GDPointBakedData
	{
		// Token: 0x0400215E RID: 8542
		public string id;

		// Token: 0x0400215F RID: 8543
		public string customTag;

		// Token: 0x04002160 RID: 8544
		public Direction direction;

		// Token: 0x04002161 RID: 8545
		public Vector3 localPosition;

		// Token: 0x04002162 RID: 8546
		public bool isTransitPoint;

		// Token: 0x04002163 RID: 8547
		public string transitToGdPointId;

		// Token: 0x04002164 RID: 8548
		public string worldIdToTransit;

		// Token: 0x04002165 RID: 8549
		public bool enabled;

		// Token: 0x04002166 RID: 8550
		public List<string> nextGdPointIds = new List<string>();
	}

	// Token: 0x020005C5 RID: 1477
	[Serializable]
	public class CustomNavMeshCutPrefabsVariationBakedData
	{
		// Token: 0x04002167 RID: 8551
		public int hash;

		// Token: 0x04002168 RID: 8552
		public List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData> entries = new List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData>();
	}

	// Token: 0x020005C6 RID: 1478
	[Serializable]
	public class CustomNavMeshCutPrefabEntryBakedData
	{
		// Token: 0x04002169 RID: 8553
		public AssetReferenceGameObject prefabRef;

		// Token: 0x0400216A RID: 8554
		public Vector3 localPosition;

		// Token: 0x0400216B RID: 8555
		public Quaternion localRotation = Quaternion.identity;

		// Token: 0x0400216C RID: 8556
		public Vector3 localScale = Vector3.one;
	}
}
