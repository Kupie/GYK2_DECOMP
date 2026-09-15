using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000975 RID: 2421
public abstract class UIBaseCraftSelectionWindow : LazyWindow<UIBaseCraftSelectionWindowData>
{
	// Token: 0x17000995 RID: 2453
	// (get) Token: 0x06003FC2 RID: 16322 RVA: 0x00131259 File Offset: 0x0012F459
	public CraftDef CraftDef
	{
		get
		{
			return this.data.CraftDefinition;
		}
	}

	// Token: 0x06003FC3 RID: 16323 RVA: 0x00131268 File Offset: 0x0012F468
	public override void Init()
	{
		base.Init();
		for (int i = 0; i < 18; i++)
		{
			this.backgroundProgressBarCellImages.Add(global::UnityEngine.Object.Instantiate<GameObject>(this.backgroundProgressBarCellImagePrefab, this.backgroundProgressBarCellGroupParent));
		}
		this.backgroundProgressBarCellImagePrefab.gameObject.SetActive(false);
	}

	// Token: 0x06003FC4 RID: 16324 RVA: 0x001312B5 File Offset: 0x0012F4B5
	protected override void SetData(UIBaseCraftSelectionWindowData data)
	{
		base.SetData(data);
		this.onAddToCraftQueuePressed = data.OnCraftToQueueAdded;
		this.onStartCraftPressed = data.OnCraftStarted;
		this.onPlusQueuePressed = data.OnPressPlusQueue;
		this.onMinusQueuePressed = data.OnPressMinusQueue;
	}

	// Token: 0x06003FC5 RID: 16325 RVA: 0x001312EE File Offset: 0x0012F4EE
	public override void Open(UIBaseCraftSelectionWindowData data)
	{
		base.Open(data);
		this.SubscribeToDataChanges();
	}

