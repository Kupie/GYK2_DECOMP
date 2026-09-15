using System;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000188 RID: 392
[Serializable]
public class DropData
{
	// Token: 0x14000003 RID: 3
	// (add) Token: 0x0600098D RID: 2445 RVA: 0x00030844 File Offset: 0x0002EA44
	// (remove) Token: 0x0600098E RID: 2446 RVA: 0x0003087C File Offset: 0x0002EA7C
	public event Action OnCountChanged;

	// Token: 0x17000178 RID: 376
	// (get) Token: 0x0600098F RID: 2447 RVA: 0x000308B1 File Offset: 0x0002EAB1
	// (set) Token: 0x06000990 RID: 2448 RVA: 0x000308B9 File Offset: 0x0002EAB9
	public Vector3 Position
	{
		get
		{
			return this.pos;
		}
		set
		{
			this.pos = value;
		}
	}

	// Token: 0x17000179 RID: 377
	// (get) Token: 0x06000991 RID: 2449 RVA: 0x000308C2 File Offset: 0x0002EAC2
	// (set) Token: 0x06000992 RID: 2450 RVA: 0x000308CA File Offset: 0x0002EACA
	public bool IsDroppedFromPlayer { get; private set; }

	// Token: 0x1700017A RID: 378
	// (get) Token: 0x06000993 RID: 2451 RVA: 0x000308D3 File Offset: 0x0002EAD3
	// (set) Token: 0x06000994 RID: 2452 RVA: 0x000308DB File Offset: 0x0002EADB
	public bool IsRemoving { get; private set; }

	// Token: 0x1700017B RID: 379
	// (get) Token: 0x06000995 RID: 2453 RVA: 0x000308E4 File Offset: 0x0002EAE4
	public SGuid UniqueId
	{
		get
		{
			return this.item.UniqueId;
		}
	}

	// Token: 0x1700017C RID: 380
	// (get) Token: 0x06000996 RID: 2454 RVA: 0x000308F1 File Offset: 0x0002EAF1
	// (set) Token: 0x06000997 RID: 2455 RVA: 0x000308F9 File Offset: 0x0002EAF9
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

	// Token: 0x1700017D RID: 381
	// (get) Token: 0x06000998 RID: 2456 RVA: 0x00030902 File Offset: 0x0002EB02
	public string Id
	{
		get
		{
			return this.item.id;
		}
	}

	// Token: 0x1700017E RID: 382
	// (get) Token: 0x06000999 RID: 2457 RVA: 0x0003090F File Offset: 0x0002EB0F
	public int Count
	{
		get
		{
			return this.item.Count;
		}
	}

	// Token: 0x1700017F RID: 383
	// (get) Token: 0x0600099A RID: 2458 RVA: 0x0003091C File Offset: 0x0002EB1C
	public string IconId
	{
		get
		{
			return this.item.Definition.iconId;
		}
	}

	// Token: 0x17000180 RID: 384
	// (get) Token: 0x0600099B RID: 2459 RVA: 0x0003092E File Offset: 0x0002EB2E
	public ItemSize Size
	{
		get
		{
			return this.item.Definition.itemSize;
		}
	}

	// Token: 0x17000181 RID: 385
	// (get) Token: 0x0600099C RID: 2460 RVA: 0x00030940 File Offset: 0x0002EB40
	public Item Item
	{
		get
		{
			return this.item;
		}
	}

	// Token: 0x17000182 RID: 386
	// (get) Token: 0x0600099D RID: 2461 RVA: 0x00030948 File Offset: 0x0002EB48
	public WgoData WgoData
	{
		get
		{
			return this.wgoData;
		}
	}

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x0600099E RID: 2462 RVA: 0x00030950 File Offset: 0x0002EB50
	public bool IsResDrop
	{
		get
		{
			return this.Item.id.StartsWith("game_res_");
		}
	}

