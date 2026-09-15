using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001C2 RID: 450
[Serializable]
public class WorldZoneData : ObjectLinkedToDefinition<WorldZoneDef>
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000B6B RID: 2923 RVA: 0x00038F78 File Offset: 0x00037178
	// (remove) Token: 0x06000B6C RID: 2924 RVA: 0x00038FB0 File Offset: 0x000371B0
	public event Action OnWgoDataChanged;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000B6D RID: 2925 RVA: 0x00038FE8 File Offset: 0x000371E8
	// (remove) Token: 0x06000B6E RID: 2926 RVA: 0x00039020 File Offset: 0x00037220
	public event Action<WgoData> OnWgoDataAdded;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000B6F RID: 2927 RVA: 0x00039058 File Offset: 0x00037258
	// (remove) Token: 0x06000B70 RID: 2928 RVA: 0x00039090 File Offset: 0x00037290
	public event Action<WgoData> OnWgoDataRemoved;

	// Token: 0x14000007 RID: 7
	// (add) Token: 0x06000B71 RID: 2929 RVA: 0x000390C8 File Offset: 0x000372C8
	// (remove) Token: 0x06000B72 RID: 2930 RVA: 0x00039100 File Offset: 0x00037300
	public event Action<WgoData> OnWgoDataToCustomQualityAdded;

	// Token: 0x14000008 RID: 8
	// (add) Token: 0x06000B73 RID: 2931 RVA: 0x00039138 File Offset: 0x00037338
	// (remove) Token: 0x06000B74 RID: 2932 RVA: 0x00039170 File Offset: 0x00037370
	public event Action<WgoData> OnWgoDataFromCustomQualityRemoved;

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06000B75 RID: 2933 RVA: 0x000391A8 File Offset: 0x000373A8
	// (remove) Token: 0x06000B76 RID: 2934 RVA: 0x000391E0 File Offset: 0x000373E0
	public event Action<OrderBase> OnOrderAdded;

	// Token: 0x1400000A RID: 10
	// (add) Token: 0x06000B77 RID: 2935 RVA: 0x00039218 File Offset: 0x00037418
	// (remove) Token: 0x06000B78 RID: 2936 RVA: 0x00039250 File Offset: 0x00037450
	public event Action<OrderBase> OnOrderRemoved;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x06000B79 RID: 2937 RVA: 0x00039288 File Offset: 0x00037488
	// (remove) Token: 0x06000B7A RID: 2938 RVA: 0x000392C0 File Offset: 0x000374C0
	public event Action<bool> OnActiveStateChanged;

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06000B7B RID: 2939 RVA: 0x000392F5 File Offset: 0x000374F5
	public List<WgoData> MultiInventoryWgoDatas
	{
		get
		{
			return this.multiInventoryWgoDatas;
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000B7C RID: 2940 RVA: 0x000392FD File Offset: 0x000374FD
	public List<Rect> CustomQualityZonesRectList
	{
		get
		{
			return this.customQualityZonesRectList;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x06000B7D RID: 2941 RVA: 0x00039305 File Offset: 0x00037505
	public Vector3 Center
	{
		get
		{
			return new Vector3(this.wholeZoneRect.center.x, this.pos.y, this.wholeZoneRect.center.y);
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x06000B7E RID: 2942 RVA: 0x00039337 File Offset: 0x00037537
	public bool IsContainer
	{
		get
		{
			return this.worldZoneType == WorldZoneData.WorldZoneType.Default;
		}
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000B7F RID: 2943 RVA: 0x00039342 File Offset: 0x00037542
	public List<LazyConsts.Navigation.Graph> MovementGraphs
	{
		get
		{
			return this.movementGraphs;
		}
	}

	// Token: 0x170001F1 RID: 497
	// (get) Token: 0x06000B80 RID: 2944 RVA: 0x0003934A File Offset: 0x0003754A
	// (set) Token: 0x06000B81 RID: 2945 RVA: 0x00039352 File Offset: 0x00037552
	public bool IsActive
	{
		get
		{
			return this.isActive;
		}
		set
		{
			if (this.isActive == value)
			{
				return;
			}
			this.isActive = value;
			Action<bool> onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged(this.isActive);
		}
	}

	// Token: 0x170001F2 RID: 498
	// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0003937B File Offset: 0x0003757B
	// (set) Token: 0x06000B83 RID: 2947 RVA: 0x00039384 File Offset: 0x00037584
	public int AdditionalQuality
	{
		get
		{
			return this.additionalQuality;
		}
		set
		{
			if (this.additionalQuality != value)
			{
				this.additionalQuality = value;
				PlayerData playerData = MainGame.PlayerData;
				if (playerData.CurrentWorldZoneData != null && playerData.CurrentWorldZoneData.Definition.id == this.id)
				{
					GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
				}
				else
				{
					this.GetTotalQuality();
				}
			}
			this.TryCallOnMaxQualityChangedExpressions();
		}
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x000393F0 File Offset: 0x000375F0
	public WorldZoneData()
	{
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x00039474 File Offset: 0x00037674
	public WorldZoneData(string id, string gameSceneId, Vector3 pos, Rect wholeZoneRect)
		: base(id)
	{
		this.gameSceneId = gameSceneId;
		this.pos = pos;
		this.wholeZoneRect = wholeZoneRect;
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x00039510 File Offset: 0x00037710
	public WorldZoneData(WorldZoneData other)
		: base(other.id)
	{
		this.gameSceneId = other.gameSceneId;
		this.contentPartName = other.contentPartName;
		this.pos = other.pos;
		this.worldZoneType = other.worldZoneType;
		this.navigationGraph = other.navigationGraph;
		this.additionalMovementGraphs = other.additionalMovementGraphs;
		this.wholeZoneRect = other.wholeZoneRect;
		this.elevationAreas = ((other.elevationAreas != null) ? new List<WorldZoneElevationAreaBakedData>(other.elevationAreas) : new List<WorldZoneElevationAreaBakedData>());
		this.navigationHoles = ((other.navigationHoles != null) ? new List<WorldZoneNavigationHoleBakedData>(other.navigationHoles) : new List<WorldZoneNavigationHoleBakedData>());
		this.isActive = other.isActive;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0003963C File Offset: 0x0003783C
	public static WorldZoneData CreateFromBaked(WorldZoneBakedData baked, string gameSceneId, Vector3 offset)
	{
		if (baked == null)
		{
			return null;
		}
		Rect rect = baked.wholeZoneRect;
		rect.center += offset.XZ2();
		WorldZoneData worldZoneData = new WorldZoneData(baked.id, gameSceneId, baked.pos + offset, rect)
		{
			contentPartName = baked.contentPartName,
			worldZoneType = baked.worldZoneType,
			navigationGraph = baked.navigationGraph,
			additionalMovementGraphs = baked.additionalMovementGraphs,
			processingPriority = baked.processingPriority
		};
		if (baked.elevationAreas != null && baked.elevationAreas.Count > 0)
		{
			worldZoneData.elevationAreas = new List<WorldZoneElevationAreaBakedData>(baked.elevationAreas.Count);
			Vector2 vector = offset.XZ2();
			for (int i = 0; i < baked.elevationAreas.Count; i++)
			{
				WorldZoneElevationAreaBakedData worldZoneElevationAreaBakedData = baked.elevationAreas[i];
				Rect xzRect = worldZoneElevationAreaBakedData.xzRect;
				xzRect.center += vector;
				worldZoneData.elevationAreas.Add(new WorldZoneElevationAreaBakedData
				{
					xzRect = xzRect,
					elevationY = worldZoneElevationAreaBakedData.elevationY
				});
			}
		}
		if (baked.navigationHoles != null && baked.navigationHoles.Count > 0)
		{
			worldZoneData.navigationHoles = new List<WorldZoneNavigationHoleBakedData>(baked.navigationHoles.Count);
			for (int j = 0; j < baked.navigationHoles.Count; j++)
			{
				worldZoneData.navigationHoles.Add(baked.navigationHoles[j].WithOffset(offset));
			}
		}
		return worldZoneData;
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x000397D8 File Offset: 0x000379D8
	public bool TryGetBuildElevationY(float x, float z, out float elevationY)
	{
		Vector2 vector = new Vector2(x, z);
		float num = float.MinValue;
		bool flag = false;
		if (this.elevationAreas != null)
		{
			for (int i = 0; i < this.elevationAreas.Count; i++)
			{
				WorldZoneElevationAreaBakedData worldZoneElevationAreaBakedData = this.elevationAreas[i];
				if (worldZoneElevationAreaBakedData.ContainsXZ(vector) && (!flag || worldZoneElevationAreaBakedData.elevationY > num))
				{
					num = worldZoneElevationAreaBakedData.elevationY;
					flag = true;
				}
			}
		}
		elevationY = num;
		return flag;
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x00039848 File Offset: 0x00037A48
	public void Init(BoxCollider zoneCollider)
	{
		if (zoneCollider == null)
		{
			Debug.LogError("ZoneCollider is null, that won't be");
			return;
		}
		this.EnsureCollectionsInitialized();
		Vector3 vector = zoneCollider.transform.TransformPoint(zoneCollider.center);
		Vector3 size = zoneCollider.size;
		this.wholeZoneRect = new Rect(new Vector2(vector.x - size.x / 2f, vector.z - size.z / 2f), new Vector2(size.x, size.z));
		this.movementGraphs.Clear();
		this.movementGraphs.Add(this.navigationGraph);
		this.movementGraphs.AddRange(this.additionalMovementGraphs);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x000398FC File Offset: 0x00037AFC
	public void PrepareForGame()
	{
		this.EnsureCollectionsInitialized();
		this.isActive = true;
		if (this.IsContainer)
		{
			if (this.navigationGraph != LazyConsts.Navigation.Graph.None)
			{
				LazySingleton<GlobalNavigationManager>.Instance.InitRecastGraph(this.navigationGraph, this.Center, this.wholeZoneRect.size, false, 1f, this.navigationHoles);
			}
			foreach (SGuid sguid in this.wgoDataList)
			{
				WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
				if (wgoData != null)
				{
					WGODef definition = wgoData.Definition;
					if (definition != null && definition.forceSetNavigationHoleType == WGODef.ForceSetNavigationHoleType.InsideWorldZone)
					{
						this.AddCutUnitByBakedData(wgoData);
					}
				}
			}
			if (this.multiInventoryWgoDatas == null || (this.multiInventoryWgoDatas.Count == 0 && this.wgoDataList.Count > 0))
			{
				this.multiInventoryWgoDatas = new List<WgoData>();
				foreach (SGuid sguid2 in this.wgoDataList)
				{
					WgoData wgoData2 = MainGame.Instance.GameSave.worldData.GetWgoData(sguid2);
					if (wgoData2 != null)
					{
						WGODef definition2 = wgoData2.Definition;
						if (definition2 != null && definition2.OpenInMultiInventory)
						{
							this.multiInventoryWgoDatas.Add(wgoData2);
						}
					}
				}
			}
			return;
		}
		this.wgoDataList.Clear();
		this.customQualityWgoDataList.Clear();
		List<WgoData> list = this.multiInventoryWgoDatas;
		if (list == null)
		{
			return;
		}
		list.Clear();
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x00039AA0 File Offset: 0x00037CA0
	public void NotifyWgoDataChanged()
	{
		Action onWgoDataChanged = this.OnWgoDataChanged;
		if (onWgoDataChanged == null)
		{
			return;
		}
		onWgoDataChanged();
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x00039AB4 File Offset: 0x00037CB4
	public bool TryAddWgoData(WgoData wgoData)
	{
		if (!this.IsContainer || !this.IsActive)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		Vector2 vector = new Vector2(wgoData.Position.x, wgoData.Position.z);
		if (this.wholeZoneRect.Contains(vector))
		{
			this.wgoDataList.Add(wgoData.UniqueId);
			if (this.multiInventoryWgoDatas != null && wgoData.Definition.OpenInMultiInventory)
			{
				this.multiInventoryWgoDatas.Add(wgoData);
			}
			Action<WgoData> onWgoDataAdded = this.OnWgoDataAdded;
			if (onWgoDataAdded != null)
			{
				onWgoDataAdded(wgoData);
			}
			wgoData.WorldZoneData = this;
			flag = true;
			if (wgoData.Definition != null && wgoData.Definition.forceSetNavigationHoleType != WGODef.ForceSetNavigationHoleType.DontSpawnHole)
			{
				this.AddCutUnitByBakedData(wgoData);
			}
			if (GardenBedNavigation.IsGardenPlot(wgoData))
			{
				GardenBedNavigation.TryRebuild(wgoData);
			}
		}
		if (base.Definition != null && base.Definition.hasCustomQualityZones && this.ContainsPointCustomQualityZonesRoughly(vector) && this.ContainsCustomQualityZonePrecisely(wgoData.Position))
		{
			this.customQualityWgoDataList.Add(wgoData.UniqueId);
			flag2 = true;
		}
		this.TryCallOnMaxQualityChangedExpressions();
		if (flag)
		{
			Action onWgoDataChanged = this.OnWgoDataChanged;
			if (onWgoDataChanged != null)
			{
				onWgoDataChanged();
			}
		}
		if (flag2)
		{
			Action<WgoData> onWgoDataToCustomQualityAdded = this.OnWgoDataToCustomQualityAdded;
			if (onWgoDataToCustomQualityAdded != null)
			{
				onWgoDataToCustomQualityAdded(wgoData);
			}
		}
		return flag;
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x00039BE8 File Offset: 0x00037DE8
	public List<WgoData> GetWgoDataByRect(Rect rect)
	{
		List<WgoData> list = new List<WgoData>();
		for (int i = 0; i < this.wgoDataList.Count; i++)
		{
			SGuid sguid = this.wgoDataList[i];
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null && ((wgoData != null) ? wgoData.MainWgoPartData : null) != null)
			{
				Rect collisionBoundsRect = wgoData.MainWgoPartData.GetCollisionBoundsRect(wgoData.Position);
				if (rect.Overlaps(collisionBoundsRect, true))
				{
					list.Add(wgoData);
				}
			}
		}
		return list;
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x00039C6C File Offset: 0x00037E6C
	private void AddCutUnitByBakedData(WgoData wgoData)
	{
		if (((wgoData != null) ? wgoData.MainWgoPartData : null) == null)
		{
			return;
		}
		int stateHash = wgoData.MainWgoPartData.GetStateHash();
		WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData;
		if (wgoData.MainWgoPartData.BakedData.TryGetGraphUpdateSceneBoxData(stateHash, out graphUpdateSceneBoxData))
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddGraphSceneUpdateUnit(wgoData.UniqueId, this.navigationGraph, wgoData.Position, graphUpdateSceneBoxData);
		}
		WgoPartBakedData.PlannerMeshData plannerMeshData;
		if (wgoData.MainWgoPartData.BakedData.TryGetPlannerMeshData(stateHash, out plannerMeshData))
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddCutUnit(wgoData.UniqueId, this.navigationGraph, wgoData.Position, plannerMeshData);
		}
		else
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddCutUnit(wgoData.UniqueId, this.navigationGraph, wgoData.MainWgoPartData.GetCollisionBoundsRect(wgoData.Position), wgoData.Position.y);
		}
		wgoData.BeginCustomNavMeshCutTracking();
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x00039D34 File Offset: 0x00037F34
	public void RemoveWgoData(WgoData wgoData)
	{
		if (this.wgoDataList.Remove(wgoData.UniqueId))
		{
			LazySingleton<GlobalNavigationManager>.Instance.RemoveCutUnit(wgoData.UniqueId);
			wgoData.StopCustomNavMeshCutTracking();
			if (this.customQualityWgoDataList.Remove(wgoData.UniqueId))
			{
				Action<WgoData> onWgoDataFromCustomQualityRemoved = this.OnWgoDataFromCustomQualityRemoved;
				if (onWgoDataFromCustomQualityRemoved != null)
				{
					onWgoDataFromCustomQualityRemoved(wgoData);
				}
			}
			wgoData.WorldZoneData = null;
			Action<WgoData> onWgoDataRemoved = this.OnWgoDataRemoved;
			if (onWgoDataRemoved != null)
			{
				onWgoDataRemoved(wgoData);
			}
			Action onWgoDataChanged = this.OnWgoDataChanged;
			if (onWgoDataChanged != null)
			{
				onWgoDataChanged();
			}
			if (wgoData.Definition.OpenInMultiInventory)
			{
				this.multiInventoryWgoDatas.Remove(wgoData);
			}
		}
		this.TryCallOnMaxQualityChangedExpressions();
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x00039DDC File Offset: 0x00037FDC
	public void AddPlayerData(PlayerData playerData)
	{
		this.playerDataList.Add(playerData);
		foreach (LazyExpression lazyExpression in base.Definition.onEnterExpressions)
		{
			lazyExpression.Evaluate();
		}
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x00039E40 File Offset: 0x00038040
	public void RemovePlayerData(PlayerData playerData)
	{
		this.playerDataList.Remove(playerData);
		foreach (LazyExpression lazyExpression in base.Definition.onExitExpressions)
		{
			lazyExpression.Evaluate();
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x00039EA4 File Offset: 0x000380A4
	public float GetTotalQuality()
	{
		if (!this.IsContainer)
		{
			return 0f;
		}
		float num = 0f;
		foreach (SGuid sguid in ((!base.Definition.hasCustomQualityZones) ? this.wgoDataList : this.customQualityWgoDataList))
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null)
			{
				num += wgoData.Quality;
			}
		}
		float num2 = num + (float)this.additionalQuality;
		if (MainGame.Instance.gameState == MainGame.GameState.InGame && MainGame.PlayerData != null)
		{
			MainGame.PlayerData.SetResWithoutSystemsCheck("wz_" + this.id, num2);
		}
		this.TryUnlockGraveyardQualityAchievement(num2);
		return num2;
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x00039F80 File Offset: 0x00038180
	public int CountItemsOnTownPalettes(string itemId)
	{
		int num = 0;
		for (int i = 0; i < this.wgoDataList.Count; i++)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoDataList[i]);
			if (wgoData != null && wgoData.Definition.interactionType == WGODef.InteractionType.TownPalette)
			{
				num += wgoData.Inventory.Data.GetTotalCountInInventory(itemId, null, false);
			}
		}
		return num;
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x00039FF0 File Offset: 0x000381F0
	public float GetTotalQuality(WorldZoneWgoQualityType qualityType)
	{
		if (!this.IsContainer)
		{
			return 0f;
		}
		float num = 0f;
		foreach (SGuid sguid in ((!base.Definition.hasCustomQualityZones) ? this.wgoDataList : this.customQualityWgoDataList))
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null)
			{
				switch (qualityType)
				{
				case WorldZoneWgoQualityType.Any:
					num += wgoData.Quality;
					break;
				case WorldZoneWgoQualityType.ConveyorCells:
					if (wgoData.Quality < 0f)
					{
						num += Mathf.Abs(wgoData.Quality);
					}
					break;
				case WorldZoneWgoQualityType.ConveyorPowerSource:
					if (wgoData.Quality >= 0f)
					{
						num += wgoData.Quality;
					}
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0003A0D4 File Offset: 0x000382D4
	public void AddCustomQualityRect(Rect rect)
	{
		if (!base.Definition.hasCustomQualityZones)
		{
			return;
		}
		bool flag = false;
		this.customQualityZonesRectList.Add(rect);
		Debug.Log(string.Format("WorldZoneData.AddRect: added rect {0}", rect));
		GameSceneData gameSceneDataById = MainGame.Instance.GameSave.worldData.GetGameSceneDataById(this.gameSceneId);
		if (gameSceneDataById == null)
		{
			return;
		}
		foreach (WgoData wgoData in gameSceneDataById.wgoDataList)
		{
			Vector2 vector = new Vector2(wgoData.Position.x, wgoData.Position.z);
			if (rect.Contains(vector))
			{
				this.customQualityWgoDataList.Add(wgoData.UniqueId);
				Action<WgoData> onWgoDataToCustomQualityAdded = this.OnWgoDataToCustomQualityAdded;
				if (onWgoDataToCustomQualityAdded != null)
				{
					onWgoDataToCustomQualityAdded(wgoData);
				}
				Debug.Log("WorldZoneData.AddRect: added CustomQualityWgoData " + wgoData.id);
				flag = true;
			}
		}
		this.FormCustomQualityZonesRoughRect();
		if (flag)
		{
			Action onWgoDataChanged = this.OnWgoDataChanged;
			if (onWgoDataChanged != null)
			{
				onWgoDataChanged();
			}
			this.TryCallOnMaxQualityChangedExpressions();
		}
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x0003A1F4 File Offset: 0x000383F4
	public bool RemoveCustomQualityRect(Rect rect)
	{
		if (!base.Definition.hasCustomQualityZones)
		{
			return false;
		}
		int num = this.customQualityZonesRectList.FindIndex((Rect r) => WorldZoneData.AreRectsEqual(r, rect));
		if (num < 0)
		{
			return false;
		}
		this.customQualityZonesRectList.RemoveAt(num);
		this.RebuildCustomQualityWgoDataList();
		Action onWgoDataChanged = this.OnWgoDataChanged;
		if (onWgoDataChanged != null)
		{
			onWgoDataChanged();
		}
		this.TryCallOnMaxQualityChangedExpressions();
		Debug.Log(string.Format("WorldZoneData.RemoveRect: removed rect {0}", rect));
		return true;
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x0003A280 File Offset: 0x00038480
	public string GetQualityString(TextStyle customQualityStyle = null)
	{
		if (base.Definition == null)
		{
			Debug.LogError("WorldZone [" + this.id + "] Definition is null");
			return string.Empty;
		}
		string text = base.Definition.stringFormat.Replace("@", base.Definition.qualityIcon.FontIcon());
		Regex regex = new Regex("^(.*?)\\{\\$([a-zA-Z0-9_]+):([^\\}]+)\\}(.*?)$");
		for (;;)
		{
			Match match = regex.Match(text);
			if (!match.Success)
			{
				break;
			}
			string text2 = match.Groups[2].Captures[0].ToString();
			Capture capture = match.Groups[1].Captures[0];
			string text3 = ((capture != null) ? capture.ToString() : null);
			string text4 = "{0:";
			Capture capture2 = match.Groups[3].Captures[0];
			string text5 = string.Format(text4 + ((capture2 != null) ? capture2.ToString() : null) + "}", MainGame.PlayerController.PlayerData.GetRes(text2, 0f));
			Capture capture3 = match.Groups[4].Captures[0];
			text = text3 + text5 + ((capture3 != null) ? capture3.ToString() : null);
		}
		Match match2 = new Regex("(.*)%([a-zA-Z_]+)(.*)").Match(text);
		if (match2.Success)
		{
			WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.WorldData.GetWorldZoneDataById(match2.Groups[2].Captures[0].ToString());
			if (worldZoneDataById != null)
			{
				Capture capture4 = match2.Groups[1].Captures[0];
				string text6 = ((capture4 != null) ? capture4.ToString() : null);
				string text7 = worldZoneDataById.GetTotalQuality().ToString();
				Capture capture5 = match2.Groups[3].Captures[0];
				text = text6 + text7 + ((capture5 != null) ? capture5.ToString() : null);
			}
		}
		Regex regex2 = new Regex("(.*)LE\\{([^\\}]+)\\}(.*)");
		for (;;)
		{
			Match match3 = regex2.Match(text);
			if (!match3.Success)
			{
				break;
			}
			LazyExpression lazyExpression = LazyExpressionBase.ParseExpression<LazyExpression>(match3.Groups[2].Captures[0].ToString());
			Capture capture6 = match3.Groups[1].Captures[0];
			string text8 = ((capture6 != null) ? capture6.ToString() : null);
			string text9 = lazyExpression.EvaluateFloat(this).ToString();
			Capture capture7 = match3.Groups[3].Captures[0];
			text = text8 + text9 + ((capture7 != null) ? capture7.ToString() : null);
		}
		text = string.Format(text, this.GetTotalQuality());
		if (customQualityStyle != null)
		{
			text = customQualityStyle.ApplyStyleToString(text, false, true);
		}
		return text;
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x0003A53C File Offset: 0x0003873C
	public bool CanDeliveryOrderBeTakenOnExecution(DeliveryOrder deliveryOrder, int currentCount = 0)
	{
		int num = 0;
		foreach (WgoData wgoData in this.multiInventoryWgoDatas)
		{
			if (wgoData.Definition.inventoryWhiteList.Contains(deliveryOrder.Item.Definition) && !wgoData.Definition.inventoryBlackList.Contains(deliveryOrder.Item.Definition))
			{
				num += wgoData.Inventory.Data.GetTotalCountInInventory(deliveryOrder.Item.id, null, false);
				if (num >= deliveryOrder.Item.Count - currentCount)
				{
					return true;
				}
			}
		}
		return num >= deliveryOrder.Item.Count - currentCount;
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0003A610 File Offset: 0x00038810
	public OrderBase GetOrderForCaretaker(Item executorCurrentItem = null)
	{
		if (this.orders == null || this.orders.Count == 0)
		{
			return null;
		}
		OrderBase orderBase = null;
		int num = int.MinValue;
		foreach (OrderBase orderBase2 in this.orders)
		{
			if (orderBase2.ExecutorUniqueId.IsEmpty && !(orderBase2 is ConveyorPickupOrder))
			{
				DeliveryOrder deliveryOrder = orderBase2 as DeliveryOrder;
				if (deliveryOrder == null || this.CanDeliveryOrderBeTakenOnExecution(deliveryOrder, (executorCurrentItem == null) ? 0 : ((executorCurrentItem.id == deliveryOrder.Item.id) ? executorCurrentItem.Count : 0)))
				{
					int priority = orderBase2.GetPriority();
					if (priority > num)
					{
						num = priority;
						orderBase = orderBase2;
					}
				}
			}
		}
		return orderBase;
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x0003A6E4 File Offset: 0x000388E4
	public OrderBase GetOrderForConveyorTransporter()
	{
		if (this.orders == null || this.orders.Count == 0)
		{
			return null;
		}
		WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById("conveyor_storage");
		if (worldZoneDataById == null)
		{
			return null;
		}
		bool flag = false;
		foreach (WgoData wgoData in worldZoneDataById.MultiInventoryWgoDatas)
		{
			if (wgoData.Inventory.Data.InventoryFillSize < wgoData.Inventory.Data.InventorySize)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return null;
		}
		OrderBase orderBase = null;
		int num = int.MinValue;
		foreach (OrderBase orderBase2 in this.orders)
		{
			if (orderBase2.ExecutorUniqueId.IsEmpty)
			{
				ConveyorPickupOrder conveyorPickupOrder = orderBase2 as ConveyorPickupOrder;
				if (conveyorPickupOrder != null)
				{
					WgoData wgoData2 = MainGame.WorldData.GetWgoData(conveyorPickupOrder.TargetWgoUniqueId);
					if (wgoData2 != null && wgoData2.Inventory.Data.GetTotalCountInInventory(conveyorPickupOrder.Item.id, null, false) >= conveyorPickupOrder.Item.Count)
					{
						int priority = orderBase2.GetPriority();
						if (priority > num)
						{
							num = priority;
							orderBase = orderBase2;
						}
					}
				}
			}
		}
		return orderBase;
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0003A84C File Offset: 0x00038A4C
	public bool CanPlantOrderBeTakenOnExecution(PlantOrder plantOrder, out string enoughItemId, int currentCount = 0)
	{
		enoughItemId = string.Empty;
		WgoData wgoData = MainGame.WorldData.GetWgoData(plantOrder.TargetWgoUniqueId);
		if (wgoData == null)
		{
			return false;
		}
		if (wgoData.CraftComponent.IsStarted)
		{
			return true;
		}
		if (!plantOrder.isStarGroupItem)
		{
			return this.HasEnoughSeedsToPlant(plantOrder.Item, out enoughItemId, currentCount);
		}
		foreach (ItemDef itemDef in GameBalance.Me.starGroupItemsCache[plantOrder.Item.id])
		{
			Item item = new Item(itemDef.id, plantOrder.Item.Count);
			if (this.HasEnoughSeedsToPlant(item, out enoughItemId, currentCount))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x0003A918 File Offset: 0x00038B18
	public bool HasEnoughSeedsToPlant(Item item, out string seedItemId, int currentCount = 0)
	{
		seedItemId = string.Empty;
		int num = 0;
		foreach (WgoData wgoData in this.multiInventoryWgoDatas)
		{
			if (wgoData.Definition.inventoryWhiteList.Contains(item.Definition) && !wgoData.Definition.inventoryBlackList.Contains(item.Definition))
			{
				num += wgoData.Inventory.Data.GetTotalCountInInventory(item.id, null, false);
				if (num >= item.Count - currentCount)
				{
					seedItemId = item.id;
					return true;
				}
			}
		}
		if (num >= item.Count - currentCount)
		{
			seedItemId = item.id;
			return true;
		}
		return false;
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0003A9E8 File Offset: 0x00038BE8
	public bool HasOtherFreeGardenerWithHigherMastery(ZombieWgoData currentGardener)
	{
		foreach (SGuid sguid in MainGame.ZombieSystemData.zombieOnSceneWgoIds)
		{
			ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(sguid);
			if (zombie != null && zombie != currentGardener && zombie.WorldZoneData.id == currentGardener.WorldZoneData.id && zombie.ZombieType == ZombieType.Gardener && zombie.GardenerState == ZombieWgoData.ZombieGardenerState.OnStation && zombie.GetMasteryLevelForTalentBranch("talent_green", null) > currentGardener.GetMasteryLevelForTalentBranch("talent_green", null))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0003AA9C File Offset: 0x00038C9C
	public OrderBase GetOrderForGardener(ZombieWgoData orderTaker, Type orderType = null, SGuid target = null)
	{
		if (this.orders == null || this.orders.Count == 0)
		{
			return null;
		}
		OrderBase orderBase = null;
		int num = int.MinValue;
		bool flag = this.HasOtherFreeGardenerWithHigherMastery(orderTaker);
		List<SGuid> list = null;
		foreach (OrderBase orderBase2 in this.orders)
		{
			if (orderBase2.ExecutorUniqueId.IsEmpty && (orderBase2 is GatherOrder || orderBase2 is PlantOrder) && (!(orderType != null) || !(orderBase2.GetType() != orderType)) && (!(target != null) || orderBase2.TargetWgoUniqueId.Equals(target)))
			{
				if (MainGame.WorldData.GetWgoData(orderBase2.TargetWgoUniqueId) == null)
				{
					if (list == null)
					{
						list = new List<SGuid>();
					}
					list.Add(orderBase2.UniqueId);
				}
				else if (!(orderBase2 is GatherOrder) || !flag)
				{
					PlantOrder plantOrder = orderBase2 as PlantOrder;
					string text;
					if (plantOrder == null || this.CanPlantOrderBeTakenOnExecution(plantOrder, out text, 0))
					{
						int priority = orderBase2.GetPriority();
						if (priority > num)
						{
							num = priority;
							orderBase = orderBase2;
						}
					}
				}
			}
		}
		if (list != null)
		{
			this.ClearOrders(list);
		}
		return orderBase;
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0003ABE0 File Offset: 0x00038DE0
	public void PlaceNewOrder(OrderBase orderBase)
	{
		this.orders.Add(orderBase);
		Action<OrderBase> onOrderAdded = this.OnOrderAdded;
		if (onOrderAdded == null)
		{
			return;
		}
		onOrderAdded(orderBase);
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x0003AC00 File Offset: 0x00038E00
	public void RemoveOrder(SGuid orderUniqueId)
	{
		OrderBase orderBase = this.FindOrder(orderUniqueId);
		if (orderBase != null)
		{
			this.orders.Remove(orderBase);
			Action<OrderBase> onOrderRemoved = this.OnOrderRemoved;
			if (onOrderRemoved == null)
			{
				return;
			}
			onOrderRemoved(orderBase);
		}
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0003AC38 File Offset: 0x00038E38
	public void RemoveOrdersByTarget(SGuid targetUniqueId)
	{
		if (this.orders == null || this.orders.Count == 0)
		{
			return;
		}
		foreach (OrderBase orderBase in this.FindOrdersByTarget(targetUniqueId, null))
		{
			if (SGuid.IsNullOrEmpty(orderBase.ExecutorUniqueId))
			{
				this.orders.Remove(orderBase);
				Action<OrderBase> onOrderRemoved = this.OnOrderRemoved;
				if (onOrderRemoved != null)
				{
					onOrderRemoved(orderBase);
				}
			}
		}
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x0003ACC8 File Offset: 0x00038EC8
	public OrderBase FindAssignedGardenOrder(SGuid targetUniqueId)
	{
		if (this.orders == null || this.orders.Count == 0)
		{
			return null;
		}
		return this.orders.Find((OrderBase o) => o.TargetWgoUniqueId == targetUniqueId && (o is PlantOrder || o is GatherOrder) && !o.ExecutorUniqueId.IsEmpty);
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x0003AD10 File Offset: 0x00038F10
	public bool HasAssignedGardenOrder(SGuid targetUniqueId)
	{
		return this.FindAssignedGardenOrder(targetUniqueId) != null;
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x0003AD1C File Offset: 0x00038F1C
	public void RemoveGardenOrdersByTarget(SGuid targetUniqueId, SGuid currentWorkerId)
	{
		if (this.orders == null || this.orders.Count == 0)
		{
			return;
		}
		foreach (OrderBase orderBase in this.FindOrdersByTarget(targetUniqueId, null))
		{
			if ((orderBase is PlantOrder || orderBase is GatherOrder) && (orderBase.ExecutorUniqueId.IsEmpty || !(orderBase.ExecutorUniqueId == currentWorkerId)))
			{
				this.orders.Remove(orderBase);
				Action<OrderBase> onOrderRemoved = this.OnOrderRemoved;
				if (onOrderRemoved != null)
				{
					onOrderRemoved(orderBase);
				}
			}
		}
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x0003ADCC File Offset: 0x00038FCC
	public void ClearOrders(List<SGuid> list)
	{
		foreach (SGuid sguid in list)
		{
			this.RemoveOrder(sguid);
		}
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x0003AE1C File Offset: 0x0003901C
	public OrderBase FindOrder(SGuid uniqueOrderId)
	{
		return this.orders.Find((OrderBase o) => o.UniqueId == uniqueOrderId);
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x0003AE50 File Offset: 0x00039050
	public List<OrderBase> FindOrdersByTarget(SGuid targetUniqueId, Type orderType = null)
	{
		if (orderType != null)
		{
			return this.orders.FindAll((OrderBase o) => o.TargetWgoUniqueId == targetUniqueId && o.GetType() == orderType);
		}
		return this.orders.FindAll((OrderBase o) => o.TargetWgoUniqueId == targetUniqueId);
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x0003AEB0 File Offset: 0x000390B0
	private void FormCustomQualityZonesRoughRect()
	{
		if (this.customQualityZonesRectList.Count == 0)
		{
			this.customQualityZonesRoughRect = Rect.zero;
			return;
		}
		Rect rect = this.customQualityZonesRectList[0];
		for (int i = 1; i < this.customQualityZonesRectList.Count; i++)
		{
			Rect rect2 = this.customQualityZonesRectList[i];
			float num = ((rect.xMin < rect2.xMin) ? rect.xMin : rect2.xMin);
			float num2 = ((rect.yMin < rect2.yMin) ? rect.yMin : rect2.yMin);
			float num3 = ((rect.xMax > rect2.xMax) ? rect.xMax : rect2.xMax);
			float num4 = ((rect.yMax > rect2.yMax) ? rect.yMax : rect2.yMax);
			float num5 = num3 - num;
			float num6 = num4 - num2;
			rect.Set(num, num2, num5, num6);
		}
		this.customQualityZonesRoughRect = rect;
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x0003AFB4 File Offset: 0x000391B4
	private void RebuildCustomQualityWgoDataList()
	{
		HashSet<SGuid> hashSet = new HashSet<SGuid>(this.customQualityWgoDataList);
		this.customQualityWgoDataList.Clear();
		HashSet<SGuid> hashSet2 = new HashSet<SGuid>();
		for (int i = 0; i < this.wgoDataList.Count; i++)
		{
			SGuid sguid = this.wgoDataList[i];
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null && this.ContainsCustomQualityZonePrecisely(wgoData.Position))
			{
				this.customQualityWgoDataList.Add(sguid);
				hashSet2.Add(sguid);
			}
		}
		foreach (SGuid sguid2 in hashSet)
		{
			WgoData wgoData2 = MainGame.Instance.GameSave.worldData.GetWgoData(sguid2);
			if (wgoData2 != null && !hashSet2.Contains(sguid2))
			{
				Action<WgoData> onWgoDataFromCustomQualityRemoved = this.OnWgoDataFromCustomQualityRemoved;
				if (onWgoDataFromCustomQualityRemoved != null)
				{
					onWgoDataFromCustomQualityRemoved(wgoData2);
				}
			}
		}
		this.FormCustomQualityZonesRoughRect();
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x0003B0BC File Offset: 0x000392BC
	private void EnsureCollectionsInitialized()
	{
		if (this.wgoDataList == null)
		{
			this.wgoDataList = new List<SGuid>();
		}
		if (this.customQualityWgoDataList == null)
		{
			this.customQualityWgoDataList = new List<SGuid>();
		}
		if (this.playerDataList == null)
		{
			this.playerDataList = new List<PlayerData>();
		}
		if (this.additionalMovementGraphs == null)
		{
			this.additionalMovementGraphs = new List<LazyConsts.Navigation.Graph>();
		}
		if (this.elevationAreas == null)
		{
			this.elevationAreas = new List<WorldZoneElevationAreaBakedData>();
		}
		if (this.navigationHoles == null)
		{
			this.navigationHoles = new List<WorldZoneNavigationHoleBakedData>();
		}
		if (this.customQualityZonesRectList == null)
		{
			this.customQualityZonesRectList = new List<Rect>();
		}
		if (this.orders == null)
		{
			this.orders = new List<OrderBase>();
		}
		if (this.movementGraphs == null)
		{
			this.movementGraphs = new List<LazyConsts.Navigation.Graph>();
		}
		if (this.multiInventoryWgoDatas == null)
		{
			this.multiInventoryWgoDatas = new List<WgoData>();
		}
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x0003B188 File Offset: 0x00039388
	private static bool AreRectsEqual(Rect a, Rect b)
	{
		return Mathf.Abs(a.x - b.x) < 0.001f && Mathf.Abs(a.y - b.y) < 0.001f && Mathf.Abs(a.width - b.width) < 0.001f && Mathf.Abs(a.height - b.height) < 0.001f;
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x0003B203 File Offset: 0x00039403
	private bool ContainsPointCustomQualityZonesRoughly(Vector2 point)
	{
		return this.customQualityZonesRectList.Count > 0 && this.customQualityZonesRoughRect.Contains(point);
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x0003B224 File Offset: 0x00039424
	private void TryCallOnMaxQualityChangedExpressions()
	{
		int num = (int)this.GetTotalQuality() - this.maxReachedQuality;
		if (num > 0)
		{
			foreach (LazyExpression lazyExpression in base.Definition.expressionsOnMaxQualityIncreased)
			{
				lazyExpression.EvaluateValueDelta(num);
			}
			this.maxReachedQuality += num;
		}
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0003B29C File Offset: 0x0003949C
	private void TryUnlockGraveyardQualityAchievement(float totalQuality)
	{
		if (this.id == "graveyard" && totalQuality >= 200f)
		{
			AchievementsSystem.Instance.Unlock("ach_graveyard_quality_200");
		}
	}

	// Token: 0x06000BAF RID: 2991 RVA: 0x0003B2C8 File Offset: 0x000394C8
	public bool ContainsCustomQualityZonePrecisely(Vector3 point)
	{
		Vector2 vector = new Vector2(point.x, point.z);
		foreach (Rect rect in this.customQualityZonesRectList)
		{
			if (rect.Contains(vector))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04000CA3 RID: 3235
	public string gameSceneId;

	// Token: 0x04000CA4 RID: 3236
	public string contentPartName;

	// Token: 0x04000CA5 RID: 3237
	public Vector3 pos;

	// Token: 0x04000CA6 RID: 3238
	public List<SGuid> wgoDataList = new List<SGuid>();

	// Token: 0x04000CA7 RID: 3239
	public List<SGuid> customQualityWgoDataList = new List<SGuid>();

	// Token: 0x04000CA8 RID: 3240
	[NonSerialized]
	public List<PlayerData> playerDataList = new List<PlayerData>();

	// Token: 0x04000CA9 RID: 3241
	public LazyConsts.Navigation.Graph navigationGraph = LazyConsts.Navigation.Graph.None;

	// Token: 0x04000CAA RID: 3242
	public List<LazyConsts.Navigation.Graph> additionalMovementGraphs = new List<LazyConsts.Navigation.Graph>();

	// Token: 0x04000CAB RID: 3243
	public Rect wholeZoneRect;

	// Token: 0x04000CAC RID: 3244
	public List<WorldZoneElevationAreaBakedData> elevationAreas = new List<WorldZoneElevationAreaBakedData>();

	// Token: 0x04000CAD RID: 3245
	public List<WorldZoneNavigationHoleBakedData> navigationHoles = new List<WorldZoneNavigationHoleBakedData>();

	// Token: 0x04000CAE RID: 3246
	public int additionalQuality;

	// Token: 0x04000CAF RID: 3247
	public WorldZoneData.WorldZoneType worldZoneType;

	// Token: 0x04000CB0 RID: 3248
	public int processingPriority;

	// Token: 0x04000CB1 RID: 3249
	[NonSerialized]
	private bool isActive = true;

	// Token: 0x04000CB2 RID: 3250
	[SerializeField]
	private int maxReachedQuality;

	// Token: 0x04000CB3 RID: 3251
	[SerializeField]
	private List<Rect> customQualityZonesRectList = new List<Rect>();

	// Token: 0x04000CB4 RID: 3252
	[SerializeField]
	private Rect customQualityZonesRoughRect;

	// Token: 0x04000CB5 RID: 3253
	[SerializeField]
	private List<OrderBase> orders = new List<OrderBase>();

	// Token: 0x04000CB6 RID: 3254
	private List<LazyConsts.Navigation.Graph> movementGraphs = new List<LazyConsts.Navigation.Graph>();

	// Token: 0x04000CB7 RID: 3255
	private List<WgoData> multiInventoryWgoDatas;

	// Token: 0x020001C3 RID: 451
	public enum WorldZoneType
	{
		// Token: 0x04000CB9 RID: 3257
		Default,
		// Token: 0x04000CBA RID: 3258
		SimpleNotContainer
	}
}
