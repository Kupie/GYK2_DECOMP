using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;

// Token: 0x020006A7 RID: 1703
public class WsoOptimizedStagesLoadManager : LazySingleton<WsoOptimizedStagesLoadManager>
{
	// Token: 0x06002D93 RID: 11667 RVA: 0x000DA390 File Offset: 0x000D8590
	public void RequestLoad(Wso wso, bool async = true)
	{
		if (wso == null)
		{
			return;
		}
		int num = this.requestTracker.Begin(wso);
		if (async)
		{
			this.LoadAsync(wso, num).Forget();
			return;
		}
		this.LoadSync(wso, num);
	}

	// Token: 0x06002D94 RID: 11668 RVA: 0x000DA3CD File Offset: 0x000D85CD
	public void CancelLoad(Wso wso)
	{
		this.requestTracker.Cancel(wso);
	}

	// Token: 0x1700071E RID: 1822
	// (get) Token: 0x06002D95 RID: 11669 RVA: 0x000DA3DB File Offset: 0x000D85DB
	public bool HasActiveRequests
	{
		get
		{
			return this.requestTracker.HasActiveRequests;
		}
	}

	// Token: 0x06002D96 RID: 11670 RVA: 0x000DA3E8 File Offset: 0x000D85E8
	public async UniTask WaitForAllRequestsAsync()
	{
		while (this.HasActiveRequests)
		{
			await UniTask.NextFrame();
		}
	}

	// Token: 0x06002D97 RID: 11671 RVA: 0x000DA42C File Offset: 0x000D862C
	public async UniTask WaitForRequestAsync(Wso wso)
	{
		if (!(wso == null))
		{
			while (this.requestTracker.IsTracking(wso))
			{
				await UniTask.NextFrame();
			}
		}
	}

	// Token: 0x06002D98 RID: 11672 RVA: 0x000DA478 File Offset: 0x000D8678
	public async UniTask WaitForRequestsAsync(IEnumerable<Wso> wsos)
	{
		if (wsos != null)
		{
			foreach (Wso wso in wsos)
			{
				await this.WaitForRequestAsync(wso);
			}
			IEnumerator<Wso> enumerator = null;
		}
	}

	// Token: 0x06002D99 RID: 11673 RVA: 0x000DA4C4 File Offset: 0x000D86C4
	private async UniTask LoadAsync(Wso wso, int requestId)
	{
		if (!(wso == null))
		{
			WsoOptimizedStagesBuildResult wsoOptimizedStagesBuildResult = await wso.BuildOptimizedStagesAsync();
			if (!this.requestTracker.IsActual(wso, requestId))
			{
				wso.ReleasePendingOptimizedStages(wsoOptimizedStagesBuildResult);
			}
			else
			{
				this.requestTracker.Complete(wso, requestId);
				wso.CompleteOptimizedStagesLoad(wsoOptimizedStagesBuildResult);
			}
		}
	}

	// Token: 0x06002D9A RID: 11674 RVA: 0x000DA518 File Offset: 0x000D8718
	private void LoadSync(Wso wso, int requestId)
	{
		if (wso == null)
		{
			return;
		}
		WsoOptimizedStagesBuildResult wsoOptimizedStagesBuildResult = wso.BuildOptimizedStagesSync();
		if (!this.requestTracker.IsActual(wso, requestId))
		{
			wso.ReleasePendingOptimizedStages(wsoOptimizedStagesBuildResult);
			return;
		}
		this.requestTracker.Complete(wso, requestId);
		wso.CompleteOptimizedStagesLoad(wsoOptimizedStagesBuildResult);
	}

	// Token: 0x04002489 RID: 9353
	private readonly AsyncLoadRequestTracker<Wso> requestTracker = new AsyncLoadRequestTracker<Wso>();
}
