using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200097F RID: 2431
[RequireComponent(typeof(LazyButton))]
public abstract class UIBaseCraftWidget<T> : LazyWidget<UIBaseCraftWidgetData> where T : UIBaseCraftWidgetData
{
	// Token: 0x170009AA RID: 2474
	// (get) Token: 0x0600403D RID: 16445 RVA: 0x001338E9 File Offset: 0x00131AE9
	public LazyButton WidgetButton
	{
		get
		{
			return this.widgetButton;
		}
	}

	// Token: 0x170009AB RID: 2475
	// (get) Token: 0x0600403E RID: 16446 RVA: 0x001338F1 File Offset: 0x00131AF1
	public LazyButton AddToQueueButton
	{
		get
		{
			return this.addToQueueButton;
		}
	}

	// Token: 0x170009AC RID: 2476
	// (get) Token: 0x0600403F RID: 16447 RVA: 0x001338F9 File Offset: 0x00131AF9
	public CraftDef CraftDef
	{
		get
		{
			return this.data.CraftDefinition;
		}
	}

	// Token: 0x06004040 RID: 16448 RVA: 0x00133908 File Offset: 0x00131B08
	public override void Init()
	{
		base.Init();
		this.itemIngredientPrefab.gameObject.SetActive(false);
		this.widgetButton.onEnter.AddListener(new UnityAction(this.OnOver));
		this.widgetButton.onExit.AddListener(new UnityAction(this.OnOut));
		this.widgetButton.onClick.AddListener(new UnityAction(this.OnPress));
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x00133980 File Offset: 0x00131B80
	public override void DeInit()
	{
		base.DeInit();
		this.widgetButton.onEnter.RemoveAllListeners();
		this.widgetButton.onExit.RemoveAllListeners();
		this.widgetButton.onClick.RemoveAllListeners();
	}

	// Token: 0x06004042 RID: 16450 RVA: 0x001339B8 File Offset: 0x00131BB8
	protected override void SetData(UIBaseCraftWidgetData data)
	{
		base.SetData(data);
		this.SubscribeToDataChanges();
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x001339C8 File Offset: 0x00131BC8
	public override void Redraw()
	{
		base.Redraw();
		this.onPress = this.data.OnPress;
		this.onOver = this.data.OnOver;
		this.onOut = this.data.OnOut;
		this.onPressPlusQueue = this.data.OnPressPlusQueue;
		this.onPressMinusQueue = this.data.OnPressMinusQueue;
		this.outputItem = UIPrefabsPooler.Instance.GetElementFromPool<UIItemCell>(this.outputItemContainer);
		this.outputItem.transform.SetParent(this.outputItemContainer);
		this.outputItem.DrawCraftOutput(this.data.CraftDefinition.GetOutputPreview(this.data.WgoData), -1, CraftStatus.OK, ItemType.None, 1, ItemRelatedWidgetState.NotSet, false);
		this.outputItem.GamepadNavigationItem.group = 1;
		foreach (UICraftItemCellData uicraftItemCellData in this.data.CraftItemCellsData)
		{
			UICraftItemCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UICraftItemCell>(this.ingredientsContainer.transform);
			elementFromPool.Draw(uicraftItemCellData, new Action(this.OnNeedItemChange), false);
			elementFromPool.GamepadNavigationItem.group = 1;
			this.displayedIngredients.Add(elementFromPool);
		}
		this.UpdateRequirements();
		this.UpdateTalenticon();
		this.UpdateProgressBar();
		this.UpdateProgressChanceBar();
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x00133B38 File Offset: 0x00131D38
	public override void Hide()
	{
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.Flush();
			uicraftItemCell.GamepadNavigationItem.group = 0;
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftItemCell>(uicraftItemCell);
		}
		this.outputItem.Flush(true);
		this.outputItem.GamepadNavigationItem.group = 0;
		UIPrefabsPooler.Instance.ReleaseElementToPool<UIItemCell>(this.outputItem);
		foreach (UICraftRequirementWidget uicraftRequirementWidget in this.craftRequirementWidgets)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UICraftRequirementWidget>(uicraftRequirementWidget);
		}
		this.craftRequirementWidgets.Clear();
		this.displayedIngredients.Clear();
		this.HideSelection();
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

	// Token: 0x06004045 RID: 16453 RVA: 0x00133C68 File Offset: 0x00131E68
	protected virtual void OnNeedItemChange()
	{
		this.UpdateProgressBar();
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x00133C70 File Offset: 0x00131E70
	protected void OnPress()
	{
		Action action = this.onPress;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06004047 RID: 16455 RVA: 0x00133C82 File Offset: 0x00131E82
	private void OnOver()
	{
		this.ShowSelection();
		Action action = this.onOver;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x00133C9A File Offset: 0x00131E9A
	private void OnOut()
	{
		this.HideSelection();
		Action action = this.onOut;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x00133CB4 File Offset: 0x00131EB4
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

	// Token: 0x0600404A RID: 16458 RVA: 0x00133D70 File Offset: 0x00131F70
	private void ShowSelection()
	{
		this.selectionFrame.gameObject.SetActive(true);
		if (!this.data.CraftDefinition.IsMultipleCraftsDisabled)
		{
			this.UpdateCraftCountElementsActiveStatus(true);
		}
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x00133D9C File Offset: 0x00131F9C
	private void HideSelection()
	{
		this.UpdateCraftCountElementsActiveStatus(false);
		this.selectionFrame.gameObject.SetActive(false);
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00133DB8 File Offset: 0x00131FB8
	private void UpdateCounters()
	{
		this.outputItem.OnMultiplierChange(this.data.CraftsCount, false);
		foreach (UICraftItemCell uicraftItemCell in this.displayedIngredients)
		{
			uicraftItemCell.SetMultiplierValue(this.data.CraftsCount);
		}
	}

	// Token: 0x0600404D RID: 16461 RVA: 0x00133E2C File Offset: 0x0013202C
	private void HideCells()
	{
		foreach (ProgressCell progressCell in this.progressCells)
		{
			progressCell.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<ProgressCell>(progressCell);
		}
		this.progressCells.Clear();
	}

	// Token: 0x0600404E RID: 16462 RVA: 0x00133E94 File Offset: 0x00132094
	private void UpdateProgressBar()
	{
		this.HideCells();
		int craftStartTicks = this.data.ParamsData.CraftStartTicks;
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
		foreach (NeedItemData needItemData in this.data.GetCurrentNeedItems())
		{
			if (!needItemData.IsGroup && needItemData.ItemDef != null && needItemData.ItemDef.qualityType == ItemDef.QualityType.Star && needItemData.ItemDef.quality > 1)
			{
				dictionary2.Add(needItemData.ItemDef, needItemData.ItemDef.quality - 1);
			}
		}
		int num2 = this.data.CraftDefinition.duration.EvaluateInt();
		for (int i = 0; i < num2; i++)
		{
			ProgressCell elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<ProgressCell>(this.progressBarParent);
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
			ItemDef itemDef = null;
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
						ItemDef itemDef2 = enumerator4.Current;
						itemDef = itemDef2;
						Dictionary<ItemDef, int> dictionary4 = dictionary2;
						ItemDef itemDef3 = itemDef2;
						dictionary4[itemDef3]--;
						if (dictionary2[itemDef2] == 0)
						{
							dictionary2.Remove(itemDef2);
						}
					}
				}
			}
			elementFromPool.Show(i, num2, i > craftStartTicks - 1, false, num3, perkDef, itemDef);
			this.progressCells.Add(elementFromPool);
		}
	}

	// Token: 0x0600404F RID: 16463 RVA: 0x001341DC File Offset: 0x001323DC
	private void UpdateTalenticon()
	{
		this.talentIcon.Draw(GameBalance.Me.GetData<TalentDef>(this.data.WgoData.Definition.talent), this.data.CraftDefinition.talentLock, this.data.DisplayableWorker.GetMasteryLevelForTalentBranch(this.data.WgoData.Definition.talent, this.data.CraftDefinition) >= this.data.CraftDefinition.talentLock, this.data.CraftDefinition.isStarCraft || this.data.CraftDefinition.isAutopsyCraft || this.data.CraftDefinition.isPocketExtractCraft);
	}

	// Token: 0x06004050 RID: 16464 RVA: 0x001342A0 File Offset: 0x001324A0
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

	// Token: 0x06004051 RID: 16465 RVA: 0x001343B6 File Offset: 0x001325B6
	private void Update()
	{
		if (this.data == null)
		{
			this.craftCountHold.Reset();
			return;
		}
		this.craftCountHold.Tick(HoldRepeatValueChanger.GetPointerHoldDirection(this.plusCraftButton, this.minusCraftButton), new Action<int>(this.ChangeCraftCount));
	}

	// Token: 0x06004052 RID: 16466 RVA: 0x001343F4 File Offset: 0x001325F4
	private void ChangeCraftCount(int delta)
	{
		if (delta == 0 || this.data == null)
		{
			return;
		}
		this.data.AddCraftsCount(delta);
		this.UpdateCounters();
		this.UpdateRequirements();
	}

	// Token: 0x06004053 RID: 16467 RVA: 0x0013441A File Offset: 0x0013261A
	private void UpdateCraftCountElementsActiveStatus(bool isActive)
	{
		this.plusCraftButton.gameObject.SetActive(isActive);
		this.minusCraftButton.gameObject.SetActive(isActive);
	}

	// Token: 0x06004054 RID: 16468 RVA: 0x0013443E File Offset: 0x0013263E
	private void SubscribeToDataChanges()
	{
		if (!this.subscribedToDataChanges)
		{
			this.data.SubscribeToDataChanges();
			this.subscribedToDataChanges = true;
		}
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x0013445A File Offset: 0x0013265A
	private void UnsubscribeFromDataChanges()
	{
		if (this.subscribedToDataChanges)
		{
			this.data.UnsubscribeFromDataChanges();
			this.subscribedToDataChanges = false;
		}
	}

	// Token: 0x04003272 RID: 12914
	private Action onPress;

	// Token: 0x04003273 RID: 12915
	private Action onOver;

	// Token: 0x04003274 RID: 12916
	private Action onOut;

	// Token: 0x04003275 RID: 12917
	private Action onPressPlusQueue;

	// Token: 0x04003276 RID: 12918
	private Action onPressMinusQueue;

	// Token: 0x04003277 RID: 12919
	[SerializeField]
	protected UIItemCell outputItem;

	// Token: 0x04003278 RID: 12920
	[SerializeField]
	protected Transform outputItemContainer;

	// Token: 0x04003279 RID: 12921
	[SerializeField]
	protected UICraftItemCell itemIngredientPrefab;

	// Token: 0x0400327A RID: 12922
	[SerializeField]
	protected Transform ingredientsContainer;

	// Token: 0x0400327B RID: 12923
	[SerializeField]
	protected Image selectionFrame;

	// Token: 0x0400327C RID: 12924
	[SerializeField]
	private Transform requirementsContainer;

	// Token: 0x0400327D RID: 12925
	[SerializeField]
	private GameObject requirementsGo;

	// Token: 0x0400327E RID: 12926
	[SerializeField]
	private LazyButton widgetButton;

	// Token: 0x0400327F RID: 12927
	[SerializeField]
	private LazyButton addToQueueButton;

	// Token: 0x04003280 RID: 12928
	[SerializeField]
	[Space]
	protected LazyButton plusCraftButton;

	// Token: 0x04003281 RID: 12929
	[SerializeField]
	protected LazyButton minusCraftButton;

	// Token: 0x04003282 RID: 12930
	[SerializeField]
	private UITalentIcon talentIcon;

	// Token: 0x04003283 RID: 12931
	[SerializeField]
	private RectTransform progressBarParent;

	// Token: 0x04003284 RID: 12932
	[SerializeField]
	private UIProgressCellsInfoWidget progressCellsInfoWidget;

	// Token: 0x04003285 RID: 12933
	protected List<UICraftItemCell> displayedIngredients = new List<UICraftItemCell>();

	// Token: 0x04003286 RID: 12934
	protected List<ProgressCell> progressCells = new List<ProgressCell>();

	// Token: 0x04003287 RID: 12935
	private List<UICraftRequirementWidget> craftRequirementWidgets = new List<UICraftRequirementWidget>();

	// Token: 0x04003288 RID: 12936
	private bool subscribedToDataChanges;

	// Token: 0x04003289 RID: 12937
	private readonly HoldRepeatValueChanger craftCountHold = new HoldRepeatValueChanger();
}
