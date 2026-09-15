using System;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

// Token: 0x0200054C RID: 1356
[JsonOptIn]
[Preserve]
public class GDPointGraph : PointGraph, IUpdatableGraph
{
	// Token: 0x060022DA RID: 8922 RVA: 0x000A31CC File Offset: 0x000A13CC
	protected override IGraphUpdatePromise ScanInternal(bool async)
	{
		return new GDPointGraph.GDPointGraphScanPromise(this);
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000A31D4 File Offset: 0x000A13D4
	public override void GetNodes(Action<GraphNode> action)
	{
		if (this.nodes == null)
		{
			return;
		}
		for (int i = 0; i < base.nodeCount; i++)
		{
			action(this.nodes[i]);
		}
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000A3209 File Offset: 0x000A1409
	protected override void DestroyAllNodes()
	{
		base.DestroyAllNodes();
		base.nodeCount = 0;
	}

	// Token: 0x0200054D RID: 1357
	private class GDPointGraphScanPromise : IGraphUpdatePromise
	{
		// Token: 0x060022DE RID: 8926 RVA: 0x000A3220 File Offset: 0x000A1420
		public GDPointGraphScanPromise(GDPointGraph graph)
		{
			this.graph = graph;
		}

		// Token: 0x060022DF RID: 8927 RVA: 0x000A322F File Offset: 0x000A142F
		public IEnumerator<JobHandle> Prepare()
		{
			yield break;
		}

		// Token: 0x060022E0 RID: 8928 RVA: 0x000A3238 File Offset: 0x000A1438
		public void Apply(IGraphUpdateContext ctx)
		{
			this.graph.DestroyAllNodes();
			if (MainGame.Instance == null)
			{
				Debug.Log("GDPointGraph: no MainGame instance, skipping scan");
				return;
			}
			List<GDPointData> points = MainGame.Instance.GameSave.worldData.gdPointsData.Points;
			if (points == null || points.Count == 0)
			{
				Debug.Log("GDPointGraph: no points data, skipping scan");
				return;
			}
			foreach (GDPointData gdpointData in points)
			{
				gdpointData.IsGraphPoint = false;
				gdpointData.DeInitNode();
			}
			foreach (GDPointData gdpointData2 in points)
			{
				if (gdpointData2.Enabled && gdpointData2.IsWaypoint)
				{
					if (gdpointData2.NextPointData.Count > 0)
					{
						gdpointData2.IsGraphPoint = true;
					}
					foreach (GDPointData gdpointData3 in gdpointData2.NextPointData)
					{
						if (gdpointData3.Enabled)
						{
							gdpointData3.IsGraphPoint = true;
						}
					}
				}
			}
			foreach (GDPointData gdpointData4 in points)
			{
				if (gdpointData4.IsGraphPoint)
				{
					Int3 @int = (Int3)gdpointData4.Position;
					gdpointData4.InitNode();
					this.graph.AddNode<GDPointNode>(gdpointData4.Node, @int);
				}
			}
			MainGame.Instance.GraphHelper.ResumePathFinding();
			Debug.Log(string.Format("GDPointGraph: scan complete. nodeCount = {0}", this.graph.nodeCount));
		}

		// Token: 0x170005A9 RID: 1449
		// (get) Token: 0x060022E1 RID: 8929 RVA: 0x000A3420 File Offset: 0x000A1620
		public float Progress
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x04001F74 RID: 8052
		private GDPointGraph graph;
	}
}
