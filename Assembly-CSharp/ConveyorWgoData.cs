using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057D RID: 1405
[Serializable]
public class ConveyorWgoData : WgoData
{
	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x060023EE RID: 9198 RVA: 0x000A8CEC File Offset: 0x000A6EEC
	public List<SGuid> HardConnectedWGOs
	{
		get
		{
			if (this.hardConnectedWGOs == null)
			{
				this.hardConnectedWGOs = new List<SGuid>();
			}
			return this.hardConnectedWGOs;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x060023EF RID: 9199 RVA: 0x000A8D07 File Offset: 0x000A6F07
	public ConveyorComponent ConveyorComponent
	{
		get
		{
			return this.conveyorComponent;
		}
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000A8D0F File Offset: 0x000A6F0F
	public ConveyorWgoData(ConveyorElementType conveyorElementType, string id, Vector3 position, string worldId)
		: base(id, position, worldId)
	{
		this.TryCreateConveyorComponent(conveyorElementType);
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x000A8D2D File Offset: 0x000A6F2D
	public override void PrepareForGame()
	{
		base.PrepareForGame();
		this.TryCreateConveyorComponent(base.Definition.conveyorType);
		this.conveyorComponent.WgoData = this;
		this.conveyorComponent.Init();
	}

	// Token: 0x060023F2 RID: 9202 RVA: 0x000A8D5D File Offset: 0x000A6F5D
	public override void DeInit()
	{
		base.DeInit();
		this.conveyorComponent.DeInit();
	}

	// Token: 0x060023F3 RID: 9203 RVA: 0x000A8D70 File Offset: 0x000A6F70
	public override MultiInventory GetCraftableMultiInventory(bool excludeWorkerInventory = false)
	{
		MultiInventory multiInventory = new MultiInventory();
		if (base.WorldZoneData != null)
		{
			foreach (SGuid sguid in base.WorldZoneData.wgoDataList)
			{
				WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(sguid);
				if (wgoData != null && wgoData.Inventory.Data.HasProperty<FuelContainerSerializedItemProperty>())
				{
					multiInventory.Add(wgoData.Inventory);
					multiInventory.Add(wgoData.CraftableObjectCraftInventory);
				}
			}
			multiInventory.Add(base.Inventory);
			multiInventory.Add(base.CraftInventory);
		}
		else
		{
			multiInventory.Add(new MultiInventory(new List<Inventory> { base.Inventory, base.CraftInventory }));
		}
		return multiInventory;
	}

	// Token: 0x060023F4 RID: 9204 RVA: 0x000A8E58 File Offset: 0x000A7058
	protected override float GetQuality()
	{
		if (base.Definition.conveyorType == ConveyorElementType.PowerSource)
		{
			List<DockPointData> dockPoints = base.MainWgoPartData.GetDockPoints(DockPointData.Availability.OnlyOccupied, DockPointData.Filter.OnlyZombie);
			return (float)((int)base.Definition.quality.EvaluateFloat(this) * dockPoints.Count);
		}
		return base.Definition.quality.EvaluateFloat(this) + (base.Definition.considerInventoryQuality ? base.Inventory.GetTotalQuality() : 0f);
	}

	// Token: 0x060023F5 RID: 9205 RVA: 0x000A8ED0 File Offset: 0x000A70D0
	public void UpdateAttachedWgoViewWidgets()
	{
		Wgo wgoViewGlobal = GameScene.GetWgoViewGlobal(base.UniqueId);
		if (wgoViewGlobal != null)
		{
			wgoViewGlobal.DrawWidgets();
		}
	}

	// Token: 0x060023F6 RID: 9206 RVA: 0x000A8EF8 File Offset: 0x000A70F8
	private void TryCreateConveyorComponent(ConveyorElementType conveyorElementType)
	{
		if (this.ConveyorComponent != null)
		{
			return;
		}
		switch (conveyorElementType)
		{
		case ConveyorElementType.Cell:
			this.conveyorComponent = new ConveyorCellComponent(this);
			return;
		case ConveyorElementType.Pallet:
			this.conveyorComponent = new ConveyorPalletComponent(this);
			return;
		case ConveyorElementType.Workbench:
			this.conveyorComponent = new ConveyorWorkbenchComponent(this);
			return;
		case ConveyorElementType.Chest:
			this.conveyorComponent = new ConveyorChestComponent(this);
			return;
		case ConveyorElementType.PowerSource:
			this.conveyorComponent = new ConveyorPowerSourceComponent(this);
			return;
		case ConveyorElementType.Splitter:
			this.conveyorComponent = new ConveyorSplitterComponent(this);
			return;
		case ConveyorElementType.UndergroundCell:
			this.conveyorComponent = new ConveyorCellUndergroundComponent(this);
			return;
		case ConveyorElementType.StationCell:
			this.conveyorComponent = new ConveyorCellStationComponent(this);
			return;
		case ConveyorElementType.ChestOut:
			this.conveyorComponent = new ConveyorChestOutComponent(this);
			return;
		default:
			return;
		}
	}

	// Token: 0x060023F7 RID: 9207 RVA: 0x000A8FB0 File Offset: 0x000A71B0
	public override void SetDataFromDefinition()
	{
		base.SetDataFromDefinition();
		ConveyorChestOutComponent conveyorChestOutComponent = this.ConveyorComponent as ConveyorChestOutComponent;
		if (conveyorChestOutComponent != null)
		{
			conveyorChestOutComponent.UpdateSlotsData();
		}
	}

	// Token: 0x04001FFF RID: 8191
	[SerializeField]
	private ConveyorComponent conveyorComponent;

	// Token: 0x04002000 RID: 8192
	[SerializeField]
	private List<SGuid> hardConnectedWGOs = new List<SGuid>();
}
