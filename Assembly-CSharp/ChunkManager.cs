using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using LazyBearTechnology;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020006F8 RID: 1784
[DefaultExecutionOrder(10000)]
public class ChunkManager : LazySingleton<ChunkManager>
{
	// Token: 0x17000750 RID: 1872
	// (get) Token: 0x06002F13 RID: 12051 RVA: 0x000E0DA7 File Offset: 0x000DEFA7
	// (set) Token: 0x06002F14 RID: 12052 RVA: 0x000E0DAF File Offset: 0x000DEFAF
	public bool IsActive { get; set; }

	// Token: 0x06002F15 RID: 12053 RVA: 0x000E0DB8 File Offset: 0x000DEFB8
	public void RegisterChunks<T>(List<T> chunkableObjects, ChunkManagerLayerType layerType) where T : IChunkableObject
	{
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		foreach (T t in chunkableObjects)
		{
			if (ChunkManager.IsChunkableObjectAlive(t))
			{
				this.SplitChunkableObjectBetweenChunks(chunkManagerLayer, t);
			}
		}
		chunkManagerLayer.EnsureCapacity();
		foreach (Chunk chunk in chunkManagerLayer.chunks)
		{
			if (chunk.Count == 0)
			{
				chunk.EnsureCapacity();
			}
			else
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (IChunkableObject chunkableObject in chunk.chunkableObjects)
				{
					if (ChunkManager.IsChunkableObjectAlive(chunkableObject))
					{
						BurstableBounds chunkableData = chunkableObject.GetChunkableData();
						num = Mathf.Min(num, chunkableData.Min.y);
						num2 = Mathf.Max(num2, chunkableData.Max.y);
					}
				}
				chunk.chunkBounds.center.y = (num + num2) / 2f;
				chunk.chunkBounds.size.y = num2 - num;
				chunk.EnsureCapacity();
			}
		}
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x000E0F30 File Offset: 0x000DF130
	public void UnregisterChunks<T>(List<T> chunkList, ChunkManagerLayerType layerType) where T : IChunkableObject
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		for (int i = chunkManagerLayer.Count - 1; i >= 0; i--)
		{
			Chunk chunk = chunkManagerLayer.chunks[i];
			List<IChunkableObject> list = null;
			foreach (T t in chunkList)
			{
				if (chunk.ContainsChunkableObject(t))
				{
					if (list == null)
					{
						list = new List<IChunkableObject>();
					}
					list.Add(t);
				}
			}
			if (list != null)
			{
				chunk.RemoveChunkableObjects(list);
			}
			if (chunk.Count == 0)
			{
				chunkManagerLayer.RemoveChunk(chunk);
			}
			else
			{
				chunk.EnsureCapacity();
			}
		}
		chunkManagerLayer.EnsureCapacity();
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000E100C File Offset: 0x000DF20C
	public void RegisterStaticChunkableObject(IChunkableObject chunkableObject, ChunkManagerLayerType layerType)
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		this.SplitChunkableObjectBetweenChunks(chunkManagerLayer, chunkableObject);
		chunkManagerLayer.EnsureCapacity();
		for (int i = 0; i < chunkManagerLayer.Count; i++)
		{
			if (this.IsObjectIntersectsChunk(chunkableObject, chunkManagerLayer.chunks[i]) && !chunkManagerLayer.chunks[i].ContainsChunkableObject(chunkableObject))
			{
				chunkManagerLayer.chunks[i].AddChunkableObject(chunkableObject);
			}
		}
		foreach (Chunk chunk in chunkManagerLayer.chunks)
		{
			if (chunk.Count == 0)
			{
				chunk.EnsureCapacity();
			}
			else
			{
				float num = float.PositiveInfinity;
				float num2 = float.NegativeInfinity;
				foreach (IChunkableObject chunkableObject2 in chunk.chunkableObjects)
				{
					BurstableBounds chunkableData = chunkableObject2.GetChunkableData();
					num = Mathf.Min(num, chunkableData.Min.y);
					num2 = Mathf.Max(num2, chunkableData.Max.y);
				}
				chunk.chunkBounds.center.y = (num + num2) / 2f;
				chunk.chunkBounds.size.y = num2 - num;
				chunk.EnsureCapacity();
			}
		}
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x000E1178 File Offset: 0x000DF378
	public void UnregisterStaticChunkableObject(IChunkableObject chunkableObject, ChunkManagerLayerType layerType)
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		for (int i = chunkManagerLayer.Count - 1; i >= 0; i--)
		{
			Chunk chunk = chunkManagerLayer.chunks[i];
			if (chunk.ContainsChunkableObject(chunkableObject))
			{
				chunk.RemoveChunkableObject(chunkableObject);
				if (chunk.Count == 0)
				{
					chunkManagerLayer.RemoveChunk(chunk);
				}
				else
				{
					chunk.EnsureCapacity();
				}
			}
		}
		chunkManagerLayer.EnsureCapacity();
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000E11F0 File Offset: 0x000DF3F0
	public void RegisterDynamicChunkableObject(IChunkableObject chunkableObject, ChunkManagerLayerType layerType)
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		chunkManagerLayer.chunks[0].AddChunkableObject(chunkableObject);
		chunkManagerLayer.chunks[0].EnsureCapacity();
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x000E122D File Offset: 0x000DF42D
	public void UnregisterDynamicChunkableObject(IChunkableObject chunkableObject, ChunkManagerLayerType layerType)
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		chunkManagerLayer.chunks[0].RemoveChunkableObject(chunkableObject);
		chunkManagerLayer.chunks[0].EnsureCapacity();
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x000E126C File Offset: 0x000DF46C
	public void SetChunkableObjectToDynamicLayer(List<IChunkableObject> chunkableObjects, ChunkManagerLayerType layerType)
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		if (!chunkManagerLayer.IsDynamic)
		{
			return;
		}
		chunkManagerLayer.chunks[0].SetChunkableObjects(chunkableObjects);
		chunkManagerLayer.chunks[0].EnsureCapacity();
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x000E12C0 File Offset: 0x000DF4C0
	public void ClearChunkableObjectFromDynamicLayer(ChunkManagerLayerType layerType)
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		this.TryInit();
		ChunkManagerLayer chunkManagerLayer = this.layers[(int)layerType];
		if (!chunkManagerLayer.IsDynamic)
		{
			return;
		}
		chunkManagerLayer.chunks[0].ClearChunkableObjects();
		chunkManagerLayer.chunks[0].EnsureCapacity();
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x000E1314 File Offset: 0x000DF514
	public void ClearAll()
	{
		if (!Application.isPlaying || PlayModeTracker.IsExitingPlayMode)
		{
			return;
		}
		if (!this.isInitialized)
		{
			return;
		}
		this.pendingVisibilityRecheckObjects.Clear();
		foreach (ChunkManagerLayer chunkManagerLayer in this.layers)
		{
			chunkManagerLayer.Clear();
			if (chunkManagerLayer.IsDynamic)
			{
				chunkManagerLayer.AddChunk(ChunkManager.CreateInfiniteChunk());
			}
			chunkManagerLayer.EnsureCapacity();
		}
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x000E13A4 File Offset: 0x000DF5A4
	private static Chunk CreateInfiniteChunk()
	{
		return new Chunk(float3.zero, new float3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity));
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x000E13C4 File Offset: 0x000DF5C4
	public void ForceProcessVisibility()
	{
		this.TryProcessChunkVisibility();
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x000E13CC File Offset: 0x000DF5CC
	public void RequestVisibilityRecheck(IChunkableObject chunkableObject)
	{
		if (!this.IsActive)
		{
			return;
		}
		this.TryInit();
		if (!ChunkManager.IsChunkableObjectAlive(chunkableObject))
		{
			return;
		}
		this.pendingVisibilityRecheckObjects.Add(chunkableObject);
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x000E13F3 File Offset: 0x000DF5F3
	public HashSet<IChunkableObject> GetAllChunkableObjectsInBounds(Bounds bounds)
	{
		return this.GetAllChunkableObjectsInBoundsForSelectedLayers(Enum.GetValues(typeof(ChunkManagerLayerType)).Cast<ChunkManagerLayerType>().ToList<ChunkManagerLayerType>(), bounds);
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x000E1418 File Offset: 0x000DF618
	public HashSet<IChunkableObject> GetAllChunkableObjectsInBoundsForSelectedLayers(List<ChunkManagerLayerType> selectedTypes, Bounds bounds)
	{
		HashSet<IChunkableObject> hashSet = new HashSet<IChunkableObject>();
		for (int i = 0; i < this.layers.Count; i++)
		{
			ChunkManagerLayer chunkManagerLayer = this.layers[i];
			if (selectedTypes.Contains(chunkManagerLayer.layerType))
			{
				for (int j = 0; j < chunkManagerLayer.Count; j++)
				{
					Chunk chunk = chunkManagerLayer.chunks[j];
					List<IChunkableObject> list = new List<IChunkableObject>();
					for (int k = 0; k < chunk.Count; k++)
					{
						IChunkableObject chunkableObject = chunk.chunkableObjects[k];
						if (chunkableObject != null)
						{
							global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
							if (@object == null || !(@object == null))
							{
								list.Add(chunkableObject);
							}
						}
					}
					if (list.Count != 0)
					{
						ChunkDataIntersectsJob chunkDataIntersectsJob = default(ChunkDataIntersectsJob);
						chunkDataIntersectsJob.sourceData = BurstConverter.ConvertBoundsToBurstable(bounds);
						chunkDataIntersectsJob.objectsData = new NativeArray<BurstableBounds>(list.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
						chunkDataIntersectsJob.results = new NativeArray<bool>(list.Count, Allocator.Persistent, NativeArrayOptions.ClearMemory);
						for (int l = 0; l < list.Count; l++)
						{
							chunkDataIntersectsJob.objectsData[l] = list[l].GetChunkableData();
						}
						chunkDataIntersectsJob.Schedule(list.Count, 4, default(JobHandle)).Complete();
						for (int m = 0; m < list.Count; m++)
						{
							if (chunkDataIntersectsJob.results[m])
							{
								hashSet.Add(list[m]);
							}
						}
						chunkDataIntersectsJob.objectsData.Dispose();
						chunkDataIntersectsJob.results.Dispose();
					}
				}
			}
		}
		return hashSet;
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x000E15C5 File Offset: 0x000DF7C5
	protected override void Awake()
	{
		base.Awake();
		this.TryInit();
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x000E15D3 File Offset: 0x000DF7D3
	private void OnEnable()
	{
		CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x000E15EB File Offset: 0x000DF7EB
	private void OnDisable()
	{
		CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x000E13C4 File Offset: 0x000DF5C4
	private void OnCameraUpdated(CinemachineBrain brain)
	{
		this.TryProcessChunkVisibility();
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x000E1604 File Offset: 0x000DF804
	private void TryProcessChunkVisibility()
	{
		if (!this.IsActive || this.isProcessingVisibility)
		{
			return;
		}
		CameraSystem instance = CameraSystem.Instance;
		Camera camera;
		if (instance == null)
		{
			camera = null;
		}
		else
		{
			MainCamera mainCamera = instance.MainCamera;
			camera = ((mainCamera != null) ? mainCamera.Camera : null);
		}
		Camera camera2 = camera;
		if (camera2 == null)
		{
			return;
		}
		this.isProcessingVisibility = true;
		try
		{
			this.ProcessChunkVisibility(camera2);
		}
		finally
		{
			this.isProcessingVisibility = false;
		}
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x000E1674 File Offset: 0x000DF874
	private void SplitChunkableObjectBetweenChunks(ChunkManagerLayer layer, IChunkableObject chunkableObject)
	{
		BurstableBounds chunkableData = chunkableObject.GetChunkableData();
		float2 @float = new float2(Mathf.Floor(chunkableData.Min.x / ChunkManager.chunkSize.x) * ChunkManager.chunkSize.x, Mathf.Floor(chunkableData.Min.z / ChunkManager.chunkSize.z) * ChunkManager.chunkSize.z);
		float2 float2 = new float2(Mathf.Ceil(chunkableData.Max.x / ChunkManager.chunkSize.x) * ChunkManager.chunkSize.x, Mathf.Ceil(chunkableData.Max.z / ChunkManager.chunkSize.z) * ChunkManager.chunkSize.z);
		float num = ((float2.x > @float.x) ? @float.x : float2.x);
		float num2 = ((float2.x > @float.x) ? float2.x : @float.x);
		float num3 = ((float2.y > @float.y) ? @float.y : float2.y);
		float num4 = ((float2.y > @float.y) ? float2.y : @float.y);
		for (float num5 = num; num5 <= num2; num5 += ChunkManager.chunkSize.x)
		{
			for (float num6 = num3; num6 <= num4; num6 += ChunkManager.chunkSize.z)
			{
				float3 float3 = new float3(num5, 30f, num6);
				Chunk chunk = layer.FindChunk(float3);
				if (chunk == null)
				{
					chunk = new Chunk(float3, ChunkManager.chunkSize);
					if (this.IsObjectIntersectsChunk(chunkableObject, chunk))
					{
						layer.AddChunk(chunk);
						chunk.AddChunkableObject(chunkableObject);
					}
				}
				else if (!chunk.ContainsChunkableObject(chunkableObject) && this.IsObjectIntersectsChunk(chunkableObject, chunk))
				{
					chunk.AddChunkableObject(chunkableObject);
				}
			}
		}
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x000E1848 File Offset: 0x000DFA48
	private void TryInit()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.isInitialized = true;
		this.pendingVisibilityRecheckObjects = new HashSet<IChunkableObject>();
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.layers.Add(new ChunkManagerLayer(ChunkManagerLayerType.StaticObjects));
		ChunkManagerLayer chunkManagerLayer = new ChunkManagerLayer(ChunkManagerLayerType.DynamicWgo);
		chunkManagerLayer.AddChunk(ChunkManager.CreateInfiniteChunk());
		chunkManagerLayer.EnsureCapacity();
		this.layers.Add(chunkManagerLayer);
		ChunkManagerLayer chunkManagerLayer2 = new ChunkManagerLayer(ChunkManagerLayerType.DropView);
		chunkManagerLayer2.AddChunk(ChunkManager.CreateInfiniteChunk());
		chunkManagerLayer2.EnsureCapacity();
		this.layers.Add(chunkManagerLayer2);
		this.layers.Add(new ChunkManagerLayer(ChunkManagerLayerType.StaticWgo));
		this.layers.Add(new ChunkManagerLayer(ChunkManagerLayerType.StaticWso));
		this.layers.Add(new ChunkManagerLayer(ChunkManagerLayerType.WsoConstructorParts));
		this.layers.Add(new ChunkManagerLayer(ChunkManagerLayerType.FightingLevelStaticObjects));
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x000E1918 File Offset: 0x000DFB18
	private void ProcessChunkVisibility(Camera worldCamera)
	{
		GeometryUtility.CalculateFrustumPlanes(worldCamera, this.cachedFrustumPlanes);
		BurstConverter.ConvertToBurstablePlanes(this.cachedFrustumPlanes, this.cachedBurstablePlanes);
		BurstablePlane burstablePlane = this.cachedBurstablePlanes[0];
		BurstablePlane burstablePlane2 = this.cachedBurstablePlanes[1];
		BurstablePlane burstablePlane3 = this.cachedBurstablePlanes[2];
		BurstablePlane burstablePlane4 = this.cachedBurstablePlanes[3];
		BurstablePlane burstablePlane5 = this.cachedBurstablePlanes[4];
		BurstablePlane burstablePlane6 = this.cachedBurstablePlanes[5];
		foreach (ChunkManagerLayer chunkManagerLayer in this.layers)
		{
			if (chunkManagerLayer.Count > 0)
			{
				if (chunkManagerLayer.IsDynamic)
				{
					for (int i = 0; i < chunkManagerLayer.Count; i++)
					{
						chunkManagerLayer.chunkVisibilityStateResults[i] = 2;
					}
				}
				else
				{
					for (int j = 0; j < chunkManagerLayer.Count; j++)
					{
						chunkManagerLayer.chunkDataArray[j] = chunkManagerLayer.chunks[j].GetChunkableData();
					}
					new ChunkVisibilityJob
					{
						plane0 = burstablePlane,
						plane1 = burstablePlane2,
						plane2 = burstablePlane3,
						plane3 = burstablePlane4,
						plane4 = burstablePlane5,
						plane5 = burstablePlane6,
						chunkableDataArray = chunkManagerLayer.chunkDataArray,
						visibilityStateResults = chunkManagerLayer.chunkVisibilityStateResults,
						expandFactor = chunkManagerLayer.expandFactor,
						prewarmPlanePadding = chunkManagerLayer.prewarmPlanePadding
					}.Schedule(chunkManagerLayer.Count, 4, default(JobHandle)).Complete();
				}
				if (chunkManagerLayer.IsDynamic)
				{
					List<IChunkableObject> dynamicUpdateBuffer = chunkManagerLayer.dynamicUpdateBuffer;
					dynamicUpdateBuffer.Clear();
					for (int k = 0; k < chunkManagerLayer.Count; k++)
					{
						Chunk chunk = chunkManagerLayer.chunks[k];
						chunk.isVisible = chunkManagerLayer.chunkVisibilityStateResults[k] == 2;
						if (chunk.isVisible)
						{
							for (int l = 0; l < chunk.Count; l++)
							{
								IChunkableObject chunkableObject = chunk.chunkableObjects[l];
								if (chunkableObject != null)
								{
									global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
									if ((@object == null || !(@object == null)) && !chunkableObject.IgnoreChunkVisibility)
									{
										dynamicUpdateBuffer.Add(chunkableObject);
									}
								}
							}
						}
					}
					int count = dynamicUpdateBuffer.Count;
					if (count > 0)
					{
						chunkManagerLayer.dynamicBatchJobBuffer.EnsureCapacity(count);
						NativeArray<BurstableBounds> data = chunkManagerLayer.dynamicBatchJobBuffer.data;
						NativeArray<byte> results = chunkManagerLayer.dynamicBatchJobBuffer.results;
						for (int m = 0; m < count; m++)
						{
							data[m] = dynamicUpdateBuffer[m].GetChunkableData();
						}
						ChunkVisibilityJob chunkVisibilityJob = new ChunkVisibilityJob
						{
							plane0 = burstablePlane,
							plane1 = burstablePlane2,
							plane2 = burstablePlane3,
							plane3 = burstablePlane4,
							plane4 = burstablePlane5,
							plane5 = burstablePlane6,
							chunkableDataArray = data,
							visibilityStateResults = results,
							expandFactor = chunkManagerLayer.expandFactor,
							prewarmPlanePadding = chunkManagerLayer.prewarmPlanePadding
						};
						chunkVisibilityJob.Schedule(count, 4, default(JobHandle)).Complete();
						for (int n = 0; n < count; n++)
						{
							IChunkableObject chunkableObject2 = dynamicUpdateBuffer[n];
							if (chunkableObject2 != null)
							{
								global::UnityEngine.Object object2 = chunkableObject2 as global::UnityEngine.Object;
								if (object2 == null || !(object2 == null))
								{
									ChunkVisibilityState chunkVisibilityState = (ChunkVisibilityState)chunkVisibilityJob.visibilityStateResults[n];
									this.DispatchChunkVisibilityState(chunkableObject2, chunkVisibilityState);
								}
							}
						}
					}
				}
				else
				{
					List<IChunkableObject> objectsLeavingVisibleBuffer = chunkManagerLayer.objectsLeavingVisibleBuffer;
					objectsLeavingVisibleBuffer.Clear();
					List<IChunkableObject> visibleCandidatesBuffer = chunkManagerLayer.visibleCandidatesBuffer;
					visibleCandidatesBuffer.Clear();
					List<IChunkableObject> prewarmCandidatesBuffer = chunkManagerLayer.prewarmCandidatesBuffer;
					prewarmCandidatesBuffer.Clear();
					for (int num = 0; num < chunkManagerLayer.Count; num++)
					{
						Chunk chunk2 = chunkManagerLayer.chunks[num];
						ChunkVisibilityState chunkVisibilityState2 = (ChunkVisibilityState)chunkManagerLayer.chunkVisibilityStateResults[num];
						bool isVisible = chunk2.isVisible;
						bool flag = chunkVisibilityState2 == ChunkVisibilityState.Visible;
						chunk2.isVisible = flag;
						if (isVisible && !flag)
						{
							objectsLeavingVisibleBuffer.AddRange(chunk2.chunkableObjects);
						}
						if (!chunk2.IgnoreChunkVisibility)
						{
							if (chunkVisibilityState2 == ChunkVisibilityState.Visible)
							{
								for (int num2 = 0; num2 < chunk2.Count; num2++)
								{
									IChunkableObject chunkableObject3 = chunk2.chunkableObjects[num2];
									if (ChunkManager.IsChunkableObjectAlive(chunkableObject3) && !chunkableObject3.IgnoreChunkVisibility)
									{
										visibleCandidatesBuffer.Add(chunkableObject3);
									}
								}
							}
							else if (chunkVisibilityState2 == ChunkVisibilityState.Prewarm)
							{
								for (int num3 = 0; num3 < chunk2.Count; num3++)
								{
									IChunkableObject chunkableObject4 = chunk2.chunkableObjects[num3];
									if (ChunkManager.IsChunkableObjectAlive(chunkableObject4) && !chunkableObject4.IgnoreChunkVisibility)
									{
										prewarmCandidatesBuffer.Add(chunkableObject4);
									}
								}
							}
						}
					}
					HashSet<IChunkableObject> currentlyVisibleObjectsBuffer = chunkManagerLayer.currentlyVisibleObjectsBuffer;
					currentlyVisibleObjectsBuffer.Clear();
					if (visibleCandidatesBuffer.Count > 0)
					{
						chunkManagerLayer.visibleJobBuffer.EnsureCapacity(visibleCandidatesBuffer.Count);
						NativeArray<BurstableBounds> data2 = chunkManagerLayer.visibleJobBuffer.data;
						NativeArray<byte> results2 = chunkManagerLayer.visibleJobBuffer.results;
						for (int num4 = 0; num4 < visibleCandidatesBuffer.Count; num4++)
						{
							data2[num4] = visibleCandidatesBuffer[num4].GetChunkableData();
						}
						new ChunkVisibilityJob
						{
							plane0 = burstablePlane,
							plane1 = burstablePlane2,
							plane2 = burstablePlane3,
							plane3 = burstablePlane4,
							plane4 = burstablePlane5,
							plane5 = burstablePlane6,
							chunkableDataArray = data2,
							visibilityStateResults = results2,
							expandFactor = chunkManagerLayer.expandFactor,
							prewarmPlanePadding = chunkManagerLayer.prewarmPlanePadding
						}.Schedule(visibleCandidatesBuffer.Count, 4, default(JobHandle)).Complete();
						for (int num5 = 0; num5 < visibleCandidatesBuffer.Count; num5++)
						{
							if (results2[num5] == 2)
							{
								currentlyVisibleObjectsBuffer.Add(visibleCandidatesBuffer[num5]);
							}
						}
					}
					HashSet<IChunkableObject> currentlyPrewarmedObjectsBuffer = chunkManagerLayer.currentlyPrewarmedObjectsBuffer;
					currentlyPrewarmedObjectsBuffer.Clear();
					if (prewarmCandidatesBuffer.Count > 0)
					{
						chunkManagerLayer.prewarmJobBuffer.EnsureCapacity(prewarmCandidatesBuffer.Count);
						NativeArray<BurstableBounds> data3 = chunkManagerLayer.prewarmJobBuffer.data;
						NativeArray<byte> results3 = chunkManagerLayer.prewarmJobBuffer.results;
						for (int num6 = 0; num6 < prewarmCandidatesBuffer.Count; num6++)
						{
							data3[num6] = prewarmCandidatesBuffer[num6].GetChunkableData();
						}
						new ChunkVisibilityJob
						{
							plane0 = burstablePlane,
							plane1 = burstablePlane2,
							plane2 = burstablePlane3,
							plane3 = burstablePlane4,
							plane4 = burstablePlane5,
							plane5 = burstablePlane6,
							chunkableDataArray = data3,
							visibilityStateResults = results3,
							expandFactor = chunkManagerLayer.expandFactor,
							prewarmPlanePadding = chunkManagerLayer.prewarmPlanePadding
						}.Schedule(prewarmCandidatesBuffer.Count, 4, default(JobHandle)).Complete();
						for (int num7 = 0; num7 < prewarmCandidatesBuffer.Count; num7++)
						{
							IChunkableObject chunkableObject5 = prewarmCandidatesBuffer[num7];
							if (!currentlyVisibleObjectsBuffer.Contains(chunkableObject5))
							{
								ChunkVisibilityState chunkVisibilityState3 = (ChunkVisibilityState)results3[num7];
								if (chunkVisibilityState3 == ChunkVisibilityState.Prewarm || chunkVisibilityState3 == ChunkVisibilityState.Visible)
								{
									currentlyPrewarmedObjectsBuffer.Add(chunkableObject5);
								}
							}
						}
					}
					HashSet<IChunkableObject> objectsToHideBuffer = chunkManagerLayer.objectsToHideBuffer;
					objectsToHideBuffer.Clear();
					foreach (IChunkableObject chunkableObject6 in objectsLeavingVisibleBuffer)
					{
						if (!currentlyVisibleObjectsBuffer.Contains(chunkableObject6) && !currentlyPrewarmedObjectsBuffer.Contains(chunkableObject6))
						{
							objectsToHideBuffer.Add(chunkableObject6);
						}
					}
					foreach (IChunkableObject chunkableObject7 in visibleCandidatesBuffer)
					{
						if (!currentlyVisibleObjectsBuffer.Contains(chunkableObject7) && !currentlyPrewarmedObjectsBuffer.Contains(chunkableObject7))
						{
							objectsToHideBuffer.Add(chunkableObject7);
						}
					}
					foreach (IChunkableObject chunkableObject8 in chunkManagerLayer.prewarmedObjects)
					{
						if (!currentlyVisibleObjectsBuffer.Contains(chunkableObject8) && !currentlyPrewarmedObjectsBuffer.Contains(chunkableObject8))
						{
							objectsToHideBuffer.Add(chunkableObject8);
						}
					}
					chunkManagerLayer.prewarmedObjects.Clear();
					chunkManagerLayer.prewarmedObjects.UnionWith(currentlyPrewarmedObjectsBuffer);
					foreach (IChunkableObject chunkableObject9 in currentlyVisibleObjectsBuffer)
					{
						this.DispatchChunkVisibilityState(chunkableObject9, ChunkVisibilityState.Visible);
					}
					foreach (IChunkableObject chunkableObject10 in currentlyPrewarmedObjectsBuffer)
					{
						this.DispatchChunkVisibilityState(chunkableObject10, ChunkVisibilityState.Prewarm);
					}
					foreach (IChunkableObject chunkableObject11 in objectsToHideBuffer)
					{
						this.DispatchChunkVisibilityState(chunkableObject11, ChunkVisibilityState.OutOfRange);
					}
				}
			}
		}
		this.ProcessPendingVisibilityRechecks(burstablePlane, burstablePlane2, burstablePlane3, burstablePlane4, burstablePlane5, burstablePlane6);
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x000E22F4 File Offset: 0x000E04F4
	private void ProcessPendingVisibilityRechecks(BurstablePlane plane0, BurstablePlane plane1, BurstablePlane plane2, BurstablePlane plane3, BurstablePlane plane4, BurstablePlane plane5)
	{
		if (this.pendingVisibilityRecheckObjects.Count == 0)
		{
			return;
		}
		List<IChunkableObject> list = this.pendingVisibilityRecheckObjects.ToList<IChunkableObject>();
		this.pendingVisibilityRecheckObjects.Clear();
		foreach (IChunkableObject chunkableObject in list)
		{
			if (ChunkManager.IsChunkableObjectAlive(chunkableObject))
			{
				ChunkManagerLayer chunkManagerLayer;
				if (chunkableObject.IgnoreChunkVisibility)
				{
					this.DispatchChunkVisibilityState(chunkableObject, ChunkVisibilityState.Visible);
				}
				else if (this.TryGetLayerForChunkableObject(chunkableObject, out chunkManagerLayer))
				{
					ChunkVisibilityState chunkVisibilityState = this.CalculateObjectVisibilityState(chunkableObject, chunkManagerLayer, plane0, plane1, plane2, plane3, plane4, plane5);
					this.DispatchChunkVisibilityState(chunkableObject, chunkVisibilityState);
				}
			}
		}
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x000E23A0 File Offset: 0x000E05A0
	private bool TryGetLayerForChunkableObject(IChunkableObject chunkableObject, out ChunkManagerLayer ownerLayer)
	{
		for (int i = 0; i < this.layers.Count; i++)
		{
			ChunkManagerLayer chunkManagerLayer = this.layers[i];
			for (int j = 0; j < chunkManagerLayer.Count; j++)
			{
				if (chunkManagerLayer.chunks[j].ContainsChunkableObject(chunkableObject))
				{
					ownerLayer = chunkManagerLayer;
					return true;
				}
			}
		}
		ownerLayer = null;
		return false;
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x000E23FC File Offset: 0x000E05FC
	private ChunkVisibilityState CalculateObjectVisibilityState(IChunkableObject chunkableObject, ChunkManagerLayer layer, BurstablePlane plane0, BurstablePlane plane1, BurstablePlane plane2, BurstablePlane plane3, BurstablePlane plane4, BurstablePlane plane5)
	{
		this.singleObjectJobBuffer.EnsureCapacity(1);
		NativeArray<BurstableBounds> data = this.singleObjectJobBuffer.data;
		NativeArray<byte> results = this.singleObjectJobBuffer.results;
		data[0] = chunkableObject.GetChunkableData();
		new ChunkVisibilityJob
		{
			plane0 = plane0,
			plane1 = plane1,
			plane2 = plane2,
			plane3 = plane3,
			plane4 = plane4,
			plane5 = plane5,
			chunkableDataArray = data,
			visibilityStateResults = results,
			expandFactor = layer.expandFactor,
			prewarmPlanePadding = layer.prewarmPlanePadding
		}.Schedule(1, 1, default(JobHandle)).Complete();
		return (ChunkVisibilityState)results[0];
	}

	// Token: 0x06002F2E RID: 12078 RVA: 0x000E24C4 File Offset: 0x000E06C4
	private void OnDestroy()
	{
		this.pendingVisibilityRecheckObjects.Clear();
		foreach (ChunkManagerLayer chunkManagerLayer in this.layers)
		{
			chunkManagerLayer.DisposeChunks();
			chunkManagerLayer.Dispose();
			chunkManagerLayer.DisposeVisibilityJobBuffers();
		}
		this.singleObjectJobBuffer.Dispose();
	}

	// Token: 0x06002F2F RID: 12079 RVA: 0x000E2538 File Offset: 0x000E0738
	private bool IsObjectIntersectsChunk(IChunkableObject chunkableObject, Chunk chunk)
	{
		if (!ChunkManager.IsChunkableObjectAlive(chunkableObject))
		{
			return false;
		}
		BurstableBounds chunkableData = chunk.GetChunkableData();
		BurstableBounds chunkableData2 = chunkableObject.GetChunkableData();
		chunkableData.center = new float3(chunkableData.center.x, 0f, chunkableData.center.z);
		chunkableData2.center = new float3(chunkableData2.center.x, 0f, chunkableData2.center.z);
		return chunkableData2.Intersects(chunkableData);
	}

	// Token: 0x06002F30 RID: 12080 RVA: 0x000E25B4 File Offset: 0x000E07B4
	private static bool IsChunkableObjectAlive(IChunkableObject chunkableObject)
	{
		if (chunkableObject != null)
		{
			global::UnityEngine.Object @object = chunkableObject as global::UnityEngine.Object;
			return @object == null || @object != null;
		}
		return false;
	}

	// Token: 0x06002F31 RID: 12081 RVA: 0x000E25DC File Offset: 0x000E07DC
	private void DispatchChunkVisibilityState(IChunkableObject chunkableObject, ChunkVisibilityState state)
	{
		if (!ChunkManager.IsChunkableObjectAlive(chunkableObject))
		{
			return;
		}
		if (chunkableObject.IgnoreChunkVisibility)
		{
			return;
		}
		IChunkVisibilityStateReceiver chunkVisibilityStateReceiver = chunkableObject as IChunkVisibilityStateReceiver;
		if (chunkVisibilityStateReceiver != null)
		{
			chunkVisibilityStateReceiver.UpdateChunkVisibilityState(state);
		}
		chunkableObject.UpdateChunkVisibility(state == ChunkVisibilityState.Visible);
	}

	// Token: 0x040025FF RID: 9727
	private const int JOB_INNER_LOOP_BATCH_COUNT = 4;

	// Token: 0x04002600 RID: 9728
	private const float CHUNK_HEIGHT = 30f;

	// Token: 0x04002601 RID: 9729
	private static float3 chunkSize = new float3(18f, 30f, 10f);

	// Token: 0x04002602 RID: 9730
	private List<ChunkManagerLayer> layers = new List<ChunkManagerLayer>();

	// Token: 0x04002603 RID: 9731
	private HashSet<IChunkableObject> pendingVisibilityRecheckObjects = new HashSet<IChunkableObject>();

	// Token: 0x04002604 RID: 9732
	private bool isInitialized;

	// Token: 0x04002605 RID: 9733
	private bool isProcessingVisibility;

	// Token: 0x04002606 RID: 9734
	private readonly Plane[] cachedFrustumPlanes = new Plane[6];

	// Token: 0x04002607 RID: 9735
	private readonly BurstablePlane[] cachedBurstablePlanes = new BurstablePlane[6];

	// Token: 0x04002608 RID: 9736
	private readonly ChunkVisibilityJobBuffer singleObjectJobBuffer = new ChunkVisibilityJobBuffer();
}
