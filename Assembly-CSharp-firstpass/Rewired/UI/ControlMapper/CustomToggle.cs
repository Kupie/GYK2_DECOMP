using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000BC RID: 188
	[AddComponentMenu("")]
	public class CustomToggle : Toggle, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0001C9B3 File Offset: 0x0001ABB3
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0001C9BB File Offset: 0x0001ABBB
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

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0001C9C4 File Offset: 0x0001ABC4
		// (set) Token: 0x06000A0B RID: 2571 RVA: 0x0001C9CC File Offset: 0x0001ABCC
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

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0001C9D5 File Offset: 0x0001ABD5
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x0001C9DD File Offset: 0x0001ABDD
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

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0001C9E6 File Offset: 0x0001ABE6
		// (set) Token: 0x06000A0F RID: 2575 RVA: 0x0001C9EE File Offset: 0x0001ABEE
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

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0001C9F7 File Offset: 0x0001ABF7
		// (set) Token: 0x06000A11 RID: 2577 RVA: 0x0001C9FF File Offset: 0x0001ABFF
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

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0001CA08 File Offset: 0x0001AC08
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x0001CA10 File Offset: 0x0001AC10
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

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0001CA19 File Offset: 0x0001AC19
		// (set) Token: 0x06000A15 RID: 2581 RVA: 0x0001CA21 File Offset: 0x0001AC21
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

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0001C086 File Offset: 0x0001A286
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000A17 RID: 2583 RVA: 0x0001CA2C File Offset: 0x0001AC2C
		// (remove) Token: 0x06000A18 RID: 2584 RVA: 0x0001CA64 File Offset: 0x0001AC64
		private event UnityAction _CancelEvent;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000A19 RID: 2585 RVA: 0x0001CA99 File Offset: 0x0001AC99
		// (remove) Token: 0x06000A1A RID: 2586 RVA: 0x0001CAA2 File Offset: 0x0001ACA2
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

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001CAAC File Offset: 0x0001ACAC
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001CAEC File Offset: 0x0001ACEC
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0001CB2C File Offset: 0x0001AD2C
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0001CB6C File Offset: 0x0001AD6C
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0001CBAB File Offset: 0x0001ADAB
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0001CBDC File Offset: 0x0001ADDC
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

		// Token: 0x06000A21 RID: 2593 RVA: 0x0001CC64 File Offset: 0x0001AE64
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0001C30E File Offset: 0x0001A50E
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0001CCA8 File Offset: 0x0001AEA8
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0001CD16 File Offset: 0x0001AF16
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0001CD26 File Offset: 0x0001AF26
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0001CD38 File Offset: 0x0001AF38
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

		// Token: 0x06000A27 RID: 2599 RVA: 0x0001CD8A File Offset: 0x0001AF8A
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x040004EB RID: 1259
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x040004EC RID: 1260
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x040004ED RID: 1261
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x040004EE RID: 1262
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x040004EF RID: 1263
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x040004F0 RID: 1264
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x040004F1 RID: 1265
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x040004F2 RID: 1266
		private bool isHighlightDisabled;
	}
}
