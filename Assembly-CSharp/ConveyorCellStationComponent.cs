using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200055F RID: 1375
[Serializable]
public class ConveyorCellStationComponent : ConveyorComponent
{
	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06002344 RID: 9028 RVA: 0x000A52C0 File Offset: 0x000A34C0
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

	// Token: 0x06002345 RID: 9029 RVA: 0x000A5347 File Offset: 0x000A3547
	public ConveyorCellStationComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000A5368 File Offset: 0x000A3568
	public override void DoJob(ConveyorComponent visitor = null)
	{
		base.CurrentVisitState = VisitState.Visiting;
		this.PerformItemTransfer();
		foreach (ConveyorWgoData conveyorWgoData in base.ParentsData.Values)
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
		base.CurrentVisitState = VisitState.Visited;
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000A53F4 File Offset: 0x000A35F4
	public override void PerformItemTransfer()
	{
		if (this.wasPerformedItemTransfer)
		{
			return;
		}
		List<ConveyorWgoData> list = new List<ConveyorWgoData>(base.ParentsData.Values.ToList<ConveyorWgoData>());
		for (int i = 0; i < list.Count; i++)
		{
			this.currentTransferIndex = MathUtilities.ClampCycle(this.currentTransferIndex, 0, list.Count - 1);
			ConveyorWgoData conveyorWgoData = list[this.currentTransferIndex];
			this.currentTransferIndex++;
			if (!conveyorWgoData.CraftComponent.IsDestroyingCraftActive && conveyorWgoData.ConveyorComponent.CanGiveItem(this))
			{
				ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
				if ((conveyorComponent is ConveyorCellComponent || conveyorComponent is ConveyorCellUndergroundComponent || conveyorComponent is ConveyorPalletComponent || conveyorComponent is ConveyorChestComponent || conveyorComponent is ConveyorSplitterComponent) && base.WgoData.Inventory.Data.Inventory.Count == 0)
				{
					List<Item> list2 = conveyorWgoData.ConveyorComponent.GiveItem(this);
					base.WgoData.Inventory.AddItemsToInventory(list2);
					this.wasPerformedItemTransfer = true;
					this.UpdateInOutItemData(conveyorWgoData);
					this.TryPlaceConveyorPickupOrder();
					return;
				}
			}
		}
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000A5510 File Offset: 0x000A3710
	private void TryPlaceConveyorPickupOrder()
	{
		WorldZoneData worldZoneData = base.WgoData.WorldZoneData;
		if (worldZoneData == null)
		{
			return;
		}
		if (base.WgoData.Inventory.Data.Inventory.Count == 0)
		{
			return;
		}
		if (worldZoneData.FindOrdersByTarget(base.WgoData.UniqueId, typeof(ConveyorPickupOrder)).Count > 0)
		{
			return;
		}
		Item item = base.WgoData.Inventory.Data.Inventory[0];
		worldZoneData.PlaceNewOrder(new ConveyorPickupOrder(base.WgoData.UniqueId, new Item(item.id, item.Count)));
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000A55B4 File Offset: 0x000A37B4
	public override bool Disconnect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType)
	{
		if (!this.connectedWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.Log(string.Format("Trying to disconnect  {0} that is not connected", conveyorWgoData.UniqueId));
			return false;
		}
		Debug.Log(string.Format("Disconnected {0} with guid {1} from {2} with guid {3}", new object[]
		{
			conveyorWgoData.id,
			conveyorWgoData.UniqueId,
			base.WgoData.id,
			this.wgoDataUniqueId
		}));
		this.connectedWgoDataUniqueId.Remove(conveyorWgoData.UniqueId);
		this.ConnectedWgoData.Remove(conveyorWgoData);
		conveyorWgoData.ConveyorComponent.RemoveParentData(base.WgoData);
		this.UpdateWgoPartState();
		return true;
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x000A5660 File Offset: 0x000A3860
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData.Contains(conveyorComponent.WgoData);
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000A5678 File Offset: 0x000A3878
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

	// Token: 0x0600234C RID: 9036 RVA: 0x000A5718 File Offset: 0x000A3918
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

	// Token: 0x0600234D RID: 9037 RVA: 0x00028294 File Offset: 0x00026494
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		return false;
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000A579C File Offset: 0x000A399C
	private void UpdateInOutItemData(ConveyorWgoData giver)
	{
		Direction direction;
		base.TryGetParentConnectionDirection(giver.UniqueId, out direction);
		if (giver.Definition.conveyorType == ConveyorElementType.Chest || giver.Definition.conveyorType == ConveyorElementType.Workbench)
		{
			giver.ConveyorComponent.OutItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, false);
			base.InItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, false);
			return;
		}
		giver.ConveyorComponent.OutItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, true);
		base.InItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, true);
	}

	// Token: 0x04001FA4 RID: 8100
	public List<SGuid> connectedWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04001FA5 RID: 8101
	[SerializeField]
	private int currentTransferIndex;

	// Token: 0x04001FA6 RID: 8102
	[NonSerialized]
	public List<ConveyorWgoData> connectedWgoData = new List<ConveyorWgoData>();
}
