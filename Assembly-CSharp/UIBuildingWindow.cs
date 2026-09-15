using System;
using System.Collections.Generic;
using DG.Tweening;
using LazyBearTechnology;
using LinqTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000904 RID: 2308
public class UIBuildingWindow : LazyWindow<UIBuildingWindowData>
{
	// Token: 0x06003C7C RID: 15484 RVA: 0x00120FCF File Offset: 0x0011F1CF
	public override void Init()
	{
		base.Init();
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, this.buildingElement, 70f);
		this.buildingElement.gameObject.SetActive(false);
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x06003C7D RID: 15485 RVA: 0x0012100C File Offset: 0x0011F20C
	public override void Redraw()
	{
		this.onBuildPressed = this.data.OnBuildPressed;
		this.canBuild = this.data.CanBuild;
		this.multiInventory = new MultiInventory(this.data.PlayerData, true);
		if (this.data.AdditionalInventories != null)
		{
			foreach (Inventory inventory in this.data.AdditionalInventories)
			{
				this.multiInventory.Add(inventory);
			}
		}
		this.tabs = this.data.TabSortedBuilds.Keys.ToList<string>();
		this.MoveDefaultTabFirst();
		this.tabsDrawn = this.tabs.Count > 1;
		this.currentTab = this.GetInitialTabIndex();
		if (this.tabsDrawn)
		{
			this.tabsContainer.SetActive(true);
			foreach (string text in this.tabs)
			{
				BuildingWindowTab elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<BuildingWindowTab>(this.tabsContentRectTransform.transform);
				this.drawnTabs.Add(elementFromPool);
				elementFromPool.Init(text, new Action<BuildingWindowTab>(this.OnTabPressed));
				elementFromPool.UpdateState(false, this.canvas);
			}
			this.drawnTabs[this.currentTab].UpdateState(true, this.canvas);
		}
		else
		{
			this.tabsContainer.SetActive(false);
		}
		this.DrawTab(this.currentTab);
		this.UpdateNoBuildingsObject();
		this.uiInfoWidget.Draw(new UIInfoWidgetData(this.data.AssignedWgo.Data, null, false));
		bool flag = this.data.AssignedWgo.Data.Definition.interactionType == WGODef.InteractionType.FightBuilder && LazySingleton<FightingGameController>.Instance.CurrentFightState == FightState.InPreFight;
		this.fightButtonsContainer.gameObject.SetActive(flag);
		if (flag)
		{
			UIDialogWindowData.ButtonData buttonData = new UIDialogWindowData.ButtonData(new Action(this.StartFight), LLBase.L("start_fight_two_lines"), null, true, GameKey.StartFight, LLBase.L("start_fight"));
			UIDialogWindowData.ButtonData buttonData2 = new UIDialogWindowData.ButtonData(new Action(this.EndPreFight), LLBase.L("end_pre_fight"), null, true, GameKey.EndPrefight, "");
			this.startFightButton.Draw(buttonData);
			this.endPreFightButton.Draw(buttonData2);
		}
		base.Redraw();
		((RectTransform)base.transform).RefreshContentFitter();
		if (flag)
		{
			this.FitAndEqualizeChildrenInHorizontalGroup(this.fightButtonsRectTransform);
			if (LazyInput.IsGamepadActive)
			{
				for (int i = 0; i < this.fightButtonsRectTransform.childCount; i++)
				{
					this.fightButtonsRectTransform.GetChild(i).GetComponent<LayoutElement>().preferredWidth = -1f;
				}
			}
		}
	}

	// Token: 0x06003C7E RID: 15486 RVA: 0x00121300 File Offset: 0x0011F500
	public void UpdateCurrentTab(string tabId)
	{
		if (!this.tabsDrawn)
		{
			return;
		}
		this.drawnTabs[this.currentTab].UpdateState(false, this.canvas);
		this.currentTab = this.drawnTabs.IndexOf(this.drawnTabs.Find((BuildingWindowTab t) => t.TabId == tabId));
		this.drawnTabs[this.currentTab].UpdateState(true, this.canvas);
	}

