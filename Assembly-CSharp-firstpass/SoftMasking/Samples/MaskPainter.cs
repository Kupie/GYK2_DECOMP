using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking.Samples
{
	// Token: 0x02000009 RID: 9
	[RequireComponent(typeof(RectTransform))]
	public class MaskPainter : UIBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000243C File Offset: 0x0000063C
		protected override void Awake()
		{
			base.Awake();
			this._rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002450 File Offset: 0x00000650
		protected override void Start()
		{
			base.Start();
			this.spot.enabled = false;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002464 File Offset: 0x00000664
		public void OnPointerDown(PointerEventData eventData)
		{
			this.UpdateStrokeByEvent(eventData, false);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000246E File Offset: 0x0000066E
		public void OnDrag(PointerEventData eventData)
		{
			this.UpdateStrokeByEvent(eventData, true);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002478 File Offset: 0x00000678
		private void UpdateStrokeByEvent(PointerEventData eventData, bool drawTrail = false)
		{
			this.UpdateStrokePosition(eventData.position, drawTrail);
			this.UpdateStrokeColor(eventData.button);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002494 File Offset: 0x00000694
		private void UpdateStrokePosition(Vector2 screenPosition, bool drawTrail = false)
		{
			Vector2 vector;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._rectTransform, screenPosition, null, out vector))
			{
				Vector2 anchoredPosition = this.stroke.anchoredPosition;
				this.stroke.anchoredPosition = vector;
				if (drawTrail)
				{
					this.SetUpTrail(anchoredPosition);
				}
				this.spot.enabled = true;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000024E0 File Offset: 0x000006E0
		private void SetUpTrail(Vector2 prevPosition)
		{
			Vector2 vector = this.stroke.anchoredPosition - prevPosition;
			this.stroke.localRotation = Quaternion.AngleAxis(Mathf.Atan2(vector.y, vector.x) * 57.29578f, Vector3.forward);
			this.spot.rectTransform.offsetMin = new Vector2(-vector.magnitude, 0f);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000254D File Offset: 0x0000074D
		private void UpdateStrokeColor(PointerEventData.InputButton pressedButton)
		{
			this.spot.materialForRendering.SetInt("_BlendOp", (pressedButton == PointerEventData.InputButton.Left) ? 0 : 2);
		}

		// Token: 0x04000018 RID: 24
		public Graphic spot;

		// Token: 0x04000019 RID: 25
		public RectTransform stroke;

		// Token: 0x0400001A RID: 26
		private RectTransform _rectTransform;
	}
}
