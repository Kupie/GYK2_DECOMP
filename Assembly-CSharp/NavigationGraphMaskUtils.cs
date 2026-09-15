using System;
using System.Collections.Generic;
using Pathfinding;

// Token: 0x02000AF5 RID: 2805
public static class NavigationGraphMaskUtils
{
	// Token: 0x06004AE3 RID: 19171 RVA: 0x00161724 File Offset: 0x0015F924
	public static GraphMask ToGraphMask(LazyConsts.Navigation.Graph graph)
	{
		if (graph == LazyConsts.Navigation.Graph.None)
		{
			return default(GraphMask);
		}
		return GraphMask.FromGraphIndex((uint)graph);
	}

	// Token: 0x06004AE4 RID: 19172 RVA: 0x00161748 File Offset: 0x0015F948
	public static GraphMask ToGraphMask(IEnumerable<LazyConsts.Navigation.Graph> graphs)
	{
		GraphMask graphMask = default(GraphMask);
		if (graphs == null)
		{
			return graphMask;
		}
		foreach (LazyConsts.Navigation.Graph graph in graphs)
		{
			if (graph != LazyConsts.Navigation.Graph.None)
			{
				graphMask |= GraphMask.FromGraphIndex((uint)graph);
			}
		}
		return graphMask;
	}

	// Token: 0x06004AE5 RID: 19173 RVA: 0x001617A8 File Offset: 0x0015F9A8
	public static GraphMask ToGraphMask(IEnumerable<LazyConsts.Navigation.Graph> graphs, LazyConsts.Navigation.Graph fallbackGraph)
	{
		GraphMask graphMask = NavigationGraphMaskUtils.ToGraphMask(graphs);
		if (!(graphMask == default(GraphMask)))
		{
			return graphMask;
		}
		return NavigationGraphMaskUtils.ToGraphMask(fallbackGraph);
	}

	// Token: 0x06004AE6 RID: 19174 RVA: 0x001617D8 File Offset: 0x0015F9D8
	public static GraphMask ToCutGraphMask(LazyConsts.Navigation.Graph graph)
	{
		GraphMask graphMask = NavigationGraphMaskUtils.ToGraphMask(graph);
		if (graphMask == default(GraphMask))
		{
			return graphMask;
		}
		return graphMask | NavigationGraphMaskUtils.ToGraphMask(LazyConsts.Navigation.Graph.RuinedTemple);
	}
}
