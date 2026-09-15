using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A72 RID: 2674
public class UIVendorOrdersSelectionWindow : LazyWindow<UIVendorOrdersSelectionWindowData>
{
	// Token: 0x17000AEE RID: 2798
	// (get) Token: 0x06004895 RID: 18581 RVA: 0x001579A9 File Offset: 0x00155BA9
	public UIVendorOrderWidget SelectedWidget
	{
		get
		{
			return this.selectedWidget;
		}
	}

	// Token: 0x06004896 RID: 18582 RVA: 0x001579B4 File Offset: 0x00155BB4
	public override void Init()
	{
		base.Init();
		SmoothMouseWheelScroll.EnsureForItem(this.vendorsScrollRect, 48f);
		if (this.scrollRect != null && this.scrollRect != this.vendorsScrollRect)
		{
			SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, 48f);
		}
	}

	// Token: 0x06004897 RID: 18583 RVA: 0x00157A0C File Offset: 0x00155C0C
	public override void Redraw()
	{
		base.Redraw();
		this.ReleaseDrawnWidgets();
		bool flag = MainGame.PlayerData.GetResInt("chalk_board_enabled") > 0;
		this.selectedWidget = null;
		this.helperLabel.text = LLBase.L("ui_order_possible_days", "day_pride".FontIcon());
		if (flag)
		{
			this.helperStyleGrey.ApplyStyle(this.helperLabel, false, null, null, null);
		}
		else
		{
			this.helperStyleRed.ApplyStyle(this.helperLabel, false, null, null, null);
		}
		foreach (Vendor vendor in this.data.VendorsToDraw)
		{
			UICraftsTabSeparatorWidgetData uicraftsTabSeparatorWidgetData = new UICraftsTabSeparatorWidgetData();
			UICraftsTabSeparatorWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftsTabSeparatorWidget>(this.vendorsScrollRect.content);
			elementFromPool.Draw(uicraftsTabSeparatorWidgetData);
			this.displayedTabSeparators.Add(elementFromPool);
			UIVendorOrdersListWidget elementFromPool2 = UIPrefabsPooler.Instance.GetElementFromPool<UIVendorOrdersListWidget>(this.vendorsScrollRect.content);
			elementFromPool2.Draw(new UIVendorOrdersListWidgetData(vendor, new Action<UIVendorOrderWidget>(this.OnElementPressed), this.data.IsAllNotInteractable));
			this.drawnListWidgets.Add(elementFromPool2);
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		this.scrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06004898 RID: 18584 RVA: 0x00157BA4 File Offset: 0x00155DA4
	private void OnElementPressed(UIVendorOrderWidget orderWidget)
	{
		if (MainGame.PlayerData.GetResInt("chalk_board_enabled") <= 0)
		{
			return;
		}
		if (orderWidget.Data.VendorOrderData.State != VendorOrderState.Default)
		{
			return;
		}
		if (orderWidget.Data.Vendor.CurTier < orderWidget.Data.VendorOrderData.Tier)
		{
			return;
		}
		this.selectedWidget = orderWidget;
		this.Close();
	}

	// Token: 0x06004899 RID: 18585 RVA: 0x00157C09 File Offset: 0x00155E09
	public override void Close()
	{
		Action onClosed = this.data.OnClosed;
		if (onClosed != null)
		{
			onClosed();
		}
		base.Close();
	}

	// Token: 0x0600489A RID: 18586 RVA: 0x00157C27 File Offset: 0x00155E27
	public override void Hide()
	{
		this.ReleaseDrawnWidgets();
		base.Hide();
	}

	// Token: 0x0600489B RID: 18587 RVA: 0x00157C38 File Offset: 0x00155E38
	private void ReleaseDrawnWidgets()
	{
		foreach (UIVendorOrdersListWidget uivendorOrdersListWidget in this.drawnListWidgets)
		{
			uivendorOrdersListWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIVendorOrdersListWidget>(uivendorOrdersListWidget);
		}
		this.drawnListWidgets.Clear();
		foreach (UICraftsTabSeparatorWidget uicraftsTabSeparatorWidget in this.displayedTabSeparators)
		{
			uicraftsTabSeparatorWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftsTabSeparatorWidget>(uicraftsTabSeparatorWidget);
		}
		this.displayedTabSeparators.Clear();
	}

	// Token: 0x0600489C RID: 18588 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003899 RID: 14489
	[SerializeField]
	private ScrollRect vendorsScrollRect;

	// Token: 0x0400389A RID: 14490
	[SerializeField]
	private TextMeshProUGUI helperLabel;

	// Token: 0x0400389B RID: 14491
	[SerializeField]
	private TextStyle helperStyleGrey;

	// Token: 0x0400389C RID: 14492
	[SerializeField]
	private TextStyle helperStyleRed;

	// Token: 0x0400389D RID: 14493
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x0400389E RID: 14494
	private List<UIVendorOrdersListWidget> drawnListWidgets = new List<UIVendorOrdersListWidget>();

	// Token: 0x0400389F RID: 14495
	private UIVendorOrderWidget selectedWidget;

	// Token: 0x040038A0 RID: 14496
	private List<UICraftsTabSeparatorWidget> displayedTabSeparators = new List<UICraftsTabSeparatorWidget>();
}
