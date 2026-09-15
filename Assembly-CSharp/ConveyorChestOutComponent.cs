using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200056A RID: 1386
[Serializable]
public class ConveyorChestOutComponent : ConveyorComponent
{
	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06002381 RID: 9089 RVA: 0x000A6BD4 File Offset: 0x000A4DD4
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

	// Token: 0x170005BE RID: 1470
	// (get) Token: 0x06002382 RID: 9090 RVA: 0x000A6C5B File Offset: 0x000A4E5B
	public List<ConveyorChestSlotData> SlotsData
	{
		get
		{
			if (this.slotsData == null)
			{
				this.UpdateSlotsData();
			}
			return this.slotsData;
		}
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000A6C71 File Offset: 0x000A4E71
	public ConveyorChestOutComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000A6C90 File Offset: 0x000A4E90
	public override void DoJob(ConveyorComponent visitor = null)
	{
		base.CurrentVisitState = VisitState.Visited;
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000A6C99 File Offset: 0x000A4E99
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		return this.Connect(conveyorWgoData, connectionType, direction, -1);
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000A6CA8 File Offset: 0x000A4EA8
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction, int slotIndex)
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
		this.UpdateSlotsData();
		ConveyorChestSlotData conveyorChestSlotData = ((slotIndex > 0) ? this.SlotsData.Find((ConveyorChestSlotData x) => x.slotIndex == slotIndex) : this.SlotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == direction && x.conveyorWgoDataUniqueId == null));
		if (conveyorChestSlotData == null || conveyorChestSlotData.conveyorWgoDataUniqueId != null)
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
		foreach (ConveyorChestSlotData conveyorChestSlotData2 in this.SlotsData)
		{
			if ((slotIndex > 0) ? (conveyorChestSlotData2.slotIndex == slotIndex) : (conveyorChestSlotData2.slotPosDirection == direction && conveyorChestSlotData2.conveyorWgoDataUniqueId == null))
			{
				conveyorChestSlotData2.conveyorWgoDataUniqueId = conveyorWgoData.UniqueId;
				if (slotIndex > 0)
				{
					conveyorChestSlotData2.slotIndex = slotIndex;
					break;
				}
				break;
			}
		}
		this.occupiedConnectorsDirections.Add(direction);
		base.OnConnectedEvent();
		return true;
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000A6EA0 File Offset: 0x000A50A0
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

	// Token: 0x06002388 RID: 9096 RVA: 0x000A6F9C File Offset: 0x000A519C
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

	// Token: 0x06002389 RID: 9097 RVA: 0x000A7090 File Offset: 0x000A5290
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData.Contains(conveyorComponent.WgoData);
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000A70A8 File Offset: 0x000A52A8
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

	// Token: 0x0600238B RID: 9099 RVA: 0x000A7148 File Offset: 0x000A5348
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		string text;
		return this.TryGetItemIdToGive(conveyorComponent, out text);
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000A7160 File Offset: 0x000A5360
	public override List<Item> GiveItem(ConveyorComponent conveyorComponent)
	{
		string text;
		if (this.TryGetItemIdToGive(conveyorComponent, out text))
		{
			return base.WgoData.Inventory.RemoveItemById(text, 1, null, null, false);
		}
		return new List<Item>();
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x000A7194 File Offset: 0x000A5394
	public override bool HasChildsInDirection(Direction direction)
	{
		foreach (ConveyorChestSlotData conveyorChestSlotData in this.SlotsData)
		{
			if (conveyorChestSlotData.slotPosDirection == direction && !SGuid.IsNullOrEmpty(conveyorChestSlotData.conveyorWgoDataUniqueId))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000A7200 File Offset: 0x000A5400
	public void UpdateSlotsData()
	{
		ConveyorConnectorsSetup conveyorConnectorsSetup = base.WgoData.Definition.conveyorConnectorsSetup;
		if (conveyorConnectorsSetup != null && !conveyorConnectorsSetup.IsEmpty)
		{
			int slotsCount = this.GetSlotsCount(conveyorConnectorsSetup.slotsCount);
			Direction direction = conveyorConnectorsSetup.direction;
			if (this.slotsData == null || this.slotsData.Count != slotsCount)
			{
				int num = 0;
				if (this.slotsData != null)
				{
					num = slotsCount - this.slotsData.Count;
				}
				if (this.slotsData != null && num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						this.slotsData.Add(new ConveyorChestSlotData(direction, this.slotsData.Count + 1));
					}
				}
				else
				{
					this.slotsData = new List<ConveyorChestSlotData>();
					for (int j = 0; j < slotsCount; j++)
					{
						this.slotsData.Add(new ConveyorChestSlotData(direction, j + 1));
					}
				}
			}
			for (int k = 0; k < this.slotsData.Count; k++)
			{
				this.slotsData[k].slotIndex = k + 1;
			}
			return;
		}
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
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000A7354 File Offset: 0x000A5554
	private int GetSlotsCount(int defaultSlotsCount)
	{
		string id = base.WgoData.id;
		int num;
		if (!(id == "garden_bags_storage_1"))
		{
			if (!(id == "garden_bags_storage_2"))
			{
				if (!(id == "garden_bags_storage_3"))
				{
					num = defaultSlotsCount;
				}
				else
				{
					num = 3;
				}
			}
			else
			{
				num = 2;
			}
		}
		else
		{
			num = 2;
		}
		return num;
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000A73A8 File Offset: 0x000A55A8
	private bool TryGetItemIdToGive(ConveyorComponent conveyorComponent, out string itemId)
	{
		itemId = null;
		this.UpdateSlotsData();
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

	// Token: 0x04001FB6 RID: 8118
	private const string GARDEN_BAGS_STORAGE1_ID = "garden_bags_storage_1";

	// Token: 0x04001FB7 RID: 8119
	private const string GARDEN_BAGS_STORAGE2_ID = "garden_bags_storage_2";

	// Token: 0x04001FB8 RID: 8120
	private const string GARDEN_BAGS_STORAGE3_ID = "garden_bags_storage_3";

	// Token: 0x04001FB9 RID: 8121
	[SerializeField]
	private List<SGuid> connectedWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04001FBA RID: 8122
	[SerializeField]
	private List<ConveyorChestSlotData> slotsData;

	// Token: 0x04001FBB RID: 8123
	[NonSerialized]
	public List<ConveyorWgoData> connectedWgoData = new List<ConveyorWgoData>();
}
