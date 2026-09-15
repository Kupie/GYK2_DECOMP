using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A0D RID: 2573
public class UIPrayReportWindow : LazyWindow<UIPrayReportWindowData>
{
	// Token: 0x06004534 RID: 17716 RVA: 0x0014743C File Offset: 0x0014563C
	public override void Init()
	{
		base.Init();
		this.okButton.onClick.AddListener(new UnityAction(this.Close));
	}

	// Token: 0x06004535 RID: 17717 RVA: 0x00147464 File Offset: 0x00145664
	public override void Redraw()
	{
		base.Redraw();
		this.prayerCell.Draw(new Item(this.data.SermonResultData.Definition.id, 1), ItemRelatedWidgetState.NotSet, true);
		int moneyOnlyParishioners = this.data.SermonResultData.MoneyOnlyParishioners;
		int faithOnlyParishioners = this.data.SermonResultData.FaithOnlyParishioners;
		this.visitorsLabel.text = string.Format("{0}{1}", "happiness_cross".FontIcon(), this.data.SermonResultData.parishionersCount);
		if (faithOnlyParishioners > 0)
		{
			this.faithLabel.text = string.Format("{0}{1}", "faith".FontIcon(), faithOnlyParishioners);
			this.faithLabel.transform.parent.parent.gameObject.SetActive(true);
		}
		else
		{
			this.faithLabel.transform.parent.parent.gameObject.SetActive(false);
		}
		if (moneyOnlyParishioners > 0)
		{
			this.moneyLabel.text = Trading.FormatMoney(moneyOnlyParishioners, false, " ", GameResIconType.MoneyBig);
			this.moneyLabel.transform.parent.parent.gameObject.SetActive(true);
		}
		else
		{
			this.moneyLabel.transform.parent.parent.gameObject.SetActive(false);
		}
		foreach (UIItemCell uiitemCell in this.rewardCells)
		{
			uiitemCell.gameObject.SetActive(false);
		}
		this.itemsBlock.gameObject.SetActive(false);
		if (this.data.SermonResultData.success)
		{
			this.rewardBlock.gameObject.SetActive(true);
			this.topLabel.text = LLBase.L("ui_pray_success_text");
			this.topLabelSuccessStyle.ApplyStyle(this.topLabel, false, null, null, null);
			int faithOnlyBonus = this.data.SermonResultData.FaithOnlyBonus;
			int moneyOnlyBonus = this.data.SermonResultData.MoneyOnlyBonus;
			if (faithOnlyBonus > 0)
			{
				this.faithBonusLabel.text = string.Format("{0}{1}", "faith".FontIcon(), faithOnlyBonus);
				this.faithBonusLabel.transform.parent.parent.gameObject.SetActive(true);
			}
			else
			{
				this.faithBonusLabel.transform.parent.parent.gameObject.SetActive(false);
			}
			if (moneyOnlyBonus > 0)
			{
				this.moneyBonusLabel.text = Trading.FormatMoney(moneyOnlyBonus, false, " ", GameResIconType.MoneyBig);
				this.moneyBonusLabel.transform.parent.parent.gameObject.SetActive(true);
			}
			else
			{
				this.moneyBonusLabel.transform.parent.parent.gameObject.SetActive(false);
			}
			bool flag = false;
			bool flag2 = false;
			if (!string.IsNullOrEmpty(this.data.SermonResultData.Definition.successRewardBuff))
			{
				this.givenBuff.Draw(new PerkWidgetData(new PerkData(this.data.SermonResultData.Definition.successRewardBuff), true, null, null, null));
				this.givenBuff.gameObject.SetActive(true);
				flag = true;
			}
			else
			{
				this.givenBuff.gameObject.SetActive(false);
			}
			WgoData wgoData = MainGame.Instance.GameSave.worldData.GetWgoData(this.data.SermonResultData.sermonConfigId);
			List<Item> list = OutputItems.MakeOutput(this.data.SermonResultData.Definition.successRewardItem.MakePreOutput(wgoData, 0f));
			for (int i = 0; i < list.Count; i++)
			{
				flag2 = true;
				this.rewardCells[i].Draw(list[i], false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
				this.rewardCells[i].gameObject.SetActive(true);
			}
			this.itemsBlock.gameObject.SetActive(flag2 || flag);
		}
		else
		{
			this.rewardBlock.gameObject.SetActive(false);
			this.topLabel.text = LLBase.L("ui_pray_fail_text");
			this.topLabelFailStyle.ApplyStyle(this.topLabel, false, null, null, null);
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004536 RID: 17718 RVA: 0x00147914 File Offset: 0x00145B14
	protected override void HideWindow()
	{
		UIPrayReportWindowData data = this.data;
		if (data != null)
		{
			data.HandleClosingWindow();
		}
		base.HideWindow();
	}

	// Token: 0x06004537 RID: 17719 RVA: 0x0014792D File Offset: 0x00145B2D
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, delegate
		{
			this.Close();
			return true;
		});
		return gameKeyDelegates;
	}

	// Token: 0x06004538 RID: 17720 RVA: 0x0014794C File Offset: 0x00145B4C
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
		this.gameKeyTipLabel.text = new LazyGameKeyTip(GameKey.Select, LLBase.L("btn_ok"), true, true, true).ToString();
	}

