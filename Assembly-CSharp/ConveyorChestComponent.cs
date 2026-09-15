using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000562 RID: 1378
[Serializable]
public class ConveyorChestComponent : ConveyorComponent
{
	// Token: 0x170005BB RID: 1467
	// (get) Token: 0x06002361 RID: 9057 RVA: 0x000A6170 File Offset: 0x000A4370
	public List<ConveyorWgoData> ConnectedWgoData
	{
		get
		{
			if (this.connectedWgoData == null)
			{
				this.connectedWgoData = new List<ConveyorWgoData>();
			}
			if (this.connectedWgoData.Count == 0 && this.connectedWgoDataUniqueId.Count > 0)
			{
				for (int i = 0; i < this.connectedWgoDataUniqueId.Count; i++)
				{
					this.connectedWgoData.Add(MainGame.Instance.GameSave.worldData.GetWgoData(this.connectedWgoDataUniqueId[i]) as ConveyorWgoData);
				}
			}
			return this.connectedWgoData;
		}
	}

	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06002362 RID: 9058 RVA: 0x000A61F8 File Offset: 0x000A43F8
	public List<ConveyorChestSlotData> SlotsData
	{
		get
		{
			if (this.slotsData == null)
			{
				this.slotsData = new List<ConveyorChestSlotData>
				{
					new ConveyorChestSlotData(Direction.Left),
					new ConveyorChestSlotData(Direction.Up),
					new ConveyorChestSlotData(Direction.Right),
					new ConveyorChestSlotData(Direction.Down)
				};
			}
			return this.slotsData;
		}
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x000A624E File Offset: 0x000A444E
	public ConveyorChestComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000A6270 File Offset: 0x000A4470
	public override void DoJob(ConveyorComponent visitor = null)
	{
		base.CurrentVisitState = VisitState.Visiting;
		this.PerformItemTransfer();
		foreach (ConveyorWgoData conveyorWgoData in base.ParentsData.Values)
		{
			if (conveyorWgoData != null)
			{
				ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
				if (conveyorComponent.CanBeVisitedBy(this))
				{
					conveyorComponent.DoJob(this);
				}
				else if (conveyorComponent.CurrentVisitState == VisitState.Visiting)
				{
					conveyorComponent.HandleCycleDependency(conveyorComponent);
				}
			}
		}
		base.CurrentVisitState = VisitState.Visited;
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000A6304 File Offset: 0x000A4504
	public override void PerformItemTransfer()
	{
		if (base.WgoData.CraftComponent.IsDestroyingCraftActive)
		{
			return;
		}
		foreach (ConveyorWgoData conveyorWgoData in base.ParentsData.Values)
		{
			if (conveyorWgoData != null && conveyorWgoData.ConveyorComponent.CanGiveItem(this))
			{
				ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
				if ((conveyorComponent is ConveyorCellComponent || conveyorComponent is ConveyorSplitterComponent) && base.WgoData.Inventory.CanAddItemToInventory(conveyorWgoData.Inventory.Data.Inventory[0].id, 1))
				{
					Direction direction;
					base.TryGetParentConnectionDirection(conveyorWgoData.UniqueId, out direction);
					List<Item> list = conveyorWgoData.Inventory.RemoveItemById(conveyorWgoData.Inventory.Data.Inventory[0].id, 1, null, null, false);
					conveyorWgoData.ConveyorComponent.OutItem = new ConveyorMovableItemData(list[0].id, direction, false);
					base.WgoData.Inventory.AddItemsToInventory(list);
					GlobalEventsSystem.FireTrigger(GlobalEventsSystem.Event.Type.ConveyorChestItemAdded, base.WgoData.id + ":" + list[0].id);
					this.wasPerformedItemTransfer = true;
				}
			}
		}
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x000A647C File Offset: 0x000A467C
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		if (this.connectedWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.LogWarning(string.Format("Trying to connect  {0} that is already connected", conveyorWgoData.UniqueId));
			return false;
		}
		ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
		if (!(conveyorComponent is ConveyorCellComponent) && !(conveyorComponent is ConveyorSplitterComponent) && !(conveyorComponent is ConveyorCellUndergroundComponent))
		{
			return false;
		}
		Debug.Log(string.Format("Connected {0} with guid {1} to {2} with guid {3}", new object[]
		{
			conveyorWgoData.id,
			conveyorWgoData.UniqueId,
			base.WgoData.id,
			this.wgoDataUniqueId
		}));
		this.connectedWgoDataUniqueId.Add(conveyorWgoData.UniqueId);
		if (!this.ConnectedWgoData.Contains(conveyorWgoData))
		{
			this.ConnectedWgoData.Add(conveyorWgoData);
		}
		conveyorWgoData.ConveyorComponent.AddParentData(base.WgoData, direction);
		ConveyorChestSlotData conveyorChestSlotData = this.SlotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == direction);
		if (conveyorChestSlotData != null)
		{
			conveyorChestSlotData.conveyorWgoDataUniqueId = conveyorWgoData.UniqueId;
		}
		this.occupiedConnectorsDirections.Add(direction);
		base.OnConnectedEvent();
		return true;
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000A65A0 File Offset: 0x000A47A0
	public override bool Disconnect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType)
	{
		if (!this.connectedWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.LogWarning(string.Format("Trying to disconnect  {0} that is not connected", conveyorWgoData.UniqueId));
			return false;
		}
		Debug.Log(string.Format("Disconnected {0} as {1}", conveyorWgoData.UniqueId, connectionType));
		this.connectedWgoDataUniqueId.Remove(conveyorWgoData.UniqueId);
		this.ConnectedWgoData.Remove(conveyorWgoData);
		Direction direction;
		bool flag = conveyorWgoData.ConveyorComponent.TryGetParentConnectionDirection(this.wgoDataUniqueId, out direction);
		conveyorWgoData.ConveyorComponent.RemoveParentData(base.WgoData);
		ConveyorChestSlotData conveyorChestSlotData = this.SlotsData.Find((ConveyorChestSlotData x) => x.conveyorWgoDataUniqueId == conveyorWgoData.UniqueId);
		if (conveyorChestSlotData != null)
		{
			conveyorChestSlotData.Clear();
		}
		if (flag)
		{
			this.occupiedConnectorsDirections.Remove(direction);
		}
		base.OnDisconnectedEvent();
		return true;
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000A669C File Offset: 0x000A489C
	public override bool DisconnectChilds()
	{
		foreach (ConveyorWgoData conveyorWgoData in this.ConnectedWgoData)
		{
			if (conveyorWgoData != null)
			{
				Debug.Log(string.Format("Disconnected {0} with guid {1} from {2} with guid {3}", new object[]
				{
					conveyorWgoData.id,
					conveyorWgoData.UniqueId,
					base.WgoData.id,
					this.wgoDataUniqueId
				}));
				conveyorWgoData.ConveyorComponent.RemoveParentData(base.WgoData);
			}
		}
		this.connectedWgoDataUniqueId.Clear();
		this.ConnectedWgoData.Clear();
		foreach (ConveyorChestSlotData conveyorChestSlotData in this.SlotsData)
		{
			conveyorChestSlotData.Clear();
		}
		return true;
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000A6790 File Offset: 0x000A4990
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData.Contains(conveyorComponent.WgoData);
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000A67A8 File Offset: 0x000A49A8
	public override void GetEndElement(ref List<ConveyorComponent> endElements)
	{
		base.CurrentVisitState = VisitState.Visiting;
		if (this.ConnectedWgoData.Count == 0)
		{
			endElements.Add(this);
		}
		foreach (ConveyorWgoData conveyorWgoData in this.ConnectedWgoData)
		{
			ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
			if (conveyorComponent.CurrentVisitState == VisitState.NotVisited)
			{
				conveyorComponent.GetEndElement(ref endElements);
			}
			else if (conveyorComponent.CurrentVisitState == VisitState.Visiting && !endElements.Contains(conveyorComponent))
			{
				endElements.Add(this);
			}
		}
		base.CurrentVisitState = VisitState.Visited;
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000A6848 File Offset: 0x000A4A48
	public override void HandleCycleDependency(ConveyorComponent cyrcleComponent)
	{
		if (this.wasPerformedItemTransfer)
		{
			return;
		}
		this.PerformItemTransfer();
		if (base.ParentsData.Values.Contains(cyrcleComponent.WgoData))
		{
			return;
		}
		foreach (ConveyorWgoData conveyorWgoData in base.ParentsData.Values)
		{
			conveyorWgoData.ConveyorComponent.HandleCycleDependency(cyrcleComponent);
		}
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000A68CC File Offset: 0x000A4ACC
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		string text;
		return this.TryGetItemIdToGive(conveyorComponent, out text);
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000A68E4 File Offset: 0x000A4AE4
	public override List<Item> GiveItem(ConveyorComponent conveyorComponent)
	{
		string text;
		if (this.TryGetItemIdToGive(conveyorComponent, out text))
		{
			return base.WgoData.Inventory.RemoveItemById(text, 1, null, null, false);
		}
		return new List<Item>();
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000A6918 File Offset: 0x000A4B18
	public override void AddParentData(ConveyorWgoData conveyorWgoData, Direction direction)
	{
		base.AddParentData(conveyorWgoData, direction);
		Direction oppositeDir = direction.OppositeDir();
		ConveyorChestSlotData conveyorChestSlotData = this.SlotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == oppositeDir);
		if (conveyorChestSlotData != null)
		{
			conveyorChestSlotData.conveyorWgoDataUniqueId = conveyorWgoData.UniqueId;
		}
		this.occupiedConnectorsDirections.Add(oppositeDir);
		base.OnConnectedEvent();
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000A6980 File Offset: 0x000A4B80
	public override void RemoveParentData(ConveyorWgoData conveyorWgoData)
	{
		Direction direction;
		bool flag = base.TryGetParentConnectionDirection(conveyorWgoData.UniqueId, out direction);
		base.RemoveParentData(conveyorWgoData);
		ConveyorChestSlotData conveyorChestSlotData = this.SlotsData.Find((ConveyorChestSlotData x) => x.conveyorWgoDataUniqueId == conveyorWgoData.UniqueId);
		if (conveyorChestSlotData != null)
		{
			conveyorChestSlotData.Clear();
		}
		if (flag)
		{
			this.occupiedConnectorsDirections.Remove(direction.OppositeDir());
		}
		base.OnDisconnectedEvent();
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000A69F4 File Offset: 0x000A4BF4
	public override bool HasChildsInDirection(Direction direction)
	{
		SGuid conveyorWgoDataUniqueId = this.slotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == direction).conveyorWgoDataUniqueId;
		return !SGuid.IsNullOrEmpty(conveyorWgoDataUniqueId) && !this.parentsUniqueIds.Contains(conveyorWgoDataUniqueId);
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000A6A44 File Offset: 0x000A4C44
	public override bool HasParentsInDirection(Direction direction)
	{
		SGuid conveyorWgoDataUniqueId = this.slotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == direction).conveyorWgoDataUniqueId;
		return !SGuid.IsNullOrEmpty(conveyorWgoDataUniqueId) && this.parentsUniqueIds.Contains(conveyorWgoDataUniqueId);
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000A6A94 File Offset: 0x000A4C94
	private bool TryGetItemIdToGive(ConveyorComponent conveyorComponent, out string itemId)
	{
		itemId = null;
		ConveyorChestSlotData conveyorChestSlotData = this.SlotsData.Find((ConveyorChestSlotData x) => x.conveyorWgoDataUniqueId == conveyorComponent.WgoData.UniqueId);
		if (conveyorChestSlotData == null)
		{
			return false;
		}
		if (!string.IsNullOrEmpty(conveyorChestSlotData.slotItemId))
		{
			if (!base.WgoData.Inventory.Data.HasItemQuantityInInventory(conveyorChestSlotData.slotItemId, 1))
			{
				return false;
			}
			itemId = conveyorChestSlotData.slotItemId;
			return true;
		}
		else
		{
			if (base.WgoData.Inventory.Data.Inventory.Count == 0)
			{
				return false;
			}
			itemId = base.WgoData.Inventory.Data.Inventory[0].id;
			return true;
		}
	}

	// Token: 0x04001FAC RID: 8108
	[SerializeField]
	private List<SGuid> connectedWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04001FAD RID: 8109
	[SerializeField]
	private List<ConveyorChestSlotData> slotsData;

	// Token: 0x04001FAE RID: 8110
	[NonSerialized]
	public List<ConveyorWgoData> connectedWgoData = new List<ConveyorWgoData>();
}
