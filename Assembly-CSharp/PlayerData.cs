using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020005A3 RID: 1443
[Serializable]
public class PlayerData
{
	// Token: 0x1400006D RID: 109
	// (add) Token: 0x060024F8 RID: 9464 RVA: 0x000AD694 File Offset: 0x000AB894
	// (remove) Token: 0x060024F9 RID: 9465 RVA: 0x000AD6CC File Offset: 0x000AB8CC
	public event Action<List<Item>> OnDropCollected;

	// Token: 0x1400006E RID: 110
	// (add) Token: 0x060024FA RID: 9466 RVA: 0x000AD704 File Offset: 0x000AB904
	// (remove) Token: 0x060024FB RID: 9467 RVA: 0x000AD73C File Offset: 0x000AB93C
	public event Action OnPinnedItemsChanged;

	// Token: 0x1400006F RID: 111
	// (add) Token: 0x060024FC RID: 9468 RVA: 0x000AD774 File Offset: 0x000AB974
	// (remove) Token: 0x060024FD RID: 9469 RVA: 0x000AD7AC File Offset: 0x000AB9AC
	public event Action OnGameResChanged;

	// Token: 0x14000070 RID: 112
	// (add) Token: 0x060024FE RID: 9470 RVA: 0x000AD7E4 File Offset: 0x000AB9E4
	// (remove) Token: 0x060024FF RID: 9471 RVA: 0x000AD81C File Offset: 0x000ABA1C
	public event Action<Item> OnItemUsed;

