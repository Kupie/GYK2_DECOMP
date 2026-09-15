using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020008A2 RID: 2210
public class BigItemInventoryWidget : InventoryWidget
{
	// Token: 0x06003911 RID: 14609 RVA: 0x0011279E File Offset: 0x0011099E
	public override void Init()
	{
		base.Init();
		if (BigItemInventoryWidget.bigCellPool == null)
		{
			BigItemInventoryWidget.bigCellPool = LazyPooler.CreatePoolById("big_item_cell_pool", this.bigItemCell, 1, Pool.PoolType.ImmediateActivation, false, false, null);
			this.bigItemCell.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x001127D8 File Offset: 0x001109D8
	public override void Redraw()
	{
		base.Redraw();
		List<Item> list = new List<Item>();
		WhiteListFilterSerializedItemProperty whiteListFilterSerializedItemProperty;
		if (!this.data.Inventory.Data.TryGetProperty<WhiteListFilterSerializedItemProperty>(out whiteListFilterSerializedItemProperty))
		{
			Debug.LogError("Trying to draw big item inventory widget for inventory without storable items!!!");
		}
		for (int n = 0; n < whiteListFilterSerializedItemProperty.WhiteList.itemsIds.Count; n++)
		{
			list.Add(new Item(whiteListFilterSerializedItemProperty.WhiteList.itemsIds[n], 0));
		}
		for (int j = 0; j < this.data.Inventory.Data.Inventory.Count; j++)
		{
			Item item = this.data.Inventory.Data.Inventory[j];
			Item item3 = list.Find((Item i) => i.id == item.id);
			if (item3 == null)
			{
				list.Add(new Item(item.id, item.Count));
			}
			else
			{
				item3.Count += item.Count;
			}
		}
		int num = list.Count;
		int count = this.uiItemCells.Count;
		while (num - count > 0)
		{
			UIItemCell newCell = this.GetNewCell();
			this.uiItemCells.Add(newCell);
			newCell.OnItemCellOver = this.onItemCellOver;
			newCell.OnItemCellOut = this.onItemCellOut;
			newCell.OnItemCellPress = this.onItemCellPress;
			newCell.OnItemCellPress2 = this.onItemCellPress2;
			newCell.OnItemCellDown = this.onItemCellDown;
			newCell.gameObject.SetActive(false);
			num--;
		}
		for (int k = 0; k < this.uiItemCells.Count; k++)
		{
			this.uiItemCells[k].gameObject.SetActive(false);
		}
		for (int l = 0; l < num; l++)
		{
			Item item2 = list[l];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(item2);
			if (item2.Count >= 1)
			{
				this.uiItemCells[l].Draw(item2, false, -1, false, 1, !flag, 0, true, true, false, this.data.ItemRelatedWidgetState, false);
			}
			else
			{
				this.uiItemCells[l].DrawEmptyWithState(this.data.ItemRelatedWidgetState, true, false);
				this.uiItemCells[l].SetWidgetState(ItemRelatedWidgetState.Disabled);
			}
			this.uiItemCells[l].OnItemCellOver = this.onItemCellOver;
			this.uiItemCells[l].OnItemCellOut = this.onItemCellOut;
			this.uiItemCells[l].OnItemCellPress = this.onItemCellPress;
			this.uiItemCells[l].OnItemCellPress2 = this.onItemCellPress2;
			this.uiItemCells[l].OnItemCellDown = this.onItemCellDown;
			this.uiItemCells[l].name = "UIItemCell (" + item2.id + ")";
			this.uiItemCells[l].gameObject.SetActive(true);
		}
		for (int m = 0; m < this.uiItemCells.Count; m++)
		{
			if (this.uiItemCells[m].gameObject.activeSelf && this.uiItemCells[m].DisplayingItem != null && !this.uiItemCells[m].DisplayingItem.IsEmpty && this.data.CustomItemsNotShowCondition != null && this.data.CustomItemsNotShowCondition(this.uiItemCells[m].DisplayingItem))
			{
				this.uiItemCells[m].gameObject.SetActive(false);
			}
		}
		this.inventoryHeaderWidget.Draw(base.Data.InventoryHeaderWidgetData);
		base.OnRedraw();
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x00112BE8 File Offset: 0x00110DE8
	protected override UIItemCell GetNewCell()
	{
		UIItemCell orCreateObject = BigItemInventoryWidget.bigCellPool.GetOrCreateObject<UIItemCell>();
		orCreateObject.transform.SetParent(this.grid.transform);
		return orCreateObject;
	}

	// Token: 0x06003914 RID: 14612 RVA: 0x00112C0A File Offset: 0x00110E0A
	protected override void ReleaseCell(UIItemCell cell)
	{
		BigItemInventoryWidget.bigCellPool.ReleaseObject<UIItemCell>(cell);
	}

	// Token: 0x04002D5A RID: 11610
	[SerializeField]
	private UIItemCell bigItemCell;

	// Token: 0x04002D5B RID: 11611
	public static Pool bigCellPool;
}
