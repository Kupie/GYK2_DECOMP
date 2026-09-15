using System;
using LazyBearTechnology;

// Token: 0x02000A7D RID: 2685
public class UIVendorWindowData : LazyWidgetDataBase
{
	// Token: 0x17000B00 RID: 2816
	// (get) Token: 0x060048ED RID: 18669 RVA: 0x00159B1F File Offset: 0x00157D1F
	// (set) Token: 0x060048EE RID: 18670 RVA: 0x00159B27 File Offset: 0x00157D27
	public Vendor Vendor { get; set; }

	// Token: 0x17000B01 RID: 2817
	// (get) Token: 0x060048EF RID: 18671 RVA: 0x00159B30 File Offset: 0x00157D30
	// (set) Token: 0x060048F0 RID: 18672 RVA: 0x00159B38 File Offset: 0x00157D38
	public MultiInventoryWidgetData PlayerMultiInventoryWidgetData { get; set; }

	// Token: 0x17000B02 RID: 2818
	// (get) Token: 0x060048F1 RID: 18673 RVA: 0x00159B41 File Offset: 0x00157D41
	// (set) Token: 0x060048F2 RID: 18674 RVA: 0x00159B49 File Offset: 0x00157D49
	public MultiInventoryWidgetData VendorMultiInventoryWidgetData { get; set; }

	// Token: 0x17000B03 RID: 2819
	// (get) Token: 0x060048F3 RID: 18675 RVA: 0x00159B52 File Offset: 0x00157D52
	// (set) Token: 0x060048F4 RID: 18676 RVA: 0x00159B5A File Offset: 0x00157D5A
	public MoneyWidgetData PlayerMoneyWidgetData { get; set; }

	// Token: 0x17000B04 RID: 2820
	// (get) Token: 0x060048F5 RID: 18677 RVA: 0x00159B63 File Offset: 0x00157D63
	// (set) Token: 0x060048F6 RID: 18678 RVA: 0x00159B6B File Offset: 0x00157D6B
	public MoneyWidgetData VendorMoneyWidgetData { get; set; }

	// Token: 0x17000B05 RID: 2821
	// (get) Token: 0x060048F7 RID: 18679 RVA: 0x00159B74 File Offset: 0x00157D74
	// (set) Token: 0x060048F8 RID: 18680 RVA: 0x00159B7C File Offset: 0x00157D7C
	public MoneyWidgetData DealMoneyWidgetData { get; set; }

	// Token: 0x17000B06 RID: 2822
	// (get) Token: 0x060048F9 RID: 18681 RVA: 0x00159B85 File Offset: 0x00157D85
	// (set) Token: 0x060048FA RID: 18682 RVA: 0x00159B8D File Offset: 0x00157D8D
	public InventoryWidgetData DealBuyInventoryWidgetData { get; set; }

	// Token: 0x17000B07 RID: 2823
	// (get) Token: 0x060048FB RID: 18683 RVA: 0x00159B96 File Offset: 0x00157D96
	// (set) Token: 0x060048FC RID: 18684 RVA: 0x00159B9E File Offset: 0x00157D9E
	public InventoryWidgetData DealSellInventoryWidgetData { get; set; }

	// Token: 0x17000B08 RID: 2824
	// (get) Token: 0x060048FD RID: 18685 RVA: 0x00159BA7 File Offset: 0x00157DA7
	// (set) Token: 0x060048FE RID: 18686 RVA: 0x00159BAF File Offset: 0x00157DAF
	public Action OnApplyDealBtnClicked { get; set; }

	// Token: 0x17000B09 RID: 2825
	// (get) Token: 0x060048FF RID: 18687 RVA: 0x00159BB8 File Offset: 0x00157DB8
	// (set) Token: 0x06004900 RID: 18688 RVA: 0x00159BC0 File Offset: 0x00157DC0
	public Action OnCancelBtnClicked { get; set; }

	// Token: 0x17000B0A RID: 2826
	// (get) Token: 0x06004901 RID: 18689 RVA: 0x00159BC9 File Offset: 0x00157DC9
	// (set) Token: 0x06004902 RID: 18690 RVA: 0x00159BD1 File Offset: 0x00157DD1
	public Func<bool> ApplyButtonInteractableCondition { get; set; }

