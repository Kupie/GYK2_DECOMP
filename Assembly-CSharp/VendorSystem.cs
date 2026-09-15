using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x020004B7 RID: 1207
[Serializable]
public class VendorSystem
{
	// Token: 0x06002023 RID: 8227 RVA: 0x00098370 File Offset: 0x00096570
	public void PrepareForGame()
	{
		if (this.vendors == null)
		{
			this.vendors = new List<Vendor>();
		}
		GameBalance me = GameBalance.Me;
		List<VendorDef> list = ((me != null) ? me.vendorDefs : null);
		if (list == null)
		{
			return;
		}
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		for (int i = 0; i < list.Count; i++)
		{
			VendorDef vendorDef = list[i];
			if (vendorDef != null && !string.IsNullOrEmpty(vendorDef.id) && this.GetVendor(vendorDef.id) == null)
			{
				this.vendors.Add(new Vendor(vendorDef.id, 0));
				if (!vendorDef.lockedByDefaultInOrdersWindow)
				{
					knowledgeSystem.UnlockVendorForOrders(vendorDef.id);
				}
				Debug.Log("VendorSystem: add vendor [" + vendorDef.id + "]");
			}
		}
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x00098434 File Offset: 0x00096634
	public Vendor GetVendor(string vendorId)
	{
		return this.vendors.Find((Vendor v) => v.id == vendorId);
	}

	// Token: 0x06002025 RID: 8229 RVA: 0x00098468 File Offset: 0x00096668
	public List<ValueTuple<VendorOrderData, Vendor>> GetCurrentOrders()
	{
		if (this.currentOrders.Count == 0)
		{
			for (int i = 0; i < ConstDef.Get("start_orders_count").IntValue; i++)
			{
				this.currentOrders.Add(SGuid.Empty);
			}
		}
		List<ValueTuple<VendorOrderData, Vendor>> list = new List<ValueTuple<VendorOrderData, Vendor>>();
		for (int j = 0; j < this.currentOrders.Count; j++)
		{
			SGuid sguid = this.currentOrders[j];
			if (sguid.IsEmpty)
			{
				list.Add(new ValueTuple<VendorOrderData, Vendor>(null, null));
			}
			else
			{
				for (int k = 0; k < this.vendors.Count; k++)
				{
					Vendor vendor = this.vendors[k];
					for (int l = 0; l < vendor.Orders.Count; l++)
					{
						VendorOrderData vendorOrderData = vendor.Orders[l];
						if (sguid.Guid == vendorOrderData.Guid.Guid)
						{
							list.Add(new ValueTuple<VendorOrderData, Vendor>(vendorOrderData, vendor));
						}
					}
				}
			}
		}
		return list;
	}

	// Token: 0x06002026 RID: 8230 RVA: 0x00098570 File Offset: 0x00096770
	public bool IsOrderFinished(string id)
	{
		foreach (Vendor vendor in this.vendors)
		{
			foreach (VendorOrderData vendorOrderData in vendor.Orders)
			{
				if (!(vendorOrderData.id != id) && (vendorOrderData.IsFinishedOnce || vendorOrderData.State == VendorOrderState.Finished))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06002027 RID: 8231 RVA: 0x0009861C File Offset: 0x0009681C
	public void TryResolveOrders()
	{
		WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById("warehouse");
		WorldZoneData worldZoneDataById2 = MainGame.WorldData.GetWorldZoneDataById("warehouse_cellar");
		List<ValueTuple<VendorOrderData, Vendor>> list = this.GetCurrentOrders();
		for (int i = list.Count - 1; i >= 0; i--)
		{
			ValueTuple<VendorOrderData, Vendor> valueTuple = list[i];
			if (valueTuple.Item1 != null)
			{
				int num = worldZoneDataById.CountItemsOnTownPalettes(valueTuple.Item1.Definition.itemId) + worldZoneDataById2.CountItemsOnTownPalettes(valueTuple.Item1.Definition.itemId);
				if (valueTuple.Item1.Definition.isUrgent)
				{
					if (num >= valueTuple.Item1.Definition.count)
					{
						valueTuple.Item1.State = VendorOrderState.Finished;
					}
				}
				else if (valueTuple.Item1.Count + num >= valueTuple.Item1.Definition.count)
				{
					valueTuple.Item1.State = VendorOrderState.Finished;
				}
				else
				{
					valueTuple.Item1.Count += num;
				}
			}
		}
	}

	// Token: 0x06002028 RID: 8232 RVA: 0x0009872C File Offset: 0x0009692C
	public void AddMoneyToVendor(string vendorId, int money)
	{
		Vendor vendor = this.GetVendor(vendorId);
		if (vendor == null)
		{
			Debug.LogError("Can't add money to vendor:[" + vendorId + "] no such vendor.");
			return;
		}
		vendor.CurMoney += money;
	}

	// Token: 0x06002029 RID: 8233 RVA: 0x00098768 File Offset: 0x00096968
	public void ForceLevelUpVendor(string vendorId)
	{
		Vendor vendor = this.GetVendor(vendorId);
		if (vendor == null)
		{
			Debug.LogError("Can't level up vendor:[" + vendorId + "] no such vendor.");
			return;
		}
		vendor.ForceLevelUp();
	}

	// Token: 0x0600202A RID: 8234 RVA: 0x0009879C File Offset: 0x0009699C
	public void UpdateSystemAtTheEndOfDay(int day)
	{
		this.UpdateVendorsAtTheEndOfDay();
	}

	// Token: 0x0600202B RID: 8235 RVA: 0x000987A4 File Offset: 0x000969A4
	private void UpdateVendorsAtTheEndOfDay()
	{
		foreach (Vendor vendor in this.vendors)
		{
			vendor.OnEndOfDay();
		}
		UIVendorWindow window = LazyUI.GetWindow<UIVendorWindow>();
		if (window.IsShown)
		{
			window.RedrawLite();
		}
	}

	// Token: 0x04001CCC RID: 7372
	public const string WAREHOUSE_ZONE_ID = "warehouse";

	// Token: 0x04001CCD RID: 7373
	public const string WAREHOUSE_CELLAR_ID = "warehouse_cellar";

	// Token: 0x04001CCE RID: 7374
	public List<SGuid> currentOrders = new List<SGuid>();

	// Token: 0x04001CCF RID: 7375
	public List<Vendor> vendors = new List<Vendor>();
}
