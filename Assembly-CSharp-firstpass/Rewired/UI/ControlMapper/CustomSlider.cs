using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000BB RID: 187
	[AddComponentMenu("")]
	public class CustomSlider : Slider, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0001C5A5 File Offset: 0x0001A7A5
		// (set) Token: 0x060009E8 RID: 2536 RVA: 0x0001C5AD File Offset: 0x0001A7AD
		public Sprite disabledHighlightedSprite
		{
			get
			{
				return this._disabledHighlightedSprite;
			}
			set
			{
				this._disabledHighlightedSprite = value;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0001C5B6 File Offset: 0x0001A7B6
		// (set) Token: 0x060009EA RID: 2538 RVA: 0x0001C5BE File Offset: 0x0001A7BE
		public Color disabledHighlightedColor
		{
			get
			{
				return this._disabledHighlightedColor;
			}
			set
			{
				this._disabledHighlightedColor = value;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060009EB RID: 2539 RVA: 0x0001C5C7 File Offset: 0x0001A7C7
		// (set) Token: 0x060009EC RID: 2540 RVA: 0x0001C5CF File Offset: 0x0001A7CF
		public string disabledHighlightedTrigger
		{
			get
			{
				return this._disabledHighlightedTrigger;
			}
			set
			{
				this._disabledHighlightedTrigger = value;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060009ED RID: 2541 RVA: 0x0001C5D8 File Offset: 0x0001A7D8
		// (set) Token: 0x060009EE RID: 2542 RVA: 0x0001C5E0 File Offset: 0x0001A7E0
		public bool autoNavUp
		{
			get
			{
				return this._autoNavUp;
			}
			set
			{
				this._autoNavUp = value;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0001C5E9 File Offset: 0x0001A7E9
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x0001C5F1 File Offset: 0x0001A7F1
		public bool autoNavDown
		{
			get
			{
				return this._autoNavDown;
			}
			set
			{
				this._autoNavDown = value;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x0001C5FA File Offset: 0x0001A7FA
		// (set) Token: 0x060009F2 RID: 2546 RVA: 0x0001C602 File Offset: 0x0001A802
		public bool autoNavLeft
		{
			get
			{
				return this._autoNavLeft;
			}
			set
			{
				this._autoNavLeft = value;
			}
		}

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0001C60B File Offset: 0x0001A80B
		// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0001C613 File Offset: 0x0001A813
		public bool autoNavRight
		{
			get
			{
				return this._autoNavRight;
			}
			set
			{
				this._autoNavRight = value;
			}
		}

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0001C086 File Offset: 0x0001A286
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060009F6 RID: 2550 RVA: 0x0001C61C File Offset: 0x0001A81C
		// (remove) Token: 0x060009F7 RID: 2551 RVA: 0x0001C654 File Offset: 0x0001A854
		private event UnityAction _CancelEvent;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060009F8 RID: 2552 RVA: 0x0001C689 File Offset: 0x0001A889
		// (remove) Token: 0x060009F9 RID: 2553 RVA: 0x0001C692 File Offset: 0x0001A892
		public event UnityAction CancelEvent
		{
			add
			{
				this._CancelEvent += value;
			}
			remove
			{
				this._CancelEvent -= value;
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0001C69C File Offset: 0x0001A89C
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0001C6DC File Offset: 0x0001A8DC
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0001C71C File Offset: 0x0001A91C
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0001C75C File Offset: 0x0001A95C
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0001C79B File Offset: 0x0001A99B
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0001C7CC File Offset: 0x0001A9CC
		protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
		{
			if (this.isHighlightDisabled)
			{
				Color disabledHighlightedColor = this._disabledHighlightedColor;
				Sprite disabledHighlightedSprite = this._disabledHighlightedSprite;
				string disabledHighlightedTrigger = this._disabledHighlightedTrigger;
				if (base.gameObject.activeInHierarchy)
				{
					switch (base.transition)
					{
					case Selectable.Transition.ColorTint:
						this.StartColorTween(disabledHighlightedColor * base.colors.colorMultiplier, instant);
						return;
					case Selectable.Transition.SpriteSwap:
						this.DoSpriteSwap(disabledHighlightedSprite);
						return;
					case Selectable.Transition.Animation:
						this.TriggerAnimation(disabledHighlightedTrigger);
						return;
					default:
						return;
					}
				}
			}
			else
			{
				base.DoStateTransition(state, instant);
			}
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0001C854 File Offset: 0x0001AA54
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001C30E File Offset: 0x0001A50E
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001C898 File Offset: 0x0001AA98
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001C906 File Offset: 0x0001AB06
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0001C916 File Offset: 0x0001AB16
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0001C928 File Offset: 0x0001AB28
		private void EvaluateHightlightDisabled(bool isSelected)
		{
			if (!isSelected)
			{
				if (this.isHighlightDisabled)
				{
					this.isHighlightDisabled = false;
					Selectable.SelectionState selectionState = (this.isDisabled ? Selectable.SelectionState.Disabled : base.currentSelectionState);
					this.DoStateTransition(selectionState, false);
					return;
				}
			}
			else
			{
				if (!this.isDisabled)
				{
					return;
				}
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0001C97A File Offset: 0x0001AB7A
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x040004E2 RID: 1250
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x040004E3 RID: 1251
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x040004E4 RID: 1252
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x040004E5 RID: 1253
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x040004E6 RID: 1254
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x040004E7 RID: 1255
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x040004E8 RID: 1256
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x040004E9 RID: 1257
		private bool isHighlightDisabled;
	}
}
