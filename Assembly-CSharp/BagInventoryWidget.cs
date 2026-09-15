using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008A0 RID: 2208
public class BagInventoryWidget : InventoryWidget
{
	// Token: 0x1700087C RID: 2172
	// (get) Token: 0x06003907 RID: 14599 RVA: 0x00112471 File Offset: 0x00110671
	private BagInventoryWidgetData CastedData
	{
		get
		{
			return this.data as BagInventoryWidgetData;
		}
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x00112480 File Offset: 0x00110680
	protected override void SubscribeToInventoryEvents()
	{
		if (!this.subscribedInventoryEvents)
		{
			this.CastedData.ParentInventory.OnItemsAdd += base.OnItemsAdded;
			this.CastedData.ParentInventory.OnItemsRemove += base.OnItemsRemoved;
			this.data.Inventory.OnItemsRemove += base.OnItemsRemoved;
			this.data.Inventory.OnItemsAdd += base.OnItemsAdded;
			this.subscribedInventoryEvents = true;
		}
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x0011250C File Offset: 0x0011070C
	protected override void UnsubscribeFromInventoryEvents()
	{
		if (this.CastedData == null || this.CastedData.ParentInventory == null || !this.subscribedInventoryEvents)
		{
			return;
		}
		this.CastedData.ParentInventory.OnItemsAdd -= base.OnItemsAdded;
		this.CastedData.ParentInventory.OnItemsRemove -= base.OnItemsRemoved;
		this.data.Inventory.OnItemsAdd -= base.OnItemsAdded;
		this.data.Inventory.OnItemsRemove -= base.OnItemsRemoved;
		this.subscribedInventoryEvents = false;
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x001125B0 File Offset: 0x001107B0
	public override void Redraw()
	{
		if (this.subscribedInventoryEvents)
		{
			this.UnsubscribeFromInventoryEvents();
		}
		this.grid.constraintCount = this.data.Inventory.Data.Definition.bagSizeX;
		base.Redraw();
		if (!this.subscribedInventoryEvents)
		{
			this.SubscribeToInventoryEvents();
		}
		this.noiseLeft.transform.SetAsFirstSibling();
		this.noiseRight.transform.SetAsFirstSibling();
	}

	// Token: 0x0600390B RID: 14603 RVA: 0x00112624 File Offset: 0x00110824
	public override void UpdateItemRelatedWidgetStateForWidget()
	{
		base.UpdateItemRelatedWidgetStateForWidget();
		if (this.data.ItemRelatedWidgetState == ItemRelatedWidgetState.Default || this.data.ItemRelatedWidgetState == ItemRelatedWidgetState.Selected)
		{
			this.noiseLeft.color = this.noiseColorActive;
			this.noiseRight.color = this.noiseColorActive;
			return;
		}
		this.noiseLeft.color = this.noiseColorInactive;
		this.noiseRight.color = this.noiseColorInactive;
	}

	// Token: 0x0600390C RID: 14604 RVA: 0x00112698 File Offset: 0x00110898
	[LazyUITest]
	protected override void TestDraw()
	{
		Inventory @default = Inventory.GetDefault();
		Item item = new Item("bag_universal", 1);
		item.AddItemToInventory(new Item("axe_0", 1), false);
		item.AddItemToInventory(new Item("shovel_0", 1), false);
		item.AddItemToInventory(new Item("leaf", 12), false);
		item.AddItemToInventory(new Item("carrot", 5), false);
		Item item2 = new Item("bag_tools", 1);
		item2.AddItemToInventory(new Item("axe_1", 1), false);
		item2.AddItemToInventory(new Item("shovel_1", 1), false);
		@default.AddItemToInventory(item, null, false);
		@default.AddItemToInventory(item2, null, false);
		this.Draw(new InventoryWidgetDataBase(@default, null, null, null, null, null, null, null, ItemRelatedWidgetState.Default));
	}

	// Token: 0x04002D55 RID: 11605
	[SerializeField]
	private Image noiseLeft;

	// Token: 0x04002D56 RID: 11606
	[SerializeField]
	private Image noiseRight;

	// Token: 0x04002D57 RID: 11607
	[SerializeField]
	private Color noiseColorActive;

	// Token: 0x04002D58 RID: 11608
	[SerializeField]
	private Color noiseColorInactive;
}
