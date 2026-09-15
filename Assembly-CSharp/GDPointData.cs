using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x0200058F RID: 1423
[Serializable]
public class GDPointData
{
	// Token: 0x1400006C RID: 108
	// (add) Token: 0x0600248A RID: 9354 RVA: 0x000ABA7C File Offset: 0x000A9C7C
	// (remove) Token: 0x0600248B RID: 9355 RVA: 0x000ABAB4 File Offset: 0x000A9CB4
	public event Action<bool> OnActiveStateChanged;

	// Token: 0x170005E8 RID: 1512
	// (get) Token: 0x0600248C RID: 9356 RVA: 0x000ABAE9 File Offset: 0x000A9CE9
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x0600248D RID: 9357 RVA: 0x000ABAF1 File Offset: 0x000A9CF1
	public int InstanceId
	{
		get
		{
			return this.instanceId;
		}
	}

	// Token: 0x170005EA RID: 1514
	// (get) Token: 0x0600248E RID: 9358 RVA: 0x000ABAF9 File Offset: 0x000A9CF9
	public string CustomTag
	{
		get
		{
			return this.customTag;
		}
	}

	// Token: 0x170005EB RID: 1515
	// (get) Token: 0x0600248F RID: 9359 RVA: 0x000ABB01 File Offset: 0x000A9D01
	public Vector3 Position
	{
		get
		{
			return this.position;
		}
	}

