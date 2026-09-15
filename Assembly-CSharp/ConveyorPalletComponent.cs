using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000578 RID: 1400
[Serializable]
public class ConveyorPalletComponent : ConveyorComponent
{
	// Token: 0x060023CD RID: 9165 RVA: 0x000A7B04 File Offset: 0x000A5D04
	public ConveyorPalletComponent(ConveyorWgoData conveyorWgoData)
		: base(conveyorWgoData)
	{
	}

	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x060023CE RID: 9166 RVA: 0x000A7B10 File Offset: 0x000A5D10
	public ConveyorWgoData ConnectedWgoData
	{
		get
		{
			if (this.connectedWgoData != null)
			{
				return this.connectedWgoData;
			}
			if (this.connectedWgoDataUniqueId == null)
			{
				return null;
			}
			this.connectedWgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.connectedWgoDataUniqueId) as ConveyorWgoData;
			return this.connectedWgoData;
		}
	}

	// Token: 0x060023CF RID: 9167 RVA: 0x000A6C90 File Offset: 0x000A4E90
	public override void DoJob(ConveyorComponent visitor = null)
	{
		base.CurrentVisitState = VisitState.Visited;
	}

	// Token: 0x060023D0 RID: 9168 RVA: 0x000A7B68 File Offset: 0x000A5D68
	public override bool Connect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType, Direction direction = Direction.None)
	{
		if (this.connectedWgoDataUniqueId != null && this.connectedWgoDataUniqueId == conveyorWgoData.UniqueId)
		{
			Debug.LogWarning(string.Format("Trying to connect  {0} that is already connected", conveyorWgoData.UniqueId));
			return false;
		}
		if (!(conveyorWgoData.ConveyorComponent is ConveyorCellComponent))
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
		this.connectedWgoDataUniqueId = conveyorWgoData.UniqueId;
		conveyorWgoData.ConveyorComponent.AddParentData(base.WgoData, direction);
		return true;
	}

	// Token: 0x060023D1 RID: 9169 RVA: 0x000A7C1C File Offset: 0x000A5E1C
	public override bool Disconnect(ConveyorWgoData conveyorWgoData, ConveyorConnectionType connectionType)
	{
		if (this.connectedWgoDataUniqueId == null)
		{
			Debug.LogWarning(string.Format("Trying to disconnect  {0}. But nothing is connected", conveyorWgoData.UniqueId));
			return false;
		}
		if (this.connectedWgoDataUniqueId != null && this.connectedWgoDataUniqueId != conveyorWgoData.UniqueId)
		{
			Debug.LogWarning(string.Format("Trying to disconnect  {0} that is not connected", conveyorWgoData.UniqueId));
			return false;
		}
		Debug.Log(string.Format("Disconnected {0} with guid {1} from {2} with guid {3}", new object[]
		{
			conveyorWgoData.id,
			conveyorWgoData.UniqueId,
			base.WgoData.id,
			this.wgoDataUniqueId
		}));
		this.connectedWgoDataUniqueId = null;
		this.connectedWgoData = null;
		conveyorWgoData.ConveyorComponent.RemoveParentData(base.WgoData);
		return true;
	}

	// Token: 0x060023D2 RID: 9170 RVA: 0x000A7CE4 File Offset: 0x000A5EE4
	public override bool DisconnectChilds()
	{
		if (this.connectedWgoDataUniqueId != null)
		{
			if (this.ConnectedWgoData != null)
			{
				Debug.Log(string.Format("Disconnected {0} with guid {1} from {2} with guid {3}", new object[]
				{
					this.ConnectedWgoData.id,
					this.connectedWgoDataUniqueId,
					base.WgoData.id,
					this.wgoDataUniqueId
				}));
				this.ConnectedWgoData.ConveyorComponent.RemoveParentData(base.WgoData);
			}
			this.connectedWgoDataUniqueId = null;
			this.connectedWgoData = null;
			return true;
		}
		return false;
	}

	// Token: 0x060023D3 RID: 9171 RVA: 0x000A7D71 File Offset: 0x000A5F71
	public override bool Contains(ConveyorComponent conveyorComponent)
	{
		return this.ConnectedWgoData == conveyorComponent.WgoData;
	}

	// Token: 0x060023D4 RID: 9172 RVA: 0x000A7D84 File Offset: 0x000A5F84
	public override void GetEndElement(ref List<ConveyorComponent> endElements)
	{
		base.CurrentVisitState = VisitState.Visiting;
		if (this.ConnectedWgoData == null)
		{
			endElements.Add(this);
		}
		else
		{
			ConveyorComponent conveyorComponent = this.ConnectedWgoData.ConveyorComponent;
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

	// Token: 0x060023D5 RID: 9173 RVA: 0x000A7DE9 File Offset: 0x000A5FE9
	public override bool CanGiveItem(ConveyorComponent conveyorComponent)
	{
		return base.WgoData.Inventory.Data.Inventory.Count > 0;
	}

	// Token: 0x04001FEC RID: 8172
	[SerializeField]
	private SGuid connectedWgoDataUniqueId;

	// Token: 0x04001FED RID: 8173
	private ConveyorWgoData connectedWgoData;
}
