using System;
using System.Collections.Generic;
using LinqTools;
using UnityEngine;

// Token: 0x0200057E RID: 1406
[Serializable]
public class ConveyorWorkbenchComponent : ConveyorComponent
{
	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000A8FD8 File Offset: 0x000A71D8
	public List<ConveyorWgoData> ConveyorInWgoDataList
	{
		get
		{
			if (this.conveyorInWgoDataList == null)
			{
				this.conveyorInWgoDataList = new List<ConveyorWgoData>();
			}
			if (this.conveyorInWgoDataList.Count == 0 && this.connectedInWgoDataUniqueId.Count > 0)
			{
				for (int i = 0; i < this.connectedInWgoDataUniqueId.Count; i++)
				{
					ConveyorWgoData conveyorWgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.connectedInWgoDataUniqueId[i]) as ConveyorWgoData;
					if (conveyorWgoData == null)
					{
						Debug.LogError(string.Format("Conveyor workbench [{0}]. Can not find Connected In WGO with UniqueId [{1}]", base.WgoData.id, this.connectedInWgoDataUniqueId[i]));
					}
					else
					{
						this.conveyorInWgoDataList.Add(conveyorWgoData);
					}
				}
			}
			return this.conveyorInWgoDataList;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x060023F9 RID: 9209 RVA: 0x000A9090 File Offset: 0x000A7290
	private List<ConveyorWgoData> ConveyorOutWgoDataList
	{
		get
		{
			if (this.conveyorOutWgoDataList == null)
			{
				this.conveyorOutWgoDataList = new List<ConveyorWgoData>();
			}
			if (this.conveyorOutWgoDataList.Count == 0 && this.connectedOutWgoDataUniqueId.Count > 0)
			{
				for (int i = 0; i < this.connectedOutWgoDataUniqueId.Count; i++)
				{
					ConveyorWgoData conveyorWgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.connectedOutWgoDataUniqueId[i]) as ConveyorWgoData;
					if (conveyorWgoData != null)
					{
						this.conveyorOutWgoDataList.Add(conveyorWgoData);
					}
				}
			}
			return this.conveyorOutWgoDataList;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x060023FA RID: 9210 RVA: 0x000A911C File Offset: 0x000A731C
	private List<ConveyorConnectionData> OccupiedConnectorsData
	{
		get
		{
			if (this.occupiedConnectorsData == null)
			{
				this.occupiedConnectorsData = new List<ConveyorConnectionData>();
			}
			return this.occupiedConnectorsData;
		}
	}

	// Token: 0x060023FB RID: 9211 RVA: 0x000A9137 File Offset: 0x000A7337
	public ConveyorWorkbenchComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x060023FC RID: 9212 RVA: 0x000A9177 File Offset: 0x000A7377
	public override void Init()
	{
		base.WgoData.CraftComponent.OnStatusChanged += this.DoJobOutForce;
	}

	// Token: 0x060023FD RID: 9213 RVA: 0x000A9195 File Offset: 0x000A7395
	public override void DeInit()
	{
		base.WgoData.CraftComponent.OnStatusChanged -= this.DoJobOutForce;
	}

	// Token: 0x060023FE RID: 9214 RVA: 0x000A6C90 File Offset: 0x000A4E90
	public override void DoJob(ConveyorComponent visitor = null)
	{
		base.CurrentVisitState = VisitState.Visited;
	}

	// Token: 0x060023FF RID: 9215 RVA: 0x00002318 File Offset: 0x00000518
	public override void PerformItemTransfer()
	{
	}

	// Token: 0x06002400 RID: 9216 RVA: 0x000A91B4 File Offset: 0x000A73B4
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		if (this.connectedOutWgoDataUniqueId.Contains(conveyorWgoData.UniqueId) || this.connectedInWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.LogWarning(string.Format("Trying to connect {0} that is already connected", conveyorWgoData.UniqueId));
			return false;
		}
		ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
		if (!(conveyorComponent is ConveyorCellComponent) && !(conveyorComponent is ConveyorSplitterComponent))
		{
			return false;
		}
		Debug.Log(string.Format("Connected {0} with guid {1} as {2} to {3} with guid {4}", new object[]
		{
			conveyorWgoData.id,
			conveyorWgoData.UniqueId,
			connectionType,
			base.WgoData.id,
			this.wgoDataUniqueId
		}));
		if (connectionType != ConveyorConnectionType.Out)
		{
			if (connectionType == ConveyorConnectionType.In)
			{
				this.connectedInWgoDataUniqueId.Add(conveyorWgoData.UniqueId);
				if (!this.ConveyorInWgoDataList.Contains(conveyorWgoData))
				{
					this.ConveyorInWgoDataList.Add(conveyorWgoData);
				}
			}
		}
		else
		{
			this.connectedOutWgoDataUniqueId.Add(conveyorWgoData.UniqueId);
			if (!this.ConveyorOutWgoDataList.Contains(conveyorWgoData))
			{
				this.ConveyorOutWgoDataList.Add(conveyorWgoData);
			}
		}
		this.OccupiedConnectorsData.Add(new ConveyorConnectionData(conveyorWgoData.UniqueId, direction));
		this.occupiedConnectorsDirections.Add(direction);
		base.OnConnectedEvent();
		return true;
	}

	// Token: 0x06002401 RID: 9217 RVA: 0x000A92E8 File Offset: 0x000A74E8
	public override bool Disconnect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType)
	{
		if (!this.connectedOutWgoDataUniqueId.Contains(conveyorWgoData.UniqueId) && !this.connectedInWgoDataUniqueId.Contains(conveyorWgoData.UniqueId))
		{
			Debug.Log(string.Format("Trying to disconnect {0} that is not connected", conveyorWgoData.UniqueId));
			return false;
		}
		Debug.Log(string.Format("Disconnected {0} with guid {1} as {2} from {3} with guid {4}", new object[]
		{
			conveyorWgoData.id,
			conveyorWgoData.UniqueId,
			connectionType,
			base.WgoData.id,
			this.wgoDataUniqueId
		}));
		if (connectionType != ConveyorConnectionType.Out)
		{
			if (connectionType == ConveyorConnectionType.In)
			{
				this.connectedInWgoDataUniqueId.Remove(conveyorWgoData.UniqueId);
				this.ConveyorInWgoDataList.Remove(conveyorWgoData);
			}
		}
		else
		{
			this.connectedOutWgoDataUniqueId.Remove(conveyorWgoData.UniqueId);
			this.ConveyorOutWgoDataList.Remove(conveyorWgoData);
		}
		Direction direction;
		if (this.TryGetConnectorDirection(conveyorWgoData.UniqueId, out direction))
		{
			this.OccupiedConnectorsData.RemoveAll((ConveyorConnectionData x) => x.connectedUniqueId == conveyorWgoData.UniqueId);
			this.occupiedConnectorsDirections.Remove(direction);
		}
		base.OnDisconnectedEvent();
		return true;
	}

