using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x02000459 RID: 1113
[Serializable]
public class Vendor : ObjectLinkedToDefinition<VendorDef>
{
	// Token: 0x170004F9 RID: 1273
	// (get) Token: 0x06001D2C RID: 7468 RVA: 0x0008982E File Offset: 0x00087A2E
	public GameRes SoldItemsWithHappinessThisWeek
	{
		get
		{
			return this.soldItemsWithHappinessThisWeek;
		}
	}

	// Token: 0x170004FA RID: 1274
	// (get) Token: 0x06001D2D RID: 7469 RVA: 0x00089836 File Offset: 0x00087A36
	// (set) Token: 0x06001D2E RID: 7470 RVA: 0x0008983E File Offset: 0x00087A3E
	public float UsedHappinessThisWeek
	{
		get
		{
			return this.usedHappinessThisWeek;
		}
		set
		{
			this.usedHappinessThisWeek = value;
		}
	}

	// Token: 0x170004FB RID: 1275
	// (get) Token: 0x06001D2F RID: 7471 RVA: 0x00089847 File Offset: 0x00087A47
	public int CurTier
	{
		get
		{
			return this.curTier;
		}
	}

	// Token: 0x170004FC RID: 1276
	// (get) Token: 0x06001D30 RID: 7472 RVA: 0x0008984F File Offset: 0x00087A4F
	// (set) Token: 0x06001D31 RID: 7473 RVA: 0x00089857 File Offset: 0x00087A57
	public int CurMoney
	{
		get
		{
			return this.curMoney;
		}
		set
		{
			this.curMoney = value;
		}
	}

	// Token: 0x170004FD RID: 1277
	// (get) Token: 0x06001D32 RID: 7474 RVA: 0x00089860 File Offset: 0x00087A60
	public Inventory Inventory
	{
		get
		{
			return this.inventory;
		}
	}

	// Token: 0x170004FE RID: 1278
	// (get) Token: 0x06001D33 RID: 7475 RVA: 0x00089868 File Offset: 0x00087A68
	public VendorTierData CurrentTierData
	{
		get
		{
			return base.Definition.tierDataList[this.curTier - 1];
		}
	}

	// Token: 0x170004FF RID: 1279
	// (get) Token: 0x06001D34 RID: 7476 RVA: 0x00089882 File Offset: 0x00087A82
	public VendorTierData NextTierData
	{
		get
		{
			return base.Definition.tierDataList[this.curTier];
		}
	}

