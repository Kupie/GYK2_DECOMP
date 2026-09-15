using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000929 RID: 2345
public class TalentLevelUpWidget : LazyWidget<TalentLevelUpWidgetData>
{
	// Token: 0x1700094F RID: 2383
	// (get) Token: 0x06003DCB RID: 15819 RVA: 0x00127630 File Offset: 0x00125830
	public TalentLevelUpWidgetData Data
	{
		get
		{
			return this.data;
		}
	}

	// Token: 0x06003DCC RID: 15820 RVA: 0x00127638 File Offset: 0x00125838
	public override void Redraw()
	{
		base.Redraw();
		for (int i = 0; i < this.icons.Length; i++)
		{
			this.icons[i].sprite = this.data.Def.Icon;
			this.icons[i].gameObject.SetActive(this.icons[i].sprite != null);
			this.dbgImageTexts[i].text = this.data.Def.id;
			this.dbgImageTexts[i].gameObject.SetActive(this.icons[i].sprite == null);
		}
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		this.button.interactable = false;
		this.unknownState.SetActive(false);
		this.visibleState.SetActive(false);
		this.availableState.SetActive(false);
		this.unlockedState.SetActive(false);
		this.redSkullObject.gameObject.SetActive(false);
		this.inactiveBorder.gameObject.SetActive(false);
		this.activeImage.material = this.activeMaterial;
		TalentData talentData;
		bool flag = (this.data.Def.isZombiePerk ? (this.data.ZombieWgoData.IsParentsUnlockedForTalentLevelUp(this.data.Def) && this.data.ZombieWgoData.IsEnoughResourcesToBuyTalentLevelUp(this.data.Def)) : MainGame.Instance.GameSave.talentSystemData.CanPurchaseLevel(this.data.Def.id, out talentData));
		for (int j = 0; j < this.priceLabels.Length; j++)
		{
			if (this.data.Def.isZombiePerk)
			{
				this.priceLabels[j].text = string.Format("{0}{1}", this.data.Def.ZombieTechPointsIcon, this.data.Def.ZombieTechPoints);
				this.priceLabels[j].rectTransform.anchoredPosition = this.zombiePos;
			}
			else
			{
				this.priceLabels[j].text = string.Format("{0}{1}", this.data.PriceIconId.FontIcon(), this.data.Def.talentExpPointsPrice);
				this.priceLabels[j].rectTransform.anchoredPosition = this.playerPos;
			}
			if (flag)
			{
				this.enoughStyle.ApplyStyle(this.priceLabels[j], false, null, null, null);
			}
			else
			{
				this.notEnoughStyle.ApplyStyle(this.priceLabels[j], false, null, null, null);
			}
		}
		switch (this.data.State)
		{
		case TalentLevelUpDef.State.Unknown:
			this.unknownState.SetActive(true);
			break;
		case TalentLevelUpDef.State.Visible:
			this.button.interactable = true;
			this.visibleState.SetActive(true);
			break;
		case TalentLevelUpDef.State.Available:
			this.button.interactable = true;
			this.availableState.SetActive(true);
			break;
		case TalentLevelUpDef.State.Unlocked:
			this.button.interactable = true;
			this.unlockedState.SetActive(true);
			this.activeBorder.SetActive(true);
			break;
		}
		base.transform.localPosition = this.data.localPosition;
		base.gameObject.SetActive(true);
		if (this.canBuyEffect != null)
		{
			this.canBuyEffect.SetActive(this.data.State == TalentLevelUpDef.State.Available && !this.data.Def.isZombiePerk);
		}
		if (this.data.Def.isZombiePerk)
		{
			if (this.data.ZombieWgoData.IsTalentLevelUpStudied(this.data.Def.id))
			{
				this.redSkullObject.gameObject.SetActive(true);
				this.redSkullObject.text = "rskull-zombie_window-active_prk".FontIcon();
			}
			if (this.data.ZombieWgoData.disabledTalentLevelUps.Contains(this.data.Def.id))
			{
				this.activeBorder.SetActive(false);
				this.inactiveBorder.gameObject.SetActive(true);
				this.activeImage.material = this.inactiveMaterial;
				this.redSkullObject.text = "rskull-zombie_window-inactive_perk".FontIcon() + "-1";
			}
		}
		if (this.availableGreenFrame != null)
		{
			this.availableGreenFrame.SetActive(this.data.Def.isZombiePerk);
		}
	}

	// Token: 0x06003DCD RID: 15821 RVA: 0x00127B2C File Offset: 0x00125D2C
	public void ClearCallbacks()
	{
		this.onPress = null;
		this.onOver = null;
		this.onOut = null;
	}

