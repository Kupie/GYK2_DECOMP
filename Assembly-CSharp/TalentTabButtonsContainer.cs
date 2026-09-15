using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000930 RID: 2352
public class TalentTabButtonsContainer : MonoBehaviour
{
	// Token: 0x06003E01 RID: 15873 RVA: 0x001283B0 File Offset: 0x001265B0
	public void Init(Action<string> onTalentSelected, Canvas canvas)
	{
		this.onTalentSelected = onTalentSelected;
		this.canvas = canvas;
		foreach (TalentDef talentDef in GameBalance.Me.talentDefs)
		{
			string talentId = talentDef.id;
			TalentTabButton talentTabButton = this.tabButtonPrefab.Copy(base.transform, false, talentId);
			talentTabButton.Init(talentId, delegate
			{
				this.DisplayTab(talentId);
			});
			this.talentTabButtons.Add(talentTabButton);
			talentTabButton.UpdateState(false, canvas);
		}
		this.talentTabButtons[0].UpdateState(true, canvas);
	}

	// Token: 0x06003E02 RID: 15874 RVA: 0x00128480 File Offset: 0x00126680
	public void Draw(string currentTabName = "", bool blockCurrentInspirationActionIndicator = true)
	{
		if (!string.IsNullOrEmpty(currentTabName))
		{
			this.UpdateCurrentTab(currentTabName);
		}
		if (blockCurrentInspirationActionIndicator)
		{
			MainGame.Instance.GameSave.talentSystemData.GetTalentBranch(this.talentTabButtons[this.currentTab].TalentId).BlockInspirationActionIndicator();
		}
		foreach (TalentTabButton talentTabButton in this.talentTabButtons)
		{
			talentTabButton.gameObject.SetActive(MainGame.Instance.GameSave.knowledgeSystem.IsTalentBranchUnlocked(talentTabButton.TalentId));
			talentTabButton.UpdateSorting(this.canvas);
			TalentData talentBranch = MainGame.Instance.GameSave.talentSystemData.GetTalentBranch(talentTabButton.TalentId);
			talentTabButton.SetActionIndicatorState(talentBranch.HasAvailableInspirationActionIndicator);
			talentTabButton.DrawMasteryValue();
		}
		foreach (TalentTabButton talentTabButton2 in this.talentTabButtons)
		{
			talentTabButton2.UpdateState(this.talentTabButtons[this.currentTab] == talentTabButton2, this.canvas);
		}
	}

	// Token: 0x06003E03 RID: 15875 RVA: 0x001285CC File Offset: 0x001267CC
	public bool OnPressedPrevTechTab(GamepadNavigationController gamepadNavigationController = null, AutoScroll autoScroll = null)
	{
		int num = this.currentTab;
		num--;
		if (num < 0)
		{
			num = this.talentTabButtons.Count - 1;
		}
		this.DisplayTab(this.talentTabButtons[num].TalentId);
		if (gamepadNavigationController != null && autoScroll != null && LazyInput.IsGamepadActive)
		{
			autoScroll.SkipNextAutoscroll = true;
			gamepadNavigationController.ReinitItems(true, null, null);
			this.scrollRect.DOKill(false);
			this.scrollRect.verticalNormalizedPosition = 1f;
		}
		return true;
	}

	// Token: 0x06003E04 RID: 15876 RVA: 0x00128654 File Offset: 0x00126854
	public bool OnPressedNextTechTab(GamepadNavigationController gamepadNavigationController = null, AutoScroll autoScroll = null)
	{
		int num = this.currentTab;
		num++;
		if (num > this.talentTabButtons.Count - 1)
		{
			num = 0;
		}
		this.DisplayTab(this.talentTabButtons[num].TalentId);
		if (gamepadNavigationController != null && autoScroll != null && LazyInput.IsGamepadActive)
		{
			autoScroll.SkipNextAutoscroll = true;
			gamepadNavigationController.ReinitItems(true, null, null);
			this.scrollRect.DOKill(false);
			this.scrollRect.verticalNormalizedPosition = 1f;
		}
		return true;
	}

	// Token: 0x06003E05 RID: 15877 RVA: 0x001286DC File Offset: 0x001268DC
	private void DisplayTab(string talendId)
	{
		this.UpdateCurrentTab(talendId);
		Action<string> action = this.onTalentSelected;
		if (action != null)
		{
			action(talendId);
		}
		if (this.scrollRect != null)
		{
			this.scrollRect.ResetPosition();
		}
	}

	// Token: 0x06003E06 RID: 15878 RVA: 0x00128710 File Offset: 0x00126910
	public void UpdateCurrentTab(string currentTalentId)
	{
		this.talentTabButtons[this.currentTab].UpdateState(false, this.canvas);
		this.currentTab = this.talentTabButtons.IndexOf(this.talentTabButtons.Find((TalentTabButton t) => t.TalentId == currentTalentId));
		this.talentTabButtons[this.currentTab].UpdateState(true, this.canvas);
	}

	// Token: 0x06003E07 RID: 15879 RVA: 0x0012878C File Offset: 0x0012698C
	private void Awake()
	{
		this.tabButtonPrefab.gameObject.SetActive(false);
	}

	// Token: 0x040030D5 RID: 12501
	[SerializeField]
	private TalentTabButton tabButtonPrefab;

	// Token: 0x040030D6 RID: 12502
	[Space]
	private List<TalentTabButton> talentTabButtons = new List<TalentTabButton>();

	// Token: 0x040030D7 RID: 12503
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x040030D8 RID: 12504
	private int currentTab;

	// Token: 0x040030D9 RID: 12505
	private Action<string> onTalentSelected;

	// Token: 0x040030DA RID: 12506
	private Canvas canvas;
}
