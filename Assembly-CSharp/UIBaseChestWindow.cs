using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200096A RID: 2410
public abstract class UIBaseChestWindow : LazyWindow<UIBaseChestWindowData>
{
	// Token: 0x06003F77 RID: 16247 RVA: 0x00130190 File Offset: 0x0012E390
	public override void Open(UIBaseChestWindowData data)
	{
		base.Open(data);
		this.playerIcon.ShowWithoutTalent(MainGame.PlayerController, MainGame.PlayerController.View.PlayerAnimation.SkinPreset);
		this.headerLeft.text = LLBase.L("ui_player");
		this.headerRight.text = LLBase.L("ui_storage");
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x001301F4 File Offset: 0x0012E3F4
	public override void Redraw()
	{
		this.leftMultiInventoryWidget.Draw(this.data.FirstMultiInventoryData);
		this.rightMultiInventoryWidget.Draw(this.data.SecondMultiInventoryData);
		this.moneyWidget.Draw(this.data.MoneyWidgetData);
		this.leftMultiInventoryWidget.OnAnyInventoryRedraw = new Action(this.UpdateGamepadDependentStuff);
		this.rightMultiInventoryWidget.OnAnyInventoryRedraw = new Action(this.UpdateGamepadDependentStuff);
		this.leftMultiInventoryWidget.OnMoveAllSimilarBtnInteractableChanged = new Action(this.RefreshMoveAllSimilarGamepadTips);
		this.leftMultiInventoryWidget.SelectFirstWidget();
		this.rightMultiInventoryWidget.SelectFirstWidget();
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
			this.UpdateGamepadDependentStuff();
		}
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x001302CB File Offset: 0x0012E4CB
	public override void Hide()
	{
		if (this.leftMultiInventoryWidget != null)
		{
			this.leftMultiInventoryWidget.OnMoveAllSimilarBtnInteractableChanged = null;
		}
		base.Hide();
		this.leftMultiInventoryWidget.Hide();
		this.rightMultiInventoryWidget.Hide();
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x00130304 File Offset: 0x0012E504
	private bool OnItemPressed2()
	{
		if (LazyInput.IsGamepadActive)
		{
			GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
			UIItemCell uiitemCell;
			if (focusedItem != null && focusedItem.TryGetComponent<UIItemCell>(out uiitemCell) && uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty)
			{
				uiitemCell.OnGamepadPress2();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x00130355 File Offset: 0x0012E555
	protected virtual void OnAllToChestPressed()
	{
		Action onMoveAllSimilarItemFromPlayerToChest = this.data.OnMoveAllSimilarItemFromPlayerToChest;
		if (onMoveAllSimilarItemFromPlayerToChest == null)
		{
			return;
		}
		onMoveAllSimilarItemFromPlayerToChest();
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x0013036C File Offset: 0x0012E56C
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.ItemMove, new Func<bool>(this.OnItemPressed2));
		gameKeyDelegates.Add(GameKey.MoveAllItemsFromPlayer, delegate
		{
			this.OnAllToChestPressed();
			return true;
		});
		return gameKeyDelegates;
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x001303A2 File Offset: 0x0012E5A2
	protected void UpdateGamepadDependentStuffBasic()
	{
		base.UpdateGamepadDependentStuff();
	}

	// Token: 0x06003F7E RID: 16254 RVA: 0x001303AC File Offset: 0x0012E5AC
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (LazyInput.IsGamepadActive)
		{
			GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
			base.GamepadNavigationController.ReinitItems(false, null, null);
			base.GamepadNavigationController.navigationGroupSources.Clear();
			int count = this.leftMultiInventoryWidget.DrawnInventories.Count;
			int count2 = this.rightMultiInventoryWidget.DrawnInventories.Count;
			List<GamepadNavigationController.NavigationGroupTarget> list = new List<GamepadNavigationController.NavigationGroupTarget>();
			List<GamepadNavigationController.NavigationGroupTarget> list2 = new List<GamepadNavigationController.NavigationGroupTarget>();
			for (int i = 0; i < count2; i++)
			{
				list.Add(new GamepadNavigationController.NavigationGroupTarget(i + 1000, GUIDirection.Right));
			}
			for (int j = 0; j < count; j++)
			{
				list2.Add(new GamepadNavigationController.NavigationGroupTarget(j, GUIDirection.Left));
			}
			for (int k = 0; k < count; k++)
			{
				InventoryWidget inventoryWidget = this.leftMultiInventoryWidget.DrawnInventories[k];
				List<GamepadNavigationController.NavigationGroupTarget> list3 = new List<GamepadNavigationController.NavigationGroupTarget>();
				list3.AddRange(list);
				list3.Add(new GamepadNavigationController.NavigationGroupTarget(k - 1, GUIDirection.Up));
				list3.Add(new GamepadNavigationController.NavigationGroupTarget(k + 1, GUIDirection.Down));
				base.GamepadNavigationController.navigationGroupSources.Add(new GamepadNavigationController.NavigationGroupSource(k, list3));
				for (int l = 0; l < inventoryWidget.Cells.Count; l++)
				{
					inventoryWidget.Cells[l].GamepadNavigationItem.group = k;
				}
			}
			for (int m = 1000; m < count2 + 1000; m++)
			{
				InventoryWidget inventoryWidget2 = this.rightMultiInventoryWidget.DrawnInventories[m - 1000];
				List<GamepadNavigationController.NavigationGroupTarget> list4 = new List<GamepadNavigationController.NavigationGroupTarget>();
				list4.AddRange(list2);
				list4.Add(new GamepadNavigationController.NavigationGroupTarget(m - 1, GUIDirection.Up));
				list4.Add(new GamepadNavigationController.NavigationGroupTarget(m + 1, GUIDirection.Down));
				base.GamepadNavigationController.navigationGroupSources.Add(new GamepadNavigationController.NavigationGroupSource(m, list4));
				for (int n = 0; n < inventoryWidget2.Cells.Count; n++)
				{
					inventoryWidget2.Cells[n].GamepadNavigationItem.group = m;
				}
			}
			if (focusedItem != null)
			{
				base.GamepadNavigationController.SetFocusedItem(focusedItem);
			}
		}
	}

	// Token: 0x06003F7F RID: 16255 RVA: 0x001305DD File Offset: 0x0012E7DD
	protected override void PrintTips()
	{
		this.PrintTips(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x06003F80 RID: 16256 RVA: 0x001305F0 File Offset: 0x0012E7F0
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		UIItemCell uiitemCell;
		if (gamepadNavigationItem != null && gamepadNavigationItem.TryGetComponent<UIItemCell>(out uiitemCell))
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
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.TryAddMoveAllSimilarItemsTip(list);
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06003F81 RID: 16257 RVA: 0x001306B1 File Offset: 0x0012E8B1
	protected void TryAddMoveAllSimilarItemsTip(List<LazyGameKeyTip> tips)
	{
		if (this.leftMultiInventoryWidget.IsMoveAllSimilarBtnInteractable)
		{
			tips.Add(new LazyGameKeyTip(GameKey.MoveAllItemsFromPlayer, "tip_move_similar_items", true, true, true));
		}
	}

	// Token: 0x06003F82 RID: 16258 RVA: 0x001306D8 File Offset: 0x0012E8D8
	private void RefreshMoveAllSimilarGamepadTips()
	{
		if (LazyInput.IsGamepadActive)
		{
			this.PrintTips(base.GamepadNavigationController.FocusedItem);
		}
	}

	// Token: 0x06003F83 RID: 16259 RVA: 0x001306F4 File Offset: 0x0012E8F4
	[LazyUITest]
	protected override void TestDraw()
	{
		WgoData wgoData = new WgoData("chest_home", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		wgoData.Inventory.AddItemToInventory(new Item("faith", 5), null, false);
		wgoData.Inventory.AddItemToInventory(new Item("stick", 15), null, false);
		wgoData.Inventory.AddItemToInventory(new Item("bag_universal", 1), null, false);
		LazyUI.GetWindow<UIChestWindow>().Open(new UIBaseChestWindowData(MainGame.PlayerData.inventory, null, wgoData));
	}

	// Token: 0x04003205 RID: 12805
	[SerializeField]
	protected MultiInventoryWidget leftMultiInventoryWidget;

	// Token: 0x04003206 RID: 12806
	[SerializeField]
	protected MultiInventoryWidget rightMultiInventoryWidget;

	// Token: 0x04003207 RID: 12807
	[SerializeField]
	private TextMeshProUGUI headerLeft;

	// Token: 0x04003208 RID: 12808
	[SerializeField]
	private TextMeshProUGUI headerRight;

	// Token: 0x04003209 RID: 12809
	[SerializeField]
	private UIWorkerIcon playerIcon;

	// Token: 0x0400320A RID: 12810
	[SerializeField]
	private MoneyWidget moneyWidget;
}
