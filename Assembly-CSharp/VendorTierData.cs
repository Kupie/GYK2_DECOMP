using System;
using System.Collections.Generic;

// Token: 0x0200045B RID: 1115
[Serializable]
public class VendorTierData
{
	// Token: 0x06001D4D RID: 7501 RVA: 0x0008A264 File Offset: 0x00088464
	public VendorProductData GetProduct(string itemId)
	{
		return this.vendorProducts.Find((VendorProductData p) => p.itemId == itemId);
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x0008A298 File Offset: 0x00088498
	public bool HasProduct(string itemId)
	{
		return this.vendorProducts.Find((VendorProductData p) => p.itemId == itemId) != null;
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x0008A2CC File Offset: 0x000884CC
	public TownVendorProductInfo GetTownVendorProductInfo(string itemId)
	{
		return this.townVendorProductInfos.Find((TownVendorProductInfo p) => p.itemId == itemId);
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x0008A300 File Offset: 0x00088500
	public bool HaTownVendorProductInfo(string itemId)
	{
		return this.townVendorProductInfos.Find((TownVendorProductInfo p) => p.itemId == itemId) != null;
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x0008A334 File Offset: 0x00088534
	public bool IsSellingProduct(string itemId)
	{
		return !this.notSelling.Contains(itemId);
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0008A345 File Offset: 0x00088545
	public bool IsBuyingProduct(string itemId)
	{
		return !this.notBuying.Contains(itemId);
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0008A358 File Offset: 0x00088558
	public override string ToString()
	{
		string text = string.Empty;
		for (int i = 0; i < this.newProducts.Count; i++)
		{
			text = text + "New product: " + this.newProducts[i] + "\n";
		}
		return text;
	}

	// Token: 0x04001B0B RID: 6923
	public int dailyMoneyIncome;

	// Token: 0x04001B0C RID: 6924
	public int levelupCosts;

	// Token: 0x04001B0D RID: 6925
	public LazyExpression happinessCap;

	// Token: 0x04001B0E RID: 6926
	public List<string> notBuying = new List<string>();

	// Token: 0x04001B0F RID: 6927
	public List<string> notSelling = new List<string>();

	// Token: 0x04001B10 RID: 6928
	public List<VendorProductData> vendorProducts = new List<VendorProductData>();

	// Token: 0x04001B11 RID: 6929
	public List<TownVendorProductInfo> townVendorProductInfos = new List<TownVendorProductInfo>();

	// Token: 0x04001B12 RID: 6930
	public List<string> newProducts = new List<string>();

	// Token: 0x04001B13 RID: 6931
	public List<string> orders = new List<string>();
}
