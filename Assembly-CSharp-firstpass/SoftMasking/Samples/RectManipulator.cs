using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x0200000C RID: 12
	[RequireComponent(typeof(RectTransform))]
	public class RectManipulator : UIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x06000026 RID: 38 RVA: 0x000026DA File Offset: 0x000008DA
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.HighlightIcon(true, false);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000026E4 File Offset: 0x000008E4
		public void OnPointerExit(PointerEventData eventData)
		{
			if (!this._isManipulatedNow)
			{
				this.HighlightIcon(false, false);
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000026F8 File Offset: 0x000008F8
		private void HighlightIcon(bool highlight, bool instant = false)
		{
			if (this.icon)
			{
				float num = (highlight ? this.selectedAlpha : this.normalAlpha);
				float num2 = (instant ? 0f : this.transitionDuration);
				this.icon.CrossFadeAlpha(num, num2, true);
			}
			if (this.showOnHover)
			{
				this.showOnHover.forcedVisible = highlight;
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000275C File Offset: 0x0000095C
		protected override void Start()
		{
			base.Start();
			this.HighlightIcon(false, true);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000276C File Offset: 0x0000096C
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._isManipulatedNow = true;
			this.RememberStartTransform();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000277C File Offset: 0x0000097C
		private void RememberStartTransform()
		{
			if (this.targetTransform)
			{
				this._startAnchoredPosition = this.targetTransform.anchoredPosition;
				this._startSizeDelta = this.targetTransform.sizeDelta;
				this._startRotation = this.targetTransform.localRotation.eulerAngles.z;
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000027D8 File Offset: 0x000009D8
		public void OnDrag(PointerEventData eventData)
		{
			if (this.targetTransform == null || this.parentTransform == null || !this._isManipulatedNow)
			{
				return;
			}
			Vector2 vector = this.ToParentSpace(eventData.pressPosition, eventData.pressEventCamera);
			Vector2 vector2 = this.ToParentSpace(eventData.position, eventData.pressEventCamera);
			this.DoRotate(vector, vector2);
			Vector2 vector3 = vector2 - vector;
			this.DoMove(vector3);
			this.DoResize(vector3);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002850 File Offset: 0x00000A50
		private Vector2 ToParentSpace(Vector2 position, Camera eventCamera)
		{
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentTransform, position, eventCamera, out vector);
			return vector;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002E RID: 46 RVA: 0x0000286E File Offset: 0x00000A6E
		private RectTransform parentTransform
		{
			get
			{
				return this.targetTransform.parent as RectTransform;
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002880 File Offset: 0x00000A80
		private void DoMove(Vector2 parentSpaceMovement)
		{
			if (this.Is(RectManipulator.ManipulationType.Move))
			{
				this.MoveTo(this._startAnchoredPosition + parentSpaceMovement);
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000289D File Offset: 0x00000A9D
		private bool Is(RectManipulator.ManipulationType expected)
		{
			return (this.manipulation & expected) == expected;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000028AA File Offset: 0x00000AAA
		private void MoveTo(Vector2 desiredAnchoredPosition)
		{
			this.targetTransform.anchoredPosition = this.ClampPosition(desiredAnchoredPosition);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000028C0 File Offset: 0x00000AC0
		private Vector2 ClampPosition(Vector2 position)
		{
			Vector2 vector = this.parentTransform.rect.size / 2f;
			return new Vector2(Mathf.Clamp(position.x, -vector.x, vector.x), Mathf.Clamp(position.y, -vector.y, vector.y));
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002920 File Offset: 0x00000B20
		private void DoRotate(Vector2 startParentPoint, Vector2 targetParentPoint)
		{
			if (this.Is(RectManipulator.ManipulationType.Rotate))
			{
				Vector2 vector = startParentPoint - this.targetTransform.localPosition;
				Vector2 vector2 = targetParentPoint - this.targetTransform.localPosition;
				float num = this.DeltaRotation(vector, vector2);
				this.targetTransform.localRotation = Quaternion.AngleAxis(this._startRotation + num, Vector3.forward);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000298C File Offset: 0x00000B8C
		private float DeltaRotation(Vector2 startLever, Vector2 endLever)
		{
			float num = Mathf.Atan2(startLever.y, startLever.x) * 57.29578f;
			float num2 = Mathf.Atan2(endLever.y, endLever.x) * 57.29578f;
			return Mathf.DeltaAngle(num, num2);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000029D0 File Offset: 0x00000BD0
		private void DoResize(Vector2 parentSpaceMovement)
		{
			Vector3 vector = Quaternion.Inverse(this.targetTransform.rotation) * parentSpaceMovement;
			Vector2 vector2 = this.ProjectResizeOffset(vector);
			if (vector2.sqrMagnitude > 0f)
			{
				this.SetSizeDirected(vector2, this.ResizeSign());
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002A24 File Offset: 0x00000C24
		private Vector2 ProjectResizeOffset(Vector2 localOffset)
		{
			bool flag = this.Is(RectManipulator.ManipulationType.ResizeLeft) || this.Is(RectManipulator.ManipulationType.ResizeRight);
			bool flag2 = this.Is(RectManipulator.ManipulationType.ResizeUp) || this.Is(RectManipulator.ManipulationType.ResizeDown);
			return new Vector2(flag ? localOffset.x : 0f, flag2 ? localOffset.y : 0f);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002A7D File Offset: 0x00000C7D
		private Vector2 ResizeSign()
		{
			return new Vector2(this.Is(RectManipulator.ManipulationType.ResizeLeft) ? (-1f) : 1f, this.Is(RectManipulator.ManipulationType.ResizeDown) ? (-1f) : 1f);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002AB0 File Offset: 0x00000CB0
		private void SetSizeDirected(Vector2 localOffset, Vector2 sizeSign)
		{
			Vector2 vector = this.ClampSize(this._startSizeDelta + Vector2.Scale(localOffset, sizeSign));
			this.targetTransform.sizeDelta = vector;
			Vector2 vector2 = Vector2.Scale((vector - this._startSizeDelta) / 2f, sizeSign);
			this.MoveTo(this._startAnchoredPosition + this.targetTransform.TransformVector(vector2));
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B26 File Offset: 0x00000D26
		private Vector2 ClampSize(Vector2 size)
		{
			return new Vector2(Mathf.Max(size.x, this.minSize.x), Mathf.Max(size.y, this.minSize.y));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B59 File Offset: 0x00000D59
		public void OnEndDrag(PointerEventData eventData)
		{
			this._isManipulatedNow = false;
			if (!eventData.hovered.Contains(base.gameObject))
			{
				this.HighlightIcon(false, false);
			}
		}

		// Token: 0x04000024 RID: 36
		public RectTransform targetTransform;

		// Token: 0x04000025 RID: 37
		public RectManipulator.ManipulationType manipulation;

		// Token: 0x04000026 RID: 38
		public ShowOnHover showOnHover;

		// Token: 0x04000027 RID: 39
		[Header("Limits")]
		public Vector2 minSize;

		// Token: 0x04000028 RID: 40
		[Header("Display")]
		public Graphic icon;

		// Token: 0x04000029 RID: 41
		public float normalAlpha = 0.2f;

		// Token: 0x0400002A RID: 42
		public float selectedAlpha = 1f;

		// Token: 0x0400002B RID: 43
		public float transitionDuration = 0.2f;

		// Token: 0x0400002C RID: 44
		private bool _isManipulatedNow;

		// Token: 0x0400002D RID: 45
		private Vector2 _startAnchoredPosition;

		// Token: 0x0400002E RID: 46
		private Vector2 _startSizeDelta;

		// Token: 0x0400002F RID: 47
		private float _startRotation;

		// Token: 0x0200000D RID: 13
		[Flags]
		public enum ManipulationType
		{
			// Token: 0x04000031 RID: 49
			None = 0,
			// Token: 0x04000032 RID: 50
			Move = 1,
			// Token: 0x04000033 RID: 51
			ResizeLeft = 2,
			// Token: 0x04000034 RID: 52
			ResizeUp = 4,
			// Token: 0x04000035 RID: 53
			ResizeRight = 8,
			// Token: 0x04000036 RID: 54
			ResizeDown = 16,
			// Token: 0x04000037 RID: 55
			ResizeUpLeft = 6,
			// Token: 0x04000038 RID: 56
			ResizeUpRight = 12,
			// Token: 0x04000039 RID: 57
			ResizeDownLeft = 18,
			// Token: 0x0400003A RID: 58
			ResizeDownRight = 24,
			// Token: 0x0400003B RID: 59
			Rotate = 32
		}
	}
}
