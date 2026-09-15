using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008DD RID: 2269
public class UIAlchemyWindow : LazyWindow<UIAlchemyWindowData>
{
	// Token: 0x06003B21 RID: 15137 RVA: 0x0011A47C File Offset: 0x0011867C
	public override void Init()
	{
		base.Init();
		this.ingredientPrefab.gameObject.SetActive(false);
		this.createBtn.onClick.AddListener(new UnityAction(this.OnStartMix));
		this.AttachAlchemyMouseTooltips();
		LazyWindowsStackController.OnWindowClosed += delegate(LazyWidgetBase window)
		{
			if (base.IsShown && window is UICraftWindow)
			{
				this.RedrawAlchemyTabLite(null);
			}
		};
	}

	// Token: 0x06003B22 RID: 15138 RVA: 0x0011A4D4 File Offset: 0x001186D4
	private void AttachAlchemyMouseTooltips()
	{
		UIMouseTooltip.Attach(this.fuelResultLabel.transform.parent.gameObject, "tt_alchemy_3", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
		UIMouseTooltip.Attach(this.fuelBoostLabel.transform.parent.gameObject, "tt_alchemy_5", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
		UIMouseTooltip.Attach(this.smallFlaskObj.transform.parent.gameObject, "tt_alchemy_6", null, true, true, default(UIMouseTooltipEdges), default(Vector2), null);
		Transform transform = this.smallFlaskObj.transform.parent.parent.Find("Decor_2");
		if (transform != null)
		{
			UIMouseTooltip.Attach(transform.gameObject, "tt_alchemy_7", null, true, false, new UIMouseTooltipEdges(0f, 50f, 0f, 0f), default(Vector2), null);
		}
	}

	// Token: 0x06003B23 RID: 15139 RVA: 0x0011A5E4 File Offset: 0x001187E4
	public override void Open(UIAlchemyWindowData data)
	{
		base.Open(data);
		UIInfoWidgetData uiinfoWidgetData = new UIInfoWidgetData(data.Wgo.Data, null, true);
		this.uiInfoWidget.Draw(uiinfoWidgetData);
		this.DisplayAlchemyTab();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003B24 RID: 15140 RVA: 0x0011A62D File Offset: 0x0011882D
	public override void Close()
	{
		this.ClearPlayerWorker();
		this.craftCountHold.Reset();
		base.Close();
	}

	// Token: 0x06003B25 RID: 15141 RVA: 0x0011A646 File Offset: 0x00118846
	protected override void Update()
	{
		this.TickCraftCountHold();
		base.Update();
	}

	// Token: 0x06003B26 RID: 15142 RVA: 0x0011A654 File Offset: 0x00118854
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.AlchemyStart, new Func<bool>(this.OnStartAlchemyPressed));
		gameKeyDelegates.Add(GameKey.AlchemyBoost, new Func<bool>(this.OnAlchemyBoostPressed));
		gameKeyDelegates.Add(GameKey.Left, () => this.OnHorizontalPressed(GameKey.Left, GUIDirection.Left, false));
		gameKeyDelegates.Add(GameKey.Right, () => this.OnHorizontalPressed(GameKey.Right, GUIDirection.Right, true));
		gameKeyDelegates.Add(GameKey.DpadLeft, () => this.OnHorizontalPressed(GameKey.DpadLeft, GUIDirection.Left, false));
		gameKeyDelegates.Add(GameKey.DpadRight, () => this.OnHorizontalPressed(GameKey.DpadRight, GUIDirection.Right, true));
		return gameKeyDelegates;
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x0011A6F1 File Offset: 0x001188F1
	protected override void PrintTips()
	{
		this.PrintTips(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x0011A704 File Offset: 0x00118904
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		if (this.data == null)
		{
			return;
		}
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Select(true, true, true));
		list.Add(LazyGameKeyTip.Back(true, true, true));
		if (this.HasAlchemyBoosts())
		{
			list.Add(new LazyGameKeyTip(GameKey.AlchemyBoost, "ui_boost", true, true, true));
		}
		if (this.IsResultFocused(gamepadNavigationItem))
		{
			list.Add(new LazyGameKeyTip(GameKey.DpadRight, "+", this.plusBtn != null && this.plusBtn.interactable, true, false));
			list.Add(new LazyGameKeyTip(GameKey.DpadLeft, "-", this.minusBtn != null && this.minusBtn.interactable, true, false));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x0011A7DC File Offset: 0x001189DC
	private bool OnPlusPressed()
	{
		if (this.plusBtn == null || !this.plusBtn.gameObject.activeSelf || !this.plusBtn.interactable)
		{
			return false;
		}
		this.craftCountHold.Press(1, new Action<int>(this.ChangeCraftCount));
		return true;
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x0011A834 File Offset: 0x00118A34
	private bool OnMinusPressed()
	{
		if (this.minusBtn == null || !this.minusBtn.gameObject.activeSelf || !this.minusBtn.interactable)
		{
			return false;
		}
		this.craftCountHold.Press(-1, new Action<int>(this.ChangeCraftCount));
		return true;
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x0011A889 File Offset: 0x00118A89
	private bool OnHorizontalPressed(GameKey gameKey, GUIDirection direction, bool increase)
	{
		if (this.IsResultFocused())
		{
			if (increase)
			{
				this.OnPlusPressed();
			}
			else
			{
				this.OnMinusPressed();
			}
			return true;
		}
		if (base.GamepadNavigationController.ignoreHoldedKeys)
		{
			LazyInput.WaitForRelease(gameKey);
		}
		base.GamepadNavigationController.Navigate(direction);
		return true;
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x0011A8C8 File Offset: 0x00118AC8
	private bool OnStartAlchemyPressed()
	{
		if (this.createBtn.interactable)
		{
			this.OnStartMix();
			return true;
		}
		return false;
	}

	// Token: 0x06003B2D RID: 15149 RVA: 0x0011A8E0 File Offset: 0x00118AE0
	private bool OnAlchemyBoostPressed()
	{
		if (!this.HasAlchemyBoosts() || !base.IsShownAndTop)
		{
			return false;
		}
		this.OpenCraftWindow();
		return true;
	}

	// Token: 0x06003B2E RID: 15150 RVA: 0x0011A8FC File Offset: 0x00118AFC
	private bool HasAlchemyBoosts()
	{
		UIAlchemyWindowData data = this.data;
		bool flag;
		if (data == null)
		{
			flag = null != null;
		}
		else
		{
			Wgo wgo = data.Wgo;
			if (wgo == null)
			{
				flag = null != null;
			}
			else
			{
				WgoData data2 = wgo.Data;
				flag = ((data2 != null) ? data2.WorkbenchExtensionsCrafts : null) != null;
			}
		}
		return flag && this.data.Wgo.Data.WorkbenchExtensionsCrafts.Count > 0;
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x0011A954 File Offset: 0x00118B54
	private bool IsResultFocused()
	{
		return this.IsResultFocused(base.GamepadNavigationController.FocusedItem);
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x0011A968 File Offset: 0x00118B68
	private bool IsResultFocused(GamepadNavigationItem gamepadNavigationItem)
	{
		if (!LazyInput.IsGamepadActive || this.result == null || gamepadNavigationItem == null)
		{
			return false;
		}
		GamepadNavigationItem gamepadNavigationItem2 = this.result.GamepadNavigationItem;
		return gamepadNavigationItem2 != null && gamepadNavigationItem2.Active && gamepadNavigationItem == gamepadNavigationItem2;
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x0011A9BC File Offset: 0x00118BBC
	private void DisplayAlchemyTab()
	{
		bool flag = this.HasAlchemyBoosts();
		this.boostItemCell.gameObject.SetActive(flag);
		this.boostLockedObject.SetActive(!flag);
		this.craftCount = 1;
		this.sumBeforeItemSelect = default(Vector3Int);
		this.currentCraftElement = null;
		this.boostCraftElement = null;
		this.boostItemCell.DrawEmptyInteractable(false, false);
		this.boostEmptyObj.SetActive(true);
		this.boostItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnBoostBtnClick);
		this.boostItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.OnBoostBtnClick2);
		this.boostItemCell.CustomTooltipShowAction = delegate(UIItemCell cell)
		{
			if (this.boostCraftElement != null)
			{
				UITooltip.ShowAlchemyBoostInfo(cell.transform as RectTransform, this.boostCraftElement.Definition);
			}
		};
		this.UpdateBoostFuel();
		foreach (UIAlchemyIngredient uialchemyIngredient in this.ingredients)
		{
			uialchemyIngredient.gameObject.SetActive(false);
		}
		for (int i = 0; i < this.data.Wgo.Data.Definition.inventorySize; i++)
		{
			if (this.ingredients.Count < i + 1)
			{
				UIAlchemyIngredient uialchemyIngredient2 = global::UnityEngine.Object.Instantiate<UIAlchemyIngredient>(this.ingredientPrefab, this.ingredientPrefab.transform.parent);
				this.ingredients.Add(uialchemyIngredient2);
			}
			this.ingredients[i].gameObject.SetActive(true);
			this.ingredients[i].cell.DrawEmptyInteractable(false, false);
			this.ingredients[i].cell.OnItemCellPress = new Action<UIItemCell>(this.OnIngredientPressed);
			this.ingredients[i].cell.OnItemCellPress2 = new Action<UIItemCell>(this.OnIngredientPressed2);
			this.ingredients[i].plusObj.SetActive(true);
			UIMouseTooltip.Attach(this.ingredients[i].plusObj, "tt_alchemy_2", null, true, false, default(UIMouseTooltipEdges), default(Vector2), null);
		}
		this.isBigView = this.data.Wgo.Data.Definition.inventorySize > 2;
		if (this.isBigView)
		{
			this.bigFlaskObj.SetActive(true);
			this.smallFlaskObj.SetActive(false);
		}
		else
		{
			this.bigFlaskObj.SetActive(false);
			this.smallFlaskObj.SetActive(true);
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		this.RedrawAlchemyTabLite(null);
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x0011AC58 File Offset: 0x00118E58
	private void RedrawAlchemyTabLite(AlchemyMixDef mix = null)
	{
		this.UpdateCountInWindow();
		this.talentIconResult.gameObject.SetActive(false);
		List<string> list = new List<string>();
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			if (this.ingredients[i].gameObject.activeSelf && this.ingredients[i].cell.DisplayingItem != null && !this.ingredients[i].cell.DisplayingItem.IsEmpty)
			{
				list.Add(this.ingredients[i].cell.DisplayingItem.id);
			}
		}
		if (mix == null)
		{
			string[] array = list.ToArray();
			CraftElement craftElement = this.boostCraftElement;
			this.mixCraftId = AlchemyMixDef.MixId(array, (craftElement != null) ? craftElement.Definition : null);
		}
		else
		{
			this.mixCraftId = mix.id;
		}
		AlchemyMixDef alchemyMixDef = GameBalance.GetAlchemyMixDef(this.mixCraftId);
		bool flag = alchemyMixDef != null;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = this.data.Wgo.Data.GetCraftableMultiInventory(true).GetTotalCount("alchemy_flask") >= this.craftCount;
		this.fuelResultLabel.text = string.Format("{0}{1}", "alchemy_flask".FontIcon(), this.craftCount);
		if (flag4)
		{
			this.fuelResultEnoughStyle.ApplyStyle(this.fuelResultLabel, false, null, null, null);
		}
		else
		{
			this.fuelResultNotEnoughStyle.ApplyStyle(this.fuelResultLabel, false, null, null, null);
		}
		if (flag)
		{
			flag2 = true;
			flag3 = MainGame.PlayerController.WorkerMultiInventory.HasItemsById(this.GetNeedItems(), 1, null);
			Debug.Log(string.Format("#mix# Has mix with id: {0} enoughMastery:[{1}] enoughItems:[{2}] enoughFlasks:[{3}]", new object[] { this.mixCraftId, flag2, flag3, flag4 }));
			if (MainGame.Instance.GameSave.knowledgeSystem.IsAlchemyFormulaKnown(alchemyMixDef.Formula))
			{
				this.result.Draw(new Item(alchemyMixDef.ResultItem.id, this.craftCount), false, -1, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
				this.result.ShowMouseSelectionFrame = false;
				this.result.LazyButton.interactable = true;
				this.result.GamepadNavigationItem.Active = true;
				this.result.gameObject.SetActive(true);
				this.resultUnknown.gameObject.SetActive(false);
				this.emptyResultObj.SetActive(false);
				this.talentIconResult.gameObject.SetActive(true);
				this.talentIconResult.Draw(GameBalance.Me.GetData<TalentDef>("talent_blue"), alchemyMixDef.talentLock.ToString());
				UIMouseTooltip component = this.result.GetComponent<UIMouseTooltip>();
				if (component != null)
				{
					component.enabled = false;
				}
			}
			else
			{
				this.result.gameObject.SetActive(false);
				this.resultUnknown.gameObject.SetActive(true);
				this.emptyResultObj.SetActive(false);
				this.talentIconResult.gameObject.SetActive(true);
				this.talentIconResult.Draw(GameBalance.Me.GetData<TalentDef>("talent_blue"), alchemyMixDef.talentLock, flag2, false);
			}
		}
		else
		{
			Debug.Log("#mix# No mix with id: " + this.mixCraftId);
			this.result.DrawEmpty(true, true, false);
			this.result.NoSelectionFrames = true;
			this.result.LazyButton.interactable = false;
			this.result.GamepadNavigationItem.Active = false;
			this.result.gameObject.SetActive(true);
			this.resultUnknown.gameObject.SetActive(false);
			this.emptyResultObj.SetActive(true);
			UIMouseTooltip component2 = this.result.GetComponent<UIMouseTooltip>();
			if (component2 != null)
			{
				component2.SetLocalizationId(string.Empty);
			}
		}
		Vector3Int vector3Int = this.CalcSum();
		if (this.isBigView)
		{
			for (int j = 0; j < this.bigRedFill.Length; j++)
			{
				this.bigRedFill[j].SetActive(vector3Int.x > j);
			}
			for (int k = 0; k < this.bigGreenFill.Length; k++)
			{
				this.bigGreenFill[k].SetActive(vector3Int.y > k);
			}
			for (int l = 0; l < this.bigBlueFill.Length; l++)
			{
				this.bigBlueFill[l].SetActive(vector3Int.z > l);
			}
		}
		else
		{
			for (int m = 0; m < this.smallRedFill.Length; m++)
			{
				this.smallRedFill[m].SetActive(vector3Int.x > m);
			}
			for (int n = 0; n < this.smallGreenFill.Length; n++)
			{
				this.smallGreenFill[n].SetActive(vector3Int.y > n);
			}
			for (int num = 0; num < this.smallBlueFill.Length; num++)
			{
				this.smallBlueFill[num].SetActive(vector3Int.z > num);
			}
		}
		this.createBtn.interactable = flag && flag2 && flag3 && flag4;
		this.minusBtn.interactable = this.craftCount > 1 && flag;
		this.plusBtn.interactable = flag;
		if (LazyInput.IsGamepadActive)
		{
			this.startTip.text = new LazyGameKeyTip(GameKey.AlchemyStart, LLBase.L("hint_alchemy"), this.createBtn.interactable, true, false).ToString();
			this.PrintTips();
		}
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x0011B22C File Offset: 0x0011942C
	private void TickCraftCountHold()
	{
		if (!base.IsShownAndTop)
		{
			this.craftCountHold.Reset();
			return;
		}
		this.craftCountHold.Tick(this.GetCraftCountHoldDirection(), new Action<int>(this.ChangeCraftCount));
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x0011B260 File Offset: 0x00119460
	private int GetCraftCountHoldDirection()
	{
		int pointerHoldDirection = HoldRepeatValueChanger.GetPointerHoldDirection(this.plusBtn, this.minusBtn);
		if (pointerHoldDirection != 0)
		{
			return pointerHoldDirection;
		}
		if (!LazyInput.IsGamepadActive)
		{
			return 0;
		}
		if (!this.IsResultFocused())
		{
			return 0;
		}
		bool flag = this.plusBtn != null && this.plusBtn.interactable && (HoldRepeatValueChanger.IsAnyKeyHeld(GameKey.DpadRight, GameKey.Right) || HoldRepeatValueChanger.GetAxisHoldDirection(false) > 0);
		bool flag2 = this.minusBtn != null && this.minusBtn.interactable && (HoldRepeatValueChanger.IsAnyKeyHeld(GameKey.DpadLeft, GameKey.Left) || HoldRepeatValueChanger.GetAxisHoldDirection(false) < 0);
		if (flag == flag2)
		{
			return 0;
		}
		if (!flag)
		{
			return -1;
		}
		return 1;
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x0011B31C File Offset: 0x0011951C
	private void ChangeCraftCount(int delta)
	{
		if (delta == 0)
		{
			return;
		}
		int num = Math.Clamp(this.craftCount + delta, 1, 999);
		if (num == this.craftCount)
		{
			return;
		}
		this.craftCount = num;
		this.RedrawAlchemyTabLite(null);
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x0011B359 File Offset: 0x00119559
	private void OnBoostBtnClick(UIItemCell cell)
	{
		this.OpenCraftWindow();
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x0011B364 File Offset: 0x00119564
	private void OnBoostBtnClick2(UIItemCell cell)
	{
		this.boostItemCell.DrawEmptyInteractable(false, false);
		this.boostEmptyObj.SetActive(true);
		this.boostItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnBoostBtnClick);
		this.boostItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.OnBoostBtnClick2);
		this.boostItemCell.CustomTooltipShowAction = delegate(UIItemCell cell)
		{
			if (this.boostCraftElement != null)
			{
				UITooltip.ShowAlchemyBoostInfo(cell.transform as RectTransform, this.boostCraftElement.Definition);
			}
		};
		this.boostCraftElement = null;
		this.RedrawAlchemyTabLite(null);
		this.UpdateBoostFuel();
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x0011B3E4 File Offset: 0x001195E4
	private void UpdateBoostFuel()
	{
		int num = 0;
		if (this.boostCraftElement != null)
		{
			NeedItemData needItemData = this.boostCraftElement.Def.needItems.Find((NeedItemData i) => i.id == "fire");
			if (needItemData != null)
			{
				num = needItemData.GetCount(this.data.Wgo.Data);
			}
		}
		this.fuelBoostLabel.text = string.Format("{0}{1}", "fire".FontIcon(), num);
	}

	// Token: 0x06003B39 RID: 15161 RVA: 0x0011B470 File Offset: 0x00119670
	private void OpenCraftWindow()
	{
		UIAlchemyBoostsWindow boostsWindow = LazyUI.GetWindow<UIAlchemyBoostsWindow>();
		bool wasPlayerSetAsWorker = false;
		if (this.data.Wgo.Data.Worker == null)
		{
			this.data.Wgo.Data.TrySetWorker(MainGame.PlayerController, null);
			wasPlayerSetAsWorker = true;
		}
		List<CraftElement> list2 = new List<CraftElement>();
		foreach (string text in this.data.Wgo.Data.WorkbenchExtensionsCrafts.Keys)
		{
			foreach (CraftDefBase craftDefBase in this.data.Wgo.Data.WorkbenchExtensionsCrafts[text])
			{
				if (!craftDefBase.id.StartsWith("mix") && craftDefBase.id.EndsWith("_boost"))
				{
					CraftDef craftDef = craftDefBase as CraftDef;
					if (craftDef != null)
					{
						list2.Add(new CraftElement(craftDef, new CraftParamsData(craftDef.id, this.data.Wgo.Data, CraftParamsData.CraftParamsType.Common, -1)));
					}
				}
			}
		}
		Action<UIItemCell> <>9__3;
		UIAlchemyBoostsWindowData uialchemyBoostsWindowData = new UIAlchemyBoostsWindowData(this.data.Wgo, MainGame.PlayerData, list2, delegate(CraftElement element, List<NeedItemData> list)
		{
			if (element.Definition.id.EndsWith("_boost"))
			{
				this.boostCraftElement = element;
				this.boostItemCell.DrawCustom(element.Definition.GetCraftResultIcon(null), 1, true, false);
				this.boostEmptyObj.SetActive(false);
				this.boostItemCell.OnItemCellPress = new Action<UIItemCell>(this.OnBoostBtnClick);
				this.boostItemCell.OnItemCellPress2 = new Action<UIItemCell>(this.OnBoostBtnClick2);
				UIItemCell uiitemCell = this.boostItemCell;
				Action<UIItemCell> action;
				if ((action = <>9__3) == null)
				{
					action = (<>9__3 = delegate(UIItemCell cell)
					{
						if (this.boostCraftElement != null)
						{
							UITooltip.ShowAlchemyBoostInfo(cell.transform as RectTransform, this.boostCraftElement.Definition);
						}
					});
				}
				uiitemCell.CustomTooltipShowAction = action;
				this.UpdateBoostFuel();
				boostsWindow.Close();
				this.RedrawAlchemyTabLite(null);
			}
		}, (CraftElement craftElement, List<NeedItemData> list) => MainGame.PlayerController.WorkerMultiInventory.HasItemsById(list, this.data.Wgo.Data), null);
		boostsWindow.Open(uialchemyBoostsWindowData, delegate(UIAlchemyBoostsWindowData _)
		{
			if (wasPlayerSetAsWorker)
			{
				this.data.Wgo.Data.ClearWorker();
			}
		});
	}

	// Token: 0x06003B3A RID: 15162 RVA: 0x0011B630 File Offset: 0x00119830
	private void ClearPlayerWorker()
	{
		UIAlchemyWindowData data = this.data;
		PlayerController playerController;
		if (data == null)
		{
			playerController = null;
		}
		else
		{
			Wgo wgo = data.Wgo;
			if (wgo == null)
			{
				playerController = null;
			}
			else
			{
				WgoData data2 = wgo.Data;
				playerController = ((data2 != null) ? data2.Worker : null);
			}
		}
		if (playerController == MainGame.PlayerController)
		{
			this.data.Wgo.Data.ClearWorker();
		}
	}

	// Token: 0x06003B3B RID: 15163 RVA: 0x0011B684 File Offset: 0x00119884
	private Vector3Int CalcSum()
	{
		Vector3Int vector3Int = default(Vector3Int);
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			if (this.ingredients[i].gameObject.activeSelf && this.ingredients[i].cell.DisplayingItem != null && !this.ingredients[i].cell.DisplayingItem.IsEmpty)
			{
				vector3Int += this.ingredients[i].cell.DisplayingItem.Definition.GetRunesAsVector3Int();
			}
		}
		if (this.boostCraftElement != null)
		{
			vector3Int += this.boostCraftElement.Definition.GetBoostRunesAsVector3Int();
		}
		return vector3Int;
	}

	// Token: 0x06003B3C RID: 15164 RVA: 0x0011B748 File Offset: 0x00119948
	private void OnIngredientPressed(UIItemCell ingredient)
	{
		this.sumBeforeItemSelect = default(Vector3Int);
		for (int i = 0; i < this.ingredients.Count; i++)
		{
			if (this.ingredients[i] != ingredient.GetComponentInParent<UIAlchemyIngredient>() && this.ingredients[i].gameObject.activeSelf && this.ingredients[i].cell.DisplayingItem != null && !this.ingredients[i].cell.DisplayingItem.IsEmpty)
			{
				this.sumBeforeItemSelect += this.ingredients[i].cell.DisplayingItem.Definition.GetRunesAsVector3Int();
			}
		}
		LazyWindow<UIMultiInventoryWindowData> window = LazyUI.GetWindow<UIMultiInventoryWindow>();
		UIMultiInventoryWindowData uimultiInventoryWindowData = new UIMultiInventoryWindowData(MainGame.PlayerData, delegate(UIItemCell uiItemCell)
		{
			this.craftCount = 1;
			ingredient.Draw(new Item(uiItemCell.DisplayingItem.id, 1), true, uiItemCell.DisplayingItem.Count, false, 1, false, 0, true, false, false, ItemRelatedWidgetState.NotSet, false);
			ingredient.GetComponentInParent<UIAlchemyIngredient>().plusObj.SetActive(false);
			LazyUI.GetWindow<UIMultiInventoryWindow>().Close();
			this.RedrawAlchemyTabLite(null);
		}, new Func<Item, bool>(this.IsItemValidForMix), true, null, null);
		window.Open(uimultiInventoryWindowData);
	}

	// Token: 0x06003B3D RID: 15165 RVA: 0x0011B860 File Offset: 0x00119A60
	private void OnIngredientPressed2(UIItemCell ingredient)
	{
		this.craftCount = 1;
		ingredient.DrawEmptyInteractable(false, false);
		ingredient.OnItemCellPress = new Action<UIItemCell>(this.OnIngredientPressed);
		ingredient.OnItemCellPress2 = new Action<UIItemCell>(this.OnIngredientPressed2);
		ingredient.GetComponentInParent<UIAlchemyIngredient>().plusObj.SetActive(true);
		this.RedrawAlchemyTabLite(null);
	}

	// Token: 0x06003B3E RID: 15166 RVA: 0x0011B8B8 File Offset: 0x00119AB8
	private void OnStartMix()
	{
		AlchemyMixDef alchemyMixDef = GameBalance.GetAlchemyMixDef(this.mixCraftId);
		Debug.Log(string.Format("OnStartMix mixCraftId:[{0}] mixDef == null:[{1}]", this.mixCraftId, alchemyMixDef == null));
		if (alchemyMixDef == null)
		{
			Debug.LogError("#alch# Can't start mix, mixDef is missing. mixCraftId:[" + this.mixCraftId + "]");
			return;
		}
		CraftDef alchemyWorkBenchCraft = alchemyMixDef.AlchemyWorkBenchCraft;
		AlchemyFormulaDef formula = alchemyMixDef.Formula;
		if (alchemyWorkBenchCraft == null || formula == null)
		{
			Debug.LogError(string.Format("#alch# Can't start mix [{0}], invalid related defs. AlchemyWorkBenchCraft null:[{1}] Formula null:[{2}]", this.mixCraftId, alchemyWorkBenchCraft == null, formula == null));
			return;
		}
		if (this.boostCraftElement != null && this.data.Wgo.Data.CraftComponent.TryStartCraft(this.boostCraftElement))
		{
			this.data.Wgo.Data.CraftComponent.TryFinishCurCraft();
		}
		this.currentCraftElement = new CraftElementMix(alchemyMixDef, new CraftParamsData(this.mixCraftId, this.data.Wgo.Data, CraftParamsData.CraftParamsType.Common, -1));
		this.currentCraftElement.Count = this.craftCount;
		MainGame.PlayerController.WorkerMultiInventory.RemoveItems(this.GetNeedItems(), 1, null);
		this.data.Wgo.Data.GetCraftableMultiInventory(true).RemoveItem(new Item("alchemy_flask", this.craftCount));
		if (this.data.Wgo.Data.CraftComponent.TryStartCraft(this.currentCraftElement))
		{
			this.data.Wgo.Data.ClearWorker();
			this.Close();
		}
	}

	// Token: 0x06003B3F RID: 15167 RVA: 0x0011BA48 File Offset: 0x00119C48
	private List<NeedItemData> GetNeedItems()
	{
		AlchemyMixDef alchemyMixDef = GameBalance.GetAlchemyMixDef(this.mixCraftId);
		List<NeedItemData> list = new List<NeedItemData>();
		for (int i = 0; i < alchemyMixDef.ingredients.Length; i++)
		{
			list.Add(new NeedItemData(alchemyMixDef.ingredients[i], this.craftCount));
		}
		return list;
	}

	// Token: 0x06003B40 RID: 15168 RVA: 0x0011BA94 File Offset: 0x00119C94
	private bool IsItemValidForMix(Item ingredient)
	{
		if (ingredient == null)
		{
			return false;
		}
		if (ingredient.IsEmpty)
		{
			return false;
		}
		if (!ingredient.Definition.canBeUsedInAlchemy)
		{
			return false;
		}
		Vector3Int runesAsVector3Int = ingredient.Definition.GetRunesAsVector3Int();
		SurveyDef surveyDefForItemOrNull = GameBalance.GetSurveyDefForItemOrNull(ingredient.Definition.id);
		if (surveyDefForItemOrNull == null && runesAsVector3Int != Vector3Int.zero)
		{
			return false;
		}
		if (surveyDefForItemOrNull != null && !MainGame.Instance.GameSave.knowledgeSystem.IsSurveyCompleted(surveyDefForItemOrNull))
		{
			return false;
		}
		Vector3Int vector3Int = runesAsVector3Int + this.sumBeforeItemSelect;
		return vector3Int.x <= 5 && vector3Int.y <= 5 && vector3Int.z <= 5 && vector3Int.x >= 0 && vector3Int.y >= 0 && vector3Int.z >= 0;
	}

	// Token: 0x06003B41 RID: 15169 RVA: 0x0011BB58 File Offset: 0x00119D58
	private void UpdateCountInWindow()
	{
		foreach (UIAlchemyIngredient uialchemyIngredient in this.ingredients)
		{
			if (uialchemyIngredient.gameObject.gameObject.activeSelf && uialchemyIngredient.cell.DisplayingItem != null && !uialchemyIngredient.cell.DisplayingItem.IsEmpty)
			{
				int num = 1;
				foreach (UIAlchemyIngredient uialchemyIngredient2 in this.ingredients)
				{
					if (!(uialchemyIngredient2 == uialchemyIngredient) && uialchemyIngredient.gameObject.gameObject.activeSelf && uialchemyIngredient.cell.DisplayingItem != null && !uialchemyIngredient.cell.DisplayingItem.IsEmpty && uialchemyIngredient2.gameObject.gameObject.activeSelf && uialchemyIngredient2.cell.DisplayingItem != null && !uialchemyIngredient2.cell.DisplayingItem.IsEmpty && uialchemyIngredient2.cell.DisplayingItem.id == uialchemyIngredient.cell.DisplayingItem.id)
					{
						num++;
					}
				}
				uialchemyIngredient.cell.OnMultiplierChange(this.craftCount * num, false);
			}
		}
		if (this.result.DisplayingItem != null && !this.result.DisplayingItem.IsEmpty)
		{
			this.result.OnMultiplierChange(this.craftCount, false);
		}
	}

	// Token: 0x06003B42 RID: 15170 RVA: 0x0011BD24 File Offset: 0x00119F24
	[LazyUITest]
	protected override void TestDraw()
	{
		WgoData wgoData = new WgoData("alchemy_mix", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		wgoData.Inventory.AddItemToInventory(new Item("alchemy_flask", 5), null, false);
		AlchemyInteractionHandler alchemyInteractionHandler = new AlchemyInteractionHandler();
		alchemyInteractionHandler.Init(wgo);
		alchemyInteractionHandler.HasInteraction(MainGame.PlayerController);
		alchemyInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x06003B43 RID: 15171 RVA: 0x0011BDA8 File Offset: 0x00119FA8
	[LazyUITest]
	protected void TestDrawFull()
	{
		WgoData wgoData = new WgoData("alchemy_mix_2", MainGame.PlayerController.MovablePosition, MainGame.PlayerData.currentGameSceneId);
		Wgo wgo = Wgo.Spawn(wgoData, MainGame.PlayerController.CurrentGameScene.transform, true, false, false, false);
		wgoData.Inventory.AddItemToInventory(new Item("alchemy_flask", 10), null, false);
		AlchemyInteractionHandler alchemyInteractionHandler = new AlchemyInteractionHandler();
		alchemyInteractionHandler.Init(wgo);
		alchemyInteractionHandler.HasInteraction(MainGame.PlayerController);
		alchemyInteractionHandler.Interact(MainGame.PlayerController);
	}

	// Token: 0x04002EB8 RID: 11960
	[Space]
	[SerializeField]
	private UIInfoWidget uiInfoWidget;

	// Token: 0x04002EB9 RID: 11961
	[Space]
	[SerializeField]
	private UIAlchemyIngredient ingredientPrefab;

	// Token: 0x04002EBA RID: 11962
	[SerializeField]
	private UIItemCell result;

	// Token: 0x04002EBB RID: 11963
	[SerializeField]
	private GameObject emptyResultObj;

	// Token: 0x04002EBC RID: 11964
	[SerializeField]
	private GameObject resultUnknown;

	// Token: 0x04002EBD RID: 11965
	[SerializeField]
	private TextMeshProUGUI fuelResultLabel;

	// Token: 0x04002EBE RID: 11966
	[SerializeField]
	private TextStyle fuelResultEnoughStyle;

	// Token: 0x04002EBF RID: 11967
	[SerializeField]
	private TextStyle fuelResultNotEnoughStyle;

	// Token: 0x04002EC0 RID: 11968
	[SerializeField]
	private UITalentIcon talentIconResult;

	// Token: 0x04002EC1 RID: 11969
	[Space]
	[SerializeField]
	private LazyButton createBtn;

	// Token: 0x04002EC2 RID: 11970
	[SerializeField]
	private UIItemCell boostItemCell;

	// Token: 0x04002EC3 RID: 11971
	[SerializeField]
	private GameObject boostEmptyObj;

	// Token: 0x04002EC4 RID: 11972
	[SerializeField]
	private GameObject boostLockedObject;

	// Token: 0x04002EC5 RID: 11973
	[SerializeField]
	private TextMeshProUGUI fuelBoostLabel;

	// Token: 0x04002EC6 RID: 11974
	[SerializeField]
	private LazyButton plusBtn;

	// Token: 0x04002EC7 RID: 11975
	[SerializeField]
	private LazyButton minusBtn;

	// Token: 0x04002EC8 RID: 11976
	[SerializeField]
	private TextMeshProUGUI startTip;

	// Token: 0x04002EC9 RID: 11977
	[SerializeField]
	private GameObject smallFlaskObj;

	// Token: 0x04002ECA RID: 11978
	[SerializeField]
	private GameObject bigFlaskObj;

	// Token: 0x04002ECB RID: 11979
	[SerializeField]
	private GameObject[] smallRedFill;

	// Token: 0x04002ECC RID: 11980
	[SerializeField]
	private GameObject[] smallGreenFill;

	// Token: 0x04002ECD RID: 11981
	[SerializeField]
	private GameObject[] smallBlueFill;

	// Token: 0x04002ECE RID: 11982
	[SerializeField]
	private GameObject[] bigRedFill;

	// Token: 0x04002ECF RID: 11983
	[SerializeField]
	private GameObject[] bigGreenFill;

	// Token: 0x04002ED0 RID: 11984
	[SerializeField]
	private GameObject[] bigBlueFill;

	// Token: 0x04002ED1 RID: 11985
	private List<UIAlchemyIngredient> ingredients = new List<UIAlchemyIngredient>();

	// Token: 0x04002ED2 RID: 11986
	private int craftCount;

	// Token: 0x04002ED3 RID: 11987
	private Vector3Int sumBeforeItemSelect;

	// Token: 0x04002ED4 RID: 11988
	private CraftElementBase currentCraftElement;

	// Token: 0x04002ED5 RID: 11989
	private CraftElement boostCraftElement;

	// Token: 0x04002ED6 RID: 11990
	private string mixCraftId;

	// Token: 0x04002ED7 RID: 11991
	private bool isBigView;

	// Token: 0x04002ED8 RID: 11992
	private readonly HoldRepeatValueChanger craftCountHold = new HoldRepeatValueChanger();
}
