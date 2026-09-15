using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A11 RID: 2577
public class UIPrayWindow : LazyWindow<UIPrayWindowData>
{
	// Token: 0x0600454D RID: 17741 RVA: 0x00147EC0 File Offset: 0x001460C0
	public override void Init()
	{
		base.Init();
		this.startButton.onDown.AddListener(new UnityAction(this.OnStartButtonPress));
		GameObject gameObject = this.churchQualityLabel.gameObject;
		string text = "tt_pray_1";
		RectTransform rectTransform = null;
		bool flag = true;
		bool flag2 = false;
		Vector2 vector = new Vector2(0f, -2f);
		UIMouseTooltip.Attach(gameObject, text, rectTransform, flag, flag2, default(UIMouseTooltipEdges), vector, null);
		GameObject gameObject2 = this.smilesCountLabel.gameObject;
		string text2 = "tt_town_happiness";
		RectTransform rectTransform2 = null;
		bool flag3 = true;
		bool flag4 = false;
		vector = new Vector2(0f, -2f);
		UIMouseTooltip.Attach(gameObject2, text2, rectTransform2, flag3, flag4, default(UIMouseTooltipEdges), vector, null);
		GameObject gameObject3 = this.resultVisitorsLabel.gameObject;
		string text3 = "tt_pray_3";
		RectTransform rectTransform3 = null;
		bool flag5 = true;
		bool flag6 = false;
		vector = new Vector2(0f, -2f);
		UIMouseTooltip.Attach(gameObject3, text3, rectTransform3, flag5, flag6, default(UIMouseTooltipEdges), vector, null);
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x00147F90 File Offset: 0x00146190
	public override void Redraw()
	{
		this.churchQualityLabel.text = "cross".FontIcon() + this.data.ChurchQuality.ToString();
		this.smilesCountLabel.text = string.Format("{0}{1}", "happiness".FontIcon(), this.data.Happiness);
		this.resultVisitorsLabel.text = string.Format("{0}{1}", "happiness_cross".FontIcon(), this.data.ResultVisitors);
		if (this.data.SermonDef == null)
		{
			this.notEnoughParishionersLabel.gameObject.SetActive(false);
			this.topLabel.text = LLBase.L("ui_choose_pray");
			this.topLabelStyleInactive.ApplyStyle(this.topLabel, false, null, null, null);
			this.prayInsertionCell.DrawEmptyInteractable();
			GameObject[] array = this.activateWhenNoPray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			array = this.activateWhenYesPray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
		}
		else
		{
			this.chance = Math.Clamp((int)((float)this.data.ResultVisitors * 100f / (float)this.data.SermonDef.sermonDifficulty), 0, 100);
			this.successChanceLabel.text = string.Format("{0}: {1}%", LLBase.L("ui_success_chance"), this.chance);
			this.prayInsertionCell.Draw(new Item(this.data.SermonDef.id, 1), ItemRelatedWidgetState.NotSet, true);
			ItemDef data = GameBalance.Me.GetData<ItemDef>(this.data.SermonDef.id);
			if (data != null)
			{
				this.topLabel.text = data.GetHeader();
			}
			else
			{
				this.topLabel.text = LLBase.L(this.data.SermonDef.id) ?? "";
			}
			this.topLabelStyleActive.ApplyStyle(this.topLabel, false, null, null, null);
			GameObject[] array = this.activateWhenNoPray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			array = this.activateWhenYesPray;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
			bool flag = this.data.EnoughParishioners();
			this.notEnoughParishionersLabel.gameObject.SetActive(!flag);
			this.notEnoughParishionersLabel.text = string.Format("{0}: {1}{2}", LLBase.L("ui_not_enough_parishioners"), "happiness_cross".FontIcon(), this.data.SermonDef.minParishioners);
			if (this.notEnoughParishionersStyle != null)
			{
				this.notEnoughParishionersStyle.ApplyStyle(this.notEnoughParishionersLabel, false, null, null, null);
			}
			this.successChanceLabel.gameObject.SetActive(flag);
		}
		this.prayInsertionCell.UIItemCell.OnItemCellPress = new Action<UIItemCell>(this.HandlePraySlotPress);
		this.CheckCraftCanStart();
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x001482E9 File Offset: 0x001464E9
	public override void Hide()
	{
		if (this.data != null)
		{
			this.data.EraseNonStartedCraft();
		}
		base.Hide();
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x00148304 File Offset: 0x00146504
	private void OnStartButtonPress()
	{
		this.data.StartCraft(this.data.ResultVisitors, this.chance);
		this.Close();
	}

	// Token: 0x06004551 RID: 17745 RVA: 0x00148328 File Offset: 0x00146528
	private void HandlePraySlotPress(UIItemCell cell)
	{
		Action onPraySlotPress = this.data.OnPraySlotPress;
		if (onPraySlotPress == null)
		{
			return;
		}
		onPraySlotPress();
	}

	// Token: 0x06004552 RID: 17746 RVA: 0x0014833F File Offset: 0x0014653F
	private void CheckCraftCanStart()
	{
		this.startButton.interactable = this.data.CanStartCraft();
	}

	// Token: 0x06004553 RID: 17747 RVA: 0x00148357 File Offset: 0x00146557
	private bool OnPrayStartPress()
	{
		if (this.startButton.interactable)
		{
			this.OnStartButtonPress();
			return true;
		}
		return false;
	}

	// Token: 0x06004554 RID: 17748 RVA: 0x0014836F File Offset: 0x0014656F
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.PrayStart, new Func<bool>(this.OnPrayStartPress));
		return gameKeyDelegates;
	}

	// Token: 0x06004555 RID: 17749 RVA: 0x00148390 File Offset: 0x00146590
	protected override void PrintTips()
	{
		this.lazyButtonTips.Print(new LazyGameKeyTip[]
		{
			LazyGameKeyTip.Select(true, true, true),
			LazyGameKeyTip.Back(true, true, true)
		});
		this.gameKeyTipLabel.text = new LazyGameKeyTip(GameKey.PrayStart, LLBase.L("ui_pray_start"), this.startButton.interactable, true, true).ToString();
	}

	// Token: 0x06004556 RID: 17750 RVA: 0x001483F8 File Offset: 0x001465F8
	[LazyUITest]
	protected override void TestDraw()
	{
		SermonConfigDef data = GameBalance.Me.GetData<SermonConfigDef>("church_tribune");
		UIPrayWindowData uiprayWindowData = new UIPrayWindowData(MainGame.PlayerData, MainGame.WorldData.GetWgoData("church_tribune"), 10, 10, 5, data.rewardBoxId);
		LazyUI.GetWindow<UIPrayWindow>().Open(uiprayWindowData);
	}

	// Token: 0x04003628 RID: 13864
	[SerializeField]
	private UIFixedTypeItemCell prayInsertionCell;

	// Token: 0x04003629 RID: 13865
	[SerializeField]
	private TextMeshProUGUI churchQualityLabel;

	// Token: 0x0400362A RID: 13866
	[SerializeField]
	private TextMeshProUGUI smilesCountLabel;

	// Token: 0x0400362B RID: 13867
	[SerializeField]
	private TextMeshProUGUI resultVisitorsLabel;

	// Token: 0x0400362C RID: 13868
	[SerializeField]
	private TextMeshProUGUI successChanceLabel;

	// Token: 0x0400362D RID: 13869
	[SerializeField]
	private TextMeshProUGUI notEnoughParishionersLabel;

	// Token: 0x0400362E RID: 13870
	[SerializeField]
	private TextMeshProUGUI topLabel;

	// Token: 0x0400362F RID: 13871
	[SerializeField]
	private GameObject[] activateWhenNoPray;

	// Token: 0x04003630 RID: 13872
	[SerializeField]
	private GameObject[] activateWhenYesPray;

	// Token: 0x04003631 RID: 13873
	[SerializeField]
	private TextMeshProUGUI gameKeyTipLabel;

	// Token: 0x04003632 RID: 13874
	[SerializeField]
	private TextStyle topLabelStyleActive;

	// Token: 0x04003633 RID: 13875
	[SerializeField]
	private TextStyle topLabelStyleInactive;

	// Token: 0x04003634 RID: 13876
	[SerializeField]
	private TextStyle notEnoughParishionersStyle;

	// Token: 0x04003635 RID: 13877
	[Space]
	[SerializeField]
	private LazyButton startButton;

	// Token: 0x04003636 RID: 13878
	private int chance;
}
