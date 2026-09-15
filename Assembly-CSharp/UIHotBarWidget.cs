using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200086A RID: 2154
public class UIHotBarWidget : LazyWidget<UIHotBarWidgetData>
{
	// Token: 0x1700082F RID: 2095
	// (get) Token: 0x06003716 RID: 14102 RVA: 0x0010A4BD File Offset: 0x001086BD
	public List<UIHotBarItemCell> HotBarItems
	{
		get
		{
			if (!LazyInput.IsGamepadActive)
			{
				return this.equippedItemsKeyboard;
			}
			return this.equippedItemsGamepad;
		}
	}

	// Token: 0x06003717 RID: 14103 RVA: 0x0010A4D3 File Offset: 0x001086D3
	public override void Init()
	{
		base.Init();
		MainGame.OnGameStarted = (Action)Delegate.Combine(MainGame.OnGameStarted, new Action(this.OnGameStarted));
	}

	// Token: 0x06003718 RID: 14104 RVA: 0x0010A4FC File Offset: 0x001086FC
	protected override void SetData(UIHotBarWidgetData data)
	{
		base.SetData(data);
		data.PlayerData.OnPinnedItemsChanged += this.Redraw;
		data.PlayerData.Inventory.OnItemsAdd += this.RedrawHotbarItems;
		data.PlayerData.Inventory.OnItemsRemove += this.RedrawHotbarItems;
		LazyInput.OnInputChanged += this.Redraw;
		this.SubscribeToFightState();
	}

