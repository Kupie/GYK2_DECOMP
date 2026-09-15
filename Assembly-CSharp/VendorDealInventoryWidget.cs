using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008D4 RID: 2260
public class VendorDealInventoryWidget : InventoryWidget
{
	// Token: 0x170008DF RID: 2271
	// (get) Token: 0x06003AEF RID: 15087 RVA: 0x00119763 File Offset: 0x00117963
	public override List<UIItemCell> Cells
	{
		get
		{
			return this.cells;
		}
	}

	// Token: 0x06003AF0 RID: 15088 RVA: 0x0011976C File Offset: 0x0011796C
	public override void Redraw()
	{
		int count = this.data.Inventory.Data.Inventory.Count;
		for (int i = 0; i < this.data.Inventory.Data.Inventory.Count; i++)
		{
			Item item = this.data.Inventory.Data.Inventory[i];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(item);
			if (item.Count >= 1)
			{
				this.cells[i].Draw(item, false, -1, false, 1, !flag, 0, true, true, false, this.data.ItemRelatedWidgetState, false);
			}
			else
			{
				this.cells[i].DrawEmpty(!flag, true, false);
			}
			this.cells[i].OnItemCellOver = this.data.OnItemCellOver;
			this.cells[i].OnItemCellOut = this.data.OnItemCellOut;
			this.cells[i].OnItemCellPress = this.data.OnItemCellPress;
			this.cells[i].OnItemCellPress2 = this.data.OnItemCellPress2;
			this.cells[i].OnItemCellDown = this.data.OnItemCellDown;
			this.cells[i].name = "UIItemCell (" + item.id + ")";
			this.cells[i].gameObject.SetActive(true);
		}
		for (int j = count; j < this.cells.Count; j++)
		{
			this.cells[j].DrawEmpty(true, true, false);
			this.cells[j].OnItemCellOver = null;
			this.cells[j].OnItemCellOut = null;
			this.cells[j].OnItemCellPress = null;
			this.cells[j].OnItemCellPress2 = null;
			this.cells[j].OnItemCellDown = null;
			this.cells[j].name = "UIItemCell (empty)";
			this.cells[j].gameObject.SetActive(true);
		}
		base.OnRedraw();
	}

	// Token: 0x06003AF1 RID: 15089 RVA: 0x001199DC File Offset: 0x00117BDC
	public override void UpdatePrices(InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate priceDelegate, int countModificator)
	{
		if (priceDelegate == null)
		{
			return;
		}
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		foreach (UIItemCell uiitemCell in this.cells)
		{
			Item displayingItem = uiitemCell.DisplayingItem;
			if (displayingItem != null && !displayingItem.IsEmpty)
			{
				if (!dictionary.ContainsKey(displayingItem.id))
				{
					dictionary[displayingItem.id] = 0;
				}
				Dictionary<string, int> dictionary2 = dictionary;
				string text = displayingItem.id;
				int num = dictionary2[text];
				dictionary2[text] = num + 1;
			}
		}
		Dictionary<string, int> dictionary3 = new Dictionary<string, int>();
		foreach (UIItemCell uiitemCell2 in this.cells)
		{
			Item displayingItem2 = uiitemCell2.DisplayingItem;
			if (displayingItem2 == null || displayingItem2.IsEmpty)
			{
				uiitemCell2.ClearPriceLabel();
			}
			else
			{
				if (!dictionary3.ContainsKey(displayingItem2.id))
				{
					dictionary3[displayingItem2.id] = 0;
				}
				int num2 = dictionary3[displayingItem2.id];
				Dictionary<string, int> dictionary4 = dictionary3;
				string text = displayingItem2.id;
				int num = dictionary4[text];
				dictionary4[text] = num + 1;
				int dealCellPriceCountModificator = VendorDealInventoryWidget.GetDealCellPriceCountModificator(countModificator, num2, dictionary[displayingItem2.id]);
				uiitemCell2.UpdatePriceLabel(priceDelegate(displayingItem2, dealCellPriceCountModificator));
			}
		}
	}

	// Token: 0x06003AF2 RID: 15090 RVA: 0x00119B58 File Offset: 0x00117D58
	public static int GetDealCellPriceCountModificator(int baseCountModificator, int indexAmongSameId, int sameIdCount)
	{
		if (sameIdCount < 1)
		{
			sameIdCount = 1;
		}
		if (baseCountModificator > 0)
		{
			return baseCountModificator + sameIdCount - 1 - indexAmongSameId;
		}
		return baseCountModificator - indexAmongSameId;
	}

	// Token: 0x04002E9A RID: 11930
	[SerializeField]
	private List<UIItemCell> cells;
}
