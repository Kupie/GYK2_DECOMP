using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LazyBearTechnology.Preloader
{
	// Token: 0x0200019A RID: 410
	public class LazyAssetPreloader
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0002CED4 File Offset: 0x0002B0D4
		public bool IsLoading
		{
			get
			{
				return this.loadingAssetsCount > 0;
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0002CEE0 File Offset: 0x0002B0E0
		public void AddressablesLoadAssetAsync<TObject>(string name)
		{
			Debug.Log("Loading asset: " + name);
			this.loadingAssetsCount++;
			LazyAssetPreloader.LoadingAsset assetHandle = new LazyAssetPreloader.LoadingAsset
			{
				handle = Addressables.LoadAssetAsync<TObject>(name),
				isLoading = true
			};
			this.assetHandles.Add(assetHandle);
			assetHandle.handle.Completed += delegate(AsyncOperationHandle operationHandle)
			{
				this.loadingAssetsCount--;
				assetHandle.isLoading = false;
				if (operationHandle.OperationException != null)
				{
					string[] array = new string[6];
					array[0] = "Error loading asset: ";
					array[1] = name;
					array[2] = ", result: ";
					int num = 3;
					object result = operationHandle.Result;
					array[num] = ((result != null) ? result.ToString() : null);
					array[4] = ", exception: ";
					int num2 = 5;
					Exception operationException = operationHandle.OperationException;
					array[num2] = ((operationException != null) ? operationException.ToString() : null);
					Debug.LogError(string.Concat(array));
					return;
				}
				Debug.Log("Loading asset complete: " + name);
			};
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0002CF80 File Offset: 0x0002B180
		public int GetPercentLoaded()
		{
			if (this.assetHandles.Count == 0)
			{
				return 0;
			}
			return Mathf.FloorToInt(this.assetHandles.Sum(delegate(LazyAssetPreloader.LoadingAsset handle)
			{
				if (!handle.isLoading)
				{
					return 1f;
				}
				return handle.handle.PercentComplete;
			}) / (float)this.assetHandles.Count * 100f);
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0002CFDE File Offset: 0x0002B1DE
		public IEnumerator WaitUntilLoaded()
		{
			while (this.loadingAssetsCount > 0)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400059A RID: 1434
		private readonly List<LazyAssetPreloader.LoadingAsset> assetHandles = new List<LazyAssetPreloader.LoadingAsset>();

		// Token: 0x0400059B RID: 1435
		private int loadingAssetsCount;

		// Token: 0x02000210 RID: 528
		private struct LoadingAsset
		{
			// Token: 0x040006FF RID: 1791
			public AsyncOperationHandle handle;

			// Token: 0x04000700 RID: 1792
			public bool isLoading;
		}
	}
}
