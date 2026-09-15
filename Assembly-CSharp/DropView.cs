using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;
using UnityEngine.AddressableAssets;

// Token: 0x0200018B RID: 395
public class DropView : MonoBehaviour, IBubbleDrawable, IChunkableObject, IPoolable
{
	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060009AD RID: 2477 RVA: 0x00030B52 File Offset: 0x0002ED52
	public static float PreloadProgress
	{
		get
		{
			return DropView.preloadProgress;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060009AE RID: 2478 RVA: 0x00030B59 File Offset: 0x0002ED59
	// (set) Token: 0x060009AF RID: 2479 RVA: 0x00030B61 File Offset: 0x0002ED61
	public DropData Data
	{
		get
		{
			return this.data;
		}
		set
		{
			this.data = value;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00030B6A File Offset: 0x0002ED6A
	public IInteractionHandler InteractionHandler
	{
		get
		{
			return this.interactionHandler;
		}
	}

	// Token: 0x1700018A RID: 394
	// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00030B72 File Offset: 0x0002ED72
	public bool IsDespawning
	{
		get
		{
			return this.isDespawning;
		}
	}

	// Token: 0x1700018B RID: 395
	// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00030B7A File Offset: 0x0002ED7A
	private bool CanProcessWorldEvents
	{
		get
		{
			return !this.isReleasedToPool && !this.isDespawning && this.data != null;
		}
	}

	// Token: 0x1700018C RID: 396
	// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00030B97 File Offset: 0x0002ED97
	public bool IsCollectDelayed
	{
		get
		{
			return this.isCollectDelayed;
		}
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00030B9F File Offset: 0x0002ED9F
	public bool IsPhysicDisabled
	{
		get
		{
			return this.isPhysicDisabled;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x060009B5 RID: 2485 RVA: 0x00030BA7 File Offset: 0x0002EDA7
	// (set) Token: 0x060009B6 RID: 2486 RVA: 0x00030BAF File Offset: 0x0002EDAF
	public bool IsTimedCollecting { get; private set; }

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00030BB8 File Offset: 0x0002EDB8
	public bool CanBeMerged
	{
		get
		{
			return !this.isDespawning && this.data != null && !this.isMergeDelayed && !this.isPhysicDisabled && !this.IsTimedCollecting;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x060009B8 RID: 2488 RVA: 0x00030BE5 File Offset: 0x0002EDE5
	// (set) Token: 0x060009B9 RID: 2489 RVA: 0x00030BED File Offset: 0x0002EDED
	public bool IsRiverDump { get; private set; }

	// Token: 0x060009BA RID: 2490 RVA: 0x00030BF6 File Offset: 0x0002EDF6
	private void Awake()
	{
		if (this.bigItemMeshesTransform != null)
		{
			this.bigItemMeshesStartLocalPosition = this.bigItemMeshesTransform.localPosition;
		}
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00030C18 File Offset: 0x0002EE18
	public static async UniTask PreloadAsync()
	{
		if (DropView.smallDropPool != null && DropView.bigDropPool != null)
		{
			DropView.preloadProgress = 1f;
		}
		else
		{
			GameShutdown.ThrowIfRequested();
			object obj = await UniTask.WhenAll<GameObject, GameObject, GameObject>(DropView.LoadPrefabAsync("Assets/AddressableAssets/Drops/Drop.prefab"), DropView.LoadPrefabAsync("Assets/AddressableAssets/Drops/SmallItem.prefab"), DropView.LoadPrefabAsync("Assets/AddressableAssets/Drops/BigItem.prefab"));
			GameObject item = obj.Item1;
			GameObject item2 = obj.Item2;
			GameObject item3 = obj.Item3;
			DropView.CreatePools(item, item2, item3);
			DropView.preloadProgress = 1f;
		}
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x00030C54 File Offset: 0x0002EE54
	private static async UniTask<GameObject> LoadPrefabAsync(string key)
	{
		object obj = await Addressables.LoadAssetAsync<GameObject>(key).ToUniTask(null, PlayerLoopTiming.Update, GameShutdown.Token, true, true);
		if (obj == null)
		{
			Debug.LogError("[DropView] Failed to preload prefab at [" + key + "]");
		}
		return obj;
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x00030C98 File Offset: 0x0002EE98
	private static void EnsurePools()
	{
		if (DropView.smallDropPool != null && DropView.bigDropPool != null)
		{
			return;
		}
		if (GameShutdown.IsRequested)
		{
			return;
		}
		GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Drops/Drop.prefab").WaitForCompletion();
		GameObject gameObject2 = Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Drops/SmallItem.prefab").WaitForCompletion();
		GameObject gameObject3 = Addressables.LoadAssetAsync<GameObject>("Assets/AddressableAssets/Drops/BigItem.prefab").WaitForCompletion();
		DropView.CreatePools(gameObject, gameObject2, gameObject3);
		DropView.preloadProgress = 1f;
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x00030D08 File Offset: 0x0002EF08
	private static void CreatePools(GameObject shellPrefab, GameObject smallAtomPrefab, GameObject bigAtomPrefab)
	{
		if (shellPrefab == null || smallAtomPrefab == null || bigAtomPrefab == null)
		{
			return;
		}
		if (DropView.smallDropPool != null && DropView.bigDropPool != null)
		{
			return;
		}
		DropView.EnsurePrototypeHolder();
		if (DropView.smallDropPool == null)
		{
			DropView dropView = DropView.CreateComposedPrototype(shellPrefab, smallAtomPrefab, ItemSize.Small);
			DropView.smallDropPool = LazyPooler.CreatePoolById("DropView.Small", dropView, 0, Pool.PoolType.ImmediateActivation, true, false, null);
		}
		if (DropView.bigDropPool == null)
		{
			DropView dropView2 = DropView.CreateComposedPrototype(shellPrefab, bigAtomPrefab, ItemSize.Big);
			DropView.bigDropPool = LazyPooler.CreatePoolById("DropView.Big", dropView2, 0, Pool.PoolType.ImmediateActivation, true, false, null);
		}
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x00030D8F File Offset: 0x0002EF8F
	private static void EnsurePrototypeHolder()
	{
		if (DropView.poolPrototypeHolder != null)
		{
			return;
		}
		DropView.poolPrototypeHolder = new GameObject("[DropViewPoolPrototypes]");
		global::UnityEngine.Object.DontDestroyOnLoad(DropView.poolPrototypeHolder);
		DropView.poolPrototypeHolder.SetActive(false);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00030DC4 File Offset: 0x0002EFC4
	private static DropView CreateComposedPrototype(GameObject shellPrefab, GameObject atomPrefab, ItemSize size)
	{
		DropView component = global::UnityEngine.Object.Instantiate<GameObject>(shellPrefab, DropView.poolPrototypeHolder.transform).GetComponent<DropView>();
		component.gameObject.name = ((size == ItemSize.Small) ? "DropView_Small" : "DropView_Big");
		DropView.ClearShellChildren(component.transform);
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(atomPrefab, component.transform, false);
		gameObject.name = ((size == ItemSize.Small) ? "SmallItem" : "BigItem");
		if (size == ItemSize.Big)
		{
			gameObject.transform.localPosition = Vector3.zero;
		}
		component.BindAtom(gameObject, size);
		component.gameObject.SetActive(false);
		return component;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x00030E5C File Offset: 0x0002F05C
	private static void ClearShellChildren(Transform shell)
	{
		for (int i = shell.childCount - 1; i >= 0; i--)
		{
			Transform child = shell.GetChild(i);
			if (!(child.name == "BigDropTopSurface"))
			{
				global::UnityEngine.Object.DestroyImmediate(child.gameObject);
			}
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x00030EA4 File Offset: 0x0002F0A4
	private void BindAtom(GameObject atom, ItemSize size)
	{
		this.atomSize = size;
		this.bounds = ((size == ItemSize.Small) ? DropView.SmallDropBounds : DropView.BigDropBounds);
		this.smallItem = null;
		this.bigItem = null;
		this.bigItemMeshesTransform = null;
		this.smallItemCollider = null;
		if (size == ItemSize.Small)
		{
			this.smallItem = atom.GetComponent<DropViewAtomBase>();
			TriggerColliderComponentLinker componentInChildren = atom.GetComponentInChildren<TriggerColliderComponentLinker>(true);
			if (componentInChildren != null)
			{
				componentInChildren.Component = this;
				this.smallItemCollider = componentInChildren.GetComponent<CapsuleCollider>();
			}
			return;
		}
		this.bigItem = atom.GetComponent<DropViewAtomBase>();
		Transform transform = atom.transform.Find("Views");
		this.bigItemMeshesTransform = ((transform != null) ? transform : atom.transform);
		this.bigItemMeshesStartLocalPosition = this.bigItemMeshesTransform.localPosition;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x00030F65 File Offset: 0x0002F165
	private static Pool GetPool(DropView view)
	{
		if (view != null && view.smallItem != null)
		{
			return DropView.smallDropPool;
		}
		return DropView.bigDropPool;
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00030F89 File Offset: 0x0002F189
	private static Pool GetPool(ItemSize size)
	{
		if (size != ItemSize.Small)
		{
			return DropView.bigDropPool;
		}
		return DropView.smallDropPool;
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00030F9C File Offset: 0x0002F19C
	public static DropView SpawnDrop(DropData drop, Transform parent, bool skipWorldPlacement = false)
	{
		DropView.EnsurePools();
		Pool pool = DropView.GetPool(drop.Size);
		if (pool == null)
		{
			Debug.LogError("[DropView] Failed to spawn drop [" + ((drop != null) ? drop.Id : null) + "]. Pool is null.");
			return null;
		}
		DropView orCreateObject = pool.GetOrCreateObject<DropView>();
		orCreateObject.transform.SetParent(parent, false);
		orCreateObject.isReleasedToPool = false;
		orCreateObject.isDespawning = false;
		orCreateObject.ResetAfterPoolGet();
		orCreateObject.Data = drop;
		DropView dropView = orCreateObject;
		bool flag;
		if (drop.DropType == DropType.Item)
		{
			Item item = drop.Item;
			if (((item != null) ? item.Definition : null) != null)
			{
				flag = drop.Item.Definition.CanNotBeDestroyed;
				goto IL_0095;
			}
		}
		flag = false;
		IL_0095:
		dropView.ignoresItemCollisions = flag;
		orCreateObject.UpdateChunkVisibility(true);
		LazySingleton<ChunkManager>.Instance.RegisterDynamicChunkableObject(orCreateObject, ChunkManagerLayerType.DropView);
		orCreateObject.registeredInChunkManager = true;
		Transform transform = orCreateObject.transform;
		orCreateObject.isKicked = false;
		orCreateObject.rb.linearVelocity = Vector3.zero;
		orCreateObject.rb.angularVelocity = Vector3.zero;
		if (!skipWorldPlacement && !orCreateObject.Data.IsDroppedFromPlayer)
		{
			if (orCreateObject.Data.Size == ItemSize.Small)
			{
				orCreateObject.DoKick(orCreateObject.CorrectSpawnPos().normalized, 0.33f);
			}
			else
			{
				Vector3 vector = orCreateObject.Data.Position;
				bool flag2 = false;
				List<Vector3> occupiedBigDropPositions = DropView.GetOccupiedBigDropPositions(orCreateObject.Data.UniqueId);
				if (SpecialPhysicsCastUtils.GetSpawnPosForBigDropRectangular(orCreateObject.data.Position, LazyConsts.BIG_DROP_COLLIDER_SIZE, 0.19999999f, 20, 0.19999999f, 20, out vector, occupiedBigDropPositions))
				{
					flag2 = true;
				}
				if (!flag2)
				{
					vector = DropView.GetSpawnPosFromSample(orCreateObject.Data.Position);
				}
				orCreateObject.Data.Position = vector;
				Array.Clear(DropView.overlapColliders, 0, 50);
				Vector3 vector2 = orCreateObject.Data.Position + Vector3.up * (LazyConsts.BIG_DROP_COLLIDER_SIZE.y / 2f + 0.1f);
				Vector3 vector3 = LazyConsts.BIG_DROP_COLLIDER_SIZE / 2f;
				Physics.OverlapBoxNonAlloc(vector2, vector3, DropView.overlapColliders, Quaternion.Euler(Vector3.zero), 65536);
				float num = Mathf.Max(LazyConsts.BIG_DROP_COLLIDER_SIZE.x, LazyConsts.BIG_DROP_COLLIDER_SIZE.z);
				foreach (Collider collider in DropView.overlapColliders)
				{
					if (!(collider == null))
					{
						DropView componentInParent = collider.gameObject.GetComponentInParent<DropView>();
						if (!(componentInParent == null) && !(componentInParent == orCreateObject) && !componentInParent.isPhysicDisabled)
						{
							Vector3 vector4 = componentInParent.transform.position - orCreateObject.Data.Position;
							componentInParent.SeparateAlongSurfaceFrom(orCreateObject.Data.Position, vector4, num);
							componentInParent.StartCoroutine(componentInParent.BeKinematicDropForGivenFrames(2));
						}
					}
				}
			}
		}
		bool flag3 = drop.DropType != DropType.Item || drop.Item.Definition.itemGroupIds.Contains("body");
		orCreateObject.interactionHandler = (flag3 ? new ZombieDropInteractionHandler().Init(orCreateObject) : new BigDropInteractionHandler().Init(orCreateObject));
		transform.position = orCreateObject.Data.Position;
		orCreateObject.rb.position = orCreateObject.Data.Position;
		orCreateObject.Data.OnCountChanged += orCreateObject.UpdateTextSprite;
		orCreateObject.UpdateView();
		if (!skipWorldPlacement)
		{
			orCreateObject.UpdateEffects();
			orCreateObject.StartCoroutine(orCreateObject.MergeDelayCoroutine());
			orCreateObject.StartCoroutine(orCreateObject.CollectDelayCoroutine());
		}
		orCreateObject.UpdateTextSprite();
		orCreateObject.PlaySpawnScaleTween();
		if (orCreateObject.smallItemCollider != null)
		{
			orCreateObject.SetColliderSmall();
		}
		if (!skipWorldPlacement)
		{
			DropView.PlayItemDropSound(orCreateObject);
		}
		return orCreateObject;
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0003133C File Offset: 0x0002F53C
	public static void PlayItemDropSound(DropView dropView)
	{
		List<string> itemGroupIds = dropView.Data.Item.Definition.itemGroupIds;
		if (itemGroupIds.Contains("zombie"))
		{
			LazyAudio.PlayAtGameObject("oh_zombie_drop", dropView.transform, SpatialType.sound3D, true);
			return;
		}
		if (itemGroupIds.Contains("corpse"))
		{
			LazyAudio.PlayAtGameObject("oh_corpse_drop", dropView.transform, SpatialType.sound3D, true);
			return;
		}
		if (dropView.Data.Item.id == "wood")
		{
			LazyAudio.PlayAtGameObject("oh_wood_drop", dropView.transform, SpatialType.sound3D, true);
			return;
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x000313D4 File Offset: 0x0002F5D4
	private static List<Vector3> GetOccupiedBigDropPositions(SGuid excludeUniqueId)
	{
		List<Vector3> list = new List<Vector3>();
		GameSceneData gameSceneDataById = MainGame.Instance.GameSave.WorldData.GetGameSceneDataById(MainGame.PlayerData.currentGameSceneId);
		if (gameSceneDataById == null)
		{
			return list;
		}
		foreach (DropData dropData in gameSceneDataById.droppedItems)
		{
			if (dropData != null && !dropData.IsRemoving && !(dropData.UniqueId == excludeUniqueId) && dropData.Size == ItemSize.Big)
			{
				list.Add(dropData.Position);
			}
		}
		return list;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0003147C File Offset: 0x0002F67C
	private void SeparateAlongSurfaceFrom(Vector3 anchorWorldPos, Vector3 preferredAway, float minSeparation)
	{
		RaycastHit raycastHit;
		Vector3 vector = (this.TryGetGroundNormal(out raycastHit) ? raycastHit.normal : Vector3.up);
		Vector3 vector2 = Vector3.ProjectOnPlane(base.transform.position - anchorWorldPos, vector);
		float magnitude = vector2.magnitude;
		if (magnitude >= minSeparation)
		{
			return;
		}
		Vector3 vector3 = ((magnitude * magnitude > 1E-06f) ? vector2.normalized : DropView.GetSurfaceSeparationDirection(preferredAway, vector));
		float num = minSeparation - magnitude;
		Vector3 vector4 = base.transform.position + vector3 * num;
		Vector3 vector5;
		if (DropView.TrySnapToGround(vector4, out vector5))
		{
			vector4 = vector5;
		}
		this.ApplySyncedWorldPosition(VisualConsts.GetRoundedPosXZ(vector4, 2, 1));
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00031524 File Offset: 0x0002F724
	private void UnstackAlongSurface(Vector3 penetrationDirection, float penetrationDistance)
	{
		RaycastHit raycastHit;
		Vector3 vector = (this.TryGetGroundNormal(out raycastHit) ? raycastHit.normal : Vector3.up);
		Vector3 surfaceSeparationDirection = DropView.GetSurfaceSeparationDirection(penetrationDirection, vector);
		float num = Mathf.Max(penetrationDistance, 0.05f);
		Vector3 vector2 = base.transform.position + surfaceSeparationDirection * num;
		Vector3 vector3;
		if (DropView.TrySnapToGround(vector2, out vector3))
		{
			vector2 = vector3;
		}
		this.ApplySyncedWorldPosition(VisualConsts.GetRoundedPosXZ(vector2, 2, 1));
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x00031598 File Offset: 0x0002F798
	private static Vector3 GetSurfaceSeparationDirection(Vector3 rawSep, Vector3 groundNormal)
	{
		Vector3 vector = Vector3.ProjectOnPlane(rawSep, groundNormal);
		if (vector.sqrMagnitude > 1E-06f)
		{
			return vector.normalized;
		}
		Vector3 vector2 = rawSep.XZ();
		vector = Vector3.ProjectOnPlane((vector2.sqrMagnitude > 1E-06f) ? vector2 : Vector3.right, groundNormal);
		if (vector.sqrMagnitude > 1E-06f)
		{
			return vector.normalized;
		}
		Vector3 vector3 = Vector3.Cross(groundNormal, Vector3.up);
		if (vector3.sqrMagnitude <= 1E-06f)
		{
			vector3 = Vector3.Cross(groundNormal, Vector3.right);
		}
		if (vector3.sqrMagnitude <= 1E-06f)
		{
			return Vector3.right;
		}
		return vector3.normalized;
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0003163E File Offset: 0x0002F83E
	private bool TryGetGroundNormal(out RaycastHit groundHit)
	{
		return SpecialPhysicsCastUtils.TryGetTopmostGroundHit(base.transform.position, out groundHit);
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x00031651 File Offset: 0x0002F851
	private static bool TrySnapToGround(Vector3 worldPos, out Vector3 snapped)
	{
		return SpecialPhysicsCastUtils.TrySnapDropPosToTopmostGround(worldPos, out snapped);
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0003165C File Offset: 0x0002F85C
	private void SyncBigDropTopSurface(bool physicsEnabled)
	{
		if (this.bigDropTopSurface == null)
		{
			return;
		}
		if (this.data == null || this.data.Size != ItemSize.Big || !physicsEnabled || this.ignoresItemCollisions)
		{
			this.bigDropTopSurface.gameObject.SetActive(false);
			return;
		}
		DropViewAtomMesh dropViewAtomMesh = this.activeDropViewAtom as DropViewAtomMesh;
		Collider collider = ((dropViewAtomMesh != null && dropViewAtomMesh.MeshElement != null) ? dropViewAtomMesh.MeshElement.physicsCollider : null);
		if (collider == null)
		{
			this.bigDropTopSurface.gameObject.SetActive(false);
			return;
		}
		this.ApplyBigDropTopSurfaceShape(collider);
		this.bigDropTopSurface.gameObject.SetActive(true);
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x00031710 File Offset: 0x0002F910
	private void ApplyBigDropTopSurfaceShape(Collider phys)
	{
		Bounds bounds = phys.bounds;
		float num = 0.04f;
		float num2 = num / Mathf.Max(base.transform.lossyScale.y, 0.0001f);
		Vector3 vector = new Vector3(bounds.center.x, bounds.max.y - num * 0.5f, bounds.center.z);
		Vector3 vector2 = base.transform.InverseTransformPoint(vector);
		Vector3 vector3 = base.transform.InverseTransformVector(new Vector3(bounds.size.x, num, bounds.size.z));
		vector3 = new Vector3(Mathf.Abs(vector3.x), num2, Mathf.Abs(vector3.z));
		this.bigDropTopSurface.center = vector2;
		this.bigDropTopSurface.size = vector3;
		if (phys.sharedMaterial != null)
		{
			this.bigDropTopSurface.sharedMaterial = phys.sharedMaterial;
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0003180C File Offset: 0x0002FA0C
	private void ApplySyncedWorldPosition(Vector3 worldPos)
	{
		this.data.Position = worldPos;
		base.transform.position = worldPos;
		this.rb.position = worldPos;
		if (!this.rb.isKinematic)
		{
			this.rb.linearVelocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0003186C File Offset: 0x0002FA6C
	private static Vector3 GetSpawnPosFromSample(Vector3 gridCenter)
	{
		float num = 0.2f;
		float num2 = 2f;
		float num3 = 2f;
		float num4 = 0.6f;
		float num5 = 2f;
		float num6 = 0.6f;
		float num7 = 0.12f;
		PoissonDiskSampler3D poissonDiskSampler3D = new PoissonDiskSampler3D(num2, num, num3, num4);
		Array.Clear(DropView.overlapColliders, 0, 50);
		Physics.OverlapSphereNonAlloc(gridCenter, num5, DropView.overlapColliders, 66880);
		List<Vector3> list = poissonDiskSampler3D.Samples();
		for (int i = 0; i < list.Count; i++)
		{
			list[i] = new Vector3(list[i].x - num2 / 2f, 0f, list[i].z - num3 / 2f);
		}
		List<Vector3> list2 = new List<Vector3>();
		foreach (Vector3 vector in list.ToList<Vector3>())
		{
			bool flag = false;
			foreach (Collider collider in DropView.overlapColliders)
			{
				if (!(collider == null) && !(collider.gameObject == null) && collider.gameObject.layer != 16 && Vector3.Distance(collider.bounds.center, gridCenter + vector) <= num6)
				{
					list.Remove(vector);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (DropData dropData in MainGame.Instance.GameSave.WorldData.GetGameSceneDataById(MainGame.PlayerData.currentGameSceneId).droppedItems)
				{
					if (dropData.Size == ItemSize.Big)
					{
						if (Vector3.Distance(dropData.Position, gridCenter + vector) <= num6)
						{
							list.Remove(vector);
							flag = true;
							break;
						}
					}
					else if (dropData.Size == ItemSize.Small && Vector3.Distance(dropData.Position, gridCenter + vector) <= num7)
					{
						list.Remove(vector);
						flag = true;
						break;
					}
				}
				Vector3 vector2;
				if (!flag && SpecialPhysicsCastUtils.TrySnapDropPosToTopmostGround(gridCenter + vector, out vector2) && SpecialPhysicsCastUtils.IsReachableDropElevation(gridCenter.y, vector2.y))
				{
					list2.Add(vector2);
				}
			}
		}
		if (list2.Count > 0)
		{
			return list2.GetRandom<Vector3>();
		}
		return gridCenter;
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x00031B18 File Offset: 0x0002FD18
	public void DespawnView()
	{
		Pool pool = DropView.GetPool(this);
		this.PrepareForRelease();
		if (pool != null)
		{
			pool.ReleaseObject<DropView>(this);
			return;
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x00031B48 File Offset: 0x0002FD48
	public void OnPoolableObjReleased()
	{
		this.PrepareForRelease();
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x00031B50 File Offset: 0x0002FD50
	private void ResetAfterPoolGet()
	{
		this.isKicked = false;
		this.isMergeDelayed = false;
		this.isCollectDelayed = false;
		this.IsTimedCollecting = false;
		this.IsRiverDump = false;
		this.isPhysicDisabled = false;
		this.isSettled = false;
		this.settleStillFrames = 0;
		this.shouldPlayBounce = false;
		this.bounceTimer = 0f;
		this.bounceAnimationCurve = null;
		this.bouncePlayId++;
		if (this.bigItemMeshesTransform != null)
		{
			this.bigItemMeshesTransform.localPosition = this.bigItemMeshesStartLocalPosition;
		}
		this.isColliderFullyGrown = true;
		this.allNestedColliders = null;
		this.cachedRenderers = Array.Empty<Renderer>();
		this.interactionHandler = null;
		this.spawnDirection = Vector3.back;
		this.lastSuccessfulMergePartnerId = 0;
		this.lastSuccessfulMergeTime = 0f;
		base.transform.localScale = Vector3.one;
		base.transform.rotation = Quaternion.identity;
		this.SetNestedCollidersEnabled(true);
		if (this.registeredInChunkManager)
		{
			if (LazySingleton<ChunkManager>.Instance != null)
			{
				LazySingleton<ChunkManager>.Instance.UnregisterDynamicChunkableObject(this, ChunkManagerLayerType.DropView);
			}
			this.registeredInChunkManager = false;
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x00031C68 File Offset: 0x0002FE68
	private void PrepareForRelease()
	{
		if (this.isReleasedToPool)
		{
			return;
		}
		this.isReleasedToPool = true;
		this.isDespawning = true;
		base.StopAllCoroutines();
		this.moveToCollectorCoroutine = null;
		this.moveToPositionCoroutine = null;
		this.IsTimedCollecting = false;
		this.SetNestedCollidersEnabled(false);
		base.transform.DOKill(false);
		this.ForceStopBounce();
		this.RestoreWaterBobHierarchy();
		if (UIObjectBubbleManager.Instance != null && this.data != null)
		{
			UIObjectBubble uiobjectBubble;
			if (UIObjectBubbleManager.Instance.TryGetDisplayedBubble(this.data.UniqueId, out uiobjectBubble))
			{
				uiobjectBubble.ForceCancelDelayedHides();
			}
			UIObjectBubbleManager.Instance.Hide(this);
		}
		if (this.data != null)
		{
			this.data.OnCountChanged -= this.UpdateTextSprite;
		}
		this.data = null;
		this.interactionHandler = null;
		if (this.registeredInChunkManager)
		{
			if (LazySingleton<ChunkManager>.Instance != null)
			{
				LazySingleton<ChunkManager>.Instance.UnregisterDynamicChunkableObject(this, ChunkManagerLayerType.DropView);
			}
			this.registeredInChunkManager = false;
		}
		if (this.smallItemCollider != null && this.smallColliderRadius > 0f)
		{
			this.smallItemCollider.radius = this.smallColliderRadius;
		}
		if (this.bigDropTopSurface != null)
		{
			this.bigDropTopSurface.gameObject.SetActive(false);
		}
		DropViewAtomBase dropViewAtomBase = this.smallItem;
		if (dropViewAtomBase != null)
		{
			dropViewAtomBase.Deactivate();
		}
		DropViewAtomBase dropViewAtomBase2 = this.bigItem;
		if (dropViewAtomBase2 != null)
		{
			dropViewAtomBase2.Deactivate();
		}
		this.activeDropViewAtom = null;
		this.EnsureAllNestedColliders();
		for (int i = 0; i < this.allNestedColliders.Count; i++)
		{
			Collider collider = this.allNestedColliders[i];
			if (!(collider == null))
			{
				Collider collider2 = collider;
				collider2.excludeLayers &= -65537;
			}
		}
		this.allNestedColliders = null;
		for (int j = 0; j < this.cachedRenderers.Length; j++)
		{
			if (this.cachedRenderers[j] != null)
			{
				this.cachedRenderers[j].forceRenderingOff = false;
			}
		}
		this.cachedRenderers = Array.Empty<Renderer>();
		this.isChunkVisible = false;
		this.isSettled = false;
		this.isPhysicDisabled = false;
		this.IsRiverDump = false;
		this.ignoresItemCollisions = false;
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x00031E84 File Offset: 0x00030084
	private void RestoreWaterBobHierarchy()
	{
		if (this.waterBobRoot == null)
		{
			return;
		}
		if (this.bigItemMeshesTransform != null)
		{
			Transform transform = ((this.bigItem != null) ? this.bigItem.transform : base.transform);
			this.bigItemMeshesTransform.SetParent(transform, true);
			this.bigItemMeshesTransform.localPosition = this.bigItemMeshesStartLocalPosition;
		}
		if (this.waterFloatingObject != null)
		{
			this.waterFloatingObject.enabled = false;
		}
		global::UnityEngine.Object.Destroy(this.waterBobRoot.gameObject);
		this.waterBobRoot = null;
		this.waterFloatingObject = null;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x00031F26 File Offset: 0x00030126
	private void OnDisable()
	{
		base.transform.DOKill(false);
		Transform transform = this.bigItemMeshesTransform;
		if (transform == null)
		{
			return;
		}
		transform.DOKill(true);
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x00031F47 File Offset: 0x00030147
	private void OnDestroy()
	{
		if (this.registeredInChunkManager)
		{
			LazySingleton<ChunkManager>.Instance.UnregisterDynamicChunkableObject(this, ChunkManagerLayerType.DropView);
			this.registeredInChunkManager = false;
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x00031F64 File Offset: 0x00030164
	public void TryMoveToCollector(Transform target)
	{
		if (!this.CanProcessWorldEvents || this.isPhysicDisabled || this.IsTimedCollecting)
		{
			return;
		}
		this.WakeUp();
		this.moveToCollectorCoroutine = base.StartCoroutine(this.MoveToCollector(target));
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x00031F98 File Offset: 0x00030198
	public void MoveToCollectorTimed(Transform target, float duration)
	{
		if (this.isDespawning || this.data == null || this.data.IsRemoving)
		{
			return;
		}
		this.WakeUp();
		this.StopMoveCoroutines();
		this.isCollectDelayed = false;
		this.IsTimedCollecting = true;
		if (target == null || duration <= 0f)
		{
			this.CollectTimedDrop();
			return;
		}
		this.moveToCollectorCoroutine = base.StartCoroutine(this.MoveToCollectorTimedCoroutine(target, duration));
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x00032009 File Offset: 0x00030209
	public void MoveToCustomPosition(Vector3 position)
	{
		if (!this.CanProcessWorldEvents || this.isPhysicDisabled)
		{
			return;
		}
		this.WakeUp();
		this.moveToPositionCoroutine = base.StartCoroutine(this.MoveToPosition(position));
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x00032035 File Offset: 0x00030235
	public void SetInteractionVisualState(bool isUnderInteraction)
	{
		DropViewAtomBase dropViewAtomBase = this.activeDropViewAtom;
		if (dropViewAtomBase == null)
		{
			return;
		}
		dropViewAtomBase.SetInteractionState(isUnderInteraction);
	}

	// Token: 0x060009DC RID: 2524 RVA: 0x00032048 File Offset: 0x00030248
	public List<LazyWidgetDataBase> GetWidgetData()
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		if (this.data == null || this.interactionHandler == null)
		{
			return list;
		}
		PlayerController playerController = MainGame.PlayerController;
		PlayerInteractionComponent playerInteractionComponent = ((playerController != null) ? playerController.PlayerInteractionComponent : null);
		if (playerInteractionComponent != null && playerInteractionComponent.BigDropUnderInteraction == this && (this.interactionHandler.HasInteraction() || this.interactionHandler.HasInteraction2()))
		{
			InteractionInfos interactionInfos = this.interactionHandler.GetInteractionInfos();
			List<UIInteractionHintRowWidgetData> list2 = new List<UIInteractionHintRowWidgetData>();
			foreach (InteractionInfo interactionInfo in interactionInfos.list)
			{
				if (!string.IsNullOrEmpty(interactionInfo.text) || !string.IsNullOrEmpty(interactionInfo.customIconId))
				{
					list2.Add(new UIInteractionHintRowWidgetData(interactionInfo));
				}
			}
			if (list2.Count > 0)
			{
				list.Add(new UIInteractionHintWidgetData(list2));
			}
		}
		return list;
	}

	// Token: 0x060009DD RID: 2525 RVA: 0x00032148 File Offset: 0x00030348
	private void Update()
	{
		if (MainGame.IsGamePaused || !this.CanProcessWorldEvents)
		{
			return;
		}
		if (this.isSettled && !this.shouldPlayBounce)
		{
			return;
		}
		if (this.data != null && this.data.Position != base.transform.position)
		{
			this.data.Position = base.transform.position;
		}
		if (this.shouldPlayBounce && !this.isPhysicBounce)
		{
			this.PlayBounce();
		}
	}

	// Token: 0x060009DE RID: 2526 RVA: 0x000321C7 File Offset: 0x000303C7
	private void OnTriggerEnter(Collider other)
	{
		if (!this.CanProcessWorldEvents)
		{
			return;
		}
		this.TryUnwaterDrop(other);
		this.TryMergeIfDrop(other);
	}

	// Token: 0x060009DF RID: 2527 RVA: 0x000321E0 File Offset: 0x000303E0
	private void TryMergeIfDrop(Collider other)
	{
		if (!this.CanBeMerged)
		{
			return;
		}
		DropView componentInParent = other.GetComponentInParent<DropView>();
		if (componentInParent != null && componentInParent.CanBeMerged)
		{
			this.TryMergePair(componentInParent);
		}
	}

	// Token: 0x060009E0 RID: 2528 RVA: 0x00032218 File Offset: 0x00030418
	private void TryUnwaterDrop(Collider other)
	{
		if (other.gameObject.layer != 4)
		{
			return;
		}
		this.WakeUp();
		Vector3 vector;
		SpecialPhysicsCastUtils.GetPlayerDropPosition(MainGame.PlayerData.position.Value, MainGame.PlayerData.Direction, out vector);
		base.transform.position = vector;
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x00032268 File Offset: 0x00030468
	private void UpdateTextSprite()
	{
		if (this.isDespawning || this.data == null || this.activeDropViewAtom == null)
		{
			return;
		}
		SpriteText spriteText = this.activeDropViewAtom.GetSpriteText();
		if (spriteText == null)
		{
			return;
		}
		spriteText.SetText((this.data.Count <= 1) ? string.Empty : this.data.Count.ToString());
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x000322D8 File Offset: 0x000304D8
	private void UpdateEffects()
	{
		if (this.data.Size == ItemSize.Big)
		{
			this.shouldPlayBounce = true;
		}
		if (!this.data.IsDroppedFromPlayer && this.data.Size == ItemSize.Small)
		{
			this.rb.AddForce(this.spawnDirection * this.spawnForceImpulse, ForceMode.Impulse);
		}
		if (this.shouldPlayBounce && !this.isPhysicBounce)
		{
			if (this.bounceTime <= 0f || this.bounceAnimationCurves == null || this.bounceAnimationCurves.Count == 0)
			{
				this.shouldPlayBounce = false;
				return;
			}
			int num = global::UnityEngine.Random.Range(0, this.bounceAnimationCurves.Count);
			this.bounceAnimationCurve = this.bounceAnimationCurves[num];
			int playId = this.bouncePlayId;
			LazyTimer.AddTimer(this.bounceTime, delegate
			{
				if (playId != this.bouncePlayId || this.isReleasedToPool)
				{
					return;
				}
				this.StopBounce();
			}, null);
		}
	}

	// Token: 0x060009E3 RID: 2531 RVA: 0x000323C4 File Offset: 0x000305C4
	private void UpdateView()
	{
		DropViewAtomBase dropViewAtomBase = this.smallItem;
		if (dropViewAtomBase != null)
		{
			dropViewAtomBase.Deactivate();
		}
		DropViewAtomBase dropViewAtomBase2 = this.bigItem;
		if (dropViewAtomBase2 != null)
		{
			dropViewAtomBase2.Deactivate();
		}
		this.activeDropViewAtom = ((this.data.Size == ItemSize.Small) ? this.smallItem : this.bigItem);
		if (this.activeDropViewAtom == null)
		{
			Debug.LogError(string.Format("[DropView] Missing view atom for size [{0}] on [{1}]", this.data.Size, base.name));
			return;
		}
		DropViewAtomMesh dropViewAtomMesh = this.activeDropViewAtom as DropViewAtomMesh;
		if (dropViewAtomMesh != null && this.data.Item.Definition.isLinkedToWgo)
		{
			ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(this.data.Item.UniqueId);
			if (zombie != null)
			{
				SkinPresetGK2 presetForWgoData = ZombieSkinHelper.GetPresetForWgoData(zombie, "zombie_worker");
				if (presetForWgoData != null)
				{
					dropViewAtomMesh.ActivateZombie(presetForWgoData.head.id.ToString("D4"), presetForWgoData.head.palette);
					this.SyncBigDropTopSurface(!this.isPhysicDisabled);
					this.ApplyItemCollisionFilter();
					this.CacheActiveAtomRenderers();
					return;
				}
			}
		}
		this.activeDropViewAtom.Activate(this.data.IconId);
		this.SyncBigDropTopSurface(!this.isPhysicDisabled);
		this.ApplyItemCollisionFilter();
		this.CacheActiveAtomRenderers();
	}

	// Token: 0x060009E4 RID: 2532 RVA: 0x0003251C File Offset: 0x0003071C
	private void PlaySpawnScaleTween()
	{
		if (this.data.Size == ItemSize.Big)
		{
			return;
		}
		base.transform.DOKill(false);
		Vector3 localScale = base.transform.localScale;
		base.transform.localScale = Vector3.zero;
		base.transform.DOScale(localScale, this.spawnScaleTweenDuration).SetEase(this.spawnScaleTweenEase);
	}

	// Token: 0x060009E5 RID: 2533 RVA: 0x0003257F File Offset: 0x0003077F
	private void DoBounceFromGround()
	{
		this.rb.AddForce(Vector3.up * (this.data.IsDroppedFromPlayer ? this.bounceForceImpulseFromPlayer : this.bounceForceImpulse), ForceMode.Impulse);
	}

	// Token: 0x060009E6 RID: 2534 RVA: 0x000325B2 File Offset: 0x000307B2
	private IEnumerator MergeDelayCoroutine()
	{
		float mergeDelayTimer = 0f;
		this.isMergeDelayed = true;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		while (mergeDelayTimer < this.mergeDelayTime)
		{
			if (!MainGame.IsGamePaused)
			{
				mergeDelayTimer += Time.deltaTime;
			}
			yield return wait;
		}
		this.isMergeDelayed = false;
		mergeDelayTimer = 0f;
		while (mergeDelayTimer < 0.1f)
		{
			if (!MainGame.IsGamePaused)
			{
				mergeDelayTimer += Time.deltaTime;
			}
			yield return wait;
		}
		this.TryGetDropsAroundAndMerge();
		yield break;
	}

	// Token: 0x060009E7 RID: 2535 RVA: 0x000325C1 File Offset: 0x000307C1
	private IEnumerator CollectDelayCoroutine()
	{
		float timer = 0f;
		this.isCollectDelayed = true;
		bool isFishing = LazyUI.Get<UIFishingWindow>().IsShownAndTop;
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		while (timer < this.collectDelayTime)
		{
			if (!MainGame.IsGamePaused || isFishing)
			{
				timer += Time.deltaTime;
			}
			yield return wait;
		}
		this.isCollectDelayed = false;
		timer = 0f;
		while (timer < 0.1f)
		{
			if (!MainGame.IsGamePaused || isFishing)
			{
				timer += Time.deltaTime;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060009E8 RID: 2536 RVA: 0x000325D0 File Offset: 0x000307D0
	private void TryMergePair(DropView other)
	{
		if (other == null || other == this)
		{
			return;
		}
		if (this.ShouldAbsorbFrom(other))
		{
			this.Merge(other);
			return;
		}
		other.Merge(this);
	}

	// Token: 0x060009E9 RID: 2537 RVA: 0x00032600 File Offset: 0x00030800
	private void Merge(DropView dropToMergeWith)
	{
		if (!this.ShouldAbsorbFrom(dropToMergeWith))
		{
			return;
		}
		if (this.data.TryAddDropItemPartial(dropToMergeWith.data))
		{
			this.RememberSuccessfulMergePair(dropToMergeWith);
			this.UpdateTextSprite();
			this.data.NotifyCountChanged();
			if (dropToMergeWith.Data.Count == 0)
			{
				MainGame.Instance.dropSystem.RemoveDrop(dropToMergeWith.Data, this.Data.WorldId);
			}
			if (!dropToMergeWith.isDespawning)
			{
				dropToMergeWith.UpdateTextSprite();
				DropData dropData = dropToMergeWith.data;
				if (dropData == null)
				{
					return;
				}
				dropData.NotifyCountChanged();
			}
		}
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x00032690 File Offset: 0x00030890
	private bool ShouldAbsorbFrom(DropView other)
	{
		bool flag;
		if (other == null)
		{
			flag = null != null;
		}
		else
		{
			DropData dropData = other.Data;
			flag = ((dropData != null) ? dropData.Item : null) != null;
		}
		if (flag)
		{
			DropData dropData2 = this.data;
			if (((dropData2 != null) ? dropData2.Item : null) != null)
			{
				if (this.IsSuccessfulMergeOnCooldown(other))
				{
					return false;
				}
				if (this.data.Item.CanAddItemCount(other.Data.Item) <= 0)
				{
					return false;
				}
				int num = this.data.Count.CompareTo(other.Data.Count);
				if (num != 0)
				{
					return num > 0;
				}
				return base.GetInstanceID() < other.GetInstanceID();
			}
		}
		return false;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0003272D File Offset: 0x0003092D
	private bool IsSuccessfulMergeOnCooldown(DropView other)
	{
		return other.GetInstanceID() == this.lastSuccessfulMergePartnerId && Time.unscaledTime - this.lastSuccessfulMergeTime < 1f;
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00032754 File Offset: 0x00030954
	private void RememberSuccessfulMergePair(DropView other)
	{
		float unscaledTime = Time.unscaledTime;
		int instanceID = other.GetInstanceID();
		this.lastSuccessfulMergePartnerId = instanceID;
		this.lastSuccessfulMergeTime = unscaledTime;
		other.lastSuccessfulMergePartnerId = base.GetInstanceID();
		other.lastSuccessfulMergeTime = unscaledTime;
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x00032790 File Offset: 0x00030990
	private void TryGetDropsAroundAndMerge()
	{
		if (!this.CanBeMerged)
		{
			return;
		}
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, 1f, DropView.overlapColliders, 65536);
		for (int i = 0; i < num; i++)
		{
			Collider collider = DropView.overlapColliders[i];
			if (!(collider == null))
			{
				DropView componentInParent = collider.GetComponentInParent<DropView>();
				if (!(componentInParent == null) && !(componentInParent == this) && componentInParent.CanBeMerged)
				{
					this.TryMergePair(componentInParent);
				}
			}
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0003280B File Offset: 0x00030A0B
	public void StopMoving()
	{
		this.StopMoveCoroutines();
		this.WakeUp();
		this.SetNonPhysicState(false);
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x00032820 File Offset: 0x00030A20
	private void StopMoveCoroutines()
	{
		if (this.moveToCollectorCoroutine != null)
		{
			base.StopCoroutine(this.moveToCollectorCoroutine);
			this.moveToCollectorCoroutine = null;
		}
		if (this.moveToPositionCoroutine != null)
		{
			base.StopCoroutine(this.moveToPositionCoroutine);
			this.moveToPositionCoroutine = null;
		}
		this.IsTimedCollecting = false;
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x0003285F File Offset: 0x00030A5F
	private IEnumerator MoveToCollector(Transform target)
	{
		Vector3 moveVector = target.position - base.transform.position;
		float speedK = 1f;
		this.SetNonPhysicState(true);
		bool isFishing = LazyUI.Get<UIFishingWindow>().IsShownAndTop;
		while (moveVector.magnitude > 0.1f)
		{
			if (!MainGame.IsGamePaused || isFishing)
			{
				speedK += 0.2f;
				Vector3 vector = moveVector.normalized * ((3f + speedK) * Time.deltaTime);
				if (vector.magnitude > moveVector.magnitude)
				{
					base.transform.position = target.position;
				}
				else
				{
					base.transform.position += vector;
				}
				moveVector = target.position - base.transform.position;
				if (moveVector.magnitude >= 4f)
				{
					break;
				}
			}
			yield return new WaitForEndOfFrame();
		}
		this.SetNonPhysicState(false);
		this.moveToCollectorCoroutine = null;
		yield break;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00032875 File Offset: 0x00030A75
	private IEnumerator MoveToCollectorTimedCoroutine(Transform target, float duration)
	{
		Vector3 startPosition = base.transform.position;
		float elapsed = 0f;
		this.SetNonPhysicState(true);
		WaitForEndOfFrame wait = new WaitForEndOfFrame();
		while (elapsed < duration)
		{
			if (this.isDespawning || this.data == null || this.data.IsRemoving)
			{
				this.IsTimedCollecting = false;
				yield break;
			}
			if (target == null)
			{
				this.moveToCollectorCoroutine = null;
				this.CollectTimedDrop();
				yield break;
			}
			if (!MainGame.IsGamePaused)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				this.SetWorldPosition(Vector3.Lerp(startPosition, target.position, num * num));
			}
			yield return wait;
		}
		if (this.isDespawning || this.data == null || this.data.IsRemoving)
		{
			this.IsTimedCollecting = false;
			yield break;
		}
		if (target != null)
		{
			this.SetWorldPosition(target.position);
		}
		this.moveToCollectorCoroutine = null;
		this.CollectTimedDrop();
		yield break;
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00032892 File Offset: 0x00030A92
	private void CollectTimedDrop()
	{
		this.IsTimedCollecting = false;
		if (this.isDespawning || this.data == null || this.data.IsRemoving)
		{
			return;
		}
		MainGame.PlayerData.CollectDrop(this);
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x000328C4 File Offset: 0x00030AC4
	private void SetWorldPosition(Vector3 position)
	{
		base.transform.position = position;
		if (this.rb != null)
		{
			this.rb.position = position;
		}
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x000328EC File Offset: 0x00030AEC
	private IEnumerator MoveToPosition(Vector3 position)
	{
		Vector3 moveVector = position - base.transform.position;
		this.SetNonPhysicState(true);
		while (moveVector.magnitude > 0.1f)
		{
			if (!MainGame.IsGamePaused)
			{
				Vector3 vector = moveVector.normalized * (2f * Time.deltaTime);
				if (vector.magnitude > moveVector.magnitude)
				{
					base.transform.position = position;
				}
				else
				{
					base.transform.position += vector;
				}
				moveVector = position - base.transform.position;
			}
			yield return new WaitForEndOfFrame();
		}
		this.SetNonPhysicState(false);
		this.moveToPositionCoroutine = null;
		yield break;
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00032902 File Offset: 0x00030B02
	private IEnumerator BeKinematicDropForGivenFrames(int framesCount = 1)
	{
		this.rb.isKinematic = true;
		int num;
		for (int i = 0; i < framesCount; i = num + 1)
		{
			yield return new WaitForFixedUpdate();
			num = i;
		}
		this.ApplyRigidbodyState();
		yield return null;
		yield break;
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x00032918 File Offset: 0x00030B18
	private void PlayBounce()
	{
		if (!(this.bigItemMeshesTransform == null))
		{
			DropView.DropResCurve dropResCurve = this.bounceAnimationCurve;
			if (((dropResCurve != null) ? dropResCurve.curve : null) != null && this.bounceTime > 0f)
			{
				if (this.bounceTimer >= this.bounceTime)
				{
					return;
				}
				this.bounceTimer += Time.deltaTime;
				float num = Mathf.Clamp01(this.bounceTimer / this.bounceTime);
				float num2 = this.bounceAnimationCurve.curve.Evaluate(num);
				if (float.IsNaN(num2) || float.IsInfinity(num2))
				{
					return;
				}
				this.bigItemMeshesTransform.localPosition = this.bigItemMeshesStartLocalPosition + Vector3.up * (num2 * this.bounceHeight);
				return;
			}
		}
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x000329D4 File Offset: 0x00030BD4
	private void StopBounce()
	{
		if (this.bounceAnimationCurve == null || this.bigItemMeshesTransform == null)
		{
			return;
		}
		this.bounceAnimationCurve = null;
		this.shouldPlayBounce = false;
		this.bigItemMeshesTransform.DOKill(true);
		this.bigItemMeshesTransform.DOLocalMove(this.bigItemMeshesStartLocalPosition, 0.2f, false);
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00032A2C File Offset: 0x00030C2C
	private void ForceStopBounce()
	{
		this.bouncePlayId++;
		this.bounceAnimationCurve = null;
		this.shouldPlayBounce = false;
		if (this.bigItemMeshesTransform == null)
		{
			return;
		}
		this.bigItemMeshesTransform.DOKill(true);
		this.bigItemMeshesTransform.localPosition = this.bigItemMeshesStartLocalPosition;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x00032A84 File Offset: 0x00030C84
	private void FixedUpdate()
	{
		if (MainGame.IsGamePaused || !this.CanProcessWorldEvents)
		{
			return;
		}
		if (!this.isColliderFullyGrown)
		{
			this.UpdateCollidersSizes();
		}
		if (this.isSettled || this.isPhysicDisabled)
		{
			return;
		}
		if (!this.isChunkVisible)
		{
			this.settleStillFrames = 0;
			return;
		}
		this.UpdateDropPhysics(Time.fixedDeltaTime);
		this.TrySettle();
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x00032AE4 File Offset: 0x00030CE4
	public void DoKick(Transform collisionSourceTransform, bool ignoreDistanceToSource = false)
	{
		if (this.data == null)
		{
			return;
		}
		KickSettings kickSettings = ((this.data.Size == ItemSize.Small) ? this.smallDropKickSettings : this.bigDropKickSettings);
		if (!ignoreDistanceToSource && Vector3.Distance(collisionSourceTransform.position, base.transform.position) > kickSettings.distanceToKick)
		{
			return;
		}
		this.DoKick(base.transform.position - collisionSourceTransform.position, 1f);
	}

	// Token: 0x060009FB RID: 2555 RVA: 0x00032B5C File Offset: 0x00030D5C
	private void DoKick(Vector3 direction, float forceFactor = 1f)
	{
		if (this.isKicked || this.data == null)
		{
			return;
		}
		this.WakeUp();
		KickSettings kickSettings = ((this.data.Size == ItemSize.Small) ? this.smallDropKickSettings : this.bigDropKickSettings);
		this.isKicked = true;
		Vector3 vector = Vector3.Scale(direction.normalized, kickSettings.kickForce * forceFactor);
		this.rb.AddForce(vector, ForceMode.Impulse);
		LazyTimer.AddTimer(kickSettings.kickDelay, delegate
		{
			this.isKicked = false;
		}, null);
		this.data.TryStartAutoDestroyTimer();
	}

	// Token: 0x060009FC RID: 2556 RVA: 0x00032BF0 File Offset: 0x00030DF0
	private void UpdateDropPhysics(float deltaTime)
	{
		if (this.shouldPlayBounce && this.isPhysicBounce && this.rb.linearVelocity.y <= 0.001f)
		{
			this.shouldPlayBounce = false;
			this.DoBounceFromGround();
		}
		if (this.isPhysicDisabled || this.isMergeDelayed || this.data == null)
		{
			return;
		}
		if (this.data.Size == ItemSize.Small && !this.smallItemCollider.isTrigger)
		{
			return;
		}
		float num = ((this.data.Size == ItemSize.Small) ? this.smallItemCollider.radius : 0.7f);
		int num2 = Physics.OverlapSphereNonAlloc(base.transform.position, num, DropView.overlapColliders, 16843008);
		if (num2 > 0 && this.data.Size != ItemSize.Small)
		{
			DropViewAtomMesh dropViewAtomMesh = this.bigItem as DropViewAtomMesh;
			for (int i = 0; i < num2; i++)
			{
				Collider collider = DropView.overlapColliders[i];
				if (!(dropViewAtomMesh.MeshElement.physicsCollider == collider))
				{
					DropView componentInParent = collider.GetComponentInParent<DropView>();
					Vector3 vector;
					float num3;
					if ((!(componentInParent != null) || (!(componentInParent == this) && !componentInParent.isPhysicDisabled && !this.ignoresItemCollisions && !componentInParent.ignoresItemCollisions)) && Physics.ComputePenetration(dropViewAtomMesh.MeshElement.physicsCollider, dropViewAtomMesh.MeshElement.physicsCollider.transform.position, dropViewAtomMesh.MeshElement.physicsCollider.transform.rotation, collider, collider.transform.position, collider.transform.rotation, out vector, out num3) && vector.y <= 0.5f)
					{
						this.UnstackAlongSurface(vector, num3);
						return;
					}
				}
			}
			return;
		}
		this.SetCollidersTriggerState(false);
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x00032DB4 File Offset: 0x00030FB4
	public void PrepareAsRiverDump()
	{
		this.IsRiverDump = true;
		this.SetNonPhysicState(true);
		this.interactionHandler = new DisabledDropInteractionHandler();
		this.EnsureWaterBobRoot();
		this.SetWaterBobEnabled(false);
		this.EnsureAllNestedColliders();
		for (int i = 0; i < this.allNestedColliders.Count; i++)
		{
			if (this.allNestedColliders[i] != null)
			{
				this.allNestedColliders[i].enabled = false;
			}
		}
	}

	// Token: 0x060009FE RID: 2558 RVA: 0x00032E29 File Offset: 0x00031029
	public void SetRiverWorldPosition(Vector3 worldPos)
	{
		if (this.data != null)
		{
			this.data.Position = worldPos;
		}
		base.transform.position = worldPos;
		if (this.rb != null)
		{
			this.rb.position = worldPos;
		}
	}

	// Token: 0x060009FF RID: 2559 RVA: 0x00032E65 File Offset: 0x00031065
	public void SetWaterBobEnabled(bool enabled)
	{
		this.EnsureWaterBobRoot();
		if (this.waterFloatingObject != null)
		{
			this.waterFloatingObject.enabled = enabled;
		}
	}

	// Token: 0x06000A00 RID: 2560 RVA: 0x00032E88 File Offset: 0x00031088
	private void EnsureWaterBobRoot()
	{
		if (this.waterBobRoot != null)
		{
			return;
		}
		if (this.bigItemMeshesTransform == null)
		{
			return;
		}
		GameObject gameObject = new GameObject("RiverWaterBob");
		this.waterBobRoot = gameObject.transform;
		this.waterBobRoot.SetParent(base.transform, false);
		this.waterBobRoot.localPosition = Vector3.zero;
		this.waterBobRoot.localRotation = Quaternion.identity;
		this.waterBobRoot.localScale = Vector3.one;
		this.bigItemMeshesTransform.SetParent(this.waterBobRoot, true);
		this.waterFloatingObject = gameObject.AddComponent<WaterFloatingObject>();
		this.waterFloatingObject.enabled = false;
	}

	// Token: 0x06000A01 RID: 2561 RVA: 0x00032F38 File Offset: 0x00031138
	public void SetNonPhysicState(bool isPhysicDisabled)
	{
		if (isPhysicDisabled)
		{
			this.ForceStopBounce();
			this.rb.linearVelocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
			this.isPhysicDisabled = true;
			this.ApplyRigidbodyState();
			this.SetCollidersTriggerState(true);
			return;
		}
		this.isPhysicDisabled = false;
		this.WakeUp();
		this.SetCollidersTriggerState(false);
		this.rb.linearVelocity = Vector3.zero;
		this.rb.angularVelocity = Vector3.zero;
	}

	// Token: 0x06000A02 RID: 2562 RVA: 0x00032FB8 File Offset: 0x000311B8
	public void SetCollidersTriggerState(bool isTrigger)
	{
		if (this.smallItemCollider != null)
		{
			this.smallItemCollider.isTrigger = isTrigger;
		}
		if (this.data != null && this.data.Size == ItemSize.Big)
		{
			DropViewAtomMesh dropViewAtomMesh = this.activeDropViewAtom as DropViewAtomMesh;
			if (dropViewAtomMesh != null && dropViewAtomMesh.MeshElement != null)
			{
				if (dropViewAtomMesh.MeshElement.colliderContainer != null)
				{
					dropViewAtomMesh.MeshElement.colliderContainer.SetActive(!isTrigger);
				}
				else if (dropViewAtomMesh.MeshElement.physicsCollider != null)
				{
					dropViewAtomMesh.MeshElement.physicsCollider.enabled = !isTrigger;
				}
			}
		}
		this.SyncBigDropTopSurface(this.data != null && this.data.Size == ItemSize.Big && !isTrigger);
		this.ApplyItemCollisionFilter();
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0003308C File Offset: 0x0003128C
	private void ApplyItemCollisionFilter()
	{
		if (!this.ignoresItemCollisions)
		{
			return;
		}
		DropView.ExcludeDropLayer(this.smallItemCollider);
		DropView.ExcludeDropLayer(this.bigDropTopSurface);
		DropViewAtomMesh dropViewAtomMesh = this.activeDropViewAtom as DropViewAtomMesh;
		if (dropViewAtomMesh != null && dropViewAtomMesh.MeshElement != null)
		{
			DropView.ExcludeDropLayer(dropViewAtomMesh.MeshElement.physicsCollider);
			if (dropViewAtomMesh.MeshElement.colliderContainer != null)
			{
				Collider[] componentsInChildren = dropViewAtomMesh.MeshElement.colliderContainer.GetComponentsInChildren<Collider>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					DropView.ExcludeDropLayer(componentsInChildren[i]);
				}
			}
		}
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x00033120 File Offset: 0x00031320
	private static void ExcludeDropLayer(Collider col)
	{
		if (col != null)
		{
			col.excludeLayers |= 65536;
		}
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x00033148 File Offset: 0x00031348
	public void SetColliderSmall()
	{
		if (this.smallItemCollider == null)
		{
			this.isColliderFullyGrown = true;
			return;
		}
		this.smallColliderRadius = this.smallItemCollider.radius;
		this.smallItemCollider.radius = 0.001f;
		this.isColliderFullyGrown = this.smallColliderRadius <= 0.001f;
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x000331A4 File Offset: 0x000313A4
	public void UpdateCollidersSizes()
	{
		if (this.isColliderFullyGrown || this.smallItemCollider == null)
		{
			return;
		}
		if (this.smallItemCollider.radius.EqualsTo(this.smallColliderRadius, 0.005f) || this.smallItemCollider.radius >= this.smallColliderRadius)
		{
			this.smallItemCollider.radius = this.smallColliderRadius;
			this.isColliderFullyGrown = true;
			return;
		}
		this.smallItemCollider.radius += 0.005f;
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00033228 File Offset: 0x00031428
	private Vector3 CorrectSpawnPos()
	{
		Vector3 vector = this.RandPos();
		bool flag = DropView.IsOverlappingSomething(this.data.Position + vector, Vector3.zero, 0.05f);
		if (flag)
		{
			int num = 15;
			while (flag && num > 0)
			{
				num--;
				vector = this.RandPos();
				flag = DropView.IsOverlappingSomething(this.data.Position + vector, Vector3.zero, 0.05f);
			}
		}
		if (!flag)
		{
			this.data.Position += vector;
		}
		return vector;
	}

	// Token: 0x06000A08 RID: 2568 RVA: 0x000332B5 File Offset: 0x000314B5
	private static bool IsOverlappingSomething(Vector3 pos, Vector3 dir, float radius)
	{
		return Physics.OverlapSphereNonAlloc(pos + dir / 2f, radius, DropView.overlapColliders, 16843008) > 0;
	}

	// Token: 0x06000A09 RID: 2569 RVA: 0x000332DB File Offset: 0x000314DB
	private Vector3 RandPos()
	{
		return new Vector3(global::UnityEngine.Random.Range(-this.dropRandomizerRadius, this.dropRandomizerRadius), 0f, global::UnityEngine.Random.Range(-this.dropRandomizerRadius, this.dropRandomizerRadius)) * this.positionRandomization;
	}

	// Token: 0x06000A0A RID: 2570 RVA: 0x00033316 File Offset: 0x00031516
	private void OnCollisionEnter(Collision collisionSource)
	{
		if (!this.CanProcessWorldEvents)
		{
			return;
		}
		this.TryKickFromPlayerCollision(collisionSource);
		this.TryWakeFromDropCollision(collisionSource.collider);
	}

	// Token: 0x06000A0B RID: 2571 RVA: 0x00033334 File Offset: 0x00031534
	private void OnCollisionStay(Collision collisionSource)
	{
		if (!this.CanProcessWorldEvents)
		{
			return;
		}
		this.TryKickFromPlayerCollision(collisionSource);
	}

	// Token: 0x06000A0C RID: 2572 RVA: 0x00033348 File Offset: 0x00031548
	private void TryKickFromPlayerCollision(Collision collisionSource)
	{
		if (!this.CanProcessWorldEvents || this.isPhysicDisabled || this.isKicked)
		{
			return;
		}
		if (this.data.Size == ItemSize.Big && collisionSource.gameObject.layer == 10)
		{
			this.DoKick(collisionSource.transform, false);
		}
	}

	// Token: 0x06000A0D RID: 2573 RVA: 0x00033398 File Offset: 0x00031598
	private void TryWakeFromDropCollision(Collider otherCollider)
	{
		if (!this.isSettled || otherCollider == null)
		{
			return;
		}
		DropView componentInParent = otherCollider.GetComponentInParent<DropView>();
		if (componentInParent == null || componentInParent == this || componentInParent.isPhysicDisabled || componentInParent.isSettled)
		{
			return;
		}
		this.WakeUp();
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x000333E7 File Offset: 0x000315E7
	public SGuid BubbleDrawableUniqueId
	{
		get
		{
			if (this.data == null)
			{
				return SGuid.Empty;
			}
			return this.data.UniqueId;
		}
	}

	// Token: 0x17000192 RID: 402
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00033402 File Offset: 0x00031602
	public List<LazyWidgetDataBase> BubbleDrawableWidgets
	{
		get
		{
			return this.GetWidgetData();
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0003340A File Offset: 0x0003160A
	public Vector3 BubbleDrawablePosition
	{
		get
		{
			return ((this.data != null) ? this.data.Position : base.transform.position) + Vector3.up * 0.75f;
		}
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00033440 File Offset: 0x00031640
	public BurstableBounds GetChunkableData()
	{
		return new BurstableBounds(this.rb.position + this.bounds.center, this.bounds.size);
	}

	// Token: 0x17000194 RID: 404
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x00033477 File Offset: 0x00031677
	// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0003347F File Offset: 0x0003167F
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x06000A14 RID: 2580 RVA: 0x00033488 File Offset: 0x00031688
	public void UpdateChunkVisibility(bool isVisible)
	{
		if (this.isChunkVisible != isVisible)
		{
			this.isChunkVisible = isVisible;
			this.ApplyRigidbodyState();
			this.ApplyRendererVisibility();
		}
		if (this.data != null)
		{
			this.data.CanNotBeAutoDestroyed.UpdateFlag(CanNotBeAutoDestroyedReason.WhenVisibleOnScreen, isVisible);
		}
	}

	// Token: 0x06000A15 RID: 2581 RVA: 0x000334C0 File Offset: 0x000316C0
	private void WakeUp()
	{
		this.settleStillFrames = 0;
		if (this.isSettled)
		{
			this.SetSettled(false);
		}
		else
		{
			this.ApplyRigidbodyState();
		}
		if (!this.rb.isKinematic)
		{
			this.rb.WakeUp();
		}
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x000334F8 File Offset: 0x000316F8
	private void SetSettled(bool settled)
	{
		if (this.isSettled == settled)
		{
			return;
		}
		this.isSettled = settled;
		this.settleStillFrames = 0;
		if (settled)
		{
			this.rb.linearVelocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
		}
		this.ApplyRigidbodyState();
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00033548 File Offset: 0x00031748
	private void TrySettle()
	{
		if (this.isSettled || this.isPhysicDisabled || this.isMergeDelayed || this.shouldPlayBounce || this.isKicked || !this.isColliderFullyGrown)
		{
			return;
		}
		if (this.moveToCollectorCoroutine != null || this.moveToPositionCoroutine != null || this.IsTimedCollecting)
		{
			return;
		}
		if (this.rb.IsSleeping() || (this.rb.linearVelocity.sqrMagnitude <= 0.0001f && this.rb.angularVelocity.sqrMagnitude <= 0.0001f))
		{
			this.settleStillFrames++;
		}
		else
		{
			this.settleStillFrames = 0;
		}
		if (this.settleStillFrames >= 8)
		{
			this.SetSettled(true);
		}
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00033614 File Offset: 0x00031814
	private void ApplyRigidbodyState()
	{
		if (this.isChunkVisible && !this.isPhysicDisabled && !this.isSettled)
		{
			if (this.rb.isKinematic)
			{
				this.rb.isKinematic = false;
			}
			if (!this.rb.useGravity)
			{
				this.rb.useGravity = true;
			}
			if (this.rb.interpolation != RigidbodyInterpolation.Interpolate)
			{
				this.rb.interpolation = RigidbodyInterpolation.Interpolate;
			}
			if (this.rb.collisionDetectionMode != CollisionDetectionMode.Continuous)
			{
				this.rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
			}
			return;
		}
		if (this.rb.collisionDetectionMode != CollisionDetectionMode.Discrete)
		{
			this.rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
		}
		if (this.rb.interpolation != RigidbodyInterpolation.None)
		{
			this.rb.interpolation = RigidbodyInterpolation.None;
		}
		if (!this.rb.isKinematic)
		{
			this.rb.isKinematic = true;
		}
		if (this.rb.useGravity)
		{
			this.rb.useGravity = false;
		}
	}

	// Token: 0x06000A19 RID: 2585 RVA: 0x0003370A File Offset: 0x0003190A
	private void CacheActiveAtomRenderers()
	{
		this.cachedRenderers = ((this.activeDropViewAtom != null) ? this.activeDropViewAtom.GetComponentsInChildren<Renderer>(false) : Array.Empty<Renderer>());
		this.ApplyRendererVisibility();
	}

	// Token: 0x06000A1A RID: 2586 RVA: 0x0003373C File Offset: 0x0003193C
	private void ApplyRendererVisibility()
	{
		bool flag = !this.isChunkVisible;
		for (int i = 0; i < this.cachedRenderers.Length; i++)
		{
			if (this.cachedRenderers[i] != null)
			{
				this.cachedRenderers[i].forceRenderingOff = flag;
			}
		}
	}

	// Token: 0x06000A1B RID: 2587 RVA: 0x00033784 File Offset: 0x00031984
	private void EnsureAllNestedColliders()
	{
		if (this.allNestedColliders == null)
		{
			this.allNestedColliders = new List<Collider>(base.GetComponentsInChildren<Collider>(true));
		}
	}

	// Token: 0x06000A1C RID: 2588 RVA: 0x000337A0 File Offset: 0x000319A0
	private void SetNestedCollidersEnabled(bool enabled)
	{
		this.EnsureAllNestedColliders();
		for (int i = 0; i < this.allNestedColliders.Count; i++)
		{
			if (this.allNestedColliders[i] != null)
			{
				this.allNestedColliders[i].enabled = enabled;
			}
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x000337EF File Offset: 0x000319EF
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		this.DrawChunkGizmos();
	}

	// Token: 0x04000B3B RID: 2875
	private const int POSSIBLE_MASK = 16843008;

	// Token: 0x04000B3C RID: 2876
	private const float START_COL_RADIUS = 0.001f;

	// Token: 0x04000B3D RID: 2877
	private const float COL_RADIUS_STEP = 0.005f;

	// Token: 0x04000B3E RID: 2878
	private const int OVERLAP_COLLIDERS_MAX_COUNT = 50;

	// Token: 0x04000B3F RID: 2879
	private const int SETTLE_STILL_FRAMES = 8;

	// Token: 0x04000B40 RID: 2880
	private const float SETTLE_VELOCITY_SQR = 0.0001f;

	// Token: 0x04000B41 RID: 2881
	private const float BigDropTopSurfaceThickness = 0.04f;

	// Token: 0x04000B42 RID: 2882
	private const float SurfaceSepEpsilonSq = 1E-06f;

	// Token: 0x04000B43 RID: 2883
	private const float SuccessfulMergeCooldown = 1f;

	// Token: 0x04000B44 RID: 2884
	private const string DROP_SHELL_PREFAB_KEY = "Assets/AddressableAssets/Drops/Drop.prefab";

	// Token: 0x04000B45 RID: 2885
	private const string SMALL_ATOM_PREFAB_KEY = "Assets/AddressableAssets/Drops/SmallItem.prefab";

	// Token: 0x04000B46 RID: 2886
	private const string BIG_ATOM_PREFAB_KEY = "Assets/AddressableAssets/Drops/BigItem.prefab";

	// Token: 0x04000B47 RID: 2887
	private const string SMALL_POOL_ID = "DropView.Small";

	// Token: 0x04000B48 RID: 2888
	private const string BIG_POOL_ID = "DropView.Big";

	// Token: 0x04000B49 RID: 2889
	private static readonly Bounds SmallDropBounds = new Bounds(new Vector3(0f, 0.45f, 0f), new Vector3(0.7f, 1.1f, 0.7f));

	// Token: 0x04000B4A RID: 2890
	private static readonly Bounds BigDropBounds = new Bounds(new Vector3(0f, 0.5f, 0f), new Vector3(1.5f, 1.8f, 1.5f));

	// Token: 0x04000B4B RID: 2891
	private static Collider[] overlapColliders = new Collider[50];

	// Token: 0x04000B4C RID: 2892
	private static Pool smallDropPool;

	// Token: 0x04000B4D RID: 2893
	private static Pool bigDropPool;

	// Token: 0x04000B4E RID: 2894
	private static GameObject poolPrototypeHolder;

	// Token: 0x04000B4F RID: 2895
	private static float preloadProgress;

	// Token: 0x04000B50 RID: 2896
	[SerializeField]
	private CapsuleCollider smallItemCollider;

	// Token: 0x04000B51 RID: 2897
	[SerializeField]
	private Rigidbody rb;

	// Token: 0x04000B52 RID: 2898
	[SerializeField]
	private BoxCollider bigDropTopSurface;

	// Token: 0x04000B53 RID: 2899
	[SerializeField]
	[Space]
	private float mergeDelayTime = 0.3f;

	// Token: 0x04000B54 RID: 2900
	[SerializeField]
	private float collectDelayTime = 0.6f;

	// Token: 0x04000B55 RID: 2901
	[SerializeField]
	private float spawnForceImpulse;

	// Token: 0x04000B56 RID: 2902
	[SerializeField]
	private float unstackForceImpulse = 5f;

	// Token: 0x04000B57 RID: 2903
	[SerializeField]
	private float unstackForceImpulseBigDrop = 100f;

	// Token: 0x04000B58 RID: 2904
	[SerializeField]
	private float spawnScaleTweenDuration = 0.25f;

	// Token: 0x04000B59 RID: 2905
	[SerializeField]
	private Ease spawnScaleTweenEase = Ease.OutBack;

	// Token: 0x04000B5A RID: 2906
	[SerializeField]
	[Space]
	private bool isPhysicBounce;

	// Token: 0x04000B5B RID: 2907
	[SerializeField]
	private float bounceForceImpulseFromPlayer;

	// Token: 0x04000B5C RID: 2908
	[SerializeField]
	private float bounceForceImpulse;

	// Token: 0x04000B5D RID: 2909
	[SerializeField]
	private float bounceTime;

	// Token: 0x04000B5E RID: 2910
	[SerializeField]
	private float bounceHeight;

	// Token: 0x04000B5F RID: 2911
	[SerializeField]
	private List<DropView.DropResCurve> bounceAnimationCurves = new List<DropView.DropResCurve>();

	// Token: 0x04000B60 RID: 2912
	[SerializeField]
	[Space]
	private DropViewAtomBase smallItem;

	// Token: 0x04000B61 RID: 2913
	[SerializeField]
	private DropViewAtomBase bigItem;

	// Token: 0x04000B62 RID: 2914
	[SerializeField]
	private Transform bigItemMeshesTransform;

	// Token: 0x04000B63 RID: 2915
	[SerializeField]
	[Space]
	private float dropRandomizerRadius = 0.8f;

	// Token: 0x04000B64 RID: 2916
	[SerializeField]
	[Range(0f, 1f)]
	private float positionRandomization;

	// Token: 0x04000B65 RID: 2917
	[SerializeField]
	[Tooltip("Frustum AABB for ChunkManager. Sized per prefab (small vs big drop).")]
	private Bounds bounds;

	// Token: 0x04000B66 RID: 2918
	[SerializeField]
	private KickSettings smallDropKickSettings;

	// Token: 0x04000B67 RID: 2919
	[SerializeField]
	private KickSettings bigDropKickSettings;

	// Token: 0x04000B68 RID: 2920
	[SerializeField]
	[Space]
	private DropData data;

	// Token: 0x04000B69 RID: 2921
	private bool isKicked;

	// Token: 0x04000B6A RID: 2922
	private bool isDespawning;

	// Token: 0x04000B6B RID: 2923
	private bool isMergeDelayed;

	// Token: 0x04000B6C RID: 2924
	private bool isCollectDelayed;

	// Token: 0x04000B6D RID: 2925
	private Vector3 spawnDirection = Vector3.back;

	// Token: 0x04000B6E RID: 2926
	private DropViewAtomBase activeDropViewAtom;

	// Token: 0x04000B6F RID: 2927
	private bool isPhysicDisabled;

	// Token: 0x04000B70 RID: 2928
	private bool shouldPlayBounce;

	// Token: 0x04000B71 RID: 2929
	private float smallColliderRadius;

	// Token: 0x04000B72 RID: 2930
	private float bounceTimer;

	// Token: 0x04000B73 RID: 2931
	private DropView.DropResCurve bounceAnimationCurve;

	// Token: 0x04000B74 RID: 2932
	private Vector3 bigItemMeshesStartLocalPosition;

	// Token: 0x04000B75 RID: 2933
	private bool registeredInChunkManager;

	// Token: 0x04000B76 RID: 2934
	private WaterFloatingObject waterFloatingObject;

	// Token: 0x04000B77 RID: 2935
	private Transform waterBobRoot;

	// Token: 0x04000B78 RID: 2936
	private bool ignoresItemCollisions;

	// Token: 0x04000B79 RID: 2937
	private bool isSettled;

	// Token: 0x04000B7A RID: 2938
	private bool isChunkVisible;

	// Token: 0x04000B7B RID: 2939
	private bool isColliderFullyGrown = true;

	// Token: 0x04000B7C RID: 2940
	private int settleStillFrames;

	// Token: 0x04000B7D RID: 2941
	private Renderer[] cachedRenderers = Array.Empty<Renderer>();

	// Token: 0x04000B7E RID: 2942
	private List<Collider> allNestedColliders;

	// Token: 0x04000B7F RID: 2943
	[SerializeField]
	private ItemSize atomSize;

	// Token: 0x04000B80 RID: 2944
	private bool isReleasedToPool;

	// Token: 0x04000B81 RID: 2945
	private int lastSuccessfulMergePartnerId;

	// Token: 0x04000B82 RID: 2946
	private float lastSuccessfulMergeTime;

	// Token: 0x04000B83 RID: 2947
	private int bouncePlayId;

	// Token: 0x04000B84 RID: 2948
	private IInteractionHandler interactionHandler;

	// Token: 0x04000B87 RID: 2951
	private Coroutine moveToCollectorCoroutine;

	// Token: 0x04000B88 RID: 2952
	private Coroutine moveToPositionCoroutine;

	// Token: 0x0200018C RID: 396
	[Serializable]
	public class DropResCurve
	{
		// Token: 0x04000B8A RID: 2954
		public AnimationCurve curve;

		// Token: 0x04000B8B RID: 2955
		[Range(0f, 1f)]
		public float durationFactor;
	}
}
