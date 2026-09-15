using System;
using Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x02000550 RID: 1360
[Serializable]
public class GraphHelper : MonoBehaviour
{
	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x060022EA RID: 8938 RVA: 0x000A3486 File Offset: 0x000A1686
	public static GraphHelper Instance
	{
		get
		{
			if (GraphHelper.instance == null)
			{
				GraphHelper.instance = global::UnityEngine.Object.FindFirstObjectByType<GraphHelper>();
			}
			return GraphHelper.instance;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x060022EB RID: 8939 RVA: 0x000A34A4 File Offset: 0x000A16A4
	public SceneGraphsData SceneGraphsData
	{
		get
		{
			if (this.sceneGraphsData == null)
			{
				this.sceneGraphsData = Addressables.LoadAssetAsync<SceneGraphsData>("AStarGraphs/SceneGraphsData.asset").WaitForCompletion();
			}
			return this.sceneGraphsData;
		}
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000A34E0 File Offset: 0x000A16E0
	public void ScanGDPointGraph()
	{
		GDPointGraph gdpointGraph = (GDPointGraph)AstarPath.active.data.FindGraphOfType(typeof(GDPointGraph));
		if (gdpointGraph == null)
		{
			return;
		}
		AstarPath.active.Scan(gdpointGraph);
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000A351B File Offset: 0x000A171B
	public void RescanGDPointGraph()
	{
		this.PausePathFinding();
		this.ScanGDPointGraph();
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000A3529 File Offset: 0x000A1729
	public void QueueRescanGDPointGraph()
	{
		if (AstarPath.active == null)
		{
			return;
		}
		this.gdPointGraphRescanQueued = true;
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000A3540 File Offset: 0x000A1740
	private void LateUpdate()
	{
		if (!this.gdPointGraphRescanQueued)
		{
			return;
		}
		this.gdPointGraphRescanQueued = false;
		if (AstarPath.active != null)
		{
			this.RescanGDPointGraph();
		}
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000A3565 File Offset: 0x000A1765
	public void PausePathFinding()
	{
		if (!this.graphUpdateLock.Held)
		{
			this.graphUpdateLock = AstarPath.active.PausePathfinding();
		}
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000A3584 File Offset: 0x000A1784
	public void ResumePathFinding()
	{
		if (this.graphUpdateLock.Held)
		{
			this.graphUpdateLock.Release();
		}
	}

	// Token: 0x04001F78 RID: 8056
	private const string GRAPH_PATH = "/AddressableAssets/AStarGraphs/graphReady.bytes";

	// Token: 0x04001F79 RID: 8057
	private const string CONFIG_LOAD_KEY = "AStarGraphs/SceneGraphsData.asset";

	// Token: 0x04001F7A RID: 8058
	private const string ASTAR_PREFAB_PATH = "Assets/Prefabs/Systems/AStar.prefab";

	// Token: 0x04001F7B RID: 8059
	private PathProcessor.GraphUpdateLock graphUpdateLock;

	// Token: 0x04001F7C RID: 8060
	private bool gdPointGraphRescanQueued;

	// Token: 0x04001F7D RID: 8061
	private static GameObject astarPrefab;

	// Token: 0x04001F7E RID: 8062
	private static GraphHelper instance;

	// Token: 0x04001F7F RID: 8063
	private SceneGraphsData sceneGraphsData;
}
