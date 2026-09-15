using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A77 RID: 2679
public class UIVendorOrderWidget : LazyWidget<UIVendorOrderWidgetData>
{
	// Token: 0x17000AF2 RID: 2802
	// (get) Token: 0x060048B0 RID: 18608 RVA: 0x001584CF File Offset: 0x001566CF
	public LazyButton BtnGrey
	{
		get
		{
			return this.btnGrey;
		}
	}

	// Token: 0x17000AF3 RID: 2803
	// (get) Token: 0x060048B1 RID: 18609 RVA: 0x001584D7 File Offset: 0x001566D7
	public UIVendorOrderWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x060048B2 RID: 18610 RVA: 0x001584E0 File Offset: 0x001566E0
	public override void Init()
	{
		if (!this.isInitialized)
		{
			this.isInitialized = true;
			base.Init();
			this.btnGrey.onClick.AddListener(new UnityAction(this.OnPress));
			this.btnGrey.onEnter.AddListener(new UnityAction(this.OnOver));
			this.btnGrey.onExit.AddListener(new UnityAction(this.OnOut));
			this.btnGrey.onNotInteractableEnter.AddListener(new UnityAction(this.OnOver));
			this.btnGrey.onNotInteractableExit.AddListener(new UnityAction(this.OnOut));
			this.btnGreen.onClick.AddListener(new UnityAction(this.OnPress));
			this.btnGreen.onEnter.AddListener(new UnityAction(this.OnOver));
			this.btnGreen.onExit.AddListener(new UnityAction(this.OnOut));
			this.btnGreen.onNotInteractableEnter.AddListener(new UnityAction(this.OnOver));
			this.btnGreen.onNotInteractableExit.AddListener(new UnityAction(this.OnOut));
			this.btnGrey.SetCallbacksIntoGamepadNavigationItem();
			this.btnGreen.SetCallbacksIntoGamepadNavigationItem();
		}
	}

	// Token: 0x060048B3 RID: 18611 RVA: 0x00158634 File Offset: 0x00156834
	public override void Redraw()
	{
		base.Redraw();
		this.tierIcon.gameObject.SetActive(false);
		this.btnGrey.gameObject.SetActive(false);
		this.btnGreen.gameObject.SetActive(false);
		this.decorTop.gameObject.SetActive(true);
		this.decorBot.gameObject.SetActive(true);
		this.btnGreen.interactable = true;
		this.btnGrey.interactable = true;
		if (this.data.IsEmpty)
		{
			this.btnGrey.gameObject.SetActive(true);
			this.completedCheckbox.SetActive(false);
			this.lockedObject.SetActive(false);
			this.urgentObject.SetActive(false);
			this.repeatableObject.gameObject.SetActive(false);
			this.rewardLabel.text = string.Empty;
			this.itemCell.DrawEmptyInteractable(false, false);
			this.plusObj.gameObject.SetActive(true);
		}
		else
		{
			bool flag = this.data.VendorOrderData.State == VendorOrderState.Finished;
			bool flag2 = this.data.Vendor.CurTier < this.data.VendorOrderData.Tier;
			bool isUrgent = this.data.VendorOrderData.Definition.isUrgent;
			bool isRenewable = this.data.VendorOrderData.Definition.isRenewable;
			bool isFinishedOnce = this.data.VendorOrderData.IsFinishedOnce;
			this.repeatableObject.sprite = (isFinishedOnce ? this.repeatableGrey : this.repeatableYellow);
			this.rewardLabel.text = string.Format("{0}+{1}", "happiness".FontIcon(), this.data.VendorOrderData.Definition.happinessReward.EvaluateFloat());
			WorldZoneData worldZoneDataById = MainGame.WorldData.GetWorldZoneDataById("warehouse");
			WorldZoneData worldZoneDataById2 = MainGame.WorldData.GetWorldZoneDataById("warehouse_cellar");
			int num = worldZoneDataById.CountItemsOnTownPalettes(this.data.VendorOrderData.Definition.itemId) + worldZoneDataById2.CountItemsOnTownPalettes(this.data.VendorOrderData.Definition.itemId) + this.data.VendorOrderData.Count;
			this.itemCell.Draw(new Item(this.data.VendorOrderData.Definition.itemId, this.data.VendorOrderData.Definition.count), true, num, false, 1, false, 0, this.data.VendorOrderData.State != VendorOrderState.Finished, false, false, ItemRelatedWidgetState.NotSet, false);
			this.completedCheckbox.SetActive(flag);
			this.repeatableObject.gameObject.SetActive(false);
			if (isRenewable)
			{
				this.repeatableObject.gameObject.SetActive(!flag2);
			}
			if (flag)
			{
				this.lockedObject.SetActive(false);
				this.urgentObject.SetActive(false);
				if (isRenewable)
				{
					if (this.data.IsRenewableGreen)
					{
						this.btnGreen.gameObject.SetActive(true);
					}
					else
					{
						this.btnGrey.gameObject.SetActive(true);
					}
				}
				else
				{
					this.btnGreen.gameObject.SetActive(true);
				}
			}
			else
			{
				this.btnGrey.gameObject.SetActive(true);
				this.lockedObject.SetActive(flag2);
				if (flag2)
				{
					this.rewardLabel.text = string.Empty;
					this.tierIcon.sprite = this.tierIcons[this.data.VendorOrderData.Tier - 1];
					this.tierIcon.gameObject.SetActive(true);
					this.btnGrey.interactable = false;
				}
				this.urgentObject.SetActive(isUrgent && !flag2);
				if (isUrgent)
				{
					this.decorBot.gameObject.SetActive(false);
				}
			}
			this.plusObj.gameObject.SetActive(false);
		}
		if (!string.IsNullOrEmpty(this.rewardLabel.text))
		{
			this.decorTop.gameObject.SetActive(false);
		}
		if (this.data.ForcedInteractableState.Item1)
		{
			this.btnGrey.interactable = (this.btnGreen.interactable = this.data.ForcedInteractableState.Item2);
		}
		this.itemCell.LazyButton.interactable = false;
	}

