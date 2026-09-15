using System;
using TMPro;
using UnityEngine;

namespace LazyBearTechnology
{
	// Token: 0x02000155 RID: 341
	public class LazyAutoScrollLabel : MonoBehaviour
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00025647 File Offset: 0x00023847
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x0002564F File Offset: 0x0002384F
		public bool IsPaused
		{
			get
			{
				return this.isPaused;
			}
			set
			{
				this.isPaused = value;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00025658 File Offset: 0x00023858
		public TextMeshProUGUI Label
		{
			get
			{
				if (this.label == null)
				{
					this.label = this.Mask.GetComponentInChildren<TextMeshProUGUI>(true);
				}
				return this.label;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00025680 File Offset: 0x00023880
		private RectTransform Mask
		{
			get
			{
				if (this.mask == null)
				{
					this.mask = base.transform.GetChild(0).GetComponent<RectTransform>();
				}
				return this.mask;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000752 RID: 1874 RVA: 0x000256AD File Offset: 0x000238AD
		public RectTransform RectTransform
		{
			get
			{
				if (this.rectTransform == null)
				{
					this.rectTransform = base.GetComponent<RectTransform>();
				}
				return this.rectTransform;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x000256CF File Offset: 0x000238CF
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x000256DC File Offset: 0x000238DC
		public string Text
		{
			get
			{
				return this.Label.text;
			}
			set
			{
				this.Label.text = value;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x000256EA File Offset: 0x000238EA
		private float DeltaTime
		{
			get
			{
				return LazyTime.GetUnscaledDeltaTime;
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x000256F4 File Offset: 0x000238F4
		public void Update()
		{
			this.SetSpeed();
			if (this.isPaused)
			{
				if (this.axis == RectTransform.Axis.Horizontal)
				{
					if (Mathf.Abs(this.Label.rectTransform.anchoredPosition.x - this.defaultPosition.x) <= Mathf.Abs(this.speed.x * this.DeltaTime))
					{
						this.Label.rectTransform.anchoredPosition = this.defaultPosition;
					}
					else
					{
						this.direction = ((this.Label.rectTransform.anchoredPosition.x > this.defaultPosition.x) ? LazyAutoScrollLabel.Direction.RightToLeft : LazyAutoScrollLabel.Direction.LeftToRight);
						this.Scroll();
					}
				}
				else if (Mathf.Abs(this.Label.rectTransform.anchoredPosition.y - this.defaultPosition.y) <= Mathf.Abs(this.speed.y * this.DeltaTime))
				{
					this.SetDefaultPosition();
				}
				else
				{
					this.direction = ((this.Label.rectTransform.anchoredPosition.y > this.defaultPosition.y) ? LazyAutoScrollLabel.Direction.RightToLeft : LazyAutoScrollLabel.Direction.LeftToRight);
					this.Scroll();
				}
				this.SetDefaultDirection();
				return;
			}
			if (this.IsScrollNecessary())
			{
				this.RectTransform.GetWorldCorners(this.selfCorners);
				this.Label.rectTransform.GetWorldCorners(this.labelCorners);
				if (this.axis == RectTransform.Axis.Horizontal)
				{
					if (this.direction == LazyAutoScrollLabel.Direction.RightToLeft)
					{
						if (this.labelCorners[2].x <= this.selfCorners[2].x - this.additionalDistance * LazyUI.ScaleFactor)
						{
							this.direction = LazyAutoScrollLabel.Direction.LeftToRight;
						}
					}
					else if (this.labelCorners[1].x >= this.selfCorners[1].x + this.additionalDistance * LazyUI.ScaleFactor)
					{
						this.direction = LazyAutoScrollLabel.Direction.RightToLeft;
					}
				}
				else if (this.direction == LazyAutoScrollLabel.Direction.RightToLeft)
				{
					if (this.labelCorners[1].y <= this.selfCorners[1].y - this.additionalDistance * LazyUI.ScaleFactor)
					{
						this.direction = LazyAutoScrollLabel.Direction.LeftToRight;
					}
				}
				else if (this.labelCorners[0].y >= this.selfCorners[0].y + this.additionalDistance * LazyUI.ScaleFactor)
				{
					this.direction = LazyAutoScrollLabel.Direction.RightToLeft;
				}
				this.Scroll();
				return;
			}
			this.SetDefaultPosition();
			this.SetDefaultDirection();
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x0002597C File Offset: 0x00023B7C
		private void SetSpeed()
		{
			if (this.axis == RectTransform.Axis.Horizontal)
			{
				this.speed.x = this.baseSpeed * (this.Label.rectTransform.sizeDelta.x / this.RectTransform.sizeDelta.x);
				return;
			}
			this.speed.y = this.baseSpeed * (this.Label.rectTransform.sizeDelta.y / this.RectTransform.sizeDelta.y);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x00025A04 File Offset: 0x00023C04
		private bool IsScrollNecessary()
		{
			if (this.axis == RectTransform.Axis.Horizontal)
			{
				return this.Label.rectTransform.sizeDelta.x > this.RectTransform.sizeDelta.x;
			}
			return this.Label.rectTransform.sizeDelta.y > this.RectTransform.sizeDelta.y;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00025A68 File Offset: 0x00023C68
		private void Scroll()
		{
			this.Label.rectTransform.anchoredPosition += this.speed * ((float)this.direction * this.DeltaTime);
		}

		// Token: 0x0600075A RID: 1882 RVA: 0x00025A9E File Offset: 0x00023C9E
		private void SetDefaultPosition()
		{
			this.Label.rectTransform.anchoredPosition = this.defaultPosition;
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x00025AB6 File Offset: 0x00023CB6
		private void SetDefaultDirection()
		{
			this.direction = ((this.axis == RectTransform.Axis.Horizontal) ? LazyAutoScrollLabel.Direction.RightToLeft : LazyAutoScrollLabel.Direction.RightToLeft);
		}

		// Token: 0x0400046E RID: 1134
		[SerializeField]
		private float baseSpeed = 25f;

		// Token: 0x0400046F RID: 1135
		[SerializeField]
		private float additionalDistance;

		// Token: 0x04000470 RID: 1136
		[SerializeField]
		[Space]
		private Vector2 defaultPosition;

		// Token: 0x04000471 RID: 1137
		[Space]
		[SerializeField]
		private RectTransform.Axis axis;

		// Token: 0x04000472 RID: 1138
		[SerializeField]
		[Space]
		private bool isPaused;

		// Token: 0x04000473 RID: 1139
		private TextMeshProUGUI label;

		// Token: 0x04000474 RID: 1140
		private RectTransform mask;

		// Token: 0x04000475 RID: 1141
		private RectTransform rectTransform;

		// Token: 0x04000476 RID: 1142
		private LazyAutoScrollLabel.Direction direction = LazyAutoScrollLabel.Direction.RightToLeft;

		// Token: 0x04000477 RID: 1143
		private Vector2 speed;

		// Token: 0x04000478 RID: 1144
		private Vector3[] selfCorners = new Vector3[4];

		// Token: 0x04000479 RID: 1145
		private Vector3[] labelCorners = new Vector3[4];

		// Token: 0x020001F9 RID: 505
		private enum Direction
		{
			// Token: 0x040006B0 RID: 1712
			LeftToRight = 1,
			// Token: 0x040006B1 RID: 1713
			RightToLeft = -1,
			// Token: 0x040006B2 RID: 1714
			BottomToTop = 1,
			// Token: 0x040006B3 RID: 1715
			TopToBottom = -1
		}
	}
}
