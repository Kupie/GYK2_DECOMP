using System;
using System.Collections;
using System.Collections.Generic;
using LazyBearTechnology;
using LinqTools;
using Rewired.Integration.UnityUI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

// Token: 0x02000824 RID: 2084
[ExecuteInEditMode]
public class GUIElements : MonoBehaviour
{
	// Token: 0x140000B3 RID: 179
	// (add) Token: 0x0600354F RID: 13647 RVA: 0x001008F8 File Offset: 0x000FEAF8
	// (remove) Token: 0x06003550 RID: 13648 RVA: 0x0010092C File Offset: 0x000FEB2C
	public static event Action<UIWindowSizeType> OnWindowSizeTypeChanged;

	// Token: 0x170007F0 RID: 2032
	// (get) Token: 0x06003551 RID: 13649 RVA: 0x0010095F File Offset: 0x000FEB5F
	public Transform WorldMin
	{
		get
		{
			return this.worldMin;
		}
	}

	// Token: 0x170007F1 RID: 2033
	// (get) Token: 0x06003552 RID: 13650 RVA: 0x00100967 File Offset: 0x000FEB67
	public Transform WorldMax
	{
		get
		{
			return this.worldMax;
		}
	}

	// Token: 0x170007F2 RID: 2034
	// (get) Token: 0x06003553 RID: 13651 RVA: 0x0010096F File Offset: 0x000FEB6F
	public static GUIElements Instance
	{
		get
		{
			if (GUIElements.instance == null)
			{
				GUIElements.instance = global::UnityEngine.Object.FindObjectOfType<GUIElements>();
			}
			return GUIElements.instance;
		}
	}

	// Token: 0x170007F3 RID: 2035
	// (get) Token: 0x06003554 RID: 13652 RVA: 0x0010098D File Offset: 0x000FEB8D
	public RectTransform Root
	{
		get
		{
			if (this.root == null)
			{
				this.root = base.GetComponent<RectTransform>();
			}
			return this.root;
		}
	}

	// Token: 0x170007F4 RID: 2036
	// (get) Token: 0x06003555 RID: 13653 RVA: 0x001009AF File Offset: 0x000FEBAF
	public WorldZoneWidget WorldZoneWidget
	{
		get
		{
			return this.worldZoneWidget;
		}
	}

	// Token: 0x170007F5 RID: 2037
	// (get) Token: 0x06003556 RID: 13654 RVA: 0x001009B7 File Offset: 0x000FEBB7
	public UINpcWidget NpcWidget
	{
		get
		{
			return this.npcWidget;
		}
	}

