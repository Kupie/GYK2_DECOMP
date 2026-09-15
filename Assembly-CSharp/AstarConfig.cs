using System;
using Pathfinding;
using UnityEngine;

// Token: 0x02000A91 RID: 2705
public class AstarConfig : MonoBehaviour
{
	// Token: 0x06004980 RID: 18816 RVA: 0x0015B2EC File Offset: 0x001594EC
	private void CreateGraph()
	{
		if (this.astarPath == null)
		{
			Debug.LogError("Empty AstarPath field");
			return;
		}
		if (string.IsNullOrEmpty(this.defaultGraphId))
		{
			Debug.LogError("Empty DefaultGraphId field");
			return;
		}
		if (this.SearchGraph(this.defaultGraphId) == -1)
		{
			return;
		}
		this.data = this.astarPath.data;
		this.newGraph = this.data.AddGraph(typeof(RecastGraph)) as RecastGraph;
		this.newGraph.name = this.newGraphId;
		this.Copy(this.newGraph, this.data.graphs[this.SearchGraph(this.defaultGraphId)] as RecastGraph);
		Debug.Log(this.newGraphId + " graph created with default parameters");
	}

	// Token: 0x06004981 RID: 18817 RVA: 0x0015B3BC File Offset: 0x001595BC
	private void CopyGraph()
	{
		if (this.astarPath == null)
		{
			Debug.LogError("Empty AstarPath field");
			return;
		}
		this.data = this.astarPath.data;
		if (this.SearchGraph(this.graphToCopyId) != -1)
		{
			this.newGraph = this.data.AddGraph(typeof(RecastGraph)) as RecastGraph;
			this.newGraph.name = this.newGraphId;
			this.Copy(this.newGraph, this.data.graphs[this.SearchGraph(this.graphToCopyId)] as RecastGraph);
			Debug.Log(this.newGraphId + " graph created with parameters of " + this.graphToCopyId);
		}
	}

	// Token: 0x06004982 RID: 18818 RVA: 0x0015B478 File Offset: 0x00159678
	private void CopyParams()
	{
		if (this.astarPath == null)
		{
			Debug.LogError("Empty AstarPath field");
			return;
		}
		this.data = this.astarPath.data;
		if (this.SearchGraph(this.copyToId) != -1 && this.SearchGraph(this.copyFromId) != -1)
		{
			this.Copy(this.data.graphs[this.SearchGraph(this.copyToId)] as RecastGraph, this.data.graphs[this.SearchGraph(this.copyFromId)] as RecastGraph);
			Debug.Log(this.copyToId + " parameters from " + this.copyFromId);
		}
	}

	// Token: 0x06004983 RID: 18819 RVA: 0x0015B528 File Offset: 0x00159728
	private int SearchGraph(string graphId)
	{
		if (this.astarPath == null)
		{
			Debug.LogError("Empty AstarPath field");
			return -1;
		}
		this.data = this.astarPath.data;
		for (int i = 0; i < this.data.graphs.Length; i++)
		{
			if (this.data.graphs[i].name == graphId)
			{
				return i;
			}
		}
		Debug.LogError(graphId + " graph not found");
		return -1;
	}

	// Token: 0x06004984 RID: 18820 RVA: 0x0015B5A8 File Offset: 0x001597A8
	private void Copy(RecastGraph copyTo, RecastGraph copyFrom)
	{
		copyTo.cellSize = copyFrom.cellSize;
		copyTo.useTiles = copyFrom.useTiles;
		copyTo.editorTileSize = copyFrom.editorTileSize;
		copyTo.minRegionSize = copyFrom.minRegionSize;
		copyTo.walkableHeight = copyFrom.walkableHeight;
		copyTo.walkableClimb = copyFrom.walkableClimb;
		copyTo.characterRadius = copyFrom.characterRadius;
		copyTo.maxSlope = copyFrom.maxSlope;
		copyTo.maxEdgeLength = copyFrom.maxEdgeLength;
		copyTo.contourMaxError = copyFrom.contourMaxError;
		copyTo.rasterizeTerrain = copyFrom.rasterizeTerrain;
		copyTo.rasterizeMeshes = copyFrom.rasterizeMeshes;
		copyTo.rasterizeColliders = copyFrom.rasterizeColliders;
		copyTo.colliderRasterizeDetail = copyFrom.colliderRasterizeDetail;
		copyTo.mask = copyFrom.mask;
		copyTo.enableNavmeshCutting = copyFrom.enableNavmeshCutting;
		copyTo.nearestSearchOnlyXZ = copyFrom.nearestSearchOnlyXZ;
		copyTo.initialPenalty = copyFrom.initialPenalty;
	}

	// Token: 0x04003954 RID: 14676
	[SerializeField]
	private AstarPath astarPath;

	// Token: 0x04003955 RID: 14677
	[SerializeField]
	private string defaultGraphId;

	// Token: 0x04003956 RID: 14678
	[Space(20f)]
	[Space(10f)]
	[SerializeField]
	private string newGraphId = string.Empty;

	// Token: 0x04003957 RID: 14679
	[SerializeField]
	private string graphToCopyId = string.Empty;

	// Token: 0x04003958 RID: 14680
	[Space(10f)]
	[Space(10f)]
	[SerializeField]
	private string copyFromId = string.Empty;

	// Token: 0x04003959 RID: 14681
	[SerializeField]
	private string copyToId = string.Empty;

	// Token: 0x0400395A RID: 14682
	private AstarData data;

	// Token: 0x0400395B RID: 14683
	private RecastGraph newGraph;
}
