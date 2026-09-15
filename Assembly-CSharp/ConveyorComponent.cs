using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056F RID: 1391
[Serializable]
public class ConveyorComponent
{
	// Token: 0x1400005C RID: 92
	// (add) Token: 0x0600239D RID: 9117 RVA: 0x000A7568 File Offset: 0x000A5768
	// (remove) Token: 0x0600239E RID: 9118 RVA: 0x000A75A0 File Offset: 0x000A57A0
	public event Action OnConnected;

	// Token: 0x1400005D RID: 93
	// (add) Token: 0x0600239F RID: 9119 RVA: 0x000A75D8 File Offset: 0x000A57D8
	// (remove) Token: 0x060023A0 RID: 9120 RVA: 0x000A7610 File Offset: 0x000A5810
	public event Action OnDisconnected;

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x060023A1 RID: 9121 RVA: 0x000A7645 File Offset: 0x000A5845
	// (set) Token: 0x060023A2 RID: 9122 RVA: 0x000A764D File Offset: 0x000A584D
	public VisitState CurrentVisitState { get; set; }

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x060023A3 RID: 9123 RVA: 0x000A7658 File Offset: 0x000A5858
	// (set) Token: 0x060023A4 RID: 9124 RVA: 0x000A76AF File Offset: 0x000A58AF
	public ConveyorWgoData WgoData
	{
		get
		{
			if (this.wgoData != null)
			{
				return this.wgoData;
			}
			if (this.wgoDataUniqueId == null)
			{
				return null;
			}
			this.wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.wgoDataUniqueId) as ConveyorWgoData;
			return this.wgoData;
		}
		set
		{
			this.wgoData = value;
			this.wgoDataUniqueId = this.wgoData.UniqueId;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x060023A5 RID: 9125 RVA: 0x000A76CC File Offset: 0x000A58CC
	public Dictionary<SGuid, ConveyorWgoData> ParentsData
	{
		get
		{
			if (this.parentsData == null)
			{
				this.parentsData = new Dictionary<SGuid, ConveyorWgoData>();
			}
			if (this.parentsUniqueIds.Count > 0 && this.parentsData.Count == 0)
			{
				foreach (SGuid sguid in this.parentsUniqueIds)
				{
					this.parentsData.TryAdd(sguid, MainGame.Instance.GameSave.worldData.GetWgoData(sguid) as ConveyorWgoData);
				}
			}
			return this.parentsData;
		}
	}

	// Token: 0x170005C3 RID: 1475
	// (get) Token: 0x060023A6 RID: 9126 RVA: 0x000A7774 File Offset: 0x000A5974
	public List<ConveyorConnectionData> ParentsConnectionsData
	{
		get
		{
			if (this.parentsConnectionsData == null)
			{
				this.parentsConnectionsData = new List<ConveyorConnectionData>();
			}
			return this.parentsConnectionsData;
		}
	}

	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x060023A7 RID: 9127 RVA: 0x000A778F File Offset: 0x000A598F
	// (set) Token: 0x060023A8 RID: 9128 RVA: 0x000A7797 File Offset: 0x000A5997
	public ConveyorMovableItemData OutItem
	{
		get
		{
			return this.outItem;
		}
		set
		{
			this.outItem = value;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x060023A9 RID: 9129 RVA: 0x000A77A0 File Offset: 0x000A59A0
	// (set) Token: 0x060023AA RID: 9130 RVA: 0x000A77A8 File Offset: 0x000A59A8
	public ConveyorMovableItemData InItem
	{
		get
		{
			return this.inItem;
		}
		set
		{
			this.inItem = value;
		}
	}

	// Token: 0x060023AB RID: 9131 RVA: 0x000A77B4 File Offset: 0x000A59B4
	public ConveyorComponent(ConveyorWgoData parentWgoData)
	{
		this.wgoDataUniqueId = parentWgoData.UniqueId;
		this.wgoData = parentWgoData;
	}

	// Token: 0x060023AC RID: 9132 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void Init()
	{
	}

	// Token: 0x060023AD RID: 9133 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void DeInit()
	{
	}

	// Token: 0x060023AE RID: 9134 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void GetEndElement(ref List<ConveyorComponent> endElements)
	{
	}

	// Token: 0x060023AF RID: 9135 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void PerformItemTransfer()
	{
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void DoJob(ConveyorComponent visitor = null)
	{
	}

	// Token: 0x060023B1 RID: 9137 RVA: 0x000A7806 File Offset: 0x000A5A06
	public virtual bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		Debug.Log("Conveyer Wgo data connected");
		return true;
	}

	// Token: 0x060023B2 RID: 9138 RVA: 0x000A7813 File Offset: 0x000A5A13
	public virtual bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction, int slotIndex)
	{
		return this.Connect(conveyorWgoData, connectionType, direction);
	}

	// Token: 0x060023B3 RID: 9139 RVA: 0x000A781E File Offset: 0x000A5A1E
	public virtual bool Disconnect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType)
	{
		Debug.Log("Conveyer Wgo data disconnected");
		return true;
	}

	// Token: 0x060023B4 RID: 9140 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	public virtual bool DisconnectChilds()
	{
		return true;
	}

	// Token: 0x060023B5 RID: 9141 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool Contains(ConveyorComponent conveyorComponent)
	{
		return false;
	}

	// Token: 0x060023B6 RID: 9142 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void UpdateWgoPartState()
	{
	}

	// Token: 0x060023B7 RID: 9143 RVA: 0x000A782C File Offset: 0x000A5A2C
	public virtual void AddParentData(ConveyorWgoData conveyorWgoData, Direction direction)
	{
		ConveyorWgoData conveyorWgoData2;
		if (!this.ParentsData.TryGetValue(conveyorWgoData.UniqueId, out conveyorWgoData2))
		{
			this.ParentsData.Add(conveyorWgoData.UniqueId, conveyorWgoData);
			this.parentsUniqueIds.Add(conveyorWgoData.UniqueId);
			this.ParentsConnectionsData.Add(new ConveyorConnectionData(conveyorWgoData.UniqueId, direction));
		}
	}

	// Token: 0x060023B8 RID: 9144 RVA: 0x000A7888 File Offset: 0x000A5A88
	public virtual void RemoveParentData(ConveyorWgoData conveyorWgoData)
	{
		ConveyorWgoData conveyorWgoData2;
		if (!this.ParentsData.TryGetValue(conveyorWgoData.UniqueId, out conveyorWgoData2))
		{
			return;
		}
		this.ParentsData.Remove(conveyorWgoData.UniqueId);
		this.parentsUniqueIds.Remove(conveyorWgoData.UniqueId);
		this.ParentsConnectionsData.RemoveAll((ConveyorConnectionData x) => x.connectedUniqueId == conveyorWgoData.UniqueId);
	}

	// Token: 0x060023B9 RID: 9145 RVA: 0x000A7904 File Offset: 0x000A5B04
	public void RemoveParentLink(SGuid parentUniqueId)
	{
		if (SGuid.IsNullOrEmpty(parentUniqueId))
		{
			return;
		}
		Dictionary<SGuid, ConveyorWgoData> dictionary = this.parentsData;
		if (dictionary != null)
		{
			dictionary.Remove(parentUniqueId);
		}
		List<SGuid> list = this.parentsUniqueIds;
		if (list != null)
		{
			list.Remove(parentUniqueId);
		}
		List<ConveyorConnectionData> list2 = this.parentsConnectionsData;
		if (list2 == null)
		{
			return;
		}
		list2.RemoveAll((ConveyorConnectionData x) => x.connectedUniqueId == parentUniqueId);
	}

	// Token: 0x060023BA RID: 9146 RVA: 0x000A797C File Offset: 0x000A5B7C
	public bool TryGetParentConnectionDirection(SGuid parentUniqueId, out Direction direction)
	{
		ConveyorConnectionData conveyorConnectionData = this.ParentsConnectionsData.Find((ConveyorConnectionData x) => x.connectedUniqueId == parentUniqueId);
		direction = ((conveyorConnectionData == null) ? Direction.Down : conveyorConnectionData.connectionDirection);
		return conveyorConnectionData != null;
	}

	// Token: 0x060023BB RID: 9147 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void HandleCycleDependency(ConveyorComponent inputComponent)
	{
	}

	// Token: 0x060023BC RID: 9148 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		return false;
	}

