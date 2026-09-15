using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A64 RID: 2660
public class UIMainMenuWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x06004809 RID: 18441 RVA: 0x00155B98 File Offset: 0x00153D98
	public override void Init()
	{
		this.UpdateDemoDependentStuff();
		this.startNewGameButton.onClick.AddListener(new UnityAction(this.OnStartNewGameButtonClicked));
		this.gameSettingsButton.onClick.AddListener(new UnityAction(this.OnGameSettingsButtonClicked));
		this.exitGameButton.onClick.AddListener(new UnityAction(this.OnExitGameButtonClicked));
		this.continueGameButton.onClick.AddListener(new UnityAction(this.OnContinueButtonClicked));
		this.loadGameButton.onClick.AddListener(new UnityAction(this.OnLoadButtonClicked));
		this.creditsButton.onClick.AddListener(new UnityAction(this.OnCreditsButtonClicked));
		if (this.consolesGameButton != null)
		{
			this.consolesGameButton.onClick.AddListener(new UnityAction(this.OnConsolesGameButtonClicked));
		}
		this.startNewGameButton.SetCallbacksIntoGamepadNavigationItem();
		this.gameSettingsButton.SetCallbacksIntoGamepadNavigationItem();
		this.exitGameButton.SetCallbacksIntoGamepadNavigationItem();
		this.continueGameButton.SetCallbacksIntoGamepadNavigationItem();
		this.loadGameButton.SetCallbacksIntoGamepadNavigationItem();
		this.creditsButton.SetCallbacksIntoGamepadNavigationItem();
		if (this.consolesGameButton != null)
		{
			this.consolesGameButton.SetCallbacksIntoGamepadNavigationItem();
		}
		this.SetupStoreButtons();
		this.exitGameButton.gameObject.SetActive(true);
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x0600480A RID: 18442 RVA: 0x00155D01 File Offset: 0x00153F01
	private void UpdateDemoDependentStuff()
	{
		this.demoLogo.SetActive(false);
		this.releaseLogo.SetActive(true);
	}

	// Token: 0x0600480B RID: 18443 RVA: 0x00155D1C File Offset: 0x00153F1C
	private void SetupStoreButtons()
	{
		this.UpdateStoreButtonsVisibility();
		UIMainMenuWindow.SetupSocialStoreButton(this.discordButton, new Action(this.OnDiscordButtonClicked));
		UIMainMenuWindow.SetupSocialStoreButton(this.twitterButton, new Action(this.OnTwitterButtonClicked));
		UIMainMenuWindow.SetupSocialStoreButton(this.bilibiliButton, new Action(this.OnBilibiliButtonClicked));
	}

	// Token: 0x0600480C RID: 18444 RVA: 0x00155D74 File Offset: 0x00153F74
	private void UpdateStoreButtonsVisibility()
	{
		UIMainMenuWindow.SetButtonActive(this.preorderButton, false);
		UIMainMenuWindow.SetButtonActive(this.leaveReviewButton, false);
		UIMainMenuWindow.SetButtonActive(this.discordButton, true);
		UIMainMenuWindow.SetButtonActive(this.twitterButton, true);
		UIMainMenuWindow.SetButtonActive(this.bilibiliButton, UIMainMenuWindow.IsChineseLanguageSelected());
	}

	// Token: 0x0600480D RID: 18445 RVA: 0x00155DC1 File Offset: 0x00153FC1
	private static void SetButtonActive(LazyButton button, bool active)
	{
		if (button != null)
		{
			button.gameObject.SetActive(active);
		}
	}

	// Token: 0x0600480E RID: 18446 RVA: 0x00155DD8 File Offset: 0x00153FD8
	private static void SetupSocialStoreButton(LazyButton button, Action onClick)
	{
		if (button == null)
		{
			return;
		}
		button.onClick.AddListener(delegate
		{
			onClick();
		});
		GamepadNavigationItem[] componentsInChildren = button.GetComponentsInChildren<GamepadNavigationItem>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Active = false;
		}
	}

	// Token: 0x0600480F RID: 18447 RVA: 0x00155E32 File Offset: 0x00154032
	private void OnDiscordButtonClicked()
	{
		Application.OpenURL("https://discord.gg/lazybeargames");
	}

	// Token: 0x06004810 RID: 18448 RVA: 0x00155E3E File Offset: 0x0015403E
	private void OnTwitterButtonClicked()
	{
		Application.OpenURL("https://x.com/lazybeargames");
	}

	// Token: 0x06004811 RID: 18449 RVA: 0x00155E4A File Offset: 0x0015404A
	private void OnBilibiliButtonClicked()
	{
		Application.OpenURL("https://space.bilibili.com/3707062611609883");
	}

	// Token: 0x06004812 RID: 18450 RVA: 0x00155E58 File Offset: 0x00154058
	private static bool IsChineseLanguageSelected()
	{
		string currentLang = LLBase.CurrentLang;
		return currentLang == "zh_cn" || currentLang == "zh_cht";
	}

	// Token: 0x06004813 RID: 18451 RVA: 0x00155E88 File Offset: 0x00154088
	public override void Open(LazyWidgetDataBase data)
	{
		DLCEngine.ResetDLCStateCached();
		base.Open(data);
		this.continueGameButton.SetKeepPressed(false);
		this.continueGameButton.interactable = true;
		SaveSlotData lastSaveSlot = SaveSystem.GetLastSaveSlot();
		this.continueGameButton.gameObject.SetActive(lastSaveSlot != null);
		this.loadGameButton.gameObject.SetActive(lastSaveSlot != null);
		bool isLimitedSaveSlotsEnabled = SaveSystem.IsLimitedSaveSlotsEnabled;
		this.startNewGameButton.gameObject.SetActive(!isLimitedSaveSlotsEnabled);
		this.loadGameButton.gameObject.SetActive(!isLimitedSaveSlotsEnabled && this.loadGameButton.gameObject.activeSelf);
		if (this.consolesGameButton != null)
		{
			this.consolesGameButton.gameObject.SetActive(isLimitedSaveSlotsEnabled);
		}
		this.UpdateStoreButtonsVisibility();
		if (LazyInput.IsGamepadActive)
		{
			base.GamepadNavigationController.ReinitItems(true, null, null);
		}
		((RectTransform)base.transform).RefreshContentFitterAndDisable();
		if (this.pendingPreloaderOverlay != null)
		{
			UIPreloadOverlay uipreloadOverlay = this.pendingPreloaderOverlay;
			this.pendingPreloaderOverlay = null;
			this.PlayPreloaderIntro(uipreloadOverlay);
		}
	}

	// Token: 0x06004814 RID: 18452 RVA: 0x00155F94 File Offset: 0x00154194
	public void OpenFromPreloader(UIPreloadOverlay overlay, Action onComplete)
	{
		this.pendingPreloaderOverlay = overlay;
		this.onPreloaderIntroComplete = onComplete;
		this.Open(null);
	}

	// Token: 0x06004815 RID: 18453 RVA: 0x00155FAB File Offset: 0x001541AB
	protected override void HideWindow()
	{
		this.CompleteIntroImmediate();
		base.HideWindow();
	}

	// Token: 0x06004816 RID: 18454 RVA: 0x00155FB9 File Offset: 0x001541B9
	public bool IsContinueButtonWillBeActive()
	{
		return SaveSystem.GetLastSaveSlot() != null;
	}

	// Token: 0x06004817 RID: 18455 RVA: 0x00155FC3 File Offset: 0x001541C3
	public void OnStartNewGameButtonClicked()
	{
		Debug.Log("OnStartNewGameButtonClicked");
		MainGame.Instance.StartNewGame(false);
		this.Close();
	}

	// Token: 0x06004818 RID: 18456 RVA: 0x00155FE0 File Offset: 0x001541E0
	public void OnContinueButtonClicked()
	{
		SaveSlotData saveSlotData = SaveSystem.GetActiveSaveData();
		Debug.Log("OnContinueButtonClicked saveSlotData:[" + saveSlotData.slotName + "]");
		this.continueGameButton.SetKeepPressed(true);
		this.continueGameButton.interactable = false;
		UILoadingOverlay overlay = LazyUI.Get<UILoadingOverlay>();
		Action<GameSave> <>9__1;
		overlay.Draw(new LoadingWindowData(MainGame.EntrySceneToLoad, delegate
		{
			SaveSlotData saveSlotData2 = saveSlotData;
			Action<GameSave> action;
			if ((action = <>9__1) == null)
			{
				action = (<>9__1 = delegate(GameSave s)
				{
					if (s != null)
					{
						MainGame.Instance.ContinueGame(saveSlotData, s);
						this.Close();
						return;
					}
					overlay.Hide();
					this.continueGameButton.SetKeepPressed(false);
					this.continueGameButton.interactable = true;
				});
			}
			SaveSystem.Load(saveSlotData2, action);
		}, false));
	}

	// Token: 0x06004819 RID: 18457 RVA: 0x00156069 File Offset: 0x00154269
	public void OnLoadButtonClicked()
	{
		this.Close();
		LazyUI.GetWindow<UISaveSlotsWindow>().Open(null);
	}

	// Token: 0x0600481A RID: 18458 RVA: 0x0015607C File Offset: 0x0015427C
	public void OnConsolesGameButtonClicked()
	{
		this.Close();
		LazyUI.GetWindow<UISaveSlotsWindowLimited>().Open(null);
	}

	// Token: 0x0600481B RID: 18459 RVA: 0x0015608F File Offset: 0x0015428F
	private void OnStartHostButtonClicked()
	{
		this.Close();
		LazyUI.GetWindow<UIStartHostGameWindow>().Open(null);
	}

	// Token: 0x0600481C RID: 18460 RVA: 0x001560A2 File Offset: 0x001542A2
	private void OnConnectToHostButtonClicked()
	{
		this.Close();
		LazyUI.GetWindow<UIConnectToHostGameWindow>().Open(null);
	}

	// Token: 0x0600481D RID: 18461 RVA: 0x001560B5 File Offset: 0x001542B5
	private void OnGameSettingsButtonClicked()
	{
		this.Close();
		UIGameSettingsWindow window = LazyUI.GetWindow<UIGameSettingsWindow>();
		window.onClosed = delegate
		{
			this.Open(null);
		};
		window.Open(null);
	}

	// Token: 0x0600481E RID: 18462 RVA: 0x001560DA File Offset: 0x001542DA
	private void OnCreditsButtonClicked()
	{
		MainGame.Instance.SetMainMenuInfoPanelEnabled(false);
		this.Close();
		LazyUI.GetWindow<UICreditsWindow>().Open(null);
	}

	// Token: 0x0600481F RID: 18463 RVA: 0x001560F8 File Offset: 0x001542F8
	private void OnExitGameButtonClicked()
	{
		GameShutdown.RequestQuit();
		Application.Quit();
	}

	// Token: 0x06004820 RID: 18464 RVA: 0x00156104 File Offset: 0x00154304
	private void PlayPreloaderIntro(UIPreloadOverlay overlay)
	{
		RectTransform activeLogoRect = this.GetActiveLogoRect();
		RectTransform rectTransform = ((activeLogoRect != null) ? (activeLogoRect.parent as RectTransform) : null);
		RectTransform rectTransform2 = ((rectTransform != null) ? (rectTransform.parent as RectTransform) : null);
		if (activeLogoRect == null || rectTransform == null || rectTransform2 == null)
		{
			this.CompleteIntroImmediate();
			return;
		}
		this.ResetIntroVisuals();
		this.introLogo = activeLogoRect;
		this.introLogoRestAnchored = activeLogoRect.anchoredPosition;
		this.introLogoGroup = UIMainMenuWindow.GetOrAddCanvasGroup(rectTransform.gameObject);
		this.introLogoGroup.ignoreParentGroups = true;
		this.introLogoGroup.alpha = 1f;
		this.introButtonsGroup = UIMainMenuWindow.GetOrAddCanvasGroup(rectTransform2.gameObject);
		this.introButtonsGroup.alpha = 0f;
		this.introButtonsGroup.blocksRaycasts = false;
		this.introButtonRests.Clear();
		for (int i = 0; i < rectTransform2.childCount; i++)
		{
			RectTransform rectTransform3 = rectTransform2.GetChild(i) as RectTransform;
			if (!(rectTransform3 == null) && !(rectTransform3 == rectTransform) && rectTransform3.gameObject.activeSelf)
			{
				this.introButtonRests.Add(new ValueTuple<RectTransform, Vector2>(rectTransform3, rectTransform3.anchoredPosition));
				rectTransform3.anchoredPosition -= new Vector2(0f, this.buttonsMoveOffset);
			}
		}
		Canvas.ForceUpdateCanvases();
		RectTransform rectTransform4 = ((overlay != null) ? overlay.GetActiveLogoRect() : null);
		if (rectTransform4 != null)
		{
			UIMainMenuWindow.MatchPivotToScreenOf(activeLogoRect, rectTransform4);
		}
		if (overlay != null)
		{
			overlay.HideActiveLogo();
		}
		float num = this.logoMoveDuration;
		float num2 = num * Mathf.Clamp01(this.buttonsStartAtLogoProgress);
		this.introSequence = DOTween.Sequence().SetUpdate(true).SetLink(base.gameObject, LinkBehaviour.KillOnDisable);
		this.introSequence.Join(activeLogoRect.DOAnchorPos(this.introLogoRestAnchored, num, false).SetEase(this.logoMoveEase));
		this.introSequence.Insert(num2, this.introButtonsGroup.DOFade(1f, this.buttonsMoveDuration).SetEase(Ease.OutQuad));
		for (int j = 0; j < this.introButtonRests.Count; j++)
		{
			ValueTuple<RectTransform, Vector2> valueTuple = this.introButtonRests[j];
			RectTransform item = valueTuple.Item1;
			Vector2 item2 = valueTuple.Item2;
			this.introSequence.Insert(num2, item.DOAnchorPos(item2, this.buttonsMoveDuration, false).SetEase(this.buttonsMoveEase));
		}
		this.introSequence.OnComplete(delegate
		{
			this.introSequence = null;
			this.introLogo = null;
			this.introButtonRests.Clear();
			if (this.introButtonsGroup != null)
			{
				this.introButtonsGroup.blocksRaycasts = true;
			}
			this.NotifyIntroCompleted();
		});
		this.introSequence.OnKill(delegate
		{
			this.introSequence = null;
			this.NotifyIntroCompleted();
		});
	}

	// Token: 0x06004821 RID: 18465 RVA: 0x001563AE File Offset: 0x001545AE
	private void CompleteIntroImmediate()
	{
		this.ResetIntroVisuals();
		this.NotifyIntroCompleted();
	}

	// Token: 0x06004822 RID: 18466 RVA: 0x001563BC File Offset: 0x001545BC
	private void ResetIntroVisuals()
	{
		Sequence sequence = this.introSequence;
		if (sequence != null)
		{
			sequence.Kill(false);
		}
		this.introSequence = null;
		if (this.introLogo != null)
		{
			this.introLogo.anchoredPosition = this.introLogoRestAnchored;
		}
		for (int i = 0; i < this.introButtonRests.Count; i++)
		{
			ValueTuple<RectTransform, Vector2> valueTuple = this.introButtonRests[i];
			RectTransform item = valueTuple.Item1;
			Vector2 item2 = valueTuple.Item2;
			if (item != null)
			{
				item.anchoredPosition = item2;
			}
		}
		this.introButtonRests.Clear();
		if (this.introButtonsGroup != null)
		{
			this.introButtonsGroup.alpha = 1f;
			this.introButtonsGroup.blocksRaycasts = true;
		}
		if (this.introLogoGroup != null)
		{
			this.introLogoGroup.alpha = 1f;
		}
		this.introLogo = null;
	}

	// Token: 0x06004823 RID: 18467 RVA: 0x0015649A File Offset: 0x0015469A
	private void NotifyIntroCompleted()
	{
		Action action = this.onPreloaderIntroComplete;
		this.onPreloaderIntroComplete = null;
		if (action == null)
		{
			return;
		}
		action();
	}

	// Token: 0x06004824 RID: 18468 RVA: 0x001564B4 File Offset: 0x001546B4
	private RectTransform GetActiveLogoRect()
	{
		GameObject gameObject = (this.releaseLogo.activeSelf ? this.releaseLogo : this.demoLogo);
		if (!(gameObject != null))
		{
			return null;
		}
		return (RectTransform)gameObject.transform;
	}

	// Token: 0x06004825 RID: 18469 RVA: 0x001564F4 File Offset: 0x001546F4
	private static CanvasGroup GetOrAddCanvasGroup(GameObject target)
	{
		CanvasGroup canvasGroup;
		if (!target.TryGetComponent<CanvasGroup>(out canvasGroup))
		{
			canvasGroup = target.AddComponent<CanvasGroup>();
		}
		return canvasGroup;
	}

	// Token: 0x06004826 RID: 18470 RVA: 0x00156514 File Offset: 0x00154714
	private static void MatchPivotToScreenOf(RectTransform target, RectTransform source)
	{
		Camera canvasCamera = UIMainMenuWindow.GetCanvasCamera(source);
		Camera canvasCamera2 = UIMainMenuWindow.GetCanvasCamera(target);
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(canvasCamera, source.position);
		RectTransform rectTransform = target.parent as RectTransform;
		if (rectTransform == null)
		{
			return;
		}
		Vector3 vector2;
		if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, vector, canvasCamera2, out vector2))
		{
			target.position = vector2;
		}
	}

	// Token: 0x06004827 RID: 18471 RVA: 0x00156564 File Offset: 0x00154764
	private static Camera GetCanvasCamera(RectTransform rect)
	{
		Canvas canvas = rect.GetComponentInParent<Canvas>();
		if (canvas == null)
		{
			return null;
		}
		canvas = canvas.rootCanvas;
		if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			return canvas.worldCamera;
		}
		return null;
	}

	// Token: 0x06004828 RID: 18472 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003835 RID: 14389
	[SerializeField]
	private LazyButton startNewGameButton;

	// Token: 0x04003836 RID: 14390
	[SerializeField]
	private LazyButton continueGameButton;

	// Token: 0x04003837 RID: 14391
	[SerializeField]
	private LazyButton loadGameButton;

	// Token: 0x04003838 RID: 14392
	[SerializeField]
	private LazyButton gameSettingsButton;

	// Token: 0x04003839 RID: 14393
	[SerializeField]
	private LazyButton creditsButton;

	// Token: 0x0400383A RID: 14394
	[SerializeField]
	private LazyButton exitGameButton;

	// Token: 0x0400383B RID: 14395
	[SerializeField]
	private LazyButton consolesGameButton;

	// Token: 0x0400383C RID: 14396
	[Header("Store Buttons")]
	[SerializeField]
	private LazyButton preorderButton;

	// Token: 0x0400383D RID: 14397
	[SerializeField]
	private LazyButton leaveReviewButton;

	// Token: 0x0400383E RID: 14398
	[SerializeField]
	private LazyButton discordButton;

	// Token: 0x0400383F RID: 14399
	[SerializeField]
	private LazyButton twitterButton;

	// Token: 0x04003840 RID: 14400
	[SerializeField]
	private LazyButton bilibiliButton;

	// Token: 0x04003841 RID: 14401
	[SerializeField]
	private GameObject releaseLogo;

	// Token: 0x04003842 RID: 14402
	[SerializeField]
	private GameObject demoLogo;

	// Token: 0x04003843 RID: 14403
	[SerializeField]
	private float logoMoveDuration = 0.5f;

	// Token: 0x04003844 RID: 14404
	[SerializeField]
	private Ease logoMoveEase = Ease.OutCubic;

	// Token: 0x04003845 RID: 14405
	[SerializeField]
	private float buttonsMoveDuration = 0.35f;

	// Token: 0x04003846 RID: 14406
	[SerializeField]
	private float buttonsMoveOffset = 18f;

	// Token: 0x04003847 RID: 14407
	[SerializeField]
	private Ease buttonsMoveEase = Ease.OutCubic;

	// Token: 0x04003848 RID: 14408
	[SerializeField]
	private float buttonsStartAtLogoProgress = 0.8f;

	// Token: 0x04003849 RID: 14409
	private UIPreloadOverlay pendingPreloaderOverlay;

	// Token: 0x0400384A RID: 14410
	private Action onPreloaderIntroComplete;

	// Token: 0x0400384B RID: 14411
	private Sequence introSequence;

	// Token: 0x0400384C RID: 14412
	private RectTransform introLogo;

	// Token: 0x0400384D RID: 14413
	private Vector2 introLogoRestAnchored;

	// Token: 0x0400384E RID: 14414
	private CanvasGroup introButtonsGroup;

	// Token: 0x0400384F RID: 14415
	private CanvasGroup introLogoGroup;

	// Token: 0x04003850 RID: 14416
	[TupleElementNames(new string[] { "rect", "rest" })]
	private readonly List<ValueTuple<RectTransform, Vector2>> introButtonRests = new List<ValueTuple<RectTransform, Vector2>>();
}