	// Token: 0x06003719 RID: 14105 RVA: 0x0010A578 File Offset: 0x00108778
	public override void Redraw()
	{
		if (this.data == null)
		{
			return;
		}
		base.Redraw();
		this.RedrawHotbarItems(null);
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x0010A590 File Offset: 0x00108790
	public void SetDataOutside(UIHotBarWidgetData outsideData)
	{
		this.SetData(outsideData);
	}

	// Token: 0x0600371B RID: 14107 RVA: 0x0010A599 File Offset: 0x00108799
	private void OnGameStarted()
	{
		this.Redraw();
	}

	// Token: 0x0600371C RID: 14108 RVA: 0x0010A5A4 File Offset: 0x001087A4
	public void RedrawHotbarItems(List<Item> items = null)
	{
		this.keyboardItemsParent.SetActive(!LazyInput.IsGamepadActive);
		this.gamepadItemsParent.SetActive(LazyInput.IsGamepadActive);
		bool flag = this.data.PinnableItem != null;
		for (int i = 0; i < this.HotBarItems.Count; i++)
		{
			this.HotBarItems[i].UIItemCell.ClearCallbacks();
			if (string.IsNullOrEmpty(this.data.PlayerData.pinnedItems[i]))
			{
				if (!flag)
				{
					this.HotBarItems[i].UIItemCell.SetWidgetState(ItemRelatedWidgetState.Default);
					this.HotBarItems[i].UIItemCell.DrawEmpty(false, true, false);
				}
				else
				{
					this.HotBarItems[i].UIItemCell.DrawEmptyInteractable(false, false);
				}
			}
			else
			{
				int totalCountInInventory = this.data.PlayerData.inventory.Data.GetTotalCountInInventory(this.data.PlayerData.pinnedItems[i], null, false);
				Item item = new Item(this.data.PlayerData.pinnedItems[i], totalCountInInventory);
				if (!flag && (totalCountInInventory == 0 || !this.CanBeUsedNow(item)))
				{
					this.HotBarItems[i].DrawNonInteractable(item);
				}
				else
				{
					this.HotBarItems[i].Draw(item, this.data.IsUsable, new Action<UIItemCell>(this.TryUseHotBarItem));
				}
			}
			if (flag)
			{
				int index = i;
				UIItemCell uiitemCell = this.HotBarItems[i].UIItemCell;
				uiitemCell.OnItemCellPress = (Action<UIItemCell>)Delegate.Combine(uiitemCell.OnItemCellPress, new Action<UIItemCell>(delegate(UIItemCell x)
				{
					this.TryPinItemToHotBar(index);
				}));
			}
			this.HotBarItems[i].Init(i);
		}
		this.UpdateButtonsText();
	}

	// Token: 0x0600371D RID: 14109 RVA: 0x0010A77C File Offset: 0x0010897C
	public void UpdateButtonsText()
	{
		foreach (UIHotBarItemCell uihotBarItemCell in this.equippedItemsKeyboard)
		{
			uihotBarItemCell.UpdateBtnText();
		}
	}

	// Token: 0x0600371E RID: 14110 RVA: 0x0010A7CC File Offset: 0x001089CC
	public override void Hide()
	{
		base.Hide();
		foreach (UIHotBarItemCell uihotBarItemCell in this.equippedItemsKeyboard)
		{
			uihotBarItemCell.UIItemCell.ClearCallbacks();
		}
		foreach (UIHotBarItemCell uihotBarItemCell2 in this.equippedItemsGamepad)
		{
			uihotBarItemCell2.UIItemCell.ClearCallbacks();
		}
		if (this.data != null)
		{
			this.data.PlayerData.OnPinnedItemsChanged -= this.Redraw;
			this.data.PlayerData.Inventory.OnItemsAdd -= this.RedrawHotbarItems;
			this.data.PlayerData.Inventory.OnItemsRemove -= this.RedrawHotbarItems;
		}
		this.UnsubscribeFromFightState();
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x0010A8D8 File Offset: 0x00108AD8
	private void TryUseHotBarItem(UIItemCell uiItemCell)
	{
		MainGame.PlayerData.TryUseHotBarItem(uiItemCell.DisplayingItem.id);
	}

	// Token: 0x06003720 RID: 14112 RVA: 0x0010A8EF File Offset: 0x00108AEF
	private bool CanBeUsedNow(Item item)
	{
		return item.IsFertilizer || item.IsSeed || item.Definition.CanBeUsed;
	}

	// Token: 0x06003721 RID: 14113 RVA: 0x0010A90E File Offset: 0x00108B0E
	private void SubscribeToFightState()
	{
		if (LazySingleton<FightingGameController>.Instance == null)
		{
			return;
		}
		LazySingleton<FightingGameController>.Instance.OnFightStateChanged -= this.HandleFightStateChanged;
		LazySingleton<FightingGameController>.Instance.OnFightStateChanged += this.HandleFightStateChanged;
	}

	// Token: 0x06003722 RID: 14114 RVA: 0x0010A94A File Offset: 0x00108B4A
	private void UnsubscribeFromFightState()
	{
		if (LazySingleton<FightingGameController>.Instance == null)
		{
			return;
		}
		LazySingleton<FightingGameController>.Instance.OnFightStateChanged -= this.HandleFightStateChanged;
	}

	// Token: 0x06003723 RID: 14115 RVA: 0x0010A970 File Offset: 0x00108B70
	private void HandleFightStateChanged(FightState fightState)
	{
		this.RedrawHotbarItems(null);
	}

	// Token: 0x06003724 RID: 14116 RVA: 0x0010A979 File Offset: 0x00108B79
	private void TryPinItemToHotBar(int index)
	{
		this.HotBarItems[index].Set(this.data.PinnableItem);
		LazyUI.GetWindow<UIHotBarSelectionWindow>().Close();
	}

	// Token: 0x06003725 RID: 14117 RVA: 0x0010A9A1 File Offset: 0x00108BA1
	[LazyUITest]
	protected override void TestDraw()
	{
		this.Draw(new UIHotBarWidgetData(MainGame.Instance.GameSave, true, null));
	}

	// Token: 0x04002BF4 RID: 11252
	[SerializeField]
	private List<UIHotBarItemCell> equippedItemsKeyboard = new List<UIHotBarItemCell>();

	// Token: 0x04002BF5 RID: 11253
	[SerializeField]
	private List<UIHotBarItemCell> equippedItemsGamepad = new List<UIHotBarItemCell>();

	// Token: 0x04002BF6 RID: 11254
	[SerializeField]
	private GameObject keyboardItemsParent;

	// Token: 0x04002BF7 RID: 11255
	[SerializeField]
	private GameObject gamepadItemsParent;
}
