using System;
using LazyBearTechnology;

// Token: 0x02000A71 RID: 2673
public class UIVendorOrdersListWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000AEB RID: 2795
	// (get) Token: 0x0600488E RID: 18574 RVA: 0x00157959 File Offset: 0x00155B59
	// (set) Token: 0x0600488F RID: 18575 RVA: 0x00157961 File Offset: 0x00155B61
	public Vendor Vendor { get; private set; }

	// Token: 0x17000AEC RID: 2796
	// (get) Token: 0x06004890 RID: 18576 RVA: 0x0015796A File Offset: 0x00155B6A
	// (set) Token: 0x06004891 RID: 18577 RVA: 0x00157972 File Offset: 0x00155B72
	public Action<UIVendorOrderWidget> OnElementPressed { get; private set; }

	// Token: 0x17000AED RID: 2797
	// (get) Token: 0x06004892 RID: 18578 RVA: 0x0015797B File Offset: 0x00155B7B
	// (set) Token: 0x06004893 RID: 18579 RVA: 0x00157983 File Offset: 0x00155B83
	public bool IsAllNotInteractable { get; private set; }

	// Token: 0x06004894 RID: 18580 RVA: 0x0015798C File Offset: 0x00155B8C
	public UIVendorOrdersListWidgetData(Vendor vendor, Action<UIVendorOrderWidget> onElementPressed, bool isAllNotInteractable)
	{
		this.Vendor = vendor;
		this.OnElementPressed = onElementPressed;
		this.IsAllNotInteractable = isAllNotInteractable;
	}
}
