using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A7B RID: 2683
public class UIVendorWindow : LazyWindow<UIVendorWindowData>
{
	// Token: 0x060048D2 RID: 18642 RVA: 0x00158C04 File Offset: 0x00156E04
	public override void Init()
	{
		base.Init();
		this.applyDealBtn.onClick.AddListener(new UnityAction(this.OnApplyDealBtnClicked));
		this.cancelDealBtn.onClick.AddListener(new UnityAction(this.OnCancelDealBtnClicked));
		this.applyDealBtn.SetCallbacksIntoGamepadNavigationItem();
		this.cancelDealBtn.SetCallbacksIntoGamepadNavigationItem();
		UIMouseTooltip.Attach(this.happinessProgressBar.gameObject, "tt_trade_hap_bar", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
		UIMouseTooltip.Attach(this.happinessVendorLabel.transform.parent.gameObject, "tt_trade_hap_likes", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x060048D3 RID: 18643 RVA: 0x00158CC8 File Offset: 0x00156EC8
	public override void Open(UIVendorWindowData data)
	{
		base.Open(data);
		data.OnHappinessRewardGranted = new Action<int>(this.PlayHappinessReward);
		this.moneyBefore = MainGame.PlayerData.GetResInt("money");
		UINotificator.isSilent = true;
		data.OnRedraw = new Action(this.RedrawLite);
		this.playerInventoryWidget.Draw(data.PlayerMultiInventoryWidgetData);
		this.vendorInventoryWidget.Draw(data.VendorMultiInventoryWidgetData);
		this.playerMoneyWidget.Draw(data.PlayerMoneyWidgetData);
		this.vendorMoneyWidget.Draw(data.VendorMoneyWidgetData);
		this.dealMoneyWidget.Draw(data.DealMoneyWidgetData);
		this.enoughMoneyStyle.ApplyStyle(this.dealMoneyWidget.MoneyLabel, false, null, null, null);
		this.sellDealInventoryWidget.Draw(data.DealSellInventoryWidgetData);
		this.buyDealInventoryWidget.Draw(data.DealBuyInventoryWidgetData);
		this.playerIcon.ShowWithoutTalent(MainGame.PlayerController, MainGame.PlayerController.View.PlayerAnimation.SkinPreset);
		this.headerlLeft.text = LLBase.L("ui_player");
		this.headerlRight.text = LLBase.L(data.Vendor.Definition.id);
		this.vendorIcon.sprite = data.Vendor.Definition.Icon;
		this.vendorIcon.enabled = this.vendorIcon.sprite != null;
		this.vendorIcon.BlueColorReplace(this.toReplace);
		this.UpdatePrices();
		this.UpdateButtons();
		this.UpdateHappiness();
		this.TryUpdateHappinessStatusIcons();
		this.UpdateDecor();
		this.UpdateInventoryFullGamepadHint();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
			this.UpdateGamepadDependentStuff();
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060048D4 RID: 18644 RVA: 0x00158EB4 File Offset: 0x001570B4
	protected override void HideWindow()
	{
		if (this.data == null)
		{
			return;
		}
		this.CompletePendingHappinessFlights();
		this.data.OnHappinessRewardGranted = null;
		Action onWindowClosed = this.data.OnWindowClosed;
		if (onWindowClosed != null)
		{
			onWindowClosed();
		}
		this.playerInventoryWidget.Hide();
		this.vendorInventoryWidget.Hide();
		this.buyDealInventoryWidget.Hide();
		this.sellDealInventoryWidget.Hide();
		UINotificator.isSilent = false;
		int resInt = MainGame.PlayerData.GetResInt("money");
		if (this.moneyBefore != resInt)
		{
			LazySingleton<UINotificator>.Instance.ShowMoneyNotification(resInt - this.moneyBefore);
		}
		base.HideWindow();
	}

	// Token: 0x060048D5 RID: 18645 RVA: 0x00158F58 File Offset: 0x00157158
	public void RedrawLite()
	{
		this.playerInventoryWidget.Redraw();
		this.UpdateMoneyWidget();
		this.UpdatePrices();
		this.UpdateButtons();
		this.UpdateHappiness();
		this.TryUpdateHappinessStatusIcons();
		this.UpdateDecor();
		this.UpdateInventoryFullGamepadHint();
		if (LazyInput.IsGamepadActive)
		{
			this.UpdateGamepadDependentStuff();
		}
	}

	// Token: 0x060048D6 RID: 18646 RVA: 0x00158FA8 File Offset: 0x001571A8
	private void UpdatePrices()
	{
		this.playerInventoryWidget.UpdatePrices(this.data.PlayerInvPriceDelegate, 1);
		this.sellDealInventoryWidget.UpdatePrices(this.data.SellInvPriceDelegate, 0);
		this.vendorInventoryWidget.UpdatePrices(this.data.VendorInvPriceDelegate, 0);
		this.buyDealInventoryWidget.UpdatePrices(this.data.BuyInvPriceDelegate, 1);
	}

	// Token: 0x060048D7 RID: 18647 RVA: 0x00159014 File Offset: 0x00157214
	private void TryUpdateHappinessStatusIcons()
	{
		if (this.data.Vendor.Definition.townVendor)
		{
			Func<float> getTotalHappinessDealDelegate = this.data.GetTotalHappinessDealDelegate;
			float num = ((getTotalHappinessDealDelegate != null) ? getTotalHappinessDealDelegate() : 0f);
			this.playerInventoryWidget.UpdateHappinessStatusIcons(this.data.Vendor, this.data.GetPendingHappinessSoldCount, num);
			this.vendorInventoryWidget.UpdateHappinessStatusIcons(this.data.Vendor, this.data.GetPendingHappinessSoldCount, num);
			this.sellDealInventoryWidget.UpdateHappinessStatusIcons(this.data.Vendor, this.data.GetPendingHappinessSoldCount, num);
			this.buyDealInventoryWidget.UpdateHappinessStatusIcons(this.data.Vendor, this.data.GetPendingHappinessSoldCount, num);
		}
	}

	// Token: 0x060048D8 RID: 18648 RVA: 0x001590E0 File Offset: 0x001572E0
	private void UpdateMoneyWidget()
	{
		this.playerMoneyWidget.Redraw();
		this.vendorMoneyWidget.Redraw();
		this.dealMoneyWidget.Redraw();
		if (this.data.EnoughMoneyCondition())
		{
			this.enoughMoneyStyle.ApplyStyle(this.dealMoneyWidget.MoneyLabel, false, null, null, null);
			return;
		}
		this.notEnoughMoneyStyle.ApplyStyle(this.dealMoneyWidget.MoneyLabel, false, null, null, null);
	}

	// Token: 0x060048D9 RID: 18649 RVA: 0x00159188 File Offset: 0x00157388
	private void UpdateHappiness()
	{
		if (!this.data.Vendor.Definition.townVendor)
		{
			this.happinessResultLabel.gameObject.SetActive(false);
			this.happinessVendorLabel.gameObject.SetActive(false);
			this.happinessProgressBar.transform.parent.gameObject.SetActive(false);
			return;
		}
		this.happinessResultLabel.gameObject.SetActive(true);
		this.happinessVendorLabel.gameObject.SetActive(true);
		this.happinessProgressBar.transform.parent.gameObject.SetActive(true);
		float num = Mathf.Max(0f, this.data.GetTotalHappinessDealDelegate());
		float num2 = Mathf.Floor(num * 100f) / 100f;
		this.happinessResultLabel.text = string.Format("+{0}{1:0.##}", "happiness".FontIcon(), num2);
		this.happinessVendorLabel.text = string.Format("{0}{1}", "happiness".FontIcon(), this.data.Vendor.CurrentTierData.happinessCap.EvaluateInt() - (int)this.data.Vendor.UsedHappinessThisWeek);
		float num3 = this.data.Vendor.UsedHappinessThisWeek - Mathf.Floor(this.data.Vendor.UsedHappinessThisWeek);
		float num4 = Mathf.Clamp01(num3 + num);
		this.happinessProgressBar.value = num4;
		Canvas.ForceUpdateCanvases();
		float width = this.happinessProgressBar.fillRect.rect.width;
		this.red.sizeDelta = new Vector2(0f, this.red.sizeDelta.y);
		float num5 = num4 - num3;
		float num6 = ((num4 > 0f) ? (Mathf.Clamp01(num5 / num4) * width) : 0f);
		this.green.sizeDelta = new Vector2(num6, this.green.sizeDelta.y);
		if (this.data.EnoughHappinessCondition())
		{
			this.enoughMoneyStyle.ApplyStyle(this.happinessResultLabel, false, null, null, null);
			return;
		}
		this.notEnoughMoneyStyle.ApplyStyle(this.happinessResultLabel, false, null, null, null);
	}

	// Token: 0x060048DA RID: 18650 RVA: 0x00159405 File Offset: 0x00157605
	private void UpdateDecor()
	{
		this.dealDecor.SetActive(string.IsNullOrEmpty(this.dealMoneyWidget.MoneyLabel.text) && !this.happinessVendorLabel.gameObject.activeSelf);
	}

	// Token: 0x060048DB RID: 18651 RVA: 0x00159440 File Offset: 0x00157640
	private void UpdateInventoryFullGamepadHint()
	{
		if (this.inventoryFullGamepadHint == null)
		{
			return;
		}
		bool flag = LazyInput.IsGamepadActive && this.data != null && this.data.PlayerInventoryCanAcceptBuyItemsCondition != null && !this.data.PlayerInventoryCanAcceptBuyItemsCondition();
		this.inventoryFullGamepadHint.SetActive(flag);
	}

	// Token: 0x060048DC RID: 18652 RVA: 0x0015949B File Offset: 0x0015769B
	private void UpdateButtons()
	{
		this.cancelDealBtn.interactable = this.data.CancelButtonInteractableCondition();
		this.applyDealBtn.interactable = this.data.ApplyButtonInteractableCondition();
	}

	// Token: 0x060048DD RID: 18653 RVA: 0x001594D3 File Offset: 0x001576D3
	private void OnApplyDealBtnClicked()
	{
		Action onApplyDealBtnClicked = this.data.OnApplyDealBtnClicked;
		if (onApplyDealBtnClicked == null)
		{
			return;
		}
		onApplyDealBtnClicked();
	}

	// Token: 0x060048DE RID: 18654 RVA: 0x001594EC File Offset: 0x001576EC
	private void PlayHappinessReward(int amount)
	{
		if (amount <= 0)
		{
			return;
		}
		if (GUIElements.Instance.UIWindowSizeType != UIWindowSizeType.Big)
		{
			TechPointsSpawner.CreateSpawner(MainGame.PlayerController.MovablePosition, 0, 0, 0, amount);
			return;
		}
		Vector3 happinessIconWorldPosition = this.GetHappinessIconWorldPosition();
		int num = this.canvas.sortingOrder + 1;
		for (int i = 0; i < amount; i++)
		{
			FlyingTechPoint drop = null;
			drop = FlyingTechPoint.DropFromUI(happinessIconWorldPosition, TechDef.FlyingReses[3], delegate
			{
				this.pendingHappinessFlights.Remove(drop);
			}, num);
			this.pendingHappinessFlights.Add(drop);
			LazyAudio.Play("tech_point_collect");
		}
	}

	// Token: 0x060048DF RID: 18655 RVA: 0x00159594 File Offset: 0x00157794
	private Vector3 GetHappinessIconWorldPosition()
	{
		if (this.happinessResultLabel == null)
		{
			return base.transform.position;
		}
		this.happinessResultLabel.ForceMeshUpdate(false, false);
		TMP_TextInfo textInfo = this.happinessResultLabel.textInfo;
		if (textInfo != null)
		{
			for (int i = 0; i < textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible && tmp_CharacterInfo.elementType == TMP_TextElementType.Sprite)
				{
					Vector3 vector = (tmp_CharacterInfo.bottomLeft + tmp_CharacterInfo.topRight) * 0.5f;
					return this.happinessResultLabel.transform.TransformPoint(vector);
				}
			}
		}
		return this.happinessResultLabel.transform.position;
	}

	// Token: 0x060048E0 RID: 18656 RVA: 0x00159644 File Offset: 0x00157844
	private void CompletePendingHappinessFlights()
	{
		if (this.pendingHappinessFlights.Count == 0)
		{
			return;
		}
		FlyingTechPoint[] array = this.pendingHappinessFlights.ToArray();
		this.pendingHappinessFlights.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				array[i].CompleteImmediately();
			}
		}
	}

	// Token: 0x060048E1 RID: 18657 RVA: 0x00159697 File Offset: 0x00157897
	private void OnCancelDealBtnClicked()
	{
		Action onCancelBtnClicked = this.data.OnCancelBtnClicked;
		if (onCancelBtnClicked == null)
		{
			return;
		}
		onCancelBtnClicked();
	}

	// Token: 0x060048E2 RID: 18658 RVA: 0x001596B0 File Offset: 0x001578B0
	private bool OnItemMovePressed()
	{
		if (LazyInput.IsGamepadActive)
		{
			GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
			UIItemCell uiitemCell;
			if (focusedItem != null && focusedItem.TryGetComponent<UIItemCell>(out uiitemCell) && uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty)
			{
				uiitemCell.OnGamepadPress2();
			}
		}
		return true;
	}

	// Token: 0x060048E3 RID: 18659 RVA: 0x001596FF File Offset: 0x001578FF
	protected override bool OnPressedBack()
	{
		if (this.cancelDealBtn.interactable)
		{
			this.OnCancelDealBtnClicked();
			return true;
		}
		return base.OnPressedBack();
	}

	// Token: 0x060048E4 RID: 18660 RVA: 0x0015971C File Offset: 0x0015791C
	protected override void UpdateGamepadDependentStuff()
	{
		LazyPlatformDependentElement[] componentsInChildren = base.GetComponentsInChildren<LazyPlatformDependentElement>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Init();
		}
		LazyGamepadDependentElement[] componentsInChildren2 = base.GetComponentsInChildren<LazyGamepadDependentElement>(true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].UpdateState();
		}
		this.UpdateInventoryFullGamepadHint();
		if (LazyInput.IsGamepadActive)
		{
			if (this.closeButton)
			{
				this.closeButton.gameObject.SetActive(false);
			}
			for (int j = 0; j < this.playerInventoryWidget.DrawnInventories.Count; j++)
			{
				InventoryWidget inventoryWidget = this.playerInventoryWidget.DrawnInventories[j];
				for (int k = 0; k < inventoryWidget.Cells.Count; k++)
				{
					inventoryWidget.Cells[k].GamepadNavigationItem.group = 0;
				}
			}
			for (int l = 0; l < this.vendorInventoryWidget.DrawnInventories.Count; l++)
			{
				InventoryWidget inventoryWidget2 = this.vendorInventoryWidget.DrawnInventories[l];
				for (int m = 0; m < inventoryWidget2.Cells.Count; m++)
				{
					inventoryWidget2.Cells[m].GamepadNavigationItem.group = 5;
				}
			}
			for (int n = 0; n < this.sellDealInventoryWidget.Cells.Count; n++)
			{
				this.sellDealInventoryWidget.Cells[n].GamepadNavigationItem.group = 1;
			}
			for (int num = 0; num < this.buyDealInventoryWidget.Cells.Count; num++)
			{
				this.buyDealInventoryWidget.Cells[num].GamepadNavigationItem.group = 2;
			}
			this.PrintTips(base.GamepadNavigationController.FocusedItem);
			base.ChangeTipsState(true);
			return;
		}
		if (this.closeButton)
		{
			this.closeButton.gameObject.SetActive(true);
		}
		base.ChangeTipsState(false);
	}

	// Token: 0x060048E5 RID: 18661 RVA: 0x00159913 File Offset: 0x00157B13
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.ItemMove, new Func<bool>(this.OnItemMovePressed));
		gameKeyDelegates.Add(GameKey.AcceptVendorDeal, delegate
		{
			this.OnApplyDealBtnClicked();
			return true;
		});
		return gameKeyDelegates;
	}

	// Token: 0x060048E6 RID: 18662 RVA: 0x0015994C File Offset: 0x00157B4C
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		UIItemCell uiitemCell;
		if (gamepadNavigationItem != null && gamepadNavigationItem.TryGetComponent<UIItemCell>(out uiitemCell))
		{
			if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress != null)
			{
				list.Add(LazyGameKeyTip.Select(true, true, true));
			}
			if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress2 != null)
			{
				list.Add(new LazyGameKeyTip(GameKey.ItemMove, "tip_item_action", true, true, true));
			}
		}
		else
		{
			list.Add(LazyGameKeyTip.Select(true, true, true));
		}
		if (this.applyDealBtn.interactable)
		{
			list.Add(new LazyGameKeyTip(GameKey.AcceptVendorDeal, "tip_accept", true, true, true));
		}
		if (this.closeButton)
		{
			if (this.cancelDealBtn.interactable)
			{
				list.Add(new LazyGameKeyTip(GameKey.Back, "tip_cancel", true, true, true));
			}
			else
			{
				list.Add(LazyGameKeyTip.Back(true, true, true));
			}
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x060048E7 RID: 18663 RVA: 0x00159A6C File Offset: 0x00157C6C
	[LazyUITest]
	protected override void TestDraw()
	{
		Trading trading = new Trading();
		UIVendorWindowData uivendorWindowData = new UIVendorWindowData();
		trading.FillVendorWindowData(uivendorWindowData, "npc_herm", null);
		LazyUI.GetWindow<UIVendorWindow>().Open(uivendorWindowData);
	}

	// Token: 0x060048E8 RID: 18664 RVA: 0x00159A9C File Offset: 0x00157C9C
	[LazyUITest]
	protected void TestTownVendorDraw()
	{
		Trading trading = new Trading();
		UIVendorWindowData uivendorWindowData = new UIVendorWindowData();
		trading.FillVendorWindowData(uivendorWindowData, "test_town_vendor", null);
		LazyUI.GetWindow<UIVendorWindow>().Open(uivendorWindowData);
	}

	// Token: 0x040038CC RID: 14540
	[SerializeField]
	private TextMeshProUGUI headerlLeft;

	// Token: 0x040038CD RID: 14541
	[SerializeField]
	private TextMeshProUGUI headerlRight;

	// Token: 0x040038CE RID: 14542
	[SerializeField]
	private MultiInventoryWidget playerInventoryWidget;

	// Token: 0x040038CF RID: 14543
	[SerializeField]
	private MultiInventoryWidget vendorInventoryWidget;

	// Token: 0x040038D0 RID: 14544
	[SerializeField]
	private MoneyWidget playerMoneyWidget;

	// Token: 0x040038D1 RID: 14545
	[SerializeField]
	private MoneyWidget vendorMoneyWidget;

	// Token: 0x040038D2 RID: 14546
	[SerializeField]
	private MoneyWidget dealMoneyWidget;

	// Token: 0x040038D3 RID: 14547
	[SerializeField]
	private TextStyle enoughMoneyStyle;

	// Token: 0x040038D4 RID: 14548
	[SerializeField]
	private TextStyle notEnoughMoneyStyle;

	// Token: 0x040038D5 RID: 14549
	[SerializeField]
	private VendorDealInventoryWidget sellDealInventoryWidget;

	// Token: 0x040038D6 RID: 14550
	[SerializeField]
	private VendorDealInventoryWidget buyDealInventoryWidget;

	// Token: 0x040038D7 RID: 14551
	[SerializeField]
	private LazyButton cancelDealBtn;

	// Token: 0x040038D8 RID: 14552
	[SerializeField]
	private LazyButton applyDealBtn;

	// Token: 0x040038D9 RID: 14553
	[SerializeField]
	private Image vendorIcon;

	// Token: 0x040038DA RID: 14554
	[SerializeField]
	private Color toReplace = new Color(1f, 1f, 1f, 0f);

	// Token: 0x040038DB RID: 14555
	[SerializeField]
	private UIWorkerIcon playerIcon;

	// Token: 0x040038DC RID: 14556
	[SerializeField]
	private TextMeshProUGUI happinessResultLabel;

	// Token: 0x040038DD RID: 14557
	[SerializeField]
	private TextMeshProUGUI happinessVendorLabel;

	// Token: 0x040038DE RID: 14558
	[SerializeField]
	private Slider happinessProgressBar;

	// Token: 0x040038DF RID: 14559
	[SerializeField]
	private RectTransform green;

	// Token: 0x040038E0 RID: 14560
	[SerializeField]
	private RectTransform red;

	// Token: 0x040038E1 RID: 14561
	[SerializeField]
	private GameObject dealDecor;

	// Token: 0x040038E2 RID: 14562
	[SerializeField]
	private GameObject inventoryFullGamepadHint;

	// Token: 0x040038E3 RID: 14563
	private int moneyBefore;

	// Token: 0x040038E4 RID: 14564
	private readonly List<FlyingTechPoint> pendingHappinessFlights = new List<FlyingTechPoint>();
}
