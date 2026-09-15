using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using Pathfinding;
using UnityEngine;

// Token: 0x020005ED RID: 1517
[Serializable]
public class ZombieWgoData : WgoData, IWorker
{
	// Token: 0x1400008F RID: 143
	// (add) Token: 0x06002826 RID: 10278 RVA: 0x000BAEF8 File Offset: 0x000B90F8
	// (remove) Token: 0x06002827 RID: 10279 RVA: 0x000BAF30 File Offset: 0x000B9130
	public event Action<global::AnimationState, bool> OnAnimationStateChanged;

	// Token: 0x14000090 RID: 144
	// (add) Token: 0x06002828 RID: 10280 RVA: 0x000BAF68 File Offset: 0x000B9168
	// (remove) Token: 0x06002829 RID: 10281 RVA: 0x000BAFA0 File Offset: 0x000B91A0
	public event Action<Item, bool> OnSetOverheadItem;

	// Token: 0x14000091 RID: 145
	// (add) Token: 0x0600282A RID: 10282 RVA: 0x000BAFD8 File Offset: 0x000B91D8
	// (remove) Token: 0x0600282B RID: 10283 RVA: 0x000BB010 File Offset: 0x000B9210
	public event Action OnRemoveOverheadItem;

	// Token: 0x14000092 RID: 146
	// (add) Token: 0x0600282C RID: 10284 RVA: 0x000BB048 File Offset: 0x000B9248
	// (remove) Token: 0x0600282D RID: 10285 RVA: 0x000BB080 File Offset: 0x000B9280
	public event Action<Item> OnSetInteractingItem;

	// Token: 0x14000093 RID: 147
	// (add) Token: 0x0600282E RID: 10286 RVA: 0x000BB0B8 File Offset: 0x000B92B8
	// (remove) Token: 0x0600282F RID: 10287 RVA: 0x000BB0F0 File Offset: 0x000B92F0
	public event Action OnRemoveInteractingItem;

	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06002830 RID: 10288 RVA: 0x000BB128 File Offset: 0x000B9328
	// (remove) Token: 0x06002831 RID: 10289 RVA: 0x000BB160 File Offset: 0x000B9360
	public event Action<Inventory> OnEquipmentChanged;

	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06002832 RID: 10290 RVA: 0x000BB198 File Offset: 0x000B9398
	// (remove) Token: 0x06002833 RID: 10291 RVA: 0x000BB1D0 File Offset: 0x000B93D0
	public event Action OnCaretakerStateChanged;

	// Token: 0x17000677 RID: 1655
	// (get) Token: 0x06002834 RID: 10292 RVA: 0x000BB205 File Offset: 0x000B9405
	public ZombieType ZombieType
	{
		get
		{
			return this.zombieType;
		}
	}