	// Token: 0x17000500 RID: 1280
	// (get) Token: 0x06001D35 RID: 7477 RVA: 0x0008989A File Offset: 0x00087A9A
	public List<VendorOrderData> Orders
	{
		get
		{
			return this.orders;
		}
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x000898A2 File Offset: 0x00087AA2
	public Vendor()
	{
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x000898C0 File Offset: 0x00087AC0
	public Vendor(string id, int globalCountModificator)
	{
		this.id = id;
		this.inventory = new Inventory("inventory", 0, true);
		this.curTier = base.Definition.startTier;
		this.curMoney = base.Definition.startMoney;
		for (int i = 0; i < this.CurrentTierData.vendorProducts.Count; i++)
		{
			VendorProductData vendorProductData = this.CurrentTierData.vendorProducts[i];
			this.inventory.AddItemToInventory(new Item(vendorProductData.itemId, this.CurBaseCount(vendorProductData, globalCountModificator)), null, false);
		}
		for (int j = 0; j < base.Definition.tierDataList.Count; j++)
		{
			foreach (string text in base.Definition.tierDataList[j].orders)
			{
				this.TryAddMissingOrder(text, j + 1);
			}
		}
	}

	// Token: 0x06001D38 RID: 7480 RVA: 0x000899EC File Offset: 0x00087BEC
	public bool TryAddMissingOrder(string orderId, int tier)
	{
		if (string.IsNullOrEmpty(orderId))
		{
			return false;
		}
		if (this.orders == null)
		{
			this.orders = new List<VendorOrderData>();
		}
		for (int i = 0; i < this.orders.Count; i++)
		{
			if (this.orders[i] != null && this.orders[i].id == orderId)
			{
				return false;
			}
		}
		VendorOrderData vendorOrderData = new VendorOrderData(orderId);
		vendorOrderData.Tier = tier;
		this.orders.Add(vendorOrderData);
		return true;
	}

	// Token: 0x06001D39 RID: 7481 RVA: 0x00089A70 File Offset: 0x00087C70
	public int CurBasePrice(VendorProductData productData)
	{
		if (productData.Definition.isStaticCost)
		{
			return productData.Definition.basePrice;
		}
		int num = productData.Definition.basePrice + MainGame.PlayerData.GetResInt(productData.itemId + "_base_price_global_mod") + productData.priceMod;
		if (num <= 0)
		{
			return 1;
		}
		return num;
	}

	// Token: 0x06001D3A RID: 7482 RVA: 0x00089ACB File Offset: 0x00087CCB
	public int CurBaseCount(VendorProductData productData)
	{
		return this.CurBaseCount(productData, MainGame.PlayerData.GetResInt(productData.itemId + "_base_count_global_mod"));
	}

	// Token: 0x06001D3B RID: 7483 RVA: 0x00089AEE File Offset: 0x00087CEE
	public int CurBaseCount(VendorProductData productData, int globalCountModificator)
	{
		return globalCountModificator + productData.baseCount;
	}

	// Token: 0x06001D3C RID: 7484 RVA: 0x00089AF8 File Offset: 0x00087CF8
	public int CurPrice(string itemId, bool buy, int itemsCount = 0)
	{
		return this.CurPrice(this.CurrentTierData.GetProduct(itemId), buy, itemsCount);
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x00089B0E File Offset: 0x00087D0E
	public float CurPriceFloat(string itemId, bool buy, int itemsCount = 0)
	{
		return this.CurPriceFloat(this.CurrentTierData.GetProduct(itemId), buy, itemsCount);
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x00089B24 File Offset: 0x00087D24
	public int CurPrice(VendorProductData productData, bool buy, int itemsCount = 0)
	{
		if (productData == null)
		{
			return 0;
		}
		if (itemsCount == 0)
		{
			itemsCount = this.CurCount(productData);
		}
		if (itemsCount == 0)
		{
			itemsCount = 1;
		}
		if (productData.Definition.isStaticCost)
		{
			return productData.Definition.basePrice * itemsCount;
		}
		int num = Mathf.RoundToInt(1f * (float)this.CurBasePrice(productData) * Mathf.Sqrt((float)this.CurBaseCount(productData) / (float)itemsCount) * (buy ? 1f : 0.75f));
		if (num <= 0)
		{
			return 1;
		}
		return num;
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x00089BA0 File Offset: 0x00087DA0
	public float CurPriceFloat(VendorProductData productData, bool buy, int itemsCount = 0)
	{
		if (productData == null)
		{
			return 0f;
		}
		if (itemsCount == 0)
		{
			itemsCount = this.CurCount(productData);
		}
		if (itemsCount == 0)
		{
			itemsCount = 1;
		}
		if (productData.Definition.isStaticCost)
		{
			return (float)(productData.Definition.basePrice * itemsCount);
		}
		float num = 1f * (float)this.CurBasePrice(productData) * Mathf.Sqrt((float)this.CurBaseCount(productData) / (float)itemsCount) * (buy ? 1f : 0.75f);
		if (num <= 0f)
		{
			return 1f;
		}
		return num;
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x00089C23 File Offset: 0x00087E23
	public int CurCount(VendorProductData productData)
	{
		return this.CurCount(productData.itemId);
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x00089C31 File Offset: 0x00087E31
	public int CurCount(string itemId)
	{
		return this.inventory.Data.GetTotalCountInInventory(itemId, null, false);
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x00089C46 File Offset: 0x00087E46
	public bool CanSellItemToPlayer(ItemDef itemDef)
	{
		return this.CurrentTierData.HasProduct(itemDef.id) && this.CurrentTierData.IsSellingProduct(itemDef.id);
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x00089C73 File Offset: 0x00087E73
	public bool CanBuyItemFromPlayer(ItemDef itemDef)
	{
		return this.CurrentTierData.HasProduct(itemDef.id) && this.CurrentTierData.IsBuyingProduct(itemDef.id);
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x00089CA0 File Offset: 0x00087EA0
	public void OnEndOfDay()
	{
		this.TradeWithBank();
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x00089CA8 File Offset: 0x00087EA8
	public bool HasHappinessForItem(string itemId, int extraSoldCount = 0, float extraUsedHappiness = 0f)
	{
		if (string.IsNullOrEmpty(itemId))
		{
			return false;
		}
		TownVendorProductInfo townVendorProductInfo = this.CurrentTierData.GetTownVendorProductInfo(itemId);
		return townVendorProductInfo != null && this.CurrentTierData.happinessCap.EvaluateFloat() - this.usedHappinessThisWeek - Math.Max(0f, extraUsedHappiness) > 0f && this.soldItemsWithHappinessThisWeek.GetInt(itemId) + Math.Max(0, extraSoldCount) < townVendorProductInfo.itemCount;
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x00089D1C File Offset: 0x00087F1C
	public void ForceLevelUp()
	{
		if (this.curTier >= base.Definition.tierDataList.Count)
		{
			return;
		}
		int num = this.NeedCapitalForLevelUp();
		this.curTier++;
		this.soldItemsWithHappinessThisWeek.Clear();
		this.usedHappinessThisWeek = 0f;
		this.AddMissingCurrentTierProductsToInventory();
		int num2 = this.CurCapital();
		if (num2 < num)
		{
			this.curMoney += num - num2;
		}
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x00089D90 File Offset: 0x00087F90
	private void AddMissingCurrentTierProductsToInventory()
	{
		List<VendorProductData> vendorProducts = this.CurrentTierData.vendorProducts;
		for (int i = 0; i < vendorProducts.Count; i++)
		{
			VendorProductData vendorProductData = vendorProducts[i];
			if (!string.IsNullOrEmpty(vendorProductData.itemId) && !this.inventory.Data.HasItemQuantityInInventory(vendorProductData.itemId, 1))
			{
				this.inventory.AddItemToInventory(new Item(vendorProductData.itemId, this.CurBaseCount(vendorProductData)), null, false);
			}
		}
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x00089E08 File Offset: 0x00088008
	private int CalcPurchaseCountAtTheEndOfDay(int curCount, int curBaseCount, bool isStaticCost)
	{
		if (curCount == curBaseCount)
		{
			return 0;
		}
		if (curCount != curBaseCount && isStaticCost)
		{
			return curBaseCount - curCount;
		}
		if (curBaseCount == 0)
		{
			return Mathf.FloorToInt(-0.2f * (float)curCount);
		}
		float num = (float)curCount / (float)curBaseCount;
		if (num <= 0.5f)
		{
			return Mathf.RoundToInt(0.2f * (float)curBaseCount);
		}
		if (num >= 1.5f)
		{
			return Mathf.RoundToInt(-0.2f * (float)curBaseCount);
		}
		if (num > 0.5f && num < 1f)
		{
			return Mathf.CeilToInt(0.8f * (num * num - 2f * num + 1f) * (float)curBaseCount);
		}
		if (num > 1f && num < 1.5f)
		{
			return Mathf.FloorToInt(-0.8f * (num * num - 2f * num + 1f) * (float)curBaseCount);
		}
		return 0;
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x00089ED0 File Offset: 0x000880D0
	private void TradeWithBank()
	{
		this.curMoney += this.CurrentTierData.dailyMoneyIncome;
		Debug.Log("#economy# TradeWithBank:[" + this.id + "]");
		List<ItemDef> list = new List<ItemDef>();
		List<int> list2 = new List<int>();
		Vendor.<>c__DisplayClass44_0 CS$<>8__locals1;
		CS$<>8__locals1.itemsToCheck = new List<string>();
		for (int i = 0; i < this.CurrentTierData.vendorProducts.Count; i++)
		{
			Vendor.<TradeWithBank>g__TryAddItemToCheck|44_0(this.CurrentTierData.vendorProducts[i].itemId, ref CS$<>8__locals1);
		}
		for (int j = 0; j < this.inventory.Data.Inventory.Count; j++)
		{
			Vendor.<TradeWithBank>g__TryAddItemToCheck|44_0(this.inventory.Data.Inventory[j].id, ref CS$<>8__locals1);
		}
		foreach (string text in CS$<>8__locals1.itemsToCheck)
		{
			ItemDef data = GameBalance.Me.GetData<ItemDef>(text);
			int num;
			if (this.CurrentTierData.HasProduct(text))
			{
				VendorProductData product = this.CurrentTierData.GetProduct(data.id);
				num = this.CalcPurchaseCountAtTheEndOfDay(this.CurCount(product), this.CurBaseCount(product), data.isStaticCost);
			}
			else
			{
				num = this.CalcPurchaseCountAtTheEndOfDay(this.CurCount(text), 0, data.isStaticCost);
			}
			if (num > 0)
			{
				list.Add(data);
				list2.Add(num);
			}
			else if (num < 0)
			{
				for (int k = 0; k < -num; k++)
				{
					this.inventory.RemoveItemById(data.id, 1, null, null, false);
					this.curMoney += data.basePrice;
				}
			}
		}
		bool flag;
		do
		{
			flag = false;
			for (int l = 0; l < list2.Count; l++)
			{
				if (list2[l] > 0 && this.curMoney >= list[l].basePrice)
				{
					List<int> list3 = list2;
					int num2 = l;
					int num3 = list3[num2];
					list3[num2] = num3 - 1;
					this.curMoney -= list[l].basePrice;
					this.inventory.AddItemToInventory(new Item(list[l].id, 1), null, false);
					flag = true;
				}
			}
		}
		while (flag);
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x0008A14C File Offset: 0x0008834C
	private int CurCapital()
	{
		int num = this.curMoney;
		foreach (Item item in this.inventory.Data.Inventory)
		{
			VendorProductData vendorProductData = this.CurrentTierData.GetProduct(item.id);
			if (vendorProductData == null)
			{
				vendorProductData = new VendorProductData();
				vendorProductData.itemId = item.id;
			}
			num += this.CurBaseCount(vendorProductData) * this.CurBasePrice(vendorProductData);
		}
		return num;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x0008A1E4 File Offset: 0x000883E4
	private int NeedCapitalForLevelUp()
	{
		int num = 0;
		for (int i = 0; i < this.NextTierData.vendorProducts.Count; i++)
		{
			VendorProductData vendorProductData = this.NextTierData.vendorProducts[i];
			int num2 = this.CurBaseCount(vendorProductData);
			int num3 = this.CurBasePrice(vendorProductData);
			num += num2 * num3;
		}
		return num + this.CurrentTierData.levelupCosts;
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x0008A247 File Offset: 0x00088447
	[CompilerGenerated]
	internal static void <TradeWithBank>g__TryAddItemToCheck|44_0(string id, ref Vendor.<>c__DisplayClass44_0 A_1)
	{
		if (!A_1.itemsToCheck.Contains(id))
		{
			A_1.itemsToCheck.Add(id);
		}
	}

	// Token: 0x04001B03 RID: 6915
	private const float SELL_TO_VENDOR_DISCOUNT = 0.75f;

	// Token: 0x04001B04 RID: 6916
	[SerializeField]
	private Inventory inventory;

	// Token: 0x04001B05 RID: 6917
	[SerializeField]
	private int curMoney;

	// Token: 0x04001B06 RID: 6918
	[SerializeField]
	private int curTier;

	// Token: 0x04001B07 RID: 6919
	[SerializeField]
	private float usedHappinessThisWeek;

	// Token: 0x04001B08 RID: 6920
	[SerializeField]
	private GameRes soldItemsWithHappinessThisWeek = new GameRes();

	// Token: 0x04001B09 RID: 6921
	[SerializeField]
	private List<VendorOrderData> orders = new List<VendorOrderData>();
}