	// Token: 0x17000184 RID: 388
	// (get) Token: 0x0600099F RID: 2463 RVA: 0x00030967 File Offset: 0x0002EB67
	public DropType DropType
	{
		get
		{
			return this.dropType;
		}
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060009A0 RID: 2464 RVA: 0x0003096F File Offset: 0x0002EB6F
	// (set) Token: 0x060009A1 RID: 2465 RVA: 0x00030977 File Offset: 0x0002EB77
	public float AutoDestroyTimer
	{
		get
		{
			return this.autoDestroyTimer;
		}
		set
		{
			this.autoDestroyTimer = value;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060009A2 RID: 2466 RVA: 0x00030980 File Offset: 0x0002EB80
	// (set) Token: 0x060009A3 RID: 2467 RVA: 0x000309BD File Offset: 0x0002EBBD
	public MultiFlagOR<CanNotBeAutoDestroyedReason> CanNotBeAutoDestroyed
	{
		get
		{
			if (this.canNotBeAutoDestroyed == null)
			{
				this.canNotBeAutoDestroyed = new MultiFlagOR<CanNotBeAutoDestroyedReason>();
				if (this.item != null && this.item.HasProperty<NeverAutoDestroyDropSerializedItemProperty>())
				{
					this.canNotBeAutoDestroyed.UpdateFlag(CanNotBeAutoDestroyedReason.Never, true);
				}
			}
			return this.canNotBeAutoDestroyed;
		}
		set
		{
			this.canNotBeAutoDestroyed = value;
		}
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x000309C6 File Offset: 0x0002EBC6
	public DropData()
	{
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x000309DC File Offset: 0x0002EBDC
	public DropData(Item item, Vector3 pos, string worldId)
	{
		this.dropType = DropType.Item;
		this.item = item;
		this.pos = pos;
		this.worldId = worldId;
		PlayerData playerData = MainGame.PlayerData;
		Vector3 vector;
		if (SpecialPhysicsCastUtils.GetPlayerDropPosition(playerData.position.Value, playerData.Direction, out vector) && this.pos == vector)
		{
			this.IsDroppedFromPlayer = true;
		}
		if (item.Definition.isLinkedToWgo)
		{
			this.dropType = DropType.WgoData;
		}
		this.canNotBeAutoDestroyed = new MultiFlagOR<CanNotBeAutoDestroyedReason>();
		if (item.HasProperty<NeverAutoDestroyDropSerializedItemProperty>())
		{
			this.canNotBeAutoDestroyed.UpdateFlag(CanNotBeAutoDestroyedReason.Never, true);
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00030A7F File Offset: 0x0002EC7F
	public DropData(GameResAtom gameResAtom, Vector3 pos, string worldId)
		: this(gameResAtom.ItemFromAtom(), pos, worldId)
	{
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x00030A8F File Offset: 0x0002EC8F
	public bool TryAddDropItemPartial(DropData drop)
	{
		return this.dropType != DropType.WgoData && this.item.TryAddItemPartial(drop.item);
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00030AAD File Offset: 0x0002ECAD
	public bool AddItem(Item item)
	{
		return this.dropType != DropType.WgoData && item.TryAddItem(item);
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00030AC1 File Offset: 0x0002ECC1
	public int CanAddItemCount(Item item)
	{
		if (this.dropType == DropType.WgoData)
		{
			return 0;
		}
		return item.CanAddItemCount(item);
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x00030AD5 File Offset: 0x0002ECD5
	public void MarkAsRemoving()
	{
		this.IsRemoving = true;
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x00030ADE File Offset: 0x0002ECDE
	public void NotifyCountChanged()
	{
		Action onCountChanged = this.OnCountChanged;
		if (onCountChanged == null)
		{
			return;
		}
		onCountChanged();
	}

	// Token: 0x060009AC RID: 2476 RVA: 0x00030AF0 File Offset: 0x0002ECF0
	public void TryStartAutoDestroyTimer()
	{
		if (this.dropType == DropType.Item && this.item.Definition.itemGroupIds.Contains("body") && this.item.Definition.itemGroupIds.Contains("corpse"))
		{
			this.autoDestroyTimer = ConstDef.Get("corpse_auto_destroy_timer").FloatValue;
		}
	}

	// Token: 0x04000B2C RID: 2860
	[SerializeField]
	private DropType dropType;

	// Token: 0x04000B2D RID: 2861
	[SerializeField]
	private Item item;

	// Token: 0x04000B2E RID: 2862
	[SerializeField]
	private Vector3 pos;

	// Token: 0x04000B2F RID: 2863
	[SerializeField]
	private string worldId;

	// Token: 0x04000B30 RID: 2864
	[SerializeField]
	private float autoDestroyTimer = -1f;

	// Token: 0x04000B31 RID: 2865
	private WgoData wgoData;

	// Token: 0x04000B34 RID: 2868
	private MultiFlagOR<CanNotBeAutoDestroyedReason> canNotBeAutoDestroyed;
}
