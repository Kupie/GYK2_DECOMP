using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;

// Token: 0x0200069F RID: 1695
public class WsoConstructorPartsLoadManager : LazySingleton<WsoConstructorPartsLoadManager>
{
	// Token: 0x06002D7F RID: 11647 RVA: 0x000D9D74 File Offset: 0x000D7F74
	public void RequestRebuild(Wso wso, bool async = true)
	{
		if (wso == null)
		{
			return;
		}
		int num = this.requestTracker.Begin(wso);
		if (async)
		{
			this.RebuildAsync(wso, num).Forget();
			return;
		}
		this.RebuildSync(wso, num);
	}

	// Token: 0x06002D80 RID: 11648 RVA: 0x000D9DB1 File Offset: 0x000D7FB1
	public void CancelRebuild(Wso wso)
	{
		this.requestTracker.Cancel(wso);
	}

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x06002D81 RID: 11649 RVA: 0x000D9DBF File Offset: 0x000D7FBF
	public bool HasActiveRequests
	{
		get
		{
			return this.requestTracker.HasActiveRequests;
		}
	}

	// Token: 0x06002D82 RID: 11650 RVA: 0x000D9DCC File Offset: 0x000D7FCC
	public async UniTask WaitForAllRequestsAsync()
	{
		while (this.HasActiveRequests)
		{
			await UniTask.NextFrame();
		}
	}

	// Token: 0x06002D83 RID: 11651 RVA: 0x000D9E10 File Offset: 0x000D8010
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

	// Token: 0x06002D84 RID: 11652 RVA: 0x000D9E5C File Offset: 0x000D805C
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

	// Token: 0x06002D85 RID: 11653 RVA: 0x000D9EA8 File Offset: 0x000D80A8
	private async UniTask RebuildAsync(Wso wso, int requestId)
	{
		if (!(wso == null))
		{
			WsoConstructorPartsBuildResult wsoConstructorPartsBuildResult = await wso.BuildRuntimeConstructorPartsAsync();
			if (!this.requestTracker.IsActual(wso, requestId))
			{
				wso.ReleasePendingRuntimeParts(wsoConstructorPartsBuildResult);
			}
			else
			{
				this.requestTracker.Complete(wso, requestId);
				wso.CompleteRuntimePartsRebuild(wsoConstructorPartsBuildResult);
			}
		}
	}

	// Token: 0x06002D86 RID: 11654 RVA: 0x000D9EFC File Offset: 0x000D80FC
	private void RebuildSync(Wso wso, int requestId)
	{
		if (wso == null)
		{
			return;
		}
		WsoConstructorPartsBuildResult wsoConstructorPartsBuildResult = wso.BuildRuntimeConstructorPartsSync();
		if (!this.requestTracker.IsActual(wso, requestId))
		{
			wso.ReleasePendingRuntimeParts(wsoConstructorPartsBuildResult);
			return;
		}
		this.requestTracker.Complete(wso, requestId);
		wso.CompleteRuntimePartsRebuild(wsoConstructorPartsBuildResult);
	}

	// Token: 0x0400246F RID: 9327
	private readonly AsyncLoadRequestTracker<Wso> requestTracker = new AsyncLoadRequestTracker<Wso>();
}