	// Token: 0x14000071 RID: 113
	// (add) Token: 0x06002500 RID: 9472 RVA: 0x000AD854 File Offset: 0x000ABA54
	// (remove) Token: 0x06002501 RID: 9473 RVA: 0x000AD888 File Offset: 0x000ABA88
	public static event Action<string> OnReputationTechEnoughRep;

	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x06002502 RID: 9474 RVA: 0x000AD8BB File Offset: 0x000ABABB
	// (set) Token: 0x06002503 RID: 9475 RVA: 0x000AD8C8 File Offset: 0x000ABAC8
	public Vector2 Direction
	{
		get
		{
			return this.direction.Value;
		}
		set
		{
			if (this.isDirectionLocked)
			{
				return;
			}
			this.direction.Value = value;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x06002504 RID: 9476 RVA: 0x000AD8DF File Offset: 0x000ABADF
	public Item overheadItem
	{
		get
		{
			if (!this.HasOverheadItem)
			{
				return null;
			}
			return this.overheadItems[this.overheadItems.Count - 1];
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x06002505 RID: 9477 RVA: 0x000AD904 File Offset: 0x000ABB04
	public IReadOnlyList<Item> OverheadItems
	{
		get
		{
			IReadOnlyList<Item> readOnlyList = this.overheadItems;
			return readOnlyList ?? Array.Empty<Item>();
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x06002506 RID: 9478 RVA: 0x000AD922 File Offset: 0x000ABB22
	public int OverheadCount
	{
		get
		{
			if (this.overheadItems == null)
			{
				return 0;
			}
			return this.overheadItems.Count;
		}
	}

	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x06002507 RID: 9479 RVA: 0x000AD939 File Offset: 0x000ABB39
	public bool HasOverheadItem
	{
		get
		{
			return this.OverheadCount > 0;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x06002508 RID: 9480 RVA: 0x000AD944 File Offset: 0x000ABB44
	public bool HasMultipleOverheadItems
	{
		get
		{
			return this.OverheadCount > 1;
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x06002509 RID: 9481 RVA: 0x000AD94F File Offset: 0x000ABB4F
	public int ExtraOverhead
	{
		get
		{
			return Mathf.Max(0, this.GetResInt("extra_overhead"));
		}
	}

	// Token: 0x17000601 RID: 1537
	// (get) Token: 0x0600250A RID: 9482 RVA: 0x000AD962 File Offset: 0x000ABB62
	public int OverheadStackLimit
	{
		get
		{
			return 1 + this.ExtraOverhead;
		}
	}

	// Token: 0x17000602 RID: 1538
	// (get) Token: 0x0600250B RID: 9483 RVA: 0x000AD96C File Offset: 0x000ABB6C
	public bool HasFreeOverheadSlot
	{
		get
		{
			return this.OverheadCount < this.OverheadStackLimit;
		}
	}

	// Token: 0x17000603 RID: 1539
	// (get) Token: 0x0600250C RID: 9484 RVA: 0x000AD97C File Offset: 0x000ABB7C
	public bool HasInteractingItem
	{
		get
		{
			return this.interactingItem != null && !this.interactingItem.IsEmpty;
		}
	}

	// Token: 0x17000604 RID: 1540
	// (get) Token: 0x0600250D RID: 9485 RVA: 0x000AD996 File Offset: 0x000ABB96
	public Inventory Inventory
	{
		get
		{
			return this.inventory;
		}
	}

	// Token: 0x17000605 RID: 1541
	// (get) Token: 0x0600250E RID: 9486 RVA: 0x000AD99E File Offset: 0x000ABB9E
	public WorldZoneData CurrentWorldZoneData
	{
		get
		{
			return this.currentWorldZoneData;
		}
	}

	// Token: 0x17000606 RID: 1542
	// (get) Token: 0x0600250F RID: 9487 RVA: 0x000AD9A6 File Offset: 0x000ABBA6
	public SGuid Guid
	{
		get
		{
			return this.guid;
		}
	}

	// Token: 0x06002510 RID: 9488 RVA: 0x000AD9AE File Offset: 0x000ABBAE
	public static PlayerData CreatePlayerData()
	{
		PlayerData playerData = new PlayerData();
		playerData.Init();
		return playerData;
	}

	// Token: 0x06002511 RID: 9489 RVA: 0x000AD9BB File Offset: 0x000ABBBB
	public void PrepareForGame()
	{
		this.lastOpenedPage = CharacterWindowData.CharPage.Main;
		if (this.insideTownZones == null)
		{
			this.insideTownZones = new List<TownZone>();
		}
		if (this.insideTownSubZones == null)
		{
			this.insideTownSubZones = new List<TownSubZone>();
		}
		this.MigrateOverheadItems();
		this.InitGameResSystems();
	}

	// Token: 0x06002512 RID: 9490 RVA: 0x00002318 File Offset: 0x00000518
	public void UnPrepareFromGame()
	{
	}

	// Token: 0x06002513 RID: 9491 RVA: 0x000AD9F6 File Offset: 0x000ABBF6
	public void SetNPCRep(string repRes, int value)
	{
		this.res.Set(repRes, (float)value);
		this.ValidateReputationTechs();
	}

	// Token: 0x06002514 RID: 9492 RVA: 0x000ADA0C File Offset: 0x000ABC0C
	public void AddNPCRep(string repRes, int value)
	{
		this.res.Add(repRes, (float)value);
		this.ValidateReputationTechs();
	}

	// Token: 0x06002515 RID: 9493 RVA: 0x000ADA22 File Offset: 0x000ABC22
	public int GetNPCRep(string repRes)
	{
		return this.res.GetInt(repRes);
	}

	// Token: 0x06002516 RID: 9494 RVA: 0x000ADA30 File Offset: 0x000ABC30
	private void ValidateReputationTechs()
	{
		foreach (TechDef techDef in GameBalance.Me.techDefs)
		{
			if (!MainGame.Instance.GameSave.knowledgeSystem.IsTechUnlocked(techDef.id) && (techDef.techDefType == TechDefType.CharRep || techDef.techDefType == TechDefType.DisRep) && techDef.TechState == TechState.Available && techDef.EnoughResources)
			{
				Action<string> onReputationTechEnoughRep = PlayerData.OnReputationTechEnoughRep;
				if (onReputationTechEnoughRep != null)
				{
					onReputationTechEnoughRep(techDef.id);
				}
			}
		}
	}

	// Token: 0x06002517 RID: 9495 RVA: 0x000ADAD8 File Offset: 0x000ABCD8
	public void EquipItem(Item item)
	{
		Item item2;
		if (!this.toolBeltInventory.Data.TryGetItemInInventory(item.id, out item2))
		{
			this.toolBeltInventory.AddItemToInventory(item, null, false);
			return;
		}
		Debug.LogWarning(string.Format("Item [{0}] with type [{1}] was already equipped", item.id, item.Definition.type));
	}

	// Token: 0x06002518 RID: 9496 RVA: 0x000ADB34 File Offset: 0x000ABD34
	public void UseItem(Item item)
	{
		PlayerData.<>c__DisplayClass85_0 CS$<>8__locals1;
		CS$<>8__locals1.item = item;
		CS$<>8__locals1.<>4__this = this;
		if (!CS$<>8__locals1.item.Definition.CanBeUsed)
		{
			return;
		}
		if (CS$<>8__locals1.item.Definition.stayOnUse)
		{
			this.<UseItem>g__UseLogic|85_0(ref CS$<>8__locals1);
			return;
		}
		if (this.inventory.RemoveItemById(CS$<>8__locals1.item.id, 1, null, null, false).Count > 0)
		{
			this.<UseItem>g__UseLogic|85_0(ref CS$<>8__locals1);
		}
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000ADBA9 File Offset: 0x000ABDA9
	public void RemoveItem(Item item)
	{
		this.inventory.RemoveItemById(item.id, 1, null, null, false);
	}

	// Token: 0x0600251A RID: 9498 RVA: 0x000ADBC4 File Offset: 0x000ABDC4
	public void AddOverheadItem(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return;
		}
		this.EnsureOverheadItems();
		if (!this.HasFreeOverheadSlot)
		{
			this.DropOverheadItem();
		}
		if (this.HasInteractingItem)
		{
			this.RemoveInteractingItem();
		}
		Debug.Log(string.Format("AddOverheadItem item:[{0}] count:[{1}]", item.id, item.Count));
		this.overheadItems.Add(item);
		Action<Item> onOverheadItemAdded = this.OnOverheadItemAdded;
		if (onOverheadItemAdded != null)
		{
			onOverheadItemAdded(item);
		}
		this.RefreshOverheadVisuals();
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.AddOverhead, item.id);
		this.PlayOverheadItemTakeSound(item);
	}

	// Token: 0x0600251B RID: 9499 RVA: 0x000ADC57 File Offset: 0x000ABE57
	public bool TryAddOverheadItemNoReplace(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return false;
		}
		if (!this.HasFreeOverheadSlot)
		{
			return false;
		}
		this.AddOverheadItem(item);
		return true;
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000ADC78 File Offset: 0x000ABE78
	public bool TryGetOverheadItem(Predicate<Item> match, out Item item)
	{
		item = null;
		if (match == null || this.overheadItems == null)
		{
			return false;
		}
		for (int i = this.overheadItems.Count - 1; i >= 0; i--)
		{
			Item item2 = this.overheadItems[i];
			if (item2 != null && !item2.IsEmpty && match(item2))
			{
				item = item2;
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x000ADCD4 File Offset: 0x000ABED4
	public void DropOverheadItem()
	{
		if (!this.HasOverheadItem)
		{
			return;
		}
		this.DropOverheadItem(this.overheadItem);
	}

	// Token: 0x0600251E RID: 9502 RVA: 0x000ADCEC File Offset: 0x000ABEEC
	public void DropOverheadItem(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return;
		}
		Debug.Log("DropOverheadItem:[" + item.id + "]");
		Vector3 vector;
		bool playerDropPosition = SpecialPhysicsCastUtils.GetPlayerDropPosition(this.position.Value, this.direction.Value, out vector);
		Debug.Log(string.Format("Player drop pos by [{0}] result: {1}, [{2}]", "SpecialPhysicsCastUtils", playerDropPosition ? "success" : "fail", vector));
		MainGame.Instance.dropSystem.DropItem(item, MainGame.PlayerData.currentGameSceneId, vector, null);
		this.RemoveOverheadItem(item);
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x000ADD8A File Offset: 0x000ABF8A
	public void RemoveOverheadItem()
	{
		if (!this.HasOverheadItem)
		{
			return;
		}
		this.RemoveOverheadItem(this.overheadItem);
	}

	// Token: 0x06002520 RID: 9504 RVA: 0x000ADDA4 File Offset: 0x000ABFA4
	public void RemoveOverheadItem(Item item)
	{
		this.EnsureOverheadItems();
		int num = this.IndexOfOverheadItem(item);
		if (num < 0)
		{
			return;
		}
		this.overheadItems.RemoveAt(num);
		Action onOverheadItemRemoved = this.OnOverheadItemRemoved;
		if (onOverheadItemRemoved != null)
		{
			onOverheadItemRemoved();
		}
		this.RefreshOverheadVisuals();
	}

	// Token: 0x06002521 RID: 9505 RVA: 0x000ADDE7 File Offset: 0x000ABFE7
	public void InsertOverheadItemTo(WgoData wgoData)
	{
		this.InsertOverheadItemTo(wgoData, this.overheadItem);
	}

	// Token: 0x06002522 RID: 9506 RVA: 0x000ADDF8 File Offset: 0x000ABFF8
	public void InsertOverheadItemTo(WgoData wgoData, Item item)
	{
		if (wgoData == null || item == null || item.IsEmpty)
		{
			return;
		}
		string id = item.id;
		wgoData.Inventory.AddItemToInventory(item, null, false);
		this.RemoveOverheadItem(item);
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerInsertOverheadToWgoAnItem, id);
	}

	// Token: 0x06002523 RID: 9507 RVA: 0x000ADE38 File Offset: 0x000AC038
	public void IncreaseOverheadStackLimit(int increaseValue)
	{
		if (increaseValue <= 0)
		{
			return;
		}
		this.AddRes("extra_overhead", (float)increaseValue);
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x000ADE4C File Offset: 0x000AC04C
	public void SetOverheadStackLimit(int limit)
	{
		if (limit < 1)
		{
			return;
		}
		this.SetRes("extra_overhead", (float)(limit - 1));
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x000ADE64 File Offset: 0x000AC064
	private void MigrateOverheadItems()
	{
		this.EnsureOverheadItems();
		if (this.overheadItemLegacy != null && !this.overheadItemLegacy.IsEmpty)
		{
			if (this.overheadItems.Count == 0)
			{
				this.overheadItems.Add(this.overheadItemLegacy);
			}
			this.overheadItemLegacy = null;
		}
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x000ADEB1 File Offset: 0x000AC0B1
	private void EnsureOverheadItems()
	{
		if (this.overheadItems == null)
		{
			this.overheadItems = new List<Item>();
		}
	}

	// Token: 0x06002527 RID: 9511 RVA: 0x000ADEC8 File Offset: 0x000AC0C8
	private int IndexOfOverheadItem(Item item)
	{
		if (item == null || this.overheadItems == null)
		{
			return -1;
		}
		int num = this.overheadItems.LastIndexOf(item);
		if (num >= 0)
		{
			return num;
		}
		for (int i = this.overheadItems.Count - 1; i >= 0; i--)
		{
			Item item2 = this.overheadItems[i];
			if (item2 != null && item2.UniqueId.Equals(item.UniqueId) && item2.id == item.id)
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06002528 RID: 9512 RVA: 0x000ADF48 File Offset: 0x000AC148
	private void RefreshOverheadVisuals()
	{
		PlayerController playerController = MainGame.PlayerController;
		if (playerController == null)
		{
			return;
		}
		if (!this.HasOverheadItem)
		{
			playerController.RemoveOverheadItem();
			return;
		}
		playerController.SetOverheadItems(this.overheadItems);
	}

	// Token: 0x06002529 RID: 9513 RVA: 0x000ADF80 File Offset: 0x000AC180
	private void PlayOverheadItemTakeSound(Item item)
	{
		if (item.Definition.itemGroupIds.Contains("zombie"))
		{
			LazyAudio.PlayAndForget("oh_zombie_grab");
			return;
		}
		if (item.Definition.itemGroupIds.Contains("corpse"))
		{
			LazyAudio.PlayAndForget("oh_corpse_grab");
			return;
		}
		if (item.id == "wood")
		{
			LazyAudio.PlayAndForget("oh_wood_grab");
			return;
		}
	}

	// Token: 0x0600252A RID: 9514 RVA: 0x000ADFF0 File Offset: 0x000AC1F0
	public void SetInteractingItem(Item item)
	{
		if (LazySingleton<FightingGameController>.Instance.CurrentFightState != FightState.Disabled)
		{
			return;
		}
		if (this.HasMultipleOverheadItems)
		{
			return;
		}
		if (this.HasOverheadItem)
		{
			this.DropOverheadItem();
		}
		if (this.HasInteractingItem)
		{
			this.RemoveInteractingItem();
		}
		Debug.Log("#SetInteractingItem:[" + item.id + "]");
		this.interactingItem = item;
		MainGame.PlayerController.SetInteractingItem(item, this.inventory.Data.GetTotalCountInInventory(item.id, null, false));
		MainGame.PlayerController.PlayerInteractionComponent.ResetInteractionState();
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x000AE082 File Offset: 0x000AC282
	public void RemoveInteractingItem()
	{
		if (!this.HasInteractingItem)
		{
			return;
		}
		Debug.Log("RemoveInteractingItem:[" + this.interactingItem.id + "]");
		this.interactingItem = null;
		MainGame.PlayerController.RemoveInteractingItem();
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x000AE0C0 File Offset: 0x000AC2C0
	public void UpdateInteractingItem()
	{
		if (this.HasInteractingItem && this.inventory.Data.GetTotalCountInInventory(this.interactingItem.id, null, false) <= 0)
		{
			this.RemoveInteractingItem();
			MainGame.PlayerController.PlayerInteractionComponent.ResetInteractionState();
			return;
		}
		this.SetInteractingItem(this.interactingItem);
		MainGame.PlayerController.PlayerInteractionComponent.ResetInteractionState();
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x000AE128 File Offset: 0x000AC328
	public void SetCurrentWorldZoneData(WorldZoneData worldZoneData, Action listener)
	{
		if (this.currentWorldZoneData != null)
		{
			this.currentWorldZoneData.RemovePlayerData(this);
			this.currentWorldZoneData.OnWgoDataChanged -= listener;
		}
		this.currentWorldZoneData = worldZoneData;
		if (this.currentWorldZoneData != null)
		{
			this.currentWorldZoneData.AddPlayerData(this);
			this.currentWorldZoneData.OnWgoDataChanged += listener;
		}
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x000AE17C File Offset: 0x000AC37C
	public void AddTownZone(TownZone townZone)
	{
		if (!this.insideTownZones.Contains(townZone))
		{
			Debug.Log("#town_zone# AddTownZone:[" + townZone.name + "]");
			this.insideTownZones.Add(townZone);
		}
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x000AE1B2 File Offset: 0x000AC3B2
	public void RemoveTownZone(TownZone townZone)
	{
		if (this.insideTownZones.Contains(townZone))
		{
			Debug.Log("#town_zone# RemoveTownZone:[" + townZone.name + "]");
			this.insideTownZones.Remove(townZone);
		}
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x000AE1E9 File Offset: 0x000AC3E9
	public void AddTownSubZone(TownSubZone townSubZone)
	{
		if (!this.insideTownSubZones.Contains(townSubZone))
		{
			Debug.Log("#town_zone# AddTownSubZone:[" + townSubZone.id + "]");
			this.insideTownSubZones.Add(townSubZone);
		}
	}

	// Token: 0x06002531 RID: 9521 RVA: 0x000AE21F File Offset: 0x000AC41F
	public void RemoveTownSubZone(TownSubZone townSubZone)
	{
		if (this.insideTownSubZones.Contains(townSubZone))
		{
			Debug.Log("#town_zone# RemoveTownSubZone:[" + townSubZone.id + "]");
			this.insideTownSubZones.Remove(townSubZone);
		}
	}

	// Token: 0x06002532 RID: 9522 RVA: 0x000AE258 File Offset: 0x000AC458
	public void TryApplyStartState()
	{
		StartReses startReses = StartReses.Load();
		if (startReses != null)
		{
			foreach (StartReses.StartItemData startItemData in startReses.startItems)
			{
				Item item = new Item(startItemData.id, startItemData.count);
				if (startItemData.shouldBeEquipped)
				{
					this.EquipItem(item);
				}
				else
				{
					this.inventory.AddItemToInventory(item, null, false);
				}
			}
			this.res.Set(startReses.startGameRes);
		}
	}

	// Token: 0x06002533 RID: 9523 RVA: 0x000AE2F8 File Offset: 0x000AC4F8
	public void CollectDrop(DropView dropView)
	{
		DropData dropData = ((dropView != null) ? dropView.Data : null);
		if (dropData == null)
		{
			return;
		}
		if (dropData.IsResDrop)
		{
			this.CollectResDrop(dropData);
			return;
		}
		if (dropData.Size == ItemSize.Big)
		{
			return;
		}
		if (this.inventory.Data.CanAddItemCountToInventory(dropData.Item, true, null, false) <= 0)
		{
			bool isPhysicDisabled = dropView.IsPhysicDisabled;
			dropView.StopMoving();
			dropView.DoKick(MainGame.PlayerController.transform, isPhysicDisabled);
			LazySingleton<UINotificator>.Instance.HandleInventoryFull();
			return;
		}
		List<Item> list;
		this.inventory.AddItemToInventory(dropData.Item, out list, null, false);
		foreach (LazyExpression lazyExpression in dropData.Item.Definition.onDropCollected)
		{
			lazyExpression.Evaluate(list[0]);
		}
		Action<List<Item>> onDropCollected = this.OnDropCollected;
		if (onDropCollected != null)
		{
			onDropCollected(list);
		}
		Debug.Log(string.Format("Drop[{0}] collected successfully, left count = [{1}]", dropData.Id, dropData.Count));
		if (dropData.Count == 0)
		{
			MainGame.Instance.dropSystem.RemoveDrop(dropData, dropData.WorldId);
			return;
		}
		dropData.NotifyCountChanged();
	}

	// Token: 0x06002534 RID: 9524 RVA: 0x000AE43C File Offset: 0x000AC63C
	public void CollectResDrop(DropData drop)
	{
		if (drop == null || !drop.IsResDrop || drop.IsRemoving)
		{
			return;
		}
		string text = drop.Item.id.Replace("game_res_", string.Empty);
		int count = drop.Item.Count;
		if (TechDef.FlyingReses.Contains(text))
		{
			Vector3 vector = ((MainGame.PlayerController != null) ? MainGame.PlayerController.transform.position : drop.Position);
			for (int i = 0; i < count; i++)
			{
				FlyingTechPoint.Drop(vector, text, null, -1);
			}
			LazyAudio.Play("tech_point_collect");
		}
		else
		{
			this.res.Add(text, (float)count);
		}
		foreach (LazyExpression lazyExpression in drop.Item.Definition.onDropCollected)
		{
			lazyExpression.Evaluate(drop.Item);
		}
		drop.Item.Count = 0;
		MainGame.Instance.dropSystem.RemoveDrop(drop, drop.WorldId);
		Debug.Log(string.Format("Drop RES[{0}] collected successfully, left count = [{1}]", drop.Id, drop.Count));
	}

	// Token: 0x06002535 RID: 9525 RVA: 0x000AE580 File Offset: 0x000AC780
	public void ApplyCustomization(PlayerCustomizationData customizationData)
	{
		this.customization = customizationData;
		PlayerSkinHelper.ApplySkin(customizationData, false);
		PlayerSkinHelper.ApplyPlayerColorsByData(customizationData, false);
	}

	// Token: 0x06002536 RID: 9526 RVA: 0x000AE598 File Offset: 0x000AC798
	private void Init()
	{
		this.inventory = new Inventory("inventory", 20);
		this.toolBeltInventory = new Inventory("toolBeltInventory", 14);
		this.toolBeltInventory.AddItemToInventory(new Item("hand_tool", 1), null, false);
		this.res.Set("money", 50f);
		this.res.Set("g_garden_fertilizer_slots", (float)GameBalance.Me.GetData<ConstDef>("g_garden_fertilizer_slots").IntValue);
		this.res.Set("g_garden_farming_base", (float)GameBalance.Me.GetData<ConstDef>("g_garden_farming_base").IntValue);
		this.res.Set("g_vineyard_farming_base", (float)GameBalance.Me.GetData<ConstDef>("g_vineyard_farming_base").IntValue);
		this.res.Set("g_garden_autocraft_dec", (float)GameBalance.Me.GetData<ConstDef>("g_garden_autocraft_dec").IntValue);
		GameResSystemDef data = GameBalance.Me.GetData<GameResSystemDef>("energy");
		GameResSystemDef data2 = GameBalance.Me.GetData<GameResSystemDef>("insanity");
		GameResSystemDef data3 = GameBalance.Me.GetData<GameResSystemDef>("tech_red");
		GameResSystemDef data4 = GameBalance.Me.GetData<GameResSystemDef>("tech_green");
		GameResSystemDef data5 = GameBalance.Me.GetData<GameResSystemDef>("tech_blue");
		GameResSystemDef data6 = GameBalance.Me.GetData<GameResSystemDef>("donkey_body_drop_chance");
		this.res.Set(data.ResId, data.start.EvaluateFloat());
		this.res.Set(data2.ResId, data2.start.EvaluateFloat());
		this.res.Set(data3.ResId, data3.start.EvaluateFloat());
		this.res.Set(data4.ResId, data4.start.EvaluateFloat());
		this.res.Set(data5.ResId, data5.start.EvaluateFloat());
		this.res.Set(data6.ResId, data6.start.EvaluateFloat());
	}

	// Token: 0x06002537 RID: 9527 RVA: 0x000AE79C File Offset: 0x000AC99C
	private void InitGameResSystems()
	{
		Dictionary<string, GameResSystemBase> dictionary = new Dictionary<string, GameResSystemBase>();
		dictionary.Add("energy", new PlayerEnergyGameResSystem(GameBalance.Me.GetData<GameResSystemDef>("energy").ResId, this.res));
		dictionary.Add("insanity", new PlayerInsanityGameResSystem(GameBalance.Me.GetData<GameResSystemDef>("insanity").ResId, this.res));
		dictionary.Add("stamina", new PlayerStaminaGameResSystem(GameBalance.Me.GetData<GameResSystemDef>("stamina").ResId, this.res));
		dictionary.Add("money", new PlayerMoneyGameResSystem(GameBalance.Me.GetData<GameResSystemDef>("money").ResId, this.res));
		dictionary.Add("happiness", new PlayerHappinessGameResSystem(GameBalance.Me.GetData<GameResSystemDef>("happiness").ResId, this.res));
		foreach (WorldZoneDef worldZoneDef in GameBalance.Me.worldZoneDefs)
		{
			string text = "wz_" + worldZoneDef.id;
			dictionary.TryAdd(text, new WorldZoneQualitySystemGameResSystem(text, this.res));
		}
		for (int i = 0; i < GameBalance.Me.gameResSystemDefs.Count; i++)
		{
			dictionary.TryAdd(GameBalance.Me.gameResSystemDefs[i].ResId, new GK2GameResSystem(GameBalance.Me.gameResSystemDefs[i].ResId, this.res));
		}
		this.res.SetSystems(dictionary);
	}

	// Token: 0x06002538 RID: 9528 RVA: 0x000AE954 File Offset: 0x000ACB54
	public void SetHotBarItemAtIndex(string equippedItem, int index)
	{
		for (int i = 0; i < this.pinnedItems.Length; i++)
		{
			if (this.pinnedItems[i] == equippedItem)
			{
				this.pinnedItems[i] = "";
			}
		}
		this.pinnedItems[index] = equippedItem;
		Action onPinnedItemsChanged = this.OnPinnedItemsChanged;
		if (onPinnedItemsChanged == null)
		{
			return;
		}
		onPinnedItemsChanged();
	}

	// Token: 0x06002539 RID: 9529 RVA: 0x000AE9AC File Offset: 0x000ACBAC
	public void TryUseHotBarItem(string itemId)
	{
		if (string.IsNullOrEmpty(itemId))
		{
			return;
		}
		if (!this.pinnedItems.Contains(itemId))
		{
			return;
		}
		Item itemById = this.inventory.GetItemById(itemId);
		if (itemById == null || itemById.IsEmpty)
		{
			return;
		}
		if (itemById.IsSeed || itemById.IsFertilizer)
		{
			this.SetInteractingItem(itemById);
			return;
		}
		if (itemById.Definition.CanBeUsed)
		{
			this.UseItem(itemById);
		}
	}

	// Token: 0x0600253A RID: 9530 RVA: 0x000AEA16 File Offset: 0x000ACC16
	public void SetTutorialModeState(bool isActive)
	{
		this.isInTutorialMode = isActive;
		if (!this.isInTutorialMode)
		{
			this.tutorialModeExcludedList.Clear();
		}
	}

	// Token: 0x0600253B RID: 9531 RVA: 0x000AEA32 File Offset: 0x000ACC32
	public void AddToTutorialModeExcludedList(List<string> wgoExcludedUniqueIdList)
	{
		if (!this.isInTutorialMode)
		{
			return;
		}
		this.tutorialModeExcludedList.AddRange(wgoExcludedUniqueIdList);
	}

	// Token: 0x0600253C RID: 9532 RVA: 0x000AEA49 File Offset: 0x000ACC49
	public void RemoveFromTutorialModeExcludedList(List<string> wgoExcludedUniqueIdList)
	{
		if (!this.isInTutorialMode)
		{
			return;
		}
		this.tutorialModeExcludedList.RemoveAll(new Predicate<string>(wgoExcludedUniqueIdList.Contains));
	}

	// Token: 0x0600253D RID: 9533 RVA: 0x000AEA6D File Offset: 0x000ACC6D
	public void SetDirectionLock(bool isEnabled)
	{
		this.isDirectionLocked = !isEnabled;
	}

	// Token: 0x0600253E RID: 9534 RVA: 0x000AEA79 File Offset: 0x000ACC79
	public void AddDirectionListener(Action<Vector2> action)
	{
		this.direction.ValueChanged += action;
	}

	// Token: 0x0600253F RID: 9535 RVA: 0x000AEA87 File Offset: 0x000ACC87
	public void RemoveDirectionListener(Action<Vector2> action)
	{
		this.direction.ValueChanged -= action;
	}

	// Token: 0x06002540 RID: 9536 RVA: 0x000AEA95 File Offset: 0x000ACC95
	public void IncreaseInventorySize(int increaseValue)
	{
		MainGame.PlayerData.Inventory.Data.InventorySize += increaseValue;
		Debug.Log("Player inventory size increased by  " + increaseValue.ToString());
	}

	// Token: 0x06002541 RID: 9537 RVA: 0x000AEACC File Offset: 0x000ACCCC
	public void ReduceInventorySize(int reduceValue)
	{
		if (MainGame.PlayerData.Inventory.Data.InventorySize <= reduceValue)
		{
			Debug.LogError(string.Format("Cannot reduce inventory size by {0}. Current size: {1} (must be > {2})", reduceValue, MainGame.PlayerData.Inventory.Data.InventorySize, reduceValue));
			return;
		}
		if (MainGame.PlayerData.Inventory.Data.InventoryFillSize > MainGame.PlayerData.Inventory.Data.InventorySize - reduceValue)
		{
			Debug.LogError(string.Format("Cannot reduce inventory size by {0}. Current size: {1} (must be > current fill size {2})", reduceValue, MainGame.PlayerData.Inventory.Data.InventorySize, MainGame.PlayerData.Inventory.Data.InventoryFillSize));
			return;
		}
		MainGame.PlayerData.Inventory.Data.InventorySize -= reduceValue;
		Debug.Log("Player inventory size reduced by  " + reduceValue.ToString());
	}

	// Token: 0x06002542 RID: 9538 RVA: 0x000AEBCB File Offset: 0x000ACDCB
	public bool IsEnoughRes(GameRes gameRes)
	{
		return this.res.IsEnough(gameRes);
	}

	// Token: 0x06002543 RID: 9539 RVA: 0x000AEBD9 File Offset: 0x000ACDD9
	public bool IsEnoughRes(GameResAtom gameResAtom)
	{
		return this.res.IsEnough(gameResAtom);
	}

	// Token: 0x06002544 RID: 9540 RVA: 0x000AEBE7 File Offset: 0x000ACDE7
	public void AddRes(GameRes gameRes)
	{
		this.res.Add(gameRes);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x06002545 RID: 9541 RVA: 0x000AEC05 File Offset: 0x000ACE05
	public void AddResWithoutGlobalChangeEvent(GameRes gameRes)
	{
		this.res.Add(gameRes);
	}

	// Token: 0x06002546 RID: 9542 RVA: 0x000AEC13 File Offset: 0x000ACE13
	public void AddRes(string type, float value)
	{
		this.res.Add(type, value);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x06002547 RID: 9543 RVA: 0x000AEC32 File Offset: 0x000ACE32
	public void SetRes(GameRes gameRes)
	{
		this.res.Set(gameRes);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x06002548 RID: 9544 RVA: 0x000AEC50 File Offset: 0x000ACE50
	public void SetRes(string type, float value)
	{
		this.res.Set(type, value);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x06002549 RID: 9545 RVA: 0x000AEC70 File Offset: 0x000ACE70
	public void SetResWithoutSystemsCheck(string type, float value)
	{
		bool flag = !this.res.Has(type) || !Mathf.Approximately(this.res.GetWithoutSystemsCheck(type, 0f), value);
		this.res.SetWithoutSystemsCheck(type, value);
		if (flag)
		{
			Action onGameResChanged = this.OnGameResChanged;
			if (onGameResChanged == null)
			{
				return;
			}
			onGameResChanged();
		}
	}

	// Token: 0x0600254A RID: 9546 RVA: 0x000AECC7 File Offset: 0x000ACEC7
	public float GetRes(string type, float defaultValue = 0f)
	{
		return this.res.Get(type, defaultValue);
	}

	// Token: 0x0600254B RID: 9547 RVA: 0x000ADA22 File Offset: 0x000ABC22
	public int GetResInt(string type)
	{
		return this.res.GetInt(type);
	}

	// Token: 0x0600254C RID: 9548 RVA: 0x000AECD6 File Offset: 0x000ACED6
	public GameResSystemBase GetResSystem(string type)
	{
		return this.res.GetSystem(type);
	}

	// Token: 0x0600254D RID: 9549 RVA: 0x000AECE4 File Offset: 0x000ACEE4
	public void SubRes(string type, float value)
	{
		this.res.Sub(type, value);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x0600254E RID: 9550 RVA: 0x000AED03 File Offset: 0x000ACF03
	public void AddResWithoutSystemsCheck(string type, float value)
	{
		this.res.AddWithoutSystemsCheck(type, value);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x0600254F RID: 9551 RVA: 0x000AED22 File Offset: 0x000ACF22
	public void SubRes(GameRes gameRes)
	{
		this.res.Sub(gameRes);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x000AED40 File Offset: 0x000ACF40
	public void MultiplyRes(string type, float value)
	{
		this.res.Multiply(type, value);
		Action onGameResChanged = this.OnGameResChanged;
		if (onGameResChanged == null)
		{
			return;
		}
		onGameResChanged();
	}

	// Token: 0x06002552 RID: 9554 RVA: 0x000AEE1C File Offset: 0x000AD01C
	[CompilerGenerated]
	private void <UseItem>g__UseLogic|85_0(ref PlayerData.<>c__DisplayClass85_0 A_1)
	{
		GameRes gameResOnUse = A_1.item.Definition.GetGameResOnUse();
		if (!gameResOnUse.IsEmpty())
		{
			foreach (GameResAtom gameResAtom in gameResOnUse.List)
			{
				string type = gameResAtom.type;
				if (!(type == "energy"))
				{
					if (!(type == "insanity"))
					{
						this.res.Add(gameResOnUse);
					}
					else
					{
						PlayerInsanityGameResSystem.GetSystem().Add(gameResAtom.value, false);
					}
				}
				else
				{
					PlayerEnergyGameResSystem.GetSystem().Add(gameResAtom.value, false);
				}
			}
		}
		if (!string.IsNullOrEmpty(A_1.item.Definition.onUseSound))
		{
			LazyAudio.PlayAndForget(A_1.item.Definition.onUseSound);
		}
		foreach (LazyExpression lazyExpression in A_1.item.Definition.onUseExpressions)
		{
			lazyExpression.Evaluate(A_1.item);
		}
		GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.PlayerUseItem, A_1.item.id);
		Action<Item> onItemUsed = this.OnItemUsed;
		if (onItemUsed == null)
		{
			return;
		}
		onItemUsed(A_1.item);
	}

	// Token: 0x04002095 RID: 8341
	public VariableNotificator<Vector3> position = new VariableNotificator<Vector3>();

	// Token: 0x04002096 RID: 8342
	[SerializeField]
	private VariableNotificator<Vector2> direction = new VariableNotificator<Vector2>();

	// Token: 0x04002097 RID: 8343
	public VariableNotificator<global::AnimationState> charState = new VariableNotificator<global::AnimationState>();

	// Token: 0x04002098 RID: 8344
	public Action<Item> OnOverheadItemAdded;

	// Token: 0x04002099 RID: 8345
	public Action OnOverheadItemRemoved;

	// Token: 0x0400209A RID: 8346
	public Inventory inventory;

	// Token: 0x0400209B RID: 8347
	public Inventory toolBeltInventory;

	// Token: 0x0400209C RID: 8348
	[SerializeField]
	[FormerlySerializedAs("overheadItem")]
	[PreviouslySerializedAs("overheadItem")]
	private Item overheadItemLegacy;

	// Token: 0x0400209D RID: 8349
	[SerializeField]
	private List<Item> overheadItems = new List<Item>();

	// Token: 0x0400209E RID: 8350
	public Item interactingItem;

	// Token: 0x0400209F RID: 8351
	[SerializeField]
	private GameRes res = new GameRes();

	// Token: 0x040020A0 RID: 8352
	public EnergySystem energySystem = new EnergySystem();

	// Token: 0x040020A1 RID: 8353
	public StaminaSystem staminaSystem = new StaminaSystem();

	// Token: 0x040020A2 RID: 8354
	public string[] pinnedItems = new string[4];

	// Token: 0x040020A3 RID: 8355
	public PlayerCustomizationData customization = new PlayerCustomizationData();

	// Token: 0x040020A4 RID: 8356
	public string currentGameSceneId;

	// Token: 0x040020A5 RID: 8357
	public SermonResultData currentSermon;

	// Token: 0x040020A6 RID: 8358
	public bool isInTutorialMode;

	// Token: 0x040020A7 RID: 8359
	public List<string> tutorialModeExcludedList = new List<string>();

	// Token: 0x040020A8 RID: 8360
	public SGuid tutorialArrowWgoId;

	// Token: 0x040020A9 RID: 8361
	public bool isWispEnabled;

	// Token: 0x040020AA RID: 8362
	public bool isDirectionLocked;

	// Token: 0x040020AB RID: 8363
	public bool openedCraftWindowOnce;

	// Token: 0x040020AC RID: 8364
	public bool interactedWithFishingReservoirOnce;

	// Token: 0x040020AD RID: 8365
	public bool sawFightTutorialOnce;

	// Token: 0x040020AE RID: 8366
	public bool sawInspirationTutorialOnce;

	// Token: 0x040020AF RID: 8367
	public bool sawInspirationTalentsTutorialOnce;

	// Token: 0x040020B0 RID: 8368
	public bool interactedWithChalkBoardOnce;

	// Token: 0x040020B1 RID: 8369
	public bool openedGraveWindowOnce;

	// Token: 0x040020B2 RID: 8370
	public HPComponent hpComponent = new HPComponent(100);

	// Token: 0x040020B3 RID: 8371
	[SerializeField]
	private SGuid guid = new SGuid("49042eb8-eda7-4612-80c8-6fbfc39b52ac");

	// Token: 0x040020B4 RID: 8372
	[NonSerialized]
	private WorldZoneData currentWorldZoneData;

	// Token: 0x040020B5 RID: 8373
	[NonSerialized]
	public List<TownZone> insideTownZones = new List<TownZone>();

	// Token: 0x040020B6 RID: 8374
	[NonSerialized]
	public List<TownSubZone> insideTownSubZones = new List<TownSubZone>();

	// Token: 0x040020B7 RID: 8375
	[NonSerialized]
	public CharacterWindowData.CharPage lastOpenedPage = CharacterWindowData.CharPage.Main;
}
