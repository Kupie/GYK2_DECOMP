using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A74 RID: 2676
public class UIVendorOrdersWindow : LazyWindow<UIVendorOrdersWindowData>
{
	// Token: 0x060048A6 RID: 18598 RVA: 0x00157E51 File Offset: 0x00156051
	public override void Init()
	{
		base.Init();
		this.prefab.gameObject.SetActive(false);
	}

	// Token: 0x060048A7 RID: 18599 RVA: 0x00157E6C File Offset: 0x0015606C
	public override void Redraw()
	{
		base.Redraw();
		UIDialogWindowData.ButtonData buttonData = new UIDialogWindowData.ButtonData(new Action(this.OpenSelectionWindow), LLBase.L("ui_traders"), null, true, GameKey.OrderWindowTraders, "");
		this.tradersButton.Draw(buttonData);
		foreach (UIVendorOrderWidget uivendorOrderWidget in this.drawnWidgets)
		{
			uivendorOrderWidget.gameObject.SetActive(false);
		}
		bool flag = MainGame.PlayerData.GetResInt("chalk_board_enabled") > 0;
		this.helperLabel.text = LLBase.L("ui_order_possible_days", "day_pride".FontIcon());
		if (flag)
		{
			this.helperStyleGrey.ApplyStyle(this.helperLabel, false, null, null, null);
		}
		else
		{
			this.helperStyleRed.ApplyStyle(this.helperLabel, false, null, null, null);
		}
		List<ValueTuple<VendorOrderData, Vendor>> currentOrders = MainGame.Instance.GameSave.vendorSystem.GetCurrentOrders();
		for (int i = 0; i < currentOrders.Count; i++)
		{
			ValueTuple<VendorOrderData, Vendor> valueTuple = currentOrders[i];
			UIVendorOrderWidget uivendorOrderWidget2 = null;
			foreach (UIVendorOrderWidget uivendorOrderWidget3 in this.drawnWidgets)
			{
				if (!uivendorOrderWidget3.gameObject.activeSelf)
				{
					uivendorOrderWidget2 = uivendorOrderWidget3;
					break;
				}
			}
			if (uivendorOrderWidget2 == null)
			{
				uivendorOrderWidget2 = global::UnityEngine.Object.Instantiate<UIVendorOrderWidget>(this.prefab, this.prefab.transform.parent);
				this.drawnWidgets.Add(uivendorOrderWidget2);
			}
			uivendorOrderWidget2.Init();
			uivendorOrderWidget2.gameObject.SetActive(true);
			if (valueTuple.Item1 == null || valueTuple.Item1.Guid == SGuid.Empty)
			{
				if (flag)
				{
					uivendorOrderWidget2.Draw(new UIVendorOrderWidgetData(null, null, true, new ValueTuple<bool, bool>(false, false), new Action<UIVendorOrderWidget>(this.OpenSelectionWindow), i, true));
				}
				else
				{
					uivendorOrderWidget2.Draw(new UIVendorOrderWidgetData(null, null, true, new ValueTuple<bool, bool>(true, false), null, i, true));
				}
			}
			else if (valueTuple.Item1.State == VendorOrderState.Finished)
			{
				uivendorOrderWidget2.Draw(new UIVendorOrderWidgetData(currentOrders[i].Item2, currentOrders[i].Item1, false, new ValueTuple<bool, bool>(false, false), new Action<UIVendorOrderWidget>(this.OnGetOrderRewardPress), i, true));
			}
			else if (flag)
			{
				uivendorOrderWidget2.Draw(new UIVendorOrderWidgetData(currentOrders[i].Item2, currentOrders[i].Item1, false, new ValueTuple<bool, bool>(false, false), new Action<UIVendorOrderWidget>(this.OpenSelectionWindow), i, true));
			}
			else
			{
				uivendorOrderWidget2.Draw(new UIVendorOrderWidgetData(currentOrders[i].Item2, currentOrders[i].Item1, false, new ValueTuple<bool, bool>(false, false), null, i, true));
			}
		}
		this.helperLabel.gameObject.SetActive(!flag);
		((RectTransform)base.transform).RefreshContentFitter();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x060048A8 RID: 18600 RVA: 0x001581D0 File Offset: 0x001563D0
	private void OpenSelectionWindow()
	{
		LazyUI.GetWindow<UIVendorOrdersSelectionWindow>().Open(new UIVendorOrdersSelectionWindowData(null, true));
	}

	// Token: 0x060048A9 RID: 18601 RVA: 0x001581E4 File Offset: 0x001563E4
	private void OpenSelectionWindow(UIVendorOrderWidget orderWidget)
	{
		if (MainGame.PlayerData.GetResInt("chalk_board_enabled") <= 0)
		{
			return;
		}
		UIVendorOrdersSelectionWindow window = LazyUI.GetWindow<UIVendorOrdersSelectionWindow>();
		window.Open(new UIVendorOrdersSelectionWindowData(delegate
		{
			if (orderWidget != null && window.SelectedWidget != null)
			{
				orderWidget.Draw(new UIVendorOrderWidgetData(window.SelectedWidget.Data.Vendor, window.SelectedWidget.Data.VendorOrderData, false, new ValueTuple<bool, bool>(false, false), new Action<UIVendorOrderWidget>(this.OpenSelectionWindow), orderWidget.Data.IndexInOrders, true));
				MainGame.Instance.GameSave.vendorSystem.currentOrders[orderWidget.Data.IndexInOrders] = window.SelectedWidget.Data.VendorOrderData.Guid;
			}
		}, false));
	}

	// Token: 0x060048AA RID: 18602 RVA: 0x00158244 File Offset: 0x00156444
	private void OnGetOrderRewardPress(UIVendorOrderWidget orderWidget)
	{
		if (orderWidget.Data.VendorOrderData.State != VendorOrderState.Finished)
		{
			return;
		}
		MainGame.Instance.GameSave.vendorSystem.currentOrders[orderWidget.Data.IndexInOrders] = SGuid.Empty;
		int num = orderWidget.Data.VendorOrderData.Definition.happinessReward.EvaluateInt();
		AchievementsSystem.Instance.Unlock("ach_first_town_order");
		AchievementsSystem.Instance.TriggerCountable("order_done", 1);
		if (orderWidget.Data.VendorOrderData.Definition.isRenewable)
		{
			orderWidget.Data.VendorOrderData.State = VendorOrderState.Default;
		}
		for (int i = 0; i < num; i++)
		{
			FlyingTechPoint flyingTechPoint = FlyingTechPoint.Drop(MainGame.PlayerController.transform.position, TechDef.FlyingReses[3], null, this.canvas.sortingOrder + 1);
			Debug.Log(flyingTechPoint.transform.position, flyingTechPoint);
			LazyAudio.Play("tech_point_collect");
		}
		if (MainGame.PlayerData.GetResInt("chalk_board_enabled") > 0)
		{
			orderWidget.Draw(new UIVendorOrderWidgetData(null, null, true, new ValueTuple<bool, bool>(false, false), new Action<UIVendorOrderWidget>(this.OpenSelectionWindow), orderWidget.Data.IndexInOrders, true));
		}
		else
		{
			orderWidget.Draw(new UIVendorOrderWidgetData(null, null, true, new ValueTuple<bool, bool>(true, false), null, orderWidget.Data.IndexInOrders, true));
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.SetFocusedItem(orderWidget.BtnGrey.GetComponent<GamepadNavigationItem>());
		}
	}

	// Token: 0x060048AB RID: 18603 RVA: 0x001583CD File Offset: 0x001565CD
	[LazyUITest]
	protected override void TestDraw()
	{
		LazyUI.GetWindow<UIVendorOrdersWindow>().Open(new UIVendorOrdersWindowData());
	}

	// Token: 0x040038A4 RID: 14500
	[SerializeField]
	private TextMeshProUGUI helperLabel;

	// Token: 0x040038A5 RID: 14501
	[SerializeField]
	private TextStyle helperStyleGrey;

	// Token: 0x040038A6 RID: 14502
	[SerializeField]
	private TextStyle helperStyleRed;

	// Token: 0x040038A7 RID: 14503
	[SerializeField]
	private UIDialogWindowButton tradersButton;

	// Token: 0x040038A8 RID: 14504
	[SerializeField]
	private UIVendorOrderWidget prefab;

	// Token: 0x040038A9 RID: 14505
	private List<UIVendorOrderWidget> drawnWidgets = new List<UIVendorOrderWidget>();
}