	// Token: 0x06003DCE RID: 15822 RVA: 0x00127B44 File Offset: 0x00125D44
	private void Awake()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnPress));
		this.button.onEnter.AddListener(new UnityAction(this.OnOver));
		this.button.onExit.AddListener(new UnityAction(this.OnOut));
		this.selectionFrame.SetActive(false);
	}

	// Token: 0x06003DCF RID: 15823 RVA: 0x00127BB4 File Offset: 0x00125DB4
	private void OnPress()
	{
		if (this.data.State != TalentLevelUpDef.State.Available)
		{
			if (this.data.State == TalentLevelUpDef.State.Visible && !this.data.Def.isZombiePerk)
			{
				this.ShowCantPurchaseDialog();
			}
			return;
		}
		Action action = this.onPress;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003DD0 RID: 15824 RVA: 0x00127C08 File Offset: 0x00125E08
	private void ShowCantPurchaseDialog()
	{
		TalentData talentBranch = MainGame.Instance.GameSave.talentSystemData.GetTalentBranch(this.data.Def.talentId);
		string text = "";
		if (talentBranch.talentExpPoints < this.data.Def.talentExpPointsPrice)
		{
			text = LLBase.L("ui_dont_have_points", this.data.PriceIconId.FontIcon());
		}
		if (!this.data.Def.ParentsUnlocked)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			text += LLBase.L("ui_previous_perk_locked");
		}
		UIDialogWindowData uidialogWindowData = new UIDialogWindowData(this.GetDialogHeader(), text, new UIDialogWindowData.ButtonData(new Action(LazyUI.GetWindow<UIDialogWindow>().Close), LLBase.L("btn_ok"), null, true, GameKey.Select, ""));
		LazyUI.GetWindow<UIDialogWindow>().Open(uidialogWindowData);
	}

	// Token: 0x06003DD1 RID: 15825 RVA: 0x00127CEC File Offset: 0x00125EEC
	private string GetDialogHeader()
	{
		if (!string.IsNullOrEmpty(this.data.Def.linkedPerk))
		{
			PerkDef data = GameBalance.Me.GetData<PerkDef>(this.data.Def.linkedPerk);
			if (data != null)
			{
				return data.GetHeader();
			}
		}
		return LLBase.L(this.data.Def.id);
	}

	// Token: 0x06003DD2 RID: 15826 RVA: 0x00127D4A File Offset: 0x00125F4A
	private void OnOver()
	{
		this.selectionFrame.SetActive(true);
		UITooltip.ShowTalentLevelUpWidget(this);
		Action action = this.onOver;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003DD3 RID: 15827 RVA: 0x00127D6E File Offset: 0x00125F6E
	private void OnOut()
	{
		this.selectionFrame.SetActive(false);
		UITooltip.Hide();
		Action action = this.onOut;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06003DD4 RID: 15828 RVA: 0x00127D91 File Offset: 0x00125F91
	private void OnDisable()
	{
		this.selectionFrame.SetActive(false);
		if (UITooltip.IsTooltipShowingAtTarget(base.transform as RectTransform))
		{
			UITooltip.HideImmediately();
		}
	}

	// Token: 0x06003DD5 RID: 15829 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003098 RID: 12440
	[SerializeField]
	private LazyButton button;

	// Token: 0x04003099 RID: 12441
	[SerializeField]
	private Image[] icons;

	// Token: 0x0400309A RID: 12442
	[SerializeField]
	private TextMeshProUGUI[] dbgImageTexts;

	// Token: 0x0400309B RID: 12443
	[SerializeField]
	private GameObject selectionFrame;

	// Token: 0x0400309C RID: 12444
	[SerializeField]
	private Vector2 zombiePos;

	// Token: 0x0400309D RID: 12445
	[SerializeField]
	private Vector2 playerPos;

	// Token: 0x0400309E RID: 12446
	[SerializeField]
	private TextMeshProUGUI[] priceLabels;

	// Token: 0x0400309F RID: 12447
	[SerializeField]
	private TextStyle enoughStyle;

	// Token: 0x040030A0 RID: 12448
	[SerializeField]
	private TextStyle notEnoughStyle;

	// Token: 0x040030A1 RID: 12449
	[SerializeField]
	private GameObject availableGreenFrame;

	// Token: 0x040030A2 RID: 12450
	[SerializeField]
	private GameObject unknownState;

	// Token: 0x040030A3 RID: 12451
	[SerializeField]
	private GameObject visibleState;

	// Token: 0x040030A4 RID: 12452
	[SerializeField]
	private GameObject availableState;

	// Token: 0x040030A5 RID: 12453
	[SerializeField]
	private GameObject unlockedState;

	// Token: 0x040030A6 RID: 12454
	[SerializeField]
	private TextMeshProUGUI redSkullObject;

	// Token: 0x040030A7 RID: 12455
	[SerializeField]
	private GameObject activeBorder;

	// Token: 0x040030A8 RID: 12456
	[SerializeField]
	private GameObject inactiveBorder;

	// Token: 0x040030A9 RID: 12457
	[SerializeField]
	private Image activeImage;

	// Token: 0x040030AA RID: 12458
	[SerializeField]
	private Material inactiveMaterial;

	// Token: 0x040030AB RID: 12459
	[SerializeField]
	private Material activeMaterial;

	// Token: 0x040030AC RID: 12460
	[SerializeField]
	private GameObject canBuyEffect;

	// Token: 0x040030AD RID: 12461
	private Action onPress;

	// Token: 0x040030AE RID: 12462
	private Action onOver;

	// Token: 0x040030AF RID: 12463
	private Action onOut;
}
