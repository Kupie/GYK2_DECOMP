using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace LazyBearTechnology
{
	// Token: 0x0200013E RID: 318
	public class UIBasicBubble : MonoBehaviour
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x00020C9D File Offset: 0x0001EE9D
		public RectTransform RootTransform
		{
			get
			{
				if (this.rootTransform == null)
				{
					this.rootTransform = base.GetComponent<RectTransform>();
				}
				return this.rootTransform;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00020CC0 File Offset: 0x0001EEC0
		protected virtual void UpdatePositionAndCorner(Vector3 screenPos, UIBasicBubble.ForceCornerPosition forceCornerPosition = UIBasicBubble.ForceCornerPosition.Auto)
		{
			Bounds screenBounds = LazyUI.GetScreenBounds();
			Vector3 zero = Vector3.zero;
			Vector2 vector = this.RootTransform.rect.size * this.RootTransform.localScale;
			this.forcedCornerPosition = forceCornerPosition;
			switch (this.forcedCornerPosition)
			{
			case UIBasicBubble.ForceCornerPosition.BottomCenter:
			{
				float num = screenPos.x - vector.x * 0.5f * LazyUI.ScaleFactor;
				float num2 = screenPos.x + vector.x * 0.5f * LazyUI.ScaleFactor;
				this.pickedCorner = UIBasicBubble.BubbleCornerDirection.BottomCenter;
				if (num < 0f)
				{
					zero.x = -num;
				}
				else if (num2 > screenBounds.max.x)
				{
					zero.x = -(num2 - screenBounds.max.x);
				}
				break;
			}
			case UIBasicBubble.ForceCornerPosition.BottomRight:
				this.pickedCorner = UIBasicBubble.BubbleCornerDirection.RightDown;
				break;
			case UIBasicBubble.ForceCornerPosition.BottomLeft:
				this.pickedCorner = UIBasicBubble.BubbleCornerDirection.LeftDown;
				break;
			case UIBasicBubble.ForceCornerPosition.TopCenter:
			{
				float num3 = screenPos.x - vector.x * 0.5f * LazyUI.ScaleFactor;
				float num4 = screenPos.x + vector.x * 0.5f * LazyUI.ScaleFactor;
				this.pickedCorner = UIBasicBubble.BubbleCornerDirection.TopCenter;
				if (num3 < 0f)
				{
					zero.x = -num3;
				}
				else if (num4 > screenBounds.max.x)
				{
					zero.x = -(num4 - screenBounds.max.x);
				}
				break;
			}
			case UIBasicBubble.ForceCornerPosition.TopRight:
				this.pickedCorner = UIBasicBubble.BubbleCornerDirection.RightUp;
				break;
			case UIBasicBubble.ForceCornerPosition.TopLeft:
				this.pickedCorner = UIBasicBubble.BubbleCornerDirection.LeftUp;
				break;
			default:
			{
				Vector2 vector2 = vector;
				vector2.Scale(Vector2.one * LazyUI.ScaleFactor);
				vector2 += new Vector2(0f, Mathf.Abs(this.corners[1].rectTransform.localPosition.y)) * LazyUI.ScaleFactor;
				bool flag = screenPos.y + vector2.y < screenBounds.max.y;
				bool flag2 = screenPos.x + vector2.x < screenBounds.max.x;
				switch (this.forcedCornerPosition)
				{
				case UIBasicBubble.ForceCornerPosition.Auto:
					this.pickedCorner = (flag ? (flag2 ? UIBasicBubble.BubbleCornerDirection.LeftDown : UIBasicBubble.BubbleCornerDirection.RightDown) : (flag2 ? UIBasicBubble.BubbleCornerDirection.LeftUp : UIBasicBubble.BubbleCornerDirection.RightUp));
					break;
				case UIBasicBubble.ForceCornerPosition.Right:
					this.pickedCorner = (flag ? UIBasicBubble.BubbleCornerDirection.RightDown : UIBasicBubble.BubbleCornerDirection.RightUp);
					break;
				case UIBasicBubble.ForceCornerPosition.Left:
					this.pickedCorner = (flag ? UIBasicBubble.BubbleCornerDirection.LeftDown : UIBasicBubble.BubbleCornerDirection.LeftUp);
					break;
				default:
					throw new NotImplementedException();
				}
				break;
			}
			}
			this.ApplyCornerIndex(screenPos, zero, this.pickedCorner);
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00020F70 File Offset: 0x0001F170
		protected void ApplyCornerIndex(Vector3 screenPos, Vector3 offset, UIBasicBubble.BubbleCornerDirection corner)
		{
			UIBubbleCorner uibubbleCorner = this.currentCorner;
			Vector3 vector = screenPos - (this.corners[(int)corner].rectTransform.position - this.RootTransform.position) + offset;
			if (this.useSmoothPositioning)
			{
				if ((vector - base.transform.position).sqrMagnitude > this.instantUpdateOffset * LazyUI.ScaleFactor)
				{
					base.transform.position = vector;
				}
				else
				{
					base.transform.DOMove(vector, this.animationTime, false);
				}
			}
			else
			{
				base.transform.position = vector;
			}
			for (int i = 0; i < this.corners.Count; i++)
			{
				UIBubbleCorner uibubbleCorner2 = this.corners[i];
				uibubbleCorner2.gameObject.SetActive(i == (int)corner);
				if (i == (int)corner)
				{
					this.currentCorner = uibubbleCorner2;
				}
			}
			if (uibubbleCorner != this.currentCorner)
			{
				this.OnCornerChanged();
			}
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x00021073 File Offset: 0x0001F273
		protected virtual void OnCornerChanged()
		{
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x00021075 File Offset: 0x0001F275
		protected virtual void UpdatePositionAndCorner(Vector3 screenPos, Vector3 offsetForDownPosition)
		{
			this.UpdatePositionAndCorner(screenPos, UIBasicBubble.ForceCornerPosition.Auto);
			if (this.pickedCorner == UIBasicBubble.BubbleCornerDirection.LeftUp || this.pickedCorner == UIBasicBubble.BubbleCornerDirection.RightUp)
			{
				base.transform.position += offsetForDownPosition;
			}
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000210A8 File Offset: 0x0001F2A8
		public bool IsBubbleOverstepsScreen(Vector2 screenPosition, UIBasicBubble.ForceCornerPosition cornerPosition)
		{
			Bounds screenBounds = LazyUI.GetScreenBounds();
			int directionByCorner = (int)this.GetDirectionByCorner(cornerPosition);
			Rect worldRect = this.RootTransform.GetWorldRect();
			worldRect.position = screenPosition - (this.corners[directionByCorner].rectTransform.position - this.RootTransform.position) + worldRect.size * (Vector2.up - this.RootTransform.pivot);
			bool flag = worldRect.xMax < screenBounds.max.x;
			bool flag2 = worldRect.xMin > screenBounds.min.x;
			bool flag3 = worldRect.yMin < screenBounds.max.y;
			bool flag4 = worldRect.yMin - worldRect.height > screenBounds.min.y;
			return !flag || !flag2 || !flag3 || !flag4;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x0002119C File Offset: 0x0001F39C
		public bool IsBubbleOverstepsScreen()
		{
			Bounds screenBounds = LazyUI.GetScreenBounds();
			Rect worldRect = this.RootTransform.GetWorldRect();
			bool flag = worldRect.xMax < screenBounds.max.x;
			bool flag2 = worldRect.xMin > screenBounds.min.x;
			bool flag3 = worldRect.yMin < screenBounds.max.y;
			bool flag4 = worldRect.yMin - worldRect.height > screenBounds.min.y;
			return !flag || !flag2 || !flag3 || !flag4;
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00021228 File Offset: 0x0001F428
		public float GetEffectiveSpaceOfBubbleVisibility()
		{
			Bounds screenBounds = LazyUI.GetScreenBounds();
			Rect worldRect = this.RootTransform.GetWorldRect();
			float num = Mathf.Abs(worldRect.xMax - worldRect.xMin) * Mathf.Abs(worldRect.yMin - worldRect.yMax);
			float num2 = ((float)this.Step(screenBounds.max.x, worldRect.xMax) * Mathf.Abs(screenBounds.max.x - worldRect.xMax) + (float)this.Step(screenBounds.min.x, worldRect.xMin) * Mathf.Abs(screenBounds.min.x - worldRect.xMin)) * Mathf.Abs(worldRect.yMin - worldRect.yMax) + ((float)this.Step(screenBounds.max.y, worldRect.yMax) * Mathf.Abs(screenBounds.max.y - worldRect.yMax) + (float)this.Step(screenBounds.min.y, worldRect.yMin) * Mathf.Abs(screenBounds.min.y - worldRect.yMin)) * Mathf.Abs(worldRect.xMin - worldRect.xMax);
			return num - num2 / num;
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00021374 File Offset: 0x0001F574
		public bool IsBubbleOverlapsWithAnotherRects(Vector2 screenPosition, Rect[] anotherRects, UIBasicBubble.ForceCornerPosition cornerPosition)
		{
			int directionByCorner = (int)this.GetDirectionByCorner(cornerPosition);
			Rect worldRect = this.RootTransform.GetWorldRect();
			worldRect.position = screenPosition - (this.corners[directionByCorner].rectTransform.position - this.RootTransform.position) + worldRect.size * (Vector2.up - this.RootTransform.pivot);
			return worldRect.LCS_IsOverlapsWithAnyOthers(anotherRects);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x000213FA File Offset: 0x0001F5FA
		private UIBasicBubble.BubbleCornerDirection GetDirectionByCorner(UIBasicBubble.ForceCornerPosition cornerPosition)
		{
			switch (cornerPosition)
			{
			case UIBasicBubble.ForceCornerPosition.BottomCenter:
				return UIBasicBubble.BubbleCornerDirection.BottomCenter;
			case UIBasicBubble.ForceCornerPosition.BottomRight:
				return UIBasicBubble.BubbleCornerDirection.RightDown;
			case UIBasicBubble.ForceCornerPosition.BottomLeft:
				return UIBasicBubble.BubbleCornerDirection.LeftDown;
			case UIBasicBubble.ForceCornerPosition.TopCenter:
				return UIBasicBubble.BubbleCornerDirection.TopCenter;
			case UIBasicBubble.ForceCornerPosition.TopRight:
				return UIBasicBubble.BubbleCornerDirection.RightUp;
			case UIBasicBubble.ForceCornerPosition.TopLeft:
				return UIBasicBubble.BubbleCornerDirection.LeftUp;
			default:
				return UIBasicBubble.BubbleCornerDirection.LeftDown;
			}
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002142B File Offset: 0x0001F62B
		private int Step(float a, float x)
		{
			if (x < a)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x040003A7 RID: 935
		[SerializeField]
		protected List<UIBubbleCorner> corners;

		// Token: 0x040003A8 RID: 936
		[SerializeField]
		private RectTransform rootTransform;

		// Token: 0x040003A9 RID: 937
		[SerializeField]
		private bool flipBackgroundH;

		// Token: 0x040003AA RID: 938
		[SerializeField]
		private bool useSmoothPositioning;

		// Token: 0x040003AB RID: 939
		[SerializeField]
		private float instantUpdateOffset = 400f;

		// Token: 0x040003AC RID: 940
		[SerializeField]
		private float animationTime = 0.2f;

		// Token: 0x040003AD RID: 941
		protected UIBubbleCorner currentCorner;

		// Token: 0x040003AE RID: 942
		[NonSerialized]
		protected UIBasicBubble.BubbleCornerDirection pickedCorner;

		// Token: 0x040003AF RID: 943
		[NonSerialized]
		protected UIBasicBubble.ForceCornerPosition forcedCornerPosition;

		// Token: 0x020001EF RID: 495
		public enum BubbleCornerDirection
		{
			// Token: 0x04000690 RID: 1680
			LeftUp,
			// Token: 0x04000691 RID: 1681
			LeftDown,
			// Token: 0x04000692 RID: 1682
			RightUp,
			// Token: 0x04000693 RID: 1683
			RightDown,
			// Token: 0x04000694 RID: 1684
			BottomCenter,
			// Token: 0x04000695 RID: 1685
			TopCenter
		}

		// Token: 0x020001F0 RID: 496
		public enum ForceCornerPosition
		{
			// Token: 0x04000697 RID: 1687
			Auto,
			// Token: 0x04000698 RID: 1688
			Right,
			// Token: 0x04000699 RID: 1689
			Left,
			// Token: 0x0400069A RID: 1690
			BottomCenter,
			// Token: 0x0400069B RID: 1691
			BottomRight,
			// Token: 0x0400069C RID: 1692
			BottomLeft,
			// Token: 0x0400069D RID: 1693
			TopCenter,
			// Token: 0x0400069E RID: 1694
			TopRight,
			// Token: 0x0400069F RID: 1695
			TopLeft
		}
	}
}
