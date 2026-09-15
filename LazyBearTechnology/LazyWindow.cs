using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LazyBearTechnology
{
	// Token: 0x0200016A RID: 362
	[RequireComponent(typeof(GamepadNavigationController))]
	[RequireComponent(typeof(Canvas))]
	public abstract class LazyWindow<T> : LazyWidget<T> where T : LazyWidgetDataBase
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x0002769D File Offset: 0x0002589D
		protected GamepadNavigationController GamepadNavigationController
		{
			get
			{
				if (this.gamepadNavigationController == null)
				{
					this.gamepadNavigationController = base.GetComponent<GamepadNavigationController>();
				}
				return this.gamepadNavigationController;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x000276BF File Offset: 0x000258BF
		public Canvas Canvas
		{
			get
			{
				return this.canvas;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x000276C7 File Offset: 0x000258C7
		public bool IsShown
		{
			get
			{
				return this.isShown;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060007CB RID: 1995 RVA: 0x000276CF File Offset: 0x000258CF
		public bool IsTop
		{
			get
			{
				return LazyWindowsStackController.IsWindowOnTop<T>(this);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x000276D7 File Offset: 0x000258D7
		public bool IsShownAndTop
		{
			get
			{
				return this.IsShown && this.IsTop;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x000276E9 File Offset: 0x000258E9
		public bool IsModalWindow
		{
			get
			{
				return this.isModalWindow;
			}
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x000276F1 File Offset: 0x000258F1
		public override void Init()
		{
			base.Init();
			if (this.closeButton)
			{
				this.InitCloseButton(this.closeButton);
			}
			this.InitInputController();
			this.InitButtonTipsStr();
			this.InitCanvas();
			this.SubscribePermanentEvents();
			this.HideWindow();
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00027730 File Offset: 0x00025930
		public virtual void Open(T data)
		{
			this.ShowWindow();
			this.Draw(data);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002773F File Offset: 0x0002593F
		public void Open(T data, Action<T> onClosed)
		{
			this.Open(data);
			this.OnClosed = onClosed;
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x0002774F File Offset: 0x0002594F
		public virtual void Close()
		{
			this.HideWindow();
			Action<T> onClosed = this.OnClosed;
			if (onClosed != null)
			{
				onClosed(this.data);
			}
			this.OnClosed = null;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00027775 File Offset: 0x00025975
		public virtual void CloseWithoutCallback()
		{
			this.HideWindow();
			this.OnClosed = null;
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00027784 File Offset: 0x00025984
		protected virtual void ShowWindow()
		{
			if (this.isShown)
			{
				Debug.LogWarning("ShowWindow(): Window " + base.name + " is already shown. Call Redraw.", this);
				return;
			}
			if (!string.IsNullOrEmpty(this.openSoundId))
			{
				LazyAudio.PlayAndForget(this.openSoundId);
			}
			this.isShown = true;
			this.lazyWindowInputController.Enable(false);
			LazyInput.OnInputChanged += this.OnInputChanged;
			this.UpdateGamepadDependentStuff();
			this.lazyWindowInputController.UpdateGamepadDependentStuff();
			base.gameObject.SetActive(true);
			LazyWindowsStackController.AddToStack<T>(this);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00027814 File Offset: 0x00025A14
		protected virtual void HideWindow()
		{
			this.lazyWindowInputController.Disable(false);
			LazyInput.OnInputChanged -= this.OnInputChanged;
			if (this.isShown && !string.IsNullOrEmpty(this.closeSoundId))
			{
				LazyAudio.PlayAndForget(this.closeSoundId);
			}
			this.isShown = false;
			LazyWindowsStackController.RemoveFromStack<T>(this);
			this.Hide();
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00027871 File Offset: 0x00025A71
		protected virtual void Update()
		{
			this.lazyWindowInputController.Update();
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0002787E File Offset: 0x00025A7E
		protected virtual void InitCloseButton(LazyButton button)
		{
			button.onClick.AddListener(new UnityAction(this.Close));
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00027898 File Offset: 0x00025A98
		protected virtual void SubscribePermanentEvents()
		{
			LazyWindowsStackController.OnWindowBecameVisibleInStack += this.OnWindowBecameVisibleInStack;
			LazyWindowsStackController.OnWindowBecameHiddenInStack += this.OnWindowBecameHiddenInStack;
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000278BC File Offset: 0x00025ABC
		protected virtual bool OnPressedBack()
		{
			if (this.closeButton)
			{
				this.Close();
				return true;
			}
			return false;
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000278D4 File Offset: 0x00025AD4
		protected virtual void UpdateGamepadDependentStuff()
		{
			LazyPlatformDependentElement[] componentsInChildren = base.GetComponentsInChildren<LazyPlatformDependentElement>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Init();
			}
			LazyGamepadDependentElement[] componentsInChildren2 = base.GetComponentsInChildren<LazyGamepadDependentElement>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].UpdateState();
			}
			if (LazyInput.IsGamepadActive)
			{
				if (this.closeButton)
				{
					this.closeButton.gameObject.SetActive(false);
				}
				this.PrintTips();
				this.ChangeTipsState(true);
				return;
			}
			if (this.closeButton)
			{
				this.closeButton.gameObject.SetActive(true);
			}
			this.ChangeTipsState(false);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00027978 File Offset: 0x00025B78
		protected virtual void PrintTips()
		{
			if (this.closeButton)
			{
				this.lazyButtonTips.Print(new LazyGameKeyTip[]
				{
					LazyGameKeyTip.Select(true, true, true),
					LazyGameKeyTip.Back(true, true, true)
				});
				return;
			}
			this.lazyButtonTips.Print(LazyGameKeyTip.Select(true, true, true));
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x000279CD File Offset: 0x00025BCD
		protected virtual void PrintTips(GamepadNavigationItem gamepadNavigationItem)
		{
			this.PrintTips();
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000279D5 File Offset: 0x00025BD5
		protected virtual void OnFocusedItemChanged(GamepadNavigationItem gamepadNavigationItem)
		{
			this.PrintTips(gamepadNavigationItem);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x000279DE File Offset: 0x00025BDE
		protected virtual void OnBecameVisibleInStack()
		{
			this.ChangeTipsState(true);
			this.lazyWindowInputController.Enable(true);
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x000279F3 File Offset: 0x00025BF3
		protected virtual void OnBecameHiddenInStack()
		{
			this.ChangeTipsState(false);
			this.lazyWindowInputController.Disable(true);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00027A08 File Offset: 0x00025C08
		protected virtual Dictionary<GameKey, Func<bool>> GetGameKeyDelegates()
		{
			return new Dictionary<GameKey, Func<bool>> { 
			{
				GameKey.Back,
				new Func<bool>(this.OnPressedBack)
			} };
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00027A27 File Offset: 0x00025C27
		protected virtual GamepadNavigationItem GetFocusedNavigationItem()
		{
			return null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00027A2A File Offset: 0x00025C2A
		private void InitInputController()
		{
			this.GamepadNavigationController.OnFocusedItemChanged += this.OnFocusedItemChanged;
			this.lazyWindowInputController = new LazyWindowInputController(this.GamepadNavigationController, this.GetGameKeyDelegates(), new Func<GamepadNavigationItem>(this.GetFocusedNavigationItem));
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00027A68 File Offset: 0x00025C68
		private void InitCanvas()
		{
			this.canvas = base.GetComponent<Canvas>();
			this.canvas.overrideSorting = true;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00027A84 File Offset: 0x00025C84
		private void InitButtonTipsStr()
		{
			LazyButtonTipsStr[] componentsInChildren = base.GetComponentsInChildren<LazyButtonTipsStr>(true);
			if (componentsInChildren.Length != 0)
			{
				foreach (LazyButtonTipsStr lazyButtonTipsStr in componentsInChildren)
				{
					if (lazyButtonTipsStr.gameObject.activeSelf)
					{
						this.lazyButtonTips = lazyButtonTipsStr;
						break;
					}
				}
				if (this.lazyButtonTips == null)
				{
					this.lazyButtonTips = componentsInChildren[0];
				}
			}
			if (this.lazyButtonTips != null)
			{
				this.lazyButtonTips.Clear();
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00027AF7 File Offset: 0x00025CF7
		private void OnInputChanged()
		{
			this.UpdateGamepadDependentStuff();
			if (this.IsShownAndTop)
			{
				this.lazyWindowInputController.UpdateGamepadDependentStuff();
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00027B12 File Offset: 0x00025D12
		private void OnWindowBecameVisibleInStack(LazyWidgetBase window)
		{
			if (window != this)
			{
				return;
			}
			if (!this.isShown)
			{
				return;
			}
			this.OnBecameVisibleInStack();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00027B2D File Offset: 0x00025D2D
		private void OnWindowBecameHiddenInStack(LazyWidgetBase window)
		{
			if (window != this)
			{
				return;
			}
			if (!this.isShown)
			{
				return;
			}
			this.OnBecameHiddenInStack();
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00027B48 File Offset: 0x00025D48
		protected void ChangeTipsState(bool active)
		{
			LazyButtonTipsStr lazyButtonTipsStr = this.lazyButtonTips;
			if (lazyButtonTipsStr == null)
			{
				return;
			}
			lazyButtonTipsStr.gameObject.SetActive(active && LazyInput.IsGamepadActive);
		}

		// Token: 0x040004C4 RID: 1220
		[Space]
		[SerializeField]
		protected bool isModalWindow = true;

		// Token: 0x040004C5 RID: 1221
		[Space]
		[SerializeField]
		protected LazyButton closeButton;

		// Token: 0x040004C6 RID: 1222
		[Space]
		[SerializeField]
		private string openSoundId;

		// Token: 0x040004C7 RID: 1223
		[SerializeField]
		private string closeSoundId;

		// Token: 0x040004C8 RID: 1224
		private GamepadNavigationController gamepadNavigationController;

		// Token: 0x040004C9 RID: 1225
		protected Action<T> OnClosed;

		// Token: 0x040004CA RID: 1226
		protected LazyButtonTipsStr lazyButtonTips;

		// Token: 0x040004CB RID: 1227
		protected LazyWindowInputController lazyWindowInputController;

		// Token: 0x040004CC RID: 1228
		protected Canvas canvas;

		// Token: 0x040004CD RID: 1229
		private bool isShown;
	}
}
