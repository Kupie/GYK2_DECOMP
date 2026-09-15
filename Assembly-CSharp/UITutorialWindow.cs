using System;
using System.Collections.Generic;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A46 RID: 2630
public class UITutorialWindow : LazyWindow<UITutorialWindowData>
{
	// Token: 0x060046E3 RID: 18147 RVA: 0x0014F534 File Offset: 0x0014D734
	public override void Init()
	{
		base.Init();
		this.nextButton.onClick.AddListener(new UnityAction(this.ShowNextPage));
		this.prevButton.onClick.AddListener(new UnityAction(this.ShowPrevPage));
		this.nextButton.SetCallbacksIntoGamepadNavigationItem();
		this.prevButton.SetCallbacksIntoGamepadNavigationItem();
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x0014F598 File Offset: 0x0014D798
	public override void Open(UITutorialWindowData data)
	{
		MainGame.Instance.GameSave.knowledgeSystem.UnlockTutorial(data.Page);
		MainGame.Instance.GameSave.knowledgeSystem.AddViewedTutorial(data.Page);
		this.nextButton.gameObject.SetActive(false);
		this.prevButton.gameObject.SetActive(false);
		this.currentPageIndex = 0;
		this.currentTutorialPages.Clear();
		this.pages.ForEach(delegate(GameObject x)
		{
			x.gameObject.SetActive(false);
		});
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), () => this.isOkBtnAvailable, true, GameKey.Select, "");
		base.Open(data);
		bool flag = false;
		foreach (GameObject gameObject in this.pages)
		{
			if (gameObject.name == data.Page)
			{
				flag = true;
				this.currentTutorialPages.Add(gameObject);
			}
			gameObject.SetActive(false);
		}
		if (!flag)
		{
			Debug.LogError("UITutorialWindow:Cannot find tutorial page  [" + data.Page + "].");
			this.Close();
		}
		else
		{
			this.UpdateHeaderLabel();
			this.currentTutorialPages[0].SetActive(true);
			this.UpdateButtons();
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x0014F734 File Offset: 0x0014D934
	private void UpdateHeaderLabel()
	{
		GameObject gameObject = this.currentTutorialPages[this.currentPageIndex];
		this.headerLabel.text = LLBase.L(UITutorialWindow.GetHeaderLocaleId(this.data.Page, gameObject));
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x0014F774 File Offset: 0x0014D974
	private static string GetHeaderLocaleId(string defaultHeaderLocaleId, GameObject page)
	{
		UITutorialWindowPageHeaderOverride uitutorialWindowPageHeaderOverride;
		if (page.TryGetComponent<UITutorialWindowPageHeaderOverride>(out uitutorialWindowPageHeaderOverride))
		{
			return uitutorialWindowPageHeaderOverride.HeaderLocaleId;
		}
		return defaultHeaderLocaleId;
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x0014F793 File Offset: 0x0014D993
	public override void Close()
	{
		LazyInput.ClearAllKeysDown();
		base.Close();
		Action onCompleteCallback = this.data.OnCompleteCallback;
		if (onCompleteCallback == null)
		{
			return;
		}
		onCompleteCallback();
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x0014F7B8 File Offset: 0x0014D9B8
	private void ShowNextPage()
	{
		if (this.currentTutorialPages.Count <= 1 || this.currentPageIndex == this.currentTutorialPages.Count - 1)
		{
			return;
		}
		this.currentTutorialPages[this.currentPageIndex].SetActive(false);
		this.currentPageIndex++;
		this.currentTutorialPages[this.currentPageIndex].SetActive(true);
		this.UpdateHeaderLabel();
		this.UpdateButtons();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x0014F844 File Offset: 0x0014DA44
	private void ShowPrevPage()
	{
		if (this.currentTutorialPages.Count <= 1 || this.currentPageIndex == 0)
		{
			return;
		}
		this.currentTutorialPages[this.currentPageIndex].SetActive(false);
		this.currentPageIndex--;
		this.currentTutorialPages[this.currentPageIndex].SetActive(true);
		this.UpdateHeaderLabel();
		this.UpdateButtons();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x0014F8C0 File Offset: 0x0014DAC0
	private void UpdateButtons()
	{
		UITutorialWindowData data = this.data;
		bool flag = (data != null && data.CanCloseFromAnyPage) || this.currentPageIndex == this.currentTutorialPages.Count - 1;
		if (this.currentTutorialPages.Count == 1)
		{
			this.isOkBtnAvailable = true;
			this.nextButton.interactable = false;
			this.prevButton.interactable = false;
			this.nextButton.gameObject.SetActive(false);
			this.prevButton.gameObject.SetActive(false);
			this.arrowsObj.gameObject.SetActive(false);
		}
		else if (this.currentPageIndex == this.currentTutorialPages.Count - 1)
		{
			this.arrowsObj.gameObject.SetActive(true);
			this.isOkBtnAvailable = flag;
			this.nextButton.gameObject.SetActive(true);
			this.prevButton.gameObject.SetActive(true);
			this.nextButton.interactable = false;
			this.prevButton.interactable = true;
		}
		else if (this.currentPageIndex == 0)
		{
			this.arrowsObj.gameObject.SetActive(true);
			this.isOkBtnAvailable = flag;
			this.nextButton.gameObject.SetActive(true);
			this.prevButton.gameObject.SetActive(true);
			this.nextButton.interactable = true;
			this.prevButton.interactable = false;
		}
		else
		{
			this.arrowsObj.gameObject.SetActive(true);
			this.isOkBtnAvailable = flag;
			this.nextButton.gameObject.SetActive(true);
			this.prevButton.gameObject.SetActive(true);
			this.nextButton.interactable = true;
			this.prevButton.interactable = true;
		}
		this.ApplyVerticalLayoutPadding(this.arrowsObj.activeSelf);
		this.lazyButton.Draw(this.btnData);
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
			this.PrintTips();
		}
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x0014FAB8 File Offset: 0x0014DCB8
	private void ApplyVerticalLayoutPadding(bool hasArrows)
	{
		if (this.verticalLayoutGroup == null)
		{
			return;
		}
		RectOffset rectOffset = (hasArrows ? this.paddingWithArrows : this.paddingWithoutArrows);
		if (rectOffset == null)
		{
			return;
		}
		RectOffset rectOffset2 = this.verticalLayoutGroup.padding;
		if (rectOffset2 == null)
		{
			rectOffset2 = new RectOffset();
			this.verticalLayoutGroup.padding = rectOffset2;
		}
		rectOffset2.left = rectOffset.left;
		rectOffset2.right = rectOffset.right;
		rectOffset2.top = rectOffset.top;
		rectOffset2.bottom = rectOffset.bottom;
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x0014FB3B File Offset: 0x0014DD3B
	protected override bool OnPressedBack()
	{
		if (!this.isOkBtnAvailable)
		{
			return false;
		}
		this.Close();
		return true;
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x0014FB4E File Offset: 0x0014DD4E
	protected bool OnPressedLeft()
	{
		this.ShowPrevPage();
		return true;
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x0014FB57 File Offset: 0x0014DD57
	protected bool OnPressedRight()
	{
		this.ShowNextPage();
		return true;
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x0014FB60 File Offset: 0x0014DD60
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		gameKeyDelegates.Add(GameKey.DpadRight, new Func<bool>(this.OnPressedRight));
		gameKeyDelegates.Add(GameKey.DpadLeft, new Func<bool>(this.OnPressedLeft));
		gameKeyDelegates.Add(GameKey.Right, new Func<bool>(this.OnPressedRight));
		gameKeyDelegates.Add(GameKey.Left, new Func<bool>(this.OnPressedLeft));
		return gameKeyDelegates;
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x0014FBFC File Offset: 0x0014DDFC
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		if (this.nextButton.gameObject.activeSelf)
		{
			list.Add(new LazyGameKeyTip(GameKey.DpadRight, "tip_next", this.nextButton.interactable, true, true));
		}
		if (this.prevButton.gameObject.activeSelf)
		{
			list.Add(new LazyGameKeyTip(GameKey.DpadLeft, "tip_prev", this.prevButton.interactable, true, true));
		}
		list.Add(LazyGameKeyTip.Back(this.isOkBtnAvailable, true, true));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x0014FC9C File Offset: 0x0014DE9C
	public void TestOpen(string page)
	{
		UITutorialWindowData uitutorialWindowData = new UITutorialWindowData(page, null, false);
		this.Open(uitutorialWindowData);
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x0014FCB9 File Offset: 0x0014DEB9
	[LazyUITest]
	protected override void TestDraw()
	{
		this.TestOpen("tut_energy_hdr");
	}

	// Token: 0x060046F3 RID: 18163 RVA: 0x0014FCC6 File Offset: 0x0014DEC6
	[LazyUITest]
	protected void TestDrawTwoPages()
	{
		this.TestOpen("tutorial_hdr_questslog");
	}

	// Token: 0x0400374D RID: 14157
	[SerializeField]
	private TextMeshProUGUI headerLabel;

	// Token: 0x0400374E RID: 14158
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x0400374F RID: 14159
	[SerializeField]
	private LazyButton nextButton;

	// Token: 0x04003750 RID: 14160
	[SerializeField]
	private LazyButton prevButton;

	// Token: 0x04003751 RID: 14161
	[SerializeField]
	private List<GameObject> pages = new List<GameObject>();

	// Token: 0x04003752 RID: 14162
	[SerializeField]
	private GameObject arrowsObj;

	// Token: 0x04003753 RID: 14163
	[SerializeField]
	private VerticalLayoutGroup verticalLayoutGroup;

	// Token: 0x04003754 RID: 14164
	[SerializeField]
	private RectOffset paddingWithArrows;

	// Token: 0x04003755 RID: 14165
	[SerializeField]
	private RectOffset paddingWithoutArrows;

	// Token: 0x04003756 RID: 14166
	private List<GameObject> currentTutorialPages = new List<GameObject>();

	// Token: 0x04003757 RID: 14167
	private int currentPageIndex;

	// Token: 0x04003758 RID: 14168
	private UIDialogWindowData.ButtonData btnData;

	// Token: 0x04003759 RID: 14169
	private bool isOkBtnAvailable;
}
