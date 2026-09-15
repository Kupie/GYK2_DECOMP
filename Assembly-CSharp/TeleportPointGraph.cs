using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class TeleportPointGraph
{
	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06002307 RID: 8967 RVA: 0x000A3981 File Offset: 0x000A1B81
	// (set) Token: 0x06002308 RID: 8968 RVA: 0x000A3988 File Offset: 0x000A1B88
	public static TeleportPointGraph Instance { get; private set; }

	// Token: 0x06002309 RID: 8969 RVA: 0x000A3990 File Offset: 0x000A1B90
	public static void Build(WorldData worldData)
	{
		TeleportPointGraph.Instance = new TeleportPointGraph();
		TeleportPointGraph.Instance.BuildInternal(worldData);
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000A39A7 File Offset: 0x000A1BA7
	public bool TryResolveDoor(Vector3 playerPos, Vector3 objectPos, out WgoData doorWgo)
	{
		return this.TryResolveDoor(playerPos, objectPos, null, out doorWgo);
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000A39B4 File Offset: 0x000A1BB4
	public bool TryResolveDoor(Vector3 playerPos, Vector3 objectPos, WgoData targetObject, out WgoData doorWgo)
	{
		doorWgo = null;
		int num2;
		int num3;
		string text;
		int num = this.ResolveDoorIndex(playerPos, objectPos, out num2, out num3, out text);
		if (num < 0)
		{
			return false;
		}
		doorWgo = this.nodeWgos[num];
		return doorWgo != null;
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000A39F0 File Offset: 0x000A1BF0
	private int ResolveDoorIndex(Vector3 playerPos, Vector3 objectPos, out int playerArea, out int objectArea, out string branch)
	{
		playerArea = -1;
		objectArea = -1;
		branch = "empty-graph";
		if (this.positions.Count == 0)
		{
			return -1;
		}
		playerArea = this.FindAreaIndex(playerPos);
		objectArea = this.FindAreaIndex(objectPos);
		if (playerArea >= 0 && playerArea == objectArea)
		{
			branch = "same-indoor-area";
			return -1;
		}
		int num2;
		if (playerArea >= 0)
		{
			int num = ((objectArea >= 0) ? this.BfsIndoorOnlyToArea(playerPos, objectArea, playerArea) : (-1));
			if (num >= 0)
			{
				num2 = num;
				branch = "indoor-connected";
			}
			else
			{
				num2 = this.FindDoorTowardStreet(playerPos, playerArea);
				branch = "indoor-to-street";
			}
		}
		else
		{
			if (objectArea < 0)
			{
				branch = "outdoor-to-outdoor";
				return -1;
			}
			num2 = this.FindOutdoorEntranceForArea(playerPos, objectArea);
			branch = "outdoor-to-object-entrance";
		}
		return num2;
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000A3AAC File Offset: 0x000A1CAC
	private void BuildInternal(WorldData worldData)
	{
		if (worldData == null || GameBalance.Me == null)
		{
			return;
		}
		this.LoadIndoorAreas();
		List<WGODef> dataCollection = GameBalance.Me.GetDataCollection<WGODef>();
		if (dataCollection == null)
		{
			return;
		}
		int num = 0;
		foreach (WGODef wgodef in dataCollection)
		{
			if (wgodef != null && !string.IsNullOrEmpty(wgodef.id) && wgodef.teleportDestinationWgoIds != null && wgodef.teleportDestinationWgoIds.Count != 0)
			{
				WgoData wgoData = TeleportPointGraph.ResolveWgo(worldData, wgodef.id);
				if (wgoData != null)
				{
					int orAddNode = this.GetOrAddNode(wgoData);
					foreach (string text in wgodef.teleportDestinationWgoIds)
					{
						if (!string.IsNullOrEmpty(text) && !(text == wgodef.id))
						{
							WgoData wgoData2 = TeleportPointGraph.ResolveWgo(worldData, text);
							if (wgoData2 != null)
							{
								int orAddNode2 = this.GetOrAddNode(wgoData2);
								if (orAddNode != orAddNode2)
								{
									List<int> list = this.edges[orAddNode];
									if (!list.Contains(orAddNode2))
									{
										list.Add(orAddNode2);
										num++;
									}
								}
							}
						}
					}
				}
			}
		}
		this.BuildPartnerLists();
		this.AssignNodeAreas();
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000A3C18 File Offset: 0x000A1E18
	private void LoadIndoorAreas()
	{
		this.indoorAreas.Clear();
		List<GameSceneConfig> list = ((MainGame.Instance != null) ? MainGame.Instance.gameSceneConfigs : null);
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			GameSceneConfig gameSceneConfig = list[i];
			IReadOnlyList<IndoorAreaData> readOnlyList = ((gameSceneConfig != null) ? gameSceneConfig.IndoorAreas : null);
			if (readOnlyList != null)
			{
				for (int j = 0; j < readOnlyList.Count; j++)
				{
					IndoorAreaData indoorAreaData = readOnlyList[j];
					if (indoorAreaData != null)
					{
						this.indoorAreas.Add(indoorAreaData);
					}
				}
			}
		}
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000A3CB0 File Offset: 0x000A1EB0
	private void AssignNodeAreas()
	{
		for (int i = 0; i < this.positions.Count; i++)
		{
			this.areaIndex[i] = this.FindAreaIndex(this.positions[i]);
		}
		this.CacheOutdoorMouths();
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000A3CF8 File Offset: 0x000A1EF8
	private void CacheOutdoorMouths()
	{
		this.outdoorMouthsByArea.Clear();
		for (int i = 0; i < this.indoorAreas.Count; i++)
		{
			List<int> list = new List<int>();
			this.CollectOutdoorMouths(i, list);
			this.outdoorMouthsByArea.Add(list);
		}
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000A3D40 File Offset: 0x000A1F40
	private void CollectOutdoorMouths(int area, List<int> mouths)
	{
		int count = this.positions.Count;
		bool[] array = new bool[count];
		Queue<int> queue = new Queue<int>();
		for (int i = 0; i < count; i++)
		{
			if (this.areaIndex[i] == area)
			{
				array[i] = true;
				queue.Enqueue(i);
			}
		}
		while (queue.Count > 0)
		{
			int num = queue.Dequeue();
			List<int> list = this.partners[num];
			for (int j = 0; j < list.Count; j++)
			{
				int num2 = list[j];
				if (!array[num2])
				{
					array[num2] = true;
					if (this.areaIndex[num2] < 0)
					{
						mouths.Add(num2);
					}
					else
					{
						queue.Enqueue(num2);
					}
				}
			}
		}
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000A3DFC File Offset: 0x000A1FFC
	private int FindAreaIndex(Vector3 pos)
	{
		int num = -1;
		float num2 = float.PositiveInfinity;
		for (int i = 0; i < this.indoorAreas.Count; i++)
		{
			IndoorAreaData indoorAreaData = this.indoorAreas[i];
			if (indoorAreaData != null && indoorAreaData.ContainsXZ(pos))
			{
				float xzarea = indoorAreaData.GetXZArea();
				if (xzarea < num2)
				{
					num2 = xzarea;
					num = i;
				}
			}
		}
		return num;
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000A3E54 File Offset: 0x000A2054
	private int BfsIndoorOnlyToArea(Vector3 playerPos, int startArea, int targetArea)
	{
		int count = this.positions.Count;
		int[] array = new int[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = -1;
		}
		Queue<int> queue = new Queue<int>();
		for (int j = 0; j < count; j++)
		{
			if (this.areaIndex[j] == startArea)
			{
				array[j] = 0;
				queue.Enqueue(j);
			}
		}
		return this.BfsIndoorOnlyFromQueue(playerPos, targetArea, array, queue);
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000A3EC4 File Offset: 0x000A20C4
	private int FindDoorTowardStreet(Vector3 playerPos, int playerArea)
	{
		int count = this.positions.Count;
		int[] array = new int[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = -1;
		}
		Queue<int> queue = new Queue<int>();
		for (int j = 0; j < count; j++)
		{
			if (this.areaIndex[j] >= 0 && this.HasOutdoorPartner(j))
			{
				array[j] = 0;
				queue.Enqueue(j);
			}
		}
		return this.BfsIndoorOnlyFromQueue(playerPos, playerArea, array, queue);
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000A3F3C File Offset: 0x000A213C
	private int BfsIndoorOnlyFromQueue(Vector3 playerPos, int targetArea, int[] hops, Queue<int> queue)
	{
		int num = -1;
		int num2 = int.MaxValue;
		float num3 = float.PositiveInfinity;
		while (queue.Count > 0)
		{
			int num4 = queue.Dequeue();
			if (this.areaIndex[num4] == targetArea)
			{
				float num5 = TeleportPointGraph.DistXZ(playerPos, this.positions[num4]);
				if (hops[num4] < num2 || (hops[num4] == num2 && num5 < num3))
				{
					num2 = hops[num4];
					num3 = num5;
					num = num4;
				}
			}
			int num6 = hops[num4] + 1;
			List<int> list = this.partners[num4];
			for (int i = 0; i < list.Count; i++)
			{
				int num7 = list[i];
				if (hops[num7] < 0 && this.areaIndex[num7] >= 0)
				{
					hops[num7] = num6;
					queue.Enqueue(num7);
				}
			}
		}
		return num;
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000A400C File Offset: 0x000A220C
	private bool HasOutdoorPartner(int node)
	{
		List<int> list = this.partners[node];
		for (int i = 0; i < list.Count; i++)
		{
			if (this.areaIndex[list[i]] < 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000A404F File Offset: 0x000A224F
	private int FindOutdoorEntranceForArea(Vector3 playerPos, int area)
	{
		if (area < 0 || area >= this.outdoorMouthsByArea.Count)
		{
			return -1;
		}
		return this.FindNearestAmong(playerPos, this.outdoorMouthsByArea[area]);
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x000A4078 File Offset: 0x000A2278
	private int FindNearestAmong(Vector3 pos, List<int> nodes)
	{
		int num = -1;
		float num2 = float.PositiveInfinity;
		if (nodes == null)
		{
			return num;
		}
		for (int i = 0; i < nodes.Count; i++)
		{
			int num3 = nodes[i];
			float num4 = TeleportPointGraph.DistXZ(pos, this.positions[num3]);
			if (num4 < num2)
			{
				num2 = num4;
				num = num3;
			}
		}
		return num;
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000A40CC File Offset: 0x000A22CC
	private int GetOrAddNode(WgoData wgoData)
	{
		string id = wgoData.id;
		int count;
		if (this.indexById.TryGetValue(id, out count))
		{
			return count;
		}
		count = this.positions.Count;
		this.indexById.Add(id, count);
		this.positions.Add(wgoData.GetTeleportPointPosition());
		this.nodeWgos.Add(wgoData);
		this.edges.Add(new List<int>());
		this.partners.Add(new List<int>());
		this.areaIndex.Add(-1);
		return count;
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000A4158 File Offset: 0x000A2358
	private void BuildPartnerLists()
	{
		for (int i = 0; i < this.edges.Count; i++)
		{
			List<int> list = this.edges[i];
			for (int j = 0; j < list.Count; j++)
			{
				int num = list[j];
				this.AddUniquePartner(i, num);
				this.AddUniquePartner(num, i);
			}
		}
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000A41B4 File Offset: 0x000A23B4
	private void AddUniquePartner(int from, int to)
	{
		if (from == to)
		{
			return;
		}
		List<int> list = this.partners[from];
		if (!list.Contains(to))
		{
			list.Add(to);
		}
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000A41E4 File Offset: 0x000A23E4
	private static WgoData ResolveWgo(WorldData worldData, string key)
	{
		WgoData wgoData = worldData.GetWgoData(key);
		if (wgoData != null)
		{
			return wgoData;
		}
		return worldData.GetWgoDataByCustomTag(key);
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000A4208 File Offset: 0x000A2408
	private static float DistXZ(Vector3 a, Vector3 b)
	{
		float num = a.x - b.x;
		float num2 = a.z - b.z;
		return Mathf.Sqrt(num * num + num2 * num2);
	}

	// Token: 0x04001F8F RID: 8079
	private readonly List<Vector3> positions = new List<Vector3>();

	// Token: 0x04001F90 RID: 8080
	private readonly List<WgoData> nodeWgos = new List<WgoData>();

	// Token: 0x04001F91 RID: 8081
	private readonly List<List<int>> edges = new List<List<int>>();

	// Token: 0x04001F92 RID: 8082
	private readonly List<List<int>> partners = new List<List<int>>();

	// Token: 0x04001F93 RID: 8083
	private readonly List<int> areaIndex = new List<int>();

	// Token: 0x04001F94 RID: 8084
	private readonly List<IndoorAreaData> indoorAreas = new List<IndoorAreaData>();

	// Token: 0x04001F95 RID: 8085
	private readonly List<List<int>> outdoorMouthsByArea = new List<List<int>>();

	// Token: 0x04001F96 RID: 8086
	private readonly Dictionary<string, int> indexById = new Dictionary<string, int>();
}
