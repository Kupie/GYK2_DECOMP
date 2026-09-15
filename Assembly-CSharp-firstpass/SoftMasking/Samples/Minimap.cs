using System;
using UnityEngine;

namespace SoftMasking.Samples
{
	// Token: 0x0200000A RID: 10
	public class Minimap : MonoBehaviour
	{
		// Token: 0x0600001E RID: 30 RVA: 0x0000256B File Offset: 0x0000076B
		public void LateUpdate()
		{
			this.map.anchoredPosition = -this.marker.anchoredPosition * this._zoom;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002593 File Offset: 0x00000793
		public void ZoomIn()
		{
			this._zoom = this.Clamp(this._zoom + this.zoomStep);
			this.map.localScale = Vector3.one * this._zoom;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000025C9 File Offset: 0x000007C9
		public void ZoomOut()
		{
			this._zoom = this.Clamp(this._zoom - this.zoomStep);
			this.map.localScale = Vector3.one * this._zoom;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000025FF File Offset: 0x000007FF
		private float Clamp(float zoom)
		{
			return Mathf.Clamp(zoom, this.minZoom, this.maxZoom);
		}

		// Token: 0x0400001B RID: 27
		public RectTransform map;

		// Token: 0x0400001C RID: 28
		public RectTransform marker;

		// Token: 0x0400001D RID: 29
		[Space]
		public float minZoom = 0.8f;

		// Token: 0x0400001E RID: 30
		public float maxZoom = 1.4f;

		// Token: 0x0400001F RID: 31
		public float zoomStep = 0.2f;

		// Token: 0x04000020 RID: 32
		private float _zoom = 1f;
	}
}
