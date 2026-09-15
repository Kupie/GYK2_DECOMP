using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A62 RID: 2658
public class UIGameSettingsWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x17000ADC RID: 2780
	// (get) Token: 0x060047E2 RID: 18402 RVA: 0x00155090 File Offset: 0x00153290
	private GameSettings GameSettings
	{
		get
		{
			return GameSettings.Instance;
		}
	}

	// Token: 0x060047E3 RID: 18403 RVA: 0x00155098 File Offset: 0x00153298
	public override void Init()
	{
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
		ResolutionConfig resolutionConfig = this.GameSettings.resolutionConfig;
		if (resolutionConfig == null)
		{
			IntVector2 resolutionIntVector = this.GameSettings.GetResolutionIntVector2();
			resolutionConfig = new ResolutionConfig(resolutionIntVector.x, resolutionIntVector.y);
		}
		string[] resolutionsStringArray = ResolutionConfig.GetResolutionsStringArray();
		this.resolutionSwitch.Initialize(delegate(int index)
		{
			ResolutionConfig resolutionConfigByIndex = ResolutionConfig.GetResolutionConfigByIndex(index);
			this.GameSettings.resolutionConfig = resolutionConfigByIndex;
			this.GameSettings.ApplyGraphicSettings(true, true);
			((RectTransform)base.transform).RefreshContentFitter();
		}, resolutionsStringArray, Mathf.Max(0, ResolutionConfig.FindResolutionConfigIndex(resolutionConfig)), "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		this.fullscreenButton.Initialize(delegate(int value)
		{
			this.GameSettings.screenMode = (ScreenMode)value;
			this.GameSettings.ApplyGraphicSettings(true, true);
		}, this.GetLocalizedArray(this.screenModes), (int)this.GameSettings.screenMode, "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		this.voiceOverButton.Initialize(delegate(int value)
		{
			this.GameSettings.voiceOverMode = (VoiceOverMode)value;
			VoiceOverSettings.IsEnabled = value == 0;
			SaveSystem.SaveGameSettings();
			VoiceOverModePreview.Play((VoiceOverMode)value, this);
		}, this.GetLocalizedArray(this.voiceOverModes), (int)this.GameSettings.voiceOverMode, "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		if (DevUtils.IsDemoBitsummitActive)
		{
			this.voiceOverButton.gameObject.SetActive(false);
		}
		this.vSyncButton.Initialize(delegate(int value)
		{
			this.GameSettings.vsyncMode = (VSyncMode)value;
			this.GameSettings.ApplyGraphicSettings(true, true);
		}, this.GetLocalizedArray(this.vsyncModes), (int)this.GameSettings.vsyncMode, "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		this.fpsLockButton.Initialize(delegate(int value)
		{
			this.GameSettings.targetFrameRate = (TargetFrameRate)value;
			this.GameSettings.ApplyGraphicSettings(true, true);
		}, this.GetTargetFrameRateLabels(), (int)this.GameSettings.targetFrameRate, "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		this.cursorBtn.Initialize(delegate(int value)
		{
			this.GameSettings.cursorMode = (GameCursorMode)value;
			CursorController.UpdateCursorState();
			SaveSystem.SaveGameSettings();
		}, this.GetCursorModeLabels(), (int)this.GameSettings.cursorMode, "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		if (this.graphicsTierButton != null)
		{
			this.graphicsTierButton.Initialize(delegate(int value)
			{
				this.GameSettings.graphicsTier = UIGameSettingsWindow.FromGraphicsTierSwitchIndex(value);
				this.GameSettings.ApplyGraphicsTier(true);
			}, this.GetLocalizedArray(this.graphicsTierModes), UIGameSettingsWindow.ToGraphicsTierSwitchIndex(this.GameSettings.graphicsTier), "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, false);
		}
		this.masterVolumeSlider.Initialize(delegate(float volume)
		{
			this.GameSettings.masterVolume = volume;
			this.GameSettings.ApplyAudioSettings();
		}, this.GameSettings.masterVolume, 5f);
		this.musicVolumeSlider.Initialize(delegate(float volume)
		{
			this.GameSettings.musicVolume = volume;
			this.GameSettings.ApplyAudioSettings();
		}, this.GameSettings.musicVolume, 5f);
		this.sfxVolumeSlider.Initialize(delegate(float volume)
		{
			this.GameSettings.sfxVolume = volume;
			this.GameSettings.ApplyAudioSettings();
		}, this.GameSettings.sfxVolume, 5f);
		this.speechVolumeSlider.Initialize(delegate(float volume)
		{
			this.GameSettings.speechVolume = volume;
			this.GameSettings.ApplyAudioSettings();
		}, this.GameSettings.speechVolume, 5f);
		this.InitLanguageButton();
	}

	// Token: 0x060047E4 RID: 18404 RVA: 0x00155354 File Offset: 0x00153554
	public static void RefreshLanguageSwitcherIfOpen()
	{
		UIGameSettingsWindow[] array = global::UnityEngine.Object.FindObjectsByType<UIGameSettingsWindow>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				array[i].InitLanguageButton();
			}
		}
	}

	// Token: 0x060047E5 RID: 18405 RVA: 0x0015538C File Offset: 0x0015358C
	private void InitLanguageButton()
	{
		if (this.languageLocalizedLabel != null)
		{
			this.languageLocalizedLabel.IgnoreLocalize = true;
		}
		string[] availableLanguageNamesRange = LLBase.GetAvailableLanguageNamesRange();
		this.languageButton.Initialize(delegate(int value)
		{
			this.GameSettings.language = LLBase.GetAvailableLanguageInfoByIndex(value).id;
			this.GameSettings.ApplyLanguageSettings(true);
			GUIElements.Instance.UpdateLocalizedLabels();
			this.fullscreenButton.ReinitLabels(this.GetLocalizedArray(this.screenModes));
			this.vSyncButton.ReinitLabels(this.GetLocalizedArray(this.vsyncModes));
			this.fpsLockButton.ReinitLabels(this.GetTargetFrameRateLabels());
			this.voiceOverButton.ReinitLabels(this.GetLocalizedArray(this.voiceOverModes));
			this.cursorBtn.ReinitLabels(this.GetCursorModeLabels());
			if (this.graphicsTierButton != null)
			{
				this.graphicsTierButton.ReinitLabels(this.GetLocalizedArray(this.graphicsTierModes));
			}
			this.languageButton.ReinitLabels(LLBase.GetAvailableLanguageNamesRange());
			this.languageStyleComponent.ApplyStyle();
			this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
			this.lazyButton.Draw(this.btnData);
			if (MainGame.Instance.gameState == MainGame.GameState.InGame)
			{
				GUIElements.Instance.WorldZoneWidget.Draw(new WorldZoneWidgetData());
			}
			TextStyleComponent[] componentsInChildren = GUIElements.Instance.GetComponentsInChildren<TextStyleComponent>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ApplyStyle();
			}
			this.PrintTips();
		}, availableLanguageNamesRange, string.IsNullOrEmpty(this.GameSettings.language) ? 0 : LLBase.GetIndexByLanguage(this.GameSettings.language), "", UIGameSettingsWindow.decreaseKeys, UIGameSettingsWindow.increaseKeys, true);
		this.languageButton.IsInteractable = availableLanguageNamesRange.Length > 1;
	}

	// Token: 0x060047E6 RID: 18406 RVA: 0x00155418 File Offset: 0x00153618
	private void RefreshScreenModeSwitch(bool syncFromHardware)
	{
		if (syncFromHardware && !this.GameSettings.GraphicSettingsAppliedThisFrame)
		{
			this.GameSettings.SyncScreenModeFromHardware(true);
		}
		int screenMode = (int)this.GameSettings.screenMode;
		if (this.fullscreenButton.CurrentFieldIndex != screenMode)
		{
			this.fullscreenButton.UpdateField(screenMode, false);
		}
	}

	// Token: 0x060047E7 RID: 18407 RVA: 0x00155469 File Offset: 0x00153669
	private void LateUpdate()
	{
		if (base.IsShown)
		{
			this.RefreshScreenModeSwitch(false);
		}
	}

	// Token: 0x060047E8 RID: 18408 RVA: 0x00120036 File Offset: 0x0011E236
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x060047E9 RID: 18409 RVA: 0x0015547C File Offset: 0x0015367C
	protected override void PrintTips()
	{
		if (LazyInput.IsGamepadActive)
		{
			this.lazyButtonTips.Print(new LazyGameKeyTip[]
			{
				LazyGameKeyTip.Back(true, true, true),
				new LazyGameKeyTip(GameKey.DpadLeft, "-", true, true, true),
				new LazyGameKeyTip(GameKey.DpadRight, "+", true, true, true)
			});
			if (this.tipsStyleComponent != null)
			{
				this.tipsStyleComponent.ApplyStyle();
				return;
			}
		}
		else
		{
			this.lazyButtonTips.Clear();
		}
	}

	// Token: 0x060047EA RID: 18410 RVA: 0x001554FB File Offset: 0x001536FB
	protected override bool OnPressedBack()
	{
		this.Close();
		return true;
	}

	// Token: 0x060047EB RID: 18411 RVA: 0x00155504 File Offset: 0x00153704
	protected override Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
	{
		Dictionary<GameKey, Func<bool>> gameKeyDelegates = base.GetGameKeyDelegates();
		gameKeyDelegates.Add(GameKey.Select, () => false);
		gameKeyDelegates.TryAdd(GameKey.DpadLeft, () => false);
		gameKeyDelegates.TryAdd(GameKey.DpadRight, () => false);
		gameKeyDelegates.TryAdd(GameKey.Left, () => false);
		gameKeyDelegates.TryAdd(GameKey.Right, () => false);
		return gameKeyDelegates;
	}

	// Token: 0x060047EC RID: 18412 RVA: 0x001555F0 File Offset: 0x001537F0
	public override void Open(LazyWidgetDataBase data)
	{
		base.Open(data);
		this.InitLanguageButton();
		this.RefreshScreenModeSwitch(true);
		if (SteamManager.Initialized && SteamUtils.IsSteamRunningOnSteamDeck())
		{
			this.fullscreenButton.gameObject.SetActive(false);
			this.vSyncButton.gameObject.SetActive(false);
			this.fpsLockButton.gameObject.SetActive(false);
			this.cursorBtn.gameObject.SetActive(false);
			this.resolutionSwitch.gameObject.SetActive(false);
		}
		this.btnData = new UIDialogWindowData.ButtonData(new Action(this.Close), LLBase.L("btn_ok"), null, true, GameKey.Select, "");
		this.lazyButton.Draw(this.btnData);
		((RectTransform)base.transform).RefreshContentFitter();
		VoiceOverModePreview.Warmup();
		this.PrintTips();
	}

	// Token: 0x060047ED RID: 18413 RVA: 0x001556D0 File Offset: 0x001538D0
	private static int ToGraphicsTierSwitchIndex(GraphicsTier tier)
	{
		int num;
		switch (tier)
		{
		case GraphicsTier.Medium:
			num = 2;
			break;
		case GraphicsTier.Low:
			num = 1;
			break;
		case GraphicsTier.Lowest:
			num = 0;
			break;
		default:
			num = 3;
			break;
		}
		return num;
	}

	// Token: 0x060047EE RID: 18414 RVA: 0x00155704 File Offset: 0x00153904
	private static GraphicsTier FromGraphicsTierSwitchIndex(int index)
	{
		GraphicsTier graphicsTier;
		switch (index)
		{
		case 0:
			graphicsTier = GraphicsTier.Lowest;
			break;
		case 1:
			graphicsTier = GraphicsTier.Low;
			break;
		case 2:
			graphicsTier = GraphicsTier.Medium;
			break;
		default:
			graphicsTier = GraphicsTier.High;
			break;
		}
		return graphicsTier;
	}

	// Token: 0x060047EF RID: 18415 RVA: 0x00155734 File Offset: 0x00153934
	private string[] GetLocalizedArray(string[] array)
	{
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = LLBase.L(array[i]);
		}
		return array2;
	}

	// Token: 0x060047F0 RID: 18416 RVA: 0x00155764 File Offset: 0x00153964
	private string[] GetTargetFrameRateLabels()
	{
		return new string[]
		{
			"30",
			"60",
			"120",
			LLBase.L("ui_fps_unlimited")
		};
	}

	// Token: 0x060047F1 RID: 18417 RVA: 0x00155794 File Offset: 0x00153994
	private string[] GetCursorModeLabels()
	{
		string text = LLBase.L("ui_cursor_software");
		return new string[]
		{
			LLBase.L("ui_cursor_hardware"),
			text ?? "",
			text + " 150%"
		};
	}

	// Token: 0x060047F2 RID: 18418 RVA: 0x001557DA File Offset: 0x001539DA
	public override void Close()
	{
		base.Close();
		Action action = this.onClosed;
		if (action != null)
		{
			action();
		}
		this.onClosed = null;
	}

	// Token: 0x060047F3 RID: 18419 RVA: 0x00002318 File Offset: 0x00000518
	protected override void TestDraw()
	{
	}

	// Token: 0x04003809 RID: 14345
	private static readonly GameKey[] decreaseKeys = new GameKey[]
	{
		GameKey.DecSlider,
		GameKey.Left
	};

	// Token: 0x0400380A RID: 14346
	private static readonly GameKey[] increaseKeys = new GameKey[]
	{
		GameKey.IncSlider,
		GameKey.Right
	};

	// Token: 0x0400380B RID: 14347
	private const string VSYNC_OFF = "ui_off";

	// Token: 0x0400380C RID: 14348
	private const string VSYNC_ON = "ui_on";

	// Token: 0x0400380D RID: 14349
	private const string FPS_UNLIMITED = "ui_fps_unlimited";

	// Token: 0x0400380E RID: 14350
	private const string VOICE_OVER = "ui_voiceover_mode";

	// Token: 0x0400380F RID: 14351
	private const string MUMBLING = "ui_mumbling_mode";

	// Token: 0x04003810 RID: 14352
	private const string FULLSCREEN = "ui_fullscreen";

	// Token: 0x04003811 RID: 14353
	private const string WINDOWED = "ui_windowed";

	// Token: 0x04003812 RID: 14354
	private const string CURSOR_HARDWARE = "ui_cursor_hardware";

	// Token: 0x04003813 RID: 14355
	private const string CURSOR_SOFTWARE = "ui_cursor_software";

	// Token: 0x04003814 RID: 14356
	private const string GRAPHICS_LOWEST = "ui_graphics_lowest";

	// Token: 0x04003815 RID: 14357
	private const string GRAPHICS_LOW = "ui_graphics_low";

	// Token: 0x04003816 RID: 14358
	private const string GRAPHICS_MEDIUM = "ui_graphics_medium";

	// Token: 0x04003817 RID: 14359
	private const string GRAPHICS_HIGH = "ui_graphics_high";

	// Token: 0x04003818 RID: 14360
	private const float VolumeSliderStep = 5f;

	// Token: 0x04003819 RID: 14361
	[SerializeField]
	private UISwitchButton resolutionSwitch;

	// Token: 0x0400381A RID: 14362
	[SerializeField]
	private UISwitchButton fullscreenButton;

	// Token: 0x0400381B RID: 14363
	[SerializeField]
	private UISwitchButton vSyncButton;

	// Token: 0x0400381C RID: 14364
	[SerializeField]
	private UISwitchButton fpsLockButton;

	// Token: 0x0400381D RID: 14365
	[SerializeField]
	private UISwitchButton cursorBtn;

	// Token: 0x0400381E RID: 14366
	[SerializeField]
	private UISwitchButton graphicsTierButton;

	// Token: 0x0400381F RID: 14367
	[SerializeField]
	private UISwitchButton voiceOverButton;

	// Token: 0x04003820 RID: 14368
	[SerializeField]
	private UISwitchButton languageButton;

	// Token: 0x04003821 RID: 14369
	[SerializeField]
	private LocalizedLabel languageLocalizedLabel;

	// Token: 0x04003822 RID: 14370
	[SerializeField]
	private TextStyleComponent languageStyleComponent;

	// Token: 0x04003823 RID: 14371
	[SerializeField]
	private UIDialogWindowButton lazyButton;

	// Token: 0x04003824 RID: 14372
	[SerializeField]
	private UISlider masterVolumeSlider;

	// Token: 0x04003825 RID: 14373
	[SerializeField]
	private UISlider musicVolumeSlider;

	// Token: 0x04003826 RID: 14374
	[SerializeField]
	private UISlider sfxVolumeSlider;

	// Token: 0x04003827 RID: 14375
	[SerializeField]
	private UISlider speechVolumeSlider;

	// Token: 0x04003828 RID: 14376
	[SerializeField]
	private TextStyleComponent tipsStyleComponent;

	// Token: 0x04003829 RID: 14377
	public Action onClosed;

	// Token: 0x0400382A RID: 14378
	private UIDialogWindowData.ButtonData btnData;

	// Token: 0x0400382B RID: 14379
	private string[] screenModes = new string[] { "ui_fullscreen", "ui_windowed" };

	// Token: 0x0400382C RID: 14380
	private string[] vsyncModes = new string[] { "ui_off", "ui_on" };

	// Token: 0x0400382D RID: 14381
	private string[] voiceOverModes = new string[] { "ui_voiceover_mode", "ui_mumbling_mode" };

	// Token: 0x0400382E RID: 14382
	private string[] graphicsTierModes = new string[] { "ui_graphics_lowest", "ui_graphics_low", "ui_graphics_medium", "ui_graphics_high" };
}
