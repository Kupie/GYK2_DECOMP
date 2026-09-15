using System;
using LazyBearTechnology;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009EA RID: 2538
public class UIDialogWindowButton : MonoBehaviour, IPoolable
{
	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x0600442D RID: 17453 RVA: 0x001440F8 File Offset: 0x001422F8
	public bool ReplaceForGamepad
	{
		get
		{
			return this.replaceForGamepad;
		}
	}

	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x0600442E RID: 17454 RVA: 0x00144100 File Offset: 0x00142300
	public GameKey KeyToReplace
	{
		get
		{
			return this.keyToReplace;
		}
	}

	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x0600442F RID: 17455 RVA: 0x00144108 File Offset: 0x00142308
	public LazyButton LazyButton
	{
		get
		{
			return this.lazyButton;
		}
	}

	// Token: 0x06004430 RID: 17456 RVA: 0x00144110 File Offset: 0x00142310
	public void Draw(UIDialogWindowData.ButtonData data)
	{
		this.replaceForGamepad = data.replaceForGamepad;
		this.keyToReplace = data.keyToReplace;
		this.label.text = data.text;
		this.textGamepad = data.textGamepad;
		this.lazyButton.onClick.RemoveAllListeners();
		this.lazyButton.onClick.AddListener(delegate
		{
			Action onPressed = data.onPressed;
			if (onPressed == null)
			{
				return;
			}
			onPressed();
		});
		Selectable selectable = this.lazyButton;
		Func<bool> buttonAvailableCondition = data.buttonAvailableCondition;
		selectable.interactable = buttonAvailableCondition == null || buttonAvailableCondition();
		this.lazyButton.RefreshTextTransitions();
		this.CacheOwnerWindow();
		this.SyncHotkeyProcessing();
		this.UpdateGamepadDependentState();
	}

	// Token: 0x06004431 RID: 17457 RVA: 0x001441E0 File Offset: 0x001423E0
	private void Awake()
	{
		if (this.replaceForGamepad)
		{
			LazyInput.OnInputChanged += this.UpdateGamepadDependentState;
			LazyInput.OnActiveGamepadChangedEvent += this.UpdateGamepadDependentState;
		}
		LazyWindowsStackController.OnWindowBecameVisibleInStack += this.OnWindowBecameVisibleInStack;
		LazyWindowsStackController.OnWindowBecameHiddenInStack += this.OnWindowBecameHiddenInStack;
	}

	// Token: 0x06004432 RID: 17458 RVA: 0x0014423C File Offset: 0x0014243C
	private void Update()
	{
		if (!this.canProcessHotkey)
		{
			return;
		}
		if ((LazyInput.IsGamepadActive || this.keyToReplace.value == GameKey.Back.value) && this.replaceForGamepad && this.lazyButton.interactable && LazyInput.GetKeyDown(this.keyToReplace))
		{
			LazyInput.ClearKeyDown(this.keyToReplace);
			Button.ButtonClickedEvent onClick = this.lazyButton.onClick;
			if (onClick == null)
			{
				return;
			}
			onClick.Invoke();
		}
	}

	// Token: 0x06004433 RID: 17459 RVA: 0x001442B2 File Offset: 0x001424B2
	private void OnWindowBecameVisibleInStack(LazyWidgetBase window)
	{
		if (window == this.ownerWindow)
		{
			this.canProcessHotkey = true;
		}
	}

	// Token: 0x06004434 RID: 17460 RVA: 0x001442C9 File Offset: 0x001424C9
	private void OnWindowBecameHiddenInStack(LazyWidgetBase window)
	{
		if (window == this.ownerWindow)
		{
			this.canProcessHotkey = false;
		}
	}

