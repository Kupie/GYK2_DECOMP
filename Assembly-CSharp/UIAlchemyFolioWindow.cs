using System;
using System.Collections.Generic;
using System.Linq;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008E5 RID: 2277
public class UIAlchemyFolioWindow : LazyWindow<UIAlchemyFolioWindowData>
{
	// Token: 0x06003B83 RID: 15235 RVA: 0x0011C7C8 File Offset: 0x0011A9C8
	public override void Init()
	{
		base.Init();
		foreach (UIFolioWindowTab uifolioWindowTab in this.tabs)
		{
			uifolioWindowTab.Init(new Action<UIFolioWindowTab>(this.OnTabButtonClicked));
			uifolioWindowTab.gameObject.SetActive(true);
		}
		if (this.tabsRunes != null)
		{
			foreach (UIFolioWindowTab uifolioWindowTab2 in this.tabsRunes)
			{
				uifolioWindowTab2.Init(new Action<UIFolioWindowTab>(this.OnTabButtonClicked));
				uifolioWindowTab2.gameObject.SetActive(false);
			}
		}
		this.scrollRect.Init(new Func<LazyScrollableElement, LazyWidgetBase>(this.GetFormulaWidget), new Action<LazyScrollableElement>(this.ReleaseWidgetForParent), base.GamepadNavigationController, null, null, null, null, null);
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, 41f);
	}

	// Token: 0x06003B84 RID: 15236 RVA: 0x0011C8D4 File Offset: 0x0011AAD4
	public override void Open(UIAlchemyFolioWindowData data)
	{
		base.Open(data);
		bool hasAnyTabs = data.HasAnyTabs;
		this.emptyObject.SetActive(!hasAnyTabs);
		this.tabsParent.SetActive(hasAnyTabs);
		this.HideCurrentElements();
		foreach (UIFolioWindowTab uifolioWindowTab in this.tabs)
		{
			uifolioWindowTab.gameObject.SetActive(this.IsTabUnlocked(uifolioWindowTab.Tab));
		}
		if (this.tabsRunes != null)
		{
			foreach (UIFolioWindowTab uifolioWindowTab2 in this.tabsRunes)
			{
				uifolioWindowTab2.gameObject.SetActive(this.IsTabUnlocked(uifolioWindowTab2.Tab));
				uifolioWindowTab2.transform.SetAsLastSibling();
			}
		}
		if (!hasAnyTabs)
		{
			return;
		}
		this.DisplayTab(this.GetInitialTab());
	}

	// Token: 0x06003B85 RID: 15237 RVA: 0x0011C9DC File Offset: 0x0011ABDC
	public void DisplayTab(AlchemyFormulaTab tab)
	{
		this.currentTab = tab;
		UIAlchemyFolioWindow.lastOpenedTab = new AlchemyFormulaTab?(tab);
		this.ForEachTab(delegate(UIFolioWindowTab tabButton)
		{
			tabButton.SetState(tabButton.Tab == this.currentTab);
		});
		this.HideCurrentElements();
		if (tab.IsRuneTab())
		{
			List<ItemDef> list = this.data.RuneItems[tab];
			for (int i = 0; i < list.Count; i++)
			{
				this.AddFormulaElement(new UIAlchemyFormulaWidgetData
				{
					ItemDef = list[i]
				}, i);
			}
		}
		else
		{
			for (int j = 0; j < this.data.Data[tab].Count; j++)
			{
				AlchemyFormulaDef alchemyFormulaDef = this.data.Data[tab][j];
				this.AddFormulaElement(new UIAlchemyFormulaWidgetData
				{
					AlchemyFormulaDef = alchemyFormulaDef
				}, j);
			}
		}
		((RectTransform)base.transform).RefreshContentFitter();
		this.scrollRect.CheckVisibility();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06003B86 RID: 15238 RVA: 0x0011CADE File Offset: 0x0011ACDE
	private void AddFormulaElement(UIAlchemyFormulaWidgetData formulaWidgetData, int index)
	{
		this.scrollRect.AddScrollableElement(formulaWidgetData).transform.SetSiblingIndex(index);
	}

	// Token: 0x06003B87 RID: 15239 RVA: 0x0011CAF7 File Offset: 0x0011ACF7
	private void HideCurrentElements()
	{
		this.scrollRect.ClearDisplayingScrollableElements();
	}

	// Token: 0x06003B88 RID: 15240 RVA: 0x0011CB04 File Offset: 0x0011AD04
	protected override void PrintTips()
	{
		this.lazyButtonTips.Print(LazyGameKeyTip.Back(true, true, true));
	}

	// Token: 0x06003B89 RID: 15241 RVA: 0x0011CB19 File Offset: 0x0011AD19
	private void OnTabButtonClicked(UIFolioWindowTab tabButton)
	{
		if (this.currentTab != tabButton.Tab)
		{
			this.DisplayTab(tabButton.Tab);
		}
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x0011CB35 File Offset: 0x0011AD35
	private UIAlchemyFormulaWidget GetFormulaWidget(LazyScrollableElement parent)
	{
		UIAlchemyFormulaWidget elementFromPool = UIPrefabsPooler.Instance.GetElementFromPool<UIAlchemyFormulaWidget>(parent.RectTransform);
		elementFromPool.Init();
		elementFromPool.Draw(parent.Data);
		return elementFromPool;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x0011CB59 File Offset: 0x0011AD59
	private void ReleaseWidgetForParent(LazyScrollableElement parent)
	{
		this.ReleaseCommonWidget(parent.Widget as UIAlchemyFormulaWidget);
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x0011CB6C File Offset: 0x0011AD6C
	private void ReleaseCommonWidget(UIAlchemyFormulaWidget formulaWidget)
	{
		formulaWidget.DeInit();
		formulaWidget.Hide();
		UIPrefabsPooler.Instance.ReleaseElementToPool<UIAlchemyFormulaWidget>(formulaWidget);
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x0011CB88 File Offset: 0x0011AD88
	private bool OnPressedPrevTab()
	{
		List<UIFolioWindowTab> visibleTabs = this.GetVisibleTabs();
		if (visibleTabs.Count <= 0)
		{
			return false;
		}
		int num = this.GetCurrentVisibleTabIndex(visibleTabs);
		num--;
		if (num < 0)
		{
			num = visibleTabs.Count - 1;
		}
		this.DisplayTab(visibleTabs[num].Tab);
		return true;
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x0011CBD4 File Offset: 0x0011ADD4
	private bool OnPressedNextTab()
	{
		List<UIFolioWindowTab> visibleTabs = this.GetVisibleTabs();
		if (visibleTabs.Count <= 0)
		{
			return false;
		}
		int num = this.GetCurrentVisibleTabIndex(visibleTabs);
		num++;
		if (num > visibleTabs.Count - 1)
		{
			num = 0;
		}
		this.DisplayTab(visibleTabs[num].Tab);
		return true;
	}

	// Token: 0x06003B8F RID: 15247 RVA: 0x0011CC20 File Offset: 0x0011AE20
	private AlchemyFormulaTab GetInitialTab()
	{
		if (UIAlchemyFolioWindow.lastOpenedTab != null && this.IsTabUnlocked(UIAlchemyFolioWindow.lastOpenedTab.Value))
		{
			return UIAlchemyFolioWindow.lastOpenedTab.Value;
		}
		List<UIFolioWindowTab> visibleTabs = this.GetVisibleTabs();
		if (visibleTabs.Count > 0)
		{
			return visibleTabs[0].Tab;
		}
		if (this.data.Data.Count > 0)
		{
			return this.data.Data.Keys.First<AlchemyFormulaTab>();
		}
		return this.data.RuneItems.Keys.First<AlchemyFormulaTab>();
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x0011CCB1 File Offset: 0x0011AEB1
	private bool IsTabUnlocked(AlchemyFormulaTab tab)
	{
		return this.data.HasTab(tab);
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x0011CCC0 File Offset: 0x0011AEC0
	private List<UIFolioWindowTab> GetVisibleTabs()
	{
		List<UIFolioWindowTab> list = new List<UIFolioWindowTab>();
		foreach (UIFolioWindowTab uifolioWindowTab in this.tabs)
		{
			if (uifolioWindowTab.gameObject.activeSelf)
			{
				list.Add(uifolioWindowTab);
			}
		}
		if (this.tabsRunes != null)
		{
			foreach (UIFolioWindowTab uifolioWindowTab2 in this.tabsRunes)
			{
				if (uifolioWindowTab2.gameObject.activeSelf)
				{
					list.Add(uifolioWindowTab2);
				}
			}
		}
		return list;
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0011CD80 File Offset: 0x0011AF80
	private int GetCurrentVisibleTabIndex(List<UIFolioWindowTab> visibleTabs)
	{
		for (int i = 0; i < visibleTabs.Count; i++)
		{
			if (visibleTabs[i].Tab == this.currentTab)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x0011CDB8 File Offset: 0x0011AFB8
	private void ForEachTab(Action<UIFolioWindowTab> action)
	{
		foreach (UIFolioWindowTab uifolioWindowTab in this.tabs)
		{
			action(uifolioWindowTab);
		}
		if (this.tabsRunes == null)
		{
			return;
		}
		foreach (UIFolioWindowTab uifolioWindowTab2 in this.tabsRunes)
		{
			action(uifolioWindowTab2);
		}
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x0011CE58 File Offset: 0x0011B058
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.NextTab, new Func<bool>(this.OnPressedNextTab));
		gameKeyDelegates.Add(GameKey.PrevTab, new Func<bool>(this.OnPressedPrevTab));
		return gameKeyDelegates;
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04002EFB RID: 12027
	[Space]
	[SerializeField]
	private LazyScrollRect scrollRect;

	// Token: 0x04002EFC RID: 12028
	[Space]
	[SerializeField]
	private List<UIFolioWindowTab> tabs;

	// Token: 0x04002EFD RID: 12029
	[Space]
	[SerializeField]
	private List<UIFolioWindowTab> tabsRunes;

	// Token: 0x04002EFE RID: 12030
	[Space]
	[SerializeField]
	private GameObject emptyObject;

	// Token: 0x04002EFF RID: 12031
	[Space]
	[SerializeField]
	private GameObject tabsParent;

	// Token: 0x04002F00 RID: 12032
	private AlchemyFormulaTab currentTab;

	// Token: 0x04002F01 RID: 12033
	private static AlchemyFormulaTab? lastOpenedTab;
}
