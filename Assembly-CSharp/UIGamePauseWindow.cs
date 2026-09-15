using System;
using System.Runtime.CompilerServices;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000A61 RID: 2657
public class UIGamePauseWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x060047D2 RID: 18386 RVA: 0x00154BF4 File Offset: 0x00152DF4
	public override void Init()
	{
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
		bool flag = false;
		this.controlsBtn.gameObject.SetActive(!flag);
		this.goToMenuBtn.gameObject.SetActive(!flag);
		if (this.goToMenuSpacing != null)
		{
			this.goToMenuSpacing.SetActive(this.goToMenuBtn.gameObject.activeSelf);
		}
		this.restartBtn.gameObject.SetActive(flag);
		if (this.tutorialListBtn == null)
		{
			this.tutorialListBtn = UIGamePauseWindow.CreateRuntimeTutorialListButton(base.transform);
		}
		this.settingsBtn.onClick.AddListener(delegate
		{
			this.Close();
			UIGameSettingsWindow window = LazyUI.GetWindow<UIGameSettingsWindow>();
			window.onClosed = delegate
			{
				this.Open(null);
			};
			window.Open(null);
		});
		this.settingsBtn.SetCallbacksIntoGamepadNavigationItem();
		if (this.tutorialListBtn != null)
		{
			this.tutorialListBtn.onClick.AddListener(new UnityAction(this.OpenTutorialList));
			this.tutorialListBtn.SetCallbacksIntoGamepadNavigationItem();
		}
		KnowledgeSystem.OnTutorialViewed = (Action<string>)Delegate.Remove(KnowledgeSystem.OnTutorialViewed, new Action<string>(this.HandleTutorialViewed));
		KnowledgeSystem.OnTutorialViewed = (Action<string>)Delegate.Combine(KnowledgeSystem.OnTutorialViewed, new Action<string>(this.HandleTutorialViewed));
		this.UpdateTutorialListButton();
		if (!flag)
		{
			this.controlsBtn.onClick.AddListener(delegate
			{
				this.Close();
				UIGameBindingSettingsWindow window2 = LazyUI.GetWindow<UIGameBindingSettingsWindow>();
				window2.onClosed = delegate
				{
					this.Open(null);
				};
				window2.Open(null);
			});
			this.goToMenuBtn.onClick.AddListener(new UnityAction(this.OnPressedGoToMainMenu));
			this.goToMenuBtn.SetCallbacksIntoGamepadNavigationItem();
			this.controlsBtn.SetCallbacksIntoGamepadNavigationItem();
		}
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x00154D89 File Offset: 0x00152F89
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		this.UpdateTutorialListButton();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x00154DA8 File Offset: 0x00152FA8
	public override void Close()
	{
		base.Close();
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x00154DB0 File Offset: 0x00152FB0
	private void OpenTutorialList()
	{
		this.Close();
		LazyUI.GetWindow<UITutorialListWindow>().Open(new UITutorialListWindowData(UITutorialListOpenSource.PauseWindow));
	}

	// Token: 0x060047D6 RID: 18390 RVA: 0x00154DC8 File Offset: 0x00152FC8
	private void HandleTutorialViewed(string _)
	{
		this.UpdateTutorialListButton();
	}

	// Token: 0x060047D7 RID: 18391 RVA: 0x00154DD0 File Offset: 0x00152FD0
	private void UpdateTutorialListButton()
	{
		if (!(this.tutorialListBtn == null))
		{
			MainGame instance = MainGame.Instance;
			bool flag;
			if (instance == null)
			{
				flag = null != null;
			}
			else
			{
				GameSave gameSave = instance.GameSave;
				flag = ((gameSave != null) ? gameSave.knowledgeSystem : null) != null;
			}
			if (flag)
			{
				this.tutorialListBtn.gameObject.SetActive(MainGame.Instance.GameSave.knowledgeSystem.HasViewedTutorials());
				return;
			}
		}
	}

	// Token: 0x060047D8 RID: 18392 RVA: 0x00154E2F File Offset: 0x0015302F
	private void OnDestroy()
	{
		KnowledgeSystem.OnTutorialViewed = (Action<string>)Delegate.Remove(KnowledgeSystem.OnTutorialViewed, new Action<string>(this.HandleTutorialViewed));
	}

	// Token: 0x060047D9 RID: 18393 RVA: 0x00154E54 File Offset: 0x00153054
	private static LazyButton CreateRuntimeTutorialListButton(Transform parent)
	{
		GameObject gameObject = new GameObject("TutorialListButton", new Type[] { typeof(RectTransform) });
		gameObject.transform.SetParent(parent, false);
		RectTransform rectTransform = (RectTransform)gameObject.transform;
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchoredPosition = new Vector2(0f, -110f);
		rectTransform.sizeDelta = new Vector2(220f, 42f);
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color(0.2f, 0.1f, 0.08f, 0.95f);
		LazyButton lazyButton = gameObject.AddComponent<LazyButton>();
		lazyButton.targetGraphic = image;
		gameObject.AddComponent<GamepadNavigationItem>();
		GameObject gameObject2 = new GameObject("Label", new Type[] { typeof(RectTransform) });
		gameObject2.transform.SetParent(gameObject.transform, false);
		RectTransform rectTransform2 = (RectTransform)gameObject2.transform;
		rectTransform2.anchorMin = Vector2.zero;
		rectTransform2.anchorMax = Vector2.one;
		rectTransform2.offsetMin = Vector2.zero;
		rectTransform2.offsetMax = Vector2.zero;
		TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
		textMeshProUGUI.text = "Tutorials";
		textMeshProUGUI.alignment = TextAlignmentOptions.Center;
		textMeshProUGUI.fontSize = 18f;
		textMeshProUGUI.color = Color.white;
		return lazyButton;
	}

	// Token: 0x060047DA RID: 18394 RVA: 0x00154FD0 File Offset: 0x001531D0
	public void OnPressedGoToMainMenu()
	{
		LazyUI.GetWindow<UIDialogWindow>().Open(new UIDialogWindowData(LLBase.L("exit_menu_confirm"), LLBase.L("exit_menu_confirm_txt_new"), new Action(this.<OnPressedGoToMainMenu>g__Yes|14_0), new Action(LazyUI.GetWindow<UIDialogWindow>().Close), false)
		{
			ShowCloseButton = true
		});
	}

	// Token: 0x060047DB RID: 18395 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x060047E1 RID: 18401 RVA: 0x00155071 File Offset: 0x00153271
	[CompilerGenerated]
	private void <OnPressedGoToMainMenu>g__Yes|14_0()
	{
		LazyUI.GetWindow<UIDialogWindow>().Close();
		this.Close();
		MainGame.Instance.GoToMenu(null, false, FadeFlag.Common);
	}

	// Token: 0x04003803 RID: 14339
	[SerializeField]
	private LazyButton settingsBtn;

	// Token: 0x04003804 RID: 14340
	[SerializeField]
	private LazyButton controlsBtn;

	// Token: 0x04003805 RID: 14341
	[SerializeField]
	private LazyButton goToMenuBtn;

	// Token: 0x04003806 RID: 14342
	[SerializeField]
	private LazyButton restartBtn;

	// Token: 0x04003807 RID: 14343
	[SerializeField]
	private GameObject goToMenuSpacing;

	// Token: 0x04003808 RID: 14344
	[SerializeField]
	private LazyButton tutorialListBtn;
}
