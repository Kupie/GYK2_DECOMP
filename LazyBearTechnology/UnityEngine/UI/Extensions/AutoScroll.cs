using System;
using LazyBearTechnology;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200002A RID: 42
	[RequireComponent(typeof(ScrollRect))]
	public class AutoScroll : MonoBehaviour
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x000058D0 File Offset: 0x00003AD0
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x000058D8 File Offset: 0x00003AD8
		public bool IsNextAutoscrollInstant
		{
			get
			{
				return this.isNextAutoscrollInstant;
			}
			set
			{
				this.isNextAutoscrollInstant = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x000058E1 File Offset: 0x00003AE1
		// (set) Token: 0x060000CA RID: 202 RVA: 0x000058E9 File Offset: 0x00003AE9
		public bool SkipNextAutoscroll
		{
			get
			{
				return this.skipNextAutoscroll;
			}
			set
			{
				this.skipNextAutoscroll = value;
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000058F2 File Offset: 0x00003AF2
		protected void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.scrollRectTransform = this.scrollRect.GetComponent<RectTransform>();
			this.gamepadController.OnFocusedItemChanged += delegate(GamepadNavigationItem item)
			{
				if (this.skipNextAutoscroll)
				{
					this.skipNextAutoscroll = false;
					return;
				}
				this.ScrollToItem(item);
			};
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005928 File Offset: 0x00003B28
		public void ScrollToItem(GamepadNavigationItem item)
		{
			if (item == null || item.ignoreScroll)
			{
				return;
			}
			if (!this.IsItemInsideContent(item))
			{
				return;
			}
			if (this.scrollRect == null)
			{
				this.scrollRect = base.GetComponent<ScrollRect>();
				this.scrollRectTransform = this.scrollRect.GetComponent<RectTransform>();
			}
			RectTransform rectTransform;
			if (!item.TryGetComponent<RectTransform>(out rectTransform) || !this.scrollRect.enabled)
			{
				this.isNextAutoscrollInstant = false;
				return;
			}
			AutoScrollTargetType autoScrollTargetType = this.autoScrollTargetType;
			if (autoScrollTargetType != AutoScrollTargetType.Center)
			{
				if (autoScrollTargetType != AutoScrollTargetType.NearestVisiblePosition)
				{
					throw new ArgumentOutOfRangeException(string.Format("Unhandled value [{0}]", this.autoScrollTargetType));
				}
				this.scrollRect.ScrollToVisiblePosition(rectTransform, this.direction, this.borderOffset * LazyUI.ScaleFactor, this.isNextAutoscrollInstant ? 0f : this.autoScrollTime, true);
			}
			else if (this.isNextAutoscrollInstant)
			{
				this.scrollRect.ScrollToTargetInstant(rectTransform, this.direction);
			}
			else
			{
				this.scrollRect.ScrollToTarget(rectTransform, this.direction, this.autoScrollTime, true);
			}
			this.isNextAutoscrollInstant = false;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00005A3C File Offset: 0x00003C3C
		private bool IsItemInsideContent(GamepadNavigationItem item)
		{
			Transform transform = item.transform.parent;
			while (!(transform == this.content))
			{
				transform = transform.parent;
				if (!(transform != null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000069 RID: 105
		[SerializeField]
		private RectTransform content;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private AutoScrollTargetType autoScrollTargetType;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		private float borderOffset;

		// Token: 0x0400006C RID: 108
		[Space]
		[SerializeField]
		private float autoScrollTime = 0.3f;

		// Token: 0x0400006D RID: 109
		[SerializeField]
		private GamepadNavigationController gamepadController;

		// Token: 0x0400006E RID: 110
		[Space]
		[SerializeField]
		private RectTransform.Axis direction = RectTransform.Axis.Vertical;

		// Token: 0x0400006F RID: 111
		protected RectTransform scrollRectTransform;

		// Token: 0x04000070 RID: 112
		protected ScrollRect scrollRect;

		// Token: 0x04000071 RID: 113
		private bool isNextAutoscrollInstant;

		// Token: 0x04000072 RID: 114
		private bool skipNextAutoscroll;
	}
}
