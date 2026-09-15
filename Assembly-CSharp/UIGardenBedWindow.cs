using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009D9 RID: 2521
public class UIGardenBedWindow : LazyWindow<UIGardenBedWindowData>
{
	// Token: 0x0600435D RID: 17245 RVA: 0x001401B8 File Offset: 0x0013E3B8
	public override void Init()
	{
		base.Init();
		this.AttachGarden3Tooltip();
		UIMouseTooltip.Attach(this.masteryRequirementLabel.gameObject, "tt_garden_6", null, true, false, new UIMouseTooltipEdges(0f, 0f, -3f, -3f), new Vector2(16f, -2f), null);
	}

	// Token: 0x0600435E RID: 17246 RVA: 0x00140213 File Offset: 0x0013E413
	protected override void SetData(UIGardenBedWindowData data)
	{
		base.SetData(data);
		this.SubscribeToDataChanges();
	}

	// Token: 0x0600435F RID: 17247 RVA: 0x00140224 File Offset: 0x0013E424
	public override void Redraw()
	{
		base.Redraw();
		this.UpdateCraftOutput();
		this.uiInfoWidget.Draw(this.data.UIInfoWidgetData);
		if (this.data.IsGrowing)
		{
			this.selectedSeed = null;
			this.selectedSeedCraft = null;
			this.slotsObj.anchoredPosition = this.slotsPos1.anchoredPosition;
			this.workerBack.sprite = this.workerBackGrowing;
			this.workerBack2.sprite = this.workerBackGrowing2;
			this.customBedImage.sprite = this.uiInfoWidget.Icon;
			this.customBedImageGamepad.sprite = this.uiInfoWidget.Icon;
			this.customBedImageShown = true;
			if (LazyInput.IsGamepadActive)
			{
				this.customBedImageGamepad.gameObject.SetActive(true);
				this.customBedImage.gameObject.SetActive(false);
			}
			else
			{
				this.customBedImage.gameObject.SetActive(true);
				this.customBedImageGamepad.gameObject.SetActive(false);
			}
			this.arrowDecor.SetActive(true);
			this.leftDecor.SetActive(true);
			this.craftProgressObj.SetActive(true);
			this.workerIcon.WorkerIconParent.gameObject.SetActive(false);
			this.workerIcon2.WorkerIconParent.gameObject.SetActive(false);
		}
		else
		{
			this.craftProgressObj.SetActive(false);
			this.slotsObj.anchoredPosition = this.slotsPos2.anchoredPosition;
			this.workerBack.sprite = this.workerBackCommon;
			this.workerBack2.sprite = this.workerBackCommon2;
			this.arrowDecor.SetActive(false);
			this.leftDecor.SetActive(false);
			this.customBedImage.gameObject.SetActive(false);
			this.customBedImageGamepad.gameObject.SetActive(false);
			this.customBedImageShown = false;
			this.workerIcon.WorkerIconParent.gameObject.SetActive(true);
			this.workerIcon2.WorkerIconParent.gameObject.SetActive(true);
		}
		this.UpdateProgressBar();
		this.UpdateCraftLabels();
		this.UpdateFertilizerLabel();
		this.UpdateInputItem();
		this.UpdatePlantControls();
		this.UpdatePerks();
		this.UpdateBedTalentIcon();
		this.UpdateProgressChanceBar();
		this.UpdateTickDuration();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06004360 RID: 17248 RVA: 0x0014047C File Offset: 0x0013E67C
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (this.customBedImageShown)
		{
			if (LazyInput.IsGamepadActive)
			{
				this.customBedImageGamepad.gameObject.SetActive(true);
				this.customBedImage.gameObject.SetActive(false);
				return;
			}
			this.customBedImage.gameObject.SetActive(true);
			this.customBedImageGamepad.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004361 RID: 17249 RVA: 0x001404E3 File Offset: 0x0013E6E3
	public override void Hide()
	{
		base.Hide();
		this.HideCells();
		this.UnSubscribeToDataChanges();
	}

	// Token: 0x06004362 RID: 17250 RVA: 0x001404F8 File Offset: 0x0013E6F8
	private void HideCells()
	{
		foreach (ProgressCellCraft progressCellCraft in this.progressCells)
		{
			progressCellCraft.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<ProgressCellCraft>(progressCellCraft);
		}
		this.progressCells.Clear();
	}

	// Token: 0x06004363 RID: 17251 RVA: 0x00140560 File Offset: 0x0013E760
	private void UpdateProgressBar()
	{
		if (!this.data.IsGrowing)
		{
			this.craftProgressObj.SetActive(false);
			return;
		}
		int totalProgressTicks = this.data.CraftElement.TotalProgressTicks;
		int succeededProgressTicks = this.data.CraftElement.SucceededProgressTicks;
		int num = this.data.CraftElement.TotalProgressTicks - this.data.CraftElement.FailedProgressTicks;
		if (this.data.CraftElement.ProgressTicks >= totalProgressTicks)
		{
			this.Close();
			return;
		}
		this.HideCells();
		for (int i = 0; i < totalProgressTicks; i++)
		{
			ProgressCellCraft elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<ProgressCellCraft>(this.progressBarParent);
			elementFromPool.Show(i, totalProgressTicks, i >= succeededProgressTicks && i < num, i >= num, -1, -1, this.data.CraftDefinition.GetQualityForGardenProgressTick(i + 1));
			this.progressCells.Add(elementFromPool);
		}
		UIMouseTooltip.Attach(this.progressBarParent.gameObject, "tt_garden_7", null, true, true, UIMouseTooltipEdges.All(-3f), default(Vector2), null);
		for (int j = 0; j < this.progressCells.Count; j++)
		{
			if (this.progressCells[j].PlusOneObject != null && this.progressCells[j].PlusOneObject.activeSelf)
			{
				UIMouseTooltip.Attach(this.progressCells[j].PlusOneObject, "tt_garden_4", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
			}
		}
		((RectTransform)this.progressBarParent.transform).RefreshContentFitter();
	}

	// Token: 0x06004364 RID: 17252 RVA: 0x0014070C File Offset: 0x0013E90C
	private void UpdateCraftLabels()
	{
		if (!this.data.IsGrowing)
		{
			this.resultLabel.text = string.Empty;
			this.masteryRequirementLabel.text = string.Empty;
			return;
		}
		this.resultLabel.text = LLBase.L(this.data.CraftDefinition.GetOutputPreview(this.data.WgoData).itemId);
		this.masteryRequirementLabel.text = this.data.WgoData.Definition.talent.FontIcon() + "<space=2px>" + this.data.CraftDefinition.talentLock.ToString();
	}

	// Token: 0x06004365 RID: 17253 RVA: 0x001407BB File Offset: 0x0013E9BB
	private void UpdateFertilizerLabel()
	{
		this.fertilizingLabel.gameObject.SetActive(!this.data.IsGrowing);
	}

	// Token: 0x06004366 RID: 17254 RVA: 0x001407DC File Offset: 0x0013E9DC
	private void UpdateTickDuration()
	{
		if (!this.data.IsGrowing)
		{
			return;
		}
		this.tickDuration.text = "";
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.data.WgoData.Definition.autocraftTickDuration.EvaluateFloat(this.data.WgoData));
		this.tickDuration.text = "cell_time".FontIcon() + timeSpan.ToString("m\\:ss");
		this.uiInfoWidget.TurnOnTickDuration();
	}

	// Token: 0x06004367 RID: 17255 RVA: 0x00140864 File Offset: 0x0013EA64
	private void UpdateInputItem()
	{
		if (!this.data.IsGrowing)
		{
			this.inputItem.gameObject.SetActive(false);
			this.craftProgressObj.SetActive(false);
			return;
		}
		List<ItemDef> list = new List<ItemDef>();
		foreach (ChanceOutputItem chanceOutputItem in this.data.CraftDefinition.addItemsToWgoOnFinish.chanceOutputItems)
		{
			ItemDef data = GameBalance.Me.GetData<ItemDef>(chanceOutputItem.id);
			if (data.itemGroupIds.Contains("seed"))
			{
				list.Add(data);
			}
		}
		if (list.Count < 0)
		{
			Debug.LogError("No seed item for growing craft:[" + this.data.CraftDefinition.id + "]");
			return;
		}
		int num = int.MaxValue;
		ItemDef itemDef = null;
		for (int i = 0; i < list.Count; i++)
		{
			ItemDef itemDef2 = list[i];
			if (itemDef2.quality < num)
			{
				itemDef = itemDef2;
				num = itemDef2.quality;
			}
		}
		if (itemDef == null)
		{
			Debug.LogError("No valid seed item for growing craft:[" + this.data.CraftDefinition.id + "]");
			this.inputItem.gameObject.SetActive(false);
			return;
		}
		this.inputItem.DrawSeedSlot(new Item(itemDef.id, 1));
		this.inputItem.gameObject.SetActive(true);
	}

	// Token: 0x06004368 RID: 17256 RVA: 0x001409EC File Offset: 0x0013EBEC
	private void UpdateBedTalentIcon()
	{
		CraftParamsData.GardenType gardenTypeFromWgo = UIGardenBedWindow.GetGardenTypeFromWgo(this.data.WgoData);
		Debug.Log(string.Format("Garden type: {0}", gardenTypeFromWgo));
		int num;
		if (gardenTypeFromWgo == CraftParamsData.GardenType.Vineyard)
		{
			num = MainGame.PlayerData.GetResInt("g_vineyard_farming_base");
		}
		else
		{
			num = MainGame.PlayerData.GetResInt("g_garden_farming_base");
		}
		Image image = this.bedIcon;
		EasySpritesCollection instance = LazySingletonSO<EasySpritesCollection>.Instance;
		int num2;
		if (gardenTypeFromWgo == CraftParamsData.GardenType.Vineyard)
		{
			num2 = MainGame.PlayerData.GetResInt("g_vineyard_lvl");
		}
		else
		{
			num2 = MainGame.PlayerData.GetResInt("g_garden_lvl");
		}
		image.sprite = instance.GetSprite("i_b_garden_bed_" + num2.ToString(), "i_placeholder");
		this.bedTalent.Draw(GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent), num, true, false);
		this.AttachGarden3Tooltip();
		this.bedIcon.BlueColorReplace(this.toReplace);
		if (this.data.IsGrowing)
		{
			this.workerIcon.SetTalentValue(GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent), num + this.GetFertilizersMasteryBonus());
			this.workerIcon2.SetTalentValue(GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent), num + this.GetFertilizersMasteryBonus());
		}
	}

	// Token: 0x06004369 RID: 17257 RVA: 0x00140B50 File Offset: 0x0013ED50
	private int GetFertilizersMasteryBonus()
	{
		int num = 0;
		foreach (UIGardenBedSlot uigardenBedSlot in this.perkWidgets)
		{
			if (uigardenBedSlot.PerkData != null)
			{
				num += uigardenBedSlot.PerkData.Definition.craftMasteryBonus;
			}
		}
		return num;
	}

	// Token: 0x0600436A RID: 17258 RVA: 0x00140BBC File Offset: 0x0013EDBC
	private void AttachGarden3Tooltip()
	{
		if (this.bedTalent == null)
		{
			return;
		}
		RectTransform rectTransform = (RectTransform)this.bedTalent.transform;
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
		RectTransform orCreateOverlay = UIMouseTooltip.GetOrCreateOverlay(rectTransform, "HoverArea");
		UIMouseTooltip.FitOverlayToPreferredSize(orCreateOverlay, rectTransform);
		orCreateOverlay.SetAsLastSibling();
		UIMouseTooltip.Attach(orCreateOverlay.gameObject, "tt_garden_3", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
	}

	// Token: 0x0600436B RID: 17259 RVA: 0x00140C2D File Offset: 0x0013EE2D
	private static CraftParamsData.GardenType GetGardenTypeFromWgo(WgoData wgoData)
	{
		if (wgoData.Definition.wgoGroup.Contains("vineyard_objects"))
		{
			return CraftParamsData.GardenType.Vineyard;
		}
		return CraftParamsData.GardenType.None;
	}

	// Token: 0x0600436C RID: 17260 RVA: 0x00140C4C File Offset: 0x0013EE4C
	private void UpdateProgressChanceBar()
	{
		if (!this.data.IsGrowing)
		{
			this.craftProgressObj.SetActive(false);
			return;
		}
		this.progressCellsInfoWidget.Hide();
		UIProgressCellsInfoWidgetData uiprogressCellsInfoWidgetData = new UIProgressCellsInfoWidgetData(this.data.CraftElement.ParamsData.MasteryValue, this.data.CraftDefinition.talentLock, GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent), true, (this.data.CraftElement.ParamsData.MasteryValue < this.data.CraftDefinition.talentLock) ? "tt_garden_5_part" : "tt_garden_5", 0);
		this.progressCellsInfoWidget.Draw(uiprogressCellsInfoWidgetData);
	}

	// Token: 0x0600436D RID: 17261 RVA: 0x00140D0C File Offset: 0x0013EF0C
	private void SubscribeToDataChanges()
	{
		if (!this.isSubscribedToDataChanges)
		{
			if (!this.data.IsGrowing)
			{
				return;
			}
			this.data.CraftElement.OnProgressChanged += this.UpdateProgressBar;
			this.data.CraftElement.OnProgressChanged += this.UpdateCraftOutput;
			this.isSubscribedToDataChanges = true;
		}
	}

	// Token: 0x0600436E RID: 17262 RVA: 0x00140D70 File Offset: 0x0013EF70
	private void UnSubscribeToDataChanges()
	{
		if (this.isSubscribedToDataChanges)
		{
			if (!this.data.IsGrowing)
			{
				return;
			}
			this.data.CraftElement.OnProgressChanged -= this.UpdateProgressBar;
			this.data.CraftElement.OnProgressChanged -= this.UpdateCraftOutput;
			this.isSubscribedToDataChanges = false;
		}
	}

	// Token: 0x0600436F RID: 17263 RVA: 0x00140DD4 File Offset: 0x0013EFD4
	private void UpdateCraftOutput()
	{
		if (!this.data.IsGrowing)
		{
			this.craftProgressObj.SetActive(false);
			this.outputItem.gameObject.SetActive(false);
			return;
		}
		OutputPreview outputPreview = this.data.CraftDefinition.GetOutputPreview(this.data.WgoData);
		this.outputItem.Draw(new Item(outputPreview.itemId, 1), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.outputItem.ShowMouseSelectionFrame = false;
		this.outputItem.TooltipPlacementPriority = TooltipPlacementPriority.BottomRight;
		this.outputItem.gameObject.SetActive(true);
		this.outputItem.GamepadNavigationItem.Active = true;
	}

	// Token: 0x06004370 RID: 17264 RVA: 0x00140E84 File Offset: 0x0013F084
	private void UpdatePerks()
	{
		int resInt = MainGame.PlayerData.GetResInt("g_garden_fertilizer_slots");
		bool flag = !this.data.IsGrowing && !GardenInteractionHandler.HasAssignedGardenOrder(this.data.WgoData);
		for (int i = 0; i < this.perkWidgets.Count; i++)
		{
			if (resInt > i)
			{
				PerkData perkData;
				if (this.HasPerkForSlot(i, out perkData))
				{
					this.perkWidgets[i].DrawFertilizerSlot(perkData, new Action<UIGardenBedSlot>(this.TryApplyFertilizer), flag);
				}
				else
				{
					this.perkWidgets[i].DrawEmpty(new Action<UIGardenBedSlot>(this.TryApplyFertilizer), flag);
				}
			}
			else
			{
				this.perkWidgets[i].DrawLocked();
			}
			this.perkWidgets[i].slotIndex = i;
		}
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x00140F51 File Offset: 0x0013F151
	protected override void PrintTips()
	{
		this.lazyButtonTips.Print(LazyGameKeyTip.Back(true, true, true));
	}

	// Token: 0x06004372 RID: 17266 RVA: 0x00140F68 File Offset: 0x0013F168
	private bool HasPerkForSlot(int uiSlotIndex, out PerkData slotPerk)
	{
		slotPerk = null;
		for (int i = 0; i < this.data.GardenPerks.Count; i++)
		{
			int gameResInt = this.data.WgoData.GetGameResInt("perk_fertilize_" + this.data.GardenPerks[i].Definition.id);
			if (gameResInt > 0 && gameResInt - 1 == uiSlotIndex)
			{
				slotPerk = this.data.GardenPerks[i];
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x00140FEC File Offset: 0x0013F1EC
	private void AddFertilizerPerk(UIItemCell cell, UIGardenBedSlot fertilizerSlot)
	{
		if (GardenInteractionHandler.HasAssignedGardenOrder(this.data.WgoData))
		{
			return;
		}
		if (fertilizerSlot.PerkData != null)
		{
			this.RemoveFertilizerPerk(fertilizerSlot.PerkData);
		}
		CraftDefBase craftDefBase = GardenInteractionHandler.TryFindGardenCraft(cell.DisplayingItem, this.data.WgoData, true);
		if (craftDefBase == null)
		{
			this.EndFertilizerApply();
			return;
		}
		this.data.TrySetWorker();
		if (GardenInteractionHandler.TryApplyFertilizer(cell.DisplayingItem, craftDefBase, this.data.WgoData))
		{
			foreach (PerkData perkData in this.data.WgoData.ActivePerks)
			{
				if (!this.data.GardenPerks.Contains(perkData))
				{
					this.data.WgoData.SetGameRes("perk_fertilize_" + perkData.Definition.id, fertilizerSlot.slotIndex + 1);
				}
			}
			this.EndFertilizerApply();
		}
		this.data.TryRemoveWorker();
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x00141100 File Offset: 0x0013F300
	private void RemoveFertilizerPerk(PerkData perk)
	{
		this.data.WgoData.RemovePerk(perk);
	}

	// Token: 0x06004375 RID: 17269 RVA: 0x00141114 File Offset: 0x0013F314
	private void TryApplyFertilizer(UIGardenBedSlot gardenFertilizerSlot)
	{
		if (this.data.CraftElement != null)
		{
			return;
		}
		if (GardenInteractionHandler.HasAssignedGardenOrder(this.data.WgoData))
		{
			return;
		}
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, delegate(UIItemCell x)
		{
			this.AddFertilizerPerk(x, gardenFertilizerSlot);
		}, new Func<Item, bool>(this.IsItemValid), true, null, null);
		window.Open(uimultiInventoryWindowData);
	}

	// Token: 0x06004376 RID: 17270 RVA: 0x00141187 File Offset: 0x0013F387
	private void EndFertilizerApply()
	{
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		this.data.TryRemoveWorker();
		this.data.UpdateData();
		this.Redraw();
	}

	// Token: 0x06004377 RID: 17271 RVA: 0x001411B0 File Offset: 0x0013F3B0
	private bool IsItemValid(Item item)
	{
		if (item == null || !item.IsFertilizer)
		{
			return false;
		}
		foreach (PerkData perkData in this.data.WgoData.ActivePerks)
		{
			string fertilizerItemId = perkData.Definition.fertilizerItemId;
			if (!string.IsNullOrEmpty(fertilizerItemId))
			{
				ItemDef dataOrNull = GameBalance.Me.GetDataOrNull<ItemDef>(fertilizerItemId);
				if (dataOrNull != null && dataOrNull.isFertilizer && dataOrNull.id == item.id)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06004378 RID: 17272 RVA: 0x00141258 File Offset: 0x0013F458
	private void UpdatePlantControls()
	{
		UIGardenBedWindow.SetOptionalActive(this.emptyBedBackObj, !this.data.IsGrowing);
		UIGardenBedWindow.SetOptionalActive(this.growingBedBackObj, this.data.IsGrowing);
		if (this.seedItemCell == null && this.plantButton == null)
		{
			return;
		}
		bool flag = !this.data.IsGrowing;
		UIGardenBedWindow.SetOptionalActive(this.emptyPlantCellObj, flag && (this.selectedSeed == null || this.selectedSeed.IsEmpty));
		if (this.seedItemCell != null)
		{
			this.seedItemCell.gameObject.SetActive(flag);
			if (flag)
			{
				this.UpdateSeedItemCell();
			}
		}
		if (this.plantButton != null)
		{
			this.plantButton.gameObject.SetActive(flag);
			if (flag)
			{
				this.plantButton.Draw(new UIDialogWindowData.ButtonData(new Action(this.OnPlantButtonPress), LLBase.L("ui_plant"), new Func<bool>(this.CanPlantSelectedSeed), true, GameKey.Fold, ""));
			}
		}
	}

	// Token: 0x06004379 RID: 17273 RVA: 0x00141370 File Offset: 0x0013F570
	private void UpdateSeedItemCell()
	{
		if (this.selectedSeed == null || this.selectedSeed.IsEmpty)
		{
			this.seedItemCell.DrawEmptyInteractable(false, false);
			this.seedItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnSeedItemCellPress);
			return;
		}
		int selectedSeedNeedCount = this.GetSelectedSeedNeedCount();
		int availableSeedCount = this.GetAvailableSeedCount(this.selectedSeed.id);
		this.seedItemCell.Draw(new Item(this.selectedSeed.id, selectedSeedNeedCount), true, availableSeedCount, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
		this.seedItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnSeedItemCellPress);
		this.seedItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.OnSeedItemCellPress2);
		this.seedItemCell.TooltipPlacementPriority = TooltipPlacementPriority.TopRight;
	}

	// Token: 0x0600437A RID: 17274 RVA: 0x00141434 File Offset: 0x0013F634
	private void OnSeedItemCellPress(UIItemCell cell)
	{
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, new Action<UIItemCell>(this.OnSeedSelected), new Func<Item, bool>(this.IsSeedValid), true, null, null);
		window.Open(uimultiInventoryWindowData);
	}

	// Token: 0x0600437B RID: 17275 RVA: 0x00141472 File Offset: 0x0013F672
	private void OnSeedItemCellPress2(UIItemCell cell)
	{
		this.selectedSeed = null;
		this.selectedSeedCraft = null;
		this.UpdatePlantControls();
	}

	// Token: 0x0600437C RID: 17276 RVA: 0x00141488 File Offset: 0x0013F688
	private void OnSeedSelected(UIItemCell cell)
	{
		this.selectedSeed = new Item(cell.DisplayingItem.id, 1);
		this.selectedSeedCraft = GardenInteractionHandler.TryFindGardenCraft(this.selectedSeed, this.data.WgoData, true);
		LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
		this.UpdatePlantControls();
	}

	// Token: 0x0600437D RID: 17277 RVA: 0x001414DC File Offset: 0x0013F6DC
	private bool IsSeedValid(Item item)
	{
		if (item == null || item.IsEmpty)
		{
			return false;
		}
		if (!GardenInteractionHandler.IsSeedableSeed(this.data.WgoData.id, item))
		{
			return false;
		}
		CraftDefBase craftDefBase = GardenInteractionHandler.TryFindGardenCraft(item, this.data.WgoData, false);
		return craftDefBase != null && craftDefBase.needItems.Count > 0;
	}

	// Token: 0x0600437E RID: 17278 RVA: 0x00141538 File Offset: 0x0013F738
	private bool CanPlantSelectedSeed()
	{
		if (this.data.IsGrowing || this.selectedSeed == null || this.selectedSeed.IsEmpty)
		{
			return false;
		}
		if (GardenInteractionHandler.HasAssignedGardenOrder(this.data.WgoData))
		{
			return false;
		}
		if (this.selectedSeedCraft == null)
		{
			this.selectedSeedCraft = GardenInteractionHandler.TryFindGardenCraft(this.selectedSeed, this.data.WgoData, true);
		}
		return this.selectedSeedCraft != null && this.selectedSeedCraft.needItems.Count != 0 && this.GetAvailableSeedCount(this.selectedSeed.id) >= this.GetSelectedSeedNeedCount();
	}

	// Token: 0x0600437F RID: 17279 RVA: 0x001415DA File Offset: 0x0013F7DA
	private int GetSelectedSeedNeedCount()
	{
		if (this.selectedSeedCraft == null || this.selectedSeedCraft.needItems.Count == 0)
		{
			return 0;
		}
		return this.selectedSeedCraft.needItems[0].GetCount(this.data.WgoData);
	}

	// Token: 0x06004380 RID: 17280 RVA: 0x00141619 File Offset: 0x0013F819
	private int GetAvailableSeedCount(string itemId)
	{
		return new MultiInventory(MainGame.PlayerData, true).GetTotalCount(itemId);
	}

	// Token: 0x06004381 RID: 17281 RVA: 0x0014162C File Offset: 0x0013F82C
	private void OnPlantButtonPress()
	{
		if (!this.CanPlantSelectedSeed())
		{
			return;
		}
		if (MainGame.PlayerController.GetMasteryLevelForTalentBranch("talent_green", null) <= 0)
		{
			Bubble.Talk(new PhraseData(true, null, "gardening_no_mastery", null, null, SpeechBubbleType.Think, UIBasicBubble.ForceCornerPosition.Auto, 0f, false));
			return;
		}
		this.data.TrySetWorker();
		this.data.WgoData.CraftComponent.Clear();
		bool flag = GardenInteractionHandler.TryApplySeed(this.selectedSeed, this.selectedSeedCraft, this.data.WgoData);
		this.data.TryRemoveWorker();
		if (!flag)
		{
			this.UpdatePlantControls();
			return;
		}
		LazyAudio.Play("planting");
		this.Close();
	}

	// Token: 0x06004382 RID: 17282 RVA: 0x001416D2 File Offset: 0x0013F8D2
	private static void SetOptionalActive(GameObject obj, bool value)
	{
		if (obj != null)
		{
			obj.SetActive(value);
		}
	}

	// Token: 0x06004383 RID: 17283 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003481 RID: 13441
	[SerializeField]
	private UIInfoWidget uiInfoWidget;

	// Token: 0x04003482 RID: 13442
	[SerializeField]
	private Image customBedImage;

	// Token: 0x04003483 RID: 13443
	[SerializeField]
	private Image customBedImageGamepad;

	// Token: 0x04003484 RID: 13444
	[SerializeField]
	private UIGardenBedSlot inputItem;

	// Token: 0x04003485 RID: 13445
	[SerializeField]
	private UIItemCell outputItem;

	// Token: 0x04003486 RID: 13446
	[SerializeField]
	private Transform progressBarParent;

	// Token: 0x04003487 RID: 13447
	[SerializeField]
	private TextMeshProUGUI tickDuration;

	// Token: 0x04003488 RID: 13448
	[SerializeField]
	private TextMeshProUGUI resultLabel;

	// Token: 0x04003489 RID: 13449
	[SerializeField]
	private TextMeshProUGUI fertilizingLabel;

	// Token: 0x0400348A RID: 13450
	[SerializeField]
	private TextMeshProUGUI masteryRequirementLabel;

	// Token: 0x0400348B RID: 13451
	[SerializeField]
	private UIProgressCellsInfoWidget progressCellsInfoWidget;

	// Token: 0x0400348C RID: 13452
	[SerializeField]
	private Color toReplace;

	// Token: 0x0400348D RID: 13453
	[SerializeField]
	private RectTransform slotsPos1;

	// Token: 0x0400348E RID: 13454
	[SerializeField]
	private RectTransform slotsPos2;

	// Token: 0x0400348F RID: 13455
	[SerializeField]
	private RectTransform slotsObj;

	// Token: 0x04003490 RID: 13456
	[SerializeField]
	private GameObject craftProgressObj;

	// Token: 0x04003491 RID: 13457
	[SerializeField]
	private UIItemCell seedItemCell;

	// Token: 0x04003492 RID: 13458
	[SerializeField]
	private UIDialogWindowButton plantButton;

	// Token: 0x04003493 RID: 13459
	[SerializeField]
	private GameObject emptyBedBackObj;

	// Token: 0x04003494 RID: 13460
	[SerializeField]
	private GameObject growingBedBackObj;

	// Token: 0x04003495 RID: 13461
	[SerializeField]
	private GameObject emptyPlantCellObj;

	// Token: 0x04003496 RID: 13462
	[SerializeField]
	[Space]
	private Image bedIcon;

	// Token: 0x04003497 RID: 13463
	[SerializeField]
	private UITalentIcon bedTalent;

	// Token: 0x04003498 RID: 13464
	[SerializeField]
	private UIWorkerIcon workerIcon;

	// Token: 0x04003499 RID: 13465
	[SerializeField]
	private UIWorkerIcon workerIcon2;

	// Token: 0x0400349A RID: 13466
	[SerializeField]
	private Image workerBack;

	// Token: 0x0400349B RID: 13467
	[SerializeField]
	private Image workerBack2;

	// Token: 0x0400349C RID: 13468
	[SerializeField]
	private Sprite workerBackCommon;

	// Token: 0x0400349D RID: 13469
	[SerializeField]
	private Sprite workerBackCommon2;

	// Token: 0x0400349E RID: 13470
	[SerializeField]
	private Sprite workerBackGrowing;

	// Token: 0x0400349F RID: 13471
	[SerializeField]
	private Sprite workerBackGrowing2;

	// Token: 0x040034A0 RID: 13472
	[SerializeField]
	private GameObject arrowDecor;

	// Token: 0x040034A1 RID: 13473
	[SerializeField]
	private GameObject leftDecor;

	// Token: 0x040034A2 RID: 13474
	[SerializeField]
	private List<UIGardenBedSlot> perkWidgets = new List<UIGardenBedSlot>();

	// Token: 0x040034A3 RID: 13475
	private List<ProgressCellCraft> progressCells = new List<ProgressCellCraft>();

	// Token: 0x040034A4 RID: 13476
	private bool isSubscribedToDataChanges;

	// Token: 0x040034A5 RID: 13477
	private bool customBedImageShown;

	// Token: 0x040034A6 RID: 13478
	private Item selectedSeed;

	// Token: 0x040034A7 RID: 13479
	private CraftDefBase selectedSeedCraft;
}
