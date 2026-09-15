using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000916 RID: 2326
public class InspirationWidget : LazyWidget<InspirationWidgetData>
{
	// Token: 0x17000930 RID: 2352
	// (get) Token: 0x06003D44 RID: 15684 RVA: 0x00124F59 File Offset: 0x00123159
	public string IdWithoutLevel
	{
		get
		{
			return this.data.IdWithoutLevel;
		}
	}

	// Token: 0x17000931 RID: 2353
	// (get) Token: 0x06003D45 RID: 15685 RVA: 0x00124F66 File Offset: 0x00123166
	public int Level
	{
		get
		{
			return this.data.CurrentLevel;
		}
	}

	// Token: 0x17000932 RID: 2354
	// (get) Token: 0x06003D46 RID: 15686 RVA: 0x00124F73 File Offset: 0x00123173
	public InspirationData InspirationData
	{
		get
		{
			InspirationWidgetData data = this.data;
			if (data == null)
			{
				return null;
			}
			return data.InspirationData;
		}
	}

	// Token: 0x17000933 RID: 2355
	// (get) Token: 0x06003D47 RID: 15687 RVA: 0x00124F86 File Offset: 0x00123186
	public RectTransform BuyExpRect
	{
		get
		{
			if (!(this.buyExp != null))
			{
				return null;
			}
			return this.buyExp.rectTransform;
		}
	}

	// Token: 0x17000934 RID: 2356
	// (get) Token: 0x06003D48 RID: 15688 RVA: 0x00124FA3 File Offset: 0x001231A3
	public TextMeshProUGUI BuyExpLabel
	{
		get
		{
			return this.buyExp;
		}
	}

	// Token: 0x17000935 RID: 2357
	// (get) Token: 0x06003D49 RID: 15689 RVA: 0x00124FAB File Offset: 0x001231AB
	public string PointIconId
	{
		get
		{
			InspirationWidget.TalentViewData talentViewData = this.currentViewData;
			if (talentViewData == null)
			{
				return null;
			}
			return talentViewData.pointIconId;
		}
	}

	// Token: 0x17000936 RID: 2358
	// (get) Token: 0x06003D4A RID: 15690 RVA: 0x00124FBE File Offset: 0x001231BE
	public int DisplayedCompletionExp
	{
		get
		{
			return this.displayedCompletionExp;
		}
	}