	// Token: 0x060048B4 RID: 18612 RVA: 0x00158A8C File Offset: 0x00156C8C
	private void OnPress()
	{
		Action<UIVendorOrderWidget> onPress = this.data.OnPress;
		if (onPress == null)
		{
			return;
		}
		onPress(this);
	}

	// Token: 0x060048B5 RID: 18613 RVA: 0x00158AA4 File Offset: 0x00156CA4
	private void OnOver()
	{
		if (this.Data.IsEmpty)
		{
			return;
		}
		if (this.data.Vendor.CurTier < this.data.VendorOrderData.Tier)
		{
			return;
		}
		UITooltip.ShowOrderWidget(this);
	}

	// Token: 0x060048B6 RID: 18614 RVA: 0x001080F0 File Offset: 0x001062F0
	private void OnOut()
	{
		UITooltip.Hide();
	}

	// Token: 0x060048B7 RID: 18615 RVA: 0x00140197 File Offset: 0x0013E397
	private void OnDisable()
	{
		if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x060048B8 RID: 18616 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x040038AD RID: 14509
	[SerializeField]
	private LazyButton btnGrey;

	// Token: 0x040038AE RID: 14510
	[SerializeField]
	private LazyButton btnGreen;

	// Token: 0x040038AF RID: 14511
	[SerializeField]
	private UIItemCell itemCell;

	// Token: 0x040038B0 RID: 14512
	[SerializeField]
	private GameObject decorTop;

	// Token: 0x040038B1 RID: 14513
	[SerializeField]
	private GameObject decorBot;

	// Token: 0x040038B2 RID: 14514
	[SerializeField]
	private GameObject completedCheckbox;

	// Token: 0x040038B3 RID: 14515
	[SerializeField]
	private GameObject lockedObject;

	// Token: 0x040038B4 RID: 14516
	[SerializeField]
	private GameObject urgentObject;

	// Token: 0x040038B5 RID: 14517
	[SerializeField]
	private Image repeatableObject;

	// Token: 0x040038B6 RID: 14518
	[SerializeField]
	private Sprite repeatableGrey;

	// Token: 0x040038B7 RID: 14519
	[SerializeField]
	private Sprite repeatableYellow;

	// Token: 0x040038B8 RID: 14520
	[SerializeField]
	private TextMeshProUGUI rewardLabel;

	// Token: 0x040038B9 RID: 14521
	[SerializeField]
	private GameObject plusObj;

	// Token: 0x040038BA RID: 14522
	[SerializeField]
	private Sprite[] tierIcons;

	// Token: 0x040038BB RID: 14523
	[SerializeField]
	private Image tierIcon;

	// Token: 0x040038BC RID: 14524
	private bool isInitialized;
}
