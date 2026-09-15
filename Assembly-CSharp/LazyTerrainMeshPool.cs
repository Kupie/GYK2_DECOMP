using System;
using Cysharp.Threading.Tasks;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x020006BC RID: 1724
public class LazyTerrainMeshPool : LazySingleton<LazyTerrainMeshPool>, IProgress<float>
{
	// Token: 0x17000726 RID: 1830
	// (get) Token: 0x06002DCF RID: 11727 RVA: 0x000DB1D8 File Offset: 0x000D93D8
	// (set) Token: 0x06002DD0 RID: 11728 RVA: 0x000DB1E0 File Offset: 0x000D93E0
	public float LocalProgress { get; private set; }

	// Token: 0x06002DD1 RID: 11729 RVA: 0x000DB1EC File Offset: 0x000D93EC
	public async UniTask InitAsync()
	{
		GameShutdown.ThrowIfRequested();
		this.Report(0f);
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Prefabs/LazyTerrainMeshPrefab.prefab");
		AsyncOperationHandle<GameObject> asyncOperationHandle2 = Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Prefabs/DeformingGrassPrefab.prefab");
		float mesh01 = 0f;
		float grass01 = 0f;
		Progress<float> progress = new Progress<float>(delegate(float p)
		{
			mesh01 = p;
			base.<InitAsync>g__OnAddressablesProgressChanged|0();
		});
		Progress<float> progress2 = new Progress<float>(delegate(float p)
		{
			grass01 = p;
			base.<InitAsync>g__OnAddressablesProgressChanged|0();
		});
		ValueTuple<GameObject, GameObject> valueTuple = await UniTask.WhenAll<GameObject, GameObject>(asyncOperationHandle.ToUniTask(progress, PlayerLoopTiming.Update, GameShutdown.Token, true, true), asyncOperationHandle2.ToUniTask(progress2, PlayerLoopTiming.Update, GameShutdown.Token, true, true));
		GameObject goMesh = valueTuple.Item1;
		GameObject goGrass = valueTuple.Item2;
		GameShutdown.ThrowIfRequested();
		this.Report(0.8f);
		if (BackgroundLoading.IsActive)
		{
			await UniTask.Yield(PlayerLoopTiming.Update, GameShutdown.Token, false);
		}
		this.meshPrefab = goMesh.GetComponent<LazyTerrainMesh>();
		if (this.meshesPool == null)
		{
			this.meshesPool = LazyPooler.CreatePool<LazyTerrainMesh>(this.meshPrefab, 450, Pool.PoolType.ImmediateActivation, true, false, null);
		}
		this.Report(0.9f);
		if (BackgroundLoading.IsActive)
		{
			await UniTask.Yield(PlayerLoopTiming.Update, GameShutdown.Token, false);
		}
		this.grassPrefab = goGrass.GetComponent<DeformingGrass>();
		if (this.grassPool == null)
		{
			this.grassPool = LazyPooler.CreatePool<DeformingGrass>(this.grassPrefab, 80, Pool.PoolType.ImmediateActivation, true, false, null);
		}
		this.Report(1f);
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06002DD2 RID: 11730 RVA: 0x000DB22F File Offset: 0x000D942F
	public static LazyTerrainMesh GetMesh()
	{
		return LazySingleton<LazyTerrainMeshPool>.Instance.meshesPool.GetOrCreateObject<LazyTerrainMesh>();
	}

	// Token: 0x06002DD3 RID: 11731 RVA: 0x000DB240 File Offset: 0x000D9440
	public static void ReleaseMesh(LazyTerrainMesh mesh)
	{
		if (mesh == null)
		{
			return;
		}
		if (LazySingleton<LazyTerrainMeshPool>.Instance == null || LazySingleton<LazyTerrainMeshPool>.Instance.meshesPool == null)
		{
			return;
		}
		LazySingleton<LazyTerrainMeshPool>.Instance.meshesPool.ReleaseObject<LazyTerrainMesh>(mesh);
	}

	// Token: 0x06002DD4 RID: 11732 RVA: 0x000DB276 File Offset: 0x000D9476
	public static DeformingGrass GetGrass()
	{
		return LazySingleton<LazyTerrainMeshPool>.Instance.grassPool.GetOrCreateObject<DeformingGrass>();
	}

	// Token: 0x06002DD5 RID: 11733 RVA: 0x000DB287 File Offset: 0x000D9487
	public static void ReleaseGrass(DeformingGrass grass)
	{
		if (grass == null)
		{
			return;
		}
		if (LazySingleton<LazyTerrainMeshPool>.Instance == null || LazySingleton<LazyTerrainMeshPool>.Instance.grassPool == null)
		{
			return;
		}
		LazySingleton<LazyTerrainMeshPool>.Instance.grassPool.ReleaseObject<DeformingGrass>(grass);
	}

	// Token: 0x06002DD6 RID: 11734 RVA: 0x000DB2BD File Offset: 0x000D94BD
	public void Report(float value)
	{
		this.LocalProgress = Mathf.Clamp01(value);
	}

	// Token: 0x040024DF RID: 9439
	public const string PATH_TO_MESH_PREFAB = "Assets/AddressableAssets/Prefabs/LazyTerrainMeshPrefab.prefab";

	// Token: 0x040024E0 RID: 9440
	public const string PATH_TO_GRASS_PREFAB = "Assets/AddressableAssets/Prefabs/DeformingGrassPrefab.prefab";

	// Token: 0x040024E1 RID: 9441
	private Pool meshesPool;

	// Token: 0x040024E2 RID: 9442
	private Pool grassPool;

	// Token: 0x040024E3 RID: 9443
	private LazyTerrainMesh meshPrefab;

	// Token: 0x040024E4 RID: 9444
	private DeformingGrass grassPrefab;
}