	// Token: 0x060023BD RID: 9149 RVA: 0x000A79C0 File Offset: 0x000A5BC0
	public virtual bool CanBeVisitedBy(ConveyorComponent conveyorComponent)
	{
		return this.CurrentVisitState == VisitState.NotVisited;
	}

	// Token: 0x060023BE RID: 9150 RVA: 0x000A79CB File Offset: 0x000A5BCB
	public virtual List<Item> GiveItem(ConveyorComponent reciever)
	{
		return this.WgoData.Inventory.RemoveItemById(this.WgoData.Inventory.Data.Inventory[0].id, 1, null, null, false);
	}

	// Token: 0x060023BF RID: 9151 RVA: 0x000A7A01 File Offset: 0x000A5C01
	public virtual void ClearInAndOutItemDatas()
	{
		this.outItem = null;
		this.inItem = null;
	}

	// Token: 0x060023C0 RID: 9152 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool HasChildsInDirection(Direction direction)
	{
		return false;
	}

	// Token: 0x060023C1 RID: 9153 RVA: 0x00028294 File Offset: 0x00026494
	public virtual bool HasParentsInDirection(Direction direction)
	{
		return false;
	}

	// Token: 0x060023C2 RID: 9154 RVA: 0x000A7A11 File Offset: 0x000A5C11
	public void OnConnectedEvent()
	{
		Action onConnected = this.OnConnected;
		if (onConnected == null)
		{
			return;
		}
		onConnected();
	}

	// Token: 0x060023C3 RID: 9155 RVA: 0x000A7A23 File Offset: 0x000A5C23
	public void OnDisconnectedEvent()
	{
		Action onDisconnected = this.OnDisconnected;
		if (onDisconnected == null)
		{
			return;
		}
		onDisconnected();
	}

	// Token: 0x04001FC7 RID: 8135
	public bool wasPerformedItemTransfer;

	// Token: 0x04001FC9 RID: 8137
	public SGuid wgoDataUniqueId;

	// Token: 0x04001FCA RID: 8138
	public List<Direction> occupiedConnectorsDirections = new List<Direction>();

	// Token: 0x04001FCB RID: 8139
	private ConveyorMovableItemData outItem;

	// Token: 0x04001FCC RID: 8140
	private ConveyorMovableItemData inItem;

	// Token: 0x04001FCD RID: 8141
	[SerializeField]
	protected List<SGuid> parentsUniqueIds = new List<SGuid>();

	// Token: 0x04001FCE RID: 8142
	[SerializeField]
	protected List<ConveyorConnectionData> parentsConnectionsData = new List<ConveyorConnectionData>();

	// Token: 0x04001FCF RID: 8143
	private Dictionary<SGuid, ConveyorWgoData> parentsData = new Dictionary<SGuid, ConveyorWgoData>();

	// Token: 0x04001FD0 RID: 8144
	private ConveyorWgoData wgoData;
}
