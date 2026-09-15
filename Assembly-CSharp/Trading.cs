using System;
using System.Collections.Generic;
using System.Text;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000451 RID: 1105
public class Trading
{
	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06001CEA RID: 7402 RVA: 0x00087C66 File Offset: 0x00085E66
	// (set) Token: 0x06001CEB RID: 7403 RVA: 0x00087C77 File Offset: 0x00085E77
	private int PlayerMoney
	{
		get
		{
			return MainGame.PlayerData.GetResInt("money");
		}
		set
		{
			MainGame.PlayerData.SetRes("money", (float)value);
		}
	}

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06001CEC RID: 7404 RVA: 0x00087C8A File Offset: 0x00085E8A
	// (set) Token: 0x06001CED RID: 7405 RVA: 0x00087CA0 File Offset: 0x00085EA0
	private float PlayerHappiness
	{
		get
		{
			return MainGame.PlayerData.GetRes("happiness", 0f);
		}
		set
		{
			MainGame.PlayerData.SetRes("happiness", value);
		}
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x00087CB4 File Offset: 0x00085EB4
	public void FillVendorWindowData(UIVendorWindowData vendorWindowData, string vendorId, Action onClosed)
	{
		Trading.<>c__DisplayClass9_0 CS$<>8__locals1 = new Trading.<>c__DisplayClass9_0();
		CS$<>8__locals1.<>4__this = this;
		CS$<>8__locals1.onClosed = onClosed;
		CS$<>8__locals1.vendor = MainGame.Instance.GameSave.vendorSystem.GetVendor(vendorId);
		if (CS$<>8__locals1.vendor != null)
		{
			this.cachedWindowData = vendorWindowData;
			this.buyInventory = new Inventory("inventory", 6);
			this.sellInventory = new Inventory("inventory", 6);
			this.cachedWindowData.Vendor = CS$<>8__locals1.vendor;
			this.cachedWindowData.PlayerMoneyWidgetData = new MoneyWidgetData();
			this.cachedWindowData.PlayerMoneyWidgetData.Money = () => CS$<>8__locals1.<>4__this.PlayerMoney;
			this.cachedWindowData.VendorMoneyWidgetData = new MoneyWidgetData();
			this.cachedWindowData.VendorMoneyWidgetData.Money = () => CS$<>8__locals1.vendor.CurMoney;
			this.cachedWindowData.DealMoneyWidgetData = new MoneyWidgetData();
			this.cachedWindowData.DealMoneyWidgetData.Money = new Func<int>(this.GetTotalDealPrice);
			this.cachedWindowData.GetTotalHappinessDealDelegate = new Func<float>(this.GetTotalDealHappiness);
			this.cachedWindowData.GetPendingHappinessSoldCount = new Func<string, int>(this.GetPendingHappinessSoldCount);
			this.cachedWindowData.PlayerMultiInventoryWidgetData = new MultiInventoryWidgetData(MultiInventoryWidgetMode.Default);
			List<InventoryWidgetDataBase> widgetsDataForInventory = InventoryWidgetDataHelper.GetWidgetsDataForInventory(MainGame.PlayerData.inventory, delegate(UIItemCell _)
			{
				LazyAudio.PlayAndForget("gui_hover_light");
			}, null, new Action<UIItemCell>(this.OnPlayerItemPress1), new Action<UIItemCell>(this.OnPlayerItemPress2), null, new Func<Item, bool>(this.PlayerItemsAvailableCondition), true, false, ItemRelatedWidgetState.Default, ItemRelatedWidgetState.Default);
			for (int i = 0; i < widgetsDataForInventory.Count; i++)
			{
				widgetsDataForInventory[i].DrawEmptyCellsAsDisabledWhenUnavailable = true;
			}
			this.cachedWindowData.PlayerMultiInventoryWidgetData.AddRange(widgetsDataForInventory);
			this.SortVendorInventory(null);
			CS$<>8__locals1.vendor.Inventory.OnItemsAdd += this.SortVendorInventory;
			this.cachedWindowData.OnWindowClosed = delegate
			{
				CS$<>8__locals1.vendor.Inventory.OnItemsAdd -= CS$<>8__locals1.<>4__this.SortVendorInventory;
				CS$<>8__locals1.<>4__this.ResetDeal(false);
				Action onClosed3 = CS$<>8__locals1.onClosed;
				if (onClosed3 != null)
				{
					onClosed3();
				}
				CS$<>8__locals1.onClosed = null;
			};
			InventoryWidgetData inventoryWidgetData = new InventoryWidgetData(CS$<>8__locals1.vendor.Inventory, new InventoryHeaderWidgetData(CS$<>8__locals1.vendor.Inventory, string.Format("comm-header_2-type_icon-star_tier_{0}", CS$<>8__locals1.vendor.CurTier), string.Format("ui_vendor_tier_{0}", CS$<>8__locals1.vendor.CurTier), true, null), delegate(UIItemCell _)
			{
				LazyAudio.PlayAndForget("gui_hover_light");
			}, null, new Action<UIItemCell>(this.OnVendorItemPress1), new Action<UIItemCell>(this.OnVendorItemPress2), null, new Func<Item, bool>(this.VendorItemsAvailableCondition), new Func<Item, bool>(this.VendorItemsNotShowCondition), ItemRelatedWidgetState.Default, null);
			this.cachedWindowData.VendorMultiInventoryWidgetData = new MultiInventoryWidgetData(MultiInventoryWidgetMode.Default);
			this.cachedWindowData.VendorMultiInventoryWidgetData.Add(inventoryWidgetData);
			if (CS$<>8__locals1.vendor.Definition.tierDataList.Count > CS$<>8__locals1.vendor.CurTier)
			{
				CS$<>8__locals1.<FillVendorWindowData>g__TryFormFakeInventoryForTier|5(CS$<>8__locals1.vendor.NextTierData, CS$<>8__locals1.vendor.CurTier + 1);
			}
			if (CS$<>8__locals1.vendor.Definition.tierDataList.Count > CS$<>8__locals1.vendor.CurTier + 1)
			{
				CS$<>8__locals1.<FillVendorWindowData>g__TryFormFakeInventoryForTier|5(CS$<>8__locals1.vendor.Definition.tierDataList[CS$<>8__locals1.vendor.CurTier + 1], CS$<>8__locals1.vendor.CurTier + 2);
			}
			this.cachedWindowData.DealBuyInventoryWidgetData = new InventoryWidgetData(this.buyInventory, new InventoryHeaderWidgetData(this.buyInventory, "comm-header_2-type_icon-main_inventory", "", true, null), delegate(UIItemCell _)
			{
				LazyAudio.PlayAndForget("gui_hover_light");
			}, null, new Action<UIItemCell>(this.OnBuyInventoryItemPress1), new Action<UIItemCell>(this.OnBuyInventoryItemPress2), null, null, null, ItemRelatedWidgetState.Default, null);
			this.cachedWindowData.DealSellInventoryWidgetData = new InventoryWidgetData(this.sellInventory, new InventoryHeaderWidgetData(this.sellInventory, "comm-header_2-type_icon-main_inventory", "", true, null), delegate(UIItemCell _)
			{
				LazyAudio.PlayAndForget("gui_hover_light");
			}, null, new Action<UIItemCell>(this.OnSellInventoryItemPress1), new Action<UIItemCell>(this.OnSellInventoryItemPress2), null, null, null, ItemRelatedWidgetState.Default, null);
			this.cachedWindowData.OnApplyDealBtnClicked = new Action(this.DoAcceptDeal);
			this.cachedWindowData.OnCancelBtnClicked = delegate
			{
				CS$<>8__locals1.<>4__this.ResetDeal(true);
			};
			this.cachedWindowData.ApplyButtonInteractableCondition = () => CS$<>8__locals1.<>4__this.CanAcceptDeal() && !CS$<>8__locals1.<>4__this.IsDealEmpty();
			this.cachedWindowData.CancelButtonInteractableCondition = () => !CS$<>8__locals1.<>4__this.IsDealEmpty();
			this.cachedWindowData.EnoughMoneyCondition = () => CS$<>8__locals1.<>4__this.EnoughMoney();
			this.cachedWindowData.EnoughHappinessCondition = () => CS$<>8__locals1.<>4__this.EnoughHappiness();
			this.cachedWindowData.PlayerInventoryCanAcceptBuyItemsCondition = new Func<bool>(this.CanPlayerInventoryAcceptBuyItems);
			this.cachedWindowData.PlayerInvPriceDelegate = new InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate(this.GetSingleItemCostInPlayerInventory);
			this.cachedWindowData.VendorInvPriceDelegate = new InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate(this.GetSingleItemCostInTraderInventory);
			this.cachedWindowData.SellInvPriceDelegate = new InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate(this.GetSingleItemCostInPlayerInventory);
			this.cachedWindowData.BuyInvPriceDelegate = new InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate(this.GetSingleItemCostInTraderInventory);
			return;
		}
		Debug.LogError("Can't open vendor window with id:[" + vendorId + "] no such vendor.");
		Action onClosed2 = CS$<>8__locals1.onClosed;
		if (onClosed2 == null)
		{
			return;
		}
		onClosed2();
	}

	// Token: 0x06001CEF RID: 7407 RVA: 0x00088214 File Offset: 0x00086414
	private int GetSingleItemCostInTraderInventory(Item item, int countModificator = 0)
	{
		int num = this.cachedWindowData.Vendor.Inventory.Data.GetTotalCountInInventory(item.id, null, false) + countModificator;
		return Mathf.RoundToInt(Mathf.Round((float)this.cachedWindowData.Vendor.CurPrice(item.id, true, num) * 100f) / 100f);
	}

	// Token: 0x06001CF0 RID: 7408 RVA: 0x00088275 File Offset: 0x00086475
	private int GetSingleItemCostInTraderInventory(string itemID, int countModificator = 0)
	{
		return this.GetSingleItemCostInTraderInventory(new Item(itemID, this.cachedWindowData.Vendor.Inventory.Data.GetTotalCountInInventory(itemID, null, false)), countModificator);
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x000882A4 File Offset: 0x000864A4
	private int GetSingleItemCostInPlayerInventory(Item item, int countModificator = 0)
	{
		int num = this.cachedWindowData.Vendor.Inventory.Data.GetTotalCountInInventory(item.id, null, false) + this.sellInventory.Data.GetTotalCountInInventory(item.id, null, false) + countModificator;
		float num2 = (float)this.cachedWindowData.Vendor.CurPrice(item.id, false, num);
		if (num2 > (float)item.Definition.basePrice)
		{
			num2 = (float)item.Definition.basePrice;
		}
		return Mathf.RoundToInt(Mathf.Round(num2 * 100f) / 100f);
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x0008833C File Offset: 0x0008653C
	private int GetSingleItemCostInPlayerInventory(string itemID, int countModificator = 0)
	{
		return this.GetSingleItemCostInPlayerInventory(new Item(itemID, MainGame.PlayerData.inventory.Data.GetTotalCountInInventory(itemID, null, false)), countModificator);
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x00088362 File Offset: 0x00086562
	private bool IsDealEmpty()
	{
		return this.buyInventory.Data.InventoryCount <= 0 && this.sellInventory.Data.InventoryCount <= 0;
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x0008838F File Offset: 0x0008658F
	private bool CanPlayerInventoryAcceptBuyItems()
	{
		return MainGame.PlayerData.inventory.CanAddItemsToInventory(this.buyInventory);
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x000883A8 File Offset: 0x000865A8
	private bool CanAcceptDeal()
	{
		if (!this.CanPlayerInventoryAcceptBuyItems())
		{
			return false;
		}
		if (!this.cachedWindowData.Vendor.Inventory.CanAddItemsToInventory(this.sellInventory))
		{
			return false;
		}
		float num = (float)this.GetTotalDealPrice();
		return (float)this.PlayerMoney + num >= 0f && (float)this.cachedWindowData.Vendor.CurMoney - num >= 0f && this.EnoughHappiness();
	}

	// Token: 0x06001CF6 RID: 7414 RVA: 0x00088420 File Offset: 0x00086620
	private bool EnoughMoney()
	{
		float num = (float)this.GetTotalDealPrice();
		return (float)this.PlayerMoney + num >= 0f && (float)this.cachedWindowData.Vendor.CurMoney - num >= 0f;
	}

	// Token: 0x06001CF7 RID: 7415 RVA: 0x00088464 File Offset: 0x00086664
	private bool EnoughHappiness()
	{
		float totalDealHappiness = this.GetTotalDealHappiness();
		if (totalDealHappiness >= 0f)
		{
			return true;
		}
		float num = (float)((int)this.cachedWindowData.Vendor.UsedHappinessThisWeek) - this.cachedWindowData.Vendor.UsedHappinessThisWeek;
		return totalDealHappiness >= num;
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x000884B0 File Offset: 0x000866B0
	private void DoAcceptDeal()
	{
		if (!this.CanAcceptDeal())
		{
			return;
		}
		int totalDealPrice = this.GetTotalDealPrice();
		float totalDealHappiness = this.GetTotalDealHappiness();
		this.PlayerMoney += totalDealPrice;
		float num = totalDealHappiness;
		foreach (Item item in this.sellInventory.Data.Inventory)
		{
			foreach (LazyExpression lazyExpression in item.Definition.expressionsOnSell)
			{
				lazyExpression.Evaluate(item);
			}
			TownVendorProductInfo townVendorProductInfo = this.cachedWindowData.Vendor.CurrentTierData.GetTownVendorProductInfo(item.id);
			if (townVendorProductInfo != null && totalDealHappiness > 0f && num > 0f)
			{
				int num2 = item.Count;
				while (num > 0f && num2 > 0)
				{
					num -= townVendorProductInfo.perOne;
					num2--;
					this.cachedWindowData.Vendor.SoldItemsWithHappinessThisWeek.Add(item.id, 1f);
				}
			}
		}
		foreach (Item item2 in this.buyInventory.Data.Inventory)
		{
			foreach (LazyExpression lazyExpression2 in item2.Definition.expressionsOnBuy)
			{
				lazyExpression2.Evaluate(item2);
			}
		}
		if (totalDealHappiness > 0f)
		{
			int num3 = (int)this.cachedWindowData.Vendor.UsedHappinessThisWeek;
			this.cachedWindowData.Vendor.UsedHappinessThisWeek += totalDealHappiness;
			int num4 = (int)this.cachedWindowData.Vendor.UsedHappinessThisWeek;
			if (num4 > num3)
			{
				int num5 = num4 - num3;
				if (this.cachedWindowData.OnHappinessRewardGranted != null)
				{
					this.cachedWindowData.OnHappinessRewardGranted(num5);
				}
				else
				{
					TechPointsSpawner.CreateSpawner(MainGame.PlayerController.MovablePosition, 0, 0, 0, num5);
				}
			}
		}
		this.cachedWindowData.Vendor.CurMoney -= totalDealPrice;
		if (!this.cachedWindowData.Vendor.Inventory.AddItemsToInventory(this.sellInventory))
		{
			Debug.LogError("Can not add player's deal to vendor's inventory");
			return;
		}
		foreach (Item item3 in this.buyInventory.Data.Inventory)
		{
			if (!MainGame.PlayerData.inventory.AddItemToInventory(item3, null, false))
			{
				Debug.LogError("Can not add vendor's deaf's item \"" + item3.id + "\" to players's inventory");
			}
		}
		this.sellInventory.Clear();
		this.buyInventory.Clear();
		Debug.Log("Accepted deal!");
		Action onRedraw = this.cachedWindowData.OnRedraw;
		if (onRedraw == null)
		{
			return;
		}
		onRedraw();
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x00088800 File Offset: 0x00086A00
	private void ResetDeal(bool triggerOnRedraw)
	{
		if (this.sellInventory.Data.InventoryCount > 0)
		{
			MainGame.PlayerData.inventory.AddItemsToInventory(this.sellInventory);
			this.sellInventory.Clear();
		}
		if (this.buyInventory.Data.InventoryCount > 0)
		{
			this.cachedWindowData.Vendor.Inventory.AddItemsToInventory(this.buyInventory);
			this.buyInventory.Clear();
		}
		if (triggerOnRedraw)
		{
			Action onRedraw = this.cachedWindowData.OnRedraw;
			if (onRedraw == null)
			{
				return;
			}
			onRedraw();
		}
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x00088894 File Offset: 0x00086A94
	private int GetTotalDealPrice()
	{
		int num = 0;
		List<string> list = new List<string>();
		foreach (Item item in this.sellInventory.Data.Inventory)
		{
			if (!list.Contains(item.id))
			{
				int totalCountInInventory = this.sellInventory.Data.GetTotalCountInInventory(item.id, null, false);
				for (int i = 0; i < totalCountInInventory; i++)
				{
					num += this.GetSingleItemCostInPlayerInventory(item, -i);
				}
				list.Add(item.id);
			}
		}
		int num2 = 0;
		list = new List<string>();
		foreach (Item item2 in this.buyInventory.Data.Inventory)
		{
			if (!list.Contains(item2.id))
			{
				int totalCountInInventory2 = this.buyInventory.Data.GetTotalCountInInventory(item2.id, null, false);
				for (int j = 0; j < totalCountInInventory2; j++)
				{
					num2 += this.GetSingleItemCostInTraderInventory(item2, j + 1);
				}
				list.Add(item2.id);
			}
		}
		return num - num2;
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000889F4 File Offset: 0x00086BF4
	private int GetPendingHappinessSoldCount(string itemId)
	{
		if (string.IsNullOrEmpty(itemId))
		{
			return 0;
		}
		return this.sellInventory.Data.GetTotalCountInInventory(itemId, null, false) - this.buyInventory.Data.GetTotalCountInInventory(itemId, null, false);
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x00088A28 File Offset: 0x00086C28
	private float GetTotalDealHappiness()
	{
		if (!this.cachedWindowData.Vendor.Definition.townVendor)
		{
			return 0f;
		}
		float num = 0f;
		GameRes gameRes = new GameRes();
		foreach (Item item in this.sellInventory.Data.Inventory)
		{
			TownVendorProductInfo townVendorProductInfo = this.cachedWindowData.Vendor.CurrentTierData.GetTownVendorProductInfo(item.id);
			int @int = this.cachedWindowData.Vendor.SoldItemsWithHappinessThisWeek.GetInt(item.id);
			if (townVendorProductInfo != null && @int < townVendorProductInfo.itemCount)
			{
				gameRes.Add(item.id, (float)item.Count);
			}
		}
		foreach (Item item2 in this.buyInventory.Data.Inventory)
		{
			if (this.cachedWindowData.Vendor.CurrentTierData.GetTownVendorProductInfo(item2.id) != null)
			{
				gameRes.Sub(item2.id, (float)item2.Count);
			}
		}
		for (int i = 0; i < gameRes.List.Count; i++)
		{
			GameResAtom gameResAtom = gameRes.List[i];
			int int2 = this.cachedWindowData.Vendor.SoldItemsWithHappinessThisWeek.GetInt(gameResAtom.type);
			TownVendorProductInfo townVendorProductInfo2 = this.cachedWindowData.Vendor.CurrentTierData.GetTownVendorProductInfo(gameResAtom.type);
			if (gameResAtom.value > 0f)
			{
				int num2 = Math.Clamp((int)gameResAtom.value, 0, townVendorProductInfo2.itemCount - int2);
				num += (float)num2 * townVendorProductInfo2.perOne;
			}
		}
		return Math.Clamp(num, 0f, this.cachedWindowData.Vendor.CurrentTierData.happinessCap.EvaluateFloat() - this.cachedWindowData.Vendor.UsedHappinessThisWeek);
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x00088C54 File Offset: 0x00086E54
	private bool PlayerItemsAvailableCondition(Item item)
	{
		return item != null && !item.IsEmpty && this.cachedWindowData.Vendor.CanBuyItemFromPlayer(item.Definition);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x00088C7C File Offset: 0x00086E7C
	private void OnPlayerItemPress1(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem.Count <= 1)
		{
			this.OnPlayerItemPress2(itemCell);
			return;
		}
		this.OpenItemCountWindow(itemCell, MainGame.PlayerData.inventory, this.sellInventory, delegate(int amount)
		{
			int num = 0;
			for (int i = 0; i < amount; i++)
			{
				num += this.GetSingleItemCostInPlayerInventory(itemCell.DisplayingItem.id, i + 1);
			}
			return num;
		});
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x00088CE5 File Offset: 0x00086EE5
	private void OnPlayerItemPress2(UIItemCell itemCell)
	{
		if (this.TryMoveItem(itemCell, 1, MainGame.PlayerData.inventory, this.sellInventory))
		{
			LazyAudio.PlayAndForget("item_put");
		}
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x00088D0C File Offset: 0x00086F0C
	private bool VendorItemsAvailableCondition(Item item)
	{
		return item != null && !item.IsEmpty && this.cachedWindowData.Vendor.CanSellItemToPlayer(item.Definition) && (!item.IsSeed || this.cachedWindowData.Vendor.Inventory.Data.GetTotalCountInInventory(item.id, null, false) >= 4);
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x00088D70 File Offset: 0x00086F70
	private bool VendorItemsNotShowCondition(Item item)
	{
		return item == null || item.IsEmpty || !this.cachedWindowData.Vendor.CurrentTierData.HasProduct(item.id);
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x00088DA0 File Offset: 0x00086FA0
	private void OnVendorItemPress1(UIItemCell itemCell)
	{
		int purchaseMoveCount = this.GetPurchaseMoveCount(itemCell.DisplayingItem, this.cachedWindowData.Vendor.Inventory);
		if (purchaseMoveCount <= 0)
		{
			return;
		}
		if (itemCell.DisplayingItem.Count <= purchaseMoveCount)
		{
			this.OnVendorItemPress2(itemCell);
			return;
		}
		this.OpenItemCountWindow(itemCell, this.cachedWindowData.Vendor.Inventory, this.buyInventory, delegate(int amount)
		{
			int num = 0;
			for (int i = 0; i < amount; i++)
			{
				num += this.GetSingleItemCostInTraderInventory(itemCell.DisplayingItem.id, -i);
			}
			return num;
		});
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x00088E38 File Offset: 0x00087038
	private void OnVendorItemPress2(UIItemCell itemCell)
	{
		int purchaseMoveCount = this.GetPurchaseMoveCount(itemCell.DisplayingItem, this.cachedWindowData.Vendor.Inventory);
		if (purchaseMoveCount > 0 && this.TryMoveItem(itemCell, purchaseMoveCount, this.cachedWindowData.Vendor.Inventory, this.buyInventory))
		{
			LazyAudio.PlayAndForget("item_put");
		}
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x00088E90 File Offset: 0x00087090
	private void OnBuyInventoryItemPress1(UIItemCell itemCell)
	{
		int purchaseMoveCount = this.GetPurchaseMoveCount(itemCell.DisplayingItem, this.buyInventory);
		if (purchaseMoveCount <= 0)
		{
			return;
		}
		if (itemCell.DisplayingItem.Count <= purchaseMoveCount)
		{
			this.OnBuyInventoryItemPress2(itemCell);
			return;
		}
		this.OpenItemCountWindow(itemCell, this.buyInventory, this.cachedWindowData.Vendor.Inventory, delegate(int amount)
		{
			int num = 0;
			for (int i = 0; i < amount; i++)
			{
				num += this.GetSingleItemCostInPlayerInventory(itemCell.DisplayingItem.id, -i);
			}
			return num;
		});
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x00088F1C File Offset: 0x0008711C
	private void OnBuyInventoryItemPress2(UIItemCell itemCell)
	{
		int purchaseMoveCount = this.GetPurchaseMoveCount(itemCell.DisplayingItem, this.buyInventory);
		if (purchaseMoveCount > 0)
		{
			this.TryMoveItem(itemCell, purchaseMoveCount, this.buyInventory, this.cachedWindowData.Vendor.Inventory);
		}
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x00088F60 File Offset: 0x00087160
	private void OnSellInventoryItemPress1(UIItemCell itemCell)
	{
		if (itemCell.DisplayingItem.Count <= 1)
		{
			this.OnSellInventoryItemPress2(itemCell);
			return;
		}
		this.OpenItemCountWindow(itemCell, this.sellInventory, MainGame.PlayerData.inventory, delegate(int amount)
		{
			int num = 0;
			for (int i = 0; i < amount; i++)
			{
				num += this.GetSingleItemCostInTraderInventory(itemCell.DisplayingItem.id, i + 1);
			}
			return num;
		});
	}

	// Token: 0x06001D07 RID: 7431 RVA: 0x00088FC9 File Offset: 0x000871C9
	private void OnSellInventoryItemPress2(UIItemCell itemCell)
	{
		this.TryMoveItem(itemCell, 1, this.sellInventory, MainGame.PlayerData.inventory);
	}

	// Token: 0x06001D08 RID: 7432 RVA: 0x00088FE4 File Offset: 0x000871E4
	private void OpenItemCountWindow(UIItemCell itemCell, Inventory from, Inventory to, UIItemCountWindowData.PriceCalculateDelegate priceCalculateDelegate)
	{
		if (!to.CanAddItemToInventory(itemCell.DisplayingItem))
		{
			return;
		}
		UIItemCountWindowData uiitemCountWindowData = new UIItemCountWindowData();
		uiitemCountWindowData.Item = new Item(itemCell.DisplayingItem.id, 1);
		uiitemCountWindowData.Min = 1;
		int totalCountInInventory = from.Data.GetTotalCountInInventory(itemCell.DisplayingItem.id, null, false);
		uiitemCountWindowData.Max = to.Data.CanAddItemCountToInventory(itemCell.DisplayingItem.Definition, totalCountInInventory, true, null, false);
		uiitemCountWindowData.OnConfirm = delegate(int count)
		{
			this.TryMoveItem(itemCell, count, from, to);
		};
		uiitemCountWindowData.PriceCalculateDel = priceCalculateDelegate;
		uiitemCountWindowData.IsForVendor = true;
		uiitemCountWindowData.SnapStep = this.GetPurchaseSnapStep(itemCell.DisplayingItem, from, to);
		uiitemCountWindowData.OkBtnData = new UIDialogWindowData.ButtonData(null, LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		uiitemCountWindowData.BackBtnData = new UIDialogWindowData.ButtonData(null, LLBase.L("btn_cancel"), null, true, GameKey.Back, "");
		if (uiitemCountWindowData.SnapStep > 1 && uiitemCountWindowData.Max < uiitemCountWindowData.SnapStep)
		{
			return;
		}
		LazyAudio.PlayAndForget("item_put");
		LazyUI.GetWindow<UIItemCountWindow>().Open(uiitemCountWindowData);
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x00089158 File Offset: 0x00087358
	private bool TryMoveItem(UIItemCell itemCell, int count, Inventory from, Inventory to)
	{
		if (to.AddItemToInventory(new Item(itemCell.DisplayingItem.id, count), null, false))
		{
			from.RemoveItemById(itemCell.DisplayingItem.id, count, null, from.TryFindSourceBagForItem(itemCell.DisplayingItem), false);
			Action onRedraw = this.cachedWindowData.OnRedraw;
			if (onRedraw != null)
			{
				onRedraw();
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x000891BC File Offset: 0x000873BC
	private int GetPurchaseSnapStep(Item item, Inventory from, Inventory to)
	{
		if (item == null || !item.IsSeed)
		{
			return 1;
		}
		bool flag = from == this.cachedWindowData.Vendor.Inventory && to == this.buyInventory;
		bool flag2 = from == this.buyInventory && to == this.cachedWindowData.Vendor.Inventory;
		if (!flag && !flag2)
		{
			return 1;
		}
		return 4;
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x0008921C File Offset: 0x0008741C
	private int GetPurchaseMoveCount(Item item, Inventory source)
	{
		if (item == null || item.IsEmpty)
		{
			return 0;
		}
		if (!item.IsSeed)
		{
			return 1;
		}
		int totalCountInInventory = source.Data.GetTotalCountInInventory(item.id, null, false);
		int num = 4;
		if (totalCountInInventory >= num)
		{
			return num;
		}
		if (source != this.buyInventory)
		{
			return 0;
		}
		return totalCountInInventory;
	}

	// Token: 0x06001D0C RID: 7436 RVA: 0x00089268 File Offset: 0x00087468
	private void SortVendorInventory(List<Item> itemsChanged)
	{
		this.cachedWindowData.Vendor.Inventory.Sort(delegate(Item x, Item y)
		{
			VendorProductData product = this.cachedWindowData.Vendor.CurrentTierData.GetProduct(x.id);
			VendorProductData product2 = this.cachedWindowData.Vendor.CurrentTierData.GetProduct(y.id);
			if (product == null)
			{
				if (product2 == null)
				{
					return 0;
				}
				return -1;
			}
			else
			{
				if (product2 == null)
				{
					return 1;
				}
				int num = this.cachedWindowData.Vendor.CurrentTierData.vendorProducts.IndexOf(product);
				int num2 = this.cachedWindowData.Vendor.CurrentTierData.vendorProducts.IndexOf(product2);
				return num.CompareTo(num2);
			}
		});
	}

	// Token: 0x06001D0D RID: 7437 RVA: 0x0008928C File Offset: 0x0008748C
	public static string FormatMoney(int value, bool printZero = false, string delimiter = " ", GameResIconType iconType = null)
	{
		if (iconType == null)
		{
			iconType = GameResIconType.Common;
		}
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = value < 0;
		value = Math.Abs(value);
		int num = value / 10000;
		int num2 = (value - num * 10000) / 100;
		int num3 = value - num * 10000 - num2 * 100;
		GameResIconConfig configForRes = GameResDisplayConfig.GetConfigForRes("gld", iconType);
		GameResIconConfig configForRes2 = GameResDisplayConfig.GetConfigForRes("slv", iconType);
		GameResIconConfig configForRes3 = GameResDisplayConfig.GetConfigForRes("brz", iconType);
		if (flag)
		{
			stringBuilder.Append("-");
		}
		stringBuilder.Append((num > 0) ? (configForRes.iconName.FontIcon() + num.ToString()) : "");
		stringBuilder.Append((num > 0 && (num2 > 0 || num3 > 0)) ? delimiter : "");
		stringBuilder.Append((num2 > 0) ? (configForRes2.iconName.FontIcon() + num2.ToString()) : "");
		stringBuilder.Append((num2 > 0 && num3 > 0) ? delimiter : "");
		stringBuilder.Append((num3 > 0) ? (configForRes3.iconName.FontIcon() + num3.ToString()) : "");
		if (stringBuilder.Length == 0 && printZero)
		{
			stringBuilder.Append(configForRes3.iconName.FontIcon() + "0");
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04001AEA RID: 6890
	private UIVendorWindowData cachedWindowData;

	// Token: 0x04001AEB RID: 6891
	private Inventory buyInventory;

	// Token: 0x04001AEC RID: 6892
	private Inventory sellInventory;
}
