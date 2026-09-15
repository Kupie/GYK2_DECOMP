using System;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000908 RID: 2312
public class CharacterWindow : LazyWindow<CharacterWindowData>, IUIWindowCustomOperable
{
	// Token: 0x17000924 RID: 2340
	// (get) Token: 0x06003CA6 RID: 15526 RVA: 0x00121F68 File Offset: 0x00120168
	public GamepadNavigationController NavigationController
	{
		get
		{
			return base.GamepadNavigationController;
		}
	}

	// Token: 0x17000925 RID: 2341
	// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x00121F70 File Offset: 0x00120170
	public TechTreePageWidget TechTreePageWidget
	{
		get
		{
			return this.techTreePageWidget;
		}
	}

	// Token: 0x06003CA8 RID: 15528 RVA: 0x00121F78 File Offset: 0x00120178
	public void RefreshCharPageTabButtonParentContentFitter()
	{
		((RectTransform)this.charPageTabButtonParent.transform).RefreshContentFitter();
	}

	// Token: 0x06003CA9 RID: 15529 RVA: 0x00121F90 File Offset: 0x00120190
	public void RefreshTechTreePageWidgetContentFitter()
	{
		try
		{
			((RectTransform)this.techTreePageWidget.transform).RefreshContentFitter();
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x140000BC RID: 188
	// (add) Token: 0x06003CAA RID: 15530 RVA: 0x00121FC8 File Offset: 0x001201C8
	// (remove) Token: 0x06003CAB RID: 15531 RVA: 0x00121FFC File Offset: 0x001201FC
	public static event Action<CharacterWindowData.CharPage> OnTabChanged;

	// Token: 0x17000926 RID: 2342
	// (get) Token: 0x06003CAC RID: 15532 RVA: 0x0012202F File Offset: 0x0012022F
	// (set) Token: 0x06003CAD RID: 15533 RVA: 0x0012204B File Offset: 0x0012024B
	public CharacterWindowData.CharPage LastOpenedPage
	{
		get
		{
			if (this.data != null)
			{
				return this.data.PlayerData.lastOpenedPage;
			}
			return CharacterWindowData.CharPage.Undefined;
		}
		set
		{
			if (this.data == null)
			{
				return;
			}
			this.data.PlayerData.lastOpenedPage = value;
		}
	}

	// Token: 0x06003CAE RID: 15534 RVA: 0x00122068 File Offset: 0x00120268
	public override void Init()
	{
		base.Init();
		this.pages.Add(CharacterWindowData.CharPage.Main, this.mainPageWidget);
		this.pages.Add(CharacterWindowData.CharPage.TechTree, this.techTreePageWidget);
		this.pages.Add(CharacterWindowData.CharPage.Inspiration, this.inspirationPageWidget);
		this.pages.Add(CharacterWindowData.CharPage.QuestTree, this.questTreePageWidget);
		this.pages.Add(CharacterWindowData.CharPage.Map, this.mapPageWidget);
		this.charPageTabButtonPrefab.gameObject.SetActive(false);
		int count = this.pages.Count;
		int num = 0;
		using (Dictionary<CharacterWindowData.CharPage, LazyWidgetBase>.Enumerator enumerator = this.pages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<CharacterWindowData.CharPage, LazyWidgetBase> page = enumerator.Current;
				bool flag = num == count - 1;
				string name = page.Value.gameObject.name;
				string text = name + "_Button";
				CharPageTabButton charPageTabButton = this.charPageTabButtonPrefab.Copy(this.charPageTabButtonParent.transform, true, text);
				charPageTabButton.Init(name, delegate
				{
					this.SwitchPage(page.Key, true);
				}, flag);
				this.pagesButtons.Add(page.Key, charPageTabButton);
				num++;
			}
		}
		this.LastOpenedPage = CharacterWindowData.CharPage.Undefined;
	}

	// Token: 0x06003CAF RID: 15535 RVA: 0x001221D0 File Offset: 0x001203D0
	protected override void SetData(CharacterWindowData data)
	{
		base.SetData(data);
		data.SubscribeEvents();
		data.onPageStatusChanged = new Action<CharacterWindowData.CharPage, bool>(this.UpdatePageHasActionStatus);
		data.CharMainPageWidgetData.OnBagShow = new Action<Item>(this.OnBagShown);
		data.CharMainPageWidgetData.OnBagHide = new Action<Item>(this.OnBagHide);
	}

	// Token: 0x06003CB0 RID: 15536 RVA: 0x0012222C File Offset: 0x0012042C
	public override void Redraw()
	{
		this.SwitchPage(this.data.Page, false);
		this.pagesButtons[CharacterWindowData.CharPage.TechTree].UpdateActionIndicatorStatus(false);
		this.pagesButtons[CharacterWindowData.CharPage.QuestTree].UpdateActionIndicatorStatus(false);
		this.pagesButtons[CharacterWindowData.CharPage.Inspiration].UpdateActionIndicatorStatus(this.data.HasAvailableActionOnInspirationPage);
		this.pagesButtons[CharacterWindowData.CharPage.Main].UpdateActionIndicatorStatus(false);
		this.pagesButtons[CharacterWindowData.CharPage.Map].UpdateActionIndicatorStatus(false);
		for (int i = 0; i < this.pagesButtons.Keys.Count; i++)
		{
			CharacterWindowData.CharPage charPage = this.pagesButtons.Keys.ElementAt(i);
			this.pagesButtons[charPage].gameObject.SetActive(!MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(charPage));
		}
	}

	// Token: 0x06003CB1 RID: 15537 RVA: 0x0012230A File Offset: 0x0012050A
	public override void Close()
	{
		CharacterWindow.CloseContextMenuIfOpen();
		base.Close();
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x00122318 File Offset: 0x00120518
	public override void Hide()
	{
		base.Hide();
		if (this.data != null)
		{
			this.data.UnsubscribeEvents();
			this.data.InspirationPageWidgetData.UnsubscribeEvents();
		}
		if (this.pages.ContainsKey(this.LastOpenedPage))
		{
			this.pages[this.LastOpenedPage].Hide();
		}
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00122378 File Offset: 0x00120578
	private static void CloseContextMenuIfOpen()
	{
		UIContextMenuWindow window = LazyUI.GetWindow<UIContextMenuWindow>();
		if (window != null && window.IsShown)
		{
			window.Close();
		}
	}

	// Token: 0x06003CB4 RID: 15540 RVA: 0x001223A2 File Offset: 0x001205A2
	public void SetInspirationPageWithSpecificTalent(string talentId)
	{
		this.data.UpdateTalentInInspirationWidgetData(talentId);
		this.SwitchPage(CharacterWindowData.CharPage.Inspiration, false);
	}

	// Token: 0x06003CB5 RID: 15541 RVA: 0x001223B8 File Offset: 0x001205B8
	public void RefreshInspirationPageActionIndicatorStatus()
	{
		this.data.UpdateHasAvailableActionOnInspirationPageStatus("");
	}

	// Token: 0x06003CB6 RID: 15542 RVA: 0x001223CC File Offset: 0x001205CC
	private void SwitchPage(CharacterWindowData.CharPage page, bool checkCurrent = false)
	{
		if (checkCurrent && page == this.LastOpenedPage)
		{
			return;
		}
		if (this.pages.ContainsKey(this.LastOpenedPage))
		{
			this.pages[this.LastOpenedPage].Hide();
		}
		foreach (KeyValuePair<CharacterWindowData.CharPage, CharPageTabButton> keyValuePair in this.pagesButtons)
		{
			CharacterWindowData.CharPage key = keyValuePair.Key;
			keyValuePair.Value.UpdateState(key == page);
		}
		switch (page)
		{
		case CharacterWindowData.CharPage.Main:
			this.mainPageWidget.Draw(this.data.CharMainPageWidgetData);
			try
			{
				((RectTransform)this.mainPageWidget.transform).RefreshContentFitter();
				goto IL_01B0;
			}
			catch (Exception)
			{
				goto IL_01B0;
			}
			break;
		case CharacterWindowData.CharPage.TechTree:
			goto IL_011A;
		case CharacterWindowData.CharPage.Inspiration:
			break;
		case CharacterWindowData.CharPage.QuestTree:
			goto IL_0150;
		case CharacterWindowData.CharPage.Map:
			goto IL_0186;
		default:
			throw new ArgumentOutOfRangeException("page", page, null);
		}
		this.inspirationPageWidget.Draw(this.data.InspirationPageWidgetData);
		this.data.InspirationPageWidgetData.SubscribeEvents();
		try
		{
			((RectTransform)this.inspirationPageWidget.transform).RefreshContentFitter();
			goto IL_01B0;
		}
		catch (Exception)
		{
			goto IL_01B0;
		}
		IL_011A:
		this.techTreePageWidget.Draw(null);
		this.techTreePageWidget.DisplayLastTab("");
		try
		{
			((RectTransform)this.techTreePageWidget.transform).RefreshContentFitter();
			goto IL_01B0;
		}
		catch (Exception)
		{
			goto IL_01B0;
		}
		IL_0150:
		this.questTreePageWidget.Draw(null);
		this.questTreePageWidget.Display("");
		try
		{
			((RectTransform)this.questTreePageWidget.transform).RefreshContentFitter();
			goto IL_01B0;
		}
		catch (Exception)
		{
			goto IL_01B0;
		}
		IL_0186:
		this.mapPageWidget.Draw(this.data.MapPageWidgetData);
		IL_01B0:
		this.UpdateGamepadNavigationMaxDistanceBetweenElements(page);
		((RectTransform)this.charPageTabButtonParent.transform).RefreshContentFitter();
		this.LastOpenedPage = page;
		if (page == CharacterWindowData.CharPage.Inspiration)
		{
			this.data.UpdateHasAvailableActionOnInspirationPageStatus("");
		}
		if (LazyInput.IsGamepadActive && page != CharacterWindowData.CharPage.TechTree)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		Action<CharacterWindowData.CharPage> onTabChanged = CharacterWindow.OnTabChanged;
		if (onTabChanged != null)
		{
			onTabChanged(page);
		}
		if (page == CharacterWindowData.CharPage.Inspiration)
		{
			this.TryShowInspirationTutorial();
		}
	}

	// Token: 0x06003CB7 RID: 15543 RVA: 0x00122634 File Offset: 0x00120834
	private void UpdateGamepadNavigationMaxDistanceBetweenElements(CharacterWindowData.CharPage page)
	{
		bool flag = page == CharacterWindowData.CharPage.Inspiration;
		base.GamepadNavigationController.maxDistanceBetweenElements = (flag ? this.inspirationGamepadNavigationMaxDistanceBetweenElements : this.defaultGamepadNavigationMaxDistanceBetweenElements);
		base.GamepadNavigationController.checkMaxCrossAxisDistance = flag;
		base.GamepadNavigationController.maxCrossAxisDistance = this.inspirationGamepadNavigationMaxCrossAxisDistance;
	}

	// Token: 0x06003CB8 RID: 15544 RVA: 0x0012267F File Offset: 0x0012087F
	private bool TrySwitchPageToMain()
	{
		return this.TrySwitchToPage(CharacterWindowData.CharPage.Main);
	}

	// Token: 0x06003CB9 RID: 15545 RVA: 0x00122688 File Offset: 0x00120888
	private bool TrySwitchPageToTechTree()
	{
		return this.TrySwitchToPage(CharacterWindowData.CharPage.TechTree);
	}

	// Token: 0x06003CBA RID: 15546 RVA: 0x00122691 File Offset: 0x00120891
	private bool TrySwitchPageToMap()
	{
		return this.TrySwitchToPage(CharacterWindowData.CharPage.Map);
	}

	// Token: 0x06003CBB RID: 15547 RVA: 0x0012269A File Offset: 0x0012089A
	private bool TrySwitchPageToQuestTree()
	{
		return this.TrySwitchToPage(CharacterWindowData.CharPage.QuestTree);
	}

	// Token: 0x06003CBC RID: 15548 RVA: 0x001226A3 File Offset: 0x001208A3
	private bool TrySwitchPageToInspirations()
	{
		return this.TrySwitchToPage(CharacterWindowData.CharPage.Inspiration);
	}

	// Token: 0x06003CBD RID: 15549 RVA: 0x001226AC File Offset: 0x001208AC
	private bool TrySwitchToPage(CharacterWindowData.CharPage page)
	{
		if (this.LastOpenedPage == page)
		{
			return this.OnPressedBack();
		}
		if (!CharacterWindow.IsPageAvailable(page))
		{
			return false;
		}
		if (page == CharacterWindowData.CharPage.Inspiration)
		{
			MainGame.Instance.GameSave.knowledgeSystem.TryUnlockInspirationTab();
			this.pagesButtons[CharacterWindowData.CharPage.Inspiration].gameObject.SetActive(true);
		}
		this.SwitchPage(page, false);
		return true;
	}

	// Token: 0x06003CBE RID: 15550 RVA: 0x0012270B File Offset: 0x0012090B
	private static bool IsPageAvailable(CharacterWindowData.CharPage page)
	{
		return page == CharacterWindowData.CharPage.Main || !MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(page) || (page == CharacterWindowData.CharPage.Inspiration && CharacterWindow.HasPendingInspirationTutorial());
	}

	// Token: 0x06003CBF RID: 15551 RVA: 0x00073F8A File Offset: 0x0007218A
	private static bool HasPendingInspirationTutorial()
	{
		return !MainGame.PlayerData.sawInspirationTutorialOnce && MainGame.Instance.GameSave.talentSystemData.HasTwoZeroFaithInspirationsToBuyInSameBranch();
	}

	// Token: 0x06003CC0 RID: 15552 RVA: 0x00122738 File Offset: 0x00120938
	private void TryShowInspirationTutorial()
	{
		if (MainGame.PlayerData.sawInspirationTutorialOnce)
		{
			return;
		}
		if (!MainGame.Instance.GameSave.talentSystemData.HasTwoZeroFaithInspirationsToBuyInSameBranch())
		{
			return;
		}
		MainGame.PlayerData.sawInspirationTutorialOnce = true;
		LazyUI.GetWindow<UITutorialWindow>().Open(new UITutorialWindowData("tut_insp_talents_hdr", null, false));
	}

	// Token: 0x06003CC1 RID: 15553 RVA: 0x0012278C File Offset: 0x0012098C
	public bool OnPressedPrevTab()
	{
		int num = (int)this.LastOpenedPage;
		CharacterWindowData.CharPage charPage;
		do
		{
			num--;
			if (num < 1)
			{
				num = this.pages.Count;
			}
			charPage = (CharacterWindowData.CharPage)num;
		}
		while (MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(charPage));
		if (LazyInput.IsGamepadActive)
		{
			LazyAudio.PlayAndForget("tab_click");
		}
		this.SwitchPage(charPage, false);
		return true;
	}

	// Token: 0x06003CC2 RID: 15554 RVA: 0x001227E8 File Offset: 0x001209E8
	public bool OnPressedNextTab()
	{
		int num = (int)this.LastOpenedPage;
		CharacterWindowData.CharPage charPage;
		do
		{
			num++;
			if (num > this.pages.Count)
			{
				num = 1;
			}
			charPage = (CharacterWindowData.CharPage)num;
		}
		while (MainGame.Instance.GameSave.knowledgeSystem.IsCharTabLocked(charPage));
		if (LazyInput.IsGamepadActive)
		{
			LazyAudio.PlayAndForget("tab_click");
		}
		this.SwitchPage(charPage, false);
		return true;
	}

	// Token: 0x06003CC3 RID: 15555 RVA: 0x00122844 File Offset: 0x00120A44
	public bool OnPressedPrevSubTab()
	{
		bool flag = false;
		CharacterWindowData.CharPage lastOpenedPage = this.LastOpenedPage;
		if (lastOpenedPage != CharacterWindowData.CharPage.TechTree)
		{
			if (lastOpenedPage == CharacterWindowData.CharPage.Inspiration)
			{
				flag = this.inspirationPageWidget.OnPressedPrevTechTab(base.GamepadNavigationController);
			}
		}
		else
		{
			flag = this.techTreePageWidget.OnPressedPrevTechTab();
		}
		if (flag)
		{
			LazyAudio.PlayAndForget("tab_click");
		}
		return flag;
	}

	// Token: 0x06003CC4 RID: 15556 RVA: 0x00122894 File Offset: 0x00120A94
	public bool OnPressedNextSubTab()
	{
		bool flag = false;
		CharacterWindowData.CharPage lastOpenedPage = this.LastOpenedPage;
		if (lastOpenedPage != CharacterWindowData.CharPage.TechTree)
		{
			if (lastOpenedPage == CharacterWindowData.CharPage.Inspiration)
			{
				flag = this.inspirationPageWidget.OnPressedNextTechTab(base.GamepadNavigationController);
			}
		}
		else
		{
			flag = this.techTreePageWidget.OnPressedNextTechTab();
		}
		if (flag)
		{
			LazyAudio.PlayAndForget("tab_click");
		}
		return flag;
	}

	// Token: 0x06003CC5 RID: 15557 RVA: 0x001228E4 File Offset: 0x00120AE4
	private bool OnItemPressed2()
	{
		if (!base.IsShownAndTop)
		{
			return false;
		}
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

	// Token: 0x06003CC6 RID: 15558 RVA: 0x00122940 File Offset: 0x00120B40
	protected override bool OnPressedBack()
	{
		if (!base.IsShownAndTop)
		{
			return false;
		}
		if (this.data != null && this.LastOpenedPage == CharacterWindowData.CharPage.Main && this.data.CharMainPageWidgetData != null && this.data.CharMainPageWidgetData.IsBagShown)
		{
			Action onHideBagPressed = this.data.CharMainPageWidgetData.OnHideBagPressed;
			if (onHideBagPressed != null)
			{
				onHideBagPressed();
			}
			return true;
		}
		if (this.closeButton)
		{
			this.Close();
			return true;
		}
		return false;
	}

	// Token: 0x06003CC7 RID: 15559 RVA: 0x001229BA File Offset: 0x00120BBA
	private void UpdatePageHasActionStatus(CharacterWindowData.CharPage pageType, bool value)
	{
		this.pagesButtons[pageType].UpdateActionIndicatorStatus(value);
	}

	// Token: 0x06003CC8 RID: 15560 RVA: 0x001229D0 File Offset: 0x00120BD0
	private void OnBagShown(Item bag)
	{
		GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
		this.mainPageWidget.OnBagShown(bag);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(false, null, null);
			base.GamepadNavigationController.SetFocusedItem(focusedItem);
		}
	}

	// Token: 0x06003CC9 RID: 15561 RVA: 0x00122A18 File Offset: 0x00120C18
	private void OnBagHide(Item bag)
	{
		GamepadNavigationItem focusedItem = base.GamepadNavigationController.FocusedItem;
		this.mainPageWidget.OnBagHide(bag);
		if (LazyInput.IsGamepadActive)
		{
			if (base.GamepadNavigationController.FocusedItem != null && base.GamepadNavigationController.FocusedItem.gameObject.activeSelf)
			{
				base.GamepadNavigationController.ReinitItems(false, null, null);
				base.GamepadNavigationController.SetFocusedItem(focusedItem);
				return;
			}
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
	}

	// Token: 0x06003CCA RID: 15562 RVA: 0x00122A98 File Offset: 0x00120C98
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.CharacterWindow, new Func<bool>(this.OnCharacterButtonPressed));
		gameKeyDelegates.Add(GameKey.ItemMove, new Func<bool>(this.OnItemPressed2));
		gameKeyDelegates.Add(GameKey.NextTab, new Func<bool>(this.OnPressedNextTab));
		gameKeyDelegates.Add(GameKey.PrevTab, new Func<bool>(this.OnPressedPrevTab));
		gameKeyDelegates.Add(GameKey.NextSubTab, new Func<bool>(this.OnPressedNextSubTab));
		gameKeyDelegates.Add(GameKey.PrevSubTab, new Func<bool>(this.OnPressedPrevSubTab));
		gameKeyDelegates.Add(GameKey.Inventory, new Func<bool>(this.TrySwitchPageToMain));
		gameKeyDelegates.Add(GameKey.TechTree, new Func<bool>(this.TrySwitchPageToTechTree));
		gameKeyDelegates.Add(GameKey.Map, new Func<bool>(this.TrySwitchPageToMap));
		gameKeyDelegates.Add(GameKey.QuestTree, new Func<bool>(this.TrySwitchPageToQuestTree));
		gameKeyDelegates.Add(GameKey.Inspirations, new Func<bool>(this.TrySwitchPageToInspirations));
		gameKeyDelegates.Add(GameKey.MoveAllItemsFromPlayer, delegate
		{
			if (this.LastOpenedPage == CharacterWindowData.CharPage.Main)
			{
				if (this.mainPageWidget.IsBagModeEnabled())
				{
					this.mainPageWidget.OnAllToChestPressed();
				}
				else if (LazyInput.IsGamepadActive)
				{
					this.OnPressedBack();
				}
			}
			return true;
		});
		return gameKeyDelegates;
	}

	// Token: 0x06003CCB RID: 15563 RVA: 0x00122BBF File Offset: 0x00120DBF
	private bool OnCharacterButtonPressed()
	{
		return !LazyInput.IsGamepadActive && this.OnPressedBack();
	}

	// Token: 0x06003CCC RID: 15564 RVA: 0x00122BD0 File Offset: 0x00120DD0
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		if (this.LastOpenedPage == CharacterWindowData.CharPage.TechTree)
		{
			TechTreePageWidget techTreePageWidget = this.techTreePageWidget;
			if (techTreePageWidget != null)
			{
				techTreePageWidget.UpdateGamepadDependentStuff();
			}
		}
		if (this.LastOpenedPage == CharacterWindowData.CharPage.Map)
		{
			MapPageWidget mapPageWidget = this.mapPageWidget;
			if (mapPageWidget != null)
			{
				mapPageWidget.UpdateGamepadDependentStuff();
			}
		}
		if (this.LastOpenedPage == CharacterWindowData.CharPage.Inspiration)
		{
			CharInspirationPageWidget charInspirationPageWidget = this.inspirationPageWidget;
			if (charInspirationPageWidget != null)
			{
				charInspirationPageWidget.UpdateGamepadDependentStuff();
			}
		}
		bool isGamepadActive = LazyInput.IsGamepadActive;
		if (this.nextTabGamepadHelper != null)
		{
			this.nextTabGamepadHelper.gameObject.SetActive(isGamepadActive);
			if (isGamepadActive)
			{
				this.nextTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.NextTab, null, true);
			}
			this.nextTabGamepadHelper.transform.SetAsLastSibling();
		}
		if (this.prevTabGamepadHelper != null)
		{
			this.prevTabGamepadHelper.gameObject.SetActive(isGamepadActive);
			if (isGamepadActive)
			{
				this.prevTabGamepadHelper.text = ControllerIconLibrary.GetIconId(GameKey.PrevTab, null, true);
			}
			this.prevTabGamepadHelper.transform.SetAsLastSibling();
		}
	}

