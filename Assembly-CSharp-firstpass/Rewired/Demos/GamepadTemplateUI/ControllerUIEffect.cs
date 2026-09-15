using System;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.Demos.GamepadTemplateUI
{
	// Token: 0x02000114 RID: 276
	[RequireComponent(typeof(Image))]
	public class ControllerUIEffect : MonoBehaviour
	{
		// Token: 0x06000D18 RID: 3352 RVA: 0x00026691 File Offset: 0x00024891
		private void Awake()
		{
			this._image = base.GetComponent<Image>();
			this._origColor = this._image.color;
			this._color = this._origColor;
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x000266BC File Offset: 0x000248BC
		public void Activate(float amount)
		{
			amount = Mathf.Clamp01(amount);
			if (this._isActive && amount == this._highlightAmount)
			{
				return;
			}
			this._highlightAmount = amount;
			this._color = Color.Lerp(this._origColor, this._highlightColor, this._highlightAmount);
			this._isActive = true;
			this.RedrawImage();
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x00026714 File Offset: 0x00024914
		public void Deactivate()
		{
			if (!this._isActive)
			{
				return;
			}
			this._color = this._origColor;
			this._highlightAmount = 0f;
			this._isActive = false;
			this.RedrawImage();
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00026743 File Offset: 0x00024943
		private void RedrawImage()
		{
			this._image.color = this._color;
			this._image.enabled = this._isActive;
		}

		// Token: 0x040006D6 RID: 1750
		[SerializeField]
		private Color _highlightColor = Color.white;

		// Token: 0x040006D7 RID: 1751
		private Image _image;

		// Token: 0x040006D8 RID: 1752
		private Color _color;

		// Token: 0x040006D9 RID: 1753
		private Color _origColor;

		// Token: 0x040006DA RID: 1754
		private bool _isActive;

		// Token: 0x040006DB RID: 1755
		private float _highlightAmount;
	}
}