	// Token: 0x06004435 RID: 17461 RVA: 0x001442E0 File Offset: 0x001424E0
	private void CacheOwnerWindow()
	{
		this.ownerWindow = null;
		Transform transform = base.transform;
		while (transform != null)
		{
			LazyWidgetBase lazyWidgetBase;
			Canvas canvas;
			if (transform.TryGetComponent<LazyWidgetBase>(out lazyWidgetBase) && transform.TryGetComponent<Canvas>(out canvas))
			{
				this.ownerWindow = lazyWidgetBase;
				return;
			}
			transform = transform.parent;
		}
	}

	// Token: 0x06004436 RID: 17462 RVA: 0x00144329 File Offset: 0x00142529
	private void SyncHotkeyProcessing()
	{
		this.canProcessHotkey = this.ownerWindow != null && this.ownerWindow == LazyWindowsStackController.ActiveWindow;
	}

	// Token: 0x06004437 RID: 17463 RVA: 0x00144354 File Offset: 0x00142554
	private void OnDestroy()
	{
		if (this.replaceForGamepad)
		{
			LazyInput.OnInputChanged -= this.UpdateGamepadDependentState;
			LazyInput.OnActiveGamepadChangedEvent -= this.UpdateGamepadDependentState;
		}
		LazyWindowsStackController.OnWindowBecameVisibleInStack -= this.OnWindowBecameVisibleInStack;
		LazyWindowsStackController.OnWindowBecameHiddenInStack -= this.OnWindowBecameHiddenInStack;
	}

	// Token: 0x06004438 RID: 17464 RVA: 0x001443B0 File Offset: 0x001425B0
	private void UpdateGamepadDependentState()
	{
		if (this.replaceForGamepad && LazyInput.IsGamepadActive && base.gameObject.activeInHierarchy)
		{
			if (this.mouseObj != null)
			{
				this.mouseObj.gameObject.SetActive(false);
			}
			if (this.gamepadObj != null)
			{
				this.gamepadObj.gameObject.SetActive(true);
				if (this.gamepadTip != null && this.keyToReplace != null)
				{
					string text = (string.IsNullOrEmpty(this.textGamepad) ? this.label.text : this.textGamepad);
					this.gamepadTip.text = new LazyGameKeyTip(this.keyToReplace, text, this.lazyButton.interactable, true, true).ToString();
				}
			}
		}
		else
		{
			if (!this.replaceForGamepad)
			{
				this.lazyButton.SetCallbacksIntoGamepadNavigationItem();
			}
			if (this.mouseObj != null)
			{
				this.mouseObj.gameObject.SetActive(true);
			}
			if (this.gamepadObj != null)
			{
				this.gamepadObj.gameObject.SetActive(false);
			}
		}
		((RectTransform)base.transform).RefreshContentFitter();
	}

	// Token: 0x06004439 RID: 17465 RVA: 0x001444EC File Offset: 0x001426EC
	public void OnPoolableObjReleased()
	{
		this.textGamepad = string.Empty;
		this.ownerWindow = null;
		this.canProcessHotkey = false;
	}

	// Token: 0x04003531 RID: 13617
	[SerializeField]
	private LazyButton lazyButton;

	// Token: 0x04003532 RID: 13618
	[SerializeField]
	private TextMeshProUGUI label;

	// Token: 0x04003533 RID: 13619
	[SerializeField]
	private bool replaceForGamepad;

	// Token: 0x04003534 RID: 13620
	[SerializeField]
	private GameObject mouseObj;

	// Token: 0x04003535 RID: 13621
	[SerializeField]
	private GameObject gamepadObj;

	// Token: 0x04003536 RID: 13622
	[SerializeField]
	private TextMeshProUGUI gamepadTip;

	// Token: 0x04003537 RID: 13623
	private GameKey keyToReplace;

	// Token: 0x04003538 RID: 13624
	private LazyWidgetBase ownerWindow;

	// Token: 0x04003539 RID: 13625
	private bool canProcessHotkey;

	// Token: 0x0400353A RID: 13626
	private string textGamepad;
}
