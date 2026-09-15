using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using UnityEngine;

// Token: 0x020005B5 RID: 1461
[Serializable]
public class WgoData : ObjectLinkedToDefinition<WGODef>, IEquatable<WgoData>, IMovable, ICraftable
{
	// Token: 0x14000077 RID: 119
	// (add) Token: 0x060025C0 RID: 9664 RVA: 0x000B1364 File Offset: 0x000AF564
	// (remove) Token: 0x060025C1 RID: 9665 RVA: 0x000B139C File Offset: 0x000AF59C
	public event WgoData.DelOnToolTickApply OnToolTickApply;

	// Token: 0x14000078 RID: 120
	// (add) Token: 0x060025C2 RID: 9666 RVA: 0x000B13D4 File Offset: 0x000AF5D4
	// (remove) Token: 0x060025C3 RID: 9667 RVA: 0x000B140C File Offset: 0x000AF60C
	public event Action OnOccuredDeath;

	// Token: 0x14000079 RID: 121
	// (add) Token: 0x060025C4 RID: 9668 RVA: 0x000B1444 File Offset: 0x000AF644
	// (remove) Token: 0x060025C5 RID: 9669 RVA: 0x000B147C File Offset: 0x000AF67C
	public event Action<WgoPartData> OnAdditionalWgoPartAdd;

	// Token: 0x1400007A RID: 122
	// (add) Token: 0x060025C6 RID: 9670 RVA: 0x000B14B4 File Offset: 0x000AF6B4
	// (remove) Token: 0x060025C7 RID: 9671 RVA: 0x000B14EC File Offset: 0x000AF6EC
	public event Action<WgoPartData> OnAdditionalWgoPartRemove;

	// Token: 0x1400007B RID: 123
	// (add) Token: 0x060025C8 RID: 9672 RVA: 0x000B1524 File Offset: 0x000AF724
	// (remove) Token: 0x060025C9 RID: 9673 RVA: 0x000B155C File Offset: 0x000AF75C
	public event Action<string> OnGameResChanged;

	// Token: 0x1400007C RID: 124
	// (add) Token: 0x060025CA RID: 9674 RVA: 0x000B1594 File Offset: 0x000AF794
	// (remove) Token: 0x060025CB RID: 9675 RVA: 0x000B15CC File Offset: 0x000AF7CC
	public event Action<Vector3> OnPositionChanged;

	// Token: 0x1400007D RID: 125
	// (add) Token: 0x060025CC RID: 9676 RVA: 0x000B1604 File Offset: 0x000AF804
	// (remove) Token: 0x060025CD RID: 9677 RVA: 0x000B163C File Offset: 0x000AF83C
	public event Action<Vector2> OnDirectionChanged;

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x060025CE RID: 9678 RVA: 0x000B1674 File Offset: 0x000AF874
	// (remove) Token: 0x060025CF RID: 9679 RVA: 0x000B16AC File Offset: 0x000AF8AC
	public event Action OnInteractionEventChanged;

	// Token: 0x1400007F RID: 127
	// (add) Token: 0x060025D0 RID: 9680 RVA: 0x000B16E4 File Offset: 0x000AF8E4
	// (remove) Token: 0x060025D1 RID: 9681 RVA: 0x000B1718 File Offset: 0x000AF918
	public static event Action<WgoData> OnAnyInteractionEventChanged;

	// Token: 0x14000080 RID: 128
	// (add) Token: 0x060025D2 RID: 9682 RVA: 0x000B174C File Offset: 0x000AF94C
	// (remove) Token: 0x060025D3 RID: 9683 RVA: 0x000B1784 File Offset: 0x000AF984
	public event Action<bool> OnInteractableStateChanged;

	// Token: 0x14000081 RID: 129
	// (add) Token: 0x060025D4 RID: 9684 RVA: 0x000B17BC File Offset: 0x000AF9BC
	// (remove) Token: 0x060025D5 RID: 9685 RVA: 0x000B17F4 File Offset: 0x000AF9F4
	public event Action OnWorkerChanged;

	// Token: 0x14000082 RID: 130
	// (add) Token: 0x060025D6 RID: 9686 RVA: 0x000B182C File Offset: 0x000AFA2C
	// (remove) Token: 0x060025D7 RID: 9687 RVA: 0x000B1864 File Offset: 0x000AFA64
	public event Action<string> OnAnimationTriggerSet;

	// Token: 0x14000083 RID: 131
	// (add) Token: 0x060025D8 RID: 9688 RVA: 0x000B189C File Offset: 0x000AFA9C
	// (remove) Token: 0x060025D9 RID: 9689 RVA: 0x000B18D4 File Offset: 0x000AFAD4
	public event Action<bool> OnHiddenStateChanged;

	// Token: 0x14000084 RID: 132
	// (add) Token: 0x060025DA RID: 9690 RVA: 0x000B190C File Offset: 0x000AFB0C
	// (remove) Token: 0x060025DB RID: 9691 RVA: 0x000B1944 File Offset: 0x000AFB44
	public event Action<global::AnimationState> OnAnimationStateSet;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x060025DC RID: 9692 RVA: 0x000B197C File Offset: 0x000AFB7C
	// (remove) Token: 0x060025DD RID: 9693 RVA: 0x000B19B4 File Offset: 0x000AFBB4
	public event Action<int, float> OnAnimationLayerSet;

	// Token: 0x14000086 RID: 134
	// (add) Token: 0x060025DE RID: 9694 RVA: 0x000B19EC File Offset: 0x000AFBEC
	// (remove) Token: 0x060025DF RID: 9695 RVA: 0x000B1A24 File Offset: 0x000AFC24
	public event Action OnTakenDockPointChanged;