	// Token: 0x06003C7F RID: 15487 RVA: 0x00121385 File Offset: 0x0011F585
	private void OnTabPressed(BuildingWindowTab tab)
	{
		this.DrawTab(this.tabs.IndexOf(tab.TabId));
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x001213A0 File Offset: 0x0011F5A0
	private void DrawTab(int tab)
	{
		this.HideDisplayedBuildItems();
		if (this.foldedElement != null)
		{
			this.foldedElement.Fold();
			this.foldedElement = null;
		}
		if (tab < 0)
		{
			return;
		}
		UIBuildingWindow.lastOpenedTabByWgoId[this.data.AssignedWgo.Data.id] = this.tabs[tab];
		foreach (BuildData buildData in this.data.TabSortedBuilds[this.tabs[tab]])
		{
			UIBuildingWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIBuildingWidget>(this.buildingsListContent.transform);
			this.displayedBuildItemGUIs.Add(elementFromPool);
			UIBuildingWidgetData uibuildingWidgetData = new UIBuildingWidgetData(buildData, this.multiInventory, new Action<BuildData, List<NeedItemData>>(this.OnBuildPressed), this.canBuild, null, null, this.data.AssignedWgo.Data.WorldZoneData);
			elementFromPool.Init();
			elementFromPool.Draw(uibuildingWidgetData);
		}
		if (this.tabsDrawn)
		{
			this.UpdateCurrentTab(this.tabs[tab]);
		}
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		this.scrollRect.DOKill(false);
		this.scrollRect.verticalNormalizedPosition = 1f;
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x00121508 File Offset: 0x0011F708
	private int GetInitialTabIndex()
	{
		if (this.tabs.Count == 0)
		{
			return -1;
		}
		string text;
		if (UIBuildingWindow.lastOpenedTabByWgoId.TryGetValue(this.data.AssignedWgo.Data.id, out text))
		{
			int num = this.tabs.IndexOf(text);
			if (num >= 0)
			{
				return num;
			}
		}
		return 0;
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x0012155C File Offset: 0x0011F75C
	private void MoveDefaultTabFirst()
	{
		int num = this.tabs.IndexOf("tab_building_default");
		if (num <= 0)
		{
			return;
		}
		this.tabs.RemoveAt(num);
		this.tabs.Insert(0, "tab_building_default");
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x0012159C File Offset: 0x0011F79C
	private void HideDisplayedBuildItems()
	{
		foreach (UIBuildingWidget uibuildingWidget in this.displayedBuildItemGUIs)
		{
			uibuildingWidget.DeInit();
			uibuildingWidget.Hide();
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIBuildingWidget>(uibuildingWidget);
		}
		this.displayedBuildItemGUIs.Clear();
	}

	// Token: 0x06003C84 RID: 15492 RVA: 0x0012160C File Offset: 0x0011F80C
	private void FitAndEqualizeChildrenInHorizontalGroup(RectTransform rectTransform)
	{
		if (rectTransform == null)
		{
			return;
		}
		for (int i = 0; i < rectTransform.childCount; i++)
		{
			Transform child = rectTransform.GetChild(i);
			Transform child2 = child.GetChild(0);
			child2.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			child2.GetComponent<LayoutElement>().preferredWidth = -1f;
			child.GetComponent<LayoutElement>().preferredWidth = -1f;
		}
		rectTransform.RefreshContentFitter();
		float num = 0f;
		for (int j = 0; j < rectTransform.childCount; j++)
		{
			RectTransform rectTransform2 = rectTransform.GetChild(j) as RectTransform;
			num = Mathf.Max(num, rectTransform2.rect.width);
		}
		for (int k = 0; k < rectTransform.childCount; k++)
		{
			Transform child3 = rectTransform.GetChild(k);
			Transform child4 = child3.GetChild(0);
			child4.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
			child4.GetComponent<LayoutElement>().preferredWidth = num;
			child3.GetComponent<LayoutElement>().preferredWidth = num;
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
	}

	// Token: 0x06003C85 RID: 15493 RVA: 0x001216F8 File Offset: 0x0011F8F8
	public override void Hide()
	{
		this.HideDisplayedBuildItems();
		foreach (BuildingWindowTab buildingWindowTab in this.drawnTabs)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<BuildingWindowTab>(buildingWindowTab);
		}
		this.drawnTabs.Clear();
		base.Hide();
	}

	// Token: 0x06003C86 RID: 15494 RVA: 0x00121768 File Offset: 0x0011F968
	private void OnBuildPressed(BuildData buildData, List<NeedItemData> needItems)
	{
		Action<BuildData, List<NeedItemData>> action = this.onBuildPressed;
		if (action == null)
		{
			return;
		}
		action(buildData, needItems);
	}

	// Token: 0x06003C87 RID: 15495 RVA: 0x0012177C File Offset: 0x0011F97C
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Fold, new Func<bool>(this.FoldPress));
		gameKeyDelegates.Add(GameKey.NextTab, new Func<bool>(this.OnPressedNextTab));
		gameKeyDelegates.Add(GameKey.PrevTab, new Func<bool>(this.OnPressedPrevTab));
		gameKeyDelegates.Add(GameKey.RightClick, new Func<bool>(this.OnPressedBack));
		return gameKeyDelegates;
	}

	// Token: 0x06003C88 RID: 15496 RVA: 0x001217EC File Offset: 0x0011F9EC
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (!LazyInput.IsGamepadActive && this.foldedElement != null)
		{
			this.foldedElement.Fold();
			this.foldedElement = null;
		}
		if (LazyInput.IsGamepadActive)
		{
			this.nextTabGamepadHelper.gameObject.SetActive(true);
			this.prevTabGamepadHelper.gameObject.SetActive(true);
			this.nextTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.NextTab, null, true);
			this.prevTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.PrevTab, null, true);
			return;
		}
		this.nextTabGamepadHelper.gameObject.SetActive(false);
		this.prevTabGamepadHelper.gameObject.SetActive(false);
	}

	// Token: 0x06003C89 RID: 15497 RVA: 0x001218A0 File Offset: 0x0011FAA0
	public bool OnPressedPrevTab()
	{
		if (!this.tabsDrawn)
		{
			return false;
		}
		int num = this.currentTab;
		num--;
		if (num < 0)
		{
			num = this.tabs.Count - 1;
		}
		this.DrawTab(num);
		return true;
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x001218DC File Offset: 0x0011FADC
	public bool OnPressedNextTab()
	{
		if (!this.tabsDrawn)
		{
			return false;
		}
		int num = this.currentTab;
		num++;
		if (num > this.tabs.Count - 1)
		{
			num = 0;
		}
		this.DrawTab(num);
		return true;
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00121918 File Offset: 0x0011FB18
	protected override bool OnPressedBack()
	{
		UICraftItemCell uicraftItemCell;
		if (LazyInput.IsGamepadActive && base.GamepadNavigationController.FocusedItem != null && base.GamepadNavigationController.FocusedItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
		{
			this.foldedElement.Fold();
			base.GamepadNavigationController.SetFocusedItem(this.foldedElement.GetComponentInParent<GamepadNavigationItem>());
			this.foldedElement = null;
			return true;
		}
		return base.OnPressedBack();
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00121984 File Offset: 0x0011FB84
	private bool FoldPress()
	{
		GamepadNavigationController gamepadNavigationController = base.GamepadNavigationController;
		if (gamepadNavigationController == null)
		{
			return false;
		}
		GamepadNavigationItem focusedItem = gamepadNavigationController.FocusedItem;
		if (focusedItem == null)
		{
			return false;
		}
		UIBuildingWidget uibuildingWidget;
		if (focusedItem.TryGetComponent<UIBuildingWidget>(out uibuildingWidget))
		{
			List<UICraftItemCell> displayedIngredients = uibuildingWidget.DisplayedIngredients;
			if (displayedIngredients == null || displayedIngredients.Count == 0)
			{
				return false;
			}
			foreach (UICraftItemCell uicraftItemCell in displayedIngredients)
			{
				if (uicraftItemCell == null || uicraftItemCell.ItemCell == null || uicraftItemCell.GamepadNavigationItem == null)
				{
					return false;
				}
			}
			GamepadNavigationItem gamepadNavigationItem = displayedIngredients[0].GamepadNavigationItem;
			if (gamepadNavigationItem == null)
			{
				return false;
			}
			uibuildingWidget.Unfold();
			this.foldedElement = uibuildingWidget;
			gamepadNavigationController.SetFocusedItem(gamepadNavigationItem);
			return true;
		}
		else
		{
			UICraftItemCell uicraftItemCell2;
			if (!focusedItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell2))
			{
				return false;
			}
			if (this.foldedElement == null)
			{
				return false;
			}
			List<UICraftItemCell> displayedIngredients2 = this.foldedElement.DisplayedIngredients;
			if (displayedIngredients2 == null)
			{
				return false;
			}
			foreach (UICraftItemCell uicraftItemCell3 in displayedIngredients2)
			{
				if (uicraftItemCell3 == null || uicraftItemCell3.ItemCell == null || uicraftItemCell3.GamepadNavigationItem == null)
				{
					return false;
				}
			}
			GamepadNavigationItem componentInParent = this.foldedElement.GetComponentInParent<GamepadNavigationItem>();
			if (componentInParent == null)
			{
				return false;
			}
			this.foldedElement.Fold();
			gamepadNavigationController.SetFocusedItem(componentInParent);
			this.foldedElement = null;
			return true;
		}
		bool flag;
		return flag;
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00121B44 File Offset: 0x0011FD44
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Select(true, true, true));
		UIBuildingWidget uibuildingWidget;
		if (gamepadNavigationItem.TryGetComponent<UIBuildingWidget>(out uibuildingWidget) && uibuildingWidget.DisplayedIngredients.Count > 0)
		{
			list.Add(new LazyGameKeyTip(GameKey.Fold, "tip_unfold", true, true, true));
		}
		UICraftItemCell uicraftItemCell;
		if (gamepadNavigationItem.TryGetComponent<UICraftItemCell>(out uicraftItemCell))
		{
			list.Add(new LazyGameKeyTip(GameKey.Fold, "tip_fold", true, true, true));
			list.Add(new LazyGameKeyTip(GameKey.Back, "tip_back", true, true, true));
		}
		else if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00121BFD File Offset: 0x0011FDFD
	private void StartFight()
	{
		LazySingleton<FightingGameController>.Instance.Play();
		this.Close();
	}

	// Token: 0x06003C8F RID: 15503 RVA: 0x00121C10 File Offset: 0x0011FE10
	private void EndPreFight()
	{
		if (LazySingleton<FightingGameController>.Instance.CurrentLevel == null)
		{
			return;
		}
		this.Close();
		LazyUI.Get<UIFade>().Fade(1f, delegate
		{
			LazySingleton<FightingGameController>.Instance.CancelPreFight();
		}, null);
	}

	// Token: 0x06003C90 RID: 15504 RVA: 0x00121C68 File Offset: 0x0011FE68
	private void UpdateNoBuildingsObject()
	{
		if (this.tabs.Count != 0)
		{
			this.noBuildingsObj.gameObject.SetActive(false);
			return;
		}
		this.noBuildingsObj.gameObject.SetActive(true);
		if (!(LazySingleton<FightingGameController>.Instance.CurrentLevel != null))
		{
			this.noBuildingsText.text = LLBase.L("ui_builddesk_is_empty") ?? "";
			return;
		}
		FightDef data = GameBalance.Me.GetData<FightDef>(LazySingleton<FightingGameController>.Instance.CurrentLevel.id);
		if (data != null && data.isBarricadesUnavailable && data.isTowersUnavailable)
		{
			this.noBuildingsText.text = LLBase.L("ui_fight_construction_unavailable") ?? "";
			return;
		}
		this.noBuildingsText.text = LLBase.L("ui_builddesk_is_empty") ?? "";
	}

	// Token: 0x06003C91 RID: 15505 RVA: 0x00121D43 File Offset: 0x0011FF43
	[LazyUITest]
	protected override void TestDraw()
	{
		LazySingleton<BuildManager>.Instance.TryEnable(GameScene.GetWgoViewGlobal(MainGame.WorldData.GetWgoData("builder_graveyard").UniqueId), null);
	}

	// Token: 0x04002F98 RID: 12184
	[SerializeField]
	private UIInfoWidget uiInfoWidget;

	// Token: 0x04002F99 RID: 12185
	[SerializeField]
	private UIBuildingWidget buildingElement;

	// Token: 0x04002F9A RID: 12186
	[SerializeField]
	[Space]
	private GameObject buildingsListContent;

	// Token: 0x04002F9B RID: 12187
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04002F9C RID: 12188
	[SerializeField]
	private GameObject tabsContainer;

	// Token: 0x04002F9D RID: 12189
	[SerializeField]
	private RectTransform tabsContentRectTransform;

	// Token: 0x04002F9E RID: 12190
	[SerializeField]
	private AutoScroll autoScroll;

	// Token: 0x04002F9F RID: 12191
	[SerializeField]
	private GameObject noBuildingsObj;

	// Token: 0x04002FA0 RID: 12192
	[SerializeField]
	private TextMeshProUGUI noBuildingsText;

	// Token: 0x04002FA1 RID: 12193
	[Space]
	[SerializeField]
	private GameObject fightButtonsContainer;

	// Token: 0x04002FA2 RID: 12194
	[Space]
	[SerializeField]
	private RectTransform fightButtonsRectTransform;

	// Token: 0x04002FA3 RID: 12195
	[SerializeField]
	private UIDialogWindowButton startFightButton;

	// Token: 0x04002FA4 RID: 12196
	[SerializeField]
	private UIDialogWindowButton endPreFightButton;

	// Token: 0x04002FA5 RID: 12197
	private List<UIBuildingWidget> displayedBuildItemGUIs = new List<UIBuildingWidget>();

	// Token: 0x04002FA6 RID: 12198
	private Action<BuildData, List<NeedItemData>> onBuildPressed;

	// Token: 0x04002FA7 RID: 12199
	private Func<BuildData, List<NeedItemData>, bool> canBuild;

	// Token: 0x04002FA8 RID: 12200
	[SerializeField]
	private TextMeshProUGUI nextTabGamepadHelper;

	// Token: 0x04002FA9 RID: 12201
	[SerializeField]
	private TextMeshProUGUI prevTabGamepadHelper;

	// Token: 0x04002FAA RID: 12202
	private List<BuildingWindowTab> drawnTabs = new List<BuildingWindowTab>();

	// Token: 0x04002FAB RID: 12203
	private UIBuildingWidget foldedElement;

	// Token: 0x04002FAC RID: 12204
	private int currentTab;

	// Token: 0x04002FAD RID: 12205
	private List<string> tabs;

	// Token: 0x04002FAE RID: 12206
	private MultiInventory multiInventory;

	// Token: 0x04002FAF RID: 12207
	private bool tabsDrawn;

	// Token: 0x04002FB0 RID: 12208
	private static readonly Dictionary<string, string> lastOpenedTabByWgoId = new Dictionary<string, string>();
}
