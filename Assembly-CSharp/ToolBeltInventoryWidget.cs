using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008B7 RID: 2231
public class ToolBeltInventoryWidget : InventoryWidgetBase<ToolBeltInventoryWidgetData>
{
	// Token: 0x06003A04 RID: 14852 RVA: 0x00115BEC File Offset: 0x00113DEC
	public override void Init()
	{
		base.Init();
		this.fixedTypeItemCells = base.GetComponentsInChildren<UIFixedTypeItemCell>(true).ToList<UIFixedTypeItemCell>();
		foreach (UIFixedTypeItemCell uifixedTypeItemCell in this.fixedTypeItemCells)
		{
			if (!this.itemCellsByType.TryAdd(uifixedTypeItemCell.ItemType, uifixedTypeItemCell) && uifixedTypeItemCell.ItemType != ItemType.None)
			{
				Debug.LogError(string.Format("ToolBeltInventoryWidget: itemType {0} was already added", uifixedTypeItemCell.ItemType));
			}
		}
	}

	// Token: 0x06003A05 RID: 14853 RVA: 0x00115C88 File Offset: 0x00113E88
	protected override void ClearCallbacks()
	{
		this.UnsubscribeFromInventoryEvents();
		base.ClearCallbacks();
		foreach (UIFixedTypeItemCell uifixedTypeItemCell in this.fixedTypeItemCells)
		{
			uifixedTypeItemCell.UIItemCell.ClearCallbacks();
		}
	}

	// Token: 0x06003A06 RID: 14854 RVA: 0x00115CEC File Offset: 0x00113EEC
	public override void Redraw()
	{
		base.Redraw();
		this.SubscribeToInventoryEvents();
		foreach (UIFixedTypeItemCell uifixedTypeItemCell in this.fixedTypeItemCells)
		{
			uifixedTypeItemCell.DrawEmpty();
			uifixedTypeItemCell.UIItemCell.OnItemCellOver = this.onItemCellOver;
			uifixedTypeItemCell.UIItemCell.OnItemCellOut = this.onItemCellOut;
			uifixedTypeItemCell.UIItemCell.OnItemCellPress = this.onItemCellPress;
			uifixedTypeItemCell.UIItemCell.OnItemCellPress2 = this.onItemCellPress2;
		}
		foreach (Item item in this.data.Inventory.Data.Inventory)
		{
			UIFixedTypeItemCell uifixedTypeItemCell2;
			if (this.itemCellsByType.TryGetValue(item.Definition.type, out uifixedTypeItemCell2))
			{
				bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(item);
				uifixedTypeItemCell2.Draw(item, this.data.ItemRelatedWidgetState, flag);
			}
		}
		Item itemByType = this.data.Inventory.GetItemByType(ItemType.BodyArmor);
		Item itemByType2 = this.data.Inventory.GetItemByType(ItemType.Sword);
		Item itemByType3 = this.data.Inventory.GetItemByType(ItemType.Bow);
		int num = (itemByType.IsEmpty ? 0 : itemByType.Definition.quality);
		int num2;
		if (!itemByType2.IsEmpty)
		{
			LazyExpression damage = itemByType2.Definition.damage;
			PlayerController playerController = MainGame.PlayerController;
			num2 = damage.EvaluateInt((playerController != null) ? playerController.PhysicalBody : null);
		}
		else
		{
			num2 = 0;
		}
		int num3 = num2;
		int num4;
		if (!itemByType3.IsEmpty)
		{
			LazyExpression damage2 = itemByType3.Definition.damage;
			PlayerController playerController2 = MainGame.PlayerController;
			num4 = damage2.EvaluateInt((playerController2 != null) ? playerController2.PhysicalBody : null);
		}
		else
		{
			num4 = 0;
		}
		int num5 = num4;
		this.armorLabel.text = string.Format("{0}{1}", "equip_icon_armor".FontIcon(), num);
		this.swordLabel.text = string.Format("{0}{1}", "equip_icon_sword".FontIcon(), num3);
		this.bowLabel.text = string.Format("{0}{1}", "equip_icon_arrow".FontIcon(), num5);
	}

	// Token: 0x06003A07 RID: 14855 RVA: 0x00115F4C File Offset: 0x0011414C
	public override void UpdateItemRelatedWidgetStateForCells()
	{
		for (int i = 0; i < this.fixedTypeItemCells.Count; i++)
		{
			UIFixedTypeItemCell uifixedTypeItemCell = this.fixedTypeItemCells[i];
			bool flag = this.data.CustomItemsAvailableCondition == null || this.data.CustomItemsAvailableCondition(uifixedTypeItemCell.UIItemCell.DisplayingItem);
			uifixedTypeItemCell.UIItemCell.SetWidgetState(flag ? this.data.ItemRelatedWidgetState : ItemRelatedWidgetState.Disabled);
		}
	}

	// Token: 0x06003A08 RID: 14856 RVA: 0x00115FC4 File Offset: 0x001141C4
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new InventoryWidgetDataBase(MainGame.PlayerData.toolBeltInventory, null, null, new Action<UIItemCell>(new PlayerInventoryUIItemOpHandler(MainGame.PlayerData).TryUnEquipItem), null, null, null, null, ItemRelatedWidgetState.Default));
	}

	// Token: 0x04002DC1 RID: 11713
	[SerializeField]
	private TextMeshProUGUI armorLabel;

	// Token: 0x04002DC2 RID: 11714
	[SerializeField]
	private TextMeshProUGUI swordLabel;

	// Token: 0x04002DC3 RID: 11715
	[SerializeField]
	private TextMeshProUGUI bowLabel;

	// Token: 0x04002DC4 RID: 11716
	private List<UIFixedTypeItemCell> fixedTypeItemCells = new List<UIFixedTypeItemCell>();

	// Token: 0x04002DC5 RID: 11717
	private Dictionary<ItemType, UIFixedTypeItemCell> itemCellsByType = new Dictionary<ItemType, UIFixedTypeItemCell>();
}
