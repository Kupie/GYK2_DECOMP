using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LazyBearTechnology;
using Steamworks;
using UnityEngine;

// Token: 0x0200000B RID: 11
[CreateAssetMenu(fileName = "ControllerIconLibrary", menuName = "Lazy/ControllerIconLibrary", order = 1)]
public class ControllerIconLibrary : LazySingletonSO<ControllerIconLibrary>
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x0600000A RID: 10 RVA: 0x00002128 File Offset: 0x00000328
	// (remove) Token: 0x0600000B RID: 11 RVA: 0x0000215C File Offset: 0x0000035C
	public static event Action<ControllerIconViewType> OnViewChanged;

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600000C RID: 12 RVA: 0x0000218F File Offset: 0x0000038F
	public static bool IsViewForced
	{
		get
		{
			return ControllerIconLibrary.forcedViewActive;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x0600000D RID: 13 RVA: 0x00002196 File Offset: 0x00000396
	public static ControllerIconViewType ForcedView
	{
		get
		{
			return ControllerIconLibrary.forcedView;
		}
	}

	// Token: 0x0600000E RID: 14 RVA: 0x0000219D File Offset: 0x0000039D
	public static void SetForcedView(ControllerIconViewType view)
	{
		ControllerIconLibrary.forcedViewActive = true;
		ControllerIconLibrary.forcedView = view;
		LazySingletonSO<ControllerIconLibrary>.Instance.ApplyForcedView();
		Action<ControllerIconViewType> onViewChanged = ControllerIconLibrary.OnViewChanged;
		if (onViewChanged != null)
		{
			onViewChanged(view);
		}
		LazyButtonTipsStr.RefreshAll();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x000021CB File Offset: 0x000003CB
	public static void ClearForcedView()
	{
		if (!ControllerIconLibrary.forcedViewActive)
		{
			return;
		}
		ControllerIconLibrary.ResetForcedViewRuntimeState();
		ControllerIconLibrary.RefreshFromInput();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x000021DF File Offset: 0x000003DF
	private static void ResetForcedViewRuntimeState()
	{
		ControllerIconLibrary.forcedViewActive = false;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000021E7 File Offset: 0x000003E7
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetForcedViewOnSubsystemRegistration()
	{
		ControllerIconLibrary.ResetForcedViewRuntimeState();
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000021F0 File Offset: 0x000003F0
	public static void RefreshFromInput()
	{
		switch (Platform.Type)
		{
		case PlatformType.PС:
			if (!ControllerIconLibrary.forcedViewActive)
			{
				LazySingletonSO<ControllerIconLibrary>.Instance.UpdateInputDeviceForPC();
			}
			break;
		case PlatformType.PlayStation:
			if (!ControllerIconLibrary.forcedViewActive)
			{
				LazySingletonSO<ControllerIconLibrary>.Instance.UpdateInputDeviceForPlaystation();
			}
			break;
		case PlatformType.Switch:
		case PlatformType.Switch2:
			if (!ControllerIconLibrary.forcedViewActive)
			{
				LazySingletonSO<ControllerIconLibrary>.Instance.currentIcons = LazySingletonSO<ControllerIconLibrary>.Instance.joyConControllerIcons;
			}
			break;
		}
		LazyButtonTipsStr.RefreshAll();
	}

	// Token: 0x06000013 RID: 19 RVA: 0x0000226C File Offset: 0x0000046C
	public static string GetViewFileName(ControllerIconViewType view)
	{
		string text;
		switch (view)
		{
		case ControllerIconViewType.Keyboard:
			text = "keyboard";
			break;
		case ControllerIconViewType.Xbox:
			text = "xbox";
			break;
		case ControllerIconViewType.DualShock:
			text = "dualshock";
			break;
		case ControllerIconViewType.DualSense:
			text = "dualsense";
			break;
		case ControllerIconViewType.JoyCon:
			text = "joycon";
			break;
		default:
			text = view.ToString().ToLowerInvariant();
			break;
		}
		return text;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000022D4 File Offset: 0x000004D4
	private void ApplyForcedView()
	{
		List<ControllerIconData> list;
		switch (ControllerIconLibrary.forcedView)
		{
		case ControllerIconViewType.Keyboard:
			list = this.standaloneIcons;
			break;
		case ControllerIconViewType.Xbox:
			list = this.xBoxControllerIcons;
			break;
		case ControllerIconViewType.DualShock:
			list = this.dualShockControllerIcons;
			break;
		case ControllerIconViewType.DualSense:
			list = this.dualSenseControllerIcons;
			break;
		case ControllerIconViewType.JoyCon:
			list = this.joyConControllerIcons;
			break;
		default:
			list = this.standaloneIcons;
			break;
		}
		this.currentIcons = list;
	}

	// Token: 0x06000015 RID: 21 RVA: 0x0000233E File Offset: 0x0000053E
	public static void UpdateStandaloneIcons()
	{
		LazySingletonSO<ControllerIconLibrary>.Instance.GenerateStandaloneIcons();
		LazySingletonSO<ControllerIconLibrary>.Instance.currentIcons = LazySingletonSO<ControllerIconLibrary>.Instance.standaloneIcons;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x0000235E File Offset: 0x0000055E
	public static void UpdateStandaloneIcons(List<KeyBinding> keyBindings)
	{
		LazySingletonSO<ControllerIconLibrary>.Instance.UpdateStandaloneIconsForBindings(keyBindings);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000236B File Offset: 0x0000056B
	public static string GetIconId(GameKey key, GameKeyIconType gameKeyIconType = null, bool trailingSpace = true)
	{
		return LazySingletonSO<ControllerIconLibrary>.Instance.GetTextIcon(key, gameKeyIconType, trailingSpace);
	}

	// Token: 0x06000018 RID: 24 RVA: 0x0000237C File Offset: 0x0000057C
	public virtual void Init()
	{
		ControllerIconLibrary.ResetForcedViewRuntimeState();
		this.InitCacheData();
		switch (Platform.Type)
		{
		case PlatformType.PС:
			this.GenerateStandaloneIcons();
			this.UpdateInputDeviceForPC();
			LazyInput.OnInputChanged += this.UpdateInputDeviceForPC;
			return;
		case PlatformType.XBox:
			this.currentIcons = this.xBoxControllerIcons;
			return;
		case PlatformType.PlayStation:
			this.UpdateInputDeviceForPlaystation();
			LazyInput.OnInputChanged += this.UpdateInputDeviceForPlaystation;
			return;
		case PlatformType.Switch:
		case PlatformType.Switch2:
			this.currentIcons = this.joyConControllerIcons;
			return;
		}
		this.currentIcons = this.xBoxControllerIcons;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002418 File Offset: 0x00000618
	private void InitCacheData()
	{
		foreach (KeyCodeReadableReplacement keyCodeReadableReplacement in this.readableReplacementsForDisplay)
		{
			if (!this.keyCodeReadableReplacementsCache.TryAdd(keyCodeReadableReplacement.keyCode, keyCodeReadableReplacement))
			{
				Debug.LogError(string.Format("Error: KeyCode [{0}] is already exist, skipping [{1}]", keyCodeReadableReplacement.keyCode, keyCodeReadableReplacement.replacementDisplay));
			}
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002498 File Offset: 0x00000698
	private string GetIcon(GameKey key, GameKeyIconType gameKeyIconType)
	{
		if (this.swapAB)
		{
			GamepadBinding gamepadBinding = LazyInput.GameBindings.gamepadBindings.Find((GamepadBinding b) => b.gameKey.value == key.value);
			if (gamepadBinding != null)
			{
				if (gamepadBinding.gamepadButton.value == GamepadButton.A.value)
				{
					return this.GetIconTyped(GameKey.Back, gameKeyIconType);
				}
				if (gamepadBinding.gamepadButton.value == GamepadButton.B.value)
				{
					return this.GetIconTyped(GameKey.Select, gameKeyIconType);
				}
			}
		}
		return this.GetIconTyped(key, gameKeyIconType);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002530 File Offset: 0x00000730
	private string GetIconTyped(GameKey key, GameKeyIconType iconType)
	{
		if (iconType == null)
		{
			iconType = GameKeyIconType.Default;
		}
		GameKey gameKey = key;
		if (this.currentIcons != this.standaloneIcons)
		{
			Predicate<BindingAlias> <>9__0;
			for (int i = 0; i < this.gameKeyIconAliases.Count; i++)
			{
				List<BindingAlias> list = this.gameKeyIconAliases;
				Predicate<BindingAlias> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (BindingAlias a) => a.gameKey1.value == key.value);
				}
				BindingAlias bindingAlias = list.Find(predicate);
				if (bindingAlias != null)
				{
					gameKey = bindingAlias.gameKey2;
					break;
				}
			}
		}
		foreach (ControllerIconData controllerIconData in this.currentIcons)
		{
			if (controllerIconData.gameKey == gameKey)
			{
				for (int j = 0; j < controllerIconData.typedIcons.Length; j++)
				{
					if (controllerIconData.typedIcons[j].iconType == iconType)
					{
						return controllerIconData.typedIcons[j].iconId;
					}
				}
			}
		}
		Debug.LogError(string.Concat(new string[]
		{
			"Cannot find icon for GameKey [",
			Enumeration.GetNameOfStaticField<GameKey>(gameKey.value),
			"], icon type [",
			Enumeration.GetNameOfStaticField<GameKeyIconType>(iconType.value),
			"]"
		}));
		return string.Empty;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002698 File Offset: 0x00000898
	private string GetTextIcon(GameKey key, GameKeyIconType gameKeyIconType, bool trailingSpace = true)
	{
		string icon = this.GetIcon(key, gameKeyIconType);
		if (icon.StartsWith("["))
		{
			return icon + (trailingSpace ? " " : "");
		}
		return "<sprite name=\"" + icon + "\">" + (trailingSpace ? " " : "");
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000026F0 File Offset: 0x000008F0
	protected void GenerateStandaloneIcons()
	{
		this.standaloneIcons.Clear();
		foreach (KeyBinding keyBinding in LazyInput.GameBindings.keyBindings)
		{
			string keycodeString = this.GetKeycodeString(keyBinding.keyCode);
			this.standaloneIcons.Add(new ControllerIconData(keyBinding.gameKey, new ControllerIconDataTyped[]
			{
				new ControllerIconDataTyped(GameKeyIconType.Default, "[" + keycodeString + "]"),
				new ControllerIconDataTyped(GameKeyIconType.Inactive, string.Concat(new string[]
				{
					"[<color=#",
					ColorUtility.ToHtmlStringRGB(this.standaloneInactiveColor),
					">",
					keycodeString,
					"</color>]"
				}))
			}));
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000027DC File Offset: 0x000009DC
	protected void UpdateStandaloneIconsForBindings(List<KeyBinding> keyBindings)
	{
		foreach (ControllerIconData controllerIconData in this.standaloneIcons)
		{
			foreach (KeyBinding keyBinding in keyBindings)
			{
				if (controllerIconData.gameKey == keyBinding.gameKey)
				{
					string keycodeString = this.GetKeycodeString(keyBinding.keyCode);
					controllerIconData.typedIcons[0] = new ControllerIconDataTyped(GameKeyIconType.Default, "[" + keycodeString + "]");
					controllerIconData.typedIcons[1] = new ControllerIconDataTyped(GameKeyIconType.Inactive, string.Concat(new string[]
					{
						"[<color=#",
						ColorUtility.ToHtmlStringRGB(this.standaloneInactiveColor),
						">",
						keycodeString,
						"</color>]"
					}));
				}
			}
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000028F8 File Offset: 0x00000AF8
	protected virtual void UpdateInputDeviceForPC()
	{
		if (ControllerIconLibrary.forcedViewActive)
		{
			this.ApplyForcedView();
			return;
		}
		if (ControllerIconLibrary.IsRunningOnSteamDeck())
		{
			this.currentIcons = this.xBoxControllerIcons;
			return;
		}
		if (!LazyInput.IsGamepadActive)
		{
			this.currentIcons = this.standaloneIcons;
			return;
		}
		if (LazyInput.CurrentGamepadType == null)
		{
			this.currentIcons = this.xBoxControllerIcons;
			return;
		}
		int value = LazyInput.CurrentGamepadType.value;
		if (value == GamepadType.Sony_DualShock.value)
		{
			this.currentIcons = this.dualShockControllerIcons;
			return;
		}
		if (value == GamepadType.Sony_DualSense.value)
		{
			this.currentIcons = this.dualSenseControllerIcons;
			return;
		}
		if (value == GamepadType.Switch_Handheld.value || value == GamepadType.Switch_Pro.value || value == GamepadType.Switch_JoyCon_Left.value || value == GamepadType.Switch_JoyCon_Right.value || value == GamepadType.Switch_JoyCon_Dual.value)
		{
			this.currentIcons = this.joyConControllerIcons;
			return;
		}
		this.currentIcons = this.xBoxControllerIcons;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000029EB File Offset: 0x00000BEB
	private static bool IsRunningOnSteamDeck()
	{
		return SteamManager.Initialized && SteamUtils.IsSteamRunningOnSteamDeck();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000029FC File Offset: 0x00000BFC
	protected virtual void UpdateInputDeviceForPlaystation()
	{
		GamepadType currentGamepadType = LazyInput.CurrentGamepadType;
		int num = ((currentGamepadType != null) ? currentGamepadType.value : (-1));
		if (num == GamepadType.Sony_DualSense.value)
		{
			this.currentIcons = this.dualSenseControllerIcons;
			return;
		}
		if (num == GamepadType.Sony_DualShock.value)
		{
			this.currentIcons = this.dualShockControllerIcons;
			return;
		}
		this.currentIcons = this.GetDefaultPlaystationIcons();
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002A5B File Offset: 0x00000C5B
	private List<ControllerIconData> GetDefaultPlaystationIcons()
	{
		return this.dualShockControllerIcons;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00002A64 File Offset: 0x00000C64
	public string GetKeycodeString(KeyCode keyCode)
	{
		string text = keyCode.ToString();
		text = text.Replace("Alpha", "");
		KeyCodeReadableReplacement keyCodeReadableReplacement;
		if (this.useReadableKeyCodeReplacements && this.keyCodeReadableReplacementsCache.TryGetValue(keyCode, out keyCodeReadableReplacement))
		{
			return keyCodeReadableReplacement.replacementDisplay;
		}
		if (text == "Escape")
		{
			return "Esc";
		}
		if (this.useSplittingForKeyCodeStr)
		{
			return Regex.Replace(text, "([A-Z][a-z]*|\\d+)", " $1").Trim();
		}
		return text;
	}

	// Token: 0x0400002A RID: 42
	[SerializeField]
	private Color standaloneInactiveColor = new Color(215f, 215f, 215f, 255f);

	// Token: 0x0400002B RID: 43
	[Space]
	public List<ControllerIconData> xBoxControllerIcons = new List<ControllerIconData>();

	// Token: 0x0400002C RID: 44
	public List<ControllerIconData> dualShockControllerIcons = new List<ControllerIconData>();

	// Token: 0x0400002D RID: 45
	public List<ControllerIconData> dualSenseControllerIcons = new List<ControllerIconData>();

	// Token: 0x0400002E RID: 46
	public List<ControllerIconData> joyConControllerIcons = new List<ControllerIconData>();

	// Token: 0x0400002F RID: 47
	protected List<ControllerIconData> standaloneIcons = new List<ControllerIconData>();

	// Token: 0x04000030 RID: 48
	public List<BindingAlias> gameKeyIconAliases;

	// Token: 0x04000031 RID: 49
	protected List<ControllerIconData> currentIcons;

	// Token: 0x04000032 RID: 50
	private static bool forcedViewActive;

	// Token: 0x04000033 RID: 51
	private static ControllerIconViewType forcedView;

	// Token: 0x04000035 RID: 53
	[Header("KeyCode Standalone Options")]
	[Space]
	public bool useReadableKeyCodeReplacements;

	// Token: 0x04000036 RID: 54
	public List<KeyCodeReadableReplacement> readableReplacementsForDisplay = new List<KeyCodeReadableReplacement>();

	// Token: 0x04000037 RID: 55
	private Dictionary<KeyCode, KeyCodeReadableReplacement> keyCodeReadableReplacementsCache = new Dictionary<KeyCode, KeyCodeReadableReplacement>();

	// Token: 0x04000038 RID: 56
	public bool useSplittingForKeyCodeStr;

	// Token: 0x04000039 RID: 57
	private bool swapAB;
}
