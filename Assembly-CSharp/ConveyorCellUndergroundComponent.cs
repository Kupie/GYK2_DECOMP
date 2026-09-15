using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000560 RID: 1376
[Serializable]
public class ConveyorCellUndergroundComponent : ConveyorComponent
{
	// Token: 0x170005BA RID: 1466
	// (get) Token: 0x0600234F RID: 9039 RVA: 0x000A5894 File Offset: 0x000A3A94
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

	// Token: 0x06002350 RID: 9040 RVA: 0x000A591B File Offset: 0x000A3B1B
	public ConveyorCellUndergroundComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x000A593C File Offset: 0x000A3B3C
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

	// Token: 0x06002352 RID: 9042 RVA: 0x000A59C8 File Offset: 0x000A3BC8
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
					return;
				}
			}
		}
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000A5ADC File Offset: 0x000A3CDC
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		if (this.connectedWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.LogWarning(string.Format("Trying to connect  {0} that is already connected", conveyorWgoData.UniqueId));
			return false;
		}
		ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
		if (!(conveyorComponent is ConveyorCellComponent) && !(conveyorComponent is ConveyorChestComponent) && !(conveyorComponent is ConveyorSplitterComponent) && !(conveyorComponent is ConveyorCellUndergroundComponent))
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

	// Token: 0x06002354 RID: 9044 RVA: 0x000A5BC0 File Offset: 0x000A3DC0
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

	// Token: 0x06002355 RID: 9045 RVA: 0x000A5C6C File Offset: 0x000A3E6C
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

	// Token: 0x06002356 RID: 9046 RVA: 0x000A5D24 File Offset: 0x000A3F24
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData.Contains(conveyorComponent.WgoData);
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000A5D3C File Offset: 0x000A3F3C
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

	// Token: 0x06002358 RID: 9048 RVA: 0x000A4F20 File Offset: 0x000A3120
	public override void AddParentData(ConveyorWgoData conveyorWgoData, Direction direction)
	{
		base.AddParentData(conveyorWgoData, direction);
		this.UpdateWgoPartState();
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000A4F30 File Offset: 0x000A3130
	public override void RemoveParentData(ConveyorWgoData conveyorWgoData)
	{
		base.RemoveParentData(conveyorWgoData);
		this.UpdateWgoPartState();
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000A5EE8 File Offset: 0x000A40E8
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

	// Token: 0x0600235B RID: 9051 RVA: 0x000A5F88 File Offset: 0x000A4188
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

	// Token: 0x0600235C RID: 9052 RVA: 0x000A600C File Offset: 0x000A420C
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		return !this.wasPerformedItemTransfer && base.WgoData.Inventory.Data.Inventory.Count > 0;
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000A6038 File Offset: 0x000A4238
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

	// Token: 0x04001FA7 RID: 8103
	public List<SGuid> connectedWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04001FA8 RID: 8104
	[SerializeField]
	private int currentTransferIndex;

	// Token: 0x04001FA9 RID: 8105
	[NonSerialized]
	public List<ConveyorWgoData> connectedWgoData = new List<ConveyorWgoData>();
}
