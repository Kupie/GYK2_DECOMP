using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;

// Token: 0x02000725 RID: 1829
public class AStarScanScheduler : LazySingleton<AStarScanScheduler>
{
	// Token: 0x06002FCC RID: 12236 RVA: 0x000E55B8 File Offset: 0x000E37B8
	public IEnumerable<Progress> ScanGraphAsync(NavGraph graph)
	{
		bool isCompleted = false;
		bool hasStartedScanning = false;
		AStarScanScheduler.ScanItem item = new AStarScanScheduler.ScanItem(graph, delegate
		{
			isCompleted = true;
		}, delegate
		{
			hasStartedScanning = true;
		});
		this.scanQueue.Enqueue(item);
		if (!this.isScanningInProgress)
		{
			base.StartCoroutine(this.ProcessScanQueue());
		}
		while (!hasStartedScanning && !isCompleted)
		{
			yield return new Progress(0f, ScanningStage.PreProcessingGraphs, 0, 0);
		}
		if (hasStartedScanning && !isCompleted)
		{
			foreach (Progress progress in item.GetScanProgress())
			{
				yield return progress;
			}
			IEnumerator<Progress> enumerator = null;
		}
		yield break;
		yield break;
	}

	// Token: 0x06002FCD RID: 12237 RVA: 0x000E55CF File Offset: 0x000E37CF
	public void ScanGraph(NavGraph graph)
	{
		AstarPath.active.Scan(graph);
	}

	// Token: 0x06002FCE RID: 12238 RVA: 0x000E55DC File Offset: 0x000E37DC
	public void ClearScanQueue()
	{
		int count = this.scanQueue.Count;
		this.scanQueue.Clear();
		Debug.LogWarning(string.Format("[SCAN QUEUE] Cleared {0} pending scans from queue", count));
	}

	// Token: 0x06002FCF RID: 12239 RVA: 0x000E5615 File Offset: 0x000E3815
	private IEnumerator ProcessScanQueue()
	{
		this.isScanningInProgress = true;
		while (this.scanQueue.Count > 0)
		{
			if (AstarPath.active.isScanning)
			{
				yield return null;
			}
			AStarScanScheduler.ScanItem request = this.scanQueue.Dequeue();
			Debug.Log(string.Format("[SCAN QUEUE] Processing scan for graph {0}. Queue remaining: {1}", request.graph, this.scanQueue.Count));
			request.StartScan();
			foreach (Progress progress in request.GetScanProgress())
			{
				yield return null;
			}
			IEnumerator<Progress> enumerator = null;
			Action onCompleted = request.onCompleted;
			if (onCompleted != null)
			{
				onCompleted();
			}
			Debug.Log(string.Format("[SCAN QUEUE] Completed scan for graph {0}", request.graph));
			yield return new WaitForEndOfFrame();
			request = null;
		}
		this.isScanningInProgress = false;
		Debug.Log("[SCAN QUEUE] All scans completed. Queue is empty.");
		yield break;
		yield break;
	}

	// Token: 0x040026BC RID: 9916
	private Queue<AStarScanScheduler.ScanItem> scanQueue = new Queue<AStarScanScheduler.ScanItem>();

	// Token: 0x040026BD RID: 9917
	private bool isScanningInProgress;

	// Token: 0x02000726 RID: 1830
	private class ScanItem
	{
		// Token: 0x06002FD1 RID: 12241 RVA: 0x000E5637 File Offset: 0x000E3837
		public ScanItem(NavGraph graph, Action onCompleted, Action onStarted = null)
		{
			this.graph = graph;
			this.onStarted = onStarted;
			this.onCompleted = onCompleted;
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x000E5654 File Offset: 0x000E3854
		public void StartScan()
		{
			this.scanProgress = AstarPath.active.ScanAsync(this.graph);
			Action action = this.onStarted;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000E567C File Offset: 0x000E387C
		public IEnumerable<Progress> GetScanProgress()
		{
			return this.scanProgress ?? Enumerable.Empty<Progress>();
		}

		// Token: 0x040026BE RID: 9918
		public NavGraph graph;

		// Token: 0x040026BF RID: 9919
		public Action onStarted;

		// Token: 0x040026C0 RID: 9920
		public Action onCompleted;

		// Token: 0x040026C1 RID: 9921
		private IEnumerable<Progress> scanProgress;
	}
}
