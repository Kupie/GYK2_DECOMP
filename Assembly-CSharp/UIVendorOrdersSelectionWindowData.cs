using System;
using System.Collections.Generic;
using LazyBearTechnology;

// Token: 0x02000A73 RID: 2675
public class UIVendorOrdersSelectionWindowData : LazyWidgetDataBase
{
	// Token: 0x17000AEF RID: 2799
	// (get) Token: 0x0600489E RID: 18590 RVA: 0x00157D16 File Offset: 0x00155F16
	// (set) Token: 0x0600489F RID: 18591 RVA: 0x00157D1E File Offset: 0x00155F1E
	public List<Vendor> VendorsToDraw { get; private set; }

	// Token: 0x17000AF0 RID: 2800
	// (get) Token: 0x060048A0 RID: 18592 RVA: 0x00157D27 File Offset: 0x00155F27
	// (set) Token: 0x060048A1 RID: 18593 RVA: 0x00157D2F File Offset: 0x00155F2F
	public Action OnClosed { get; private set; }

	// Token: 0x17000AF1 RID: 2801
	// (get) Token: 0x060048A2 RID: 18594 RVA: 0x00157D38 File Offset: 0x00155F38
	// (set) Token: 0x060048A3 RID: 18595 RVA: 0x00157D40 File Offset: 0x00155F40
	public bool IsAllNotInteractable { get; private set; }

	// Token: 0x060048A4 RID: 18596 RVA: 0x00157D4C File Offset: 0x00155F4C
	public UIVendorOrdersSelectionWindowData(Action onClosed, bool isAllNotInteractable)
	{
		this.VendorsToDraw = new List<Vendor>();
		KnowledgeSystem knowledgeSystem = MainGame.Instance.GameSave.knowledgeSystem;
		foreach (Vendor vendor in MainGame.Instance.GameSave.vendorSystem.vendors)
		{
			if (vendor.Definition.townVendor && knowledgeSystem.IsVendorForOrdersUnlocked(vendor.id) && this.HasOrdersInTierData(vendor))
			{
				this.VendorsToDraw.Add(vendor);
			}
		}
		this.IsAllNotInteractable = isAllNotInteractable;
		this.OnClosed = onClosed;
	}

	// Token: 0x060048A5 RID: 18597 RVA: 0x00157E08 File Offset: 0x00156008
	private bool HasOrdersInTierData(Vendor vendor)
	{
		for (int i = 0; i < vendor.Definition.tierDataList.Count; i++)
		{
			if (vendor.Definition.tierDataList[i].orders.Count > 0)
			{
				return true;
			}
		}
		return false;
	}
}