	// Token: 0x17000678 RID: 1656
	// (get) Token: 0x06002835 RID: 10293 RVA: 0x000BB20D File Offset: 0x000B940D
	// (set) Token: 0x06002836 RID: 10294 RVA: 0x000BB215 File Offset: 0x000B9415
	public string Name
	{
		get
		{
			return this.name;
		}
		set
		{
			this.name = value;
		}
	}

	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06002837 RID: 10295 RVA: 0x000BB21E File Offset: 0x000B941E
	public Item ZombieItem
	{
		get
		{
			return this.zombieItem;
		}
	}

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06002838 RID: 10296 RVA: 0x000BB226 File Offset: 0x000B9426
	public WgoData AttachedWgoData
	{
		get
		{
			if (this.attachedWgoData == null)
			{
				this.attachedWgoData = MainGame.Instance.GameSave.WorldData.GetWgoData(this.attachedWgoDataUniqueId);
			}
			return this.attachedWgoData;
		}
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06002839 RID: 10297 RVA: 0x000BB258 File Offset: 0x000B9458
	public int WhiteSkulls
	{
		get
		{
			int num = 0;
			foreach (Item item in this.ZombieItem.Inventory)
			{
				num += item.Definition.whiteSkulls * item.Count;
			}
			return Mathf.Clamp(num, 0, 999);
		}
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x0600283A RID: 10298 RVA: 0x000BB2CC File Offset: 0x000B94CC
	public int RedSkulls
	{
		get
		{
			int num = 0;
			foreach (Item item in this.ZombieItem.Inventory)
			{
				num += item.Definition.redSkulls * item.Count;
			}
			return Mathf.Clamp(num, 0, 999);
		}
	}

	// Token: 0x0600283B RID: 10299 RVA: 0x000BB340 File Offset: 0x000B9540
	public ZombieWgoData()
	{
	}

	// Token: 0x0600283C RID: 10300 RVA: 0x000BB43C File Offset: 0x000B963C
	public ZombieWgoData(string id, Vector3 position, string worldId)
		: base(id, position, worldId)
	{
		foreach (TalentDef talentDef in GameBalance.Me.talentDefs)
		{
			ZombieTalentData zombieTalentData = new ZombieTalentData(talentDef.id);
			foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
			{
				if (talentLevelUpDef.isZombiePerk && talentLevelUpDef.availableAtStart && talentLevelUpDef.talentId == talentDef.id)
				{
					zombieTalentData.studiedLevelUps.Add(talentLevelUpDef.id);
					zombieTalentData.curTalentValue += talentLevelUpDef.talentValueAdd;
					if (!string.IsNullOrEmpty(talentLevelUpDef.linkedPerk))
					{
						base.AddPerk(talentLevelUpDef.linkedPerk);
					}
				}
			}
			this.talentData.Add(zombieTalentData);
		}
		this.RollName();
	}

	// Token: 0x0600283D RID: 10301 RVA: 0x000BB648 File Offset: 0x000B9848
	public ZombieWgoData CreateFighterFromThis(Vector3 position, string worldId)
	{
		ZombieWgoData zombieWgoData = new ZombieWgoData("zmb_wild_mob_allie", position, worldId);
		this.CopyNameTo(zombieWgoData);
		zombieWgoData.talentData = this.talentData;
		zombieWgoData.activePerks = this.activePerks;
		zombieWgoData.zombieItem = this.zombieItem;
		zombieWgoData.equippedHand = this.equippedHand;
		zombieWgoData.equippedArmor = this.equippedArmor;
		zombieWgoData.equippedCollar = this.equippedCollar;
		zombieWgoData.SetGameRes(this.gameRes);
		zombieWgoData.GameResStr.Set(base.GameResStr);
		zombieWgoData.techRed = this.techRed;
		zombieWgoData.techBlue = this.techBlue;
		zombieWgoData.techGreen = this.techGreen;
		return zombieWgoData;
	}

	// Token: 0x0600283E RID: 10302 RVA: 0x000BB6F4 File Offset: 0x000B98F4
	public ZombieWgoData CreateAssistantFromThis(Vector3 position, string worldId, Direction direction = Direction.Down)
	{
		ZombieWgoData zombieWgoData = new ZombieWgoData("zombie_assistant", position, worldId);
		this.zombieItem.UniqueId.SetGuid(zombieWgoData.UniqueId);
		this.CopyNameTo(zombieWgoData);
		zombieWgoData.talentData = this.talentData;
		zombieWgoData.activePerks = this.activePerks;
		zombieWgoData.zombieItem = this.zombieItem;
		zombieWgoData.equippedHand = this.equippedHand;
		zombieWgoData.equippedArmor = this.equippedArmor;
		zombieWgoData.equippedCollar = this.equippedCollar;
		zombieWgoData.WorkerGameRes.Set(this.WorkerGameRes);
		zombieWgoData.WorkerGameRes.Set("zombie_body_id", 1002f);
		zombieWgoData.GameResStr.Set(base.GameResStr);
		zombieWgoData.direction.Value = direction.ConvertToVector2XZ();
		zombieWgoData.techRed = this.techRed;
		zombieWgoData.techBlue = this.techBlue;
		zombieWgoData.techGreen = this.techGreen;
		return zombieWgoData;
	}

	// Token: 0x0600283F RID: 10303 RVA: 0x000BB7E0 File Offset: 0x000B99E0
	public ZombieWgoData CreateCommonZombieFromThis(Vector3 position, string worldId, Direction direction = Direction.Down)
	{
		ZombieWgoData zombieWgoData = new ZombieWgoData("zombie", position, worldId);
		this.zombieItem.UniqueId.SetGuid(zombieWgoData.UniqueId);
		this.CopyNameTo(zombieWgoData);
		zombieWgoData.talentData = this.talentData;
		zombieWgoData.activePerks = this.activePerks;
		zombieWgoData.zombieItem = this.zombieItem;
		zombieWgoData.equippedHand = this.equippedHand;
		zombieWgoData.equippedArmor = this.equippedArmor;
		zombieWgoData.equippedCollar = this.equippedCollar;
		this.WorkerGameRes.Set("zombie_body_id", 0f);
		this.WorkerGameRes.RemoveZeroValues();
		zombieWgoData.WorkerGameRes.Set(this.WorkerGameRes);
		zombieWgoData.GameResStr.Set(base.GameResStr);
		zombieWgoData.direction.Value = direction.ConvertToVector2XZ();
		zombieWgoData.techRed = this.techRed;
		zombieWgoData.techBlue = this.techBlue;
		zombieWgoData.techGreen = this.techGreen;
		return zombieWgoData;
	}

	// Token: 0x06002840 RID: 10304 RVA: 0x000BB8D7 File Offset: 0x000B9AD7
	public void SetName(string zombieName, bool nameRandomed)
	{
		MainGame.Instance.GameSave.knowledgeSystem.ReturnZombieName(this.name, this.nameRandomed);
		this.name = zombieName;
		this.nameRandomed = nameRandomed;
	}

	// Token: 0x06002841 RID: 10305 RVA: 0x000BB908 File Offset: 0x000B9B08
	public void RollName()
	{
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		string text = this.name;
		bool flag = this.nameRandomed;
		this.name = knowledgeSystem.GetZombieName(out this.nameRandomed);
		knowledgeSystem.ReturnZombieName(text, flag);
	}

	// Token: 0x06002842 RID: 10306 RVA: 0x000BB94D File Offset: 0x000B9B4D
	private void CopyNameTo(ZombieWgoData target)
	{
		target.SetName(this.name, this.nameRandomed);
	}

	// Token: 0x06002843 RID: 10307 RVA: 0x000BB964 File Offset: 0x000B9B64
	public override string ToString()
	{
		return string.Format("[Zombie: id={0}, uniqueId={1}, type={2}, name={3}, item={4}]", new object[] { this.id, base.UniqueId, this.zombieType, this.name, this.zombieItem });
	}

	// Token: 0x06002844 RID: 10308 RVA: 0x000BB9B3 File Offset: 0x000B9BB3
	public void PrepareForGameBase()
	{
		base.PrepareForGame();
	}

	// Token: 0x06002845 RID: 10309 RVA: 0x000BB9BC File Offset: 0x000B9BBC
	public override void PrepareForGame()
	{
		base.PrepareForGame();
		this.SyncTalentLevelUpsFromBalance();
		if (this.zombieType != ZombieType.Gardener)
		{
			this.GardenerStopWorkActivity(null, true);
		}
		if (this.AttachedWgoData != null)
		{
			switch (this.zombieType)
			{
			case ZombieType.Crafter:
				this.CrafterHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
				this.AttachedWgoData.TrySetWorker(this, null);
				this.AttachedWgoData.CraftComponent.OnStatusChanged += this.CrafterHandleCraftStatusChange;
				this.AttachedWgoData.CraftComponent.OnCraftCurProgressNormalizedChanged += this.CrafterHandleCraftProgressChange;
				base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
				base.WorldZoneData.OnOrderAdded += this.OnOrderAdded;
				this.AttachedWgoData.CraftComponent.OnCraftAddedToQueue += this.CrafterOnCraftAddedToQueue;
				this.AttachedWgoData.CraftComponent.OnCraftRemovedFromQueue += this.CrafterOnCraftRemovedFromQueue;
				if (this.AttachedWgoData.CraftComponent.CurrentCraftElement != null && this.AttachedWgoData.CraftComponent.CurrentCraftElement.IsStarted && this.ZombieCraftActivity == null)
				{
					this.currentActivity = MainGame.Instance.craftSystem.TryGetCraftActivity(this);
					if (this.currentActivity == null)
					{
						this.CrafterStartCraftActivity(false);
					}
					else
					{
						this.ZombieCraftActivity.OnActiveStateChanged += this.CrafterOnCraftActivityStateChanged;
						this.CrafterOnCraftActivityStateChanged();
						this.UpdateAttachedWgoViewWidgets();
					}
				}
				break;
			case ZombieType.Caretaker:
				this.AttachedWgoData.TrySetWorker(this, null);
				base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
				break;
			case ZombieType.ConveyorCrafter:
				this.ConveyorCrafterHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
				this.AttachedWgoData.TrySetWorker(this, null);
				this.AttachedWgoData.CraftComponent.OnStatusChanged += this.ConveyorCrafterHandleCraftStatusChange;
				this.AttachedWgoData.CraftComponent.OnStatusChanged += this.ConveyorHadleCraftStatusChange;
				this.AttachedWgoData.CraftComponent.OnCraftCurProgressNormalizedChanged += this.CrafterHandleCraftProgressChange;
				this.AttachedWgoData.CraftComponent.OnCraftAddedToQueue += this.ConveyorCrafterOnCraftAddedToQueue;
				this.AttachedWgoData.CraftComponent.OnCraftRemovedFromQueue += this.ConveyorCrafterOnCraftRemovedFromQueue;
				this.AttachedWgoData.CraftComponent.CraftableObject.CraftableObjectCraftInventory.OnItemsAdd += this.ConveyorCrafterTryStartCurrentCraft;
				if (this.AttachedWgoData.CraftComponent.CurrentCraftElement != null && this.AttachedWgoData.CraftComponent.CurrentCraftElement.IsStarted && this.ZombieCraftActivity == null)
				{
					this.currentActivity = MainGame.Instance.conveyorSystem.TryGetCraftActivity(this);
					if (this.currentActivity == null)
					{
						this.ConveyorCrafterStartCraftActivity(false);
					}
					else
					{
						this.ZombieCraftActivity.OnActiveStateChanged += this.ConveyorCrafterOnCraftActivityStateChanged;
						this.ConveyorCrafterOnCraftActivityStateChanged();
						this.UpdateAttachedWgoViewWidgets();
					}
				}
				else if (this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
				{
					this.ConveyorCrafterTryStartCurrentCraft(null);
				}
				break;
			case ZombieType.Porter:
				if (base.GetGameResInt("is_staying_at_porter_station") == 1)
				{
					base.IsInteractable = false;
					this.AttachedWgoData.SetTriggerToAnimator("with_zombie");
				}
				else
				{
					base.IsInteractable = true;
					this.AttachedWgoData.SetTriggerToAnimator("path_state");
					base.SetLayerWeightToAnimator(4, 1f);
				}
				break;
			case ZombieType.Gardener:
				this.AttachedWgoData.TrySetWorker(this, null);
				base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
				if (this.gardenerState == ZombieWgoData.ZombieGardenerState.PlantingSeeds && this.ZombieCraftActivity == null)
				{
					this.currentActivity = MainGame.Instance.craftSystem.TryGetCraftActivity(this);
					if (this.ZombieCraftActivity != null)
					{
						this.ZombieCraftActivity.OnActiveStateChanged += this.GardenerOnCraftActivityStateChanged;
						this.GardenerOnCraftActivityStateChanged();
					}
					else
					{
						this.GardenerTryStartOrderExecutionOrGoToStation();
					}
				}
				else if ((this.gardenerState == ZombieWgoData.ZombieGardenerState.GatheringPlants || this.gardenerState == ZombieWgoData.ZombieGardenerState.WaitingForWgoDeath) && this.ZombieHPActivity == null)
				{
					this.currentActivity = MainGame.Instance.craftSystem.TryGetHPActivity(this);
					if (this.ZombieHPActivity != null)
					{
						this.ZombieHPActivity.OnActiveStateChanged += this.GardenerOnWorkActivityStateChanged;
						this.GardenerOnWorkActivityStateChanged();
					}
					else
					{
						this.GardenerTryStartOrderExecutionOrGoToStation();
					}
				}
				break;
			case ZombieType.ConveyorTransporter:
				this.AttachedWgoData.TrySetWorker(this, null);
				base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
				break;
			}
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, false);
		}
	}

	// Token: 0x06002846 RID: 10310 RVA: 0x000BBE81 File Offset: 0x000BA081
	public override void DeInit()
	{
		base.DeInit();
		if (this.AttachedWgoData != null)
		{
			this.AttachedWgoData.CraftComponent.OnStatusChanged -= this.CrafterHandleCraftStatusChange;
		}
	}

	// Token: 0x06002847 RID: 10311 RVA: 0x000BBEAD File Offset: 0x000BA0AD
	public void AttachToPowerSourceWgoData(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.Worker;
		this.CaretakerState = ZombieWgoData.ZombieCaretakerState.OnStation;
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
	}

	// Token: 0x06002848 RID: 10312 RVA: 0x000BBEE8 File Offset: 0x000BA0E8
	public void AttachToStationWgoData(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.Caretaker;
		this.CaretakerState = ZombieWgoData.ZombieCaretakerState.OnStation;
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
		base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
		ZombieDeliveryIndication.RedrawCrafterWorkbenchesInZone(this.AttachedWgoData.WorldZoneData);
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x000BBF54 File Offset: 0x000BA154
	public void AttachToGardenStationWgoData(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.Gardener;
		this.GardenerState = ZombieWgoData.ZombieGardenerState.OnStation;
		this.gardenerStation.SetGuid(uniqueId);
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
		base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
	}

	// Token: 0x0600284A RID: 10314 RVA: 0x000BBFBC File Offset: 0x000BA1BC
	public void AttachToConveyorTransporterStationWgoData(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.ConveyorTransporter;
		this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.OnStation;
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
		base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
	}

	// Token: 0x0600284B RID: 10315 RVA: 0x000BC018 File Offset: 0x000BA218
	public void AttachToCraftWgoData(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.Crafter;
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.CrafterHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
		this.AttachedWgoData.CraftComponent.OnStatusChanged += this.CrafterHandleCraftStatusChange;
		this.AttachedWgoData.CraftComponent.OnCraftCurProgressNormalizedChanged += this.CrafterHandleCraftProgressChange;
		base.WorldZoneData.OnOrderRemoved += this.OnOrderRemoved;
		base.WorldZoneData.OnOrderAdded += this.OnOrderAdded;
		this.AttachedWgoData.CraftComponent.OnCraftAddedToQueue += this.CrafterOnCraftAddedToQueue;
		this.AttachedWgoData.CraftComponent.OnCraftRemovedFromQueue += this.CrafterOnCraftRemovedFromQueue;
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement != null && this.AttachedWgoData.CraftComponent.CurrentCraftElement.IsStarted && this.ZombieCraftActivity == null)
		{
			this.CrafterStartCraftActivity(false);
			return;
		}
		if (this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
		{
			this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
		}
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x000BC15C File Offset: 0x000BA35C
	public void AttachToConveyorCraftWgoData(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.ConveyorCrafter;
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.ConveyorCrafterHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
		this.AttachedWgoData.CraftComponent.OnStatusChanged += this.ConveyorCrafterHandleCraftStatusChange;
		this.AttachedWgoData.CraftComponent.OnStatusChanged += this.ConveyorHadleCraftStatusChange;
		this.AttachedWgoData.CraftComponent.OnCraftCurProgressNormalizedChanged += this.CrafterHandleCraftProgressChange;
		this.AttachedWgoData.CraftComponent.OnCraftAddedToQueue += this.ConveyorCrafterOnCraftAddedToQueue;
		this.AttachedWgoData.CraftComponent.OnCraftRemovedFromQueue += this.ConveyorCrafterOnCraftRemovedFromQueue;
		this.AttachedWgoData.CraftComponent.CraftableObject.CraftableObjectCraftInventory.OnItemsAdd += this.ConveyorCrafterTryStartCurrentCraft;
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement != null && this.AttachedWgoData.CraftComponent.CurrentCraftElement.IsStarted && this.ZombieCraftActivity == null)
		{
			this.ConveyorCrafterStartCraftActivity(false);
			return;
		}
		if (this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
		{
			this.ConveyorCrafterTryStartCurrentCraft(null);
		}
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x000BC2B4 File Offset: 0x000BA4B4
	public void AttachToPorterStation(SGuid uniqueId, Item zombie, DockPointData dockPointData = null)
	{
		this.zombieType = ZombieType.Porter;
		this.attachedWgoDataUniqueId.SetGuid(uniqueId);
		this.attachedWgoData = null;
		this.zombieItem = zombie;
		this.AttachedWgoData.TrySetWorker(this, dockPointData);
		base.SetGameRes("is_staying_at_porter_station", 1);
		this.porterInventory = Inventory.Create(4, false, null, null, "");
		this.AttachedWgoData.SetTriggerToAnimator("with_zombie");
		base.IsInteractable = false;
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x000BC327 File Offset: 0x000BA527
	public void AttachToFightersContainer()
	{
		this.zombieType = ZombieType.Fighter;
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x000BC330 File Offset: 0x000BA530
	public void UnAttachFromWgoData(bool dropPorterInventoryNearPlayer = false)
	{
		this.SetDefaultAnimState();
		if (!this.AttachedWgoData.CraftComponent.IsDestroyingCraftActive && this.AttachedWgoData.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp)
		{
			this.AttachedWgoData.CraftComponent.TryFinishCurCraft();
		}
		WorldZoneData worldZoneData = null;
		switch (this.zombieType)
		{
		case ZombieType.Free:
		case ZombieType.Worker:
		case ZombieType.Fighter:
			break;
		case ZombieType.Crafter:
			this.CrafterDropCraftInventory();
			this.AttachedWgoData.CraftComponent.OnStatusChanged -= this.CrafterHandleCraftStatusChange;
			this.AttachedWgoData.CraftComponent.OnCraftCurProgressNormalizedChanged -= this.CrafterHandleCraftProgressChange;
			this.AttachedWgoData.CraftComponent.OnCraftAddedToQueue -= this.CrafterOnCraftAddedToQueue;
			this.AttachedWgoData.CraftComponent.OnCraftRemovedFromQueue -= this.CrafterOnCraftRemovedFromQueue;
			base.WorldZoneData.ClearOrders(new List<SGuid>(this.crafterOrders));
			this.CrafterStopCraftActivity();
			this.crafterOrderedCraftId = string.Empty;
			base.WorldZoneData.OnOrderRemoved -= this.OnOrderRemoved;
			base.WorldZoneData.OnOrderAdded -= this.OnOrderAdded;
			break;
		case ZombieType.Caretaker:
			this.CaretakerTryStopOrderExecution();
			this.caretakerCurrentMovementTargetUniqueId = SGuid.Empty;
			if (!this.CaretakerPortableItem.IsEmpty)
			{
				MainGame.Instance.dropSystem.DropItem(this.CaretakerPortableItem, base.WorldId, base.Position, null);
				this.CaretakerPortableItem = Item.Empty;
			}
			worldZoneData = this.AttachedWgoData.WorldZoneData;
			break;
		case ZombieType.ConveyorCrafter:
			this.AttachedWgoData.CraftComponent.OnStatusChanged -= this.ConveyorCrafterHandleCraftStatusChange;
			this.AttachedWgoData.CraftComponent.OnStatusChanged -= this.ConveyorHadleCraftStatusChange;
			this.AttachedWgoData.CraftComponent.OnCraftCurProgressNormalizedChanged -= this.CrafterHandleCraftProgressChange;
			this.AttachedWgoData.CraftComponent.OnCraftAddedToQueue -= this.ConveyorCrafterOnCraftAddedToQueue;
			this.AttachedWgoData.CraftComponent.OnCraftRemovedFromQueue -= this.ConveyorCrafterOnCraftRemovedFromQueue;
			this.AttachedWgoData.CraftComponent.CraftableObject.CraftableObjectCraftInventory.OnItemsAdd -= this.ConveyorCrafterTryStartCurrentCraft;
			this.ConveyorCrafterStopCraftActivity();
			break;
		case ZombieType.Porter:
		{
			base.SetGameRes("is_staying_at_porter_station", 0);
			string text = base.WorldId;
			Vector3 vector = base.Position;
			if (dropPorterInventoryNearPlayer)
			{
				PlayerData playerData = MainGame.PlayerData;
				Vector2 vector2 = playerData.Direction * 1f;
				text = playerData.currentGameSceneId;
				vector = playerData.position.Value + new Vector3(vector2.x, 0f, vector2.y);
			}
			foreach (Item item in this.porterInventory.Data.Inventory)
			{
				if (!(item.id == "fake_porter_slot_filler"))
				{
					MainGame.Instance.dropSystem.DropItem(item, text, vector, null);
				}
			}
			this.porterInventory = null;
			this.AttachedWgoData.SetTriggerToAnimator("backpack_anim");
			base.IsInteractable = true;
			break;
		}
		case ZombieType.Gardener:
			this.GardenerStopWorkActivity(null, true);
			this.GardenerStopCraftActivity(true);
			this.GardenerTryStopOrderExecution();
			this.gardenerCurrentMovementTargetUniqueId = SGuid.Empty;
			if (!this.GardenerPortableItem.IsEmpty)
			{
				MainGame.Instance.dropSystem.DropItem(this.GardenerPortableItem, base.WorldId, base.Position, null);
				this.GardenerPortableItem = Item.Empty;
			}
			this.GardenerClearBedWorkerIfMe(this.AttachedWgoData);
			this.attachedWgoData = null;
			this.attachedWgoDataUniqueId.SetGuid(this.gardenerStation);
			this.gardenerStation = SGuid.Empty;
			base.WorldZoneData.OnOrderRemoved -= this.OnOrderRemoved;
			break;
		case ZombieType.ConveyorTransporter:
			this.ConveyorTransporterTryStopOrderExecution();
			this.conveyorTransporterCurrentMovementTargetUniqueId = SGuid.Empty;
			if (!this.ConveyorTransporterPortableItem.IsEmpty)
			{
				MainGame.Instance.dropSystem.DropItem(this.ConveyorTransporterPortableItem, base.WorldId, base.Position, null);
				this.ConveyorTransporterPortableItem = Item.Empty;
			}
			base.WorldZoneData.OnOrderRemoved -= this.OnOrderRemoved;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		this.AttachedWgoData.ClearWorker();
		this.AttachedWgoData.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
		this.UpdateAttachedWgoViewWidgets();
		this.attachedWgoDataUniqueId = SGuid.Empty;
		this.attachedWgoData = null;
		this.takenDockPointsParentSGuid = null;
		this.zombieType = ZombieType.Free;
		base.MovementComponent.ForceStop();
		if (worldZoneData != null)
		{
			ZombieDeliveryIndication.RedrawCrafterWorkbenchesInZone(worldZoneData);
		}
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x000BC800 File Offset: 0x000BAA00
	public void SetDefaultAnimState()
	{
		this.curAnimState = global::AnimationState.Idle;
		Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
		if (onAnimationStateChanged == null)
		{
			return;
		}
		onAnimationStateChanged(this.curAnimState, false);
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x000BC820 File Offset: 0x000BAA20
	public override void OnReleaseWgoPartToPool()
	{
		base.OnReleaseWgoPartToPool();
		Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
		if (onAnimationStateChanged == null)
		{
			return;
		}
		onAnimationStateChanged(global::AnimationState.Idle, false);
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x000BC83A File Offset: 0x000BAA3A
	public void SetZombieItem(Item zombie)
	{
		this.zombieItem = zombie;
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x000BC843 File Offset: 0x000BAA43
	public override MultiInventory GetCraftableMultiInventory(bool excludeWorkerInventory = false)
	{
		return new MultiInventory(this.WorkerInventory);
	}

	// Token: 0x06002854 RID: 10324 RVA: 0x000BC850 File Offset: 0x000BAA50
	public void CustomUpdate(float deltaTime)
	{
		if (this.zombieType == ZombieType.Caretaker)
		{
			this.CaretakerUpdateBehaviour(deltaTime);
		}
		if (this.zombieType == ZombieType.Gardener)
		{
			this.GardenerUpdateBehaviour(deltaTime);
		}
		if (this.zombieType == ZombieType.ConveyorTransporter)
		{
			this.ConveyorTransporterUpdateBehaviour(deltaTime);
		}
	}

	// Token: 0x06002855 RID: 10325 RVA: 0x000BC882 File Offset: 0x000BAA82
	private void OnOrderAdded(OrderBase order)
	{
		if (this.zombieType == ZombieType.Crafter)
		{
			this.CrafterOnOrderAdded(order);
		}
	}

	// Token: 0x06002856 RID: 10326 RVA: 0x000BC894 File Offset: 0x000BAA94
	private void OnOrderRemoved(OrderBase order)
	{
		switch (this.zombieType)
		{
		case ZombieType.Crafter:
			this.CrafterOnOrderRemoved(order);
			return;
		case ZombieType.Caretaker:
			this.CaretakerOnOrderRemoved(order);
			return;
		case ZombieType.ConveyorCrafter:
		case ZombieType.Worker:
		case ZombieType.Porter:
			break;
		case ZombieType.Gardener:
			this.GardenerOnOrderRemoved(order);
			return;
		case ZombieType.ConveyorTransporter:
			this.ConveyorTransporterOnOrderRemoved(order);
			break;
		default:
			return;
		}
	}

	// Token: 0x06002857 RID: 10327 RVA: 0x000BC8EC File Offset: 0x000BAAEC
	private void UpdateAttachedWgoViewWidgets()
	{
		if (this.attachedWgoDataUniqueId.IsEmpty || this.AttachedWgoData == null)
		{
			return;
		}
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(this.AttachedWgoData.UniqueId);
		if (wgoViewGlobal != null)
		{
			wgoViewGlobal.DrawWidgets();
		}
	}

	// Token: 0x06002858 RID: 10328 RVA: 0x000BC930 File Offset: 0x000BAB30
	private void UpdateOwnWgoViewWidgets()
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(base.UniqueId);
		if (wgoViewGlobal != null)
		{
			wgoViewGlobal.DrawWidgets();
		}
	}

	// Token: 0x1700067D RID: 1661
	// (get) Token: 0x06002859 RID: 10329 RVA: 0x000BC958 File Offset: 0x000BAB58
	public SGuid Id
	{
		get
		{
			return base.UniqueId;
		}
	}

	// Token: 0x1700067E RID: 1662
	// (get) Token: 0x0600285A RID: 10330 RVA: 0x000BC960 File Offset: 0x000BAB60
	public IWorkActivity WorkerActivity
	{
		get
		{
			return this.currentActivity;
		}
	}

	// Token: 0x1700067F RID: 1663
	// (get) Token: 0x0600285B RID: 10331 RVA: 0x000BC968 File Offset: 0x000BAB68
	public MultiInventory WorkerMultiInventory
	{
		get
		{
			return new MultiInventory(base.Inventory);
		}
	}

	// Token: 0x17000680 RID: 1664
	// (get) Token: 0x0600285C RID: 10332 RVA: 0x000BC975 File Offset: 0x000BAB75
	public Inventory WorkerInventory
	{
		get
		{
			if (this.zombieType == ZombieType.Gardener)
			{
				return new Inventory(this.zombieItem);
			}
			return this.AttachedWgoData.CraftableObjectCraftInventory;
		}
	}

	// Token: 0x17000681 RID: 1665
	// (get) Token: 0x0600285D RID: 10333 RVA: 0x000BC997 File Offset: 0x000BAB97
	public override Inventory CraftableObjectCraftInventory
	{
		get
		{
			return this.AttachedWgoData.CraftableObjectCraftInventory;
		}
	}

	// Token: 0x17000682 RID: 1666
	// (get) Token: 0x0600285E RID: 10334 RVA: 0x000BC9A4 File Offset: 0x000BABA4
	public Inventory WorkerToolInventory
	{
		get
		{
			Inventory inventory = new Inventory("toolBeltInventory", 2);
			if (!this.equippedHand.IsEmpty)
			{
				inventory.AddItemToInventory(new Item(this.Hand.id, 1), null, false);
			}
			if (!this.equippedArmor.IsEmpty)
			{
				inventory.AddItemToInventory(new Item(this.Armor.id, 1), null, false);
			}
			return inventory;
		}
	}

	// Token: 0x17000683 RID: 1667
	// (get) Token: 0x0600285F RID: 10335 RVA: 0x000BCA0C File Offset: 0x000BAC0C
	public GameRes WorkerGameRes
	{
		get
		{
			return this.gameRes;
		}
	}

	// Token: 0x17000684 RID: 1668
	// (get) Token: 0x06002860 RID: 10336 RVA: 0x000BCA14 File Offset: 0x000BAC14
	public Item Collar
	{
		get
		{
			Item item;
			this.ZombieItem.TryGetItemInInventoryByGUID(this.equippedCollar.Id, out item);
			return item;
		}
	}

	// Token: 0x17000685 RID: 1669
	// (get) Token: 0x06002861 RID: 10337 RVA: 0x000BCA3C File Offset: 0x000BAC3C
	public Item Hand
	{
		get
		{
			Item item;
			this.ZombieItem.TryGetItemInInventoryByGUID(this.equippedHand.Id, out item);
			return item;
		}
	}

	// Token: 0x17000686 RID: 1670
	// (get) Token: 0x06002862 RID: 10338 RVA: 0x000BCA64 File Offset: 0x000BAC64
	public int AttackValue
	{
		get
		{
			Item hand = this.Hand;
			if (hand.IsEmpty)
			{
				return 0;
			}
			if (hand.Definition.type == ItemType.Pike)
			{
				return hand.Definition.damage.EvaluateInt();
			}
			if (hand.Definition.type == ItemType.Bow)
			{
				return hand.Definition.damage.EvaluateInt();
			}
			return 0;
		}
	}

	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06002863 RID: 10339 RVA: 0x000BCAC4 File Offset: 0x000BACC4
	public Item Armor
	{
		get
		{
			Item item;
			this.ZombieItem.TryGetItemInInventoryByGUID(this.equippedArmor.Id, out item);
			return item;
		}
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06002864 RID: 10340 RVA: 0x000BCAEC File Offset: 0x000BACEC
	public int ArmorValue
	{
		get
		{
			Item armor = this.Armor;
			if (!armor.IsEmpty)
			{
				return armor.Definition.quality;
			}
			return 0;
		}
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x000BCB18 File Offset: 0x000BAD18
	public int GetMasteryLevelForTalentBranch(string talentId, CraftDefBase craftDef = null)
	{
		int num = 0;
		foreach (Item item in this.WorkerToolInventory.Data.Inventory)
		{
			if (item.Definition.talentIds.Contains(talentId))
			{
				num += item.Definition.talentBonus;
			}
		}
		int perksCraftMasteryBonusValue = this.GetPerksCraftMasteryBonusValue(craftDef);
		return this.GetTalentBranch(talentId).curTalentValue + num + perksCraftMasteryBonusValue;
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x000BCBAC File Offset: 0x000BADAC
	public bool HasToolForWork(WgoData wgoData, CraftDefBase craftDef)
	{
		ItemType itemType = ItemType.None;
		if (craftDef != null)
		{
			CraftDef craftDef2 = craftDef as CraftDef;
			if (craftDef2 != null && craftDef2.customItemTypeAction != ItemType.None && !craftDef.isAuto)
			{
				itemType = craftDef2.customItemTypeAction;
			}
		}
		if (itemType == ItemType.None)
		{
			itemType = wgoData.Definition.toolAction.actionableTool;
		}
		return itemType == ItemType.None || itemType == ItemType.Hand || !this.WorkerToolInventory.Data.GetItemByType(itemType).IsEmpty;
	}

	// Token: 0x06002867 RID: 10343 RVA: 0x000BCC1C File Offset: 0x000BAE1C
	public bool HasToolForWork(WgoData wgoData, out ItemType possibleTool)
	{
		possibleTool = ItemType.None;
		foreach (CraftDefBase craftDefBase in wgoData.CraftComponent.AvailableCrafts)
		{
			CraftDef craftDef = craftDefBase as CraftDef;
			if (craftDef != null && craftDef.customItemTypeAction != ItemType.None && !craftDefBase.isAuto)
			{
				possibleTool = craftDef.customItemTypeAction;
				break;
			}
		}
		if (possibleTool == ItemType.None)
		{
			possibleTool = wgoData.Definition.toolAction.actionableTool;
		}
		return possibleTool == ItemType.None || !this.WorkerToolInventory.Data.GetItemByType(possibleTool).IsEmpty;
	}

	// Token: 0x06002868 RID: 10344 RVA: 0x000BCCCC File Offset: 0x000BAECC
	public int GetPerksCraftMasteryBonusValue(CraftDefBase craftDef)
	{
		int num = 0;
		if (craftDef == null)
		{
			return 0;
		}
		foreach (string text in craftDef.linkedPerks)
		{
			if (base.HasPerk(text))
			{
				num += GameBalance.Me.GetData<PerkDef>(text).craftMasteryBonus;
			}
		}
		return num;
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x000BCD3C File Offset: 0x000BAF3C
	public int GetPerksCraftStartTicksBonusValue(CraftDefBase craftDef)
	{
		int num = 0;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = this.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.craftStartTicks;
				}
			}
		}
		return num;
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x000BCDC0 File Offset: 0x000BAFC0
	public int GetPerksCraftAddTotalProgressTicksValue(CraftDefBase craftDef)
	{
		int num = 0;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = this.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.craftTotalProgressTicksBonus;
				}
			}
		}
		return num;
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x000BCE44 File Offset: 0x000BB044
	public bool CanRemoveItemFromBody(Item item)
	{
		int num = this.RedSkulls - item.Definition.redSkulls;
		int num2 = this.WhiteSkulls - item.Definition.whiteSkulls;
		return this.SkullsInCollarBorders(num, num2);
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x000BCE80 File Offset: 0x000BB080
	public bool CanAddItemToBody(Item item)
	{
		int num = this.RedSkulls + item.Definition.redSkulls;
		int num2 = this.WhiteSkulls + item.Definition.whiteSkulls;
		return this.SkullsInCollarBorders(num, num2);
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x000BCEBC File Offset: 0x000BB0BC
	public bool CanChangeItemInBody(Item itemFrom, Item itemTo)
	{
		int num = this.RedSkulls + itemTo.Definition.redSkulls - itemFrom.Definition.redSkulls;
		int num2 = this.WhiteSkulls + itemTo.Definition.whiteSkulls - itemFrom.Definition.whiteSkulls;
		return this.SkullsInCollarBorders(num, num2);
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x000BCF10 File Offset: 0x000BB110
	public bool CanUpgradeCollarTo(Item item)
	{
		if (item == null || item.IsEmpty || item.Definition.type != ItemType.Collar)
		{
			return false;
		}
		Item collar = this.Collar;
		return collar != null && !collar.IsEmpty && item.Definition.redSkullsMaxCollar > collar.Definition.redSkullsMaxCollar && item.Definition.SkullsInBorders(this.WhiteSkulls, this.RedSkulls);
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x000BCF80 File Offset: 0x000BB180
	private bool SkullsInCollarBorders(int red, int white)
	{
		Item collar = this.Collar;
		return collar != null && !collar.IsEmpty && collar.Definition.SkullsInBorders(white, red);
	}

	// Token: 0x06002870 RID: 10352 RVA: 0x000BCFB0 File Offset: 0x000BB1B0
	public float GetPerksEnergyBonusValue(CraftDefBase craftDef)
	{
		float num = 0f;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = this.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.energyAdd;
				}
			}
		}
		return num;
	}

	// Token: 0x06002871 RID: 10353 RVA: 0x000BD038 File Offset: 0x000BB238
	public float GetPerksInsanityBonusValue(CraftDefBase craftDef)
	{
		float num = 0f;
		using (List<string>.Enumerator enumerator = craftDef.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string linkedPerk = enumerator.Current;
				PerkData perkData = this.activePerks.Find((PerkData x) => x.id == linkedPerk);
				if (perkData != null)
				{
					num += perkData.Definition.insanityAdd;
				}
			}
		}
		return num;
	}

	// Token: 0x06002872 RID: 10354 RVA: 0x000BD0C0 File Offset: 0x000BB2C0
	public Item GetToolForWorkOnCraft(WgoData wgoData, CraftDefBase craftDef)
	{
		ItemType itemType = ItemType.None;
		CraftDef craftDef2 = craftDef as CraftDef;
		if (craftDef2 != null && craftDef2.customItemTypeAction != ItemType.None && !craftDef.isAuto)
		{
			itemType = craftDef2.customItemTypeAction;
		}
		if (itemType == ItemType.None)
		{
			itemType = wgoData.Definition.toolAction.actionableTool;
		}
		if (itemType == ItemType.Hand)
		{
			return new Item("hand_tool", 1);
		}
		foreach (Item item in this.WorkerToolInventory.Data.Inventory)
		{
			if (item.Definition.type == itemType)
			{
				return item;
			}
		}
		return Item.Empty;
	}

	// Token: 0x06002873 RID: 10355 RVA: 0x000BD17C File Offset: 0x000BB37C
	public CraftStatus CheckWorkerDependentValues(CraftElement craftElement, float deltaTime = 1f, bool skipEnergyCheck = false, bool skipInsanityCheck = false)
	{
		CraftDef definition = craftElement.Definition;
		if (!craftElement.ParamsData.HasRequiredTool)
		{
			return CraftStatus.DoesntHaveRequiredTool;
		}
		if (!definition.isStarCraft && !definition.isAutopsyCraft && craftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.Common && craftElement.ParamsData.MasteryValue < craftElement.ParamsData.MasteryLock)
		{
			return CraftStatus.NotEnoughMastery;
		}
		if ((definition.isStarCraft || definition.isAutopsyCraft) && craftElement.ParamsData.MasteryValue <= 0)
		{
			return CraftStatus.NotEnoughMastery;
		}
		if (craftElement.ParamsData.craftParamsType == CraftParamsData.CraftParamsType.GardenPlanting && craftElement.ParamsData.MasteryValue <= 0)
		{
			return CraftStatus.NotEnoughMastery;
		}
		return CraftStatus.OK;
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x000BD218 File Offset: 0x000BB418
	public void AddRes(string type, float value)
	{
		base.AddGameRes(type, value);
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x000BD222 File Offset: 0x000BB422
	public void MultiplyRes(string type, float value)
	{
		base.MultiplyGameRes(type, value);
	}

	// Token: 0x06002876 RID: 10358 RVA: 0x000BD22C File Offset: 0x000BB42C
	public void SetRes(string stype, float value)
	{
		base.SetGameRes(stype, value);
	}

	// Token: 0x06002877 RID: 10359 RVA: 0x000BD236 File Offset: 0x000BB436
	public float GetRes(string stype, float defaultValue = 0f)
	{
		return base.GetGameRes(stype);
	}

	// Token: 0x14000096 RID: 150
	// (add) Token: 0x06002878 RID: 10360 RVA: 0x000BD240 File Offset: 0x000BB440
	// (remove) Token: 0x06002879 RID: 10361 RVA: 0x000BD274 File Offset: 0x000BB474
	public static event Action<WgoData, ZombieWgoData, string, int> OnTechPointsAddedToZombie;

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x0600287A RID: 10362 RVA: 0x000BD2A8 File Offset: 0x000BB4A8
	// (remove) Token: 0x0600287B RID: 10363 RVA: 0x000BD2DC File Offset: 0x000BB4DC
	public static event Action OnTalentLevelUpPurchased;

	// Token: 0x0600287C RID: 10364 RVA: 0x000BD310 File Offset: 0x000BB510
	public void DoTechPointsReward(WgoData from, int r, int g, int b)
	{
		if (r > 0)
		{
			this.techRed += r;
			Action<WgoData, ZombieWgoData, string, int> onTechPointsAddedToZombie = ZombieWgoData.OnTechPointsAddedToZombie;
			if (onTechPointsAddedToZombie != null)
			{
				onTechPointsAddedToZombie(from, this, "tech_red", r);
			}
		}
		if (g > 0)
		{
			this.techGreen += g;
			Action<WgoData, ZombieWgoData, string, int> onTechPointsAddedToZombie2 = ZombieWgoData.OnTechPointsAddedToZombie;
			if (onTechPointsAddedToZombie2 != null)
			{
				onTechPointsAddedToZombie2(from, this, "tech_green", g);
			}
		}
		if (b > 0)
		{
			this.techBlue += b;
			Action<WgoData, ZombieWgoData, string, int> onTechPointsAddedToZombie3 = ZombieWgoData.OnTechPointsAddedToZombie;
			if (onTechPointsAddedToZombie3 == null)
			{
				return;
			}
			onTechPointsAddedToZombie3(from, this, "tech_blue", b);
		}
	}

	// Token: 0x0600287D RID: 10365 RVA: 0x000BD3A0 File Offset: 0x000BB5A0
	public ZombieTalentData GetTalentBranch(string talentId)
	{
		return this.talentData.Find((ZombieTalentData x) => x.id == talentId);
	}

	// Token: 0x0600287E RID: 10366 RVA: 0x000BD3D4 File Offset: 0x000BB5D4
	public bool IsTalentLevelUpStudied(string id)
	{
		using (List<ZombieTalentData>.Enumerator enumerator = this.talentData.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.studiedLevelUps.Contains(id))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000BD434 File Offset: 0x000BB634
	public void OnAddOrgan(Item item)
	{
		if (!string.IsNullOrEmpty(item.Definition.bodyLinkedPerk))
		{
			base.AddPerk(item.Definition.bodyLinkedPerk);
		}
		if (item.Definition.redSkulls < 0)
		{
			this.CheckRedSkulls(false);
		}
		if (item.Definition.redSkulls > 0)
		{
			this.CheckRedSkulls(true);
		}
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x000BD490 File Offset: 0x000BB690
	public void OnRemoveOrgan(Item item)
	{
		if (!string.IsNullOrEmpty(item.Definition.bodyLinkedPerk))
		{
			base.RemovePerk(item.Definition.bodyLinkedPerk);
		}
		if (item.Definition.redSkulls < 0)
		{
			this.CheckRedSkulls(true);
		}
		if (item.Definition.redSkulls > 0)
		{
			this.CheckRedSkulls(false);
		}
	}

	// Token: 0x06002881 RID: 10369 RVA: 0x000BD4EC File Offset: 0x000BB6EC
	public void CheckRedSkulls(bool addRedSkulls)
	{
		int usedPerksCount = this.GetUsedPerksCount();
		int redSkulls = this.RedSkulls;
		if (addRedSkulls)
		{
			if (this.disabledTalentLevelUps.Count <= 0)
			{
				return;
			}
			int num = redSkulls + this.disabledTalentLevelUps.Count - usedPerksCount;
			using (List<ZombieTalentData>.Enumerator enumerator = this.talentData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ZombieTalentData zombieTalentData = enumerator.Current;
					if (num <= 0)
					{
						break;
					}
					int num2 = 0;
					while (num2 < zombieTalentData.studiedLevelUps.Count && num > 0)
					{
						if (this.disabledTalentLevelUps.Contains(zombieTalentData.studiedLevelUps[num2]))
						{
							this.disabledTalentLevelUps.Remove(zombieTalentData.studiedLevelUps[num2]);
							TalentLevelUpDef data = GameBalance.Me.GetData<TalentLevelUpDef>(zombieTalentData.studiedLevelUps[num2]);
							if (!string.IsNullOrEmpty(data.linkedPerk))
							{
								base.AddPerk(data.linkedPerk);
							}
							this.GetTalentBranch(data.talentId).curTalentValue += data.talentValueAdd;
							num--;
						}
						num2++;
					}
				}
				return;
			}
		}
		int num3 = usedPerksCount - redSkulls - this.disabledTalentLevelUps.Count;
		if (num3 > 0)
		{
			foreach (ZombieTalentData zombieTalentData2 in this.talentData)
			{
				if (num3 <= 0)
				{
					break;
				}
				int num4 = zombieTalentData2.studiedLevelUps.Count - 1;
				while (num4 >= 0 && num3 > 0)
				{
					if (!this.disabledTalentLevelUps.Contains(zombieTalentData2.studiedLevelUps[num4]))
					{
						this.disabledTalentLevelUps.Add(zombieTalentData2.studiedLevelUps[num4]);
						TalentLevelUpDef data2 = GameBalance.Me.GetData<TalentLevelUpDef>(zombieTalentData2.studiedLevelUps[num4]);
						if (!string.IsNullOrEmpty(data2.linkedPerk))
						{
							base.RemovePerk(data2.linkedPerk);
						}
						this.GetTalentBranch(data2.talentId).curTalentValue -= data2.talentValueAdd;
						num3--;
					}
					num4--;
				}
			}
		}
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x000BD758 File Offset: 0x000BB958
	public TalentLevelUpDef.State GetLevelUpState(TalentLevelUpDef def)
	{
		if (this.GetTalentBranch(def.talentId).studiedLevelUps.Contains(def.id))
		{
			return TalentLevelUpDef.State.Unlocked;
		}
		if (MainGame.Instance.GameSave.knowledgeSystem.hiddenTalentLevelUps.Contains(def.id))
		{
			return TalentLevelUpDef.State.Hidden;
		}
		if (MainGame.Instance.GameSave.knowledgeSystem.unknownTalentLevelUps.Contains(def.id))
		{
			return TalentLevelUpDef.State.Unknown;
		}
		if (this.IsParentsUnlockedForTalentLevelUp(def) && this.IsEnoughResourcesToBuyTalentLevelUp(def) && this.IsEnoughFreeSkullsToBuyTalentLevelUp(def))
		{
			return TalentLevelUpDef.State.Available;
		}
		return TalentLevelUpDef.State.Visible;
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x000BD7EC File Offset: 0x000BB9EC
	public bool IsParentsUnlockedForTalentLevelUp(TalentLevelUpDef def)
	{
		bool flag = false;
		ZombieTalentData talentBranch = this.GetTalentBranch(def.talentId);
		TalentLevelUpDef.LockType lockType = def.lockType;
		if (lockType != TalentLevelUpDef.LockType.All)
		{
			if (lockType != TalentLevelUpDef.LockType.Any)
			{
				goto IL_00A8;
			}
		}
		else
		{
			flag = true;
			using (List<string>.Enumerator enumerator = def.parents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string text = enumerator.Current;
					if (!talentBranch.studiedLevelUps.Contains(text))
					{
						flag = false;
						break;
					}
				}
				return flag;
			}
		}
		using (List<string>.Enumerator enumerator = def.parents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string text2 = enumerator.Current;
				if (talentBranch.studiedLevelUps.Contains(text2))
				{
					flag = true;
				}
			}
			return flag;
		}
		IL_00A8:
		throw new ArgumentOutOfRangeException();
	}

	// Token: 0x06002884 RID: 10372 RVA: 0x000BD8C4 File Offset: 0x000BBAC4
	public bool IsEnoughResourcesToBuyTalentLevelUp(TalentLevelUpDef def)
	{
		if (def.techRed > 0)
		{
			return this.techRed >= def.techRed;
		}
		if (def.techBlue > 0)
		{
			return this.techBlue >= def.techBlue;
		}
		return def.techGreen <= 0 || this.techGreen >= def.techGreen;
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000BD924 File Offset: 0x000BBB24
	public bool IsEnoughFreeSkullsToBuyTalentLevelUp(TalentLevelUpDef def)
	{
		int usedPerksCount = this.GetUsedPerksCount();
		int redSkulls = this.RedSkulls;
		return usedPerksCount < redSkulls;
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000BD944 File Offset: 0x000BBB44
	public void PurchaseTalentLevelUp(TalentLevelUpDef def, bool free = false)
	{
		ZombieTalentData talentBranch = this.GetTalentBranch(def.talentId);
		talentBranch.studiedLevelUps.Add(def.id);
		talentBranch.curTalentValue += def.talentValueAdd;
		if (!string.IsNullOrEmpty(def.linkedPerk))
		{
			base.AddPerk(def.linkedPerk);
		}
		if (!free)
		{
			if (def.techRed > 0)
			{
				this.techRed -= def.techRed;
			}
			if (def.techBlue > 0)
			{
				this.techBlue -= def.techBlue;
			}
			if (def.techGreen > 0)
			{
				this.techGreen -= def.techGreen;
			}
		}
		Action onTalentLevelUpPurchased = ZombieWgoData.OnTalentLevelUpPurchased;
		if (onTalentLevelUpPurchased != null)
		{
			onTalentLevelUpPurchased();
		}
		if (this.zombieType == ZombieType.Crafter)
		{
			this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
		}
		foreach (LazyExpression lazyExpression in def.expressionsOnBuy)
		{
			lazyExpression.Evaluate();
		}
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000BDA54 File Offset: 0x000BBC54
	public int GetUsedPerksCount()
	{
		int num = 0;
		foreach (ZombieTalentData zombieTalentData in this.talentData)
		{
			num += zombieTalentData.studiedLevelUps.Count;
		}
		return num;
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000BDAB4 File Offset: 0x000BBCB4
	private void SyncTalentLevelUpsFromBalance()
	{
		ZombieWgoData.RemoveMissingTalentLevelUps(this.disabledTalentLevelUps);
		foreach (ZombieTalentData zombieTalentData in this.talentData)
		{
			ZombieWgoData.RemoveMissingTalentLevelUps(zombieTalentData.studiedLevelUps);
		}
		foreach (TalentLevelUpDef talentLevelUpDef in GameBalance.Me.talentLevelUpDefs)
		{
			if (talentLevelUpDef.isZombiePerk)
			{
				ZombieTalentData talentBranch = this.GetTalentBranch(talentLevelUpDef.talentId);
				if (talentBranch != null)
				{
					if (talentLevelUpDef.availableAtStart && !talentBranch.studiedLevelUps.Contains(talentLevelUpDef.id))
					{
						talentBranch.studiedLevelUps.Add(talentLevelUpDef.id);
						talentBranch.curTalentValue += talentLevelUpDef.talentValueAdd;
						if (this.GetUsedPerksCount() - this.disabledTalentLevelUps.Count > this.RedSkulls)
						{
							this.disabledTalentLevelUps.Add(talentLevelUpDef.id);
							talentBranch.curTalentValue -= talentLevelUpDef.talentValueAdd;
						}
					}
					if (talentBranch.studiedLevelUps.Contains(talentLevelUpDef.id) && !this.disabledTalentLevelUps.Contains(talentLevelUpDef.id) && !string.IsNullOrEmpty(talentLevelUpDef.linkedPerk) && !base.HasPerk(talentLevelUpDef.linkedPerk))
					{
						base.AddPerk(talentLevelUpDef.linkedPerk);
					}
				}
			}
		}
	}

	// Token: 0x06002889 RID: 10377 RVA: 0x000BDC5C File Offset: 0x000BBE5C
	private static void RemoveMissingTalentLevelUps(List<string> ids)
	{
		for (int i = ids.Count - 1; i >= 0; i--)
		{
			if (GameBalance.Me.GetData<TalentLevelUpDef>(ids[i]) == null)
			{
				ids.RemoveAt(i);
			}
		}
	}

	// Token: 0x14000098 RID: 152
	// (add) Token: 0x0600288A RID: 10378 RVA: 0x000BDC98 File Offset: 0x000BBE98
	// (remove) Token: 0x0600288B RID: 10379 RVA: 0x000BDCD0 File Offset: 0x000BBED0
	public event Action CrafterOnOrderAddedEvent;

	// Token: 0x14000099 RID: 153
	// (add) Token: 0x0600288C RID: 10380 RVA: 0x000BDD08 File Offset: 0x000BBF08
	// (remove) Token: 0x0600288D RID: 10381 RVA: 0x000BDD40 File Offset: 0x000BBF40
	public event Action CrafterOnOrderRemovedEvent;

	// Token: 0x17000689 RID: 1673
	// (get) Token: 0x0600288E RID: 10382 RVA: 0x000BDD75 File Offset: 0x000BBF75
	public List<SGuid> CrafterOrders
	{
		get
		{
			return this.crafterOrders;
		}
	}

	// Token: 0x1700068A RID: 1674
	// (get) Token: 0x0600288F RID: 10383 RVA: 0x000BDD7D File Offset: 0x000BBF7D
	public OrderBase CrafterCurrentOrder
	{
		get
		{
			if (this.CrafterOrders.Count <= 0)
			{
				return null;
			}
			return base.WorldZoneData.FindOrder(this.CrafterOrders[0]);
		}
	}

	// Token: 0x1700068B RID: 1675
	// (get) Token: 0x06002890 RID: 10384 RVA: 0x000BDDA6 File Offset: 0x000BBFA6
	private ZombieCraftActivity ZombieCraftActivity
	{
		get
		{
			return this.currentActivity as ZombieCraftActivity;
		}
	}

	// Token: 0x1700068C RID: 1676
	// (get) Token: 0x06002891 RID: 10385 RVA: 0x000BDDB3 File Offset: 0x000BBFB3
	private ZombieHPActivity ZombieHPActivity
	{
		get
		{
			return this.currentActivity as ZombieHPActivity;
		}
	}

	// Token: 0x06002892 RID: 10386 RVA: 0x000BDDC0 File Offset: 0x000BBFC0
	public void CrafterAddCraftDrop(Item drop)
	{
		PickupOrder pickupOrder = new PickupOrder(base.UniqueId, drop);
		base.WorldZoneData.PlaceNewOrder(pickupOrder);
		this.WorkerInventory.AddItemToInventory(drop, null, false);
		this.crafterOrderedCraftId = string.Empty;
	}

	// Token: 0x06002893 RID: 10387 RVA: 0x000BDE00 File Offset: 0x000BC000
	public void CrafterOnAttachedWgoCraftEnd(CraftElementBase ce)
	{
		this.CrafterStopCraftActivity();
		this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
	}

	// Token: 0x06002894 RID: 10388 RVA: 0x000BDE10 File Offset: 0x000BC010
	public void CrafterOnOrderExecuted(OrderBase order)
	{
		DeliveryOrder deliveryOrder = order as DeliveryOrder;
		if (deliveryOrder != null)
		{
			this.CrafterOnDeliveryOrderExecuted(deliveryOrder);
			return;
		}
		PickupOrder pickupOrder = order as PickupOrder;
		if (pickupOrder != null)
		{
			this.CrafterOnPickupOrderExecuted(pickupOrder);
			return;
		}
		throw new ArgumentOutOfRangeException("order", order, null);
	}

	// Token: 0x06002895 RID: 10389 RVA: 0x000BDE50 File Offset: 0x000BC050
	private void CrafterOnPickupOrderExecuted(PickupOrder pickupOrder)
	{
		foreach (SGuid sguid in this.crafterOrders)
		{
			if (pickupOrder.UniqueId != sguid)
			{
				return;
			}
		}
		this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
	}

	// Token: 0x06002896 RID: 10390 RVA: 0x000BDEB4 File Offset: 0x000BC0B4
	public void CrafterFinishAndContinueAfterBigItemDropped()
	{
		WgoData wgoData = this.AttachedWgoData;
		PickupOrder pickupOrder = new PickupOrder(base.UniqueId, Item.Empty);
		base.WorldZoneData.PlaceNewOrder(pickupOrder);
		CraftDefBase craftDefBase = null;
		GameRes gameRes = null;
		if (wgoData.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp)
		{
			if (wgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.GetInt("auto_start_same_craft_after_pickup") == 1)
			{
				craftDefBase = wgoData.CraftComponent.CurrentCraftElement.ParamsData.CraftDef;
				gameRes = wgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Clone();
			}
			wgoData.CraftComponent.TryFinishCurCraft();
			wgoData.DropStoredTechPoints();
		}
		base.WorldZoneData.RemoveOrder(pickupOrder.UniqueId);
		this.CrafterOnOrderExecuted(pickupOrder);
		if (craftDefBase != null)
		{
			CraftParamsData craftParamsData = new CraftParamsData(wgoData.CraftComponent.AvailableCrafts[0].id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
			if (gameRes != null)
			{
				craftParamsData.customRes = gameRes.Clone();
			}
			craftParamsData.customRes.Set("auto_start_same_craft_after_pickup", 1f);
			craftParamsData.customRes.Set("do_not_check_multiinventory_space", 1f);
			craftParamsData.customRes.Set("do_not_check_worker_dependent_values", 1f);
			wgoData.CraftComponent.TryStartCraft(new CraftElement(wgoData.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData));
			this.CrafterStopCraftActivity();
			this.CrafterStartCraftActivity(true);
		}
	}

	// Token: 0x06002897 RID: 10391 RVA: 0x000BE028 File Offset: 0x000BC228
	private void CrafterOnOrderAdded(OrderBase order)
	{
		if (order.TargetWgoUniqueId == base.UniqueId)
		{
			this.crafterOrders.Add(order.UniqueId);
			Action crafterOnOrderAddedEvent = this.CrafterOnOrderAddedEvent;
			if (crafterOnOrderAddedEvent != null)
			{
				crafterOnOrderAddedEvent();
			}
			this.UpdateAttachedWgoViewWidgets();
		}
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x000BE065 File Offset: 0x000BC265
	private void CrafterOnOrderRemoved(OrderBase order)
	{
		if (order.TargetWgoUniqueId == base.UniqueId)
		{
			this.crafterOrders.Remove(order.UniqueId);
			Action crafterOnOrderRemovedEvent = this.CrafterOnOrderRemovedEvent;
			if (crafterOnOrderRemovedEvent != null)
			{
				crafterOnOrderRemovedEvent();
			}
			this.UpdateAttachedWgoViewWidgets();
		}
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x000BE0A3 File Offset: 0x000BC2A3
	private void CrafterOnDeliveryOrderExecuted(DeliveryOrder deliveryOrder)
	{
		this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x000BE0AC File Offset: 0x000BC2AC
	private void CrafterDropCraftInventory()
	{
		foreach (Item item in this.WorkerInventory.Data.Inventory)
		{
			MainGame.Instance.dropSystem.DropItem(item, base.WorldId, base.Position, null);
		}
		this.WorkerInventory.Clear();
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x000BE0A3 File Offset: 0x000BC2A3
	private void CrafterOnCraftAddedToQueue(CraftElementBase craftQueueElement)
	{
		this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x000BE12C File Offset: 0x000BC32C
	private void CrafterOnCraftRemovedFromQueue(CraftElementBase craftQueueElement)
	{
		if (this.crafterOrderedCraftId == craftQueueElement.CraftId && craftQueueElement.Count == 0 && !craftQueueElement.IsFinished)
		{
			this.CrafterDropCraftInventory();
			this.crafterOrderedCraftId = string.Empty;
			base.WorldZoneData.ClearOrders(new List<SGuid>(this.crafterOrders));
			this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
		}
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement == null && !this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
		{
			this.CrafterStopCraftActivity();
		}
	}

	// Token: 0x0600289D RID: 10397 RVA: 0x000BE1B3 File Offset: 0x000BC3B3
	public void TryResumeCrafterWorkAfterLoad()
	{
		if (this.zombieType != ZombieType.Crafter || this.AttachedWgoData == null)
		{
			return;
		}
		if (this.ZombieCraftActivity != null)
		{
			return;
		}
		if (this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
		{
			this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
		}
	}

	// Token: 0x0600289E RID: 10398 RVA: 0x000BE1E8 File Offset: 0x000BC3E8
	private void CrafterTryPlaceOrderForCurrentCraftOrStartIt()
	{
		CraftElementBase currentCraftElement = this.AttachedWgoData.CraftComponent.CurrentCraftElement;
		if (currentCraftElement == null || currentCraftElement.IsStarted)
		{
			return;
		}
		if (this.crafterOrders.Count > 0)
		{
			return;
		}
		if (!this.CrafterCanUseTool(this.AttachedWgoData, currentCraftElement.Def))
		{
			return;
		}
		if (!this.CrafterIsEnoughMastery(this.AttachedWgoData))
		{
			return;
		}
		for (int i = 0; i < currentCraftElement.Requirements.Count; i++)
		{
			NeedItemData needItemData = currentCraftElement.Requirements[i];
			if (!GameBalance.Me.GetData<ItemDef>(needItemData.id).isFuel && !this.WorkerInventory.Data.HasItemQuantityInInventory(needItemData.id, needItemData.GetCount(this.AttachedWgoData)))
			{
				DeliveryOrder deliveryOrder = new DeliveryOrder(base.UniqueId, new Item(needItemData.id, needItemData.GetCount(this.AttachedWgoData)));
				base.WorldZoneData.PlaceNewOrder(deliveryOrder);
				this.crafterOrderedCraftId = currentCraftElement.CraftId;
				this.CrafterStopCraftActivity();
			}
		}
		if (this.CrafterCurrentOrder == null && this.ZombieCraftActivity == null)
		{
			this.CrafterStartCraftActivity(true);
			this.crafterOrderedCraftId = string.Empty;
		}
	}

	// Token: 0x0600289F RID: 10399 RVA: 0x000BE30C File Offset: 0x000BC50C
	public void CrafterStartCraftActivity(bool resetTicks = true)
	{
		ZombieCraftActivity zombieCraftActivity = new ZombieCraftActivity(this.AttachedWgoData, this);
		this.currentActivity = zombieCraftActivity;
		this.ZombieCraftActivity.OnActiveStateChanged += this.CrafterOnCraftActivityStateChanged;
		this.CrafterOnCraftActivityStateChanged();
		MainGame.Instance.craftSystem.AddWorker(zombieCraftActivity);
		this.UpdateAttachedWgoViewWidgets();
		if (resetTicks)
		{
			this.AttachedWgoData.CraftComponent.ZombieSubTicks = 0;
		}
		this.AttachedWgoData.CraftComponent.TryContinueFromQueue();
	}

	// Token: 0x060028A0 RID: 10400 RVA: 0x000BE388 File Offset: 0x000BC588
	public void CrafterStopCraftActivity()
	{
		if (this.ZombieCraftActivity != null)
		{
			MainGame.Instance.craftSystem.RemoveWorker(this.ZombieCraftActivity);
			if (this.ZombieCraftActivity != null)
			{
				this.ZombieCraftActivity.OnActiveStateChanged -= this.CrafterOnCraftActivityStateChanged;
				this.CrafterOnCraftActivityStateChanged();
				this.currentActivity = null;
			}
		}
	}

	// Token: 0x060028A1 RID: 10401 RVA: 0x000BE3E0 File Offset: 0x000BC5E0
	public bool CrafterIsEnoughMastery(WgoData wgoData)
	{
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def.isStarCraft)
		{
			return true;
		}
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def.isAutopsyCraft)
		{
			return true;
		}
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def.isPocketExtractCraft)
		{
			return true;
		}
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def is SurveyDef)
		{
			return true;
		}
		string talent = wgoData.Definition.talent;
		CraftElementBase currentCraftElement = this.AttachedWgoData.CraftComponent.CurrentCraftElement;
		return this.GetMasteryLevelForTalentBranch(talent, (currentCraftElement != null) ? currentCraftElement.Def : null) >= ((this.AttachedWgoData.CraftComponent.CurrentCraftElement != null) ? this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def.talentLock : wgoData.Definition.MasteryLock);
	}

	// Token: 0x060028A2 RID: 10402 RVA: 0x000BE4D0 File Offset: 0x000BC6D0
	public bool CrafterCanUseTool(WgoData wgoData, CraftDefBase craftDefBase)
	{
		return this.HasToolForWork(wgoData, craftDefBase);
	}

	// Token: 0x060028A3 RID: 10403 RVA: 0x000BE4DA File Offset: 0x000BC6DA
	private void CrafterOnCraftActivityStateChanged()
	{
		this.CrafterHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
	}

	// Token: 0x060028A4 RID: 10404 RVA: 0x000BE4F2 File Offset: 0x000BC6F2
	public void SetCustomAnimationState(global::AnimationState newState)
	{
		this.curAnimState = newState;
		Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
		if (onAnimationStateChanged == null)
		{
			return;
		}
		onAnimationStateChanged(this.curAnimState, false);
	}

	// Token: 0x060028A5 RID: 10405 RVA: 0x000BE512 File Offset: 0x000BC712
	public void InvokeOnAnimationStateChanged(bool playSound = false)
	{
		Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
		if (onAnimationStateChanged == null)
		{
			return;
		}
		onAnimationStateChanged(this.curAnimState, playSound);
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x000BE52C File Offset: 0x000BC72C
	public void SyncPorterBackpackLayer()
	{
		if (this.zombieType != ZombieType.Porter)
		{
			return;
		}
		float num = ((base.GetGameResInt("is_staying_at_porter_station") == 1) ? 0f : 1f);
		base.SetLayerWeightToAnimator(4, num);
	}

	// Token: 0x060028A7 RID: 10407 RVA: 0x000BE568 File Offset: 0x000BC768
	private void CrafterHandleCraftStatusChange(CraftComponentStatus craftStatus)
	{
		string id = this.attachedWgoData.id;
		if (id == "sawmill_wood_crafter" || id == "mine_ore_coal_crafter" || id == "clay_zombie_crafter" || id == "sand_zombie_crafter")
		{
			return;
		}
		if (craftStatus == CraftComponentStatus.Finished)
		{
			this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
		}
		global::AnimationState animationState;
		if (this.ZombieCraftActivity != null && this.ZombieCraftActivity.IsActive)
		{
			if (craftStatus == CraftComponentStatus.Started)
			{
				animationState = (this.AttachedWgoData.Definition.isAutoCrafter ? global::AnimationState.Idle : this.CrafterGetAnimationStateForCraft(this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def));
			}
			else
			{
				animationState = global::AnimationState.Idle;
			}
		}
		else
		{
			animationState = global::AnimationState.Idle;
		}
		if (this.curAnimState != animationState)
		{
			this.curAnimState = animationState;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, false);
		}
	}

	// Token: 0x060028A8 RID: 10408 RVA: 0x000BE63C File Offset: 0x000BC83C
	private global::AnimationState CrafterGetAnimationStateForCraft(CraftDefBase craftDefBase)
	{
		if (craftDefBase.isAuto)
		{
			return global::AnimationState.Idle;
		}
		ItemType itemType = ItemType.None;
		CraftDef craftDef = craftDefBase as CraftDef;
		if (craftDef != null && craftDef.customItemTypeAction != ItemType.None && !craftDefBase.isAuto)
		{
			itemType = craftDef.customItemTypeAction;
		}
		if (itemType == ItemType.None)
		{
			itemType = this.AttachedWgoData.Definition.toolAction.actionableTool;
		}
		return (global::AnimationState)(itemType + 19);
	}

	// Token: 0x060028A9 RID: 10409 RVA: 0x000BE694 File Offset: 0x000BC894
	private void CrafterHandleCraftProgressChange(float progress)
	{
		if (!this.AttachedWgoData.Definition.isAutoCrafter)
		{
			return;
		}
		string id = this.attachedWgoData.id;
		if (id == "sawmill_wood_crafter" || id == "mine_ore_coal_crafter" || id == "clay_zombie_crafter" || id == "sand_zombie_crafter")
		{
			return;
		}
		if (progress > 0f && this.curAnimState != global::AnimationState.Idle)
		{
			this.curAnimState = global::AnimationState.Idle;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, false);
		}
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x000BE724 File Offset: 0x000BC924
	public void CrafterOnToolChanged()
	{
		if (this.attachedWgoDataUniqueId.IsEmpty)
		{
			return;
		}
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement != null && this.AttachedWgoData.CraftComponent.CurrentCraftElement.IsStarted && this.ZombieCraftActivity == null)
		{
			this.CrafterStartCraftActivity(false);
		}
		else if (this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
		{
			this.CrafterTryPlaceOrderForCurrentCraftOrStartIt();
		}
		this.AttachedWgoData.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
		this.UpdateAttachedWgoViewWidgets();
	}

	// Token: 0x1700068D RID: 1677
	// (get) Token: 0x060028AB RID: 10411 RVA: 0x000BE7AC File Offset: 0x000BC9AC
	private OrderBase CaretakerExecutingOrder
	{
		get
		{
			if (!this.caretakerExecutingOrder.IsEmpty)
			{
				return base.WorldZoneData.FindOrder(this.caretakerExecutingOrder);
			}
			return null;
		}
	}

	// Token: 0x1700068E RID: 1678
	// (get) Token: 0x060028AC RID: 10412 RVA: 0x000BE7CE File Offset: 0x000BC9CE
	private bool HasCaretakerExecutingOrder
	{
		get
		{
			return !this.caretakerExecutingOrder.IsEmpty;
		}
	}

	// Token: 0x1700068F RID: 1679
	// (get) Token: 0x060028AD RID: 10413 RVA: 0x000BE7DE File Offset: 0x000BC9DE
	private WgoData CaretakerCurrentTarget
	{
		get
		{
			if (!this.caretakerCurrentTargetUniqueId.IsEmpty)
			{
				return MainGame.WorldData.GetWgoData(this.caretakerCurrentTargetUniqueId);
			}
			return null;
		}
	}

	// Token: 0x17000690 RID: 1680
	// (get) Token: 0x060028AE RID: 10414 RVA: 0x000BE7FF File Offset: 0x000BC9FF
	public SGuid CaretakerCurrentTargetUniqueId
	{
		get
		{
			return this.caretakerCurrentTargetUniqueId;
		}
	}

	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x060028AF RID: 10415 RVA: 0x000BE807 File Offset: 0x000BCA07
	// (set) Token: 0x060028B0 RID: 10416 RVA: 0x000BE80F File Offset: 0x000BCA0F
	public ZombieWgoData.ZombieCaretakerState CaretakerState
	{
		get
		{
			return this.caretakerState;
		}
		set
		{
			this.caretakerPreviousState = this.caretakerState;
			this.caretakerState = value;
			Action onCaretakerStateChanged = this.OnCaretakerStateChanged;
			if (onCaretakerStateChanged == null)
			{
				return;
			}
			onCaretakerStateChanged();
		}
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x060028B1 RID: 10417 RVA: 0x000BE834 File Offset: 0x000BCA34
	// (set) Token: 0x060028B2 RID: 10418 RVA: 0x000BE83C File Offset: 0x000BCA3C
	public Item CaretakerPortableItem
	{
		get
		{
			return this.caretakerPortableItem;
		}
		set
		{
			if (value.IsEmpty)
			{
				if (this.caretakerPortableItem.id != "empty")
				{
					if (this.caretakerPortableItem.Definition.itemSize == ItemSize.Big)
					{
						Action onRemoveOverheadItem = this.OnRemoveOverheadItem;
						if (onRemoveOverheadItem != null)
						{
							onRemoveOverheadItem();
						}
					}
					else
					{
						Action onRemoveInteractingItem = this.OnRemoveInteractingItem;
						if (onRemoveInteractingItem != null)
						{
							onRemoveInteractingItem();
						}
					}
				}
			}
			else if (value.Definition.itemSize == ItemSize.Big)
			{
				Action<Item, bool> onSetOverheadItem = this.OnSetOverheadItem;
				if (onSetOverheadItem != null)
				{
					onSetOverheadItem(value, true);
				}
			}
			else
			{
				Action<Item> onSetInteractingItem = this.OnSetInteractingItem;
				if (onSetInteractingItem != null)
				{
					onSetInteractingItem(value);
				}
			}
			this.caretakerPortableItem = value;
		}
	}

	// Token: 0x060028B3 RID: 10419 RVA: 0x000BE8E0 File Offset: 0x000BCAE0
	private void CaretakerUpdateBehaviour(float deltaTime)
	{
		switch (this.CaretakerState)
		{
		case ZombieWgoData.ZombieCaretakerState.OnStation:
			this.CaretakerTryGetNewOrder();
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToStation:
			this.CaretakerTryGetNewOrder();
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPickUpOrderItem:
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem:
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithExistingOrder:
			this.CaretakerTryCheckIsInventoryBusyIfInZone();
			return;
		case ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory:
			this.CaretakerTryPickUpFromInventory(deltaTime);
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToZombieToDeliverOrderItem:
		case ZombieWgoData.ZombieCaretakerState.GoToZombieToPickUpOrderItem:
			break;
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder:
			this.CaretakerTryGetNewOrder();
			if (!this.HasCaretakerExecutingOrder)
			{
				this.CaretakerTryCheckIsInventoryBusyIfInZone();
				return;
			}
			break;
		case ZombieWgoData.ZombieCaretakerState.WaitingOtherCaretakersOnInventory:
			this.CaretakerTryCheckIsInventoryFree();
			return;
		case ZombieWgoData.ZombieCaretakerState.FailedToFindPath:
			this.caretakerTimeForCheckFailedPathAgain -= deltaTime;
			this.CaretakerTryMoveToCurrentTarget();
			return;
		case ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory:
			switch (this.caretakerPreviousState)
			{
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem:
				this.CaretakerState = this.caretakerPreviousState;
				this.CaretakerTryPutPickedUpOrderItemToInventory();
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithExistingOrder:
				this.CaretakerState = this.caretakerPreviousState;
				this.CaretakerTryPutPortableItemToInventoryWithExistingOrder();
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder:
				this.CaretakerState = this.caretakerPreviousState;
				this.CaretakerTryPutPortableItemToInventoryWithoutExistingOrder();
				break;
			default:
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x000BE9D0 File Offset: 0x000BCBD0
	public override void OnPathComplete(MovementComponent component)
	{
		switch (this.ZombieType)
		{
		case ZombieType.Free:
		case ZombieType.Crafter:
		case ZombieType.ConveyorCrafter:
		case ZombieType.Worker:
		case ZombieType.Fighter:
			base.OnPathComplete(component);
			return;
		case ZombieType.Caretaker:
			this.caretakerCurrentMovementTargetUniqueId = SGuid.Empty;
			base.OnPathComplete(component);
			if (this.CaretakerState == ZombieWgoData.ZombieCaretakerState.WaitingOtherCaretakersOnInventory)
			{
				return;
			}
			if (component.Completion == MovementComponent.CompletionState.Success)
			{
				this.CaretakerOnPathSuccess();
				return;
			}
			if (component.Completion != MovementComponent.CompletionState.Canceled)
			{
				this.caretakerTimeForCheckFailedPathAgain = 1f;
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.FailedToFindPath;
				return;
			}
			break;
		case ZombieType.Porter:
			this.PorterOnPathComplete(component);
			return;
		case ZombieType.Gardener:
			this.gardenerCurrentMovementTargetUniqueId = SGuid.Empty;
			base.OnPathComplete(component);
			if (component.Completion == MovementComponent.CompletionState.Success)
			{
				this.GardenerOnPathSuccess();
				return;
			}
			if (component.Completion != MovementComponent.CompletionState.Canceled)
			{
				this.gardenerTimeForCheckFailedPathAgain = 1f;
				this.GardenerState = ZombieWgoData.ZombieGardenerState.FailedToFindPath;
				return;
			}
			break;
		case ZombieType.ConveyorTransporter:
			this.conveyorTransporterCurrentMovementTargetUniqueId = SGuid.Empty;
			base.OnPathComplete(component);
			if (component.Completion == MovementComponent.CompletionState.Success)
			{
				this.ConveyorTransporterOnPathSuccess();
				return;
			}
			if (component.Completion != MovementComponent.CompletionState.Canceled)
			{
				this.conveyorTransporterTimeForCheckFailedPathAgain = 1f;
				this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.FailedToFindPath;
				return;
			}
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x000BEAF4 File Offset: 0x000BCCF4
	private void CaretakerOnPathSuccess()
	{
		this.curAnimState = global::AnimationState.Idle;
		switch (this.CaretakerState)
		{
		case ZombieWgoData.ZombieCaretakerState.GoToStation:
		{
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.OnStation;
			GDPointData gdpointData = this.AttachedWgoData.GetGDPointData("zombie_porter_station_gd_point");
			base.Position = gdpointData.Position;
			this.direction.Value = gdpointData.Direction.ConvertToVector2XZ();
			return;
		}
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPickUpOrderItem:
		{
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory;
			this.curAnimState = global::AnimationState.WorkHands;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, true);
			return;
		}
		case ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory:
			break;
		case ZombieWgoData.ZombieCaretakerState.GoToZombieToDeliverOrderItem:
			this.CaretakerTryExecuteDeliveryOrder();
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToZombieToPickUpOrderItem:
			this.CaretakerTryExecutePickUpOrder();
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem:
			this.CaretakerTryPutPickedUpOrderItemToInventory();
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithExistingOrder:
			this.CaretakerTryPutPortableItemToInventoryWithExistingOrder();
			return;
		case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder:
			this.CaretakerTryPutPortableItemToInventoryWithoutExistingOrder();
			break;
		default:
			return;
		}
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x000BEBBC File Offset: 0x000BCDBC
	private void CaretakerTryGetNewOrder()
	{
		if (!this.HasCaretakerExecutingOrder)
		{
			OrderBase orderForCaretaker = base.WorldZoneData.GetOrderForCaretaker(this.CaretakerPortableItem);
			if (orderForCaretaker != null)
			{
				this.caretakerExecutingOrder = orderForCaretaker.UniqueId;
				orderForCaretaker.ExecutorUniqueId = base.UniqueId;
				this.CaretakerTryStartOrderExecutionOrGoToStation();
			}
		}
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x000BEC04 File Offset: 0x000BCE04
	private void CaretakerTryGetNewOrderOrMoveToStation()
	{
		this.CaretakerTryGetNewOrder();
		if (!this.HasCaretakerExecutingOrder)
		{
			this.CaretakerTryMoveToStation();
		}
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x000BEC1C File Offset: 0x000BCE1C
	private void CaretakerOnOrderRemoved(OrderBase order)
	{
		if (order.UniqueId == this.caretakerExecutingOrder)
		{
			if (this.CaretakerState == ZombieWgoData.ZombieCaretakerState.FailedToFindPath)
			{
				this.CaretakerState = this.caretakerPreviousState;
				this.caretakerTimeForCheckFailedPathAgain = 0f;
			}
			if (this.CaretakerState == ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory)
			{
				this.CaretakerState = this.caretakerPreviousState;
			}
			switch (this.CaretakerState)
			{
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPickUpOrderItem:
				this.CaretakerTryStopOrderExecution();
				this.CaretakerTryGetNewOrderOrMoveToStation();
				return;
			case ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory:
				this.caretakerPickingUpFromInventoryTime = 0f;
				this.CaretakerTryStopOrderExecution();
				this.CaretakerTryGetNewOrderOrMoveToStation();
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToZombieToDeliverOrderItem:
				this.CaretakerTryStopOrderExecution();
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
				this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToZombieToPickUpOrderItem:
				this.CaretakerTryStopOrderExecution();
				this.CaretakerTryGetNewOrderOrMoveToStation();
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem:
				this.CaretakerTryStopOrderExecution();
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithExistingOrder:
				this.CaretakerTryStopOrderExecution();
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
				return;
			case ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder:
				break;
			case ZombieWgoData.ZombieCaretakerState.WaitingOtherCaretakersOnInventory:
				this.CaretakerTryStopOrderExecution();
				this.CaretakerTryGetNewOrderOrMoveToStation();
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x000BED14 File Offset: 0x000BCF14
	private void CaretakerTryPickUpFromInventory(float deltaTime)
	{
		this.caretakerPickingUpFromInventoryTime += deltaTime;
		LazyExpression lazyExpression = new LazyExpression(ConstDef.Get("zombie_caretaker_picking_up_time").StringValue);
		if (this.caretakerPickingUpFromInventoryTime >= lazyExpression.EvaluateFloat(this))
		{
			this.caretakerPickingUpFromInventoryTime = 0f;
			Item item = this.CaretakerExecutingOrder.Item;
			bool isEmpty = this.CaretakerPortableItem.IsEmpty;
			int num = (isEmpty ? 0 : this.CaretakerPortableItem.Count);
			int num2 = item.Count - num;
			WgoData caretakerCurrentTarget = this.CaretakerCurrentTarget;
			int totalCountInInventory = caretakerCurrentTarget.Inventory.Data.GetTotalCountInInventory(item.id, null, false);
			int num3 = 0;
			if (totalCountInInventory >= num2)
			{
				caretakerCurrentTarget.Inventory.RemoveItemById(item.id, num2, null, null, false);
				if (isEmpty)
				{
					this.CaretakerPortableItem = new Item(item.id, item.Count);
				}
				else
				{
					this.CaretakerPortableItem.Count += num2;
					this.CaretakerPortableItem = this.CaretakerPortableItem;
				}
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToZombieToDeliverOrderItem;
				this.CaretakerTryMoveToZombie();
				return;
			}
			foreach (WgoData wgoData in base.WorldZoneData.MultiInventoryWgoDatas)
			{
				if (wgoData != caretakerCurrentTarget)
				{
					num3 += wgoData.Inventory.Data.GetTotalCountInInventory(item.id, null, false);
				}
			}
			if (totalCountInInventory + num3 >= num2)
			{
				caretakerCurrentTarget.Inventory.RemoveItemById(item.id, totalCountInInventory, null, null, false);
				if (isEmpty)
				{
					this.CaretakerPortableItem = new Item(item.id, totalCountInInventory);
				}
				else
				{
					this.CaretakerPortableItem.Count += totalCountInInventory;
					this.CaretakerPortableItem = this.CaretakerPortableItem;
				}
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPickUpOrderItem;
				this.CaretakerTryMoveToNearestInventoryWithRequiredItemCountToPickUp();
				return;
			}
			this.CaretakerTryStopOrderExecution();
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
			this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
		}
	}

	// Token: 0x060028BA RID: 10426 RVA: 0x000BEF08 File Offset: 0x000BD108
	private void CaretakerTryStartOrderExecutionOrGoToStation()
	{
		OrderBase orderBase = this.CaretakerExecutingOrder;
		bool isEmpty = this.CaretakerPortableItem.IsEmpty;
		if (orderBase != null)
		{
			if (orderBase is PickupOrder)
			{
				if (isEmpty)
				{
					this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToZombieToPickUpOrderItem;
					this.CaretakerTryMoveToZombie();
					return;
				}
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithExistingOrder;
				this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
				return;
			}
			else
			{
				DeliveryOrder deliveryOrder = orderBase as DeliveryOrder;
				if (deliveryOrder != null)
				{
					if (isEmpty)
					{
						this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPickUpOrderItem;
						this.CaretakerTryMoveToNearestInventoryWithRequiredItemCountToPickUp();
						return;
					}
					if (!(this.CaretakerPortableItem.id == deliveryOrder.Item.id))
					{
						this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithExistingOrder;
						this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
						return;
					}
					if (this.CaretakerPortableItem.Count >= deliveryOrder.Item.Count)
					{
						this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToZombieToDeliverOrderItem;
						this.CaretakerTryMoveToZombie();
						return;
					}
					this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPickUpOrderItem;
					this.CaretakerTryMoveToNearestInventoryWithRequiredItemCountToPickUp();
					return;
				}
			}
		}
		else
		{
			this.CaretakerTryMoveToStation();
		}
	}

	// Token: 0x060028BB RID: 10427 RVA: 0x000BEFD5 File Offset: 0x000BD1D5
	private void CaretakerTryStopOrderExecution()
	{
		if (!this.caretakerExecutingOrder.IsEmpty)
		{
			if (this.CaretakerExecutingOrder != null)
			{
				this.CaretakerExecutingOrder.ExecutorUniqueId = SGuid.Empty;
			}
			this.caretakerExecutingOrder = SGuid.Empty;
		}
	}

	// Token: 0x060028BC RID: 10428 RVA: 0x000BF008 File Offset: 0x000BD208
	private void CaretakerTryExecuteDeliveryOrder()
	{
		if (!this.HasCaretakerExecutingOrder)
		{
			this.CaretakerTryStopOrderExecution();
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
			return;
		}
		ZombieWgoData zombieWgoData = this.CaretakerExecutingOrder.ZombieWgoData;
		string text;
		if (!this.CaretakerExecutingOrder.TryExecuteOrder(new ZombieCaretakerOrderExecutor(this), out text))
		{
			this.CaretakerTryStopOrderExecution();
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
			this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
			return;
		}
		OrderBase orderBase = this.CaretakerExecutingOrder;
		this.CaretakerTryStopOrderExecution();
		zombieWgoData.WorldZoneData.RemoveOrder(orderBase.UniqueId);
		zombieWgoData.CrafterOnOrderExecuted(orderBase);
		if (this.CaretakerPortableItem.IsEmpty)
		{
			this.CaretakerTryGetNewOrderOrMoveToStation();
			return;
		}
		this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
		this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
	}

	// Token: 0x060028BD RID: 10429 RVA: 0x000BF0A8 File Offset: 0x000BD2A8
	private void CaretakerTryExecutePickUpOrder()
	{
		if (this.HasCaretakerExecutingOrder)
		{
			ZombieWgoData zombieWgoData = this.CaretakerExecutingOrder.ZombieWgoData;
			WgoData wgoData = zombieWgoData.AttachedWgoData;
			CraftDefBase craftDefBase = null;
			GameRes gameRes = null;
			if (wgoData.CraftComponent.Status == CraftComponentStatus.WaitingForWorkerPickUp && wgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.GetInt("auto_start_same_craft_after_pickup") == 1)
			{
				craftDefBase = wgoData.CraftComponent.CurrentCraftElement.ParamsData.CraftDef;
				gameRes = wgoData.CraftComponent.CurrentCraftElement.ParamsData.customRes.Clone();
			}
			string text;
			if (!this.CaretakerExecutingOrder.TryExecuteOrder(new ZombieCaretakerOrderExecutor(this), out text))
			{
				this.CaretakerTryStopOrderExecution();
				this.CaretakerTryGetNewOrderOrMoveToStation();
				return;
			}
			OrderBase orderBase = this.CaretakerExecutingOrder;
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem;
			this.CaretakerTryMoveToNearestInventoryToPutPickedUpOrderItem();
			this.CaretakerTryStopOrderExecution();
			zombieWgoData.WorldZoneData.RemoveOrder(orderBase.UniqueId);
			zombieWgoData.CrafterOnOrderExecuted(orderBase);
			if (craftDefBase != null)
			{
				CraftParamsData craftParamsData = new CraftParamsData(wgoData.CraftComponent.AvailableCrafts[0].id, wgoData, CraftParamsData.CraftParamsType.Common, -1);
				if (gameRes != null)
				{
					craftParamsData.customRes = gameRes.Clone();
				}
				craftParamsData.customRes.Set("auto_start_same_craft_after_pickup", 1f);
				craftParamsData.customRes.Set("do_not_check_multiinventory_space", 1f);
				craftParamsData.customRes.Set("do_not_check_worker_dependent_values", 1f);
				wgoData.CraftComponent.TryStartCraft(new CraftElement(wgoData.CraftComponent.AvailableCrafts[0].id, 1, craftParamsData));
				zombieWgoData.CrafterStopCraftActivity();
				zombieWgoData.CrafterStartCraftActivity(true);
				return;
			}
		}
		else
		{
			this.CaretakerTryGetNewOrderOrMoveToStation();
		}
	}

	// Token: 0x060028BE RID: 10430 RVA: 0x000BF24E File Offset: 0x000BD44E
	private void CaretakerTryMoveToStation()
	{
		this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToStation;
		this.caretakerCurrentTargetUniqueId = this.AttachedWgoData.UniqueId;
		this.CaretakerTryMoveToCurrentTarget();
	}

	// Token: 0x060028BF RID: 10431 RVA: 0x000BF270 File Offset: 0x000BD470
	private void CaretakerTryMoveToNearestInventoryWithRequiredItemCountToPickUp()
	{
		Item item = this.CaretakerExecutingOrder.Item;
		int num = 0;
		if (!this.CaretakerPortableItem.IsEmpty)
		{
			num = this.CaretakerPortableItem.Count;
		}
		if (base.WorldZoneData.CanDeliveryOrderBeTakenOnExecution(this.CaretakerExecutingOrder as DeliveryOrder, num))
		{
			List<WgoData> list = new List<WgoData>();
			foreach (WgoData wgoData in base.WorldZoneData.MultiInventoryWgoDatas)
			{
				if (wgoData.Inventory.Data.GetTotalCountInInventory(item.id, null, false) >= item.Count - num)
				{
					list.Add(wgoData);
				}
			}
			float num2 = float.MaxValue;
			WgoData wgoData2 = null;
			if (list.Count > 0)
			{
				using (List<WgoData>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WgoData wgoData3 = enumerator.Current;
						float num3 = Mathf.Abs(Vector3.Distance(wgoData3.Position, base.Position));
						if (num3 < num2)
						{
							num2 = num3;
							wgoData2 = wgoData3;
						}
					}
					goto IL_01C4;
				}
			}
			List<WgoData> list2 = new List<WgoData>();
			foreach (WgoData wgoData4 in base.WorldZoneData.MultiInventoryWgoDatas)
			{
				if (wgoData4.Inventory.Data.GetTotalCountInInventory(item.id, null, false) >= 0)
				{
					list2.Add(wgoData4);
				}
			}
			foreach (WgoData wgoData5 in list2)
			{
				float num4 = Mathf.Abs(Vector3.Distance(wgoData5.Position, base.Position));
				if (num4 < num2)
				{
					num2 = num4;
					wgoData2 = wgoData5;
				}
			}
			IL_01C4:
			this.caretakerCurrentTargetUniqueId = wgoData2.UniqueId;
			this.CaretakerTryMoveToCurrentTarget();
			return;
		}
		this.CaretakerTryStopOrderExecution();
		this.CaretakerTryGetNewOrderOrMoveToStation();
	}

	// Token: 0x060028C0 RID: 10432 RVA: 0x000BF498 File Offset: 0x000BD698
	private void CaretakerTryMoveToNearestInventoryToPutPortableItem()
	{
		WgoData wgoData = this.CaretakerGetNearestInventoryWithSpaceForPortableItem();
		if (wgoData == null)
		{
			this.caretakerCurrentTargetUniqueId = SGuid.Empty;
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
		this.caretakerCurrentTargetUniqueId = wgoData.UniqueId;
		this.CaretakerTryMoveToCurrentTarget();
	}

	// Token: 0x060028C1 RID: 10433 RVA: 0x000BF4D8 File Offset: 0x000BD6D8
	private void CaretakerTryMoveToNearestInventoryToPutPickedUpOrderItem()
	{
		WgoData wgoData = this.CaretakerGetNearestInventoryWithSpaceForPortableItem();
		if (wgoData == null)
		{
			this.caretakerCurrentTargetUniqueId = SGuid.Empty;
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
		this.caretakerCurrentTargetUniqueId = wgoData.UniqueId;
		this.CaretakerTryMoveToCurrentTarget();
	}

	// Token: 0x060028C2 RID: 10434 RVA: 0x000BF518 File Offset: 0x000BD718
	private WgoData CaretakerGetNearestInventoryWithSpaceForPortableItem()
	{
		float num = float.MaxValue;
		WgoData wgoData = null;
		float num2 = float.MaxValue;
		WgoData wgoData2 = null;
		foreach (WgoData wgoData3 in base.WorldZoneData.MultiInventoryWgoDatas)
		{
			if (wgoData3.Inventory.CanAddItemToInventory(this.CaretakerPortableItem))
			{
				float num3 = Vector3.Distance(wgoData3.Position, base.Position);
				if (wgoData3.Inventory.Data.HasItemByItemId(this.CaretakerPortableItem.id, 1))
				{
					if (num3 < num)
					{
						num = num3;
						wgoData = wgoData3;
					}
				}
				else if (num3 < num2)
				{
					num2 = num3;
					wgoData2 = wgoData3;
				}
			}
		}
		if (wgoData == null)
		{
			return wgoData2;
		}
		return wgoData;
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x000BF5E0 File Offset: 0x000BD7E0
	private void CaretakerTryMoveToCurrentTarget()
	{
		SGuid sguid = this.caretakerCurrentMovementTargetUniqueId;
		Guid? guid = ((sguid != null) ? new Guid?(sguid.Guid) : null);
		SGuid sguid2 = this.caretakerCurrentTargetUniqueId;
		if (guid == ((sguid2 != null) ? new Guid?(sguid2.Guid) : null))
		{
			return;
		}
		if (this.CaretakerState == ZombieWgoData.ZombieCaretakerState.FailedToFindPath)
		{
			if (this.caretakerTimeForCheckFailedPathAgain <= 0f)
			{
				this.CaretakerState = this.caretakerPreviousState;
				this.caretakerTimeForCheckFailedPathAgain = 0f;
				this.<CaretakerTryMoveToCurrentTarget>g__MoveAction|223_0();
				return;
			}
		}
		else
		{
			this.<CaretakerTryMoveToCurrentTarget>g__MoveAction|223_0();
		}
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000BF69D File Offset: 0x000BD89D
	private void CaretakerTryMoveToZombie()
	{
		this.caretakerCurrentTargetUniqueId = MainGame.ZombieSystemData.GetZombie(this.CaretakerExecutingOrder.TargetWgoUniqueId).AttachedWgoData.UniqueId;
		this.CaretakerTryMoveToCurrentTarget();
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x000BF6CC File Offset: 0x000BD8CC
	private void CaretakerTryPutPickedUpOrderItemToInventory()
	{
		if (this.CaretakerCurrentTarget == null)
		{
			if (this.CaretakerIsAnyInventoryWithSpaceForPortableItemExists())
			{
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem;
				this.CaretakerTryMoveToNearestInventoryToPutPickedUpOrderItem();
				return;
			}
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
		else
		{
			this.CaretakerCurrentTarget.Inventory.AddItemToInventory(this.CaretakerPortableItem, null, false);
			if (this.CaretakerPortableItem.Count <= 0)
			{
				this.CaretakerPortableItem = Item.Empty;
				this.CaretakerTryGetNewOrderOrMoveToStation();
				return;
			}
			if (this.CaretakerIsAnyInventoryWithSpaceForPortableItemExists())
			{
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem;
				this.CaretakerTryMoveToNearestInventoryToPutPickedUpOrderItem();
				return;
			}
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x000BF758 File Offset: 0x000BD958
	private void CaretakerTryPutPortableItemToInventoryWithExistingOrder()
	{
		if (this.CaretakerCurrentTarget == null)
		{
			if (this.CaretakerIsAnyInventoryWithSpaceForPortableItemExists())
			{
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem;
				this.CaretakerTryMoveToNearestInventoryToPutPickedUpOrderItem();
				return;
			}
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
		else
		{
			this.CaretakerCurrentTarget.Inventory.AddItemToInventory(this.CaretakerPortableItem, null, false);
			if (this.CaretakerPortableItem.Count <= 0)
			{
				this.CaretakerPortableItem = Item.Empty;
				this.CaretakerTryStartOrderExecutionOrGoToStation();
				return;
			}
			if (this.CaretakerIsAnyInventoryWithSpaceForPortableItemExists())
			{
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToDeliverOrderItem;
				this.CaretakerTryMoveToNearestInventoryToPutPickedUpOrderItem();
				return;
			}
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x000BF7E4 File Offset: 0x000BD9E4
	private void CaretakerTryPutPortableItemToInventoryWithoutExistingOrder()
	{
		if (this.CaretakerCurrentTarget == null)
		{
			if (this.CaretakerIsAnyInventoryWithSpaceForPortableItemExists())
			{
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
				this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
				return;
			}
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
		else
		{
			this.CaretakerCurrentTarget.Inventory.AddItemToInventory(this.CaretakerPortableItem, null, false);
			if (this.CaretakerPortableItem.Count <= 0)
			{
				this.CaretakerPortableItem = Item.Empty;
				this.CaretakerTryMoveToStation();
				return;
			}
			if (this.CaretakerIsAnyInventoryWithSpaceForPortableItemExists())
			{
				this.CaretakerState = ZombieWgoData.ZombieCaretakerState.GoToInventoryToPutPortableItemWithoutExistingOrder;
				this.CaretakerTryMoveToNearestInventoryToPutPortableItem();
				return;
			}
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.CanNotPutItemToInventory;
			return;
		}
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x000BF870 File Offset: 0x000BDA70
	private void CaretakerTryCheckIsInventoryBusyIfInZone()
	{
		if (this.CaretakerCurrentTarget == null)
		{
			return;
		}
		if (Mathf.Abs(Vector3.Distance(this.CaretakerCurrentTarget.Position, base.Position)) <= 2f)
		{
			foreach (SGuid sguid in MainGame.ZombieSystemData.zombieOnSceneWgoIds)
			{
				ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(sguid);
				if (zombie != this && zombie.CaretakerState == ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory && zombie.CaretakerCurrentTargetUniqueId == this.CaretakerCurrentTargetUniqueId)
				{
					this.CaretakerState = ZombieWgoData.ZombieCaretakerState.WaitingOtherCaretakersOnInventory;
					base.MovementComponent.ForceStop();
				}
			}
		}
	}

	// Token: 0x060028C9 RID: 10441 RVA: 0x000BF92C File Offset: 0x000BDB2C
	private void CaretakerTryCheckIsInventoryFree()
	{
		foreach (SGuid sguid in MainGame.ZombieSystemData.zombieOnSceneWgoIds)
		{
			ZombieWgoData zombie = MainGame.ZombieSystemData.GetZombie(sguid);
			if (zombie != this && zombie.CaretakerState == ZombieWgoData.ZombieCaretakerState.PickingUpOrderItemFromInventory && zombie.CaretakerCurrentTargetUniqueId == this.CaretakerCurrentTargetUniqueId)
			{
				return;
			}
		}
		this.CaretakerState = this.caretakerPreviousState;
		this.CaretakerTryMoveToCurrentTarget();
	}

	// Token: 0x060028CA RID: 10442 RVA: 0x000BF9BC File Offset: 0x000BDBBC
	private bool CaretakerIsAnyInventoryWithSpaceForPortableItemExists()
	{
		return this.CaretakerGetNearestInventoryWithSpaceForPortableItem() != null;
	}

	// Token: 0x060028CB RID: 10443 RVA: 0x000BF9C8 File Offset: 0x000BDBC8
	public void ConveyorCrafterOnToolChanged()
	{
		if (this.attachedWgoDataUniqueId.IsEmpty)
		{
			return;
		}
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement != null && this.AttachedWgoData.CraftComponent.CurrentCraftElement.IsStarted && this.ZombieCraftActivity == null)
		{
			this.ConveyorCrafterStartCraftActivity(false);
		}
		else if (this.AttachedWgoData.CraftComponent.HasCraftsInQueue)
		{
			this.ConveyorCrafterTryStartCurrentCraft(null);
		}
		this.AttachedWgoData.CraftComponent.UpdateCanContinueManualCraftState(Time.deltaTime);
		this.UpdateAttachedWgoViewWidgets();
	}

	// Token: 0x060028CC RID: 10444 RVA: 0x000BFA51 File Offset: 0x000BDC51
	public void ConveyorCrafterOnAttachedWgoCraftEnd(CraftElementBase ce)
	{
		if (this.attachedWgoData.CraftComponent.Status == CraftComponentStatus.Started)
		{
			return;
		}
		this.ConveyorCrafterStopCraftActivity();
	}

	// Token: 0x060028CD RID: 10445 RVA: 0x000BFA6D File Offset: 0x000BDC6D
	private void ConveyorCrafterOnCraftAddedToQueue(CraftElementBase craftQueueElement)
	{
		this.ConveyorCrafterTryStartCurrentCraft(null);
	}

	// Token: 0x060028CE RID: 10446 RVA: 0x000BFA76 File Offset: 0x000BDC76
	private void ConveyorCrafterOnCraftRemovedFromQueue(CraftElementBase craftQueueElement)
	{
		if (this.AttachedWgoData.CraftComponent.CurrentCraftElement == null)
		{
			this.ConveyorCrafterStopCraftActivity();
		}
	}

	// Token: 0x060028CF RID: 10447 RVA: 0x000BFA90 File Offset: 0x000BDC90
	private void ConveyorCrafterTryStartCurrentCraft(List<Item> items = null)
	{
		CraftElementBase currentCraftElement = this.AttachedWgoData.CraftComponent.CurrentCraftElement;
		if (currentCraftElement == null || this.AttachedWgoData.CraftComponent.Status == CraftComponentStatus.WaitingForOutputDrop)
		{
			return;
		}
		if (!this.CrafterCanUseTool(this.AttachedWgoData, currentCraftElement.Def))
		{
			return;
		}
		if (!this.CrafterIsEnoughMastery(this.AttachedWgoData))
		{
			return;
		}
		if (this.ZombieCraftActivity == null)
		{
			this.ConveyorCrafterStartCraftActivity(true);
		}
	}

	// Token: 0x060028D0 RID: 10448 RVA: 0x000BFAF9 File Offset: 0x000BDCF9
	private void ConveyorHadleCraftStatusChange(CraftComponentStatus craftStatus)
	{
		craftStatus = this.AttachedWgoData.CraftComponent.Status;
		if (craftStatus == CraftComponentStatus.Finished)
		{
			this.ConveyorCrafterStopCraftActivity();
			return;
		}
		if (craftStatus == CraftComponentStatus.Started && this.ZombieCraftActivity == null)
		{
			this.ConveyorCrafterStartCraftActivity(true);
		}
	}

	// Token: 0x060028D1 RID: 10449 RVA: 0x000BFB2C File Offset: 0x000BDD2C
	private void ConveyorCrafterHandleCraftStatusChange(CraftComponentStatus craftStatus)
	{
		craftStatus = this.AttachedWgoData.CraftComponent.Status;
		global::AnimationState animationState;
		if (this.ZombieCraftActivity != null && this.ZombieCraftActivity.IsActive)
		{
			if (craftStatus == CraftComponentStatus.Started)
			{
				animationState = (this.AttachedWgoData.Definition.isAutoCrafter ? global::AnimationState.Idle : this.CrafterGetAnimationStateForCraft(this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def));
			}
			else
			{
				animationState = global::AnimationState.Idle;
			}
		}
		else
		{
			animationState = global::AnimationState.Idle;
		}
		if (this.curAnimState != animationState)
		{
			this.curAnimState = animationState;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, false);
		}
	}

	// Token: 0x060028D2 RID: 10450 RVA: 0x000BFBC4 File Offset: 0x000BDDC4
	private void ConveyorCrafterStartCraftActivity(bool resetTicks = true)
	{
		ZombieCraftActivity zombieCraftActivity = new ZombieCraftActivity(this.AttachedWgoData, this);
		this.currentActivity = zombieCraftActivity;
		this.ZombieCraftActivity.OnActiveStateChanged += this.ConveyorCrafterOnCraftActivityStateChanged;
		this.ConveyorCrafterOnCraftActivityStateChanged();
		MainGame.Instance.conveyorSystem.AddWorker(zombieCraftActivity);
		this.UpdateAttachedWgoViewWidgets();
		if (resetTicks)
		{
			this.AttachedWgoData.CraftComponent.ZombieSubTicks = 0;
		}
		if (this.AttachedWgoData.CraftComponent.Status != CraftComponentStatus.WaitingForOutputDrop)
		{
			this.AttachedWgoData.CraftComponent.TryContinueFromQueue();
		}
	}

	// Token: 0x060028D3 RID: 10451 RVA: 0x000BFC54 File Offset: 0x000BDE54
	private void ConveyorCrafterStopCraftActivity()
	{
		if (this.ZombieCraftActivity != null)
		{
			MainGame.Instance.conveyorSystem.RemoveWorker(this.ZombieCraftActivity);
			if (this.ZombieCraftActivity != null)
			{
				this.ZombieCraftActivity.OnActiveStateChanged -= this.ConveyorCrafterOnCraftActivityStateChanged;
				this.ConveyorCrafterOnCraftActivityStateChanged();
				this.currentActivity = null;
			}
		}
	}

	// Token: 0x060028D4 RID: 10452 RVA: 0x000BFCAA File Offset: 0x000BDEAA
	private void ConveyorCrafterOnCraftActivityStateChanged()
	{
		this.ConveyorCrafterHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x000BFCC4 File Offset: 0x000BDEC4
	public bool PorterCheckDeliveryStart()
	{
		if (base.GetGameResInt("is_staying_at_porter_station") == 1)
		{
			PorterStationDef data = GameBalance.Me.GetData<PorterStationDef>(this.AttachedWgoData.id);
			if (data != null)
			{
				for (int i = 0; i < base.WorldZoneData.MultiInventoryWgoDatas.Count; i++)
				{
					Inventory inventory = base.WorldZoneData.MultiInventoryWgoDatas[i].Inventory;
					for (int j = 0; j < data.items.Count; j++)
					{
						NeedItemData needItemData = data.items[j];
						if (this.attachedWgoData.GetGameResInt(needItemData.id) == 1)
						{
							this.PorterTryTakeItemFromInventory(inventory, needItemData);
						}
					}
				}
				this.PorterFillRemainingEmptySlotsEvenly(data);
				if (this.porterInventory.Data.InventoryFillSize > 0)
				{
					GDPointData gdpointDataById = MainGame.WorldData.gdPointsData.GetGDPointDataById(base.WorldZoneData.Definition.porterStartPoint);
					GDPointData gdpointDataById2 = MainGame.WorldData.gdPointsData.GetGDPointDataById(base.WorldZoneData.Definition.porterEndPoint);
					base.Position = gdpointDataById.Position;
					MovementComponent.StartPathResult startPathResult = base.MovementComponent.StartPath(gdpointDataById2.Position, base.WorldId, base.WorldId, MovementType.GDGraph, 1.5f, "", null, null, MovementComponent.DestinationType.Position);
					if (startPathResult == MovementComponent.StartPathResult.Started)
					{
						base.SetGameRes("is_staying_at_porter_station", 0);
						base.SetGameRes("is_moving_to_target_world_zone", 1);
						this.AttachedWgoData.SetTriggerToAnimator("path_anim");
						base.SetLayerWeightToAnimator(4, 1f);
						base.IsInteractable = true;
						return true;
					}
					if (startPathResult - MovementComponent.StartPathResult.AlreadyAtDestinationPoint > 1)
					{
						throw new ArgumentOutOfRangeException();
					}
				}
			}
		}
		return false;
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x000BFE60 File Offset: 0x000BE060
	private void PorterOnPathComplete(MovementComponent component)
	{
		switch (component.Completion)
		{
		case MovementComponent.CompletionState.None:
		case MovementComponent.CompletionState.Fail:
		case MovementComponent.CompletionState.Canceled:
			return;
		case MovementComponent.CompletionState.Success:
			if (base.GetGameResInt("is_moving_to_target_world_zone") == 1)
			{
				this.PorterOnCameToTargetWorldZone();
				return;
			}
			this.PorterOnCameToStationWorldZone();
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x060028D7 RID: 10455 RVA: 0x000BFEB0 File Offset: 0x000BE0B0
	private void PorterOnCameToTargetWorldZone()
	{
		PorterStationDef data = GameBalance.Me.GetData<PorterStationDef>(this.AttachedWgoData.id);
		if (data != null)
		{
			WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById(data.targetWorldZone);
			MultiInventory multiInventory;
			if (data.customTargets.Count > 0)
			{
				List<Inventory> list = new List<Inventory>();
				for (int i = 0; i < data.customTargets.Count; i++)
				{
					WgoData wgoData = MainGame.WorldData.GetWgoData(data.customTargets[i]);
					if (wgoData != null)
					{
						list.Add(wgoData.Inventory);
						WgoData wgoData2;
						GameSceneData gameSceneData;
						if (wgoData.id == "crates_small" && MainGame.Instance.GameSave.worldData.TryGetWgoData("warehouse_crane", out wgoData2, out gameSceneData))
						{
							wgoData2.SetTriggerToAnimator("Work");
						}
					}
				}
				multiInventory = new MultiInventory(list);
			}
			else
			{
				multiInventory = new MultiInventory(worldZoneDataById, null, false);
			}
			if (multiInventory.inventoryList.Count > 0)
			{
				List<Item> list2 = new List<Item>();
				foreach (Item item in this.porterInventory.Data.Inventory)
				{
					if (!(item.id == "fake_porter_slot_filler") && item.Count > 0)
					{
						Item item2 = new Item(item.id, item.Count);
						int num = this.TryAddItemPreferringSameItem(multiInventory.inventoryList, item2);
						if (num > 0)
						{
							list2.Add(new Item(item.id, num));
						}
					}
				}
				foreach (Item item3 in list2)
				{
					this.porterInventory.RemoveItemById(item3.id, item3.Count, null, null, false);
					if (item3.Definition.itemSize == ItemSize.Big)
					{
						this.porterInventory.RemoveItemById("fake_porter_slot_filler", item3.Count, null, null, false);
					}
				}
			}
			GDPointData gdpointDataById = MainGame.WorldData.gdPointsData.GetGDPointDataById(base.WorldZoneData.Definition.porterStartPoint);
			switch (base.MovementComponent.StartPath(gdpointDataById.Position, base.WorldId, base.WorldId, MovementType.GDGraph, 1.5f, "", null, null, MovementComponent.DestinationType.Position))
			{
			case MovementComponent.StartPathResult.Started:
				base.SetGameRes("is_moving_to_target_world_zone", 0);
				return;
			case MovementComponent.StartPathResult.AlreadyAtDestinationPoint:
			case MovementComponent.StartPathResult.IncorrectMovementType:
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	// Token: 0x060028D8 RID: 10456 RVA: 0x000C014C File Offset: 0x000BE34C
	private void PorterOnCameToStationWorldZone()
	{
		base.SetGameRes("is_staying_at_porter_station", 1);
		if (!this.PorterCheckDeliveryStart())
		{
			GDPointData gdpointData = this.attachedWgoData.GetGDPointData("zombie_porter_station_gd_point");
			base.Position = gdpointData.Position;
			this.direction.Value = Direction.Down.ConvertToVector2XZ();
			this.AttachedWgoData.SetTriggerToAnimator("with_zombie");
			base.IsInteractable = false;
			base.SetLayerWeightToAnimator(4, 0f);
		}
	}

	// Token: 0x060028D9 RID: 10457 RVA: 0x000C01C0 File Offset: 0x000BE3C0
	private void PorterTryTakeItemFromInventory(Inventory inventory, NeedItemData cur)
	{
		if (cur.ItemDef == null)
		{
			return;
		}
		if (cur.ItemDef.itemSize == ItemSize.Big)
		{
			if (this.porterInventory.Data.InventorySize - this.porterInventory.Data.InventoryFillSize < 2)
			{
				return;
			}
			int num = inventory.Data.GetTotalCountInInventory(cur.id, null, false);
			if (num <= 0)
			{
				return;
			}
			do
			{
				this.porterInventory.AddItemToInventory(new Item("fake_porter_slot_filler", 1), null, false);
				this.porterInventory.AddItemToInventory(new Item(cur.id, 1), null, false);
				inventory.RemoveItemById(cur.id, 1, null, null, false);
				num--;
			}
			while (num > 0 && this.porterInventory.Data.InventorySize - this.porterInventory.Data.InventoryFillSize >= 2);
			return;
		}
		else
		{
			int totalCountInInventory = inventory.Data.GetTotalCountInInventory(cur.id, null, false);
			if (totalCountInInventory <= 0)
			{
				return;
			}
			int num2 = this.porterInventory.Data.CanAddItemCountToInventory(new Item(cur.id, totalCountInInventory), false, null, false);
			this.PorterAddTakenItems(inventory, cur.id, num2);
			return;
		}
	}

	// Token: 0x060028DA RID: 10458 RVA: 0x000C02E0 File Offset: 0x000BE4E0
	private void PorterFillRemainingEmptySlotsEvenly(PorterStationDef stationDef)
	{
		for (int i = 0; i < stationDef.items.Count; i++)
		{
			NeedItemData needItemData = stationDef.items[i];
			if (this.IsPorterEnabledRegularItem(needItemData) && this.GetPorterSourceItemCount(needItemData.id) > 0)
			{
				int porterRemainingRegularItemsWithStock = this.GetPorterRemainingRegularItemsWithStock(stationDef, i);
				int num = this.porterInventory.Data.InventorySize - this.porterInventory.Data.InventoryFillSize;
				if (porterRemainingRegularItemsWithStock <= 0 || num <= 0)
				{
					return;
				}
				int num2 = Mathf.Max(1, num / porterRemainingRegularItemsWithStock);
				int num3 = 0;
				while (num3 < base.WorldZoneData.MultiInventoryWgoDatas.Count && num2 > 0)
				{
					int inventoryFillSize = this.porterInventory.Data.InventoryFillSize;
					this.PorterTryFillEmptySlotsFromInventory(base.WorldZoneData.MultiInventoryWgoDatas[num3].Inventory, needItemData, num2);
					num2 -= this.porterInventory.Data.InventoryFillSize - inventoryFillSize;
					num3++;
				}
			}
		}
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000C03E4 File Offset: 0x000BE5E4
	private void PorterTryFillEmptySlotsFromInventory(Inventory inventory, NeedItemData cur, int emptySlotsToUse)
	{
		int totalCountInInventory = inventory.Data.GetTotalCountInInventory(cur.id, null, false);
		if (totalCountInInventory <= 0 || emptySlotsToUse <= 0)
		{
			return;
		}
		int num = this.porterInventory.Data.InventorySize - this.porterInventory.Data.InventoryFillSize;
		int num2 = Mathf.Min(emptySlotsToUse, num);
		if (num2 <= 0)
		{
			return;
		}
		int num3 = Mathf.Max(1, cur.ItemDef.stackCount);
		int num4 = this.porterInventory.Data.CanAddItemCountToInventory(new Item(cur.id, totalCountInInventory), false, null, false) + num2 * num3;
		if (num4 > totalCountInInventory)
		{
			num4 = totalCountInInventory;
		}
		this.PorterAddTakenItems(inventory, cur.id, num4);
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000C048C File Offset: 0x000BE68C
	private void PorterAddTakenItems(Inventory sourceInventory, string itemId, int countToTake)
	{
		if (countToTake <= 0)
		{
			return;
		}
		List<Item> list;
		this.porterInventory.TryAddItemToInventory(new Item(itemId, countToTake), out list, null, false);
		foreach (Item item in list)
		{
			sourceInventory.RemoveItemById(item.id, item.Count, null, null, false);
		}
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x000C0508 File Offset: 0x000BE708
	private bool IsPorterEnabledRegularItem(NeedItemData item)
	{
		return this.attachedWgoData.GetGameResInt(item.id) == 1 && item.ItemDef != null && item.ItemDef.itemSize != ItemSize.Big;
	}

	// Token: 0x060028DE RID: 10462 RVA: 0x000C053C File Offset: 0x000BE73C
	private int GetPorterRemainingRegularItemsWithStock(PorterStationDef stationDef, int startIndex)
	{
		int num = 0;
		for (int i = startIndex; i < stationDef.items.Count; i++)
		{
			NeedItemData needItemData = stationDef.items[i];
			if (this.IsPorterEnabledRegularItem(needItemData) && this.GetPorterSourceItemCount(needItemData.id) > 0)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x000C058C File Offset: 0x000BE78C
	private int GetPorterSourceItemCount(string itemId)
	{
		int num = 0;
		for (int i = 0; i < base.WorldZoneData.MultiInventoryWgoDatas.Count; i++)
		{
			num += base.WorldZoneData.MultiInventoryWgoDatas[i].Inventory.Data.GetTotalCountInInventory(itemId, null, false);
		}
		return num;
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x000C05E0 File Offset: 0x000BE7E0
	private Inventory GetInventoryWithSpacePreferringSameItem(List<Inventory> inventories, Item item)
	{
		Inventory inventory = null;
		foreach (Inventory inventory2 in inventories)
		{
			if (inventory2.Data.CanAddItemCountToInventory(item, true, null, false) > 0)
			{
				if (inventory2.Data.HasItemByItemId(item.id, 1))
				{
					return inventory2;
				}
				if (inventory == null)
				{
					inventory = inventory2;
				}
			}
		}
		return inventory;
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x000C065C File Offset: 0x000BE85C
	private int TryAddItemPreferringSameItem(List<Inventory> inventories, Item item)
	{
		int num = 0;
		while (item.Count > 0)
		{
			Inventory inventoryWithSpacePreferringSameItem = this.GetInventoryWithSpacePreferringSameItem(inventories, item);
			if (inventoryWithSpacePreferringSameItem == null)
			{
				break;
			}
			if (item.Definition.stackCount == 1)
			{
				int count = item.Count;
				inventoryWithSpacePreferringSameItem.AddItemToInventory(item, null, false);
				int num2 = count - item.Count;
				if (num2 <= 0)
				{
					break;
				}
				num += num2;
			}
			else
			{
				int num3 = inventoryWithSpacePreferringSameItem.Data.CanAddItemCountToInventory(item, true, null, false);
				if (num3 > item.Count)
				{
					num3 = item.Count;
				}
				if (num3 <= 0)
				{
					break;
				}
				Item item2 = item.Split(num3, false);
				inventoryWithSpacePreferringSameItem.AddItemToInventory(item2, null, false);
				int num4 = num3 - item2.Count;
				if (item2.Count > 0)
				{
					item.Count += item2.Count;
				}
				if (num4 <= 0)
				{
					break;
				}
				num += num4;
			}
		}
		return num;
	}

	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x060028E2 RID: 10466 RVA: 0x000C0726 File Offset: 0x000BE926
	private OrderBase GardenerExecutingOrder
	{
		get
		{
			if (!this.gardenerExecutingOrder.IsEmpty)
			{
				return base.WorldZoneData.FindOrder(this.gardenerExecutingOrder);
			}
			return null;
		}
	}

	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000C0748 File Offset: 0x000BE948
	private bool HasGardenerExecutingOrder
	{
		get
		{
			return !this.gardenerExecutingOrder.IsEmpty;
		}
	}

	// Token: 0x17000695 RID: 1685
	// (get) Token: 0x060028E4 RID: 10468 RVA: 0x000C0758 File Offset: 0x000BE958
	private WgoData GardenerCurrentTarget
	{
		get
		{
			return MainGame.WorldData.GetWgoData(this.gardenerCurrentTargetUniqueId);
		}
	}

	// Token: 0x17000696 RID: 1686
	// (get) Token: 0x060028E5 RID: 10469 RVA: 0x000C076A File Offset: 0x000BE96A
	public SGuid GardenerCurrentTargetUniqueId
	{
		get
		{
			return this.gardenerCurrentTargetUniqueId;
		}
	}

	// Token: 0x17000697 RID: 1687
	// (get) Token: 0x060028E6 RID: 10470 RVA: 0x000C0772 File Offset: 0x000BE972
	public bool ShouldShowNoStorageIcon
	{
		get
		{
			return this.zombieType == ZombieType.Gardener && this.gardenerState == ZombieWgoData.ZombieGardenerState.CanNotPutItemToInventory;
		}
	}

	// Token: 0x17000698 RID: 1688
	// (get) Token: 0x060028E7 RID: 10471 RVA: 0x000C0789 File Offset: 0x000BE989
	// (set) Token: 0x060028E8 RID: 10472 RVA: 0x000C0794 File Offset: 0x000BE994
	public ZombieWgoData.ZombieGardenerState GardenerState
	{
		get
		{
			return this.gardenerState;
		}
		set
		{
			if (this.gardenerState == value)
			{
				return;
			}
			bool shouldShowNoStorageIcon = this.ShouldShowNoStorageIcon;
			this.gardenerPreviousState = this.gardenerState;
			this.gardenerState = value;
			Action onCaretakerStateChanged = this.OnCaretakerStateChanged;
			if (onCaretakerStateChanged != null)
			{
				onCaretakerStateChanged();
			}
			if (shouldShowNoStorageIcon != this.ShouldShowNoStorageIcon)
			{
				this.UpdateOwnWgoViewWidgets();
			}
		}
	}

	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x060028E9 RID: 10473 RVA: 0x000BE834 File Offset: 0x000BCA34
	// (set) Token: 0x060028EA RID: 10474 RVA: 0x000C07E4 File Offset: 0x000BE9E4
	public Item GardenerPortableItem
	{
		get
		{
			return this.caretakerPortableItem;
		}
		set
		{
			if (value.IsEmpty)
			{
				if (this.caretakerPortableItem.id != "empty")
				{
					if (this.caretakerPortableItem.Definition.itemSize == ItemSize.Big)
					{
						Action onRemoveOverheadItem = this.OnRemoveOverheadItem;
						if (onRemoveOverheadItem != null)
						{
							onRemoveOverheadItem();
						}
					}
					else
					{
						Action onRemoveInteractingItem = this.OnRemoveInteractingItem;
						if (onRemoveInteractingItem != null)
						{
							onRemoveInteractingItem();
						}
					}
				}
			}
			else if (value.Definition.itemSize == ItemSize.Big)
			{
				Action<Item, bool> onSetOverheadItem = this.OnSetOverheadItem;
				if (onSetOverheadItem != null)
				{
					onSetOverheadItem(value, true);
				}
			}
			else
			{
				Action<Item> onSetInteractingItem = this.OnSetInteractingItem;
				if (onSetInteractingItem != null)
				{
					onSetInteractingItem(value);
				}
			}
			this.caretakerPortableItem = value;
		}
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x000C0888 File Offset: 0x000BEA88
	private void GardenerUpdateBehaviour(float deltaTime)
	{
		switch (this.GardenerState)
		{
		case ZombieWgoData.ZombieGardenerState.OnStation:
			this.GardenerTryGetNewOrder(null, null);
			return;
		case ZombieWgoData.ZombieGardenerState.GoToStation:
		case ZombieWgoData.ZombieGardenerState.GoToGardenBedToPlantSeeds:
		case ZombieWgoData.ZombieGardenerState.GoToGardenBedToTakePlants:
			break;
		case ZombieWgoData.ZombieGardenerState.TeleportSeedsFromMultiInventory:
			this.GardenerTryTakeSeedsFromMultiInventory();
			return;
		case ZombieWgoData.ZombieGardenerState.PlantingSeeds:
			if ((this.AttachedWgoData != null && this.attachedWgoData.CraftComponent.CurrentCraftElement != null && !this.attachedWgoData.CraftComponent.CurrentCraftElement.Def.id.Contains("_planting")) || this.AttachedWgoData == null || this.AttachedWgoData.CraftComponent.CurrentCraftElement == null)
			{
				this.GardenerStopCraftActivity(false);
				return;
			}
			break;
		case ZombieWgoData.ZombieGardenerState.GatheringPlants:
			if (this.AttachedWgoData != null && this.AttachedWgoData.HpComponent.isDeathDelayed)
			{
				this.GardenerState = ZombieWgoData.ZombieGardenerState.WaitingForWgoDeath;
				return;
			}
			if (this.AttachedWgoData == null || (this.AttachedWgoData.HpComponent.Hp == 0 && !this.AttachedWgoData.HpComponent.isDeathDelayed))
			{
				this.GardenerStopWorkActivity(null, false);
				return;
			}
			if (this.ZombieHPActivity == null)
			{
				if (this.GardenerExecutingOrder != null)
				{
					this.GardenerStartWorkActivity(true);
					return;
				}
				this.GardenerTryStopOrderExecution();
				this.GardenerTryGetNewOrderOrMoveToStation();
				return;
			}
			break;
		case ZombieWgoData.ZombieGardenerState.WaitingForWgoDeath:
			if (this.AttachedWgoData == null || !this.AttachedWgoData.HpComponent.isDeathDelayed)
			{
				if (this.ZombieHPActivity != null)
				{
					this.GardenerStopWorkActivity(null, false);
					return;
				}
				if (this.HasGardenerExecutingOrder)
				{
					OrderBase orderBase = this.GardenerExecutingOrder;
					this.GardenerTryStopOrderExecution();
					if (orderBase != null)
					{
						base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
					}
				}
				this.GardenerTryGetNewOrderOrMoveToStation();
				return;
			}
			break;
		case ZombieWgoData.ZombieGardenerState.FailedToFindPath:
			this.gardenerTimeForCheckFailedPathAgain -= deltaTime;
			this.GardenerTryMoveToCurrentTarget();
			return;
		case ZombieWgoData.ZombieGardenerState.CanNotPutItemToInventory:
			this.GardenerTryPutGardenItemsToMultiInventory();
			break;
		default:
			return;
		}
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x000C0A38 File Offset: 0x000BEC38
	private void GardenerOnPathSuccess()
	{
		this.curAnimState = global::AnimationState.Idle;
		switch (this.GardenerState)
		{
		case ZombieWgoData.ZombieGardenerState.GoToStation:
		{
			GDPointData gdpointData = this.AttachedWgoData.GetGDPointData("zombie_garden_crafter_gd_point");
			base.Position = gdpointData.Position;
			this.direction.Value = gdpointData.Direction.ConvertToVector2XZ();
			this.GardenerTryPutGardenItemsToMultiInventory();
			return;
		}
		case ZombieWgoData.ZombieGardenerState.TeleportSeedsFromMultiInventory:
		case ZombieWgoData.ZombieGardenerState.PlantingSeeds:
			break;
		case ZombieWgoData.ZombieGardenerState.GoToGardenBedToPlantSeeds:
		{
			this.curAnimState = global::AnimationState.WorkHands;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged != null)
			{
				onAnimationStateChanged(this.curAnimState, false);
			}
			this.GardenerTryStartPlanting();
			return;
		}
		case ZombieWgoData.ZombieGardenerState.GoToGardenBedToTakePlants:
		{
			this.curAnimState = global::AnimationState.Idle;
			Action<global::AnimationState, bool> onAnimationStateChanged2 = this.OnAnimationStateChanged;
			if (onAnimationStateChanged2 != null)
			{
				onAnimationStateChanged2(this.curAnimState, false);
			}
			this.GardenerTryStartGathering();
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x000C0AF8 File Offset: 0x000BECF8
	private void GardenerTryGetNewOrder(Type orderType = null, SGuid target = null)
	{
		if (this.Hand.IsEmpty || this.Hand.Definition.type != ItemType.Shovel)
		{
			return;
		}
		if (!this.HasGardenerExecutingOrder)
		{
			OrderBase orderForGardener = base.WorldZoneData.GetOrderForGardener(this, orderType, target);
			if (orderForGardener != null)
			{
				this.gardenerExecutingOrder = orderForGardener.UniqueId;
				orderForGardener.ExecutorUniqueId = base.UniqueId;
				this.GardenerTryStartOrderExecutionOrGoToStation();
			}
		}
	}

	// Token: 0x060028EE RID: 10478 RVA: 0x000C0B5E File Offset: 0x000BED5E
	private void GardenerTryGetNewOrderOrMoveToStation()
	{
		this.GardenerTryGetNewOrder(null, null);
		if (!this.HasGardenerExecutingOrder)
		{
			this.GardenerTryMoveToStation();
		}
	}

	// Token: 0x060028EF RID: 10479 RVA: 0x000C0B76 File Offset: 0x000BED76
	private void GardenerTryGetNewOrderFromPrevTargetOrMoveToStation(Type orderType, SGuid target)
	{
		this.GardenerTryGetNewOrder(orderType, target);
		if (!this.HasGardenerExecutingOrder)
		{
			this.GardenerTryMoveToStation();
		}
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x000C0B90 File Offset: 0x000BED90
	private void GardenerOnOrderRemoved(OrderBase order)
	{
		if (order.UniqueId == this.gardenerExecutingOrder)
		{
			if (this.GardenerState == ZombieWgoData.ZombieGardenerState.FailedToFindPath)
			{
				this.GardenerState = this.gardenerPreviousState;
				this.gardenerTimeForCheckFailedPathAgain = 0f;
			}
			if (this.GardenerState == ZombieWgoData.ZombieGardenerState.CanNotPutItemToInventory)
			{
				this.GardenerState = this.gardenerPreviousState;
			}
			switch (this.GardenerState)
			{
			case ZombieWgoData.ZombieGardenerState.GoToGardenBedToPlantSeeds:
			case ZombieWgoData.ZombieGardenerState.GoToGardenBedToTakePlants:
				this.GardenerTryStopOrderExecution();
				this.GardenerTryGetNewOrderOrMoveToStation();
				return;
			case ZombieWgoData.ZombieGardenerState.PlantingSeeds:
				this.GardenerStopCraftActivity(true);
				this.GardenerTryStopOrderExecution();
				this.GardenerTryGetNewOrderOrMoveToStation();
				return;
			case ZombieWgoData.ZombieGardenerState.GatheringPlants:
			case ZombieWgoData.ZombieGardenerState.WaitingForWgoDeath:
				this.GardenerStopWorkActivity(null, true);
				this.GardenerTryStopOrderExecution();
				this.GardenerTryGetNewOrderOrMoveToStation();
				break;
			default:
				return;
			}
		}
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x000C0C44 File Offset: 0x000BEE44
	private void GardenerTryStartOrderExecutionOrGoToStation()
	{
		OrderBase orderBase = this.GardenerExecutingOrder;
		if (orderBase != null)
		{
			if (orderBase is PlantOrder)
			{
				this.GardenerState = ZombieWgoData.ZombieGardenerState.TeleportSeedsFromMultiInventory;
				this.GardenerTryTakeSeedsFromMultiInventory();
				return;
			}
			if (orderBase is GatherOrder)
			{
				this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToGardenBedToTakePlants;
				this.gardenerCurrentTargetUniqueId = orderBase.TargetWgoUniqueId;
				this.GardenerTryMoveToCurrentTarget();
				return;
			}
		}
		else
		{
			this.GardenerTryMoveToStation();
		}
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x000C0C9C File Offset: 0x000BEE9C
	private void GardenerTryTakeSeedsFromMultiInventory()
	{
		OrderBase orderBase = this.GardenerExecutingOrder;
		if (orderBase == null)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			this.GardenerTryMoveToStation();
			return;
		}
		WgoData wgoData = MainGame.WorldData.GetWgoData(orderBase.TargetWgoUniqueId);
		if (wgoData == null)
		{
			this.GardenerTryStopOrderExecution();
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			this.GardenerTryMoveToStation();
			return;
		}
		if (wgoData.CraftComponent.IsStarted)
		{
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToGardenBedToPlantSeeds;
			this.gardenerCurrentTargetUniqueId = orderBase.TargetWgoUniqueId;
			this.GardenerTryMoveToCurrentTarget();
			return;
		}
		string text;
		if (base.WorldZoneData.CanPlantOrderBeTakenOnExecution(orderBase as PlantOrder, out text, 0))
		{
			ItemDef data = GameBalance.Me.GetData<ItemDef>(text);
			int num = orderBase.Item.Count;
			foreach (WgoData wgoData2 in base.WorldZoneData.MultiInventoryWgoDatas)
			{
				if (wgoData2.Definition.inventoryWhiteList.Contains(data) && !wgoData2.Definition.inventoryBlackList.Contains(data))
				{
					int num2 = wgoData2.Inventory.Data.GetTotalCountInInventory(text, null, false);
					if (num2 > 0)
					{
						num2 = Mathf.Min(num2, num);
						num -= num2;
						this.WorkerInventory.AddItemsToInventory(wgoData2.Inventory.RemoveItemById(text, num2, null, null, false));
						if (num == 0)
						{
							this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToGardenBedToPlantSeeds;
							this.gardenerCurrentTargetUniqueId = orderBase.TargetWgoUniqueId;
							this.GardenerTryMoveToCurrentTarget();
							return;
						}
					}
				}
			}
		}
		this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
		this.GardenerTryMoveToStation();
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x000C0E44 File Offset: 0x000BF044
	private void GardenerTryStopOrderExecution()
	{
		if (!this.gardenerExecutingOrder.IsEmpty)
		{
			OrderBase orderBase = this.GardenerExecutingOrder;
			if (orderBase != null)
			{
				orderBase.ExecutorUniqueId = SGuid.Empty;
				this.GardenerClearBedWorkerIfMe(MainGame.WorldData.GetWgoData(orderBase.TargetWgoUniqueId));
			}
			else
			{
				this.GardenerClearBedWorkerIfMe(this.AttachedWgoData);
			}
			this.gardenerExecutingOrder = SGuid.Empty;
			return;
		}
		this.GardenerClearBedWorkerIfMe(this.AttachedWgoData);
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x000C0EAF File Offset: 0x000BF0AF
	private void GardenerClearBedWorkerIfMe(WgoData bed)
	{
		if (bed != null && GardenBedNavigation.IsGardenPlot(bed) && bed.Worker != null && bed.Worker.Id == base.UniqueId)
		{
			bed.ClearWorker();
		}
	}

	// Token: 0x060028F5 RID: 10485 RVA: 0x000C0EE4 File Offset: 0x000BF0E4
	private void GardenerTryStartPlanting()
	{
		if (!this.HasGardenerExecutingOrder)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			this.GardenerTryMoveToStation();
			return;
		}
		WgoData wgoData = MainGame.WorldData.GetWgoData(this.GardenerExecutingOrder.TargetWgoUniqueId);
		if (wgoData != null)
		{
			if (!wgoData.CraftComponent.IsStarted)
			{
				List<Item> itemsByGroupId = this.WorkerInventory.GetItemsByGroupId("seed");
				Item item = null;
				PlantOrder plantOrder = this.GardenerExecutingOrder as PlantOrder;
				if (plantOrder != null)
				{
					foreach (Item item2 in itemsByGroupId)
					{
						if (!plantOrder.isStarGroupItem && item2.id == plantOrder.Item.id)
						{
							item = item2;
							break;
						}
						if (plantOrder.isStarGroupItem && GameBalance.Me.starGroupItemsCache[plantOrder.Item.id].Contains(item2.Definition))
						{
							item = item2;
							break;
						}
					}
				}
				if (item == null || item.IsEmpty)
				{
					this.GardenerTryStopOrderExecution();
					this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
					this.GardenerTryMoveToStation();
					return;
				}
				CraftDefBase craftDefBase = GardenInteractionHandler.TryFindGardenCraft(item, wgoData, true);
				wgoData.TrySetWorker(this, null);
				GardenInteractionHandler.TryApplySeed(item, craftDefBase, wgoData);
				wgoData.ClearWorker();
			}
			this.GardenerStartCraftActivity(true);
			return;
		}
		OrderBase orderBase = this.GardenerExecutingOrder;
		this.GardenerTryStopOrderExecution();
		if (orderBase != null)
		{
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
		}
		this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
		this.GardenerTryMoveToStation();
	}

	// Token: 0x060028F6 RID: 10486 RVA: 0x000C1074 File Offset: 0x000BF274
	private void GardenerTryStartGathering()
	{
		if (!this.HasGardenerExecutingOrder)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			this.GardenerTryMoveToStation();
			return;
		}
		if (MainGame.WorldData.GetWgoData(this.GardenerExecutingOrder.TargetWgoUniqueId) != null)
		{
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GatheringPlants;
			this.GardenerStartWorkActivity(true);
			return;
		}
		OrderBase orderBase = this.GardenerExecutingOrder;
		this.GardenerTryStopOrderExecution();
		if (orderBase != null)
		{
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
		}
		this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
		this.GardenerTryMoveToStation();
	}

	// Token: 0x060028F7 RID: 10487 RVA: 0x000C10F4 File Offset: 0x000BF2F4
	public void GardenerStartCraftActivity(bool resetTicks = true)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(this.GardenerExecutingOrder.TargetWgoUniqueId);
		this.attachedWgoData = wgoData;
		this.attachedWgoDataUniqueId.SetGuid(wgoData.UniqueId);
		ZombieCraftActivity zombieCraftActivity = new ZombieCraftActivity(this.AttachedWgoData, this);
		this.currentActivity = zombieCraftActivity;
		this.ZombieCraftActivity.OnActiveStateChanged += this.GardenerOnCraftActivityStateChanged;
		this.GardenerOnCraftActivityStateChanged();
		this.attachedWgoData.TrySetWorker(this, null);
		MainGame.Instance.craftSystem.AddWorker(zombieCraftActivity);
		this.UpdateAttachedWgoViewWidgets();
		if (resetTicks)
		{
			this.AttachedWgoData.CraftComponent.ZombieSubTicks = 0;
		}
		this.AttachedWgoData.CraftComponent.TryContinueFromQueue();
		Vector3 vector;
		Direction direction;
		if (GardenBedNavigation.TryGetOpenApproach(this.AttachedWgoData, base.Position, out vector, out direction))
		{
			this.direction.Value = direction.ConvertToVector2XZ();
		}
		else
		{
			DockPointData nearestDockPointData = this.AttachedWgoData.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
			if (nearestDockPointData != null)
			{
				this.direction.Value = nearestDockPointData.Direction.ConvertToVector2XZ();
			}
		}
		this.GardenerState = ZombieWgoData.ZombieGardenerState.PlantingSeeds;
	}

	// Token: 0x060028F8 RID: 10488 RVA: 0x000C120C File Offset: 0x000BF40C
	public void GardenerStopCraftActivity(bool stopOnly = false)
	{
		ZombieCraftActivity zombieCraftActivity = this.ZombieCraftActivity ?? MainGame.Instance.craftSystem.TryGetCraftActivity(this);
		if (zombieCraftActivity != null)
		{
			MainGame.Instance.craftSystem.RemoveWorker(zombieCraftActivity);
			zombieCraftActivity.OnActiveStateChanged -= this.GardenerOnCraftActivityStateChanged;
			if (this.currentActivity == zombieCraftActivity)
			{
				this.curAnimState = global::AnimationState.Idle;
				Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
				if (onAnimationStateChanged != null)
				{
					onAnimationStateChanged(this.curAnimState, false);
				}
				this.currentActivity = null;
			}
			if (stopOnly)
			{
				return;
			}
			if (this.GardenerState == ZombieWgoData.ZombieGardenerState.PlantingSeeds)
			{
				this.GardenerTryExecutePlantOrder();
				this.GardenerTryMoveToStation();
				return;
			}
			this.GardenerTryGetNewOrderOrMoveToStation();
		}
	}

	// Token: 0x060028F9 RID: 10489 RVA: 0x000C12A8 File Offset: 0x000BF4A8
	private void GardenerOnCraftActivityStateChanged()
	{
		this.GardenerHandleCraftStatusChange(this.AttachedWgoData.CraftComponent.Status);
	}

	// Token: 0x060028FA RID: 10490 RVA: 0x000C12C0 File Offset: 0x000BF4C0
	private void GardenerHandleCraftStatusChange(CraftComponentStatus craftStatus)
	{
		if (craftStatus == CraftComponentStatus.Finished)
		{
			this.GardenerStopCraftActivity(false);
		}
		global::AnimationState animationState;
		if (this.ZombieCraftActivity != null && this.ZombieCraftActivity.IsActive)
		{
			if (craftStatus == CraftComponentStatus.Started)
			{
				animationState = (this.AttachedWgoData.Definition.isAutoCrafter ? global::AnimationState.Idle : this.CrafterGetAnimationStateForCraft(this.AttachedWgoData.CraftComponent.CurrentCraftElement.Def));
			}
			else
			{
				animationState = global::AnimationState.Idle;
			}
		}
		else
		{
			animationState = global::AnimationState.Idle;
		}
		if (this.curAnimState != animationState)
		{
			this.curAnimState = animationState;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, false);
		}
	}

	// Token: 0x060028FB RID: 10491 RVA: 0x000C1351 File Offset: 0x000BF551
	private void GardenerOnWorkActivityStateChanged()
	{
		this.GardenerHandleWorkStatusChange();
	}

	// Token: 0x060028FC RID: 10492 RVA: 0x000C135C File Offset: 0x000BF55C
	private void GardenerHandleWorkStatusChange()
	{
		if (this.ZombieHPActivity == null || !this.ZombieHPActivity.IsActive)
		{
			if (this.AttachedWgoData != null && this.AttachedWgoData.HpComponent.isDeathDelayed)
			{
				this.GardenerState = ZombieWgoData.ZombieGardenerState.WaitingForWgoDeath;
			}
			else if (this.AttachedWgoData != null && this.AttachedWgoData.HpComponent.Hp > 0)
			{
				this.GardenerStartWorkActivity(true);
			}
			else
			{
				this.GardenerStopWorkActivity(null, false);
			}
		}
		global::AnimationState animationState;
		if (this.ZombieHPActivity != null && this.ZombieHPActivity.IsActive)
		{
			animationState = ((!this.ZombieHPActivity.IsActive) ? global::AnimationState.Idle : this.GardenerGetAnimationStateForWork());
		}
		else
		{
			animationState = global::AnimationState.Idle;
		}
		if (this.curAnimState != animationState)
		{
			this.curAnimState = animationState;
			Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
			if (onAnimationStateChanged == null)
			{
				return;
			}
			onAnimationStateChanged(this.curAnimState, false);
		}
	}

	// Token: 0x060028FD RID: 10493 RVA: 0x000C1424 File Offset: 0x000BF624
	public void GardenerStartWorkActivity(bool resetTicks = true)
	{
		WgoData wgoData = MainGame.WorldData.GetWgoData(this.GardenerExecutingOrder.TargetWgoUniqueId);
		this.attachedWgoData = wgoData;
		this.attachedWgoDataUniqueId.SetGuid(wgoData.UniqueId);
		ZombieHPActivity zombieHPActivity = new ZombieHPActivity(this.AttachedWgoData, this);
		this.currentActivity = zombieHPActivity;
		this.ZombieHPActivity.OnActiveStateChanged += this.GardenerOnWorkActivityStateChanged;
		this.GardenerOnWorkActivityStateChanged();
		this.attachedWgoData.TrySetWorker(this, null);
		MainGame.Instance.craftSystem.AddHPWorker(zombieHPActivity);
		this.UpdateAttachedWgoViewWidgets();
		if (resetTicks)
		{
			this.AttachedWgoData.CraftComponent.ZombieSubTicks = 0;
		}
		this.AttachedWgoData.CraftComponent.TryContinueFromQueue();
		Vector3 vector;
		Direction direction;
		if (GardenBedNavigation.TryGetOpenApproach(this.AttachedWgoData, base.Position, out vector, out direction))
		{
			this.direction.Value = direction.ConvertToVector2XZ();
			return;
		}
		DockPointData nearestDockPointData = this.AttachedWgoData.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
		if (nearestDockPointData != null)
		{
			this.direction.Value = nearestDockPointData.Direction.ConvertToVector2XZ();
		}
	}

	// Token: 0x060028FE RID: 10494 RVA: 0x000C1534 File Offset: 0x000BF734
	public void GardenerStopWorkActivity(SGuid respawnedEmptyGardenBed = null, bool stopOnly = false)
	{
		ZombieHPActivity zombieHPActivity = this.ZombieHPActivity ?? MainGame.Instance.craftSystem.TryGetHPActivity(this);
		if (zombieHPActivity != null)
		{
			MainGame.Instance.craftSystem.RemoveHPWorker(zombieHPActivity);
			zombieHPActivity.OnActiveStateChanged -= this.GardenerOnWorkActivityStateChanged;
			if (this.currentActivity == zombieHPActivity)
			{
				this.curAnimState = global::AnimationState.Idle;
				Action<global::AnimationState, bool> onAnimationStateChanged = this.OnAnimationStateChanged;
				if (onAnimationStateChanged != null)
				{
					onAnimationStateChanged(this.curAnimState, false);
				}
				this.currentActivity = null;
			}
			if (stopOnly)
			{
				return;
			}
			if (this.GardenerState == ZombieWgoData.ZombieGardenerState.WaitingForWgoDeath)
			{
				this.GardenerTryExecuteGatherOrder();
				if (respawnedEmptyGardenBed != null)
				{
					this.GardenerTryGetNewOrderFromPrevTargetOrMoveToStation(typeof(PlantOrder), respawnedEmptyGardenBed);
					return;
				}
				this.GardenerTryGetNewOrderOrMoveToStation();
				return;
			}
			else
			{
				OrderBase orderBase = this.GardenerExecutingOrder;
				this.GardenerTryStopOrderExecution();
				if (orderBase != null)
				{
					base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
				}
				this.GardenerTryGetNewOrderOrMoveToStation();
			}
		}
	}

	// Token: 0x060028FF RID: 10495 RVA: 0x000C160F File Offset: 0x000BF80F
	private global::AnimationState GardenerGetAnimationStateForWork()
	{
		return (global::AnimationState)(this.AttachedWgoData.Definition.toolAction.actionableTool + 19);
	}

	// Token: 0x06002900 RID: 10496 RVA: 0x000C162C File Offset: 0x000BF82C
	private void GardenerTryExecutePlantOrder()
	{
		if (!this.HasGardenerExecutingOrder)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			return;
		}
		OrderBase orderBase = this.GardenerExecutingOrder;
		if (orderBase == null)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			return;
		}
		string text;
		if (orderBase.TryExecuteOrder(new ZombieGardenerOrderExecutor(this), out text))
		{
			this.GardenerTryStopOrderExecution();
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
			if (this.AttachedWgoData != null && this.AttachedWgoData.Worker != null && this.AttachedWgoData.Worker.Id == base.UniqueId)
			{
				this.AttachedWgoData.ClearWorker();
			}
			this.attachedWgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.gardenerStation);
			this.attachedWgoDataUniqueId.SetGuid(this.attachedWgoData.UniqueId);
			return;
		}
		this.GardenerTryStopOrderExecution();
		this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
	}

	// Token: 0x06002901 RID: 10497 RVA: 0x000C1718 File Offset: 0x000BF918
	private void GardenerTryExecuteGatherOrder()
	{
		if (!this.HasGardenerExecutingOrder)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			return;
		}
		OrderBase orderBase = this.GardenerExecutingOrder;
		if (orderBase == null)
		{
			this.GardenerTryStopOrderExecution();
			this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
			return;
		}
		string text;
		if (orderBase.TryExecuteOrder(new ZombieGardenerOrderExecutor(this), out text))
		{
			this.GardenerTryStopOrderExecution();
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
			if (this.AttachedWgoData != null && this.AttachedWgoData.Worker != null && this.AttachedWgoData.Worker.Id == base.UniqueId)
			{
				this.AttachedWgoData.ClearWorker();
			}
			this.attachedWgoData = null;
			this.attachedWgoDataUniqueId.SetGuid(this.gardenerStation);
			return;
		}
		this.GardenerTryStopOrderExecution();
		this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
	}

	// Token: 0x06002902 RID: 10498 RVA: 0x000C17E0 File Offset: 0x000BF9E0
	private void GardenerTryMoveToStation()
	{
		this.GardenerState = ZombieWgoData.ZombieGardenerState.GoToStation;
		if (this.AttachedWgoData == null || !this.AttachedWgoData.UniqueId.Equals(this.gardenerStation))
		{
			this.attachedWgoData = null;
			this.attachedWgoDataUniqueId.SetGuid(this.gardenerStation);
		}
		this.gardenerCurrentTargetUniqueId = this.gardenerStation;
		this.GardenerTryMoveToCurrentTarget();
	}

	// Token: 0x06002903 RID: 10499 RVA: 0x000C1840 File Offset: 0x000BFA40
	private void GardenerTryMoveToCurrentTarget()
	{
		SGuid sguid = this.gardenerCurrentMovementTargetUniqueId;
		Guid? guid = ((sguid != null) ? new Guid?(sguid.Guid) : null);
		SGuid sguid2 = this.gardenerCurrentTargetUniqueId;
		if (guid == ((sguid2 != null) ? new Guid?(sguid2.Guid) : null))
		{
			return;
		}
		if (this.GardenerState == ZombieWgoData.ZombieGardenerState.FailedToFindPath)
		{
			if (this.gardenerTimeForCheckFailedPathAgain <= 0f)
			{
				this.GardenerState = this.gardenerPreviousState;
				this.gardenerTimeForCheckFailedPathAgain = 0f;
				this.<GardenerTryMoveToCurrentTarget>g__MoveAction|306_0();
				return;
			}
		}
		else
		{
			this.<GardenerTryMoveToCurrentTarget>g__MoveAction|306_0();
		}
	}

	// Token: 0x06002904 RID: 10500 RVA: 0x000C18FC File Offset: 0x000BFAFC
	private void OnGardenerTargetMissing()
	{
		OrderBase orderBase = this.GardenerExecutingOrder;
		this.GardenerTryStopOrderExecution();
		if (orderBase != null)
		{
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
		}
		this.gardenerCurrentMovementTargetUniqueId = SGuid.Empty;
		if (!this.gardenerCurrentTargetUniqueId.Equals(this.gardenerStation))
		{
			this.GardenerTryMoveToStation();
			return;
		}
		if (this.GardenerState != ZombieWgoData.ZombieGardenerState.OnStation)
		{
			this.GardenerState = ZombieWgoData.ZombieGardenerState.OnStation;
		}
	}

	// Token: 0x06002905 RID: 10501 RVA: 0x000C1960 File Offset: 0x000BFB60
	private Vector3 GetGardenerDockPosition(WgoData target)
	{
		WgoPartData mainWgoPartData = target.MainWgoPartData;
		DockPointData dockPointData3;
		if (mainWgoPartData == null)
		{
			dockPointData3 = null;
		}
		else
		{
			dockPointData3 = mainWgoPartData.GetNearestDockPoint(target, base.Position, DockPointData.Availability.OnlyNotOccupied, DockPointData.Filter.All, (DockPointData dp, Vector3 parentPos) => !dp.BakedData.DisableTargetingForCaretaker);
		}
		DockPointData dockPointData2 = dockPointData3;
		if (((dockPointData2 != null) ? dockPointData2.BakedData : null) != null)
		{
			return target.GetDockPointDataWorldPosition(dockPointData2);
		}
		return target.GetNearestDockPointDataWorldPositionOrMyPosition(base.Position, DockPointData.Availability.All, (DockPointData dockPointData, Vector3 parentPos) => !dockPointData.BakedData.DisableTargetingForCaretaker);
	}

	// Token: 0x06002906 RID: 10502 RVA: 0x000C19EC File Offset: 0x000BFBEC
	private void GardenerTryPutGardenItemsToMultiInventory()
	{
		List<Item> list = new List<Item>();
		list.AddRange(this.WorkerInventory.GetItemsByGroupId("seed"));
		list.AddRange(this.WorkerInventory.GetItemsByGroupId("seedable"));
		list.AddRange(this.WorkerInventory.GetItemsByGroupId("crop"));
		MultiInventory multiInventory = new MultiInventory(base.WorldZoneData, null, false);
		bool flag = true;
		foreach (Item item in list)
		{
			int depositableCount = this.GetDepositableCount(multiInventory.inventoryList, item);
			if (depositableCount < item.Count)
			{
				flag = false;
			}
			if (depositableCount > 0)
			{
				foreach (Item item2 in this.WorkerInventory.RemoveItemById(item.id, depositableCount, null, null, false))
				{
					this.TryAddItemPreferringSameItem(multiInventory.inventoryList, item2);
					if (item2.Count > 0)
					{
						this.WorkerInventory.AddItemToInventory(item2, null, false);
						flag = false;
					}
				}
			}
		}
		this.GardenerState = (flag ? ZombieWgoData.ZombieGardenerState.OnStation : ZombieWgoData.ZombieGardenerState.CanNotPutItemToInventory);
	}

	// Token: 0x06002907 RID: 10503 RVA: 0x000C1B38 File Offset: 0x000BFD38
	private int GetDepositableCount(List<Inventory> inventories, Item item)
	{
		int num = item.Count;
		int num2 = 0;
		foreach (Inventory inventory in inventories)
		{
			if (num <= 0)
			{
				break;
			}
			int num3 = inventory.Data.CanAddItemCountToInventory(item.Definition, num, true, null, false);
			if (num3 > 0)
			{
				num2 += num3;
				num -= num3;
			}
		}
		return num2;
	}

	// Token: 0x06002908 RID: 10504 RVA: 0x000C1BB8 File Offset: 0x000BFDB8
	private bool GardenerIsAnyMultiInventoryWithSpaceForPortableItemExists()
	{
		using (List<WgoData>.Enumerator enumerator = base.WorldZoneData.MultiInventoryWgoDatas.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Inventory.Data.CanAddItemToInventory(this.CaretakerPortableItem, true, false))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002909 RID: 10505 RVA: 0x000C1C28 File Offset: 0x000BFE28
	public void GardenerOnToolChanged()
	{
		if (this.Hand.IsEmpty || this.Hand.Definition.type != ItemType.Shovel)
		{
			this.GardenerAbortWorkBecauseNoShovel();
		}
		this.UpdateAttachedWgoViewWidgets();
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x000C1C58 File Offset: 0x000BFE58
	private void GardenerAbortWorkBecauseNoShovel()
	{
		this.GardenerStopWorkActivity(null, true);
		this.GardenerStopCraftActivity(true);
		WgoData wgoData = this.AttachedWgoData;
		bool flag = wgoData != null && wgoData.UniqueId.Equals(this.gardenerStation);
		if (GardenBedNavigation.IsGardenPlot(wgoData))
		{
			wgoData.ClearWorker();
		}
		this.GardenerTryStopOrderExecution();
		if (this.GardenerState == ZombieWgoData.ZombieGardenerState.OnStation && flag)
		{
			return;
		}
		this.GardenerTryMoveToStation();
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x0600290B RID: 10507 RVA: 0x000C1CBB File Offset: 0x000BFEBB
	private OrderBase ConveyorTransporterExecutingOrder
	{
		get
		{
			if (!this.conveyorTransporterExecutingOrder.IsEmpty)
			{
				return base.WorldZoneData.FindOrder(this.conveyorTransporterExecutingOrder);
			}
			return null;
		}
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x0600290C RID: 10508 RVA: 0x000C1CDD File Offset: 0x000BFEDD
	private bool HasConveyorTransporterExecutingOrder
	{
		get
		{
			return !this.conveyorTransporterExecutingOrder.IsEmpty;
		}
	}

	// Token: 0x1700069C RID: 1692
	// (get) Token: 0x0600290D RID: 10509 RVA: 0x000C1CED File Offset: 0x000BFEED
	private WgoData ConveyorTransporterCurrentTarget
	{
		get
		{
			if (!this.conveyorTransporterCurrentTargetUniqueId.IsEmpty)
			{
				return MainGame.WorldData.GetWgoData(this.conveyorTransporterCurrentTargetUniqueId);
			}
			return null;
		}
	}

	// Token: 0x1700069D RID: 1693
	// (get) Token: 0x0600290E RID: 10510 RVA: 0x000C1D0E File Offset: 0x000BFF0E
	public SGuid ConveyorTransporterCurrentTargetUniqueId
	{
		get
		{
			return this.conveyorTransporterCurrentTargetUniqueId;
		}
	}

	// Token: 0x1700069E RID: 1694
	// (get) Token: 0x0600290F RID: 10511 RVA: 0x000C1D16 File Offset: 0x000BFF16
	// (set) Token: 0x06002910 RID: 10512 RVA: 0x000C1D1E File Offset: 0x000BFF1E
	public ZombieWgoData.ZombieConveyorTransporterState ConveyorTransporterState
	{
		get
		{
			return this.conveyorTransporterState;
		}
		set
		{
			this.conveyorTransporterPreviousState = this.conveyorTransporterState;
			this.conveyorTransporterState = value;
			Action onCaretakerStateChanged = this.OnCaretakerStateChanged;
			if (onCaretakerStateChanged == null)
			{
				return;
			}
			onCaretakerStateChanged();
		}
	}

	// Token: 0x1700069F RID: 1695
	// (get) Token: 0x06002911 RID: 10513 RVA: 0x000C1D43 File Offset: 0x000BFF43
	// (set) Token: 0x06002912 RID: 10514 RVA: 0x000C1D4C File Offset: 0x000BFF4C
	public Item ConveyorTransporterPortableItem
	{
		get
		{
			return this.conveyorTransporterPortableItem;
		}
		set
		{
			if (value.IsEmpty)
			{
				if (this.conveyorTransporterPortableItem.id != "empty")
				{
					if (this.conveyorTransporterPortableItem.Definition.itemSize == ItemSize.Big)
					{
						Action onRemoveOverheadItem = this.OnRemoveOverheadItem;
						if (onRemoveOverheadItem != null)
						{
							onRemoveOverheadItem();
						}
					}
					else
					{
						Action onRemoveInteractingItem = this.OnRemoveInteractingItem;
						if (onRemoveInteractingItem != null)
						{
							onRemoveInteractingItem();
						}
					}
				}
			}
			else if (value.Definition.itemSize == ItemSize.Big)
			{
				Action<Item, bool> onSetOverheadItem = this.OnSetOverheadItem;
				if (onSetOverheadItem != null)
				{
					onSetOverheadItem(value, true);
				}
			}
			else
			{
				Action<Item> onSetInteractingItem = this.OnSetInteractingItem;
				if (onSetInteractingItem != null)
				{
					onSetInteractingItem(value);
				}
			}
			this.conveyorTransporterPortableItem = value;
		}
	}

	// Token: 0x06002913 RID: 10515 RVA: 0x000C1DF0 File Offset: 0x000BFFF0
	private void ConveyorTransporterUpdateBehaviour(float deltaTime)
	{
		switch (this.ConveyorTransporterState)
		{
		case ZombieWgoData.ZombieConveyorTransporterState.OnStation:
		case ZombieWgoData.ZombieConveyorTransporterState.GoToStation:
			this.ConveyorTransporterTryGetNewOrder();
			return;
		case ZombieWgoData.ZombieConveyorTransporterState.GoToStationCellToPickUp:
		case ZombieWgoData.ZombieConveyorTransporterState.GoToStorageToPutItem:
			break;
		case ZombieWgoData.ZombieConveyorTransporterState.FailedToFindPath:
			this.conveyorTransporterTimeForCheckFailedPathAgain -= deltaTime;
			this.ConveyorTransporterTryMoveToCurrentTarget();
			return;
		case ZombieWgoData.ZombieConveyorTransporterState.CanNotPutItemToInventory:
			if (this.conveyorTransporterPreviousState == ZombieWgoData.ZombieConveyorTransporterState.GoToStorageToPutItem)
			{
				this.ConveyorTransporterState = this.conveyorTransporterPreviousState;
				this.ConveyorTransporterTryPutPortableItemToInventory();
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x000C1E5C File Offset: 0x000C005C
	private void ConveyorTransporterOnPathSuccess()
	{
		this.curAnimState = global::AnimationState.Idle;
		switch (this.ConveyorTransporterState)
		{
		case ZombieWgoData.ZombieConveyorTransporterState.GoToStation:
		{
			this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.OnStation;
			GDPointData gdpointData = this.AttachedWgoData.GetGDPointData("zombie_porter_station_gd_point");
			base.Position = gdpointData.Position;
			this.direction.Value = gdpointData.Direction.ConvertToVector2XZ();
			return;
		}
		case ZombieWgoData.ZombieConveyorTransporterState.GoToStationCellToPickUp:
			this.ConveyorTransporterTryExecutePickUpOrder();
			return;
		case ZombieWgoData.ZombieConveyorTransporterState.GoToStorageToPutItem:
			this.ConveyorTransporterTryPutPortableItemToInventory();
			return;
		default:
			return;
		}
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x000C1ED4 File Offset: 0x000C00D4
	private void ConveyorTransporterTryGetNewOrder()
	{
		if (this.HasConveyorTransporterExecutingOrder)
		{
			return;
		}
		OrderBase orderForConveyorTransporter = base.WorldZoneData.GetOrderForConveyorTransporter();
		if (orderForConveyorTransporter == null)
		{
			if (this.ConveyorTransporterState != ZombieWgoData.ZombieConveyorTransporterState.OnStation && this.ConveyorTransporterState != ZombieWgoData.ZombieConveyorTransporterState.GoToStation)
			{
				this.ConveyorTransporterTryMoveToStation();
			}
			return;
		}
		this.conveyorTransporterExecutingOrder = orderForConveyorTransporter.UniqueId;
		orderForConveyorTransporter.ExecutorUniqueId = base.UniqueId;
		this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.GoToStationCellToPickUp;
		this.conveyorTransporterCurrentTargetUniqueId = orderForConveyorTransporter.TargetWgoUniqueId;
		if (base.MovementComponent.IsMoving)
		{
			base.MovementComponent.ForceStop();
		}
		this.conveyorTransporterCurrentMovementTargetUniqueId = SGuid.Empty;
		this.ConveyorTransporterTryMoveToCurrentTarget();
	}

	// Token: 0x06002916 RID: 10518 RVA: 0x000C1F65 File Offset: 0x000C0165
	private void ConveyorTransporterTryStopOrderExecution()
	{
		if (!this.conveyorTransporterExecutingOrder.IsEmpty)
		{
			if (this.ConveyorTransporterExecutingOrder != null)
			{
				this.ConveyorTransporterExecutingOrder.ExecutorUniqueId = SGuid.Empty;
			}
			this.conveyorTransporterExecutingOrder = SGuid.Empty;
		}
	}

	// Token: 0x06002917 RID: 10519 RVA: 0x000C1F98 File Offset: 0x000C0198
	private void ConveyorTransporterTryExecutePickUpOrder()
	{
		if (!this.HasConveyorTransporterExecutingOrder)
		{
			this.ConveyorTransporterTryMoveToStation();
			return;
		}
		string text;
		if (this.ConveyorTransporterExecutingOrder.TryExecuteOrder(new ZombieConveyorTransporterOrderExecutor(this), out text))
		{
			OrderBase orderBase = this.ConveyorTransporterExecutingOrder;
			this.ConveyorTransporterTryStopOrderExecution();
			base.WorldZoneData.RemoveOrder(orderBase.UniqueId);
			this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.GoToStorageToPutItem;
			this.ConveyorTransporterTryMoveToNearestStorageForPortableItem();
			return;
		}
		this.ConveyorTransporterTryStopOrderExecution();
		this.ConveyorTransporterTryMoveToStation();
	}

	// Token: 0x06002918 RID: 10520 RVA: 0x000C2004 File Offset: 0x000C0204
	private void ConveyorTransporterTryPutPortableItemToInventory()
	{
		if (this.ConveyorTransporterCurrentTarget == null)
		{
			if (this.ConveyorTransporterGetNearestStorageWithSpace() != null)
			{
				this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.GoToStorageToPutItem;
				this.ConveyorTransporterTryMoveToNearestStorageForPortableItem();
				return;
			}
			this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.CanNotPutItemToInventory;
			return;
		}
		else
		{
			this.ConveyorTransporterCurrentTarget.Inventory.AddItemToInventory(this.ConveyorTransporterPortableItem, null, false);
			if (this.ConveyorTransporterPortableItem.Count <= 0)
			{
				this.ConveyorTransporterPortableItem = Item.Empty;
				this.ConveyorTransporterTryMoveToStation();
				return;
			}
			if (this.ConveyorTransporterGetNearestStorageWithSpace() != null)
			{
				this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.GoToStorageToPutItem;
				this.ConveyorTransporterTryMoveToNearestStorageForPortableItem();
				return;
			}
			this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.CanNotPutItemToInventory;
			return;
		}
	}

	// Token: 0x06002919 RID: 10521 RVA: 0x000C2090 File Offset: 0x000C0290
	private WgoData ConveyorTransporterGetNearestStorageWithSpace()
	{
		float num = float.MaxValue;
		WgoData wgoData = null;
		WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById("conveyor_storage");
		if (worldZoneDataById == null)
		{
			return null;
		}
		foreach (WgoData wgoData2 in worldZoneDataById.MultiInventoryWgoDatas)
		{
			if (wgoData2.Inventory.CanAddItemToInventory(this.ConveyorTransporterPortableItem))
			{
				float num2 = Mathf.Abs(Vector3.Distance(wgoData2.Position, base.Position));
				if (num2 < num)
				{
					num = num2;
					wgoData = wgoData2;
				}
			}
		}
		return wgoData;
	}

	// Token: 0x0600291A RID: 10522 RVA: 0x000C2134 File Offset: 0x000C0334
	private void ConveyorTransporterTryMoveToNearestStorageForPortableItem()
	{
		WgoData wgoData = this.ConveyorTransporterGetNearestStorageWithSpace();
		if (wgoData == null)
		{
			this.conveyorTransporterCurrentTargetUniqueId = SGuid.Empty;
			this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.CanNotPutItemToInventory;
			return;
		}
		this.conveyorTransporterCurrentTargetUniqueId = wgoData.UniqueId;
		this.ConveyorTransporterTryMoveToCurrentTarget();
	}

	// Token: 0x0600291B RID: 10523 RVA: 0x000C2170 File Offset: 0x000C0370
	private void ConveyorTransporterTryMoveToStation()
	{
		this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.GoToStation;
		this.conveyorTransporterCurrentTargetUniqueId = this.AttachedWgoData.UniqueId;
		this.ConveyorTransporterTryMoveToCurrentTarget();
	}

	// Token: 0x0600291C RID: 10524 RVA: 0x000C2190 File Offset: 0x000C0390
	private void ConveyorTransporterTryMoveToCurrentTarget()
	{
		SGuid sguid = this.conveyorTransporterCurrentMovementTargetUniqueId;
		Guid? guid = ((sguid != null) ? new Guid?(sguid.Guid) : null);
		SGuid sguid2 = this.conveyorTransporterCurrentTargetUniqueId;
		if (guid == ((sguid2 != null) ? new Guid?(sguid2.Guid) : null))
		{
			return;
		}
		if (this.ConveyorTransporterState == ZombieWgoData.ZombieConveyorTransporterState.FailedToFindPath)
		{
			if (this.conveyorTransporterTimeForCheckFailedPathAgain <= 0f)
			{
				this.ConveyorTransporterState = this.conveyorTransporterPreviousState;
				this.conveyorTransporterTimeForCheckFailedPathAgain = 0f;
				this.<ConveyorTransporterTryMoveToCurrentTarget>g__MoveAction|345_0();
				return;
			}
		}
		else
		{
			this.<ConveyorTransporterTryMoveToCurrentTarget>g__MoveAction|345_0();
		}
	}

	// Token: 0x0600291D RID: 10525 RVA: 0x000C224C File Offset: 0x000C044C
	private void ConveyorTransporterOnOrderRemoved(OrderBase order)
	{
		if (order.UniqueId != this.conveyorTransporterExecutingOrder)
		{
			return;
		}
		if (this.ConveyorTransporterState == ZombieWgoData.ZombieConveyorTransporterState.FailedToFindPath)
		{
			this.ConveyorTransporterState = this.conveyorTransporterPreviousState;
			this.conveyorTransporterTimeForCheckFailedPathAgain = 0f;
		}
		if (this.ConveyorTransporterState == ZombieWgoData.ZombieConveyorTransporterState.CanNotPutItemToInventory)
		{
			this.ConveyorTransporterState = this.conveyorTransporterPreviousState;
		}
		if (this.ConveyorTransporterState == ZombieWgoData.ZombieConveyorTransporterState.GoToStationCellToPickUp)
		{
			this.ConveyorTransporterTryStopOrderExecution();
			this.ConveyorTransporterTryMoveToStation();
		}
	}

	// Token: 0x0600291E RID: 10526 RVA: 0x000C22B7 File Offset: 0x000C04B7
	public void FighterOnEquipmentChange()
	{
		Action<Inventory> onEquipmentChanged = this.OnEquipmentChanged;
		if (onEquipmentChanged == null)
		{
			return;
		}
		onEquipmentChanged(new Inventory(this.zombieItem));
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x000C22D4 File Offset: 0x000C04D4
	[CompilerGenerated]
	private void <CaretakerTryMoveToCurrentTarget>g__MoveAction|223_0()
	{
		DockPointData nearestDockPointData = this.CaretakerCurrentTarget.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
		if (base.MovementComponent.IsMoving)
		{
			base.MovementComponent.ForceStop();
		}
		if (this.CaretakerState == ZombieWgoData.ZombieCaretakerState.OnStation)
		{
			base.Position = this.AttachedWgoData.GetNearestDockPointDataWorldPositionOrMyPosition(base.Position, DockPointData.Availability.OnlyOccupied, (DockPointData dockPointData, Vector3 parentPos) => !dockPointData.BakedData.DisableTargetingForCaretaker);
			VariableNotificator<Vector2> direction = this.direction;
			DockPointData nearestDockPointData2 = this.AttachedWgoData.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
			direction.Value = ((nearestDockPointData2 != null) ? nearestDockPointData2.Direction.ConvertToVector2XZ() : Direction.Down.ConvertToVector2XZ());
		}
		Vector3 nearestDockPointDataWorldPositionOrMyPosition = this.CaretakerCurrentTarget.GetNearestDockPointDataWorldPositionOrMyPosition(base.Position, DockPointData.Availability.OnlyOccupied, (DockPointData dockPointData, Vector3 parentPos) => !dockPointData.BakedData.DisableTargetingForCaretaker);
		WorldZoneData worldZoneData = base.WorldZoneData;
		IEnumerable<LazyConsts.Navigation.Graph> enumerable = ((worldZoneData != null) ? worldZoneData.MovementGraphs : null);
		WorldZoneData worldZoneData2 = base.WorldZoneData;
		GraphMask graphMask = NavigationGraphMaskUtils.ToGraphMask(enumerable, (worldZoneData2 != null) ? worldZoneData2.navigationGraph : LazyConsts.Navigation.Graph.None);
		MovementComponent.StartPathResult startPathResult;
		if (nearestDockPointData == null)
		{
			startPathResult = base.MovementComponent.StartPath(nearestDockPointDataWorldPositionOrMyPosition, graphMask, base.WorldId, 1.5f, "", null, MovementComponent.DestinationType.Position);
		}
		else
		{
			startPathResult = base.MovementComponent.StartPath(nearestDockPointDataWorldPositionOrMyPosition, nearestDockPointData.BakedData, graphMask, base.WorldId, 1.5f, "", null);
		}
		switch (startPathResult)
		{
		case MovementComponent.StartPathResult.Started:
			this.caretakerCurrentMovementTargetUniqueId = this.caretakerCurrentTargetUniqueId;
			return;
		case MovementComponent.StartPathResult.AlreadyAtDestinationPoint:
			this.CaretakerOnPathSuccess();
			return;
		case MovementComponent.StartPathResult.IncorrectMovementType:
			this.caretakerTimeForCheckFailedPathAgain = 1f;
			this.CaretakerState = ZombieWgoData.ZombieCaretakerState.FailedToFindPath;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06002920 RID: 10528 RVA: 0x000C2468 File Offset: 0x000C0668
	[CompilerGenerated]
	private void <GardenerTryMoveToCurrentTarget>g__MoveAction|306_0()
	{
		if (base.MovementComponent.IsMoving)
		{
			base.MovementComponent.ForceStop();
		}
		if (this.GardenerState == ZombieWgoData.ZombieGardenerState.OnStation)
		{
			base.Position = this.AttachedWgoData.GetNearestDockPointDataWorldPositionOrMyPosition(base.Position, DockPointData.Availability.All, (DockPointData dockPointData, Vector3 parentPos) => !dockPointData.BakedData.DisableTargetingForCaretaker);
			VariableNotificator<Vector2> direction = this.direction;
			DockPointData nearestDockPointData = this.AttachedWgoData.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
			direction.Value = ((nearestDockPointData != null) ? nearestDockPointData.Direction.ConvertToVector2XZ() : Direction.Down.ConvertToVector2XZ());
		}
		WgoData gardenerCurrentTarget = this.GardenerCurrentTarget;
		if (gardenerCurrentTarget == null)
		{
			this.OnGardenerTargetMissing();
			return;
		}
		Vector3 vector;
		Direction direction2;
		Vector3 vector2;
		if (GardenBedNavigation.IsGardenPlot(gardenerCurrentTarget) && GardenBedNavigation.TryGetOpenApproach(gardenerCurrentTarget, base.Position, out vector, out direction2))
		{
			vector2 = vector;
		}
		else
		{
			vector2 = this.GetGardenerDockPosition(gardenerCurrentTarget);
		}
		switch (base.MovementComponent.StartPath(vector2, base.WorldId, base.WorldId, MovementType.GDGraph, 1.5f, "", null, null, MovementComponent.DestinationType.Position))
		{
		case MovementComponent.StartPathResult.Started:
			this.gardenerCurrentMovementTargetUniqueId = this.gardenerCurrentTargetUniqueId;
			return;
		case MovementComponent.StartPathResult.AlreadyAtDestinationPoint:
			this.GardenerOnPathSuccess();
			return;
		case MovementComponent.StartPathResult.IncorrectMovementType:
			this.gardenerTimeForCheckFailedPathAgain = 1f;
			this.GardenerState = ZombieWgoData.ZombieGardenerState.FailedToFindPath;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x06002921 RID: 10529 RVA: 0x000C25A0 File Offset: 0x000C07A0
	[CompilerGenerated]
	private void <ConveyorTransporterTryMoveToCurrentTarget>g__MoveAction|345_0()
	{
		DockPointData nearestDockPointData = this.ConveyorTransporterCurrentTarget.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
		if (base.MovementComponent.IsMoving)
		{
			base.MovementComponent.ForceStop();
		}
		if (this.ConveyorTransporterState == ZombieWgoData.ZombieConveyorTransporterState.OnStation)
		{
			base.Position = this.AttachedWgoData.GetNearestDockPointDataWorldPositionOrMyPosition(base.Position, DockPointData.Availability.OnlyOccupied, (DockPointData dockPointData, Vector3 parentPos) => !dockPointData.BakedData.DisableTargetingForCaretaker);
			VariableNotificator<Vector2> direction = this.direction;
			DockPointData nearestDockPointData2 = this.AttachedWgoData.GetNearestDockPointData(base.Position, DockPointData.Availability.All);
			direction.Value = ((nearestDockPointData2 != null) ? nearestDockPointData2.Direction.ConvertToVector2XZ() : Direction.Down.ConvertToVector2XZ());
		}
		Vector3 nearestDockPointDataWorldPositionOrMyPosition = this.ConveyorTransporterCurrentTarget.GetNearestDockPointDataWorldPositionOrMyPosition(base.Position, DockPointData.Availability.All, (DockPointData dockPointData, Vector3 parentPos) => !dockPointData.BakedData.DisableTargetingForCaretaker);
		WorldZoneData worldZoneData = base.WorldZoneData;
		IEnumerable<LazyConsts.Navigation.Graph> enumerable = ((worldZoneData != null) ? worldZoneData.MovementGraphs : null);
		WorldZoneData worldZoneData2 = base.WorldZoneData;
		GraphMask graphMask = NavigationGraphMaskUtils.ToGraphMask(enumerable, (worldZoneData2 != null) ? worldZoneData2.navigationGraph : LazyConsts.Navigation.Graph.None);
		MovementComponent.StartPathResult startPathResult;
		if (nearestDockPointData == null)
		{
			startPathResult = base.MovementComponent.StartPath(nearestDockPointDataWorldPositionOrMyPosition, graphMask, base.WorldId, 1.5f, "", null, MovementComponent.DestinationType.Position);
		}
		else
		{
			startPathResult = base.MovementComponent.StartPath(nearestDockPointDataWorldPositionOrMyPosition, nearestDockPointData.BakedData, graphMask, base.WorldId, 1.5f, "", null);
		}
		switch (startPathResult)
		{
		case MovementComponent.StartPathResult.Started:
			this.conveyorTransporterCurrentMovementTargetUniqueId = this.conveyorTransporterCurrentTargetUniqueId;
			return;
		case MovementComponent.StartPathResult.AlreadyAtDestinationPoint:
			this.ConveyorTransporterOnPathSuccess();
			return;
		case MovementComponent.StartPathResult.IncorrectMovementType:
			this.conveyorTransporterTimeForCheckFailedPathAgain = 1f;
			this.ConveyorTransporterState = ZombieWgoData.ZombieConveyorTransporterState.FailedToFindPath;
			return;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	// Token: 0x040021CD RID: 8653
	private const float CARETAKER_WAITING_NEAR_INVENTORY_DISTANCE = 2f;

	// Token: 0x040021D5 RID: 8661
	[SerializeField]
	private string name;

	// Token: 0x040021D6 RID: 8662
	[SerializeField]
	private bool nameRandomed;

	// Token: 0x040021D7 RID: 8663
	[SerializeField]
	private ZombieType zombieType;

	// Token: 0x040021D8 RID: 8664
	[SerializeField]
	private Item zombieItem;

	// Token: 0x040021D9 RID: 8665
	[SerializeField]
	private SGuid attachedWgoDataUniqueId = SGuid.Empty;

	// Token: 0x040021DA RID: 8666
	[SerializeField]
	private global::AnimationState curAnimState;

	// Token: 0x040021DB RID: 8667
	private WgoData attachedWgoData;

	// Token: 0x040021DC RID: 8668
	private IWorkActivity currentActivity;

	// Token: 0x040021DD RID: 8669
	public SGuid equippedHand = SGuid.Empty;

	// Token: 0x040021DE RID: 8670
	public SGuid equippedArmor = SGuid.Empty;

	// Token: 0x040021DF RID: 8671
	public SGuid equippedCollar = SGuid.Empty;

	// Token: 0x040021E2 RID: 8674
	public List<ZombieTalentData> talentData = new List<ZombieTalentData>();

	// Token: 0x040021E3 RID: 8675
	public List<string> disabledTalentLevelUps = new List<string>();

	// Token: 0x040021E4 RID: 8676
	public int techRed;

	// Token: 0x040021E5 RID: 8677
	public int techBlue;

	// Token: 0x040021E6 RID: 8678
	public int techGreen;

	// Token: 0x040021E9 RID: 8681
	[SerializeField]
	private List<SGuid> crafterOrders = new List<SGuid>();

	// Token: 0x040021EA RID: 8682
	[SerializeField]
	private string crafterOrderedCraftId;

	// Token: 0x040021EB RID: 8683
	[SerializeField]
	private SGuid caretakerExecutingOrder = SGuid.Empty;

	// Token: 0x040021EC RID: 8684
	[SerializeField]
	private Item caretakerPortableItem = Item.Empty;

	// Token: 0x040021ED RID: 8685
	[SerializeField]
	private ZombieWgoData.ZombieCaretakerState caretakerState;

	// Token: 0x040021EE RID: 8686
	[SerializeField]
	private ZombieWgoData.ZombieCaretakerState caretakerPreviousState;

	// Token: 0x040021EF RID: 8687
	[SerializeField]
	private SGuid caretakerCurrentTargetUniqueId = SGuid.Empty;

	// Token: 0x040021F0 RID: 8688
	[SerializeField]
	private SGuid caretakerCurrentMovementTargetUniqueId = SGuid.Empty;

	// Token: 0x040021F1 RID: 8689
	[SerializeField]
	private float caretakerPickingUpFromInventoryTime;

	// Token: 0x040021F2 RID: 8690
	[SerializeField]
	private float caretakerTimeForCheckFailedPathAgain;

	// Token: 0x040021F3 RID: 8691
	[SerializeField]
	private Inventory porterInventory;

	// Token: 0x040021F4 RID: 8692
	[SerializeField]
	private SGuid gardenerExecutingOrder = SGuid.Empty;

	// Token: 0x040021F5 RID: 8693
	[SerializeField]
	private Item gardenerPortableItem = Item.Empty;

	// Token: 0x040021F6 RID: 8694
	[SerializeField]
	private ZombieWgoData.ZombieGardenerState gardenerState;

	// Token: 0x040021F7 RID: 8695
	[SerializeField]
	private ZombieWgoData.ZombieGardenerState gardenerPreviousState;

	// Token: 0x040021F8 RID: 8696
	[SerializeField]
	private SGuid gardenerCurrentTargetUniqueId = SGuid.Empty;

	// Token: 0x040021F9 RID: 8697
	[SerializeField]
	private SGuid gardenerCurrentMovementTargetUniqueId = SGuid.Empty;

	// Token: 0x040021FA RID: 8698
	[SerializeField]
	private float gardenerPickingUpFromInventoryTime;

	// Token: 0x040021FB RID: 8699
	[SerializeField]
	private float gardenerTimeForCheckFailedPathAgain;

	// Token: 0x040021FC RID: 8700
	[SerializeField]
	private SGuid gardenerStation = SGuid.Empty;

	// Token: 0x040021FD RID: 8701
	[SerializeField]
	private SGuid gardenerCurrentWorkingWgo = SGuid.Empty;

	// Token: 0x040021FE RID: 8702
	[SerializeField]
	private SGuid conveyorTransporterExecutingOrder = SGuid.Empty;

	// Token: 0x040021FF RID: 8703
	[SerializeField]
	private Item conveyorTransporterPortableItem = Item.Empty;

	// Token: 0x04002200 RID: 8704
	[SerializeField]
	private ZombieWgoData.ZombieConveyorTransporterState conveyorTransporterState;

	// Token: 0x04002201 RID: 8705
	[SerializeField]
	private ZombieWgoData.ZombieConveyorTransporterState conveyorTransporterPreviousState;

	// Token: 0x04002202 RID: 8706
	[SerializeField]
	private SGuid conveyorTransporterCurrentTargetUniqueId = SGuid.Empty;

	// Token: 0x04002203 RID: 8707
	[SerializeField]
	private SGuid conveyorTransporterCurrentMovementTargetUniqueId = SGuid.Empty;

	// Token: 0x04002204 RID: 8708
	[SerializeField]
	private float conveyorTransporterTimeForCheckFailedPathAgain;

	// Token: 0x020005EE RID: 1518
	public enum ZombieCaretakerState
	{
		// Token: 0x04002206 RID: 8710
		OnStation,
		// Token: 0x04002207 RID: 8711
		GoToStation,
		// Token: 0x04002208 RID: 8712
		GoToInventoryToPickUpOrderItem,
		// Token: 0x04002209 RID: 8713
		PickingUpOrderItemFromInventory,
		// Token: 0x0400220A RID: 8714
		GoToZombieToDeliverOrderItem,
		// Token: 0x0400220B RID: 8715
		GoToZombieToPickUpOrderItem,
		// Token: 0x0400220C RID: 8716
		GoToInventoryToDeliverOrderItem,
		// Token: 0x0400220D RID: 8717
		GoToInventoryToPutPortableItemWithExistingOrder,
		// Token: 0x0400220E RID: 8718
		GoToInventoryToPutPortableItemWithoutExistingOrder,
		// Token: 0x0400220F RID: 8719
		WaitingOtherCaretakersOnInventory,
		// Token: 0x04002210 RID: 8720
		FailedToFindPath,
		// Token: 0x04002211 RID: 8721
		CanNotPutItemToInventory
	}

	// Token: 0x020005EF RID: 1519
	public enum ZombieGardenerState
	{
		// Token: 0x04002213 RID: 8723
		OnStation,
		// Token: 0x04002214 RID: 8724
		GoToStation,
		// Token: 0x04002215 RID: 8725
		TeleportSeedsFromMultiInventory,
		// Token: 0x04002216 RID: 8726
		GoToGardenBedToPlantSeeds,
		// Token: 0x04002217 RID: 8727
		PlantingSeeds,
		// Token: 0x04002218 RID: 8728
		GoToGardenBedToTakePlants,
		// Token: 0x04002219 RID: 8729
		GatheringPlants,
		// Token: 0x0400221A RID: 8730
		WaitingForWgoDeath,
		// Token: 0x0400221B RID: 8731
		FailedToFindPath,
		// Token: 0x0400221C RID: 8732
		CanNotPutItemToInventory
	}

	// Token: 0x020005F0 RID: 1520
	public enum ZombieConveyorTransporterState
	{
		// Token: 0x0400221E RID: 8734
		OnStation,
		// Token: 0x0400221F RID: 8735
		GoToStation,
		// Token: 0x04002220 RID: 8736
		GoToStationCellToPickUp,
		// Token: 0x04002221 RID: 8737
		GoToStorageToPutItem,
		// Token: 0x04002222 RID: 8738
		FailedToFindPath,
		// Token: 0x04002223 RID: 8739
		CanNotPutItemToInventory
	}
}
