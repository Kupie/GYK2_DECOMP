using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

// Token: 0x0200021A RID: 538
[Serializable]
public class VendorDef : BalanceBaseObject
{
	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00040BE8 File Offset: 0x0003EDE8
	public Sprite Icon
	{
		get
		{
			WGODef dataOrNull = GameBalance.Me.GetDataOrNull<WGODef>(this.icon);
			if (dataOrNull == null)
			{
				return null;
			}
			return dataOrNull.Portrait;
		}
	}

	// Token: 0x04000F63 RID: 3939
	[AutoParse("town_vendor")]
	public bool townVendor;

	// Token: 0x04000F64 RID: 3940
	[AutoParse("start_tier")]
	public int startTier;

	// Token: 0x04000F65 RID: 3941
	[AutoParse("start_money")]
	public int startMoney;

	// Token: 0x04000F66 RID: 3942
	[SerializeField]
	[AutoParse("icon")]
	private string icon = string.Empty;

	// Token: 0x04000F67 RID: 3943
	[AutoParse("locked_by_default_in_orders_window")]
	public bool lockedByDefaultInOrdersWindow;

	// Token: 0x04000F68 RID: 3944
	public List<VendorTierData> tierDataList;
}