	// Token: 0x06003CCD RID: 15565 RVA: 0x00122CC9 File Offset: 0x00120EC9
	public void RefreshGamepadTips()
	{
		if (LazyInput.IsGamepadActive)
		{
			this.PrintTips();
		}
	}

	// Token: 0x06003CCE RID: 15566 RVA: 0x00122CD8 File Offset: 0x00120ED8
	protected override void PrintTips()
	{
		GamepadNavigationItem gamepadNavigationItem = null;
		if (base.GamepadNavigationController != null)
		{
			gamepadNavigationItem = base.GamepadNavigationController.FocusedItem;
		}
		this.PrintTips(gamepadNavigationItem);
	}

	// Token: 0x06003CCF RID: 15567 RVA: 0x00122D08 File Offset: 0x00120F08
	protected override void PrintTips(GamepadNavigationItem gamepadNavigationItem)
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		CharacterWindowData.CharPage charPage = ((this.LastOpenedPage == CharacterWindowData.CharPage.Undefined) ? CharacterWindowData.CharPage.Main : this.LastOpenedPage);
		LazyWidgetBase lazyWidgetBase;
		if (this.pages.TryGetValue(charPage, out lazyWidgetBase) && lazyWidgetBase != null)
		{
			List<LazyGameKeyTip> tips = lazyWidgetBase.GetTips(gamepadNavigationItem);
			if (tips != null)
			{
				list.AddRange(tips);
			}
		}
		if (this.closeButton)
		{
			list.Add(LazyGameKeyTip.Back(true, true, true));
		}
		LazyButtonTipsStr lazyButtonTips = this.lazyButtonTips;
		if (lazyButtonTips == null)
		{
			return;
		}
		lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06003CD0 RID: 15568 RVA: 0x00122D89 File Offset: 0x00120F89
	[LazyUITest]
	protected override void TestDraw()
	{
		LazyUI.GetWindow<CharacterWindow>().Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.Main, null));
	}

	// Token: 0x06003CD1 RID: 15569 RVA: 0x00122DA6 File Offset: 0x00120FA6
	[LazyUITest]
	protected void TestDraw_Inspiration()
	{
		LazyUI.GetWindow<CharacterWindow>().Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.Main, null));
		this.SwitchPage(CharacterWindowData.CharPage.Inspiration, false);
	}

	// Token: 0x06003CD2 RID: 15570 RVA: 0x00122DCB File Offset: 0x00120FCB
	[LazyUITest]
	protected void TestDraw_TechTree()
	{
		CharacterWindow window = LazyUI.GetWindow<CharacterWindow>();
		window.Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.Main, null));
		window.SwitchPage(CharacterWindowData.CharPage.TechTree, false);
	}

	// Token: 0x06003CD3 RID: 15571 RVA: 0x00122DF0 File Offset: 0x00120FF0
	[LazyUITest]
	protected void TestDraw_Map()
	{
		CharacterWindow window = LazyUI.GetWindow<CharacterWindow>();
		window.Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.Main, null));
		window.SwitchPage(CharacterWindowData.CharPage.Map, false);
	}

	// Token: 0x06003CD4 RID: 15572 RVA: 0x00122E18 File Offset: 0x00121018
	[LazyUITest]
	protected void TestDraw_QuestTree()
	{
		LazyWindow<CharacterWindowData> window = LazyUI.GetWindow<CharacterWindow>();
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["17_village_nun_meet"].isHidden = false;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["19_base_ceremony_church"].isHidden = false;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["17_village_nun_meet"].status = QuestStatus.InProgress;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["19_base_ceremony_church"].status = QuestStatus.InProgress;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["16_foreman_letter_wake"].isHidden = false;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["18_base_doctor_encounter"].isHidden = false;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["16_foreman_letter_wake"].status = QuestStatus.InProgress;
		MainGame.Instance.GameSave.questSystemData.questCollection.questsCache["18_base_doctor_encounter"].status = QuestStatus.InProgress;
		window.Open(new CharacterWindowData(MainGame.Instance.GameSave, CharacterWindowData.CharPage.Main, null));
		this.SwitchPage(CharacterWindowData.CharPage.QuestTree, false);
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x00122FCF File Offset: 0x001211CF
	bool IUIWindowCustomOperable.get_IsShown()
	{
		return base.IsShown;
	}

	// Token: 0x04002FBA RID: 12218
	[Space]
	[SerializeField]
	private CharMainPageWidget mainPageWidget;

	// Token: 0x04002FBB RID: 12219
	[SerializeField]
	private CharInspirationPageWidget inspirationPageWidget;

	// Token: 0x04002FBC RID: 12220
	[SerializeField]
	private TechTreePageWidget techTreePageWidget;

	// Token: 0x04002FBD RID: 12221
	[SerializeField]
	private QuestTreePageWidget questTreePageWidget;

	// Token: 0x04002FBE RID: 12222
	[SerializeField]
	private MapPageWidget mapPageWidget;

	// Token: 0x04002FBF RID: 12223
	[SerializeField]
	private TextMeshProUGUI nextTabGamepadHelper;

	// Token: 0x04002FC0 RID: 12224
	[SerializeField]
	private TextMeshProUGUI prevTabGamepadHelper;

	// Token: 0x04002FC1 RID: 12225
	private Dictionary<CharacterWindowData.CharPage, LazyWidgetBase> pages = new Dictionary<CharacterWindowData.CharPage, LazyWidgetBase>();

	// Token: 0x04002FC2 RID: 12226
	private Dictionary<CharacterWindowData.CharPage, CharPageTabButton> pagesButtons = new Dictionary<CharacterWindowData.CharPage, CharPageTabButton>();

	// Token: 0x04002FC3 RID: 12227
	[SerializeField]
	private GameObject charPageTabButtonParent;

	// Token: 0x04002FC4 RID: 12228
	[SerializeField]
	private CharPageTabButton charPageTabButtonPrefab;

	// Token: 0x04002FC5 RID: 12229
	[SerializeField]
	private float defaultGamepadNavigationMaxDistanceBetweenElements = 200f;

	// Token: 0x04002FC6 RID: 12230
	[SerializeField]
	private float inspirationGamepadNavigationMaxDistanceBetweenElements = 55f;

	// Token: 0x04002FC7 RID: 12231
	[SerializeField]
	private float inspirationGamepadNavigationMaxCrossAxisDistance = 60f;
}