	// Token: 0x14000087 RID: 135
	// (add) Token: 0x060025E0 RID: 9696 RVA: 0x000B1A5C File Offset: 0x000AFC5C
	// (remove) Token: 0x060025E1 RID: 9697 RVA: 0x000B1A94 File Offset: 0x000AFC94
	public event Action OnRemoveFromData;

	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x060025E2 RID: 9698 RVA: 0x000B1AC9 File Offset: 0x000AFCC9
	public GameResStr GameResStr
	{
		get
		{
			return this.gameResStr;
		}
	}

	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x060025E3 RID: 9699 RVA: 0x000B1AD1 File Offset: 0x000AFCD1
	public Vector3 BubblePos
	{
		get
		{
			return this.position + this.bubblePosOffset;
		}
	}

	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x060025E4 RID: 9700 RVA: 0x000B1AE4 File Offset: 0x000AFCE4
	public float Quality
	{
		get
		{
			return this.GetQuality();
		}
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x060025E5 RID: 9701 RVA: 0x000B1AEC File Offset: 0x000AFCEC
	// (set) Token: 0x060025E6 RID: 9702 RVA: 0x000B1AF4 File Offset: 0x000AFCF4
	public string WorldId
	{
		get
		{
			return this.worldId;
		}
		set
		{
			this.worldId = value;
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x060025E7 RID: 9703 RVA: 0x000B1B00 File Offset: 0x000AFD00
	// (set) Token: 0x060025E8 RID: 9704 RVA: 0x000B1B4C File Offset: 0x000AFD4C
	public WorldZoneData WorldZoneData
	{
		get
		{
			WorldZoneData worldZoneData;
			if ((worldZoneData = this.worldZoneData) == null)
			{
				GameSceneData gameSceneDataById = MainGame.Instance.GameSave.WorldData.GetGameSceneDataById(this.WorldId);
				worldZoneData = (this.worldZoneData = ((gameSceneDataById != null) ? gameSceneDataById.GetWorldZoneDataById(this.worldZoneDataId) : null));
			}
			return worldZoneData;
		}
		set
		{
			this.worldZoneDataId = ((value == null) ? string.Empty : value.id);
			this.worldZoneData = value;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x060025E9 RID: 9705 RVA: 0x000B1B6B File Offset: 0x000AFD6B
	public bool HasSerializedBounds
	{
		get
		{
			return this.hasSerializedBounds;
		}
	}

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x060025EA RID: 9706 RVA: 0x000B1B73 File Offset: 0x000AFD73
	public ChunkBoundsPair SerializedBounds
	{
		get
		{
			return this.serializedBounds;
		}
	}

	// Token: 0x060025EB RID: 9707 RVA: 0x000B1B7B File Offset: 0x000AFD7B
	public void SetSerializedBounds(ChunkBoundsPair bounds)
	{
		this.serializedBounds = bounds;
		this.hasSerializedBounds = true;
	}

	// Token: 0x060025EC RID: 9708 RVA: 0x000B1B8B File Offset: 0x000AFD8B
	public void ClearSerializedBounds()
	{
		this.hasSerializedBounds = false;
	}

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x060025ED RID: 9709 RVA: 0x000B1B94 File Offset: 0x000AFD94
	public WgoPartData MainWgoPartData
	{
		get
		{
			return this.mainWgoPartData;
		}
	}

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x060025EE RID: 9710 RVA: 0x000B1B9C File Offset: 0x000AFD9C
	public List<WgoPartData> AdditionalWgoPartsData
	{
		get
		{
			return this.additionalWgoPartsData;
		}
	}

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x060025EF RID: 9711 RVA: 0x000B1BA4 File Offset: 0x000AFDA4
	public Inventory Inventory
	{
		get
		{
			if (base.Definition.hasRefToOtherWgoInventory)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(base.Definition.refToOtherWgoInventory);
				if (wgoData != null)
				{
					return wgoData.inventory;
				}
			}
			return this.inventory;
		}
	}

	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x060025F0 RID: 9712 RVA: 0x000B1BE4 File Offset: 0x000AFDE4
	public Inventory CraftInventory
	{
		get
		{
			if (base.Definition.hasRefToOtherWgoInventory)
			{
				WgoData wgoData = MainGame.WorldData.GetWgoData(base.Definition.refToOtherWgoInventory);
				if (wgoData != null)
				{
					return wgoData.CraftInventory;
				}
			}
			return this.craftInventory;
		}
	}

	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x060025F1 RID: 9713 RVA: 0x000B1C24 File Offset: 0x000AFE24
	public SGuid UniqueId
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x060025F2 RID: 9714 RVA: 0x000B1C2C File Offset: 0x000AFE2C
	// (set) Token: 0x060025F3 RID: 9715 RVA: 0x000B1C34 File Offset: 0x000AFE34
	public string CustomTag
	{
		get
		{
			return this.customTag;
		}
		set
		{
			this.customTag = value;
		}
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x060025F4 RID: 9716 RVA: 0x000B1C3D File Offset: 0x000AFE3D
	// (set) Token: 0x060025F5 RID: 9717 RVA: 0x000B1C45 File Offset: 0x000AFE45
	public bool IsHidden
	{
		get
		{
			return this.isHidden;
		}
		set
		{
			this.isHidden = value;
			Action<bool> onHiddenStateChanged = this.OnHiddenStateChanged;
			if (onHiddenStateChanged == null)
			{
				return;
			}
			onHiddenStateChanged(this.isHidden);
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x060025F6 RID: 9718 RVA: 0x000B1C64 File Offset: 0x000AFE64
	public List<InteractionEvent> Events
	{
		get
		{
			return this.events;
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x060025F7 RID: 9719 RVA: 0x000B1C6C File Offset: 0x000AFE6C
	public MovementComponent MovementComponent
	{
		get
		{
			return this.movementComponent;
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x060025F8 RID: 9720 RVA: 0x000B1C74 File Offset: 0x000AFE74
	// (set) Token: 0x060025F9 RID: 9721 RVA: 0x000B1C7C File Offset: 0x000AFE7C
	public HPComponent HpComponent
	{
		get
		{
			return this.hpComponent;
		}
		set
		{
			this.hpComponent = value;
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x060025FA RID: 9722 RVA: 0x000B1C85 File Offset: 0x000AFE85
	public CraftComponent CraftComponent
	{
		get
		{
			return this.craftComponent;
		}
	}

	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x060025FB RID: 9723 RVA: 0x000B1C8D File Offset: 0x000AFE8D
	public SpawnWgoComponent SpawnWGOComponent
	{
		get
		{
			return this.spawnWGOComponent;
		}
	}

	// Token: 0x17000626 RID: 1574
	// (get) Token: 0x060025FC RID: 9724 RVA: 0x000B1C95 File Offset: 0x000AFE95
	// (set) Token: 0x060025FD RID: 9725 RVA: 0x000B1C9D File Offset: 0x000AFE9D
	public TownBuildingWgoComponent TownBuildingWgoComponent
	{
		get
		{
			return this.townBuildingWgoComponent;
		}
		set
		{
			this.townBuildingWgoComponent = value;
		}
	}

	// Token: 0x17000627 RID: 1575
	// (get) Token: 0x060025FE RID: 9726 RVA: 0x000B1CA6 File Offset: 0x000AFEA6
	// (set) Token: 0x060025FF RID: 9727 RVA: 0x000B1CAE File Offset: 0x000AFEAE
	public TownClusterRepairWgoComponent TownClusterRepairWgoComponent
	{
		get
		{
			return this.townClusterRepairWgoComponent;
		}
		set
		{
			this.townClusterRepairWgoComponent = value;
		}
	}

	// Token: 0x17000628 RID: 1576
	// (get) Token: 0x06002600 RID: 9728 RVA: 0x000B1CB7 File Offset: 0x000AFEB7
	// (set) Token: 0x06002601 RID: 9729 RVA: 0x000B1CBF File Offset: 0x000AFEBF
	public SGuid LinkedToTownBuildingUniqueId
	{
		get
		{
			return this.linkedToTownBuildingUniqueId;
		}
		set
		{
			this.linkedToTownBuildingUniqueId = value;
		}
	}

	// Token: 0x17000629 RID: 1577
	// (get) Token: 0x06002602 RID: 9730 RVA: 0x000B1CC8 File Offset: 0x000AFEC8
	// (set) Token: 0x06002603 RID: 9731 RVA: 0x000B1CD0 File Offset: 0x000AFED0
	public SGuid LinkedFromTownBuildingUniqueId
	{
		get
		{
			return this.linkedFromTownBuildingUniqueId;
		}
		set
		{
			this.linkedFromTownBuildingUniqueId = value;
		}
	}

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x06002604 RID: 9732 RVA: 0x000B1CD9 File Offset: 0x000AFED9
	// (set) Token: 0x06002605 RID: 9733 RVA: 0x000B1CE1 File Offset: 0x000AFEE1
	public Vector3 Position
	{
		get
		{
			return this.position;
		}
		set
		{
			this.position = value;
			Action<Vector3> onPositionChanged = this.OnPositionChanged;
			if (onPositionChanged == null)
			{
				return;
			}
			onPositionChanged(value);
		}
	}

	// Token: 0x1700062B RID: 1579
	// (get) Token: 0x06002606 RID: 9734 RVA: 0x000B1CFB File Offset: 0x000AFEFB
	// (set) Token: 0x06002607 RID: 9735 RVA: 0x000B1D1B File Offset: 0x000AFF1B
	public Vector3 Scale
	{
		get
		{
			if (!(this.scale == Vector3.zero))
			{
				return this.scale;
			}
			return Vector3.one;
		}
		set
		{
			this.scale = value;
		}
	}

	// Token: 0x1700062C RID: 1580
	// (get) Token: 0x06002608 RID: 9736 RVA: 0x000B1D24 File Offset: 0x000AFF24
	public List<PerkData> ActivePerks
	{
		get
		{
			return this.activePerks;
		}
	}

	// Token: 0x1700062D RID: 1581
	// (get) Token: 0x06002609 RID: 9737 RVA: 0x000B1D2C File Offset: 0x000AFF2C
	// (set) Token: 0x0600260A RID: 9738 RVA: 0x000B1D34 File Offset: 0x000AFF34
	public bool IsInteractable
	{
		get
		{
			return this.isInteractable;
		}
		set
		{
			bool flag = this.isInteractable;
			this.isInteractable = value;
			if (flag != this.isInteractable)
			{
				Action<bool> onInteractableStateChanged = this.OnInteractableStateChanged;
				if (onInteractableStateChanged == null)
				{
					return;
				}
				onInteractableStateChanged(this.isInteractable);
			}
		}
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000B1D64 File Offset: 0x000AFF64
	public WgoData()
	{
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000B1E91 File Offset: 0x000B0091
	public WgoData(string id, Vector3 position)
		: this(id, position, "")
	{
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000B1EA0 File Offset: 0x000B00A0
	public WgoData(string id, Vector3 position, string worldId)
		: this(id, position, worldId, "")
	{
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x000B1EB0 File Offset: 0x000B00B0
	public WgoData(string id, Vector3 position, string worldId, string customTag)
		: base(id)
	{
		this.uniqueId = new SGuid();
		this.Position = position;
		this.worldId = worldId;
		this.customTag = customTag;
		this.direction.Value = Vector2.zero;
		this.SetDataFromDefinition();
		this.TryCreateMainWgoPartData();
		this.SetupForcedNavigationHoleIfNeeded();
		this.PrepareForGame();
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x000B2027 File Offset: 0x000B0227
	public WgoData(WgoData other)
		: this(other, new SGuid())
	{
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x000B2038 File Offset: 0x000B0238
	public WgoData(WgoData other, SGuid sGuid)
	{
		this.id = other.id;
		this.uniqueId = sGuid;
		this.Position = other.Position;
		this.Scale = other.Scale;
		this.direction = other.direction;
		this.worldId = other.worldId;
		this.CustomTag = other.CustomTag;
		this.worldZoneDataId = other.worldZoneDataId;
		this.inventory = other.inventory;
		this.craftInventory = other.craftInventory;
		this.gameRes = other.gameRes;
		this.gameResStr = other.GameResStr;
		this.events = other.events;
		this.mainWgoPartData = new WgoPartData(other.mainWgoPartData);
		this.additionalWgoPartsData.AddRange(new List<WgoPartData>(other.additionalWgoPartsData.Select((WgoPartData partData) => new WgoPartData(partData)).ToList<WgoPartData>()));
		this.gdPointsRegistered = other.gdPointsRegistered;
		this.movementComponent = other.movementComponent;
		this.craftComponent = other.craftComponent;
		this.hpComponent = other.hpComponent;
		this.spawnWGOComponent = other.spawnWGOComponent;
		this.townBuildingWgoComponent = other.townBuildingWgoComponent;
		this.townClusterRepairWgoComponent = other.townClusterRepairWgoComponent;
		this.gdPointsData = other.gdPointsData;
		this.activePerks = other.activePerks;
		this.customComponentsData = other.customComponentsData;
		this.isHidden = other.isHidden;
		this.customComponentsData = new List<WgoCustomComponentData>(other.customComponentsData);
		this.SetDataFromDefinition();
		this.PrepareForGame();
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x000B22ED File Offset: 0x000B04ED
	public WgoData CreateDataFromMe(Vector3 globalOffset, string gameSceneId, bool copySGuid)
	{
		WgoData wgoData = new WgoData(this, copySGuid ? this.UniqueId : new SGuid());
		wgoData.Position += globalOffset;
		wgoData.WorldId = gameSceneId;
		return wgoData;
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x000B2320 File Offset: 0x000B0520
	public void TryCreateMainWgoPartData()
	{
		if (this.mainWgoPartData == null)
		{
			string text = base.Definition.ResolveAssetId(base.Definition.id, this);
			this.mainWgoPartData = new WgoPartData(text);
		}
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x000B2359 File Offset: 0x000B0559
	public void ReCreateMainWgoPartData(int rotationIndex = -1)
	{
		this.mainWgoPartData = null;
		this.TryCreateMainWgoPartData();
		if (this.mainWgoPartData == null)
		{
			return;
		}
		if (rotationIndex > -1)
		{
			this.mainWgoPartData.rotationIndex = rotationIndex;
		}
		this.mainWgoPartData.PrepareForGame();
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x000B238C File Offset: 0x000B058C
	public void BeginCustomNavMeshCutTracking()
	{
		if (!Application.isPlaying || LazySingleton<GlobalNavigationManager>.Instance == null)
		{
			return;
		}
		this.EnsureCustomNavMeshCutTracking();
		this.SyncCustomNavMeshCutUnit();
	}

	// Token: 0x06002615 RID: 9749 RVA: 0x000B23AF File Offset: 0x000B05AF
	public void RefreshCustomNavMeshCutUnit()
	{
		if (this.isCustomNavMeshCutTracked)
		{
			this.SyncCustomNavMeshCutUnit();
			return;
		}
		if (!GraphCustomNavMeshCutUnit.HasBakedCustomNavMeshCuts(this))
		{
			return;
		}
		this.BeginCustomNavMeshCutTracking();
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x000B23D0 File Offset: 0x000B05D0
	public void StopCustomNavMeshCutTracking()
	{
		if (!this.isCustomNavMeshCutTracked)
		{
			return;
		}
		this.isCustomNavMeshCutTracked = false;
		this.OnPositionChanged -= this.HandleCustomNavMeshCutPositionChanged;
		this.mainWgoPartData.OnStateChange -= this.HandleCustomNavMeshCutPartStateChanged;
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			wgoPartData.OnStateChange -= this.HandleCustomNavMeshCutPartStateChanged;
		}
		this.OnAdditionalWgoPartAdd -= this.HandleCustomNavMeshCutAdditionalPartAdd;
		this.OnAdditionalWgoPartRemove -= this.HandleCustomNavMeshCutAdditionalPartRemove;
		if (this.hasCustomNavMeshCutUnit && LazySingleton<GlobalNavigationManager>.Instance != null)
		{
			LazySingleton<GlobalNavigationManager>.Instance.RemoveCustomNavMeshCutUnit(this.UniqueId);
		}
		this.hasCustomNavMeshCutUnit = false;
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x000B24B8 File Offset: 0x000B06B8
	private void EnsureCustomNavMeshCutTracking()
	{
		if (this.isCustomNavMeshCutTracked)
		{
			return;
		}
		this.isCustomNavMeshCutTracked = true;
		this.OnPositionChanged += this.HandleCustomNavMeshCutPositionChanged;
		this.mainWgoPartData.OnStateChange += this.HandleCustomNavMeshCutPartStateChanged;
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			wgoPartData.OnStateChange += this.HandleCustomNavMeshCutPartStateChanged;
		}
		this.OnAdditionalWgoPartAdd += this.HandleCustomNavMeshCutAdditionalPartAdd;
		this.OnAdditionalWgoPartRemove += this.HandleCustomNavMeshCutAdditionalPartRemove;
	}

	// Token: 0x06002618 RID: 9752 RVA: 0x000B2574 File Offset: 0x000B0774
	private void SyncCustomNavMeshCutUnit()
	{
		if (!Application.isPlaying || !this.isCustomNavMeshCutTracked || LazySingleton<GlobalNavigationManager>.Instance == null)
		{
			return;
		}
		if (GraphCustomNavMeshCutUnit.HasBakedCustomNavMeshCuts(this))
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddCustomNavMeshCutUnit(this.UniqueId, this.Position, this.Scale, this);
			this.hasCustomNavMeshCutUnit = true;
			return;
		}
		if (this.hasCustomNavMeshCutUnit)
		{
			LazySingleton<GlobalNavigationManager>.Instance.RemoveCustomNavMeshCutUnit(this.UniqueId);
			this.hasCustomNavMeshCutUnit = false;
		}
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x000B25EC File Offset: 0x000B07EC
	private void SetupForcedNavigationHoleIfNeeded()
	{
		if (!Application.isPlaying || LazySingleton<GlobalNavigationManager>.Instance == null || this.mainWgoPartData == null)
		{
			return;
		}
		WGODef definition = base.Definition;
		if (definition == null || definition.forceSetNavigationHoleType != WGODef.ForceSetNavigationHoleType.SpawnHoleForce)
		{
			return;
		}
		int stateHash = this.mainWgoPartData.GetStateHash();
		WgoPartBakedData.GraphUpdateSceneBoxData graphUpdateSceneBoxData;
		if (this.mainWgoPartData.BakedData.TryGetGraphUpdateSceneBoxData(stateHash, out graphUpdateSceneBoxData))
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddGraphSceneUpdateUnit(this.uniqueId, this.Position, graphUpdateSceneBoxData);
		}
		WgoPartBakedData.PlannerMeshData plannerMeshData;
		if (this.mainWgoPartData.BakedData.TryGetPlannerMeshData(stateHash, out plannerMeshData))
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddCutUnit(this.uniqueId, this.Position, plannerMeshData);
		}
		else
		{
			LazySingleton<GlobalNavigationManager>.Instance.AddCutUnit(this.uniqueId, this.mainWgoPartData.GetCollisionBoundsRect(this.Position), this.Position.y);
		}
		this.BeginCustomNavMeshCutTracking();
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000B26C9 File Offset: 0x000B08C9
	private void HandleCustomNavMeshCutPositionChanged(Vector3 _)
	{
		if (!this.hasCustomNavMeshCutUnit || !Application.isPlaying || LazySingleton<GlobalNavigationManager>.Instance == null)
		{
			return;
		}
		LazySingleton<GlobalNavigationManager>.Instance.UpdateCustomNavMeshCutUnitTransform(this.UniqueId, this.Position, this.Scale);
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x000B2704 File Offset: 0x000B0904
	private void HandleCustomNavMeshCutPartStateChanged(string _, int __)
	{
		this.SyncCustomNavMeshCutUnit();
	}

	// Token: 0x0600261C RID: 9756 RVA: 0x000B270C File Offset: 0x000B090C
	private void HandleCustomNavMeshCutAdditionalPartAdd(WgoPartData partData)
	{
		partData.OnStateChange += this.HandleCustomNavMeshCutPartStateChanged;
		this.SyncCustomNavMeshCutUnit();
	}

	// Token: 0x0600261D RID: 9757 RVA: 0x000B2726 File Offset: 0x000B0926
	private void HandleCustomNavMeshCutAdditionalPartRemove(WgoPartData partData)
	{
		partData.OnStateChange -= this.HandleCustomNavMeshCutPartStateChanged;
		this.SyncCustomNavMeshCutUnit();
	}

	// Token: 0x0600261E RID: 9758 RVA: 0x000B2740 File Offset: 0x000B0940
	public virtual void PrepareForGame()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.mainWgoPartData.PrepareForGame();
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			wgoPartData.PrepareForGame();
		}
		this.isInitialized = true;
		this.direction.ValueChanged += this.HandleDirectionChanged;
		this.movementComponent.Init(this);
		this.craftComponent.Init(this);
		if (base.Definition == null)
		{
			Debug.LogError("Definition is null [" + this.id + "]");
			return;
		}
		if (base.Definition != null && base.Definition.reviveOnDie)
		{
			this.hpComponent.Init(new Action(this.HandleRevive), new Action(this.HandleDeath));
		}
		else
		{
			this.hpComponent.Init(new Action(this.HandleDeath), null);
		}
		this.hpComponent.OnHpChanged += this.HandleHpChanged;
		if (base.Definition.interactionType == WGODef.InteractionType.TownPalette)
		{
			TownSystem.OnClearTownPalettes += this.HandleOnClearTownPalettes;
		}
		WGODef definition = base.Definition;
		if (!string.IsNullOrEmpty((definition != null) ? definition.attachedScript : null))
		{
			WgoDataScriptsManager.CreateScript(this, base.Definition.attachedScript);
		}
	}

	// Token: 0x0600261F RID: 9759 RVA: 0x000B28B0 File Offset: 0x000B0AB0
	public void ChangeId(string newId)
	{
		this.id = newId;
		List<Item> list = this.inventory.Data.Inventory;
		List<Item> list2 = this.craftInventory.Data.Inventory;
		this.SetDataFromDefinition();
		if (!string.IsNullOrEmpty(base.Definition.attachedScript))
		{
			WgoDataScriptsManager.DestroyScript(this);
		}
		if (!string.IsNullOrEmpty(base.Definition.attachedScript))
		{
			WgoDataScriptsManager.CreateScript(this, base.Definition.attachedScript);
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.inventory.AddItemToInventory(list[i], null, false);
		}
		for (int j = 0; j < list2.Count; j++)
		{
			this.craftInventory.AddItemToInventory(list2[j], null, false);
		}
		if (base.Definition.reviveOnDie)
		{
			this.hpComponent.Init(new Action(this.HandleRevive), new Action(this.HandleDeath));
		}
		else
		{
			this.hpComponent.Init(new Action(this.HandleDeath), null);
		}
		if (GardenBedNavigation.IsGardenPlot(this))
		{
			GardenBedNavigation.UnlinkApproachPoints(this);
		}
		this.RewriteGdPointsData(new List<GDPointData>());
		this.gdPointsRegistered = false;
		this.wasSpawnedAtLeastOnce = false;
		int rotationIndex = this.mainWgoPartData.rotationIndex;
		this.ReCreateMainWgoPartData(rotationIndex);
		this.craftComponent.ResetCraftsFromBalanceCache();
	}

	// Token: 0x06002620 RID: 9760 RVA: 0x000B2A08 File Offset: 0x000B0C08
	public virtual void DeInit()
	{
		this.direction.ValueChanged -= this.HandleDirectionChanged;
		this.hpComponent.OnHpChanged -= this.HandleHpChanged;
		if (base.Definition != null && base.Definition.interactionType == WGODef.InteractionType.TownPalette)
		{
			TownSystem.OnClearTownPalettes -= this.HandleOnClearTownPalettes;
		}
		this.movementComponent.DeInit();
		this.isInitialized = false;
	}

	// Token: 0x06002621 RID: 9761 RVA: 0x000B2A7D File Offset: 0x000B0C7D
	public void SetBubblePointOffset(Vector3 position)
	{
		this.bubblePosOffset = position;
	}

	// Token: 0x06002622 RID: 9762 RVA: 0x000B2A86 File Offset: 0x000B0C86
	public bool Equals(WgoData other)
	{
		return other != null && (this == other || this.uniqueId == other.uniqueId);
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x000B2AA4 File Offset: 0x000B0CA4
	public void OnRemove(bool clearCraftComponent = true)
	{
		this.isRemovingFromData = true;
		this.StopCustomNavMeshCutTracking();
		foreach (SGuid sguid in this.WorkbenchParents)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
			if (wgoData != null)
			{
				wgoData.RemoveWorkbenchExtension(this.UniqueId);
			}
		}
		if (clearCraftComponent)
		{
			this.RemoveFullCoverSoftSlotExtensions();
		}
		if (base.Definition.townQuality > 0)
		{
			MainGame.Instance.GameSave.townSystem.Quality -= base.Definition.townQuality;
		}
		if (!string.IsNullOrEmpty(base.Definition.npcLifeSimGroup))
		{
			NPCGroupPointOfInterestData groupById = MainGame.Instance.GameSave.npcLifeSimulatorData.GetGroupById(base.Definition.npcLifeSimGroup);
			if (groupById == null)
			{
				Debug.LogError("Can't remove wgo from npc life sim group:[" + base.Definition.npcLifeSimGroup + "]");
			}
			else
			{
				groupById.RemoveWgoFromGroup(this);
			}
		}
		if (!string.IsNullOrEmpty(this.occupiedPointOfInterest))
		{
			NPCPointOfInterestData pointById = MainGame.Instance.GameSave.npcLifeSimulatorData.GetPointById(this.occupiedPointOfInterest);
			this.occupiedPointOfInterest = string.Empty;
			if (pointById != null)
			{
				pointById.Deoccupy();
			}
			MainGame.Instance.GameSave.npcLifeSimulatorData.TryRemoveAnimationData(this.UniqueId);
			MainGame.Instance.GameSave.npcLifeSimulatorData.TryRemoveActionData(this.UniqueId);
		}
		if (GardenBedNavigation.IsGardenPlot(this))
		{
			GardenBedNavigation.UnlinkApproachPoints(this);
		}
		this.RewriteGdPointsData(new List<GDPointData>());
		if (!string.IsNullOrEmpty(base.Definition.attachedScript))
		{
			WgoDataScriptsManager.DestroyScript(this);
		}
		MainGame.Instance.GameSave.wgoDelayedEventSystemData.wgoUniqueIds.Remove(this.UniqueId);
		if (clearCraftComponent)
		{
			this.craftComponent.Clear();
		}
		if (this.id.StartsWith("garden_") || this.id.StartsWith("vineyard_"))
		{
			WorldZoneData worldZoneData = this.WorldZoneData;
			if (worldZoneData != null)
			{
				worldZoneData.RemoveGardenOrdersByTarget(this.UniqueId, this.workerId);
			}
		}
		if (GardenBedNavigation.IsGardenPlot(this))
		{
			GardenBedNavigation.RefreshAround(this, this);
		}
		Action onRemoveFromData = this.OnRemoveFromData;
		if (onRemoveFromData == null)
		{
			return;
		}
		onRemoveFromData();
	}

	// Token: 0x06002624 RID: 9764 RVA: 0x000B2CE8 File Offset: 0x000B0EE8
	public void FireEvent(string eventName)
	{
		WgoDataScriptsManager.FireEvent(this, eventName);
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x000B2CF4 File Offset: 0x000B0EF4
	public void AddDelayedEvent(string eventName, float delayTime)
	{
		this.delayedEvents.Add(new DelayedEvent(eventName, delayTime));
		Debug.Log(string.Concat(new string[] { "AddDelayedEvent: [", eventName, "] to WgoData: [", this.id, "]" }));
		if (!MainGame.Instance.GameSave.wgoDelayedEventSystemData.wgoUniqueIds.Contains(this.UniqueId))
		{
			MainGame.Instance.GameSave.wgoDelayedEventSystemData.wgoUniqueIds.Add(this.UniqueId);
		}
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x000B2D88 File Offset: 0x000B0F88
	public void RemoveDelayedEvent(string eventName)
	{
		this.delayedEvents.RemoveAll((DelayedEvent de) => de.eventName == eventName);
		if (this.delayedEvents.Count == 0)
		{
			MainGame.Instance.GameSave.wgoDelayedEventSystemData.wgoUniqueIds.Remove(this.UniqueId);
		}
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x000B2DE8 File Offset: 0x000B0FE8
	public void UpdateDelayedEvents(float deltaTime)
	{
		for (int i = 0; i < this.delayedEvents.Count; i++)
		{
			DelayedEvent delayedEvent = this.delayedEvents[i];
			delayedEvent.delayTime -= deltaTime;
			if (delayedEvent.delayTime <= 0f)
			{
				this.FireEvent(delayedEvent.eventName);
				this.delayedEvents.RemoveAt(i);
				if (this.delayedEvents.Count == 0)
				{
					MainGame.Instance.GameSave.wgoDelayedEventSystemData.wgoUniqueIds.Remove(this.UniqueId);
				}
				return;
			}
		}
	}

	// Token: 0x1700062E RID: 1582
	// (get) Token: 0x06002628 RID: 9768 RVA: 0x000B2E79 File Offset: 0x000B1079
	// (set) Token: 0x06002629 RID: 9769 RVA: 0x000B2E81 File Offset: 0x000B1081
	public Vector3 MovablePosition
	{
		get
		{
			return this.Position;
		}
		set
		{
			this.Position = value;
		}
	}

	// Token: 0x1700062F RID: 1583
	// (get) Token: 0x0600262A RID: 9770 RVA: 0x000B2E79 File Offset: 0x000B1079
	// (set) Token: 0x0600262B RID: 9771 RVA: 0x000B2E81 File Offset: 0x000B1081
	public Vector3 MovablePositionWithoutDirectionChange
	{
		get
		{
			return this.Position;
		}
		set
		{
			this.Position = value;
		}
	}

	// Token: 0x17000630 RID: 1584
	// (get) Token: 0x0600262C RID: 9772 RVA: 0x000B2E8A File Offset: 0x000B108A
	// (set) Token: 0x0600262D RID: 9773 RVA: 0x000B2E97 File Offset: 0x000B1097
	public Vector2 MovableDirection
	{
		get
		{
			return this.direction.Value;
		}
		set
		{
			this.direction.Value = value;
		}
	}

	// Token: 0x17000631 RID: 1585
	// (get) Token: 0x0600262E RID: 9774 RVA: 0x000B2EA5 File Offset: 0x000B10A5
	public Direction Direction
	{
		get
		{
			return this.MovableDirection.ConvertFromVector2();
		}
	}

	// Token: 0x17000632 RID: 1586
	// (get) Token: 0x0600262F RID: 9775 RVA: 0x000B2EB2 File Offset: 0x000B10B2
	public string MovableObjectId
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x00002318 File Offset: 0x00000518
	public void OnTransitionReached(string currentWorldId, string destinationWorldId)
	{
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x000B2EBA File Offset: 0x000B10BA
	public virtual void OnPathComplete(MovementComponent component)
	{
		if (component.Completion == MovementComponent.CompletionState.Success && !string.IsNullOrEmpty(component.OnPathCompleteEvent))
		{
			this.FireEvent(component.OnPathCompleteEvent);
		}
		if (component.TypeDestination == MovementComponent.DestinationType.PointOfInterest)
		{
			MainGame.Instance.npcLifeSimulator.OnWgoReachedPointOfInterest(this);
		}
	}

	// Token: 0x06002632 RID: 9778 RVA: 0x000B2EF8 File Offset: 0x000B10F8
	public void OnTeleportToTransitPoint(Vector3 from, Vector3 to)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.UniqueId);
		if (wgoViewGlobal == null || !wgoViewGlobal.gameObject.activeSelf)
		{
			return;
		}
		WorldFX.Spawn(from, "puff_npc", null, default(Vector3));
		WorldFX.Spawn(to, "puff_npc", null, default(Vector3));
	}

	// Token: 0x17000633 RID: 1587
	// (get) Token: 0x06002633 RID: 9779 RVA: 0x000B2EB2 File Offset: 0x000B10B2
	public string CraftableObjectId
	{
		get
		{
			return this.id;
		}
	}

	// Token: 0x17000634 RID: 1588
	// (get) Token: 0x06002634 RID: 9780 RVA: 0x000B2F54 File Offset: 0x000B1154
	public virtual Inventory CraftableObjectCraftInventory
	{
		get
		{
			return this.CraftInventory;
		}
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06002635 RID: 9781 RVA: 0x000B2F5C File Offset: 0x000B115C
	public Inventory CraftableObjectInventory
	{
		get
		{
			return this.Inventory;
		}
	}

	// Token: 0x17000636 RID: 1590
	// (get) Token: 0x06002636 RID: 9782 RVA: 0x000B2F64 File Offset: 0x000B1164
	public IWorker CraftableAttachedWorker
	{
		get
		{
			return this.Worker;
		}
	}

	// Token: 0x17000637 RID: 1591
	// (get) Token: 0x06002637 RID: 9783 RVA: 0x000B2F6C File Offset: 0x000B116C
	public float AutoCraftTickDuration
	{
		get
		{
			if (base.Definition != null)
			{
				return base.Definition.autocraftTickDuration.EvaluateFloat(this);
			}
			return 0f;
		}
	}

	// Token: 0x17000638 RID: 1592
	// (get) Token: 0x06002638 RID: 9784 RVA: 0x000B2F8D File Offset: 0x000B118D
	public CraftableType CraftableType
	{
		get
		{
			if (base.Definition == null)
			{
				return CraftableType.Regular;
			}
			if (base.Definition.conveyorType != ConveyorElementType.Workbench)
			{
				return CraftableType.Regular;
			}
			return CraftableType.ConveyorWorkbench;
		}
	}

	// Token: 0x06002639 RID: 9785 RVA: 0x000B2FAC File Offset: 0x000B11AC
	public void OnAddToQueue(CraftElementBase ce)
	{
		CraftElement craftElement = ce as CraftElement;
		if (craftElement != null)
		{
			foreach (LazyExpression lazyExpression in craftElement.Definition.onCraftAddQueueExpressions)
			{
				lazyExpression.EvaluateBool(this);
			}
		}
	}

	// Token: 0x0600263A RID: 9786 RVA: 0x000B3010 File Offset: 0x000B1210
	public void OnCraftStart(CraftElementBase ce)
	{
		this.gameRes.Set(ce.Def.setWgoParamsOnStart);
		this.gameRes.Add(ce.Def.addWgoParamsOnStart);
		this.SetGameRes("reached_gold", 0);
		this.SetGameRes("reached_silver", 0);
		this.SetGameRes("reached_bronze", 0);
		foreach (Item item in OutputItems.MakeOutput(ce.PreToWgoOnStartItems))
		{
			this.Inventory.AddItemToInventory(item, null, false);
		}
		CraftElement craftElement = ce as CraftElement;
		if (craftElement != null)
		{
			foreach (LazyExpression lazyExpression in craftElement.Definition.onCraftStartExpressions)
			{
				lazyExpression.EvaluateBool(this);
			}
			CraftDef definition = craftElement.Definition;
			if (definition.transferDestinationStart != TransferDestination.None)
			{
				string destinationItemStart = definition.destinationItemStart;
				switch (definition.transferDestinationStart)
				{
				case TransferDestination.Wgo:
					if (definition.transferNeedsToDestinationOnStart)
					{
						this.Inventory.AddItemsToInventory(craftElement.CraftInput);
					}
					this.Inventory.AddItemsToInventory(OutputItems.MakeOutput(ce.PreToWgoOnStartItems));
					this.MakeDrop(this.Inventory.RemoveItems(definition.dropFromWgoItemsStart, 1, null));
					break;
				case TransferDestination.ItemInside:
					if (definition.transferNeedsToDestinationOnStart)
					{
						this.Inventory.AddItemsToNestedItemById(destinationItemStart, craftElement.CraftInput);
					}
					this.Inventory.AddItemsToNestedItemById(destinationItemStart, OutputItems.MakeOutput(ce.PreToWgoOnStartItems));
					this.MakeDrop(this.Inventory.RemoveItemsFromNestedItemById(destinationItemStart, definition.dropFromWgoItemsStart, null));
					break;
				case TransferDestination.GroupItemInside:
					if (definition.transferNeedsToDestinationOnStart)
					{
						this.Inventory.AddItemsToNestedItemByGroupId(destinationItemStart, craftElement.CraftInput);
					}
					this.Inventory.AddItemsToNestedItemByGroupId(destinationItemStart, OutputItems.MakeOutput(ce.PreToWgoOnStartItems));
					this.MakeDrop(this.Inventory.RemoveItemsFromNestedItemByGroupId(destinationItemStart, definition.dropFromWgoItemsStart, null));
					break;
				}
			}
			if (ce.SucceededProgressTicks > 0)
			{
				this.OnSuccessfulTicksChange(0, ce.SucceededProgressTicks, ce);
			}
		}
		if (ce is CraftElementSermon)
		{
			foreach (Item item2 in OutputItems.MakeOutput(ce.PreToWgoOnFinishItems))
			{
				this.Inventory.AddItemToInventory(item2, null, false);
			}
		}
	}

	// Token: 0x0600263B RID: 9787 RVA: 0x000B32B0 File Offset: 0x000B14B0
	public virtual void OnCraftEnd(CraftElementBase ce)
	{
		WgoDelayedSpawnSystem wgoDelayedSpawnSystem = MainGame.Instance.wgoDelayedSpawnSystem;
		if (base.Definition.wgoGroup == "spawner" && !wgoDelayedSpawnSystem.Contains(this) && !wgoDelayedSpawnSystem.CanSpawn(this))
		{
			wgoDelayedSpawnSystem.Add(this, ce);
			return;
		}
		this.SetGameRes(ce.Def.setWgoParamsOnFinish);
		this.AddGameRes(ce.Def.addWgoParamsOnFinish);
		bool flag;
		if (ce.PreOutputItems.Find((ItemCount x) => x.Def.quality == 3) == null)
		{
			flag = ce.PreOutputItems.Find((ItemCount x) => x.Def.quality == 3) != null;
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3;
		if (ce.PreOutputItems.Find((ItemCount x) => x.Def.quality == 2) == null)
		{
			flag3 = ce.PreOutputItems.Find((ItemCount x) => x.Def.quality == 2) != null;
		}
		else
		{
			flag3 = true;
		}
		bool flag4 = flag3;
		bool flag5;
		if (ce.PreOutputItems.Find((ItemCount x) => x.Def.quality == 1) == null)
		{
			flag5 = ce.PreOutputItems.Find((ItemCount x) => x.Def.quality == 1) != null;
		}
		else
		{
			flag5 = true;
		}
		bool flag6 = flag5;
		this.SetGameRes("reached_gold", flag2 ? 1 : 0);
		this.SetGameRes("reached_silver", (flag4 || flag2) ? 1 : 0);
		this.SetGameRes("reached_bronze", (flag6 || flag4 || flag2) ? 1 : 0);
		if (ce.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenPlanting)
		{
			this.SetGameRes("succeded_cells", ce.SucceededProgressTicks);
		}
		CraftElementSurvey craftElementSurvey = ce as CraftElementSurvey;
		if (craftElementSurvey != null)
		{
			this.DoTechPointsReward(craftElementSurvey.Definition.techRed, craftElementSurvey.Definition.techGreen, craftElementSurvey.Definition.techBlue);
			this.EvaluateOnCraftEndExpressions(craftElementSurvey.Definition.onCraftEndExpressions);
			if (craftElementSurvey.Definition.isScienceFuelCraft)
			{
				this.Inventory.AddItemsToInventory(OutputItems.MakeOutput(ce.PreOutputItems));
			}
		}
		CraftElementMix craftElementMix = ce as CraftElementMix;
		if (craftElementMix != null)
		{
			AlchemyMixDef alchemyMixDef = GameBalance.GetAlchemyMixDef(craftElementMix.CraftId);
			MainGame.Instance.GameSave.knowledgeSystem.DiscoverAlchemyMix(alchemyMixDef);
			this.DoTechPointsReward(alchemyMixDef.techRed.EvaluateInt(this), alchemyMixDef.techGreen.EvaluateInt(this), alchemyMixDef.techBlue.EvaluateInt(this));
			this.EvaluateOnCraftEndExpressions(alchemyMixDef.onCraftEndExpressions);
		}
		CraftElement craftElement = ce as CraftElement;
		if (craftElement != null)
		{
			if (ce.Def.isAuto)
			{
				this.AddGameRes("game_res_tech_red", craftElement.Definition.techRed.EvaluateInt(this));
				this.AddGameRes("game_res_tech_green", craftElement.Definition.techGreen.EvaluateInt(this));
				this.AddGameRes("game_res_tech_blue", craftElement.Definition.techBlue.EvaluateInt(this));
			}
			else
			{
				this.DoTechPointsReward(craftElement.Definition.techRed.EvaluateInt(this), craftElement.Definition.techGreen.EvaluateInt(this), craftElement.Definition.techBlue.EvaluateInt(this));
			}
			if (ce.IsFailed)
			{
				return;
			}
			if (!string.IsNullOrEmpty(craftElement.Definition.globalScriptOnCraftEnd))
			{
				this.FireEvent(craftElement.Definition.globalScriptOnCraftEnd);
			}
			CraftDef definition = craftElement.Definition;
			if (definition.autopsyTypeCraft != AutopsyTypeCraft.None)
			{
				switch (definition.autopsyTypeCraft)
				{
				case AutopsyTypeCraft.ExtractOrgan:
					this.DefineResultForExtractOrganCraft(definition, ce);
					break;
				case AutopsyTypeCraft.InsertOrgan:
					this.DefineResultForInsertOrganCraft(definition, ce);
					break;
				case AutopsyTypeCraft.ChangeOrgan:
					this.DefineResultForChangeOrganCraft(definition, ce);
					break;
				case AutopsyTypeCraft.PocketExtract:
					this.DefineResultForExtractItemFromPocketCraft(definition, ce);
					break;
				case AutopsyTypeCraft.Embalm:
					this.DefineResultForInsertToPocketCraft(definition, ce);
					break;
				}
			}
			else if (definition.transferDestinationEnd != TransferDestination.None)
			{
				string destinationItemEnd = definition.destinationItemEnd;
				switch (definition.transferDestinationEnd)
				{
				case TransferDestination.Wgo:
					if (definition.transferNeedsToDestinationOnFinish)
					{
						this.Inventory.AddItemsToInventory(craftElement.CraftInput);
					}
					this.Inventory.AddItemsToInventory(OutputItems.MakeOutput(ce.PreToWgoOnFinishItems));
					this.Inventory.RemoveItems(definition.removeItemsFromWgo, 1, null);
					this.MakeDrop(this.Inventory.RemoveItems(definition.dropFromWgoItemsEnd, 1, null));
					break;
				case TransferDestination.ItemInside:
					if (definition.transferNeedsToDestinationOnFinish)
					{
						this.Inventory.AddItemsToNestedItemById(destinationItemEnd, craftElement.CraftInput);
					}
					this.Inventory.AddItemsToNestedItemById(destinationItemEnd, OutputItems.MakeOutput(ce.PreToWgoOnFinishItems));
					this.Inventory.RemoveItemsFromNestedItemById(destinationItemEnd, definition.removeItemsFromWgo, null);
					this.MakeDrop(this.Inventory.RemoveItemsFromNestedItemById(destinationItemEnd, definition.dropFromWgoItemsEnd, null));
					break;
				case TransferDestination.GroupItemInside:
					if (definition.transferNeedsToDestinationOnFinish)
					{
						this.Inventory.AddItemsToNestedItemByGroupId(destinationItemEnd, craftElement.CraftInput);
					}
					this.Inventory.AddItemsToNestedItemByGroupId(destinationItemEnd, OutputItems.MakeOutput(ce.PreToWgoOnFinishItems));
					this.Inventory.RemoveItemsFromNestedItemByGroupId(destinationItemEnd, definition.removeItemsFromWgo, null);
					this.MakeDrop(this.Inventory.RemoveItemsFromNestedItemByGroupId(destinationItemEnd, definition.dropFromWgoItemsEnd, null));
					break;
				}
			}
			this.EvaluateOnCraftEndExpressions(craftElement.Definition.onCraftEndExpressions);
			if (!string.IsNullOrEmpty(craftElement.Definition.replaceWgoId))
			{
				if (craftElement.Definition.replaceWgoId == "0")
				{
					if (base.Definition.wgoGroup == "spawner")
					{
						MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this, true);
					}
					else
					{
						Action onOccuredDeath = this.OnOccuredDeath;
						if (onOccuredDeath != null)
						{
							onOccuredDeath();
						}
						this.customDeathCallback = null;
						MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(this, true);
					}
				}
				else
				{
					WgoData wgoData = this;
					if (craftElement.Definition.transferDataOnReplace)
					{
						MainGame.Instance.GameSave.worldData.ChangeWgoData(this, craftElement.Definition.replaceWgoId);
					}
					else
					{
						wgoData = MainGame.Instance.GameSave.worldData.ReplaceWgoData(this, craftElement.Definition.replaceWgoId);
					}
					foreach (LazyExpression lazyExpression in craftElement.Definition.executeOnReplace)
					{
						lazyExpression.EvaluateBool(wgoData);
					}
				}
				if (!string.IsNullOrEmpty(craftElement.Definition.worldFxOnReplace))
				{
					WorldFX.Spawn(this.Position, craftElement.Definition.worldFxOnReplace, null, default(Vector3));
				}
			}
		}
		ZombieWgoData zombieWgoData = this.Worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			ZombieType zombieType = zombieWgoData.ZombieType;
			if (zombieType == ZombieType.Crafter)
			{
				zombieWgoData.CrafterOnAttachedWgoCraftEnd(ce);
				return;
			}
			if (zombieType != ZombieType.ConveyorCrafter)
			{
				return;
			}
			zombieWgoData.ConveyorCrafterOnAttachedWgoCraftEnd(ce);
		}
	}

	// Token: 0x0600263C RID: 9788 RVA: 0x000B39F0 File Offset: 0x000B1BF0
	private void DefineResultForInsertOrganCraft(CraftDef craftDef, CraftElementBase craftElementBase)
	{
		string autopsyItemId = craftDef.autopsyItemId;
		List<Item> list = new List<Item>
		{
			new Item(autopsyItemId, 1)
		};
		if (craftElementBase.SucceededProgressTicks >= craftDef.goldLevel)
		{
			Debug.Log("#craft# Insert Organ Craft:[" + craftDef.id + "] success result");
			this.SetGameRes("reached_gold", 1);
			this.SetGameRes("reached_silver", 1);
			this.SetGameRes("reached_bronze", 1);
			this.Inventory.AddItemsToNestedItemByGroupId(craftDef.destinationItemEnd, list);
			return;
		}
		if (craftElementBase.SucceededProgressTicks >= craftDef.silverLevel)
		{
			Debug.Log("#craft# Insert Organ Craft:[" + craftDef.id + "] medium result");
			this.SetGameRes("reached_silver", 1);
			this.SetGameRes("reached_bronze", 1);
			return;
		}
		Debug.Log("#craft# Insert Organ Craft:[" + craftDef.id + "] failed result");
		ItemDef mistakeForThisItem = GameBalance.Me.GetData<ItemDef>(autopsyItemId).GetMistakeForThisItem();
		this.Inventory.AddItemsToNestedItemByGroupId(craftDef.destinationItemEnd, new List<Item>
		{
			new Item(mistakeForThisItem.id, 1)
		});
	}

	// Token: 0x0600263D RID: 9789 RVA: 0x000B3B0C File Offset: 0x000B1D0C
	private void DefineResultForExtractOrganCraft(CraftDef craftDef, CraftElementBase craftElementBase)
	{
		string autopsyItemId = craftDef.autopsyItemId;
		List<NeedItemData> list = new List<NeedItemData>
		{
			new NeedItemData(autopsyItemId, 1)
		};
		if (craftElementBase.SucceededProgressTicks >= craftDef.goldLevel)
		{
			Debug.Log("#craft# Extract Organ Craft:[" + craftDef.id + "] success result");
			this.SetGameRes("reached_gold", 1);
			this.SetGameRes("reached_silver", 1);
			this.SetGameRes("reached_bronze", 1);
			this.MakeDrop(this.Inventory.RemoveItemsFromNestedItemByGroupId(craftDef.destinationItemEnd, list, null));
			return;
		}
		if (craftElementBase.SucceededProgressTicks >= craftDef.silverLevel)
		{
			Debug.Log("#craft# Extract Organ Craft:[" + craftDef.id + "] medium result");
			this.SetGameRes("reached_silver", 1);
			this.SetGameRes("reached_bronze", 1);
			this.Inventory.RemoveItemsFromNestedItemByGroupId(craftDef.destinationItemEnd, list, null);
			return;
		}
		Debug.Log("#craft# Extract Organ Craft:[" + craftDef.id + "] failed result");
		this.Inventory.RemoveItemsFromNestedItemByGroupId(craftDef.destinationItemEnd, list, null);
		ItemDef mistakeForThisItem = GameBalance.Me.GetData<ItemDef>(autopsyItemId).GetMistakeForThisItem();
		this.Inventory.AddItemsToNestedItemByGroupId(craftDef.destinationItemEnd, new List<Item>
		{
			new Item(mistakeForThisItem.id, 1)
		});
	}

	// Token: 0x0600263E RID: 9790 RVA: 0x000B3C58 File Offset: 0x000B1E58
	private void DefineResultForChangeOrganCraft(CraftDef craftDef, CraftElementBase craftElementBase)
	{
		string autopsyItemId = craftDef.autopsyItemId;
		List<Item> list = new List<Item>
		{
			new Item(autopsyItemId, 1)
		};
		List<NeedItemData> list2 = new List<NeedItemData>
		{
			new NeedItemData(craftElementBase.CustomItems[0].id, 1)
		};
		if (craftElementBase.SucceededProgressTicks >= craftDef.goldLevel)
		{
			Debug.Log(string.Format("#craft# Change Organ Craft:[{0}] success result toRemove:[{1}] toAdd:[{2}]", craftDef.id, list2[0], list[0]));
			this.SetGameRes("reached_gold", 1);
			this.SetGameRes("reached_silver", 1);
			this.SetGameRes("reached_bronze", 1);
			this.Inventory.RemoveItemsFromNestedItemByGroupId(craftDef.destinationItemEnd, list2, null);
			this.Inventory.AddItemsToNestedItemByGroupId(craftDef.destinationItemEnd, list);
			return;
		}
		Debug.Log("#craft# Change Organ Craft:[" + craftDef.id + "] failed result");
	}

	// Token: 0x0600263F RID: 9791 RVA: 0x000B3D3C File Offset: 0x000B1F3C
	private void DefineResultForExtractItemFromPocketCraft(CraftDef craftDef, CraftElementBase craftElementBase)
	{
		List<NeedItemData> list = new List<NeedItemData>
		{
			new NeedItemData(craftElementBase.CustomItems[0].id, craftElementBase.CustomItems[0].Count)
		};
		Debug.Log(string.Format("#craft# Extract Item From Pocket Craft:[{0}] toRemove:[{1}]", craftDef.id, list[0]));
		this.MakeDrop(this.Inventory.RemoveItemsFromNestedItemByGroupId(craftDef.destinationItemEnd, list, null));
	}

	// Token: 0x06002640 RID: 9792 RVA: 0x000B3DB4 File Offset: 0x000B1FB4
	private void DefineResultForInsertToPocketCraft(CraftDef craftDef, CraftElementBase craftElementBase)
	{
		List<Item> list = new List<Item>
		{
			new Item(craftElementBase.CustomItems[0].id, 1)
		};
		Debug.Log(string.Format("#craft# Insert Item To Pocket Craft:[{0}] toAdd:[{1}]", craftDef.id, list[0]));
		this.Inventory.AddItemsToNestedItemByGroupId(craftDef.destinationItemEnd, list);
	}

	// Token: 0x06002641 RID: 9793 RVA: 0x000B3E14 File Offset: 0x000B2014
	public void EvaluateOnCraftEndExpressions(List<LazyExpression> onCraftEndExpressions)
	{
		foreach (LazyExpression lazyExpression in onCraftEndExpressions)
		{
			lazyExpression.EvaluateBool(this);
		}
	}

	// Token: 0x06002642 RID: 9794 RVA: 0x000B3E64 File Offset: 0x000B2064
	private void DoTechPointsReward(int r, int g, int b)
	{
		ZombieWgoData zombieWgoData = this.Worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			zombieWgoData.DoTechPointsReward(this, r, g, b);
			return;
		}
		List<Item> list = new List<Item>();
		string text = "game_res_tech_red";
		string text2 = "game_res_tech_green";
		string text3 = "game_res_tech_blue";
		if (r > 0)
		{
			list.Add(new Item(text, r));
		}
		if (g > 0)
		{
			list.Add(new Item(text2, g));
		}
		if (b > 0)
		{
			list.Add(new Item(text3, b));
		}
		this.MakeDrop(list);
	}

	// Token: 0x06002643 RID: 9795 RVA: 0x000B3EE0 File Offset: 0x000B20E0
	public void DropStoredTechPoints()
	{
		string text = "game_res_tech_red";
		string text2 = "game_res_tech_green";
		string text3 = "game_res_tech_blue";
		int gameResInt = this.GetGameResInt(text);
		int gameResInt2 = this.GetGameResInt(text2);
		int gameResInt3 = this.GetGameResInt(text3);
		this.DoTechPointsReward(gameResInt, gameResInt2, gameResInt3);
		this.SetGameRes(text, 0);
		this.SetGameRes(text2, 0);
		this.SetGameRes(text3, 0);
	}

	// Token: 0x06002644 RID: 9796 RVA: 0x00002318 File Offset: 0x00000518
	public void OnCraftCancel(CraftElementBase craftElement)
	{
	}

	// Token: 0x06002645 RID: 9797 RVA: 0x000B3F3C File Offset: 0x000B213C
	public virtual MultiInventory GetCraftableMultiInventory(bool excludeWorkerInventory = false)
	{
		MultiInventory multiInventory = new MultiInventory();
		if (!excludeWorkerInventory && this.Worker != null)
		{
			multiInventory.Add(this.Worker.WorkerInventory);
		}
		if (this.WorldZoneData != null)
		{
			multiInventory.Add(new MultiInventory(this.WorldZoneData, null, false));
			if (!excludeWorkerInventory && !(this.Worker is ZombieWgoData))
			{
				multiInventory.Add(this.CraftInventory);
			}
			PlayerData playerData = MainGame.PlayerData;
			if (!excludeWorkerInventory && playerData.CurrentWorldZoneData == this.WorldZoneData && this.Worker is PlayerController != MainGame.PlayerController)
			{
				multiInventory.Add(playerData.Inventory);
			}
		}
		else
		{
			multiInventory.Add(new MultiInventory(new List<Inventory> { this.Inventory, this.CraftInventory }));
		}
		return multiInventory;
	}

	// Token: 0x06002646 RID: 9798 RVA: 0x000B400C File Offset: 0x000B220C
	public void OnSuccessfulTicksChange(int startTick, int endTick, CraftElementBase craftElement)
	{
		CraftElement craftElement2 = craftElement as CraftElement;
		if (craftElement2 != null)
		{
			foreach (GameResPerProgress gameResPerProgress in craftElement2.Definition.gameresPerSuccessfulProgress)
			{
				if (gameResPerProgress.sucessfulProgressTick > startTick && gameResPerProgress.sucessfulProgressTick <= endTick)
				{
					this.AddGameRes(gameResPerProgress.gameRes);
				}
			}
		}
	}

	// Token: 0x06002647 RID: 9799 RVA: 0x000B4088 File Offset: 0x000B2288
	public void MakeDrop(List<Item> items)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (Item item in items)
		{
			if (item.id.EndsWith("tech_red"))
			{
				num += item.Count;
			}
			else if (item.id.EndsWith("tech_green"))
			{
				num2 += item.Count;
			}
			else if (item.id.EndsWith("tech_blue"))
			{
				num3 += item.Count;
			}
			else
			{
				this.MakeDrop(item);
			}
		}
		if (num + num2 + num3 > 0)
		{
			DockPointData nearestDockPointData = this.GetNearestDockPointData(MainGame.PlayerData.position.Value, DockPointData.Availability.OnlyNotOccupied);
			Vector3 vector = ((nearestDockPointData == null) ? this.Position : (nearestDockPointData.GetDropPos(ItemSize.Small, 0.3f, 0.4f, 0.5f) + this.Position));
			MainGame.Instance.dropSystem.DropTechPoints(vector, num, num2, num3);
		}
	}

	// Token: 0x06002648 RID: 9800 RVA: 0x000B41A0 File Offset: 0x000B23A0
	public void DropHappiness()
	{
		TechPointsSpawner.CreateSpawner(this.Position, 0, 0, 0, this.GetGameResInt("happiness"));
		this.SetGameRes("happiness", 0);
	}

	// Token: 0x06002649 RID: 9801 RVA: 0x000B41C8 File Offset: 0x000B23C8
	public void MakeDrop(Item item)
	{
		Vector3 dropPos = this.GetDropPos(item);
		ZombieWgoData zombieWgoData = this.Worker as ZombieWgoData;
		if (zombieWgoData == null)
		{
			MainGame.Instance.dropSystem.DropItem(item, this.worldId, dropPos, null);
			return;
		}
		if (zombieWgoData.ZombieType == ZombieType.Gardener)
		{
			if (zombieWgoData.WorkerInventory.CanAddItemToInventory(item))
			{
				zombieWgoData.WorkerInventory.AddItemToInventory(item, null, false);
				return;
			}
			MainGame.Instance.dropSystem.DropItem(item, this.worldId, dropPos, null);
			return;
		}
		else
		{
			if (item.Definition.itemGroupIds.Contains("town_box"))
			{
				MainGame.Instance.dropSystem.DropItem(item, this.worldId, dropPos, null);
				return;
			}
			zombieWgoData.CrafterAddCraftDrop(item);
			return;
		}
	}

	// Token: 0x0600264A RID: 9802 RVA: 0x000B4280 File Offset: 0x000B2480
	public void ProcessReadyToFinishCraft()
	{
		this.CraftComponent.TryFinishCurCraft();
		List<Item> list = new List<Item>();
		list.AddRange(this.CraftableObjectCraftInventory.Data.RemoveAllItems());
		if (list.Count > 0)
		{
			this.MakeDrop(list);
		}
		this.DropStoredTechPoints();
	}

	// Token: 0x0600264B RID: 9803 RVA: 0x000B42CA File Offset: 0x000B24CA
	[NetworkMethod(typeof(WgoDataCommand), "ApplyTool", new object[] { })]
	public void NotifyApplyTool(bool isFirstHit)
	{
		WgoData.DelOnToolTickApply onToolTickApply = this.OnToolTickApply;
		if (onToolTickApply == null)
		{
			return;
		}
		onToolTickApply(isFirstHit);
	}

	// Token: 0x0600264C RID: 9804 RVA: 0x000B42DD File Offset: 0x000B24DD
	public void SetCustomDeathMoment()
	{
		this.customDeathCallback = new Action(this.RunLogicsAfterDeath);
		this.customDeathWasTriggered = false;
	}

	// Token: 0x0600264D RID: 9805 RVA: 0x000B42F8 File Offset: 0x000B24F8
	public void TriggerCustomDeathMoment()
	{
		if (this.customDeathCallback == null)
		{
			Debug.LogWarning("No set custom death moment, but you trying call it. Do nothing.");
			return;
		}
		this.customDeathCallback();
		this.customDeathCallback = null;
		this.customDeathWasTriggered = true;
	}

	// Token: 0x0600264E RID: 9806 RVA: 0x000B4328 File Offset: 0x000B2528
	public Vector3 GetDockPointDataWorldPosition(DockPointData dockPointData)
	{
		if (((dockPointData != null) ? dockPointData.BakedData : null) == null)
		{
			return this.Position;
		}
		WgoPartData wgoPartData = this.MainWgoPartData;
		WgoPartStateData wgoPartStateData;
		if (wgoPartData == null)
		{
			wgoPartStateData = null;
		}
		else
		{
			List<WgoPartStateData> availableVariations = wgoPartData.AvailableVariations;
			wgoPartStateData = ((availableVariations != null) ? availableVariations.Find((WgoPartStateData x) => x.rotationIndex == this.MainWgoPartData.rotationIndex) : null);
		}
		WgoPartStateData wgoPartStateData2 = wgoPartStateData;
		if (wgoPartStateData2 != null && wgoPartStateData2.mirror)
		{
			return this.Position - dockPointData.BakedData.Position;
		}
		return this.Position + dockPointData.BakedData.Position;
	}

	// Token: 0x0600264F RID: 9807 RVA: 0x000B43B0 File Offset: 0x000B25B0
	public Vector3 GetFirstDockPointDataWorldPosition()
	{
		DockPointData dockPointByIndex = this.MainWgoPartData.GetDockPointByIndex(0);
		if (dockPointByIndex != null)
		{
			return this.GetDockPointDataWorldPosition(dockPointByIndex);
		}
		return this.Position;
	}

	// Token: 0x06002650 RID: 9808 RVA: 0x000B43DC File Offset: 0x000B25DC
	public Vector3 GetNearestDockPointDataWorldPositionOrMyPosition(Vector3 positionFrom, DockPointData.Availability availability = DockPointData.Availability.OnlyNotOccupied, Func<DockPointData, Vector3, bool> additionalCheck = null)
	{
		DockPointData dockPointData = this.MainWgoPartData.GetNearestDockPoint(this, positionFrom, availability, DockPointData.Filter.All, null);
		if (dockPointData == null)
		{
			dockPointData = this.MainWgoPartData.GetNearestDockPoint(this, positionFrom, DockPointData.Availability.All, DockPointData.Filter.All, additionalCheck);
		}
		if (dockPointData == null)
		{
			return this.Position;
		}
		return this.GetDockPointDataWorldPosition(dockPointData);
	}

	// Token: 0x06002651 RID: 9809 RVA: 0x000B441F File Offset: 0x000B261F
	public DockPointData GetNearestDockPointData(Vector3 positionFrom, DockPointData.Availability availability = DockPointData.Availability.OnlyNotOccupied)
	{
		return this.MainWgoPartData.GetNearestDockPoint(this, positionFrom, availability, DockPointData.Filter.All, null);
	}

	// Token: 0x06002652 RID: 9810 RVA: 0x000B4434 File Offset: 0x000B2634
	public void AddWgoPart(string wgoPartId, string stateId = "", int rotationIndex = -1)
	{
		for (int i = 0; i < this.additionalWgoPartsData.Count; i++)
		{
			if (this.additionalWgoPartsData[i].id == wgoPartId)
			{
				Debug.LogWarning(string.Concat(new string[] { "Can not add wgo part [", wgoPartId, "] to wgo [", this.id, "]. Already has it" }));
				return;
			}
		}
		WgoPartData wgoPartData = new WgoPartData(wgoPartId, stateId, (rotationIndex == -1) ? this.mainWgoPartData.rotationIndex : rotationIndex);
		this.additionalWgoPartsData.Add(wgoPartData);
		Action<WgoPartData> onAdditionalWgoPartAdd = this.OnAdditionalWgoPartAdd;
		if (onAdditionalWgoPartAdd == null)
		{
			return;
		}
		onAdditionalWgoPartAdd(wgoPartData);
	}

	// Token: 0x06002653 RID: 9811 RVA: 0x000B44E0 File Offset: 0x000B26E0
	public void RemoveWgoPart(string wgoPartId)
	{
		int i = 0;
		while (i < this.additionalWgoPartsData.Count)
		{
			if (this.additionalWgoPartsData[i].id == wgoPartId)
			{
				WgoPartData wgoPartData = this.additionalWgoPartsData[i];
				this.additionalWgoPartsData.RemoveAt(i);
				Action<WgoPartData> onAdditionalWgoPartRemove = this.OnAdditionalWgoPartRemove;
				if (onAdditionalWgoPartRemove == null)
				{
					return;
				}
				onAdditionalWgoPartRemove(wgoPartData);
				return;
			}
			else
			{
				i++;
			}
		}
		Debug.LogWarning(string.Concat(new string[] { "Can not remove wgo part [", wgoPartId, "] from wgo [", this.id, "]. Doesnt have it" }));
	}

	// Token: 0x06002654 RID: 9812 RVA: 0x000B457C File Offset: 0x000B277C
	public void RemoveAllWgoParts()
	{
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			Action<WgoPartData> onAdditionalWgoPartRemove = this.OnAdditionalWgoPartRemove;
			if (onAdditionalWgoPartRemove != null)
			{
				onAdditionalWgoPartRemove(wgoPartData);
			}
		}
		this.additionalWgoPartsData.Clear();
	}

	// Token: 0x06002655 RID: 9813 RVA: 0x000B45E8 File Offset: 0x000B27E8
	public void ApplyWgoPartState(string variationId, int rotationIndex = -1)
	{
		this.mainWgoPartData.TryApplyState(this.UniqueId, variationId, rotationIndex);
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			wgoPartData.TryApplyState(this.UniqueId, variationId, rotationIndex);
		}
	}

	// Token: 0x06002656 RID: 9814 RVA: 0x000B4658 File Offset: 0x000B2858
	public void ApplyRandomState()
	{
		if (this.mainWgoPartData.AvailableVariations.Count > 0)
		{
			WgoPartStateData random = this.mainWgoPartData.AvailableVariations.GetRandom<WgoPartStateData>();
			this.mainWgoPartData.TryApplyState(this.UniqueId, random.id, random.rotationIndex);
		}
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			if (wgoPartData.AvailableVariations.Count > 0)
			{
				WgoPartStateData random2 = wgoPartData.AvailableVariations.GetRandom<WgoPartStateData>();
				wgoPartData.TryApplyState(this.UniqueId, random2.id, random2.rotationIndex);
			}
		}
	}

	// Token: 0x06002657 RID: 9815 RVA: 0x000B471C File Offset: 0x000B291C
	public void ApplyWgoPartState(string wgoPartId, string variationId, int rotationIndex = -1)
	{
		if (this.mainWgoPartData.id == wgoPartId)
		{
			this.mainWgoPartData.TryApplyState(this.UniqueId, variationId, rotationIndex);
			return;
		}
		foreach (WgoPartData wgoPartData in this.additionalWgoPartsData)
		{
			if (wgoPartData.id == wgoPartId)
			{
				wgoPartData.TryApplyState(this.UniqueId, variationId, rotationIndex);
				break;
			}
		}
	}

	// Token: 0x06002658 RID: 9816 RVA: 0x000B47B0 File Offset: 0x000B29B0
	public float GetGameRes(string id)
	{
		return this.gameRes.Get(id, 0f);
	}

	// Token: 0x06002659 RID: 9817 RVA: 0x000B47C3 File Offset: 0x000B29C3
	public int GetGameResInt(string id)
	{
		return this.gameRes.GetInt(id);
	}

	// Token: 0x0600265A RID: 9818 RVA: 0x000B47D1 File Offset: 0x000B29D1
	public void SetGameRes(string id, int value)
	{
		this.gameRes.Set(id, (float)value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x0600265B RID: 9819 RVA: 0x000B47F4 File Offset: 0x000B29F4
	public void SetGameRes(GameRes gameRes)
	{
		this.gameRes.Set(gameRes);
		foreach (GameResAtom gameResAtom in gameRes.List)
		{
			Action<string> onGameResChanged = this.OnGameResChanged;
			if (onGameResChanged != null)
			{
				onGameResChanged(gameResAtom.type);
			}
		}
	}

	// Token: 0x0600265C RID: 9820 RVA: 0x000B4864 File Offset: 0x000B2A64
	public void SetGameRes(string id, float value)
	{
		this.gameRes.Set(id, value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x0600265D RID: 9821 RVA: 0x000B4884 File Offset: 0x000B2A84
	public bool IsGameResEmpty()
	{
		return this.gameRes.IsEmpty();
	}

	// Token: 0x0600265E RID: 9822 RVA: 0x000B4894 File Offset: 0x000B2A94
	public void AddGameRes(GameRes gameRes)
	{
		this.gameRes.Add(gameRes);
		foreach (GameResAtom gameResAtom in gameRes.List)
		{
			Action<string> onGameResChanged = this.OnGameResChanged;
			if (onGameResChanged != null)
			{
				onGameResChanged(gameResAtom.type);
			}
		}
	}

	// Token: 0x0600265F RID: 9823 RVA: 0x000B4904 File Offset: 0x000B2B04
	public void SubGameRes(string id, int value)
	{
		this.gameRes.Sub(id, (float)value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000B4925 File Offset: 0x000B2B25
	public void SubGameRes(string id, float value)
	{
		this.gameRes.Sub(id, value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x06002661 RID: 9825 RVA: 0x000B4945 File Offset: 0x000B2B45
	public void AddGameRes(string id, int value)
	{
		this.gameRes.Add(id, (float)value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x06002662 RID: 9826 RVA: 0x000B4966 File Offset: 0x000B2B66
	public void AddGameRes(string id, float value)
	{
		this.gameRes.Add(id, value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x06002663 RID: 9827 RVA: 0x000B4986 File Offset: 0x000B2B86
	public void MultiplyGameRes(string id, float value)
	{
		this.gameRes.Multiply(id, value);
		Action<string> onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged(id);
	}

	// Token: 0x06002664 RID: 9828 RVA: 0x000B49A8 File Offset: 0x000B2BA8
	public void AddInteractionEvent(string id, bool isFake = false)
	{
		InteractionEvent interactionEvent = new InteractionEvent(id, isFake);
		this.events.Add(interactionEvent);
		Debug.Log(string.Format("Added interaction event: {0}, wgo: {1}, events count: {2}", id, this.id, this.events.Count));
		this.NotifyInteractionEventChanged();
	}

	// Token: 0x06002665 RID: 9829 RVA: 0x000B49F8 File Offset: 0x000B2BF8
	public void RemoveInteractionEvent(string id)
	{
		int num = this.events.FindIndex((InteractionEvent ev) => ev.str == id);
		if (num == -1)
		{
			return;
		}
		Debug.Log(string.Format("Removed interaction event: {0}, wgo: {1}, events count: {2}", id, this.id, this.events.Count));
		this.events.RemoveAt(num);
		this.NotifyInteractionEventChanged();
	}

	// Token: 0x06002666 RID: 9830 RVA: 0x000B4A6C File Offset: 0x000B2C6C
	public bool FireInteractionEvent()
	{
		if (this.events.Count == 0)
		{
			return false;
		}
		InteractionEvent interactionEvent = this.events.PopFirst<InteractionEvent>();
		if (!interactionEvent.isFake)
		{
			this.FireEvent(interactionEvent.str);
			GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.CustomInteraction, this.id + ":" + interactionEvent.str);
			this.NotifyInteractionEventChanged();
			return true;
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.Interaction, this.id ?? "");
		this.NotifyInteractionEventChanged();
		return false;
	}

	// Token: 0x06002667 RID: 9831 RVA: 0x000B4AEA File Offset: 0x000B2CEA
	private void NotifyInteractionEventChanged()
	{
		Action onInteractionEventChanged = this.OnInteractionEventChanged;
		if (onInteractionEventChanged != null)
		{
			onInteractionEventChanged();
		}
		Action<WgoData> onAnyInteractionEventChanged = WgoData.OnAnyInteractionEventChanged;
		if (onAnyInteractionEventChanged == null)
		{
			return;
		}
		onAnyInteractionEventChanged(this);
	}

	// Token: 0x06002668 RID: 9832 RVA: 0x000B4B0D File Offset: 0x000B2D0D
	public InteractionEvent PeekFirstAddedEvent()
	{
		if (this.events.Count == 0)
		{
			return null;
		}
		return this.events[0];
	}

	// Token: 0x06002669 RID: 9833 RVA: 0x000B4B2C File Offset: 0x000B2D2C
	public void RewriteGdPointsData(List<GDPointData> points)
	{
		if (this.gdPointsData.Count > 0)
		{
			MainGame.Instance.GameSave.worldData.gdPointsData.RemoveScenePoints(this.gdPointsData);
		}
		if (points.Count == 0)
		{
			this.gdPointsData = new List<GDPointData>();
			return;
		}
		this.gdPointsData = points;
		MainGame.Instance.GameSave.worldData.gdPointsData.AddScenePoints(points);
	}

	// Token: 0x0600266A RID: 9834 RVA: 0x000B4B9C File Offset: 0x000B2D9C
	public GDPointData GetGDPointData(string id)
	{
		foreach (GDPointData gdpointData in this.gdPointsData)
		{
			if (gdpointData.Id == id)
			{
				return gdpointData;
			}
		}
		return null;
	}

	// Token: 0x17000639 RID: 1593
	// (get) Token: 0x0600266B RID: 9835 RVA: 0x000B4C00 File Offset: 0x000B2E00
	public IWorker Worker
	{
		get
		{
			return IWorker.FromId(this.workerId);
		}
	}

	// Token: 0x0600266C RID: 9836 RVA: 0x000B4C10 File Offset: 0x000B2E10
	public IWorker GetWorkerForCraftExpressions()
	{
		IWorker worker = this.Worker;
		if (this.IsValidWorker(worker))
		{
			return worker;
		}
		if (base.Definition != null)
		{
			IWorker playerController = MainGame.PlayerController;
			if (this.IsValidWorker(playerController))
			{
				return playerController;
			}
		}
		return null;
	}

	// Token: 0x0600266D RID: 9837 RVA: 0x000B4C4C File Offset: 0x000B2E4C
	private bool IsValidWorker(IWorker worker)
	{
		if (worker != null)
		{
			global::UnityEngine.Object @object = worker as global::UnityEngine.Object;
			return @object == null || !(@object == null);
		}
		return false;
	}

	// Token: 0x0600266E RID: 9838 RVA: 0x000B4C74 File Offset: 0x000B2E74
	public bool TrySetWorker(IWorker worker, DockPointData dockPointData = null)
	{
		Action onWorkerChanged = this.OnWorkerChanged;
		if (onWorkerChanged != null)
		{
			onWorkerChanged();
		}
		if (!this.workerId.IsEmpty)
		{
			return false;
		}
		this.workerId.SetGuid(worker.Id);
		ZombieWgoData zombieWgoData = worker as ZombieWgoData;
		if (zombieWgoData != null)
		{
			this.TryAddWorkerCutUnit(zombieWgoData);
			if (dockPointData == null)
			{
				dockPointData = this.MainWgoPartData.GetNearestDockPoint(this, zombieWgoData.Position, DockPointData.Availability.All, DockPointData.Filter.OnlyZombie, null);
			}
			if (dockPointData == null)
			{
				dockPointData = this.MainWgoPartData.GetNearestDockPoint(this, zombieWgoData.Position, DockPointData.Availability.All, DockPointData.Filter.All, null);
			}
			if (dockPointData != null)
			{
				dockPointData.Occupy(zombieWgoData.UniqueId);
				zombieWgoData.takenDockPointsParentSGuid = this.UniqueId;
			}
		}
		Action onWorkerChanged2 = this.OnWorkerChanged;
		if (onWorkerChanged2 != null)
		{
			onWorkerChanged2();
		}
		return true;
	}

	// Token: 0x0600266F RID: 9839 RVA: 0x000B4D24 File Offset: 0x000B2F24
	public void TryAddWorkerCutUnit(IWorker worker)
	{
		if (!this.isWorkerCutUnitAdded)
		{
			ZombieWgoData zombieWgoData = worker as ZombieWgoData;
			if (zombieWgoData != null)
			{
				this.isWorkerCutUnitAdded = true;
				WgoPartBakedData wgoPartBakedData = LazySingletonSerializedSO<WgoPartBakedDataCollection>.Instance.Get(zombieWgoData.Definition.ResolveAssetId(zombieWgoData.Definition.id, zombieWgoData));
				float num = ((wgoPartBakedData != null) ? wgoPartBakedData.RadiusSpehereCutter : (-1f));
				WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.WorldData.GetWorldZoneDataById(this.worldZoneDataId);
				if (num > 0f && worldZoneDataById != null)
				{
					LazySingleton<GlobalNavigationManager>.Instance.AddCutUnit(zombieWgoData.UniqueId, worldZoneDataById.navigationGraph, zombieWgoData.Position, num);
				}
			}
		}
	}

	// Token: 0x06002670 RID: 9840 RVA: 0x000B4DC2 File Offset: 0x000B2FC2
	public void TryRemoveWorkerCutUnit(IWorker worker)
	{
		if (this.isWorkerCutUnitAdded)
		{
			this.isWorkerCutUnitAdded = false;
			LazySingleton<GlobalNavigationManager>.Instance.RemoveCutUnit(worker.Id);
		}
	}

	// Token: 0x06002671 RID: 9841 RVA: 0x000B4DE4 File Offset: 0x000B2FE4
	public void ClearWorker()
	{
		if (!this.workerId.IsEmpty)
		{
			this.TryRemoveWorkerCutUnit(this.Worker);
			this.MainWgoPartData.TryFreeDockPoint(this.UniqueId, this.workerId);
		}
		this.workerId = SGuid.Empty;
		Action onWorkerChanged = this.OnWorkerChanged;
		if (onWorkerChanged == null)
		{
			return;
		}
		onWorkerChanged();
	}

	// Token: 0x06002672 RID: 9842 RVA: 0x000B4E3C File Offset: 0x000B303C
	public Vector3 GetDropPos(Item item)
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.UniqueId);
		if (wgoViewGlobal == null)
		{
			return this.Position;
		}
		if (wgoViewGlobal.dropPoint != null)
		{
			return wgoViewGlobal.dropPoint.DropPos;
		}
		DockPointData nearestDockPoint = this.MainWgoPartData.GetNearestDockPoint(this, MainGame.PlayerData.position.Value, DockPointData.Availability.All, DockPointData.Filter.All, null);
		if (nearestDockPoint != null)
		{
			return nearestDockPoint.GetDropPos(item.Definition.itemSize, 0.3f, 0.4f, 0.5f) + this.Position;
		}
		return this.Position;
	}

	// Token: 0x1700063A RID: 1594
	// (get) Token: 0x06002673 RID: 9843 RVA: 0x000B4ED3 File Offset: 0x000B30D3
	public IReadOnlyList<SGuid> AttachedWorkbenchExtensions
	{
		get
		{
			return this.attachedWorkbenchExtensions;
		}
	}

	// Token: 0x1700063B RID: 1595
	// (get) Token: 0x06002674 RID: 9844 RVA: 0x000B4EDB File Offset: 0x000B30DB
	public IReadOnlyList<SGuid> WorkbenchParents
	{
		get
		{
			return this.workbenchParents;
		}
	}

	// Token: 0x1700063C RID: 1596
	// (get) Token: 0x06002675 RID: 9845 RVA: 0x000B4EE4 File Offset: 0x000B30E4
	public Dictionary<string, List<CraftDefBase>> WorkbenchExtensionsCrafts
	{
		get
		{
			Dictionary<string, List<CraftDefBase>> dictionary = new Dictionary<string, List<CraftDefBase>>();
			foreach (SGuid sguid in this.AttachedWorkbenchExtensions)
			{
				WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(sguid);
				if (wgoData != null)
				{
					List<CraftDef> workbenchExtensionCrafts = GameBalance.Me.GetWorkbenchExtensionCrafts(this.id, wgoData.id);
					List<CraftDefBase> list = new List<CraftDefBase>();
					KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
					foreach (CraftDef craftDef in workbenchExtensionCrafts)
					{
						if (!knowledgeSystem.IsOneTimeCraftCompleted(craftDef))
						{
							list.Add(craftDef);
						}
					}
					dictionary.TryAdd(wgoData.id, list);
				}
			}
			return dictionary;
		}
	}

	// Token: 0x06002676 RID: 9846 RVA: 0x000B4FD8 File Offset: 0x000B31D8
	public void AddWorkbenchExtension(SGuid other)
	{
		if (this.attachedWorkbenchExtensions.Contains(other))
		{
			return;
		}
		this.attachedWorkbenchExtensions.Add(other);
		this.OnWorkbenchExtensionAttached();
	}

	// Token: 0x06002677 RID: 9847 RVA: 0x000B4FFC File Offset: 0x000B31FC
	private void RemoveFullCoverSoftSlotExtensions()
	{
		for (int i = this.attachedWorkbenchExtensions.Count - 1; i >= 0; i--)
		{
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.attachedWorkbenchExtensions[i]);
			if (wgoData != null && WgoData.IsFullCoverSoftSlotExtension(wgoData))
			{
				wgoData.DropItemsOnCascadeRemove();
				MainGame.Instance.GameSave.worldData.RemoveWgoDataFromGameScene(wgoData, true);
			}
		}
	}

	// Token: 0x06002678 RID: 9848 RVA: 0x000B506C File Offset: 0x000B326C
	private void DropItemsOnCascadeRemove()
	{
		BuildingDef buildingDef;
		GameBalance.Me.removableWgos.TryGetValue(this.id, out buildingDef);
		BuildingDef buildingDef2;
		GameBalance.Me.buildableWgos.TryGetValue(this.id, out buildingDef2);
		List<ItemCount> list = null;
		if (buildingDef != null)
		{
			list = buildingDef.outputItems.MakePreOutput(this, 0f);
		}
		else if (buildingDef2 != null)
		{
			list = new List<ItemCount>();
			foreach (NeedItemData needItemData in buildingDef2.needItems)
			{
				list.Add(new ItemCount(needItemData.id, needItemData.GetCount(null)));
			}
		}
		if (list != null)
		{
			foreach (Item item in OutputItems.MakeOutput(list))
			{
				this.MakeDrop(item);
			}
		}
		if (this.Inventory.Data != null)
		{
			foreach (Item item2 in this.Inventory.Data.Inventory)
			{
				this.MakeDrop(item2);
			}
		}
	}

	// Token: 0x06002679 RID: 9849 RVA: 0x000B51CC File Offset: 0x000B33CC
	private static bool IsFullCoverSoftSlotExtension(WgoData wgoData)
	{
		BuildingDef buildingDef;
		return GameBalance.Me.buildableWgos.TryGetValue(wgoData.id, out buildingDef) && buildingDef.chooseCustomBuildAreaType == BuildingDef.BuildAreaChoosingType.FullCoverSoft;
	}

	// Token: 0x0600267A RID: 9850 RVA: 0x000B51FD File Offset: 0x000B33FD
	public void RemoveWorkbenchExtension(SGuid other)
	{
		this.attachedWorkbenchExtensions.Remove(other);
		this.OnWorkbenchExtensionRemoved();
	}

	// Token: 0x0600267B RID: 9851 RVA: 0x000B5212 File Offset: 0x000B3412
	public void AddWorkbenchParent(SGuid other)
	{
		if (this.workbenchParents.Contains(other))
		{
			return;
		}
		this.workbenchParents.Add(other);
	}

	// Token: 0x0600267C RID: 9852 RVA: 0x000B522F File Offset: 0x000B342F
	public void RemoveWorkbenchParent(SGuid other)
	{
		this.workbenchParents.Remove(other);
	}

	// Token: 0x0600267D RID: 9853 RVA: 0x000B5240 File Offset: 0x000B3440
	private void OnWorkbenchExtensionAttached()
	{
		if (this.id == "garden_empty" || this.id == "vineyard_empty")
		{
			GardenInteractionHandler.TryPlacePlantOrder(this);
			return;
		}
		if ((this.id.StartsWith("garden_") || this.id.StartsWith("vineyard_")) && this.id.EndsWith("_ready"))
		{
			GardenInteractionHandler.TryPlaceGatherOrder(this);
		}
	}

	// Token: 0x0600267E RID: 9854 RVA: 0x000B52B8 File Offset: 0x000B34B8
	private void OnWorkbenchExtensionRemoved()
	{
		if (this.id.StartsWith("garden_") || this.id.StartsWith("vineyard_"))
		{
			WorldZoneData worldZoneData = this.WorldZoneData;
			if (worldZoneData == null)
			{
				return;
			}
			worldZoneData.RemoveGardenOrdersByTarget(this.UniqueId, this.workerId);
		}
	}

	// Token: 0x0600267F RID: 9855 RVA: 0x000B5305 File Offset: 0x000B3505
	public void ForceDeath()
	{
		Action onOccuredDeath = this.OnOccuredDeath;
		if (onOccuredDeath != null)
		{
			onOccuredDeath();
		}
		if (this.customDeathCallback != null)
		{
			this.TriggerCustomDeathMoment();
			return;
		}
		this.RunLogicsAfterDeath();
	}

	// Token: 0x06002680 RID: 9856 RVA: 0x000B532D File Offset: 0x000B352D
	public void HandleDeath()
	{
		Action onOccuredDeath = this.OnOccuredDeath;
		if (onOccuredDeath != null)
		{
			onOccuredDeath();
		}
		if (this.customDeathCallback != null)
		{
			return;
		}
		this.RunLogicsAfterDeath();
	}

	// Token: 0x06002681 RID: 9857 RVA: 0x000B5350 File Offset: 0x000B3550
	private void RunLogicsAfterDeath()
	{
		if (LazyNetwork.IsInitialized && LazyNetwork.NetworkManager.IsCoopGame && !LazyNetwork.NetworkManager.IsHost)
		{
			return;
		}
		if (this.customDeathWasTriggered)
		{
			return;
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.WgoDead, this.id);
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.WgoCustomTagDead, this.customTag);
		WgoPartData wgoPartData = this.MainWgoPartData;
		if (wgoPartData != null)
		{
			wgoPartData.TryDisableDockPoints(this.UniqueId);
		}
		if (!SGuid.IsNullOrEmpty(this.takenDockPointsParentSGuid))
		{
			WgoData wgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(this.takenDockPointsParentSGuid);
			if (wgoData != null)
			{
				WgoPartData wgoPartData2 = wgoData.mainWgoPartData;
				if (wgoPartData2 != null)
				{
					wgoPartData2.TryFreeDockPoint(wgoData.UniqueId, this.UniqueId);
				}
				this.takenDockPointsParentSGuid = SGuid.Empty;
			}
		}
		if (base.Definition.deathChanceItems.HasOutput())
		{
			List<Item> list = OutputItems.MakeOutput(base.Definition.deathChanceItems.MakePreOutput(this, 0f));
			for (int i = 0; i < list.Count; i++)
			{
				this.MakeDrop(list[i]);
			}
		}
		if (base.Definition.dropInventoryOnDeath)
		{
			foreach (Item item in this.Inventory.Data.Inventory)
			{
				this.MakeDrop(item);
			}
			this.Inventory.Data.RemoveAllItems();
		}
		foreach (LazyExpression lazyExpression in base.Definition.executeOnDeath)
		{
			lazyExpression.EvaluateBool(this);
		}
		if (base.Definition.inspirationOnDeath.HasExpression)
		{
			base.Definition.inspirationOnDeath.EvaluateBool(this);
		}
		if (this.CraftComponent.IsStarted && base.Definition.interactionType != WGODef.InteractionType.WellUpgrade)
		{
			this.CraftComponent.Cancel();
		}
		this.DoTechPointsReward(base.Definition.techRed, base.Definition.techGreen, base.Definition.techBlue);
		if (!base.Definition.reviveOnDie)
		{
			string text = base.Definition.replaceToWgoOnDie.Evaluate(this);
			if (!string.IsNullOrEmpty(text))
			{
				WgoData wgoData2 = this;
				if (base.Definition.transferDataToNewWgo)
				{
					MainGame.Instance.GameSave.WorldData.ChangeWgoData(this, text);
				}
				else
				{
					List<SpawnStage> list2 = new List<SpawnStage>();
					list2.AddRange(this.SpawnWGOComponent.SpawnStages);
					wgoData2 = MainGame.Instance.GameSave.WorldData.ReplaceWgoData(this, text);
					wgoData2.SpawnWGOComponent.SpawnStages = list2;
				}
				foreach (LazyExpression lazyExpression2 in base.Definition.executeOnReplace)
				{
					lazyExpression2.EvaluateBool(wgoData2);
				}
				if (this.Worker != null)
				{
					ZombieWgoData zombieWgoData = this.Worker as ZombieWgoData;
					if (zombieWgoData != null && zombieWgoData.ZombieType == ZombieType.Gardener)
					{
						zombieWgoData.GardenerStopWorkActivity(wgoData2.UniqueId, false);
					}
				}
				return;
			}
			MainGame.Instance.GameSave.WorldData.RemoveWgoDataFromGameScene(this, true);
		}
		LazySingleton<GlobalNavigationManager>.Instance.RemoveCutUnit(this.uniqueId);
		this.StopCustomNavMeshCutTracking();
	}

	// Token: 0x06002682 RID: 9858 RVA: 0x000B56C0 File Offset: 0x000B38C0
	private void HandleRevive()
	{
		this.hpComponent.RestoreFullHp();
	}

	// Token: 0x06002683 RID: 9859 RVA: 0x000B56D0 File Offset: 0x000B38D0
	public virtual void SetDataFromDefinition()
	{
		if (base.Definition == null)
		{
			return;
		}
		if (base.Definition.inventorySize > 0)
		{
			this.inventory = Inventory.Create(base.Definition.inventorySize, base.Definition.isFuelContainer, base.Definition.inventoryWhiteList, base.Definition.inventoryBlackList, this.id);
			using (List<NeedItemData>.Enumerator enumerator = base.Definition.startItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NeedItemData needItemData = enumerator.Current;
					this.inventory.AddItemToInventory(new Item(needItemData.id, needItemData.GetCount(null)), null, false);
				}
				goto IL_00B6;
			}
		}
		this.inventory = Inventory.GetEmpty();
		IL_00B6:
		if (base.Definition.craftInventorySize > 0)
		{
			this.craftInventory = Inventory.GetCraftInventory(base.Definition.craftInventorySize);
		}
		this.hpComponent = ((base.Definition.startHpValue == -1) ? new HPComponent(base.Definition.hp) : new HPComponent(base.Definition.hp, base.Definition.startHpValue));
		this.SetupForcedNavigationHoleIfNeeded();
		if (!string.IsNullOrEmpty(base.Definition.zombieRollDataId))
		{
			ZombieSkinHelper.RollAndApplyZombieSkinToWgoData(this, base.Definition.zombieRollDataId);
		}
		if (base.Definition.townQuality > 0)
		{
			MainGame.Instance.GameSave.townSystem.Quality += base.Definition.townQuality;
		}
		if (!string.IsNullOrEmpty(base.Definition.npcLifeSimGroup))
		{
			NPCGroupPointOfInterestData groupById = MainGame.Instance.GameSave.npcLifeSimulatorData.GetGroupById(base.Definition.npcLifeSimGroup);
			if (groupById == null)
			{
				Debug.LogError("Can't add wgo to npc life sim group:[" + base.Definition.npcLifeSimGroup + "]");
			}
			else
			{
				groupById.AddWgoToGroup(this);
			}
		}
		if (!string.IsNullOrEmpty(base.Definition.npcLifeSimHome))
		{
			this.gameResStr.Set("npc_life_sim_home", base.Definition.npcLifeSimHome);
		}
		if (base.Definition.interactionType == WGODef.InteractionType.PorterStation)
		{
			PorterStationDef data = GameBalance.Me.GetData<PorterStationDef>(base.Definition.id);
			if (data != null)
			{
				foreach (NeedItemData needItemData2 in data.items)
				{
					this.SetGameRes(needItemData2.id, (needItemData2.GetCount(null) > 0) ? 1 : 0);
				}
			}
		}
	}

	// Token: 0x06002684 RID: 9860 RVA: 0x000B5970 File Offset: 0x000B3B70
	public void AddPerk(string id)
	{
		PerkData perkData = this.activePerks.Find((PerkData x) => x.id == id);
		if (perkData == null)
		{
			this.AddNewPerk(new PerkData(id));
			return;
		}
		switch (perkData.Definition.perkAddType)
		{
		case PerkAddType.Update:
			Debug.LogError(string.Format("Trying add perk:[{0}] to wgo:[{1}] with PerkAddType:[{2}], it is not supported for Wgo!!!", id, id, PerkAddType.Update));
			return;
		case PerkAddType.Sum:
			Debug.LogError(string.Format("Trying add perk:[{0}] to wgo:[{1}] with PerkAddType:[{2}], it is not supported for Wgo!!!", id, id, PerkAddType.Sum));
			return;
		case PerkAddType.AsNew:
			this.AddNewPerk(new PerkData(id));
			return;
		default:
			return;
		}
	}

	// Token: 0x06002685 RID: 9861 RVA: 0x000B5A2C File Offset: 0x000B3C2C
	public void RemovePerk(string id)
	{
		PerkData perkData = this.activePerks.Find((PerkData x) => x.id == id);
		if (perkData != null)
		{
			this.RemovePerk(perkData);
		}
	}

	// Token: 0x06002686 RID: 9862 RVA: 0x000B5A68 File Offset: 0x000B3C68
	public void RemoveAllPerks()
	{
		for (int i = this.activePerks.Count - 1; i >= 0; i--)
		{
			this.RemovePerk(this.activePerks[i]);
		}
	}

	// Token: 0x06002687 RID: 9863 RVA: 0x000B5AA0 File Offset: 0x000B3CA0
	public void RemovePerk(PerkData perk)
	{
		this.activePerks.Remove(perk);
		if (perk.Definition.setGameResOnRemove.List.Count > 0)
		{
			this.SetGameRes(perk.Definition.setGameResOnRemove);
		}
		if (!perk.Definition.addGameResOnRemove.IsEmpty())
		{
			this.AddGameRes(perk.Definition.addGameResOnRemove);
		}
		foreach (LazyExpression lazyExpression in perk.Definition.onRemoveExpressions)
		{
			lazyExpression.EvaluateBool(this);
		}
		Debug.Log(string.Concat(new string[] { "Perk:[", perk.id, "] removed from wgo:[", this.id, "]" }));
	}

	// Token: 0x06002688 RID: 9864 RVA: 0x000B5B8C File Offset: 0x000B3D8C
	public bool HasPerk(string id)
	{
		return this.activePerks.Find((PerkData x) => x.id == id) != null;
	}

	// Token: 0x06002689 RID: 9865 RVA: 0x000B5BC0 File Offset: 0x000B3DC0
	public PerkData GetPerk(string id)
	{
		return this.activePerks.Find((PerkData x) => x.id == id);
	}

	// Token: 0x0600268A RID: 9866 RVA: 0x000B5BF4 File Offset: 0x000B3DF4
	private void AddNewPerk(PerkData perk)
	{
		if (perk.Definition.duration > 0f)
		{
			Debug.LogError(string.Concat(new string[] { "Trying add perk:[", perk.id, "] to wgo:[", this.id, "] with duration, it is not supported for Wgo!!!" }));
			return;
		}
		perk.currentDuration = perk.Definition.duration;
		if (!perk.Definition.setGameResOnAdd.IsEmpty())
		{
			this.SetGameRes(perk.Definition.setGameResOnAdd);
		}
		if (!perk.Definition.addGameResOnAdd.IsEmpty())
		{
			this.AddGameRes(perk.Definition.addGameResOnAdd);
		}
		foreach (LazyExpression lazyExpression in perk.Definition.onAddExpressions)
		{
			lazyExpression.EvaluateBool(this);
		}
		this.activePerks.Add(perk);
		Debug.Log(string.Concat(new string[] { "Perk:[", perk.id, "] added to wgo:[", this.id, "]" }));
	}

	// Token: 0x0600268B RID: 9867 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void OnReleaseWgoPartToPool()
	{
	}

	// Token: 0x0600268C RID: 9868 RVA: 0x000B5D38 File Offset: 0x000B3F38
	protected virtual float GetQuality()
	{
		if (this.id == "grave_ground")
		{
			return base.Definition.quality.EvaluateFloat(this) + this.Inventory.GetTotalQualityGrave();
		}
		if (this.id.StartsWith("npc_town_barracks_mercenary"))
		{
			if (!MainGame.Instance.GameSave.militaryBaseData.IsMercenaryPayed)
			{
				return 0f;
			}
			Item itemByType = this.inventory.GetItemByType(ItemType.BodyArmor);
			Item itemByGroupId = this.inventory.GetItemByGroupId("weapon");
			return (float)((itemByType.IsEmpty ? 0 : itemByType.Definition.quality) + (itemByGroupId.IsEmpty ? 0 : itemByGroupId.Definition.quality));
		}
		else
		{
			if (!MainGame.Instance.GameSave.militaryBaseData.ContainsFighter(this, false))
			{
				return base.Definition.quality.EvaluateFloat(this) + (base.Definition.considerInventoryQuality ? this.Inventory.GetTotalQuality() : 0f);
			}
			ZombieWgoData zombieWgoData = this as ZombieWgoData;
			if (zombieWgoData == null)
			{
				return 0f;
			}
			if (zombieWgoData.Hand.IsEmpty || !zombieWgoData.Hand.Definition.isWeapon || zombieWgoData.Armor.IsEmpty)
			{
				return 0f;
			}
			return (float)(zombieWgoData.Hand.Definition.quality + zombieWgoData.Armor.Definition.quality);
		}
	}

	// Token: 0x0600268D RID: 9869 RVA: 0x000B5E9F File Offset: 0x000B409F
	private void HandleDirectionChanged(Vector2 direction)
	{
		Action<Vector2> onDirectionChanged = this.OnDirectionChanged;
		if (onDirectionChanged == null)
		{
			return;
		}
		onDirectionChanged(direction);
	}

	// Token: 0x0600268E RID: 9870 RVA: 0x000B5EB2 File Offset: 0x000B40B2
	public void SetTriggerToAnimator(string triggerName)
	{
		this.wasCustomAnimationFired = true;
		Action<string> onAnimationTriggerSet = this.OnAnimationTriggerSet;
		if (onAnimationTriggerSet == null)
		{
			return;
		}
		onAnimationTriggerSet(triggerName);
	}

	// Token: 0x0600268F RID: 9871 RVA: 0x000B5ECC File Offset: 0x000B40CC
	public void SetLayerWeightToAnimator(int layerIndex, float weight)
	{
		Action<int, float> onAnimationLayerSet = this.OnAnimationLayerSet;
		if (onAnimationLayerSet == null)
		{
			return;
		}
		onAnimationLayerSet(layerIndex, weight);
	}

	// Token: 0x06002690 RID: 9872 RVA: 0x000B5EE0 File Offset: 0x000B40E0
	public void SetStateToAnimator(global::AnimationState animationState)
	{
		this.wasCustomAnimationFired = true;
		Action<global::AnimationState> onAnimationStateSet = this.OnAnimationStateSet;
		if (onAnimationStateSet == null)
		{
			return;
		}
		onAnimationStateSet(animationState);
	}

	// Token: 0x06002691 RID: 9873 RVA: 0x000B5EFA File Offset: 0x000B40FA
	public void TryFireSerializedTrigger()
	{
		if (!string.IsNullOrEmpty(this.customAnimationTrigger))
		{
			this.SetTriggerToAnimator(this.customAnimationTrigger);
		}
	}

	// Token: 0x06002692 RID: 9874 RVA: 0x000B5F15 File Offset: 0x000B4115
	public void SetCustomAnimationTrigger(string triggerName = "")
	{
		this.customAnimationTrigger = triggerName;
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x000B5F1E File Offset: 0x000B411E
	public Vector3 GetTeleportPointPosition()
	{
		GDPointData gdpointDataById = MainGame.Instance.GameSave.worldData.gdPointsData.GetGDPointDataById("tp_point_" + this.id);
		if (gdpointDataById == null)
		{
			return this.Position;
		}
		return gdpointDataById.Position;
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x000B5F59 File Offset: 0x000B4159
	public void OccupyDockPoint(DockPointData dockPoint, SGuid dockPointParent)
	{
		dockPoint.Occupy(this.UniqueId);
		this.takenDockPointsParentSGuid = dockPointParent;
		Action onTakenDockPointChanged = this.OnTakenDockPointChanged;
		if (onTakenDockPointChanged == null)
		{
			return;
		}
		onTakenDockPointChanged();
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x000B5F7E File Offset: 0x000B417E
	public void UnOccupyDockPoint(DockPointData dockPoint)
	{
		dockPoint.UnOccupy();
		this.takenDockPointsParentSGuid = SGuid.Empty;
		Action onTakenDockPointChanged = this.OnTakenDockPointChanged;
		if (onTakenDockPointChanged == null)
		{
			return;
		}
		onTakenDockPointChanged();
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x000B5FA4 File Offset: 0x000B41A4
	private void HandleHpChanged(HPComponent component)
	{
		Dictionary<string, GlobalEventsSystem.Event> dictionary;
		if (GlobalEventsSystem.Me.GetEvents(GlobalEventsSystem.Event.Type.WgoCustomTagHpValueReached, out dictionary))
		{
			List<string> list = null;
			foreach (KeyValuePair<string, GlobalEventsSystem.Event> keyValuePair in dictionary)
			{
				string key = keyValuePair.Key;
				int num = key.IndexOf(':');
				int num2;
				if (num >= 0 && num != key.Length - 1 && this.customTag != null && this.customTag.Length == num && (num <= 0 || string.CompareOrdinal(key, 0, this.customTag, 0, num) == 0) && WgoData.TryParseIntInPlace(key, num + 1, out num2) && num2 < component.prevHp && num2 >= component.Hp)
				{
					List<string> list2;
					if ((list2 = list) == null)
					{
						list2 = (list = new List<string>());
					}
					list2.Add(key);
				}
			}
			if (list != null)
			{
				foreach (string text in list)
				{
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.WgoCustomTagHpValueReached, text);
				}
			}
		}
		if (!base.Definition.hpAction.isValidAction)
		{
			return;
		}
		if (component.Hp == base.Definition.hpAction.hp)
		{
			base.Definition.hpAction.EvaluateExpression(this);
		}
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x000B6114 File Offset: 0x000B4314
	private static bool TryParseIntInPlace(string source, int startIndex, out int value)
	{
		value = 0;
		if (startIndex >= source.Length)
		{
			return false;
		}
		for (int i = startIndex; i < source.Length; i++)
		{
			char c = source[i];
			if (c < '0' || c > '9')
			{
				return false;
			}
			value = value * 10 + (int)(c - '0');
		}
		return true;
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x000B6164 File Offset: 0x000B4364
	private void HandleOnClearTownPalettes()
	{
		foreach (Item item in this.Inventory.Data.Inventory)
		{
			TownSystem.moneyForPalettes += item.Definition.basePrice * item.Count;
			foreach (LazyExpression lazyExpression in item.Definition.expressionsOnSell)
			{
				lazyExpression.Evaluate(item);
			}
		}
		this.Inventory.Clear();
	}

	// Token: 0x040020EB RID: 8427
	private static Vector3 NPC_BUBBLE_Y_OFFSET = Vector3.up * 1.5f;

	// Token: 0x040020FD RID: 8445
	private Action customDeathCallback;

	// Token: 0x040020FE RID: 8446
	[SerializeField]
	private SGuid uniqueId = new SGuid();

	// Token: 0x040020FF RID: 8447
	[SerializeField]
	private Vector3 position;

	// Token: 0x04002100 RID: 8448
	[SerializeField]
	private Vector3 scale = Vector3.one;

	// Token: 0x04002101 RID: 8449
	[SerializeField]
	public VariableNotificator<Vector2> direction = new VariableNotificator<Vector2>();

	// Token: 0x04002102 RID: 8450
	[SerializeField]
	private string worldId;

	// Token: 0x04002103 RID: 8451
	[SerializeField]
	private string customTag;

	// Token: 0x04002104 RID: 8452
	[SerializeField]
	private Inventory inventory = new Inventory();

	// Token: 0x04002105 RID: 8453
	[SerializeField]
	protected Inventory craftInventory = new Inventory();

	// Token: 0x04002106 RID: 8454
	[SerializeField]
	protected GameRes gameRes = new GameRes();

	// Token: 0x04002107 RID: 8455
	[SerializeField]
	private GameResStr gameResStr = new GameResStr();

	// Token: 0x04002108 RID: 8456
	[SerializeField]
	private string worldZoneDataId;

	// Token: 0x04002109 RID: 8457
	[SerializeField]
	private List<InteractionEvent> events = new List<InteractionEvent>();

	// Token: 0x0400210A RID: 8458
	[SerializeField]
	private List<DelayedEvent> delayedEvents = new List<DelayedEvent>();

	// Token: 0x0400210B RID: 8459
	[SerializeField]
	private bool isInteractable = true;

	// Token: 0x0400210C RID: 8460
	[SerializeField]
	private bool isHidden;

	// Token: 0x0400210D RID: 8461
	[SerializeField]
	private string customAnimationTrigger;

	// Token: 0x0400210E RID: 8462
	[SerializeField]
	private WgoPartData mainWgoPartData;

	// Token: 0x0400210F RID: 8463
	[SerializeField]
	private List<WgoPartData> additionalWgoPartsData = new List<WgoPartData>();

	// Token: 0x04002110 RID: 8464
	[SerializeField]
	protected List<PerkData> activePerks = new List<PerkData>();

	// Token: 0x04002111 RID: 8465
	[SerializeField]
	private List<SGuid> workbenchParents = new List<SGuid>();

	// Token: 0x04002112 RID: 8466
	[SerializeField]
	private List<SGuid> attachedWorkbenchExtensions = new List<SGuid>();

	// Token: 0x04002113 RID: 8467
	[SerializeField]
	private SGuid workerId = SGuid.Empty;

	// Token: 0x04002114 RID: 8468
	[SerializeField]
	private MovementComponent movementComponent = new MovementComponent();

	// Token: 0x04002115 RID: 8469
	[SerializeField]
	private CraftComponent craftComponent = new CraftComponent();

	// Token: 0x04002116 RID: 8470
	[SerializeField]
	private HPComponent hpComponent = new HPComponent();

	// Token: 0x04002117 RID: 8471
	[SerializeField]
	private SpawnWgoComponent spawnWGOComponent = new SpawnWgoComponent();

	// Token: 0x04002118 RID: 8472
	[SerializeField]
	private TownBuildingWgoComponent townBuildingWgoComponent = new TownBuildingWgoComponent();

	// Token: 0x04002119 RID: 8473
	[SerializeField]
	private TownClusterRepairWgoComponent townClusterRepairWgoComponent = new TownClusterRepairWgoComponent();

	// Token: 0x0400211A RID: 8474
	[SerializeField]
	private SGuid linkedToTownBuildingUniqueId = SGuid.Empty;

	// Token: 0x0400211B RID: 8475
	[SerializeField]
	private SGuid linkedFromTownBuildingUniqueId = SGuid.Empty;

	// Token: 0x0400211C RID: 8476
	[SerializeField]
	private bool customDeathWasTriggered;

	// Token: 0x0400211D RID: 8477
	[HideInInspector]
	public StartReses startReses;

	// Token: 0x0400211E RID: 8478
	[NonSerialized]
	private ChunkBoundsPair serializedBounds;

	// Token: 0x0400211F RID: 8479
	[NonSerialized]
	private bool hasSerializedBounds;

	// Token: 0x04002120 RID: 8480
	public SGuid takenDockPointsParentSGuid;

	// Token: 0x04002121 RID: 8481
	public string occupiedPointOfInterest;

	// Token: 0x04002122 RID: 8482
	public List<WgoCustomComponentData> customComponentsData = new List<WgoCustomComponentData>();

	// Token: 0x04002123 RID: 8483
	[NonSerialized]
	public bool isTempObject;

	// Token: 0x04002124 RID: 8484
	private bool isInitialized;

	// Token: 0x04002125 RID: 8485
	public bool wasSpawnedAtLeastOnce;

	// Token: 0x04002126 RID: 8486
	public bool isRemovingFromData;

	// Token: 0x04002127 RID: 8487
	public bool gdPointsRegistered;

	// Token: 0x04002128 RID: 8488
	public List<GDPointData> gdPointsData = new List<GDPointData>();

	// Token: 0x04002129 RID: 8489
	[NonSerialized]
	public bool wasCustomAnimationFired;

	// Token: 0x0400212A RID: 8490
	private Vector3 bubblePosOffset = WgoData.NPC_BUBBLE_Y_OFFSET;

	// Token: 0x0400212B RID: 8491
	private WorldZoneData worldZoneData;

	// Token: 0x0400212C RID: 8492
	private bool isWorkerCutUnitAdded;

	// Token: 0x0400212D RID: 8493
	private bool isCustomNavMeshCutTracked;

	// Token: 0x0400212E RID: 8494
	private bool hasCustomNavMeshCutUnit;

	// Token: 0x020005B6 RID: 1462
	// (Invoke) Token: 0x0600269C RID: 9884
	public delegate void DelOnToolTickApply(bool isFirstHit);
}
