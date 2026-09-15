using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A7E RID: 2686
public class UIZombieWorkerWindow : LazyWindow<UIZombieWorkerWindowData>
{
	// Token: 0x0600491E RID: 18718 RVA: 0x00159CB8 File Offset: 0x00157EB8
	public override void Init()
	{
		base.Init();
		this.characterTabButton.Init(new Action(this.RedrawCharacterTab));
		this.perksTabButton.Init(new Action(this.RedrawPerksTab));
		UIZombieWorkerWindowData.OnBodyItemAddOrRemove += this.RedrawTalentIcons;
		UIZombieWorkerWindowData.OnBodyItemAddOrRemove += this.RedrawMilitaryBaseWorldZoneWidget;
		ZombieWgoData.OnTalentLevelUpPurchased += this.OnTalentLevelUpPurchased;
		ZombieWgoData.OnTechPointsAddedToZombie += this.OnTechPointsChanged;
		UIMouseTooltip.Attach(this.talentWidgets[0].transform.parent.gameObject, "tt_zombie_mastery_levels", null, true, false, new UIMouseTooltipEdges(0f, 0f, 10f, 12f), default(Vector2), null);
		UIMouseTooltip.Attach(this.redSpheresLabel.transform.parent.gameObject, "tt_zombie_2", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
		UIMouseTooltip.Attach(this.whiteSkullsLabel.transform.parent.gameObject, "tt_zombie_3", null, true, true, new UIMouseTooltipEdges(0f, 0f, 19f, 20f), default(Vector2), null);
	}

	// Token: 0x0600491F RID: 18719 RVA: 0x00159E04 File Offset: 0x00158004
	public override void Redraw()
	{
		base.Redraw();
		this.characterTabButton.UpdateText(LLBase.L("ui_zombie_wndw_char"));
		this.RedrawWorkerIcon();
		this.RedrawTalentIcons();
		this.RedrawSkulls();
		this.RedrawPerksTabLabel();
		this.RedrawName();
		this.RedrawSpheres();
		if (UIZombieWorkerWindow.lastOpenedPerksTab)
		{
			this.RedrawPerksTab();
		}
		else
		{
			this.RedrawCharacterTab();
		}
		this.contentCanvas.sortingOrder = this.canvas.sortingOrder + 5;
	}

	// Token: 0x06004920 RID: 18720 RVA: 0x00159E7D File Offset: 0x0015807D
	public override void Open(UIZombieWorkerWindowData data)
	{
		base.Open(data);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004921 RID: 18721 RVA: 0x00159E9B File Offset: 0x0015809B
	public override void Hide()
	{
		this.zombieProgressionWidget.Hide();
		this.bodyOrgansInventoryWidget.Hide();
		this.bodyPocketInventoryWidget.Hide();
		this.zombieEquipmentInventoryWidget.Hide();
		base.Hide();
	}

	// Token: 0x06004922 RID: 18722 RVA: 0x00159ED0 File Offset: 0x001580D0
	private void RedrawCharacterTab()
	{
		this.mainTabWidget.SetActive(true);
		this.zombieProgressionWidget.gameObject.SetActive(false);
		this.bodyOrgansInventoryWidget.Draw(this.data.BodyOrgansInventoryWidgetData);
		this.bodyPocketInventoryWidget.Draw(this.data.BodyPocketInventoryWidgetData);
		this.zombieEquipmentInventoryWidget.Draw(this.data.ZombieEquipmentInventoryWidgetData);
		this.characterTabButton.UpdateState(true, base.Canvas);
		this.perksTabButton.UpdateState(false, base.Canvas);
		UIZombieWorkerWindow.lastOpenedPerksTab = false;
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004923 RID: 18723 RVA: 0x00159F78 File Offset: 0x00158178
	private void RedrawPerksTab()
	{
		this.mainTabWidget.SetActive(false);
		this.zombieProgressionWidget.gameObject.SetActive(true);
		this.characterTabButton.UpdateState(false, base.Canvas);
		this.perksTabButton.UpdateState(true, base.Canvas);
		this.zombieProgressionWidget.Draw(this.data.ZombieProgressionWidgetData);
		UIZombieWorkerWindow.lastOpenedPerksTab = true;
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004924 RID: 18724 RVA: 0x00159FF2 File Offset: 0x001581F2
	private void OnTalentLevelUpPurchased()
	{
		if (!base.IsShown)
		{
			return;
		}
		this.RedrawPerksTabLabel();
		this.RedrawSpheres();
		this.RedrawTalentIcons();
	}

	// Token: 0x06004925 RID: 18725 RVA: 0x0015A00F File Offset: 0x0015820F
	private void OnTechPointsChanged(WgoData wgoData, ZombieWgoData zombieWgoData, string type, int value)
	{
		if (!base.IsShown)
		{
			return;
		}
		if (zombieWgoData == this.data.ZombieWgoData)
		{
			this.RedrawSpheres();
			if (this.zombieProgressionWidget.gameObject.activeSelf)
			{
				this.zombieProgressionWidget.Redraw();
			}
		}
	}

	// Token: 0x06004926 RID: 18726 RVA: 0x0015A04C File Offset: 0x0015824C
	private void RedrawSpheres()
	{
		this.redSpheresLabel.text = string.Format("{0}{1}", "tech_red".FontIcon(), this.data.ZombieWgoData.techRed);
		this.greenSpheresLabel.text = string.Format("{0}{1}", "tech_green".FontIcon(), this.data.ZombieWgoData.techGreen);
		this.blueSpheresLabel.text = string.Format("{0}{1}", "tech_blue".FontIcon(), this.data.ZombieWgoData.techBlue);
	}

	// Token: 0x06004927 RID: 18727 RVA: 0x0015A0F8 File Offset: 0x001582F8
	private void RedrawSkulls()
	{
		this.whiteSkullsLabel.text = string.Format("{0}{1}", "skull-zombie_window".FontIcon(), this.data.ZombieWgoData.WhiteSkulls);
		this.redSkullsLabel.text = string.Format("{0}{1}", "rskull-zombie_window".FontIcon(), this.data.ZombieWgoData.RedSkulls);
	}

	// Token: 0x06004928 RID: 18728 RVA: 0x0015A170 File Offset: 0x00158370
	private void RedrawTalentIcons()
	{
		if (!base.IsShown)
		{
			return;
		}
		for (int i = 0; i < this.talentWidgets.Length; i++)
		{
			this.talentWidgets[i].Draw(new TalentWidgetData(this.talents[i], this.data.ZombieWgoData.GetMasteryLevelForTalentBranch(this.talents[i], null)));
		}
	}

	// Token: 0x06004929 RID: 18729 RVA: 0x0015A1D4 File Offset: 0x001583D4
	private void RedrawMilitaryBaseWorldZoneWidget()
	{
		if (MainGame.Instance.GameSave.militaryBaseData.ContainsFighter(this.data.ZombieWgoData, false))
		{
			WorldZoneData worldZoneDataById = MainGame.Instance.GameSave.WorldData.GetWorldZoneDataById("town_guard_barracks");
			if (worldZoneDataById == null)
			{
				return;
			}
			worldZoneDataById.NotifyWgoDataChanged();
		}
	}

	// Token: 0x0600492A RID: 18730 RVA: 0x0015A226 File Offset: 0x00158426
	private void RedrawName()
	{
		this.nameLabel.text = LLBase.L(this.data.ZombieWgoData.Name);
	}

	// Token: 0x0600492B RID: 18731 RVA: 0x0015A248 File Offset: 0x00158448
	private void RedrawWorkerIcon()
	{
		this.workerIcon.ShowWithoutTalent(this.data.ZombieWgoData, ZombieSkinHelper.GetPresetForWgoData(this.data.ZombieWgoData, "zombie_worker"));
	}

	// Token: 0x0600492C RID: 18732 RVA: 0x0015A278 File Offset: 0x00158478
	private void RedrawPerksTabLabel()
	{
		string text = this.perksSkullsStyle.ApplyStyleToString(string.Format("{0}/{1}{2}", this.data.ZombieWgoData.GetUsedPerksCount(), this.data.ZombieWgoData.RedSkulls, "rskull".FontIcon()), false, true);
		this.perksTabButton.UpdateText(LLBase.L("ui_zombie_wndw_perks") + " " + text);
	}

	// Token: 0x0600492D RID: 18733 RVA: 0x0015A2F4 File Offset: 0x001584F4
	private bool OnPressedPrevTab()
	{
		LazyAudio.PlayAndForget("tab_click");
		if (this.zombieProgressionWidget.gameObject.activeSelf)
		{
			this.RedrawCharacterTab();
		}
		else
		{
			this.RedrawPerksTab();
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		return true;
	}

	// Token: 0x0600492E RID: 18734 RVA: 0x0015A344 File Offset: 0x00158544
	private bool OnPressedNextTab()
	{
		LazyAudio.PlayAndForget("tab_click");
		if (this.zombieProgressionWidget.gameObject.activeSelf)
		{
			this.RedrawCharacterTab();
		}
		else
		{
			this.RedrawPerksTab();
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		return true;
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x0015A394 File Offset: 0x00158594
	public bool OnPressedPrevSubTab()
	{
		if (this.zombieProgressionWidget.gameObject.activeSelf)
		{
			LazyAudio.PlayAndForget("tab_click");
			this.zombieProgressionWidget.TalentTabButtonsContainer.OnPressedPrevTechTab(null, null);
			if (LazyInput.IsGamepadActive)
			{
				base.GamepadNavigationController.ReinitItems(true, null, null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x0015A3E8 File Offset: 0x001585E8
	public bool OnPressedNextSubTab()
	{
		if (this.zombieProgressionWidget.gameObject.activeSelf)
		{
			LazyAudio.PlayAndForget("tab_click");
			this.zombieProgressionWidget.TalentTabButtonsContainer.OnPressedNextTechTab(null, null);
			if (LazyInput.IsGamepadActive)
			{
				base.GamepadNavigationController.ReinitItems(true, null, null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06004931 RID: 18737 RVA: 0x0015A43C File Offset: 0x0015863C
	private bool OnItemPressed2()
	{
		if (LazyInput.IsGamepadActive)
		{
			GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
			UIItemCell uiitemCell;
			if (focusedItem != null && focusedItem.TryGetComponent<UIItemCell>(out uiitemCell) && uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty)
			{
				uiitemCell.OnPress2();
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004932 RID: 18738 RVA: 0x0015A490 File Offset: 0x00158690
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.NextTab, new Func<bool>(this.OnPressedNextTab));
		gameKeyDelegates.Add(GameKey.PrevTab, new Func<bool>(this.OnPressedPrevTab));
		gameKeyDelegates.Add(GameKey.NextSubTab, new Func<bool>(this.OnPressedNextSubTab));
		gameKeyDelegates.Add(GameKey.PrevSubTab, new Func<bool>(this.OnPressedPrevSubTab));
		gameKeyDelegates.Add(GameKey.ItemMove, new Func<bool>(this.OnItemPressed2));
		return gameKeyDelegates;
	}

	// Token: 0x06004933 RID: 18739 RVA: 0x0015A518 File Offset: 0x00158718
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (LazyInput.IsGamepadActive)
		{
			this.nextTabGamepadHelper.gameObject.SetActive(true);
			this.prevTabGamepadHelper.gameObject.SetActive(true);
			this.nextSubTabGamepadHelper.gameObject.SetActive(true);
			this.prevSubTabGamepadHelper.gameObject.SetActive(true);
			this.nextTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.NextTab, null, true);
			this.prevTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.PrevTab, null, true);
			this.nextSubTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.NextSubTab, null, true);
			this.prevSubTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.PrevSubTab, null, true);
			return;
		}
		this.nextTabGamepadHelper.gameObject.SetActive(false);
		this.prevTabGamepadHelper.gameObject.SetActive(false);
		this.nextSubTabGamepadHelper.gameObject.SetActive(false);
		this.prevSubTabGamepadHelper.gameObject.SetActive(false);
	}

	// Token: 0x06004934 RID: 18740 RVA: 0x0015A61C File Offset: 0x0015881C
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		UIItemCell uiitemCell;
		if (gamepadNavigationItem != null && gamepadNavigationItem.TryGetComponent<UIItemCell>(out uiitemCell))
		{
			if (uiitemCell.IsInteractable && uiitemCell.OnItemCellPress != null)
			{
				list.Add(LazyGameKeyTip.Select(true, true, true));
			}
			if (uiitemCell.DisplayingItem != null && !uiitemCell.DisplayingItem.IsEmpty && uiitemCell.IsInteractable && uiitemCell.OnItemCellPress2 != null)
			{
				list.Add(new LazyGameKeyTip(GameKey.ItemMove, "tip_item_action", true, true, true));
			}
		}
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06004935 RID: 18741 RVA: 0x00002318 File Offset: 0x00000518
	[LazyUITest]
	protected override void TestDraw()
	{
	}

	// Token: 0x040038FF RID: 14591
	[SerializeField]
	private Canvas contentCanvas;

	// Token: 0x04003900 RID: 14592
	[SerializeField]
	private UIWorkerIcon workerIcon;

	// Token: 0x04003901 RID: 14593
	[SerializeField]
	private TalentWidget[] talentWidgets;

	// Token: 0x04003902 RID: 14594
	[SerializeField]
	private List<string> talents = new List<string>();

	// Token: 0x04003903 RID: 14595
	[SerializeField]
	private TextMeshProUGUI nameLabel;

	// Token: 0x04003904 RID: 14596
	[SerializeField]
	private TextMeshProUGUI whiteSkullsLabel;

	// Token: 0x04003905 RID: 14597
	[SerializeField]
	private TextMeshProUGUI redSkullsLabel;

	// Token: 0x04003906 RID: 14598
	[SerializeField]
	private GameObject mainTabWidget;

	// Token: 0x04003907 RID: 14599
	[SerializeField]
	private TextMeshProUGUI nextTabGamepadHelper;

	// Token: 0x04003908 RID: 14600
	[SerializeField]
	private TextMeshProUGUI prevTabGamepadHelper;

	// Token: 0x04003909 RID: 14601
	[SerializeField]
	private TextMeshProUGUI nextSubTabGamepadHelper;

	// Token: 0x0400390A RID: 14602
	[SerializeField]
	private TextMeshProUGUI prevSubTabGamepadHelper;

	// Token: 0x0400390B RID: 14603
	[SerializeField]
	private TextStyle perksSkullsStyle;

	// Token: 0x0400390C RID: 14604
	[SerializeField]
	private ZombieWindowTabButton characterTabButton;

	// Token: 0x0400390D RID: 14605
	[SerializeField]
	private BodyOrgansInventoryWidget bodyOrgansInventoryWidget;

	// Token: 0x0400390E RID: 14606
	[SerializeField]
	private BodyPocketInventoryWidget bodyPocketInventoryWidget;

	// Token: 0x0400390F RID: 14607
	[SerializeField]
	private ZombieEquipmentInventoryWidget zombieEquipmentInventoryWidget;

	// Token: 0x04003910 RID: 14608
	[SerializeField]
	private ZombieWindowTabButton perksTabButton;

	// Token: 0x04003911 RID: 14609
	[SerializeField]
	private ZombieProgressionWidget zombieProgressionWidget;

	// Token: 0x04003912 RID: 14610
	[SerializeField]
	private TextMeshProUGUI redSpheresLabel;

	// Token: 0x04003913 RID: 14611
	[SerializeField]
	private TextMeshProUGUI greenSpheresLabel;

	// Token: 0x04003914 RID: 14612
	[SerializeField]
	private TextMeshProUGUI blueSpheresLabel;

	// Token: 0x04003915 RID: 14613
	private static bool lastOpenedPerksTab;
}