	// Token: 0x170005EC RID: 1516
	// (get) Token: 0x06002490 RID: 9360 RVA: 0x000ABB09 File Offset: 0x000A9D09
	public Direction Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x170005ED RID: 1517
	// (get) Token: 0x06002491 RID: 9361 RVA: 0x000ABB11 File Offset: 0x000A9D11
	public bool IsWaypoint
	{
		get
		{
			return this.isWaypoint;
		}
	}

	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x06002492 RID: 9362 RVA: 0x000ABB19 File Offset: 0x000A9D19
	public string GameSceneDataIdToTransit
	{
		get
		{
			return this.worldIdToTransit;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x06002493 RID: 9363 RVA: 0x000ABB21 File Offset: 0x000A9D21
	public static AstarPath AStarPath
	{
		get
		{
			if (GDPointData.astarPath == null)
			{
				GDPointData.astarPath = AstarPath.active;
			}
			return GDPointData.astarPath;
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x06002494 RID: 9364 RVA: 0x000ABB3F File Offset: 0x000A9D3F
	public GDPointNode Node
	{
		get
		{
			return this.node;
		}
	}

	// Token: 0x170005F1 RID: 1521
	// (get) Token: 0x06002495 RID: 9365 RVA: 0x000ABB47 File Offset: 0x000A9D47
	// (set) Token: 0x06002496 RID: 9366 RVA: 0x000ABB4F File Offset: 0x000A9D4F
	public bool IsGraphPoint
	{
		get
		{
			return this.isGraphPoint;
		}
		set
		{
			this.isGraphPoint = value;
		}
	}

	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06002497 RID: 9367 RVA: 0x000ABB58 File Offset: 0x000A9D58
	public string GameSceneDataId
	{
		get
		{
			return this.gameSceneDataId;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06002498 RID: 9368 RVA: 0x000ABB60 File Offset: 0x000A9D60
	public List<GDPointData> NextPointData
	{
		get
		{
			return this.nextGdPointsData;
		}
	}

	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x06002499 RID: 9369 RVA: 0x000ABB68 File Offset: 0x000A9D68
	public IReadOnlyList<int> NextNodeInstanceIds
	{
		get
		{
			return this.nextNodesInstanceId;
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x0600249A RID: 9370 RVA: 0x000ABB70 File Offset: 0x000A9D70
	public bool IsTransitPoint
	{
		get
		{
			return this.isTransitPoint;
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x0600249B RID: 9371 RVA: 0x000ABB78 File Offset: 0x000A9D78
	public string TransitToGdPointId
	{
		get
		{
			return this.transitToGdPointId;
		}
	}

	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x0600249C RID: 9372 RVA: 0x000ABB80 File Offset: 0x000A9D80
	// (set) Token: 0x0600249D RID: 9373 RVA: 0x000ABB88 File Offset: 0x000A9D88
	public bool Enabled
	{
		get
		{
			return this.enabled;
		}
		set
		{
			this.enabled = value;
			if (this.isWaypoint)
			{
				MainGame.Instance.GraphHelper.RescanGDPointGraph();
			}
			if (this.OnActiveStateChanged == null)
			{
				Debug.LogWarning("GDPoint [" + this.id + "] has no subscribers for OnActiveStateChanged. Scene may not be loaded yet.");
			}
			Action<bool> onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(this.enabled);
			}
			Debug.Log(string.Format("GDPoint [{0}] Changed Enabled [{1}]", this.id, this.enabled));
		}
	}

	// Token: 0x0600249E RID: 9374 RVA: 0x000ABC0C File Offset: 0x000A9E0C
	public GDPointData(GDPoint gdPoint, string gameSceneDataId, Vector3 offset, bool isWaypoint = true)
	{
		this.id = gdPoint.Id;
		this.instanceId = gdPoint.GetInstanceID();
		this.customTag = gdPoint.CustomTag;
		this.direction = gdPoint.Direction;
		this.position = gdPoint.transform.position + offset;
		this.isTransitPoint = gdPoint.IsTransitPoint;
		this.transitToGdPointId = gdPoint.TransitToGdPointId;
		this.isWaypoint = isWaypoint;
		this.enabled = gdPoint.gameObject.activeSelf;
		if (this.isTransitPoint)
		{
			this.worldIdToTransit = gdPoint.WorldIdToTransit;
			this.transitToGdPointId = gdPoint.TransitToGdPointId;
		}
		for (int i = 0; i < gdPoint.NextGdPoints.Count; i++)
		{
			GDPoint gdpoint = gdPoint.NextGdPoints[i];
			if (gdpoint == null)
			{
				Debug.LogError("GD point with id " + gdPoint.Id + " has an empty next point");
			}
			else
			{
				this.nextNodesInstanceId.Add(gdpoint.GetInstanceID());
			}
		}
		this.gameSceneDataId = gameSceneDataId;
	}

	// Token: 0x0600249F RID: 9375 RVA: 0x000ABD30 File Offset: 0x000A9F30
	public GDPointData(WgoPartBakedData.GDPointBakedData bakedData, string gameSceneDataId, Vector3 worldPosition, Vector3 sceneOffset, int syntheticInstanceId)
	{
		this.id = bakedData.id;
		this.instanceId = syntheticInstanceId;
		this.customTag = bakedData.customTag;
		this.direction = bakedData.direction;
		this.position = worldPosition + bakedData.localPosition + sceneOffset;
		this.isTransitPoint = bakedData.isTransitPoint;
		this.transitToGdPointId = bakedData.transitToGdPointId;
		this.worldIdToTransit = bakedData.worldIdToTransit;
		this.isWaypoint = false;
		this.enabled = bakedData.enabled;
		this.gameSceneDataId = gameSceneDataId;
	}

	// Token: 0x060024A0 RID: 9376 RVA: 0x000ABDDC File Offset: 0x000A9FDC
	public GDPointData(GDPointData otherData)
	{
		this.id = otherData.Id;
		this.instanceId = otherData.InstanceId;
		this.customTag = otherData.CustomTag;
		this.direction = otherData.Direction;
		this.position = otherData.Position;
		this.isTransitPoint = otherData.IsTransitPoint;
		this.transitToGdPointId = otherData.TransitToGdPointId;
		this.isWaypoint = otherData.isWaypoint;
		this.enabled = otherData.Enabled;
		if (this.isTransitPoint)
		{
			this.worldIdToTransit = otherData.worldIdToTransit;
			this.transitToGdPointId = otherData.TransitToGdPointId;
		}
		for (int i = 0; i < otherData.nextNodesInstanceId.Count; i++)
		{
			int num = otherData.nextNodesInstanceId[i];
			this.nextNodesInstanceId.Add(num);
		}
		this.gameSceneDataId = otherData.GameSceneDataId;
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x000ABECC File Offset: 0x000AA0CC
	public GDPointData(string id, string customTag, int instanceId, Vector3 position, Direction direction, string gameSceneDataId, bool isWaypoint, bool enabled)
	{
		this.id = id;
		this.customTag = customTag;
		this.instanceId = instanceId;
		this.position = position;
		this.direction = direction;
		this.gameSceneDataId = gameSceneDataId;
		this.isWaypoint = isWaypoint;
		this.enabled = enabled;
		this.nextNodesInstanceId = new List<int>();
		this.nextGdPointsData = new List<GDPointData>();
	}

	// Token: 0x060024A2 RID: 9378 RVA: 0x000ABF48 File Offset: 0x000AA148
	public void SetEnabledStateSilent(bool enabled)
	{
		this.enabled = enabled;
	}

	// Token: 0x060024A3 RID: 9379 RVA: 0x000ABF51 File Offset: 0x000AA151
	public void SetNextNodeInstanceIds(List<int> ids)
	{
		this.nextNodesInstanceId = ids ?? new List<int>();
		this.nextGdPointsData = new List<GDPointData>();
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x000ABF70 File Offset: 0x000AA170
	public void AddNextNodeInstanceId(int nextInstanceId)
	{
		if (this.nextNodesInstanceId == null)
		{
			this.nextNodesInstanceId = new List<int>();
		}
		if (this.nextNodesInstanceId.Contains(nextInstanceId))
		{
			return;
		}
		this.nextNodesInstanceId.Add(nextInstanceId);
		MainGame instance = MainGame.Instance;
		GDPointData gdpointData;
		if (instance == null)
		{
			gdpointData = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			if (gameSave == null)
			{
				gdpointData = null;
			}
			else
			{
				WorldData worldData = gameSave.worldData;
				if (worldData == null)
				{
					gdpointData = null;
				}
				else
				{
					GdPointsData gdPointsData = worldData.gdPointsData;
					gdpointData = ((gdPointsData != null) ? gdPointsData.GetGDPointDataByInstanceId(nextInstanceId) : null);
				}
			}
		}
		GDPointData gdpointData2 = gdpointData;
		if (gdpointData2 == null)
		{
			return;
		}
		if (this.nextGdPointsData == null)
		{
			this.nextGdPointsData = new List<GDPointData>();
		}
		if (!this.nextGdPointsData.Contains(gdpointData2))
		{
			this.nextGdPointsData.Add(gdpointData2);
		}
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x000AC014 File Offset: 0x000AA214
	public void RemoveNextNodeInstanceId(int nextInstanceId)
	{
		List<int> list = this.nextNodesInstanceId;
		if (list != null)
		{
			list.Remove(nextInstanceId);
		}
		List<GDPointData> list2 = this.nextGdPointsData;
		if (list2 == null)
		{
			return;
		}
		list2.RemoveAll((GDPointData point) => point.InstanceId == nextInstanceId);
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x000AC064 File Offset: 0x000AA264
	public void LinkNextGdPointsData()
	{
		this.nextGdPointsData = new List<GDPointData>();
		for (int i = 0; i < this.nextNodesInstanceId.Count; i++)
		{
			int num = this.nextNodesInstanceId[i];
			GDPointData gdpointDataByInstanceId = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataByInstanceId(num);
			if (gdpointDataByInstanceId != null)
			{
				this.nextGdPointsData.Add(gdpointDataByInstanceId);
			}
		}
		if (this.isTransitPoint)
		{
			GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.transitToGdPointId);
			if (gdpointDataById == null)
			{
				return;
			}
			this.nextGdPointsData.Add(gdpointDataById);
		}
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x000AC104 File Offset: 0x000AA304
	public void InitNode()
	{
		if (this.nodeInited)
		{
			return;
		}
		this.nodeInited = true;
		this.node = new GDPointNode(GDPointData.AStarPath, this);
		this.node.GraphIndex = 2U;
		this.node.position = (Int3)this.position;
		if (this.nextGdPointsData.Count == 0)
		{
			return;
		}
		foreach (GDPointData gdpointData in this.nextGdPointsData)
		{
			if (gdpointData.IsGraphPoint)
			{
				gdpointData.InitNode();
				this.node.AddConnection(gdpointData.node, (uint)Mathf.CeilToInt(this.Distance(gdpointData)));
			}
		}
	}

	// Token: 0x060024A8 RID: 9384 RVA: 0x000AC1CC File Offset: 0x000AA3CC
	public void DeInitNode()
	{
		this.nodeInited = false;
		this.node = null;
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x000AC1DC File Offset: 0x000AA3DC
	public void TryCreateOutsideConnections()
	{
		if (this.isTransitPoint && !string.IsNullOrEmpty(this.transitToGdPointId))
		{
			GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById(this.transitToGdPointId);
			if (gdpointDataById == null)
			{
				return;
			}
			this.node.AddConnection(gdpointDataById.Node, (uint)Mathf.CeilToInt(this.Distance(gdpointDataById)));
		}
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x000AC23F File Offset: 0x000AA43F
	public static float Distance(GDPointData from, GDPointData to)
	{
		if (from == null || to == null)
		{
			return 0f;
		}
		return Vector3.Distance(from.position, to.position);
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x000AC25E File Offset: 0x000AA45E
	public float Distance(GDPointData to)
	{
		return GDPointData.Distance(this, to);
	}

	// Token: 0x04002055 RID: 8277
	[SerializeField]
	private string id;

	// Token: 0x04002056 RID: 8278
	[SerializeField]
	private string customTag;

	// Token: 0x04002057 RID: 8279
	[SerializeField]
	private int instanceId;

	// Token: 0x04002058 RID: 8280
	[SerializeField]
	private List<int> nextNodesInstanceId = new List<int>();

	// Token: 0x04002059 RID: 8281
	[NonSerialized]
	private List<GDPointData> nextGdPointsData = new List<GDPointData>();

	// Token: 0x0400205A RID: 8282
	[SerializeField]
	private Vector3 position;

	// Token: 0x0400205B RID: 8283
	[SerializeField]
	private Direction direction;

	// Token: 0x0400205C RID: 8284
	[SerializeField]
	private bool isTransitPoint;

	// Token: 0x0400205D RID: 8285
	[SerializeField]
	private string transitToGdPointId;

	// Token: 0x0400205E RID: 8286
	[SerializeField]
	private string worldIdToTransit;

	// Token: 0x0400205F RID: 8287
	[SerializeField]
	private string gameSceneDataId;

	// Token: 0x04002060 RID: 8288
	[SerializeField]
	[HideInInspector]
	private bool enabled;

	// Token: 0x04002061 RID: 8289
	[SerializeField]
	[HideInInspector]
	private bool isWaypoint;

	// Token: 0x04002062 RID: 8290
	private GDPointNode node;

	// Token: 0x04002063 RID: 8291
	private bool nodeInited;

	// Token: 0x04002064 RID: 8292
	private static AstarPath astarPath;

	// Token: 0x04002065 RID: 8293
	private bool isGraphPoint;
}
