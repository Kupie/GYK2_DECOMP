using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x02000699 RID: 1689
public class Wso : MonoBehaviour, IChunkableObject, IChunkVisibilityStateReceiver, IUniqueIdUser
{
	// Token: 0x1400009E RID: 158
	// (add) Token: 0x06002D3B RID: 11579 RVA: 0x000D7FC8 File Offset: 0x000D61C8
	// (remove) Token: 0x06002D3C RID: 11580 RVA: 0x000D7FFC File Offset: 0x000D61FC
	public static event Action<Wso> OnWsoSpawn;

	// Token: 0x1400009F RID: 159
	// (add) Token: 0x06002D3D RID: 11581 RVA: 0x000D8030 File Offset: 0x000D6230
	// (remove) Token: 0x06002D3E RID: 11582 RVA: 0x000D8064 File Offset: 0x000D6264
	public static event Action<Wso> OnWsoDestroy;

	// Token: 0x17000711 RID: 1809
	// (get) Token: 0x06002D3F RID: 11583 RVA: 0x000D8097 File Offset: 0x000D6297
	// (set) Token: 0x06002D40 RID: 11584 RVA: 0x000D809E File Offset: 0x000D629E
	public static Wso.RuntimeStagesLoadMode GlobalRuntimeStagesLoadMode { get; set; }

	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06002D41 RID: 11585 RVA: 0x000D80A6 File Offset: 0x000D62A6
	// (set) Token: 0x06002D42 RID: 11586 RVA: 0x000D80AE File Offset: 0x000D62AE
	public bool RegisteredInChunker
	{
		get
		{
			return this.registeredInChunker;
		}
		set
		{
			this.registeredInChunker = value;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06002D43 RID: 11587 RVA: 0x000D80B7 File Offset: 0x000D62B7
	public SGuid UniqueIdSGuid
	{
		get
		{
			return this.data.UniqueId;
		}
	}

	// Token: 0x17000714 RID: 1812
	// (get) Token: 0x06002D44 RID: 11588 RVA: 0x000D80C4 File Offset: 0x000D62C4
	string IUniqueIdUser.UniqueId
	{
		get
		{
			return this.data.UniqueId.ToString();
		}
	}

	// Token: 0x17000715 RID: 1813
	// (get) Token: 0x06002D45 RID: 11589 RVA: 0x000D80D6 File Offset: 0x000D62D6
	// (set) Token: 0x06002D46 RID: 11590 RVA: 0x000D80E9 File Offset: 0x000D62E9
	public string CustomTag
	{
		get
		{
			WsoData wsoData = this.data;
			if (wsoData == null)
			{
				return null;
			}
			return wsoData.CustomTag;
		}
		set
		{
			if (this.data != null)
			{
				this.data.CustomTag = value;
			}
		}
	}

	// Token: 0x17000716 RID: 1814
	// (get) Token: 0x06002D47 RID: 11591 RVA: 0x000D80FF File Offset: 0x000D62FF
	public WsoData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x17000717 RID: 1815
	// (get) Token: 0x06002D48 RID: 11592 RVA: 0x000D8107 File Offset: 0x000D6307
	public bool HasData
	{
		get
		{
			return this.hasData;
		}
	}

	// Token: 0x17000718 RID: 1816
	// (get) Token: 0x06002D49 RID: 11593 RVA: 0x000D810F File Offset: 0x000D630F
	public bool ShouldOptimize
	{
		get
		{
			return this.shouldOptimize;
		}
	}

	// Token: 0x17000719 RID: 1817
	// (get) Token: 0x06002D4A RID: 11594 RVA: 0x000D8117 File Offset: 0x000D6317
	public IReadOnlyList<WsoRepairableStage> RepairableStages
	{
		get
		{
			return this.repairableStages;
		}
	}

	// Token: 0x1700071A RID: 1818
	// (get) Token: 0x06002D4B RID: 11595 RVA: 0x000D811F File Offset: 0x000D631F
	public IReadOnlyList<ConstructorPart> RuntimeConstructorParts
	{
		get
		{
			return this.runtimeConstructorParts;
		}
	}

	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06002D4C RID: 11596 RVA: 0x000D8127 File Offset: 0x000D6327
	private bool IsVisible
	{
		get
		{
			if (this.isVisible)
			{
				WsoData wsoData = this.data;
				return wsoData == null || !wsoData.IsHidden;
			}
			return false;
		}
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x000D8148 File Offset: 0x000D6348
	public BurstableBounds GetChunkableData()
	{
		if (!this.boundsCalculated)
		{
			WsoData wsoData = this.data;
			WsoSerializedBoundsData wsoSerializedBoundsData = ((wsoData != null) ? wsoData.GetComponentData<WsoSerializedBoundsData>() : null);
			if (wsoSerializedBoundsData != null)
			{
				this.bounds = wsoSerializedBoundsData.Bounds;
			}
			else
			{
				this.bounds = ChunkSizeCalculator.CalculateChunkBounds(base.gameObject);
			}
			this.initialBoundsPosition = base.transform.position;
			this.boundsCalculated = true;
		}
		Vector3 vector = base.transform.position - this.initialBoundsPosition;
		return new BurstableBounds(this.bounds.GetBounds().center + vector, this.bounds.GetBounds().size);
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x000337EF File Offset: 0x000319EF
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		this.DrawChunkGizmos();
	}

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06002D4F RID: 11599 RVA: 0x000D81FC File Offset: 0x000D63FC
	// (set) Token: 0x06002D50 RID: 11600 RVA: 0x000D8204 File Offset: 0x000D6404
	[CanBeNull]
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x06002D51 RID: 11601 RVA: 0x000D820D File Offset: 0x000D640D
	public void UpdateChunkVisibility(bool isVisible)
	{
		if (this.isVisible == isVisible || !this)
		{
			return;
		}
		this.isVisible = isVisible;
		this.RefreshVisuals();
	}

	// Token: 0x06002D52 RID: 11602 RVA: 0x000D822E File Offset: 0x000D642E
	public void UpdateChunkVisibilityState(ChunkVisibilityState state)
	{
		if (!this)
		{
			return;
		}
		if (this.chunkVisibilityState == state)
		{
			return;
		}
		this.chunkVisibilityState = state;
		if (!this.isVisible)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x06002D53 RID: 11603 RVA: 0x000D8258 File Offset: 0x000D6458
	private void RefreshVisuals()
	{
		bool flag = this.IsVisible;
		bool flag2 = this.runtimeConstructorParts.Count > 0 || this.loadedOptimizedInstances.Count > 0;
		if (flag)
		{
			base.gameObject.SetActive(true);
			if (this.hasData && (this.pendingRuntimePartsRebuild || !flag2) && !this.runtimePartsLoading)
			{
				this.RequestRuntimePartsRebuild(false);
			}
			return;
		}
		if (this.chunkVisibilityState == ChunkVisibilityState.Prewarm)
		{
			base.gameObject.SetActive(false);
			if (Wso.ShouldLoadRuntimeStagesInPrewarm() && this.hasData && (this.pendingRuntimePartsRebuild || !flag2) && !this.runtimePartsLoading)
			{
				this.RequestRuntimePartsRebuild(true);
			}
			return;
		}
		LazySingleton<WsoConstructorPartsLoadManager>.Instance.CancelRebuild(this);
		LazySingleton<WsoOptimizedStagesLoadManager>.Instance.CancelLoad(this);
		if (this.runtimePartsLoading)
		{
			this.pendingRuntimePartsRebuild = true;
		}
		this.runtimePartsLoading = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002D54 RID: 11604 RVA: 0x000D8334 File Offset: 0x000D6534
	private void TryRegisterInChunkManager()
	{
		if (Application.isPlaying && !this.registeredInChunker)
		{
			LazySingleton<ChunkManager>.Instance.RegisterStaticChunkableObject(this, ChunkManagerLayerType.StaticWso);
			this.pendingRuntimePartsRebuild = true;
			this.registeredInChunker = true;
			this.UpdateChunkVisibility(false);
			if (Wso.ShouldLoadRuntimeStagesOnSpawn() && !this.runtimePartsLoading)
			{
				this.RequestRuntimePartsRebuild(true);
			}
		}
	}

	// Token: 0x06002D55 RID: 11605 RVA: 0x000D8387 File Offset: 0x000D6587
	private void TryUnregisterFromChunkManager()
	{
		if (Application.isPlaying && this.registeredInChunker)
		{
			LazySingleton<ChunkManager>.Instance.UnregisterStaticChunkableObject(this, ChunkManagerLayerType.StaticWso);
			this.registeredInChunker = false;
		}
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x000D83AC File Offset: 0x000D65AC
	public static Wso Spawn(WsoData wsoData, Transform parentTransform)
	{
		string text = "Assets/AddressableAssets/WSOs/" + wsoData.id + ".prefab";
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(text);
		asyncOperationHandle.WaitForCompletion();
		Wso wso;
		if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && asyncOperationHandle.Result != null)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(asyncOperationHandle.Result, parentTransform);
			wso = gameObject.GetComponent<Wso>();
			if (wso == null)
			{
				wso = gameObject.AddComponent<Wso>();
			}
			wso.loadedPrefabAsset = asyncOperationHandle.Result;
		}
		else
		{
			wso = new GameObject().AddComponent<Wso>();
			wso.transform.parent = parentTransform;
			Debug.LogError(string.Concat(new string[] { "[Wso] Could not load prefab for '", wsoData.id, "' from Addressables at '", text, "'" }));
		}
		wso.data = wsoData;
		wso.id = wsoData.id;
		wso.transform.position = wsoData.Position;
		wso.transform.localScale = wsoData.Scale;
		wso.Init();
		wso.TryRegisterInChunkManager();
		Action<Wso> onWsoSpawn = Wso.OnWsoSpawn;
		if (onWsoSpawn != null)
		{
			onWsoSpawn(wso);
		}
		wso.HandleHiddenChanged(wso.data.IsHidden);
		Debug.Log("Spawned Wso with id: " + wso.data.id, wso.gameObject);
		return wso;
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000D84FC File Offset: 0x000D66FC
	public void InitFromScene(string worldId)
	{
		if (this.data == null)
		{
			this.data = new WsoData(GameBalance.Me.GetData<WSODef>(this.id))
			{
				Position = base.transform.position,
				Scale = base.transform.localScale,
				WorldId = worldId
			};
		}
		this.CollectRepairableStages();
		this.Init();
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x000D8564 File Offset: 0x000D6764
	public void InitFromContentPart(Vector3 globalOffset, string gameSceneId)
	{
		if (this.data == null)
		{
			Debug.LogWarning("[Wso] InitFromContentPart: data is null on " + base.name + ", skipping");
			return;
		}
		this.data.Position = base.transform.position + globalOffset;
		this.data.WorldId = gameSceneId;
		this.Init();
		this.TryRegisterInChunkManager();
		Action<Wso> onWsoSpawn = Wso.OnWsoSpawn;
		if (onWsoSpawn != null)
		{
			onWsoSpawn(this);
		}
		this.HandleHiddenChanged(this.data.IsHidden);
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x000D85EC File Offset: 0x000D67EC
	private void Init()
	{
		this.hasData = true;
		this.IgnoreMultiFlag = new MultiFlagOR<ChunkingIgnoreType>();
		if (this.data != null)
		{
			this.data.OnHiddenStateChanged += this.HandleHiddenChanged;
			this.data.OnRepairStateChanged += this.OnRepairStateChanged;
			this.data.PrepareForGame();
		}
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x000D864C File Offset: 0x000D684C
	private void DeInit()
	{
		if (!this.hasData)
		{
			return;
		}
		this.hasData = false;
		if (this.data != null)
		{
			this.data.OnHiddenStateChanged -= this.HandleHiddenChanged;
			this.data.OnRepairStateChanged -= this.OnRepairStateChanged;
			this.data.Cleanup();
		}
		LazySingleton<WsoConstructorPartsLoadManager>.Instance.CancelRebuild(this);
		LazySingleton<WsoOptimizedStagesLoadManager>.Instance.CancelLoad(this);
		this.runtimePartsLoading = false;
		this.pendingRuntimePartsRebuild = false;
		this.ClearRuntimeConstructorParts();
		if (this.loadedPrefabAsset != null)
		{
			Addressables.Release<GameObject>(this.loadedPrefabAsset);
			this.loadedPrefabAsset = null;
		}
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x000D86F4 File Offset: 0x000D68F4
	private void OnDestroy()
	{
		this.TryUnregisterFromChunkManager();
		this.DeInit();
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x000D8704 File Offset: 0x000D6904
	public void CollectRepairableStages()
	{
		if (this.data != null && string.IsNullOrEmpty(this.data.Definition.replacementConfigId))
		{
			return;
		}
		this.repairableStages.Clear();
		base.GetComponentsInChildren<WsoRepairableStage>(true, this.repairableStages);
		foreach (WsoRepairableStage wsoRepairableStage in this.repairableStages)
		{
			wsoRepairableStage.CollectParts();
		}
		if (this.data != null)
		{
			WsoRepairablePartData orCreateRepairablePartData = this.data.GetOrCreateRepairablePartData();
			orCreateRepairablePartData.ClearStages();
			for (int i = 0; i < this.repairableStages.Count; i++)
			{
				WsoStageData wsoStageData = this.repairableStages[i].CreateStageData(i);
				orCreateRepairablePartData.AddStage(wsoStageData);
			}
		}
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x000D87D8 File Offset: 0x000D69D8
	private void RequestRuntimePartsRebuild(bool async = true)
	{
		if (!Application.isPlaying || !this.hasData)
		{
			return;
		}
		this.pendingRuntimePartsRebuild = false;
		this.runtimePartsLoading = true;
		if (this.IsOptimizedRuntime())
		{
			LazySingleton<WsoOptimizedStagesLoadManager>.Instance.RequestLoad(this, async);
			return;
		}
		LazySingleton<WsoConstructorPartsLoadManager>.Instance.RequestRebuild(this, async);
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x000D8824 File Offset: 0x000D6A24
	private bool IsOptimizedRuntime()
	{
		WsoData wsoData = this.data;
		WsoOptimizedStagesData wsoOptimizedStagesData = ((wsoData != null) ? wsoData.GetComponentData<WsoOptimizedStagesData>() : null);
		return wsoOptimizedStagesData != null && wsoOptimizedStagesData.IsOptimized;
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x000D884F File Offset: 0x000D6A4F
	private static bool ShouldLoadRuntimeStagesOnSpawn()
	{
		return Wso.GlobalRuntimeStagesLoadMode == Wso.RuntimeStagesLoadMode.LoadOnSpawn;
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x000D8859 File Offset: 0x000D6A59
	private static bool ShouldLoadRuntimeStagesInPrewarm()
	{
		return Wso.ShouldLoadRuntimeStagesOnSpawn();
	}

	// Token: 0x06002D61 RID: 11617 RVA: 0x000D8860 File Offset: 0x000D6A60
	public async Awaitable<WsoConstructorPartsBuildResult> BuildRuntimeConstructorPartsAsync()
	{
		WsoConstructorPartsBuildResult buildResult = new WsoConstructorPartsBuildResult();
		WsoData wsoData = this.data;
		WsoRepairablePartData wsoRepairablePartData = ((wsoData != null) ? wsoData.GetComponentData<WsoRepairablePartData>() : null);
		WsoConstructorPartsBuildResult wsoConstructorPartsBuildResult;
		if (wsoRepairablePartData == null || wsoRepairablePartData.Stages.Count == 0)
		{
			wsoConstructorPartsBuildResult = buildResult;
		}
		else
		{
			foreach (WsoStageData wsoStageData in wsoRepairablePartData.Stages)
			{
				foreach (ConstructorPartStateData constructorPartStateData in wsoStageData.PartsData)
				{
					ConstructorPart constructorPart = await this.SpawnConstructorPartFromDataAsync(constructorPartStateData, buildResult.LutHandles);
					if (constructorPart != null)
					{
						buildResult.Parts.Add(constructorPart);
					}
				}
				IEnumerator<ConstructorPartStateData> enumerator2 = null;
			}
			IEnumerator<WsoStageData> enumerator = null;
			wsoConstructorPartsBuildResult = buildResult;
		}
		return wsoConstructorPartsBuildResult;
	}

	// Token: 0x06002D62 RID: 11618 RVA: 0x000D88A4 File Offset: 0x000D6AA4
	public WsoConstructorPartsBuildResult BuildRuntimeConstructorPartsSync()
	{
		WsoConstructorPartsBuildResult wsoConstructorPartsBuildResult = new WsoConstructorPartsBuildResult();
		WsoData wsoData = this.data;
		WsoRepairablePartData wsoRepairablePartData = ((wsoData != null) ? wsoData.GetComponentData<WsoRepairablePartData>() : null);
		if (wsoRepairablePartData == null || wsoRepairablePartData.Stages.Count == 0)
		{
			return wsoConstructorPartsBuildResult;
		}
		foreach (WsoStageData wsoStageData in wsoRepairablePartData.Stages)
		{
			foreach (ConstructorPartStateData constructorPartStateData in wsoStageData.PartsData)
			{
				ConstructorPart constructorPart = this.SpawnConstructorPartFromDataSync(constructorPartStateData, wsoConstructorPartsBuildResult.LutHandles);
				if (constructorPart != null)
				{
					wsoConstructorPartsBuildResult.Parts.Add(constructorPart);
				}
			}
		}
		return wsoConstructorPartsBuildResult;
	}

	// Token: 0x06002D63 RID: 11619 RVA: 0x000D8974 File Offset: 0x000D6B74
	public async Awaitable<WsoOptimizedStagesBuildResult> BuildOptimizedStagesAsync()
	{
		WsoOptimizedStagesBuildResult buildResult = new WsoOptimizedStagesBuildResult();
		WsoData wsoData = this.data;
		WsoOptimizedStagesData wsoOptimizedStagesData = ((wsoData != null) ? wsoData.GetComponentData<WsoOptimizedStagesData>() : null);
		WsoOptimizedStagesBuildResult wsoOptimizedStagesBuildResult;
		if (wsoOptimizedStagesData == null || !wsoOptimizedStagesData.IsOptimized)
		{
			wsoOptimizedStagesBuildResult = buildResult;
		}
		else
		{
			WsoData wsoData2 = this.data;
			WsoRepairablePartData repairData = ((wsoData2 != null) ? wsoData2.GetComponentData<WsoRepairablePartData>() : null);
			IReadOnlyList<WsoOptimizedStageEntry> stages = wsoOptimizedStagesData.Stages;
			for (int i = 0; i < stages.Count; i++)
			{
				WsoOptimizedStageEntry wsoOptimizedStageEntry = stages[i];
				WsoRepairablePartData wsoRepairablePartData = repairData;
				bool? flag;
				if (wsoRepairablePartData == null)
				{
					flag = null;
				}
				else
				{
					WsoStageData stage = wsoRepairablePartData.GetStage(i);
					flag = ((stage != null) ? new bool?(stage.IsRepaired) : null);
				}
				bool? flag2 = flag;
				string address = (flag2.GetValueOrDefault() ? wsoOptimizedStageEntry.repairedPrefabAddress : wsoOptimizedStageEntry.destroyedPrefabAddress);
				if (!string.IsNullOrEmpty(address))
				{
					AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
					await handle.Task;
					if (GameShutdown.IsRequested)
					{
						if (handle.IsValid())
						{
							Addressables.Release<GameObject>(handle);
						}
						UniTask<bool>.Awaiter awaiter = GameShutdown.WaitForResumeAsync().GetAwaiter();
						if (!awaiter.IsCompleted)
						{
							await awaiter;
							UniTask<bool>.Awaiter awaiter2;
							awaiter = awaiter2;
							awaiter2 = default(UniTask<bool>.Awaiter);
						}
						if (!awaiter.GetResult() || this == null)
						{
							break;
						}
						i--;
					}
					else
					{
						if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
						{
							this.BuildOptimizedStageInstance(handle, buildResult);
							int num = 0;
							try
							{
								await BackgroundLoading.YieldIfNeeded(i + 1, 1);
							}
							catch (OperationCanceledException)
							{
								num = 1;
							}
							if (num == 1)
							{
								UniTask<bool>.Awaiter awaiter = GameShutdown.WaitForResumeAsync().GetAwaiter();
								if (!awaiter.IsCompleted)
								{
									await awaiter;
									UniTask<bool>.Awaiter awaiter2;
									awaiter = awaiter2;
									awaiter2 = default(UniTask<bool>.Awaiter);
								}
								if (!awaiter.GetResult())
								{
									break;
								}
								if (this == null)
								{
									break;
								}
							}
						}
						else
						{
							if (handle.IsValid())
							{
								Addressables.Release<GameObject>(handle);
							}
							Debug.LogError(string.Concat(new string[] { "[Wso] Failed to load optimized stage prefab at '", address, "' for '", base.name, "'" }));
						}
						address = null;
						handle = default(AsyncOperationHandle<GameObject>);
					}
				}
			}
			wsoOptimizedStagesBuildResult = buildResult;
		}
		return wsoOptimizedStagesBuildResult;
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x000D89B8 File Offset: 0x000D6BB8
	public WsoOptimizedStagesBuildResult BuildOptimizedStagesSync()
	{
		WsoOptimizedStagesBuildResult wsoOptimizedStagesBuildResult = new WsoOptimizedStagesBuildResult();
		WsoData wsoData = this.data;
		WsoOptimizedStagesData wsoOptimizedStagesData = ((wsoData != null) ? wsoData.GetComponentData<WsoOptimizedStagesData>() : null);
		if (wsoOptimizedStagesData == null || !wsoOptimizedStagesData.IsOptimized)
		{
			return wsoOptimizedStagesBuildResult;
		}
		WsoData wsoData2 = this.data;
		WsoRepairablePartData wsoRepairablePartData = ((wsoData2 != null) ? wsoData2.GetComponentData<WsoRepairablePartData>() : null);
		IReadOnlyList<WsoOptimizedStageEntry> stages = wsoOptimizedStagesData.Stages;
		for (int i = 0; i < stages.Count; i++)
		{
			WsoOptimizedStageEntry wsoOptimizedStageEntry = stages[i];
			bool? flag;
			if (wsoRepairablePartData == null)
			{
				flag = null;
			}
			else
			{
				WsoStageData stage = wsoRepairablePartData.GetStage(i);
				flag = ((stage != null) ? new bool?(stage.IsRepaired) : null);
			}
			bool? flag2 = flag;
			string text = (flag2.GetValueOrDefault() ? wsoOptimizedStageEntry.repairedPrefabAddress : wsoOptimizedStageEntry.destroyedPrefabAddress);
			if (!string.IsNullOrEmpty(text))
			{
				AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(text);
				asyncOperationHandle.WaitForCompletion();
				if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && asyncOperationHandle.Result != null)
				{
					this.BuildOptimizedStageInstance(asyncOperationHandle, wsoOptimizedStagesBuildResult);
				}
				else
				{
					if (asyncOperationHandle.IsValid())
					{
						Addressables.Release<GameObject>(asyncOperationHandle);
					}
					Debug.LogError(string.Concat(new string[] { "[Wso] Failed to load optimized stage prefab at '", text, "' for '", base.name, "'" }));
				}
			}
		}
		return wsoOptimizedStagesBuildResult;
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x000D8B00 File Offset: 0x000D6D00
	private void BuildOptimizedStageInstance(AsyncOperationHandle<GameObject> handle, WsoOptimizedStagesBuildResult buildResult)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(handle.Result, base.transform);
		gameObject.transform.localPosition = Vector3.zero;
		buildResult.Instances.Add(gameObject);
		buildResult.Handles.Add(handle);
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x000D8B48 File Offset: 0x000D6D48
	private async Awaitable<ConstructorPart> SpawnConstructorPartFromDataAsync(ConstructorPartStateData partData, List<AsyncOperationHandle<Texture2D>> newLutHandles)
	{
		ConstructorPart constructorPart;
		if (partData.IsDeleted)
		{
			constructorPart = null;
		}
		else
		{
			WsoData wsoData = this.Data;
			ConstructorPartReplacementConfig constructorPartReplacementConfig = ((wsoData != null) ? wsoData.Definition.ReplacementConfig : null);
			string assetPath = partData.GetCurrentAssetPath(constructorPartReplacementConfig);
			if (string.IsNullOrEmpty(assetPath))
			{
				constructorPart = null;
			}
			else
			{
				string currentModelId = partData.GetCurrentModelId(constructorPartReplacementConfig);
				GameObject partGo = new GameObject(currentModelId ?? "");
				partGo.transform.SetParent(base.transform);
				partGo.transform.localPosition = partData.LocalPosition;
				partGo.transform.localScale = new Vector3(partData.LocalXScale, 1f, 1f);
				ConstructorPart part = partGo.AddComponent<ConstructorPart>();
				part.constructorPartChildData.pathToObject = assetPath;
				part.constructorPartChildData.canNotBeBaked = true;
				Texture2D texture2D = null;
				string text;
				if (constructorPartReplacementConfig != null && constructorPartReplacementConfig.TryGetLutAssetPath(partData.IsRepaired, partData.LutName, out text))
				{
					AsyncOperationHandle<Texture2D> handle = Addressables.LoadAssetAsync<Texture2D>(text);
					texture2D = await handle.Task;
					if (handle.Status == AsyncOperationStatus.Succeeded)
					{
						newLutHandles.Add(handle);
					}
					else if (handle.IsValid())
					{
						Addressables.Release<Texture2D>(handle);
					}
					handle = default(AsyncOperationHandle<Texture2D>);
				}
				part.constructorPartChildData.lut = texture2D;
				BurstableChunkBoundsPair burstableChunkBoundsPair;
				if (LazySingletonSO<ConstructorPartBoundsConfig>.Instance.BoundsCollection.TryGetBounds(assetPath, partGo.transform.position, partGo.transform.lossyScale, out burstableChunkBoundsPair))
				{
					part.constructorPartChildData.chunkBounds = burstableChunkBoundsPair;
				}
				else
				{
					Debug.LogError("Failed to get chunk bounds for " + assetPath);
				}
				part.UpdateChunkVisibility(false);
				constructorPart = part;
			}
		}
		return constructorPart;
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x000D8B9C File Offset: 0x000D6D9C
	private ConstructorPart SpawnConstructorPartFromDataSync(ConstructorPartStateData partData, List<AsyncOperationHandle<Texture2D>> newLutHandles)
	{
		if (partData.IsDeleted)
		{
			return null;
		}
		WsoData wsoData = this.Data;
		ConstructorPartReplacementConfig constructorPartReplacementConfig = ((wsoData != null) ? wsoData.Definition.ReplacementConfig : null);
		string currentAssetPath = partData.GetCurrentAssetPath(constructorPartReplacementConfig);
		if (string.IsNullOrEmpty(currentAssetPath))
		{
			return null;
		}
		GameObject gameObject = new GameObject(partData.GetCurrentModelId(constructorPartReplacementConfig) ?? "");
		gameObject.transform.SetParent(base.transform);
		gameObject.transform.localPosition = partData.LocalPosition;
		gameObject.transform.localScale = new Vector3(partData.LocalXScale, 1f, 1f);
		ConstructorPart constructorPart = gameObject.AddComponent<ConstructorPart>();
		constructorPart.constructorPartChildData.pathToObject = currentAssetPath;
		constructorPart.constructorPartChildData.canNotBeBaked = true;
		Texture2D texture2D = null;
		string text;
		if (constructorPartReplacementConfig != null && constructorPartReplacementConfig.TryGetLutAssetPath(partData.IsRepaired, partData.LutName, out text))
		{
			AsyncOperationHandle<Texture2D> asyncOperationHandle = Addressables.LoadAssetAsync<Texture2D>(text);
			texture2D = asyncOperationHandle.WaitForCompletion();
			if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
			{
				newLutHandles.Add(asyncOperationHandle);
			}
			else if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<Texture2D>(asyncOperationHandle);
			}
		}
		constructorPart.constructorPartChildData.lut = texture2D;
		BurstableChunkBoundsPair burstableChunkBoundsPair;
		if (LazySingletonSO<ConstructorPartBoundsConfig>.Instance.BoundsCollection.TryGetBounds(currentAssetPath, gameObject.transform.position, gameObject.transform.lossyScale, out burstableChunkBoundsPair))
		{
			constructorPart.constructorPartChildData.chunkBounds = burstableChunkBoundsPair;
		}
		else
		{
			Debug.LogError("Failed to get chunk bounds for " + currentAssetPath);
		}
		constructorPart.UpdateChunkVisibility(false);
		return constructorPart;
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x000D8D0C File Offset: 0x000D6F0C
	public void CompleteRuntimePartsRebuild(WsoConstructorPartsBuildResult buildResult)
	{
		this.runtimePartsLoading = false;
		if (!this || !this.hasData)
		{
			this.ReleasePendingRuntimeParts(buildResult);
			return;
		}
		this.ClearRuntimeConstructorParts();
		this.runtimeConstructorParts.AddRange(buildResult.Parts);
		this.loadedLutHandles.AddRange(buildResult.LutHandles);
		if (this.runtimeConstructorParts.Count > 0)
		{
			LazySingleton<ChunkManager>.Instance.RegisterChunks<ConstructorPart>(this.runtimeConstructorParts, ChunkManagerLayerType.WsoConstructorParts);
		}
		if (this.pendingRuntimePartsRebuild)
		{
			if (this.IsVisible)
			{
				this.RequestRuntimePartsRebuild(false);
				return;
			}
			if (Wso.ShouldLoadRuntimeStagesInPrewarm() && this.chunkVisibilityState == ChunkVisibilityState.Prewarm)
			{
				this.RequestRuntimePartsRebuild(true);
			}
		}
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x000D8DB0 File Offset: 0x000D6FB0
	public void ReleasePendingRuntimeParts(WsoConstructorPartsBuildResult buildResult)
	{
		this.runtimePartsLoading = false;
		if (buildResult == null)
		{
			return;
		}
		for (int i = 0; i < buildResult.Parts.Count; i++)
		{
			ConstructorPart constructorPart = buildResult.Parts[i];
			if (constructorPart != null)
			{
				global::UnityEngine.Object.Destroy(constructorPart.gameObject);
			}
		}
		for (int j = 0; j < buildResult.LutHandles.Count; j++)
		{
			AsyncOperationHandle<Texture2D> asyncOperationHandle = buildResult.LutHandles[j];
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<Texture2D>(asyncOperationHandle);
			}
		}
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x000D8E34 File Offset: 0x000D7034
	public void CompleteOptimizedStagesLoad(WsoOptimizedStagesBuildResult buildResult)
	{
		this.runtimePartsLoading = false;
		if (!this || !this.hasData)
		{
			this.ReleasePendingOptimizedStages(buildResult);
			return;
		}
		this.ClearRuntimeConstructorParts();
		if (buildResult != null)
		{
			this.loadedOptimizedInstances.AddRange(buildResult.Instances);
			this.loadedOptimizedHandles.AddRange(buildResult.Handles);
		}
		if (this.pendingRuntimePartsRebuild)
		{
			if (this.IsVisible)
			{
				this.RequestRuntimePartsRebuild(false);
				return;
			}
			if (Wso.ShouldLoadRuntimeStagesInPrewarm() && this.chunkVisibilityState == ChunkVisibilityState.Prewarm)
			{
				this.RequestRuntimePartsRebuild(true);
			}
		}
	}

	// Token: 0x06002D6B RID: 11627 RVA: 0x000D8EBC File Offset: 0x000D70BC
	public void ReleasePendingOptimizedStages(WsoOptimizedStagesBuildResult buildResult)
	{
		this.runtimePartsLoading = false;
		if (buildResult == null)
		{
			return;
		}
		for (int i = 0; i < buildResult.Instances.Count; i++)
		{
			GameObject gameObject = buildResult.Instances[i];
			if (gameObject != null)
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
		}
		for (int j = 0; j < buildResult.Handles.Count; j++)
		{
			AsyncOperationHandle<GameObject> asyncOperationHandle = buildResult.Handles[j];
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<GameObject>(asyncOperationHandle);
			}
		}
	}

	// Token: 0x06002D6C RID: 11628 RVA: 0x000D8F38 File Offset: 0x000D7138
	private void ClearRuntimeConstructorParts()
	{
		this.ClearOptimizedInstances();
		foreach (ConstructorPart constructorPart in this.runtimeConstructorParts)
		{
			if (constructorPart != null)
			{
				global::UnityEngine.Object.Destroy(constructorPart.gameObject);
			}
		}
		LazySingleton<ChunkManager>.Instance.UnregisterChunks<ConstructorPart>(this.runtimeConstructorParts, ChunkManagerLayerType.WsoConstructorParts);
		this.runtimeConstructorParts.Clear();
		this.ReleaseLutHandles();
	}

	// Token: 0x06002D6D RID: 11629 RVA: 0x000D8FC0 File Offset: 0x000D71C0
	private void ClearOptimizedInstances()
	{
		foreach (GameObject gameObject in this.loadedOptimizedInstances)
		{
			if (gameObject != null)
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
		}
		this.loadedOptimizedInstances.Clear();
		foreach (AsyncOperationHandle<GameObject> asyncOperationHandle in this.loadedOptimizedHandles)
		{
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<GameObject>(asyncOperationHandle);
			}
		}
		this.loadedOptimizedHandles.Clear();
	}

	// Token: 0x06002D6E RID: 11630 RVA: 0x000D907C File Offset: 0x000D727C
	private void ReleaseLutHandles()
	{
		foreach (AsyncOperationHandle<Texture2D> asyncOperationHandle in this.loadedLutHandles)
		{
			if (asyncOperationHandle.IsValid())
			{
				Addressables.Release<Texture2D>(asyncOperationHandle);
			}
		}
		this.loadedLutHandles.Clear();
	}

	// Token: 0x06002D6F RID: 11631 RVA: 0x000D90E4 File Offset: 0x000D72E4
	private void OnRepairStateChanged()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.pendingRuntimePartsRebuild = true;
		if (this.runtimePartsLoading)
		{
			return;
		}
		if (this.IsVisible)
		{
			this.RequestRuntimePartsRebuild(false);
			return;
		}
		if (this.chunkVisibilityState == ChunkVisibilityState.Prewarm && Wso.ShouldLoadRuntimeStagesInPrewarm())
		{
			this.RequestRuntimePartsRebuild(true);
		}
	}

	// Token: 0x06002D70 RID: 11632 RVA: 0x000D9130 File Offset: 0x000D7330
	public void RepairAllStages()
	{
		if (this.data == null)
		{
			return;
		}
		TownUtils.RepairHouse(this.data, -1);
	}

	// Token: 0x06002D71 RID: 11633 RVA: 0x000D914A File Offset: 0x000D734A
	public void DuplicateAndRepairAllStages()
	{
		WsoData wsoData = this.data;
	}

	// Token: 0x06002D72 RID: 11634 RVA: 0x000D9154 File Offset: 0x000D7354
	public void RepairStage(int stageIndex)
	{
		if (this.data == null)
		{
			return;
		}
		WsoRepairablePartData componentData = this.data.GetComponentData<WsoRepairablePartData>();
		if (componentData == null)
		{
			Debug.LogWarning("[Wso] Cannot repair - no WsoRepairablePartData component on " + base.name);
			return;
		}
		ConstructorPartReplacementConfig replacementConfig = this.GetReplacementConfig();
		if (replacementConfig == null)
		{
			Debug.LogWarning("[Wso] Cannot repair - no replacement config found for " + base.name);
			return;
		}
		int num = componentData.RepairStage(stageIndex, replacementConfig);
		if (num > 0)
		{
			this.data.NotifyRepairStateChanged();
			Debug.Log(string.Format("[Wso] Repaired {0} parts in stage {1} of {2}", num, stageIndex, base.name));
		}
	}

	// Token: 0x06002D73 RID: 11635 RVA: 0x000D91F0 File Offset: 0x000D73F0
	public void ResetAllStages()
	{
		if (this.data == null)
		{
			return;
		}
		WsoRepairablePartData componentData = this.data.GetComponentData<WsoRepairablePartData>();
		if (componentData == null)
		{
			return;
		}
		componentData.ResetAllStages();
		this.data.NotifyRepairStateChanged();
	}

	// Token: 0x06002D74 RID: 11636 RVA: 0x000D9228 File Offset: 0x000D7428
	private ConstructorPartReplacementConfig GetReplacementConfig()
	{
		WsoData wsoData = this.Data;
		ConstructorPartReplacementConfig constructorPartReplacementConfig = ((wsoData != null) ? wsoData.Definition.ReplacementConfig : null);
		if (constructorPartReplacementConfig == null)
		{
			string presetIdFromStages = this.GetPresetIdFromStages();
			if (!string.IsNullOrEmpty(presetIdFromStages))
			{
				constructorPartReplacementConfig = ConstructorPartReplacementService.GetReplacementConfigForPreset(presetIdFromStages);
			}
		}
		return constructorPartReplacementConfig;
	}

	// Token: 0x06002D75 RID: 11637 RVA: 0x000D9270 File Offset: 0x000D7470
	private string GetPresetIdFromStages()
	{
		WsoData wsoData = this.data;
		WsoRepairablePartData wsoRepairablePartData = ((wsoData != null) ? wsoData.GetComponentData<WsoRepairablePartData>() : null);
		if (wsoRepairablePartData == null || wsoRepairablePartData.Stages.Count == 0)
		{
			return null;
		}
		WsoStageData wsoStageData = wsoRepairablePartData.Stages[0];
		if (wsoStageData.PartsData.Count == 0)
		{
			return null;
		}
		string originalModelId = wsoStageData.PartsData[0].OriginalModelId;
		if (string.IsNullOrEmpty(originalModelId))
		{
			return null;
		}
		int num = originalModelId.IndexOf('-');
		if (num <= 0)
		{
			return null;
		}
		return originalModelId.Substring(0, num);
	}

	// Token: 0x06002D76 RID: 11638 RVA: 0x000D92F1 File Offset: 0x000D74F1
	private void HandleHiddenChanged(bool isHidden)
	{
		this.RefreshVisuals();
	}

	// Token: 0x04002437 RID: 9271
	[SerializeField]
	private string id;

	// Token: 0x04002438 RID: 9272
	[SerializeField]
	private bool shouldOptimize;

	// Token: 0x04002439 RID: 9273
	[SerializeField]
	private WsoData data;

	// Token: 0x0400243A RID: 9274
	[SerializeField]
	private List<WsoRepairableStage> repairableStages = new List<WsoRepairableStage>();

	// Token: 0x0400243B RID: 9275
	[SerializeField]
	private bool isOptimized;

	// Token: 0x0400243C RID: 9276
	[SerializeField]
	private List<WsoOptimizedStageEntry> optimizedStages = new List<WsoOptimizedStageEntry>();

	// Token: 0x0400243D RID: 9277
	private List<ConstructorPart> runtimeConstructorParts = new List<ConstructorPart>();

	// Token: 0x0400243E RID: 9278
	private List<AsyncOperationHandle<Texture2D>> loadedLutHandles = new List<AsyncOperationHandle<Texture2D>>();

	// Token: 0x0400243F RID: 9279
	private List<GameObject> loadedOptimizedInstances = new List<GameObject>();

	// Token: 0x04002440 RID: 9280
	private List<AsyncOperationHandle<GameObject>> loadedOptimizedHandles = new List<AsyncOperationHandle<GameObject>>();

	// Token: 0x04002441 RID: 9281
	private bool isVisible = true;

	// Token: 0x04002442 RID: 9282
	private bool hasData;

	// Token: 0x04002443 RID: 9283
	private ChunkBoundsPair bounds;

	// Token: 0x04002444 RID: 9284
	private bool boundsCalculated;

	// Token: 0x04002445 RID: 9285
	private Vector3 initialBoundsPosition;

	// Token: 0x04002446 RID: 9286
	private GameObject loadedPrefabAsset;

	// Token: 0x04002447 RID: 9287
	private bool registeredInChunker;

	// Token: 0x04002448 RID: 9288
	private bool runtimePartsLoading;

	// Token: 0x04002449 RID: 9289
	private bool pendingRuntimePartsRebuild;

	// Token: 0x0400244A RID: 9290
	private ChunkVisibilityState chunkVisibilityState;

	// Token: 0x0200069A RID: 1690
	public enum RuntimeStagesLoadMode
	{
		// Token: 0x0400244D RID: 9293
		LoadOnSpawn,
		// Token: 0x0400244E RID: 9294
		LoadOnlyWhenVisible
	}
}
