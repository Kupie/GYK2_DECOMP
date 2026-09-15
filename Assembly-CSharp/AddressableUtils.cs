using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000A87 RID: 2695
public static class AddressableUtils
{
	// Token: 0x06004964 RID: 18788 RVA: 0x0015AC58 File Offset: 0x00158E58
	public static T LoadAssetReferenceSync<T>(AssetReferenceT<T> assetReference, ref AsyncOperationHandle<T> handle) where T : global::UnityEngine.Object
	{
		T t = default(T);
		if (handle.IsValid())
		{
			Addressables.Release<T>(handle);
		}
		if (assetReference != null && assetReference.RuntimeKeyIsValid())
		{
			handle = assetReference.LoadAssetAsync<T>();
			try
			{
				if (GameShutdown.IsRequested)
				{
					if (handle.IsValid())
					{
						Addressables.Release<T>(handle);
					}
					return default(T);
				}
				t = handle.WaitForCompletion();
			}
			catch (Exception ex)
			{
				Debug.LogError("Failed to synchronously load asset with GUID '" + assetReference.AssetGUID + "': " + ex.Message);
				if (handle.IsValid())
				{
					Addressables.Release<T>(handle);
				}
			}
			return t;
		}
		return t;
	}

	// Token: 0x06004965 RID: 18789 RVA: 0x0015AD10 File Offset: 0x00158F10
	public static void ReleaseAssetReference<T>(ref AsyncOperationHandle<T> handle) where T : class
	{
		if (handle.IsValid())
		{
			Addressables.Release<T>(handle);
			handle = default(AsyncOperationHandle<T>);
		}
	}
}
