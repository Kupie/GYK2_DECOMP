using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x020000CA RID: 202
	[AddComponentMenu("")]
	[RequireComponent(typeof(Selectable))]
	public class ScrollRectSelectableChild : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0001E010 File Offset: 0x0001C210
		private RectTransform parentScrollRectContentTransform
		{
			get
			{
				return this.parentScrollRect.content;
			}
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x0001E020 File Offset: 0x0001C220
		private Selectable selectable
		{
			get
			{
				Selectable selectable;
				if ((selectable = this._selectable) == null)
				{
					selectable = (this._selectable = base.GetComponent<Selectable>());
				}
				return selectable;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x0001E046 File Offset: 0x0001C246
		private RectTransform rectTransform
		{
			get
			{
				return base.transform as RectTransform;
			}
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0001E053 File Offset: 0x0001C253
		private void Start()
		{
			this.parentScrollRect = base.transform.GetComponentInParent<ScrollRect>();
			if (this.parentScrollRect == null)
			{
				Debug.LogError("Rewired Control Mapper: No ScrollRect found! This component must be a child of a ScrollRect!");
				return;
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x0001E080 File Offset: 0x0001C280
		public void OnSelect(BaseEventData eventData)
		{
			if (this.parentScrollRect == null)
			{
				return;
			}
			if (!(eventData is AxisEventData))
			{
				return;
			}
			RectTransform rectTransform = this.parentScrollRect.transform as RectTransform;
			Rect rect = MathTools.TransformRect(this.rectTransform.rect, this.rectTransform, rectTransform);
			Rect rect2 = rectTransform.rect;
			Rect rect3 = rectTransform.rect;
			float height;
			if (this.useCustomEdgePadding)
			{
				height = this.customEdgePadding;
			}
			else
			{
				height = rect.height;
			}
			rect3.yMax -= height;
			rect3.yMin += height;
			if (MathTools.RectContains(rect3, rect))
			{
				return;
			}
			Vector2 vector;
			if (!MathTools.GetOffsetToContainRect(rect3, rect, out vector))
			{
				return;
			}
			Vector2 anchoredPosition = this.parentScrollRectContentTransform.anchoredPosition;
			anchoredPosition.x = Mathf.Clamp(anchoredPosition.x + vector.x, 0f, Mathf.Abs(rect2.width - this.parentScrollRectContentTransform.sizeDelta.x));
			anchoredPosition.y = Mathf.Clamp(anchoredPosition.y + vector.y, 0f, Mathf.Abs(rect2.height - this.parentScrollRectContentTransform.sizeDelta.y));
			this.parentScrollRectContentTransform.anchoredPosition = anchoredPosition;
		}

		// Token: 0x0400055D RID: 1373
		public bool useCustomEdgePadding;

		// Token: 0x0400055E RID: 1374
		public float customEdgePadding = 50f;

		// Token: 0x0400055F RID: 1375
		private ScrollRect parentScrollRect;

		// Token: 0x04000560 RID: 1376
		private Selectable _selectable;
	}
}
