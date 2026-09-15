using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000692 RID: 1682
public class WgoPartLoadManager : LazySingleton<WgoPartLoadManager>
{
	// Token: 0x06002D26 RID: 11558 RVA: 0x000D7644 File Offset: 0x000D5844
	public int RequestLoad(Wgo wgo, bool applyDefaultWgoPartState, bool async = true)
	{
		if (wgo == null)
		{
			return -1;
		}
		int num = this.requestTracker.Begin(wgo);
		if (async)
		{
			this.LoadAndApplyAsync(wgo, num, applyDefaultWgoPartState).Forget();
		}
		else
		{
			this.LoadAndApplySync(wgo, num, applyDefaultWgoPartState);
		}
		return num;
	}

	// Token: 0x06002D27 RID: 11559 RVA: 0x000D7686 File Offset: 0x000D5886
	public void CancelLoad(Wgo wgo)
	{
		this.requestTracker.Cancel(wgo);
	}

	// Token: 0x17000710 RID: 1808
	// (get) Token: 0x06002D28 RID: 11560 RVA: 0x000D7694 File Offset: 0x000D5894
	public bool HasActiveRequests
	{
		get
		{
			return this.requestTracker.HasActiveRequests;
		}
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x000D76A4 File Offset: 0x000D58A4
	public async UniTask WaitForAllRequestsAsync()
	{
		while (this.HasActiveRequests)
		{
			await UniTask.NextFrame();
		}
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x000D76E8 File Offset: 0x000D58E8
	public async UniTask WaitForRequestAsync(Wgo wgo)
	{
		if (!(wgo == null))
		{
			while (this.requestTracker.IsTracking(wgo))
			{
				await UniTask.NextFrame();
			}
		}
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x000D7734 File Offset: 0x000D5934
	public async UniTask WaitForRequestsAsync(IEnumerable<Wgo> wgos)
	{
		if (wgos != null)
		{
			foreach (Wgo wgo in wgos)
			{
				await this.WaitForRequestAsync(wgo);
			}
			IEnumerator<Wgo> enumerator = null;
		}
	}

	// Token: 0x06002D2C RID: 11564 RVA: 0x000D777F File Offset: 0x000D597F
	private bool IsRequestActual(Wgo wgo, int requestId)
	{
		return this.requestTracker.IsActual(wgo, requestId);
	}

	// Token: 0x06002D2D RID: 11565 RVA: 0x000D7790 File Offset: 0x000D5990
	private void LoadAndApplySync(Wgo wgo, int requestId, bool applyDefaultWgoPartState)
	{
		if (wgo == null)
		{
			return;
		}
		string mainWgoPartAssetId = wgo.GetMainWgoPartAssetId();
		string text = "Assets/AddressableAssets/WGOs/" + mainWgoPartAssetId + ".prefab";
		WgoPart sync = LazySingleton<WgoPartPool>.Instance.GetSync(text, wgo);
		if (sync == null)
		{
			if (this.IsRequestActual(wgo, requestId))
			{
				this.requestTracker.Complete(wgo, requestId);
				wgo.HandleVisualPartsLoadFailed();
			}
			return;
		}
		List<WgoPart> list = new List<WgoPart>();
		List<WgoPartData> list2 = new List<WgoPartData>();
		List<WgoPartData> additionalWgoPartsData = wgo.Data.AdditionalWgoPartsData;
		for (int i = 0; i < additionalWgoPartsData.Count; i++)
		{
			WgoPartData wgoPartData = additionalWgoPartsData[i];
			string text2 = "Assets/AddressableAssets/WGOs/" + wgoPartData.id + ".prefab";
			WgoPart sync2 = LazySingleton<WgoPartPool>.Instance.GetSync(text2, wgo);
			if (sync2 != null)
			{
				list.Add(sync2);
				list2.Add(wgoPartData);
			}
		}
		if (!this.IsRequestActual(wgo, requestId))
		{
			WgoPartLoadManager.ReleaseLoadedParts(sync, list);
			return;
		}
		this.requestTracker.Complete(wgo, requestId);
		wgo.CompleteVisualPartsLoad(sync, list, list2, applyDefaultWgoPartState);
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x000D789C File Offset: 0x000D5A9C
	private async UniTask LoadAndApplyAsync(Wgo wgo, int requestId, bool applyDefaultWgoPartState)
	{
		if (!(wgo == null))
		{
			string mainWgoPartAssetId = wgo.GetMainWgoPartAssetId();
			string text = "Assets/AddressableAssets/WGOs/" + mainWgoPartAssetId + ".prefab";
			WgoPart wgoPart = await LazySingleton<WgoPartPool>.Instance.GetAsync(text, wgo);
			WgoPart mainPart = wgoPart;
			if (mainPart == null)
			{
				if (this.IsRequestActual(wgo, requestId))
				{
					this.requestTracker.Complete(wgo, requestId);
					wgo.HandleVisualPartsLoadFailed();
				}
			}
			else
			{
				List<WgoPart> additionalParts = new List<WgoPart>();
				List<WgoPartData> loadedAdditionalData = new List<WgoPartData>();
				List<WgoPartData> additionalData = wgo.Data.AdditionalWgoPartsData;
				for (int i = 0; i < additionalData.Count; i++)
				{
					WgoPartData addData = additionalData[i];
					string text2 = "Assets/AddressableAssets/WGOs/" + addData.id + ".prefab";
					WgoPart wgoPart2 = await LazySingleton<WgoPartPool>.Instance.GetAsync(text2, wgo);
					if (wgoPart2 != null)
					{
						additionalParts.Add(wgoPart2);
						loadedAdditionalData.Add(addData);
					}
					addData = null;
				}
				if (!this.IsRequestActual(wgo, requestId))
				{
					WgoPartLoadManager.ReleaseLoadedParts(mainPart, additionalParts);
				}
				else
				{
					this.requestTracker.Complete(wgo, requestId);
					wgo.CompleteVisualPartsLoad(mainPart, additionalParts, loadedAdditionalData, applyDefaultWgoPartState);
				}
			}
		}
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x000D78F8 File Offset: 0x000D5AF8
	private static void ReleaseLoadedParts(WgoPart mainPart, List<WgoPart> additionalParts)
	{
		if (mainPart != null && !string.IsNullOrEmpty(mainPart.PooledAddressableKey))
		{
			LazySingleton<WgoPartPool>.Instance.Release(mainPart.PooledAddressableKey, mainPart);
		}
		else if (mainPart != null)
		{
			global::UnityEngine.Object.Destroy(mainPart.gameObject);
		}
		for (int i = 0; i < additionalParts.Count; i++)
		{
			WgoPart wgoPart = additionalParts[i];
			if (!(wgoPart == null))
			{
				if (!string.IsNullOrEmpty(wgoPart.PooledAddressableKey))
				{
					LazySingleton<WgoPartPool>.Instance.Release(wgoPart.PooledAddressableKey, wgoPart);
				}
				else
				{
					global::UnityEngine.Object.Destroy(wgoPart.gameObject);
				}
			}
		}
	}

	// Token: 0x04002410 RID: 9232
	private readonly AsyncLoadRequestTracker<Wgo> requestTracker = new AsyncLoadRequestTracker<Wgo>();
}
