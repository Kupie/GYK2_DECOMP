using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SoftMasking.Samples
{
	// Token: 0x0200000B RID: 11
	[RequireComponent(typeof(RectTransform))]
	public class PaintedMask : UIBehaviour
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00002648 File Offset: 0x00000848
		protected override void Start()
		{
			base.Start();
			this._renderTexture = new RenderTexture((int)this.maskSize.x, (int)this.maskSize.y, 0, RenderTextureFormat.ARGB32);
			this._renderTexture.Create();
			this.renderCamera.targetTexture = this._renderTexture;
			this.targetMask.renderTexture = this._renderTexture;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000026B0 File Offset: 0x000008B0
		private Vector2 maskSize
		{
			get
			{
				return ((RectTransform)this.targetMask.transform).rect.size;
			}
		}

		// Token: 0x04000021 RID: 33
		public Camera renderCamera;

		// Token: 0x04000022 RID: 34
		public SoftMask targetMask;

		// Token: 0x04000023 RID: 35
		private RenderTexture _renderTexture;
	}
}
