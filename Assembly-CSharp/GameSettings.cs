using System;
using System.Collections.Generic;
using LazyBearTechnology;
using Steamworks;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000469 RID: 1129
public class GameSettings : ISerializableData
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x06001DBA RID: 7610 RVA: 0x0008B9C8 File Offset: 0x00089BC8
	// (remove) Token: 0x06001DBB RID: 7611 RVA: 0x0008B9FC File Offset: 0x00089BFC
	public static event Action<IntVector2> OnResolutionChanged;

	// Token: 0x1400004A RID: 74
	// (add) Token: 0x06001DBC RID: 7612 RVA: 0x0008BA30 File Offset: 0x00089C30
	// (remove) Token: 0x06001DBD RID: 7613 RVA: 0x0008BA64 File Offset: 0x00089C64
	public static event Action OnScreenSettingsApplied;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06001DBE RID: 7614 RVA: 0x0008BA98 File Offset: 0x00089C98
	// (remove) Token: 0x06001DBF RID: 7615 RVA: 0x0008BACC File Offset: 0x00089CCC
	public static event Action OnLanguageChanged;

	// Token: 0x1700050E RID: 1294
	// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x0008BAFF File Offset: 0x00089CFF
	public static GameSettings Instance
	{
		get
		{
			if (GameSettings.instance == null)
			{
				GameSettings.instance = GameSettings.LoadAndApplyPlatformDefaults();
				GameSettings.ApplyPlatformSpecificRenderSettings();
			}
			return GameSettings.instance;
		}
	}

	// Token: 0x1700050F RID: 1295
	// (get) Token: 0x06001DC1 RID: 7617 RVA: 0x0008BB1C File Offset: 0x00089D1C
	public bool GraphicSettingsAppliedThisFrame
	{
		get
		{
			return this.graphicSettingsAppliedFrame == Time.frameCount;
		}
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x00002318 File Offset: 0x00000518
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x00002318 File Offset: 0x00000518
	public void OnAfterSerialize()
	{
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x0008BB2B File Offset: 0x00089D2B
	public void ApplySettings()
	{
		this.ApplySteamDeckDefaultGraphicsIfNeeded();
		this.ApplyGraphicSettings(false, false);
		this.ApplyGraphicsTier(false);
		this.ApplyLanguageSettings(false);
		this.ApplyAudioSettings();
		this.TryInitDefaultBindings();
		this.ApplySavedGameBindings();
		SaveSystem.SaveGameSettings();
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x0008BB60 File Offset: 0x00089D60
	public void ApplyAudioSettings()
	{
		if (LazyAudio.IsInitialized)
		{
			LazyAudio.SetChannelVolume("master", this.masterVolume / 100f);
			LazyAudio.SetChannelVolume("music", this.musicVolume / 100f);
			LazyAudio.SetChannelVolume("sfx", this.sfxVolume / 100f);
			LazyAudio.SetChannelVolume("speech", this.speechVolume / 100f);
			SaveSystem.SaveGameSettings();
		}
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x0008BBD1 File Offset: 0x00089DD1
	public void ApplyGraphicSettings(bool applySave = true, bool applyEditorGameView = true)
	{
		if (this.GraphicSettingsAppliedThisFrame)
		{
			return;
		}
		this.graphicSettingsAppliedFrame = Time.frameCount;
		this.ApplyResolutionSettings(false, applyEditorGameView);
		this.ApplyScreenSettings();
		if (applySave)
		{
			SaveSystem.SaveGameSettings();
		}
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x0008BBFD File Offset: 0x00089DFD
	public void ApplyGraphicsTier(bool applySave = true)
	{
		PlatformFeatures.ReapplyAll();
		if (applySave)
		{
			SaveSystem.SaveGameSettings();
		}
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x0008BC0C File Offset: 0x00089E0C
	public void ApplyResolutionSettings(bool applySave = false, bool applyEditorGameView = true)
	{
		if (this.resolutionConfig == null)
		{
			this.resolutionConfig = ResolutionConfig.GetOptimalResolution();
		}
		if (!this.resolutionConfig.IsValid)
		{
			this.resolutionConfig = ResolutionConfig.GetOptimalResolution();
		}
		ResolutionConfig.SetResolution(this.resolutionConfig);
		if (ResolutionConfig.currentResolution != null)
		{
			this.resolutionConfig = ResolutionConfig.currentResolution.Copy();
		}
		if (applySave)
		{
			SaveSystem.SaveGameSettings();
		}
		ResolutionConfig.LogCurrentResolutionConfig();
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x0008BC74 File Offset: 0x00089E74
	public void NotifyResolutionChanged()
	{
		Action<IntVector2> onResolutionChanged = GameSettings.OnResolutionChanged;
		if (onResolutionChanged == null)
		{
			return;
		}
		IntVector2 resolutionIntVector = this.GetResolutionIntVector2();
		Delegate[] invocationList = onResolutionChanged.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			try
			{
				((Action<IntVector2>)invocationList[i])(resolutionIntVector);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
		}
	}

	// Token: 0x06001DCA RID: 7626 RVA: 0x0008BCD0 File Offset: 0x00089ED0
	public void ApplyScreenSettings()
	{
		if (ResolutionConfig.currentResolution == null)
		{
			Debug.Log("currentResolution is null");
			return;
		}
		ScreenMode screenMode = this.screenMode;
		FullScreenMode fullScreenMode;
		if (screenMode != ScreenMode.FullScreen)
		{
			if (screenMode != ScreenMode.Windowed)
			{
				string text = "Unsupported screen mode: ";
				ScreenMode screenMode2 = this.screenMode;
				throw new Exception(text + screenMode2.ToString());
			}
			fullScreenMode = FullScreenMode.Windowed;
		}
		else
		{
			fullScreenMode = FullScreenMode.FullScreenWindow;
		}
		int unityTargetFrameRate = this.GetUnityTargetFrameRate();
		int monitorRefreshRateHz = GameSettings.GetMonitorRefreshRateHz();
		bool flag = this.vsyncMode == VSyncMode.Enabled && !GameSettings.IsTargetFrameRateBelowMonitorRefreshRate(unityTargetFrameRate, monitorRefreshRateHz);
		Debug.Log(string.Format("ApplyScreenMode Resolution: {0}x{1} PixelSize: ", ResolutionConfig.currentResolution.AppliedWidth, ResolutionConfig.currentResolution.AppliedHeight) + string.Format("{0}, FullScreenMode: {1}, VSync: {2} (effective {3}), TargetFPS: {4}, MonitorHz: {5}", new object[]
		{
			ResolutionConfig.PixelSize,
			fullScreenMode,
			this.vsyncMode > VSyncMode.Disabled,
			flag,
			unityTargetFrameRate,
			monitorRefreshRateHz
		}));
		Screen.SetResolution(ResolutionConfig.currentResolution.AppliedWidth, ResolutionConfig.currentResolution.AppliedHeight, fullScreenMode, new RefreshRate
		{
			numerator = 0U
		});
		VSyncMode vsyncMode = this.vsyncMode;
		if (vsyncMode <= VSyncMode.Enabled)
		{
			QualitySettings.vSyncCount = (flag ? 1 : 0);
			Application.targetFrameRate = unityTargetFrameRate;
		}
		else
		{
			Debug.LogError(string.Format("Unsupported vsync mode = {0}", this.vsyncMode));
		}
		this.NotifyResolutionChanged();
		Action onScreenSettingsApplied = GameSettings.OnScreenSettingsApplied;
		if (onScreenSettingsApplied == null)
		{
			return;
		}
		onScreenSettingsApplied();
	}

	// Token: 0x06001DCB RID: 7627 RVA: 0x0008BE54 File Offset: 0x0008A054
	public bool SyncScreenModeFromHardware(bool applySave = true)
	{
		ScreenMode screenMode;
		if (!GameSettings.TryGetScreenModeFromFullScreenMode(Screen.fullScreenMode, out screenMode))
		{
			return false;
		}
		if (this.screenMode == screenMode)
		{
			return false;
		}
		this.screenMode = screenMode;
		if (applySave)
		{
			SaveSystem.SaveGameSettings();
		}
		return true;
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x0008BE8C File Offset: 0x0008A08C
	private static bool TryGetScreenModeFromFullScreenMode(FullScreenMode fullScreenMode, out ScreenMode screenMode)
	{
		if (fullScreenMode <= FullScreenMode.FullScreenWindow)
		{
			screenMode = ScreenMode.FullScreen;
			return true;
		}
		if (fullScreenMode - FullScreenMode.MaximizedWindow > 1)
		{
			screenMode = ScreenMode.Windowed;
			return false;
		}
		screenMode = ScreenMode.Windowed;
		return true;
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x0008BEA8 File Offset: 0x0008A0A8
	public void ApplyLanguageSettings(bool applySave = true)
	{
		if (string.IsNullOrEmpty(this.language))
		{
			this.language = LL.GetCurrentLocaleCode();
		}
		LLBase.LoadLanguageResource(this.language);
		if (applySave)
		{
			SaveSystem.SaveGameSettings();
		}
		VoiceOverModLoader.Refresh(this.language);
		VoiceOverSettings.LanguageId = (VoiceOverModLoader.IsActive ? VoiceOverModLoader.ActiveLanguage : "en");
		Action onLanguageChanged = GameSettings.OnLanguageChanged;
		if (onLanguageChanged == null)
		{
			return;
		}
		onLanguageChanged();
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x0008BF14 File Offset: 0x0008A114
	public void ApplyDefaultGameBindings()
	{
		foreach (GameSettings.KeyBindingForSave keyBindingForSave in this.defaultKeyboardKeybindings)
		{
			foreach (KeyBinding keyBinding in LazyInput.GameBindings.keyBindings)
			{
				if (keyBinding.gameKey.value == keyBindingForSave.gameKeyValue)
				{
					keyBinding.keyCode = keyBindingForSave.keyCode;
					break;
				}
			}
		}
		ControllerIconLibrary.UpdateStandaloneIcons();
		this.SaveCurrentGameBindings();
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x0008BFCC File Offset: 0x0008A1CC
	public void SaveCurrentGameBindings()
	{
		if (!LazyInput.IsInitialized)
		{
			return;
		}
		this.FillKeyBindingsForSave(this.keyboardKeybindings);
		SaveSystem.SaveGameSettings();
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x0008BFE7 File Offset: 0x0008A1E7
	public bool IsDlcStartupPopUpShown(DLCVersion dlcVersion)
	{
		this.EnsureShownDlcStartupPopUpsInitialized();
		return this.shownDlcStartupPopUps.Contains((int)dlcVersion);
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x0008BFFC File Offset: 0x0008A1FC
	public void MarkDlcStartupPopUpShown(DLCVersion dlcVersion)
	{
		this.EnsureShownDlcStartupPopUpsInitialized();
		if (this.shownDlcStartupPopUps.Contains((int)dlcVersion))
		{
			return;
		}
		this.shownDlcStartupPopUps.Add((int)dlcVersion);
		SaveSystem.SaveGameSettings();
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x0008C031 File Offset: 0x0008A231
	private void EnsureShownDlcStartupPopUpsInitialized()
	{
		if (this.shownDlcStartupPopUps == null)
		{
			this.shownDlcStartupPopUps = new List<int>();
		}
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x0008C048 File Offset: 0x0008A248
	private void ApplySavedGameBindings()
	{
		if (!LazyInput.IsInitialized || this.savedGameBindingsApplied)
		{
			return;
		}
		if (this.keyboardKeybindings.Count == 0)
		{
			this.FillKeyBindingsForSave(this.keyboardKeybindings);
			this.savedGameBindingsApplied = true;
			return;
		}
		foreach (GameSettings.KeyBindingForSave keyBindingForSave in this.keyboardKeybindings)
		{
			foreach (KeyBinding keyBinding in LazyInput.GameBindings.keyBindings)
			{
				if (keyBinding.gameKey.value == keyBindingForSave.gameKeyValue)
				{
					keyBinding.keyCode = keyBindingForSave.keyCode;
					break;
				}
			}
		}
		if (this.keyboardKeybindings.Count != LazyInput.GameBindings.keyBindings.Count)
		{
			this.FillKeyBindingsForSave(this.keyboardKeybindings);
		}
		ControllerIconLibrary.UpdateStandaloneIcons();
		this.savedGameBindingsApplied = true;
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x0008C15C File Offset: 0x0008A35C
	private void TryInitDefaultBindings()
	{
		if (!LazyInput.IsInitialized || this.savedGameBindingsApplied)
		{
			return;
		}
		if (this.defaultKeyboardKeybindings.Count == 0)
		{
			this.FillKeyBindingsForSave(this.defaultKeyboardKeybindings);
			return;
		}
		if (this.IsSameBindingsAsCurrentSource(this.defaultKeyboardKeybindings))
		{
			return;
		}
		this.MigrateSavedBindingsToCurrentDefaults();
		this.FillKeyBindingsForSave(this.defaultKeyboardKeybindings);
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x0008C1B4 File Offset: 0x0008A3B4
	private void MigrateSavedBindingsToCurrentDefaults()
	{
		List<GameSettings.KeyBindingForSave> list = new List<GameSettings.KeyBindingForSave>();
		foreach (KeyBinding keyBinding in LazyInput.GameBindings.keyBindings)
		{
			GameSettings.KeyBindingForSave keyBindingForSave = this.FindBindingForSave(this.keyboardKeybindings, keyBinding.gameKey.value);
			GameSettings.KeyBindingForSave keyBindingForSave2 = this.FindBindingForSave(this.defaultKeyboardKeybindings, keyBinding.gameKey.value);
			bool flag = keyBindingForSave != null && keyBindingForSave2 != null && keyBindingForSave.keyCode != keyBindingForSave2.keyCode;
			list.Add(new GameSettings.KeyBindingForSave
			{
				gameKeyValue = keyBinding.gameKey.value,
				keyCode = (flag ? keyBindingForSave.keyCode : keyBinding.keyCode)
			});
		}
		this.keyboardKeybindings = list;
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x0008C2A0 File Offset: 0x0008A4A0
	private bool IsSameBindingsAsCurrentSource(List<GameSettings.KeyBindingForSave> keyBindingsForSave)
	{
		if (keyBindingsForSave.Count != LazyInput.GameBindings.keyBindings.Count)
		{
			return false;
		}
		foreach (KeyBinding keyBinding in LazyInput.GameBindings.keyBindings)
		{
			GameSettings.KeyBindingForSave keyBindingForSave = this.FindBindingForSave(keyBindingsForSave, keyBinding.gameKey.value);
			if (keyBindingForSave == null || keyBindingForSave.keyCode != keyBinding.keyCode)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x0008C334 File Offset: 0x0008A534
	private GameSettings.KeyBindingForSave FindBindingForSave(List<GameSettings.KeyBindingForSave> keyBindingsForSave, int gameKeyValue)
	{
		foreach (GameSettings.KeyBindingForSave keyBindingForSave in keyBindingsForSave)
		{
			if (keyBindingForSave.gameKeyValue == gameKeyValue)
			{
				return keyBindingForSave;
			}
		}
		return null;
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x0008C38C File Offset: 0x0008A58C
	private void FillKeyBindingsForSave(List<GameSettings.KeyBindingForSave> keyBindingsForSave)
	{
		keyBindingsForSave.Clear();
		foreach (KeyBinding keyBinding in LazyInput.GameBindings.keyBindings)
		{
			keyBindingsForSave.Add(new GameSettings.KeyBindingForSave
			{
				gameKeyValue = keyBinding.gameKey.value,
				keyCode = keyBinding.keyCode
			});
		}
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x0008C40C File Offset: 0x0008A60C
	private static GameSettings LoadAndApplyPlatformDefaults()
	{
		GameSettings gameSettings = SaveSystem.LoadGameSettings();
		gameSettings.ApplyGpuDetectedDefaultGraphicsIfNeeded();
		gameSettings.ApplySteamDeckDefaultGraphicsIfNeeded();
		return gameSettings;
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x0008C420 File Offset: 0x0008A620
	private void ApplyGpuDetectedDefaultGraphicsIfNeeded()
	{
		if (this.gpuGraphicsDefaultApplied)
		{
			Debug.Log(string.Format("[GameSettings] Graphics tier already initialized (tier {0}); skipping GPU-based default detection.", this.graphicsTier));
			return;
		}
		this.gpuGraphicsDefaultApplied = true;
		if (GameSettings.IsRunningOnSteamDeck())
		{
			Debug.Log("[GameSettings] Running on Steam Deck; skipping GPU-based default detection in favor of Steam Deck defaults.");
			return;
		}
		global::GraphicsTier graphicsTier = this.graphicsTier;
		this.graphicsTier = GpuGraphicsTierDetector.DetectDefaultGraphicsTier();
		Debug.Log(string.Format("[GameSettings] No graphics tier stored yet: default {0} -> {1} (GPU-based).", graphicsTier, this.graphicsTier));
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x0008C49B File Offset: 0x0008A69B
	private void ApplySteamDeckDefaultGraphicsIfNeeded()
	{
		if (this.steamDeckGraphicsDefaultApplied || !GameSettings.IsRunningOnSteamDeck())
		{
			return;
		}
		this.steamDeckGraphicsDefaultApplied = true;
		if (this.graphicsTier == global::GraphicsTier.High)
		{
			this.graphicsTier = global::GraphicsTier.Medium;
		}
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x0008C4C3 File Offset: 0x0008A6C3
	private static bool IsRunningOnSteamDeck()
	{
		return SteamManager.Initialized && SteamUtils.IsSteamRunningOnSteamDeck();
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x0008C4D3 File Offset: 0x0008A6D3
	private static void ApplyPlatformSpecificRenderSettings()
	{
		RenderSettings.ambientMode = AmbientMode.Flat;
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x0008C4DB File Offset: 0x0008A6DB
	public static bool IsHBAOEnabled()
	{
		return PlatformFeatures.IsHBAOEnabled();
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x0008C4E4 File Offset: 0x0008A6E4
	public int GetUnityTargetFrameRate()
	{
		int num;
		switch (this.targetFrameRate)
		{
		case TargetFrameRate.Fps30:
			num = 30;
			break;
		case TargetFrameRate.Fps60:
			num = 60;
			break;
		case TargetFrameRate.Fps120:
			num = 120;
			break;
		case TargetFrameRate.Unlimited:
			num = -1;
			break;
		default:
			num = 60;
			break;
		}
		return num;
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x0008C528 File Offset: 0x0008A728
	private static int GetMonitorRefreshRateHz()
	{
		double value = Screen.currentResolution.refreshRateRatio.value;
		if (value <= 0.0)
		{
			return 0;
		}
		return Mathf.RoundToInt((float)value);
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x0008C560 File Offset: 0x0008A760
	private static bool IsTargetFrameRateBelowMonitorRefreshRate(int unityTargetFrameRate, int monitorRefreshRateHz)
	{
		return unityTargetFrameRate > 0 && monitorRefreshRateHz > 0 && unityTargetFrameRate < monitorRefreshRateHz;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x0008C570 File Offset: 0x0008A770
	public IntVector2 GetResolutionIntVector2()
	{
		if (this.resolutionConfig != null)
		{
			return new IntVector2(this.resolutionConfig.AppliedWidth, this.resolutionConfig.AppliedHeight);
		}
		if (ResolutionConfig.currentResolution != null)
		{
			return new IntVector2(ResolutionConfig.currentResolution.AppliedWidth, ResolutionConfig.currentResolution.AppliedHeight);
		}
		return new IntVector2(Screen.width, Screen.height);
	}

	// Token: 0x04001B53 RID: 6995
	private static GameSettings instance;

	// Token: 0x04001B54 RID: 6996
	public float masterVolume = 100f;

	// Token: 0x04001B55 RID: 6997
	public float musicVolume = 80f;

	// Token: 0x04001B56 RID: 6998
	public float sfxVolume = 80f;

	// Token: 0x04001B57 RID: 6999
	public float speechVolume = 80f;

	// Token: 0x04001B58 RID: 7000
	public string language = "";

	// Token: 0x04001B59 RID: 7001
	public ResolutionConfig resolutionConfig;

	// Token: 0x04001B5A RID: 7002
	public ScreenMode screenMode;

	// Token: 0x04001B5B RID: 7003
	public VSyncMode vsyncMode = VSyncMode.Enabled;

	// Token: 0x04001B5C RID: 7004
	public TargetFrameRate targetFrameRate = TargetFrameRate.Fps60;

	// Token: 0x04001B5D RID: 7005
	public VoiceOverMode voiceOverMode;

	// Token: 0x04001B5E RID: 7006
	public GameCursorMode cursorMode;

	// Token: 0x04001B5F RID: 7007
	public global::GraphicsTier graphicsTier;

	// Token: 0x04001B60 RID: 7008
	public bool gpuGraphicsDefaultApplied;

	// Token: 0x04001B61 RID: 7009
	public bool steamDeckGraphicsDefaultApplied;

	// Token: 0x04001B62 RID: 7010
	public List<int> shownDlcStartupPopUps = new List<int>();

	// Token: 0x04001B63 RID: 7011
	public List<GameSettings.KeyBindingForSave> defaultKeyboardKeybindings = new List<GameSettings.KeyBindingForSave>();

	// Token: 0x04001B64 RID: 7012
	public List<GameSettings.KeyBindingForSave> keyboardKeybindings = new List<GameSettings.KeyBindingForSave>();

	// Token: 0x04001B65 RID: 7013
	[NonSerialized]
	private bool savedGameBindingsApplied;

	// Token: 0x04001B66 RID: 7014
	[NonSerialized]
	private int graphicSettingsAppliedFrame = -1;

	// Token: 0x0200046A RID: 1130
	[Serializable]
	public class KeyBindingForSave
	{
		// Token: 0x04001B67 RID: 7015
		public int gameKeyValue;

		// Token: 0x04001B68 RID: 7016
		public KeyCode keyCode;
	}
}
