using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200055D RID: 1373
[Serializable]
public class ConveyorCellComponent : ConveyorComponent
{
	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x06002332 RID: 9010 RVA: 0x000A48BC File Offset: 0x000A2ABC
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

	// Token: 0x06002333 RID: 9011 RVA: 0x000A4943 File Offset: 0x000A2B43
	public ConveyorCellComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x06002334 RID: 9012 RVA: 0x000A4964 File Offset: 0x000A2B64
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

	// Token: 0x06002335 RID: 9013 RVA: 0x000A49F0 File Offset: 0x000A2BF0
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
				if ((conveyorComponent is ConveyorCellComponent || conveyorComponent is ConveyorCellUndergroundComponent || conveyorComponent is ConveyorPalletComponent || conveyorComponent is ConveyorChestComponent || conveyorComponent is ConveyorSplitterComponent || conveyorComponent is ConveyorChestOutComponent) && base.WgoData.Inventory.Data.Inventory.Count == 0)
				{
					List<Item> list2 = conveyorWgoData.ConveyorComponent.GiveItem(this);
					base.WgoData.Inventory.AddItemsToInventory(list2);
					this.wasPerformedItemTransfer = true;
					this.UpdateInOutItemData(conveyorWgoData);
					return;
				}
			}
		}
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000A4B0C File Offset: 0x000A2D0C
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		if (this.connectedWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.LogWarning(string.Format("Trying to connect  {0} that is already connected", conveyorWgoData.UniqueId));
			return false;
		}
		ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
		if (!(conveyorComponent is ConveyorCellComponent) && !(conveyorComponent is ConveyorChestComponent) && !(conveyorComponent is ConveyorSplitterComponent) && !(conveyorComponent is ConveyorCellUndergroundComponent) && !(conveyorComponent is ConveyorCellStationComponent))
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
		this.UpdateWgoPartState();
		return true;
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000A4BF8 File Offset: 0x000A2DF8
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

	// Token: 0x06002338 RID: 9016 RVA: 0x000A4CA4 File Offset: 0x000A2EA4
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
		return true;
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000A4D5C File Offset: 0x000A2F5C
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData.Contains(conveyorComponent.WgoData);
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000A4D74 File Offset: 0x000A2F74
	public override void UpdateWgoPartState()
	{
		if (base.WgoData == null)
		{
			return;
		}
		ConveyorWgoData conveyorWgoData = this.ConnectedWgoData.Find(delegate(ConveyorWgoData x)
		{
			if (x != null)
			{
				ConveyorComponent conveyorComponent = x.ConveyorComponent;
				return conveyorComponent is ConveyorCellComponent || conveyorComponent is ConveyorCellUndergroundComponent;
			}
			return false;
		});
		int rotationIndex = base.WgoData.MainWgoPartData.rotationIndex;
		bool flag = false;
		bool flag2 = conveyorWgoData != null && conveyorWgoData.MainWgoPartData.rotationIndex == rotationIndex;
		if (this.parentsUniqueIds.Count > 0)
		{
			foreach (ConveyorWgoData conveyorWgoData2 in base.ParentsData.Values)
			{
				ConveyorElementType conveyorType = conveyorWgoData2.Definition.conveyorType;
				if (conveyorType == ConveyorElementType.Cell)
				{
					goto IL_00A7;
				}
				if (conveyorType != ConveyorElementType.Splitter)
				{
					if (conveyorType == ConveyorElementType.UndergroundCell)
					{
						goto IL_00A7;
					}
				}
				else if (conveyorWgoData2.MainWgoPartData.rotationIndex % 2 == base.WgoData.MainWgoPartData.rotationIndex % 2)
				{
					flag = true;
				}
				IL_00DE:
				if (flag)
				{
					break;
				}
				continue;
				IL_00A7:
				if (conveyorWgoData2.MainWgoPartData.rotationIndex == rotationIndex)
				{
					flag = true;
					goto IL_00DE;
				}
				goto IL_00DE;
			}
		}
		if (flag && flag2)
		{
			base.WgoData.ApplyWgoPartState("centre", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		if (!flag && flag2)
		{
			base.WgoData.ApplyWgoPartState("start", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		if (flag)
		{
			base.WgoData.ApplyWgoPartState("end", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		base.WgoData.ApplyWgoPartState("single", base.WgoData.MainWgoPartData.rotationIndex);
	}

	// Token: 0x0600233B RID: 9019 RVA: 0x000A4F20 File Offset: 0x000A3120
	public override void AddParentData(ConveyorWgoData conveyorWgoData, Direction direction)
	{
		base.AddParentData(conveyorWgoData, direction);
		this.UpdateWgoPartState();
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000A4F30 File Offset: 0x000A3130
	public override void RemoveParentData(ConveyorWgoData conveyorWgoData)
	{
		base.RemoveParentData(conveyorWgoData);
		this.UpdateWgoPartState();
	}

	// Token: 0x0600233D RID: 9021 RVA: 0x000A4F40 File Offset: 0x000A3140
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

	// Token: 0x0600233E RID: 9022 RVA: 0x000A4FE0 File Offset: 0x000A31E0
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

	// Token: 0x0600233F RID: 9023 RVA: 0x000A5064 File Offset: 0x000A3264
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		if (this.wasPerformedItemTransfer)
		{
			return false;
		}
		bool flag = false;
		foreach (Item item in base.WgoData.Inventory.Data.Inventory)
		{
			if (conveyorComponent.WgoData.Definition.inventoryWhiteList.Contains(item.Definition) && !conveyorComponent.WgoData.Definition.inventoryBlackList.Contains(item.Definition))
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000A510C File Offset: 0x000A330C
	private void UpdateInOutItemData(ConveyorWgoData giver)
	{
		Direction direction;
		base.TryGetParentConnectionDirection(giver.UniqueId, out direction);
		if (giver.Definition.conveyorType == ConveyorElementType.Chest || giver.Definition.conveyorType == ConveyorElementType.ChestOut || giver.Definition.conveyorType == ConveyorElementType.Workbench)
		{
			giver.ConveyorComponent.OutItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, false);
			base.InItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, false);
			return;
		}
		if (giver.Definition.conveyorType == ConveyorElementType.Splitter)
		{
			giver.ConveyorComponent.OutItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, true);
			base.InItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, true);
			return;
		}
		giver.ConveyorComponent.OutItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, true);
		base.InItem = new ConveyorMovableItemData(base.WgoData.Inventory.Data.Inventory[0].id, direction, true);
	}

	// Token: 0x04001F9F RID: 8095
	public List<SGuid> connectedWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04001FA0 RID: 8096
	[SerializeField]
	private int currentTransferIndex;

	// Token: 0x04001FA1 RID: 8097
	[NonSerialized]
	public List<ConveyorWgoData> connectedWgoData = new List<ConveyorWgoData>();
}
