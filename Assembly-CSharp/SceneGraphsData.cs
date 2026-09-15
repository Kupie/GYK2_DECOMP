using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000551 RID: 1361
[CreateAssetMenu(fileName = "SceneGraphsData", menuName = "ScriptableObjects/Create scene graphs data object", order = 1)]
public class SceneGraphsData : ScriptableObject
{
	// Token: 0x170005AF RID: 1455
	// (get) Token: 0x060022F3 RID: 8947 RVA: 0x000A359E File Offset: 0x000A179E
	public int GDPointGraphIndex
	{
		get
		{
			if (this.gdPointGraphIndex == -1)
			{
				this.gdPointGraphIndex = (int)AstarPath.active.data.FindGraphOfType(typeof(GDPointGraph)).graphIndex;
			}
			return this.gdPointGraphIndex;
		}
	}

	// Token: 0x170005B0 RID: 1456
	// (get) Token: 0x060022F4 RID: 8948 RVA: 0x000A35D3 File Offset: 0x000A17D3
	public int GDPointGraphMask
	{
		get
		{
			if (this.gdPointGraphMask == -1)
			{
				this.gdPointGraphMask = (int)Mathf.Pow(2f, (float)this.GDPointGraphIndex);
			}
			return this.gdPointGraphMask;
		}
	}

	// Token: 0x170005B1 RID: 1457
	// (get) Token: 0x060022F5 RID: 8949 RVA: 0x000A35FC File Offset: 0x000A17FC
	public IReadOnlyList<string> ConfiguredSceneNames
	{
		get
		{
			List<string> list = new List<string>(this.graphsIdsData.Count);
			foreach (SceneGraphsData.SceneGraphData sceneGraphData in this.graphsIdsData)
			{
				if (!string.IsNullOrEmpty(sceneGraphData.sceneName))
				{
					list.Add(sceneGraphData.sceneName);
				}
			}
			return list;
		}
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000A3674 File Offset: 0x000A1874
	public Dictionary<int, List<string>> GetBakeGroupsByGraphIndex()
	{
		Dictionary<int, List<string>> dictionary = new Dictionary<int, List<string>>();
		foreach (SceneGraphsData.SceneGraphData sceneGraphData in this.graphsIdsData)
		{
			if (!string.IsNullOrEmpty(sceneGraphData.sceneName))
			{
				foreach (SceneGraphsData.GraphData graphData in sceneGraphData.recastGraphs)
				{
					List<string> list;
					if (!dictionary.TryGetValue(graphData.index, out list))
					{
						list = new List<string>();
						dictionary[graphData.index] = list;
					}
					if (!list.Contains(sceneGraphData.sceneName))
					{
						list.Add(sceneGraphData.sceneName);
					}
				}
			}
		}
		return dictionary;
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000A375C File Offset: 0x000A195C
	public List<int> GetRecastGraphIndexesBySceneName(string sceneName)
	{
		SceneGraphsData.SceneGraphData sceneGraphData = this.graphsIdsData.Find((SceneGraphsData.SceneGraphData x) => x.sceneName == sceneName);
		if (sceneGraphData == null)
		{
			Debug.LogError("[RecastGraphIndex]Cant get graph data. Wrong sceneName [" + sceneName + "]");
			return new List<int>();
		}
		return sceneGraphData.recastGraphs.ConvertAll<int>((SceneGraphsData.GraphData x) => x.index);
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000A37DC File Offset: 0x000A19DC
	public List<int> GetRecastGraphIndexByWorldId(string worldId)
	{
		SceneGraphsData.SceneGraphData sceneGraphData = this.graphsIdsData.Find((SceneGraphsData.SceneGraphData x) => x.worldId == worldId);
		if (sceneGraphData == null)
		{
			Debug.LogError("[RecastGraphIndex]Cant get graph data. Wrong worldId [" + worldId + "]");
			return new List<int>();
		}
		return sceneGraphData.recastGraphs.ConvertAll<int>((SceneGraphsData.GraphData x) => x.index);
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000A385C File Offset: 0x000A1A5C
	public int GetRecastGraphMaskByWorldId(string worldId)
	{
		SceneGraphsData.SceneGraphData sceneGraphData = this.graphsIdsData.Find((SceneGraphsData.SceneGraphData x) => x.worldId == worldId);
		if (sceneGraphData == null)
		{
			Debug.LogError("[RecastGraphMask]Cant get graph data. Wrong worldId [" + worldId + "]");
			return 0;
		}
		int num = 0;
		foreach (SceneGraphsData.GraphData graphData in sceneGraphData.recastGraphs)
		{
			num |= 1 << graphData.index;
		}
		return num;
	}

	// Token: 0x04001F80 RID: 8064
	[SerializeField]
	private List<SceneGraphsData.SceneGraphData> graphsIdsData = new List<SceneGraphsData.SceneGraphData>();

	// Token: 0x04001F81 RID: 8065
	private int gdPointGraphIndex = -1;

	// Token: 0x04001F82 RID: 8066
	private int gdPointGraphMask = -1;

	// Token: 0x02000552 RID: 1362
	[Serializable]
	private class SceneGraphData
	{
		// Token: 0x04001F83 RID: 8067
		public string sceneName;

		// Token: 0x04001F84 RID: 8068
		public string worldId;

		// Token: 0x04001F85 RID: 8069
		public List<SceneGraphsData.GraphData> recastGraphs = new List<SceneGraphsData.GraphData>();
	}

	// Token: 0x02000553 RID: 1363
	[Serializable]
	private class GraphData
	{
		// Token: 0x04001F86 RID: 8070
		public string name;

		// Token: 0x04001F87 RID: 8071
		public int index;
	}
}
