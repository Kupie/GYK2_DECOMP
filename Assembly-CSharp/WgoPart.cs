using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using LazyBearTechnology;
using LinqTools;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ProBuilder;

// Token: 0x0200068E RID: 1678
public class WgoPart : MonoBehaviour
{
	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x06002CE7 RID: 11495 RVA: 0x000D6080 File Offset: 0x000D4280
	public IReadOnlyList<DockPoint> DockPoints
	{
		get
		{
			return this.dockPoints;
		}
	}

	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000D6088 File Offset: 0x000D4288
	public List<WgoPartState> Variations
	{
		get
		{
			return this.variations;
		}
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06002CE9 RID: 11497 RVA: 0x000D6090 File Offset: 0x000D4290
	// (set) Token: 0x06002CEA RID: 11498 RVA: 0x000D6098 File Offset: 0x000D4298
	public WgoPartData WgoPartData { get; set; }

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06002CEB RID: 11499 RVA: 0x000D60A1 File Offset: 0x000D42A1
	// (set) Token: 0x06002CEC RID: 11500 RVA: 0x000D60A9 File Offset: 0x000D42A9
	public string PooledAddressableKey { get; set; }

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06002CED RID: 11501 RVA: 0x000D60B2 File Offset: 0x000D42B2
	public Transform BubblePoint
	{
		get
		{
			if (this.customBubblePoint != null)
			{
				return this.customBubblePoint;
			}
			if (this.bubblePoint != null)
			{
				return this.bubblePoint;
			}
			return null;
		}
	}

	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x06002CEE RID: 11502 RVA: 0x000D60DF File Offset: 0x000D42DF
	public Wgo Wgo
	{
		get
		{
			return this.wgo;
		}
	}

	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x06002CEF RID: 11503 RVA: 0x000D60E7 File Offset: 0x000D42E7
	public string Id
	{
		get
		{
			if (this.WgoPartData == null || string.IsNullOrEmpty(this.WgoPartData.id))
			{
				return base.name;
			}
			return this.WgoPartData.id;
		}
	}

	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x06002CF0 RID: 11504 RVA: 0x000D6115 File Offset: 0x000D4315
	public Object3D Object3D
	{
		get
		{
			return this.object3D;
		}
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x06002CF1 RID: 11505 RVA: 0x000D611D File Offset: 0x000D431D
	public AnimationComponentBase AnimationComponent
	{
		get
		{
			return this.animationComponent;
		}
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000D6128 File Offset: 0x000D4328
	public bool EnsureAnimationComponentInitialized()
	{
		if (this.animationComponentInitialized)
		{
			return this.animationComponent != null;
		}
		this.animationComponent = base.GetComponentInChildren<AnimationComponentBase>();
		if (this.animationComponent == null)
		{
			return false;
		}
		string text = ((this.wgo.Data.Definition != null) ? (this.wgo.Data.Definition.hasCustomVisualId ? this.wgo.Data.Definition.customVisualId : this.wgo.Data.Definition.id) : string.Empty);
		ZombieWgoData zombieWgoData = this.wgo.Data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			this.SetupZombieSkin(zombieWgoData, text);
		}
		else if (this.wgo.Data.Definition != null && !string.IsNullOrEmpty(this.wgo.Data.Definition.zombieRollDataId))
		{
			this.animationComponent.SetSkinPreset(ZombieSkinHelper.GetPresetForWgoData(this.wgo.Data, this.wgo.Data.Definition.zombieRollDataId));
			this.animationComponent.InitWithSkinOrApplyCurrentSkin(text);
		}
		else
		{
			this.animationComponent.InitWithSkinOrApplySkin(text);
		}
		AnimationComponent animationComponent = this.animationComponent as AnimationComponent;
		if (animationComponent != null && animationComponent.AutoInitOnAwake)
		{
			this.animationComponent.SetDirection(animationComponent.Direction);
		}
		else
		{
			this.animationComponent.SetDirection(this.wgo.Data.direction.Value);
		}
		this.wgo.Data.OnDirectionChanged += this.HandleDirectionChanged;
		this.animationComponentInitialized = true;
		return true;
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x06002CF3 RID: 11507 RVA: 0x000D62C8 File Offset: 0x000D44C8
	public List<Collider> InteractableColliders
	{
		get
		{
			return this.interactableColliders;
		}
	}

	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x06002CF4 RID: 11508 RVA: 0x000D62D0 File Offset: 0x000D44D0
	public WgoPartState CurrentWgoPartState
	{
		get
		{
			if (this.WgoPartData == null)
			{
				return null;
			}
			return this.GetWgoPartState(this.WgoPartData.variationId, this.WgoPartData.rotationIndex);
		}
	}

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x000D62F8 File Offset: 0x000D44F8
	public Transform CustomBubblePoint
	{
		get
		{
			return this.customBubblePoint;
		}
	}

	// Token: 0x1700070E RID: 1806
	// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000D6300 File Offset: 0x000D4500
	// (set) Token: 0x06002CF7 RID: 11511 RVA: 0x000D6308 File Offset: 0x000D4508
	public SpawnConfiguration SpawnConfiguration
	{
		get
		{
			return this.spawnConfiguration;
		}
		set
		{
			this.spawnConfiguration = value;
		}
	}

	// Token: 0x06002CF8 RID: 11512 RVA: 0x000D6314 File Offset: 0x000D4514
	public DockPointData GetDockPointData(DockPoint dockPoint)
	{
		int num = this.dockPoints.IndexOf(dockPoint);
		if (num == -1)
		{
			return null;
		}
		return this.WgoPartData.GetDockPointByIndex(num);
	}

	// Token: 0x06002CF9 RID: 11513 RVA: 0x000D6340 File Offset: 0x000D4540
	public void InitVisuals(GameObject assetReference, WgoPartData wgoPartData, WGODef definition)
	{
		this.assetReference = assetReference;
		this.WgoPartData = wgoPartData;
		this.UpdateAvailableVariationsData();
		this.EnsureAnimationComponentInitialized();
		if (this.BubblePoint != null)
		{
			this.Wgo.Data.SetBubblePointOffset(this.BubblePoint.localPosition);
		}
		this.SubscribeToDataChanges();
		if (this.label != null)
		{
			this.label.gameObject.SetActive(!string.IsNullOrEmpty(definition.devLabelStr));
			this.label.text = definition.devLabelStr;
		}
		this.UpdateDropViewFromInventory(null);
		this.InitDockPoints();
	}

	// Token: 0x06002CFA RID: 11514 RVA: 0x000D63E4 File Offset: 0x000D45E4
	public void CleanupChunkableComponents()
	{
		ChunkableObjectComponent[] componentsInChildren = base.GetComponentsInChildren<ChunkableObjectComponent>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			global::UnityEngine.Object.Destroy(componentsInChildren[i]);
		}
		BakedChunkableObjectComponent[] componentsInChildren2 = base.GetComponentsInChildren<BakedChunkableObjectComponent>(true);
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			global::UnityEngine.Object.Destroy(componentsInChildren2[j]);
		}
	}

	// Token: 0x06002CFB RID: 11515 RVA: 0x000D6430 File Offset: 0x000D4630
	public void DeInitForPool()
	{
		if (this.WgoPartData != null)
		{
			this.UnsubscribeFromDataChanges();
		}
		if (this.flagPlacementPoints != null)
		{
			foreach (FlagPlacementPoint flagPlacementPoint in this.flagPlacementPoints)
			{
				flagPlacementPoint.DeInit();
			}
		}
		Object3D object3D = this.Object3D;
		if (((object3D != null) ? object3D.Object3DMeshes : null) != null)
		{
			for (int i = 0; i < this.Object3D.Object3DMeshes.Count; i++)
			{
				Object3DMesh object3DMesh = this.Object3D.Object3DMeshes[i];
				if (!(object3DMesh == null))
				{
					object3DMesh.ResetAnimValues();
				}
			}
		}
		DecoyComponent componentInChildren = base.GetComponentInChildren<DecoyComponent>(true);
		if (componentInChildren)
		{
			global::UnityEngine.Object.Destroy(componentInChildren);
		}
		RiverBodyReceiver riverBodyReceiver = this.riverBodyReceiver;
		if (riverBodyReceiver != null)
		{
			riverBodyReceiver.DeInit();
		}
		AnimationComponentBase animationComponentBase = this.animationComponent;
		if (animationComponentBase != null)
		{
			animationComponentBase.ResetForPool();
		}
		this.assetReference = null;
		this.WgoPartData = null;
		this.wgo = null;
		this.animationComponent = null;
		this.animationComponentInitialized = false;
	}

	// Token: 0x06002CFC RID: 11516 RVA: 0x000D6544 File Offset: 0x000D4744
	public void ReInitFromPool(Wgo newWgo)
	{
		this.wgo = newWgo;
		bool activeSelf = base.gameObject.activeSelf;
		base.gameObject.SetActive(true);
		this.Init(newWgo);
		if (activeSelf)
		{
			if (this.conditionalDrawer == null)
			{
				this.conditionalDrawer = base.GetComponent<ConditionalDrawer>();
			}
			ConditionalDrawer conditionalDrawer = this.conditionalDrawer;
			if (conditionalDrawer != null)
			{
				conditionalDrawer.Init(this);
			}
		}
		if (this.reservoirView != null)
		{
			this.reservoirView.Init(this.wgo);
		}
		if (this.riverBodyReceiver == null)
		{
			this.riverBodyReceiver = base.GetComponentInChildren<RiverBodyReceiver>(true);
		}
		if (this.riverBodyReceiver != null)
		{
			this.riverBodyReceiver.Init(this.wgo);
		}
		if (this.flagPlacementPoints != null)
		{
			foreach (FlagPlacementPoint flagPlacementPoint in this.flagPlacementPoints)
			{
				Wgo wgo = this.wgo;
				flagPlacementPoint.Init((wgo != null) ? wgo.Data : null);
			}
		}
	}

	// Token: 0x06002CFD RID: 11517 RVA: 0x000D6650 File Offset: 0x000D4850
	public void SetCustomBubblePoint(Transform bubblePoint)
	{
		this.customBubblePoint = bubblePoint;
	}

	// Token: 0x06002CFE RID: 11518 RVA: 0x000D665C File Offset: 0x000D485C
	public void SetSelectionTint(Color color, float amount)
	{
		if (this.cachedObjects3D == null)
		{
			return;
		}
		for (int i = 0; i < this.cachedObjects3D.Count; i++)
		{
			Object3D object3D = this.cachedObjects3D[i];
			if (object3D != null)
			{
				object3D.SetSelectionTint(color, amount);
			}
		}
	}

	// Token: 0x06002CFF RID: 11519 RVA: 0x000D66A1 File Offset: 0x000D48A1
	public void SetDropViewInteractionState(bool isUnderInteraction)
	{
		this.dropViewUnderInteraction = isUnderInteraction;
		this.ApplyDropViewInteractionState();
	}

	// Token: 0x06002D00 RID: 11520 RVA: 0x000D66B0 File Offset: 0x000D48B0
	private void Init(Wgo wgo)
	{
		this.wgo = wgo;
		this.object3D = base.GetComponentInChildren<Object3D>();
		this.cachedObjects3D = base.GetComponentsInChildren<Object3D>(true).ToList<Object3D>();
	}

	// Token: 0x06002D01 RID: 11521 RVA: 0x000D66D7 File Offset: 0x000D48D7
	private void Start()
	{
		this.UpdateColliders();
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x000D66E0 File Offset: 0x000D48E0
	private void OnEnable()
	{
		if (this.conditionalDrawer == null)
		{
			this.conditionalDrawer = base.GetComponent<ConditionalDrawer>();
		}
		if (this.conditionalDrawer)
		{
			this.conditionalDrawer.Init(this);
		}
		if (this.reservoirView == null)
		{
			this.reservoirView = base.GetComponent<ReservoirView>();
		}
		if (this.reservoirView)
		{
			this.reservoirView.Init(this.wgo);
		}
		if (this.riverBodyReceiver == null)
		{
			this.riverBodyReceiver = base.GetComponentInChildren<RiverBodyReceiver>(true);
		}
		if (this.riverBodyReceiver)
		{
			this.riverBodyReceiver.Init(this.wgo);
		}
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x000D677F File Offset: 0x000D497F
	private void OnDisable()
	{
		if (this.conditionalDrawer != null)
		{
			this.conditionalDrawer.DeInit();
		}
	}

	// Token: 0x06002D04 RID: 11524 RVA: 0x000D679A File Offset: 0x000D499A
	public void KillPhysicsCollAnimTween()
	{
		if (this.physicsCollAnimTween != null)
		{
			this.physicsCollAnimTween.Kill(false);
			this.physicsCollAnimTween = null;
		}
	}

	// Token: 0x06002D05 RID: 11525 RVA: 0x000D67B7 File Offset: 0x000D49B7
	private void OnDestroy()
	{
		this.KillPhysicsCollAnimTween();
		if (this.WgoPartData != null)
		{
			this.UnsubscribeFromDataChanges();
		}
		if (this.assetReference != null)
		{
			Addressables.Release<GameObject>(this.assetReference);
			this.assetReference = null;
		}
	}

	// Token: 0x06002D06 RID: 11526 RVA: 0x000D67F0 File Offset: 0x000D49F0
	private void UpdateColliders()
	{
		List<Collider> cols = new List<Collider>();
		this.cachedObjects3D.ForEach(delegate(Object3D obj)
		{
			cols.AddRange(obj.CachedColliders);
		});
		foreach (Collider collider in cols)
		{
			if (collider == null)
			{
				Debug.LogError("Null cached collider on WgoPart:[" + this.Id + "]");
			}
			else if (collider.IsLossyScaleNegative())
			{
				BoxCollider boxCollider = collider as BoxCollider;
				if (boxCollider != null)
				{
					boxCollider.FixBoxColliderLossyScale();
				}
				MeshCollider meshCollider = collider as MeshCollider;
				if (meshCollider != null && meshCollider.GetComponent<ProBuilderMesh>())
				{
					MirrorMeshCollider component = meshCollider.GetComponent<MirrorMeshCollider>();
					if (component == null)
					{
						meshCollider.gameObject.AddComponent<MirrorMeshCollider>();
					}
					else
					{
						component.MirrorCollider();
					}
				}
			}
		}
	}

	// Token: 0x06002D07 RID: 11527 RVA: 0x000D68EC File Offset: 0x000D4AEC
	private void SubscribeToDataChanges()
	{
		this.WgoPartData.OnStateChange += this.ApplyWgoPartState;
	}

	// Token: 0x06002D08 RID: 11528 RVA: 0x000D6905 File Offset: 0x000D4B05
	private void UnsubscribeFromDataChanges()
	{
		this.WgoPartData.OnStateChange -= this.ApplyWgoPartState;
		this.wgo.Data.OnDirectionChanged -= this.HandleDirectionChanged;
	}

	// Token: 0x06002D09 RID: 11529 RVA: 0x000D693C File Offset: 0x000D4B3C
	private void InitDockPoints()
	{
		foreach (DockPoint dockPoint in this.DockPoints)
		{
			dockPoint.Init(this);
		}
	}

	// Token: 0x06002D0A RID: 11530 RVA: 0x000D6988 File Offset: 0x000D4B88
	public bool CanBeRotated(string variationId = "", int rotationIndex = -1)
	{
		string text = (string.IsNullOrEmpty(variationId) ? this.WgoPartData.variationId : variationId);
		int num = ((rotationIndex == -1) ? this.WgoPartData.rotationIndex : rotationIndex);
		foreach (WgoPartState wgoPartState in this.variations)
		{
			if (wgoPartState.variationId == text && wgoPartState.rotationIndex != num)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002D0B RID: 11531 RVA: 0x000D6A20 File Offset: 0x000D4C20
	public bool TryApplyCustomRotationSequenceStart()
	{
		if (this.customRotationSequence == null || this.customRotationSequence.Count == 0)
		{
			return false;
		}
		string text = ((this.WgoPartData != null) ? this.WgoPartData.variationId : null);
		if (string.IsNullOrEmpty(text))
		{
			text = this.GetDefaultVariationId();
		}
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		WgoPartState wgoPartState = this.GetWgoPartState(text, this.customRotationSequence[0]);
		if (wgoPartState == null)
		{
			return false;
		}
		this.ApplyWgoPartState(wgoPartState);
		return true;
	}

	// Token: 0x06002D0C RID: 11532 RVA: 0x000D6A98 File Offset: 0x000D4C98
	public bool Rotate(bool inReverse = false)
	{
		if (this.customRotationSequence == null || this.customRotationSequence.Count <= 0)
		{
			List<WgoPartState> list = new List<WgoPartState>();
			foreach (WgoPartState wgoPartState in this.variations)
			{
				if (wgoPartState.variationId == this.WgoPartData.variationId)
				{
					list.Add(wgoPartState);
				}
			}
			int num = this.WgoPartData.rotationIndex;
			int num2 = num;
			do
			{
				num = MathUtilities.ClampCycle(inReverse ? (--num) : (++num), 0, 3);
				foreach (WgoPartState wgoPartState2 in list)
				{
					if (wgoPartState2.rotationIndex == num)
					{
						this.WgoPartData.rotationIndex = num;
						this.ApplyWgoPartState(wgoPartState2);
						return true;
					}
				}
			}
			while (num != num2);
			return false;
		}
		int num3 = this.customRotationSequence.IndexOf(this.WgoPartData.rotationIndex);
		if (num3 == -1)
		{
			return false;
		}
		int num4 = MathUtilities.ClampCycle(inReverse ? (num3 - 1) : (num3 + 1), 0, this.customRotationSequence.Count - 1);
		WgoPartState wgoPartState3 = this.GetWgoPartState(this.WgoPartData.variationId, this.customRotationSequence[num4]);
		if (wgoPartState3 == null)
		{
			return false;
		}
		this.ApplyWgoPartState(wgoPartState3);
		return true;
	}

	// Token: 0x06002D0D RID: 11533 RVA: 0x000D6C18 File Offset: 0x000D4E18
	public void ApplyWgoPartState()
	{
		if (this.variations.Count == 0)
		{
			return;
		}
		if (string.IsNullOrEmpty(this.WgoPartData.variationId) && this.WgoPartData.rotationIndex == -1)
		{
			this.ApplyDefaultWgoPartState();
			return;
		}
		foreach (WgoPartState wgoPartState in this.variations)
		{
			if (string.IsNullOrEmpty(this.WgoPartData.variationId) && wgoPartState.rotationIndex == this.WgoPartData.rotationIndex)
			{
				this.ApplyWgoPartState(wgoPartState);
				return;
			}
			if (this.WgoPartData.rotationIndex == -1 && wgoPartState.variationId == this.WgoPartData.variationId)
			{
				this.ApplyWgoPartState(wgoPartState);
				return;
			}
			if (wgoPartState.variationId == this.WgoPartData.variationId && wgoPartState.rotationIndex == this.WgoPartData.rotationIndex)
			{
				this.ApplyWgoPartState(wgoPartState);
				return;
			}
		}
		Debug.LogError("Cannot apply WgoPartState for WgoPartData: " + this.WgoPartData.ToString());
		this.ApplyWgoPartState(this.variations[0]);
	}

	// Token: 0x06002D0E RID: 11534 RVA: 0x000D6D60 File Offset: 0x000D4F60
	public void ApplyDefaultWgoPartState()
	{
		if (this.variations.Count == 0)
		{
			return;
		}
		bool flag = false;
		foreach (WgoPartState wgoPartState in this.variations)
		{
			if (wgoPartState.isDefault)
			{
				this.ApplyWgoPartState(wgoPartState);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogError("WgoPart [" + base.gameObject.name + "] initialized without default state.");
			this.ApplyWgoPartState(this.variations[0]);
		}
	}

	// Token: 0x06002D0F RID: 11535 RVA: 0x000D6E04 File Offset: 0x000D5004
	private string GetDefaultVariationId()
	{
		foreach (WgoPartState wgoPartState in this.variations)
		{
			if (wgoPartState.isDefault)
			{
				return wgoPartState.variationId;
			}
		}
		if (this.variations.Count <= 0)
		{
			return string.Empty;
		}
		return this.variations[0].variationId;
	}

	// Token: 0x06002D10 RID: 11536 RVA: 0x000D6E88 File Offset: 0x000D5088
	public WgoPartState GetWgoPartState(string variationId, int rotationIndex = -1)
	{
		List<WgoPartState> list = this.variations;
		if (list != null && list.Count > 0)
		{
			foreach (WgoPartState wgoPartState in this.Variations)
			{
				if (wgoPartState.variationId == variationId && (rotationIndex == -1 || wgoPartState.rotationIndex == rotationIndex))
				{
					return wgoPartState;
				}
			}
			Debug.LogError(string.Format("[{0}]: Cannot find state for wgoPart [{1}] with [{2}-{3}]", new object[] { "WgoPart", this.Id, variationId, rotationIndex }), this);
		}
		return null;
	}

	// Token: 0x06002D11 RID: 11537 RVA: 0x000D6F44 File Offset: 0x000D5144
	public void ApplyWgoPartState(string id, int rotationIndex)
	{
		WgoPartState wgoPartState = this.GetWgoPartState(id, rotationIndex);
		if (wgoPartState == null)
		{
			return;
		}
		this.ApplyWgoPartState(wgoPartState);
	}

	// Token: 0x06002D12 RID: 11538 RVA: 0x000D6F68 File Offset: 0x000D5168
	public void TryAnimatePhysicsCollider()
	{
		if (this.capsulePhysicsCollider != null)
		{
			float startRadius = 0.001f;
			float radius = this.capsulePhysicsCollider.radius;
			this.capsulePhysicsCollider.radius = startRadius;
			this.physicsCollAnimTween = DOTween.To(() => startRadius, delegate(float x)
			{
				startRadius = x;
				if (this.capsulePhysicsCollider != null)
				{
					this.capsulePhysicsCollider.radius = x;
				}
			}, radius, 0.15f);
		}
	}

	// Token: 0x06002D13 RID: 11539 RVA: 0x000D6FE4 File Offset: 0x000D51E4
	private void ApplyWgoPartState(WgoPartState wgoPartState)
	{
		foreach (WgoPartState wgoPartState2 in this.variations)
		{
			if (!(wgoPartState2.gameObject == null))
			{
				wgoPartState2.gameObject.SetActive(false);
			}
		}
		this.WgoPartData.variationId = wgoPartState.variationId;
		this.WgoPartData.rotationIndex = wgoPartState.rotationIndex;
		if (wgoPartState.gameObject != null)
		{
			wgoPartState.gameObject.SetActive(true);
			Vector3 localScale = wgoPartState.gameObject.transform.localScale;
			wgoPartState.gameObject.transform.localScale = new Vector3(Mathf.Abs(localScale.x) * (float)(wgoPartState.mirror ? (-1) : 1), localScale.y, localScale.z);
			this.object3D = wgoPartState.gameObject.GetComponentInChildren<Object3D>(true);
		}
		this.customBubblePoint = wgoPartState.customBubblePoint;
		foreach (UnityEvent unityEvent in wgoPartState.customEvents)
		{
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
		}
		this.UpdateColliders();
	}

	// Token: 0x06002D14 RID: 11540 RVA: 0x000D713C File Offset: 0x000D533C
	private void UpdateAvailableVariationsData()
	{
		this.WgoPartData.AvailableVariations = new List<WgoPartStateData>();
		foreach (WgoPartState wgoPartState in this.variations)
		{
			WgoPartStateData wgoPartStateData = new WgoPartStateData(wgoPartState.variationId, wgoPartState.rotationIndex, wgoPartState.mirror);
			this.WgoPartData.AvailableVariations.Add(wgoPartStateData);
		}
	}

	// Token: 0x06002D15 RID: 11541 RVA: 0x000D71C4 File Offset: 0x000D53C4
	private void HandleDirectionChanged(Vector2 direction)
	{
		if (this.animationComponent != null)
		{
			this.animationComponent.SetDirection(direction);
		}
	}

	// Token: 0x06002D16 RID: 11542 RVA: 0x000D71E4 File Offset: 0x000D53E4
	private void SetupZombieSkin(ZombieWgoData zombieWgoData, string visualId = "")
	{
		if (this.animationComponent == null)
		{
			return;
		}
		int? num = null;
		ZombieType zombieType = zombieWgoData.ZombieType;
		if (zombieType != ZombieType.Crafter)
		{
			if (zombieType == ZombieType.Gardener)
			{
				num = new int?(1800);
			}
		}
		else
		{
			WgoData attachedWgoData = zombieWgoData.AttachedWgoData;
			if (attachedWgoData != null && attachedWgoData.Definition.interactionType == WGODef.InteractionType.ZombieMine)
			{
				num = new int?(1700);
			}
		}
		if (num != null)
		{
			int gameResInt = zombieWgoData.GetGameResInt("zombie_head_id");
			int gameResInt2 = zombieWgoData.GetGameResInt("zombie_body_id");
			string text = zombieWgoData.GameResStr.Get("zombie_head_lut", "");
			int num2;
			switch (gameResInt)
			{
			case 1050:
			case 1056:
				num2 = 1;
				goto IL_00DE;
			case 1052:
			case 1054:
				num2 = 2;
				goto IL_00DE;
			case 1058:
				num2 = 3;
				goto IL_00DE;
			}
			num2 = 0;
			IL_00DE:
			int num3 = num2;
			SkinPresetGK2 presetForCustomizationData = ZombieSkinHelper.GetPresetForCustomizationData("zombie_worker", gameResInt2, num.Value + num3, string.Empty, text);
			this.animationComponent.SetSkinPreset(presetForCustomizationData);
			this.animationComponent.InitWithSkinOrApplyCurrentSkin(visualId);
			return;
		}
		this.animationComponent.SetSkinPreset(ZombieSkinHelper.GetPresetForWgoData(zombieWgoData, "zombie_worker"));
		this.animationComponent.InitWithSkinOrApplyCurrentSkin(visualId);
	}

	// Token: 0x06002D17 RID: 11543 RVA: 0x000D7330 File Offset: 0x000D5530
	public void UpdateDropViewFromInventory(List<Item> customInventory = null)
	{
		if (customInventory == null && this.wgo != null && this.wgo.Data != null)
		{
			if (this.wgo.Data.Inventory.Data.Inventory.Count == 0)
			{
				this.UpdateDropViewGroup(null);
				return;
			}
			this.UpdateDropViewGroup(this.wgo.Data.Inventory.Data.Inventory[0].Definition);
			return;
		}
		else
		{
			if (customInventory.Count == 0)
			{
				this.UpdateDropViewGroup(null);
				return;
			}
			this.UpdateDropViewGroup(customInventory[0].Definition);
			return;
		}
	}

	// Token: 0x06002D18 RID: 11544 RVA: 0x000D73D2 File Offset: 0x000D55D2
	public void UpdateDropViewFromItemData(ConveyorMovableItemData itemData)
	{
		if (itemData == null || string.IsNullOrEmpty(itemData.itemId))
		{
			this.UpdateDropViewGroup(null);
			return;
		}
		this.UpdateDropViewGroup(GameBalance.Me.GetData<ItemDef>(itemData.itemId));
	}

	// Token: 0x06002D19 RID: 11545 RVA: 0x000D7404 File Offset: 0x000D5604
	private void UpdateDropView(ItemDef itemDef, DropViewAtomMesh dropBig, DropViewAtomSprite dropSmall)
	{
		if (dropBig == null || dropSmall == null)
		{
			return;
		}
		if (itemDef == null)
		{
			dropBig.Deactivate();
			dropSmall.Deactivate();
			return;
		}
		ItemSize itemSize = itemDef.itemSize;
		if (itemSize != ItemSize.Small)
		{
			if (itemSize != ItemSize.Big)
			{
				return;
			}
			if (dropBig.gameObject.activeSelf && !string.IsNullOrEmpty(dropBig.IconId) && dropBig.IconId == itemDef.iconId)
			{
				return;
			}
			dropBig.Activate(itemDef.iconId);
			dropSmall.Deactivate();
			return;
		}
		else
		{
			if (dropSmall.gameObject.activeSelf && !string.IsNullOrEmpty(dropSmall.IconId) && dropSmall.IconId == itemDef.iconId)
			{
				return;
			}
			dropSmall.Activate(itemDef.iconId);
			dropBig.Deactivate();
			return;
		}
	}

	// Token: 0x06002D1A RID: 11546 RVA: 0x000D74C5 File Offset: 0x000D56C5
	private void UpdateDropViewGroup(ItemDef itemDef)
	{
		this.UpdateDropView(itemDef, this.dropViewBig, this.dropViewSmall);
		this.ApplyDropViewInteractionState();
	}

	// Token: 0x06002D1B RID: 11547 RVA: 0x000D74E0 File Offset: 0x000D56E0
	private void ApplyDropViewInteractionState()
	{
		DropViewAtomMesh dropViewAtomMesh = this.dropViewBig;
		if (dropViewAtomMesh != null)
		{
			dropViewAtomMesh.SetInteractionState(this.dropViewUnderInteraction);
		}
		DropViewAtomSprite dropViewAtomSprite = this.dropViewSmall;
		if (dropViewAtomSprite == null)
		{
			return;
		}
		dropViewAtomSprite.SetInteractionState(this.dropViewUnderInteraction);
	}

	// Token: 0x040023EF RID: 9199
	private const float START_PHYSICS_COL_RADIUS = 0.001f;

	// Token: 0x040023F0 RID: 9200
	[SerializeField]
	private Transform bubblePoint;

	// Token: 0x040023F1 RID: 9201
	private Wgo wgo;

	// Token: 0x040023F2 RID: 9202
	private Object3D object3D;

	// Token: 0x040023F3 RID: 9203
	[SerializeField]
	private List<Object3D> cachedObjects3D;

	// Token: 0x040023F4 RID: 9204
	private Transform customBubblePoint;

	// Token: 0x040023F5 RID: 9205
	[SerializeField]
	private List<WgoPartState> variations = new List<WgoPartState>();

	// Token: 0x040023F6 RID: 9206
	[SerializeField]
	private List<int> customRotationSequence = new List<int>();

	// Token: 0x040023F7 RID: 9207
	[SerializeField]
	private SpawnConfiguration spawnConfiguration;

	// Token: 0x040023F8 RID: 9208
	[SerializeField]
	private List<Collider> interactableColliders = new List<Collider>();

	// Token: 0x040023F9 RID: 9209
	[SerializeField]
	private List<DockPoint> dockPoints = new List<DockPoint>();

	// Token: 0x040023FA RID: 9210
	[SerializeField]
	private CapsuleCollider capsulePhysicsCollider;

	// Token: 0x040023FB RID: 9211
	[SerializeField]
	private DropViewAtomMesh dropViewBig;

	// Token: 0x040023FC RID: 9212
	[SerializeField]
	private DropViewAtomSprite dropViewSmall;

	// Token: 0x040023FD RID: 9213
	[SerializeField]
	[CanBeNull]
	private ReservoirView reservoirView;

	// Token: 0x040023FE RID: 9214
	[SerializeField]
	[CanBeNull]
	private RiverBodyReceiver riverBodyReceiver;

	// Token: 0x040023FF RID: 9215
	[SerializeField]
	[CanBeNull]
	private List<FlagPlacementPoint> flagPlacementPoints;

	// Token: 0x04002400 RID: 9216
	private ConditionalDrawer conditionalDrawer;

	// Token: 0x04002401 RID: 9217
	private bool dropViewUnderInteraction;

	// Token: 0x04002402 RID: 9218
	private GameObject assetReference;

	// Token: 0x04002403 RID: 9219
	[CanBeNull]
	private AnimationComponentBase animationComponent;

	// Token: 0x04002404 RID: 9220
	[SerializeField]
	[Space]
	[CanBeNull]
	private TextMeshPro label;

	// Token: 0x04002407 RID: 9223
	private bool animationComponentInitialized;

	// Token: 0x04002408 RID: 9224
	private Tween physicsCollAnimTween;
}
