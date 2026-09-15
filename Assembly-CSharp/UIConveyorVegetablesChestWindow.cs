using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000A51 RID: 2641
public class UIConveyorVegetablesChestWindow : UIBaseChestWindow
{
	// Token: 0x17000AD1 RID: 2769
	// (get) Token: 0x0600472E RID: 18222 RVA: 0x00150DB9 File Offset: 0x0014EFB9
	public bool IsItemSelectionModActive
	{
		get
		{
			return this.isItemSelectionModActive;
		}
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x00150DC1 File Offset: 0x0014EFC1
	public override void Redraw()
	{
		base.Redraw();
		this.RedrawChestSlots(null);
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x00150DD0 File Offset: 0x0014EFD0
	public override void Close()
	{
		base.Close();
		this.data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsAdd -= this.RedrawChestSlots;
		this.data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsRemove -= this.RedrawChestSlots;
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x00150E30 File Offset: 0x0014F030
	protected override void SetData(UIBaseChestWindowData data)
	{
		base.SetData(data);
		data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsAdd += this.RedrawChestSlots;
		data.SecondMultiInventoryData.SelectedWidgetData.Inventory.OnItemsRemove += this.RedrawChestSlots;
	}

	// Token: 0x06004732 RID: 18226 RVA: 0x00150E88 File Offset: 0x0014F088
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

	// Token: 0x06004733 RID: 18227 RVA: 0x00150EF8 File Offset: 0x0014F0F8
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

	// Token: 0x06004734 RID: 18228 RVA: 0x0015104D File Offset: 0x0014F24D
	protected override void InitCloseButton(LazyButton button)
	{
		button.onClick.AddListener(new UnityAction(this.CloseOrDeactivateItemSettingMode));
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x00151066 File Offset: 0x0014F266
	protected override bool OnPressedBack()
	{
		this.CloseOrDeactivateItemSettingMode();
		return true;
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x0015106F File Offset: 0x0014F26F
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

	// Token: 0x06004737 RID: 18231 RVA: 0x00151090 File Offset: 0x0014F290
	private void RedrawChestSlots(List<Item> items = null)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		ConveyorChestOutComponent conveyorChestOutComponent = conveyorWgoData.ConveyorComponent as ConveyorChestOutComponent;
		if (conveyorChestOutComponent == null)
		{
			return;
		}
		conveyorChestOutComponent.UpdateSlotsData();
		List<UIConveyorChestSlot> chestSlots = this.GetChestSlots();
		int num = ((conveyorWgoData.id == "garden_bags_storage_3") ? 3 : 2);
		for (int i = 0; i < chestSlots.Count; i++)
		{
			UIConveyorChestSlot uiconveyorChestSlot = chestSlots[i];
			if (!(uiconveyorChestSlot == null))
			{
				if (i >= num)
				{
					uiconveyorChestSlot.gameObject.SetActive(false);
				}
				else
				{
					int slotIndex = ((uiconveyorChestSlot.SlotIndex > 0) ? uiconveyorChestSlot.SlotIndex : (i + 1));
					ConveyorChestSlotData conveyorChestSlotData = conveyorChestOutComponent.SlotsData.Find((ConveyorChestSlotData x) => x.slotIndex == slotIndex);
					if (conveyorChestSlotData == null)
					{
						uiconveyorChestSlot.gameObject.SetActive(false);
					}
					else
					{
						uiconveyorChestSlot.gameObject.SetActive(true);
						uiconveyorChestSlot.Draw(conveyorChestSlotData, conveyorChestOutComponent, new Action<UIConveyorChestSlot>(this.SetChestItemSettingMode), new Action<UIConveyorChestSlot>(this.RemoveItemFromConveyorSlot));
					}
				}
			}
		}
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x001511B0 File Offset: 0x0014F3B0
	private List<UIConveyorChestSlot> GetChestSlots()
	{
		List<UIConveyorChestSlot> list = new List<UIConveyorChestSlot>();
		if (this.slot1 != null)
		{
			list.Add(this.slot1);
		}
		if (this.slot2 != null)
		{
			list.Add(this.slot2);
		}
		if (this.slot3 != null)
		{
			list.Add(this.slot3);
		}
		if (list.Count == 0)
		{
			list.AddRange(base.GetComponentsInChildren<UIConveyorChestSlot>(true));
			list.Sort(new Comparison<UIConveyorChestSlot>(UIConveyorVegetablesChestWindow.CompareChestSlots));
		}
		return list;
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x0015123C File Offset: 0x0014F43C
	private static int CompareChestSlots(UIConveyorChestSlot a, UIConveyorChestSlot b)
	{
		if (a.SlotIndex > 0 && b.SlotIndex > 0)
		{
			return a.SlotIndex.CompareTo(b.SlotIndex);
		}
		if (a.SlotIndex > 0)
		{
			return -1;
		}
		if (b.SlotIndex > 0)
		{
			return 1;
		}
		return a.transform.position.x.CompareTo(b.transform.position.x);
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x001512B0 File Offset: 0x0014F4B0
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

	// Token: 0x0600473B RID: 18235 RVA: 0x0015130C File Offset: 0x0014F50C
	private void RemoveItemFromConveyorSlot(UIConveyorChestSlot conveyorChestSlot)
	{
		ConveyorWgoData conveyorWgoData = this.data.Chest as ConveyorWgoData;
		if (conveyorWgoData == null)
		{
			return;
		}
		if (!(conveyorWgoData.ConveyorComponent is ConveyorChestOutComponent))
		{
			return;
		}
		conveyorChestSlot.SlotData.SetItemSlotId(string.Empty);
		this.RedrawChestSlots(null);
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x00151353 File Offset: 0x0014F553
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

	// Token: 0x0600473D RID: 18237 RVA: 0x0015137C File Offset: 0x0014F57C
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

	// Token: 0x0600473E RID: 18238 RVA: 0x0015166C File Offset: 0x0014F86C
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

	// Token: 0x0400378A RID: 14218
	private const string GardenBagsStorage3Id = "garden_bags_storage_3";

	// Token: 0x0400378B RID: 14219
	[SerializeField]
	private UIConveyorChestSlot slot1;

	// Token: 0x0400378C RID: 14220
	[SerializeField]
	private UIConveyorChestSlot slot2;

	// Token: 0x0400378D RID: 14221
	[SerializeField]
	private UIConveyorChestSlot slot3;

	// Token: 0x0400378E RID: 14222
	[SerializeField]
	private List<GameObject> whenSelectionEnabledObjects;

	// Token: 0x0400378F RID: 14223
	private Action<UIItemCell> previousChestOnPress;

	// Token: 0x04003790 RID: 14224
	private Action<UIItemCell> previousChestOnPress2;

	// Token: 0x04003791 RID: 14225
	private bool isItemSelectionModActive;

	// Token: 0x04003792 RID: 14226
	private UIConveyorChestSlot selectedSlot;
}
