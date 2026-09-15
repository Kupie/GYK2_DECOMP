using System;
using System.Runtime.CompilerServices;
using LazyBearTechnology;

// Token: 0x02000A78 RID: 2680
public class UIVendorOrderWidgetData : LazyWidgetDataBase
{
	// Token: 0x17000AF4 RID: 2804
	// (get) Token: 0x060048BA RID: 18618 RVA: 0x00158AE7 File Offset: 0x00156CE7
	// (set) Token: 0x060048BB RID: 18619 RVA: 0x00158AEF File Offset: 0x00156CEF
	public VendorOrderData VendorOrderData { get; private set; }

	// Token: 0x17000AF5 RID: 2805
	// (get) Token: 0x060048BC RID: 18620 RVA: 0x00158AF8 File Offset: 0x00156CF8
	// (set) Token: 0x060048BD RID: 18621 RVA: 0x00158B00 File Offset: 0x00156D00
	public Vendor Vendor { get; private set; }

	// Token: 0x17000AF6 RID: 2806
	// (get) Token: 0x060048BE RID: 18622 RVA: 0x00158B09 File Offset: 0x00156D09
	// (set) Token: 0x060048BF RID: 18623 RVA: 0x00158B11 File Offset: 0x00156D11
	public bool IsEmpty { get; private set; }

	// Token: 0x17000AF7 RID: 2807
	// (get) Token: 0x060048C0 RID: 18624 RVA: 0x00158B1A File Offset: 0x00156D1A
	// (set) Token: 0x060048C1 RID: 18625 RVA: 0x00158B22 File Offset: 0x00156D22
	public Action<UIVendorOrderWidget> OnPress { get; private set; }

	// Token: 0x17000AF8 RID: 2808
	// (get) Token: 0x060048C2 RID: 18626 RVA: 0x00158B2B File Offset: 0x00156D2B
	// (set) Token: 0x060048C3 RID: 18627 RVA: 0x00158B33 File Offset: 0x00156D33
	[TupleElementNames(new string[] { "hasForceState", "isInteractable" })]
	public ValueTuple<bool, bool> ForcedInteractableState
	{
		[return: TupleElementNames(new string[] { "hasForceState", "isInteractable" })]
		get;
		[param: TupleElementNames(new string[] { "hasForceState", "isInteractable" })]
		private set;
	}

	// Token: 0x17000AF9 RID: 2809
	// (get) Token: 0x060048C4 RID: 18628 RVA: 0x00158B3C File Offset: 0x00156D3C
	// (set) Token: 0x060048C5 RID: 18629 RVA: 0x00158B44 File Offset: 0x00156D44
	public int IndexInOrders { get; private set; }

	// Token: 0x17000AFA RID: 2810
	// (get) Token: 0x060048C6 RID: 18630 RVA: 0x00158B4D File Offset: 0x00156D4D
	// (set) Token: 0x060048C7 RID: 18631 RVA: 0x00158B55 File Offset: 0x00156D55
	public bool IsRenewableGreen { get; private set; }

	// Token: 0x060048C8 RID: 18632 RVA: 0x00158B5E File Offset: 0x00156D5E
	public UIVendorOrderWidgetData(Vendor vendor, VendorOrderData vendorOrderData, bool isEmpty, [TupleElementNames(new string[] { "hasForceState", "isInteractable" })] ValueTuple<bool, bool> forcedInteractableState, Action<UIVendorOrderWidget> onPress = null, int indexInOrders = -1, bool isRenewableGreen = true)
	{
		this.IndexInOrders = indexInOrders;
		this.VendorOrderData = vendorOrderData;
		this.Vendor = vendor;
		this.IsEmpty = isEmpty;
		this.OnPress = onPress;
		this.ForcedInteractableState = forcedInteractableState;
		this.IsRenewableGreen = isRenewableGreen;
	}
}
