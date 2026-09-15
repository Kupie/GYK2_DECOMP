using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A55 RID: 2645
public class UIConveyorWineChestWindow : UIBaseChestWindow
{
	// Token: 0x17000AD2 RID: 2770
	// (get) Token: 0x06004747 RID: 18247 RVA: 0x001516D9 File Offset: 0x0014F8D9
	public bool IsItemSelectionModActive
	{
		get
		{
			return this.isItemSelectionModActive;
		}
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x001516E1 File Offset: 0x0014F8E1
	public override void Redraw()
	{
		base.Redraw();
		this.RedrawChestSlot(null);
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x001516F0 File Offset: 0x0014F8F0
	public override void Close()
	{
		base.Close();
		this.data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsAdd -= this.RedrawChestSlot;
		this.data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsRemove -= this.RedrawChestSlot;
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x00151750 File Offset: 0x0014F950
	protected override void SetData(UIBaseChestWindowData data)
	{
		base.SetData(data);
		data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsAdd += this.RedrawChestSlot;
		data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsRemove += this.RedrawChestSlot;
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x001517A8 File Offset: 0x0014F9A8
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuffBasic();
		if (LazyInput.IsGamepadActive)
		{
			GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
			if (this.isItemSelectionModActive)
			{
				base.GamepadNavigationController.ReinitItems(true, null, null);
				return;
			}
			if (focusedItem != null)
			{
				base.GamepadNavigationController.ReinitItems(false, null, null);
				base.GamepadNavigationController.SetFocusedItem(focusedItem);
				return;
			}
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x00151818 File Offset: 0x0014FA18
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		UIItemCell uiitemCell;
		if (gamepadNavigationItem != null && gamepadNavigationItem.TryGetComponent<UIItemCell>(out uiitemCell))
		{
			if (uiitemCell.GetComponentInParentExcludeCurrent(true) != null)
			{
				if (!this.isItemSelectionModActive)
				{
					if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.OnItemCellPress != null)
					{
						list.Add(LazyGameKeyTip.Select(true, true, true));
					}
					if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress2 != null)
					{
						list.Add(new LazyGameKeyTip(GameKey.ItemMove, "tip_item_action", true, true, true));
					}
				}
			}
			else
			{
				if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.OnItemCellPress != null)
				{
					list.Add(LazyGameKeyTip.Select(true, true, true));
				}
				if (!this.isItemSelectionModActive)
				{
					if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress2 != null)
					{
						list.Add(new LazyGameKeyTip(GameKey.ItemMove, "tip_item_action", true, true, true));
					}
					base.TryAddMoveAllSimilarItemsTip(list);
				}
			}
		}
		list.Add(LazyGameKeyTip.Back(true, true, true));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x0600474D RID: 18253 RVA: 0x0015196D File Offset: 0x0014FB6D
	protected override void InitCloseButton(LazyButton button)
	{
		button.onClick.AddListener(new UnityAction(this.CloseOrDeactivateItemSettingMode));
	}

	// Token: 0x0600474E RID: 18254 RVA: 0x00151986 File Offset: 0x0014FB86
	protected override bool OnPressedBack()
	{
		this.CloseOrDeactivateItemSettingMode();
		return true;
	}

	// Token: 0x0600474F RID: 18255 RVA: 0x0015198F File Offset: 0x0014FB8F
	protected override void OnAllToChestPressed()
	{
		if (this.isItemSelectionModActive)
		{
			return;
		}
		Action onMoveAllSimilarItemFromPlayerToChest = this.data.OnMoveAllSimilarItemFromPlayerToChest;
		if (onMoveAllSimilarItemFromPlayerToChest == null)
		{
			return;
		}
		onMoveAllSimilarItemFromPlayerToChest();
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x001519B0 File Offset: 0x0014FBB0
	private void RedrawChestSlot(List<Item> items = null)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		UIConveyorChestSlot uiconveyorChestSlot = this.GetChestSlot();
		if (conveyorWgoData == null || uiconveyorChestSlot == null)
		{
			return;
		}
		ConveyorChestComponent conveyorChestComponent = conveyorWgoData.ConveyorComponent as ConveyorChestComponent;
		if (conveyorChestComponent == null)
		{
			ConveyorChestOutComponent conveyorChestOutComponent = conveyorWgoData.ConveyorComponent as ConveyorChestOutComponent;
			if (conveyorChestOutComponent != null)
			{
				conveyorChestOutComponent.UpdateSlotsData();
				ConveyorChestSlotData conveyorChestSlotData = conveyorChestOutComponent.SlotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == Direction.Right);
				if (conveyorChestSlotData == null)
				{
					uiconveyorChestSlot.gameObject.SetActive(false);
					return;
				}
				uiconveyorChestSlot.gameObject.SetActive(true);
				uiconveyorChestSlot.Draw(conveyorChestSlotData, conveyorChestOutComponent, new Action<UIConveyorChestSlot>(this.SetChestItemSettingMode), new Action<UIConveyorChestSlot>(this.RemoveItemFromConveyorSlot));
			}
			return;
		}
		ConveyorChestSlotData conveyorChestSlotData2 = conveyorChestComponent.SlotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == Direction.Right);
		if (conveyorChestSlotData2 == null)
		{
			uiconveyorChestSlot.gameObject.SetActive(false);
			return;
		}
		uiconveyorChestSlot.gameObject.SetActive(true);
		uiconveyorChestSlot.Draw(conveyorChestSlotData2, conveyorChestComponent, new Action<UIConveyorChestSlot>(this.SetChestItemSettingMode), new Action<UIConveyorChestSlot>(this.RemoveItemFromConveyorSlot));
	}

	// Token: 0x06004751 RID: 18257 RVA: 0x00151ADD File Offset: 0x0014FCDD
	private UIConveyorChestSlot GetChestSlot()
	{
		if (this.chestSlot == null)
		{
			this.chestSlot = base.GetComponentInChildren<UIConveyorChestSlot>(true);
		}
		return this.chestSlot;
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x00151B00 File Offset: 0x0014FD00
	private void SetItemSlot(UIItemCell cell, ConveyorChestSlotData slotData)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
		if (!(conveyorComponent is ConveyorChestComponent) && !(conveyorComponent is ConveyorChestOutComponent))
		{
			return;
		}
		slotData.SetItemSlotId(cell.DisplayingItem.id);
		this.SetChestItemSettingMode(false, null);
		this.RedrawChestSlot(null);
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x00151B5C File Offset: 0x0014FD5C
	private void RemoveItemFromConveyorSlot(UIConveyorChestSlot conveyorChestSlot)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		ConveyorComponent conveyorComponent = conveyorWgoData.ConveyorComponent;
		if (!(conveyorComponent is ConveyorChestComponent) && !(conveyorComponent is ConveyorChestOutComponent))
		{
			return;
		}
		conveyorChestSlot.SlotData.SetItemSlotId(string.Empty);
		this.RedrawChestSlot(null);
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x00151BAD File Offset: 0x0014FDAD
	private void SetChestItemSettingMode(UIConveyorChestSlot conveyorChestSlot)
	{
		if (this.isItemSelectionModActive)
		{
			return;
		}
		this.selectedSlot = conveyorChestSlot;
		this.SetChestItemSettingMode(true, conveyorChestSlot.SlotData);
		conveyorChestSlot.SetState(UIConveyorChestSlot.State.OutDuringSelection);
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x00151BD4 File Offset: 0x0014FDD4
	private void SetChestItemSettingMode(bool isActive, ConveyorChestSlotData slotData = null)
	{
		if (isActive && this.isItemSelectionModActive)
		{
			return;
		}
		this.isItemSelectionModActive = isActive;
		foreach (GameObject gameObject in this.whenSelectionEnabledObjects)
		{
			gameObject.SetActive(isActive);
		}
		if (isActive)
		{
			this.previousChestOnPress = this.data.SecondMultiInventoryData.inventoriesData[0].OnItemCellPress;
			this.previousChestOnPress2 = this.data.SecondMultiInventoryData.inventoriesData[0].OnItemCellPress2;
			Action<UIItemCell> <>9__0;
			foreach (InventoryWidgetDataBase inventoryWidgetDataBase in this.data.SecondMultiInventoryData.inventoriesData)
			{
				Action<UIItemCell> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(UIItemCell x)
					{
						this.SetItemSlot(x, slotData);
					});
				}
				inventoryWidgetDataBase.SetCustomOnCellPressedAction(action);
				inventoryWidgetDataBase.SetCustomOnCellPressed2Action(delegate(UIItemCell x)
				{
				});
			}
			this.leftMultiInventoryWidget.Redraw();
			this.rightMultiInventoryWidget.Redraw();
			using (List<InventoryWidget>.Enumerator enumerator3 = this.leftMultiInventoryWidget.DrawnInventories.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					InventoryWidget inventoryWidget = enumerator3.Current;
					foreach (UIItemCell uiitemCell in inventoryWidget.Cells)
					{
						uiitemCell.GamepadNavigationItem.Active = false;
					}
				}
				goto IL_027C;
			}
		}
		foreach (InventoryWidgetDataBase inventoryWidgetDataBase2 in this.data.SecondMultiInventoryData.inventoriesData)
		{
			inventoryWidgetDataBase2.SetCustomOnCellPressedAction(this.previousChestOnPress);
			inventoryWidgetDataBase2.SetCustomOnCellPressed2Action(this.previousChestOnPress2);
		}
		this.leftMultiInventoryWidget.Redraw();
		this.rightMultiInventoryWidget.Redraw();
		foreach (InventoryWidget inventoryWidget2 in this.leftMultiInventoryWidget.DrawnInventories)
		{
			foreach (UIItemCell uiitemCell2 in inventoryWidget2.Cells)
			{
				uiitemCell2.GamepadNavigationItem.Active = true;
			}
		}
		IL_027C:
		this.RedrawChestSlot(null);
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x06004756 RID: 18262 RVA: 0x00151EC4 File Offset: 0x001500C4
	private void CloseOrDeactivateItemSettingMode()
	{
		if (this.isItemSelectionModActive)
		{
			if (this.selectedSlot != null)
			{
				this.selectedSlot.SetState(this.selectedSlot.PreviousState);
			}
			this.SetChestItemSettingMode(false, null);
			return;
		}
		this.Close();
	}

	// Token: 0x04003799 RID: 14233
	[SerializeField]
	private UIConveyorChestSlot chestSlot;

	// Token: 0x0400379A RID: 14234
	[SerializeField]
	private List<GameObject> whenSelectionEnabledObjects;

	// Token: 0x0400379B RID: 14235
	private Action<UIItemCell> previousChestOnPress;

	// Token: 0x0400379C RID: 14236
	private Action<UIItemCell> previousChestOnPress2;

	// Token: 0x0400379D RID: 14237
	private bool isItemSelectionModActive;

	// Token: 0x0400379E RID: 14238
	private UIConveyorChestSlot selectedSlot;
}
