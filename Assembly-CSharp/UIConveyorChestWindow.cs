using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A4D RID: 2637
public class UIConveyorChestWindow : UIBaseChestWindow
{
	// Token: 0x17000AD0 RID: 2768
	// (get) Token: 0x06004717 RID: 18199 RVA: 0x00150519 File Offset: 0x0014E719
	public bool IsItemSelectionModActive
	{
		get
		{
			return this.isItemSelectionModActive;
		}
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x00150521 File Offset: 0x0014E721
	public override void Redraw()
	{
		base.Redraw();
		this.RedrawChestSlots(null);
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x00150530 File Offset: 0x0014E730
	public override void Close()
	{
		base.Close();
		this.data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsAdd -= this.RedrawChestSlots;
		this.data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsRemove -= this.RedrawChestSlots;
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x00150590 File Offset: 0x0014E790
	protected override void SetData(UIBaseChestWindowData data)
	{
		base.SetData(data);
		data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsAdd += this.RedrawChestSlots;
		data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsRemove += this.RedrawChestSlots;
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x001505E8 File Offset: 0x0014E7E8
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

	// Token: 0x0600471C RID: 18204 RVA: 0x00150658 File Offset: 0x0014E858
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

	// Token: 0x0600471D RID: 18205 RVA: 0x001507AD File Offset: 0x0014E9AD
	protected override void InitCloseButton(LazyButton button)
	{
		button.onClick.AddListener(new UnityAction(this.CloseOrDeactivateItemSettingMode));
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x001507C6 File Offset: 0x0014E9C6
	protected override bool OnPressedBack()
	{
		this.CloseOrDeactivateItemSettingMode();
		return true;
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x001507CF File Offset: 0x0014E9CF
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

	// Token: 0x06004720 RID: 18208 RVA: 0x001507F0 File Offset: 0x0014E9F0
	private void RedrawChestSlots(List<Item> items = null)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		ConveyorChestComponent conveyorChestComponent = conveyorWgoData.ConveyorComponent as ConveyorChestComponent;
		if (conveyorChestComponent != null)
		{
			using (List<UIConveyorChestSlot>.Enumerator enumerator = this.chestSlots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIConveyorChestSlot chestSlot = enumerator.Current;
					ConveyorChestSlotData conveyorChestSlotData = conveyorChestComponent.SlotsData.Find((ConveyorChestSlotData x) => x.slotPosDirection == chestSlot.ChestPosDirection);
					if (conveyorChestSlotData != null)
					{
						chestSlot.gameObject.SetActive(true);
						chestSlot.Draw(conveyorChestSlotData, conveyorChestComponent, new Action<UIConveyorChestSlot>(this.SetChestItemSettingMode), new Action<UIConveyorChestSlot>(this.RemoveItemFromConveyorSlot));
					}
				}
				return;
			}
		}
		ConveyorChestOutComponent conveyorChestOutComponent = conveyorWgoData.ConveyorComponent as ConveyorChestOutComponent;
		if (conveyorChestOutComponent != null)
		{
			int count = conveyorChestOutComponent.SlotsData.Count;
			for (int i = 0; i < this.chestSlots.Count; i++)
			{
				if (i >= count)
				{
					this.chestSlots[i].gameObject.SetActive(false);
				}
				else
				{
					this.chestSlots[i].gameObject.SetActive(true);
					this.chestSlots[i].Draw(conveyorChestOutComponent.SlotsData[i], conveyorChestOutComponent, new Action<UIConveyorChestSlot>(this.SetChestItemSettingMode), new Action<UIConveyorChestSlot>(this.RemoveItemFromConveyorSlot));
				}
			}
		}
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x00150978 File Offset: 0x0014EB78
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
		this.RedrawChestSlots(null);
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x001509D4 File Offset: 0x0014EBD4
	private void RemoveItemFromConveyorSlot(UIConveyorChestSlot conveyorChestSlot)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		if (!(conveyorWgoData.ConveyorComponent is ConveyorChestComponent))
		{
			return;
		}
		conveyorChestSlot.SlotData.SetItemSlotId(string.Empty);
		this.RedrawChestSlots(null);
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x00150A1B File Offset: 0x0014EC1B
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

	// Token: 0x06004724 RID: 18212 RVA: 0x00150A44 File Offset: 0x0014EC44
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
		this.RedrawChestSlots(null);
		this.UpdateGamepadDependentStuff();
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x00150D34 File Offset: 0x0014EF34
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

	// Token: 0x0400377E RID: 14206
	[SerializeField]
	private List<UIConveyorChestSlot> chestSlots = new List<UIConveyorChestSlot>();

	// Token: 0x0400377F RID: 14207
	[SerializeField]
	private List<GameObject> whenSelectionEnabledObjects;

	// Token: 0x04003780 RID: 14208
	private Action<UIItemCell> previousChestOnPress;

	// Token: 0x04003781 RID: 14209
	private Action<UIItemCell> previousChestOnPress2;

	// Token: 0x04003782 RID: 14210
	private bool isItemSelectionModActive;

	// Token: 0x04003783 RID: 14211
	private UIConveyorChestSlot selectedSlot;
}