	// Token: 0x06003FC6 RID: 16326 RVA: 0x00131300 File Offset: 0x0012F500
	public override void Hide()
	{
		foreach (UICraftRequirementWidget uicraftRequirementWidget in this.craftRequirementWidgets)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftRequirementWidget>(uicraftRequirementWidget);
		}
		this.craftRequirementWidgets.Clear();
		this.HideCells();
		this.progressCellsInfoWidget.Hide();
		this.UpdateCraftCountElementsActiveStatus(false);
		this.craftCountHold.Reset();
		if (this.data != null)
		{
			this.UnsubscribeFromDataChanges();
		}
		base.Hide();
	}

	// Token: 0x06003FC7 RID: 16327 RVA: 0x0013139C File Offset: 0x0012F59C
	protected void DrawBaseElements()
	{
		this.outputItem.Draw(this.data.CraftDefinition.GetOutputPreview(this.data.WgoData), 0);
		this.outputItem.UIItemCell.GamepadNavigationItem.group = 1;
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.data.CraftItemCellsData.Count; i++)
		{
			UICraftItemCell uicraftItemCell2 = this.displayedIngredients[i];
			uicraftItemCell2.Draw(this.data.CraftItemCellsData[i], new Action(this.OnNeedItemChange), false);
			uicraftItemCell2.GamepadNavigationItem.group = 1;
		}
		this.headerLabel.text = LLBase.L(this.data.CraftDefinition.id);
		this.UpdateRequirements();
		this.UpdateProgressBar();
		this.UpdateProgressChanceBar();
		this.UpdateCraftCountElements();
		this.UpdateCraftCountElementsInteractableStatus();
		this.UpdateButtonsText();
		this.UpdateButtonsInteractableState();
		this.UpdateCounters();
	}

	// Token: 0x06003FC8 RID: 16328 RVA: 0x001314D8 File Offset: 0x0012F6D8
	protected virtual void OnStartCraftPressed()
	{
		Action action = this.onStartCraftPressed;
		if (action != null)
		{
			action();
		}
		this.Close();
	}

	// Token: 0x06003FC9 RID: 16329 RVA: 0x001314F4 File Offset: 0x0012F6F4
	protected void UpdateTalent()
	{
		int talentLock = this.data.CraftDefinition.talentLock;
		if (talentLock == 0)
		{
			this.talentOverlap.gameObject.SetActive(false);
			return;
		}
		this.talentOverlap.gameObject.SetActive(true);
		bool flag = this.data.DisplayableWorker.GetMasteryLevelForTalentBranch(this.data.WgoData.Definition.talent, this.data.CraftDefinition) >= talentLock;
		this.talentLabel.text = this.data.WgoData.Definition.talent.FontIcon() ?? "";
		this.talentValueLabel.text = talentLock.ToString();
		if (this.data.CraftDefinition.isStarCraft || this.data.CraftDefinition.isAutopsyCraft)
		{
			this.starCraftTalentStyle.ApplyStyle(this.talentValueLabel, false, null, null, null);
			return;
		}
		if (flag)
		{
			this.greenTalentStyle.ApplyStyle(this.talentValueLabel, false, null, null, null);
			return;
		}
		this.redTalentStyle.ApplyStyle(this.talentValueLabel, false, null, null, null);
	}

	// Token: 0x06003FCA RID: 16330 RVA: 0x0013165F File Offset: 0x0012F85F
	protected bool OnStartCraft()
	{
		if (this.startCraftButton.interactable)
		{
			this.OnStartCraftPressed();
			return true;
		}
		return false;
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x00131677 File Offset: 0x0012F877
	protected virtual void OnPressPlusQueue()
	{
		this.ChangeCraftCount(1);
	}

	// Token: 0x06003FCC RID: 16332 RVA: 0x00131680 File Offset: 0x0012F880
	protected virtual void OnPressMinusQueue()
	{
		this.ChangeCraftCount(-1);
	}

	// Token: 0x06003FCD RID: 16333 RVA: 0x00131689 File Offset: 0x0012F889
	protected virtual void ChangeCraftCount(int delta)
	{
		if (delta == 0 || this.data == null)
		{
			return;
		}
		this.data.AddCraftsCount(delta);
		this.UpdateCounters();
		this.UpdateRequirements();
		this.UpdateCraftCountElementsInteractableStatus();
		this.UpdateButtonsInteractableState();
	}

	// Token: 0x06003FCE RID: 16334 RVA: 0x001316BB File Offset: 0x0012F8BB
	protected override void Update()
	{
		this.TickCraftCountHold();
		base.Update();
	}

	// Token: 0x06003FCF RID: 16335 RVA: 0x001316C9 File Offset: 0x0012F8C9
	protected virtual void TickCraftCountHold()
	{
		if (!base.IsShownAndTop || this.data == null)
		{
			this.craftCountHold.Reset();
			return;
		}
		this.craftCountHold.Tick(this.GetCraftCountHoldDirection(), new Action<int>(this.ChangeCraftCount));
	}

	// Token: 0x06003FD0 RID: 16336 RVA: 0x00131708 File Offset: 0x0012F908
	protected virtual int GetCraftCountHoldDirection()
	{
		int pointerHoldDirection = HoldRepeatValueChanger.GetPointerHoldDirection(this.plusCraftButton, this.minusCraftButton);
		if (pointerHoldDirection != 0)
		{
			return pointerHoldDirection;
		}
		if (!this.CanChangeCraftCountWithGamepad())
		{
			return 0;
		}
		bool flag = HoldRepeatValueChanger.IsAnyKeyHeld(GameKey.DpadUp, GameKey.Up) || HoldRepeatValueChanger.GetAxisHoldDirection(true) > 0;
		bool flag2 = HoldRepeatValueChanger.IsAnyKeyHeld(GameKey.DpadDown, GameKey.Down) || HoldRepeatValueChanger.GetAxisHoldDirection(true) < 0;
		if (flag == flag2)
		{
			return 0;
		}
		if (flag && (this.plusCraftButton == null || !this.plusCraftButton.gameObject.activeSelf || !this.plusCraftButton.interactable))
		{
			return 0;
		}
		if (flag2 && (this.minusCraftButton == null || !this.minusCraftButton.gameObject.activeSelf || !this.minusCraftButton.interactable))
		{
			return 0;
		}
		if (!flag)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06003FD1 RID: 16337 RVA: 0x001317E4 File Offset: 0x0012F9E4
	protected virtual bool CanChangeCraftCountWithGamepad()
	{
		return LazyInput.IsGamepadActive && !(base.GamepadNavigationController.FocusedItem == null) && !(this.outputItem == null) && (this.plusCraftButton != null && this.plusCraftButton.gameObject.activeSelf) && base.GamepadNavigationController.FocusedItem == this.outputItem.UIItemCell.GamepadNavigationItem;
	}

	// Token: 0x06003FD2 RID: 16338 RVA: 0x0013185D File Offset: 0x0012FA5D
	protected virtual void UpdateButtonsText()
	{
		this.startCraftButtonText.text = LLBase.L("ui_craft");
	}

	// Token: 0x06003FD3 RID: 16339 RVA: 0x00131874 File Offset: 0x0012FA74
	protected void AddTalentLockText(TextMeshProUGUI label)
	{
		if (this.data == null || this.data.CraftDefinition == null || this.data.DisplayableWorker == null || this.data.WgoData == null || label == null)
		{
			return;
		}
		if (!this.data.CraftDefinition.isStarCraft && !this.data.CraftDefinition.isAutopsyCraft)
		{
			int masteryLevelForTalentBranch = this.data.DisplayableWorker.GetMasteryLevelForTalentBranch(this.data.WgoData.Definition.talent, this.data.CraftDefinition);
			int talentLock = this.data.CraftDefinition.talentLock;
			if (talentLock > 0 && masteryLevelForTalentBranch < talentLock)
			{
				string text = talentLock.ToString() ?? "";
				label.text = string.Concat(new string[]
				{
					label.text,
					" ",
					this.data.WgoData.Definition.talent.FontIcon(),
					" ",
					this.redTalentStyleButton.ApplyStyleToString(masteryLevelForTalentBranch.ToString(), false, true),
					this.slashTalentStyleButton.ApplyStyleToString("/", false, true),
					this.commonTalentStyleButton.ApplyStyleToString(text, false, true)
				});
			}
		}
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x001319D0 File Offset: 0x0012FBD0
	protected virtual string GetStartCraftGamepadTipKey()
	{
		return "ui_craft";
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x001319D8 File Offset: 0x0012FBD8
	protected virtual void UpdateCraftGamepadTips()
	{
		if (this.gamepadTipStartCraft == null || this.startCraftButton == null)
		{
			return;
		}
		this.gamepadTipStartCraft.text = new LazyGameKeyTip(GameKey.StartCraft, this.GetStartCraftGamepadTipKey(), this.startCraftButton.interactable, true, true).ToString();
		this.AddTalentLockText(this.gamepadTipStartCraft);
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x00131A3B File Offset: 0x0012FC3B
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		this.UpdateCraftGamepadTips();
	}

	// Token: 0x06003FD7 RID: 16343 RVA: 0x00131A49 File Offset: 0x0012FC49
	protected virtual void UpdateButtonsInteractableState()
	{
		this.startCraftButton.interactable = this.data.CanStartCraft;
		this.UpdateCraftGamepadTips();
	}

	// Token: 0x06003FD8 RID: 16344 RVA: 0x00131A68 File Offset: 0x0012FC68
	protected virtual bool OnDpadUpPressed()
	{
		return LazyInput.IsGamepadActive && !(base.GamepadNavigationController.FocusedItem == null) && (this.plusCraftButton.gameObject.activeSelf && this.plusCraftButton.interactable && base.GamepadNavigationController.FocusedItem == this.outputItem.UIItemCell.GamepadNavigationItem);
	}

	// Token: 0x06003FD9 RID: 16345 RVA: 0x00131AD8 File Offset: 0x0012FCD8
	protected virtual bool OnDpadDownPressed()
	{
		return LazyInput.IsGamepadActive && !(base.GamepadNavigationController.FocusedItem == null) && (this.minusCraftButton.gameObject.activeSelf && this.minusCraftButton.interactable && base.GamepadNavigationController.FocusedItem == this.outputItem.UIItemCell.GamepadNavigationItem);
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x00131B45 File Offset: 0x0012FD45
	private void OnNeedItemChange()
	{
		this.UpdateProgressBar();
		this.UpdateButtonsInteractableState();
	}

	// Token: 0x06003FDB RID: 16347 RVA: 0x00131B54 File Offset: 0x0012FD54
	private void UpdateRequirements()
	{
		if (this.data.CraftDefinition.isAuto)
		{
			this.requirementsGo.gameObject.SetActive(false);
			return;
		}
		this.requirementsGo.gameObject.SetActive(true);
		for (int i = 0; i < this.data.CraftRequirementWidgetData.Count; i++)
		{
			UICraftRequirementWidget uicraftRequirementWidget;
			if (i > this.craftRequirementWidgets.Count - 1)
			{
				uicraftRequirementWidget = UIPrefabsPooler.Instance.GetElementFromPool<UICraftRequirementWidget>(this.requirementsContainer);
				this.craftRequirementWidgets.Add(uicraftRequirementWidget);
			}
			else
			{
				uicraftRequirementWidget = this.craftRequirementWidgets[i];
			}
			uicraftRequirementWidget.gameObject.SetActive(true);
			uicraftRequirementWidget.Draw(this.data.CraftRequirementWidgetData[i]);
		}
	}

	// Token: 0x06003FDC RID: 16348 RVA: 0x00131C10 File Offset: 0x0012FE10
	private void UpdateCraftCountElements()
	{
		if (this.data != null)
		{
			this.UpdateCraftCountElementsActiveStatus(!this.data.CraftDefinition.IsMultipleCraftsDisabled);
		}
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x00131C34 File Offset: 0x0012FE34
	protected virtual void UpdateCounters()
	{
		this.outputItem.UIItemCell.OnMultiplierChange(this.data.CraftsCount, false);
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.SetMultiplierValue(this.data.CraftsCount);
		}
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x00131CAC File Offset: 0x0012FEAC
	private void HideCells()
	{
		foreach (GameObject gameObject in this.backgroundProgressBarCellImages)
		{
			gameObject.gameObject.SetActive(false);
		}
		foreach (ProgressCellCraft progressCellCraft in this.progressCells)
		{
			progressCellCraft.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<ProgressCellCraft>(progressCellCraft);
		}
		this.progressCells.Clear();
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x00131D5C File Offset: 0x0012FF5C
	private void UpdateProgressBar()
	{
		this.HideCells();
		int craftStartTicks = this.data.ParamsData.CraftStartTicks;
		int perksCraftAddTotalProgressTicksValue = this.data.ParamsData.PerksCraftAddTotalProgressTicksValue;
		Dictionary<PerkDef, int> dictionary = new Dictionary<PerkDef, int>();
		Dictionary<ItemDef, int> dictionary2 = new Dictionary<ItemDef, int>();
		PerkSystemData perkSystemData = MainGame.Instance.GameSave.perkSystemData;
		using (List<string>.Enumerator enumerator = this.data.CraftDefinition.linkedPerks.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				string perkId = enumerator.Current;
				int num = perkSystemData.activePerks.FindIndex((PerkData x) => x.id == perkId);
				if (num != -1 && perkSystemData.activePerks[num].Definition.craftStartTicks > 0)
				{
					dictionary.Add(perkSystemData.activePerks[num].Definition, perkSystemData.activePerks[num].Definition.craftStartTicks);
				}
			}
		}
		foreach (NeedItemData needItemData in this.data.CurrentNeedItems)
		{
			if (!needItemData.IsGroup && needItemData.ItemDef != null && needItemData.ItemDef.qualityType == ItemDef.QualityType.Star && needItemData.ItemDef.quality > 1)
			{
				dictionary2.Add(needItemData.ItemDef, needItemData.ItemDef.quality - 1);
			}
		}
		int num2 = this.data.CraftDefinition.duration.EvaluateInt() + perksCraftAddTotalProgressTicksValue;
		for (int i = 0; i < Math.Clamp(num2, 0, 18); i++)
		{
			ProgressCellCraft elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<ProgressCellCraft>(this.progressBarParent);
			int num3 = -1;
			int num4 = i + 1;
			if (this.data.CraftDefinition.isStarCraft)
			{
				if (num4 == this.data.CraftDefinition.goldLevel)
				{
					num3 = 3;
				}
				else if (num4 == this.data.CraftDefinition.silverLevel)
				{
					num3 = 2;
				}
				else if (num4 == this.data.CraftDefinition.bronzeLevel)
				{
					num3 = 1;
				}
			}
			PerkDef perkDef = null;
			using (Dictionary<PerkDef, int>.KeyCollection.Enumerator enumerator3 = dictionary.Keys.GetEnumerator())
			{
				if (enumerator3.MoveNext())
				{
					PerkDef perkDef2 = enumerator3.Current;
					perkDef = perkDef2;
					Dictionary<PerkDef, int> dictionary3 = dictionary;
					PerkDef perkDef3 = perkDef2;
					dictionary3[perkDef3]--;
					if (dictionary[perkDef2] == 0)
					{
						dictionary.Remove(perkDef2);
					}
				}
			}
			if (perkDef == null)
			{
				using (Dictionary<ItemDef, int>.KeyCollection.Enumerator enumerator4 = dictionary2.Keys.GetEnumerator())
				{
					if (enumerator4.MoveNext())
					{
						ItemDef itemDef = enumerator4.Current;
						Dictionary<ItemDef, int> dictionary4 = dictionary2;
						ItemDef itemDef2 = itemDef;
						dictionary4[itemDef2]--;
						if (dictionary2[itemDef] == 0)
						{
							dictionary2.Remove(itemDef);
						}
					}
				}
			}
			int num5 = -1;
			AutopsyTypeCraft autopsyTypeCraft = this.data.CraftDefinition.autopsyTypeCraft;
			if (autopsyTypeCraft == AutopsyTypeCraft.ExtractOrgan || autopsyTypeCraft == AutopsyTypeCraft.InsertOrgan)
			{
				if (num4 == this.data.CraftDefinition.goldLevel)
				{
					num5 = 2;
				}
				else if (num4 == this.data.CraftDefinition.silverLevel)
				{
					num5 = 1;
				}
			}
			else if (this.data.CraftDefinition.autopsyTypeCraft == AutopsyTypeCraft.ChangeOrgan && num4 == this.data.CraftDefinition.goldLevel)
			{
				num5 = 2;
			}
			elementFromPool.Show(i, num2, i > craftStartTicks - 1, false, num3, num5, -1);
			this.progressCells.Add(elementFromPool);
		}
		for (int j = 0; j < this.progressCells.Count; j++)
		{
			this.backgroundProgressBarCellImages[j].gameObject.SetActive(true);
		}
		this.progressBarParent.RefreshContentFitter();
	}

	// Token: 0x06003FE0 RID: 16352 RVA: 0x00132174 File Offset: 0x00130374
	private void UpdateProgressChanceBar()
	{
		this.progressCellsInfoWidget.Hide();
		if (this.data.CraftDefinition.isAuto)
		{
			return;
		}
		int masteryLevelForTalentBranch = this.data.DisplayableWorker.GetMasteryLevelForTalentBranch(this.data.WgoData.Definition.talent, this.data.CraftDefinition);
		int perksCraftMasteryBonusValue = this.data.DisplayableWorker.GetPerksCraftMasteryBonusValue(this.data.CraftDefinition);
		bool flag = this.data.CraftDefinition.isStarCraft || this.data.CraftDefinition.isAutopsyCraft || this.data.CraftDefinition.isPocketExtractCraft;
		UIProgressCellsInfoWidgetData uiprogressCellsInfoWidgetData = new UIProgressCellsInfoWidgetData(masteryLevelForTalentBranch, this.data.CraftDefinition.talentLock, GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent), flag, (flag && masteryLevelForTalentBranch < this.data.CraftDefinition.talentLock) ? "tt_craft_tick_part" : "tt_craft_tick", perksCraftMasteryBonusValue);
		this.progressCellsInfoWidget.Draw(uiprogressCellsInfoWidgetData);
	}

	// Token: 0x06003FE1 RID: 16353 RVA: 0x0013228A File Offset: 0x0013048A
	private void UpdateCraftCountElementsActiveStatus(bool isActive)
	{
		this.plusCraftButton.gameObject.SetActive(isActive);
		this.minusCraftButton.gameObject.SetActive(isActive);
	}

	// Token: 0x06003FE2 RID: 16354 RVA: 0x001322AE File Offset: 0x001304AE
	protected virtual void UpdateCraftCountElementsInteractableStatus()
	{
		this.minusCraftButton.interactable = this.data.CraftsCount > 1;
	}

	// Token: 0x06003FE3 RID: 16355 RVA: 0x001322CC File Offset: 0x001304CC
	protected void AddCraftCountGamepadTips(List<LazyGameKeyTip> tips)
	{
		if (tips == null)
		{
			return;
		}
		if (this.plusCraftButton != null && this.plusCraftButton.gameObject.activeSelf)
		{
			tips.Add(new LazyGameKeyTip(GameKey.DpadUp, "+", this.plusCraftButton.interactable, true, true));
		}
		if (this.minusCraftButton != null && this.minusCraftButton.gameObject.activeSelf)
		{
			tips.Add(new LazyGameKeyTip(GameKey.DpadDown, "-", this.minusCraftButton.interactable, true, true));
		}
	}

	// Token: 0x06003FE4 RID: 16356 RVA: 0x00132361 File Offset: 0x00130561
	protected virtual void SubscribeToDataChanges()
	{
		if (!this.subscribedToDataChanges)
		{
			this.data.SubscribeToDataChanges();
			this.subscribedToDataChanges = true;
		}
	}

	// Token: 0x06003FE5 RID: 16357 RVA: 0x0013237D File Offset: 0x0013057D
	protected virtual void UnsubscribeFromDataChanges()
	{
		if (this.subscribedToDataChanges)
		{
			this.data.UnsubscribeFromDataChanges();
			this.subscribedToDataChanges = false;
		}
	}

	// Token: 0x0400322E RID: 12846
	protected Action onAddToCraftQueuePressed;

	// Token: 0x0400322F RID: 12847
	protected Action onStartCraftPressed;

	// Token: 0x04003230 RID: 12848
	protected Action onPlusQueuePressed;

	// Token: 0x04003231 RID: 12849
	protected Action onMinusQueuePressed;

	// Token: 0x04003232 RID: 12850
	[SerializeField]
	protected TextMeshProUGUI headerLabel;

	// Token: 0x04003233 RID: 12851
	[SerializeField]
	protected Transform requirementsContainer;

	// Token: 0x04003234 RID: 12852
	[SerializeField]
	protected GameObject requirementsGo;

	// Token: 0x04003235 RID: 12853
	[SerializeField]
	protected LazyButton startCraftButton;

	// Token: 0x04003236 RID: 12854
	[SerializeField]
	protected TextMeshProUGUI gamepadTipStartCraft;

	// Token: 0x04003237 RID: 12855
	[SerializeField]
	private TextMeshProUGUI talentLabel;

	// Token: 0x04003238 RID: 12856
	[SerializeField]
	private TextMeshProUGUI talentValueLabel;

	// Token: 0x04003239 RID: 12857
	[SerializeField]
	protected TextStyle greenTalentStyle;

	// Token: 0x0400323A RID: 12858
	[SerializeField]
	protected TextStyle redTalentStyle;

	// Token: 0x0400323B RID: 12859
	[SerializeField]
	protected TextStyle starCraftTalentStyle;

	// Token: 0x0400323C RID: 12860
	[SerializeField]
	protected TextStyle commonTalentStyle;

	// Token: 0x0400323D RID: 12861
	[SerializeField]
	protected TextStyle commonTalentStyleButton;

	// Token: 0x0400323E RID: 12862
	[SerializeField]
	protected TextStyle redTalentStyleButton;

	// Token: 0x0400323F RID: 12863
	[SerializeField]
	protected TextStyle slashTalentStyleButton;

	// Token: 0x04003240 RID: 12864
	[SerializeField]
	private Image talentOverlap;

	// Token: 0x04003241 RID: 12865
	[SerializeField]
	[Space]
	protected LazyButton plusCraftButton;

	// Token: 0x04003242 RID: 12866
	[SerializeField]
	protected LazyButton minusCraftButton;

	// Token: 0x04003243 RID: 12867
	[SerializeField]
	protected RectTransform progressBarParent;

	// Token: 0x04003244 RID: 12868
	[SerializeField]
	private GameObject backgroundProgressBarCellImagePrefab;

	// Token: 0x04003245 RID: 12869
	[SerializeField]
	private RectTransform backgroundProgressBarCellGroupParent;

	// Token: 0x04003246 RID: 12870
	[SerializeField]
	protected UIProgressCellsInfoWidget progressCellsInfoWidget;

	// Token: 0x04003247 RID: 12871
	[SerializeField]
	protected TextMeshProUGUI startCraftButtonText;

	// Token: 0x04003248 RID: 12872
	[SerializeField]
	protected List<UICraftItemCell> displayedIngredients = new List<UICraftItemCell>();

	// Token: 0x04003249 RID: 12873
	protected List<ProgressCellCraft> progressCells = new List<ProgressCellCraft>();

	// Token: 0x0400324A RID: 12874
	private List<GameObject> backgroundProgressBarCellImages = new List<GameObject>();

	// Token: 0x0400324B RID: 12875
	protected List<UICraftRequirementWidget> craftRequirementWidgets = new List<UICraftRequirementWidget>();

	// Token: 0x0400324C RID: 12876
	[SerializeField]
	protected UICraftSelectionOutputItemCell outputItem;

	// Token: 0x0400324D RID: 12877
	protected bool subscribedToDataChanges;

	// Token: 0x0400324E RID: 12878
	private readonly HoldRepeatValueChanger craftCountHold = new HoldRepeatValueChanger();
}
