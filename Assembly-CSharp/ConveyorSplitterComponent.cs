using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200057C RID: 1404
[Serializable]
public class ConveyorSplitterComponent : ConveyorComponent
{
	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x060023DD RID: 9181 RVA: 0x000A82B8 File Offset: 0x000A64B8
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

	// Token: 0x060023DE RID: 9182 RVA: 0x000A833F File Offset: 0x000A653F
	public ConveyorSplitterComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000A8360 File Offset: 0x000A6560
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

	// Token: 0x060023E0 RID: 9184 RVA: 0x000A83EC File Offset: 0x000A65EC
	public override void PerformItemTransfer()
	{
		if (this.wasPerformedItemTransfer)
		{
			return;
		}
		List<ConveyorWgoData> list = new List<ConveyorWgoData>(base.ParentsData.Values.ToList<ConveyorWgoData>());
		for (int i = 0; i < list.Count; i++)
		{
			this.currentTransferIndexFrom = MathUtilities.ClampCycle(this.currentTransferIndexFrom, 0, list.Count - 1);
			ConveyorWgoData conveyorWgoData = list[this.currentTransferIndexFrom];
			this.currentTransferIndexFrom++;
			if (!conveyorWgoData.CraftComponent.IsDestroyingCraftActive && conveyorWgoData.ConveyorComponent.CanGiveItem(this))
			{
				ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
				if ((conveyorComponent is ConveyorCellComponent || conveyorComponent is ConveyorCellUndergroundComponent || conveyorComponent is ConveyorPalletComponent || conveyorComponent is ConveyorChestComponent || conveyorComponent is ConveyorSplitterComponent) && base.WgoData.Inventory.Data.Inventory.Count == 0)
				{
					base.WgoData.Inventory.AddItemsToInventory(conveyorWgoData.ConveyorComponent.GiveItem(this));
					this.wasPerformedItemTransfer = true;
					this.UpdateInOutItemData(conveyorWgoData);
					return;
				}
			}
		}
	}

	// Token: 0x060023E1 RID: 9185 RVA: 0x000A84FC File Offset: 0x000A66FC
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

	// Token: 0x060023E2 RID: 9186 RVA: 0x000A85E0 File Offset: 0x000A67E0
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

	// Token: 0x060023E3 RID: 9187 RVA: 0x000A868C File Offset: 0x000A688C
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

	// Token: 0x060023E4 RID: 9188 RVA: 0x000A8744 File Offset: 0x000A6944
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData.Contains(conveyorComponent.WgoData);
	}

	// Token: 0x060023E5 RID: 9189 RVA: 0x000A875C File Offset: 0x000A695C
	public override void UpdateWgoPartState()
	{
		if (base.WgoData == null)
		{
			return;
		}
		int rotationIndex = base.WgoData.MainWgoPartData.rotationIndex;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		foreach (ConveyorWgoData conveyorWgoData in this.ConnectedWgoData)
		{
			ConveyorComponent conveyorComponent = ((conveyorWgoData != null) ? conveyorWgoData.ConveyorComponent : null);
			if (conveyorComponent is ConveyorCellComponent || conveyorComponent is ConveyorCellUndergroundComponent)
			{
				if (rotationIndex == 0)
				{
					if (!flag3)
					{
						flag3 = conveyorWgoData.MainWgoPartData.rotationIndex == 2;
					}
					if (!flag4)
					{
						flag4 = conveyorWgoData.MainWgoPartData.rotationIndex == 0;
					}
				}
				else if (rotationIndex == 1)
				{
					if (!flag2)
					{
						flag2 = conveyorWgoData.MainWgoPartData.rotationIndex == 1;
					}
					if (!flag)
					{
						flag = conveyorWgoData.MainWgoPartData.rotationIndex == 3;
					}
				}
			}
		}
		if ((flag3 && flag4) || (flag2 && flag))
		{
			base.WgoData.ApplyWgoPartState("centre", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		if (flag3)
		{
			base.WgoData.ApplyWgoPartState("centre_down_end", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		if (flag4)
		{
			base.WgoData.ApplyWgoPartState("centre_up_end", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		if (flag2)
		{
			base.WgoData.ApplyWgoPartState("centre_right_end", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		if (flag)
		{
			base.WgoData.ApplyWgoPartState("centre_left_end", base.WgoData.MainWgoPartData.rotationIndex);
			return;
		}
		base.WgoData.ApplyWgoPartState("end", base.WgoData.MainWgoPartData.rotationIndex);
	}

	// Token: 0x060023E6 RID: 9190 RVA: 0x000A4F20 File Offset: 0x000A3120
	public override void AddParentData(ConveyorWgoData conveyorWgoData, Direction direction)
	{
		base.AddParentData(conveyorWgoData, direction);
		this.UpdateWgoPartState();
	}

	// Token: 0x060023E7 RID: 9191 RVA: 0x000A4F30 File Offset: 0x000A3130
	public override void RemoveParentData(ConveyorWgoData conveyorWgoData)
	{
		base.RemoveParentData(conveyorWgoData);
		this.UpdateWgoPartState();
	}

	// Token: 0x060023E8 RID: 9192 RVA: 0x000A892C File Offset: 0x000A6B2C
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

	// Token: 0x060023E9 RID: 9193 RVA: 0x000A89CC File Offset: 0x000A6BCC
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

	// Token: 0x060023EA RID: 9194 RVA: 0x000A8A50 File Offset: 0x000A6C50
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		if (this.wasPerformedItemTransfer)
		{
			return false;
		}
		if (this.ConnectedWgoData.Count == 2 && this.ConnectedWgoData[this.currentChildTransferIndex] == conveyorComponent.WgoData)
		{
			return base.WgoData.Inventory.Data.Inventory.Count > 0;
		}
		return this.ConnectedWgoData.Count == 1 && this.ConnectedWgoData[0] == conveyorComponent.WgoData && base.WgoData.Inventory.Data.Inventory.Count > 0;
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x000A8AED File Offset: 0x000A6CED
	public void SwitchDirection()
	{
		this.currentChildTransferIndex++;
		this.currentChildTransferIndex = MathUtilities.ClampCycle(this.currentChildTransferIndex, 0, 1);
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x000A8B10 File Offset: 0x000A6D10
	public override bool CanBeVisitedBy(ConveyorComponent conveyorComponent)
	{
		if (conveyorComponent == null)
		{
			return base.CurrentVisitState == VisitState.NotVisited;
		}
		return (this.ConnectedWgoData.Count == 2 && conveyorComponent.WgoData == this.ConnectedWgoData[this.currentChildTransferIndex]) || (this.ConnectedWgoData.Count == 1 && this.ConnectedWgoData[0] == conveyorComponent.WgoData);
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x000A8B78 File Offset: 0x000A6D78
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

	// Token: 0x04001FFA RID: 8186
	private const int MAX_CHILDREN_COUNT = 2;

	// Token: 0x04001FFB RID: 8187
	public List<SGuid> connectedWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04001FFC RID: 8188
	[SerializeField]
	private int currentTransferIndexFrom;

	// Token: 0x04001FFD RID: 8189
	[SerializeField]
	private int currentChildTransferIndex;

	// Token: 0x04001FFE RID: 8190
	[NonSerialized]
	public List<ConveyorWgoData> connectedWgoData = new List<ConveyorWgoData>();
}
