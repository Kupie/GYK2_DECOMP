using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020008FD RID: 2301
public class UIGameBindingSettingsWindow : LazyWindow<LazyWidgetDataBase>
{
	// Token: 0x140000BB RID: 187
	// (add) Token: 0x06003C37 RID: 15415 RVA: 0x0011FC04 File Offset: 0x0011DE04
	// (remove) Token: 0x06003C38 RID: 15416 RVA: 0x0011FC38 File Offset: 0x0011DE38
	public static event Action OnBtnUpdated;

	// Token: 0x06003C39 RID: 15417 RVA: 0x0011FC6B File Offset: 0x0011DE6B
	private void Awake()
	{
		this.okBtn.onClick.AddListener(new UnityAction(this.Close));
		this.restoreDefaultBindings.onClick.AddListener(new UnityAction(this.OnReset));
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x0011FCA8 File Offset: 0x0011DEA8
	public override void Init()
	{
		this.bindings = LazyInput.GameBindings;
		this.keyBindings = this.bindings.keyBindings;
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRect, 21f);
		SmoothMouseWheelScroll.EnsureForItem(this.scrollRectGamepad, 21f);
		base.Init();
		base.GamepadNavigationController.loopVerticalNavigation = true;
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x0011FD08 File Offset: 0x0011DF08
	public override void Redraw()
	{
		base.Redraw();
		this.isLocked = false;
		this.okBtn.interactable = true;
		this.restoreDefaultBindings.interactable = true;
		foreach (KeyValuePair<GameKey, UIGameBindingElement> keyValuePair in this.cachedBindingElements)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIGameBindingElement>(keyValuePair.Value);
		}
		this.cachedBindingElements.Clear();
		foreach (UIGameBindingElement uigameBindingElement in this.gamepadBindingsList)
		{
			UIPrefabsPooler.Instance.ReleaseElementToPool<UIGameBindingElement>(uigameBindingElement);
		}
		this.gamepadBindingsList.Clear();
		using (List<GameKey>.Enumerator enumerator3 = this.keysToBind.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				GameKey gameKey = enumerator3.Current;
				UIGameBindingElement uigameBindingElement2 = UIPrefabsPooler.Instance.GetElementFromPool<UIGameBindingElement>(this.scrollRect.content);
				uigameBindingElement2.gameObject.SetActive(true);
				KeyBinding keyBinding = this.keyBindings.Find((KeyBinding k) => k.gameKey.value == gameKey.value);
				uigameBindingElement2.Initialize(keyBinding, new Action<UIGameBindingElement>(this.UpdateBinding), delegate
				{
					this.SetLockOnBindingButtons(true);
				});
				this.cachedBindingElements.Add(gameKey, uigameBindingElement2);
			}
		}
		using (List<GameKey>.Enumerator enumerator3 = this.gamepadBindings.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				GameKey gamepadBinding = enumerator3.Current;
				UIGameBindingElement uigameBindingElement2 = UIPrefabsPooler.Instance.GetElementFromPool<UIGameBindingElement>(this.scrollRectGamepad.content);
				uigameBindingElement2.gameObject.SetActive(true);
				uigameBindingElement2.InitializedForGamepad(this.bindings.gamepadBindings.Find((GamepadBinding b) => b.gameKey.value == gamepadBinding.value));
				this.gamepadBindingsList.Add(uigameBindingElement2);
			}
		}
		((RectTransform)base.transform).RefreshContentFitter();
		this.scrollRect.ResetPosition();
		this.scrollRectGamepad.ResetPosition();
	}

	// Token: 0x06003C3C RID: 15420 RVA: 0x0011FF68 File Offset: 0x0011E168
	public override void Close()
	{
		if (this.isLocked)
		{
			return;
		}
		base.Close();
		Action action = this.onClosed;
		if (action != null)
		{
			action();
		}
		this.onClosed = null;
	}

	// Token: 0x06003C3D RID: 15421 RVA: 0x0011FF94 File Offset: 0x0011E194
	private void OnReset()
	{
		GameSettings.Instance.ApplyDefaultGameBindings();
		foreach (UIGameBindingElement uigameBindingElement in this.cachedBindingElements.Values)
		{
			uigameBindingElement.UpdateKeyLabel();
		}
		Action onBtnUpdated = UIGameBindingSettingsWindow.OnBtnUpdated;
		if (onBtnUpdated == null)
		{
			return;
		}
		onBtnUpdated();
	}

	// Token: 0x06003C3E RID: 15422 RVA: 0x00120004 File Offset: 0x0011E204
	protected override void PrintTips()
	{
		List<LazyGameKeyTip> list = new List<LazyGameKeyTip>();
		list.Add(LazyGameKeyTip.Back(true, true, true));
		this.lazyButtonTips.Print(list, "  ");
	}

	// Token: 0x06003C3F RID: 15423 RVA: 0x00120036 File Offset: 0x0011E236
	protected override void UpdateGamepadDependentStuff()
	{
		base.UpdateGamepadDependentStuff();
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06003C40 RID: 15424 RVA: 0x00120050 File Offset: 0x0011E250
	private void SetLockOnBindingButtons(bool state)
	{
		this.isLocked = state;
		this.okBtn.interactable = !this.isLocked;
		this.restoreDefaultBindings.interactable = !this.isLocked;
		foreach (KeyValuePair<GameKey, UIGameBindingElement> keyValuePair in this.cachedBindingElements)
		{
			keyValuePair.Value.ChangeIsAvailable(!state);
		}
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x001200DC File Offset: 0x0011E2DC
	private void UpdateBinding(UIGameBindingElement bindingElement)
	{
		KeyBinding keyBinding = null;
		foreach (KeyBinding keyBinding2 in this.keyBindings)
		{
			if (keyBinding2.gameKey.value == GameKey.Interaction.value)
			{
				keyBinding = keyBinding2;
			}
			if (keyBinding2.gameKey.value != bindingElement.keyBinding.gameKey.value && keyBinding2.keyCode == bindingElement.keyBinding.keyCode)
			{
				keyBinding2.keyCode = KeyCode.None;
			}
		}
		if (keyBinding != null)
		{
			foreach (KeyBinding keyBinding3 in this.keyBindings)
			{
				if (keyBinding3.gameKey.value == GameKey.SpeechSkip2.value)
				{
					keyBinding3.keyCode = keyBinding.keyCode;
					break;
				}
			}
		}
		this.SetLockOnBindingButtons(false);
		foreach (KeyValuePair<GameKey, UIGameBindingElement> keyValuePair in this.cachedBindingElements)
		{
			keyValuePair.Value.UpdateKeyLabel();
		}
		GameSettings.Instance.SaveCurrentGameBindings();
		ControllerIconLibrary.UpdateStandaloneIcons();
		Action onBtnUpdated = UIGameBindingSettingsWindow.OnBtnUpdated;
		if (onBtnUpdated == null)
		{
			return;
		}
		onBtnUpdated();
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x0012024C File Offset: 0x0011E44C
	[LazyUITest]
	protected override void TestDraw()
	{
		LazyUI.GetWindow<UIGameBindingSettingsWindow>().Open(null);
	}

	// Token: 0x04002F5E RID: 12126
	[SerializeField]
	private LazyButton restoreDefaultBindings;

	// Token: 0x04002F5F RID: 12127
	[SerializeField]
	private LazyButton okBtn;

	// Token: 0x04002F60 RID: 12128
	[SerializeField]
	private List<GameKey> keysToBind = new List<GameKey>();

	// Token: 0x04002F61 RID: 12129
	[SerializeField]
	private List<GameKey> gamepadBindings = new List<GameKey>();

	// Token: 0x04002F62 RID: 12130
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04002F63 RID: 12131
	[SerializeField]
	private ScrollRect scrollRectGamepad;

	// Token: 0x04002F64 RID: 12132
	private Dictionary<GameKey, UIGameBindingElement> cachedBindingElements = new Dictionary<GameKey, UIGameBindingElement>();

	// Token: 0x04002F65 RID: 12133
	private List<UIGameBindingElement> gamepadBindingsList = new List<UIGameBindingElement>();

	// Token: 0x04002F66 RID: 12134
	private GameBindings bindings;

	// Token: 0x04002F67 RID: 12135
	private List<KeyBinding> keyBindings;

	// Token: 0x04002F68 RID: 12136
	private bool isLocked;

	// Token: 0x04002F69 RID: 12137
	public Action onClosed;
}
