using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// Token: 0x0200067F RID: 1663
public class Wgo : MonoBehaviour, IBuildRemovable, IBubbleDrawable, IChunkableObject, IChunkVisibilityStateReceiver, ICombatEntity, IKnockbackable
{
	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06002C15 RID: 11285 RVA: 0x000D0614 File Offset: 0x000CE814
	// (remove) Token: 0x06002C16 RID: 11286 RVA: 0x000D0648 File Offset: 0x000CE848
	public static event Action<Wgo> OnWgoSpawn;

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06002C17 RID: 11287 RVA: 0x000D067C File Offset: 0x000CE87C
	// (remove) Token: 0x06002C18 RID: 11288 RVA: 0x000D06B0 File Offset: 0x000CE8B0
	public static event Action<Wgo> OnWgoDestroy;

	// Token: 0x170006E2 RID: 1762
	// (get) Token: 0x06002C19 RID: 11289 RVA: 0x000D06E3 File Offset: 0x000CE8E3
	// (set) Token: 0x06002C1A RID: 11290 RVA: 0x000D06F0 File Offset: 0x000CE8F0
	private string WGOId
	{
		get
		{
			return this.data.id;
		}
		set
		{
			this.data.id = value;
		}
	}

	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x06002C1B RID: 11291 RVA: 0x000D06FE File Offset: 0x000CE8FE
	// (set) Token: 0x06002C1C RID: 11292 RVA: 0x000D070B File Offset: 0x000CE90B
	private string CustomTag
	{
		get
		{
			return this.data.CustomTag;
		}
		set
		{
			this.data.CustomTag = value;
		}
	}

	// Token: 0x06002C1D RID: 11293 RVA: 0x000D0719 File Offset: 0x000CE919
	private void RuntimeDirection(Direction direction)
	{
		if (this.data == null)
		{
			return;
		}
		this.data.direction.Value = direction.ConvertToVector2XZ();
	}

