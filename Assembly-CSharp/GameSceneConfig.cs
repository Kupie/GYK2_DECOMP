using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000539 RID: 1337
public class GameSceneConfig : ScriptableObject
{
	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x0600225C RID: 8796 RVA: 0x000A14B5 File Offset: 0x0009F6B5
	public GDPointData[] GdPointsData
	{
		get
		{
			return this.gdPointsData;
		}
	}

	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x0600225D RID: 8797 RVA: 0x000A14C0 File Offset: 0x0009F6C0
	public IReadOnlyList<IndoorAreaData> IndoorAreas
	{
		get
		{
			List<IndoorAreaData> list;
			if ((list = this.indoorAreas) == null)
			{
				list = (this.indoorAreas = new List<IndoorAreaData>());
			}
			return list;
		}
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x000A14E5 File Offset: 0x0009F6E5
	public static string GetPathToGameSceneConfig(bool isDev)
	{
		if (!isDev)
		{
			return "Assets/AddressableAssets/SceneContents/_GameSceneConfigs";
		}
		return "Assets/AddressableAssets/SceneContentsDev/_GameSceneConfigs";
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x000A14F5 File Offset: 0x0009F6F5
	public static string GetPathToSceneData(bool isDev)
	{
		if (!isDev)
		{
			return "Assets/AddressableAssets/SceneContents/WgoContentsData";
		}
		return "Assets/AddressableAssets/SceneContentsDev/WgoContentsData";
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x000A1505 File Offset: 0x0009F705
	public static string GetPathToSceneContent(bool isDev)
	{
		if (!isDev)
		{
			return "Assets/AddressableAssets/SceneContents/WgoContents";
		}
		return "Assets/AddressableAssets/SceneContentsDev/WgoContents";
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x000A1515 File Offset: 0x0009F715
	public static string GetPathToSceneWaypoint(bool isDev)
	{
		if (!isDev)
		{
			return "Assets/AddressableAssets/SceneContents/WaypointContents";
		}
		return "Assets/AddressableAssets/SceneContentsDev/WaypointContents";
	}

	// Token: 0x06002262 RID: 8802 RVA: 0x000A1528 File Offset: 0x0009F728
	public static List<GameSceneConfig> LoadAllConfigs()
	{
		List<GameSceneConfig> configs = new List<GameSceneConfig>();
		Debug.Log(string.Format("#shutdown# GameSceneConfig.LoadAllConfigs: WaitForCompletion begin (shutdownRequested:[{0}])", GameShutdown.IsRequested));
		if (GameShutdown.IsQuitting)
		{
			return configs;
		}
		Addressables.LoadAssetsAsync<ScriptableObject>("GameSceneConfigs", delegate(ScriptableObject obj)
		{
			GameSceneConfig gameSceneConfig = obj as GameSceneConfig;
			if (gameSceneConfig != null)
			{
				configs.Add(gameSceneConfig);
			}
		}).WaitForCompletion();
		Debug.Log("#shutdown# GameSceneConfig.LoadAllConfigs: WaitForCompletion done");
		Debug.Log(string.Format("Loaded {0} GameSceneConfigs", configs.Count));
		return configs;
	}

	// Token: 0x06002263 RID: 8803 RVA: 0x000A15BC File Offset: 0x0009F7BC
	public bool TryLoadSceneDataContent(out List<SceneWgoContentData> sceneWgoContentDatas)
	{
		if (this.contentDataRefs == null || this.contentDataRefs.Count == 0)
		{
			sceneWgoContentDatas = null;
			return false;
		}
		this.contentDataRefInstances = new Dictionary<AssetReference, GameObject>();
		sceneWgoContentDatas = new List<SceneWgoContentData>();
		for (int i = 0; i < this.contentDataRefs.Count; i++)
		{
			AssetReference assetReference = this.contentDataRefs[i];
			SceneWgoContentData sceneWgoContentData;
			if ((this.autoLoadFlags == null || this.autoLoadFlags.Count <= i || this.autoLoadFlags[i]) && this.TryLoadSceneWgoContentData(assetReference, out sceneWgoContentData))
			{
				sceneWgoContentDatas.Add(sceneWgoContentData);
			}
		}
		return sceneWgoContentDatas.Count > 0;
	}

	// Token: 0x06002264 RID: 8804 RVA: 0x000A165C File Offset: 0x0009F85C
	public bool TryLoadSceneDataContentByName(string contentName, out SceneWgoContentData sceneWgoContentData)
	{
		sceneWgoContentData = null;
		AssetReference contentDataRef = this.GetContentDataRef(contentName);
		return contentDataRef != null && this.TryLoadSceneWgoContentData(contentDataRef, out sceneWgoContentData);
	}

	// Token: 0x06002265 RID: 8805 RVA: 0x000A1684 File Offset: 0x0009F884
	public bool TryUnloadSceneDataContent(string contentName)
	{
		AssetReference contentDataRef = this.GetContentDataRef(contentName);
		GameObject gameObject;
		if (contentDataRef != null && this.contentDataRefInstances.TryGetValue(contentDataRef, out gameObject))
		{
			contentDataRef.ReleaseInstance(gameObject);
			this.contentDataRefInstances.Remove(contentDataRef);
			return true;
		}
		return false;
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000A16C4 File Offset: 0x0009F8C4
	public bool IsSceneContentDataLoaded(string contentName)
	{
		AssetReference contentDataRef = this.GetContentDataRef(contentName);
		return this.IsSceneContentDataLoaded(contentDataRef);
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000A16E0 File Offset: 0x0009F8E0
	public void UnloadSceneDataContents()
	{
		if (this.contentDataRefInstances != null)
		{
			foreach (KeyValuePair<AssetReference, GameObject> keyValuePair in this.contentDataRefInstances)
			{
				keyValuePair.Key.ReleaseInstance(keyValuePair.Value);
			}
			this.contentDataRefInstances.Clear();
		}
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000A1754 File Offset: 0x0009F954
	public void UnloadSceneDataContentByObject(GameObject contentObject)
	{
		if (contentObject == null)
		{
			return;
		}
		AssetReference key = this.contentDataRefInstances.FirstOrDefault((KeyValuePair<AssetReference, GameObject> x) => x.Value == contentObject).Key;
		if (key != null)
		{
			key.ReleaseInstance(contentObject);
			this.contentDataRefInstances.Remove(key);
		}
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x000A17BC File Offset: 0x0009F9BC
	public AssetReference GetContentDataRef(string contentName)
	{
		contentName += "Data";
		int num = this.assetNames.FindIndex((string c) => c == contentName);
		if (num <= -1)
		{
			return null;
		}
		return this.contentDataRefs[num];
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x000A1818 File Offset: 0x0009FA18
	public AssetReference GetContentDataRefByGuid(string assetGuid)
	{
		if (string.IsNullOrEmpty(assetGuid) || this.contentDataRefs == null)
		{
			return null;
		}
		for (int i = 0; i < this.contentDataRefs.Count; i++)
		{
			AssetReference assetReference = this.contentDataRefs[i];
			if (assetReference != null && assetReference.AssetGUID == assetGuid)
			{
				return assetReference;
			}
		}
		if (this.assetGuids != null)
		{
			int num = this.assetGuids.FindIndex((string guid) => guid == assetGuid);
			if (num > -1 && num < this.contentDataRefs.Count)
			{
				return this.contentDataRefs[num];
			}
		}
		return null;
	}

	// Token: 0x0600226B RID: 8811 RVA: 0x000A18C4 File Offset: 0x0009FAC4
	public bool TryLoadSceneDataContentByRef(AssetReference contentDataRef, out SceneWgoContentData sceneWgoContentData)
	{
		return this.TryLoadSceneWgoContentData(contentDataRef, out sceneWgoContentData);
	}

	// Token: 0x0600226C RID: 8812 RVA: 0x000A18D0 File Offset: 0x0009FAD0
	public bool IsSceneContentDataLoaded(AssetReference contentDataRef)
	{
		GameObject gameObject;
		return this.TryGetLoadedContentInstance(contentDataRef, out gameObject);
	}

	// Token: 0x0600226D RID: 8813 RVA: 0x000A18E8 File Offset: 0x0009FAE8
	public bool TryUnloadSceneDataContent(AssetReference contentDataRef)
	{
		GameObject gameObject;
		if (!this.TryGetLoadedContentInstance(contentDataRef, out gameObject))
		{
			return false;
		}
		AssetReference loadedContentDictionaryKey = this.GetLoadedContentDictionaryKey(contentDataRef);
		if (loadedContentDictionaryKey == null)
		{
			return false;
		}
		loadedContentDictionaryKey.ReleaseInstance(gameObject);
		this.contentDataRefInstances.Remove(loadedContentDictionaryKey);
		return true;
	}

	// Token: 0x0600226E RID: 8814 RVA: 0x000A1924 File Offset: 0x0009FB24
	public bool TryGetLoadedContent(string contentName, out GameObject contentDataInstance)
	{
		contentDataInstance = null;
		AssetReference contentDataRef = this.GetContentDataRef(contentName);
		return contentDataRef != null && this.contentDataRefInstances.TryGetValue(contentDataRef, out contentDataInstance);
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x000A1950 File Offset: 0x0009FB50
	private bool TryGetLoadedContentInstance(AssetReference contentDataRef, out GameObject contentDataInstance)
	{
		contentDataInstance = null;
		if (contentDataRef == null || this.contentDataRefInstances == null)
		{
			return false;
		}
		if (this.contentDataRefInstances.TryGetValue(contentDataRef, out contentDataInstance))
		{
			return contentDataInstance != null;
		}
		AssetReference loadedContentDictionaryKey = this.GetLoadedContentDictionaryKey(contentDataRef);
		return loadedContentDictionaryKey != null && this.contentDataRefInstances.TryGetValue(loadedContentDictionaryKey, out contentDataInstance) && contentDataInstance != null;
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000A19AC File Offset: 0x0009FBAC
	private AssetReference GetLoadedContentDictionaryKey(AssetReference contentDataRef)
	{
		if (contentDataRef == null || this.contentDataRefInstances == null)
		{
			return null;
		}
		if (this.contentDataRefInstances.ContainsKey(contentDataRef))
		{
			return contentDataRef;
		}
		string assetGUID = contentDataRef.AssetGUID;
		if (string.IsNullOrEmpty(assetGUID))
		{
			return null;
		}
		foreach (KeyValuePair<AssetReference, GameObject> keyValuePair in this.contentDataRefInstances)
		{
			if (keyValuePair.Key != null && keyValuePair.Key.AssetGUID == assetGUID)
			{
				return keyValuePair.Key;
			}
		}
		return this.GetContentDataRefByGuid(assetGUID);
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x000A1A58 File Offset: 0x0009FC58
	private bool TryLoadSceneWgoContentData(AssetReference contentDataRef, out SceneWgoContentData sceneWgoContentData)
	{
		sceneWgoContentData = null;
		if (this.contentDataRefInstances == null)
		{
			this.contentDataRefInstances = new Dictionary<AssetReference, GameObject>();
		}
		GameObject gameObject;
		if (this.contentDataRefInstances.TryGetValue(contentDataRef, out gameObject))
		{
			sceneWgoContentData = gameObject.GetComponent<SceneWgoContentData>();
			return true;
		}
		AsyncOperationHandle<GameObject> asyncOperationHandle = contentDataRef.InstantiateAsync(null, false);
		asyncOperationHandle.WaitForCompletion();
		this.contentDataRefInstances.Add(contentDataRef, asyncOperationHandle.Result);
		asyncOperationHandle.Result.transform.position = Vector3.zero;
		asyncOperationHandle.Result.SetActive(true);
		return asyncOperationHandle.Result.TryGetComponent<SceneWgoContentData>(out sceneWgoContentData);
	}

	// Token: 0x04001EEB RID: 7915
	public const string GAME_SCENE_CONFIG_PATH = "Assets/AddressableAssets/SceneContents/_GameSceneConfigs";

	// Token: 0x04001EEC RID: 7916
	public const string DEV_GAME_SCENE_CONFIG_PATH = "Assets/AddressableAssets/SceneContentsDev/_GameSceneConfigs";

	// Token: 0x04001EED RID: 7917
	private const string SCENE_CONTENT_DATA_REF_PATH = "Assets/AddressableAssets/SceneContents/WgoContents";

	// Token: 0x04001EEE RID: 7918
	private const string DEV_SCENE_CONTENT_DATA_REF_PATH = "Assets/AddressableAssets/SceneContentsDev/WgoContents";

	// Token: 0x04001EEF RID: 7919
	private const string SCENE_DATA_CONTENT_DATA_REF_PATH = "Assets/AddressableAssets/SceneContents/WgoContentsData";

	// Token: 0x04001EF0 RID: 7920
	private const string DEV_SCENE_DATA_CONTENT_DATA_REF_PATH = "Assets/AddressableAssets/SceneContentsDev/WgoContentsData";

	// Token: 0x04001EF1 RID: 7921
	private const string SCENE_WAYPOINT_CONTENT_REF_PATH = "Assets/AddressableAssets/SceneContents/WaypointContents";

	// Token: 0x04001EF2 RID: 7922
	private const string DEV_SCENE_WAYPOINT_CONTENT_REF_PATH = "Assets/AddressableAssets/SceneContentsDev/WaypointContents";

	// Token: 0x04001EF3 RID: 7923
	public const string FIGHTING_LEVEL_CONTENT_REF_PATH = "Assets/AddressableAssets/Fighting/Levels/Contents";

	// Token: 0x04001EF4 RID: 7924
	private const string CONFIG_ADDRESSABLES_LABEL = "GameSceneConfigs";

	// Token: 0x04001EF5 RID: 7925
	public Vector3 sceneGlobalPosition;

	// Token: 0x04001EF6 RID: 7926
	public List<AssetReference> contentDataRefs;

	// Token: 0x04001EF7 RID: 7927
	public List<bool> autoLoadFlags;

	// Token: 0x04001EF8 RID: 7928
	public List<string> assetNames = new List<string>();

	// Token: 0x04001EF9 RID: 7929
	public List<string> assetGuids = new List<string>();

	// Token: 0x04001EFA RID: 7930
	[SerializeField]
	private GDPointData[] gdPointsData;

	// Token: 0x04001EFB RID: 7931
	[SerializeField]
	private List<IndoorAreaData> indoorAreas = new List<IndoorAreaData>();

	// Token: 0x04001EFC RID: 7932
	public bool disableUnloadOnTeleport;

	// Token: 0x04001EFD RID: 7933
	private Dictionary<AssetReference, GameObject> contentDataRefInstances = new Dictionary<AssetReference, GameObject>();
}
