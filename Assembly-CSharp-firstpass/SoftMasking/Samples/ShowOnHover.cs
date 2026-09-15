using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x0200000E RID: 14
	[RequireComponent(typeof(RectTransform))]
	public class ShowOnHover : UIBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002BA6 File Offset: 0x00000DA6
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002BAE File Offset: 0x00000DAE
		public bool forcedVisible
		{
			get
			{
				return this._forcedVisible;
			}
			set
			{
				if (this._forcedVisible != value)
				{
					this._forcedVisible = value;
					this.UpdateVisibility();
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002BC6 File Offset: 0x00000DC6
		protected override void Start()
		{
			base.Start();
			this.UpdateVisibility();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002BD4 File Offset: 0x00000DD4
		private void UpdateVisibility()
		{
			this.SetVisible(this.ShouldBeVisible());
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BE2 File Offset: 0x00000DE2
		private bool ShouldBeVisible()
		{
			return this._forcedVisible || this._isPointerOver;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002BF4 File Offset: 0x00000DF4
		private void SetVisible(bool visible)
		{
			if (this.targetGroup)
			{
				this.targetGroup.alpha = (visible ? 1f : 0f);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002C1D File Offset: 0x00000E1D
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._isPointerOver = true;
			this.UpdateVisibility();
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002C2C File Offset: 0x00000E2C
		public void OnPointerExit(PointerEventData eventData)
		{
			this._isPointerOver = false;
			this.UpdateVisibility();
		}

		// Token: 0x0400003C RID: 60
		public CanvasGroup targetGroup;

		// Token: 0x0400003D RID: 61
		private bool _forcedVisible;

		// Token: 0x0400003E RID: 62
		private bool _isPointerOver;
	}
}