	// Token: 0x170007F6 RID: 2038
	// (get) Token: 0x06003557 RID: 13655 RVA: 0x001009BF File Offset: 0x000FEBBF
	public UIWindowSizeType UIWindowSizeType
	{
		get
		{
			return this.uiWindowSizeType;
		}
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x001009C8 File Offset: 0x000FEBC8
	public void Initialize()
	{
		this.OnResolutionChanged(GameSettings.Instance.GetResolutionIntVector2());
		GameSettings.OnResolutionChanged += this.OnResolutionChanged;
		LazySingleton<LazyWidgetPrefabContainer>.Instance.Init();
		LazyGameKeyTip.InitSelectAndBackLocales("tip_select", "tip_back");
		this.dynamicSelector.Init();
		this.dynamicSelector.SetOnUpdateCheckActivity(new Func<bool>(this.OnUpdateDynamicSelectorActivity));
		LazyInput.OnInputChanged += this.OnInputChanged;
		LazyInput.ClearAllKeysDown();
		LazyInput.OnInputChanged += this.UpdateRewiredInputType;
		this.standaloneInputModule.ForceInitEventSystem();
		this.worldZoneWidget.gameObject.SetActive(false);
		MainGame.OnGoToMainMenu = (Action)Delegate.Combine(MainGame.OnGoToMainMenu, new Action(delegate
		{
			this.worldZoneWidget.gameObject.SetActive(false);
		}));
		this.UpdateRewiredInputType();
		LazyUI.SetWindowLoadAction(new Func<string, LazyWidgetBase>(this.LoadWindowFrom));
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x00100AAB File Offset: 0x000FECAB
	public void Clear()
	{
		UIObjectBubbleManager.Instance.Clear();
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x00100AB7 File Offset: 0x000FECB7
	public void OnResolutionChanged(IntVector2 res)
	{
		this.ApplyUiForResolution();
		if (!base.isActiveAndEnabled)
		{
			return;
		}
		if (this.delayedResolutionApply != null)
		{
			base.StopCoroutine(this.delayedResolutionApply);
		}
		this.delayedResolutionApply = base.StartCoroutine(this.ApplyUiForResolutionNextFrame());
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x00100AEE File Offset: 0x000FECEE
	private IEnumerator ApplyUiForResolutionNextFrame()
	{
		yield return null;
		this.ApplyUiForResolution();
		this.delayedResolutionApply = null;
		yield break;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x00100B00 File Offset: 0x000FED00
	private void ApplyUiForResolution()
	{
		float uiScaleFactor = ResolutionConfig.GetUiScaleFactor();
		this.canvasScaler.scaleFactor = uiScaleFactor;
		if (LazyUI.IsInitialized)
		{
			LazyUI.SetCanvasScaleFactor(uiScaleFactor);
			LazyUI.SetSafeZones(Screen.safeArea);
		}
		if (ResolutionConfig.currentResolution != null)
		{
			this.SetUIMode(ResolutionConfig.currentResolution.WindowSizeType);
		}
		Canvas.ForceUpdateCanvases();
		if (LazyWindowsStackController.ActiveWindow != null)
		{
			RectTransform rectTransform = (RectTransform)LazyWindowsStackController.ActiveWindow.transform;
			if (LazyWindowsStackController.ActiveWindow is UIMainMenuWindow)
			{
				rectTransform.RefreshContentFitterAndDisable();
			}
			else
			{
				rectTransform.RefreshContentFitter();
			}
		}
		if (this.worldZoneWidget != null && this.worldZoneWidget.gameObject.activeInHierarchy)
		{
			((RectTransform)this.worldZoneWidget.transform).RefreshContentFitter();
		}
		if (LazyUI.IsInitialized)
		{
			HUD hud = LazyUI.Get<HUD>();
			if (hud != null)
			{
				((RectTransform)hud.transform).RefreshContentFitter();
				hud.RefreshTechPointsPanelForResolution();
			}
		}
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x00100BEC File Offset: 0x000FEDEC
	public void SetUIMode(UIWindowSizeType uiWindowSizeType)
	{
		Debug.Log("SetUIMode: " + uiWindowSizeType.ToString());
		if (this.uiWindowSizeType != uiWindowSizeType)
		{
			if (this.uiWindowSizeType == UIWindowSizeType.Big)
			{
				LazyUI.ClearWindowsFromCache(this.bigSizeWindows.Keys.ToList<string>());
				this.bigSizeWindows.Clear();
			}
			else
			{
				LazyUI.ClearWindowsFromCache(this.smallSizeWindows.Keys.ToList<string>());
				this.smallSizeWindows.Clear();
			}
			this.uiWindowSizeType = uiWindowSizeType;
			Action<UIWindowSizeType> onWindowSizeTypeChanged = GUIElements.OnWindowSizeTypeChanged;
			if (onWindowSizeTypeChanged == null)
			{
				return;
			}
			onWindowSizeTypeChanged(uiWindowSizeType);
		}
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x00100C80 File Offset: 0x000FEE80
	public void SetVisibilityState(bool isVisible)
	{
		if (isVisible == !this.isVisuallyHidden)
		{
			return;
		}
		foreach (ILazyGUIElement lazyGUIElement in new List<ILazyGUIElement>
		{
			LazyUI.Get<Bubble>(),
			LazyUI.Get<UICinematic>(),
			LazyUI.Get<UIFade>(),
			LazyUI.Get<UISleepFade>()
		})
		{
			MonoBehaviour monoBehaviour = lazyGUIElement as MonoBehaviour;
			CanvasGroup canvasGroup;
			if (monoBehaviour != null && monoBehaviour.TryGetComponent<CanvasGroup>(out canvasGroup))
			{
				if (!isVisible)
				{
					this.storedGroupStates.Add(lazyGUIElement, canvasGroup.ignoreParentGroups);
					canvasGroup.ignoreParentGroups = true;
				}
				else
				{
					canvasGroup.ignoreParentGroups = this.storedGroupStates[lazyGUIElement];
					this.storedGroupStates.Remove(lazyGUIElement);
				}
			}
		}
		if (!isVisible)
		{
			CinematicsTextWidget[] componentsInChildren = this.Root.GetComponentsInChildren<CinematicsTextWidget>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				CanvasGroup canvasGroup2;
				if (componentsInChildren[i].TryGetComponent<CanvasGroup>(out canvasGroup2))
				{
					canvasGroup2.ignoreParentGroups = true;
				}
			}
		}
		this.canvasGroup.alpha = (isVisible ? 1f : 0f);
		this.isVisuallyHidden = !isVisible;
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x00100DB4 File Offset: 0x000FEFB4
	public LazyWidgetBase LoadWindowFrom(string windowTypeName)
	{
		Dictionary<string, LazyWidgetBase> dictionary;
		string text;
		if (this.uiWindowSizeType == UIWindowSizeType.Small && this.windowsWithSmallVersion.Contains(windowTypeName))
		{
			dictionary = this.smallSizeWindows;
			text = "Assets/AddressableAssets/UIElements/WindowsSmall/" + windowTypeName + "_Small.prefab";
		}
		else
		{
			dictionary = this.bigSizeWindows;
			text = "Assets/AddressableAssets/UIElements/WindowsBig/" + windowTypeName + "_Big.prefab";
		}
		LazyWidgetBase component;
		if (dictionary.TryGetValue(windowTypeName, out component))
		{
			return component;
		}
		Debug.Log(string.Format("#shutdown# GUIElements: window [{0}] WaitForCompletion begin (shutdownRequested:[{1}])", windowTypeName, GameShutdown.IsRequested));
		if (GameShutdown.IsQuitting)
		{
			return null;
		}
		GameObject gameObject = Addressables.LoadAssetAsync<GameObject>(text).WaitForCompletion();
		Debug.Log("#shutdown# GUIElements: window [" + windowTypeName + "] WaitForCompletion done");
		component = global::UnityEngine.Object.Instantiate<GameObject>(gameObject, this.uiFitter.transform).GetComponent<LazyWidgetBase>();
		ILazyGUIElement[] componentsInChildren = component.GetComponentsInChildren<ILazyGUIElement>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Init();
		}
		component.gameObject.SetActive(false);
		dictionary.Add(windowTypeName, component);
		return component;
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x00100EB0 File Offset: 0x000FF0B0
	public void UpdateLocalizedLabels()
	{
		LocalizedLabel[] componentsInChildren = base.GetComponentsInChildren<LocalizedLabel>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Localize();
		}
		LocalizedAssetReferenceImage[] componentsInChildren2 = base.GetComponentsInChildren<LocalizedAssetReferenceImage>(true);
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].Localize();
		}
		LocalizedTextMargins[] componentsInChildren3 = base.GetComponentsInChildren<LocalizedTextMargins>(true);
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].ApplyMargins();
		}
		LocalizedVerticalOffset[] componentsInChildren4 = base.GetComponentsInChildren<LocalizedVerticalOffset>(true);
		for (int i = 0; i < componentsInChildren4.Length; i++)
		{
			componentsInChildren4[i].Apply();
		}
		LocalizedSize[] componentsInChildren5 = base.GetComponentsInChildren<LocalizedSize>(true);
		for (int i = 0; i < componentsInChildren5.Length; i++)
		{
			componentsInChildren5[i].Apply();
		}
	}

	// Token: 0x06003561 RID: 13665 RVA: 0x00100F59 File Offset: 0x000FF159
	public void PreloadWindows()
	{
		LazyUI.GetWindow<CharacterWindow>();
		LazyUI.GetWindow<UIMainMenuWindow>();
	}

	// Token: 0x06003562 RID: 13666 RVA: 0x0003C7FE File Offset: 0x0003A9FE
	private bool OnUpdateDynamicSelectorActivity()
	{
		return true;
	}

	// Token: 0x06003563 RID: 13667 RVA: 0x00002318 File Offset: 0x00000518
	private void OnInputChanged()
	{
	}

	// Token: 0x06003564 RID: 13668 RVA: 0x00100F67 File Offset: 0x000FF167
	private void UpdateRewiredInputType()
	{
		this.standaloneInputModule.isGamepadActive = LazyInput.IsGamepadActive;
		if (LazyInput.IsGamepadActive)
		{
			this.standaloneInputModule.ClearMouseSelection();
		}
	}

	// Token: 0x04002AC0 RID: 10944
	private readonly string[] windowsWithSmallVersion = new string[] { "CharacterWindow", "UIMapWindow" };

	// Token: 0x04002AC1 RID: 10945
	public const int OBJECT_BUBBLE_DEFAULT_SORTING_ORDER_VALUE = 40;

	// Token: 0x04002AC2 RID: 10946
	public const int SLEEP_BLACKOUT_DEFAULT_SORTING_ORDER_VALUE = 49;

	// Token: 0x04002AC3 RID: 10947
	public const int HUD_DEFAULT_SORTING_ORDER_VALUE = 50;

	// Token: 0x04002AC4 RID: 10948
	public const int TUTORIAL_DEFAULT_ARROW_SORTING = 51;

	// Token: 0x04002AC5 RID: 10949
	public const int CINEMATIC_DEFAULT_SORTING_ORDER_VALUE = 300;

	// Token: 0x04002AC6 RID: 10950
	public const int SPEECH_DEFAULT_SORTING_ORDER_VALUE = 350;

	// Token: 0x04002AC7 RID: 10951
	public const int TOOLTIP_DEFAULT_SORTING_ORDER_VALUE = 700;

	// Token: 0x04002AC8 RID: 10952
	public const int BLACKOUT_DEFAULT_SORTING_ORDER_VALUE = 800;

	// Token: 0x04002AC9 RID: 10953
	public const int OVER_BLACKOUT_SORTING_ORDER_VALUE = 900;

	// Token: 0x04002ACB RID: 10955
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x04002ACC RID: 10956
	[SerializeField]
	private CanvasScaler canvasScaler;

	// Token: 0x04002ACD RID: 10957
	[SerializeField]
	private UIFitter uiFitter;

	// Token: 0x04002ACE RID: 10958
	[SerializeField]
	private GamepadDynamicSelector dynamicSelector;

	// Token: 0x04002ACF RID: 10959
	[SerializeField]
	public RewiredStandaloneInputModule standaloneInputModule;

	// Token: 0x04002AD0 RID: 10960
	[SerializeField]
	private Transform worldMin;

	// Token: 0x04002AD1 RID: 10961
	[SerializeField]
	private Transform worldMax;

	// Token: 0x04002AD2 RID: 10962
	[Space]
	[SerializeField]
	private WorldZoneWidget worldZoneWidget;

	// Token: 0x04002AD3 RID: 10963
	[Space]
	[SerializeField]
	private UINpcWidget npcWidget;

	// Token: 0x04002AD4 RID: 10964
	private bool isVisuallyHidden;

	// Token: 0x04002AD5 RID: 10965
	private Dictionary<ILazyGUIElement, bool> storedGroupStates = new Dictionary<ILazyGUIElement, bool>();

	// Token: 0x04002AD6 RID: 10966
	private static GUIElements instance;

	// Token: 0x04002AD7 RID: 10967
	private Dictionary<string, LazyWidgetBase> smallSizeWindows = new Dictionary<string, LazyWidgetBase>();

	// Token: 0x04002AD8 RID: 10968
	private Dictionary<string, LazyWidgetBase> bigSizeWindows = new Dictionary<string, LazyWidgetBase>();

	// Token: 0x04002AD9 RID: 10969
	private RectTransform root;

	// Token: 0x04002ADA RID: 10970
	private UIWindowSizeType uiWindowSizeType;

	// Token: 0x04002ADB RID: 10971
	private Coroutine delayedResolutionApply;
}