	// Token: 0x17000B0B RID: 2827
	// (get) Token: 0x06004903 RID: 18691 RVA: 0x00159BDA File Offset: 0x00157DDA
	// (set) Token: 0x06004904 RID: 18692 RVA: 0x00159BE2 File Offset: 0x00157DE2
	public Func<bool> CancelButtonInteractableCondition { get; set; }

	// Token: 0x17000B0C RID: 2828
	// (get) Token: 0x06004905 RID: 18693 RVA: 0x00159BEB File Offset: 0x00157DEB
	// (set) Token: 0x06004906 RID: 18694 RVA: 0x00159BF3 File Offset: 0x00157DF3
	public Func<bool> EnoughMoneyCondition { get; set; }

	// Token: 0x17000B0D RID: 2829
	// (get) Token: 0x06004907 RID: 18695 RVA: 0x00159BFC File Offset: 0x00157DFC
	// (set) Token: 0x06004908 RID: 18696 RVA: 0x00159C04 File Offset: 0x00157E04
	public Func<bool> EnoughHappinessCondition { get; set; }

	// Token: 0x17000B0E RID: 2830
	// (get) Token: 0x06004909 RID: 18697 RVA: 0x00159C0D File Offset: 0x00157E0D
	// (set) Token: 0x0600490A RID: 18698 RVA: 0x00159C15 File Offset: 0x00157E15
	public Func<bool> PlayerInventoryCanAcceptBuyItemsCondition { get; set; }

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x0600490B RID: 18699 RVA: 0x00159C1E File Offset: 0x00157E1E
	// (set) Token: 0x0600490C RID: 18700 RVA: 0x00159C26 File Offset: 0x00157E26
	public Func<float> GetTotalHappinessDealDelegate { get; set; }

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x0600490D RID: 18701 RVA: 0x00159C2F File Offset: 0x00157E2F
	// (set) Token: 0x0600490E RID: 18702 RVA: 0x00159C37 File Offset: 0x00157E37
	public Func<string, int> GetPendingHappinessSoldCount { get; set; }

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x0600490F RID: 18703 RVA: 0x00159C40 File Offset: 0x00157E40
	// (set) Token: 0x06004910 RID: 18704 RVA: 0x00159C48 File Offset: 0x00157E48
	public Action<int> OnHappinessRewardGranted { get; set; }

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06004911 RID: 18705 RVA: 0x00159C51 File Offset: 0x00157E51
	// (set) Token: 0x06004912 RID: 18706 RVA: 0x00159C59 File Offset: 0x00157E59
	public InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate PlayerInvPriceDelegate { get; set; }

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06004913 RID: 18707 RVA: 0x00159C62 File Offset: 0x00157E62
	// (set) Token: 0x06004914 RID: 18708 RVA: 0x00159C6A File Offset: 0x00157E6A
	public InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate VendorInvPriceDelegate { get; set; }

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06004915 RID: 18709 RVA: 0x00159C73 File Offset: 0x00157E73
	// (set) Token: 0x06004916 RID: 18710 RVA: 0x00159C7B File Offset: 0x00157E7B
	public InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate SellInvPriceDelegate { get; set; }

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06004917 RID: 18711 RVA: 0x00159C84 File Offset: 0x00157E84
	// (set) Token: 0x06004918 RID: 18712 RVA: 0x00159C8C File Offset: 0x00157E8C
	public InventoryWidgetBase<InventoryWidgetData>.ItemPriceDelegate BuyInvPriceDelegate { get; set; }

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x06004919 RID: 18713 RVA: 0x00159C95 File Offset: 0x00157E95
	// (set) Token: 0x0600491A RID: 18714 RVA: 0x00159C9D File Offset: 0x00157E9D
	public Action OnRedraw { get; set; }

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x0600491B RID: 18715 RVA: 0x00159CA6 File Offset: 0x00157EA6
	// (set) Token: 0x0600491C RID: 18716 RVA: 0x00159CAE File Offset: 0x00157EAE
	public Action OnWindowClosed { get; set; }
}
