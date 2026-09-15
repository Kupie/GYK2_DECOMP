using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x020008AE RID: 2222
public abstract class InventoryWidgetBase<T> : LazyWidget<InventoryWidgetDataBase> where T : InventoryWidgetDataBase
{
	// Token: 0x06003996 RID: 14742 RVA: 0x00114B5C File Offset: 0x00112D5C
	public override void Redraw()
	{
		base.Redraw();
		this.onItemCellOver = this.data.OnItemCellOver;
		this.onItemCellOut = this.data.OnItemCellOut;
		this.onItemCellPress = this.data.OnItemCellPress;
		this.onItemCellPress2 = this.data.OnItemCellPress2;
		this.onItemCellDown = this.data.OnItemCellDown;
	}

	// Token: 0x06003997 RID: 14743 RVA: 0x00114BC4 File Offset: 0x00112DC4
	public override void Hide()
	{
		base.Hide();
		this.ClearCallbacks();
	}

	// Token: 0x06003998 RID: 14744 RVA: 0x00114BD2 File Offset: 0x00112DD2
	public void UpdateItemRelatedWidgetStateForEveryThing()
	{
		this.UpdateItemRelatedWidgetStateForCells();
		this.UpdateItemRelatedWidgetStateForWidget();
	}

	// Token: 0x06003999 RID: 14745
	public abstract void UpdateItemRelatedWidgetStateForCells();

	// Token: 0x0600399A RID: 14746 RVA: 0x00002318 File Offset: 0x00000518
	public virtual void UpdateItemRelatedWidgetStateForWidget()
	{
	}

	// Token: 0x0600399B RID: 14747 RVA: 0x00114BE0 File Offset: 0x00112DE0
	protected virtual void ClearCallbacks()
	{
		this.onItemCellOver = null;
		this.onItemCellOut = null;
		this.onItemCellPress = null;
		this.onItemCellPress2 = null;
		this.onItemCellDown = null;
	}

	// Token: 0x0600399C RID: 14748 RVA: 0x00114C08 File Offset: 0x00112E08
	protected virtual void SubscribeToInventoryEvents()
	{
		if (!this.subscribedInventoryEvents)
		{
			this.data.Inventory.OnItemsAdd += this.OnItemsAdded;
			this.data.Inventory.OnItemsRemove += this.OnItemsRemoved;
			this.subscribedInventoryEvents = true;
		}
	}

	// Token: 0x0600399D RID: 14749 RVA: 0x00114C5C File Offset: 0x00112E5C
	protected virtual void UnsubscribeFromInventoryEvents()
	{
		if (this.data == null || this.data.Inventory == null || !this.subscribedInventoryEvents)
		{
			return;
		}
		this.data.Inventory.OnItemsAdd -= this.OnItemsAdded;
		this.data.Inventory.OnItemsRemove -= this.OnItemsRemoved;
		this.subscribedInventoryEvents = false;
	}

	// Token: 0x0600399E RID: 14750 RVA: 0x0010A599 File Offset: 0x00108799
	protected void OnItemsAdded(List<Item> items)
	{
		this.Redraw();
	}

	// Token: 0x0600399F RID: 14751 RVA: 0x0010A599 File Offset: 0x00108799
	protected void OnItemsRemoved(List<Item> items)
	{
		this.Redraw();
	}

	// Token: 0x04002D91 RID: 11665
	protected Action<UIItemCell> onItemCellOver;

	// Token: 0x04002D92 RID: 11666
	protected Action<UIItemCell> onItemCellOut;

	// Token: 0x04002D93 RID: 11667
	protected Action<UIItemCell> onItemCellPress;

	// Token: 0x04002D94 RID: 11668
	protected Action<UIItemCell> onItemCellPress2;

	// Token: 0x04002D95 RID: 11669
	protected Action<UIItemCell> onItemCellDown;

	// Token: 0x04002D96 RID: 11670
	protected bool subscribedInventoryEvents;

	// Token: 0x020008AF RID: 2223
	// (Invoke) Token: 0x060039A2 RID: 14754
	public delegate int ItemPriceDelegate(Item item, int countModificator);
}
