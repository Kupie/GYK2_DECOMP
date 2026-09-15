using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000B9 RID: 185
	[AddComponentMenu("")]
	public class CustomButton : Button, ICustomSelectable, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0001C00F File Offset: 0x0001A20F
		// (set) Token: 0x060009BD RID: 2493 RVA: 0x0001C017 File Offset: 0x0001A217
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

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0001C020 File Offset: 0x0001A220
		// (set) Token: 0x060009BF RID: 2495 RVA: 0x0001C028 File Offset: 0x0001A228
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

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x0001C031 File Offset: 0x0001A231
		// (set) Token: 0x060009C1 RID: 2497 RVA: 0x0001C039 File Offset: 0x0001A239
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

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0001C042 File Offset: 0x0001A242
		// (set) Token: 0x060009C3 RID: 2499 RVA: 0x0001C04A File Offset: 0x0001A24A
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

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060009C4 RID: 2500 RVA: 0x0001C053 File Offset: 0x0001A253
		// (set) Token: 0x060009C5 RID: 2501 RVA: 0x0001C05B File Offset: 0x0001A25B
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

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x0001C064 File Offset: 0x0001A264
		// (set) Token: 0x060009C7 RID: 2503 RVA: 0x0001C06C File Offset: 0x0001A26C
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

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0001C075 File Offset: 0x0001A275
		// (set) Token: 0x060009C9 RID: 2505 RVA: 0x0001C07D File Offset: 0x0001A27D
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

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0001C086 File Offset: 0x0001A286
		private bool isDisabled
		{
			get
			{
				return !this.IsInteractable();
			}
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060009CB RID: 2507 RVA: 0x0001C094 File Offset: 0x0001A294
		// (remove) Token: 0x060009CC RID: 2508 RVA: 0x0001C0CC File Offset: 0x0001A2CC
		private event UnityAction _CancelEvent;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060009CD RID: 2509 RVA: 0x0001C101 File Offset: 0x0001A301
		// (remove) Token: 0x060009CE RID: 2510 RVA: 0x0001C10A File Offset: 0x0001A30A
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

		// Token: 0x060009CF RID: 2511 RVA: 0x0001C114 File Offset: 0x0001A314
		public override Selectable FindSelectableOnLeft()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.left);
			}
			return base.FindSelectableOnLeft();
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0001C154 File Offset: 0x0001A354
		public override Selectable FindSelectableOnRight()
		{
			if ((base.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.right);
			}
			return base.FindSelectableOnRight();
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0001C194 File Offset: 0x0001A394
		public override Selectable FindSelectableOnUp()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.up);
			}
			return base.FindSelectableOnUp();
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0001C1D4 File Offset: 0x0001A3D4
		public override Selectable FindSelectableOnDown()
		{
			if ((base.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown)
			{
				return UISelectionUtility.FindNextSelectable(this, base.transform, Vector3.down);
			}
			return base.FindSelectableOnDown();
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0001C213 File Offset: 0x0001A413
		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			if (EventSystem.current == null)
			{
				return;
			}
			this.EvaluateHightlightDisabled(EventSystem.current.currentSelectedGameObject == base.gameObject);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0001C244 File Offset: 0x0001A444
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

		// Token: 0x060009D5 RID: 2517 RVA: 0x0001C2CC File Offset: 0x0001A4CC
		private void StartColorTween(Color targetColor, bool instant)
		{
			if (base.targetGraphic == null)
			{
				return;
			}
			base.targetGraphic.CrossFadeColor(targetColor, instant ? 0f : base.colors.fadeDuration, true, true);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0001C30E File Offset: 0x0001A50E
		private void DoSpriteSwap(Sprite newSprite)
		{
			if (base.image == null)
			{
				return;
			}
			base.image.overrideSprite = newSprite;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0001C32C File Offset: 0x0001A52C
		private void TriggerAnimation(string triggername)
		{
			if (base.animator == null || !base.animator.enabled || !base.animator.isActiveAndEnabled || base.animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
			{
				return;
			}
			base.animator.ResetTrigger(this._disabledHighlightedTrigger);
			base.animator.SetTrigger(triggername);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0001C39A File Offset: 0x0001A59A
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			this.EvaluateHightlightDisabled(true);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0001C3AA File Offset: 0x0001A5AA
		public override void OnDeselect(BaseEventData eventData)
		{
			base.OnDeselect(eventData);
			this.EvaluateHightlightDisabled(false);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0001C3BA File Offset: 0x0001A5BA
		private void Press()
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			base.onClick.Invoke();
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0001C3D8 File Offset: 0x0001A5D8
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (!this.IsActive() || !this.IsInteractable())
			{
				return;
			}
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
			}
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0001C424 File Offset: 0x0001A624
		public override void OnSubmit(BaseEventData eventData)
		{
			this.Press();
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isHighlightDisabled = true;
				this.DoStateTransition(Selectable.SelectionState.Disabled, false);
				return;
			}
			this.DoStateTransition(Selectable.SelectionState.Pressed, false);
			base.StartCoroutine(this.OnFinishSubmit());
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x0001C461 File Offset: 0x0001A661
		private IEnumerator OnFinishSubmit()
		{
			float fadeTime = base.colors.fadeDuration;
			float elapsedTime = 0f;
			while (elapsedTime < fadeTime)
			{
				elapsedTime += Time.unscaledDeltaTime;
				yield return null;
			}
			this.DoStateTransition(base.currentSelectionState, false);
			yield break;
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x0001C470 File Offset: 0x0001A670
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

		// Token: 0x060009DF RID: 2527 RVA: 0x0001C4C2 File Offset: 0x0001A6C2
		public void OnCancel(BaseEventData eventData)
		{
			if (this._CancelEvent != null)
			{
				this._CancelEvent();
			}
		}

		// Token: 0x040004D4 RID: 1236
		[SerializeField]
		private Sprite _disabledHighlightedSprite;

		// Token: 0x040004D5 RID: 1237
		[SerializeField]
		private Color _disabledHighlightedColor;

		// Token: 0x040004D6 RID: 1238
		[SerializeField]
		private string _disabledHighlightedTrigger;

		// Token: 0x040004D7 RID: 1239
		[SerializeField]
		private bool _autoNavUp = true;

		// Token: 0x040004D8 RID: 1240
		[SerializeField]
		private bool _autoNavDown = true;

		// Token: 0x040004D9 RID: 1241
		[SerializeField]
		private bool _autoNavLeft = true;

		// Token: 0x040004DA RID: 1242
		[SerializeField]
		private bool _autoNavRight = true;

		// Token: 0x040004DB RID: 1243
		private bool isHighlightDisabled;
	}
}