	// Token: 0x17000937 RID: 2359
	// (get) Token: 0x06003D4B RID: 15691 RVA: 0x00124FC6 File Offset: 0x001231C6
	public bool IsBuyLocked
	{
		get
		{
			return this.isBuyLocked;
		}
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x00124FCE File Offset: 0x001231CE
	public void HideFlyingReward()
	{
		this.LockBuyButton();
		if (this.buyExp != null)
		{
			this.buyExp.alpha = 0f;
		}
	}

	// Token: 0x06003D4D RID: 15693 RVA: 0x00124FF4 File Offset: 0x001231F4
	public override void Init()
	{
		base.Init();
		this.button.onClick.RemoveAllListeners();
		this.button.onClick.AddListener(new UnityAction(this.OnPress));
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x00125028 File Offset: 0x00123228
	public override void Redraw()
	{
		this.currentViewData = this.viewDatas.Find((InspirationWidget.TalentViewData d) => d.talentId == this.data.InspirationDef.talentId);
		InspirationData inspirationData = this.data.InspirationData;
		string text = inspirationData.id + string.Format("_{0}", this.data.CurrentLevel);
		this.idLabel.text = LLBase.L(text);
		this.descriptionLabel.text = LLBase.L(text + "_d");
		this.icon.sprite = this.data.InspirationDef.Icon;
		this.icon.BlueColorReplace(this.iconOutlineColor);
		this.iconFrame.sprite = this.framesSprites[this.data.CurrentLevelFrame - 1];
		this.UpdateBuyButton(this.data.Price);
		this.progressBar.value = inspirationData.Progress01;
		this.progressLabel.text = ((inspirationData.curProgressValue > inspirationData.completionGoalValue) ? string.Format("{0}/{1}", inspirationData.completionGoalValue, inspirationData.completionGoalValue) : string.Format("{0}/{1}", inspirationData.curProgressValue, inspirationData.completionGoalValue));
		this.displayedCompletionExp = GameBalance.Me.GetData<InspirationDef>(string.Format("{0}_{1}", this.data.InspirationData.id, this.data.CurrentLevel)).completionExp;
		this.buyExp.text = string.Format("+{0}{1}", this.currentViewData.pointIconId.FontIcon(), this.displayedCompletionExp);
		this.buyExp.alpha = 1f;
		this.isBuyLocked = false;
		this.SetBuyButtonInteractable(inspirationData.IsAvailableToBuy);
		this.UpdateDownPart();
		this.onPress = this.data.OnPress;
		if (this.data.InspirationData.IsCompleted)
		{
			this.background.sprite = this.backCompletedSprite;
			this.idStyleComponent.SetTextStyle(this.idStyleCompleted);
			return;
		}
		this.background.sprite = this.backNotCompletedSprite;
		this.idStyleComponent.SetTextStyle(this.idStyleNotCompleted);
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x00125278 File Offset: 0x00123478
	private void UpdateDownPart()
	{
		if (this.data.InspirationData.IsCompleted && !this.data.InspirationData.isAllLevelsBought)
		{
			this.button.gameObject.SetActive(true);
			this.progressBar.transform.parent.gameObject.SetActive(false);
			return;
		}
		this.button.gameObject.SetActive(false);
		this.progressBar.transform.parent.gameObject.SetActive(true);
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x00125302 File Offset: 0x00123502
	private void ResolveBuyLabel()
	{
		if (this.buyLocalizedLabel == null && this.button != null)
		{
			this.buyLocalizedLabel = this.button.GetComponentInChildren<LocalizedLabel>(true);
		}
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x00125334 File Offset: 0x00123534
	private void UpdateBuyButton(int price)
	{
		this.ResolveBuyLabel();
		bool flag = price <= 0;
		if (this.priceLabel != null)
		{
			this.priceLabel.gameObject.SetActive(!flag);
		}
		if (this.visualSeparator != null)
		{
			this.visualSeparator.SetActive(!flag);
		}
		if (this.buyLocalizedLabel != null)
		{
			this.buyLocalizedLabel.gameObject.SetActive(!flag);
			if (!flag)
			{
				this.buyLocalizedLabel.langToken = "ui_insp_btn_buy";
				this.buyLocalizedLabel.Localize();
			}
		}
		if (this.getCenteredLabel != null)
		{
			this.getCenteredLabel.gameObject.SetActive(flag);
			if (flag)
			{
				this.getCenteredLabel.text = LLBase.L("action_get");
			}
		}
		if (!flag)
		{
			this.priceLabel.text = string.Format("{0}{1}", "faith".FontIcon(), price);
			this.priceLabelStyle.SetTextStyle(MainGame.PlayerData.inventory.Data.HasItemQuantityInInventory("faith", price) ? this.priceLabelEnoughStyle : this.priceLabelNotEnoughStyle);
		}
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x00125464 File Offset: 0x00123664
	private void OnPress()
	{
		InspirationWidget.<>c__DisplayClass51_0 CS$<>8__locals1 = new InspirationWidget.<>c__DisplayClass51_0();
		if (this.data.InspirationData == null || this.isBuyLocked)
		{
			return;
		}
		CharInspirationPageWidget componentInParent = base.GetComponentInParent<CharInspirationPageWidget>();
		CS$<>8__locals1.confirmPurchase = this.onPress;
		string id = this.data.InspirationData.id;
		if (!this.data.InspirationData.IsCompleted)
		{
			return;
		}
		if (this.data.InspirationData.isAllLevelsBought)
		{
			return;
		}
		int completionExp = GameBalance.Me.GetData<InspirationDef>(string.Format("{0}_{1}", this.data.InspirationData.id, this.data.CurrentLevel)).completionExp;
		int price = this.data.Price;
		string text = this.idLabel.text;
		string text2 = ((this.currentViewData != null) ? string.Format("{0}{1}", this.currentViewData.pointIconId.FontIcon(), completionExp) : completionExp.ToString());
		CS$<>8__locals1.widgetToLock = this.FinishFlyAndLock(componentInParent, id);
		CS$<>8__locals1.infoWindow = LazyUI.GetWindow<UIDialogWindow>();
		string text3 = LLBase.L("complete_insp_for", string.Format("{0}{1}", "faith".FontIcon(), price));
		if (completionExp != 0)
		{
			text3 = text3 + "\n" + LLBase.L("complete_insp_you_get", text2);
		}
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData(text, text3, new UIDialogWindowData.ButtonData(new Action(CS$<>8__locals1.<OnPress>g__OnConfirm|0), LLBase.L("btn_yes"), null, true, GameKey.Select, ""));
		uidialogWindowData.CloseButtonAction = new Action(CS$<>8__locals1.<OnPress>g__OnCancel|1);
		CS$<>8__locals1.infoWindow.Open(uidialogWindowData);
	}

	// Token: 0x06003D53 RID: 15699 RVA: 0x00125609 File Offset: 0x00123809
	private InspirationWidget FinishFlyAndLock(CharInspirationPageWidget page, string inspirationId)
	{
		if (page != null)
		{
			page.CompleteFlyingExpAnimationImmediately();
		}
		InspirationWidget inspirationWidget = ((page != null) ? page.FindDisplayedInspiration(inspirationId) : this);
		if (inspirationWidget == null)
		{
			return inspirationWidget;
		}
		inspirationWidget.LockBuyButton();
		return inspirationWidget;
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00125633 File Offset: 0x00123833
	public void LockBuyButton()
	{
		this.isBuyLocked = true;
		this.SetBuyButtonInteractable(false);
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x00125643 File Offset: 0x00123843
	public void UnlockBuyButton()
	{
		this.isBuyLocked = false;
		InspirationWidgetData data = this.data;
		this.SetBuyButtonInteractable(((data != null) ? data.InspirationData : null) != null && this.data.InspirationData.IsAvailableToBuy);
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x00125679 File Offset: 0x00123879
	private void SetBuyButtonInteractable(bool interactable)
	{
		if (this.button != null)
		{
			this.button.interactable = interactable;
		}
	}

	// Token: 0x06003D57 RID: 15703 RVA: 0x00125698 File Offset: 0x00123898
	public void ShowExpTooltip()
	{
		if (this.data == null || this.data.InspirationData == null)
		{
			return;
		}
		if (GameBalance.Me.GetData<InspirationDef>(string.Format("{0}_{1}", this.data.InspirationData.id, this.data.CurrentLevel)).completionExp == 0 || string.IsNullOrEmpty(this.data.TalentExpPointIconId))
		{
			return;
		}
		UITooltip.ShowSimpleInfo(this.expPointTooltipRect.transform, LLBase.L("hint_insp_reward", this.data.TalentExpPointIconId.FontIcon()), default(Vector2), null);
	}

	// Token: 0x06003D58 RID: 15704 RVA: 0x0012573D File Offset: 0x0012393D
	public void HideExpTooltip()
	{
		if (UITooltip.IsTooltipShowingAtTarget(this.expPointTooltipRect))
		{
			UITooltip.Hide();
		}
	}

	// Token: 0x06003D59 RID: 15705 RVA: 0x00125751 File Offset: 0x00123951
	private void OnDisable()
	{
		if (UITooltip.IsTooltipShowingAtTarget(this.expPointTooltipRect))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x06003D5A RID: 15706 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003019 RID: 12313
	[SerializeField]
	private TextMeshProUGUI idLabel;

	// Token: 0x0400301A RID: 12314
	[SerializeField]
	private TextStyleComponent idStyleComponent;

	// Token: 0x0400301B RID: 12315
	[SerializeField]
	private TextStyle idStyleCompleted;

	// Token: 0x0400301C RID: 12316
	[SerializeField]
	private TextStyle idStyleNotCompleted;

	// Token: 0x0400301D RID: 12317
	[SerializeField]
	private TextMeshProUGUI descriptionLabel;

	// Token: 0x0400301E RID: 12318
	[SerializeField]
	private TextMeshProUGUI buyExp;

	// Token: 0x0400301F RID: 12319
	[SerializeField]
	private Image icon;

	// Token: 0x04003020 RID: 12320
	[SerializeField]
	private Image iconFrame;

	// Token: 0x04003021 RID: 12321
	[SerializeField]
	private Color iconOutlineColor;

	// Token: 0x04003022 RID: 12322
	[SerializeField]
	private TextMeshProUGUI priceLabel;

	// Token: 0x04003023 RID: 12323
	[SerializeField]
	private TextStyleComponent priceLabelStyle;

	// Token: 0x04003024 RID: 12324
	[SerializeField]
	private TextStyle priceLabelEnoughStyle;

	// Token: 0x04003025 RID: 12325
	[SerializeField]
	private TextStyle priceLabelNotEnoughStyle;

	// Token: 0x04003026 RID: 12326
	[SerializeField]
	private Slider progressBar;

	// Token: 0x04003027 RID: 12327
	[SerializeField]
	private TextMeshProUGUI progressLabel;

	// Token: 0x04003028 RID: 12328
	[SerializeField]
	private Sprite[] framesSprites;

	// Token: 0x04003029 RID: 12329
	[SerializeField]
	private Image background;

	// Token: 0x0400302A RID: 12330
	[SerializeField]
	private Sprite backCompletedSprite;

	// Token: 0x0400302B RID: 12331
	[SerializeField]
	private Sprite backNotCompletedSprite;

	// Token: 0x0400302C RID: 12332
	[SerializeField]
	private RectTransform expPointTooltipRect;

	// Token: 0x0400302D RID: 12333
	[SerializeField]
	private LazyButton button;

	// Token: 0x0400302E RID: 12334
	[SerializeField]
	private LocalizedLabel buyLocalizedLabel;

	// Token: 0x0400302F RID: 12335
	[SerializeField]
	private TMP_Text getCenteredLabel;

	// Token: 0x04003030 RID: 12336
	[SerializeField]
	private GameObject visualSeparator;

	// Token: 0x04003031 RID: 12337
	private Action onPress;

	// Token: 0x04003032 RID: 12338
	[SerializeField]
	private List<InspirationWidget.TalentViewData> viewDatas = new List<InspirationWidget.TalentViewData>();

	// Token: 0x04003033 RID: 12339
	private InspirationWidget.TalentViewData currentViewData;

	// Token: 0x04003034 RID: 12340
	private int displayedCompletionExp;

	// Token: 0x04003035 RID: 12341
	private bool isBuyLocked;

	// Token: 0x02000917 RID: 2327
	[Serializable]
	private class TalentViewData
	{
		// Token: 0x04003036 RID: 12342
		public string talentId;

		// Token: 0x04003037 RID: 12343
		public string pointIconId;
	}
}
