using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x02000115 RID: 277
	[RequireComponent(typeof(Image))]
	public class ControllerUIElement : MonoBehaviour
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0002677A File Offset: 0x0002497A
		private bool hasEffects
		{
			get
			{
				return this._positiveUIEffect != null || this._negativeUIEffect != null;
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00026798 File Offset: 0x00024998
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
			this.ClearLabels();
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000267CC File Offset: 0x000249CC
		public void Activate(float amount)
		{
			amount = Mathf.Clamp(amount, -1f, 1f);
			if (this.hasEffects)
			{
				if (amount < 0f && this._negativeUIEffect != null)
				{
					this._negativeUIEffect.Activate(Mathf.Abs(amount));
				}
				if (amount > 0f && this._positiveUIEffect != null)
				{
					this._positiveUIEffect.Activate(Mathf.Abs(amount));
				}
			}
			else
			{
				if (this._isActive && amount == this._highlightAmount)
				{
					return;
				}
				this._highlightAmount = amount;
				this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
			}
			this._isActive = true;
			this.RedrawImage();
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].Activate(amount);
					}
				}
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000268C0 File Offset: 0x00024AC0
		public void Deactivate()
		{
			if (!this._isActive)
			{
				return;
			}
			this._color = this._origColor;
			this._highlightAmount = 0f;
			if (this._positiveUIEffect != null)
			{
				this._positiveUIEffect.Deactivate();
			}
			if (this._negativeUIEffect != null)
			{
				this._negativeUIEffect.Deactivate();
			}
			this._isActive = false;
			this.RedrawImage();
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].Deactivate();
					}
				}
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x00026968 File Offset: 0x00024B68
		public void SetLabel(string text, AxisRange labelType)
		{
			Text text2;
			switch (labelType)
			{
			case AxisRange.Full:
				text2 = this._label;
				break;
			case AxisRange.Positive:
				text2 = this._positiveLabel;
				break;
			case AxisRange.Negative:
				text2 = this._negativeLabel;
				break;
			default:
				text2 = null;
				break;
			}
			if (text2 != null)
			{
				text2.text = text;
			}
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].SetLabel(text, labelType);
					}
				}
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000269F4 File Offset: 0x00024BF4
		public void ClearLabels()
		{
			if (this._label != null)
			{
				this._label.text = string.Empty;
			}
			if (this._positiveLabel != null)
			{
				this._positiveLabel.text = string.Empty;
			}
			if (this._negativeLabel != null)
			{
				this._negativeLabel.text = string.Empty;
			}
			if (this._childElements.Length != 0)
			{
				for (int i = 0; i < this._childElements.Length; i++)
				{
					if (!(this._childElements[i] == null))
					{
						this._childElements[i].ClearLabels();
					}
				}
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00026A94 File Offset: 0x00024C94
		private void RedrawImage()
		{
			this._image.color = this._color;
		}

		// Token: 0x040006DC RID: 1756
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x040006DD RID: 1757
		[SerializeField]
		private ControllerUIEffect _positiveUIEffect;

		// Token: 0x040006DE RID: 1758
		[SerializeField]
		private ControllerUIEffect _negativeUIEffect;

		// Token: 0x040006DF RID: 1759
		[SerializeField]
		private Text _label;

		// Token: 0x040006E0 RID: 1760
		[SerializeField]
		private Text _positiveLabel;

		// Token: 0x040006E1 RID: 1761
		[SerializeField]
		private Text _negativeLabel;

		// Token: 0x040006E2 RID: 1762
		[SerializeField]
		private ControllerUIElement[] _childElements = new ControllerUIElement[0];

		// Token: 0x040006E3 RID: 1763
		private Image _image;

		// Token: 0x040006E4 RID: 1764
		private Color _color;

		// Token: 0x040006E5 RID: 1765
		private Color _origColor;

		// Token: 0x040006E6 RID: 1766
		private bool _isActive;

		// Token: 0x040006E7 RID: 1767
		private float _highlightAmount;
	}
}
