using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000731 RID: 1841
public class GraphCustomNavMeshCutUnit : MonoBehaviour
{
	// Token: 0x06003018 RID: 12312 RVA: 0x000E6D78 File Offset: 0x000E4F78
	public void Assign(SGuid holder)
	{
		this.holder = holder;
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x000E6D81 File Offset: 0x000E4F81
	public void Release()
	{
		this.CleanupSpawnedInstances();
		this.holder = SGuid.Empty;
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x000E6D94 File Offset: 0x000E4F94
	public void UpdateParameters(Vector3 center, Vector3 scale, WgoData wgoData)
	{
		base.transform.position = center;
		base.transform.localScale = scale;
		this.RebuildInstances(wgoData);
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x000E6DB5 File Offset: 0x000E4FB5
	public void UpdateTransform(Vector3 center, Vector3 scale)
	{
		base.transform.position = center;
		base.transform.localScale = scale;
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x000E6DD0 File Offset: 0x000E4FD0
	public static bool HasBakedCustomNavMeshCuts(WgoData wgoData)
	{
		if (wgoData == null)
		{
			return false;
		}
		if (GraphCustomNavMeshCutUnit.PartHasCustomNavMeshCuts(wgoData.MainWgoPartData))
		{
			return true;
		}
		using (List<WgoPartData>.Enumerator enumerator = wgoData.AdditionalWgoPartsData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (GraphCustomNavMeshCutUnit.PartHasCustomNavMeshCuts(enumerator.Current))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x000E6E40 File Offset: 0x000E5040
	private static bool PartHasCustomNavMeshCuts(WgoPartData partData)
	{
		if (partData != null)
		{
			WgoPartBakedData bakedData = partData.BakedData;
			if (!string.IsNullOrEmpty((bakedData != null) ? bakedData.id : null))
			{
				int stateHash = partData.GetStateHash();
				List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData> list;
				if (!partData.BakedData.TryGetCustomNavMeshCutPrefabs(stateHash, out list) || list == null)
				{
					return false;
				}
				for (int i = 0; i < list.Count; i++)
				{
					WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData customNavMeshCutPrefabEntryBakedData = list[i];
					if (((customNavMeshCutPrefabEntryBakedData != null) ? customNavMeshCutPrefabEntryBakedData.prefabRef : null) != null && customNavMeshCutPrefabEntryBakedData.prefabRef.RuntimeKeyIsValid())
					{
						return true;
					}
				}
				return false;
			}
		}
		return false;
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x000E6EC0 File Offset: 0x000E50C0
	public static bool TryCollectSpawnEntries(WgoData wgoData, out List<GraphCustomNavMeshCutUnit.SpawnEntry> entries)
	{
		entries = new List<GraphCustomNavMeshCutUnit.SpawnEntry>();
		if (wgoData == null)
		{
			return false;
		}
		GraphCustomNavMeshCutUnit.CollectPartEntries(wgoData.MainWgoPartData, entries);
		foreach (WgoPartData wgoPartData in wgoData.AdditionalWgoPartsData)
		{
			GraphCustomNavMeshCutUnit.CollectPartEntries(wgoPartData, entries);
		}
		return entries.Count > 0;
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000E6F38 File Offset: 0x000E5138
	private static void CollectPartEntries(WgoPartData partData, List<GraphCustomNavMeshCutUnit.SpawnEntry> entries)
	{
		if (partData != null)
		{
			WgoPartBakedData bakedData = partData.BakedData;
			if (!string.IsNullOrEmpty((bakedData != null) ? bakedData.id : null))
			{
				int stateHash = partData.GetStateHash();
				List<WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData> list;
				if (!partData.BakedData.TryGetCustomNavMeshCutPrefabs(stateHash, out list) || list == null)
				{
					return;
				}
				for (int i = 0; i < list.Count; i++)
				{
					WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData customNavMeshCutPrefabEntryBakedData = list[i];
					if (((customNavMeshCutPrefabEntryBakedData != null) ? customNavMeshCutPrefabEntryBakedData.prefabRef : null) != null && customNavMeshCutPrefabEntryBakedData.prefabRef.RuntimeKeyIsValid())
					{
						entries.Add(new GraphCustomNavMeshCutUnit.SpawnEntry
						{
							wgoPartData = partData,
							bakedEntry = customNavMeshCutPrefabEntryBakedData
						});
					}
				}
				return;
			}
		}
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000E6FD4 File Offset: 0x000E51D4
	private void RebuildInstances(WgoData wgoData)
	{
		this.CleanupSpawnedInstances();
		if (wgoData == null)
		{
			return;
		}
		List<GraphCustomNavMeshCutUnit.SpawnEntry> list;
		if (!GraphCustomNavMeshCutUnit.TryCollectSpawnEntries(wgoData, out list))
		{
			string text = string.Format("[GraphCustomNavMeshCutUnit] No spawn entries for WGO [{0}] holder [{1}], ", wgoData.id, this.holder);
			string text2 = "part [{0}] hash [{1}]";
			WgoPartData mainWgoPartData = wgoData.MainWgoPartData;
			object obj = ((mainWgoPartData != null) ? mainWgoPartData.id : null);
			WgoPartData mainWgoPartData2 = wgoData.MainWgoPartData;
			Debug.LogWarning(text + string.Format(text2, obj, (mainWgoPartData2 != null) ? new int?(mainWgoPartData2.GetStateHash()) : null));
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			GraphCustomNavMeshCutUnit.SpawnEntry spawnEntry = list[i];
			string assetGUID = spawnEntry.bakedEntry.prefabRef.AssetGUID;
			AsyncOperationHandle<GameObject> asyncOperationHandle = spawnEntry.bakedEntry.prefabRef.InstantiateAsync(base.transform, false);
			GameObject gameObject = asyncOperationHandle.WaitForCompletion();
			if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded || gameObject == null)
			{
				string[] array = new string[6];
				array[0] = "[GraphCustomNavMeshCutUnit] Failed to spawn cut prefab [";
				array[1] = assetGUID;
				array[2] = "] for WGO [";
				array[3] = wgoData.id;
				array[4] = "] ";
				int num = 5;
				string text3 = "holder [{0}] status [{1}] error [{2}]";
				object obj2 = this.holder;
				object obj3 = asyncOperationHandle.Status;
				Exception operationException = asyncOperationHandle.OperationException;
				array[num] = string.Format(text3, obj2, obj3, (operationException != null) ? operationException.Message : null);
				Debug.LogWarning(string.Concat(array));
				if (asyncOperationHandle.IsValid())
				{
					Addressables.ReleaseInstance(asyncOperationHandle);
				}
			}
			else
			{
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
				Transform transform = gameObject.transform;
				transform.localPosition = spawnEntry.bakedEntry.localPosition;
				transform.localRotation = spawnEntry.bakedEntry.localRotation;
				transform.localScale = spawnEntry.bakedEntry.localScale;
				GraphCustomNavMeshCutUnit.NotifySpawned(gameObject, wgoData, spawnEntry.wgoPartData);
				this.spawnedInstances.Add(new GraphCustomNavMeshCutUnit.SpawnedInstance
				{
					instance = gameObject,
					handle = asyncOperationHandle
				});
				Debug.Log(string.Concat(new string[]
				{
					"[GraphCustomNavMeshCutUnit] Spawned [",
					gameObject.name,
					"] prefab [",
					assetGUID,
					"] for WGO [",
					wgoData.id,
					"] ",
					string.Format("holder [{0}] localPos [{1}]", this.holder, spawnEntry.bakedEntry.localPosition)
				}));
			}
		}
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000E7224 File Offset: 0x000E5424
	private static void NotifySpawned(GameObject instance, WgoData wgoData, WgoPartData wgoPartData)
	{
		ICustomNavMeshCut[] componentsInChildren = instance.GetComponentsInChildren<ICustomNavMeshCut>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].OnCustomNavMeshCutSpawn(wgoData, wgoPartData);
		}
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x000E7254 File Offset: 0x000E5454
	private void CleanupSpawnedInstances()
	{
		for (int i = this.spawnedInstances.Count - 1; i >= 0; i--)
		{
			GraphCustomNavMeshCutUnit.SpawnedInstance spawnedInstance = this.spawnedInstances[i];
			if (spawnedInstance.handle.IsValid())
			{
				Addressables.ReleaseInstance(spawnedInstance.handle);
			}
			else if (spawnedInstance.instance != null)
			{
				global::UnityEngine.Object.Destroy(spawnedInstance.instance);
			}
		}
		this.spawnedInstances.Clear();
	}

	// Token: 0x040026FB RID: 9979
	private readonly List<GraphCustomNavMeshCutUnit.SpawnedInstance> spawnedInstances = new List<GraphCustomNavMeshCutUnit.SpawnedInstance>();

	// Token: 0x040026FC RID: 9980
	public SGuid holder = SGuid.Empty;

	// Token: 0x02000732 RID: 1842
	public struct SpawnEntry
	{
		// Token: 0x040026FD RID: 9981
		public WgoPartData wgoPartData;

		// Token: 0x040026FE RID: 9982
		public WgoPartBakedData.CustomNavMeshCutPrefabEntryBakedData bakedEntry;
	}

	// Token: 0x02000733 RID: 1843
	private class SpawnedInstance
	{
		// Token: 0x040026FF RID: 9983
		public GameObject instance;

		// Token: 0x04002700 RID: 9984
		public AsyncOperationHandle<GameObject> handle;
	}
}
