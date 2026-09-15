using System;
using System.Runtime.CompilerServices;
using DG.Tweening;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A58 RID: 2648
public class UICreditsWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x17000AD3 RID: 2771
	// (get) Token: 0x0600475F RID: 18271 RVA: 0x00151F2C File Offset: 0x0015012C
	public RectTransform CameraTrackRect
	{
		get
		{
			return this.firstElementRect;
		}
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x00151F34 File Offset: 0x00150134
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		this.RestoreEndGameScrollRectInsets();
		this.InitializeBackButton(true);
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOKill(false);
		this.canvasGroup.DOFade(1f, this.fadeTime);
		if (!this.CreateContentIfNeeded())
		{
			return;
		}
		if (this.tween != null && this.tween.active)
		{
			this.tween.Kill(false);
		}
		this.scrollRect.StopMovement();
		this.scrollRect.verticalNormalizedPosition = 1f;
		this.tweenTime = this.CalculateTweenTime();
		if (this.tweenTime <= 0f)
		{
			Debug.LogWarning("UICreditsWindow scroll distance is zero. Credits auto-scroll was not started.");
			return;
		}
		this.tween = this.scrollRect.DOVerticalNormalizedPos(0f, this.tweenTime, false);
		this.tween.SetEase(Ease.Linear);
		this.tween.OnComplete(new TweenCallback(this.ReturnToMainMenu));
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x00152034 File Offset: 0x00150234
	public void OpenAfterGameComplete(bool shouldGoToMenuOnReturn, Action onClosed)
	{
		base.Open(null, delegate(LazyWidgetDataBase _)
		{
			Action onClosed2 = onClosed;
			if (onClosed2 == null)
			{
				return;
			}
			onClosed2();
		});
		this.openAfterGameComplete = true;
		this.shouldGoToMenuOnReturn = shouldGoToMenuOnReturn;
		this.ApplyEndGameSortingOrder();
		if (shouldGoToMenuOnReturn)
		{
			this.ApplyEndGameScrollRectInsets();
		}
		this.InitializeBackButton(false);
		this.backgroundImage.color = new Color(this.backgroundImage.color.r, this.backgroundImage.color.g, this.backgroundImage.color.b, shouldGoToMenuOnReturn ? 0f : 0.5f);
		this.canvasGroup.alpha = 0f;
		this.canvasGroup.DOKill(false);
		this.canvasGroup.DOFade(1f, this.fadeTime);
		if (!this.CreateContentIfNeeded())
		{
			return;
		}
		if (shouldGoToMenuOnReturn)
		{
			this.ApplyEndGameContentStartFromViewportBottom();
		}
		if (this.tween != null && this.tween.active)
		{
			this.tween.Kill(false);
		}
		this.scrollRect.StopMovement();
		this.scrollRect.verticalNormalizedPosition = 1f;
		this.tweenTime = this.CalculateTweenTime();
		if (shouldGoToMenuOnReturn)
		{
			this.EnableCreditsCamera();
		}
		if (this.tweenTime <= 0f)
		{
			Debug.LogWarning("UICreditsWindow scroll distance is zero. Credits auto-scroll was not started.");
			return;
		}
		this.tween = this.scrollRect.DOVerticalNormalizedPos(0f, this.tweenTime, false);
		this.tween.SetEase(Ease.Linear);
		this.tween.OnComplete(new TweenCallback(this.ReturnToMainMenu));
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x001521C5 File Offset: 0x001503C5
	public override void Close()
	{
		this.DisableCreditsCamera();
		this.RestoreEndGameSortingOrder();
		this.RestoreEndGameScrollRectInsets();
		base.Close();
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x001521E0 File Offset: 0x001503E0
	public void ShowBackButton()
	{
		if (this.isBackButtonShown)
		{
			return;
		}
		this.lazyButton.gameObject.SetActive(true);
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.OnPressBtn), LLBase.L("tip_back"), null, true, GameKey.Back, "");
		this.lazyButton.Draw(this.btnData);
		this.isBackButtonShown = true;
	}

	// Token: 0x06004765 RID: 18277 RVA: 0x0015224C File Offset: 0x0015044C
	public void HideBackButton()
	{
		if (!this.isBackButtonShown)
		{
			return;
		}
		this.lazyButton.gameObject.SetActive(false);
		this.isBackButtonShown = false;
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x00152270 File Offset: 0x00150470
	private void InitializeBackButton(bool showImmediately = true)
	{
		this.lazyButton.gameObject.SetActive(showImmediately);
		if (showImmediately)
		{
			this.btnData = new UIDialogWindowData.ButtonData(new Action(this.OnPressBtn), LLBase.L("tip_back"), null, true, GameKey.Back, "");
			this.lazyButton.Draw(this.btnData);
			this.isBackButtonShown = true;
			return;
		}
		this.isBackButtonShown = false;
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x001522DE File Offset: 0x001504DE
	private void EnableCreditsCamera()
	{
		CreditsCameraController.TryEnable(this);
		this.creditsCameraEnabled = true;
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x001522ED File Offset: 0x001504ED
	private void DisableCreditsCamera()
	{
		if (!this.creditsCameraEnabled)
		{
			return;
		}
		this.creditsCameraEnabled = false;
		CreditsCameraController.TryDisable();
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x00152304 File Offset: 0x00150504
	private void ApplyEndGameSortingOrder()
	{
		if (base.Canvas == null)
		{
			return;
		}
		if (!this.isSortingOrderCached)
		{
			this.cachedOverrideSorting = base.Canvas.overrideSorting;
			this.cachedSortingOrder = base.Canvas.sortingOrder;
			this.isSortingOrderCached = true;
		}
		base.Canvas.overrideSorting = true;
		base.Canvas.sortingOrder = 30000;
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x0015236D File Offset: 0x0015056D
	private void RestoreEndGameSortingOrder()
	{
		if (!this.isSortingOrderCached)
		{
			return;
		}
		base.Canvas.overrideSorting = this.cachedOverrideSorting;
		base.Canvas.sortingOrder = this.cachedSortingOrder;
		this.isSortingOrderCached = false;
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x001523A4 File Offset: 0x001505A4
	private void ApplyEndGameScrollRectInsets()
	{
		RectTransform scrollRectTransform = this.GetScrollRectTransform();
		if (scrollRectTransform == null)
		{
			return;
		}
		this.CacheScrollRectInsetsIfNeeded(scrollRectTransform);
		scrollRectTransform.offsetMin = Vector2.zero;
		scrollRectTransform.offsetMax = Vector2.zero;
		this.endGameScrollRectInsetsApplied = true;
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x001523E8 File Offset: 0x001505E8
	private void ApplyEndGameContentStartFromViewportBottom()
	{
		VerticalLayoutGroup contentLayoutGroup = this.GetContentLayoutGroup();
		RectTransform viewport = this.GetViewport();
		if (contentLayoutGroup == null || this.content == null || viewport == null)
		{
			return;
		}
		Canvas.ForceUpdateCanvases();
		int num = Mathf.RoundToInt(viewport.rect.height);
		contentLayoutGroup.padding = new RectOffset(0, 0, num, num);
		this.content.RefreshContentFitterAndDisable();
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x00152458 File Offset: 0x00150658
	private void RestoreEndGameScrollRectInsets()
	{
		if (!this.endGameScrollRectInsetsApplied)
		{
			return;
		}
		RectTransform scrollRectTransform = this.GetScrollRectTransform();
		if (scrollRectTransform != null && this.isScrollRectInsetsCached)
		{
			scrollRectTransform.offsetMin = this.cachedScrollOffsetMin;
			scrollRectTransform.offsetMax = this.cachedScrollOffsetMax;
		}
		VerticalLayoutGroup contentLayoutGroup = this.GetContentLayoutGroup();
		if (contentLayoutGroup != null && this.cachedContentPadding != null)
		{
			contentLayoutGroup.padding = new RectOffset(this.cachedContentPadding.left, this.cachedContentPadding.right, this.cachedContentPadding.top, this.cachedContentPadding.bottom);
		}
		if (this.isContentCreated && this.content != null)
		{
			this.content.RefreshContentFitterAndDisable();
		}
		this.endGameScrollRectInsetsApplied = false;
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x00152515 File Offset: 0x00150715
	private RectTransform GetViewport()
	{
		if (this.scrollRect != null && this.scrollRect.viewport != null)
		{
			return this.scrollRect.viewport;
		}
		return this.GetScrollRectTransform();
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x0015254C File Offset: 0x0015074C
	private void CacheScrollRectInsetsIfNeeded(RectTransform scrollRectTransform)
	{
		if (this.isScrollRectInsetsCached)
		{
			return;
		}
		this.cachedScrollOffsetMin = scrollRectTransform.offsetMin;
		this.cachedScrollOffsetMax = scrollRectTransform.offsetMax;
		VerticalLayoutGroup contentLayoutGroup = this.GetContentLayoutGroup();
		if (contentLayoutGroup != null)
		{
			RectOffset padding = contentLayoutGroup.padding;
			this.cachedContentPadding = new RectOffset(padding.left, padding.right, padding.top, padding.bottom);
		}
		this.isScrollRectInsetsCached = true;
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x001525BB File Offset: 0x001507BB
	private RectTransform GetScrollRectTransform()
	{
		if (this.scrollRect == null)
		{
			return null;
		}
		return (RectTransform)this.scrollRect.transform;
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x001525E0 File Offset: 0x001507E0
	private VerticalLayoutGroup GetContentLayoutGroup()
	{
		if (this.content == null && this.scrollRect != null)
		{
			this.content = this.scrollRect.content;
		}
		if (this.content == null)
		{
			return null;
		}
		return this.content.GetComponent<VerticalLayoutGroup>();
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x00152638 File Offset: 0x00150838
	private bool CreateContentIfNeeded()
	{
		if (this.isContentCreated)
		{
			return true;
		}
		if (this.content == null && this.scrollRect != null)
		{
			this.content = this.scrollRect.content;
		}
		if (this.content == null)
		{
			Debug.LogError("UICreditsWindow content is not set.");
			return false;
		}
		if (this.prefab == null)
		{
			Debug.LogError("UICreditsWindow element prefab is not set.");
			return false;
		}
		if (this.spacePrefab == null)
		{
			Debug.LogError("UICreditsWindow space prefab is not set.");
			return false;
		}
		this.ClearContent();
		this.AddRows(UICreditsWindow.groupTeams);
		this.AddSpace();
		this.AddText(UICreditsWindow.lazyBearTeamString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupLazyBear);
		this.AddSpace();
		this.AddText(UICreditsWindow.translationTeamString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupTranslation);
		this.AddSpace();
		this.AddText(UICreditsWindow.translationTeamAkebonoString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupTranslationAkebono);
		this.AddSpace();
		this.AddText(UICreditsWindow.fromTheVoidString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupFromTheVoid);
		this.AddSpace();
		this.AddText(UICreditsWindow.omukString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupOmuk);
		this.AddSpace();
		this.AddText(UICreditsWindow.voiceOverString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupVoiceOver);
		this.AddSpace();
		this.AddText(UICreditsWindow.tinyBuildTeamString);
		this.AddSpace();
		this.AddRows(UICreditsWindow.groupTinyBuild);
		this.content.RefreshContentFitterAndDisable();
		this.isContentCreated = true;
		return true;
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x001527DC File Offset: 0x001509DC
	private void ClearContent()
	{
		this.firstElementRect = null;
		for (int i = this.content.childCount - 1; i >= 0; i--)
		{
			Transform child = this.content.GetChild(i);
			child.SetParent(null, false);
			global::UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x00152828 File Offset: 0x00150A28
	private void AddRows([TupleElementNames(new string[] { "leftText", "rightText" })] ValueTuple<string, string>[] rows)
	{
		foreach (ValueTuple<string, string> valueTuple in rows)
		{
			this.AddRow(valueTuple);
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x00152854 File Offset: 0x00150A54
	private void AddRow([TupleElementNames(new string[] { "leftText", "rightText" })] ValueTuple<string, string> row)
	{
		if (string.IsNullOrEmpty(row.Item1) && string.IsNullOrEmpty(row.Item2))
		{
			this.AddSpace();
			return;
		}
		UICreditsWindowElement uicreditsWindowElement = this.AddElement();
		if (uicreditsWindowElement == null)
		{
			return;
		}
		uicreditsWindowElement.DrawRow(row.Item1, row.Item2);
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x001528A8 File Offset: 0x00150AA8
	private void AddText(string text)
	{
		UICreditsWindowElement uicreditsWindowElement = this.AddElement();
		if (uicreditsWindowElement == null)
		{
			return;
		}
		uicreditsWindowElement.DrawCenter(text);
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x001528D0 File Offset: 0x00150AD0
	private UICreditsWindowElement AddElement()
	{
		UICreditsWindowElement uicreditsWindowElement = global::UnityEngine.Object.Instantiate<UICreditsWindowElement>(this.prefab, this.content);
		uicreditsWindowElement.gameObject.SetActive(true);
		if (this.firstElementRect == null)
		{
			this.firstElementRect = (RectTransform)uicreditsWindowElement.transform;
		}
		return uicreditsWindowElement;
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x0015291B File Offset: 0x00150B1B
	private void AddSpace()
	{
		global::UnityEngine.Object.Instantiate<GameObject>(this.spacePrefab, this.content).SetActive(true);
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x00152934 File Offset: 0x00150B34
	private float CalculateTweenTime()
	{
		Canvas.ForceUpdateCanvases();
		LayoutRebuilder.ForceRebuildLayoutImmediate(this.content);
		RectTransform viewport = this.GetViewport();
		float num = Mathf.Max(this.content.rect.height, LayoutUtility.GetPreferredHeight(this.content));
		float num2 = Mathf.Max(0f, num - viewport.rect.height);
		if (num2 <= 0f)
		{
			return 0f;
		}
		return num2 / Mathf.Max(0.01f, this.scrollSpeed);
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x001529B7 File Offset: 0x00150BB7
	private void OnPressBtn()
	{
		this.OnPressedBack();
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x001529C0 File Offset: 0x00150BC0
	protected override bool OnPressedBack()
	{
		if (!this.lazyButton.gameObject.activeSelf || !this.isBackButtonShown)
		{
			return false;
		}
		if (this.tween != null && this.tween.active)
		{
			this.tween.Kill(false);
		}
		this.ReturnToMainMenu();
		return true;
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x00152A14 File Offset: 0x00150C14
	private void ReturnToMainMenu()
	{
		if (this.openAfterGameComplete)
		{
			this.openAfterGameComplete = false;
			this.Close();
			if (this.shouldGoToMenuOnReturn)
			{
				this.shouldGoToMenuOnReturn = false;
				MainGame.Instance.GoToMenu(null, true, FadeFlag.All);
			}
			return;
		}
		MainGame.Instance.SetMainMenuInfoPanelEnabled(true);
		LazyUI.GetWindow<UIMainMenuWindow>().Open(null);
		this.Close();
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x00152A6F File Offset: 0x00150C6F
	protected override void PrintTips()
	{
		this.lazyButtonTips.Clear();
	}

	// Token: 0x040037A6 RID: 14246
	private const int EndGameSortingOrder = 30000;

	// Token: 0x040037A7 RID: 14247
	private static ValueTuple<string, string>[] groupTeams = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Game by", "Lazy Bear Games"),
		new ValueTuple<string, string>("Published by", "tinyBuild")
	};

	// Token: 0x040037A8 RID: 14248
	private static string lazyBearTeamString = "Lazy Bear Games team:";

	// Token: 0x040037A9 RID: 14249
	private static ValueTuple<string, string>[] groupLazyBear = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Nikita Kulaga", "Creative Director\nScriptwriter"),
		new ValueTuple<string, string>("Slava Cherkasov", "Technical Director"),
		new ValueTuple<string, string>("Bulat Zaripov", "Producer\nLead Game Designer"),
		new ValueTuple<string, string>("Roman Belikov", "Senior Game Designer"),
		new ValueTuple<string, string>("Evgenii Emelianov", "Senior Game Designer\nSound Designer"),
		new ValueTuple<string, string>("Aleksander Mukha", "Junior Game Designer"),
		new ValueTuple<string, string>("", ""),
		new ValueTuple<string, string>("Artur Akhmadeev", "Technical Lead"),
		new ValueTuple<string, string>("Ilia Filippov", "Senior Programmer\nConsole Programmer"),
		new ValueTuple<string, string>("Vladislav Kornienko", "Senior Programmer"),
		new ValueTuple<string, string>("Vladislav Fomenko", "Senior Technical Game Designer"),
		new ValueTuple<string, string>("Andrei Skriabin", "Technical Game Designer\nProgrammer"),
		new ValueTuple<string, string>("", ""),
		new ValueTuple<string, string>("Pavel Malakhov", "Lead Artist"),
		new ValueTuple<string, string>("Aleksandr Minichev", "UI Lead\nArt Style Heritage Holder\n3D Modeler"),
		new ValueTuple<string, string>("Aleksei Zaikin", "Animation Lead"),
		new ValueTuple<string, string>("Aleksei Nikolaev", "Artist"),
		new ValueTuple<string, string>("Daniil Yamsya", "Artist"),
		new ValueTuple<string, string>("Vadim Minnebaev", "Artist\n3D Modeler"),
		new ValueTuple<string, string>("Timur Bulatov", "Artist\n3D Modeler"),
		new ValueTuple<string, string>("Dmitry Vetrov", "Artist\n3D Modeler"),
		new ValueTuple<string, string>("Ekaterina Gorbunova", "Artist\nAnimator"),
		new ValueTuple<string, string>("", ""),
		new ValueTuple<string, string>("Justas Gabrusenas", "Junior QA"),
		new ValueTuple<string, string>("Stanislav Rozhdestvenskiy", "Senior QA"),
		new ValueTuple<string, string>("", ""),
		new ValueTuple<string, string>("Alexey Nechaev", "Sound Designer"),
		new ValueTuple<string, string>("Igor Chernyshev", "Sound Designer"),
		new ValueTuple<string, string>("Hamza El Hamri", "Music Composer"),
		new ValueTuple<string, string>("Lilija Kulaga", "Financial Director"),
		new ValueTuple<string, string>("", "")
	};

	// Token: 0x040037AA RID: 14250
	private static string translationTeamString = "Localization:";

	// Token: 0x040037AB RID: 14251
	private static ValueTuple<string, string>[] groupTranslation = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Steve Breslin", "English Proofreading"),
		new ValueTuple<string, string>("Thomas Faust", "German Localization"),
		new ValueTuple<string, string>("Words of Magic", "French Localization"),
		new ValueTuple<string, string>("Letícia Araujo", "Brazilian Portuguese Localization"),
		new ValueTuple<string, string>("Ramón Méndez", "Spanish Localization"),
		new ValueTuple<string, string>("Alba Calvo", "Spanish Localization"),
		new ValueTuple<string, string>("Ainhoa García", "Spanish Localization"),
		new ValueTuple<string, string>("Javier Llópiz", "Spanish Localization"),
		new ValueTuple<string, string>("", "")
	};

	// Token: 0x040037AC RID: 14252
	private static string translationTeamAkebonoString = "Akebono Translation Service:";

	// Token: 0x040037AD RID: 14253
	private static ValueTuple<string, string>[] groupTranslationAkebono = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Loek van Kooten", "CJK Localization Project Management"),
		new ValueTuple<string, string>("Niu Pengfei", "Chinese Translation"),
		new ValueTuple<string, string>("Aya Pickard", "Japanese Translation"),
		new ValueTuple<string, string>("Rumi Tasaki", "Japanese Translation"),
		new ValueTuple<string, string>("Cindy Kim", "Korean Translation"),
		new ValueTuple<string, string>("Lois Yang", "Korean Translation"),
		new ValueTuple<string, string>("", "")
	};

	// Token: 0x040037AE RID: 14254
	private static string fromTheVoidString = "From the Void:";

	// Token: 0x040037AF RID: 14255
	private static ValueTuple<string, string>[] groupFromTheVoid = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Marc Eybert-Guillon", "Director & Project Manager"),
		new ValueTuple<string, string>("Alicja Kaniecka", "Polish Localization"),
		new ValueTuple<string, string>("Agnieszka Chęcińska", "Polish Localization"),
		new ValueTuple<string, string>("Wojciech Brudziński", "Polish Localization"),
		new ValueTuple<string, string>("Natalia Paterek", "Polish Localization"),
		new ValueTuple<string, string>("Ebru Yılmaz Akca", "Turkish Localization"),
		new ValueTuple<string, string>("Nazaret Poyraz", "Turkish Localization"),
		new ValueTuple<string, string>("Nehir Durmuşoğlu", "Turkish Localization"),
		new ValueTuple<string, string>("Onur Küçük", "Turkish Localization"),
		new ValueTuple<string, string>("Dmitry Kornyukhov", "Russian Localization"),
		new ValueTuple<string, string>("Arty Ra", "Russian Localization"),
		new ValueTuple<string, string>("Indy", "Team Mascot"),
		new ValueTuple<string, string>("", "")
	};

	// Token: 0x040037B0 RID: 14256
	private static string omukString = "Cast and Recorded at OMUK:";

	// Token: 0x040037B1 RID: 14257
	private static ValueTuple<string, string>[] groupOmuk = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Thomas Mitchells", "Voice Director"),
		new ValueTuple<string, string>("Freda D'Souza", "Casting Director"),
		new ValueTuple<string, string>("Mia Coffield", "Casting Assistant"),
		new ValueTuple<string, string>("Lukas Jakubenas", "Dialogue Recording"),
		new ValueTuple<string, string>("James Hazel", "Dialogue Recording"),
		new ValueTuple<string, string>("Josh Hayward", "Dialogue Recording"),
		new ValueTuple<string, string>("Lukas Jakubenas", "Dialogue Editing"),
		new ValueTuple<string, string>("James Hazel", "Dialogue Editing"),
		new ValueTuple<string, string>("Tom Murton", "Dialogue Editing"),
		new ValueTuple<string, string>("Joel Douglas", "Dialogue Editing"),
		new ValueTuple<string, string>("Marcy Jensen", "Dialogue Editing"),
		new ValueTuple<string, string>("Tabby Griffiths", "Dialogue Editing"),
		new ValueTuple<string, string>("Joshua Hayward", "Dialogue Editing"),
		new ValueTuple<string, string>("Josh Hayward", "Dialogue Mastering"),
		new ValueTuple<string, string>("Josh Hayward", "Audio Manager"),
		new ValueTuple<string, string>("Freda D'Souza", "Production Manager"),
		new ValueTuple<string, string>("Mia Coffield", "Production Assistant"),
		new ValueTuple<string, string>("Creative Dialogue Tools", "Dialogue Production tech"),
		new ValueTuple<string, string>("", "")
	};

	// Token: 0x040037B2 RID: 14258
	private static string voiceOverString = "Voiceover:";

	// Token: 0x040037B3 RID: 14259
	private static ValueTuple<string, string>[] groupVoiceOver = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Adam Longworth", "Workshop Foreman, Jack"),
		new ValueTuple<string, string>("Alex Jordan", "Looters leader, Davy Dagger\nScout_2"),
		new ValueTuple<string, string>("Anna Cass", "Goddess of Nature"),
		new ValueTuple<string, string>("Beth Robb Adams", "Port Club owner, Linda"),
		new ValueTuple<string, string>("Billie Fulford Brown", "Nun, Aghata"),
		new ValueTuple<string, string>("Chris Tester", "Head of the Guards, Herbert\nTrademaster\nMonk\nSoldier_1\nCarpenter_1\nVilage trader, Herm\nHomobonus"),
		new ValueTuple<string, string>("Joseph Capp", "Old God\nBlack screen\nZombie\nBrother Bishop\nAstrologer, Gunter"),
		new ValueTuple<string, string>("Lisa Graydon", "Fairy, Soul\nKeepers Wife"),
		new ValueTuple<string, string>("Matthew Biddulph", "Plague Doctor, Albert\nMirror\nWoodcarver"),
		new ValueTuple<string, string>("Neil Roberts", "Tavern owner, Januarius\nHead of the Vilage, Jully"),
		new ValueTuple<string, string>("Peter Warnock", "Village guard_2\nRefugee_2\nSome Guy"),
		new ValueTuple<string, string>("Phil Rowe", "Comrad Donkey\nMerc Leader"),
		new ValueTuple<string, string>("Shaun Mendum", "Heffry the Twin\nJeffry the Twin\nVillage guard_1"),
		new ValueTuple<string, string>("Shogo Miyakita", "Bandit, Sven\nSoldier_2\nCarpenter_2"),
		new ValueTuple<string, string>("Stu McLoughlin", "Larry\nGarry\nCaptain\nScout_1"),
		new ValueTuple<string, string>("Barry McStay", "Refugee_1"),
		new ValueTuple<string, string>("Tobias Weatherburn", "Dig\nBandit, Hans\nFisherman\nMain Hero (The Keeper)\nAlter Keeper\nNobel Knight"),
		new ValueTuple<string, string>("", "")
	};

	// Token: 0x040037B4 RID: 14260
	private static string tinyBuildTeamString = "tinyBuild Publishing:";

	// Token: 0x040037B5 RID: 14261
	private static ValueTuple<string, string>[] groupTinyBuild = new ValueTuple<string, string>[]
	{
		new ValueTuple<string, string>("Alex Nichiporchik", "CEO"),
		new ValueTuple<string, string>("Giasone Salati", "Chief Financial Officer"),
		new ValueTuple<string, string>("Annette Patent", "Executive Assistant"),
		new ValueTuple<string, string>("Corey Caplan", "Director of Business Development and Partner Licensing"),
		new ValueTuple<string, string>("Carla Woo", "Director, Contract Management"),
		new ValueTuple<string, string>("Michael Kuzmin", "Sales Director"),
		new ValueTuple<string, string>("Artem Bochkarev", "Head of Publishing"),
		new ValueTuple<string, string>("Mike Rafiienko", "Executive Producer"),
		new ValueTuple<string, string>("Anton Pavlov", "Executive Producer"),
		new ValueTuple<string, string>("Dmitry Yashanov", "Executive Producer"),
		new ValueTuple<string, string>("Vladimir Tolmachev", "Producer"),
		new ValueTuple<string, string>("Aleksei Glebov", "Producer"),
		new ValueTuple<string, string>("Artyom Safarov", "Producer"),
		new ValueTuple<string, string>("Nadya Zhuk", "Associate Producer"),
		new ValueTuple<string, string>("Anton Daty", "Senior Marketing Manager"),
		new ValueTuple<string, string>("Sergey Smirnov", "Marketing Manager"),
		new ValueTuple<string, string>("Arnaud Richard", "Marketing Manager"),
		new ValueTuple<string, string>("Koen Rebel", "Head of Influencer Management"),
		new ValueTuple<string, string>("Vera Lubbers", "Senior Influencer Manager"),
		new ValueTuple<string, string>("George Kulko", "Senior Community Manager"),
		new ValueTuple<string, string>("James Croucher", "Senior Community Manager"),
		new ValueTuple<string, string>("Tina Benoit", "Community Manager"),
		new ValueTuple<string, string>("Shunise Wise", "Social Media Marketing Manager"),
		new ValueTuple<string, string>("Francesca Falcini", "Social Media Marketing Manager"),
		new ValueTuple<string, string>("Tori Gerbeshi", "Social Media Marketing Manager"),
		new ValueTuple<string, string>("Anna Bienek", "Social Media Marketing Manager"),
		new ValueTuple<string, string>("Artem Peganov", "User Acquisition Manager"),
		new ValueTuple<string, string>("Aleksandra Akimova", "Porting Producer"),
		new ValueTuple<string, string>("Alex Leoveanu", "LiveOps Producer"),
		new ValueTuple<string, string>("Artem Chernyshev", "Head of Development Services"),
		new ValueTuple<string, string>("Artem Pozhilenkov", "Release Manager"),
		new ValueTuple<string, string>("Bradley Manning", "Release Manager"),
		new ValueTuple<string, string>("Lisa Sidorova", "Localization Manager"),
		new ValueTuple<string, string>("Alina Aliabieva", "Localization Manager"),
		new ValueTuple<string, string>("Jacky Motta", "Discord Manager"),
		new ValueTuple<string, string>("Kimon Sklavounos", "Customer Support Manager"),
		new ValueTuple<string, string>("Oleksandr Striuk", "Customer Support Manager"),
		new ValueTuple<string, string>("Tiberiu Cristea", "QA Manager"),
		new ValueTuple<string, string>("George Popa", "QA Manager"),
		new ValueTuple<string, string>("Igor Surov", "Head of Media Production"),
		new ValueTuple<string, string>("Christina Osipova", "Media Project Manager"),
		new ValueTuple<string, string>("Nikita Varnakov", "Trailer Producer"),
		new ValueTuple<string, string>("Alexander Isaev", "Trailer Producer"),
		new ValueTuple<string, string>("Eugene Chataev", "Video Editor"),
		new ValueTuple<string, string>("Tatiana Kurguzova", "Graphic Designer"),
		new ValueTuple<string, string>("Olga Barlet", "Head of HR"),
		new ValueTuple<string, string>("Valerii Zotov", "System Administrator\nDevOps Engineer")
	};

	// Token: 0x040037B6 RID: 14262
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x040037B7 RID: 14263
	[SerializeField]
	private UICreditsWindowElement prefab;

	// Token: 0x040037B8 RID: 14264
	[SerializeField]
	private GameObject spacePrefab;

	// Token: 0x040037B9 RID: 14265
	[SerializeField]
	private RectTransform content;

	// Token: 0x040037BA RID: 14266
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x040037BB RID: 14267
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x040037BC RID: 14268
	[SerializeField]
	private Image backgroundImage;

	// Token: 0x040037BD RID: 14269
	[SerializeField]
	private float scrollSpeed = 12f;

	// Token: 0x040037BE RID: 14270
	[SerializeField]
	private float fadeTime = 0.5f;

	// Token: 0x040037BF RID: 14271
	private float tweenTime = 165f;

	// Token: 0x040037C0 RID: 14272
	private Tween tween;

	// Token: 0x040037C1 RID: 14273
	private UIDialogWindowData.ButtonData btnData;

	// Token: 0x040037C2 RID: 14274
	private bool isContentCreated;

	// Token: 0x040037C3 RID: 14275
	private bool shouldGoToMenuOnReturn;

	// Token: 0x040037C4 RID: 14276
	private bool openAfterGameComplete;

	// Token: 0x040037C5 RID: 14277
	private bool creditsCameraEnabled;

	// Token: 0x040037C6 RID: 14278
	private bool isSortingOrderCached;

	// Token: 0x040037C7 RID: 14279
	private bool cachedOverrideSorting;

	// Token: 0x040037C8 RID: 14280
	private bool isBackButtonShown;

	// Token: 0x040037C9 RID: 14281
	private int cachedSortingOrder;

	// Token: 0x040037CA RID: 14282
	private bool isScrollRectInsetsCached;

	// Token: 0x040037CB RID: 14283
	private bool endGameScrollRectInsetsApplied;

	// Token: 0x040037CC RID: 14284
	private Vector2 cachedScrollOffsetMin;

	// Token: 0x040037CD RID: 14285
	private Vector2 cachedScrollOffsetMax;

	// Token: 0x040037CE RID: 14286
	private RectOffset cachedContentPadding;

	// Token: 0x040037CF RID: 14287
	private RectTransform firstElementRect;
}
