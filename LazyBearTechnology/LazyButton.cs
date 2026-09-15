using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x02000151 RID: 337
	[DefaultExecutionOrder(-10000)]
	public class LazyButton : Button, ILazyUIElementWithId, ISerializationCallbackReceiver
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x000245E9 File Offset: 0x000227E9
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x000245F1 File Offset: 0x000227F1
		public string LazyUIElementId
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x000245FA File Offset: 0x000227FA
		public MonoBehaviour MonoBehaviour
		{
			get
			{
				return this;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000717 RID: 1815 RVA: 0x000245FD File Offset: 0x000227FD
		public bool IsPointerHeld
		{
			get
			{
				return this.pointerHeld;
			}
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00024605 File Offset: 0x00022805
		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00024607 File Offset: 0x00022807
		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			this.SanitizeEmptyEvents();
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00024610 File Offset: 0x00022810
		public static void RefreshAll()
		{
			LazyButton[] array = global::UnityEngine.Object.FindObjectsByType<LazyButton>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			for (int i = 0; i < array.Length; i++)
			{
				array[i].RefreshTextTransitions();
			}
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002463B File Offset: 0x0002283B
		public void RefreshTextTransitions()
		{
			this.ApplyTextTransitionsForState(base.currentSelectionState);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00024649 File Offset: 0x00022849
		protected override void Awake()
		{
			this.SanitizeEmptyEvents();
			base.Awake();
			if (Application.isPlaying && !string.IsNullOrEmpty(this.id))
			{
				LazyUIElementManager.TryRegisterElement(this);
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x00024674 File Offset: 0x00022874
		private void SanitizeEmptyEvents()
		{
			if (this.eventsSanitized)
			{
				return;
			}
			this.eventsSanitized = true;
			this.onDown = LazyButton.RecreateIfEmpty(this.onDown);
			this.onNotInteractableDown = LazyButton.RecreateIfEmpty(this.onNotInteractableDown);
			this.onUp = LazyButton.RecreateIfEmpty(this.onUp);
			this.onNotInteractableUp = LazyButton.RecreateIfEmpty(this.onNotInteractableUp);
			this.onEnter = LazyButton.RecreateIfEmpty(this.onEnter);
			this.onNotInteractableEnter = LazyButton.RecreateIfEmpty(this.onNotInteractableEnter);
			this.onExit = LazyButton.RecreateIfEmpty(this.onExit);
			this.onNotInteractableExit = LazyButton.RecreateIfEmpty(this.onNotInteractableExit);
			this.onNotInteractableClick = LazyButton.RecreateIfEmpty(this.onNotInteractableClick);
			if (base.onClick == null || base.onClick.GetPersistentEventCount() == 0)
			{
				base.onClick = new Button.ButtonClickedEvent();
			}
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002474A File Offset: 0x0002294A
		private static LazyUIEvent RecreateIfEmpty(LazyUIEvent evt)
		{
			if (evt == null || evt.GetPersistentEventCount() == 0)
			{
				return new LazyUIEvent();
			}
			return evt;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002475E File Offset: 0x0002295E
		protected override void OnDisable()
		{
			this.pointerHeld = false;
			base.OnDisable();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002476D File Offset: 0x0002296D
		protected override void OnDestroy()
		{
			if (Application.isPlaying && !string.IsNullOrEmpty(this.id))
			{
				LazyUIElementManager.TryUnregisterElement(this);
			}
			base.OnDestroy();
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x00024790 File Offset: 0x00022990
		public void SetKeepPressed(bool keepPressed)
		{
			this.keepPressed = keepPressed;
			this.DoStateTransition(keepPressed ? Selectable.SelectionState.Pressed : base.currentSelectionState, true);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x000247AC File Offset: 0x000229AC
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.keepPressed)
			{
				state = Selectable.SelectionState.Pressed;
			}
			base.DoStateTransition(state, instant);
			foreach (IconTransition iconTransition in this.iconTransitions)
			{
				if (!(iconTransition.targetImage == null))
				{
					Sprite sprite = null;
					switch (state)
					{
					case Selectable.SelectionState.Normal:
						sprite = iconTransition.defaultSprite;
						break;
					case Selectable.SelectionState.Highlighted:
						sprite = iconTransition.highlightedSprite;
						break;
					case Selectable.SelectionState.Pressed:
						sprite = iconTransition.pressedSprite;
						break;
					case Selectable.SelectionState.Selected:
						sprite = iconTransition.selectedSprite;
						break;
					case Selectable.SelectionState.Disabled:
						sprite = iconTransition.disabledSprite;
						break;
					}
					if (sprite != null)
					{
						iconTransition.targetImage.overrideSprite = null;
						iconTransition.targetImage.sprite = sprite;
						iconTransition.targetImage.SetNativeSize();
					}
				}
			}
			this.ApplyTextTransitionsForState(state);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002489C File Offset: 0x00022A9C
		private void ApplyTextTransitionsForState(Selectable.SelectionState state)
		{
			foreach (TextTransition textTransition in this.textTransitions)
			{
				if (!(textTransition.targetLabel == null))
				{
					TextStyle textStyle = null;
					switch (state)
					{
					case Selectable.SelectionState.Normal:
						textStyle = textTransition.defaultStyle;
						break;
					case Selectable.SelectionState.Highlighted:
						textStyle = textTransition.highlightedStyle;
						break;
					case Selectable.SelectionState.Pressed:
						textStyle = textTransition.pressedStyle;
						break;
					case Selectable.SelectionState.Selected:
						textStyle = textTransition.selectedStyle;
						break;
					case Selectable.SelectionState.Disabled:
						textStyle = textTransition.disabledStyle;
						break;
					}
					if (textStyle != null)
					{
						textStyle.ApplyStyle(textTransition.targetLabel, false, null, null, null);
					}
				}
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x00024978 File Offset: 0x00022B78
		public override void OnPointerDown(PointerEventData eventData)
		{
			if (base.interactable)
			{
				this.pointerHeld = true;
				this.onDown.Invoke();
				this.PlaySound(this.onDownSound);
			}
			else
			{
				this.onNotInteractableDown.Invoke();
				this.PlaySound(this.onNotInteractableDownSound);
			}
			base.OnPointerDown(eventData);
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x000249CC File Offset: 0x00022BCC
		public override void OnPointerUp(PointerEventData eventData)
		{
			this.pointerHeld = false;
			if (base.interactable)
			{
				this.onUp.Invoke();
				this.PlaySound(this.onUpSound);
			}
			else
			{
				this.onNotInteractableUp.Invoke();
				this.PlaySound(this.onNotInteractableUpSound);
			}
			base.OnPointerUp(eventData);
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00024A20 File Offset: 0x00022C20
		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (base.interactable)
			{
				this.onEnter.Invoke();
				this.PlaySound(this.onEnterSound);
			}
			else
			{
				this.onNotInteractableEnter.Invoke();
				this.PlaySound(this.onNotInteractableEnterSound);
			}
			base.OnPointerEnter(eventData);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00024A6C File Offset: 0x00022C6C
		public override void OnPointerExit(PointerEventData eventData)
		{
			if (!eventData.fullyExited)
			{
				return;
			}
			if (base.interactable)
			{
				this.onExit.Invoke();
				this.PlaySound(this.onExitSound);
			}
			else
			{
				this.onNotInteractableExit.Invoke();
				this.PlaySound(this.onNotInteractableExitSound);
			}
			base.OnPointerExit(eventData);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00024AC1 File Offset: 0x00022CC1
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!base.interactable)
			{
				this.PlaySound(this.onNotInteractableClickSound);
				this.onNotInteractableClick.Invoke();
			}
			else
			{
				this.PlaySound(this.onClickSound);
			}
			base.OnPointerClick(eventData);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00024AF8 File Offset: 0x00022CF8
		public void ForceOnEnter()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
			{
				pointerEnter = base.gameObject
			};
			this.OnPointerEnter(pointerEventData);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00024B24 File Offset: 0x00022D24
		public void ForceOnExit()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
			{
				fullyExited = true,
				pointerEnter = base.gameObject
			};
			this.OnPointerExit(pointerEventData);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00024B58 File Offset: 0x00022D58
		public void ForceOnClick()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
			{
				pointerClick = base.gameObject,
				pointerEnter = base.gameObject
			};
			this.OnPointerClick(pointerEventData);
		}

		// Token: 0x0600072C RID: 1836 RVA: 0x00024B90 File Offset: 0x00022D90
		public void SetCallbacksIntoGamepadNavigationItem()
		{
			GamepadNavigationItem gamepadNavigationItem;
			if (base.TryGetComponent<GamepadNavigationItem>(out gamepadNavigationItem))
			{
				gamepadNavigationItem.SetCallbacks(new UnityAction(this.ForceOnEnter), new UnityAction(this.ForceOnExit), new UnityAction(this.ForceOnClick));
			}
		}

		// Token: 0x0600072D RID: 1837 RVA: 0x00024BD1 File Offset: 0x00022DD1
		private void PlaySound(string sound)
		{
			if (!string.IsNullOrEmpty(sound))
			{
				LazyAudio.PlayAndForget(sound);
			}
		}

		// Token: 0x0400044A RID: 1098
		[SerializeField]
		private string id;

		// Token: 0x0400044B RID: 1099
		[NonSerialized]
		private bool eventsSanitized;

		// Token: 0x0400044C RID: 1100
		[NonSerialized]
		private bool keepPressed;

		// Token: 0x0400044D RID: 1101
		[NonSerialized]
		private bool pointerHeld;

		// Token: 0x0400044E RID: 1102
		[SerializeField]
		private List<IconTransition> iconTransitions = new List<IconTransition>();

		// Token: 0x0400044F RID: 1103
		[SerializeField]
		private List<TextTransition> textTransitions = new List<TextTransition>();

		// Token: 0x04000450 RID: 1104
		public LazyUIEvent onDown = new LazyUIEvent();

		// Token: 0x04000451 RID: 1105
		public LazyUIEvent onNotInteractableDown = new LazyUIEvent();

		// Token: 0x04000452 RID: 1106
		public LazyUIEvent onUp = new LazyUIEvent();

		// Token: 0x04000453 RID: 1107
		public LazyUIEvent onNotInteractableUp = new LazyUIEvent();

		// Token: 0x04000454 RID: 1108
		public LazyUIEvent onEnter = new LazyUIEvent();

		// Token: 0x04000455 RID: 1109
		public LazyUIEvent onNotInteractableEnter = new LazyUIEvent();

		// Token: 0x04000456 RID: 1110
		public LazyUIEvent onExit = new LazyUIEvent();

		// Token: 0x04000457 RID: 1111
		public LazyUIEvent onNotInteractableExit = new LazyUIEvent();

		// Token: 0x04000458 RID: 1112
		public LazyUIEvent onNotInteractableClick = new LazyUIEvent();

		// Token: 0x04000459 RID: 1113
		public string onDownSound;

		// Token: 0x0400045A RID: 1114
		public string onNotInteractableDownSound;

		// Token: 0x0400045B RID: 1115
		public string onUpSound;

		// Token: 0x0400045C RID: 1116
		public string onNotInteractableUpSound;

		// Token: 0x0400045D RID: 1117
		public string onEnterSound;

		// Token: 0x0400045E RID: 1118
		public string onNotInteractableEnterSound;

		// Token: 0x0400045F RID: 1119
		public string onExitSound;

		// Token: 0x04000460 RID: 1120
		public string onNotInteractableExitSound;

		// Token: 0x04000461 RID: 1121
		public string onClickSound;

		// Token: 0x04000462 RID: 1122
		public string onNotInteractableClickSound;
	}
}
