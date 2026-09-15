using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x020001BE RID: 446
public class WorldZone : MonoBehaviour
{
	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0003841C File Offset: 0x0003661C
	public List<Wgo> Wgos
	{
		get
		{
			return this.wgos;
		}
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00038424 File Offset: 0x00036624
	// (set) Token: 0x06000B43 RID: 2883 RVA: 0x0003842C File Offset: 0x0003662C
	public int WgosVersion { get; private set; }

	// Token: 0x170001E1 RID: 481
	// (get) Token: 0x06000B44 RID: 2884 RVA: 0x00038435 File Offset: 0x00036635
	public WorldZoneData Data
	{
		get
		{
			return this.worldZoneData;
		}
	}

	// Token: 0x170001E2 RID: 482
	// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0003843D File Offset: 0x0003663D
	// (set) Token: 0x06000B46 RID: 2886 RVA: 0x00038445 File Offset: 0x00036645
	public WorldZoneBakedData BakedData
	{
		get
		{
			return this.bakedData;
		}
		set
		{
			this.bakedData = value;
		}
	}

	// Token: 0x170001E3 RID: 483
	// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0003844E File Offset: 0x0003664E
	public string Id
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x06000B48 RID: 2888 RVA: 0x00038456 File Offset: 0x00036656
	public WorldZoneData.WorldZoneType WorldZoneType
	{
		get
		{
			return this.worldZoneType;
		}
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0003845E File Offset: 0x0003665E
	public BoxCollider ZoneCollider
	{
		get
		{
			return this.zoneCollider;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00038466 File Offset: 0x00036666
	public LazyConsts.Navigation.Graph NavigationGraph
	{
		get
		{
			return this.navigationGraph;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000B4B RID: 2891 RVA: 0x0003846E File Offset: 0x0003666E
	public List<LazyConsts.Navigation.Graph> AdditionalMovementGraphs
	{
		get
		{
			return this.additionalMovementGraphs;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00038476 File Offset: 0x00036676
	public IReadOnlyList<Collider> NavigationHoleColliders
	{
		get
		{
			return this.navigationHoleColliders;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000B4D RID: 2893 RVA: 0x0003847E File Offset: 0x0003667E
	// (set) Token: 0x06000B4E RID: 2894 RVA: 0x00038486 File Offset: 0x00036686
	public HashSet<IChunkableObject> AllStaticObjectsInZone { get; set; }

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0003848F File Offset: 0x0003668F
	public int ProcessingPriority
	{
		get
		{
			return this.processingPriority;
		}
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x00038498 File Offset: 0x00036698
	public void Init(WorldZoneData worldZoneData)
	{
		this.worldZoneData = worldZoneData;
		this.worldZoneData.worldZoneType = this.worldZoneType;
		if (worldZoneData.IsContainer)
		{
			worldZoneData.OnWgoDataAdded += this.HandleWgoDataAdded;
			worldZoneData.OnWgoDataToCustomQualityAdded += this.HandleWgoDataCustomQualityZoneAdded;
			worldZoneData.OnWgoDataFromCustomQualityRemoved += this.HandleWgoDataCustomQualityZoneRemoved;
			worldZoneData.OnWgoDataRemoved += this.HandleWgoDataRemoved;
			worldZoneData.OnWgoDataChanged += this.RedrawWgosWidgets;
		}
		worldZoneData.OnActiveStateChanged += this.ApplyActiveState;
		this.ApplyActiveState(worldZoneData.IsActive);
		this.InitGDPoints();
		this.DisableNavigationHoleColliders();
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0003854C File Offset: 0x0003674C
	private void DisableNavigationHoleColliders()
	{
		if (this.navigationHoleColliders == null)
		{
			return;
		}
		for (int i = 0; i < this.navigationHoleColliders.Count; i++)
		{
			Collider collider = this.navigationHoleColliders[i];
			if (!(collider == null))
			{
				collider.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0003859C File Offset: 0x0003679C
	private void InitGDPoints()
	{
		GDPoint[] componentsInChildren = base.GetComponentsInChildren<GDPoint>(true);
		if (componentsInChildren.Length == 0)
		{
			return;
		}
		GdPointsData gdPointsData = MainGame.Instance.GameSave.worldData.gdPointsData;
		foreach (GDPoint gdpoint in componentsInChildren)
		{
			GDPointData gdpointData = gdPointsData.GetGDPointDataByView(gdpoint);
			if (gdpointData != null)
			{
				gdpoint.Init(gdpointData);
				gdpoint.gameObject.SetActive(gdpointData.Enabled);
			}
			else
			{
				gdpointData = new GDPointData(gdpoint, this.worldZoneData.gameSceneId, Vector3.zero, false);
				gdPointsData.AddScenePoint(gdpointData);
				gdpoint.Init(gdpointData);
			}
		}
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x00038638 File Offset: 0x00036838
	public void AddWgosOnGameSceneStart()
	{
		foreach (SGuid sguid in this.worldZoneData.wgoDataList)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null)
			{
				this.HandleWgoDataAdded(wgoData);
			}
		}
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x000386AC File Offset: 0x000368AC
	[CanBeNull]
	public static WorldZone Spawn(WorldZoneData data, Transform parent)
	{
		GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/WorldZones/" + data.id + ".prefab").WaitForCompletion();
		WorldZone worldZone;
		if (!gameObject || !gameObject.TryGetComponent<WorldZone>(out worldZone))
		{
			Debug.LogError("WorldZone [" + data.id + "] not found");
			return null;
		}
		WorldZone worldZone2 = global::UnityEngine.Object.Instantiate<WorldZone>(worldZone, parent);
		if (worldZone2 != null)
		{
			worldZone2.Init(data);
		}
		worldZone2.transform.position = data.pos;
		data.Init(worldZone2.ZoneCollider);
		return worldZone2;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0003873C File Offset: 0x0003693C
	private void OnDestroy()
	{
		if (this.worldZoneData == null)
		{
			return;
		}
		this.worldZoneData.OnWgoDataAdded -= this.HandleWgoDataAdded;
		this.worldZoneData.OnWgoDataToCustomQualityAdded -= this.HandleWgoDataCustomQualityZoneAdded;
		this.worldZoneData.OnWgoDataFromCustomQualityRemoved -= this.HandleWgoDataCustomQualityZoneRemoved;
		this.worldZoneData.OnWgoDataRemoved -= this.HandleWgoDataRemoved;
		this.worldZoneData.OnWgoDataChanged -= this.RedrawWgosWidgets;
		this.worldZoneData.OnActiveStateChanged -= this.ApplyActiveState;
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x000387DC File Offset: 0x000369DC
	private void ApplyActiveState(bool isActive)
	{
		if (base.gameObject.activeSelf != isActive)
		{
			base.gameObject.SetActive(isActive);
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x06000B57 RID: 2903 RVA: 0x000387F8 File Offset: 0x000369F8
	public float GroundPlaneY
	{
		get
		{
			return base.transform.position.y;
		}
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0003880A File Offset: 0x00036A0A
	public Vector3 GetBuildPos()
	{
		return VisualConsts.GetRoundedPosXZ(base.transform.position, BuildConsts.BUILD_GRID_SIZE);
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x00038824 File Offset: 0x00036A24
	public List<BuildElevationArea> GetBuildElevationAreas()
	{
		List<BuildElevationArea> list = new List<BuildElevationArea>();
		WorldZoneElevationArea[] componentsInChildren = base.GetComponentsInChildren<WorldZoneElevationArea>(true);
		if (componentsInChildren.Length != 0)
		{
			foreach (WorldZoneElevationArea worldZoneElevationArea in componentsInChildren)
			{
				if (!(worldZoneElevationArea == null) && !(worldZoneElevationArea.FootprintCollider == null))
				{
					list.Add(new BuildElevationArea(worldZoneElevationArea.GetXZRect(), worldZoneElevationArea.ElevationY, worldZoneElevationArea.GroundPlaneY));
				}
			}
			return list;
		}
		WorldZoneData worldZoneData = this.worldZoneData;
		if (((worldZoneData != null) ? worldZoneData.elevationAreas : null) != null)
		{
			float y = base.transform.position.y;
			for (int j = 0; j < this.worldZoneData.elevationAreas.Count; j++)
			{
				WorldZoneElevationAreaBakedData worldZoneElevationAreaBakedData = this.worldZoneData.elevationAreas[j];
				list.Add(new BuildElevationArea(worldZoneElevationAreaBakedData.xzRect, worldZoneElevationAreaBakedData.elevationY, y));
			}
		}
		return list;
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x00038900 File Offset: 0x00036B00
	public bool TryGetBuildElevationY(float x, float z, out float elevationY)
	{
		Vector2 vector = new Vector2(x, z);
		float num = float.MinValue;
		bool flag = false;
		foreach (WorldZoneElevationArea worldZoneElevationArea in base.GetComponentsInChildren<WorldZoneElevationArea>(true))
		{
			if (!(worldZoneElevationArea == null) && worldZoneElevationArea.ContainsXZ(vector) && (!flag || worldZoneElevationArea.ElevationY > num))
			{
				num = worldZoneElevationArea.ElevationY;
				flag = true;
			}
		}
		float num2;
		if (!flag && this.worldZoneData != null && this.worldZoneData.TryGetBuildElevationY(x, z, out num2))
		{
			num = num2;
			flag = true;
		}
		elevationY = num;
		return flag;
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x00038990 File Offset: 0x00036B90
	private void Awake()
	{
		if (this.zoneCollider == null && !base.TryGetComponent<BoxCollider>(out this.zoneCollider))
		{
			Debug.LogError("WorldZone [" + this.id + "] must have zoneCollider");
			return;
		}
		this.zoneCollider.isTrigger = true;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x000389E0 File Offset: 0x00036BE0
	public void RedrawWgosWidgets()
	{
		foreach (Wgo wgo in this.wgos)
		{
			wgo.DrawWidgets();
		}
	}

	// Token: 0x06000B5D RID: 2909 RVA: 0x00038A30 File Offset: 0x00036C30
	public void HideWgosWorldZoneWidgets()
	{
		foreach (Wgo wgo in this.wgos)
		{
			wgo.HideWorldZoneWidgets();
		}
	}

	// Token: 0x06000B5E RID: 2910 RVA: 0x00038A80 File Offset: 0x00036C80
	private void HandleWgoDataAdded(WgoData wgoData)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		if (wgoViewGlobal == null)
		{
			return;
		}
		if (this.wgos.Contains(wgoViewGlobal))
		{
			return;
		}
		if (!this.worldZoneData.Definition.hasCustomQualityZones || (this.worldZoneData.Definition.hasCustomQualityZones && this.worldZoneData.ContainsCustomQualityZonePrecisely(wgoData.Position)))
		{
			wgoViewGlobal.SetWorldZoneWidgets(this.worldZoneData);
		}
		this.wgos.Add(wgoViewGlobal);
		int wgosVersion = this.WgosVersion;
		this.WgosVersion = wgosVersion + 1;
	}

	// Token: 0x06000B5F RID: 2911 RVA: 0x00038B14 File Offset: 0x00036D14
	private void HandleWgoDataCustomQualityZoneAdded(WgoData wgoData)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		if (wgoViewGlobal == null)
		{
			return;
		}
		wgoViewGlobal.SetWorldZoneWidgets(this.worldZoneData);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x00038B44 File Offset: 0x00036D44
	private void HandleWgoDataCustomQualityZoneRemoved(WgoData wgoData)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		if (wgoViewGlobal == null)
		{
			return;
		}
		wgoViewGlobal.SetWorldZoneWidgets(null);
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x00038B70 File Offset: 0x00036D70
	private void HandleWgoDataRemoved(WgoData wgoData)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(wgoData.UniqueId);
		if (wgoViewGlobal == null)
		{
			Wgo wgo = this.wgos.Find((Wgo x) => x.Data.UniqueId == wgoData.UniqueId);
			if (wgo != null)
			{
				this.wgos.Remove(wgo);
				int num = this.WgosVersion;
				this.WgosVersion = num + 1;
			}
			return;
		}
		wgoViewGlobal.SetWorldZoneWidgets(null);
		if (this.wgos.Remove(wgoViewGlobal))
		{
			int num = this.WgosVersion;
			this.WgosVersion = num + 1;
		}
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x00038C08 File Offset: 0x00036E08
	private void OnNavGraphChanged()
	{
		if (this.Data == null)
		{
			return;
		}
		this.Data.navigationGraph = this.navigationGraph;
	}

	// Token: 0x04000C7E RID: 3198
	private const string WORLD_ZONE_PREFAB_PATH = "Assets/AddressableAssets/WorldZones";

	// Token: 0x04000C7F RID: 3199
	[SerializeField]
	private string id;

	// Token: 0x04000C80 RID: 3200
	[SerializeField]
	private WorldZoneData.WorldZoneType worldZoneType;

	// Token: 0x04000C81 RID: 3201
	[SerializeField]
	private BoxCollider zoneCollider;

	// Token: 0x04000C82 RID: 3202
	[SerializeField]
	[Tooltip("Baked data asset - created/updated during content bake")]
	private WorldZoneBakedData bakedData;

	// Token: 0x04000C83 RID: 3203
	[SerializeField]
	[Tooltip("Links this WorldZone with Astar")]
	private LazyConsts.Navigation.Graph navigationGraph = LazyConsts.Navigation.Graph.None;

	// Token: 0x04000C84 RID: 3204
	[SerializeField]
	private List<LazyConsts.Navigation.Graph> additionalMovementGraphs = new List<LazyConsts.Navigation.Graph>();

	// Token: 0x04000C85 RID: 3205
	[SerializeField]
	[Tooltip("Box colliders baked into WorldZoneBakedData as unwalkable Recast holes. Only BoxCollider is supported for now. Runtime scene objects are not required after bake.")]
	private List<Collider> navigationHoleColliders = new List<Collider>();

	// Token: 0x04000C86 RID: 3206
	[NonSerialized]
	private WorldZoneData worldZoneData;

	// Token: 0x04000C87 RID: 3207
	private List<Wgo> wgos = new List<Wgo>();

	// Token: 0x04000C88 RID: 3208
	private List<Collider> colliders = new List<Collider>();

	// Token: 0x04000C89 RID: 3209
	[SerializeField]
	private int processingPriority;
}