	// Token: 0x06002C1E RID: 11294 RVA: 0x000D073C File Offset: 0x000CE93C
	private void Editor_Talk()
	{
		if (this.data == null)
		{
			return;
		}
		string text = (string.IsNullOrWhiteSpace(this.editorTalkText) ? "Lorem ipsum dolor sit amet" : this.editorTalkText);
		Bubble.Talk(new PhraseData
		{
			text = text,
			isPlayer = false,
			npcWgoData = this.data,
			speechType = SpeechBubbleType.Talk
		});
	}

	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x06002C1F RID: 11295 RVA: 0x000D07A0 File Offset: 0x000CE9A0
	// (set) Token: 0x06002C20 RID: 11296 RVA: 0x000D07AD File Offset: 0x000CE9AD
	public bool IsHidden
	{
		get
		{
			return this.data.IsHidden;
		}
		set
		{
			this.data.IsHidden = value;
		}
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x06002C21 RID: 11297 RVA: 0x000D07BB File Offset: 0x000CE9BB
	// (set) Token: 0x06002C22 RID: 11298 RVA: 0x000D07C3 File Offset: 0x000CE9C3
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

	// Token: 0x06002C23 RID: 11299 RVA: 0x000D07CC File Offset: 0x000CE9CC
	public DockPointData GetDockPointData(DockPoint dockPoint)
	{
		WgoPart mainWgoPart = this.MainWgoPart;
		if (mainWgoPart == null)
		{
			return null;
		}
		return mainWgoPart.GetDockPointData(dockPoint);
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x06002C24 RID: 11300 RVA: 0x000D07E0 File Offset: 0x000CE9E0
	// (set) Token: 0x06002C25 RID: 11301 RVA: 0x000D07E8 File Offset: 0x000CE9E8
	public WgoData Data
	{
		get
		{
			return this.data;
		}
		private set
		{
			this.data = value;
		}
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x06002C26 RID: 11302 RVA: 0x000D07F1 File Offset: 0x000CE9F1
	public bool HasData
	{
		get
		{
			return this.hasData;
		}
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x06002C27 RID: 11303 RVA: 0x000D07F9 File Offset: 0x000CE9F9
	public string Id
	{
		get
		{
			return this.WGOId;
		}
	}

	// Token: 0x170006E9 RID: 1769
	// (get) Token: 0x06002C28 RID: 11304 RVA: 0x000D0801 File Offset: 0x000CEA01
	// (set) Token: 0x06002C29 RID: 11305 RVA: 0x000D0809 File Offset: 0x000CEA09
	public WgoPart MainWgoPart { get; private set; }

	// Token: 0x170006EA RID: 1770
	// (get) Token: 0x06002C2A RID: 11306 RVA: 0x000D0812 File Offset: 0x000CEA12
	public List<WgoPart> AdditionalWgoParts
	{
		get
		{
			return this.additionalWgoParts;
		}
	}

	// Token: 0x170006EB RID: 1771
	// (get) Token: 0x06002C2B RID: 11307 RVA: 0x000D081A File Offset: 0x000CEA1A
	public IWGOInteractionHandler InteractionHandler
	{
		get
		{
			return this.interactionHandler;
		}
	}

	// Token: 0x170006EC RID: 1772
	// (get) Token: 0x06002C2C RID: 11308 RVA: 0x000D0822 File Offset: 0x000CEA22
	public bool IsDespawning
	{
		get
		{
			return this.isDespawning;
		}
	}

	// Token: 0x170006ED RID: 1773
	// (get) Token: 0x06002C2D RID: 11309 RVA: 0x000D082A File Offset: 0x000CEA2A
	public IReadOnlyList<DockPoint> DockPoints
	{
		get
		{
			WgoPart mainWgoPart = this.MainWgoPart;
			if (mainWgoPart == null)
			{
				return null;
			}
			return mainWgoPart.DockPoints;
		}
	}

	// Token: 0x170006EE RID: 1774
	// (get) Token: 0x06002C2E RID: 11310 RVA: 0x000D083D File Offset: 0x000CEA3D
	// (set) Token: 0x06002C2F RID: 11311 RVA: 0x000D0845 File Offset: 0x000CEA45
	public RiverBodyReceiver RiverBodyReceiver { get; private set; }

	// Token: 0x170006EF RID: 1775
	// (get) Token: 0x06002C30 RID: 11312 RVA: 0x000D084E File Offset: 0x000CEA4E
	// (set) Token: 0x06002C31 RID: 11313 RVA: 0x000D0856 File Offset: 0x000CEA56
	public bool UpdatePosByData { get; set; } = true;

	// Token: 0x06002C32 RID: 11314 RVA: 0x000D0860 File Offset: 0x000CEA60
	public static Wgo Spawn(WgoData data, Transform parentTransform, bool registerInChunkManagerIfStatic = true, bool ignoreChunkRegistration = false, bool applyDefaultWgoPartState = false, bool recheckVisibilityOnSpawn = false)
	{
		Wgo wgo = new GameObject().AddComponent<Wgo>();
		wgo.Data = data;
		wgo.WGOId = data.id;
		wgo.wgoMovementAdjustComponent = wgo.gameObject.AddComponent<WgoMovementAdjustComponent>();
		wgo.pendingApplyDefaultState = applyDefaultWgoPartState;
		Transform transform = wgo.transform;
		transform.parent = parentTransform;
		transform.position = wgo.Data.Position;
		transform.localScale = wgo.Data.Scale;
		wgo.InitDataBindings();
		if (!wgo.Data.isTempObject)
		{
			if (!wgo.Data.gdPointsRegistered)
			{
				wgo.Data.gdPointsRegistered = true;
				wgo.RegisterGDPointsFromBakedData();
			}
			else
			{
				GardenBedNavigation.TryRebuildOnViewRespawn(wgo.Data);
			}
			if (!wgo.Data.wasSpawnedAtLeastOnce)
			{
				wgo.Data.wasSpawnedAtLeastOnce = true;
				WGODef workbenchExtensionLogicDef = GameBalance.Me.GetWorkbenchExtensionLogicDef(wgo.Data.id);
				if ((workbenchExtensionLogicDef != null && GameBalance.Me.workbenchesWhichUseExtensions.Contains(workbenchExtensionLogicDef)) || GameBalance.Me.IsWorkbenchExtensionId(wgo.Data.id))
				{
					wgo.needsWorkbenchExtensionInit = true;
				}
			}
		}
		WorldZoneData worldZoneData = data.WorldZoneData;
		try
		{
			if (worldZoneData != null && (!worldZoneData.Definition.hasCustomQualityZones || (worldZoneData.Definition.hasCustomQualityZones && worldZoneData.ContainsCustomQualityZonePrecisely(data.Position))))
			{
				wgo.SetWorldZoneWidgets(worldZoneData);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(string.Format("wgo spawn:[{0}] wz:[{1}] wzData.Definition:[{2}] exception:{3}", new object[]
			{
				data.id,
				(worldZoneData != null) ? worldZoneData.id : null,
				((worldZoneData != null) ? worldZoneData.Definition : null) == null,
				ex
			}));
		}
		wgo.PrecomputeSerializedBounds();
		wgo.gameObject.SetActive(false);
		wgo.spawnCompleted = true;
		if (!ignoreChunkRegistration)
		{
			WGODef definition = wgo.Data.Definition;
			if ((definition != null && definition.isMovable) || registerInChunkManagerIfStatic)
			{
				wgo.TryRegisterInChunkManager();
				if (recheckVisibilityOnSpawn)
				{
					LazySingleton<ChunkManager>.Instance.RequestVisibilityRecheck(wgo);
				}
				else
				{
					wgo.UpdateChunkVisibility(true);
				}
			}
		}
		Action<Wgo> onWgoSpawn = Wgo.OnWgoSpawn;
		if (onWgoSpawn != null)
		{
			onWgoSpawn(wgo);
		}
		return wgo;
	}

	// Token: 0x06002C33 RID: 11315 RVA: 0x000D0A78 File Offset: 0x000CEC78
	public void RemoveWithData()
	{
		MainGame.WorldData.RemoveWgoDataFromGameScene(this.data.UniqueId);
	}

	// Token: 0x06002C34 RID: 11316 RVA: 0x000D0A90 File Offset: 0x000CEC90
	public void DespawnAfterDataWasRemoved()
	{
		this.isDespawning = true;
		if (MainGame.PlayerController.PlayerInteractionComponent.WgoUnderInteraction == this)
		{
			this.interactionHandler.OnInteractionTargetExit();
		}
		this.SetWorldZoneWidgets(null);
		if (this.ShouldDelayForCraftHintCompletion())
		{
			this.pendingDespawnAfterCraftHintCompletion = true;
			WgoBubbleDisplayHandler.Hide(this);
			return;
		}
		this.DeInit();
		if (this.hasCustomDestroyMoment)
		{
			return;
		}
		Action<Wgo> onWgoDestroy = Wgo.OnWgoDestroy;
		if (onWgoDestroy != null)
		{
			onWgoDestroy(this);
		}
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002C35 RID: 11317 RVA: 0x000D0B10 File Offset: 0x000CED10
	public List<LazyWidgetDataBase> GetWidgetData()
	{
		List<LazyWidgetDataBase> list = new List<LazyWidgetDataBase>();
		if (this.data.IsHidden)
		{
			return list;
		}
		WorkbenchAdditionWorldIconPresenter.State state;
		if (WorkbenchAdditionWorldIconPresenter.TryGet(this.data.UniqueId, out state))
		{
			list.Add(new UIWorkbenchAdditionWorldIconWidgetData(state.IconId, state.IsInRange));
		}
		if (this.ShouldShowConveyorNoPowerIcon())
		{
			list.Add(new UIConveyorNoPowerIconWidgetData());
		}
		if (this.ShouldShowZombieNoStorageIcon())
		{
			list.Add(new UIZombieNoStorageIconWidgetData());
		}
		if (ZombieDeliveryIndication.ShouldShowNoCaretakerAssigned(this.data))
		{
			list.Add(UIWorkbenchAdditionWorldIconWidgetData.StationWithoutCaretaker());
		}
		if (((this.data.HpComponent.WasDamagedAtLeastOnce && this.data.HpComponent.Hp > 0) || this.data.HpComponent.firstDamageWasMax) && !this.data.Definition.hasInfiniteHp)
		{
			this.data.HpComponent.firstDamageWasMax = false;
			if (!this.IsActiveCombatant)
			{
				list.Add(new HpBarWidgetData(this.data.HpComponent));
			}
			else
			{
				HpBarSimpleWidgetData.SpriteType spriteType = ((this.TeamType == LazyConsts.Fighting.TeamType.Player) ? HpBarSimpleWidgetData.SpriteType.Ally : HpBarSimpleWidgetData.SpriteType.Enemy);
				HpWidgetDataCustomParameters hpWidgetDataCustomParameters;
				if (this.MainWgoPart == null || !this.MainWgoPart.TryGetComponent<HpWidgetDataCustomParameters>(out hpWidgetDataCustomParameters))
				{
					list.Add(new HpBarSimpleWidgetData(this.data.HpComponent, 30f, 5f, spriteType));
				}
				else
				{
					float num;
					float num2;
					list.Add(new HpBarSimpleWidgetData(this.data.HpComponent, hpWidgetDataCustomParameters.GetCustomWidthIfHasSet(out num) ? num : 30f, hpWidgetDataCustomParameters.GetCustomHeightIfHasSet(out num2) ? num2 : 5f, spriteType));
				}
			}
		}
		string text;
		int num3;
		if (this.TryGetFuelContainerStoredItem(out text, out num3))
		{
			list.Add(new UIQualityTooltipWidgetData(text, num3.ToInvariantCultureString()));
		}
		string text2;
		int num4;
		if (this.TryGetPowerSourceGearOutput(out text2, out num4))
		{
			list.Add(new UIQualityTooltipWidgetData(text2, num4.ToInvariantCultureString()));
		}
		CraftElementBase currentCraftElement = this.data.CraftComponent.CurrentCraftElement;
		if (ZombieDeliveryIndication.IsCraftStalledWithoutCaretaker(this.data))
		{
			list.Add(UIWorkbenchAdditionWorldIconWidgetData.NoCaretakerInZone());
		}
		else if (currentCraftElement != null && !currentCraftElement.Def.isHidden && currentCraftElement.ParamsData.craftParamsType != CraftParamsData.CraftParamsType.GardenGrowing && (this.data.CraftComponent.IsStarted || this.data.CraftComponent.Status == CraftComponentStatus.ReadyToStartCraft || this.data.CraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft || this.data.CraftComponent.IsQueueDelayed || this.data.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp || this.data.CraftComponent.Status == CraftComponentStatus.WaitingForOutputDrop))
		{
			list.Add(new UICraftHintWidgetData(this.data.CraftComponent));
		}
		bool isControlsEnabledForInteractionHints = MainGame.PlayerController.IsControlsEnabledForInteractionHints;
		bool flag = MainGame.Instance.GameSave.questSystemData.WgoHasReadyToFinishQuest(this.Id);
		this.lastReadyToFinishQuest = flag;
		if (this.interactionHandler != null && this.data.IsInteractable && ((this.data.Events.Count > 0 && isControlsEnabledForInteractionHints) || ((MainGame.PlayerController.PlayerInteractionComponent.WgoUnderInteraction == this || MainGame.PlayerController.PlayerWorkComponent.Wgo == this) && (this.interactionHandler.HasInteraction(MainGame.PlayerController) || this.interactionHandler.HasInteraction2(MainGame.PlayerController))) || (flag && isControlsEnabledForInteractionHints)))
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
		if (MainGame.PlayerData.CurrentWorldZoneData != null && this.data.WorldZoneData == MainGame.PlayerData.CurrentWorldZoneData)
		{
			list.AddRange(this.worldZoneWidgetsData);
		}
		return list;
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x000D0F50 File Offset: 0x000CF150
	public void FireEvent(string eventName)
	{
		this.Data.FireEvent(eventName);
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000D0F5E File Offset: 0x000CF15E
	public void SetCustomBubblePoint(Transform customBubblePoint)
	{
		if (this.MainWgoPart == null)
		{
			return;
		}
		this.MainWgoPart.SetCustomBubblePoint(customBubblePoint);
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x000D0F7C File Offset: 0x000CF17C
	public bool TryRotate(bool isPrev = false)
	{
		if (this.MainWgoPart.Variations.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < this.AdditionalWgoParts.Count; i++)
		{
			if (this.AdditionalWgoParts[i].Variations.Count == 0)
			{
				return false;
			}
		}
		if (!this.MainWgoPart.Rotate(isPrev))
		{
			return false;
		}
		for (int j = 0; j < this.AdditionalWgoParts.Count; j++)
		{
			if (!this.MainWgoPart.Rotate(isPrev))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x000D1004 File Offset: 0x000CF204
	public bool CanBeRotated()
	{
		if (!this.MainWgoPart.CanBeRotated("", -1))
		{
			return false;
		}
		for (int i = 0; i < this.AdditionalWgoParts.Count; i++)
		{
			if (!this.AdditionalWgoParts[i].CanBeRotated("", -1))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x000D1058 File Offset: 0x000CF258
	public void UpdateWgoPartState(bool applyDefaultWgoPartState = false)
	{
		if (applyDefaultWgoPartState || string.IsNullOrEmpty(this.data.MainWgoPartData.variationId))
		{
			WgoPart mainWgoPart = this.MainWgoPart;
			if (mainWgoPart != null)
			{
				mainWgoPart.ApplyWgoPartState();
			}
		}
		else
		{
			WgoPart mainWgoPart2 = this.MainWgoPart;
			if (mainWgoPart2 != null)
			{
				mainWgoPart2.ApplyWgoPartState(this.data.MainWgoPartData.variationId, this.data.MainWgoPartData.rotationIndex);
			}
		}
		for (int i = 0; i < this.data.AdditionalWgoPartsData.Count; i++)
		{
			WgoPart wgoPart = this.AdditionalWgoParts[i];
			if (wgoPart != null)
			{
				wgoPart.ApplyWgoPartState();
			}
		}
		this.data.RefreshCustomNavMeshCutUnit();
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x00002318 File Offset: 0x00000518
	public void ValidateSpawnerComponent()
	{
	}

	// Token: 0x06002C3C RID: 11324 RVA: 0x000D1100 File Offset: 0x000CF300
	public void SetSelectionTint(Color color, float amount)
	{
		WgoPart mainWgoPart = this.MainWgoPart;
		if (mainWgoPart != null)
		{
			mainWgoPart.SetSelectionTint(color, amount);
		}
		for (int i = 0; i < this.AdditionalWgoParts.Count; i++)
		{
			WgoPart wgoPart = this.AdditionalWgoParts[i];
			if (wgoPart != null)
			{
				wgoPart.SetSelectionTint(color, amount);
			}
		}
	}

	// Token: 0x06002C3D RID: 11325 RVA: 0x000D1150 File Offset: 0x000CF350
	public void PlayHPTickAnimation(bool isFirstHit)
	{
		Action<Object3DMesh> hpTickAnimMethod = null;
		if (!string.IsNullOrEmpty(this.data.Definition.sfxOnActionTick))
		{
			LazyAudio.PlayAtGameObject(this.data.Definition.sfxOnActionTick, base.transform, SpatialType.sound3D, true);
		}
		string wgoGroup = this.data.Definition.wgoGroup;
		if (!(wgoGroup == "stones"))
		{
			if (!(wgoGroup == "trees") && !(wgoGroup == "bushes") && !(wgoGroup == "collectable_bushes"))
			{
				return;
			}
			if (isFirstHit && !string.IsNullOrEmpty(this.data.Definition.worldFxOnHpFirstHit))
			{
				WorldFX.Spawn(this.BubbleDrawablePosition, this.data.Definition.worldFxOnHpFirstHit, null, this.data.Definition.worldFxOnHpFirstHitActionSize);
			}
			Action <>9__2;
			hpTickAnimMethod = delegate(Object3DMesh o3DMesh)
			{
				Action action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate
					{
						if (string.IsNullOrEmpty(this.data.Definition.worldFxOnHpActionTick))
						{
							return;
						}
						WorldFX.Spawn(this.transform.position, this.data.Definition.worldFxOnHpActionTick, null, this.data.Definition.worldFxOnHpActionSize);
					});
				}
				o3DMesh.PlayAnimationChop(action);
			};
		}
		else
		{
			if (isFirstHit && !string.IsNullOrEmpty(this.data.Definition.worldFxOnHpFirstHit))
			{
				WorldFX.Spawn(this.BubbleDrawablePosition, this.data.Definition.worldFxOnHpFirstHit, null, this.data.Definition.worldFxOnHpFirstHitActionSize);
			}
			if (string.IsNullOrEmpty(this.data.Definition.worldFxOnHpActionTick))
			{
				return;
			}
			WorldFX.Spawn(base.transform.position, this.data.Definition.worldFxOnHpActionTick, null, this.data.Definition.worldFxOnHpActionSize);
		}
		if (hpTickAnimMethod != null)
		{
			Object3D object3D = this.MainWgoPart.Object3D;
			if (object3D == null)
			{
				return;
			}
			object3D.Object3DMeshes.ForEach(delegate(Object3DMesh o3DMesh)
			{
				hpTickAnimMethod(o3DMesh);
			});
		}
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x000D1310 File Offset: 0x000CF510
	public void PlayDestroyAnimation()
	{
		Action<Object3DMesh> destructionMeshAnimMethod = null;
		Action<HorizontalSprite> destructionHorSpriteAnimMethod = null;
		bool isDestructionMeshAnimMethodCalled = false;
		if (!string.IsNullOrEmpty(this.data.Definition.sfxOnDie))
		{
			LazyAudio.PlayAtGameObject(this.data.Definition.sfxOnDie, base.transform, SpatialType.sound3D, true);
		}
		string wgoGroup = this.data.Definition.wgoGroup;
		if (wgoGroup == "trees" || wgoGroup == "bushes")
		{
			Action <>9__5;
			Action <>9__6;
			Action <>9__4;
			destructionMeshAnimMethod = delegate(Object3DMesh o3DMesh)
			{
				Action action;
				if ((action = <>9__4) == null)
				{
					action = (<>9__4 = delegate
					{
						if (isDestructionMeshAnimMethodCalled)
						{
							return;
						}
						isDestructionMeshAnimMethodCalled = true;
						if (string.IsNullOrEmpty(this.data.Definition.worldFxOnDie))
						{
							this.data.TriggerCustomDeathMoment();
							return;
						}
						ValueTuple<Vector3, Vector3> fxOnDieParams2 = this.GetFxOnDieParams();
						if (this.data.Definition.customDeathTime > 0f)
						{
							this.hasCustomDestroyMoment = true;
							Vector3 item = fxOnDieParams2.Item1;
							string worldFxOnDie = this.data.Definition.worldFxOnDie;
							Action action2;
							if ((action2 = <>9__5) == null)
							{
								action2 = (<>9__5 = delegate
								{
									this.hasCustomDestroyMoment = false;
									this.isPlayingDestroyAnim = false;
									this.DespawnAfterDataWasRemoved();
								});
							}
							WorldFX.Spawn(item, worldFxOnDie, action2, fxOnDieParams2.Item2);
							MainGame.Instance.GameSave.wgoCustomDeathSystemData.AddCustomDeath(this.data);
							return;
						}
						Vector3 item2 = fxOnDieParams2.Item1;
						string worldFxOnDie2 = this.data.Definition.worldFxOnDie;
						Action action3;
						if ((action3 = <>9__6) == null)
						{
							action3 = (<>9__6 = delegate
							{
								this.isPlayingDestroyAnim = false;
								this.data.TriggerCustomDeathMoment();
							});
						}
						WorldFX.Spawn(item2, worldFxOnDie2, action3, fxOnDieParams2.Item2);
					});
				}
				o3DMesh.PlayAnimationDestruction(action, false);
			};
			destructionHorSpriteAnimMethod = delegate(HorizontalSprite horSprite)
			{
				LazyTimer.AddTimer(LazySingletonSO<GlobalResources>.Instance.fxSettings.treeDestroyGndSpriteDisableTime, delegate
				{
					if (horSprite != null)
					{
						horSprite.gameObject.SetActive(false);
					}
				}, null);
			};
			this.data.SetCustomDeathMoment();
			if (!this.MainWgoPart.Object3D)
			{
				return;
			}
			this.MainWgoPart.Object3D.Object3DMeshes.ForEach(delegate(Object3DMesh o3DMesh)
			{
				destructionMeshAnimMethod(o3DMesh);
			});
			this.MainWgoPart.Object3D.HorizontalSprites.ForEach(delegate(HorizontalSprite horSprite)
			{
				destructionHorSpriteAnimMethod(horSprite);
			});
			this.isPlayingDestroyAnim = true;
			return;
		}
		else
		{
			if (string.IsNullOrEmpty(this.data.Definition.worldFxOnDie))
			{
				return;
			}
			ValueTuple<Vector3, Vector3> fxOnDieParams = this.GetFxOnDieParams();
			WorldFX.Spawn(fxOnDieParams.Item1, this.data.Definition.worldFxOnDie, null, fxOnDieParams.Item2);
			return;
		}
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x000D1484 File Offset: 0x000CF684
	[return: TupleElementNames(new string[] { "position", "size" })]
	private ValueTuple<Vector3, Vector3> GetFxOnDieParams()
	{
		if (this.data.Definition.getFxOnDieSizeFromBuildCollider)
		{
			if (this.MainWgoPart == null)
			{
				return new ValueTuple<Vector3, Vector3>(base.transform.position, this.data.Definition.fxOnDieSize);
			}
			BoxCollider boxCollider = Wgo.<GetFxOnDieParams>g__FindBuildArea|99_0((this.MainWgoPart.CurrentWgoPartState == null) ? this.MainWgoPart.gameObject : this.MainWgoPart.CurrentWgoPartState.gameObject, LayerMask.NameToLayer("BuildArea"));
			if (boxCollider != null)
			{
				Vector3 vector = new Vector3(boxCollider.size.x, 1f, boxCollider.size.z);
				return new ValueTuple<Vector3, Vector3>(boxCollider.transform.TransformPoint(boxCollider.center), vector);
			}
		}
		return new ValueTuple<Vector3, Vector3>(base.transform.position, this.data.Definition.fxOnDieSize);
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x000D1574 File Offset: 0x000CF774
	public DockPoint TryGetDockPointForWorker(bool findNearest = false, Vector3 searcherPosition = default(Vector3))
	{
		IReadOnlyList<DockPoint> dockPoints = this.DockPoints;
		if (dockPoints != null && dockPoints.Count == 0)
		{
			return null;
		}
		if (!findNearest)
		{
			foreach (DockPoint dockPoint in this.DockPoints)
			{
				if (dockPoint.gameObject.activeInHierarchy && !dockPoint.DontUseForWorkerPlacement && dockPoint.IsForZombie)
				{
					return dockPoint;
				}
			}
			foreach (DockPoint dockPoint2 in this.DockPoints)
			{
				if (!dockPoint2.DontUseForWorkerPlacement && dockPoint2.gameObject.activeInHierarchy)
				{
					return dockPoint2;
				}
			}
			return null;
		}
		DockPoint dockPoint3 = null;
		DockPoint dockPoint4 = null;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		foreach (DockPoint dockPoint5 in this.DockPoints)
		{
			if (dockPoint5.gameObject.activeInHierarchy && !dockPoint5.DontUseForWorkerPlacement)
			{
				float num3 = Vector3.Distance(dockPoint5.transform.position, searcherPosition);
				if (dockPoint5.IsForZombie)
				{
					if (num3 < num)
					{
						num = num3;
						dockPoint3 = dockPoint5;
					}
				}
				else if (num3 < num2)
				{
					num2 = num3;
					dockPoint4 = dockPoint5;
				}
			}
		}
		if (!(dockPoint3 != null))
		{
			return dockPoint4;
		}
		return dockPoint3;
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x000D170C File Offset: 0x000CF90C
	public void SetInteractableCollidersState(bool isActive)
	{
		WgoPart mainWgoPart = this.MainWgoPart;
		if (mainWgoPart == null)
		{
			return;
		}
		mainWgoPart.InteractableColliders.ForEach(delegate(Collider collider)
		{
			collider.enabled = isActive;
		});
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x000D1748 File Offset: 0x000CF948
	public void SetLayerToAllColliders(int layer)
	{
		if (this.areCollidersLayersOverrode)
		{
			this.ResetLayerFromAllColliders();
		}
		foreach (Collider collider in base.gameObject.GetComponentsInChildren<Collider>())
		{
			this.collidersPreviousLayer.TryAdd(collider.GetHashCode(), collider.gameObject.layer);
			collider.gameObject.layer = layer;
		}
		this.areCollidersLayersOverrode = true;
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x000D17B4 File Offset: 0x000CF9B4
	public void ResetLayerFromAllColliders()
	{
		if (!this.areCollidersLayersOverrode)
		{
			return;
		}
		this.areCollidersLayersOverrode = false;
		foreach (Collider collider in base.gameObject.GetComponentsInChildren<Collider>())
		{
			int num;
			if (this.collidersPreviousLayer.TryGetValue(collider.GetHashCode(), out num))
			{
				collider.gameObject.layer = num;
			}
		}
		this.collidersPreviousLayer.Clear();
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x000D181C File Offset: 0x000CFA1C
	private void InitDataBindings()
	{
		this.hasData = true;
		this.IgnoreMultiFlag = new MultiFlagOR<ChunkingIgnoreType>();
		this.data.OnPositionChanged += this.HandleChangedPos;
		this.data.HpComponent.OnFirstDamageDealt += this.HandleFirstDamageDealt;
		this.data.HpComponent.OnFullHpRestored += this.DrawWidgets;
		this.data.CraftComponent.OnStatusChanged += this.HandleCraftStatusChange;
		this.data.CraftComponent.OnPreFinishHoldReleased += this.HandleCraftPreFinishHoldReleased;
		this.data.OnWorkerChanged += this.DrawWidgets;
		this.data.OnAdditionalWgoPartAdd += this.OnAdditionalWgoPartAdd;
		this.data.OnAdditionalWgoPartRemove += this.OnAdditionalWgoPartRemove;
		this.data.OnInteractionEventChanged += this.DrawWidgets;
		this.data.OnInteractableStateChanged += this.HandleInteractedStateChanged;
		this.data.OnHiddenStateChanged += this.HandleHiddenChanged;
		this.wgoMovementAdjustComponent.Init(this.data);
		this.data.OnToolTickApply += this.PlayHPTickAnimation;
		this.data.OnOccuredDeath += this.PlayDestroyAnimation;
		this.data.Inventory.OnItemsAdd += this.HandleItemInsertSound;
		if (this.data.Definition.isFuelContainer)
		{
			this.data.Inventory.OnItemsAdd += this.HandleFuelContainerInventoryChanged;
			this.data.Inventory.OnItemsRemove += this.HandleFuelContainerInventoryChanged;
		}
		this.SubscribeQuestFinishIndicator();
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			zombieWgoData.OnAnimationStateChanged += this.HandleAnimationStateChanged;
			zombieWgoData.CrafterOnOrderAddedEvent += this.DrawWidgets;
			zombieWgoData.CrafterOnOrderRemovedEvent += this.DrawWidgets;
			zombieWgoData.OnSetOverheadItem += this.SetOverheadItem;
			zombieWgoData.OnRemoveOverheadItem += this.RemoveOverheadItem;
		}
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x000D1A60 File Offset: 0x000CFC60
	private void InitVisualBindings()
	{
		if (!this.MainWgoPart)
		{
			return;
		}
		this.TryBindAnimationComponent();
		this.UpdateDropPoint();
		this.interactionItemPoint = base.GetComponentInChildren<InteractingItemPoint>(true);
		this.RiverBodyReceiver = base.GetComponentInChildren<RiverBodyReceiver>(true);
		RiverBodyReceiver riverBodyReceiver = this.RiverBodyReceiver;
		if (riverBodyReceiver != null)
		{
			riverBodyReceiver.Init(this);
		}
		this.interactionHandler = this.GetNewInteractionHandler();
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			zombieWgoData.OnSetInteractingItem += this.SetInteractingItem;
			zombieWgoData.OnRemoveInteractingItem += this.RemoveInteractingItem;
			zombieWgoData.SyncPorterBackpackLayer();
			WgoPart mainWgoPart = this.MainWgoPart;
			if (mainWgoPart != null)
			{
				mainWgoPart.TryAnimatePhysicsCollider();
			}
			if (this.animationComponent != null)
			{
				if (MainGame.Instance.GameSave.militaryBaseData.ContainsFighter(this.data, false))
				{
					this.InitZombieFighter();
				}
				if (!this.data.MovementComponent.IsMoving)
				{
					zombieWgoData.InvokeOnAnimationStateChanged(false);
				}
			}
			Item carriedPortableItem = this.GetCarriedPortableItem(zombieWgoData);
			if (carriedPortableItem != null && !carriedPortableItem.IsEmpty)
			{
				if (carriedPortableItem.Definition.itemSize == ItemSize.Big)
				{
					this.SetOverheadItem(carriedPortableItem, false);
				}
				else
				{
					this.SetInteractingItem(carriedPortableItem);
				}
			}
		}
		ConveyorWgoData conveyorWgoData = this.data as ConveyorWgoData;
		if (conveyorWgoData != null && MainGame.Instance.GameSave.conveyorSystemData.conveyorComponents.Contains(conveyorWgoData.ConveyorComponent))
		{
			ConveyorAnimator componentInChildren = base.GetComponentInChildren<ConveyorAnimator>();
			if (componentInChildren != null)
			{
				componentInChildren.TryRegister();
			}
		}
		this.CleanupChunkableComponents();
		WgoCustomComponentSerializer.RestoreComponents(this, this.data);
		this.BindGDPointViews();
		if (this.needsWorkbenchExtensionInit)
		{
			this.needsWorkbenchExtensionInit = false;
			WGODef workbenchExtensionLogicDef = GameBalance.Me.GetWorkbenchExtensionLogicDef(this.data.id);
			bool flag = workbenchExtensionLogicDef != null && GameBalance.Me.workbenchesWhichUseExtensions.Contains(workbenchExtensionLogicDef);
			Debug.Log(string.Format("Try register workbench extensions for WGO {0}, parent? :{1}", this.data.id, flag));
			base.StartCoroutine(this.TryRegisterWorkbenchExtensionDelayed(flag));
		}
		ConditionalDrawer[] componentsInChildren = base.gameObject.GetComponentsInChildren<ConditionalDrawer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].UpdateVisualsByConditions();
		}
		this.DoCheckDelayedAnimation();
		this.visualBindingsInitialized = true;
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x000D1C85 File Offset: 0x000CFE85
	private void UpdateDropPoint()
	{
		this.dropPoint = base.GetComponentInChildren<DropPoint>(false);
		if (this.dropPoint == null)
		{
			this.dropPoint = base.GetComponentInChildren<DropPoint>(true);
		}
	}

	// Token: 0x06002C47 RID: 11335 RVA: 0x000D1CB0 File Offset: 0x000CFEB0
	private void TryBindAnimationComponent()
	{
		if (this.animationBindingsBound)
		{
			return;
		}
		this.animationComponent = (this.MainWgoPart ? this.MainWgoPart.AnimationComponent : null);
		if (this.animationComponent == null && this.MainWgoPart && this.MainWgoPart.EnsureAnimationComponentInitialized())
		{
			this.animationComponent = this.MainWgoPart.AnimationComponent;
		}
		if (this.animationComponent == null)
		{
			return;
		}
		this.data.MovementComponent.SetCallbacks(delegate(float speed)
		{
			this.animationComponent.SetWalkAnimationSpeedMultiplier(speed);
			this.animationComponent.SetState(global::AnimationState.Walk);
		}, delegate
		{
			this.animationComponent.SetState(global::AnimationState.Idle);
		}, delegate(Vector2 dir)
		{
			this.animationComponent.SetDirection(dir);
		});
		if (this.data.MovementComponent.IsMoving)
		{
			this.animationComponent.SetState(global::AnimationState.Walk);
			this.animationComponent.SetDirection(this.data.MovableDirection);
		}
		else
		{
			this.animationComponent.SetDirection(this.data.direction.Value);
		}
		this.data.OnAnimationTriggerSet += this.HandleAnimationSetTrigger;
		this.data.OnAnimationLayerSet += this.HandleAnimationSetLayerWeight;
		this.data.OnAnimationStateSet += this.HandleAnimationSetState;
		WgoData wgoData = this.data;
		if (wgoData != null)
		{
			wgoData.TryFireSerializedTrigger();
		}
		this.animationBindingsBound = true;
	}

	// Token: 0x06002C48 RID: 11336 RVA: 0x000D1E14 File Offset: 0x000D0014
	private void DeInitVisualBindings()
	{
		this.visualBindingsInitialized = false;
		this.animationBindingsBound = false;
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			this.RemoveInteractingItem();
			this.RemoveOverheadItem();
			zombieWgoData.OnSetInteractingItem -= this.SetInteractingItem;
			zombieWgoData.OnRemoveInteractingItem -= this.RemoveInteractingItem;
		}
		WgoCustomComponentSerializer.ResetComponents(this);
		WgoData wgoData = this.data;
		if (wgoData != null)
		{
			MovementComponent movementComponent = wgoData.MovementComponent;
			if (movementComponent != null)
			{
				movementComponent.DeInit();
			}
		}
		this.data.OnAnimationTriggerSet -= this.HandleAnimationSetTrigger;
		this.data.OnAnimationLayerSet -= this.HandleAnimationSetLayerWeight;
		this.data.OnAnimationStateSet -= this.HandleAnimationSetState;
		this.animationComponent = null;
		this.dropPoint = null;
		this.interactionItemPoint = null;
		RiverBodyReceiver riverBodyReceiver = this.RiverBodyReceiver;
		if (riverBodyReceiver != null)
		{
			riverBodyReceiver.DeInit();
		}
		this.RiverBodyReceiver = null;
		ConveyorAnimator componentInChildren = base.GetComponentInChildren<ConveyorAnimator>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.Unregister();
	}

	// Token: 0x06002C49 RID: 11337 RVA: 0x000D1F12 File Offset: 0x000D0112
	private bool ShouldDelayForCraftHintCompletion()
	{
		return this.hasData && this.data != null && this.data.CraftComponent != null && this.data.CraftComponent.IsPreFinishHeld;
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x000D1F43 File Offset: 0x000D0143
	private void HandleCraftPreFinishHoldReleased()
	{
		if (this.pendingDespawnAfterCraftHintCompletion)
		{
			this.pendingDespawnAfterCraftHintCompletion = false;
			this.DespawnAfterDataWasRemoved();
			return;
		}
		if (this.pendingRefreshAfterCraftHintCompletion)
		{
			this.pendingRefreshAfterCraftHintCompletion = false;
			this.RefreshVisuals();
		}
	}

	// Token: 0x06002C4B RID: 11339 RVA: 0x000D1F70 File Offset: 0x000D0170
	private void RefreshVisuals()
	{
		if (!this.spawnCompleted)
		{
			return;
		}
		if (!this || !base.gameObject)
		{
			return;
		}
		if (this.isVisible && !this.data.IsHidden)
		{
			if (this.wgoPartsLoaded)
			{
				base.gameObject.SetActive(true);
				if (!this.visualBindingsInitialized)
				{
					this.InitVisualBindings();
				}
				else
				{
					this.DoCheckDelayedAnimation();
				}
				this.DrawWidgets();
				return;
			}
			base.gameObject.SetActive(false);
			this.RequestWgoPartsLoad(this.pendingApplyDefaultState, false);
			this.pendingApplyDefaultState = false;
			this.ValidateSpawnerComponent();
			if (this.wgoPartsLoaded && !this.visualBindingsInitialized)
			{
				this.InitVisualBindings();
			}
			if (!this.wgoPartsLoaded)
			{
				return;
			}
			this.boundsCalculated = false;
			return;
		}
		else
		{
			if (this.ShouldDelayForCraftHintCompletion())
			{
				this.pendingRefreshAfterCraftHintCompletion = true;
				WgoBubbleDisplayHandler.Hide(this);
				return;
			}
			if (this.chunkVisibilityState == ChunkVisibilityState.Prewarm)
			{
				base.gameObject.SetActive(false);
				if (!this.wgoPartsLoaded && !this.wgoPartsLoading)
				{
					this.RequestWgoPartsLoad(this.pendingApplyDefaultState, true);
					this.pendingApplyDefaultState = false;
				}
				return;
			}
			if (this.chunkVisibilityState == ChunkVisibilityState.Visible && !this.isVisible)
			{
				return;
			}
			LazySingleton<WgoPartLoadManager>.Instance.CancelLoad(this);
			this.wgoPartsLoading = false;
			base.gameObject.SetActive(false);
			if (this.wgoPartsLoaded)
			{
				WgoBubbleDisplayHandler.Hide(this);
				this.DeInitVisualBindings();
				this.ReleaseWgoPartsToPool();
				this.wgoPartsLoaded = false;
			}
			return;
		}
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x000D20D6 File Offset: 0x000D02D6
	private void RequestWgoPartsLoad(bool applyDefaultWgoPartState, bool async = true)
	{
		if (this.wgoPartsLoading || this.wgoPartsLoaded)
		{
			return;
		}
		this.wgoPartsLoading = true;
		LazySingleton<WgoPartLoadManager>.Instance.RequestLoad(this, applyDefaultWgoPartState, async);
	}

	// Token: 0x06002C4D RID: 11341 RVA: 0x000D2100 File Offset: 0x000D0300
	public void CompleteVisualPartsLoad(WgoPart mainPart, List<WgoPart> additionalParts, List<WgoPartData> additionalData, bool applyDefaultWgoPartState)
	{
		bool flag = this.isVisible && this.data != null && !this.data.IsHidden;
		bool flag2 = this.chunkVisibilityState == ChunkVisibilityState.Prewarm;
		if ((!flag && !flag2) || this.data == null)
		{
			LazySingleton<WgoPartLoadManager>.Instance.CancelLoad(this);
			this.wgoPartsLoading = false;
			this.ReleaseIncomingParts(mainPart, additionalParts);
			return;
		}
		this.MainWgoPart = mainPart;
		this.MainWgoPart.InitVisuals(null, this.data.MainWgoPartData, this.data.Definition);
		this.AdditionalWgoParts.Clear();
		for (int i = 0; i < additionalParts.Count; i++)
		{
			WgoPart wgoPart = additionalParts[i];
			if (!(wgoPart == null))
			{
				WgoPartData wgoPartData = additionalData[i];
				wgoPart.InitVisuals(null, wgoPartData, this.data.Definition);
				this.AdditionalWgoParts.Add(wgoPart);
			}
		}
		if (flag)
		{
			base.gameObject.SetActive(true);
			this.InitVisualBindings();
		}
		this.wgoPartsLoading = false;
		this.wgoPartsLoaded = true;
		this.ReinitBalanceRelatedStuff();
		this.UpdateWgoPartState(applyDefaultWgoPartState);
		this.TryBindAnimationComponent();
		this.DoCheckDelayedAnimation();
		this.ApplyZombieAnimationStateAfterWgoPartState();
		this.ValidateSpawnerComponent();
		this.boundsCalculated = false;
		if (flag)
		{
			this.DrawWidgets();
			this.UpdateDropPoint();
		}
	}

	// Token: 0x06002C4E RID: 11342 RVA: 0x000D2240 File Offset: 0x000D0440
	public void HandleVisualPartsLoadFailed()
	{
		this.wgoPartsLoading = false;
	}

	// Token: 0x06002C4F RID: 11343 RVA: 0x000D224C File Offset: 0x000D044C
	private void ApplyZombieAnimationStateAfterWgoPartState()
	{
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		if (zombieWgoData == null || this.MainWgoPart == null)
		{
			return;
		}
		AnimationComponentBase componentInChildren = this.MainWgoPart.GetComponentInChildren<AnimationComponentBase>();
		if (componentInChildren == null)
		{
			return;
		}
		componentInChildren.SetDirection(this.data.direction.Value);
		if (zombieWgoData.ZombieType == ZombieType.Porter)
		{
			float num = ((zombieWgoData.GetGameResInt("is_staying_at_porter_station") == 1) ? 0f : 1f);
			componentInChildren.SetLayerWeight(4, num);
		}
	}

	// Token: 0x06002C50 RID: 11344 RVA: 0x000D22D0 File Offset: 0x000D04D0
	private void ReleaseIncomingParts(WgoPart mainPart, List<WgoPart> additionalParts)
	{
		if (mainPart != null)
		{
			if (!string.IsNullOrEmpty(mainPart.PooledAddressableKey))
			{
				this.ReleaseWgoPart(mainPart);
			}
			else
			{
				global::UnityEngine.Object.Destroy(mainPart.gameObject);
			}
		}
		for (int i = 0; i < additionalParts.Count; i++)
		{
			WgoPart wgoPart = additionalParts[i];
			if (!(wgoPart == null))
			{
				if (!string.IsNullOrEmpty(wgoPart.PooledAddressableKey))
				{
					this.ReleaseWgoPart(wgoPart);
				}
				else
				{
					global::UnityEngine.Object.Destroy(wgoPart.gameObject);
				}
			}
		}
	}

	// Token: 0x06002C51 RID: 11345 RVA: 0x000D234C File Offset: 0x000D054C
	private void ReleaseWgoPartsToPool()
	{
		if (this.MainWgoPart != null && !string.IsNullOrEmpty(this.MainWgoPart.PooledAddressableKey))
		{
			FightingAgent fightingAgent;
			if (this.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
			{
				fightingAgent.DeInitForPool();
			}
			this.ReleaseWgoPart(this.MainWgoPart);
			this.MainWgoPart = null;
		}
		for (int i = 0; i < this.AdditionalWgoParts.Count; i++)
		{
			WgoPart wgoPart = this.AdditionalWgoParts[i];
			if (wgoPart != null && !string.IsNullOrEmpty(wgoPart.PooledAddressableKey))
			{
				this.ReleaseWgoPart(wgoPart);
			}
		}
		this.AdditionalWgoParts.Clear();
	}

	// Token: 0x06002C52 RID: 11346 RVA: 0x000D23EC File Offset: 0x000D05EC
	private void CleanupChunkableComponents()
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

	// Token: 0x06002C53 RID: 11347 RVA: 0x000D2438 File Offset: 0x000D0638
	public void OnZombieItemsChanged(Inventory zombieInventory)
	{
		AnimationComponent.Layers layers = AnimationComponent.Layers.Armor;
		Item itemByGroupId = zombieInventory.GetItemByGroupId("weapon");
		Item itemByType = zombieInventory.GetItemByType(ItemType.BodyArmor);
		AnimationComponent animationComponent = this.animationComponent as AnimationComponent;
		if (animationComponent == null)
		{
			return;
		}
		if (itemByType.IsEmpty)
		{
			animationComponent.ChangeSkinPreset(ZombieSkinHelper.GetPresetForWgoData(this.data, this.data.Definition.zombieRollDataId));
			animationComponent.ResetArmorLayers();
			animationComponent.UseAdditionalStepSound = false;
			return;
		}
		SkinPresetGK2 skinPresetGK = ZombieSkinHelper.CopySkinPresetAndChangeHead(SkinPresetGK2.Load("9019_zombie_ally"), this.data, "zombie_worker");
		skinPresetGK.arms.colorReplaceType = ColorReplaceType.USE_PALETTE_COLOR_REPLACE;
		WgoPartData mainWgoPartData = GameScene.GetWgoViewGlobal(SGuid.Parse(this.data.GameResStr.Get("fighters_flag", ""))).Data.MainWgoPartData;
		skinPresetGK.body.palette = ZombieCustomizationConfig.GetBodyReplacePalette(mainWgoPartData.variationId).palette;
		skinPresetGK.arms.palette = ZombieCustomizationConfig.GetArmsArmorReplacePalette(itemByType.Definition.id).palette;
		animationComponent.ChangeSkinPreset(skinPresetGK);
		if (!itemByGroupId.IsEmpty)
		{
			layers = ((itemByGroupId.Definition.type == ItemType.Bow) ? AnimationComponent.Layers.ArmorWithBow : AnimationComponent.Layers.ArmorWithPike);
		}
		animationComponent.ResetArmorLayers();
		animationComponent.SetLayerWeight(layers, 1f);
		animationComponent.AdditionalStepSoundId = "ally_armor_footsteps";
		animationComponent.Animator.Update(0f);
	}

	// Token: 0x06002C54 RID: 11348 RVA: 0x000D2594 File Offset: 0x000D0794
	public void InitZombieFighter()
	{
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		if (zombieWgoData == null)
		{
			Debug.LogError("Wgo: [" + this.data.id + "] is not zombie fighter");
			return;
		}
		this.OnZombieItemsChanged(new Inventory(zombieWgoData.ZombieItem));
		zombieWgoData.OnEquipmentChanged += this.OnZombieItemsChanged;
	}

	// Token: 0x06002C55 RID: 11349 RVA: 0x000D25F4 File Offset: 0x000D07F4
	private void DeInit()
	{
		if (!this.hasData)
		{
			return;
		}
		this.hasData = false;
		this.RemoveInteractingItem();
		this.RemoveOverheadItem();
		this.TryUnregisterInChunkManager();
		this.data.DeInit();
		WgoBubbleDisplayHandler.Hide(this);
		if (this.data.Definition != null && this.data.Definition.conveyorType != ConveyorElementType.None)
		{
			this.OnConveyorObjectRemoved();
		}
		if (this.wgoPartsLoaded)
		{
			this.DeInitVisualBindings();
			this.wgoPartsLoaded = false;
		}
		LazySingleton<WgoPartLoadManager>.Instance.CancelLoad(this);
		this.wgoPartsLoading = false;
		this.DestroyParts();
		this.data.OnPositionChanged -= this.HandleChangedPos;
		this.data.HpComponent.OnFirstDamageDealt -= this.HandleFirstDamageDealt;
		this.data.HpComponent.OnFullHpRestored -= this.DrawWidgets;
		this.data.CraftComponent.OnStatusChanged -= this.HandleCraftStatusChange;
		this.data.CraftComponent.OnPreFinishHoldReleased -= this.HandleCraftPreFinishHoldReleased;
		this.data.OnWorkerChanged -= this.DrawWidgets;
		this.data.OnAdditionalWgoPartAdd -= this.OnAdditionalWgoPartAdd;
		this.data.OnAdditionalWgoPartRemove -= this.OnAdditionalWgoPartRemove;
		this.data.OnInteractionEventChanged -= this.DrawWidgets;
		this.data.OnInteractableStateChanged -= this.HandleInteractedStateChanged;
		this.data.OnHiddenStateChanged -= this.HandleHiddenChanged;
		this.data.OnToolTickApply -= this.PlayHPTickAnimation;
		this.data.OnOccuredDeath -= this.PlayDestroyAnimation;
		this.data.Inventory.OnItemsAdd -= this.HandleItemInsertSound;
		if (this.data.Definition.isFuelContainer)
		{
			this.data.Inventory.OnItemsAdd -= this.HandleFuelContainerInventoryChanged;
			this.data.Inventory.OnItemsRemove -= this.HandleFuelContainerInventoryChanged;
		}
		this.UnsubscribeQuestFinishIndicator();
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		if (zombieWgoData != null)
		{
			zombieWgoData.OnAnimationStateChanged -= this.HandleAnimationStateChanged;
			zombieWgoData.CrafterOnOrderAddedEvent -= this.DrawWidgets;
			zombieWgoData.CrafterOnOrderRemovedEvent -= this.DrawWidgets;
			zombieWgoData.OnSetOverheadItem -= this.SetOverheadItem;
			zombieWgoData.OnRemoveOverheadItem -= this.RemoveOverheadItem;
			if (this.animationComponent != null)
			{
				zombieWgoData.OnEquipmentChanged -= this.OnZombieItemsChanged;
			}
		}
	}

	// Token: 0x06002C56 RID: 11350 RVA: 0x000D28BA File Offset: 0x000D0ABA
	private void DoCheckDelayedAnimation()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (this.animationComponent != null)
		{
			base.StartCoroutine(this.CheckDelayedAnimation());
		}
	}

	// Token: 0x06002C57 RID: 11351 RVA: 0x000D28E5 File Offset: 0x000D0AE5
	private IEnumerator CheckDelayedAnimation()
	{
		yield return null;
		if (!this || !this.hasData || this.animationComponent == null)
		{
			yield break;
		}
		bool isMoving = this.data.MovementComponent.IsMoving;
		if (isMoving && this.animationComponent.GetState() != global::AnimationState.Walk)
		{
			this.animationComponent.SetState(global::AnimationState.Walk);
		}
		else if (!isMoving)
		{
			ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
			if (zombieWgoData != null)
			{
				zombieWgoData.InvokeOnAnimationStateChanged(false);
				ConditionalDrawer[] componentsInChildren = base.GetComponentsInChildren<ConditionalDrawer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].UpdateVisualsByConditions();
				}
			}
		}
		ZombieWgoData zombieWgoData2 = this.data as ZombieWgoData;
		if (zombieWgoData2 != null)
		{
			this.RestoreZombiePortableItemVisuals(zombieWgoData2);
		}
		this.animationComponent.SetDirection(isMoving ? this.data.MovableDirection : this.data.direction.Value);
		yield break;
	}

	// Token: 0x06002C58 RID: 11352 RVA: 0x000D28F4 File Offset: 0x000D0AF4
	private Item GetCarriedPortableItem(ZombieWgoData zombie)
	{
		if (zombie.ZombieType != ZombieType.ConveyorTransporter)
		{
			return zombie.CaretakerPortableItem;
		}
		return zombie.ConveyorTransporterPortableItem;
	}

	// Token: 0x06002C59 RID: 11353 RVA: 0x000D290C File Offset: 0x000D0B0C
	private void RestoreZombiePortableItemVisuals(ZombieWgoData zombie)
	{
		zombie.SyncPorterBackpackLayer();
		Item carriedPortableItem = this.GetCarriedPortableItem(zombie);
		if (carriedPortableItem == null || carriedPortableItem.IsEmpty)
		{
			return;
		}
		if (carriedPortableItem.Definition.itemSize == ItemSize.Big)
		{
			this.SetOverheadItem(carriedPortableItem, false);
			return;
		}
		if (this.interactingItem == null)
		{
			this.SetInteractingItem(carriedPortableItem);
			return;
		}
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(5, 1f);
			this.animationComponent.DisableDropView();
		}
	}

	// Token: 0x06002C5A RID: 11354 RVA: 0x000D298C File Offset: 0x000D0B8C
	public void ForceDeath()
	{
		Inventory inventory = this.data.Inventory;
		if (((inventory != null) ? inventory.Data : null) != null)
		{
			this.DropItemsFromInventory();
			this.data.Inventory.Data.RemoveAllItems();
		}
		this.data.ForceDeath();
	}

	// Token: 0x06002C5B RID: 11355 RVA: 0x000D29D9 File Offset: 0x000D0BD9
	private void OnEnable()
	{
		this.DoCheckDelayedAnimation();
	}

	// Token: 0x06002C5C RID: 11356 RVA: 0x000D29E1 File Offset: 0x000D0BE1
	private void OnDestroy()
	{
		if (this.isPlayingDestroyAnim)
		{
			this.data.TriggerCustomDeathMoment();
		}
		this.TryUnregisterInChunkManager();
		this.DeInit();
	}

	// Token: 0x06002C5D RID: 11357 RVA: 0x000D2A04 File Offset: 0x000D0C04
	private void ReinitBalanceRelatedStuff()
	{
		this.interactionHandler = this.GetNewInteractionHandler();
		if (this.interactionHandler is ReservoirInteractionHandler)
		{
			List<FishingDef> list = GameBalance.Me.fishingDefs.FindAll((FishingDef x) => x.reservoirId == this.data.id);
			bool flag = this.data.IsGameResEmpty();
			GameSave gameSave = MainGame.Instance.GameSave;
			using (List<FishingDef>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FishingDef fishingDef = enumerator.Current;
					if (flag)
					{
						this.data.AddGameRes(fishingDef.fishId, fishingDef.baseCount);
					}
					string gameLogicName = fishingDef.id + "_restore";
					if (!gameSave.gameLogicSystemData.gameLogics.Exists((GameLogicData x) => x.id == gameLogicName))
					{
						float num = (float)gameSave.environmentData.Day + fishingDef.regenTime / gameSave.environmentData.EnvironmentEngine.gameplayDayInMinutes;
						CustomGameLogicData customGameLogicData = new CustomGameLogicData(gameLogicName, num, delegate
						{
							if (this.data.GetGameResInt(fishingDef.fishId) < fishingDef.baseCount)
							{
								this.data.AddGameRes(fishingDef.fishId, 1);
							}
						});
						customGameLogicData.Init();
						gameSave.gameLogicSystemData.gameLogics.Add(customGameLogicData);
					}
				}
			}
		}
	}

	// Token: 0x06002C5E RID: 11358 RVA: 0x000D2B78 File Offset: 0x000D0D78
	private WgoPart LoadWgoPart(WgoPartData wgoPartData, bool isMain = false)
	{
		string text;
		if (isMain)
		{
			if (this.data.Definition == null)
			{
				Debug.LogError("Can not load main wgo part, Definition is null [" + this.WGOId + "]");
				return null;
			}
			text = this.data.Definition.ResolveAssetId(this.WGOId, this.data);
		}
		else
		{
			text = wgoPartData.id;
		}
		string text2 = "Assets/AddressableAssets/WGOs/" + text + ".prefab";
		AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(text2);
		GameObject gameObject = asyncOperationHandle.WaitForCompletion();
		WgoPart wgoPart;
		if (asyncOperationHandle.Status == AsyncOperationStatus.Failed || gameObject == null)
		{
			Debug.LogError("Can't spawn wgo with id [" + text + "] at " + text2);
			wgoPart = new GameObject
			{
				transform = 
				{
					parent = base.transform,
					localPosition = Vector3.zero
				},
				name = "[empty WgoPart] " + text
			}.AddComponent<WgoPart>();
		}
		else
		{
			wgoPart = global::UnityEngine.Object.Instantiate<WgoPart>(gameObject.GetComponent<WgoPart>(), base.transform);
		}
		if (wgoPart != null)
		{
			wgoPart.InitVisuals(gameObject, wgoPartData, this.data.Definition);
		}
		if (isMain)
		{
			this.MainWgoPart = wgoPart;
		}
		else
		{
			this.AdditionalWgoParts.Add(wgoPart);
		}
		this.boundsCalculated = false;
		return wgoPart;
	}

	// Token: 0x06002C5F RID: 11359 RVA: 0x000D2CB0 File Offset: 0x000D0EB0
	private IWGOInteractionHandler GetNewInteractionHandler()
	{
		WGODef definition = this.data.Definition;
		WGODef.InteractionType interactionType = ((definition != null) ? definition.interactionType : WGODef.InteractionType.None);
		CraftElement craftElement = this.data.CraftComponent.CurrentCraftElement as CraftElement;
		if (craftElement != null && craftElement.Definition != null && craftElement.Definition.isObjDestroyCraft)
		{
			return new CraftInteractionHandler().Init(this);
		}
		if (!this.data.IsInteractable)
		{
			return new DefaultInteractionHandler().Init(this);
		}
		switch (interactionType)
		{
		case WGODef.InteractionType.Work:
			return new WorkInteractionHandler().Init(this);
		case WGODef.InteractionType.Craft:
			return new CraftInteractionHandler().Init(this);
		case WGODef.InteractionType.Script:
			return new ScriptInteractionHandler().Init(this);
		case WGODef.InteractionType.CustomInteraction:
			return new CustomInteractionHandler().Init(this);
		case WGODef.InteractionType.Builder:
			return new BuildInteractionHandler().Init(this);
		case WGODef.InteractionType.Chest:
			return new ChestInteractionHandler().Init(this);
		case WGODef.InteractionType.Grave:
			return new GraveInteractionHandler().Init(this);
		case WGODef.InteractionType.Ladder:
			return new LadderInteractionHandler().Init(this);
		case WGODef.InteractionType.PrayerStand:
			return new PrayerStandInteractionHandler().Init(this);
		case WGODef.InteractionType.Autopsy:
			return new AutopsyInteractionHandler().Init(this);
		case WGODef.InteractionType.Garden:
			return new GardenInteractionHandler().Init(this);
		case WGODef.InteractionType.Zombie:
			return new ZombieInteractionHandler().Init(this);
		case WGODef.InteractionType.Survey:
			return new SurveyInteractionHandler().Init(this);
		case WGODef.InteractionType.Alchemy:
			return new AlchemyInteractionHandler().Init(this);
		case WGODef.InteractionType.Reservoir:
			return new ReservoirInteractionHandler().Init(this);
		case WGODef.InteractionType.Barricade:
			return new BarricadeInteractionHandler().Init(this);
		case WGODef.InteractionType.Flag:
			return new FlagInteractionHandler().Init(this);
		case WGODef.InteractionType.Station:
			return new StationInteractionHandler().Init(this);
		case WGODef.InteractionType.TownBuildingPlace:
			return new TownBuildingPlaceInteractionHandler().Init(this);
		case WGODef.InteractionType.ConveyorCell:
			return new ConveyorCellInteractionHandler().Init(this);
		case WGODef.InteractionType.PowerSource:
			return new PowerSourceInteractionHandler().Init(this);
		case WGODef.InteractionType.TakeAll:
			return new TakeAllInteractionHandler().Init(this);
		case WGODef.InteractionType.FlagStand:
			return new FlagStandInteractionHandler().Init(this);
		case WGODef.InteractionType.Embalm:
			return new EmbalmInteractionHandler().Init(this);
		case WGODef.InteractionType.FighterContainer:
			return new FightersContainerInteractionHandler().Init(this);
		case WGODef.InteractionType.FightBuilder:
			return new FightBuildInteractionHandler().Init(this);
		case WGODef.InteractionType.TownPalette:
			return new TownPaletteInteractionHandler().Init(this);
		case WGODef.InteractionType.ChoirPlace:
			return new ChoirPlaceInteractionHandler().Init(this);
		case WGODef.InteractionType.ZombieCarrier:
			return new ZombieCarrierInteractionHandler().Init(this);
		case WGODef.InteractionType.ZombieSawmill:
			return new ZombieSawmillInteractionHandler().Init(this);
		case WGODef.InteractionType.TeleportMilestone:
			return new TeleportMilestoneInteractionHandler().Init(this);
		case WGODef.InteractionType.PorterStation:
			return new PorterStationInteractionHandler().Init(this);
		case WGODef.InteractionType.ZombieMine:
			return new ZombieMineInteractionHandler().Init(this);
		case WGODef.InteractionType.ZombieClay:
			return new ZombieClayInteractionHandler().Init(this);
		case WGODef.InteractionType.ZombieSand:
			return new ZombieSandInteractionHandler().Init(this);
		case WGODef.InteractionType.GardenStation:
			return new GardenStationInteractionHandler().Init(this);
		case WGODef.InteractionType.CargoLift:
			return new CargoLiftInteractionHandler().Init(this);
		case WGODef.InteractionType.Crematorium:
			return new CrematoriumInteractionHandler().Init(this);
		case WGODef.InteractionType.ConveyorTransporterStation:
			return new ConveyorTransporterStationInteractionHandler().Init(this);
		case WGODef.InteractionType.PanicReductionMachine:
			return new PanicReductionMachineInteractionHandler().Init(this);
		case WGODef.InteractionType.ResurrectionTable:
			return new ResurrectionInteractionHandler().Init(this);
		case WGODef.InteractionType.WellUpgrade:
			return new WellUpgradeInteractionHandler().Init(this);
		case WGODef.InteractionType.RiverDump:
			return new RiverDumpInteractionHandler().Init(this);
		}
		return new DefaultInteractionHandler().Init(this);
	}

	// Token: 0x06002C60 RID: 11360 RVA: 0x000D3000 File Offset: 0x000D1200
	private void HandleChangedPos(Vector3 newPosition)
	{
		if (!this.UpdatePosByData)
		{
			return;
		}
		base.transform.position = newPosition;
	}

	// Token: 0x06002C61 RID: 11361 RVA: 0x000D3018 File Offset: 0x000D1218
	private Vector3 GetBubblePointPos()
	{
		if (this.MainWgoPart == null)
		{
			return this.data.BubblePos;
		}
		if (this.RiverBodyReceiver != null && this.RiverBodyReceiver.UseDynamicBubble)
		{
			if (MainGame.PlayerController == null)
			{
				return this.data.BubblePos;
			}
			PlayerData playerData = MainGame.PlayerController.PlayerData;
			return this.RiverBodyReceiver.GetDynamicBubblePos(playerData.position.Value);
		}
		else
		{
			if (this.MainWgoPart.CustomBubblePoint != null)
			{
				return this.MainWgoPart.CustomBubblePoint.position;
			}
			if (this.MainWgoPart.BubblePoint != null)
			{
				return this.MainWgoPart.BubblePoint.position;
			}
			return this.data.BubblePos;
		}
	}

	// Token: 0x06002C62 RID: 11362 RVA: 0x000D30E8 File Offset: 0x000D12E8
	private void LoadWgoParts()
	{
		this.LoadWgoPart(this.data.MainWgoPartData, true);
		for (int i = 0; i < this.data.AdditionalWgoPartsData.Count; i++)
		{
			this.LoadWgoPart(this.data.AdditionalWgoPartsData[i], false);
		}
	}

	// Token: 0x06002C63 RID: 11363 RVA: 0x000D313C File Offset: 0x000D133C
	private void DestroyParts()
	{
		if (this.MainWgoPart != null)
		{
			if (!string.IsNullOrEmpty(this.MainWgoPart.PooledAddressableKey) && !this.Data.isTempObject)
			{
				FightingAgent fightingAgent;
				if (this.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
				{
					fightingAgent.DeInitForPool();
				}
				this.ReleaseWgoPart(this.MainWgoPart);
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.MainWgoPart.gameObject);
			}
			this.MainWgoPart = null;
		}
		this.ReleaseAdditionalWgoParts();
	}

	// Token: 0x06002C64 RID: 11364 RVA: 0x000D31B8 File Offset: 0x000D13B8
	private void ReleaseAdditionalWgoParts()
	{
		for (int i = 0; i < this.AdditionalWgoParts.Count; i++)
		{
			WgoPart wgoPart = this.AdditionalWgoParts[i];
			if (!(wgoPart == null))
			{
				if (!string.IsNullOrEmpty(wgoPart.PooledAddressableKey) && !this.Data.isTempObject)
				{
					this.ReleaseWgoPart(wgoPart);
				}
				else
				{
					global::UnityEngine.Object.Destroy(wgoPart.gameObject);
				}
			}
		}
		this.AdditionalWgoParts.Clear();
	}

	// Token: 0x06002C65 RID: 11365 RVA: 0x000D322A File Offset: 0x000D142A
	private void ReleaseWgoPart(WgoPart part)
	{
		part.KillPhysicsCollAnimTween();
		this.data.OnReleaseWgoPartToPool();
		LazySingleton<WgoPartPool>.Instance.Release(part.PooledAddressableKey, part);
	}

	// Token: 0x06002C66 RID: 11366 RVA: 0x000D324E File Offset: 0x000D144E
	private void HandleFirstDamageDealt()
	{
		Debug.Log("HandleFirstDamageDealt: " + this.WGOId);
		if (this.data.HpComponent.Hp == 0)
		{
			this.data.HpComponent.firstDamageWasMax = true;
		}
		this.DrawWidgets();
	}

	// Token: 0x06002C67 RID: 11367 RVA: 0x000D328E File Offset: 0x000D148E
	private void HandleCraftStatusChange(CraftComponentStatus craftStatusEvent)
	{
		if (craftStatusEvent == CraftComponentStatus.ReadyToStartCraft || craftStatusEvent == CraftComponentStatus.Started || craftStatusEvent == CraftComponentStatus.QueueDelayed || craftStatusEvent == CraftComponentStatus.Canceled || craftStatusEvent == CraftComponentStatus.Finished || craftStatusEvent == CraftComponentStatus.ReadyToFinishAutoCraft)
		{
			this.DrawWidgets();
		}
	}

	// Token: 0x06002C68 RID: 11368 RVA: 0x000D32B0 File Offset: 0x000D14B0
	private async void OnAdditionalWgoPartAdd(WgoPartData wgoPartData)
	{
		if (this.wgoPartsLoaded && !this.wgoPartsLoading)
		{
			string text = "Assets/AddressableAssets/WGOs/" + wgoPartData.id + ".prefab";
			WgoPart wgoPart = await LazySingleton<WgoPartPool>.Instance.GetAsync(text, this);
			if (!this.wgoPartsLoaded || this.wgoPartsLoading || !this.isVisible || this.data.IsHidden)
			{
				if (wgoPart != null && !string.IsNullOrEmpty(wgoPart.PooledAddressableKey))
				{
					this.ReleaseWgoPart(wgoPart);
				}
			}
			else
			{
				if (wgoPart != null)
				{
					wgoPart.InitVisuals(null, wgoPartData, this.data.Definition);
					this.AdditionalWgoParts.Add(wgoPart);
					wgoPart.ApplyWgoPartState();
				}
				WorldZoneData worldZoneData = this.data.WorldZoneData;
				if (worldZoneData != null)
				{
					worldZoneData.NotifyWgoDataChanged();
				}
			}
		}
	}

	// Token: 0x06002C69 RID: 11369 RVA: 0x000D32F0 File Offset: 0x000D14F0
	private void OnAdditionalWgoPartRemove(WgoPartData wgoPartData)
	{
		if (!this.wgoPartsLoaded)
		{
			return;
		}
		int i = 0;
		while (i < this.AdditionalWgoParts.Count)
		{
			if (this.AdditionalWgoParts[i].Id == wgoPartData.id)
			{
				WgoPart wgoPart = this.AdditionalWgoParts[i];
				if (!string.IsNullOrEmpty(wgoPart.PooledAddressableKey))
				{
					this.ReleaseWgoPart(wgoPart);
				}
				else
				{
					global::UnityEngine.Object.Destroy(wgoPart.gameObject);
				}
				this.AdditionalWgoParts.RemoveAt(i);
				WorldZoneData worldZoneData = this.data.WorldZoneData;
				if (worldZoneData == null)
				{
					return;
				}
				worldZoneData.NotifyWgoDataChanged();
				return;
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06002C6A RID: 11370 RVA: 0x000D338C File Offset: 0x000D158C
	private void HandleAnimationStateChanged(global::AnimationState animationState, bool playSound = false)
	{
		Debug.Log(string.Format("HandleAnimationStateChanged ZombieWgoData animationState:{0}", animationState));
		WgoPart mainWgoPart = this.MainWgoPart;
		if (((mainWgoPart != null) ? mainWgoPart.AnimationComponent : null) != null)
		{
			if (this.data.MovementComponent.IsMoving)
			{
				Debug.Log(string.Format("[Wgo] Skip zombie anim state [{0}] for [{1}]: wgo is moving", animationState, this.data.id));
				return;
			}
			this.MainWgoPart.AnimationComponent.SetState(animationState);
			if (playSound)
			{
				ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
				if (zombieWgoData != null && zombieWgoData.CaretakerState == ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory)
				{
					LazyAudio.PlayAtGameObject("zombie_chest_rummage", base.transform, SpatialType.sound3D, true);
				}
			}
		}
	}

	// Token: 0x06002C6B RID: 11371 RVA: 0x000D343C File Offset: 0x000D163C
	public void SetOverheadItem(Item item, bool playSound = false)
	{
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(3, 1f);
			this.animationComponent.SetOverheadItem(item);
			if (playSound && item.id == "wood")
			{
				LazyAudio.PlayAtGameObject("oh_wood_grab", base.transform, SpatialType.sound3D, true);
			}
		}
		this.isOverheadActive = true;
	}

	// Token: 0x06002C6C RID: 11372 RVA: 0x000D34A3 File Offset: 0x000D16A3
	private void RemoveOverheadItem()
	{
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(3, 0f);
			this.animationComponent.RemoveOverheadItem();
		}
		this.isOverheadActive = false;
	}

	// Token: 0x06002C6D RID: 11373 RVA: 0x000D34D8 File Offset: 0x000D16D8
	private void SetInteractingItem(Item item)
	{
		if (this.interactionItemPoint == null)
		{
			return;
		}
		if (this.isOverheadActive && this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(3, 0f);
			this.animationComponent.RemoveOverheadItem();
		}
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(5, 1f);
		}
		if (item != null)
		{
			if (this.interactingItem != null)
			{
				this.interactingItem.DisableBubble();
			}
			this.interactingItem = UIInteractingItem.ShowInteractingItem(new Item(item.id, item.Count), this.interactionItemPoint.transform);
			if (this.animationComponent != null)
			{
				this.animationComponent.DisableDropView();
			}
		}
	}

	// Token: 0x06002C6E RID: 11374 RVA: 0x000D35A4 File Offset: 0x000D17A4
	private void RemoveInteractingItem()
	{
		if (!this.isOverheadActive && this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(5, 0f);
		}
		if (this.animationComponent != null)
		{
			this.animationComponent.EnableDropView();
		}
		if (this.interactingItem != null)
		{
			this.interactingItem.DisableBubble();
		}
		this.interactingItem = null;
	}

	// Token: 0x06002C6F RID: 11375 RVA: 0x000D3611 File Offset: 0x000D1811
	private void HandleAnimationSetTrigger(string triggerName)
	{
		if (this.animationComponent != null)
		{
			this.UpdateFlag(ChunkingIgnoreType.Animation, true);
			this.animationComponent.SetTrigger(triggerName);
		}
	}

	// Token: 0x06002C70 RID: 11376 RVA: 0x000D3635 File Offset: 0x000D1835
	private void HandleAnimationSetLayerWeight(int layerIndex, float weight)
	{
		if (this.animationComponent != null)
		{
			this.animationComponent.SetLayerWeight(layerIndex, weight);
		}
	}

	// Token: 0x06002C71 RID: 11377 RVA: 0x000D3652 File Offset: 0x000D1852
	private void HandleAnimationSetState(global::AnimationState animationState)
	{
		if (this.animationComponent != null)
		{
			this.UpdateFlag(ChunkingIgnoreType.Animation, true);
			this.animationComponent.SetState(animationState);
		}
	}

	// Token: 0x06002C72 RID: 11378 RVA: 0x000D3678 File Offset: 0x000D1878
	private void HandleItemInsertSound(List<Item> items)
	{
		if (this.data.id == "wood_container")
		{
			LazyAudio.PlayAtGameObject("oh_wood_container_drop", base.transform, SpatialType.sound3D, true);
			return;
		}
		if (this.data.id == "grave_empty")
		{
			LazyAudio.PlayAtGameObject("oh_corpse_grave_drop", base.transform, SpatialType.sound3D, true);
		}
	}

	// Token: 0x06002C73 RID: 11379 RVA: 0x000D36DA File Offset: 0x000D18DA
	private void HandleFuelContainerInventoryChanged(List<Item> items)
	{
		this.DrawWidgets();
	}

	// Token: 0x06002C74 RID: 11380 RVA: 0x000D36E4 File Offset: 0x000D18E4
	private bool TryGetFuelContainerStoredItem(out string itemId, out int count)
	{
		itemId = null;
		count = 0;
		WgoData wgoData = this.data;
		if (((wgoData != null) ? wgoData.Definition : null) == null || !this.data.Definition.isFuelContainer)
		{
			return false;
		}
		itemId = this.data.Definition.fuelItemId;
		if (string.IsNullOrEmpty(itemId))
		{
			itemId = this.GetCachedFuelContainerItemId();
		}
		if (string.IsNullOrEmpty(itemId))
		{
			return false;
		}
		Inventory inventory = this.data.Inventory;
		int? num;
		if (inventory == null)
		{
			num = null;
		}
		else
		{
			Item item = inventory.Data;
			num = ((item != null) ? new int?(item.GetTotalCountInInventory(itemId, null, false)) : null);
		}
		int? num2 = num;
		count = num2.GetValueOrDefault();
		return true;
	}

	// Token: 0x06002C75 RID: 11381 RVA: 0x000D3794 File Offset: 0x000D1994
	private string GetCachedFuelContainerItemId()
	{
		if (this.fuelContainerItemIdResolved)
		{
			return this.cachedFuelContainerItemId;
		}
		CraftComponent craftComponent = this.data.CraftComponent;
		if (((craftComponent != null) ? craftComponent.CraftableObject : null) == null)
		{
			return null;
		}
		List<CraftDefBase> list;
		if (!GameBalance.Me.craftsInCache.TryGetValue(this.data.CraftComponent.CraftableObject.CraftableObjectId, out list) || list == null)
		{
			this.fuelContainerItemIdResolved = true;
			return null;
		}
		MainGame instance = MainGame.Instance;
		List<string> list2;
		if (instance == null)
		{
			list2 = null;
		}
		else
		{
			GameSave gameSave = instance.GameSave;
			if (gameSave == null)
			{
				list2 = null;
			}
			else
			{
				KnowledgeSystem knowledgeSystem = gameSave.knowledgeSystem;
				list2 = ((knowledgeSystem != null) ? knowledgeSystem.blackListCrafts : null);
			}
		}
		List<string> list3 = list2;
		foreach (CraftDefBase craftDefBase in list)
		{
			if (craftDefBase != null && craftDefBase.isFuelCraft && craftDefBase.FuelItemDef != null && (list3 == null || !list3.Contains(craftDefBase.id)))
			{
				this.cachedFuelContainerItemId = craftDefBase.FuelItemDef.id;
				break;
			}
		}
		this.fuelContainerItemIdResolved = true;
		return this.cachedFuelContainerItemId;
	}

	// Token: 0x06002C76 RID: 11382 RVA: 0x000D38A8 File Offset: 0x000D1AA8
	private bool ShouldShowConveyorNoPowerIcon()
	{
		WgoData wgoData = this.data;
		if (((wgoData != null) ? wgoData.Definition : null) != null && !(this.data.id != "conveyor_cell") && !this.data.isTempObject)
		{
			WorldZoneData worldZoneData = this.data.WorldZoneData;
			if (!(((worldZoneData != null) ? worldZoneData.id : null) != "conveyor"))
			{
				MainGame instance = MainGame.Instance;
				ConveyorSystem conveyorSystem = ((instance != null) ? instance.conveyorSystem : null);
				return conveyorSystem != null && !conveyorSystem.HasEnoughPower;
			}
		}
		return false;
	}

	// Token: 0x06002C77 RID: 11383 RVA: 0x000D3934 File Offset: 0x000D1B34
	private bool ShouldShowZombieNoStorageIcon()
	{
		ZombieWgoData zombieWgoData = this.data as ZombieWgoData;
		return zombieWgoData != null && zombieWgoData.ShouldShowNoStorageIcon;
	}

	// Token: 0x06002C78 RID: 11384 RVA: 0x000D3958 File Offset: 0x000D1B58
	private bool TryGetPowerSourceGearOutput(out string iconName, out int count)
	{
		iconName = null;
		count = 0;
		WgoData wgoData = this.data;
		if (((wgoData != null) ? wgoData.Definition : null) == null || this.data.Definition.conveyorType != ConveyorElementType.PowerSource)
		{
			return false;
		}
		WorldZoneData worldZoneData = this.data.WorldZoneData;
		string text;
		if (worldZoneData == null)
		{
			text = null;
		}
		else
		{
			WorldZoneDef definition = worldZoneData.Definition;
			text = ((definition != null) ? definition.qualityIcon : null);
		}
		iconName = text;
		if (string.IsNullOrEmpty(iconName))
		{
			iconName = "gear";
		}
		count = (int)this.data.Quality;
		return true;
	}

	// Token: 0x06002C79 RID: 11385 RVA: 0x000D39D8 File Offset: 0x000D1BD8
	private void HandleHiddenChanged(bool newState)
	{
		this.RefreshVisuals();
	}

	// Token: 0x06002C7A RID: 11386 RVA: 0x000D39E0 File Offset: 0x000D1BE0
	private void HandleInteractedStateChanged(bool isInteractable)
	{
		Wgo wgoUnderInteraction = MainGame.PlayerController.PlayerInteractionComponent.WgoUnderInteraction;
		if (wgoUnderInteraction == this && !isInteractable)
		{
			this.interactionHandler.OnInteractionTargetExit();
		}
		this.interactionHandler = this.GetNewInteractionHandler();
		if (wgoUnderInteraction == this && isInteractable)
		{
			this.interactionHandler.OnInteractionTargetEnter();
		}
		this.DrawWidgets();
	}

	// Token: 0x06002C7B RID: 11387 RVA: 0x000D3A3A File Offset: 0x000D1C3A
	private void TestRegisterWorkbenchExtension()
	{
		base.StartCoroutine(this.TryRegisterWorkbenchExtensionDelayed(false));
	}

	// Token: 0x06002C7C RID: 11388 RVA: 0x000D3A4A File Offset: 0x000D1C4A
	private IEnumerator TryRegisterWorkbenchExtensionDelayed(bool isParentWorkbench)
	{
		yield return new WaitForFixedUpdate();
		if (!this)
		{
			yield break;
		}
		HashSet<Wgo> hashSet;
		if (SpecialPhysicsCastUtils.TryGetWgosIntersectedByBuffCollider(this, isParentWorkbench, out hashSet))
		{
			if (isParentWorkbench)
			{
				using (HashSet<Wgo>.Enumerator enumerator = hashSet.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Wgo wgo = enumerator.Current;
						wgo.Data.AddWorkbenchParent(this.Data.UniqueId);
						this.Data.AddWorkbenchExtension(wgo.Data.UniqueId);
					}
					goto IL_0110;
				}
			}
			foreach (Wgo wgo2 in hashSet)
			{
				wgo2.Data.AddWorkbenchExtension(this.Data.UniqueId);
				this.Data.AddWorkbenchParent(wgo2.Data.UniqueId);
			}
			IL_0110:
			ConditionalDrawer[] componentsInChildren = base.gameObject.GetComponentsInChildren<ConditionalDrawer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].SubscribeToParentCraftStatusChanged(this.Data);
			}
		}
		yield break;
	}

	// Token: 0x06002C7D RID: 11389 RVA: 0x000D3A60 File Offset: 0x000D1C60
	public void SetWorldZoneWidgets([CanBeNull] WorldZoneData worldZoneData)
	{
		this.worldZoneWidgetsData.Clear();
		if (worldZoneData != null && !string.IsNullOrEmpty(worldZoneData.id) && worldZoneData.Definition.displayType != WorldZoneDef.DisplayType.None && this.Data.Definition.qualityDisplayType == WGODef.QualityDisplayType.Show)
		{
			this.worldZoneWidgetsData.Add(new UIQualityTooltipWidgetData(worldZoneData.Definition.qualityIcon, this.Data));
			return;
		}
		if (worldZoneData == null)
		{
			this.worldZoneWidgetsData.Clear();
		}
	}

	// Token: 0x06002C7E RID: 11390 RVA: 0x000D3AD8 File Offset: 0x000D1CD8
	public void DrawWidgets()
	{
		if (!this.isVisible)
		{
			return;
		}
		WgoBubbleDisplayHandler.Display(this);
	}

	// Token: 0x06002C7F RID: 11391 RVA: 0x000D3AE9 File Offset: 0x000D1CE9
	public void DrawMultiAnswerReadyWidget(List<Item> items)
	{
		this.DrawMultiAnswerReadyWidget();
	}

	// Token: 0x06002C80 RID: 11392 RVA: 0x000D3AE9 File Offset: 0x000D1CE9
	public void DrawMultiAnswerReadyWidget(int dayNumber)
	{
		this.DrawMultiAnswerReadyWidget();
	}

	// Token: 0x06002C81 RID: 11393 RVA: 0x000D3AF4 File Offset: 0x000D1CF4
	private void SubscribeQuestFinishIndicator()
	{
		if (this.subscribedQuestFinishIndicator || !FinishPhrasesByWgoParser.HasFinishPhrases(this.Id))
		{
			return;
		}
		MainGame.PlayerData.inventory.OnItemsAdd += this.DrawMultiAnswerReadyWidget;
		MainGame.PlayerData.inventory.OnItemsRemove += this.DrawMultiAnswerReadyWidget;
		MainGame.PlayerData.OnGameResChanged += this.DrawMultiAnswerReadyWidget;
		EnvironmentEngine.OnNewDayStarted += this.DrawMultiAnswerReadyWidget;
		this.subscribedQuestFinishIndicator = true;
	}

	// Token: 0x06002C82 RID: 11394 RVA: 0x000D3B7C File Offset: 0x000D1D7C
	private void UnsubscribeQuestFinishIndicator()
	{
		if (!this.subscribedQuestFinishIndicator)
		{
			return;
		}
		PlayerData playerData = MainGame.PlayerData;
		if (playerData != null)
		{
			playerData.inventory.OnItemsAdd -= this.DrawMultiAnswerReadyWidget;
			playerData.inventory.OnItemsRemove -= this.DrawMultiAnswerReadyWidget;
			playerData.OnGameResChanged -= this.DrawMultiAnswerReadyWidget;
		}
		EnvironmentEngine.OnNewDayStarted -= this.DrawMultiAnswerReadyWidget;
		this.subscribedQuestFinishIndicator = false;
		this.lastReadyToFinishQuest = false;
	}

	// Token: 0x06002C83 RID: 11395 RVA: 0x000D3BFC File Offset: 0x000D1DFC
	public void DrawMultiAnswerReadyWidget()
	{
		if (!this.isVisible)
		{
			return;
		}
		bool flag = MainGame.Instance.GameSave.questSystemData.WgoHasReadyToFinishQuest(this.Id);
		if (flag == this.lastReadyToFinishQuest)
		{
			return;
		}
		this.lastReadyToFinishQuest = flag;
		WgoBubbleDisplayHandler.Display(this);
	}

	// Token: 0x06002C84 RID: 11396 RVA: 0x000D3C44 File Offset: 0x000D1E44
	public void TryDrawNpcWidget()
	{
		if (!string.IsNullOrEmpty(this.data.Definition.repResName))
		{
			UINpcWidgetData uinpcWidgetData = new UINpcWidgetData();
			uinpcWidgetData.NpcId = this.data.id;
			GUIElements.Instance.NpcWidget.Draw(uinpcWidgetData);
		}
	}

	// Token: 0x06002C85 RID: 11397 RVA: 0x000D3C90 File Offset: 0x000D1E90
	public void HideWorldZoneWidgets()
	{
		foreach (LazyWidgetDataBase lazyWidgetDataBase in this.worldZoneWidgetsData)
		{
			WgoBubbleDisplayHandler.HideWidget(this, lazyWidgetDataBase);
		}
	}

	// Token: 0x06002C86 RID: 11398 RVA: 0x000D3CE4 File Offset: 0x000D1EE4
	public bool IsBuildRemovable()
	{
		BuildingDef buildingDef;
		GameBalance.Me.buildableWgos.TryGetValue(this.WGOId, out buildingDef);
		BuildingDef buildingDef2;
		GameBalance.Me.removableWgos.TryGetValue(this.WGOId, out buildingDef2);
		if (this.data.GetGameResInt("conveyor_build_is_not_removable") > 0)
		{
			return false;
		}
		if (this.data.GetGameResInt("lock_building_removal") > 0)
		{
			return false;
		}
		if (buildingDef != null || buildingDef2 != null)
		{
			Rect rect;
			if (buildingDef2 != null && buildingDef != null && buildingDef2.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.Strict && !string.IsNullOrEmpty(buildingDef.customBuildAreaId) && this.TryGetCustomBuildAreaRect(out rect))
			{
				WorldZoneData worldZoneData;
				if ((worldZoneData = this.data.WorldZoneData) == null)
				{
					WorldZone worldZone = LazySingleton<BuildManager>.Instance.WorldZone;
					worldZoneData = ((worldZone != null) ? worldZone.Data : null);
				}
				WorldZoneData worldZoneData2 = worldZoneData;
				if (worldZoneData2 != null)
				{
					using (List<WgoData>.Enumerator enumerator = worldZoneData2.GetWgoDataByRect(rect).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (Wgo.IsOccupyingWgoOnBuildArea(enumerator.Current, this.data.UniqueId, buildingDef, true))
							{
								return false;
							}
						}
					}
				}
			}
			return (buildingDef != null && buildingDef.deleteInstantly) || buildingDef2 != null;
		}
		return false;
	}

	// Token: 0x06002C87 RID: 11399 RVA: 0x000D3E1C File Offset: 0x000D201C
	public static bool IsOccupyingWgoOnBuildArea(WgoData occupant, SGuid ownerUniqueId, BuildingDef placeDef, bool skipFullCoverSoftExtensions)
	{
		return occupant != null && !(occupant.UniqueId == ownerUniqueId) && (placeDef == null || occupant.Definition == null || !placeDef.ShouldIgnoreWgoGroupAsObstacle(occupant.Definition.wgoGroup)) && (!skipFullCoverSoftExtensions || !Wgo.IsFullCoverSoftSlotExtension(occupant));
	}

	// Token: 0x06002C88 RID: 11400 RVA: 0x000D3E6C File Offset: 0x000D206C
	private static bool IsFullCoverSoftSlotExtension(WgoData wgoData)
	{
		BuildingDef buildingDef;
		return GameBalance.Me.buildableWgos.TryGetValue(wgoData.id, out buildingDef) && buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft;
	}

	// Token: 0x06002C89 RID: 11401 RVA: 0x000D3EA0 File Offset: 0x000D20A0
	private bool TryGetCustomBuildAreaRect(out Rect buildAreaRect)
	{
		buildAreaRect = Rect.zero;
		foreach (Collider collider in base.GetComponentsInChildren<Collider>(true))
		{
			BuildArea buildArea;
			if (!(collider == null) && collider.gameObject.layer == 19 && collider.TryGetComponent<BuildArea>(out buildArea) && !string.IsNullOrEmpty(buildArea.Id) && !buildArea.fullCoveringMode)
			{
				Bounds bounds = collider.bounds;
				buildAreaRect = new Rect(bounds.min.x, bounds.min.z, bounds.size.x, bounds.size.z);
				return true;
			}
		}
		return false;
	}

	// Token: 0x06002C8A RID: 11402 RVA: 0x000D3F58 File Offset: 0x000D2158
	public bool DoBuildRemove()
	{
		BuildingDef buildingDef;
		GameBalance.Me.buildableWgos.TryGetValue(this.WGOId, out buildingDef);
		BuildingDef buildingDef2;
		GameBalance.Me.removableWgos.TryGetValue(this.WGOId, out buildingDef2);
		if (buildingDef != null || buildingDef2 != null)
		{
			if (buildingDef != null && buildingDef.deleteInstantly)
			{
				BuildingDef.BuildingMode buildingMode = buildingDef.buildingMode;
				if (buildingMode != BuildingDef.BuildingMode.FightingPlace)
				{
					if (buildingMode == BuildingDef.BuildingMode.FightBuilding)
					{
						MainGame.Instance.GameSave.militaryBaseData.RemoveBaseBuilding(this.data);
					}
					List<ItemCount> list = new List<ItemCount>();
					foreach (NeedItemData needItemData in buildingDef.needItems)
					{
						list.Add(new ItemCount(needItemData.id, needItemData.GetCount(null)));
					}
					this.DropItemsFromBuild(list);
					if (this.data.Inventory.Data != null)
					{
						this.DropItemsFromInventory();
					}
					MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this.data, true);
					return true;
				}
				MainGame.Instance.GameSave.militaryBaseData.RemoveFightBuilding(this.data);
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this.data, true);
				return true;
			}
			else if (buildingDef2 != null)
			{
				if (buildingDef2.deleteInstantly)
				{
					if (buildingDef != null)
					{
						BuildingDef.BuildingMode buildingMode = buildingDef.buildingMode;
						if (buildingMode == BuildingDef.BuildingMode.FightingPlace)
						{
							MainGame.Instance.GameSave.militaryBaseData.RemoveFightBuilding(this.data);
							MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this.data, true);
							return true;
						}
						if (buildingMode == BuildingDef.BuildingMode.FightBuilding)
						{
							MainGame.Instance.GameSave.militaryBaseData.RemoveBaseBuilding(this.data);
						}
					}
					this.DropItemsFromBuild(buildingDef2.outputItems.MakePreOutput(this.data, 0f));
					MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this.data, true);
					if (this.data.Inventory.Data != null)
					{
						this.DropItemsFromInventory();
					}
					return true;
				}
				CraftComponent craftComponent = this.data.CraftComponent;
				if (craftComponent.CurrentCraftElement != null && craftComponent.CurrentCraftElement.CraftId == buildingDef2.startCraft)
				{
					craftComponent.RemoveDestroyCraft();
					this.interactionHandler = this.GetNewInteractionHandler();
					if (craftComponent.CraftElementsQueue.Count == 0)
					{
						WgoBubbleDisplayHandler.HideWidget<UICraftHintWidgetData>(this);
					}
					return false;
				}
				CraftElement craftElement = new CraftElement(buildingDef2.startCraft, 1, new CraftParamsData(buildingDef2.startCraft, this.data, CraftParamsData.CraftParamsType.Common, -1));
				List<Item> list2 = OutputItems.MakeOutput(buildingDef2.outputItems.MakePreOutput(this.data, 0f));
				CraftDefBase craftDefBase = null;
				foreach (CraftDefBase craftDefBase2 in this.data.CraftComponent.AvailableCrafts)
				{
					if (craftDefBase2.isFuelCraft)
					{
						craftDefBase = craftDefBase2;
						break;
					}
				}
				if (this.data.Inventory.Data != null)
				{
					foreach (Item item in this.data.Inventory.Data.Inventory)
					{
						if (!item.IsEmpty && (craftDefBase == null || !item.Definition.isFuel))
						{
							list2.Add(item);
						}
					}
				}
				if (craftDefBase != null)
				{
					NeedItemData needItemData2 = ((craftDefBase.needItems.Count > 0) ? craftDefBase.needItems[0] : null);
					List<ItemCount> list3 = craftDefBase.addItemsToWgoOnFinish.MakePreOutput(this.data, 0f);
					int num = ((list3.Count > 0) ? list3[0].count : 0);
					if (needItemData2 != null && num > 0 && this.data.Inventory.Data != null)
					{
						foreach (Item item2 in this.data.Inventory.Data.Inventory)
						{
							if (!item2.IsEmpty && item2.Definition.isFuel)
							{
								list2.Add(new Item(needItemData2.id, Mathf.Clamp(item2.Count / num * needItemData2.GetCount(null), 1, 999)));
							}
						}
					}
				}
				craftElement.SetCustomOutputItems(list2);
				this.data.CraftComponent.AddDestroyCraft(craftElement);
				this.interactionHandler = this.GetNewInteractionHandler();
				return false;
			}
		}
		return false;
	}

	// Token: 0x06002C8B RID: 11403 RVA: 0x000D4428 File Offset: 0x000D2628
	private void DropItemsFromInventory()
	{
		foreach (Item item in this.data.Inventory.Data.Inventory)
		{
			this.data.MakeDrop(item);
		}
	}

	// Token: 0x06002C8C RID: 11404 RVA: 0x000D4490 File Offset: 0x000D2690
	private void DropItemsFromBuild(List<ItemCount> itemCounts)
	{
		foreach (Item item in OutputItems.MakeOutput(itemCounts))
		{
			this.data.MakeDrop(item);
		}
	}

	// Token: 0x06002C8D RID: 11405 RVA: 0x000D44E8 File Offset: 0x000D26E8
	public void OnConveyorObjectRemoved()
	{
		ConveyorWgoData conveyorWgoData = this.Data as ConveyorWgoData;
		if (conveyorWgoData != null)
		{
			conveyorWgoData.ConveyorComponent.DisconnectChilds();
			BoxCollider[] componentsInChildren = base.GetComponentsInChildren<BoxCollider>();
			BoxCollider boxCollider = null;
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].gameObject.layer == 19)
				{
					boxCollider = componentsInChildren[i];
					break;
				}
			}
			if (boxCollider == null)
			{
				return;
			}
			Collider[] array = new Collider[20];
			Vector3 vector = boxCollider.bounds.extents / 2f;
			vector.y = 0.2f;
			if (Physics.OverlapBoxNonAlloc(boxCollider.bounds.center, vector, array, Quaternion.identity, 134217728) > 0)
			{
				for (int j = 0; j < array.Length; j++)
				{
					if (array[j] != null)
					{
						BuildConnector component = array[j].GetComponent<BuildConnector>();
						if (component != null)
						{
							component.TryDisconnect(conveyorWgoData);
						}
					}
				}
			}
			for (int k = conveyorWgoData.HardConnectedWGOs.Count - 1; k >= 0; k--)
			{
				MainGame.WorldData.RemoveWgoDataFromGameScene(conveyorWgoData.HardConnectedWGOs[k]);
			}
			MainGame.Instance.conveyorSystem.RemoveConveyorObject(conveyorWgoData.ConveyorComponent);
		}
	}

	// Token: 0x170006F0 RID: 1776
	// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000D461F File Offset: 0x000D281F
	public SGuid BubbleDrawableUniqueId
	{
		get
		{
			return this.data.UniqueId;
		}
	}

	// Token: 0x170006F1 RID: 1777
	// (get) Token: 0x06002C8F RID: 11407 RVA: 0x000D462C File Offset: 0x000D282C
	public List<LazyWidgetDataBase> BubbleDrawableWidgets
	{
		get
		{
			return this.GetWidgetData();
		}
	}

	// Token: 0x170006F2 RID: 1778
	// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000D4634 File Offset: 0x000D2834
	public Vector3 BubbleDrawablePosition
	{
		get
		{
			WorkbenchAdditionWorldIconPresenter.State state;
			if (this.data != null && WorkbenchAdditionWorldIconPresenter.TryGet(this.data.UniqueId, out state) && state.UsePlotWorldY)
			{
				Vector3 position = this.data.Position;
				position.y += state.PlotWorldYOffset;
				return position;
			}
			return this.GetBubblePointPos();
		}
	}

	// Token: 0x06002C91 RID: 11409 RVA: 0x000D468C File Offset: 0x000D288C
	public BurstableBounds GetChunkableData()
	{
		if (!this.boundsCalculated)
		{
			if (this.HasUsableSerializedChunkBounds())
			{
				ChunkBoundsPair serializedBounds = this.data.SerializedBounds;
				Vector3 position = base.transform.position;
				this.bounds = new ChunkBoundsPair(new Bounds(serializedBounds.withShadows.center + position, serializedBounds.withShadows.size), new Bounds(serializedBounds.withoutShadows.center + position, serializedBounds.withoutShadows.size));
			}
			else if (this.wgoPartsLoaded)
			{
				this.bounds = ChunkSizeCalculator.CalculateChunkBounds(base.gameObject);
			}
			else
			{
				if (this.data != null && !this.data.HasSerializedBounds)
				{
					Debug.LogError("[Wgo] No baked bounds for [" + this.data.id + "]. Run 'GK2/Bake WgoPartBakedData prefabs for all prefabs'. Using fallback bounds.");
				}
				Vector3 position2 = base.transform.position;
				Vector3 vector = new Vector3(2f, 2f, 2f);
				this.bounds = new ChunkBoundsPair(new Bounds(position2, vector), new Bounds(position2, vector));
			}
			this.initialBoundsPosition = base.transform.position;
			this.boundsCalculated = true;
		}
		Vector3 vector2 = base.transform.position - this.initialBoundsPosition;
		return new BurstableBounds(this.bounds.GetBounds().center + vector2, this.bounds.GetBounds().size);
	}

	// Token: 0x06002C92 RID: 11410 RVA: 0x000D4814 File Offset: 0x000D2A14
	private bool HasUsableSerializedChunkBounds()
	{
		if (this.data == null || !this.data.HasSerializedBounds)
		{
			return false;
		}
		ChunkBoundsPair serializedBounds = this.data.SerializedBounds;
		return serializedBounds.withShadows.size.sqrMagnitude > 0.0001f || serializedBounds.withoutShadows.size.sqrMagnitude > 0.0001f;
	}

	// Token: 0x170006F3 RID: 1779
	// (get) Token: 0x06002C93 RID: 11411 RVA: 0x000D487C File Offset: 0x000D2A7C
	// (set) Token: 0x06002C94 RID: 11412 RVA: 0x000D4884 File Offset: 0x000D2A84
	public MultiFlagOR<ChunkingIgnoreType> IgnoreMultiFlag { get; set; }

	// Token: 0x06002C95 RID: 11413 RVA: 0x000D4890 File Offset: 0x000D2A90
	public void UpdateChunkVisibility(bool isVisible)
	{
		if (!this)
		{
			return;
		}
		bool flag = this.isVisible != isVisible;
		if (flag)
		{
			this.isVisible = isVisible;
		}
		if (flag || (isVisible && this.spawnCompleted && !base.gameObject.activeSelf && this.data != null && !this.data.IsHidden))
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x06002C96 RID: 11414 RVA: 0x000D48F1 File Offset: 0x000D2AF1
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
		if (!this.isVisible && state != ChunkVisibilityState.Visible)
		{
			this.RefreshVisuals();
		}
	}

	// Token: 0x06002C97 RID: 11415 RVA: 0x000D4920 File Offset: 0x000D2B20
	private void TryRegisterInChunkManager()
	{
		if (Application.isPlaying && !this.registeredInChunker)
		{
			WGODef definition = this.data.Definition;
			if (definition != null && definition.isMovable)
			{
				LazySingleton<ChunkManager>.Instance.RegisterDynamicChunkableObject(this, ChunkManagerLayerType.DynamicWgo);
			}
			else
			{
				LazySingleton<ChunkManager>.Instance.RegisterStaticChunkableObject(this, ChunkManagerLayerType.StaticWgo);
			}
			this.registeredInChunker = true;
		}
	}

	// Token: 0x06002C98 RID: 11416 RVA: 0x000D4978 File Offset: 0x000D2B78
	private void TryUnregisterInChunkManager()
	{
		if (Application.isPlaying && this.registeredInChunker)
		{
			WGODef definition = this.data.Definition;
			if (definition != null && definition.isMovable)
			{
				LazySingleton<ChunkManager>.Instance.UnregisterDynamicChunkableObject(this, ChunkManagerLayerType.DynamicWgo);
			}
			else
			{
				LazySingleton<ChunkManager>.Instance.UnregisterStaticChunkableObject(this, ChunkManagerLayerType.StaticWgo);
			}
			this.registeredInChunker = false;
		}
	}

	// Token: 0x06002C99 RID: 11417 RVA: 0x000D49D0 File Offset: 0x000D2BD0
	private void PrecomputeSerializedBounds()
	{
		if (this.data.HasSerializedBounds)
		{
			return;
		}
		string mainWgoPartAssetId = this.GetMainWgoPartAssetId();
		WgoPartBakedData wgoPartBakedData = LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.Get(mainWgoPartAssetId);
		if (wgoPartBakedData != null && wgoPartBakedData.HasChunkBounds)
		{
			Bounds withShadows = wgoPartBakedData.ChunkBounds.withShadows;
			Bounds withoutShadows = wgoPartBakedData.ChunkBounds.withoutShadows;
			foreach (WgoPartData wgoPartData in this.data.AdditionalWgoPartsData)
			{
				WgoPartBakedData wgoPartBakedData2 = LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.Get(wgoPartData.id);
				if (wgoPartBakedData2 != null && wgoPartBakedData2.HasChunkBounds)
				{
					withShadows.Encapsulate(wgoPartBakedData2.ChunkBounds.withShadows);
					withoutShadows.Encapsulate(wgoPartBakedData2.ChunkBounds.withoutShadows);
				}
			}
			this.data.SetSerializedBounds(new ChunkBoundsPair(withShadows, withoutShadows));
		}
	}

	// Token: 0x06002C9A RID: 11418 RVA: 0x000D4AC8 File Offset: 0x000D2CC8
	public string GetMainWgoPartAssetId()
	{
		if (this.data.Definition == null)
		{
			return this.data.id;
		}
		return this.data.Definition.ResolveAssetId(this.data.id, this.data);
	}

	// Token: 0x06002C9B RID: 11419 RVA: 0x000D4B04 File Offset: 0x000D2D04
	public void UpdateViewAsset()
	{
		WgoData wgoData = this.data;
		if (((wgoData != null) ? wgoData.Definition : null) == null)
		{
			return;
		}
		string mainWgoPartAssetId = this.GetMainWgoPartAssetId();
		WgoPartData mainWgoPartData = this.data.MainWgoPartData;
		string text = ((mainWgoPartData != null) ? mainWgoPartData.id : null);
		if (mainWgoPartAssetId == text)
		{
			return;
		}
		WgoPartData mainWgoPartData2 = this.data.MainWgoPartData;
		int num = ((mainWgoPartData2 != null) ? mainWgoPartData2.rotationIndex : (-1));
		this.data.ReCreateMainWgoPartData(num);
		if (this.wgoPartsLoaded)
		{
			this.SwapMainWgoPart();
			return;
		}
		if (this.wgoPartsLoading)
		{
			LazySingleton<WgoPartLoadManager>.Instance.CancelLoad(this);
			this.wgoPartsLoading = false;
			this.RequestWgoPartsLoad(this.pendingApplyDefaultState, false);
			this.pendingApplyDefaultState = false;
		}
	}

	// Token: 0x06002C9C RID: 11420 RVA: 0x000D4BB0 File Offset: 0x000D2DB0
	private void SwapMainWgoPart()
	{
		WgoBubbleDisplayHandler.Hide(this);
		this.DeInitVisualBindings();
		if (this.MainWgoPart != null)
		{
			if (!string.IsNullOrEmpty(this.MainWgoPart.PooledAddressableKey) && !this.Data.isTempObject)
			{
				FightingAgent fightingAgent;
				if (this.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
				{
					fightingAgent.DeInitForPool();
				}
				this.ReleaseWgoPart(this.MainWgoPart);
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.MainWgoPart.gameObject);
			}
			this.MainWgoPart = null;
		}
		string addressableKey = WgoPartPool.GetAddressableKey(this.data.MainWgoPartData.id);
		this.MainWgoPart = LazySingleton<WgoPartPool>.Instance.GetSync(addressableKey, this);
		if (this.MainWgoPart == null)
		{
			this.wgoPartsLoaded = false;
			this.ReleaseAdditionalWgoParts();
			this.HandleVisualPartsLoadFailed();
			return;
		}
		this.MainWgoPart.InitVisuals(null, this.data.MainWgoPartData, this.data.Definition);
		bool flag = this.isVisible && !this.data.IsHidden;
		if (flag)
		{
			base.gameObject.SetActive(true);
			this.InitVisualBindings();
		}
		this.UpdateWgoPartState(false);
		this.TryBindAnimationComponent();
		this.DoCheckDelayedAnimation();
		this.ApplyZombieAnimationStateAfterWgoPartState();
		this.ValidateSpawnerComponent();
		this.data.ClearSerializedBounds();
		this.boundsCalculated = false;
		this.PrecomputeSerializedBounds();
		this.data.RewriteGdPointsData(new List<GDPointData>());
		this.RegisterGDPointsFromBakedData();
		if (flag)
		{
			this.DrawWidgets();
			this.UpdateDropPoint();
		}
	}

	// Token: 0x06002C9D RID: 11421 RVA: 0x000D4D28 File Offset: 0x000D2F28
	private void RegisterGDPointsFromBakedData()
	{
		string mainWgoPartAssetId = this.GetMainWgoPartAssetId();
		List<WgoPartBakedData.GDPointBakedData> list = new List<WgoPartBakedData.GDPointBakedData>();
		Wgo.CollectGDPointsForVariation(mainWgoPartAssetId, this.data.MainWgoPartData, list);
		foreach (WgoPartData wgoPartData in this.data.AdditionalWgoPartsData)
		{
			Wgo.CollectGDPointsForVariation(wgoPartData.id, wgoPartData, list);
		}
		if (list.Count == 0)
		{
			GardenBedNavigation.TryRebuild(this.data);
			return;
		}
		GameSceneData gameSceneDataById = MainGame.Instance.GameSave.worldData.GetGameSceneDataById(this.data.WorldId);
		Vector3 vector = ((gameSceneDataById != null) ? gameSceneDataById.offset : Vector3.zero);
		Vector3 position = this.data.Position;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		int num = -1073741824 + this.data.UniqueId.GetHashCode();
		for (int i = 0; i < list.Count; i++)
		{
			dictionary[list[i].id] = num + i;
		}
		List<GDPointData> list2 = new List<GDPointData>();
		foreach (WgoPartBakedData.GDPointBakedData gdpointBakedData in list)
		{
			int num2 = dictionary[gdpointBakedData.id];
			GDPointData gdpointData = new GDPointData(gdpointBakedData, this.data.WorldId, position, vector, num2);
			List<int> list3 = new List<int>();
			foreach (string text in gdpointBakedData.nextGdPointIds)
			{
				int num3;
				if (dictionary.TryGetValue(text, out num3))
				{
					list3.Add(num3);
				}
			}
			gdpointData.SetNextNodeInstanceIds(list3);
			list2.Add(gdpointData);
		}
		this.data.RewriteGdPointsData(list2);
		GardenBedNavigation.TryRebuild(this.data);
	}

	// Token: 0x06002C9E RID: 11422 RVA: 0x000D4F34 File Offset: 0x000D3134
	private static void CollectGDPointsForVariation(string assetId, WgoPartData partData, List<WgoPartBakedData.GDPointBakedData> target)
	{
		WgoPartBakedData wgoPartBakedData = LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.Get(assetId);
		if (wgoPartBakedData == null)
		{
			return;
		}
		int stateHash = WgoPartData.GetStateHash(partData.variationId, partData.rotationIndex);
		List<WgoPartBakedData.GDPointBakedData> list;
		if (wgoPartBakedData.VariationGDPointsDict.TryGetValue(stateHash, out list))
		{
			target.AddRange(list);
		}
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000D4F7C File Offset: 0x000D317C
	public void BindGDPointViews()
	{
		if (this.data.gdPointsData.Count == 0)
		{
			return;
		}
		foreach (GDPoint gdpoint in base.GetComponentsInChildren<GDPoint>(true))
		{
			GDPointData gdpointData = this.data.GetGDPointData(gdpoint.Id);
			if (gdpointData != null)
			{
				gdpoint.Init(gdpointData);
			}
		}
	}

	// Token: 0x170006F4 RID: 1780
	// (get) Token: 0x06002CA0 RID: 11424 RVA: 0x000D4FD2 File Offset: 0x000D31D2
	public SGuid CombatEntityUID
	{
		get
		{
			return this.Data.UniqueId;
		}
	}

	// Token: 0x170006F5 RID: 1781
	// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x000D4FDF File Offset: 0x000D31DF
	public LazyConsts.Fighting.TeamType TeamType
	{
		get
		{
			if (!this.Data.id.Contains("zmb") || this.Data.id.Contains("allie"))
			{
				return LazyConsts.Fighting.TeamType.Player;
			}
			return LazyConsts.Fighting.TeamType.WildZombie;
		}
	}

	// Token: 0x170006F6 RID: 1782
	// (get) Token: 0x06002CA2 RID: 11426 RVA: 0x000D5012 File Offset: 0x000D3212
	public LazyConsts.Fighting.EntityType EntityType
	{
		get
		{
			if (this.Data.id.Contains("npc"))
			{
				return LazyConsts.Fighting.EntityType.Soldier;
			}
			if (!this.Data.id.Contains("zmb"))
			{
				return LazyConsts.Fighting.EntityType.None;
			}
			return LazyConsts.Fighting.EntityType.Zombie;
		}
	}

	// Token: 0x170006F7 RID: 1783
	// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000D5047 File Offset: 0x000D3247
	public Vector3 CombatEntityPosition
	{
		get
		{
			return this.Data.Position;
		}
	}

	// Token: 0x170006F8 RID: 1784
	// (get) Token: 0x06002CA4 RID: 11428 RVA: 0x000D5054 File Offset: 0x000D3254
	// (set) Token: 0x06002CA5 RID: 11429 RVA: 0x000D505C File Offset: 0x000D325C
	public bool IsActiveCombatant { get; set; }

	// Token: 0x06002CA6 RID: 11430 RVA: 0x000D5068 File Offset: 0x000D3268
	public float GetCombatEntityDistance(Vector3 from, LazyConsts.Fighting.TeamType teamType)
	{
		DockPointData dockPointData = null;
		Vector3 zero = Vector3.zero;
		if (teamType != LazyConsts.Fighting.TeamType.Player)
		{
			if (teamType == LazyConsts.Fighting.TeamType.WildZombie)
			{
				dockPointData = this.data.MainWgoPartData.GetNearestDockPoint(this.data, from, out zero, DockPointData.Availability.All, DockPointData.Filter.OnlyZombie, null);
			}
		}
		else
		{
			dockPointData = this.data.MainWgoPartData.GetNearestDockPoint(this.data, from, out zero, DockPointData.Availability.All, DockPointData.Filter.OnlyNotZombie, null);
		}
		if (dockPointData != null)
		{
			return (from - zero).magnitude;
		}
		return (from - this.data.Position).magnitude;
	}

	// Token: 0x170006F9 RID: 1785
	// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x000D50EE File Offset: 0x000D32EE
	public HPComponent CombatEntityHpComponent
	{
		get
		{
			return this.Data.HpComponent;
		}
	}

	// Token: 0x170006FA RID: 1786
	// (get) Token: 0x06002CA8 RID: 11432 RVA: 0x000D50FB File Offset: 0x000D32FB
	public int CombatEntityQuality
	{
		get
		{
			return (int)this.data.Definition.quality.EvaluateFloat(this.data) + this.data.GetGameResInt("excitement");
		}
	}

	// Token: 0x170006FB RID: 1787
	// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x000D512A File Offset: 0x000D332A
	// (set) Token: 0x06002CAA RID: 11434 RVA: 0x000D5132 File Offset: 0x000D3332
	public int AttackPriority { get; set; }

	// Token: 0x170006FC RID: 1788
	// (get) Token: 0x06002CAB RID: 11435 RVA: 0x000D513B File Offset: 0x000D333B
	public bool HasAnyDockPoint
	{
		get
		{
			return this.Data.MainWgoPartData.DockPointsCount > 0;
		}
	}

	// Token: 0x170006FD RID: 1789
	// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000D5150 File Offset: 0x000D3350
	public int ArmorValue
	{
		get
		{
			if (this.attackComponent == null)
			{
				this.attackComponent = base.GetComponentInChildren<AttackComponent>();
			}
			if (this.attackComponent == null)
			{
				return 0;
			}
			return this.attackComponent.Armor;
		}
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x000D5187 File Offset: 0x000D3387
	public float GetCombatEntityGameRes(string resId)
	{
		if (this.Data == null)
		{
			return 0f;
		}
		return this.Data.GetGameRes(resId);
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x000D51A4 File Offset: 0x000D33A4
	public void OnOtherCombatTargetReachedToMe(ICombatEntity other)
	{
		WgoPartData mainWgoPartData = this.Data.MainWgoPartData;
		DockPointData dockPointData = ((mainWgoPartData != null) ? mainWgoPartData.GetNearestDockPoint(this.data, other.CombatEntityPosition, DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.All, null) : null);
		if (dockPointData == null)
		{
			return;
		}
		Wgo wgo = other as Wgo;
		if (wgo != null)
		{
			if (!SGuid.IsNullOrEmpty(wgo.Data.takenDockPointsParentSGuid))
			{
				if (wgo.Data.takenDockPointsParentSGuid == this.Data.UniqueId)
				{
					return;
				}
				MainGame instance = MainGame.Instance;
				WgoData wgoData;
				if (instance == null)
				{
					wgoData = null;
				}
				else
				{
					GameSave gameSave = instance.GameSave;
					if (gameSave == null)
					{
						wgoData = null;
					}
					else
					{
						WorldData worldData = gameSave.WorldData;
						wgoData = ((worldData != null) ? worldData.GetWgoData(wgo.Data.takenDockPointsParentSGuid) : null);
					}
				}
				WgoData wgoData2 = wgoData;
				if (wgoData2 != null)
				{
					WgoPartData mainWgoPartData2 = wgoData2.MainWgoPartData;
					if (mainWgoPartData2 != null)
					{
						mainWgoPartData2.TryFreeDockPoint(wgoData2.UniqueId, wgo.Data.UniqueId);
					}
				}
				wgo.Data.takenDockPointsParentSGuid = SGuid.Empty;
			}
			wgo.Data.takenDockPointsParentSGuid = this.Data.UniqueId;
		}
		dockPointData.Occupy(other.CombatEntityUID);
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x000D52A8 File Offset: 0x000D34A8
	public void OnOtherCombatTargetHitMe(AttackContext ctx)
	{
		FightingAgent fightingAgent;
		if (!this.MainWgoPart || !this.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
		{
			return;
		}
		if (fightingAgent.AttackComponent && fightingAgent.AttackComponent.teamType == LazyConsts.Fighting.TeamType.WildZombie && this.Data.HpComponent.Hp <= 0)
		{
			foreach (LazyExpression lazyExpression in fightingAgent.FighterDef.expressionsOnCombatDeath)
			{
				lazyExpression.EvaluateWithAttackerAttacksMe(ctx.attacker);
			}
		}
		fightingAgent.ProvideReturnDamageLogic(ctx);
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x000D5358 File Offset: 0x000D3558
	public void ApplyKnockback(AttackContext attackContext, float duration)
	{
		FightingAgent fightingAgent;
		if (!this.MainWgoPart || !this.MainWgoPart.TryGetComponent<FightingAgent>(out fightingAgent))
		{
			return;
		}
		if (duration.More(0f, 1E-05f))
		{
			Debug.Log(string.Format("Apply knockback to {0}, direction: {1:F4}, duration: {2}", this.CombatEntityUID, attackContext.direction, duration));
			fightingAgent.DoKnockback(attackContext.direction, duration);
		}
		WgoPart mainWgoPart = this.MainWgoPart;
		AnimationComponentBase animationComponentBase = ((mainWgoPart != null) ? mainWgoPart.AnimationComponent : null);
		if (animationComponentBase && attackContext.Damage == 0 && animationComponentBase.AnimationState != global::AnimationState.AttackBlock)
		{
			if (fightingAgent.IsExecutingCommand)
			{
				MobCommandGoTo mobCommandGoTo = fightingAgent.MobCommand as MobCommandGoTo;
				if (mobCommandGoTo != null)
				{
					mobCommandGoTo.SetCustomAnimationState(global::AnimationState.AttackBlock);
					return;
				}
			}
			if (!fightingAgent.IsExecutingCommand)
			{
				WgoPart mainWgoPart2 = this.MainWgoPart;
				if (mainWgoPart2 == null)
				{
					return;
				}
				mainWgoPart2.AnimationComponent.SetTrigger(AnimationComponentBase.ATTACK_BLOCK_TRIGGER);
			}
		}
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x000D5488 File Offset: 0x000D3688
	[CompilerGenerated]
	internal static BoxCollider <GetFxOnDieParams>g__FindBuildArea|99_0(GameObject parent, int layer)
	{
		foreach (object obj in parent.transform)
		{
			Transform transform = (Transform)obj;
			if (transform.gameObject.layer == layer)
			{
				return transform.gameObject.GetComponent<BoxCollider>();
			}
			BoxCollider boxCollider = Wgo.<GetFxOnDieParams>g__FindBuildArea|99_0(transform.gameObject, layer);
			if (boxCollider != null)
			{
				return boxCollider;
			}
		}
		return null;
	}

	// Token: 0x040023A1 RID: 9121
	private const string DefaultTalkText = "Lorem ipsum dolor sit amet";

	// Token: 0x040023A2 RID: 9122
	private string editorTalkText = "Lorem ipsum dolor sit amet";

	// Token: 0x040023A3 RID: 9123
	[SerializeField]
	protected WgoData data;

	// Token: 0x040023A4 RID: 9124
	[SerializeField]
	[HideInInspector]
	public List<WgoPart> additionalWgoParts = new List<WgoPart>();

	// Token: 0x040023A5 RID: 9125
	[NonSerialized]
	public DropPoint dropPoint;

	// Token: 0x040023A6 RID: 9126
	[NonSerialized]
	public InteractingItemPoint interactionItemPoint;

	// Token: 0x040023A7 RID: 9127
	private ChunkBoundsPair bounds;

	// Token: 0x040023A8 RID: 9128
	private bool boundsCalculated;

	// Token: 0x040023A9 RID: 9129
	private bool isVisible;

	// Token: 0x040023AA RID: 9130
	private bool lastReadyToFinishQuest;

	// Token: 0x040023AB RID: 9131
	private bool subscribedQuestFinishIndicator;

	// Token: 0x040023AC RID: 9132
	private bool spawnCompleted;

	// Token: 0x040023AD RID: 9133
	[SerializeField]
	[HideInInspector]
	private Vector3 initialBoundsPosition;

	// Token: 0x040023AE RID: 9134
	private bool registeredInChunker;

	// Token: 0x040023AF RID: 9135
	public StartReses startReses;

	// Token: 0x040023B0 RID: 9136
	public Direction startDirection = Direction.Down;

	// Token: 0x040023B1 RID: 9137
	private WgoMovementAdjustComponent wgoMovementAdjustComponent;

	// Token: 0x040023B2 RID: 9138
	private bool hasData;

	// Token: 0x040023B3 RID: 9139
	private bool isDespawning;

	// Token: 0x040023B4 RID: 9140
	private bool hasCustomDestroyMoment;

	// Token: 0x040023B5 RID: 9141
	private bool isPlayingDestroyAnim;

	// Token: 0x040023B6 RID: 9142
	private AnimationComponentBase animationComponent;

	// Token: 0x040023B7 RID: 9143
	private bool wgoPartsLoaded;

	// Token: 0x040023B8 RID: 9144
	private bool wgoPartsLoading;

	// Token: 0x040023B9 RID: 9145
	private bool visualBindingsInitialized;

	// Token: 0x040023BA RID: 9146
	private bool animationBindingsBound;

	// Token: 0x040023BB RID: 9147
	private bool pendingApplyDefaultState;

	// Token: 0x040023BC RID: 9148
	private bool needsWorkbenchExtensionInit;

	// Token: 0x040023BD RID: 9149
	private bool pendingRefreshAfterCraftHintCompletion;

	// Token: 0x040023BE RID: 9150
	private bool pendingDespawnAfterCraftHintCompletion;

	// Token: 0x040023BF RID: 9151
	private ChunkVisibilityState chunkVisibilityState;

	// Token: 0x040023C0 RID: 9152
	private AttackComponent attackComponent;

	// Token: 0x040023C1 RID: 9153
	[NonSerialized]
	public List<LazyWidgetDataBase> worldZoneWidgetsData = new List<LazyWidgetDataBase>();

	// Token: 0x040023C3 RID: 9155
	private IWGOInteractionHandler interactionHandler;

	// Token: 0x040023C6 RID: 9158
	private bool isOverheadActive;

	// Token: 0x040023C7 RID: 9159
	private UIInteractingItem interactingItem;

	// Token: 0x040023C8 RID: 9160
	private bool areCollidersLayersOverrode;

	// Token: 0x040023C9 RID: 9161
	private Dictionary<int, int> collidersPreviousLayer = new Dictionary<int, int>();

	// Token: 0x040023CA RID: 9162
	private string cachedFuelContainerItemId;

	// Token: 0x040023CB RID: 9163
	private bool fuelContainerItemIdResolved;

	// Token: 0x040023CC RID: 9164
	private const string POWER_SOURCE_GEAR_ICON = "gear";
}