	// Token: 0x06004539 RID: 17721 RVA: 0x00147980 File Offset: 0x00145B80
	[LazyUITest]
	protected override void TestDraw()
	{
		UIPrayReportWindowData uiprayReportWindowData = new UIPrayReportWindowData(new SermonResultData("prayer_base:1", "church_tribune", 5, true, 5, 4), null);
		LazyUI.GetWindow<UIPrayReportWindow>().Open(uiprayReportWindowData);
	}

	// Token: 0x0600453A RID: 17722 RVA: 0x001479B4 File Offset: 0x00145BB4
	[LazyUITest]
	protected void TestDraw2()
	{
		UIPrayReportWindowData uiprayReportWindowData = new UIPrayReportWindowData(new SermonResultData("prayer_faith:2", "church_tribune", 11, true, 11, 55), null);
		LazyUI.GetWindow<UIPrayReportWindow>().Open(uiprayReportWindowData);
	}

	// Token: 0x0600453B RID: 17723 RVA: 0x001479EC File Offset: 0x00145BEC
	[LazyUITest]
	protected void TestDraw3()
	{
		UIPrayReportWindowData uiprayReportWindowData = new UIPrayReportWindowData(new SermonResultData("prayer_harvest:1", "church_tribune", 2, true, 2, 55), null);
		LazyUI.GetWindow<UIPrayReportWindow>().Open(uiprayReportWindowData);
	}

	// Token: 0x0600453C RID: 17724 RVA: 0x00147A20 File Offset: 0x00145C20
	[LazyUITest]
	protected void TestDrawFail()
	{
		UIPrayReportWindowData uiprayReportWindowData = new UIPrayReportWindowData(new SermonResultData("prayer_combat:2", "church_tribune", 11, false, 11, 55), null);
		LazyUI.GetWindow<UIPrayReportWindow>().Open(uiprayReportWindowData);
	}

	// Token: 0x04003607 RID: 13831
	[SerializeField]
	private LazyButton okButton;

	// Token: 0x04003608 RID: 13832
	[SerializeField]
	private UIFixedTypeItemCell prayerCell;

	// Token: 0x04003609 RID: 13833
	[SerializeField]
	private TextMeshProUGUI topLabel;

	// Token: 0x0400360A RID: 13834
	[SerializeField]
	private TextStyle topLabelSuccessStyle;

	// Token: 0x0400360B RID: 13835
	[SerializeField]
	private TextStyle topLabelFailStyle;

	// Token: 0x0400360C RID: 13836
	[SerializeField]
	private TextMeshProUGUI visitorsLabel;

	// Token: 0x0400360D RID: 13837
	[SerializeField]
	private TextMeshProUGUI faithLabel;

	// Token: 0x0400360E RID: 13838
	[SerializeField]
	private TextMeshProUGUI moneyLabel;

	// Token: 0x0400360F RID: 13839
	[SerializeField]
	private TextMeshProUGUI faithBonusLabel;

	// Token: 0x04003610 RID: 13840
	[SerializeField]
	private TextMeshProUGUI moneyBonusLabel;

	// Token: 0x04003611 RID: 13841
	[SerializeField]
	private GameObject rewardBlock;

	// Token: 0x04003612 RID: 13842
	[SerializeField]
	private GameObject itemsBlock;

	// Token: 0x04003613 RID: 13843
	[SerializeField]
	private PerkWidget givenBuff;

	// Token: 0x04003614 RID: 13844
	[SerializeField]
	private List<UIItemCell> rewardCells = new List<UIItemCell>();

	// Token: 0x04003615 RID: 13845
	[SerializeField]
	private TextMeshProUGUI gameKeyTipLabel;
}