	// Token: 0x06002402 RID: 9218 RVA: 0x000A943C File Offset: 0x000A763C
	public override bool DisconnectChilds()
	{
		foreach (ConveyorWgoData conveyorWgoData in this.ConveyorInWgoDataList)
		{
			if (conveyorWgoData != null)
			{
				Debug.Log(string.Format("Disconnected {0} with guid {1} as {2} from {3} with guid {4}", new object[]
				{
					conveyorWgoData.id,
					conveyorWgoData.UniqueId,
					ConveyorConnectionType.In,
					base.WgoData.id,
					this.wgoDataUniqueId
				}));
				conveyorWgoData.ConveyorComponent.Disconnect(base.WgoData, ConveyorConnectionType.In);
				conveyorWgoData.ConveyorComponent.RemoveParentData(base.WgoData);
			}
		}
		foreach (ConveyorWgoData conveyorWgoData2 in this.ConveyorOutWgoDataList)
		{
			if (conveyorWgoData2 != null)
			{
				Debug.Log(string.Format("Disconnected {0} with guid {1} as {2} from {3} with guid {4}", new object[]
				{
					conveyorWgoData2.id,
					conveyorWgoData2.UniqueId,
					ConveyorConnectionType.Out,
					base.WgoData.id,
					this.wgoDataUniqueId
				}));
				conveyorWgoData2.ConveyorComponent.RemoveParentData(base.WgoData);
			}
		}
		this.ConveyorOutWgoDataList.Clear();
		this.connectedOutWgoDataUniqueId.Clear();
		this.ConveyorInWgoDataList.Clear();
		this.connectedInWgoDataUniqueId.Clear();
		return true;
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x000A95BC File Offset: 0x000A77BC
	public override void GetEndElement(ref List<ConveyorComponent> endElements)
	{
		base.CurrentVisitState = VisitState.Visiting;
		if (this.ConveyorOutWgoDataList.Count == 0)
		{
			endElements.Add(this);
		}
		foreach (ConveyorWgoData conveyorWgoData in this.ConveyorOutWgoDataList)
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

	// Token: 0x06002404 RID: 9220 RVA: 0x000A965C File Offset: 0x000A785C
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
		foreach (ConveyorWgoData conveyorWgoData in this.ConveyorInWgoDataList)
		{
			conveyorWgoData.ConveyorComponent.HandleCycleDependency(cyrcleComponent);
		}
	}

	// Token: 0x06002405 RID: 9221 RVA: 0x000A96DC File Offset: 0x000A78DC
	private void GetItemFromConveyor(ConveyorWgoData conveyorCell)
	{
		if (base.WgoData.CraftComponent.Status == CraftComponentStatus.WaitingForOutputDrop || !conveyorCell.ConveyorComponent.CanGiveItem(this))
		{
			return;
		}
		if (base.WgoData.CraftComponent.HasCraftsInQueue && !base.WgoData.CraftComponent.IsStarted)
		{
			List<NeedItemData> requirements = base.WgoData.CraftComponent.CraftElementsQueue[0].Requirements;
			base.WgoData.CraftComponent.UpdateQueueElementsCraftStatus();
			if (base.WgoData.CraftComponent.CraftElementsQueue[0].CraftStatus == CraftStatus.NotEnoughResources)
			{
				foreach (NeedItemData needItemData in requirements)
				{
					if (!base.WgoData.CraftableObjectCraftInventory.Data.HasItemQuantityInInventory(needItemData.id, needItemData.GetCount(base.WgoData)) && conveyorCell.Inventory.Data.HasItemQuantityInInventory(needItemData.id, 1))
					{
						conveyorCell.ConveyorComponent.OutItem = new ConveyorMovableItemData(needItemData.id, conveyorCell.MainWgoPartData.rotationIndex, false);
						base.WgoData.CraftableObjectCraftInventory.AddItemsToInventory(conveyorCell.Inventory.RemoveItemById(needItemData.id, 1, null, null, false));
						this.wasPerformedItemTransfer = true;
					}
				}
			}
		}
	}

	// Token: 0x06002406 RID: 9222 RVA: 0x000A9854 File Offset: 0x000A7A54
	private void PutItemToConveyor(ConveyorWgoData conveyorCell)
	{
		if (base.WgoData.CraftComponent.Status == CraftComponentStatus.WaitingForOutputDrop)
		{
			if (conveyorCell.Inventory.Data.Inventory.Count == 0)
			{
				Direction direction;
				this.TryGetConnectorDirection(conveyorCell.UniqueId, out direction);
				conveyorCell.ConveyorComponent.InItem = new ConveyorMovableItemData(base.WgoData.CraftableObjectCraftInventory.Data.Inventory[0].id, direction, false);
				conveyorCell.Inventory.AddItemsToInventory(base.WgoData.CraftableObjectCraftInventory.RemoveItemById(base.WgoData.CraftableObjectCraftInventory.Data.Inventory[0].id, 1, null, null, false));
				this.wasPerformedItemTransfer = true;
			}
			if (base.WgoData.Definition.isAutoCrafter && base.WgoData.CraftableObjectCraftInventory.Data.Inventory.Count == 0)
			{
				base.WgoData.CraftComponent.Status = CraftComponentStatus.ReadyToStartCraft;
			}
		}
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x000A995C File Offset: 0x000A7B5C
	public void DoJobIn()
	{
		if (base.WgoData.CraftComponent.IsDestroyingCraftActive)
		{
			return;
		}
		foreach (ConveyorWgoData conveyorWgoData in this.ConveyorInWgoDataList)
		{
			this.GetItemFromConveyor(conveyorWgoData);
		}
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x000A99C4 File Offset: 0x000A7BC4
	public void DoJobOut()
	{
		if (base.WgoData.CraftComponent.IsDestroyingCraftActive)
		{
			return;
		}
		foreach (ConveyorWgoData conveyorWgoData in this.ConveyorOutWgoDataList)
		{
			this.PutItemToConveyor(conveyorWgoData);
		}
		if (this.wasPerformedItemTransfer)
		{
			if (base.WgoData.CraftableObjectCraftInventory.Data.Inventory.Count == 0)
			{
				this.TryFinishNormalCraft();
				this.TryStartOrFinishAutoCraft();
				return;
			}
		}
		else
		{
			if (base.WgoData.CraftComponent.Status == CraftComponentStatus.WaitingForOutputDrop)
			{
				if (base.WgoData.CraftComponent.CurrentCraftElement != null)
				{
					base.WgoData.CraftComponent.CurrentCraftElement.CraftStatus = CraftStatus.NotEnoughSpaceInWgo;
				}
				base.WgoData.UpdateAttachedWgoViewWidgets();
				return;
			}
			this.TryStartOrFinishAutoCraft();
		}
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x000A9AAC File Offset: 0x000A7CAC
	public void DoJobOutForce(CraftComponentStatus craftComponentStatus)
	{
		if (craftComponentStatus != CraftComponentStatus.WaitingForOutputDrop)
		{
			return;
		}
		this.DoJobOut();
	}

	// Token: 0x0600240A RID: 9226 RVA: 0x00028294 File Offset: 0x00026494
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		return false;
	}

	// Token: 0x0600240B RID: 9227 RVA: 0x000A9ABC File Offset: 0x000A7CBC
	private bool TryGetConnectorDirection(SGuid connectedUniqueId, out Direction direction)
	{
		ConveyorConnectionData conveyorConnectionData = this.OccupiedConnectorsData.Find((ConveyorConnectionData x) => x.connectedUniqueId == connectedUniqueId);
		direction = ((conveyorConnectionData == null) ? Direction.Down : conveyorConnectionData.connectionDirection);
		return conveyorConnectionData != null;
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x000A9B00 File Offset: 0x000A7D00
	private void TryFinishNormalCraft()
	{
		if (base.WgoData.Definition.isAutoCrafter)
		{
			return;
		}
		base.WgoData.CraftComponent.TryFinishCurCraft();
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x000A9B28 File Offset: 0x000A7D28
	private void TryStartOrFinishAutoCraft()
	{
		if (!base.WgoData.Definition.isAutoCrafter)
		{
			return;
		}
		if (!base.WgoData.CraftComponent.HasCraftsInQueue)
		{
			List<CraftDefBase> list;
			if (GameBalance.Me.craftsInCache.TryGetValue(base.WgoData.id, out list))
			{
				base.WgoData.CraftComponent.AddToQueue(new CraftElement(list[0].id, 1, new List<NeedItemData>(list[0].needItems), new CraftParamsData(list[0].id, base.WgoData, CraftParamsData.CraftParamsType.Common, -1)), false, -1);
				return;
			}
		}
		else if (base.WgoData.CraftComponent.Status == CraftComponentStatus.ReadyToFinishAutoCraft)
		{
			bool worker = base.WgoData.Worker != null;
			bool flag = false;
			if (!worker)
			{
				flag = true;
				base.WgoData.TrySetWorker(MainGame.PlayerController, null);
			}
			base.WgoData.CraftComponent.ContinueAutoCraft();
			if (flag)
			{
				base.WgoData.ClearWorker();
			}
			List<CraftDefBase> list2;
			if (GameBalance.Me.craftsInCache.TryGetValue(base.WgoData.id, out list2))
			{
				base.WgoData.CraftComponent.AddToQueue(new CraftElement(list2[0].id, 1, new List<NeedItemData>(list2[0].needItems), new CraftParamsData(list2[0].id, base.WgoData, CraftParamsData.CraftParamsType.Common, -1)), false, -1);
			}
			base.WgoData.CraftComponent.Status = CraftComponentStatus.WaitingForOutputDrop;
		}
	}

	// Token: 0x04002001 RID: 8193
	public List<SGuid> connectedInWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04002002 RID: 8194
	public List<SGuid> connectedOutWgoDataUniqueId = new List<SGuid>();

	// Token: 0x04002003 RID: 8195
	[SerializeField]
	private List<ConveyorConnectionData> occupiedConnectorsData = new List<ConveyorConnectionData>();

	// Token: 0x04002004 RID: 8196
	[NonSerialized]
	private List<ConveyorWgoData> conveyorInWgoDataList = new List<ConveyorWgoData>();

	// Token: 0x04002005 RID: 8197
	[NonSerialized]
	private List<ConveyorWgoData> conveyorOutWgoDataList = new List<ConveyorWgoData>();
}
